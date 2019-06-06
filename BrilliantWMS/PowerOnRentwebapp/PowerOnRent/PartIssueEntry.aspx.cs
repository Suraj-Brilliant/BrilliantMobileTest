using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.PORServicePartIssue;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.ProductSubCategoryService;
using System.Data;

namespace BrilliantWMS.PowerOnRent
{
    public partial class PartIssueEntry : System.Web.UI.Page
    {
        static string ObjectName = "IssuePartDetail";
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Dispatch";
            if (!IsPostBack)
            {
                Toolbar1.SetUserRights("MaterialIssue", "EntryForm", "");
                if (Session["PORIssueID"] != null)
                {
                    if (Session["PORIssueID"].ToString() != "0")
                    {
                        GetIssueHead();
                    }
                    else
                    {
                        //WMpageAddNew();
                        DisplayRequestData();
                    }
                }
                else if (Session["PORRequestID"] != null)
                {
                    if (Session["PORRequestID"].ToString() != "0")
                    {
                        DisplayRequestData();
                    }
                }
                //Add by Suresh
                binddropdown();
                gvApprovalRemarkBind();
                divVisibility();
            }
            UC_IssueDate.DateIsRequired(true, "", "");
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        #endregion

        #region Toolbar Code
        [WebMethod]
        public static void WMpageAddNew()
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["PORIssueID"] = "0";
                HttpContext.Current.Session["PORstate"] = "Add";
                objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
               // objService.GetIssuePartDetailByRequestID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr).ToList();
            }
            catch { }
            finally
            {
                objService.Close();
            }
        }

        [WebMethod]
        public static PORtMINHead WMSetSessionIssue(string InvokerObject, long IssueID, string state)
        {
            iPartIssueClient objService = new iPartIssueClient();
            PORtMINHead IssueHead = new PORtMINHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["PORIssueID"] = IssueID;
                HttpContext.Current.Session["PORstate"] = state;
                objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                objService.GetIssuePartDetailByIssueID(IssueID, HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr).ToList();
                IssueHead = objService.GetIssueHeadByIssueID(IssueID, profile.DBConnection._constr);

            }
            catch { }
            finally { objService.Close(); }
            return IssueHead;
        }
        #endregion

        #region Issue Head
        protected void GetIssueHead()
        {
            iPartIssueClient objServiceIssue = new iPartIssueClient();
            iPartRequestClient objServiceRequest = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                PORtMINHead IssueHead = new PORtMINHead();
                IssueHead = objServiceIssue.GetIssueHeadByIssueID(Convert.ToInt64(HttpContext.Current.Session["PORIssueID"]), profile.DBConnection._constr);

                ddlStatus.DataSource = WMFillStatus();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(IssueHead.StatusID.ToString()));

                lblIssueNo.Text = IssueHead.MINH_ID.ToString();
                UC_IssueDate.Date = DateTime.Now;
                if (IssueHead.IssueDate != null) UC_IssueDate.Date = IssueHead.IssueDate;
                FillUserList(1);
                ddlIssuedBy.SelectedIndex = ddlIssuedBy.Items.IndexOf(ddlIssuedBy.Items.FindByValue(IssueHead.IssuedByUserID.ToString()));
                if (ddlIssuedBy.SelectedIndex == 0)
                {
                    ddlIssuedBy.SelectedIndex = ddlIssuedBy.Items.IndexOf(ddlIssuedBy.Items.FindByValue(profile.Personal.UserID.ToString()));
                }
                /*Transport detail*/
                if (IssueHead.AirwayBill != null) txtAirwayBill.Text = IssueHead.AirwayBill;
                if (IssueHead.ShippingType != null) txtShippingType.Text = IssueHead.ShippingType;
                UC_ShippingDate.Date = DateTime.Now;
                if (IssueHead.ShippingDate != null) UC_ShippingDate.Date = IssueHead.ShippingDate;
                UC_ExpDeliveryDate.Date = DateTime.Now;
                if (IssueHead.ExpectedDelDate != null) UC_ExpDeliveryDate.Date = IssueHead.ExpectedDelDate;
                if (IssueHead.TransporterName != null) txtTransporterName.Text = IssueHead.TransporterName;
                if (IssueHead.Remark != null) txtIssueRemark.Text = IssueHead.Remark;

                FillGrid1ByIssueID(IssueHead.MINH_ID);
                Session["PORRequestID"] = IssueHead.PRH_ID.ToString();
                DisplayRequestData();
              //  iframePORIssue.Attributes.Add("src", "../PowerOnRent/GridIssueSummary.aspx?FillBy=RequestID");
                divDisabled();
            }
            catch { }
            finally { objServiceIssue.Close(); }
        }

        [WebMethod]
        public static string WMSaveIssueHead(object objIssue)
        {
            string result = "";
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                PORtMINHead IssueHead = new PORtMINHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objIssue;

                if (HttpContext.Current.Session["PORIssueID"] != null)
                {
                    if (HttpContext.Current.Session["PORIssueID"].ToString() == "0")
                    {
                        IssueHead.CreatedBy = profile.Personal.UserID;
                        IssueHead.CreationDt = DateTime.Now;
                    }
                    else
                    {
                        IssueHead = objService.GetIssueHeadByIssueID(Convert.ToInt64(HttpContext.Current.Session["PORIssueID"]), profile.DBConnection._constr);
                        IssueHead.LastModifiedBy = profile.Personal.UserID;
                        IssueHead.LastModifiedDt = DateTime.Now;
                    }

                    IssueHead.SiteID = Convert.ToInt64(dictionary["SiteID"]);
                    IssueHead.PRH_ID = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());
                    IssueHead.MIN_No = "N/A";
                    IssueHead.IssueDate = Convert.ToDateTime(dictionary["IssueDate"]);
                    IssueHead.IssuedByUserID = Convert.ToInt64(dictionary["IssuedByUserID"]);
                    IssueHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);

                    IssueHead.AirwayBill = dictionary["AirwayBill"].ToString();
                    IssueHead.ShippingType = dictionary["ShippingType"].ToString();
                    if (dictionary["ShippingDate"].ToString() != "") IssueHead.ShippingDate = Convert.ToDateTime(dictionary["ShippingDate"].ToString());
                    if (dictionary["ExpectedDelDate"].ToString() != "") IssueHead.ExpectedDelDate = Convert.ToDateTime(dictionary["ExpectedDelDate"].ToString());
                    IssueHead.TransporterName = dictionary["TransporterName"].ToString();
                    IssueHead.Remark = dictionary["Remark"].ToString();
                    IssueHead.IsSubmit = Convert.ToBoolean(dictionary["IsSubmit"]);
                    if (IssueHead.StatusID != 1 && IssueHead.StatusID != 10)
                    {
                        IssueHead.StatusID = objService.GetStatusOfIssueHead(HttpContext.Current.Session.SessionID.ToString(), profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt64(IssueHead.PRH_ID), profile.DBConnection._constr);
                    }
                    long IssueID = objService.SetIntoMINHead(IssueHead, profile.DBConnection._constr);

                    if (IssueID > 0)
                    {
                        string IssueStatus = "NA";
                        if (IssueHead.StatusID != 1 && IssueHead.StatusID != 10) IssueStatus = "Issue";
                        objService.FinalSaveIssuePartDetail(HttpContext.Current.Session.SessionID, ObjectName, IssueID, Convert.ToInt64(IssueHead.PRH_ID), profile.Personal.UserID.ToString(), IssueStatus, profile.DBConnection._constr);
                        result = "Material Issue record saved successfully";
                    }
                }
            }
            catch { result = "Some error occurred"; }
            finally { objService.Close(); }
            return result;
        }
        #endregion

        #region Issue Part Details
        protected void FillGrid1ByIssueID(long IssueID)
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<POR_SP_GetPartDetails_OfMIN_Result> IssuePartList = new List<POR_SP_GetPartDetails_OfMIN_Result>();
                IssuePartList = objService.GetIssuePartDetailByIssueID(IssueID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr).ToList();

                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = IssuePartList;
                Grid1.DataBind();

            }
            catch { }
            finally { objService.Close(); }
        }

        protected void FillGrid1ByRequestID(long RequestID)
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<POR_SP_GetPartDetails_OfMIN_Result> IssuePartList = new List<POR_SP_GetPartDetails_OfMIN_Result>();
                IssuePartList = objService.GetIssuePartDetailByRequestID(RequestID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr).ToList();

                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = IssuePartList;
                Grid1.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<POR_SP_GetPartDetails_OfMIN_Result> IssuePartList = new List<POR_SP_GetPartDetails_OfMIN_Result>();
                if (hdnPendingSelectedRec.Value == "")
                {
                    IssuePartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdnPendingSelectedRec.Value != "")
                {
                    IssuePartList = objService.AddPartIntoIssue_TempData(hdnPendingSelectedRec.Value, 0, 0, "true", Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    hdnPendingSelectedRec.Value = "";
                }
                Grid1.DataSource = null;
                Grid1.DataBind();

               // IssuePartList.Where(r => r.Installable == Convert.ToBoolean(1)).FirstOrDefault();                 

                Grid1.DataSource = IssuePartList;
                Grid1.DataBind();

              
            }
            catch { }
            finally { objService.Close(); }
        }


        // add by suresh =====>
        protected void Grid1_RowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            try
            {
                // if (e.Row.RowType == DataControlRowType.DataRow)
                //if (Obout.Grid.GridRowType == DataControlRowType.DataRow)
                // {
                ImageButton imgbtndetails = (ImageButton)e.Row.Cells[0].FindControl("imgbtndetails");
               
                if (e.Row.Cells[11].Text == "False")
                {
                    imgbtndetails.Visible = false;
                    imgbtndetails.Enabled = false;
                }
                else
                {
                    imgbtndetails.Visible = true;
                    imgbtndetails.Enabled = true;
                }
                //}
            }
            catch { }
            finally { }

        }
        // <===== add by suresh

        [WebMethod]
        public static string WMUpdateIssueQty(object objIssue)
        {
            string RemaningQty = "";
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objIssue;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetails_OfMIN_Result IssuePart = new POR_SP_GetPartDetails_OfMIN_Result();
                IssuePart.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                IssuePart.IssuedQty = Convert.ToDecimal(dictionary["IssuedQty"]);

                RemaningQty = objService.UpdatePartIssue_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), IssuePart, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return RemaningQty;
        }

        //add by suresh
        [WebMethod]
        public static string WMUpdateHQStock(object objHQ)
        {
            string HQQty = "";
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objHQ;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetails_OfMIN_Result IssuePart = new POR_SP_GetPartDetails_OfMIN_Result();
                IssuePart.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                IssuePart.IssuedQty = Convert.ToDecimal(dictionary["IssuedQty"]);

                HQQty = objService.UpdateHQStock_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), IssuePart, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return HQQty;
        }

        [WebMethod]
        public static void WMRemovePartFromIssue(Int32 Sequence)
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                CustomProfile profile = CustomProfile.GetProfile();
                objService.RemovePartFromIssue_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
        }

        #endregion

        #region Pending Issue Part List
        protected void Grid2_OnRebind(object sender, EventArgs e)
        {
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Grid2.DataSource = null;
                Grid2.DataBind();
                Grid2.DataSource = objService.GetPendingIssuePartList(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt64(Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                Grid2.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Fill Dropdown
        [WebMethod]
        public static List<BrilliantWMS.PORServicePartIssue.mStatu> WMFillStatus()
        {
            string state = HttpContext.Current.Session["PORstate"].ToString();
            iPartIssueClient objService = new iPartIssueClient();
            List<BrilliantWMS.PORServicePartIssue.mStatu> StatusList = new List<BrilliantWMS.PORServicePartIssue.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if ((HttpContext.Current.Session["PORIssueID"] != null) && (state == "Add" || state == "Edit"))
                {
                    BrilliantWMS.PORServicePartIssue.mStatu IssueStatus = new BrilliantWMS.PORServicePartIssue.mStatu() { ID = 100, Status = "Issue" };
                    StatusList = objService.GetStatusListForIssue("All", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    StatusList.Insert(1, IssueStatus);
                }
                else if (HttpContext.Current.Session["PORIssueID"].ToString() != "0" && state == "View")
                {
                    StatusList = objService.GetStatusListForIssue("", "", 0, profile.DBConnection._constr).ToList();
                }

                BrilliantWMS.PORServicePartIssue.mStatu select = new BrilliantWMS.PORServicePartIssue.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return StatusList;
        }

        public void FillUserList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListBySiteID(SiteID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Distinct().ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);

                ddlIssuedBy.DataSource = null;
                ddlIssuedBy.DataBind();
                ddlIssuedBy.DataSource = UsersList;
                ddlIssuedBy.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Request Head
        [WebMethod]
        public static List<POR_SP_GetRequestByRequestIDs_Result> WMGetRequestHead(string RequestIDs)
        {
            iPartRequestClient objService = new iPartRequestClient();
            List<POR_SP_GetRequestByRequestIDs_Result> RequestHead = new List<POR_SP_GetRequestByRequestIDs_Result>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                RequestHead = objService.GetRequestSummayByRequestIDs(RequestIDs, profile.DBConnection._constr).ToList();
            }
            catch { }
            finally { objService.Close(); }
            return RequestHead;
        }

        protected void DisplayRequestData()
        {
            iPartRequestClient objService = new iPartRequestClient();
            iPartIssueClient objServiceIssue = new iPartIssueClient();
            POR_SP_GetRequestByRequestIDs_Result RequestHead = new POR_SP_GetRequestByRequestIDs_Result();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                RequestHead = objService.GetRequestSummayByRequestIDs(Session["PORRequestID"].ToString(), profile.DBConnection._constr).FirstOrDefault();
                hdnRequestID.Value = RequestHead.PRH_ID.ToString();
                hdnSiteID.Value = RequestHead.SiteID.ToString();
                lblRequestNo2.Text = RequestHead.PRH_ID.ToString();
                lblRequestNo3.Text = RequestHead.PRH_ID.ToString();
                lblSites.Text = RequestHead.SiteName;
                lblRequestStatus.Text = RequestHead.RequestStatus;
                lblRequestDate.Text = RequestHead.RequestDate.Value.ToString("dd-MMM-yyyy");
                lblRequestType.Text = RequestHead.RequestType;
                lblRequestedBy.Text = RequestHead.RequestByUserName;
                if (WMGetIssueSummaryByRequestID() == 0)
                {
                    FillGrid1ByRequestID(Convert.ToInt64(Session["PORRequestID"].ToString()));
                    HttpContext.Current.Session["PORstate"] = "Add";
                    HttpContext.Current.Session["PORIssueID"] = "0";
                    lblIssueNo.Text = "Generate when Save";

                    ddlStatus.DataSource = WMFillStatus();
                    ddlStatus.DataBind();
                    if (ddlStatus.Items.Count > 1) ddlStatus.SelectedIndex = 1;
                    FillUserList(1);
                    ddlIssuedBy.SelectedIndex = ddlIssuedBy.Items.IndexOf(ddlIssuedBy.Items.FindByValue(profile.Personal.UserID.ToString()));

                    UC_IssueDate.Date = DateTime.Now;
                    UC_ShippingDate.Date = DateTime.Now;
                    UC_ExpDeliveryDate.Date = DateTime.Now;
                }
                else
                {
                    FillUserList(1);
                    ddlIssuedBy.SelectedIndex = ddlIssuedBy.Items.IndexOf(ddlIssuedBy.Items.FindByValue(profile.Personal.UserID.ToString()));
                    //add by suresh
                    ddlStatus.DataSource= WMFillStatus();
                    ddlStatus.DataBind();

                    UC_IssueDate.Date = DateTime.Now;
                    UC_ShippingDate.Date = DateTime.Now;
                    UC_ExpDeliveryDate.Date = DateTime.Now;
                }

                bool AddNewAccess = false;
                AddNewAccess = objServiceIssue.CheckPendingIssueListToDecideAddNewAccess(RequestHead.PRH_ID, profile.DBConnection._constr);
                if (Toolbar1.GetUserRightsByObjectName("MaterialIssue", "", "").Add == true)
                {
                    if (AddNewAccess == false)
                    {
                        Toolbar1.SetAddNewRight(false, "Here is no pending issue against current request", "#");
                    }
                }
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region IssueSummary
        [WebMethod]
        public static int WMGetIssueSummaryByRequestID()
        {
            int result = 0;
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                result = objService.GetIssueSummayByRequestID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr).Count();
            }
            catch { }
            finally { objService.Close(); }
            return result;
        }
        #endregion

        protected void divDisabled()
        {
            if (ddlStatus.Items.Count > 0)
            {
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(true, 'divIssueDetail')", true);
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                    Toolbar1.SetEditRight(false, "NotAllowed");

                    PopUP.Visible = false;
                }
                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(false, 'divIssueDetail')", true); }
            }
        } 
        
         

        #region Add By Suresh

        protected void imgbtndetails_OnClick(object sender, ImageClickEventArgs e)
        {
            PopUP.Visible=true;
            ImageButton btndetails =(ImageButton)sender;
            //GridViewRow gvrow = (GridViewRow)btndetails.NamingContainer;
            Obout.Grid.TemplateContainer gvrow = (Obout.Grid.TemplateContainer)btndetails.NamingContainer;

            hdnprodID.Value = btndetails.ToolTip.ToString();

            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            GetProductDetail obj = new GetProductDetail();
            obj = productClient.GetProductDetailByProductID(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
            productClient.Close();
            if (obj.Installable == Convert.ToBoolean(0))
            { 
                this.ModelPopUp.Hide();
                //WebMsgBox.MsgBox.Show("Not Allowed");
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showAlert('Not Allowed!', 'Error', '#');", true);
            }
            else
            {
                if (obj.ProductTypeID != null) ddlProductType.SelectedIndex = ddlProductType.Items.IndexOf(ddlProductType.Items.FindByValue(obj.ProductTypeID.Value.ToString()));

                if (obj.ProductCategoryID != null) ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(obj.ProductCategoryID.Value.ToString()));
                BindProductSubCategory();
                if (obj.ProductSubCategoryID != null) ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByValue(obj.ProductSubCategoryID.Value.ToString()));
                if (obj.ProductCode != null) txtProductCode.Text = obj.ProductCode.ToString();
                if (obj.Name != null) txtProductName.Text = obj.Name.ToString();
                if (obj.UOMID != null) ddlUOM.SelectedIndex = ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(obj.UOMID.Value.ToString()));
                if (obj.PrincipalPrice != null) txtPrincipalPrice.Text = obj.PrincipalPrice.ToString();
                if (obj.Description != null) txtPrdDesc.Text = obj.Description.ToString();

                this.ModelPopUp.Show();
            }
        }


        public void binddropdown()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            ddlProductType.DataSource = productClient.GetProductTypeList(profile.DBConnection._constr);
            ddlProductType.DataBind();

            ListItem lst1 = new ListItem();
            lst1.Text = "-Select-";
            lst1.Value = "0";
            ddlProductType.Items.Insert(0, lst1);

            ddlUOM.SelectedIndex = -1;
            ddlUOM.DataSource = productClient.GetProductUOMList(profile.DBConnection._constr);
            ddlUOM.DataBind();
            ListItem lst3 = new ListItem();
            lst3.Text = "-Select-";
            lst3.Value = "0";
            ddlUOM.Items.Insert(0, lst3);
            productClient.Close();

            BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient productcategoryClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();

            ddlCategory.SelectedIndex = -1;
            ddlCategory.DataSource = productcategoryClient.GetProductCategoryList(profile.DBConnection._constr);
            ddlCategory.DataBind();
            productcategoryClient.Close();
            ListItem lst2 = new ListItem();
            lst2.Text = "-Select-";
            lst2.Value = "0";
            ddlCategory.Items.Insert(0, lst2);
        }


        [WebMethod]
        public static List<vGetProductSubCagetoryList> PMprint_ProductSubCategory(long ProductSubCategoryID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient productsubcategoryClient = new BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient();
            List<vGetProductSubCagetoryList> SubCategoryList = new List<vGetProductSubCagetoryList>();
            SubCategoryList = productsubcategoryClient.GetProductSubCategoryByProductCategoryID(ProductSubCategoryID, profile.DBConnection._constr).ToList();
            return SubCategoryList;
        }

        protected void BindProductSubCategory()
        {
            ddlSubCategory.Items.Clear();

            if (ddlCategory.SelectedIndex > 0)
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient productsubcategoryClient = new BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient();
                //ProductSubCategoryService.connectiondetails conn = new ProductSubCategoryService.connectiondetails() { DataBaseName = Profile.DataBase, DataSource = Profile.DataSource, DBPassword = Profile.DBPassword };
                ddlSubCategory.DataSource = productsubcategoryClient.GetProductSubCategoryByProductCategoryID(Convert.ToInt32(ddlCategory.SelectedItem.Value), profile.DBConnection._constr);
                ddlSubCategory.DataBind();
                productsubcategoryClient.Close();
            }
            if (ddlSubCategory.Items.Count > 0)
            {
                if (ddlSubCategory.Items[0].Text != "Not available")
                {
                    ListItem lst = new ListItem();
                    lst.Text = "-Select-";
                    lst.Value = "0";
                    ddlSubCategory.Items.Insert(0, lst);
                }
            }
        }


        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            try
            {
                string state;
                CustomProfile profile = CustomProfile.GetProfile();
                //if (checkduplicate() == "")
                //{
                    iProductMasterClient productClient = new iProductMasterClient();
                    mProduct obj = new mProduct();

                    state = "Edit";
                    obj = productClient.GetmProductToUpdate(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
                    obj.LastModifiedBy = profile.Personal.UserID.ToString();
                    obj.LastModifiedDate = DateTime.Now;

                    obj.ProductTypeID = Convert.ToInt64(ddlProductType.SelectedItem.Value);
                    obj.ProductCategoryID = Convert.ToInt64(ddlCategory.SelectedItem.Value);
                    if (ddlSubCategory.SelectedIndex > 0) obj.ProductSubCategoryID = Convert.ToInt64(ddlSubCategory.SelectedItem.Value);
                    obj.ProductCode = txtProductCode.Text.ToString().Trim();
                    obj.Name = txtProductName.Text.ToString().Trim();
                    obj.UOMID = Convert.ToInt64(ddlUOM.SelectedItem.Value);
                    if (txtPrincipalPrice.Text == "") txtPrincipalPrice.Text = "0";
                    obj.PrincipalPrice = Convert.ToDecimal(txtPrincipalPrice.Text);
                    obj.Description = txtPrdDesc.Text.ToString().Trim();

                    obj.Installable = Convert.ToBoolean(0);

                    hdnprodID.Value = productClient.FinalSaveProductDetailByProductID(obj, profile.DBConnection._constr).ToString();

                    productClient.Close();

                   GetIssueHead();

                   this.ModelPopUp.Hide();
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "PartIssueEntry", "ProductSave");
            }
            finally
            {
            }
        }

        protected void btnTransferFromSite_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("../PowerOnRent/TransferFrmSite.aspx",true);
        }

        protected void btnHQGRN_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("../PowerOnRent/HQGoodsReceiptEntry.aspx", true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureChildControls();
        }
        //public string checkduplicate()
        //{
        //    try
        //    {
        //        CustomProfile profile = CustomProfile.GetProfile();
        //        iProductMasterClient productclient = new iProductMasterClient();
        //        string result = "";

        //        result = productclient.checkDuplicateRecord(txtProductName.Text.Trim(), profile.DBConnection._constr);
        //        if (result != string.Empty)
        //        {
        //            WebMsgBox.MsgBox.Show(result);
        //        }
        //        txtProductName.Focus();
        //        return result;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Login.Profile.ErrorHandling(ex, this, "PartRequestEntry", "checkDuplicate");
        //        string result = "";
        //        return result;
        //    }
        //    finally
        //    {
        //    }
        //}

        protected void gvApprovalRemarkBind()
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            gvApprovalRemark.DataSource = null;
            gvApprovalRemark.DataBind();
            DataSet dsGetApprovalDetail = new DataSet();
            dsGetApprovalDetail = objService.GetApprovalDetailsAllApproved(profile.DBConnection._constr);
            gvApprovalRemark.DataSource = dsGetApprovalDetail;
            gvApprovalRemark.DataBind();
        }

        protected void divVisibility()
        {
            linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
            divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";

            linkRequest.Attributes["innerHTML"] = "Expand";
            divRequestDetail.Attributes["class"] = "divDetailCollapse";
        }
        #endregion

    }
}
