using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Catalog;
using BVSoftware.CommerceDTO.v1.Client;
using BVSoftware.CommerceDTO.v1.Contacts;
using BVSoftware.CommerceDTO.v1.Content;
using BVSoftware.CommerceDTO.v1.Marketing;
using BVSoftware.CommerceDTO.v1.Membership;
using BVSoftware.CommerceDTO.v1.Orders;
using BVSoftware.CommerceDTO.v1.Shipping;
using BVSoftware.CommerceDTO.v1.Taxes;

namespace BVSoftware.Commerce.Migration.Migrators.BV6
{
    public class Migrator : IMigrator
    {

        private const int CHUNKSIZE = 131072;
        private MigrationSettings settings = null;
        private Dictionary<string, long> AffiliateMapper = new Dictionary<string, long>();
        private Dictionary<long, long> TaxScheduleMapper = new Dictionary<long, long>();
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
        
        private void DumpErrors(List<ApiError> errors)
        {
            foreach (ApiError e in errors)
            {
                wl("ERROR: " + e.Code + " | " + e.Description);
            }
        }

        private BVSoftware.CommerceDTO.v1.Client.Api GetBV6Proxy()
        {
            Api result = null;
            try
            {
                string serviceUrl = settings.DestinationServiceRootUrl;
                string apiKey = settings.ApiKey;
                result = new Api(serviceUrl, apiKey);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
            }
            return result;
        }
        private BVSoftware.CommerceDTO.v1.Client.Api GetOldStoreBV6Proxy()
        {
            Api result = null;
            try
            {
                string serviceUrl = settings.SourceServiceRootUrl;
                string apiKey = settings.SourceApiKey;
                result = new Api(serviceUrl, apiKey);
            }
            catch (Exception ex)
            {
                wl("EXCEPTION While attempting to create service proxy for old store BV 6!");
                wl(ex.Message);
                wl(ex.StackTrace);
            }
            return result;
        }

        public void Migrate(MigrationSettings s)
        {
            wl("");
            wl("BV Commerce 6 Migrator Started");
            wl("");

            settings = s;

            try
            {

                // Clear Products
                if (s.ClearProducts && s.ImportProductImagesOnly == false)
                {
                    ClearProducts();
                }

                // Clear Categories
                if (s.ClearCategories)
                {
                    ClearCategories();
                }

                // Clear Users
                if (s.ClearUsers)
                {
                    ClearUsers();
                }

                // Users 
                if (s.ImportUsers)
                {
                    //ImportRoles();
                    ImportPriceGroups();
                    //ImportUsers();
                }

                // Affiliates
                if (s.ImportAffiliates)
                {
                    //ImportAffiliates();
                }

                // Taxes and Shipping Classes are prerequisite for product import
                if (s.ImportOtherSettings || (s.ImportProducts && s.SkipProductPrerequisites == false))
                {
                    ImportTaxSchedules();
                    ImportTaxes();
                }

                // Vendors and Manufacturers
                if ((s.ImportProducts && s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false) || s.ImportCategories)
                {
                    ImportVendors();
                    ImportManufacturers();
                }

                // Product Types
                if (s.ImportProducts && s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false)
                {
                    //ImportProductProperties();
                    ImportProductTypes();
                }

                // Categories
                if (s.ImportCategories)
                {
                    ImportCategories();
                }

                if (s.ImportProducts)
                {
                    if (s.ImportProductImagesOnly == false && s.SkipProductPrerequisites == false)
                    {                        
                        ImportProductChoices();
                    }

                    ImportProducts();                    

                    if (s.ImportProductImagesOnly == false)
                    {
                        ImportRelatedItems();
                    }
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

            Api oldStore = GetOldStoreBV6Proxy();

            if (settings.SingleOrderImport.Trim().Length > 0)
            {
                ApiResponse<OrderDTO> o = oldStore.OrdersFind(settings.SingleOrderImport);
                ImportSingleOrder(o.Content);                        
            }
            else
            {
                // Multi-Order Mode
                int pageSize = int.MaxValue;
                int totalRecords = int.MaxValue; //oldDatabase.bvc_Order.Where(y => y.IsPlaced == 1).Count();
                int totalPages = 0; // (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));
                for (int i = 0; i < totalPages; i++)
                {
                    wl("Getting Orders page " + (i + 1) + " of " + totalPages.ToString());

                    ApiResponse<List<OrderSnapshotDTO>> oldOrders = oldStore.OrdersFindAll();
                    if (oldOrders == null) continue;

                    if (settings.DisableMultiThreading)
                    {
                        foreach (OrderSnapshotDTO o in oldOrders.Content)
                        {
                            ApiResponse<OrderDTO> ofull = oldStore.OrdersFind(o.bvin);
                            ImportSingleOrder(ofull.Content);
                        }
                    }
                    else
                    {
                        System.Threading.Tasks.Parallel.ForEach(oldOrders.Content, ImportSingleOrder);
                    }
                }
            }
        }
        private void ImportSingleOrder(OrderSnapshotDTO snap)
        {
            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<OrderDTO> response = oldProxy.OrdersFind(snap.bvin);
            ImportSingleOrder(snap);
        }
        private void ImportSingleOrder(OrderDTO old)
        {
            if (old == null) return;
            wl("Processing Order: " + old.OrderNumber);

            OrderDTO o = old;            
            if (o != null)
            {
                Api proxy = GetBV6Proxy();
                var res = proxy.OrdersCreate(o);
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
                        ImportOrderTransactions(o.Bvin, o.OrderNumber);
                    }
                }

            }
        }        
        private void ImportOrderTransactions(string orderBvin, string orderNumber)
        {
            wl(" - Transactions for Order " + orderNumber);

            Api oldProxy = GetOldStoreBV6Proxy();            
            Api proxy = GetBV6Proxy();

            ApiResponse<List<OrderTransactionDTO>> old = oldProxy.OrderTransactionsFindForOrder(orderBvin);
            if (old == null) return;

            foreach (OrderTransactionDTO item in old.Content)
            {
                wl("Transaction: " + item.Id);
                proxy.OrderTransactionsCreate(item);
            }
        }

        private void ImportRelatedItems()
        {
            Header("Importing Related Items");

            Api oldProxy = GetOldStoreBV6Proxy();            
            Api proxy = GetBV6Proxy();

            //TODO: Need to get relationships pulled

            //ApiResponse<List<ProductRelationshipDTO>> all = oldProxy.pro
            //var crosses = db.bvc_ProductCrossSell;
            //if (crosses == null) return;
            //foreach (data.bvc_ProductCrossSell x in crosses)
            //{
            //    wl("Relating " + x.ProductBvin + " to " + x.CrossSellBvin);
            //    proxy.ProductRelationshipsQuickCreate(x.ProductBvin, x.CrossSellBvin, false);
            //}

            //var ups = db.bvc_ProductUpSell;
            //if (ups == null) return;
            //foreach (data.bvc_ProductUpSell up in ups)
            //{
            //    wl("Relating Up " + up.ProductBvin + " to " + up.UpSellBvin);
            //    proxy.ProductRelationshipsQuickCreate(up.ProductBvin, up.UpSellBvin, true);
            //}
        }

        private void ImportProducts()
        {
            Header("Importing Products");

            Api oldProxy = GetOldStoreBV6Proxy();

            int limit = -1;
            if (settings.ImportProductLimit > 0)
            {
                limit = settings.ImportProductLimit;
            }
            int totalMigrated = 0;

            ApiResponse<List<ProductDTO>> allProducts = oldProxy.ProductsFindAll();

            int pageSize = 10;
            int totalRecords = allProducts.Content.Count();
            int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

                if (settings.DisableMultiThreading)
                {
                    foreach (ProductDTO p in allProducts.Content)
                    {
                        ImportSingleProduct(p);

                        totalMigrated += 1;
                        if (limit > 0 && totalMigrated >= limit)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    System.Threading.Tasks.Parallel.ForEach(allProducts.Content, ImportSingleProduct);
                }
            
        }
        //private List<CustomPropertyDTO> TranslateOldProperties(string oldXml)
        //{
        //    List<CustomPropertyDTO> result = new List<CustomPropertyDTO>();

        //    CustomPropertyCollection props = CustomPropertyCollection.FromXml(oldXml);
        //    if (props != null)
        //    {
        //        foreach (CustomProperty prop in props)
        //        {
        //            result.Add(prop.ToDto());
        //        }
        //    }
        //    return result;
        //}
        private void ImportSingleProduct(ProductDTO p)
        {
            wl("Product: " + p.ProductName + " [" + p.Sku + "]");
            byte[] bytes = new byte[0];

            Api proxy = GetBV6Proxy();
            var res = proxy.ProductsCreate(p, bytes);
            if (res != null)
            {
                if (res.Errors.Count > 0)
                {
                    DumpErrors(res.Errors);
                    wl("FAILED");
                }
                else
                {
                    if (settings.ImportProductImagesOnly == false)
                    {                        
                        AssignOptionsToProduct(p.Bvin);
                        AssignProductPropertyValues(p.Bvin);
                    }
                    wl("SUCCESS");
                }
            }


            if (settings.ImportProductImagesOnly == false)
            {
                // Inventory                        
                MigrateProductInventory(p.Bvin);

                // File Downloads
                //MigrateProductFileDownloads(p.Bvin);
            }

            // Additional Images
            MigrateProductAdditionalImages(p.Bvin);

            if (settings.ImportProductImagesOnly == false)
            {
                // Volume Prices
                MigrateProductVolumePrices(p.Bvin);

                // Reviews
                MigrateProductReviews(p.Bvin);

                // Link to Categories
                MigrateProductCategoryLinks(p.Bvin);
            }

            
        }
        private void AssignOptionsToProduct(string bvin)
        {
            wl(" - Migrating Options...");
            Api oldProxy = GetOldStoreBV6Proxy();
            
            ApiResponse<List<OptionDTO>> options = oldProxy.ProductOptionsFindAllByProductId(bvin);
            
            Api proxy = GetBV6Proxy();
            foreach (OptionDTO opt in options.Content)
            {
                proxy.ProductOptionsAssignToProduct(opt.Bvin, bvin, false);
            }            
        }

        private void AssignProductPropertyValues(string bvin)
        {
            wl(" - Migrating Property Values...");

            Api oldProxy = GetOldStoreBV6Proxy();                        
            Api proxy = GetBV6Proxy();

            // TODO: Need to migration property data
        }
        private void MigrateProductInventory(string bvin)
        {
            wl(" - Migrating Inventory...");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<ProductInventoryDTO> inventories = oldProxy.ProductInventoryFind(bvin);
            
            Api proxy = GetBV6Proxy();
            proxy.ProductInventoryCreate(inventories.Content);

        }
        private void MigrateProductAdditionalImages(string bvin)
        {
            wl(" - Migrating AdditionalImages...");

            Api oldProxy = GetOldStoreBV6Proxy();
            //oldProxy.ProductImagesFindForProduct(bvin);

            Api proxy = GetBV6Proxy();
            
            // TODO: Move old images over
        }
        private void MigrateProductFileDownloads(string bvin)
        {
            Header("Migrating File Downloads");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<ProductFileDTO>> files = oldProxy.ProductFilesFindForProduct(bvin);            
            Api proxy = GetBV6Proxy();

            foreach (ProductFileDTO item in files.Content)
            {
                proxy.ProductFilesCreate(item);
                proxy.ProductFilesAddToProduct(bvin, item.Bvin, item.AvailableMinutes, item.MaxDownloads);
            }            
        }

        private void MigrateProductVolumePrices(string bvin)
        {
            wl(" - Migrating Volume Prices...");

            Api oldProxy = GetOldStoreBV6Proxy();
                        
            //Api proxy = GetBV6Proxy();

            //var items = db.bvc_ProductVolumeDiscounts.Where(y => y.ProductID == bvin);
            //if (items == null) return;
            //foreach (data.bvc_ProductVolumeDiscounts item in items)
            //{
            //    ProductVolumeDiscountDTO v = new ProductVolumeDiscountDTO();
            //    v.Amount = item.Amount;
            //    v.Bvin = item.bvin;
            //    switch (item.DiscountType)
            //    {
            //        case 1:
            //            v.DiscountType = ProductVolumeDiscountTypeDTO.Percentage;
            //            break;
            //        case 2:
            //            v.DiscountType = ProductVolumeDiscountTypeDTO.Amount;
            //            break;
            //    }
            //    v.DiscountType = (ProductVolumeDiscountTypeDTO)item.DiscountType;
            //    v.LastUpdated = item.LastUpdated;
            //    v.ProductId = item.ProductID;
            //    v.Qty = item.Qty;

            //    wl("Discount for qty: " + v.Qty + " [" + v.Bvin + "]");
            //    var res = proxy.ProductVolumeDiscountsCreate(v);
            //    if (res != null)
            //    {
            //        if (res.Errors.Count > 0)
            //        {
            //            DumpErrors(res.Errors);
            //            wl("FAILED");
            //        }
            //    }
            //    else
            //    {
            //        wl("FAILED! EXCEPTION!");
            //    }
            //}
        }
        private void MigrateProductReviews(string bvin)
        {
            wl(" - Migrating Reviews...");

            Api oldProxy = GetOldStoreBV6Proxy();
            
            //data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
            //Api proxy = GetBV6Proxy();

            //var items = db.bvc_ProductReview.Where(y => y.ProductBvin == bvin);
            //if (items == null) return;
            //foreach (data.bvc_ProductReview item in items)
            //{
            //    ProductReviewDTO r = new ProductReviewDTO();
            //    r.Approved = item.Approved == 1;
            //    r.Bvin = item.bvin;
            //    r.Description = item.Description;
            //    r.Karma = item.Karma;
            //    r.ProductBvin = item.ProductBvin;
            //    switch (item.Rating)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //            break;
            //        case 2:
            //            break;
            //        case 3:
            //            break;
            //        case 4:
            //            break;
            //        case 5:
            //            break;
            //    }
            //    r.ReviewDateUtc = item.ReviewDate;
            //    r.UserID = item.UserID;

            //    wl("Review [" + r.Bvin + "]");
            //    var res = proxy.ProductReviewsCreate(r);
            //    if (res != null)
            //    {
            //        if (res.Errors.Count > 0)
            //        {
            //            DumpErrors(res.Errors);
            //            wl("FAILED");
            //        }
            //    }
            //    else
            //    {
            //        wl("FAILED! EXCEPTION!");
            //    }
            //}
        }
        private void MigrateProductCategoryLinks(string bvin)
        {
            wl(" - Migrating Category Links");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<CategorySnapshotDTO>> cats = oldProxy.CategoriesFindForProduct(bvin);
            Api proxy = GetBV6Proxy();

            foreach (CategorySnapshotDTO snap in cats.Content)
            {
                wl("To Category: " + snap.Name);
                proxy.CategoryProductAssociationsQuickCreate(bvin, snap.Bvin);
            }

        }

        private void ImportProductChoices()
        {
            Header("Importing Shared Choices");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<OptionDTO>> options = oldProxy.ProductOptionsFindAll();

            foreach (OptionDTO old in options.Content)
            {                
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductOptionsCreate(old);
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


        // Categories
        private void ImportCategories()
        {
            Header("Importing Categories");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<CategorySnapshotDTO>> allcats = oldProxy.CategoriesFindAll();
            foreach (CategorySnapshotDTO old in allcats.Content)
            {
                wl("Category: " + old.Name);

                CategoryDTO cat = oldProxy.CategoriesFind(old.Bvin).Content;
                
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.CategoriesCreate(cat);
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

        // Properties and Types
        private void ImportProductTypes()
        {
            Header("Importing Product Types");
            
            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<ProductTypeDTO>> types = oldProxy.ProductTypesFindAll();
            foreach (ProductTypeDTO old in types.Content)
            {
                wl("Item: " + old.ProductTypeName);

                ProductTypeDTO pt = old;
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductTypesCreate(pt);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        //TODO: Migrate Properties for Types
                        //MigratePropertiesForType(pt.Bvin);
                        wl("SUCCESS");
                    }
                }
            }
        }
        //private void MigratePropertiesForType(string typeBvin)
        //{
        //    wl("Migrating Properties to Type...");

        //    Api oldProxy = GetOldStoreBV6Proxy();
            
        //    data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
        //    var crosses = db.bvc_ProductTypeXProductProperty.Where(y => y.ProductTypeBvin == typeBvin);
        //    if (crosses == null) return;

        //    foreach (data.bvc_ProductTypeXProductProperty cross in crosses)
        //    {
        //        int sort = cross.SortOrder;
        //        string oldPropertyBvin = cross.ProductPropertyBvin;
        //        long newId = 0;
        //        if (ProductPropertyMapper.ContainsKey(oldPropertyBvin))
        //        {
        //            newId = ProductPropertyMapper[oldPropertyBvin];
        //        }
        //        if (newId <= 0) continue;
        //        wl("Mapping " + oldPropertyBvin + " to " + newId.ToString());
        //        Api bv6proxy = GetBV6Proxy();
        //        bv6proxy.ProductTypesAddProperty(typeBvin, newId, sort);
        //    }
        //}
        //private void ImportProductProperties()
        //{
        //    Header("Importing Product Properties");

        //    ProductPropertyMapper = new Dictionary<string, long>();

        //    foreach (data.bvc_ProductProperty old in oldDatabase.bvc_ProductProperty)
        //    {
        //        wl("Item: " + old.DisplayName);

        //        ProductPropertyDTO pp = new ProductPropertyDTO();

        //        pp.Choices = GetPropertyChoices(old.bvin);
        //        pp.CultureCode = old.CultureCode;
        //        pp.DefaultValue = old.DefaultValue;
        //        pp.DisplayName = old.DisplayName;
        //        pp.DisplayOnSite = old.DisplayOnSite == 1;
        //        pp.DisplayToDropShipper = old.DisplayToDropShipper == 1;
        //        pp.PropertyName = old.PropertyName;
        //        switch (old.TypeCode)
        //        {
        //            case 0:
        //                pp.TypeCode = ProductPropertyTypeDTO.None;
        //                break;
        //            case 1:
        //                pp.TypeCode = ProductPropertyTypeDTO.TextField;
        //                break;
        //            case 2:
        //                pp.TypeCode = ProductPropertyTypeDTO.MultipleChoiceField;
        //                break;
        //            case 3:
        //                pp.TypeCode = ProductPropertyTypeDTO.CurrencyField;
        //                break;
        //            case 4:
        //                pp.TypeCode = ProductPropertyTypeDTO.DateField;
        //                break;
        //            case 7:
        //                pp.TypeCode = ProductPropertyTypeDTO.HyperLink;
        //                break;
        //        }

        //        Api bv6proxy = GetBV6Proxy();
        //        var res = bv6proxy.ProductPropertiesCreate(pp);
        //        if (res != null)
        //        {
        //            if (res.Errors.Count() > 0)
        //            {
        //                DumpErrors(res.Errors);
        //                wl("FAILED");
        //            }
        //            else
        //            {
        //                long newId = res.Content.Id;
        //                ProductPropertyMapper.Add(old.bvin, newId);
        //                wl("SUCCESS");
        //            }
        //        }
        //    }
        //}
        //private List<ProductPropertyChoiceDTO> GetPropertyChoices(string propertyBvin)
        //{
        //    List<ProductPropertyChoiceDTO> result = new List<ProductPropertyChoiceDTO>();

        //    data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));
        //    var choices = db.bvc_ProductPropertyChoice.Where(y => y.PropertyBvin == propertyBvin)
        //                    .OrderBy(y => y.SortOrder);
        //    if (choices == null) return result;

        //    foreach (data.bvc_ProductPropertyChoice ppc in choices)
        //    {
        //        ProductPropertyChoiceDTO dto = new ProductPropertyChoiceDTO();
        //        dto.ChoiceName = ppc.ChoiceName;
        //        dto.LastUpdated = ppc.LastUpdated;
        //        //dto.PropertyId = ppc.PropertyBvin;
        //        dto.SortOrder = ppc.SortOrder;
        //        result.Add(dto);
        //    }

        //    return result;
        //}

        // Manufacturer Vendor
        private void ImportManufacturers()
        {
            Header("Importing Manufacturers");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<VendorManufacturerDTO>> items = oldProxy.ManufacturerFindAll();
            foreach (VendorManufacturerDTO old in items.Content)
            {
                wl("Item: " + old.DisplayName);

                VendorManufacturerDTO vm = old;
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ManufacturerCreate(vm);
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

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<VendorManufacturerDTO>> items = oldProxy.VendorFindAll();

            foreach (VendorManufacturerDTO old in items.Content)
            {
                wl("Item: " + old.DisplayName);
                VendorManufacturerDTO vm = old;
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.VendorCreate(vm);
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

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<TaxScheduleDTO>> items = oldProxy.TaxSchedulesFindAll();

            TaxScheduleMapper = new Dictionary<long, long>();

            foreach (TaxScheduleDTO old in items.Content)
            {
                long oldId = old.Id;

                wl("Tax Schedule: " + old.Name);

                TaxScheduleDTO ts = old;

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.TaxSchedulesCreate(ts);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        long newId = res.Content.Id;
                        TaxScheduleMapper.Add(oldId, newId);
                        wl("SUCCESS");
                    }
                }
            }

        }
        private void ImportTaxes()
        {
            Header("Importing Taxes");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<TaxDTO>> items = oldProxy.TaxesFindAll();

            foreach (TaxDTO old in items.Content)
            {                
                wl("Tax: " + old.CountryName + ", " + old.RegionAbbreviation + " " + old.PostalCode);

                long oldId = old.TaxScheduleId;
                TaxDTO tx = old;                
                if (TaxScheduleMapper.ContainsKey(oldId))
                {
                    tx.TaxScheduleId = TaxScheduleMapper[oldId];
                }
                                              
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.TaxesCreate(tx);
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

        //private void ImportAffiliates()
        //{
        //    Header("Importing Affiliates");
        //    AffiliateMapper = new Dictionary<string, long>();

        //    foreach (data.bvc_Affiliate aff in oldDatabase.bvc_Affiliate)
        //    {
        //        wl("Affiliate: " + aff.DisplayName + " | " + aff.bvin);

        //        try
        //        {
        //            AffiliateDTO a = OldToNewAffiliate(aff);

        //            Api bv6proxy = GetBV6Proxy();
        //            var res = bv6proxy.AffiliatesCreate(a);
        //            if (res != null)
        //            {
        //                if (res.Errors.Count > 0)
        //                {
        //                    DumpErrors(res.Errors);
        //                    return;
        //                }
        //                if (res.Content == null) throw new ArgumentNullException("Result object was null");
        //                long newId = res.Content.Id;
        //                AffiliateMapper.Add(aff.bvin, newId);
        //                wl("SUCCESS");
        //                ImportAffiliateReferrals(aff.bvin, newId);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            wl("FAILED: " + ex.Message + " | " + ex.StackTrace);
        //        }

        //    }
        //}
        //private AffiliateDTO OldToNewAffiliate(data.bvc_Affiliate aff)
        //{
        //    AffiliateDTO affiliate = new AffiliateDTO();

        //    BV5Address oldAddress = new BV5Address();
        //    oldAddress.FromXmlString(aff.Address);

        //    affiliate.Address = new AddressDTO();
        //    if (oldAddress != null)
        //    {
        //        oldAddress.CopyTo(affiliate.Address, EFConnString(settings.SourceConnectionString()));
        //    }
        //    affiliate.CommissionAmount = aff.CommissionAmount;
        //    switch (aff.CommissionType)
        //    {
        //        case 0:
        //            affiliate.CommissionType = AffiliateCommissionTypeDTO.None;
        //            break;
        //        case 1:
        //            affiliate.CommissionType = AffiliateCommissionTypeDTO.PercentageCommission;
        //            break;
        //        case 2:
        //            affiliate.CommissionType = AffiliateCommissionTypeDTO.FlatRateCommission;
        //            break;
        //    }
        //    affiliate.CustomThemeName = aff.StyleSheet;
        //    affiliate.DisplayName = aff.DisplayName;
        //    affiliate.DriversLicenseNumber = aff.DriversLicenseNumber;
        //    affiliate.Enabled = aff.Enabled;
        //    affiliate.Id = -1;
        //    affiliate.LastUpdatedUtc = aff.LastUpdated;
        //    affiliate.Notes = aff.Notes;
        //    affiliate.ReferralDays = aff.ReferralDays;
        //    affiliate.ReferralId = aff.ReferralID;
        //    if (affiliate.ReferralId == string.Empty) affiliate.ReferralId = aff.bvin;
        //    affiliate.TaxId = aff.TaxID;
        //    affiliate.WebSiteUrl = aff.WebSiteURL;
        //    affiliate.Contacts = new List<AffiliateContactDTO>();

        //    return affiliate;
        //}
        //private void ImportAffiliateReferrals(string bvin, long newId)
        //{
        //    wl(" - Migrating Referrals...");

        //    data.BV53Entities db = new data.BV53Entities(EFConnString(settings.SourceConnectionString()));

        //    var referrals = db.bvc_AffiliateReferral.Where(y => y.affid == bvin);
        //    if (referrals == null) return;

        //    foreach (data.bvc_AffiliateReferral r in referrals)
        //    {
        //        AffiliateReferralDTO rnew = new AffiliateReferralDTO();
        //        rnew.AffiliateId = newId;
        //        rnew.TimeOfReferralUtc = r.TimeOfReferral;
        //        rnew.ReferrerUrl = r.referrerurl;

        //        Api bv6proxy = GetBV6Proxy();
        //        var res = bv6proxy.AffiliateReferralsCreate(rnew);
        //    }

        //}

        //// Users
        //private void ImportUsers()
        //{
        //    Header("Importing Users");


        //    int pageSize = 100;
        //    int totalRecords = oldDatabase.bvc_User.Count();
        //    int totalPages = (int)(Math.Ceiling((decimal)totalRecords / (decimal)pageSize));

        //    //System.Threading.Tasks.Parallel.For(0, totalPages, ProcessPage);
        //    int startIndex = 0;
        //    if (settings.UserStartPage > 1)
        //    {
        //        startIndex = settings.UserStartPage - 1;
        //    }
        //    for (int i = startIndex; i < totalPages; i++)
        //    {
        //        wl("Getting Users page " + (i + 1) + " of " + totalPages.ToString());
        //        int startRecord = i * pageSize;
        //        var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(pageSize).ToList();
        //        foreach (data.bvc_User u in users)
        //        {
        //            ImportSingleUser(u);
        //        }
        //    }

        //}
        //private void ProcessPage(int i)
        //{
        //    wl("Getting Users page " + (i + 1));
        //    int startRecord = i * 100;
        //    var users = (from u in oldDatabase.bvc_User select u).OrderBy(y => y.Email).Skip(startRecord).Take(100).ToList();
        //    foreach (data.bvc_User u in users)
        //    {
        //        ImportSingleUser(u);
        //    }
        //}
        //private void ImportSingleUser(data.bvc_User u)
        //{
        //    if (u == null)
        //    {
        //        wl("Customer was null!");
        //        return;
        //    }
        //    wl("Importing Customer: " + u.Email);

        //    CustomerAccountDTO customer = new CustomerAccountDTO();
        //    customer.Bvin = u.bvin;
        //    customer.CreationDateUtc = u.CreationDate;
        //    customer.Email = u.Email;
        //    customer.FailedLoginCount = u.FailedLoginCount;
        //    customer.FirstName = u.FirstName;
        //    customer.LastLoginDateUtc = u.LastLoginDate;
        //    customer.LastName = u.LastName;
        //    customer.LastUpdatedUtc = u.LastUpdated;
        //    customer.Notes = u.Comment;
        //    customer.Password = string.Empty;
        //    customer.PricingGroupId = u.PricingGroup;
        //    customer.Salt = string.Empty;
        //    customer.TaxExempt = u.TaxExempt == 1 ? true : false;
        //    customer.Addresses = new List<AddressDTO>();

        //    // Preserve clear text passwords
        //    string newPassword = string.Empty;
        //    if (u.PasswordFormat == 0)
        //    {
        //        newPassword = u.Password;
        //    }

        //    BV5Address shipping = new BV5Address();
        //    shipping.FromXmlString(u.ShippingAddress);
        //    AddressDTO ship = new AddressDTO();
        //    ship.AddressType = AddressTypesDTO.Shipping;
        //    shipping.CopyTo(ship, EFConnString(settings.SourceConnectionString()));
        //    customer.Addresses.Add(ship);

        //    BV5Address billing = new BV5Address();
        //    billing.FromXmlString(u.BillingAddress);
        //    AddressDTO bill = new AddressDTO();
        //    bill.AddressType = AddressTypesDTO.Billing;
        //    billing.CopyTo(bill, EFConnString(settings.SourceConnectionString()));
        //    customer.Addresses.Add(bill);

        //    List<BV5Address> addresses = BV5Address.ReadAddressesFromXml(u.AddressBook);
        //    foreach (BV5Address addr in addresses)
        //    {
        //        AddressDTO a = new AddressDTO();
        //        a.AddressType = AddressTypesDTO.General;
        //        addr.CopyTo(a, EFConnString(settings.SourceConnectionString()));
        //        customer.Addresses.Add(a);
        //    }

        //    Api bv6proxy = GetBV6Proxy();
        //    var res = bv6proxy.CustomerAccountsCreateWithPassword(customer, newPassword);
        //    if (res != null)
        //    {
        //        if (res.Errors.Count > 0)
        //        {
        //            DumpErrors(res.Errors);
        //            return;
        //        }
        //        wl("SUCCESS");
        //    }
        //}

        // Price Groups and Roles
        private void ImportPriceGroups()
        {
            Header("Importing Price Groups");

            Api oldProxy = GetOldStoreBV6Proxy();
            ApiResponse<List<PriceGroupDTO>> items = oldProxy.PriceGroupsFindAll();

            foreach (PriceGroupDTO oldGroup in items.Content)
            {
                wl("Price Group: " + oldGroup.Name);

                PriceGroupDTO pg = oldGroup;
                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.PriceGroupsCreate(pg);
                if (res != null)
                {
                    if (res.Errors.Count() > 0)
                    {
                        DumpErrors(res.Errors);
                        wl("FAILED");
                    }
                    else
                    {
                        if (res.Content == null)
                        {
                            wl("FAILED!");
                        }
                        else
                        {
                            wl(res.Content.Bvin == string.Empty ? "FAILED" : "SUCCESS");
                        }
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

            Api bv6proxy = GetBV6Proxy();
            var res = bv6proxy.CategoriesClearAll();
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
        private void ClearUsers()
        {
            Header("Clearing All Customer Records");

            Api bv6proxy = GetBV6Proxy();
            var res = bv6proxy.CustomerAccountsClearAll();
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

                Api bv6proxy = GetBV6Proxy();
                var res = bv6proxy.ProductsClearAll(pageSize);
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
                        if (res.Content != null)
                        {
                            remaining = res.Content.ProductsRemaining;
                            wl("Clearing products: " + res.Content.ProductsRemaining + " remaining at " + DateTime.UtcNow.ToString());
                        }
                        else
                        {
                            wl("Invalid Response. Skipping Clear..");
                            remaining = 0;
                        }
                    }
                }
            }
            wl("Finished Clearing Products at : " + DateTime.UtcNow.ToString());
        }
    }
}
