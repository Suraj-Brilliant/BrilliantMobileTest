using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data;


namespace Interface.PowerOnRent
{
    [ServiceContract]
    public partial interface iPartRequest
    {
        #region Part Request Head
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        PORtPartRequestHead GetRequestHeadByRequestID(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SetIntoPartRequestHead(PORtPartRequestHead PartRequest, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mStatu> GetStatusListForRequest(string Remark, string state, long UserID, string[] conn);
        #endregion

        #region Request Part Detail
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetPartDetail_ForRequest_Result> GetRequestPartDetailByRequestID(long RequestID, long siteID, string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDB(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBNEW(string paraSessionID, string paraUserID, string CurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetPartDetail_ForRequest_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetPartDetail_ForRequest_Result> AddPartIntoRequest_TempData(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long SiteID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetPartDetail_ForRequest_Result> RemovePartFromRequest_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData1(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSaveRequestPartDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, long PreviousStatusID, string[] conn);
        #endregion

        #region GridRequestList Summary
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayByUserID(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayByUserIDIssue(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayBySiteIDs(string SiteIDs, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestByRequestIDs_Result> GetRequestSummayByRequestIDs(string RequestIDs, string[] conn);
        #endregion

        #region Approval Code
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string SaveApprovalStatus(long RequestID, string ApprovalStatus, string ApprovalRemark, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tApprovalDetail GetApprovalDetailsByReqestID(long RequestID, string[] conn);
        #endregion

        #region GWC
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTemplateDetails(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTemplateDetailsSuperAdmin(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTemplateDetailsAdmin(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTemplateDetailsBind(long UserID, long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetGetInterfaceDetails(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetGetMessageDetails(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetApprovalDetailsNew(long OrderID, long ApproverID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetApprovalDetailsAllApproved(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetUOMofSelectedProduct(int ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayOfUser(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayOfUserIssue(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SetIntotOrderHead(tOrderHead PartRequest, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tOrderHead GetOrderHeadByOrderID(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long InsertIntomRequestTemplateHead(mRequestTemplateHead ReqTemplHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSavemRequestTemplateDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSavemRequestTemplateDetailTemplate(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mRequestTemplateHead GetTemplateOrderHead(long TemplateID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetTemplatePartLstByTemplateID(long TemplateID, string[] conn);

        #endregion

        #region GWC_Approval
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatetApprovalTransAfterApproval(long ApprovalID, long RequestID, long statusID, string Remark, long ApproverID, string InvoiceNo, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatetApprovalTransAfterReject(long ApprovalID, long RequestID, long statusID, string Remark, long ApproverID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int SetApproverDataafterSave(string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, long PreviousStatusID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetPreviousStatusID(long RequestId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetApprovalDetailsNewAdmin(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void InsertIntotCorrespond(tCorrespond Msg, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCorrespondance(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tCorrespond GetCorrespondanceDetail(long CORID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBomDetails(string PrdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetSelectedUom(long OrderId, long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetSelectedUomTemplate(long OrderId, long ProdID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GridRowCount(string paraSessionID, string paraCurrentObjectName, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<POR_SP_GetPartDetail_ForRequest_Result> GridRowsTemplate(string paraSessionID, string paraCurrentObjectName, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long ChkTemplateTitle(string TemplateTitle, string[] conn);

        #endregion

        #region GWC_Deliveries

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        VW_OrderDeliveryDetails GetOrderDeliveryDetails(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetDispatchedOrders(string SelectedOrder, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDriverDetails(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int AssignSelectedDriver(long orderNo, long DriverID, string TruckDetails, long AssignBy, string[] conn);
        #endregion

        #region GWCVer2
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDeptWisePaymentMethod(long selectedDept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetPaymentMethodFields(long SelpaymentMethod, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mPaymentMethodDetail> GetPMFields(long SelpaymentMethod, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_DeptWisePaymentMethod> GEtDeptPaymentmethod(long selectedDept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCostCenterMain> GetCostCenter(long DeptId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData12(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalQTYFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetMaxDeliveryDaysofDept(long Dept, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetMandatoryFields(long pm, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetStatutoryID(string PMLabel, long pmID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void AddIntotStatutory(tStatutoryDetail pmd, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetDeptPriceChange(long deptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetNewOrderNo(long StoreId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateStatutoryDetails(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int IsPriceChanged(int ProdID, decimal price, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSelectedCostCenter(long RequestId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetAddedAdditionalFields(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetPartAccessofUser(long requestID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetProductOfOrder(long OrderID, int Sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetOrderProductAccess(long requestID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int IsPriceEditYN(long OrderID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int IsSkuChangeYN(long OrderID, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetQtyofSelectedUOM(long SelectedProduct, long SelectedUOM, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int UpdateOrderQtyTotal(decimal OrderQty, decimal Price, decimal Total, long OrderID, int Sequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tOrderHead> GetOrderHeadByOrderIDQTYTotal(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CancelSelectedOrder(long SelectedOrder, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetUOMName(long UOMID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetCostCenterApproverID(long StatutoryValue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetFilteredDriverList(string SearchValue, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetInvoiceNoofOrder(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void RemoveFromTStatutory(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int EmailSendWhenRequestSubmit(long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void EmailSendofApproved(long ApproverID, long RequestID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetApproverDepartmentWise(long Deptid, string[] conn);
        #endregion
    }
}
