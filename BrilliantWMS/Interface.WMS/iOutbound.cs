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
    public partial interface iOutbound
    {
        #region Outbound

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindOutboundGrid(long userCompany, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindOutboundGridbyUser(long userID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int CheckSelectedSOStatusIsSameOrNot(string SelectedSO, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetNextSOObject(string SelectedRecords, string ObjectName, long CompanyID, string[] conn);
        #endregion
        #region SalesOrder
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetUserWarehouseSO(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetWarehouseNameByUserIDSO(long uid, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseMaster> GetAllWarehouseListSO(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mClient> GetCompanyWiseClient(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mClient GetClientNameByID(long ClientID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mClient GetClientDetailByClientName(string clientName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveNewClientDetails(mClient objCl, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBNEWSO(string paraSessionID, string paraUserID, string CurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mStatu> GetStatusListForOutbound(string ObjectName, string Remark, string state, long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int GetWorkflowSequenceOfSO(string ObjectName, long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForSO_Result> GetExistingTempDataBySessionIDObjectNameSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForSO_Result> AddPartIntoRequest_TempDataSO(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForSO_Result> GetRequestPartDetailByRequestIDSO(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tOrderHead GetOrderHeadByOrderIDSO(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForSO_Result> RemovePartFromRequest_TempDataSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData1SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempDataRtrn(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalFromTempDataSO(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalQTYFromTempDataSO(string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData12SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetTotalQTYofSequenceFromTempDataSO(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetPrdIDofSequenceFromTempDataSO(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdatePartRequest_TempData13SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SetIntotSalesOrderHead(tOrderHead SOHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSavSODetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tOrderHead GetSoHeadBySOID(long OrderID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet CheckSelectedSOJobCardNo(long SONumber, string ObjectName, string[] conn);
        #endregion
        #region PickList

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindPickUpGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_PickUpList_Result> GetPickUpList(string ODID, string TRID, string SessionID, string UserID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetSavedPickUpListBySOID(string pkUPNo, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBPickUp(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_PickUpList_Result> GetExistingTempDataBySessionIDObjectNamePickUp(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdatePickUPLstQtyofSelRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PickUpList_Result pkuplst, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdatePkupLstLocofSelRow(string SessionID, string paraObjectName, string paraUserID, WMS_SP_PickUpList_Result pkuplst, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetPickUpHead(tPickUpHead puHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSavePickUpDetail(long ODId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSOIDfromPkUpID(long pkupId, string[] conn);
        #endregion
        #region QC
        #endregion
        #region Dispatch

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindDispatchGrid(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tOrderHead GetSoDetailByQCID(long qcID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForDispatch_Result> GetDispatchPartByQCID(string qcID, string trID,string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBDispatch(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForDispatch_Result> GetExistingTempDataBySessionIDObjectNameDispatch(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdateDispatchData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForDispatch_Result Dispatch, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SavetDispatchHead(tDispatchHead dpHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSaveDispatchDetail(long qcID, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string DObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetDispatchDetails GetDispatchDetailsByQCID(long qcID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        WMS_VW_GetDispatchDetails GetDispatchDetailsByDispatchID(long dispatchID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDispatchSkuDetailByDispatchID(long dispatchID, string[] conn);
        #endregion

        #region Return 
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet BindOutboundGridForReturn(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int ChangeStatusToMarkForReturn(string SelectedRec,long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveReturnHead(tReturnHead rHead, string[] conn);
        #endregion

        #region Transfer
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForTransfer_Result> GetExistingTempDataBySessionIDObjectNameTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForTransfer_Result> AddPartIntoTransfer_TempDataTR(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDBTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForTransfer_Result> RemovePartFromTransfer_TempDataTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<WMS_SP_GetPartDetail_ForTransfer_Result> GetTransferPartDetailByTransferID(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateTransfer_TempDataTR(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForTransfer_Result Request, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveIntotTransferHead(tTransferHead TRHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int FinalSaveTRDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tTransferHead GetTransferHeadDetailByTransferID(long TRID, string[] conn);
        #endregion

        #region Report
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mCustomer> GetCustomerByCompanyID(long CompanyID, string[] conn);
        #endregion
    }
}
