using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.DocumentService;
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.WMS
{
    public partial class DispatchDetails : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string state = "";
        static string ObjectName = "Dispatch";
        #region PageEvents
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["QCID"] != null)
                {
                    if (Session["QCID"].ToString() != "0" && Session["QCstate"].ToString() == "View")
                    {
                        state = "View";
                        GetDispatchDetails(Session["QCID"].ToString());
                    }
                    else if (Session["QCID"].ToString() != "0" && Session["QCstate"].ToString() == "Edit")
                    {
                        state = "Edit";
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCDispatchDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        UCExpDeliveryDate.Date = DateTime.Now;
                        if (Session["QCID"].ToString() != "0")
                        {
                            DisplaySOData();
                        }
                    }
                }
                else if (Session["DispID"] != null)
                {
                    if (Session["DispID"].ToString() != "0" && Session["Dispstate"].ToString() == "View")
                    {
                        state = "View";
                        GetDispatchDetails(Session["DispID"].ToString());
                    }
                }
                else if (Session["TRID"] != null)
                {
                    if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "View")
                    {
                        state = "View";
                        GetDispatchDetails(Session["TRID"].ToString());
                    }
                    else if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "Edit")
                    {
                        state = "Edit";
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCDispatchDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        UCExpDeliveryDate.Date = DateTime.Now;
                        if (Session["TRID"].ToString() != "0")
                        {
                            DisplaySOData();
                        }
                    }
                }
            }
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetEditRight(false, "Not Allowed");
            if (state != "View") { Toolbar1.SetSaveRight(true, "Not Allowed"); }
            else { Toolbar1.SetSaveRight(false, "Not Allowed"); }
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        [WebMethod]
        public static long WMSaveDispatchHead(object objDispatch)
        {
            long result = 0;
            int RSLT = 0; long DispatchID = 0;
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.tDispatchHead dh = new WMSOutbound.tDispatchHead();
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objDispatch;
                if (HttpContext.Current.Session["QCID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "Dispatch", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "Dispatch", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpQCID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotQC = grpQCID.Split(',');
                            int QCCnt = TotQC.Count();
                            for (int p = 0; p <= QCCnt - 1; p++)
                            {
                                dh.CreatedBy = profile.Personal.UserID;
                                dh.CreationDate = DateTime.Now;
                                dh.OID = long.Parse(TotQC[p].ToString());
                                dh.DispatchDate = Convert.ToDateTime(d["DispatchDate"]);
                                dh.DispatchBy = Convert.ToInt64(d["DispatchBy"].ToString());
                                dh.Remark = d["Remark"].ToString();
                                dh.AirwayBill = d["AirwayBill"].ToString();
                                dh.ShippingType = d["ShippingType"].ToString();
                                dh.CompanyID = profile.Personal.CompanyID;
                                dh.Status = Convert.ToInt64(d["Status"].ToString());
                                dh.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                                dh.TransporterName = d["TransporterName"].ToString();
                                dh.TransporterRemark = d["TransporterRemark"].ToString();
                                dh.ExpDeliveryDate = Convert.ToDateTime(d["ExpDeliveryDate"].ToString());
                                dh.ObjectName = "SalesOrder";

                                DispatchID = Outbound.SavetDispatchHead(dh, profile.DBConnection._constr);

                                if (DispatchID > 0)
                                {
                                    RSLT = Outbound.FinalSaveDispatchDetail(long.Parse(TotQC[p].ToString()), HttpContext.Current.Session.SessionID, ObjectName, DispatchID, profile.Personal.UserID.ToString(), Convert.ToInt16(dh.Status), "SalesOrder", profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = DispatchID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, DispatchID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Outbound.ClearTempDataFromDBDispatch(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        dh.CreatedBy = profile.Personal.UserID;
                        dh.CreationDate = DateTime.Now;
                        dh.OID = long.Parse(HttpContext.Current.Session["QCID"].ToString());
                        dh.DispatchDate = Convert.ToDateTime(d["DispatchDate"]);
                        dh.DispatchBy = Convert.ToInt64(d["DispatchBy"].ToString());
                        dh.Remark = d["Remark"].ToString();
                        dh.AirwayBill = d["AirwayBill"].ToString();
                        dh.ShippingType = d["ShippingType"].ToString();
                        dh.CompanyID = profile.Personal.CompanyID;
                        dh.Status = Convert.ToInt64(d["Status"].ToString());
                        dh.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                        dh.TransporterName = d["TransporterName"].ToString();
                        dh.TransporterRemark = d["TransporterRemark"].ToString();
                        dh.ExpDeliveryDate = Convert.ToDateTime(d["ExpDeliveryDate"].ToString());
                        dh.ObjectName = "SalesOrder";

                        DispatchID = Outbound.SavetDispatchHead(dh, profile.DBConnection._constr);

                        if (DispatchID > 0)
                        {
                            RSLT = Outbound.FinalSaveDispatchDetail(long.Parse(HttpContext.Current.Session["QCID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, DispatchID, profile.Personal.UserID.ToString(), Convert.ToInt16(dh.Status), "SalesOrder",profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = DispatchID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, DispatchID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                        Outbound.ClearTempDataFromDBDispatch(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
                else if (HttpContext.Current.Session["TRID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpQCID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotQC = grpQCID.Split(',');
                            int QCCnt = TotQC.Count();
                            for (int p = 0; p <= QCCnt - 1; p++)
                            {
                                dh.CreatedBy = profile.Personal.UserID;
                                dh.CreationDate = DateTime.Now;
                                dh.OID = long.Parse(TotQC[p].ToString());
                                dh.DispatchDate = Convert.ToDateTime(d["DispatchDate"]);
                                dh.DispatchBy = Convert.ToInt64(d["DispatchBy"].ToString());
                                dh.Remark = d["Remark"].ToString();
                                dh.AirwayBill = d["AirwayBill"].ToString();
                                dh.ShippingType = d["ShippingType"].ToString();
                                dh.CompanyID = profile.Personal.CompanyID;
                                dh.Status = Convert.ToInt64(d["Status"].ToString());
                                dh.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                                dh.TransporterName = d["TransporterName"].ToString();
                                dh.TransporterRemark = d["TransporterRemark"].ToString();
                                dh.ExpDeliveryDate = Convert.ToDateTime(d["ExpDeliveryDate"].ToString());
                                dh.ObjectName = "Transfer";

                                DispatchID = Outbound.SavetDispatchHead(dh, profile.DBConnection._constr);

                                if (DispatchID > 0)
                                {
                                    RSLT = Outbound.FinalSaveDispatchDetail(long.Parse(TotQC[p].ToString()), HttpContext.Current.Session.SessionID, ObjectName, DispatchID, profile.Personal.UserID.ToString(), Convert.ToInt16(dh.Status), "Transfer",profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = DispatchID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, DispatchID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Outbound.ClearTempDataFromDBDispatch(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        dh.CreatedBy = profile.Personal.UserID;
                        dh.CreationDate = DateTime.Now;
                        dh.OID = long.Parse(HttpContext.Current.Session["TRID"].ToString());
                        dh.DispatchDate = Convert.ToDateTime(d["DispatchDate"]);
                        dh.DispatchBy = Convert.ToInt64(d["DispatchBy"].ToString());
                        dh.Remark = d["Remark"].ToString();
                        dh.AirwayBill = d["AirwayBill"].ToString();
                        dh.ShippingType = d["ShippingType"].ToString();
                        dh.CompanyID = profile.Personal.CompanyID;
                        dh.Status = Convert.ToInt64(d["Status"].ToString());
                        dh.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                        dh.TransporterName = d["TransporterName"].ToString();
                        dh.TransporterRemark = d["TransporterRemark"].ToString();
                        dh.ExpDeliveryDate = Convert.ToDateTime(d["ExpDeliveryDate"].ToString());
                        dh.ObjectName = "Transfer";

                        DispatchID = Outbound.SavetDispatchHead(dh, profile.DBConnection._constr);

                        if (DispatchID > 0)
                        {
                            RSLT = Outbound.FinalSaveDispatchDetail(long.Parse(HttpContext.Current.Session["TRID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, DispatchID, profile.Personal.UserID.ToString(), Convert.ToInt16(dh.Status), "Transfer",profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = DispatchID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, DispatchID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                        Outbound.ClearTempDataFromDBDispatch(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "DispatchDetail.aspx", "WMSaveDispatchHead"); result = 0; }
            finally { Outbound.Close(); }
            return result;
        }

        public void GetDispatchDetails(string QCID)
        {
            long selQCID = 0;
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                if (Session["QCID"] != null)
                {
                    selQCID = long.Parse(QCID.ToString());
                    BrilliantWMS.WMSOutbound.WMS_VW_GetDispatchDetails dispatchLst = new WMSOutbound.WMS_VW_GetDispatchDetails();
                    dispatchLst = Outbound.GetDispatchDetailsByQCID(selQCID, profile.DBConnection._constr);

                    lblDispatchNo.Text = dispatchLst.ID.ToString();
                    UCDispatchDate.Date = dispatchLst.DispatchDate;
                    txtRemark.Text = dispatchLst.Remark.ToString();
                    txtAirwayBill.Text = dispatchLst.AirwayBill.ToString();
                    txtShippingType.Text = dispatchLst.ShippingType.ToString();
                    UC_ShippingDate.Date = dispatchLst.ShippingDate;
                    UCExpDeliveryDate.Date = dispatchLst.ExpDeliveryDate;
                    txtTransporterName.Text = dispatchLst.TransporterName.ToString();
                    txtTransporterRemark.Text = dispatchLst.TransporterRemark.ToString();
                    long WarehouseID=long.Parse(dispatchLst.StoreId.ToString());

                    UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                    UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                    vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                    UsersList.Insert(0, select);
                    ddlDispatchBy.DataSource = UsersList;
                    ddlDispatchBy.DataBind();
                    ddlDispatchBy.SelectedIndex = ddlDispatchBy.Items.IndexOf(ddlDispatchBy.Items.FindByValue(dispatchLst.DispatchBy.ToString()));

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dispatchLst.Status.ToString()));

                    Grid1.DataSource =Outbound.GetDispatchSkuDetailByDispatchID(long.Parse(dispatchLst.ID.ToString()), profile.DBConnection._constr);
                    Grid1.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvDDetail');LoadingOff();", true);

                }
                else if (Session["DispID"] != null)
                {
                    selQCID = long.Parse(QCID.ToString());
                    BrilliantWMS.WMSOutbound.WMS_VW_GetDispatchDetails dispatchLst = new WMSOutbound.WMS_VW_GetDispatchDetails();
                    dispatchLst = Outbound.GetDispatchDetailsByDispatchID(selQCID, profile.DBConnection._constr);
                    lblDispatchNo.Text = dispatchLst.ID.ToString();
                    UCDispatchDate.Date = dispatchLst.DispatchDate;
                    txtRemark.Text = dispatchLst.Remark.ToString();
                    txtAirwayBill.Text = dispatchLst.AirwayBill.ToString();
                    txtShippingType.Text = dispatchLst.ShippingType.ToString();
                    UC_ShippingDate.Date = dispatchLst.ShippingDate;
                    UCExpDeliveryDate.Date = dispatchLst.ExpDeliveryDate;
                    txtTransporterName.Text = dispatchLst.TransporterName.ToString();
                    txtTransporterRemark.Text = dispatchLst.TransporterRemark.ToString();
                    long WarehouseID = long.Parse(dispatchLst.StoreId.ToString());

                    UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                    UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                    vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                    UsersList.Insert(0, select);
                    ddlDispatchBy.DataSource = UsersList;
                    ddlDispatchBy.DataBind();
                    ddlDispatchBy.SelectedIndex = ddlDispatchBy.Items.IndexOf(ddlDispatchBy.Items.FindByValue(dispatchLst.DispatchBy.ToString()));

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dispatchLst.Status.ToString()));

                    Grid1.DataSource = Outbound.GetDispatchSkuDetailByDispatchID(long.Parse(dispatchLst.ID.ToString()), profile.DBConnection._constr);
                    Grid1.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvDDetail');LoadingOff();", true);
                }
                else if (Session["TRID"] != null)
                {
                    selQCID = long.Parse(QCID.ToString());
                    BrilliantWMS.WMSOutbound.WMS_VW_GetDispatchDetails dispatchLst = new WMSOutbound.WMS_VW_GetDispatchDetails();
                    dispatchLst = Outbound.GetDispatchDetailsByQCID(selQCID, profile.DBConnection._constr);

                    lblDispatchNo.Text = dispatchLst.ID.ToString();
                    UCDispatchDate.Date = dispatchLst.DispatchDate;
                    txtRemark.Text = dispatchLst.Remark.ToString();
                    txtAirwayBill.Text = dispatchLst.AirwayBill.ToString();
                    txtShippingType.Text = dispatchLst.ShippingType.ToString();
                    UC_ShippingDate.Date = dispatchLst.ShippingDate;
                    UCExpDeliveryDate.Date = dispatchLst.ExpDeliveryDate;
                    txtTransporterName.Text = dispatchLst.TransporterName.ToString();
                    txtTransporterRemark.Text = dispatchLst.TransporterRemark.ToString();
                    long WarehouseID = long.Parse(dispatchLst.StoreId.ToString());

                    UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                    UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                    vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                    UsersList.Insert(0, select);
                    ddlDispatchBy.DataSource = UsersList;
                    ddlDispatchBy.DataBind();
                    ddlDispatchBy.SelectedIndex = ddlDispatchBy.Items.IndexOf(ddlDispatchBy.Items.FindByValue(dispatchLst.DispatchBy.ToString()));

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dispatchLst.Status.ToString()));

                    Grid1.DataSource = Outbound.GetDispatchSkuDetailByDispatchID(long.Parse(dispatchLst.ID.ToString()), profile.DBConnection._constr);
                    Grid1.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvDDetail');LoadingOff();", true);
                }

            }
            catch { }
            finally { Outbound.Close(); objService.Close(); UsersList.Clear(); }
        }
        #endregion

        #region BindDropDown
        public void fillUser()
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.tOrderHead OH = new WMSOutbound.tOrderHead();
                long WarehouseID = 0;
                if (Session["QCID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "SalesOrder", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "SalesOrder", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            //lblSelectedPO.Text = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            //lblSelectedGTN.Text = dsJCN.Tables[0].Rows[0]["JobCardName"].ToString();
                            //lblPurchaseOrderDate.Text = dsJCN.Tables[0].Rows[0]["CreationDate"].ToString();
                            //lblPOBy.Text = dsJCN.Tables[0].Rows[0]["CreatedByUser"].ToString();
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                        }
                    }
                    else
                    {
                        OH = Outbound.GetSoDetailByQCID(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), profile.DBConnection._constr);
                        //lblSelectedPO.Text = Session["POID"].ToString();
                        //lblSelectedGTN.Text = "Not Created";
                        //lblPurchaseOrderDate.Text = POHead.POdate.Value.ToString("dd-MMM-yyyy");
                        //lblPOBy.Text = Inbound.GetUserNameByID(long.Parse(POHead.CreatedBy.ToString()), profile.DBConnection._constr);
                        WarehouseID = long.Parse(OH.StoreId.ToString());
                    }
                }
                else if (Session["TRID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {                           
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                        }
                    }
                    else
                    {
                        BrilliantWMS.WMSInbound.tTransferHead TRHead = new WMSInbound.tTransferHead();                        
                        TRHead = Inbound.GetTransferHeadByTRID(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), profile.DBConnection._constr);                                                
                        WarehouseID = long.Parse(TRHead.FromPosition.ToString());
                    }
                }
                UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);

                ddlDispatchBy.DataSource = UsersList;
                ddlDispatchBy.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        public static List<BrilliantWMS.WMSInbound.mStatu> WMFillStatus()
        {
            string orderstate = state;
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            List<BrilliantWMS.WMSInbound.mStatu> StatusList = new List<BrilliantWMS.WMSInbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (orderstate != "View")
                {
                    if (HttpContext.Current.Session["TRID"] != null)
                    {
                        StatusList = Inbound.GetStatusListForInbound("TransferDispatch", "", orderstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                    else
                    {
                        StatusList = Inbound.GetStatusListForInbound(ObjectName, "", orderstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                }
                else
                {
                    StatusList = Inbound.GetStatusListForInbound("", "SalesOrder,POSO", orderstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                }

                BrilliantWMS.WMSInbound.mStatu select = new BrilliantWMS.WMSInbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "DispatchDetails.aspx", "WMFillStatus");
            }
            finally { Inbound.Close(); }
            return StatusList;
        }

        #endregion

        #region SO
        public void DisplaySOData()
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            List<BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForDispatch_Result> QCDetail = new List<WMSOutbound.WMS_SP_GetPartDetail_ForDispatch_Result>();
            try
            {                
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["QCID"] != null)
                {
                    long QCID = long.Parse(Session["QCID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "Dispatch", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "Dispatch", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpQCID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            QCDetail = Outbound.GetDispatchPartByQCID(grpQCID, "0",Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        QCDetail = Outbound.GetDispatchPartByQCID(QCID.ToString(), "0",Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if(Session["TRID"]!=null)
                {
                    long TRID = long.Parse(Session["TRID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpQCID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            QCDetail = Outbound.GetDispatchPartByQCID("0",grpQCID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        QCDetail = Outbound.GetDispatchPartByQCID("0",TRID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = QCDetail;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "DispatchDetail.aspx", "DisplayPOData");
            }
            finally { Outbound.Close(); Inbound.Close(); }
        }


        #endregion
    }
}