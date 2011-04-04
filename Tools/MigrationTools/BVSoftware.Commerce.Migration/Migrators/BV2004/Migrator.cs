using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.IO;

namespace BVSoftware.Commerce.Migration.Migrators.BV2004
{
    public class Migrator: IMigrator
    {

        private const int CHUNKSIZE = 131072;
        private MigrationSettings settings = null;
        private MigrationServices.MigrationToolServiceClient bv6proxy = null;
        private data.bvc2004Entities oldDatabase = null;
        private Dictionary<int, long> AffiliateMapper = new Dictionary<int, long>();
        private Dictionary<int, long> TaxScheduleMapper = new Dictionary<int, long>();
        private Dictionary<int, long> ProductPropertyMapper = new Dictionary<int, long>();

        public event MigrationService.ProgressReportDelegate ProgressReport;
        private void wl(string message)
        {
            if (this.ProgressReport != null)
            {
                this.ProgressReport(message);
            }
        }
        private void Header(string title)
        {
            wl("");
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
            wl("    " + title + " at " + DateTime.UtcNow.ToString());
            wl("");
            wl("-----------------------------------------------------------");
            wl("");
        }
        private string EFConnString(string connString)
        {
            string result = "metadata=res://*/Migrators.BV2004.data.OldDatabase.csdl|" +
            "res://*/Migrators.BV2004.data.OldDatabase.ssdl|res://*/Migrators.BV2004.data.OldDatabase.msl;" +
            "provider=System.Data.SqlClient;provider connection string=\"" +
            connString.TrimEnd('/') +
            "\"";
            return result;
        }
        private void DumpErrors(List<MigrationServices.ApiError> errors)
        {
            foreach (MigrationServices.ApiError e in errors)
            {
                wl("ERROR: " + e.Code + " | " + e.Description);
            }
        }

        private MigrationServices.MigrationToolServiceClient GetBV6Proxy()
        {
            MigrationServices.MigrationToolServiceClient result = null;
            try
            {
                string serviceUrl = settings.DestinationServiceRootUrl + "/api/v1/MigrationToolService.svc";
                result = new MigrationServices.MigrationToolServiceClient("BasicHttpBinding_IMigrationToolService",
                                                                            serviceUrl);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
            }
            return result;
        }

        public void Migrate(MigrationSettings s)
        {
            wl("");
            wl("BVC 2004 Migrator Started");
            wl("");

            settings = s;

            try
            {
                string serviceUrl = s.DestinationServiceRootUrl + "/api/v1/MigrationToolService.svc";
                wl("Creating service connection to: " + serviceUrl);
                bv6proxy = new MigrationServices.MigrationToolServiceClient("BasicHttpBinding_IMigrationToolService",
                                                                            serviceUrl);
                wl("Service Connection Created");
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
                return;
            }

            try
            {
                oldDatabase = new data.bvc2004Entities(EFConnString(s.SourceConnectionString()));
            }
            catch (Exception ex2)
            {
                wl("EXCEPTION While attempting to create old database model!");
                wl(ex2.Message);
                wl(ex2.StackTrace);
                return;
            }


            try
            {

                // Clear Products
                if (s.ClearProducts)
                {
                    ClearProducts();
                }

                // Clear Categories
                if (s.ClearCategories)
                {
                    ClearCategories();
                }

                // Users 
                if (s.ImportUsers)
                {
                    //ImportRoles();
                    ImportPriceGroups();
                    ImportUsers();
                }

                // Affiliates
                if (s.ImportAffiliates)
                {
                    ImportAffiliates();
                }

                // Tax Classes are prerequisite for product import
                if (s.ImportOtherSettings || s.ImportProducts)
                {
                    ImportTaxSchedules();
                    ImportTaxes();
                }

                // Vendors and Manufacturers
                if (s.ImportProducts || s.ImportCategories)
                {
                    ImportVendors();
                    ImportManufacturers();
                }

                // Product Types
                if (s.ImportProducts)
                {
                    ImportProductProperties();
                    ImportProductTypes();
                }

                // Categories
                if (s.ImportCategories)
                {
                    ImportCategories();
                }

                if (s.ImportProducts)
                {                                        
                    ImportProductChoices();
                    MigrateProductFileDownloads();

                    ImportProducts();

                    ImportRelatedItems();
                }

                if (s.ImportOrders)
                {
                    ImportOrders();
                }

                if (s.ImportOtherSettings)
                {
                    ImportMailingLists();
                    ImportPolicies();
                    ImportFraudData();
                }
            }
            catch (Exception e)
            {
                wl("ERROR: " + e.Message);
                wl(e.StackTrace);
            }
        }

        private void ImportFraudData()
        {
            Header("Importing Fraud Data");
        }

        private void ImportPolicies()
        {
            Header("Importing Policies");
        }

        private void ImportMailingLists()
        {
            Header("Importing Mailing Lists");
        }

        private void ImportOrders()
        {
            Header("Importing Orders");

            if (settings.SingleOrderImport.Trim().Length > 0)
            {
                int singleOrderID = 0;
                if (settings.SingleOrderImport.Length > 0)
                {
                    int.TryParse(settings.SingleOrderImport, out singleOrderID);
                }
                var o = (from old in oldDatabase.bvc_Order where old.ID == singleOrderID select old).FirstOrDefault();
                if (o == null)
                {
                    wl("Unable to locate order " + settings.SingleOrderImport);
                    return;
                }
                // Single Order Mode
                ImportSingleOrder(o);
            }
            else
            {
                // Multi-Order Mode
                int pageSize = 100;
                int totalRecords = oldDatabase.bvc_Order.Where(y => y.StatusCode == 3).Count();
                int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
                for (int i = 0; i < totalPages; i++)
                {
                    wl("Getting Orders page " + (i + 1) + " of " + totalPages.ToString());
                    int startRecord = i * pageSize;
                    var orders = (from order in oldDatabase.bvc_Order where order.StatusCode == 3 select order).OrderBy(y => y.ID).Skip(startRecord).Take(pageSize).ToList();
                    if (orders == null) continue;
                    System.Threading.Tasks.Parallel.ForEach(orders, ImportSingleOrder);
                }
            }
        }
        private void ImportSingleOrder(data.bvc_Order old)
        {
            if (old == null) return;
            wl("Processing Order: " + old.ID.ToString());

            MigrationServices.OrderDTO o = new MigrationServices.OrderDTO();
            PrepOrderDto(o);
            PopulateDto(old, o);
            if (o != null)
            {
                MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();
                var res = proxy.MigrateOrder(settings.ApiKey, o);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                        ImportOrderTransactions(o.bvin, o.OrderNumber, old);
                    }
                }

            }
        }
        private void PrepOrderDto(MigrationServices.OrderDTO o)
        {
            o.BillingAddress = new MigrationServices.AddressDTO();
            o.Coupons = new List<MigrationServices.OrderCouponDTO>();
            o.CustomProperties = new List<MigrationServices.CustomPropertyDTO>();
            o.Items = new List<MigrationServices.LineItemDTO>();
            o.Notes = new List<MigrationServices.OrderNoteDTO>();
            o.OrderDiscountDetails = new List<MigrationServices.DiscountDetailDTO>();
            o.Packages = new List<MigrationServices.OrderPackageDTO>();
            o.ShippingAddress = new MigrationServices.AddressDTO();
            o.ShippingDiscountDetails = new List<MigrationServices.DiscountDetailDTO>();
        }
        private void PopulateDto(data.bvc_Order old, MigrationServices.OrderDTO o)
        {
            o.AffiliateID = old.AffiliateID == 0 ? string.Empty : old.AffiliateID.ToString();
            BVC2004Address oldBilling = new BVC2004Address();
            oldBilling.FromXmlString(old.BillingAddress);
            if (oldBilling != null)
            {
                oldBilling.CopyTo(o.BillingAddress, EFConnString(settings.SourceConnectionString()));
            }
            o.bvin = old.ID.ToString() ?? string.Empty;
            o.CustomProperties = new List<MigrationServices.CustomPropertyDTO>();
            o.FraudScore = 0;
            //o.Id = old.ID;
            o.Instructions = old.Instructions;
            o.IsPlaced = old.StatusCode == 3;
            o.LastUpdatedUtc = old.LastUpdated ?? DateTime.UtcNow;
            o.OrderNumber = old.ID.ToString() ?? string.Empty;
            if (old.OrderDiscountsTotal != 0)
            {
                o.OrderDiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * ((decimal)old.OrderDiscountsTotal), Description = "BVC 2004 Order Discounts", Id = new Guid() });
            }
            o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unknown;    
            switch (old.PaymentStatus)
            {
                case 0:
                case 99:                
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unknown;
                    break;
                case 10:
                case 20:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unpaid;
                    break;
                case 30:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Paid;
                    break;
            }

            data.bvc_Package firstPackage = old.bvc_Package.FirstOrDefault();
            if (firstPackage != null)
            {
                BVC2004Address oldShipping = new BVC2004Address();
                oldShipping.FromXmlString(firstPackage.DestinationAddress);                
                if (oldShipping != null) oldShipping.CopyTo(o.ShippingAddress, EFConnString(settings.SourceConnectionString()));
                o.ShippingMethodDisplayName = firstPackage.ShippingMethodName;
                o.ShippingMethodId = string.Empty;
                o.ShippingProviderId = string.Empty;
                o.ShippingProviderServiceCode = firstPackage.ShippingServiceCode;                
            }

            o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.Unknown;
            switch (old.ShippingStatus)
            {
                case 0:
                    o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.Unknown;
                    break;
                case 1:
                    o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.Unshipped;
                    break;
                case 2:
                    o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.PartiallyShipped;
                    break;
                case 3:
                    o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.FullyShipped;
                    break;
                case 4:
                    o.ShippingStatus = MigrationServices.OrderShippingStatusDTO.NonShipping;
                    break;
            }
            o.StatusCode = string.Empty;
            o.StatusName = "Uknown";
            switch(old.StatusCode)
            {                                         
                                     
                case -1: //com.bvsoftware.bvc2004.OrderStatusCode.Canceled
                        o.StatusCode = "A7FFDB90-C566-4cf2-93F4-D42367F359D5";
                        o.StatusName = "On Hold";
                        break;
                case 3: //com.bvsoftware.bvc2004.OrderStatusCode.Completed
                        o.StatusCode = "09D7305D-BD95-48d2-A025-16ADC827582A";
                        o.StatusName = "Complete";
                        break;
                case 2: //com.bvsoftware.bvc2004.OrderStatusCode.InProcess
                        o.StatusCode = "F37EC405-1EC6-4a91-9AC4-6836215FBBBC";
                        o.StatusName = "In Process";
                        break;
                case 1:
                case 100:
                case 200: //com.bvsoftware.bvc2004.OrderStatusCode.OnHold
                        o.StatusCode = "88B5B4BE-CA7B-41a9-9242-D96ED3CA3135";
                        o.StatusName = "On Hold";
                        break;                
                case 999: //com.bvsoftware.bvc2004.OrderStatusCode.Void
                        o.StatusCode = "A7FFDB90-C566-4cf2-93F4-D42367F359D5";
                        o.StatusName = "Void";
                        break;
                }

            o.ThirdPartyOrderId = string.Empty;
            o.TimeOfOrderUtc = old.TimeOfOrder ?? DateTime.UtcNow;
            o.TotalHandling = (decimal)old.HandlingFee;
            o.TotalShippingBeforeDiscounts = ((decimal)old.ShippingTotal + (decimal)old.ShippingDiscountsTotal);
            o.ShippingDiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * (decimal)old.ShippingDiscountsTotal, Description = "BVC2004 Shipping Discount", Id = new Guid() });
            o.TotalTax = (decimal)old.TaxTotal;
            o.TotalTax2 = 0;
            o.UserEmail = string.Empty;
            o.UserID = old.UserID.ToString();

            wl(" - Coupons for Order " + old.ID.ToString());
            o.Coupons = TranslateCoupons(old);

            wl(" - Items For order " + old.ID.ToString());
            o.Items = TranslateItems(old);
            LineItemHelper.SplitTaxesAcrossItems((decimal)old.TaxTotal, (decimal)old.SubTotal, o.Items);

            wl(" - Notes For order " + old.ID.ToString());
            o.Notes = TranslateNotes(old);

            wl(" - Packages For order " + old.ID.ToString());
            o.Packages = TranslatePackages(old);

        }
        private List<MigrationServices.OrderCouponDTO> TranslateCoupons(data.bvc_Order o)
        {
            List<MigrationServices.OrderCouponDTO> result = new List<MigrationServices.OrderCouponDTO>();            

            foreach (data.bvc_OrderCoupon oldCoupon in o.bvc_OrderCoupon)
            {
                MigrationServices.OrderCouponDTO c = new MigrationServices.OrderCouponDTO();
                c.CouponCode = oldCoupon.CouponCode ?? string.Empty;
                c.IsUsed = true;
                c.OrderBvin = o.ID.ToString() ?? string.Empty;
                result.Add(c);
            }

            return result;
        }
        private List<MigrationServices.OrderNoteDTO> TranslateNotes(data.bvc_Order o)
        {
            List<MigrationServices.OrderNoteDTO> result = new List<MigrationServices.OrderNoteDTO>();
            
            foreach (data.bvc_OrderNote item in o.bvc_OrderNote)
            {
                MigrationServices.OrderNoteDTO n = new MigrationServices.OrderNoteDTO();
                n.AuditDate = item.AuditDate;
                n.IsPublic = false;
                n.LastUpdatedUtc = item.AuditDate;
                n.Note = item.Note ?? string.Empty;
                n.OrderID = o.ID.ToString();
                result.Add(n);
            }

            return result;
        }
        private List<MigrationServices.OrderPackageDTO> TranslatePackages(data.bvc_Order o)
        {
            List<MigrationServices.OrderPackageDTO> result = new List<MigrationServices.OrderPackageDTO>();
            
            foreach (data.bvc_Package item in o.bvc_Package)
            {
                MigrationServices.OrderPackageDTO pak = new MigrationServices.OrderPackageDTO();

                pak.CustomProperties = new List<MigrationServices.CustomPropertyDTO>();
                pak.Description = string.Empty;
                pak.EstimatedShippingCost = (decimal)item.ShippingCost;
                pak.HasShipped = true;
                pak.Height = (decimal)item.Height;
                pak.Items = new List<MigrationServices.OrderPackageItemDTO>();
                pak.LastUpdatedUtc = item.ShipDate ?? DateTime.UtcNow;
                pak.Length = (decimal)item.Length;
                pak.OrderId = o.ID.ToString();
                pak.ShipDateUtc = item.ShipDate ?? DateTime.UtcNow;
                pak.ShippingMethodId = string.Empty;
                pak.ShippingProviderId = string.Empty;
                pak.ShippingProviderServiceCode = item.ShippingServiceCode;
                pak.SizeUnits = MigrationServices.LengthTypeDTO.Inches;
                pak.TrackingNumber = item.TrackingNumber;
                pak.Weight = (decimal)item.Weight;
                pak.WeightUnits = MigrationServices.WeightTypeDTO.Pounds;
                pak.Width = (decimal)item.Width;
                result.Add(pak);
            }

            return result;
        }        
        private List<MigrationServices.LineItemDTO> TranslateItems(data.bvc_Order o)
        {
            List<MigrationServices.LineItemDTO> result = new List<MigrationServices.LineItemDTO>();
            
            foreach (data.bvc_OrderItem item in o.bvc_OrderItem)
            {
                MigrationServices.LineItemDTO li = new MigrationServices.LineItemDTO();

                li.BasePricePerItem = ((decimal)item.LineTotal / (decimal)item.Qty);
                li.CustomProperties = TranslateOldProperties(item.ExtraInformation);
                li.DiscountDetails = new List<MigrationServices.DiscountDetailDTO>();
                li.ExtraShipCharge = 0;
                li.Id = -1;
                li.LastUpdatedUtc = DateTime.UtcNow;
                li.OrderBvin = o.ID.ToString();
                li.ProductId = item.ProductID;
                li.ProductName = item.DisplayDescription;
                li.ProductShortDescription = item.DisplayDescription;

                li.ProductSku = item.ProductID;
                var displaySku = li.CustomProperties.Where(y => y.Key == "BVDISPLAYSKU").FirstOrDefault();
                if (displaySku != null)
                {
                    li.ProductSku = displaySku.Value;
                }                
                li.ProductShippingHeight = (decimal)item.Height;
                li.ProductShippingLength = (decimal)item.Length;
                li.ProductShippingWeight = (decimal)item.Weight;
                li.ProductShippingWidth = (decimal)item.Width;
                li.Quantity = (int)item.Qty;
                li.QuantityReturned = (int)item.QtyReturned;
                li.QuantityShipped = (int)item.QtyShipped;
                li.SelectionData = new List<MigrationServices.OptionSelectionDTO>();
                li.ShipFromAddress = new MigrationServices.AddressDTO();
                li.ShipFromMode = MigrationServices.ShippingModeDTO.ShipFromSite;
                li.ShipFromNotificationId = string.Empty;
                li.ShippingPortion = 0;
                li.ShippingSchedule = 0;
                li.ShipSeparately = false;
                li.StatusCode = string.Empty;
                li.StatusName = string.Empty;
                li.TaxPortion = 0m;
                li.TaxSchedule = 0;
                li.VariantId = string.Empty;

                // Calculate Adjustments and Discounts
                decimal lineTotal = (decimal)item.LineTotal;
                decimal prediscountTotal = (li.BasePricePerItem * (decimal)li.Quantity);
                decimal allDiscounts = prediscountTotal - lineTotal;
                if (allDiscounts != 0)
                {
                    li.DiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * allDiscounts, Description = "BVC2004 Discounts", Id = new Guid() });
                }

                result.Add(li);
            }

            return result;
        }
        private void ImportOrderTransactions(string orderBvin, string orderNumber, data.bvc_Order old)
        {
            wl(" - Transactions for Order " + orderNumber);

            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();
            
            foreach (data.bvc_OrderPayment item in old.bvc_OrderPayment)
            {
                wl("Transaction: " + item.ID);

                
                bool hasAuth = (item.AuthorizationOnly == 1) && (item.PaymentType == 2) && (item.Amount != 0);
                bool hasCharge = (item.PaymentType == 2) && (item.Amount != 0);
                bool hasRefund = (item.PaymentType == 3) && (item.Amount != 0);
                
                if (hasAuth)
                {
                    MigrationServices.OrderTransactionDTO opAuth = new MigrationServices.OrderTransactionDTO();
                    opAuth.Id = new Guid();
                                                        
                    switch (item.PaymentMethod)
                    {
                        case 1: // Credit Card
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case 7: // Telephone
                        case 9: // Fax
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 3: // Check
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 4: // Cash
                        case 6: // Other
                        case 8: // Email
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 10: // PO
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.PurchaseOrderInfo;
                            break;
                        case 5: // Gift Card
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.GiftCardHold;
                            break;
                        case 2: // PayPal Express
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.PayPalHold;
                            break;
                        default:
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                    }
                    opAuth.Amount = (decimal)item.Amount;
                    if (hasRefund)
                    {
                        item.Amount = item.Amount * -1;
                    }
                    opAuth.CheckNumber = item.CheckNumber ?? string.Empty;
                    opAuth.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opAuth.CreditCard.CardHolderName = item.CreditCardHolder ?? string.Empty;
                    opAuth.CreditCard.CardIsEncrypted = true;
                    opAuth.CreditCard.CardNumber = string.Empty;
                    opAuth.CreditCard.ExpirationMonth = item.CreditCardExpMonth;
                    opAuth.CreditCard.ExpirationYear = item.CreditCardExpYear;
                    opAuth.CreditCard.SecurityCode = string.Empty;
                    opAuth.GiftCardNumber = item.GiftCertificateNumber ?? string.Empty;
                    opAuth.LinkedToTransaction = string.Empty;
                    opAuth.Messages = item.Note ?? string.Empty;
                    opAuth.OrderId = orderBvin ?? string.Empty;
                    opAuth.OrderNumber = orderNumber ?? string.Empty;
                    opAuth.PurchaseOrderNumber = item.PurchaseOrderNumber ?? string.Empty;
                    opAuth.RefNum1 = item.TransactionReferenceNumber ?? string.Empty;
                    opAuth.RefNum2 = item.TransactionResponseCode ?? string.Empty;
                    opAuth.Success = true;
                    opAuth.TimeStampUtc = item.AuditDate;
                    opAuth.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opAuth);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.ID.ToString());
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.ID.ToString());
                    }
                }

                if (hasCharge)
                {
                    MigrationServices.OrderTransactionDTO opCharge = new MigrationServices.OrderTransactionDTO();
                    opCharge.Id = new Guid();
                    switch (item.PaymentMethod)
                    {
                        case 1: // Credit Card
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case 7: // Telephone
                        case 9: // Fax
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 3: // Check
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 4: // Cash
                        case 6: // Other
                        case 8: // Email
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 10: // PO
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.PurchaseOrderInfo;
                            break;
                        case 5: // Gift Card
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.GiftCardHold;
                            break;
                        case 2: // PayPal Express
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.PayPalHold;
                            break;
                        default:
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                    }
                    opCharge.Amount = (decimal)item.Amount;
                    opCharge.CheckNumber = item.CheckNumber ?? string.Empty;
                    opCharge.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opCharge.CreditCard.CardHolderName = item.CreditCardHolder ?? string.Empty;
                    opCharge.CreditCard.CardIsEncrypted = true;
                    opCharge.CreditCard.CardNumber = string.Empty;
                    opCharge.CreditCard.ExpirationMonth = item.CreditCardExpMonth;
                    opCharge.CreditCard.ExpirationYear = item.CreditCardExpYear;
                    opCharge.CreditCard.SecurityCode = string.Empty;
                    opCharge.GiftCardNumber = item.GiftCertificateNumber ?? string.Empty;
                    opCharge.LinkedToTransaction = string.Empty;
                    opCharge.Messages = item.Note ?? string.Empty;
                    opCharge.OrderId = orderBvin ?? string.Empty;
                    opCharge.OrderNumber = orderNumber ?? string.Empty;
                    opCharge.PurchaseOrderNumber = item.PurchaseOrderNumber ?? string.Empty;
                    opCharge.RefNum1 = item.TransactionReferenceNumber ?? string.Empty;
                    opCharge.RefNum2 = item.TransactionResponseCode ?? string.Empty;
                    opCharge.Success = true;
                    opCharge.TimeStampUtc = item.AuditDate;
                    opCharge.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opCharge);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.ID.ToString());
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.ID.ToString());
                    }
                }

                if (hasRefund)
                {
                    MigrationServices.OrderTransactionDTO opRefund = new MigrationServices.OrderTransactionDTO();
                    opRefund.Id = new Guid();
                    switch (item.PaymentMethod)
                    {
                        case 1: // Credit Card
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case 7: // Telephone
                        case 9: // Fax
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 3: // Check
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 4: // Cash
                        case 6: // Other
                        case 8: // Email
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case 10: // PO
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.PurchaseOrderInfo;
                            break;
                        case 5: // Gift Card
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.GiftCardHold;
                            break;
                        case 2: // PayPal Express
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.PayPalHold;
                            break;
                        default:
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                    }
                    opRefund.Amount = -1 * (decimal)item.Amount;
                    opRefund.CheckNumber = item.CheckNumber ?? string.Empty;
                    opRefund.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opRefund.CreditCard.CardHolderName = item.CreditCardHolder ?? string.Empty;
                    opRefund.CreditCard.CardIsEncrypted = true;
                    opRefund.CreditCard.CardNumber = string.Empty;
                    opRefund.CreditCard.ExpirationMonth = item.CreditCardExpMonth;
                    opRefund.CreditCard.ExpirationYear = item.CreditCardExpYear;
                    opRefund.CreditCard.SecurityCode = string.Empty;
                    opRefund.GiftCardNumber = item.GiftCertificateNumber ?? string.Empty;
                    opRefund.LinkedToTransaction = string.Empty;
                    opRefund.Messages = item.Note ?? string.Empty;
                    opRefund.OrderId = orderBvin ?? string.Empty;
                    opRefund.OrderNumber = orderNumber ?? string.Empty;
                    opRefund.PurchaseOrderNumber = item.PurchaseOrderNumber ?? string.Empty;
                    opRefund.RefNum1 = item.TransactionReferenceNumber ?? string.Empty;
                    opRefund.RefNum2 = item.TransactionResponseCode ?? string.Empty;
                    opRefund.Success = true;
                    opRefund.TimeStampUtc = item.AuditDate;
                    opRefund.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opRefund);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.ID.ToString());
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.ID.ToString());
                    }
                }
            }
        }

        private void ImportRelatedItems()
        {
            Header("NOT SUPPORTED IN BVC 2004 - Importing Related Items");

            //data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            //MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            //var crosses = db.bvc_Product.bvcbvc_ProductXProduct;
            //if (crosses == null) return;
            //foreach (data.bvc_ProductCrossSell x in crosses)
            //{
            //    wl("Relating " + x.ProductBvin + " to " + x.CrossSellBvin);
            //    proxy.AssociateProducts(settings.ApiKey, x.ProductBvin, x.CrossSellBvin, false);
            //}

            //var ups = db.bvc_ProductUpSell;
            //if (ups == null) return;
            //foreach (data.bvc_ProductUpSell up in ups)
            //{
            //    wl("Relating Up " + up.ProductBvin + " to " + up.UpSellBvin);
            //    proxy.AssociateProducts(settings.ApiKey, up.ProductBvin, up.UpSellBvin, true);
            //}
        }

        private void ImportProducts()
        {
            Header("Importing Products");

            int limit = -1;
            if (settings.ImportProductLimit > 0)
            {
                limit = settings.ImportProductLimit;
            }
            //int totalMigrated = 0;

            int pageSize = 100;
            int totalRecords = oldDatabase.bvc_Product.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            for (int i = 0; i < totalPages; i++)
            {
                wl("Getting Products page " + (i + 1) + " of " + totalPages.ToString());
                int startRecord = i * pageSize;
                var products = (from p in oldDatabase.bvc_Product select p).OrderBy(y => y.ID).Skip(startRecord).Take(pageSize).ToList();

                System.Threading.Tasks.Parallel.ForEach(products, ImportSingleProduct);

                //foreach (data.bvc_Product p in products)
                //{
                //    ImportSingleProduct(p);

                //    totalMigrated += 1;
                //    if (limit > 0 && totalMigrated >= limit)
                //    {
                //        return;
                //    }
                //}
            }
        }
        private List<MigrationServices.CustomPropertyDTO> TranslateOldProperties(string oldXml)
        {
            List<MigrationServices.CustomPropertyDTO> result = new List<MigrationServices.CustomPropertyDTO>();

            List<CustomProperty> props = CustomProperty.ReadExtraInfo(oldXml);            
            if (props != null)
            {
                foreach (CustomProperty prop in props)
                {
                    result.Add(prop.ToDto());
                }
            }
            return result;
        }
        private void ImportSingleProduct(data.bvc_Product old)
        {
            if (old == null) return;

            wl("Product: " + old.ProductName + " [" + old.ID + "]");
            MigrationServices.ProductDTO p = new MigrationServices.ProductDTO();
            p.AllowReviews = true;
            p.Bvin = old.ID;
            p.CreationDateUtc = old.CreationDate;
            p.CustomProperties = new List<MigrationServices.CustomPropertyDTO>();
            p.Featured = false;
            p.GiftWrapAllowed = true;
            p.GiftWrapPrice = 0;
            p.ImageFileSmall = System.IO.Path.GetFileName(old.ImageFileMedium);
            p.ImageFileSmallAlternateText = old.ProductName;

            p.InventoryMode = MigrationServices.ProductInventoryModeDTO.AlwayInStock;
            switch(old.InventoryNotAvailableStatus)
            {
                case 1: // Ignore Inventory
                    p.InventoryMode = MigrationServices.ProductInventoryModeDTO.AlwayInStock;
                    break;
                case 2: // Allow Backorders
                    p.InventoryMode = MigrationServices.ProductInventoryModeDTO.WhenOutOfStockAllowBackorders;
                    break;
                case 3: // Show, no orders
                    p.InventoryMode = MigrationServices.ProductInventoryModeDTO.WhenOutOfStockShow;
                    break;
                case 0: // Pull from store
                    p.InventoryMode = MigrationServices.ProductInventoryModeDTO.WhenOutOfStockHide;
                    break;
            }                        
            p.IsAvailableForSale = true;
            p.Keywords = string.Empty;
            p.ListPrice = (decimal)old.ListPrice;
            p.LongDescription = old.LongDescription;
            p.ManufacturerId = old.ManufacturerID.ToString();
            p.MetaDescription = old.MetaDescription;
            p.MetaKeywords = old.MetaKeywords;
            p.MetaTitle = old.MetaTitle;
            p.MinimumQty = old.MinimumQty;
            p.PostContentColumnId = string.Empty;
            p.PreContentColumnId = string.Empty;
            p.PreTransformLongDescription = string.Empty;
            p.ProductName = old.ProductName;
            p.ProductTypeId = old.ProductTypeID.ToString();
            p.ShippingDetails = new MigrationServices.ShippableItemDTO();
            p.ShippingDetails.ExtraShipFee = old.ExtraShipFee;
            p.ShippingDetails.Height = (decimal)old.ShippingHeight;
            p.ShippingDetails.IsNonShipping = old.NonShipping == 1;
            p.ShippingDetails.Length = (decimal)old.ShippingLength;
            p.ShippingDetails.ShippingScheduleId = 0;
            p.ShippingDetails.ShipSeparately = old.ShipSeparately == 1;
            p.ShippingDetails.Weight = (decimal)old.ShippingWeight;
            p.ShippingDetails.Width = (decimal)old.ShippingWeight;
            switch (old.DropShipMode)
            {
                case 1:
                    p.ShippingMode = MigrationServices.ShippingModeDTO.ShipFromSite;
                    break;
                case 2:
                    p.ShippingMode = MigrationServices.ShippingModeDTO.ShipFromVendor;
                    break;
                case 3:
                    p.ShippingMode = MigrationServices.ShippingModeDTO.ShipFromManufacturer;
                    break;
                default:
                    p.ShippingMode = MigrationServices.ShippingModeDTO.ShipFromSite;
                    break;
            }
            p.ShortDescription = old.ShortDescription;
            p.SiteCost = (decimal)old.SiteCost;
            p.SitePrice = (decimal)old.SitePrice;
            p.SitePriceOverrideText = string.Empty;
            p.Sku = old.ID;
            switch (old.Status)
            {
                case 0:
                    p.Status = MigrationServices.ProductStatusDTO.Disabled;
                    break;
                default:
                    p.Status = MigrationServices.ProductStatusDTO.Active;
                    break;
            }
            p.Tabs = new List<MigrationServices.ProductDescriptionTabDTO>();
            p.TaxExempt = old.TaxExempt == 1;
            p.TaxSchedule = 0;
            if (TaxScheduleMapper.ContainsKey(old.TaxClass)) p.TaxSchedule = TaxScheduleMapper[old.TaxClass];
            p.VendorId = old.VendorID.ToString();

            byte[] bytes = GetBytesForLocalImage(old.ImageFileMedium);
            if (bytes != null)
            {
                wl("Found Image: " + p.ImageFileSmall + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            }
            else
            {
                wl("Missing Image: " + p.ImageFileSmall);
                bytes = new byte[0];
            }

            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();
            var res = proxy.MigrateProduct(settings.ApiKey, p, bytes);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    AssignOptionsToProduct(old.ID);
                    AssignProductPropertyValues(old.ID);
                    wl("SUCCESS");
                }
            }

            // Inventory                        
            MigrateProductInventory(old.ID);

            // Additional Images            
            MigrateProductAdditionalImages(old.ID);

            // Volume Prices
            MigrateProductVolumePrices(old.ID);

            // Reviews
            MigrateProductReviews(old.ID);

            // Link to Categories
            MigrateProductCategoryLinks(old.ID);

            // Assign File Downloads
            AssignFileDownloadsToProduct(old.ID);

        }
        private void AssignOptionsToProduct(string bvin)
        {
            wl(" - Migrating Options...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));

            var item = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (item == null) return;

            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var choices = item.bvc_ProductXProductChoices.OrderBy(y => y.SortOrder);
            if (choices == null) return;
            foreach (data.bvc_ProductXProductChoices choice in choices)
            {
                proxy.AssignOptionToProduct(settings.ApiKey, bvin, choice.ProductChoiceID.ToString());
            }
        }
        private void AssignProductPropertyValues(string bvin)
        {
            wl(" - Migrating Property Values...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var itemMain = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (itemMain == null) return;

            var items = itemMain.bvc_ProductPropertyValue;
            if (items == null) return;
            foreach (data.bvc_ProductPropertyValue item in items)
            {
                long newId = 0;
                if (ProductPropertyMapper.ContainsKey(item.PropertyID)) newId = ProductPropertyMapper[item.PropertyID];
                if (newId > 0)
                {
                    proxy.SetProductPropertyValue(settings.ApiKey, bvin, newId, item.PropertyValue, -1);
                }
            }
        }
        private void MigrateProductInventory(string bvin)
        {
            wl(" - Migrating Inventory...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var itemMain = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (itemMain == null) return;

            var old = itemMain.bvc_ProductInventory.FirstOrDefault();
            if (old == null) return;

            MigrationServices.ProductInventoryDTO inv = new MigrationServices.ProductInventoryDTO();
            inv.LowStockPoint = 0;
            inv.ProductBvin = bvin;
            inv.QuantityOnHand = old.Qty;
            inv.QuantityReserved = 0;

            var res = proxy.SetInventoryLevelsForProduct(settings.ApiKey, bvin, inv);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
            }
            else
            {
                wl("FAILED! EXCEPTION!");
            }
        }
        private void MigrateProductAdditionalImages(string bvin)
        {
            wl(" - Migrating AdditionalImages...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductImage.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductImage old in items)
            {
                MigrationServices.ProductImageDTO img = new MigrationServices.ProductImageDTO();
                img.AlternateText = old.Caption;
                img.Bvin = old.ImageID.ToString();
                img.Caption = old.Caption;
                img.FileName = System.IO.Path.GetFileName(old.FileName);
                img.LastUpdatedUtc = DateTime.UtcNow;
                img.ProductId = old.ProductID;
                img.SortOrder = 0;

                byte[] bytes = GetBytesForLocalImage(old.FileName);
                if (bytes == null) return;
                wl("Found Image: " + img.FileName + " [" + bytes.Length + " bytes]");


                var res = proxy.MigrateAdditionalImage(settings.ApiKey, img, bytes);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void AssignFileDownloadsToProduct(string bvin)
        {
            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var itemMain = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (itemMain == null) return;

            var crosses = itemMain.bvc_ProductFile.OrderBy(y => y.FileID);
            if (crosses == null) return;
            foreach (data.bvc_ProductFile x in crosses)
            {
                wl("Linking Product " + bvin + " to " + x.FileID.ToString());
                var res2 = proxy.AssociateProductFile(settings.ApiKey, bvin, x.FileID.ToString(), x.AvailableMinutes, x.MaxDownloads);
                if (res2 != null)
                {
                    if (res2.Errors.Count > 0)
                    {
                        DumpErrors(res2.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductFileDownloads()
        {
            Header("Migrating File Downloads");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductFile;
            if (items == null) return;
            foreach (data.bvc_ProductFile old in items)
            {
                wl("File: " + old.FileName);

                string safeFileName = "\\files\\" + old.FileID.ToString() + "_" + old.FileName + ".config";

                byte[] bytes = GetBytesForLocalImage(safeFileName);
                if (bytes == null)
                {
                    wl("Missing File: " + old.FileName);
                    continue;
                }
                else
                {
                    wl("Found File: " + old.FileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
                }

                int totalChunks = 0;
                byte[] partial = null;
                if (bytes != null && bytes.Length > 0)
                {
                    double ChunkCount = 0;
                    ChunkCount = (double)bytes.Length / (double)CHUNKSIZE;
                    ChunkCount = Math.Ceiling(ChunkCount);
                    totalChunks = (int)ChunkCount;
                    if (totalChunks > 0)
                    {
                        partial = GetAChunkFromFullBytes(bytes, 0);
                    }
                }

                var res = proxy.MigrateProductFileFirstPart(settings.ApiKey, old.FileID.ToString(), old.FileName, old.Title, partial);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }

                    wl("File Created");
                    if (totalChunks > 1)
                    {
                        wl("Uploading: ");
                        for (int i = 1; i < totalChunks; i++)
                        {
                            partial = GetAChunkFromFullBytes(bytes, i);
                            wl("+ " + old.FileName + " [" + FriendlyFormatBytes(bytes.Length) + "] part " + (i + 1) + " of " + totalChunks.ToString());
                            var res2 = proxy.MigrateProductFileAdditionalPart(settings.ApiKey, old.FileID.ToString(), old.FileName, partial);
                        }
                    }
                    wl("File Done Uploading!");
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }

           
        }
        private byte[] GetAChunkFromFullBytes(byte[] fullBytes, int chunkIndex)
        {
            // Get A Chunk
            byte[] chunk = null;

            try
            {
                int startIndex = (chunkIndex) * CHUNKSIZE;
                if (startIndex + CHUNKSIZE > fullBytes.Length)
                {
                    chunk = new byte[fullBytes.Length - startIndex];
                }
                else
                {
                    chunk = new byte[CHUNKSIZE];
                }

                Array.Copy(fullBytes, startIndex, chunk, 0, chunk.Length);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION: " + ex.Message + " | " + ex.StackTrace);
            }
            return chunk;
        }
        private string FriendlyFormatBytes(long sizeInBytes)
        {
            if (sizeInBytes < 1024)
            {
                return sizeInBytes + " bytes";
            }
            else
            {
                if (sizeInBytes < 1048576)
                {
                    return Math.Round((double)sizeInBytes / 1024, 1) + " KB";
                }
                else
                {
                    return Math.Round((double)sizeInBytes / 1048576, 1) + " MB";
                }
            }
        }
        private void MigrateProductVolumePrices(string bvin)
        {
            wl(" - Migrating Volume Prices...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var itemMain = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (itemMain == null) return;

            var items = db.bvc_ProductVolumeDiscounts.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductVolumeDiscounts item in items)
            {
                MigrationServices.ProductVolumeDiscountDTO v = new MigrationServices.ProductVolumeDiscountDTO();
                
                v.Bvin = item.ProductID;

                v.Amount = (decimal)item.Price;
                v.DiscountType = MigrationServices.ProductVolumeDiscountTypeDTO.Amount;

                v.LastUpdated = DateTime.UtcNow;
                v.ProductId = item.ProductID;
                v.Qty = item.Qty;

                wl("Discount for qty: " + v.Qty + " [" + v.Bvin + "]");
                var res = proxy.MigrationProductVolumeDiscount(settings.ApiKey, v);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductReviews(string bvin)
        {
            wl(" - Migrating Reviews...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductReview.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductReview item in items)
            {
                MigrationServices.ProductReviewDTO r = new MigrationServices.ProductReviewDTO();
                r.Approved = item.Approved == 1;
                r.Bvin = item.ID.ToString();
                r.Description = item.Description;
                r.Karma = item.Karma;
                r.ProductBvin = item.ProductID;
                switch (item.Rating)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
                r.ReviewDateUtc = item.ReviewDate;
                r.UserID = item.UserID.ToString();

                wl("Review [" + r.Bvin + "]");
                var res = proxy.MigrateProductReview(settings.ApiKey, r);
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }
        private void MigrateProductCategoryLinks(string bvin)
        {
            wl(" - Migrating Category Links");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var itemMain = db.bvc_Product.Where(y => y.ID == bvin).FirstOrDefault();
            if (itemMain == null) return;

            var items = itemMain.bvc_Category;
            if (items == null) return;
            foreach (data.bvc_Category item in items)
            {
                wl("To Category: " + item.ID.ToString());
                var res = proxy.MigrationProductXCategory(settings.ApiKey, bvin, item.ID.ToString());
                if (res != null)
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }
        }

        // Choices, Modifiers, Inputs               
        private void ImportProductChoices()
        {
            Header("Importing Shared Choices");

            foreach (data.bvc_ProductChoices old in oldDatabase.bvc_ProductChoices)
            {
                
                MigrationServices.OptionDTO o = new MigrationServices.OptionDTO();
                string fullName = old.PropertyName;
                if (old.DisplayName.Trim().Length > 0) fullName = old.DisplayName;

                o.Settings = new List<MigrationServices.OptionSettingDTO>();
                o.Items = new List<MigrationServices.OptionItemDTO>();
                o.Bvin = old.ID.ToString();
                o.IsShared = old.Shared == 1;
                o.IsVariant = true; // Choices are always variants in BV6
                o.NameIsHidden = false;

                switch (old.TypeCode)
                {
                    case 9: //ProductChoiceType.HtmlArea    
                        o.OptionType = MigrationServices.OptionTypesDTO.Html;
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "html", Value = old.Html });
                        break;
                    case 2: //ProductChoiceType.MultipleChoiceField
                        o.OptionType = MigrationServices.OptionTypesDTO.DropDownList;
                        break;
                    case 8: // ProductChoiceType.RadioButtonList
                        o.OptionType = MigrationServices.OptionTypesDTO.RadioButtonList;
                        break;
                    case 1: // TextField
                        o.OptionType = MigrationServices.OptionTypesDTO.TextInput;
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "rows", Value = old.TextRows.ToString() });
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "cols", Value = old.TextColumns.ToString() });
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "required", Value = (old.Html == "REQUIRED" ? "1" : "0") });
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "wraptext", Value = (old.TextWrap == 1 ? "1" : "0") });
                        o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "maxlength", Value = "255" });
                        break;
                    case 21: // AccessoryCheckBox
                        continue;
                    case 23: // AccessoryDropdownList
                        continue;
                    case 22: // AccessoryRadioButtonList                            
                        continue;
                }               
                o.Name = fullName;
                wl("Choice: " + fullName);

                // Load Items for Option Here
                o.Items = LoadOptionItemsChoice(old.ID);

                var res = bv6proxy.MigrateOption(settings.ApiKey, o);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }       
        private List<MigrationServices.OptionItemDTO> LoadOptionItemsChoice(int choiceID)
        {
            List<MigrationServices.OptionItemDTO> result = new List<MigrationServices.OptionItemDTO>();

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            var items = db.bvc_ProductChoices_Item.Where(y => y.ChoiceID == choiceID).OrderBy(y => y.SortOrder);
            if (items == null) return result;

            foreach (data.bvc_ProductChoices_Item item in items)
            {
                MigrationServices.OptionItemDTO dto = new MigrationServices.OptionItemDTO();
                dto.Bvin = item.ID.ToString();
                dto.IsLabel = item.NullItem == 1;
                dto.Name = item.DisplayText;
                dto.OptionBvin = choiceID.ToString();
                dto.PriceAdjustment = (decimal)item.PriceAdjustment;
                dto.SortOrder = item.SortOrder;
                dto.WeightAdjustment = (decimal)item.WeightAdjustment;                
                result.Add(dto);
            }

            return result;
        }

        // Categories
        private void ImportCategories()
        {
            Header("Importing Categories");

            foreach (data.bvc_Category old in oldDatabase.bvc_Category)
            {
                wl("Category: " + old.Name);

                MigrationServices.CategoryDTO cat = new MigrationServices.CategoryDTO();
                cat.BannerImageUrl = System.IO.Path.GetFileName(old.BannerImageURL);
                cat.Bvin = old.ID.ToString();
                cat.Criteria = string.Empty;
                cat.CustomerChangeableSortOrder = false;
                cat.CustomPageId = string.Empty;
                cat.CustomPageLayout = MigrationServices.CustomPageLayoutTypeDTO.WithSideBar;
                cat.CustomPageOpenInNewWindow = old.CustomPageNewWindow == 1;
                cat.CustomPageUrl = old.CustomPageURL;
                cat.Description = old.Description;
                switch (old.DisplaySortOrder)
                {
                    case 0:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.None;
                        break;
                    case 1:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.ManualOrder;
                        break;
                    case 2:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.ProductName;
                        break;
                    case 3:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.ProductPriceAscending;
                        break;
                    case 4:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.ProductPriceDescending;
                        break;
                    case 5:
                        cat.DisplaySortOrder = MigrationServices.CategorySortOrderDTO.ManufacturerName;
                        break;
                }
                cat.Hidden = old.Hidden == 1;
                cat.ImageUrl = System.IO.Path.GetFileName(old.ImageURL);
                cat.Keywords = string.Empty;
                cat.LastUpdatedUtc = DateTime.UtcNow;
                cat.LatestProductCount = old.LatestProductCount;
                cat.MetaDescription = old.MetaDescription;
                cat.MetaKeywords = old.MetaKeywords;
                cat.MetaTitle = old.MetaTitle;
                cat.Name = old.Name;
                cat.Operations = new List<MigrationServices.ApiOperation>();
                cat.ParentId = old.ParentID.ToString();
                cat.PostContentColumnId = string.Empty;
                cat.PreContentColumnId = string.Empty;
                cat.PreTransformDescription = old.Description;
                cat.RewriteUrl = string.Empty;
                cat.ShowInTopMenu = old.ShowInTopMenu == 1;
                cat.ShowTitle = old.ShowTitle == 1;
                cat.SortOrder = old.SortOrder;
                cat.SourceType = MigrationServices.CategorySourceTypeDTO.Manual;
                switch (old.SourceType)
                {
                    case 0:
                        cat.SourceType = MigrationServices.CategorySourceTypeDTO.Manual;
                        break;
                    case 1:
                        cat.SourceType = MigrationServices.CategorySourceTypeDTO.ByRules;
                        break;
                    case 2:
                        cat.SourceType = MigrationServices.CategorySourceTypeDTO.CustomLink;
                        break;
                }
                switch(old.DisplayType)
                {
                    case 1:
                        cat.TemplateName = "Grid";
                        break;
                    case 2:
                        cat.TemplateName = "Simple List";
                        break;
                    case 3:
                        cat.TemplateName = "Detailed List";
                        break;
                    default:
                        cat.TemplateName = "Grid";
                        break;
                }                 

                var res = bv6proxy.MigrateCategory(settings.ApiKey, cat);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        string bannerImageSource = old.BannerImageURL;
                        string imageSource = old.ImageURL;
                        MigrateCategoryBanner(old.ID, bannerImageSource);
                        MigrateCategoryImage(old.ID, imageSource);
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void MigrateCategoryImage(int catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            bv6proxy.ImagesUploadCategoryImage(settings.ApiKey, catBvin.ToString(), fileName, bytes);
        }
        private void MigrateCategoryBanner(int catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            bv6proxy.ImagesUploadCategoryBanner(settings.ApiKey, catBvin.ToString(), fileName, bytes);
        }

        private byte[] GetBytesForLocalImage(string relativePath)
        {
            byte[] result = null;
            string source = settings.ImagesRootFolder;
            string relative = relativePath.Replace('/', '\\');
            if (relative.StartsWith("\\") == false)
            {
                relative = "\\" + relative;
            }
            source = source + relative;
            if (File.Exists(source))
            {
                result = File.ReadAllBytes(source);
            }
            return result;
        }

        // Properties and Types
        private void ImportProductTypes()
        {
            Header("Importing Product Types");

            foreach (data.bvc_ProductType old in oldDatabase.bvc_ProductType)
            {
                wl("Item: " + old.ProductTypeName);

                MigrationServices.ProductTypeDTO pt = new MigrationServices.ProductTypeDTO();

                pt.Bvin = old.ID.ToString();
                pt.IsPermanent = false;
                pt.LastUpdated = DateTime.UtcNow;
                pt.ProductTypeName = old.ProductTypeName;

                var res = bv6proxy.MigrateProductType(settings.ApiKey, pt);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        MigratePropertiesForType(old.ID);
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void MigratePropertiesForType(int typeID)
        {
            wl("Migrating Properties to Type...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            var crosses = db.bvc_ProductTypeProperty.Where(y => y.bvc_ProductProperty.ID == typeID).OrderBy(y => y.SortOrder);
            if (crosses == null) return;

            foreach (data.bvc_ProductTypeProperty cross in crosses)
            {
                int sort = cross.SortOrder;
                int oldPropertyBvin = cross.ProductPropertyID;
                long newId = 0;
                if (ProductPropertyMapper.ContainsKey(oldPropertyBvin))
                {
                    newId = ProductPropertyMapper[oldPropertyBvin];
                }
                if (newId <= 0) continue;
                wl("Mapping " + oldPropertyBvin + " to " + newId.ToString());
                bv6proxy.AssignProductPropertyToType(settings.ApiKey, typeID.ToString(), newId, sort);
            }
        }
        private void ImportProductProperties()
        {
            Header("Importing Product Properties");

            ProductPropertyMapper = new Dictionary<int, long>();

            foreach (data.bvc_ProductProperty old in oldDatabase.bvc_ProductProperty)
            {
                wl("Item: " + old.DisplayName);

                MigrationServices.ProductPropertyDTO pp = new MigrationServices.ProductPropertyDTO();

                pp.Choices = GetPropertyChoices(old.ID);
                pp.CultureCode = old.CultureCode;
                pp.DefaultValue = old.DefaultValue;
                pp.DisplayName = old.DisplayName;
                pp.DisplayOnSite = old.DisplayOnSite == 1;
                pp.DisplayToDropShipper = old.DisplayToDropShipper == 1;
                pp.PropertyName = old.PropertyName;

                switch (old.TypeCode)
                {
                    case 0:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.None;
                        break;
                    case 1:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.TextField;
                        break;
                    case 2:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.MultipleChoiceField;
                        break;
                    case 3:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.CurrencyField;
                        break;
                    case 4:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.DateField;
                        break;
                    case 7:
                        pp.TypeCode = MigrationServices.ProductPropertyTypeDTO.HyperLink;
                        break;
                }

                var res = bv6proxy.MigrateProductProperty(settings.ApiKey, pp);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        long newId = res.Content;
                        ProductPropertyMapper.Add(old.ID, newId);
                        wl("SUCCESS");
                    }
                }
            }

        }
        private List<MigrationServices.ProductPropertyChoiceDTO> GetPropertyChoices(int propertyId)
        {
            List<MigrationServices.ProductPropertyChoiceDTO> result = new List<MigrationServices.ProductPropertyChoiceDTO>();

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));
            var choices = db.bvc_ProductPropertyChoice.Where(y => y.PropertyID == propertyId)
                            .OrderBy(y => y.SortOrder);
            if (choices == null) return result;

            foreach (data.bvc_ProductPropertyChoice ppc in choices)
            {
                MigrationServices.ProductPropertyChoiceDTO dto = new MigrationServices.ProductPropertyChoiceDTO();
                dto.ChoiceName = ppc.ChoiceName;
                dto.LastUpdated = DateTime.UtcNow;
                //dto.PropertyId = ppc.PropertyBvin;
                dto.SortOrder = ppc.SortOrder;
                result.Add(dto);
            }

            return result;
        }

        // Manufacturer Vendor
        private void ImportManufacturers()
        {
            Header("Importing Manufacturers");

            foreach (data.bvc_Manufacturer old in oldDatabase.bvc_Manufacturer)
            {
                wl("Item: " + old.DisplayName);

                MigrationServices.VendorManufacturerDTO vm = new MigrationServices.VendorManufacturerDTO();

                BVC2004Address oldAddr = new BVC2004Address();
                oldAddr.FromXmlString(old.Address);
                vm.Address = new MigrationServices.AddressDTO();
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.ID.ToString();
                vm.Contacts = new List<MigrationServices.VendorManufacturerContactDTO>();
                vm.ContactType = MigrationServices.VendorManufacturerTypeDTO.Manufacturer;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = string.Empty;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = DateTime.UtcNow;

                var res = bv6proxy.MigrateVendorManufacturer(settings.ApiKey, vm);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void ImportVendors()
        {
            Header("Importing Vendors");

            foreach (data.bvc_Vendor old in oldDatabase.bvc_Vendor)
            {
                wl("Item: " + old.DisplayName);

                MigrationServices.VendorManufacturerDTO vm = new MigrationServices.VendorManufacturerDTO();

                BVC2004Address oldAddr = new BVC2004Address();
                oldAddr.FromXmlString(old.Address);
                vm.Address = new MigrationServices.AddressDTO();
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.ID.ToString();
                vm.Contacts = new List<MigrationServices.VendorManufacturerContactDTO>();
                vm.ContactType = MigrationServices.VendorManufacturerTypeDTO.Vendor;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = string.Empty;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = DateTime.UtcNow;

                var res = bv6proxy.MigrateVendorManufacturer(settings.ApiKey, vm);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }
        }

        // Taxes
        private void ImportTaxSchedules()
        {
            Header("Importing Tax Schedules");

            TaxScheduleMapper = new Dictionary<int, long>();

            foreach (data.bvc_TaxClass old in oldDatabase.bvc_TaxClass)
            {
                wl("Tax Schedule: " + old.DisplayName);

                MigrationServices.TaxScheduleDTO ts = new MigrationServices.TaxScheduleDTO();
                ts.Name = old.DisplayName;

                var res = bv6proxy.MigrateTaxSchedule(settings.ApiKey, ts);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        long newId = res.Content;
                        TaxScheduleMapper.Add(old.ID, newId);
                        wl("SUCCESS");
                    }
                }
            }

            // Migrate Default Tax Schedule
            MigrationServices.TaxScheduleDTO defaultTaxSchedule = new MigrationServices.TaxScheduleDTO();
            defaultTaxSchedule.Name = "Default";
            var res2 = bv6proxy.MigrateTaxSchedule(settings.ApiKey, defaultTaxSchedule);
            if (res2 != null)
            {
                long defId = res2.Content;
                TaxScheduleMapper.Add(0, defId);
                TaxScheduleMapper.Add(-1, defId);
            }
        }
        private void ImportTaxes()
        {
            Header("Importing Taxes");


            foreach (data.bvc_Tax old in oldDatabase.bvc_Tax)
            {                
                BVSoftware.Web.Geography.Country newCountry = GeographyHelper.TranslateCountry(EFConnString(settings.SourceConnectionString()), old.CountryCode);
                string RegionAbbreviation = GeographyHelper.TranslateRegionBvinToAbbreviation(EFConnString(settings.SourceConnectionString()), old.RegionCode.ToString());

                wl("Tax: " + newCountry.DisplayName + ", " + RegionAbbreviation + " " + old.PostalCode);

                MigrationServices.TaxDTO tx = new MigrationServices.TaxDTO();
                tx.ApplyToShipping = false;
                tx.CountryName = newCountry.DisplayName;
                tx.PostalCode = old.PostalCode;
                tx.Rate = (decimal)old.Rate;
                tx.RegionAbbreviation = RegionAbbreviation;

                int matchId = old.TaxClass;
                if (matchId< 0) matchId = 0;

                if (TaxScheduleMapper.ContainsKey(matchId))
                {
                    tx.TaxScheduleId = TaxScheduleMapper[matchId];
                }

                var res = bv6proxy.MigrateTax(settings.ApiKey, tx);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl("SUCCESS");
                    }
                }
            }

        }

        // Affiliates
        private void ImportAffiliates()
        {
            Header("Importing Affiliates");
            AffiliateMapper = new Dictionary<int, long>();

            foreach (data.bvc_Affiliate aff in oldDatabase.bvc_Affiliate)
            {
                wl("Affiliate: " + aff.DisplayName + " | " + aff.ID);

                try
                {
                    MigrationServices.AffiliateDTO a = OldToNewAffiliate(aff);

                    var res = bv6proxy.MigrateAffiliate(settings.ApiKey, a);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            return;
                        }
                        long newId = res.Content;
                        AffiliateMapper.Add(aff.ID, newId);
                        wl("SUCCESS");
                        ImportAffiliateReferrals(aff.ID, newId);
                    }
                }
                catch (Exception ex)
                {
                    wl("FAILED: " + ex.Message + " | " + ex.StackTrace);
                }

            }
        }
        private MigrationServices.AffiliateDTO OldToNewAffiliate(data.bvc_Affiliate aff)
        {
            MigrationServices.AffiliateDTO affiliate = new MigrationServices.AffiliateDTO();

            BVC2004Address oldAddress = new BVC2004Address();
            oldAddress.FromXmlString(aff.Address);

            affiliate.Address = new MigrationServices.AddressDTO();
            if (oldAddress != null)
            {
                oldAddress.CopyTo(affiliate.Address, EFConnString(settings.SourceConnectionString()));
            }
            affiliate.CommissionAmount = (decimal)aff.CommissionAmount;
            switch (aff.CommissionType)
            {
                case 0:
                    affiliate.CommissionType = MigrationServices.AffiliateCommissionTypeDTO.None;
                    break;
                case 1:
                    affiliate.CommissionType = MigrationServices.AffiliateCommissionTypeDTO.PercentageCommission;
                    break;
                case 2:
                    affiliate.CommissionType = MigrationServices.AffiliateCommissionTypeDTO.FlatRateCommission;
                    break;
                default:
                    affiliate.CommissionType = MigrationServices.AffiliateCommissionTypeDTO.PercentageCommission;
                    break;
            }
            affiliate.CustomThemeName = aff.StyleSheet;
            affiliate.DisplayName = aff.DisplayName;
            affiliate.DriversLicenseNumber = aff.DriversLicenseNumber;
            affiliate.Enabled = true;
            affiliate.Id = -1;
            affiliate.LastUpdatedUtc = DateTime.UtcNow;
            affiliate.Notes = string.Empty;
            affiliate.ReferralDays = aff.ReferralDays;
            affiliate.ReferralId = aff.ID.ToString();
            affiliate.TaxId = aff.TaxID;
            affiliate.WebSiteUrl = aff.WebSiteURL;
            affiliate.Contacts = new List<MigrationServices.AffiliateContactDTO>();

            return affiliate;
        }
        private void ImportAffiliateReferrals(int oldId, long newId)
        {
            wl(" - Migrating Referrals...");

            data.bvc2004Entities db = new data.bvc2004Entities(EFConnString(settings.SourceConnectionString()));

            var referrals = db.bvc_Referral.Where(y => y.AffID == oldId);
            if (referrals == null) return;

            foreach (data.bvc_Referral r in referrals)
            {
                MigrationServices.AffiliateReferralDTO rnew = new MigrationServices.AffiliateReferralDTO();
                rnew.AffiliateId = newId;
                rnew.TimeOfReferralUtc = r.TimeOfReferral;
                rnew.ReferrerUrl = r.ReferrerURL;

                var res = bv6proxy.MigrateAffiliateReferral(settings.ApiKey, rnew);
            }

        }

        // Users
        private void ImportUsers()
        {
            Header("Importing Users");


            int pageSize = 100;
            int totalRecords = oldDatabase.bvc_User.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            //System.Threading.Tasks.Parallel.For(0, totalPages, ProcessPage);
            int startIndex = 0;
            if (settings.UserStartPage > 1)
            {
                startIndex = settings.UserStartPage - 1;
            }
            for (int i = startIndex; i < totalPages; i++)
            {
                wl("Getting Users page " + (i + 1) + " of " + totalPages.ToString());
                int startRecord = i * pageSize;
                var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(pageSize).ToList();
                foreach (data.bvc_User u in users)
                {
                    ImportSingleUser(u);
                }
            }

        }
        private void ProcessPage(int i)
        {
            wl("Getting Users page " + (i + 1));
            int startRecord = i * 100;
            var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(100).ToList();
            foreach (data.bvc_User u in users)
            {
                ImportSingleUser(u);
            }
        }
        private void ImportSingleUser(data.bvc_User u)
        {
            if (u == null)
            {
                wl("Customer was null!");
                return;
            }
            wl("Importing Customer: " + u.Email);

            MigrationServices.CustomerAccountDTO customer = new MigrationServices.CustomerAccountDTO();
            customer.Bvin = u.ID.ToString();
            customer.CreationDateUtc = u.CreationDate;
            customer.Email = u.Email;
            customer.FailedLoginCount = 0;
            customer.FirstName = u.FirstName;
            customer.LastLoginDateUtc = u.LastLoginDate;
            customer.LastName = u.LastName;
            customer.LastUpdatedUtc = DateTime.UtcNow;
            customer.Notes = u.Comment;
            customer.Password = string.Empty;
            customer.PricingGroupId = string.Empty;            
            if (u.PricingLevel > 0)
            {
                customer.PricingGroupId = "BVC2004" + u.PricingLevel.ToString().Trim();
            }
            customer.Salt = string.Empty;
            customer.TaxExempt = u.TaxExempt == 1 ? true : false;
            customer.Addresses = new List<MigrationServices.AddressDTO>();

            // Preserve clear text passwords
            string newPassword = u.Password;
            customer.Password = newPassword;


            List<BVC2004Address> addresses = BVC2004Address.ReadAddressesFromXml(u.AddressBook);
            if (addresses != null)
            {
                foreach (BVC2004Address a in addresses)
                {
                    MigrationServices.AddressDTO addr = new MigrationServices.AddressDTO();
                    addr.AddressType = MigrationServices.AddressTypesDTO.BillingAndShipping;
                    a.CopyTo(addr, EFConnString(settings.SourceConnectionString())); ;
                    customer.Addresses.Add(addr);
                }
            }

            var res = bv6proxy.MigrateCustomerAccount(settings.ApiKey, customer, newPassword);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    return;
                }
                wl("SUCCESS");
            }
        }

        // Price Groups and Roles
        private void ImportPriceGroups()
        {
            Header("Importing Price Groups");

            foreach (data.bvc_PricingLevel oldGroup in oldDatabase.bvc_PricingLevel)
            {
                wl("Price Group: " + oldGroup.PricingLevel.ToString());

                MigrationServices.PriceGroupDTO pg = new MigrationServices.PriceGroupDTO();
                pg.AdjustmentAmount = (decimal)oldGroup.Amount;
                pg.Bvin = "BVC2004" + oldGroup.PricingLevel.ToString();
                pg.Name = "BVC2004" + oldGroup.PricingLevel.ToString();
                switch (oldGroup.PricingType)
                {

                    case 3:
                        pg.PricingType = MigrationServices.PricingTypesDTO.AmountAboveCost;
                        break;
                    case 1:
                        pg.PricingType = MigrationServices.PricingTypesDTO.AmountOffListPrice;
                        break;
                    case 5:
                        pg.PricingType = MigrationServices.PricingTypesDTO.AmountOffSitePrice;
                        break;
                    case 2:
                        pg.PricingType = MigrationServices.PricingTypesDTO.PercentageAboveCost;
                        break;
                    case 0:
                        pg.PricingType = MigrationServices.PricingTypesDTO.PercentageOffListPrice;
                        break;
                    case 4:
                        pg.PricingType = MigrationServices.PricingTypesDTO.PercentageOffSitePrice;
                        break;
                }

                var res = bv6proxy.MigratePriceGroup(settings.ApiKey, pg);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        wl(res.Content == string.Empty ? "FAILED" : "SUCCESS");
                    }
                }
            }

        }
        private void ImportRoles()
        {
            Header("Importing Roles");
        }

        private void ClearCategories()
        {
            Header("Clearing All Categories");

            var res = bv6proxy.ClearAllCategoriesFirst(settings.ApiKey);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    wl("SUCCESS");
                }
            }
        }

        private void ClearProducts()
        {
            Header("Clearing All Products");

            int remaining = int.MaxValue;

            while (remaining > 0)
            {
                int pageSize = 100;

                var res = bv6proxy.ClearProducts(settings.ApiKey, pageSize);
                if (res == null)
                {
                    wl("FAILED TO CLEAR PRODUCTS!");
                }
                else
                {
                    if (res.Errors.Count > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                        return;
                    }
                    else
                    {
                        remaining = res.Content.ProductsRemaining;
                        wl("Clearing products: " + res.Content.ProductsRemaining + " remaining at " + DateTime.UtcNow.ToString());
                    }
                }
            }
            wl("Finished Clearing Products at : " + DateTime.UtcNow.ToString());
        }
    }
}
