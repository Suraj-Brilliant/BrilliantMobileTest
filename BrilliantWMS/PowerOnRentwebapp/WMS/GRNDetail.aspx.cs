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
using BrilliantWMS.WMSInbound;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.WMS
{
    public partial class GRNDetail : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string state = "";
        static string ObjectName = "GRN";

        #region PageEvents
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["POID"] != null)
                {
                    if (Session["POID"].ToString() != "0" && Session["POstate"].ToString() == "View")
                    {
                        state = "View";
                        GetGRNDetails(Session["POID"].ToString());
                    }
                    else if (Session["POID"].ToString() != "0" && Session["POstate"].ToString() == "Edit")
                    {
                        state = "Edit";
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCGRNDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        if (Session["POID"].ToString() != "0")
                        {
                            DisplayPOData();
                        }
                    }
                }
                else if (Session["GRNID"] != null)
                {
                    if (Session["GRNID"].ToString() != "0" && Session["GRNstate"].ToString() == "View")
                    {
                        state = "View";
                        GetGRNDetails(Session["GRNID"].ToString());
                    }
                }
                else if (Session["SOID"] != null)
                {
                    if (Session["SOID"].ToString() != "0" && Session["SOstate"].ToString() == "View")
                    {
                        state = "View";
                        GetGRNDetails(Session["SOID"].ToString());
                    }
                    else if (Session["SOID"].ToString() != "0" && Session["SOstate"].ToString() == "Edit")
                    {
                        state = "Edit";
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCGRNDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        if (Session["SOID"].ToString() != "0")
                        {
                            DisplayPOData();
                        }
                    }
                }
                else if (Session["TRID"] != null)
                {
                    if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "View")
                    {
                        state = "View";
                        GetGRNDetails(Session["TRID"].ToString());
                    }
                    else if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "Edit")
                    {
                        state = "Edit";
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCGRNDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        if (Session["TRID"].ToString() != "0")
                        {
                            DisplayPOData();
                        }
                    }
                }
            }
            /*Temparary Change*/
            divLoaderDetails.Attributes.Add("style", "display:none");
            divLoaderHead.Attributes.Add("style", "display:none");
            /*Temparary Change*/
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetEditRight(false, "Not Allowed");
            if (state != "View") { Toolbar1.SetSaveRight(true, "Not Allowed"); }
            else { Toolbar1.SetSaveRight(false, "Not Allowed"); }
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        [WebMethod]
        public static long WMSaveGRNHead(object objGRN)
        {
            long result = 0;
            int RSLT = 0; long GRNID = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tGRNHead GRNHead = new tGRNHead();
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objGRN;
                if (HttpContext.Current.Session["POID"] != null)
                {
                     int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder",profile.DBConnection._constr);
                     if (chkJObCart >= 1)
                     {
                         DataSet dsJCN = new DataSet();
                         dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder",profile.DBConnection._constr);
                         if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                         {
                             string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                             string[] TotPO = grpPOID.Split(',');
                             int POCnt = TotPO.Count();
                             for (int p = 0; p <= POCnt - 1; p++)
                             {
                                 GRNHead.CreatedBy = profile.Personal.UserID;
                                 GRNHead.Creationdate = DateTime.Now;
                                 GRNHead.ObjectName = "PurchaseOrder";
                                 GRNHead.OID = long.Parse(TotPO[p].ToString());
                                 GRNHead.ShipID = d["ShipID"].ToString();
                                 GRNHead.GRNDate = Convert.ToDateTime(d["GRNDate"]);
                                 GRNHead.ReceivedBy = Convert.ToInt64(d["ReceivedBy"].ToString());
                                 GRNHead.BatchNo = d["BatchNo"].ToString();
                                 GRNHead.Remark = d["Remark"].ToString();
                                 GRNHead.AirwayBill = d["AirwayBill"].ToString();
                                 GRNHead.ShippingType = d["ShippingType"].ToString();
                                 GRNHead.CompanyID = profile.Personal.CompanyID;
                                 GRNHead.Status = Convert.ToInt64(d["Status"].ToString());
                                 GRNHead.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                                 GRNHead.TransporterName = d["TransporterName"].ToString();
                                 GRNHead.TransporterRemark = d["TransporterRemark"].ToString();

                                 GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);

                                 if (GRNID > 0)
                                 {
                                     RSLT = Inbound.FinalSaveGRNDetail(long.Parse(TotPO[p].ToString()), HttpContext.Current.Session.SessionID, ObjectName, GRNID, profile.Personal.UserID.ToString(), Convert.ToInt16(GRNHead.Status), profile.DBConnection._constr);
                                     if (RSLT == 1 || RSLT == 2) { result = GRNID; }
                                     else if (RSLT == 0) { result = 0; }
                                     iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                     DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, GRNID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                 }
                             }
                             Inbound.ClearTempDataFromDBGRN(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                         }
                     }
                     else
                     {
                         GRNHead.CreatedBy = profile.Personal.UserID;
                         GRNHead.Creationdate = DateTime.Now;

                         GRNHead.ObjectName = "PurchaseOrder";
                         GRNHead.OID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                         GRNHead.ShipID = d["ShipID"].ToString();
                         GRNHead.GRNDate = Convert.ToDateTime(d["GRNDate"]);
                         GRNHead.ReceivedBy = Convert.ToInt64(d["ReceivedBy"].ToString());
                         GRNHead.BatchNo = d["BatchNo"].ToString();
                         GRNHead.Remark = d["Remark"].ToString();
                         GRNHead.AirwayBill = d["AirwayBill"].ToString();
                         GRNHead.ShippingType = d["ShippingType"].ToString();
                         GRNHead.CompanyID = profile.Personal.CompanyID;
                         GRNHead.Status = Convert.ToInt64(d["Status"].ToString());
                         GRNHead.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                         GRNHead.TransporterName = d["TransporterName"].ToString();
                         GRNHead.TransporterRemark = d["TransporterRemark"].ToString();

                         GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);

                         if (GRNID > 0)
                         {//Pass POID for SAve Product
                             RSLT = Inbound.FinalSaveGRNDetail(long.Parse(HttpContext.Current.Session["POID"].ToString()),HttpContext.Current.Session.SessionID, ObjectName, GRNID, profile.Personal.UserID.ToString(), Convert.ToInt16(GRNHead.Status), profile.DBConnection._constr);
                             if (RSLT == 1 || RSLT == 2) { result = GRNID; }
                             else if (RSLT == 0) { result = 0; }
                             iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                             DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, GRNID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                         }

                         Inbound.ClearTempDataFromDBGRN(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                     }
                }
                else if (HttpContext.Current.Session["SOID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotPO = grpPOID.Split(',');
                            int POCnt = TotPO.Count();
                            for (int p = 0; p <= POCnt - 1; p++)
                            {
                                GRNHead.CreatedBy = profile.Personal.UserID;
                                GRNHead.Creationdate = DateTime.Now;
                                GRNHead.ObjectName = "SalesReturn";  /**/
                                GRNHead.OID = long.Parse(TotPO[p].ToString());
                                GRNHead.ShipID = d["ShipID"].ToString();
                                GRNHead.GRNDate = Convert.ToDateTime(d["GRNDate"]);
                                GRNHead.ReceivedBy = Convert.ToInt64(d["ReceivedBy"].ToString());
                                GRNHead.BatchNo = d["BatchNo"].ToString();
                                GRNHead.Remark = d["Remark"].ToString();
                                GRNHead.AirwayBill = d["AirwayBill"].ToString();
                                GRNHead.ShippingType = d["ShippingType"].ToString();
                                GRNHead.CompanyID = profile.Personal.CompanyID;
                                GRNHead.Status = Convert.ToInt64(d["Status"].ToString());
                                GRNHead.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                                GRNHead.TransporterName = d["TransporterName"].ToString();
                                GRNHead.TransporterRemark = d["TransporterRemark"].ToString();

                                GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);

                                if (GRNID > 0)
                                {
                                    RSLT = Inbound.FinalSaveGRNDetail(long.Parse(TotPO[p].ToString()), HttpContext.Current.Session.SessionID, ObjectName, GRNID, profile.Personal.UserID.ToString(), Convert.ToInt16(GRNHead.Status), profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = GRNID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, GRNID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Inbound.ClearTempDataFromDBGRN(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        GRNHead.CreatedBy = profile.Personal.UserID;
                        GRNHead.Creationdate = DateTime.Now;

                        GRNHead.ObjectName = "SalesReturn"; /**/
                        GRNHead.OID = long.Parse(HttpContext.Current.Session["SOID"].ToString());
                        GRNHead.ShipID = d["ShipID"].ToString();
                        GRNHead.GRNDate = Convert.ToDateTime(d["GRNDate"]);
                        GRNHead.ReceivedBy = Convert.ToInt64(d["ReceivedBy"].ToString());
                        GRNHead.BatchNo = d["BatchNo"].ToString();
                        GRNHead.Remark = d["Remark"].ToString();
                        GRNHead.AirwayBill = d["AirwayBill"].ToString();
                        GRNHead.ShippingType = d["ShippingType"].ToString();
                        GRNHead.CompanyID = profile.Personal.CompanyID;
                        GRNHead.Status = Convert.ToInt64(d["Status"].ToString());
                        GRNHead.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                        GRNHead.TransporterName = d["TransporterName"].ToString();
                        GRNHead.TransporterRemark = d["TransporterRemark"].ToString();

                        GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);

                        if (GRNID > 0)
                        {//Pass POID for SAve Product
                            RSLT = Inbound.FinalSaveGRNDetail(long.Parse(HttpContext.Current.Session["SOID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, GRNID, profile.Personal.UserID.ToString(), Convert.ToInt16(GRNHead.Status), profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = GRNID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, GRNID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }

                        Inbound.ClearTempDataFromDBGRN(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
                else if (HttpContext.Current.Session["TRID"] != null)
                {
                    GRNHead.CreatedBy = profile.Personal.UserID;
                    GRNHead.Creationdate = DateTime.Now;

                    GRNHead.ObjectName = "Transfer"; /**/
                    GRNHead.OID = long.Parse(HttpContext.Current.Session["TRID"].ToString());
                    GRNHead.ShipID = d["ShipID"].ToString();
                    GRNHead.GRNDate = Convert.ToDateTime(d["GRNDate"]);
                    GRNHead.ReceivedBy = Convert.ToInt64(d["ReceivedBy"].ToString());
                    GRNHead.BatchNo = d["BatchNo"].ToString();
                    GRNHead.Remark = d["Remark"].ToString();
                    GRNHead.AirwayBill = d["AirwayBill"].ToString();
                    GRNHead.ShippingType = d["ShippingType"].ToString();
                    GRNHead.CompanyID = profile.Personal.CompanyID;
                    GRNHead.Status = Convert.ToInt64(d["Status"].ToString());
                    GRNHead.ShippingDate = Convert.ToDateTime(d["ShippingDate"].ToString());
                    GRNHead.TransporterName = d["TransporterName"].ToString();
                    GRNHead.TransporterRemark = d["TransporterRemark"].ToString();

                    GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);

                    if (GRNID > 0)
                    {
                        RSLT = Inbound.FinalSaveGRNDetail(long.Parse(HttpContext.Current.Session["TRID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, GRNID, profile.Personal.UserID.ToString(), Convert.ToInt16(GRNHead.Status), profile.DBConnection._constr);
                        if (RSLT == 1 || RSLT == 2) { result = GRNID; }
                        else if (RSLT == 0) { result = 0; }
                        iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                        DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, GRNID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                    }

                    Inbound.ClearTempDataFromDBGRN(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMSaveRequestHead"); result = 0; }
            finally { Inbound.Close(); }
            return result;
        }

        public void GetGRNDetails(string OID)
        {
            long POID = 0;
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                if (Session["POID"] != null)
                {
                    POID = long.Parse(OID.ToString());

                    int TotGRN = Inbound.GetTotalGRNPOWise(POID, profile.DBConnection._constr);
                    if (TotGRN > 1)
                    {
                        Response.Redirect("../WMS/GridGRN.aspx?POID="+ POID +"");
                    }
                    else if (TotGRN == 1)
                    {
                        WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
                        GrnLst = Inbound.GetGRNDetailsByGRNID(POID, "PurchaseOrder",profile.DBConnection._constr);

                        lblSelectedPO.Text = GrnLst.OID.ToString();
                        lblPurchaseOrderDate.Text = GrnLst.POdate.Value.ToString("dd-MMM-yyyy");
                        lblPOBy.Text = GrnLst.POCreatedBy.ToString();
                        lblGRNNumber.Text = GrnLst.ID.ToString();
                        UCGRNDate.Date = GrnLst.GRNDate;

                        long WarehouseID = 0;
                        tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                        POHead = Inbound.GetPoHeadByPOID(POID, profile.DBConnection._constr);
                        WarehouseID = long.Parse(POHead.Warehouse.ToString());
                        UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                        UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                        vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                        UsersList.Insert(0, select);
                        ddlGRNBy.DataSource = UsersList;
                        ddlGRNBy.DataBind();
                        ddlGRNBy.SelectedIndex = ddlGRNBy.Items.IndexOf(ddlGRNBy.Items.FindByValue(GrnLst.CreatedBy.ToString()));

                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(GrnLst.Status.ToString()));

                        txtBatchNo.Text = GrnLst.BatchNo.ToString();
                        txtShippingNo.Text = GrnLst.ShipID.ToString();
                        txtRemark.Text = GrnLst.Remark.ToString();
                        txtAirwayBill.Text = GrnLst.AirwayBill.ToString();
                        txtShippingType.Text = GrnLst.ShippingType.ToString();
                        UC_ShippingDate.Date = GrnLst.ShippingDate;
                        txtTransporterName.Text = GrnLst.TransporterName.ToString();
                        txtTransporterRemark.Text = GrnLst.TransporterRemark.ToString();
                        Grid1.DataSource = Inbound.GetGrnSkuDetailsbyGRNID(long.Parse(GrnLst.ID.ToString()), profile.DBConnection._constr);
                        Grid1.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvGRNDetail');LoadingOff();", true);
                    }
                }
                else if (Session["GRNID"] != null)
                {
                    POID = Inbound.GetPOIDFromGRNID(long.Parse(Session["GRNID"].ToString()), profile.DBConnection._constr);

                    WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
                    long GRNID = long.Parse(Session["GRNID"].ToString());
                    GrnLst = Inbound.GetGRNDetailsByGRNIDGRNMenu(GRNID, profile.DBConnection._constr);

                    lblSelectedPO.Text = GrnLst.OID.ToString();
                    lblPurchaseOrderDate.Text = GrnLst.POdate.Value.ToString("dd-MMM-yyyy");
                    lblPOBy.Text = GrnLst.POCreatedBy.ToString();
                    lblGRNNumber.Text = GrnLst.ID.ToString();
                    UCGRNDate.Date = GrnLst.GRNDate;

                    long WarehouseID = 0;
                    tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                    POHead = Inbound.GetPoHeadByPOID(POID, profile.DBConnection._constr);
                    WarehouseID = long.Parse(POHead.Warehouse.ToString());
                    UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                    UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                    vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                    UsersList.Insert(0, select);
                    ddlGRNBy.DataSource = UsersList;
                    ddlGRNBy.DataBind();
                    ddlGRNBy.SelectedIndex = ddlGRNBy.Items.IndexOf(ddlGRNBy.Items.FindByValue(GrnLst.CreatedBy.ToString()));

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(GrnLst.Status.ToString()));

                    txtBatchNo.Text = GrnLst.BatchNo.ToString();
                    txtShippingNo.Text = GrnLst.ShipID.ToString();
                    txtRemark.Text = GrnLst.Remark.ToString();
                    txtAirwayBill.Text = GrnLst.AirwayBill.ToString();
                    txtShippingType.Text = GrnLst.ShippingType.ToString();
                    UC_ShippingDate.Date = GrnLst.ShippingDate;
                    txtTransporterName.Text = GrnLst.TransporterName.ToString();
                    txtTransporterRemark.Text = GrnLst.TransporterRemark.ToString();
                    Grid1.DataSource = Inbound.GetGrnSkuDetailsbyGRNID(long.Parse(GrnLst.ID.ToString()), profile.DBConnection._constr);
                    Grid1.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvGRNDetail');LoadingOff();", true);
                }   
                else if(Session["SOID"]!=null)
                {
                    POID = long.Parse(OID.ToString());
                    int TotGRN = Inbound.GetTotalGRNPOWise(POID, profile.DBConnection._constr);
                    if (TotGRN > 1)
                    {
                        Response.Redirect("../WMS/GridGRN.aspx?POID="+ POID +"");
                    }
                    else if (TotGRN == 1)
                    {
                        WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
                        GrnLst = Inbound.GetGRNDetailsByGRNID(POID,"SalesReturn", profile.DBConnection._constr);
                        
                        lblSelectedPO.Text = GrnLst.OID.ToString();
                        lblPurchaseOrderDate.Text = GrnLst.POdate.Value.ToString("dd-MMM-yyyy");
                        lblPOBy.Text = GrnLst.POCreatedBy.ToString();
                        lblGRNNumber.Text = GrnLst.ID.ToString();
                        UCGRNDate.Date = GrnLst.GRNDate;

                         long WarehouseID = 0;
                        tOrderHead Ohead = new tOrderHead ();                        
                        Ohead = Inbound.GetSoHeadBySOIDForSalesReturn(POID, profile.DBConnection._constr);
                        WarehouseID = long.Parse(Ohead.StoreId.ToString());
                        UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                        UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                        vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                        UsersList.Insert(0, select);
                        ddlGRNBy.DataSource = UsersList;
                        ddlGRNBy.DataBind();
                        ddlGRNBy.SelectedIndex = ddlGRNBy.Items.IndexOf(ddlGRNBy.Items.FindByValue(GrnLst.CreatedBy.ToString()));

                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(GrnLst.Status.ToString()));

                        txtBatchNo.Text = GrnLst.BatchNo.ToString();
                        txtShippingNo.Text = GrnLst.ShipID.ToString();
                        txtRemark.Text = GrnLst.Remark.ToString();
                        txtAirwayBill.Text = GrnLst.AirwayBill.ToString();
                        txtShippingType.Text = GrnLst.ShippingType.ToString();
                        UC_ShippingDate.Date = GrnLst.ShippingDate;
                        txtTransporterName.Text = GrnLst.TransporterName.ToString();
                        txtTransporterRemark.Text = GrnLst.TransporterRemark.ToString();
                        Grid1.DataSource = Inbound.GetGrnSkuDetailsbyGRNID(long.Parse(GrnLst.ID.ToString()), profile.DBConnection._constr);
                        Grid1.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvGRNDetail');LoadingOff();", true);
                    }
                }
                else if(Session["TRID"]!=null)
                {
                    POID = long.Parse(OID.ToString());
                    WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
                    GrnLst = Inbound.GetGRNDetailsByGRNID(POID, "Transfer", profile.DBConnection._constr);
                    lblPONumbers.Text = "Transfer Number ";
                    lblSelectedPO.Text = GrnLst.OID.ToString();
                    lblPurchaseOrderDate.Text = GrnLst.POdate.Value.ToString("dd-MMM-yyyy");
                    lblPOBy.Text = GrnLst.POCreatedBy.ToString();
                    lblGRNNumber.Text = GrnLst.ID.ToString();
                    UCGRNDate.Date = GrnLst.GRNDate;

                    long WarehouseID = 0;
                    tTransferHead TH = new tTransferHead();
                    TH = Inbound.GetTransferHeadByTRID(POID, profile.DBConnection._constr);
                    WarehouseID = long.Parse(TH.ToPosition.ToString());
                    UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                    UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                    vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                    UsersList.Insert(0, select);
                    ddlGRNBy.DataSource = UsersList;
                    ddlGRNBy.DataBind();
                    ddlGRNBy.SelectedIndex = ddlGRNBy.Items.IndexOf(ddlGRNBy.Items.FindByValue(GrnLst.CreatedBy.ToString()));

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(GrnLst.Status.ToString()));

                    txtBatchNo.Text = GrnLst.BatchNo.ToString();
                    txtShippingNo.Text = GrnLst.ShipID.ToString();
                    txtRemark.Text = GrnLst.Remark.ToString();
                    txtAirwayBill.Text = GrnLst.AirwayBill.ToString();
                    txtShippingType.Text = GrnLst.ShippingType.ToString();
                    UC_ShippingDate.Date = GrnLst.ShippingDate;
                    txtTransporterName.Text = GrnLst.TransporterName.ToString();
                    txtTransporterRemark.Text = GrnLst.TransporterRemark.ToString();
                    Grid1.DataSource = Inbound.GetGrnSkuDetailsbyGRNID(long.Parse(GrnLst.ID.ToString()), profile.DBConnection._constr);
                    Grid1.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvGRNDetail');LoadingOff();", true);
                }

                /*Temparary Change*/
                //grdASN.DataSource = Inbound.GetASNHead(profile.DBConnection._constr);
                //grdASN.DataBind();

                grdLoader.DataSource = Inbound.GetLoaderDetailsOfGRN(profile.DBConnection._constr);
                grdLoader.DataBind();
            }
            catch { }
            finally { Inbound.Close(); objService.Close(); UsersList.Clear(); }
        }
        #endregion

        #region BindDropDown
        public void fillUser()
        {
            iInboundClient Inbound = new iInboundClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                long WarehouseID = 0;
                if (Session["POID"] != null)
                {                    
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            lblSelectedPO.Text = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            lblSelectedGTN.Text = dsJCN.Tables[0].Rows[0]["JobCardName"].ToString();
                            lblPurchaseOrderDate.Text = dsJCN.Tables[0].Rows[0]["CreationDate"].ToString();
                            lblPOBy.Text = dsJCN.Tables[0].Rows[0]["CreatedByUser"].ToString();
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                        }
                    }
                    else
                    {
                        tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                        POHead = Inbound.GetPoHeadByPOID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                        lblSelectedPO.Text = Session["POID"].ToString();
                        lblSelectedGTN.Text = "Not Created";
                        lblPurchaseOrderDate.Text = POHead.POdate.Value.ToString("dd-MMM-yyyy");
                        lblPOBy.Text = Inbound.GetUserNameByID(long.Parse(POHead.CreatedBy.ToString()), profile.DBConnection._constr);
                        WarehouseID = long.Parse(POHead.Warehouse.ToString());
                    }
                }
                else if (Session["SOID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                     if (chkJObCart >= 1)
                     {
                         DataSet dsJCN = new DataSet();
                         dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                         if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                         {
                             lblSelectedPO.Text = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                             lblSelectedGTN.Text = dsJCN.Tables[0].Rows[0]["JobCardName"].ToString();
                             lblPurchaseOrderDate.Text = dsJCN.Tables[0].Rows[0]["CreationDate"].ToString();
                             lblPOBy.Text = dsJCN.Tables[0].Rows[0]["CreatedByUser"].ToString();
                             WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                         }
                     }
                     else
                     {
                         tOrderHead OHead = new tOrderHead();
                         OHead = Inbound.GetSoHeadBySOIDForSalesReturn(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), profile.DBConnection._constr);
                         lblSelectedPO.Text = Session["SOID"].ToString();
                         lblSelectedGTN.Text = "Not Created";
                         lblPurchaseOrderDate.Text = OHead.Orderdate.Value.ToString("dd-MMM-yyyy");
                         lblPOBy.Text = Inbound.GetUserNameByID(long.Parse(OHead.CreatedBy.ToString()), profile.DBConnection._constr);
                         WarehouseID = long.Parse(OHead.StoreId.ToString());
                     }
                }
                else if (Session["TRID"] != null)
                {
                    tTransferHead thHead = new tTransferHead();
                    thHead = Inbound.GetTransferHeadByTRID(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), profile.DBConnection._constr);
                    lblSelectedPO.Text = Session["TRID"].ToString();
                    lblSelectedGTN.Text = "Not Created";
                    lblPurchaseOrderDate.Text =thHead.TransferDate.Value.ToString("dd-MMM-yyyy");
                    lblPOBy.Text = Inbound.GetUserNameByID(long.Parse(thHead.TransferBy.ToString()), profile.DBConnection._constr);
                    WarehouseID = long.Parse(thHead.ToPosition.ToString());
                }
                UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);

                ddlGRNBy.DataSource = UsersList;
                ddlGRNBy.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }
        public static List<BrilliantWMS.WMSInbound.mStatu> WMFillStatus()
        {
            string ordrstate = state;
            string ordrObject = "";
            iInboundClient Inbound = new iInboundClient();
            List<BrilliantWMS.WMSInbound.mStatu> StatusList = new List<BrilliantWMS.WMSInbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (state != "View")
                {
                    if (HttpContext.Current.Session["TRID"] != null)
                    {
                        StatusList = Inbound.GetStatusListForInbound("TransferGRN", "", ordrstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                    else
                    {
                        StatusList = Inbound.GetStatusListForInbound(ObjectName, "", ordrstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        if (HttpContext.Current.Session["POID"] != null)
                        {
                            long stPOID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                            tPurchaseOrderHead PH = new tPurchaseOrderHead();
                            PH = Inbound.GetPoHeadByPOID(stPOID, profile.DBConnection._constr);
                            ordrObject = PH.Object.ToString();
                        }
                        else
                        {
                            ordrObject = "SalesReturn";
                        }

                        if (ordrObject == "SalesReturn")
                        {
                            StatusList = StatusList.Where(s => s.ID == 52).ToList();
                        }
                        else if (ordrObject == "PurchaseOrder")
                        {
                            StatusList = StatusList.Where(s => s.ID == 31).ToList();
                        }
                    }
                }
                else
                {
                    StatusList = Inbound.GetStatusListForInbound("", "PurchaseOrder,POSO,Return,Transfer", ordrstate, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                }
                BrilliantWMS.WMSInbound.mStatu select = new BrilliantWMS.WMSInbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetails.aspx", "WMFillStatus");
            }
            finally { Inbound.Close(); }
            return StatusList;
        }
        #endregion

        #region PO

        protected void DisplayPOData()
        {
            iInboundClient Inbound = new iInboundClient();
            List<WMS_SP_GetPartDetails_ForGRN_Result> PODetail = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
            try
            {               
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["POID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PODetail = Inbound.GetGRNPartDetailsByPOID(grpPOID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        long POID = long.Parse(Session["POID"].ToString());
                        PODetail = Inbound.GetGRNPartDetailsByPOID(POID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if (Session["SOID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesReturn", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PODetail = Inbound.GetGRNPartDetailsBySOID(grpPOID,"", Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        long POID = long.Parse(Session["SOID"].ToString());
                        PODetail = Inbound.GetGRNPartDetailsBySOID(POID.ToString(),"", Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if (Session["TRID"] != null)
                {
                    long TRID = long.Parse(Session["TRID"].ToString());
                    PODetail = Inbound.GetGRNPartDetailsBySOID("",TRID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = PODetail;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "DisplayPOData");
            }
            finally { Inbound.Close(); }
        }

        #endregion

        #region Part List
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                long POID = long.Parse(Session["POID"].ToString());
                Grid1.DataSource = null;
                Grid1.DataBind();
                if (hdnProductSearchSelectedRec.Value == "1")
                {
                    Grid1.DataSource = Inbound.GetExistingTempDataBySessionIDObjectNameGRN(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else
                {
                    Grid1.DataSource = Inbound.GetGRNPartDetailsByPOID(POID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                Grid1.DataBind();
                hdnProductSearchSelectedRec.Value = "0";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "Grid1_OnRebind");
            }
            finally { Inbound.Close(); }
        }

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                    Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[8] as Obout.Grid.GridDataControlFieldCell;

                    TextBox txtUsrQty = e.Row.Cells[7].FindControl("txtUsrQty") as TextBox;
                    Label SelPoQty = e.Row.Cells[6].FindControl("SelPoQty") as Label;
                    Label ShortQty = e.Row.Cells[8].FindControl("ShortQty") as Label;
                    Label ExcessQty = e.Row.Cells[9].FindControl("ExcessQty") as Label;

                    Label ASNQty = e.Row.Cells[10].FindControl("ASNQty") as Label;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);

                    decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                    decimal poQty = decimal.Parse(SelPoQty.Text.ToString());
                    decimal asnQty = decimal.Parse(ASNQty.Text.ToString());

                    txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this,'" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + poQty + ",'" + ShortQty.ClientID.ToString() + "','" + ExcessQty.ClientID.ToString() + "'," + asnQty + ")");
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "Grid1_OnRowDataBound");
            }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdGRNPart(Object objGRN)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objGRN;
                CustomProfile profile = CustomProfile.GetProfile();

                WMS_SP_GetPartDetails_ForGRN_Result GRNPart = new WMS_SP_GetPartDetails_ForGRN_Result();

                GRNPart.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                GRNPart.GRNQty = Convert.ToDecimal(dictionary["GRNQty"]);
                GRNPart.ShortQty = Convert.ToDecimal(dictionary["ShortQty"]);
                GRNPart.ExcessQty = Convert.ToDecimal(dictionary["ExcessQty"]);
                Inbound.UPdateGRNTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), GRNPart, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMUpdGRNPart"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static int WMRemovePartFromRequest(Int32 Sequence)
        {
            int editOrder = 0;
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (long.Parse(HttpContext.Current.Session["POID"].ToString()) > 0)
                {
                    tPurchaseOrderHead RequestHead = new tPurchaseOrderHead();
                    long ReqID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                    RequestHead = Inbound.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                    string Status = RequestHead.Status.ToString();
                    //if (Status == "1")
                    //{
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        Inbound.RemovePartFromGRN_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                        editOrder = 1;
                    //}
                    //else { editOrder = 0; }
                }
                else
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    Inbound.RemovePartFromGRN_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                    editOrder = 1;
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMRemovePartFromRequest");
            }
            finally { Inbound.Close(); }
            return editOrder;
        }
        #endregion

        #region ASN
        protected void imgBtnView_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            var asnID = imgbtn.ToolTip.ToString();
            Session["ASNID"] = asnID.ToString();

            Response.Redirect("../WMS/AsnDetail.aspx");
        }

        protected void imgBtnLoader_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            var ldrID = imgbtn.ToolTip.ToString();
            Session["ldrID"] = ldrID.ToString();

           // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../WMS/Loader.aspx?VW=', null, 'height=275px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50')", true);
        }
        #endregion
    }
}