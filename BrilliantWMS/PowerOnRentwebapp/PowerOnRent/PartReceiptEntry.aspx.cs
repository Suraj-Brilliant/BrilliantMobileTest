using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.PORServicePartIssue;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartReceipts;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.ToolbarService;

namespace BrilliantWMS.PowerOnRent
{
    public partial class PartReceiptEntry : System.Web.UI.Page
    {
        static string ObjectName = "ReceiptPartDetail";
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Toolbar1.SetUserRights("MaterialReceipt", "EntryForm", "");
                Toolbar1.SetAddNewRight(false, "Select pending receipt from Receipt Summary to AddNew / Edit Receipt", "../PowerOnRent/Default.aspx?invoker=Receipt");
                FillStatus();
                if (Session["PORReceiptID"] != null)
                {
                    GetReceiptHeadByReceiptID();
                }
                else if (Session["PORIssueID"] != null)
                {
                    GetRequest_n_IssueHeadByIssueID();
                }
                else if (Session["PORRequestID"] != null)
                {
                    GetRequestHeadByRequestID();
                }

                FillUserList();

            }
            UC_ReceiptDate.DateIsRequired(true, "", "");
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        #endregion

        #region Toolbar Code
        [WebMethod]
        public static mUserRolesDetail WMSetSessionReceipt(string ObjectName, long ReceiptID, string state)
        {
            HttpContext.Current.Session["PORReceiptID"] = ReceiptID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Receipt":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialReceipt", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Consumption":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Consumption", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
            }
            return checkRole;
        }
        #endregion

        #region Fill Dropdown

        protected void FillStatus()
        {
            string state = HttpContext.Current.Session["PORstate"].ToString();
            iPartReceiptClient objService = new iPartReceiptClient();
            List<BrilliantWMS.PORServicePartReceipts.mStatu> StatusList = new List<BrilliantWMS.PORServicePartReceipts.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                StatusList = objService.GetStatusListForGRN("All,Receipt", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();

                BrilliantWMS.PORServicePartReceipts.mStatu select = new BrilliantWMS.PORServicePartReceipts.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
                ddlStatus.DataSource = null;
                ddlStatus.DataBind();
                ddlStatus.DataSource = StatusList;
                ddlStatus.DataBind();
            }
            catch { }
            finally { objService.Close(); }

        }

        public void FillUserList()
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListBySiteID(Convert.ToInt64(hdnSiteID.Value), profile.DBConnection._constr).ToList();
                UsersList = UsersList.Distinct().ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);

                ddlReceivedBy.DataSource = null;
                ddlReceivedBy.DataBind();
                ddlReceivedBy.DataSource = UsersList;
                ddlReceivedBy.DataBind();
                ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(profile.Personal.UserID.ToString()));

            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Receipt Part Detail
        protected void GridReceipt_OnRebind(object sender, EventArgs e)
        {
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {
                GridReceipt.DataSource = null;
                GridReceipt.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                if (hdnReceiptID.Value != "0" && hdnReceiptID.Value != "")
                {
                    GridReceipt.DataSource = objService.GetReceiptPartDetailByReceiptID(Convert.ToInt64(hdnReceiptID.Value), Convert.ToInt64(hdnSiteID.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "", profile.DBConnection._constr);
                    
                }
                else if (hdnIssueID.Value != "0" && hdnIssueID.Value != "")
                {
                    GridReceipt.DataSource = objService.GetReceiptPartDetailByIssueID(Convert.ToInt64(hdnIssueID.Value), Convert.ToInt64(hdnSiteID.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                }
                GridReceipt.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static string[] WMUpdateReceiptQty(object objReceipt)
        {
            string[] QtyResult = new string[] { };
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReceipt;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetails_OfGRN_Result ReceiptRec = new POR_SP_GetPartDetails_OfGRN_Result();
                ReceiptRec.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                ReceiptRec.ReceivedQty = Convert.ToDecimal(dictionary["ReceivedQty"]);
                QtyResult = objService.UpdatePartReceipt_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ReceiptRec, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return QtyResult;
        }
        #endregion

        #region Receipt Head
        [WebMethod]
        public static PORtGRNHead WMGetReceiptHead()
        {
            iPartReceiptClient objService = new iPartReceiptClient();
            PORtGRNHead ReceiptHead = new PORtGRNHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                ReceiptHead = objService.GetReceiptHeadByReceiptID(Convert.ToInt64(HttpContext.Current.Session["PORReceiptID"].ToString()), profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return ReceiptHead;
        }

        protected void GetReceiptHeadByReceiptID()
        {
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {
                PORtGRNHead ReceiptHead = new PORtGRNHead();
                CustomProfile profile = CustomProfile.GetProfile();
                ReceiptHead = objService.GetReceiptHeadByReceiptID(Convert.ToInt64(Session["PORReceiptID"].ToString()), profile.DBConnection._constr);
                if (ReceiptHead != null)
                {
                    Session["PORIssueID"] = ReceiptHead.ReferenceID.Value.ToString();
                    GetRequest_n_IssueHeadByIssueID();
                    hdnReceiptID.Value = ReceiptHead.GRNH_ID.ToString();
                    lblReceiptNo.Text = ReceiptHead.GRNH_ID.ToString();
                    UC_ReceiptDate.Date = DateTime.Now;
                    if (ReceiptHead.GRN_Date != null) UC_ReceiptDate.Date = ReceiptHead.GRN_Date;
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(ReceiptHead.StatusID.Value.ToString()));
                    if (ReceiptHead.ReceivedByUserID != 0) ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(ReceiptHead.ReceivedByUserID.Value.ToString()));
                    txtReceiptRemark.Text = ReceiptHead.Remark;
                    GridReceipt.DataSource = objService.GetReceiptPartDetailByReceiptID(ReceiptHead.GRNH_ID, Convert.ToInt64(hdnSiteID.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr);
                    GridReceipt.DataBind();
                    divDisabled();
                }
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void GetReceiptHeadByIssueID()
        {
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {

                PORtGRNHead ReceiptHead = new PORtGRNHead();
                CustomProfile profile = CustomProfile.GetProfile();
                ReceiptHead = objService.GetReceiptHeadByIssueID(Convert.ToInt64(Session["PORIssueID"].ToString()), profile.DBConnection._constr);
                if (ReceiptHead != null)
                {
                    //Session["PORIssueID"] = ReceiptHead.ReferenceID.Value.ToString();
                    //GetRequest_n_IssueHeadByIssueID();
                    hdnReceiptID.Value = ReceiptHead.GRNH_ID.ToString();
                    lblReceiptNo.Text = ReceiptHead.GRNH_ID.ToString();
                    UC_ReceiptDate.Date = DateTime.Now;
                    if (ReceiptHead.GRN_Date != null) UC_ReceiptDate.Date = ReceiptHead.GRN_Date;
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(ReceiptHead.StatusID.Value.ToString()));
                    if (ReceiptHead.ReceivedByUserID != 0) ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(ReceiptHead.ReceivedByUserID.Value.ToString()));
                    txtReceiptRemark.Text = ReceiptHead.Remark;
                    GridReceipt.DataSource = objService.GetReceiptPartDetailByReceiptID(ReceiptHead.GRNH_ID, Convert.ToInt64(hdnSiteID.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr);
                    GridReceipt.DataBind();
                    divDisabled();
                }
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static string WMSaveReceiptHead(object objReceipt)
        {
            HttpContext.Current.Session.Remove("PORIssueID");

            string result = "";
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                PORtGRNHead ReceiptHead = new PORtGRNHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReceipt;

                if (HttpContext.Current.Session["PORReceiptID"] != null)
                {
                    if (HttpContext.Current.Session["PORReceiptID"] == "0")
                    {
                        ReceiptHead.CreatedBy = profile.Personal.UserID;
                        ReceiptHead.CreationDt = DateTime.Now;
                    }
                    else
                    {
                        ReceiptHead = objService.GetReceiptHeadByReceiptID(Convert.ToInt64(HttpContext.Current.Session["PORReceiptID"]), profile.DBConnection._constr);
                        ReceiptHead.LastModifiedBy = profile.Personal.UserID;
                        ReceiptHead.LastModifiedDt = DateTime.Now;
                    }

                    ReceiptHead.SiteID = Convert.ToInt64(dictionary["SiteID"]);
                    ReceiptHead.ObjectName = dictionary["ObjectName"].ToString();
                    ReceiptHead.ReferenceID = Convert.ToInt64(dictionary["ReferenceID"].ToString());
                    ReceiptHead.GRN_No = "N/A";
                    ReceiptHead.GRN_Date = Convert.ToDateTime(dictionary["GRN_Date"].ToString());
                    ReceiptHead.ReceivedByUserID = Convert.ToInt64(dictionary["ReceivedByUserID"]);
                    ReceiptHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);
                    ReceiptHead.Remark = dictionary["Remark"].ToString();
                    ReceiptHead.IsSubmit = Convert.ToBoolean(dictionary["IsSubmit"]);

                    long ReceiptID = objService.SetIntoGRNHead(ReceiptHead, profile.DBConnection._constr);
                    string status = "";
                    if (ReceiptHead.StatusID == 8) status = "Received";
                    if (ReceiptID > 0)
                    {
                        objService.FinalSaveReceiptPartDetail(HttpContext.Current.Session.SessionID, ObjectName, ReceiptID, profile.Personal.UserID.ToString(), status, profile.DBConnection._constr);
                        result = "Material Receipt record saved successfully";
                    }
                }
            }
            catch { result = "Some error occurred"; }
            finally { objService.Close(); }
            return result;
        }
        #endregion

        #region Reqeust & Issue Head
        [WebMethod]
        public static POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result WMGetIssueHead(string IssueID)
        {
            iPartIssueClient objService = new iPartIssueClient();
            POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result IssueHead = new POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //IssueHead = objService.GetIssueSummayByIssueIDs(HttpContext.Current.Session["PORIssueID"].ToString(), profile.DBConnection._constr).FirstOrDefault();               
                IssueHead = objService.GetIssueSummayByIssueIDs(IssueID, profile.DBConnection._constr).FirstOrDefault();               
            }
            catch { }
            finally { objService.Close(); }
            return IssueHead;
        }

        protected void GetRequest_n_IssueHeadByIssueID()
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result IssueHead = new POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result();
                IssueHead = objService.GetIssueSummayByIssueIDs(Session["PORIssueID"].ToString(), profile.DBConnection._constr).FirstOrDefault();
                if (IssueHead != null)
                {
                    lblIssueNo.Text = IssueHead.MINH_ID.ToString();
                    hdnIssueID.Value = IssueHead.MINH_ID.ToString();
                    lblIssueDate.Text = IssueHead.IssueDate.Value.ToString("dd-MMM-yyyy");
                    lblIssuedBy.Text = IssueHead.IssuedByUserName.ToString();
                    if (IssueHead.AirwayBill != null) lblAirwayBill.Text = IssueHead.AirwayBill;
                    if (IssueHead.ShippingType != null) lblShippingType.Text = IssueHead.ShippingType;
                    if (IssueHead.ShippingDate != null) lblShippingDate.Text = IssueHead.ShippingDate.Value.ToString("dd-MMM-yyyy");
                    if (IssueHead.ExpectedDelDate != null) lblExpDelDate.Text = IssueHead.ExpectedDelDate.Value.ToString("dd-MMM-yyyy");
                    if (IssueHead.TransporterName != null) lblTransporterName.Text = IssueHead.TransporterName;
                    if (IssueHead.IssueRemark != null) lblIssueRemark.Text = IssueHead.IssueRemark;

                    lblRequestNo.Text = IssueHead.PRH_ID.ToString();
                    lblRequestNo2.Text = IssueHead.PRH_ID.ToString();
                    hdnRequestID.Value = IssueHead.PRH_ID.ToString();
                    lblRequestDate.Text = IssueHead.RequestDate.Value.ToString("dd-MMM-yyyy");
                    lblRequestStatus.Text = IssueHead.IssueStatus.ToString();
                    lblSites.Text = IssueHead.SiteName.ToString();
                    hdnSiteID.Value = IssueHead.SiteID.ToString();
                    lblRequestType.Text = IssueHead.RequestType.ToString();
                    lblRequestedBy.Text = IssueHead.RequestByUserName.ToString();
                    Session["PORRequestID"] = IssueHead.PRH_ID.ToString();
                    GetReceiptHeadByIssueID();
                    GetReceiptHistoryByRequestID();
                }

            }
            catch { }
            finally { objService.Close(); }
        }

        protected void GetRequestHeadByRequestID()
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetRequestByRequestIDs_Result RequestHead = new POR_SP_GetRequestByRequestIDs_Result();
                RequestHead = objService.GetRequestSummayByRequestIDs(Session["PORRequestID"].ToString(), profile.DBConnection._constr).FirstOrDefault();
                if (RequestHead != null)
                {
                    lblRequestNo.Text = RequestHead.PRH_ID.ToString();
                    lblRequestNo2.Text = RequestHead.PRH_ID.ToString();
                    hdnRequestID.Value = RequestHead.PRH_ID.ToString();
                    lblRequestDate.Text = RequestHead.RequestDate.Value.ToString("dd-MMM-yyyy");
                    lblRequestStatus.Text = RequestHead.RequestStatus.ToString();
                    lblSites.Text = RequestHead.SiteName.ToString();
                    hdnSiteID.Value = RequestHead.SiteID.ToString();
                    lblRequestType.Text = RequestHead.RequestType.ToString();
                    lblRequestedBy.Text = RequestHead.RequestByUserName.ToString();
                    GetReceiptHistoryByRequestID();
                }

            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Receipt History ByRequestID
        protected void GetReceiptHistoryByRequestID()
        {
            iPartReceiptClient objService = new iPartReceiptClient();
            try
            {
                iframePORReceiptSummary.Attributes.Add("src", "../PowerOnRent/GridReceiptSummary.aspx?FillBy=RequestID");
            }
            catch { }
            finally { }
        }
        #endregion

        protected void divDisabled()
        {
            if (ddlStatus.Items.Count > 0)
            {
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(true, 'divReceiptDetail')", true);
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                }
                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(false, 'divReceiptDetail')", true); }
            }
        }
    }
}

