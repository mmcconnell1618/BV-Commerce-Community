using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BVSoftware.Commerce.Migration.Migrators.BV5
{
    public class Migrator : IMigrator
    {

        private const int CHUNKSIZE = 131072;
        private MigrationSettings settings = null;
        private MigrationServices.MigrationToolServiceClient bv6proxy = null;
        private data.BV53Entities oldDatabase = null;
        private Dictionary<string, long> AffiliateMapper = new Dictionary<string, long>();
        private Dictionary<string, long> TaxScheduleMapper = new Dictionary<string, long>();
        private Dictionary<string, long> ProductPropertyMapper = new Dictionary<string, long>();

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
            string result = "metadata=res://*/Migrators.BV5.data.OldDatabase.csdl|" +
            "res://*/Migrators.BV5.data.OldDatabase.ssdl|res://*/Migrators.BV5.data.OldDatabase.msl;" +
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
            wl("BV Commerce 5 Migrator Started");
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
                oldDatabase = new data.BV53Entities(EFConnString(s.SourceConnectionString()));
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
                    ImportProductInputs();
                    ImportProductModifiers();
                    ImportProductChoices();

                    ImportProducts();
                    MigrateProductFileDownloads();
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
                var o = (from old in oldDatabase.bvc_Order where old.OrderNumber == settings.SingleOrderImport select old).FirstOrDefault();
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
                int totalRecords = oldDatabase.bvc_Order.Where(y => y.IsPlaced == 1).Count();
                int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
                for (int i = 0; i < totalPages; i++)
                {
                    wl("Getting Orders page " + (i + 1) + " of " + totalPages.ToString());
                    int startRecord = i * pageSize;
                    var orders = (from order in oldDatabase.bvc_Order where order.IsPlaced == 1 select order).OrderBy(y => y.id).Skip(startRecord).Take(pageSize).ToList();
                    if (orders == null) continue;
                    System.Threading.Tasks.Parallel.ForEach(orders, ImportSingleOrder);
                }
            }
        }
        private void ImportSingleOrder(data.bvc_Order old)
        {
            if (old == null) return;
            wl("Processing Order: " + old.OrderNumber);

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
                        ImportOrderTransactions(o.bvin, o.OrderNumber);
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
            o.AffiliateID = old.AffiliateId ?? string.Empty;
            BV5Address oldBilling = new BV5Address();
            oldBilling.FromXmlString(old.BillingAddress);
            if (oldBilling != null)
            {
                oldBilling.CopyTo(o.BillingAddress, EFConnString(settings.SourceConnectionString()));
            }
            o.bvin = old.Bvin ?? string.Empty;
            o.CustomProperties = TranslateOldProperties(old.CustomProperties);
            o.FraudScore = old.FraudScore;
            o.Id = old.id;
            o.Instructions = old.Instructions;
            o.IsPlaced = old.IsPlaced == 1;
            o.LastUpdatedUtc = old.LastUpdated;
            o.OrderNumber = old.OrderNumber ?? string.Empty;
            if (old.OrderDiscounts != 0)
            {
                o.OrderDiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * old.OrderDiscounts, Description = "BV 5 Order Discounts", Id = new Guid() });
            }
            o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unknown;
            switch (old.PaymentStatus)
            {
                case 0:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unknown;
                    break;
                case 1:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Unpaid;
                    break;
                case 2:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.PartiallyPaid;
                    break;
                case 3:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Paid;
                    break;
                case 4:
                    o.PaymentStatus = MigrationServices.OrderPaymentStatusDTO.Overpaid;
                    break;
            }
            BV5Address oldShipping = new BV5Address();
            oldShipping.FromXmlString(old.ShippingAddress);
            if (oldShipping != null) oldShipping.CopyTo(o.ShippingAddress, EFConnString(settings.SourceConnectionString()));
            o.ShippingMethodDisplayName = old.ShippingMethodDisplayName;
            o.ShippingMethodId = old.ShippingMethodId;
            o.ShippingProviderId = old.ShippingProviderId;
            o.ShippingProviderServiceCode = old.ShippingProviderServiceCode;
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
            o.StatusCode = old.StatusCode ?? string.Empty;
            o.StatusName = old.StatusName;
            o.ThirdPartyOrderId = old.ThirdPartyOrderId;
            o.TimeOfOrderUtc = old.TimeOfOrder;
            o.TotalHandling = old.HandlingTotal;
            o.TotalShippingBeforeDiscounts = old.ShippingTotal + old.ShippingDiscounts;
            o.ShippingDiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * old.ShippingDiscounts, Description = "BV5 Shipping Discount", Id = new Guid() });
            o.TotalTax = old.TaxTotal;
            o.TotalTax2 = old.TaxTotal2;
            o.UserEmail = old.UserEmail;
            o.UserID = old.UserId;

            wl(" - Coupons for Order " + old.OrderNumber);
            o.Coupons = TranslateCoupons(o.bvin);

            wl(" - Items For order " + old.OrderNumber);
            o.Items = TranslateItems(o.bvin);
            LineItemHelper.SplitTaxesAcrossItems(old.TaxTotal2 + old.TaxTotal, old.SubTotal, o.Items);

            wl(" - Notes For order " + old.OrderNumber);
            o.Notes = TranslateNotes(o.bvin);

            wl(" - Packages For order " + old.OrderNumber);
            o.Packages = TranslatePackages(o.bvin);

        }
        private List<MigrationServices.OrderCouponDTO> TranslateCoupons(string orderBvin)
        {
            List<MigrationServices.OrderCouponDTO> result = new List<MigrationServices.OrderCouponDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderCoupon.Where(y => y.OrderBvin == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderCoupon oldCoupon in old)
            {
                MigrationServices.OrderCouponDTO c = new MigrationServices.OrderCouponDTO();
                c.CouponCode = oldCoupon.CouponCode ?? string.Empty;
                c.IsUsed = true;
                c.OrderBvin = orderBvin ?? string.Empty;
                result.Add(c);
            }

            return result;
        }
        private List<MigrationServices.OrderNoteDTO> TranslateNotes(string orderBvin)
        {
            List<MigrationServices.OrderNoteDTO> result = new List<MigrationServices.OrderNoteDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderNote.Where(y => y.OrderId == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderNote item in old)
            {
                MigrationServices.OrderNoteDTO n = new MigrationServices.OrderNoteDTO();
                n.AuditDate = item.AuditDate;
                n.IsPublic = item.NoteType == 3;
                n.LastUpdatedUtc = item.LastUpdated;
                n.Note = item.Note ?? string.Empty;
                n.OrderID = orderBvin;
                result.Add(n);
            }

            return result;
        }
        private List<MigrationServices.OrderPackageDTO> TranslatePackages(string orderBvin)
        {
            List<MigrationServices.OrderPackageDTO> result = new List<MigrationServices.OrderPackageDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_OrderPackage.Where(y => y.OrderId == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_OrderPackage item in old)
            {
                MigrationServices.OrderPackageDTO pak = new MigrationServices.OrderPackageDTO();

                pak.CustomProperties = TranslateOldProperties(item.CustomProperties);
                pak.Description = item.Description ?? string.Empty;
                pak.EstimatedShippingCost = item.EstimatedShippingCost;
                pak.HasShipped = item.HasShipped == 1;
                pak.Height = item.Height;
                pak.Items = TranslateOldPackageItems(item.Items);
                pak.LastUpdatedUtc = item.LastUpdated;
                pak.Length = item.Length;
                pak.OrderId = orderBvin;
                pak.ShipDateUtc = item.ShipDate;
                pak.ShippingMethodId = string.Empty;
                pak.ShippingProviderId = item.ShippingProviderId;
                pak.ShippingProviderServiceCode = item.ShippingProviderServiceCode;
                pak.SizeUnits = MigrationServices.LengthTypeDTO.Inches;
                if (item.SizeUnits == 2)
                {
                    pak.SizeUnits = MigrationServices.LengthTypeDTO.Centimeters;
                }
                pak.TrackingNumber = item.TrackingNumber;
                pak.Weight = item.Weight;
                pak.WeightUnits = MigrationServices.WeightTypeDTO.Pounds;
                if (item.WeightUnits == 2)
                {
                    pak.WeightUnits = MigrationServices.WeightTypeDTO.Kilograms;
                }
                pak.Width = item.Width;
                result.Add(pak);
            }

            return result;
        }
        private List<MigrationServices.OrderPackageItemDTO> TranslateOldPackageItems(string xml)
        {
            List<MigrationServices.OrderPackageItemDTO> result = new List<MigrationServices.OrderPackageItemDTO>();

            System.Collections.ObjectModel.Collection<PackageItem> old = PackageItem.FromXml(xml);
            if (old != null)
            {
                foreach (PackageItem p in old)
                {
                    MigrationServices.OrderPackageItemDTO pi = new MigrationServices.OrderPackageItemDTO();
                    pi.LineItemId = p.LineItemId;
                    pi.ProductBvin = p.ProductBvin;
                    pi.Quantity = p.Quantity;
                    result.Add(pi);
                }
            }
            return result;
        }
        private List<MigrationServices.LineItemDTO> TranslateItems(string orderBvin)
        {
            List<MigrationServices.LineItemDTO> result = new List<MigrationServices.LineItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var old = db.bvc_LineItem.Where(y => y.OrderBvin == orderBvin);
            if (old == null) return result;

            foreach (data.bvc_LineItem item in old)
            {
                MigrationServices.LineItemDTO li = new MigrationServices.LineItemDTO();

                li.BasePricePerItem = item.BasePrice;
                li.CustomProperties = TranslateOldProperties(item.CustomProperties);
                li.DiscountDetails = new List<MigrationServices.DiscountDetailDTO>();
                li.ExtraShipCharge = 0;
                li.Id = -1;
                li.LastUpdatedUtc = item.LastUpdated;
                li.OrderBvin = orderBvin;
                li.ProductId = item.ProductId;
                li.ProductName = item.ProductName;
                li.ProductShortDescription = item.ProductShortDescription;
                li.ProductSku = item.ProductSku;
                li.ProductShippingHeight = 0;
                li.ProductShippingLength = 0;
                li.ProductShippingWeight = 0;
                li.ProductShippingWidth = 0;
                li.Quantity = (int)item.Quantity;
                li.QuantityReturned = (int)item.QuantityReturned;
                li.QuantityShipped = (int)item.QuantityShipped;
                li.SelectionData = new List<MigrationServices.OptionSelectionDTO>();
                li.ShipFromAddress = new MigrationServices.AddressDTO();
                li.ShipFromMode = MigrationServices.ShippingModeDTO.ShipFromSite;
                li.ShipFromNotificationId = string.Empty;
                li.ShippingPortion = 0;
                li.ShippingSchedule = 0;
                li.ShipSeparately = false;
                li.StatusCode = item.StatusCode;
                li.StatusName = item.StatusName;
                li.TaxPortion = 0m;
                li.TaxSchedule = 0;
                li.VariantId = string.Empty;

                // Calculate Adjustments and Discounts
                decimal lineTotal = item.LineTotal;
                decimal prediscountTotal = (li.BasePricePerItem * (decimal)li.Quantity);
                decimal allDiscounts = prediscountTotal - lineTotal;
                if (allDiscounts != 0)
                {
                    li.DiscountDetails.Add(new MigrationServices.DiscountDetailDTO() { Amount = -1 * allDiscounts, Description = "BV5 Discounts", Id = new Guid() });
                }

                result.Add(li);
            }

            return result;
        }
        private void ImportOrderTransactions(string orderBvin, string orderNumber)
        {
            wl(" - Transactions for Order " + orderNumber);

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var old = db.bvc_OrderPayment.Where(y => y.orderID == orderBvin);
            if (old == null) return;

            foreach (data.bvc_OrderPayment item in old)
            {
                wl("Transaction: " + item.bvin);

                bool hasAuth = item.AmountAuthorized != 0;
                bool hasCharge = item.AmountCharged != 0;
                bool hasRefund = item.AmountRefunded != 0;

                Guid AuthTransactionID = new Guid();
                Guid ChargeTransactionId = new Guid();
                Guid RefundTransactionId = new Guid();

                // Get Guids for transactions
                Guid.TryParse(item.bvin,out ChargeTransactionId);
                if (hasAuth && (hasCharge == false && hasRefund == false))
                {
                    // Auth only, no refund or charge
                    Guid.TryParse(item.bvin, out AuthTransactionID);
                }
                if (hasRefund && (hasCharge == false && hasAuth == false))
                {
                    // Refund only, no auth or charge
                    Guid.TryParse(item.bvin, out RefundTransactionId);
                }

                if (hasAuth)
                {
                    MigrationServices.OrderTransactionDTO opAuth = new MigrationServices.OrderTransactionDTO();
                    opAuth.Id = AuthTransactionID;
                    switch(item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.PurchaseOrderInfo;
                            break;                        
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.GiftCardHold;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.PayPalHold;
                            break;
                        default:
                            opAuth.Action = MigrationServices.OrderTransactionActionDTO.CreditCardHold;
                            break;
                    }                    
                    opAuth.Amount = item.AmountAuthorized;
                    opAuth.CheckNumber = item.checkNumber ?? string.Empty;
                    opAuth.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opAuth.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opAuth.CreditCard.CardIsEncrypted = true;
                    opAuth.CreditCard.CardNumber = string.Empty;
                    opAuth.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opAuth.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opAuth.CreditCard.SecurityCode = string.Empty;
                    opAuth.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opAuth.LinkedToTransaction = string.Empty;
                    opAuth.Messages = item.note ?? string.Empty;
                    opAuth.OrderId = orderBvin ?? string.Empty;
                    opAuth.OrderNumber = orderNumber ?? string.Empty;
                    opAuth.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opAuth.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opAuth.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opAuth.Success = true;
                    opAuth.TimeStampUtc = item.auditDate;
                    opAuth.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opAuth);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }

                if (hasCharge)
                {
                    MigrationServices.OrderTransactionDTO opCharge = new MigrationServices.OrderTransactionDTO();
                    opCharge.Id = ChargeTransactionId;
                    switch (item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CreditCardCharge;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CheckReceived;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CashReceived;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CashReceived;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.PurchaseOrderAccepted;
                            break;
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.GiftCardCapture;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CreditCardCharge;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.PayPalCharge;
                            break;
                        default:
                            opCharge.Action = MigrationServices.OrderTransactionActionDTO.CreditCardCharge;
                            break;
                    }                                        
                    opCharge.Amount = item.AmountCharged;
                    opCharge.CheckNumber = item.checkNumber ?? string.Empty;
                    opCharge.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opCharge.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opCharge.CreditCard.CardIsEncrypted = true;
                    opCharge.CreditCard.CardNumber = string.Empty;
                    opCharge.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opCharge.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opCharge.CreditCard.SecurityCode = string.Empty;
                    opCharge.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opCharge.LinkedToTransaction = string.Empty;
                    opCharge.Messages = item.note ?? string.Empty;
                    opCharge.OrderId = orderBvin ?? string.Empty;
                    opCharge.OrderNumber = orderNumber ?? string.Empty;
                    opCharge.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opCharge.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opCharge.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opCharge.Success = true;
                    opCharge.TimeStampUtc = item.auditDate;
                    opCharge.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opCharge);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }

                if (hasRefund)
                {
                    MigrationServices.OrderTransactionDTO opRefund = new MigrationServices.OrderTransactionDTO();
                    opRefund.Id = RefundTransactionId;
                    switch (item.paymentMethodId)
                    {

                        case "4A807645-4B9D-43f1-BC07-9F233B4E713C": // Credit Card
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CreditCardRefund;
                            break;
                        case "9FD35C50-CDCB-42ac-9549-14119BECBD0C": // Telephone
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.OfflinePaymentRequest;
                            break;
                        case "494A61C8-D7E7-457f-B293-4838EF010C32": // Check
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CheckReturned;
                            break;
                        case "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E": // Cash
                        case "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C": // COD                            
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CashReturned;
                            break;
                        case "A0300DBD-39EE-472C-9179-D4B96F27913B": // CredEx
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CashReturned;
                            break;
                        case "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7": // PO
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CashReturned;
                            break;
                        case "91a205f1-8c1c-4267-bed0-c8e410e7e680": // Gift Card
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.GiftCardIncrease;
                            break;
                        case "49de5510-dfe4-4b18-91a6-3dc9925566a1": // Google Checkout
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CreditCardRefund;
                            break;
                        case "33eeba60-e5b7-4864-9b57-3f8d614f8301": // PayPal Express
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.PayPalRefund;
                            break;
                        default:
                            opRefund.Action = MigrationServices.OrderTransactionActionDTO.CreditCardRefund;
                            break;
                    }           
                    opRefund.Amount = -1 * item.AmountCharged;
                    opRefund.CheckNumber = item.checkNumber ?? string.Empty;
                    opRefund.CreditCard = new MigrationServices.OrderTransactionCardDataDTO();
                    opRefund.CreditCard.CardHolderName = item.creditCardHolder ?? string.Empty;
                    opRefund.CreditCard.CardIsEncrypted = true;
                    opRefund.CreditCard.CardNumber = string.Empty;
                    opRefund.CreditCard.ExpirationMonth = item.creditCardExpMonth;
                    opRefund.CreditCard.ExpirationYear = item.creditCardExpYear;
                    opRefund.CreditCard.SecurityCode = string.Empty;
                    opRefund.GiftCardNumber = item.giftCertificateNumber ?? string.Empty;
                    opRefund.LinkedToTransaction = string.Empty;
                    opRefund.Messages = item.note ?? string.Empty;
                    opRefund.OrderId = orderBvin ?? string.Empty;
                    opRefund.OrderNumber = orderNumber ?? string.Empty;
                    opRefund.PurchaseOrderNumber = item.purchaseOrderNumber ?? string.Empty;
                    opRefund.RefNum1 = item.transactionReferenceNumber ?? string.Empty;
                    opRefund.RefNum2 = item.transactionResponseCode ?? string.Empty;
                    opRefund.Success = true;
                    opRefund.TimeStampUtc = item.auditDate;
                    opRefund.Voided = false;

                    var res = proxy.MigrateOrderTransaction(settings.ApiKey, opRefund);
                    if (res != null)
                    {
                        if (res.Errors.Count > 0)
                        {
                            DumpErrors(res.Errors);
                            wl("FAILED TRANSACTION: " + item.bvin);
                        }
                    }
                    else
                    {
                        wl("FAILED! EXCEPTION! TRANSACTION: " + item.bvin);
                    }
                }
                                                                
                                                
                
            }
        }

        private void ImportRelatedItems()
        {
            Header("Importing Related Items");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var crosses = db.bvc_ProductCrossSell;
            if (crosses == null) return;
            foreach (data.bvc_ProductCrossSell x in crosses)
            {
                wl("Relating " + x.ProductBvin + " to " + x.CrossSellBvin);
                proxy.AssociateProducts(settings.ApiKey, x.ProductBvin, x.CrossSellBvin, false);                
            }

            var ups = db.bvc_ProductUpSell;
            if (ups == null) return;
            foreach (data.bvc_ProductUpSell up in ups)
            {
                wl("Relating Up " + up.ProductBvin + " to " + up.UpSellBvin);
                proxy.AssociateProducts(settings.ApiKey, up.ProductBvin, up.UpSellBvin, true);
            }            
        }

        private void ImportProducts()
        {
            Header("Importing Products");

            int limit = -1;
            if (settings.ImportProductLimit > 0)
            {
                limit = settings.ImportProductLimit;
            }
            int totalMigrated = 0;


            int pageSize = 100;
            int totalRecords = oldDatabase.bvc_Product.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

            for (int i = 0; i < totalPages; i++)
            {
                wl("Getting Products page " + (i + 1) + " of " + totalPages.ToString());
                int startRecord = i * pageSize;
                var products = (from p in oldDatabase.bvc_Product select p).OrderBy(y => y.id).Skip(startRecord).Take(pageSize).ToList();

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

            CustomPropertyCollection props = CustomPropertyCollection.FromXml(oldXml);
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

            wl("Product: " + old.ProductName + " [" + old.SKU + "]");
            MigrationServices.ProductDTO p = new MigrationServices.ProductDTO();
            p.AllowReviews = true;
            p.Bvin = old.bvin;
            p.CreationDateUtc = old.CreationDate;
            p.CustomProperties = TranslateOldProperties(old.CustomProperties);
            p.Featured = false;
            p.GiftWrapAllowed = true;
            p.GiftWrapPrice = 0;
            p.ImageFileSmall = System.IO.Path.GetFileName(old.ImageFileMedium);
            p.ImageFileSmallAlternateText = old.MediumImageAlternateText;
            p.InventoryMode = MigrationServices.ProductInventoryModeDTO.AlwayInStock;
            if (old.TrackInventory == 1) p.InventoryMode = MigrationServices.ProductInventoryModeDTO.WhenOutOfStockShow;
            p.IsAvailableForSale = true;
            p.Keywords = old.Keywords;
            p.ListPrice = old.ListPrice;
            p.LongDescription = old.LongDescription;
            p.ManufacturerId = old.ManufacturerID;
            p.MetaDescription = old.MetaDescription;
            p.MetaKeywords = old.MetaKeywords;
            p.MetaTitle = old.MetaTitle;
            p.MinimumQty = old.MinimumQty;
            p.PostContentColumnId = string.Empty;
            p.PreContentColumnId = string.Empty;
            p.PreTransformLongDescription = old.PreTransformLongDescription;
            p.ProductName = old.ProductName;
            p.ProductTypeId = old.ProductTypeId;
            p.ShippingDetails = new MigrationServices.ShippableItemDTO();
            p.ShippingDetails.ExtraShipFee = old.ExtraShipFee;
            p.ShippingDetails.Height = old.ShippingHeight;
            p.ShippingDetails.IsNonShipping = old.NonShipping == 1;
            p.ShippingDetails.Length = old.ShippingLength;
            p.ShippingDetails.ShippingScheduleId = 0;
            p.ShippingDetails.ShipSeparately = old.ShipSeparately == 1;
            p.ShippingDetails.Weight = old.ShippingWeight;
            p.ShippingDetails.Width = old.ShippingWeight;
            switch (old.ShippingMode)
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
            p.SiteCost = old.SiteCost;
            p.SitePrice = old.SitePrice;
            p.SitePriceOverrideText = old.SitePriceOverrideText;
            p.Sku = old.SKU;
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
            p.VendorId = old.VendorID;

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
                    AssignOptionsToProduct(old.bvin);
                    AssignProductPropertyValues(old.bvin);
                    wl("SUCCESS");
                }
            }

            // Inventory                        
            MigrateProductInventory(old.bvin);

            // Additional Images
            MigrateProductAdditionalImages(old.bvin);

            // Volume Prices
            MigrateProductVolumePrices(old.bvin);

            // Reviews
            MigrateProductReviews(old.bvin);

            // Link to Categories
            MigrateProductCategoryLinks(old.bvin);

        }
        private void AssignOptionsToProduct(string bvin)
        {
            wl(" - Migrating Options...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));


            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var choices = db.bvc_ProductXChoice.Where(y => y.ProductId == bvin);
            if (choices == null) return;
            foreach (data.bvc_ProductXChoice choice in choices)
            {
                proxy.AssignOptionToProduct(settings.ApiKey, bvin, choice.ChoiceId);
            }

            var modifiers = db.bvc_ProductXModifier.Where(y => y.ProductId == bvin);
            if (modifiers == null) return;
            foreach (data.bvc_ProductXModifier mod in modifiers)
            {
                proxy.AssignOptionToProduct(settings.ApiKey, bvin, mod.ModifierId);
            }

            var inputs = db.bvc_ProductXInput.Where(y => y.ProductId == bvin);
            if (inputs == null) return;
            foreach (data.bvc_ProductXInput input in inputs)
            {
                proxy.AssignOptionToProduct(settings.ApiKey, bvin, input.InputId);
            }

        }
        private void AssignProductPropertyValues(string bvin)
        {
            wl(" - Migrating Property Values...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductPropertyValue.Where(y => y.ProductBvin == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductPropertyValue item in items)
            {
                long newId = 0;
                if (ProductPropertyMapper.ContainsKey(item.PropertyBvin)) newId = ProductPropertyMapper[item.PropertyBvin];
                if (newId > 0)
                {
                    proxy.SetProductPropertyValue(settings.ApiKey, bvin, newId, item.PropertyValue, -1);
                }
            }
        }
        private void MigrateProductInventory(string bvin)
        {
            wl(" - Migrating Inventory...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var old = db.bvc_ProductInventory.Where(y => y.ProductBvin == bvin).FirstOrDefault();
            if (old == null) return;

            MigrationServices.ProductInventoryDTO inv = new MigrationServices.ProductInventoryDTO();
            inv.LowStockPoint = (int)(old.ReorderLevel ?? 0);
            inv.ProductBvin = bvin;
            inv.QuantityOnHand = (int)((old.QuantityAvailableForSale ?? 0) + old.QuantityReserved);
            inv.QuantityReserved = (int)old.QuantityReserved;


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

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductImage.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductImage old in items)
            {
                MigrationServices.ProductImageDTO img = new MigrationServices.ProductImageDTO();
                img.AlternateText = old.AlternateText;
                img.Bvin = old.bvin;
                img.Caption = old.Caption;
                img.FileName = System.IO.Path.GetFileName(old.FileName);
                img.LastUpdatedUtc = old.LastUpdated;
                img.ProductId = old.ProductID;
                img.SortOrder = old.SortOrder;

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
        private void MigrateProductFileDownloads()
        {
            Header("Migrating File Downloads");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductFile;
            if (items == null) return;
            foreach (data.bvc_ProductFile old in items)
            {
                wl("File: " + old.FileName);

                string safeFileName = "\\files\\" + old.bvin + "_" + old.FileName + ".config";

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

                var res = proxy.MigrateProductFileFirstPart(settings.ApiKey, old.bvin, old.FileName, old.ShortDescription, partial);
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
                            var res2 = proxy.MigrateProductFileAdditionalPart(settings.ApiKey, old.bvin, old.FileName, partial);
                        }
                    }
                    wl("File Done Uploading!");
                }
                else
                {
                    wl("FAILED! EXCEPTION!");
                }
            }

            var crosses = db.bvc_ProductFileXProduct;
            if (crosses == null) return;
            foreach (data.bvc_ProductFileXProduct x in crosses)
            {
                wl("Linking Product " + x.ProductId + " to " + x.ProductFileId);
                var res2 = proxy.AssociateProductFile(settings.ApiKey, x.ProductId, x.ProductFileId, x.AvailableMinutes, x.MaxDownloads);
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

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductVolumeDiscounts.Where(y => y.ProductID == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductVolumeDiscounts item in items)
            {
                MigrationServices.ProductVolumeDiscountDTO v = new MigrationServices.ProductVolumeDiscountDTO();
                v.Amount = item.Amount;
                v.Bvin = item.bvin;
                switch (item.DiscountType)
                {
                    case 1:
                        v.DiscountType = MigrationServices.ProductVolumeDiscountTypeDTO.Percentage;
                        break;
                    case 2:
                        v.DiscountType = MigrationServices.ProductVolumeDiscountTypeDTO.Amount;
                        break;
                }
                v.DiscountType = (MigrationServices.ProductVolumeDiscountTypeDTO)item.DiscountType;
                v.LastUpdated = item.LastUpdated;
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

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductReview.Where(y => y.ProductBvin == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductReview item in items)
            {
                MigrationServices.ProductReviewDTO r = new MigrationServices.ProductReviewDTO();
                r.Approved = item.Approved == 1;
                r.Bvin = item.bvin;
                r.Description = item.Description;
                r.Karma = item.Karma;
                r.ProductBvin = item.ProductBvin;
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
                r.UserID = item.UserID;

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

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            MigrationServices.MigrationToolServiceClient proxy = GetBV6Proxy();

            var items = db.bvc_ProductXCategory.Where(y => y.ProductId == bvin);
            if (items == null) return;
            foreach (data.bvc_ProductXCategory item in items)
            {
                wl("To Category: " + item.CategoryId);
                var res = proxy.MigrationProductXCategory(settings.ApiKey, bvin, item.CategoryId);
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
        private void ImportProductInputs()
        {
            Header("Importing Product Inputs");

            foreach (data.bvc_ProductInputs oldInput in oldDatabase.bvc_ProductInputs)
            {


                MigrationServices.OptionDTO input = new MigrationServices.OptionDTO();
                string fullName = oldInput.InputName;
                if (oldInput.InputDisplayName.Trim().Length > 0) fullName = oldInput.InputDisplayName;

                input.Settings = new List<MigrationServices.OptionSettingDTO>();
                input.Items = new List<MigrationServices.OptionItemDTO>();
                input.Bvin = oldInput.bvin;
                input.IsShared = oldInput.SharedInput;
                input.IsVariant = false; // Inputs can't be variants

                input.NameIsHidden = false;
                switch (oldInput.InputType.Trim().ToLowerInvariant())
                {
                    case "html area":
                        input.OptionType = MigrationServices.OptionTypesDTO.Html;
                        BV5OptionHtmlSettings htmlSettings = GetOptionSettingsHtml(input.Bvin);
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "html", Value = htmlSettings.HtmlData });
                        break;
                    default: // Text Input
                        input.OptionType = MigrationServices.OptionTypesDTO.TextInput;
                        BV5OptionTextSettings textSettings = GetOptionSettingsText(input.Bvin);
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "rows", Value = textSettings.Rows });
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "cols", Value = textSettings.Columns });
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "required", Value = textSettings.Required });
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "wraptext", Value = textSettings.WrapText });
                        input.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "maxlength", Value = "255" });
                        if (textSettings.DisplayName.Trim().Length > 0) fullName = textSettings.DisplayName;
                        break;
                }
                input.Name = fullName;
                wl("Input: " + fullName);

                var res = bv6proxy.MigrateOption(settings.ApiKey, input);
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
        private BV5OptionTextSettings GetOptionSettingsText(string bvin)
        {
            BV5OptionTextSettings result = new BV5OptionTextSettings();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var componentSettings = db.bvc_ComponentSetting.Where(y => y.ComponentID == bvin).OrderBy(y => y.SettingName);
            if (componentSettings == null) return result;

            foreach (data.bvc_ComponentSetting cs in componentSettings)
            {
                switch (cs.SettingName.Trim().ToLowerInvariant())
                {
                    case "columns":
                        result.Columns = cs.SettingValue;
                        break;
                    case "rows":
                        result.Rows = cs.SettingValue;
                        break;
                    case "displayname":
                        result.DisplayName = cs.SettingValue;
                        break;
                    case "required":
                        result.Required = cs.SettingValue;
                        break;
                    case "wraptext":
                        result.WrapText = cs.SettingValue;
                        break;
                }
            }
            return result;
        }
        private BV5OptionHtmlSettings GetOptionSettingsHtml(string bvin)
        {
            BV5OptionHtmlSettings result = new BV5OptionHtmlSettings();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var componentSettings = db.bvc_ComponentSetting.Where(y => y.ComponentID == bvin).OrderBy(y => y.SettingName);
            if (componentSettings == null) return result;

            foreach (data.bvc_ComponentSetting cs in componentSettings)
            {
                switch (cs.SettingName.Trim().ToLowerInvariant())
                {
                    case "htmldata":
                        result.HtmlData = cs.SettingValue;
                        break;
                }
            }
            return result;
        }
        private void ImportProductModifiers()
        {
            Header("Importing Modifiers");

            foreach (data.bvc_ProductModifier old in oldDatabase.bvc_ProductModifier)
            {
                MigrationServices.OptionDTO o = new MigrationServices.OptionDTO();
                string fullName = old.Name;
                if (old.Displayname.Trim().Length > 0) fullName = old.Displayname;

                o.Settings = new List<MigrationServices.OptionSettingDTO>();
                o.Items = new List<MigrationServices.OptionItemDTO>();
                o.Bvin = old.bvin;
                o.IsShared = old.Shared;
                o.IsVariant = false; // Modifiers can't be variants
                o.NameIsHidden = false;
                if (old.Required) o.Settings.Add(new MigrationServices.OptionSettingDTO() { Key = "required", Value = "1" });

                switch (old.Type.Trim().ToLowerInvariant())
                {
                    case "radio button list":
                    case "image radio button list":
                        o.OptionType = MigrationServices.OptionTypesDTO.RadioButtonList;
                        break;
                    default: // Drop Down List
                        o.OptionType = MigrationServices.OptionTypesDTO.DropDownList;
                        break;
                }
                o.Name = fullName;
                wl("Modifier: " + fullName);

                // Load Items for Option Here
                o.Items = LoadOptionItemsModifier(o.Bvin);

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
        private void ImportProductChoices()
        {
            Header("Importing Shared Choices");

            foreach (data.bvc_ProductChoices old in oldDatabase.bvc_ProductChoices)
            {
                MigrationServices.OptionDTO o = new MigrationServices.OptionDTO();
                string fullName = old.ChoiceName;
                if (old.ChoiceDisplayName.Trim().Length > 0) fullName = old.ChoiceDisplayName;

                o.Settings = new List<MigrationServices.OptionSettingDTO>();
                o.Items = new List<MigrationServices.OptionItemDTO>();
                o.Bvin = old.bvin;
                o.IsShared = old.SharedChoice;
                o.IsVariant = true; // Choices are always variants in BV6
                o.NameIsHidden = false;

                switch (old.ChoiceType.Trim().ToLowerInvariant())
                {
                    case "radio button list":
                    case "image radio button list":
                        o.OptionType = MigrationServices.OptionTypesDTO.RadioButtonList;
                        break;
                    default: // Drop Down List
                        o.OptionType = MigrationServices.OptionTypesDTO.DropDownList;
                        break;
                }
                o.Name = fullName;
                wl("Choice: " + fullName);

                // Load Items for Option Here
                o.Items = LoadOptionItemsChoice(o.Bvin);

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
        private List<MigrationServices.OptionItemDTO> LoadOptionItemsModifier(string bvin)
        {
            List<MigrationServices.OptionItemDTO> result = new List<MigrationServices.OptionItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var items = db.bvc_ProductModifierOption.Where(y => y.ModifierId == bvin)
                            .OrderBy(y => y.Order);
            if (items == null) return result;

            foreach (data.bvc_ProductModifierOption item in items)
            {
                MigrationServices.OptionItemDTO dto = new MigrationServices.OptionItemDTO();
                dto.Bvin = item.bvin;
                dto.IsLabel = item.Null;
                dto.Name = item.Name;
                dto.OptionBvin = bvin;
                dto.PriceAdjustment = item.PriceAdjustment;
                dto.SortOrder = item.Order;
                dto.WeightAdjustment = item.WeightAdjustment;
                result.Add(dto);
            }

            return result;
        }
        private List<MigrationServices.OptionItemDTO> LoadOptionItemsChoice(string bvin)
        {
            List<MigrationServices.OptionItemDTO> result = new List<MigrationServices.OptionItemDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var items = db.bvc_ProductChoiceOptions.Where(y => y.ProductChoiceId == bvin)
                            .OrderBy(y => y.Order);
            if (items == null) return result;

            foreach (data.bvc_ProductChoiceOptions item in items)
            {
                MigrationServices.OptionItemDTO dto = new MigrationServices.OptionItemDTO();
                dto.Bvin = item.bvin;
                dto.IsLabel = item.Null;
                dto.Name = item.ProductChoiceName;
                dto.OptionBvin = bvin;
                dto.PriceAdjustment = 0;
                dto.SortOrder = item.Order;
                dto.WeightAdjustment = 0;
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
                cat.Bvin = old.bvin;
                cat.Criteria = old.Criteria;
                cat.CustomerChangeableSortOrder = old.CustomerChangeableSortOrder;
                cat.CustomPageId = old.CustomPageId;
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
                cat.Keywords = old.Keywords;
                cat.LastUpdatedUtc = old.LastUpdated;
                cat.LatestProductCount = old.LatestProductCount;
                cat.MetaDescription = old.MetaDescription;
                cat.MetaKeywords = old.MetaKeywords;
                cat.MetaTitle = old.MetaTitle;
                cat.Name = old.Name;
                cat.Operations = new List<MigrationServices.ApiOperation>();
                cat.ParentId = old.ParentID;
                cat.PostContentColumnId = old.PostContentColumnId;
                if (cat.PostContentColumnId.Trim() == "-None -") cat.PostContentColumnId = string.Empty;
                cat.PreContentColumnId = old.PreContentColumnId;
                if (cat.PreContentColumnId.Trim() == "-None -") cat.PreContentColumnId = string.Empty;
                cat.PreTransformDescription = old.PreTransformDescription;
                cat.RewriteUrl = old.RewriteUrl;
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
                cat.TemplateName = old.TemplateName;

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
                        MigrateCategoryBanner(old.bvin, bannerImageSource);
                        MigrateCategoryImage(old.bvin, imageSource);
                        wl("SUCCESS");
                    }
                }
            }

        }
        private void MigrateCategoryImage(string catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            bv6proxy.ImagesUploadCategoryImage(settings.ApiKey, catBvin, fileName, bytes);
        }
        private void MigrateCategoryBanner(string catBvin, string imageSource)
        {
            byte[] bytes = GetBytesForLocalImage(imageSource);
            if (bytes == null) return;
            string fileName = Path.GetFileName(imageSource);
            wl("Found Image: " + fileName + " [" + FriendlyFormatBytes(bytes.Length) + "]");
            bv6proxy.ImagesUploadCategoryBanner(settings.ApiKey, catBvin, fileName, bytes);
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

                pt.Bvin = old.bvin;
                pt.IsPermanent = old.IsPermanent;
                pt.LastUpdated = old.LastUpdated;
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
                        MigratePropertiesForType(pt.Bvin);
                        wl("SUCCESS");
                    }
                }
            }
        }
        private void MigratePropertiesForType(string typeBvin)
        {
            wl("Migrating Properties to Type...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var crosses = db.bvc_ProductTypeXProductProperty.Where(y => y.ProductTypeBvin == typeBvin);
            if (crosses == null) return;

            foreach (data.bvc_ProductTypeXProductProperty cross in crosses)
            {
                int sort = cross.SortOrder;
                string oldPropertyBvin = cross.ProductPropertyBvin;
                long newId = 0;
                if (ProductPropertyMapper.ContainsKey(oldPropertyBvin))
                {
                    newId = ProductPropertyMapper[oldPropertyBvin];
                }
                if (newId <= 0) continue;
                wl("Mapping " + oldPropertyBvin + " to " + newId.ToString());
                bv6proxy.AssignProductPropertyToType(settings.ApiKey, typeBvin, newId, sort);
            }
        }
        private void ImportProductProperties()
        {
            Header("Importing Product Properties");

            ProductPropertyMapper = new Dictionary<string, long>();

            foreach (data.bvc_ProductProperty old in oldDatabase.bvc_ProductProperty)
            {
                wl("Item: " + old.DisplayName);

                MigrationServices.ProductPropertyDTO pp = new MigrationServices.ProductPropertyDTO();

                pp.Choices = GetPropertyChoices(old.bvin);
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
                        ProductPropertyMapper.Add(old.bvin, newId);
                        wl("SUCCESS");
                    }
                }
            }

        }
        private List<MigrationServices.ProductPropertyChoiceDTO> GetPropertyChoices(string propertyBvin)
        {
            List<MigrationServices.ProductPropertyChoiceDTO> result = new List<MigrationServices.ProductPropertyChoiceDTO>();

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            var choices = db.bvc_ProductPropertyChoice.Where(y => y.PropertyBvin == propertyBvin)
                            .OrderBy(y => y.SortOrder);
            if (choices == null) return result;

            foreach (data.bvc_ProductPropertyChoice ppc in choices)
            {
                MigrationServices.ProductPropertyChoiceDTO dto = new MigrationServices.ProductPropertyChoiceDTO();
                dto.ChoiceName = ppc.ChoiceName;
                dto.LastUpdated = ppc.LastUpdated;
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

                BV5Address oldAddr = new BV5Address();
                oldAddr.FromXmlString(old.Address);
                vm.Address = new MigrationServices.AddressDTO();
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.bvin;
                vm.Contacts = new List<MigrationServices.VendorManufacturerContactDTO>();
                vm.ContactType = MigrationServices.VendorManufacturerTypeDTO.Manufacturer;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = old.DropShipEmailTemplateId;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = old.LastUpdated;


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

                BV5Address oldAddr = new BV5Address();
                oldAddr.FromXmlString(old.Address);
                vm.Address = new MigrationServices.AddressDTO();
                if (oldAddr != null)
                {
                    oldAddr.CopyTo(vm.Address, EFConnString(settings.SourceConnectionString()));
                }
                vm.Bvin = old.bvin;
                vm.Contacts = new List<MigrationServices.VendorManufacturerContactDTO>();
                vm.ContactType = MigrationServices.VendorManufacturerTypeDTO.Vendor;
                vm.DisplayName = old.DisplayName;
                vm.DropShipEmailTemplateId = old.DropShipEmailTemplateId;
                vm.EmailAddress = old.EmailAddress;
                vm.LastUpdated = old.LastUpdated;


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

            TaxScheduleMapper = new Dictionary<string, long>();

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
                        TaxScheduleMapper.Add(old.bvin, newId);
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
                TaxScheduleMapper.Add("Default", defId);
                TaxScheduleMapper.Add("", defId);
            }
        }
        private void ImportTaxes()
        {
            Header("Importing Taxes");


            foreach (data.bvc_Tax old in oldDatabase.bvc_Tax)
            {
                BVSoftware.Web.Geography.Country newCountry = GeographyHelper.TranslateCountry(EFConnString(settings.SourceConnectionString()), old.CountryBvin);
                string RegionAbbreviation = GeographyHelper.TranslateRegionBvinToAbbreviation(EFConnString(settings.SourceConnectionString()), old.RegionBvin);

                wl("Tax: " + newCountry.DisplayName + ", " + RegionAbbreviation + " " + old.PostalCode);

                MigrationServices.TaxDTO tx = new MigrationServices.TaxDTO();
                tx.ApplyToShipping = old.ApplyToShipping;
                tx.CountryName = newCountry.DisplayName;
                tx.PostalCode = old.PostalCode;
                tx.Rate = old.Rate;
                tx.RegionAbbreviation = RegionAbbreviation;

                string matchId = old.TaxClass;
                if (matchId.Trim().Length < 1) matchId = "Default";

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
            AffiliateMapper = new Dictionary<string, long>();

            foreach (data.bvc_Affiliate aff in oldDatabase.bvc_Affiliate)
            {
                wl("Affiliate: " + aff.DisplayName + " | " + aff.bvin);

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
                        AffiliateMapper.Add(aff.bvin, newId);
                        wl("SUCCESS");
                        ImportAffiliateReferrals(aff.bvin, newId);
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

            BV5Address oldAddress = new BV5Address();
            oldAddress.FromXmlString(aff.Address);

            affiliate.Address = new MigrationServices.AddressDTO();
            if (oldAddress != null)
            {
                oldAddress.CopyTo(affiliate.Address, EFConnString(settings.SourceConnectionString()));
            }
            affiliate.CommissionAmount = aff.CommissionAmount;
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
            }
            affiliate.CustomThemeName = aff.StyleSheet;
            affiliate.DisplayName = aff.DisplayName;
            affiliate.DriversLicenseNumber = aff.DriversLicenseNumber;
            affiliate.Enabled = aff.Enabled;
            affiliate.Id = -1;
            affiliate.LastUpdatedUtc = aff.LastUpdated;
            affiliate.Notes = aff.Notes;
            affiliate.ReferralDays = aff.ReferralDays;
            affiliate.ReferralId = aff.ReferralID;
            if (affiliate.ReferralId == string.Empty) affiliate.ReferralId = aff.bvin;
            affiliate.TaxId = aff.TaxID;
            affiliate.WebSiteUrl = aff.WebSiteURL;
            affiliate.Contacts = new List<MigrationServices.AffiliateContactDTO>();

            return affiliate;
        }
        private void ImportAffiliateReferrals(string bvin, long newId)
        {
            wl(" - Migrating Referrals...");

            data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

            var referrals = db.bvc_AffiliateReferral.Where(y => y.affid == bvin);
            if (referrals == null) return;

            foreach (data.bvc_AffiliateReferral r in referrals)
            {
                MigrationServices.AffiliateReferralDTO rnew = new MigrationServices.AffiliateReferralDTO();
                rnew.AffiliateId = newId;
                rnew.TimeOfReferralUtc = r.TimeOfReferral;
                rnew.ReferrerUrl = r.referrerurl;

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

            for (int i = 0; i < totalPages; i++)
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
            customer.Bvin = u.bvin;
            customer.CreationDateUtc = u.CreationDate;
            customer.Email = u.Email;
            customer.FailedLoginCount = u.FailedLoginCount;
            customer.FirstName = u.FirstName;
            customer.LastLoginDateUtc = u.LastLoginDate;
            customer.LastName = u.LastName;
            customer.LastUpdatedUtc = u.LastUpdated;
            customer.Notes = u.Comment;
            customer.Password = string.Empty;
            customer.PricingGroupId = u.PricingGroup;
            customer.Salt = string.Empty;
            customer.TaxExempt = u.TaxExempt == 1 ? true : false;
            customer.Addresses = new List<MigrationServices.AddressDTO>();

            // Preserve clear text passwords
            string newPassword = string.Empty;
            if (u.PasswordFormat == 0)
            {
                newPassword = u.Password;
            }

            BV5Address shipping = new BV5Address();
            shipping.FromXmlString(u.ShippingAddress);
            MigrationServices.AddressDTO ship = new MigrationServices.AddressDTO();
            ship.AddressType = MigrationServices.AddressTypesDTO.Shipping;
            shipping.CopyTo(ship, EFConnString(settings.SourceConnectionString()));
            customer.Addresses.Add(ship);

            BV5Address billing = new BV5Address();
            billing.FromXmlString(u.BillingAddress);
            MigrationServices.AddressDTO bill = new MigrationServices.AddressDTO();
            bill.AddressType = MigrationServices.AddressTypesDTO.Billing;
            billing.CopyTo(bill, EFConnString(settings.SourceConnectionString()));
            customer.Addresses.Add(bill);

            List<BV5Address> addresses = BV5Address.ReadAddressesFromXml(u.AddressBook);
            foreach (BV5Address addr in addresses)
            {
                MigrationServices.AddressDTO a = new MigrationServices.AddressDTO();
                a.AddressType = MigrationServices.AddressTypesDTO.General;
                addr.CopyTo(a, EFConnString(settings.SourceConnectionString()));
                customer.Addresses.Add(a);
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

            foreach (data.bvc_PriceGroup oldGroup in oldDatabase.bvc_PriceGroup)
            {
                wl("Price Group: " + oldGroup.Name);

                MigrationServices.PriceGroupDTO pg = new MigrationServices.PriceGroupDTO();
                pg.AdjustmentAmount = oldGroup.AdjustmentAmount;
                pg.Bvin = oldGroup.bvin;
                pg.Name = oldGroup.Name;
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
