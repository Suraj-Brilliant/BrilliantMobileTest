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
using System.Collections;

namespace BrilliantWMS.WMS
{
    public partial class QCDetail : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string ObjectName = "QC";
        static string QCstate = "";
        static string grnObj = "";
        #region PageEvents
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["GRNID"] != null)
                {
                    hdnCurrentSession.Value = "GRNID";
                    if (Session["GRNID"].ToString() != "0" && Session["GRNstate"].ToString() == "Edit")
                    {
                        QCstate = Session["GRNstate"].ToString();
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCQCDate.Date = DateTime.Now;
                        if (Session["GRNID"] != null)
                        {
                            DisplaySkuData();
                        }
                    }
                    else
                    {
                        if (Session["GRNID"].ToString() != "0" && Session["GRNstate"].ToString() == "View")
                        {
                            QCstate = Session["GRNstate"].ToString();
                            GetQCDetail(Session["GRNID"].ToString());
                        }
                    }
                }
                else if (Session["POID"] != null)
                {
                    hdnCurrentSession.Value = "POID";
                    if (Session["POID"].ToString() != "0" && Session["POstate"].ToString() == "Edit")
                    {
                        QCstate = Session["POstate"].ToString();
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCQCDate.Date = DateTime.Now;
                        if (Session["POID"] != null)
                        {
                            DisplaySkuData();
                        }
                    }
                    else
                    {//view
                    }
                }
                else if (Session["QCID"] != null)
                {
                    hdnCurrentSession.Value = "QCID";
                    if (Session["QCID"].ToString() != "0" && Session["QCstate"].ToString() == "View")
                    {
                        QCstate = Session["QCstate"].ToString();
                        GetQCDetail(Session["QCID"].ToString());
                    }
                }
                else if (Session["PKUPID"] != null)
                {
                    hdnCurrentSession.Value = "PKUPID";
                    if (Session["PKUPID"].ToString() != "0" && Session["PKUPstate"].ToString() == "Edit")
                    {
                        QCstate = Session["PKUPstate"].ToString();
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCQCDate.Date = DateTime.Now;
                        if (Session["PKUPID"] != null)
                        {
                            DisplaySkuData();
                        }
                    }
                }
                else if (Session["TRID"] != null)
                {
                    hdnCurrentSession.Value = "TRID";
                    if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "Edit")
                    {
                        QCstate = Session["TRstate"].ToString();
                        fillUser();
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                        UCQCDate.Date = DateTime.Now;
                        if (Session["TRID"] != null)
                        {
                            DisplaySkuData();
                        }
                    }
                    else
                    {
                        if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "View")
                        {
                            QCstate = Session["TRstate"].ToString();
                            GetQCDetail(Session["TRID"].ToString());
                        }
                    }
                }
            }
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetEditRight(false, "Not Allowed");
            if (QCstate != "View")
            {
                Toolbar1.SetSaveRight(true, "Not Allowed");
            }
            else { Toolbar1.SetSaveRight(false, "Not Allowed"); }

            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        [WebMethod]
        public static long WMSaveQCHead(object objQC)
        {
            long result = 0;
            int RSLT = 0; long QCID = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tQualityControlHead QCHead = new tQualityControlHead();
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objQC;
                if (HttpContext.Current.Session["GRNID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotGRN = grpGRNID.Split(',');
                            int GRNCnt = TotGRN.Count();
                            for (int g = 0; g <= GRNCnt - 1; g++)
                            {
                                QCHead.CreatedBy = profile.Personal.UserID;
                                QCHead.Creationdate = DateTime.Now;
                                QCHead.ObjectName = "PurchaseOrder";
                                QCHead.OID = long.Parse(TotGRN[g].ToString());
                                QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                                QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                                QCHead.Remark = d["Remark"].ToString();
                                QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                                QCHead.Company = profile.Personal.CompanyID;

                                QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);

                                if (QCID > 0)
                                {
                                    RSLT = Inbound.FinalSaveQCDetail(long.Parse(TotGRN[g].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "PurchaseOrder", profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = QCID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        QCHead.CreatedBy = profile.Personal.UserID;
                        QCHead.Creationdate = DateTime.Now;
                        if (grnObj == "PurchaseOrder") { QCHead.ObjectName = "PurchaseOrder"; }
                        else if (grnObj == "SalesReturn") { QCHead.ObjectName = "SalesReturn"; }
                        QCHead.OID = long.Parse(HttpContext.Current.Session["GRNID"].ToString());
                        QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                        QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                        QCHead.Remark = d["Remark"].ToString();
                        QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                        QCHead.Company = profile.Personal.CompanyID;
                        QCHead.ID = 0;

                        QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                        if (QCID > 0)
                        {
                            RSLT = Inbound.FinalSaveQCDetail(long.Parse(HttpContext.Current.Session["GRNID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "PurchaseOrder", profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = QCID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                        Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
                else if (HttpContext.Current.Session["PKUPID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["PKUPID"].ToString()), "PickUp", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["PKUPID"].ToString()), "PickUp", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpPKID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotPK = grpPKID.Split(',');
                            int PKCnt = TotPK.Count();
                            for (int g = 0; g <= PKCnt - 1; g++)
                            {
                                QCHead.CreatedBy = profile.Personal.UserID;
                                QCHead.Creationdate = DateTime.Now;
                                QCHead.ObjectName = "SalesOrder";
                                QCHead.OID = long.Parse(TotPK[g].ToString());
                                QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                                QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                                QCHead.Remark = d["Remark"].ToString();
                                QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                                QCHead.Company = profile.Personal.CompanyID;

                                QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);

                                if (QCID > 0)
                                {
                                    RSLT = Inbound.FinalSaveQCDetail(long.Parse(TotPK[g].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "SalesOrder", profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = QCID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        QCHead.CreatedBy = profile.Personal.UserID;
                        QCHead.Creationdate = DateTime.Now;

                        QCHead.ObjectName = "SalesOrder";
                        QCHead.OID = long.Parse(HttpContext.Current.Session["PKUPID"].ToString());
                        QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                        QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                        QCHead.Remark = d["Remark"].ToString();
                        QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                        QCHead.Company = profile.Personal.CompanyID;
                        QCHead.ID = 0;

                        QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                        if (QCID > 0)
                        {
                            RSLT = Inbound.FinalSaveQCDetail(long.Parse(HttpContext.Current.Session["PKUPID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "SalesOrder", profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = QCID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                        Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
                else if (HttpContext.Current.Session["TRID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            string[] TotGRN = grpGRNID.Split(',');
                            int GRNCnt = TotGRN.Count();
                            for (int g = 0; g <= GRNCnt - 1; g++)
                            {
                                long status = Convert.ToInt64(d["Status"].ToString());
                                if (status == 61)
                                {
                                    long grnTransferID = Inbound.GetGRNIDByTransferID(long.Parse(TotGRN[g].ToString()), profile.DBConnection._constr);
                                    QCHead.OID = grnTransferID;
                                }
                                else
                                {
                                    QCHead.OID = long.Parse(TotGRN[g].ToString());
                                }
                                QCHead.CreatedBy = profile.Personal.UserID;
                                QCHead.Creationdate = DateTime.Now;
                                QCHead.ObjectName = "Transfer";
                                QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                                QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                                QCHead.Remark = d["Remark"].ToString();
                                QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                                QCHead.Company = profile.Personal.CompanyID;

                                QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);

                                if (QCID > 0)
                                {
                                    RSLT = Inbound.FinalSaveQCDetail(long.Parse(TotGRN[g].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "Transfer", profile.DBConnection._constr);
                                    if (RSLT == 1 || RSLT == 2) { result = QCID; }
                                    else if (RSLT == 0) { result = 0; }
                                    iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                                    DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                                }
                            }
                            Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        long status = Convert.ToInt64(d["Status"].ToString());
                        if (status == 61)
                        {
                            long grnTransferID = Inbound.GetGRNIDByTransferID(long.Parse(HttpContext.Current.Session["TRID"].ToString()), profile.DBConnection._constr);
                            QCHead.OID = grnTransferID;
                        }
                        else
                        {
                            QCHead.OID = long.Parse(HttpContext.Current.Session["TRID"].ToString());
                        }
                        QCHead.CreatedBy = profile.Personal.UserID;
                        QCHead.Creationdate = DateTime.Now;
                        QCHead.ObjectName = "Transfer";
                        QCHead.QCDate = Convert.ToDateTime(d["QCDate"]);
                        QCHead.QCBy = Convert.ToInt64(d["QCby"].ToString());
                        QCHead.Remark = d["Remark"].ToString();
                        QCHead.Status = Convert.ToInt64(d["Status"].ToString());
                        QCHead.Company = profile.Personal.CompanyID;
                        QCHead.ID = 0;

                        QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                        if (QCID > 0)
                        {
                            RSLT = Inbound.FinalSaveQCDetail(long.Parse(HttpContext.Current.Session["TRID"].ToString()), HttpContext.Current.Session.SessionID, ObjectName, QCID, profile.Personal.UserID.ToString(), Convert.ToInt16(QCHead.Status), "Transfer", profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = QCID; }
                            else if (RSLT == 0) { result = 0; }
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, QCID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                        Inbound.ClearTempDataFromDBQC(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "QCDetails.aspx", "WMSaveQCHead"); result = 0; }
            finally { Inbound.Close(); }
            return result;
        }

        public void GetQCDetail(string GRNID)
        {
            long oid = 0;
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                WMS_VW_GetQCDetails grnlst = new WMS_VW_GetQCDetails();
                if (Session["GRNID"] != null)
                {
                    oid = long.Parse(GRNID.ToString());
                    grnlst = Inbound.GetQCDetailsByGRNID(oid, profile.DBConnection._constr); hdnQCStatus.Value = grnlst.Status.ToString();
                    if (QCstate == "View")
                    {
                        string qcofgrn = grnlst.ID.ToString();
                        Grid1.DataSource = Inbound.GetQCPartDetailsByQCID(qcofgrn, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                    else
                    {
                        Grid1.DataSource = Inbound.GetQCPartDetailsByGRNID(GRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                    Grid1.DataBind();
                }
                else if (Session["QCID"] != null)
                {
                    oid = long.Parse(GRNID.ToString());
                    grnlst = Inbound.GetQCDetailsByQCID(oid, profile.DBConnection._constr); hdnQCStatus.Value = grnlst.Status.ToString();

                    Grid1.DataSource = Inbound.GetQCPartDetailsByQCID(GRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    Grid1.DataBind();
                }
                else if (Session["TRID"] != null)
                {
                    oid = long.Parse(GRNID.ToString());
                    grnlst = Inbound.GetQCDetailsByGRNID(oid, profile.DBConnection._constr); hdnQCStatus.Value = grnlst.Status.ToString();

                    Grid1.DataSource = Inbound.GetQCPartDetailsByTransferID(GRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    Grid1.DataBind();
                }
                lblQCNumber.Text = grnlst.ID.ToString();
                txtRemark.Text = grnlst.Remark.ToString();
                UCQCDate.Date = grnlst.QCDate;
                long Warehouse = long.Parse(grnlst.WarehouseID.ToString());
                UsersList = objService.GetUserListByWarehouseID(Warehouse, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
                ddlQCBy.DataSource = UsersList;
                ddlQCBy.DataBind();
                ddlQCBy.SelectedIndex = ddlQCBy.Items.IndexOf(ddlQCBy.Items.FindByValue(grnlst.QCBy.ToString()));
                ddlStatus.DataSource = WMFillStatus();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(grnlst.Status.ToString()));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'dvQCDetail');LoadingOff();", true);
                Toolbar1.SetSaveRight(false, "Not Allowed");
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
                tPurchaseOrderHead POHead = new tPurchaseOrderHead();
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
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                        }
                    }
                    else
                    {
                        POHead = Inbound.GetPoHeadByPOID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                        WarehouseID = long.Parse(POHead.Warehouse.ToString());
                    }
                }
                else if (Session["GRNID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString());
                        }
                    }
                    else
                    {
                        long POID = Inbound.GetPOIDFromGRNID(long.Parse(Session["GRNID"].ToString()), profile.DBConnection._constr);
                        WMS_VW_GetGRNDetails grndt = new WMS_VW_GetGRNDetails();
                        grndt = Inbound.GetGRNDetailsByGRNIDGRNMenu(long.Parse(Session["GRNID"].ToString()), profile.DBConnection._constr);
                        grnObj = grndt.ObjectName.ToString();
                        if (grnObj == "PurchaseOrder")
                        {
                            tPurchaseOrderHead POHeadG = new tPurchaseOrderHead();
                            POHeadG = Inbound.GetPoHeadByPOID(POID, profile.DBConnection._constr);
                            WarehouseID = long.Parse(POHeadG.Warehouse.ToString());
                        }
                        else if (grnObj == "SalesReturn")
                        {
                            tOrderHead SOHeadG = new tOrderHead();
                            SOHeadG = Inbound.GetSoHeadBySOIDForSalesReturn(POID, profile.DBConnection._constr);
                            WarehouseID = long.Parse(SOHeadG.StoreId.ToString());
                        }

                    }
                }
                else if (Session["TRID"] != null)
                {
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            WarehouseID = long.Parse(dsJCN.Tables[0].Rows[0]["Warehouse"].ToString()); grnObj = "Transfer";
                        }
                    }
                    else
                    {
                        tTransferHead TRHead = new tTransferHead();
                        TRHead = Inbound.GetTransferHeadByTRID(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), profile.DBConnection._constr);
                        WarehouseID = long.Parse(TRHead.FromPosition.ToString());
                        grnObj = TRHead.ObjectName.ToString();
                    }
                }

                UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);

                ddlQCBy.DataSource = UsersList;
                ddlQCBy.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        public static List<BrilliantWMS.WMSInbound.mStatu> WMFillStatus()
        {
            string state = QCstate;
            iInboundClient Inbound = new iInboundClient();
            List<BrilliantWMS.WMSInbound.mStatu> StatusList = new List<BrilliantWMS.WMSInbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (state != "View")
                {
                    if (grnObj == "SalesReturn")
                    {
                        StatusList = Inbound.GetStatusListForInbound("QC", "", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                    else if (grnObj == "Transfer")
                    {
                        long TRID = long.Parse(HttpContext.Current.Session["TRID"].ToString());
                        tTransferHead TH = new tTransferHead();
                        TH = Inbound.GetTransferHeadByTRID(TRID, profile.DBConnection._constr);
                        long trCurrentStatus = long.Parse(TH.Status.ToString());
                        if (trCurrentStatus == 57)
                        {
                            StatusList = Inbound.GetStatusListForInbound("TransferQCOut", "", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        }
                        else
                        {
                            StatusList = Inbound.GetStatusListForInbound("TransferQCIn", "", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        StatusList = Inbound.GetStatusListForInbound("", "POSO", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                }
                else
                {
                    StatusList = Inbound.GetStatusListForInbound("", "SalesOrder,PurchaseOrder,POSO,Return,Transfer", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                }

                BrilliantWMS.WMSInbound.mStatu select = new BrilliantWMS.WMSInbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "QCDetails.aspx", "WMFillStatus");
            }
            finally { Inbound.Close(); }
            return StatusList;
        }
        #endregion

        #region GRN

        protected void DisplaySkuData()
        {
            iInboundClient Inbound = new iInboundClient();
            List<WMS_SP_GetPartDetails_ForQC_Result> PrdDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["GRNID"] != null)
                {
                    long GRNID = long.Parse(Session["GRNID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PrdDetail = Inbound.GetQCPartDetailsByGRNID(grpGRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        PrdDetail = Inbound.GetQCPartDetailsByGRNID(GRNID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if (Session["POID"] != null)
                {
                    long POID = long.Parse(Session["POID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PrdDetail = Inbound.GetQCPartDetailsByPOID(grpPOID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        PrdDetail = Inbound.GetQCPartDetailsByPOID(POID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if (Session["PKUPID"] != null)
                {
                    long pkupID = long.Parse(Session["PKUPID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["PKUPID"].ToString()), "PickUp", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["PKUPID"].ToString()), "PickUp", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PrdDetail = Inbound.GetQCPartDetailsByPickUPID(grpGRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        PrdDetail = Inbound.GetQCPartDetailsByPickUPID(pkupID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }
                else if (Session["TRID"] != null)
                {
                    long trID = long.Parse(Session["TRID"].ToString());
                    int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                    if (chkJObCart >= 1)
                    {
                        DataSet dsJCN = new DataSet();
                        dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                        {
                            string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                            PrdDetail = Inbound.GetQCPartDetailsByTransferID(grpGRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else
                    {
                        PrdDetail = Inbound.GetQCPartDetailsByTransferID(trID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }

                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = PrdDetail;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "QCDetail.aspx", "DisplaySkuData");
            }
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
                //if (HttpContext.Current.Session["POID"] != null)
                //{
                //    if (long.Parse(HttpContext.Current.Session["POID"].ToString()) > 0)
                //    {
                //        tPurchaseOrderHead RequestHead = new tPurchaseOrderHead();
                //        long ReqID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                //        RequestHead = Inbound.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                //        string Status = RequestHead.Status.ToString();
                //        //if (Status == "1")
                //{
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                Inbound.RemovePartFromQC_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                editOrder = 1;
                //}
                //else { editOrder = 0; }
                //    }
                //    else
                //    {
                //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //        Inbound.RemovePartFromQC_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                //        editOrder = 1;
                //    }
                //}
                //else if (HttpContext.Current.Session["GRNID"] != null)
                //{

                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "QCDetail.aspx", "WMRemovePartFromRequest");
            }
            finally { Inbound.Close(); }
            return editOrder;
        }

        [WebMethod]
        public static void WMUpdQCPart(Object objQC)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objQC;
                CustomProfile profile = CustomProfile.GetProfile();

                WMS_SP_GetPartDetails_ForQC_Result QCPart = new WMS_SP_GetPartDetails_ForQC_Result();
                QCPart.Sequence = Convert.ToInt64(d["Sequence"]);
                QCPart.SelectedQty = Convert.ToDecimal(d["SelectedQty"]);
                QCPart.RejectedQty = Convert.ToDecimal(d["RejectedQty"]);
                QCPart.Reason = d["Reason"].ToString();

                Inbound.UPdateQCTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), QCPart, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMUpdGRNPart"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdQCPartReason(Object objqc)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objqc;
                CustomProfile profile = CustomProfile.GetProfile();

                WMS_SP_GetPartDetails_ForQC_Result QCPart = new WMS_SP_GetPartDetails_ForQC_Result();
                QCPart.Sequence = Convert.ToInt64(d["Sequence"]);
                QCPart.Reason = d["Reason"].ToString();

                Inbound.UpdateQCTempDataReason(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), QCPart, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMUpdGRNPart"); }
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
                //long POID = long.Parse(Session["POID"].ToString());
                Grid1.DataSource = null;
                Grid1.DataBind();
                if (hdnProductSearchSelectedRec.Value == "1")
                {
                    Grid1.DataSource = Inbound.GetExistingTempDataBySessionIDObjectNameQC(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else
                {
                    if (Session["GRNID"] != null)
                    {
                        long GRNID = long.Parse(Session["GRNID"].ToString());
                        int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                        if (chkJObCart >= 1)
                        {
                            DataSet dsJCN = new DataSet();
                            dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["GRNID"].ToString()), "GRN", profile.DBConnection._constr);
                            if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                            {
                                string grpGRNID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                                Grid1.DataSource = Inbound.GetQCPartDetailsByGRNID(grpGRNID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                            }
                        }
                        else
                        {
                            Grid1.DataSource = Inbound.GetQCPartDetailsByGRNID(GRNID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
                    else if (Session["POID"] != null)
                    {
                        long POID = long.Parse(Session["POID"].ToString());
                        int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                        if (chkJObCart >= 1)
                        {
                            DataSet dsJCN = new DataSet();
                            dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), "PurchaseOrder", profile.DBConnection._constr);
                            if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                            {
                                string grpPOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                                Grid1.DataSource = Inbound.GetQCPartDetailsByPOID(grpPOID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                            }
                        }
                        else
                        {
                            Grid1.DataSource = Inbound.GetQCPartDetailsByPOID(POID.ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }
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

                    TextBox txtUsrQty = e.Row.Cells[9].FindControl("txtUsrQty") as TextBox;
                    Label SelRejectedQty = e.Row.Cells[10].FindControl("SelRejectedQty") as Label;
                    Label SelQCQty = e.Row.Cells[8].FindControl("SelQCQty") as Label;
                    TextBox txtUsrReason = e.Row.Cells[11].FindControl("txtUsrReason") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);

                    decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                    decimal qcQty = decimal.Parse(SelQCQty.Text.ToString());


                    if (hdnQCStatus.Value == "32" || hdnQCStatus.Value == "55" || hdnQCStatus.Value == "58" || hdnQCStatus.Value == "61")
                    {
                        txtUsrQty.Enabled = false;
                        txtUsrReason.Enabled = false;
                    }
                    else
                    {
                        txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this,'" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + qcQty + ",'" + SelRejectedQty.ClientID.ToString() + "','" + txtUsrReason.ClientID.ToString() + "')");
                        txtUsrReason.Attributes.Add("onblur", "javascript:GetQCReason(this," + e.Row.RowIndex + "," + qcQty + ",'" + SelRejectedQty.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "')");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "Grid1_OnRowDataBound");
            }
            finally { Inbound.Close(); }
        }

        #endregion

        #region CreditNoteDebitNote

        [WebMethod]
        public static long WMCheckQCStatusCN(string crntSession)
        {
            long result = 0;
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (crntSession=="POID")
                {
                    long poID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                    long poStatus = 0;
                    tPurchaseOrderHead PH = new tPurchaseOrderHead();
                    PH = Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                    poStatus = long.Parse(PH.Status.ToString());                    
                    if (poStatus == 32 || poStatus == 33 || poStatus == 35)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else if (crntSession=="GRNID")
                {
                    long grnID = long.Parse(HttpContext.Current.Session["GRNID"].ToString());
                    long poID = 0, poStatus = 0;
                    poID = Inbound.GetPOIDFromGRNID(grnID, profile.DBConnection._constr);
                    tPurchaseOrderHead PHG = new tPurchaseOrderHead();
                    PHG = Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                    poStatus = long.Parse(PHG.Status.ToString());
                    if (poStatus == 32 || poStatus == 33 || poStatus == 35)
                    {
                        result = Inbound.GetExcessQtyByGRNID(grnID, profile.DBConnection._constr);
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else if (crntSession=="QCID")
                {
                    WMS_VW_GetQCDetails qcD = new WMS_VW_GetQCDetails();
                    long grnID = 0;
                    long qcID = long.Parse(HttpContext.Current.Session["QCID"].ToString());
                    qcD = Inbound.GetQCDetailsByQCID(qcID, profile.DBConnection._constr);
                    if (qcD.ObjectName == "PurchaseOrder")
                    {
                        grnID = long.Parse(qcD.OID.ToString());
                        long poID = 0, poStatus = 0;
                        poID = Inbound.GetPOIDFromGRNID(grnID, profile.DBConnection._constr);
                        tPurchaseOrderHead PHG = new tPurchaseOrderHead();
                        PHG = Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                        poStatus = long.Parse(PHG.Status.ToString());
                        if (poStatus == 32 || poStatus == 33 || poStatus == 35)
                        {
                            result = Inbound.GetExcessQtyByGRNID(grnID, profile.DBConnection._constr);
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else if (qcD.ObjectName == "SalesOrder")
                    { result = 0; }
                    else if (qcD.ObjectName == "Transfer")
                    { result = 0; }
                }
                else if (crntSession=="PKUPID")
                {
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMCheckQCStatusCN");
            }
            finally { Inbound.Close(); }
            return result;
        }

        [WebMethod]
        public static long WMCheckQCStatusDN(string crntSession, string confirm_value)
        {
            long result = 0;
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (crntSession == "POID")
                {
                    long poID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                    long poStatus = 0;
                    tPurchaseOrderHead PH = new tPurchaseOrderHead();
                    PH = Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                    poStatus = long.Parse(PH.Status.ToString());
                    if (poStatus == 32 || poStatus == 33 || poStatus == 35)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else if (crntSession == "GRNID")
                {
                    long grnID = long.Parse(HttpContext.Current.Session["GRNID"].ToString());
                    long poID = 0, poStatus = 0;
                    poID = Inbound.GetPOIDFromGRNID(grnID, profile.DBConnection._constr);
                    tPurchaseOrderHead PHG = new tPurchaseOrderHead();
                    PHG = Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                    poStatus = long.Parse(PHG.Status.ToString());
                    if (poStatus == 32 || poStatus == 33 || poStatus == 35 )
                    {
                        result = Inbound.GetShortQtyByGRNID(grnID, profile.DBConnection._constr);
                    }
                    else if (poStatus == 63)
                    {                        
                        string confirmValue = confirm_value;
                        if (confirmValue == "Yes")
                        {
                            Inbound.MakePOStatusGRN(poID, profile.DBConnection._constr);   /*Make PO Status QC from Partially Completed*/
                            result = Inbound.GetShortQtyByGRNID(grnID, profile.DBConnection._constr);
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else if (crntSession == "QCID")
                {
                }
                else if (crntSession == "PKUPID")
                {
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "GRNDetail.aspx", "WMCheckQCStatusCN");
            }
            finally { Inbound.Close(); }
            return result;
        }
        #endregion
    }
}