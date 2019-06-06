using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;
using System.Data;

namespace Interface.WMS
{
    [ServiceContract]

    public partial interface iInbound
    {
        #region InboundGrid

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindInboundGrid(long userCompany, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindInboundGridbyUser(long userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckSelectedPOStatusIsSameOrNot(string SelectedPO, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetNextObject(string SelectedRecords, string ObjectName, long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        bool SaveAssignedTask(tTaskDetail objTask, string OrderObjectName, string OID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        bool CheckJobCardofSelectedRecord(string SelectedPO, string Objectname, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CancelSelectedOrder(long SelectedOrder, long UserID, string[] conn);

        #endregion

        #region PO

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetUserWarehouse(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mVendor> GetVendor(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBNEW(string paraSessionID, string paraUserID, string CurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mStatu> GetStatusListForInbound(string ObjectName, string Remark, string state, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetWorkflowSequenceOfPO(string ObjectName, long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForPO_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForPO_Result> AddPartIntoRequest_TempData(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForPO_Result> GetRequestPartDetailByRequestID(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUOMofSelectedProduct(int ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetSelectedUom(long OrderId, long ProdID, long sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tPurchaseOrderHead GetOrderHeadByOrderID(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForPO_Result> RemovePartFromRequest_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetUOMName(long UOMID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData1(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalQTYFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int IsPriceChanged(int ProdID, decimal price, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData12(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalQTYofSequenceFromTempData(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetPrdIDofSequenceFromTempData(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData13(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SetIntotPurchaseOrderHead(tPurchaseOrderHead POHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSavePODetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tPurchaseOrderHead GetPoHeadByPOID(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetWarehouseNameByUserID(long uid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetAllWarehouseList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCorrespondance(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetApprovalInWorkFlow(long CmpID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tOrderHead GetSoHeadBySOIDForSalesReturn(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long getCustomerofUser(long UserID, long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTaxofProduct(long ProdID, string[] conn);
        #endregion

        #region GRN

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindGRNGrid(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindGRNGridUserWise(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindGRNGridofSelectedPO(long POID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForGRN_Result> GetGRNPartDetailsByPOID(string POID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForGRN_Result> GetGRNPartDetailsBySOID(string SOID, string TRID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForGRN_Result> GetExistingTempDataBySessionIDObjectNameGRN(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UPdateGRNTempData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForGRN_Result GRN, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetUserNameByID(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetGRNHead(tGRNHead GRNHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSaveGRNDetail(long Poid, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetTotalGRNPOWise(long POID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetGRNDetails GetGRNDetailsByGRNID(long POID, string ObjectNM, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetGrnSkuDetailsbyGRNID(long GRNID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetPOIDFromGRNID(long GRNID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForGRN_Result> RemovePartFromGRN_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetGRNDetails GetGRNDetailsByGRNIDGRNMenu(long GRNID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet CheckSelectedPOJobCardNo(long PONumber, string ObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckJobCard(long PONumber, string ObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBGRN(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckSelectedGRNStatusIsSameOrNot(string SelectedGRN, string[] conn);

        #endregion

        #region QC

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindQCGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindQCGridofSO(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindQCGridofSelectedGRN(long GRNID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByPOID(string POID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByGRNID(string GRNID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByQCID(string GRNID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByPickUPID(string PKUPID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBQC(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetExistingTempDataBySessionIDObjectNameQC(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UPdateQCTempData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForQC_Result QC, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdateQCTempDataReason(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForQC_Result QC, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetQualityControlHead(tQualityControlHead QCHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSaveQCDetail(long GrnId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string QCObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> RemovePartFromQC_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetQCDetails GetQCDetailsByGRNID(long GRNID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetQCDetails GetQCDetailsByQCID(long QCID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckSelectedQCStatusIsSameOrNot(string SelectedQC, string[] conn);
        #endregion

        #region Lable Printing

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindLPGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductDetails(string SelectedQC, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void SetSelectedRecordToLabelPrintingStatus(string SelectedQC, string[] conn);

        #endregion

        #region PutIn

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindPIGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_PutInList_Result> GetPutInList(string QCID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBPutIn(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_PutInList_Result> GetExistingTempDataBySessionIDObjectNamePI(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);        

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_VW_GetLocationDetails> GetLocationForPutIn(int pageIndex, string filter, long qcId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_VW_GetLocationDetails> GetLocationForPickUP(int pageIndex, string filter, long soId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdatePutInLstQtyofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdatePutInLstLocofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdatePutInLstPackofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSavedPutInListbyQCID(long qcID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSavedPutInListbyPutInID(string PutInNo, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetPutInHead(tPutInHead PIHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSavePutInDetail(long QCId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string PutInObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckSelectedPutInStatusIsSameOrNot(string SelectedPutIn, string[] conn);
        #endregion

        #region Transfer

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindTransferGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tTransferHead GetTransferHeadByTRID(long TransferID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByTransferID(string trID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_PutInList_Result> GetPutInListByTRID(long trID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetGRNIDByTransferID(long trID, string[] conn);

        #endregion

        #region Return

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindReturnGrid(string[] conn);

        #endregion

        #region ASN
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetASNHead(long poID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tASNHead GetAsnByAsnID(long asnID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetASNDetailByID(long asnID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetLoaderDetailsOfGRN(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tLoaderDetail GetLoaderDetailOfGRN(long ldrID, string[] conn);
        #endregion

        #region CreditNoteDebitNote
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetExcessQtyByGRNID(long grnID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetShortQtyByGRNID(long grnID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_GetCreditNoteDetailByPOID_Result> GetCreditNotePartDetailByPOID(long poID, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetVendorNameByID(long vendorID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalofCreditNote(long poID, long warehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveIntotCreditNoteHead(tCreditNoteHead CNHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int SaveCreditNoteDetail(long cnhID, long poID, long warehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet CheckCreditNote(long poID, string objectName, string[] conn);
        #endregion

        #region DebitNote
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void MakePOStatusGRN(long poID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_GetDebitNoteDetailByPOID_Result> GetDebitNotePartDetailByPOID(long poID, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet CheckDebitNote(long poID, string objectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveIntotDebitNoteHead(tDebitNoteHead DNHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int SaveDebitNoteDetail(long dnhID, long poID, long warehouseID, string[] conn);
        #endregion
    }
    //public class iWMS
    //{
    //}
}
