using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;

using System.Data;

namespace Interface.Product
{
    [ServiceContract]
    public partial interface iProductMaster
    {
        #region Bind Dropdown
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProductType> GetProductTypeList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mUOM> GetProductUOMList(string[] conn);
        #endregion

        #region Product Info
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mProduct GetmProductToUpdate(long productID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        GetProductDetail GetProductDetailByProductID(long productID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mProduct GetProductDetailByProductIDForUpdate(long productID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long FinalSaveProductDetailByProductID(mProduct product, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetProductDetail> GetProductList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetProductDetail> GetProductListDeptWise(long UID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetProductDetail> GetAssetList(string[] conn);
        #endregion

        #region Prodcut Tax Setup
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateTempTaxSetup(string TaxID, string IsChecked, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetProductMasterTaxSetupByProductID_Result> GetProductTaxDetailByProductID(long productID, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetProductMasterTaxSetupByProductID_Result> GetTempSaveProductTaxDetailBySessionID(string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveProductTaxDetailByProductID(string sessionID, string userID, long productID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempSaveProductTaxDetailBySessionID(string sessionID, string userid, string[] conn);
        #endregion

        #region Prodcut Specification
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProductSpecificationDetail> AddProductSpecificationToTempData(mProductSpecificationDetail ProductSpecification, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProductSpecificationDetail> GetProductSpecificationDetailByProductID(long productID, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempSaveProductSpecificationDetailBySessionID(string sessionID, string userid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveProductSpecificationDetailByProductID(string sessionID, string userID, long productID, long companyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProductSpecificationDetail> SetValuesToTempData_onChange(long productID, string sessionID, string userID, string[] conn, int paraSequence, mProductSpecificationDetail paraInput);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mProductSpecificationDetail GetSpecificationDetailFromTempTableBySequence(string paraSessionID, string paraUserID, long productID, int paraSequence, string[] conn);
        #endregion

        #region ProductMaster Images
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void AddTempProductImages(tImage AddImage, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductImagesByProductID(long productID, string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTempSaveProductImagesBySessionID(string sessionID, string userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveProductImagesByProductID(string sessionID, string userID, long productID, string FilePath, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempSaveProductImagesBySessionID(string sessionID, string userid, string[] conn);
        #endregion

        #region ProductMaster checkDuplicateRecord
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecord(string ProdName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecordEdit(int ProdID, string ProdName, string[] conn);

        #endregion

        #region ProductMaster change rates
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveNewRates(mProductRateDetail NewRate, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProductRateDetail> GetProductRateHistory(long ProductID, string[] conn);
        #endregion

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductofEngine(string frmdt, string todt, string SID, string EID, string[] conn);

        #region Inventory Code
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetSiteWiseInventoryByProductIDs_Result> GetInventoryDataByProductIDs(string ProductIDs, string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDB(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetSiteWiseInventoryByProductIDs_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long UpdateProductInvetory_TempData(string SessionID, string CurrentObjectName, string UserID, SP_GetSiteWiseInventoryByProductIDs_Result InventoryRec, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveProductInventory(string paraSessionID, string paraCurrentObjectName, long ProductID, DateTime EffectiveDate, string paraUserID, string[] conn);

        #endregion

        #region ToolSite

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveToolSiteHistory(mToolSiteHistory mTool, string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_ToolSiteHistoryDetail> GetToolSiteHistory(long ProductID, string[] conn);

        //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        //List<VW_ToolSiteHistoryDetail> GetSitewiseTool(long SiteID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_SiteWiseTools_Result> GetSitewiseTool(long SiteID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_SiteWiseTools_Result> RemoveAssetFromCurrentAsset_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_SiteWiseTools_Result> GetExistingTempDataBySessionIDObjectNamePrd(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetToolTransferHead(tToolTransferHead tToolHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveToolTransferDetails(string paraSessionID, string paraObjectName, string paraReferenceID, string UserID, string ToSite, DateTime TransferDate, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_VW_ToolTransferDetails> GetTransferList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tToolTransferHead GetToolTransferHead(long TransferHeadID, string[] conn);
        #endregion

        # region  new product related code for WMS
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDepartment(long ParentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCompanyname(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUOMList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetUomShort(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntomPackUom(long SkuId, string ShortDescri, string Description, long Quantity, long Sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUOMPackDetails(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetEditmpackUOmdetail(long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatemPackUom(long Id, string ShortDescri, string Description, long Quantity, long Sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBomProductDetail(string Edit, long BOMheaderId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void saveBomDetail(long BOMheaderId, long SKUId, long Quantity, long Sequence, string Remark, string state, long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RemoveBOMDetailSKu(long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBOMDetailById(long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUOMPackDetailsByProdId(long SkuId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBOmDeyailbyIdforedit(long BOMheaderId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet getProductSpecification(long ProductID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet FillInventoryGrid(long SiteID, long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateInventryOpeningBal(decimal OpeningStock, long ProdID, long SiteID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePackUOMforSkuId(long SkuId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateBOMforsSkuId(long BOMheaderId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteMpackUOMBOMClear(string[] conn);
        # endregion

        #region WMS ImageImport Related code
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSKUByFilename(string OMSSKUCode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long getSKUIdfromtImage(long ReferenceID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertintotImage(string ObjectName, long ReferenceID, string ImageName, string Path, string Extension, string CreatedBy, DateTime CreationDate, long CompanyID, byte[] SkuImage, string ImageTitle, string ImageDescr, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntotImageImportLog(long ReferenceID, string ImageName, string Path, string CreatedBy, DateTime CreationDate, string Reason, long CompanyID, long DeptID, string OMSSkuCode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetFailedImageDetail(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void deleteimportlogdata(long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearImageUploadLog(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void SaveProductStockDetail(long SiteID, long ProdID, decimal AvailableBalance, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal UpdateProductStockQty(long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntoDistribution(long TemplateID, long ContactID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateDistribution(long TemplateID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteDistribution(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RemoveDistribution(long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetAvailStockById(long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetEditSpecifcdetail(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void Updatespecification(long ID, string SpecificationDescription, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tImage GetImageDetailByImageId(long ImgID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void EditTempProductImages(tImage AddImage, string sessionID, string userID, string state, string[] conn);
        #endregion

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCreatedByDate(string SelImageID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveProductImagesByProductIDEdit(string sessionID, string userID, long productID, string FilePath, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetAVLBalance(string MinQty, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecordSKUCODE(string omsskucode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecordEditSKUCODE(int ProdID, string omsskucode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateImageInTImage(long ReferenceID, string ImageName, string Path, string Extension, string LastModifiedBy, long CompanyID, byte[] SkuImage, string ImageTitle, string ImageDescr, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAvailQuantity(long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateVirtualBalance(long ProdID, decimal VirtualQty, decimal AvailVirtualQty, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntoInventry(long SKUId, long StoreId, DateTime Transactiondate, decimal Quantity, string[] conn);

        #region SKU Price Import
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteSKUPricetemp(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSkuPriceTemp(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetPriceImportData(long StoreId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateImportSkuPrice(string ProductCode, decimal PrincipalPrice, long StoreId, string[] conn);
        #endregion

        #region
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetLocation(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void DeleteOrderImport(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTempDirectOrder(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDirectOrderData(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDistinctPLCodes(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetImportDatabyDisDeptLocId(long ID, long locationId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string getOrderFormatNumber(long StoreId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetmaxDeliverydays(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveOrderHeaderImport(long StoreId, DateTime Orderdate, DateTime Deliverydate, long AddressId, long Status, long CreatedBy, DateTime Creationdate, string Title, DateTime ApprovalDate, decimal TotalQty, decimal GrandTotal, string OrderNo, long LocationID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void SaveOrderDetailImport(long OrderHeadId, long SkuId, decimal OrderQty, long UOMID, long Sequence, string Prod_Name, string Prod_Description, string Prod_Code, decimal Price, decimal Total, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void updateproductstockdetailimport(long SiteID, long ProdID, decimal AvailableBalance, decimal TotalDispatchQty, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTotalForOrderHead(long ID, long locationId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ImportMsgTransHeader(long RequestID, string[] conn);
        #endregion

         #region Code for Brilliant WMS according to new change

        // code for Inventry tab

        //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
       // mProductLocation GetProductLocation(Int64 ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long InsertProductLocation(mProductLocation prdloc, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int UpdateProdLocation(mProductLocation prdloc, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetProductInventryLocation> GetProductLocationbyID(long Id, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetProductInventryLocation> GetProductLocation(long ProdId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProdLocByPLID(long Id, string[] conn);

        // Product Channel tab Code
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long insertProductChannel(mProductChannel prdch, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int updateproductChannel(mProductChannel prdch, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetProductChannel> GetProductChannel(long ProdId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProdChannelByID(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long InsertOpBlInProductStock(tProductStockDetail Prdstock, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int UpdateOpBlnProductStock(tProductStockDetail Prdstock, string[] conn);

        // Product Vendor tab code
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertProductVendor(mProductVendor prdvend, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductVendor(long SKUID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetProdVendCount(long SKUID, long VendorID, string[] conn);

        // End Product Vendor

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTaxByCustomer(long CustomerID, string[] conn);

        #endregion
    }

}
