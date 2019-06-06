using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.PORServiceEngineMaster;
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

namespace BrilliantWMS.PowerOnRent
{
    public partial class PartRequestEntry : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string ObjectName = "RequestPartDetail";
        string SelTemplateID = "";
        static long UOMID = 0;

        #region Page Events

        protected void Page_LoadComplete(object sender, EventArgs e)
        {

            //if(hdnSelTemplateID.Value != "")
            //    SelTemplateID = hdnSelTemplateID.Value.ToString();

            if (Session["TemplateID"] != null)
            {
                hdnSelTemplateID.Value = Session["TemplateID"].ToString();
            }

            if (!IsPostBack)
            {
                FillSites(); UC_AttachmentDocument1.ClearDocument("RequestPartDetail");
                if (Session["PORRequestID"] != null)
                {
                    {
                        if (Session["PORRequestID"].ToString() != "0")
                        {
                            lblApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

                            // UC_ExpDeliveryDate.startdate(DateTime.Now);
                            //lblExpDeliveryDate.Text = //DateTime.TryParse(lblApprovalDate.Text.ToString(),"");
                            GetRequestHead();
                            gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()));
                            GVInboxPOR_OnRebind(sender, e);
                        }
                        else if (Session["TemplateID"] != null)
                        {
                            GetTemplateDetails(hdnSelTemplateID.Value);
                            UC_ExpDeliveryDate.startdate(DateTime.Now);
                            string mdd = hdnMaxDeliveryDays.Value;
                            if (mdd == "") { }
                            else
                            {
                                UC_ExpDeliveryDate.enddate(DateTime.Now.AddDays(int.Parse(mdd)));
                            }
                            ddlStatus.DataSource = WMFillStatus();
                            ddlStatus.DataBind();
                        }
                        else
                        {
                            WMpageAddNew();
                            UC_ExpDeliveryDate.startdate(DateTime.Now);
                            ddlStatus.DataSource = WMFillStatus();
                            ddlStatus.DataBind();
                        }
                    }
                }

                divVisibility();

            }
            UC_DateRequestDate.DateIsRequired(true, "", "");

            //Add By Suresh
            // ModelPopup1.Hide();

            //Add By Suresh
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        //Add By Suresh
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ModelPopup1.Hide();
            // ModalPopupTemplate.Hide();
        }
        //Add By Suresh

        protected void Page_Load(Object sender, EventArgs e)
        {
            //UCFormHeader1.FormHeaderText = "Material Request";

            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }

            loadstring();


            if (!IsPostBack)
            {
                iPartRequestClient objService = new iPartRequestClient();
                CustomProfile profile = CustomProfile.GetProfile();
                //  ddlSites.Attributes.Add("onchange", "jsFillUsersList();jsFillEnginList();");
                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");
                objService.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }

        }
        #endregion

        #region Toolbar Code
        [WebMethod]
        public static void WMpageAddNew()
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["PORRequestID"] = 0;
                HttpContext.Current.Session["PORstate"] = "Add";
                HttpContext.Current.Session["TemplateID"] = "0";
                // objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                objService.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);                
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Fill DropDown
        protected void FillSites()
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // ddlStatus.Enabled = false;
                List<mCompany> CompanyLst = new List<mCompany>();
                string UserType = profile.Personal.UserType.ToString();
                long UID = profile.Personal.UserID;
                if (UserType == "Admin")
                {
                    //  CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                    //CompanyLst = objService.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();

                    CompanyLst = objService.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
                }
                else if (UserType == "User" || UserType == "Requester And Approver" || UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver" || profile.Personal.UserType == "Approver")
                {
                    CompanyLst = objService.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();
                }
                else
                {
                    CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                }
                ddlCompany.DataSource = CompanyLst;
                ddlCompany.DataBind();


                if (UserType == "Admin")
                {
                    ListItem lstCmpny = new ListItem { Text = "-Select-", Value = "0" };
                    ddlCompany.Items.Insert(0, lstCmpny);
                    if (ddlCompany.Items.Count > 0) ddlCompany.SelectedIndex = 1;
                    hdnselectedCompany.Value = ddlCompany.SelectedValue.ToString();

                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    //SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    int Cmpny = int.Parse(hdnselectedCompany.Value);
                    SiteLst = WMGetSelDept(Cmpny, profile.Personal.UserID);

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 0;

                    hdnselectedDept.Value = ddlSites.SelectedValue.ToString(); Session["DeptID"] = ddlSites.SelectedValue.ToString();
                    hdnMaxDeliveryDays.Value = Convert.ToString(WMGetMaxDeliveryDays(long.Parse(hdnselectedDept.Value)));

                    //  long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString();
                    //    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    //ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    //ddlContact1.DataBind();
                    //ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlContact1.Items.Insert(0, lstContact);

                    //ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    //ddlAddress.DataBind();
                    //ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlAddress.Items.Insert(0, lstAdrs);
                }
                else if (UserType == "User" || UserType == "Requester And Approver" || UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver" || profile.Personal.UserType == "Approver")
                {
                    ddlCompany.Enabled = false;

                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    //SiteLst = UCCommonFilter.GetDepartmentListUserWise(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 0;

                    //  ddlSites.Enabled = false;
                    long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString();
                    hdnMaxDeliveryDays.Value = Convert.ToString(WMGetMaxDeliveryDays(long.Parse(hdnselectedDept.Value)));
                    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    ddlContact1.DataBind();
                    ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    ddlContact1.Items.Insert(0, lstContact);

                    ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    ddlAddress.DataBind();
                    ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    ddlAddress.Items.Insert(0, lstAdrs);
                }
                else
                {
                    ListItem lstCmpny = new ListItem { Text = "-Select-", Value = "0" };
                    ddlCompany.Items.Insert(0, lstCmpny);
                    if (ddlCompany.Items.Count > 0) ddlCompany.SelectedIndex = 1;
                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 0;

                    long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString(); hdnMaxDeliveryDays.Value = Convert.ToString(WMGetMaxDeliveryDays(long.Parse(hdnselectedDept.Value)));
                    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    ddlContact1.DataBind();
                    ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    ddlContact1.Items.Insert(0, lstContact);

                    ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    ddlAddress.DataBind();
                    ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    ddlAddress.Items.Insert(0, lstAdrs);
                }

                fillPaymentMethod(long.Parse(hdnselectedDept.Value));
            }
            catch { }
            finally { objService.Close(); }
        }

        public void fillPaymentMethod(long selectedDept)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds = objService.GetDeptWisePaymentMethod(selectedDept, profile.DBConnection._constr);
                ddlPaymentMethod.DataSource = ds;
                ddlPaymentMethod.DataBind();
                //ListItem lstpm = new ListItem { Text = "--Select--", Value = "0" };
                //ddlPaymentMethod.Items.Insert(0, lstpm);

                ddlFOC.DataSource = WMGetCostCenter(selectedDept);
                ddlFOC.DataBind();
                ListItem lstfoc = new ListItem { Text = "--Select--", Value = "0" };
                ddlFOC.Items.Insert(0, lstfoc);
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static List<BrilliantWMS.PORServicePartRequest.mStatu> WMFillStatus()
        {
            string state = HttpContext.Current.Session["PORstate"].ToString();
            iPartRequestClient objService = new iPartRequestClient();
            List<BrilliantWMS.PORServicePartRequest.mStatu> StatusList = new List<BrilliantWMS.PORServicePartRequest.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                if (HttpContext.Current.Session["PORRequestID"].ToString() == "0" && state == "Add")
                {
                    //if (profile.Personal.UserType == "User")
                    //{
                    //    StatusList = objService.GetStatusListForRequest("Request", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    //}
                    //else
                    //{
                    StatusList = objService.GetStatusListForRequest("All,Request", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    StatusList = StatusList.Where(s => s.ID == 1 || s.ID == 2).ToList();
                    // }
                }
                else if (HttpContext.Current.Session["PORRequestID"].ToString() != "0" && state == "Edit")
                {
                    if (HttpContext.Current.Session["OrderStatus"].ToString() == "1")
                    {
                        StatusList = objService.GetStatusListForRequest("All,Request", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        StatusList = StatusList.Where(s => s.ID == 1 || s.ID == 2).ToList();
                    }
                    else
                    {
                        StatusList = objService.GetStatusListForRequest("All,Request", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                }
                else if (HttpContext.Current.Session["PORRequestID"].ToString() != "0" && state == "View")
                {
                    StatusList = objService.GetStatusListForRequest("", "", 0, profile.DBConnection._constr).ToList();
                }
               
                BrilliantWMS.PORServicePartRequest.mStatu select = new BrilliantWMS.PORServicePartRequest.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return StatusList;
        }

        public List<vGetUserProfileByUserID> FillCurrentUserList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListBySiteID(SiteID, profile.DBConnection._constr).ToList();
                //UsersList = UsersList.GroupBy(x => x.userID).Select(x => x.FirstOrDefault()).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }

        [WebMethod]
        public static List<vGetUserProfileByUserID> WMFillUserList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListBySiteID(SiteID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.GroupBy(x => x.userID).Select(x => x.FirstOrDefault()).ToList();
                //  UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }

        [WebMethod]
        public static List<BrilliantWMS.PORServiceUCCommonFilter.v_GetEngineDetails> WMFillEnginList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<BrilliantWMS.PORServiceUCCommonFilter.v_GetEngineDetails> EngineList = new List<BrilliantWMS.PORServiceUCCommonFilter.v_GetEngineDetails>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                EngineList = objService.GetEngineOfSite(SiteID.ToString(), profile.DBConnection._constr).ToList();
                BrilliantWMS.PORServiceUCCommonFilter.v_GetEngineDetails select = new BrilliantWMS.PORServiceUCCommonFilter.v_GetEngineDetails() { ID = 0, Container = "-Select-" };
                EngineList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return EngineList;
        }

        [WebMethod]
        public static List<mTerritory> WMGetDept(int Cmpny)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            // SiteLst = UCCommonFilter.GetDepartmentList(Cmpny, profile.DBConnection._constr).ToList();
            if (profile.Personal.UserType == "Admin")
            {
                SiteLst = UCCommonFilter.GetAddedDepartmentList(Cmpny, profile.Personal.UserID, profile.DBConnection._constr).ToList();
            }
            else
            {
                SiteLst = UCCommonFilter.GetDepartmentList(Cmpny, profile.DBConnection._constr).ToList();
            }
            return SiteLst;
        }

        [WebMethod]
        public static List<tContactPersonDetail> WMGetContactPersonLst(long Dept)
        {
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            ConPerLst = UCCommonFilter.GetContactPersonList(Dept, profile.DBConnection._constr).ToList();

            return ConPerLst;
        }

        [WebMethod]
        public static List<tContactPersonDetail> WMGetContactPerson2Lst(long Dept, long Cont1)
        {
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            ConPerLst = UCCommonFilter.GetContactPerson2List(Dept, Cont1, profile.DBConnection._constr).ToList();

            return ConPerLst;
        }

        [WebMethod]
        public static List<tAddress> WMGetDeptAddress(long Dept)
        {
            List<tAddress> AdrsLst = new List<tAddress>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            AdrsLst = UCCommonFilter.GetDeptAddressList(Dept, profile.DBConnection._constr).ToList();

            return AdrsLst;
        }
        #endregion

        #region RequestHead
        protected void GetRequestHead()
        {
            iPartRequestClient objService = new iPartRequestClient();
            //PORtPartRequestHead RequestHead = new PORtPartRequestHead();
            tOrderHead RequestHead = new tOrderHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //RequestHead = objService.GetRequestHeadByRequestID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                RequestHead = objService.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                hdnOrderStatus.Value = RequestHead.Status.ToString(); Session["OrderStatus"] = hdnOrderStatus.Value;
                FillGrid1ByRequestID(RequestHead.Id, Convert.ToInt64(RequestHead.StoreId));

                txtTitle.Text = RequestHead.Title;

                long SiteID = long.Parse(RequestHead.StoreId.ToString());

                iUCCommonFilterClient objCommon = new iUCCommonFilterClient();
                long CompanyID = objCommon.GetCompanyIDFromSiteID(SiteID, profile.DBConnection._constr);

                //if (profile.Personal.UserType != "User" || profile.Personal.UserType != "Requester And Approver" || profile.Personal.UserType != "Requester" || profile.Personal.UserType != "Requestor" || profile.Personal.UserType != "Requestor And Approver" || profile.Personal.UserType != "Approver")
                if (profile.Personal.UserType == "Super Admin" || profile.Personal.UserType == "Admin")
                {
                    List<mCompany> CompanyLst = new List<mCompany>();

                    CompanyLst = objCommon.GetCompanyName(profile.DBConnection._constr).ToList();
                    ddlCompany.DataSource = CompanyLst;
                    ddlCompany.DataBind();


                    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyID.ToString()));
                    ddlCompany.Enabled = false;

                    long UID = profile.Personal.UserID;
                    List<mTerritory> SiteLst = new List<mTerritory>();
                    if (profile.Personal.UserType == "Admin")
                    {
                        SiteLst = objCommon.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    }
                    else if (profile.Personal.UserType == "Super Admin")
                    {
                        SiteLst = objCommon.GetAllDepartmentList(profile.DBConnection._constr).ToList();
                    }
                    //  SiteLst = objCommon.GetDepartmentListUserWise(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    //  ddlSites.Enabled = false;
                }

                ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(RequestHead.StoreId.ToString()));
                if (hdnOrderStatus.Value == "1") { lblRequestNo.Text = "Generate when Save"; }
                else
                {
                    lblRequestNo.Text = RequestHead.OrderNo.ToString(); //RequestHead.Id.ToString();
                }
                ddlStatus.DataSource = WMFillStatus();
                ddlStatus.DataBind();
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "divVisibility123", "divVisibility()");

                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(RequestHead.Status.ToString())); 

                UC_DateRequestDate.Date = RequestHead.Orderdate;
                txtRequestDate.Text = Convert.ToString(RequestHead.Orderdate.Value.ToString("dd-MMM-yyyy"));

                //      ddlRequestType.SelectedIndex = ddlRequestType.Items.IndexOf(ddlRequestType.Items.FindByValue(RequestHead.Priority.ToString()));
                if (hdnOrderStatus.Value == "1")
                {
                    ddlRequestByUserID.DataSource = FillCurrentUserList(Convert.ToInt64(hdnselectedDept.Value));
                }
                else
                {
                    ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(RequestHead.StoreId)); hdnselectedDept.Value = RequestHead.StoreId.ToString();
                }
                ddlRequestByUserID.DataBind();
                ddlRequestByUserID.SelectedIndex = ddlRequestByUserID.Items.IndexOf(ddlRequestByUserID.Items.FindByValue(RequestHead.RequestBy.ToString()));

                txtRemark.Text = RequestHead.Remark;

                txtCustOrderRefNo.Text = RequestHead.OrderNumber;
                UC_ExpDeliveryDate.Date = RequestHead.Deliverydate; //if (RequestHead.Status >= 2) { Page.ClientScript.RegisterStartupScript(this.GetType(), "reset", " disableExpDeliveryDate();", true); }

                if (profile.Personal.UserType != "User" || profile.Personal.UserType != "Requester And Approver" || profile.Personal.UserType != "Requester" || profile.Personal.UserType != "Requestor" || profile.Personal.UserType != "Requestor And Approver" || profile.Personal.UserType != "Approver")
                {
                    ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(SiteID);
                    ddlAddress.DataBind();
                    ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    ddlAddress.Items.Insert(0, lstAdrs);

                    // ddlContact1.DataSource = WMGetContactPersonLst(SiteID);
                    ddlContact1.DataSource = WMGetContactPersonLst(CompanyID);
                    ddlContact1.DataBind();
                    ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    ddlContact1.Items.Insert(0, lstContact);
                }
                ddlAddress.SelectedIndex = ddlAddress.Items.IndexOf(ddlAddress.Items.FindByValue(RequestHead.AddressId.ToString())); hdnSelAddress.Value = RequestHead.AddressId.ToString();
                ddlContact1.SelectedIndex = ddlContact1.Items.IndexOf(ddlContact1.Items.FindByValue(RequestHead.ContactId1.ToString())); hdnselectedCont1.Value = RequestHead.ContactId1.ToString();
                // ddlContact2.DataSource = WMGetContactPerson2Lst(Convert.ToInt64(RequestHead.StoreId), Convert.ToInt64(ddlContact1.SelectedIndex));
                ddlContact2.DataSource = WMGetContactPerson2Lst(CompanyID, Convert.ToInt64(ddlContact1.SelectedIndex));
                ddlContact2.DataBind();
                ddlContact2.SelectedIndex = ddlContact2.Items.IndexOf(ddlContact2.Items.FindByValue(RequestHead.ContactId2.ToString())); //hdnselectedCont2.Value = RequestHead.ContactId2.ToString();

                /*New Change Code*/
                long EdtCon1 = long.Parse(RequestHead.ContactId1.ToString());
                long EdtAddress = long.Parse(RequestHead.AddressId.ToString());

                long LocAddress = long.Parse(RequestHead.LocationID.ToString());

                string EdtCon2 = RequestHead.Con2; hdnselectedCont2.Value = RequestHead.Con2.ToString();
                if (EdtCon1 != 0)
                {
                    txtContact1.Text = objCommon.getContact1NameByID(EdtCon1, profile.DBConnection._constr);
                }
                //if (EdtCon2 != "0") 
                //{
                //    txtContact2.Text = objCommon.getContact2NamesByIDs(EdtCon2, profile.DBConnection._constr);
                //}
                if (EdtCon2 != string.Empty) { txtContact2.Text = objCommon.getContact2NamesByIDs(EdtCon2, profile.DBConnection._constr); }

                if (EdtAddress != 0)
                {
                    txtAddress.Text = objCommon.GetAddressLineByAdrsID(EdtAddress, profile.DBConnection._constr);
                    lblAddress.Text = objCommon.GetAddressLineByAdrsID(EdtAddress, profile.DBConnection._constr);
                }

                if (LocAddress != 0)
                {
                    txtLocation.Text = objCommon.GetAddressLineByAdrsID(LocAddress, profile.DBConnection._constr);
                    lblLocation.Text = objCommon.GetAddressLineByAdrsID(LocAddress, profile.DBConnection._constr);
                }
                /*New Change Code*/

                // lblAddress.Text = ddlAddress.SelectedItem.ToString();

                if (RequestHead.DispatchDate != null) { txtShippedDate.Text = RequestHead.DispatchDate.ToString(); } else { lblshipeddate.Visible = false; txtShippedDate.Visible = false; }
                if (RequestHead.CompletedDate != null) { txtReceivedDate.Text = RequestHead.CompletedDate.ToString(); } else { lblReceivedDate.Visible = false; txtReceivedDate.Visible = false; }
                if (RequestHead.CancelDate != null) { txtCloseDate.Text = RequestHead.CancelDate.ToString(); } else { lblCloseDate.Visible = false; txtCloseDate.Visible = false; }
                if (RequestHead.WMSRemark != null) { txtDispatchRemark.Text = RequestHead.WMSRemark.ToString(); }

                //ddlContainer.DataSource = WMFillEnginList(Convert.ToInt64(RequestHead.SiteID));
                //ddlContainer.DataBind();
                //ddlContainer.SelectedIndex = ddlContainer.Items.IndexOf(ddlContainer.Items.FindByText(RequestHead.Container.ToString()));
                //lblEngineModel.Text = RequestHead.EngineModel;
                //lblEngineSerial.Text = RequestHead.EngineSerial;
                //txtFailureHours.Text = RequestHead.FailureHours;
                //txtFailureCause.Text = RequestHead.FailureCause;
                //txtFailureNature.Text = RequestHead.FailureNature;

                txtTotalQty.Text = RequestHead.TotalQty.ToString();
                txtGrandTotal.Text = RequestHead.GrandTotal.ToString();

                fillPaymentMethod(long.Parse(hdnselectedDept.Value));
                ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByValue(RequestHead.PaymentID.ToString()));                
                Page.ClientScript.RegisterStartupScript(this.GetType(), "reset", " GetPaymentMethodID();", true);
                if (RequestHead.PaymentID == 5)
                {
                    WMGetCostCenter(long.Parse(hdnselectedDept.Value));
                    DataSet dsCostCenter = new DataSet();
                    dsCostCenter = objService.GetSelectedCostCenter(long.Parse(RequestHead.Id.ToString()), profile.DBConnection._constr);
                    long SelectedCostCenter = long.Parse(dsCostCenter.Tables[0].Rows[0]["StatutoryValue"].ToString());
                    //ddlFOC.SelectedIndex = ddlFOC.Items.IndexOf(ddlFOC.Items.FindByValue(SelectedCostCenter.ToString()));
                    ddlFOC.SelectedValue = SelectedCostCenter.ToString();
                }
                else
                {
                    //LstStatutoryInfo.Enabled = false;
                    if (RequestHead.Status >= 2) { LstStatutoryInfo.Enabled = false; }
                    DataSet dsAdditionalFields = new DataSet();
                    dsAdditionalFields = objService.GetAddedAdditionalFields(long.Parse(RequestHead.Id.ToString()), profile.DBConnection._constr);
                    LstStatutoryInfo.DataSource = dsAdditionalFields;
                    LstStatutoryInfo.DataBind();

                }
                GetDocumentByOrderID(long.Parse(RequestHead.Id.ToString()));
                GetApprovalDetails();

                GetDeliveryDetails(RequestHead.Id);

            }
            catch { }
            finally { objService.Close(); }
        }

        public void GetDocumentByOrderID(long OrderID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UC_AttachmentDocument1.FillDocumentByObjectNameReferenceID(OrderID, "RequestPartDetail", "RequestPartDetail");
        }

        public void GetDeliveryDetails(long RequestHeadId)
        {
            iPartRequestClient objService = new iPartRequestClient();
            VW_OrderDeliveryDetails ODD = new VW_OrderDeliveryDetails();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ODD = objService.GetOrderDeliveryDetails(RequestHeadId, profile.DBConnection._constr);

                if (ODD.Name != null) txtCustName.Text = ODD.Name.ToString();
                if (ODD.ContactNo != null) txtContactNo.Text = ODD.ContactNo.ToString();
               
                if (ODD.Address != null) txtCustAddress.Text = ODD.Address.ToString();
                if (ODD.EmailId != null) txtEmail.Text = ODD.EmailId.ToString();

                if (ODD.Landmark != null) txtLandmark.Text = ODD.Landmark.ToString();
                if (ODD.Zipcode != null) txtZipcode.Text = ODD.Zipcode.ToString();
                if (ODD.PaymentMode != null) txtPaymentMode.Text = ODD.PaymentMode.ToString();
                if (ODD.CardNo != null) txtCardNo.Text = ODD.CardNo.ToString();
                if (ODD.Remark != null) txtPaymentRemark.Text = ODD.Remark.ToString();
                if (ODD.BankName != null) txtBankName.Text = ODD.BankName.ToString();
                if (ODD.PaymentDate != null) txtPaymentDate.Text = ODD.PaymentDate.ToString();
                if (ODD.DriverName != null) txtDriverName.Text = ODD.DriverName.ToString();
                if (ODD.DriverMobileNo != null) txtDriverContactNo.Text = ODD.DriverMobileNo.ToString();
                if (ODD.DriverEmailID != null) txtDriverEmail.Text = ODD.DriverEmailID.ToString();
                if (ODD.TruckDetail != null) txtTruckDetails.Text = ODD.TruckDetail.ToString();
                if (ODD.AssignDate != null) txtAssignDate.Text = ODD.AssignDate.ToString();
                if (ODD.DeliveryType != null) txtDeliveryType.Text = ODD.DeliveryType.ToString();

                if (ODD.Card_Verified != null)
                {
                    string CV = ODD.Card_Verified.ToString();
                    if (CV == "Verified") { Verified.Visible = true; }
                    else if (CV == "Pending") { Pending.Visible = true; }
                    else if (CV == "Decline") { Decline.Visible = true; }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "GetDeliveryDetails");
            }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static BrilliantWMS.PORServiceEngineMaster.v_GetEngineDetails WMGetEngineDetails(int EngineID)
        {
            iEngineMasterClient objService = new iEngineMasterClient();
            BrilliantWMS.PORServiceEngineMaster.v_GetEngineDetails EngineRec = new BrilliantWMS.PORServiceEngineMaster.v_GetEngineDetails();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                EngineRec = objService.GetmEngineListByID(EngineID, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return EngineRec;
        }

        [WebMethod]
        public static long WMSaveRequestHead(object objReq)
        {
            long result = 0;
            long PreviousStatusID = 0;
            int RSLT = 0; long RequestID = 0;
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                tOrderHead RequestHead = new tOrderHead();
                //PORtPartRequestHead RequestHead = new PORtPartRequestHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;

                if (HttpContext.Current.Session["PORRequestID"] != null)
                {
                    if (HttpContext.Current.Session["PORRequestID"].ToString() == "0")
                    {
                        RequestHead.CreatedBy = profile.Personal.UserID;
                        // RequestHead.CreationDt = DateTime.Now;
                        RequestHead.Creationdate = DateTime.Now;
                    }
                    else
                    {
                        //RequestHead = objService.GetRequestHeadByRequestID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                        RequestHead.Id = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());
                        PreviousStatusID = objService.GetPreviousStatusID(RequestHead.Id, profile.DBConnection._constr);
                        RequestHead.LastModifiedBy = profile.Personal.UserID;
                        RequestHead.LastModifiedDt = DateTime.Now;
                    }

                    RequestHead.StoreId = Convert.ToInt64(dictionary["StoreId"]);
                    RequestHead.OrderNumber = dictionary["OrderNumber"].ToString();
                    RequestHead.Priority = dictionary["Priority"].ToString();
                    RequestHead.Status = Convert.ToInt64(dictionary["Status"]);
                    RequestHead.Title = dictionary["Title"].ToString();

                    //RequestHead.Orderdate = Convert.ToDateTime(dictionary["Orderdate"]);
                    RequestHead.Orderdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    RequestHead.RequestBy = Convert.ToInt64(dictionary["RequestBy"]);
                    RequestHead.Remark = dictionary["Remark"].ToString();
                    RequestHead.Deliverydate = Convert.ToDateTime(dictionary["Deliverydate"]);
                    RequestHead.ContactId1 = Convert.ToInt64(dictionary["ContactId1"].ToString());
                    // RequestHead.ContactId2 = Convert.ToInt64(dictionary["ContactId2"].ToString());
                    RequestHead.AddressId = Convert.ToInt64(dictionary["AddressId"].ToString());
                    RequestHead.Con2 = dictionary["ContactId2"].ToString();
                    RequestHead.PaymentID = Convert.ToInt64(dictionary["PaymentID"].ToString());
                    RequestHead.TotalQty = Convert.ToDecimal(dictionary["TotalQty"].ToString());
                    RequestHead.GrandTotal = Convert.ToDecimal(dictionary["GrandTotal"].ToString());

                    RequestHead.LocationID = Convert.ToInt64(dictionary["LocationID"].ToString());

                    if (Convert.ToInt64(dictionary["Status"]) == 1) { }
                    else
                    {
                        RequestHead.OrderNo = objService.GetNewOrderNo(Convert.ToInt64(dictionary["StoreId"]), profile.DBConnection._constr);
                    }
                    RequestHead.orderType = "OMS";

                    //  long RequestID = objService.SetIntoPartRequestHead(RequestHead, profile.DBConnection._constr);
                    RequestID = objService.SetIntotOrderHead(RequestHead, profile.DBConnection._constr); 

                    if (RequestID > 0)
                    {
                        //objService.FinalSaveRequestPartDetail(HttpContext.Current.Session.SessionID, ObjectName, RequestID, profile.Personal.UserID.ToString(), Convert.ToInt32(RequestHead.StatusID), profile.DBConnection._constr);
                        RSLT = objService.FinalSaveRequestPartDetail(HttpContext.Current.Session.SessionID, ObjectName, RequestID, profile.Personal.UserID.ToString(), Convert.ToInt32(RequestHead.Status), Convert.ToInt64(RequestHead.StoreId), PreviousStatusID, profile.DBConnection._constr);
                        //UC_AttachDocument1.FinalSaveDocument(RequestID);
                        if (RSLT == 1 || RSLT == 2) { result = RequestID; }  //"Request saved successfully";
                        else if (RSLT == 3) { result = -3; } //"Request saved successfully. Email Notification Failed..."
                        else if (RSLT == 0) { result = 0; }  //"Some error occurred";

                        // objService.UpdateStatutoryDetails(RequestID, profile.DBConnection._constr); //Update StatutoryDetails
                        iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                        DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, RequestID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                    }

                    //if (PreviousStatusID == 2)
                    //{
                    //    System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                    //    Response.Write("<script>");
                    //    Response.Write("window.open('../PowerOnRent/Approval.aspx?REQID='" + RequestID + "'&ST=24', null, 'height=180px, width=595px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');");
                    //    Response.Write("</script>");
                    //}
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMSaveRequestHead");
                result = 0; // "Some error occurred";
            }
            finally { objService.Close(); HttpContext.Current.Session["TemplateID"] = "0"; HttpContext.Current.Session["OrderID"] = RequestID; }
            if (PreviousStatusID == 2)
            {
               // result = HttpContext.Current.Session["PORRequestID"].ToString();
            }
            return result;
        }

        protected void divVisibility()
        {
            divApprovalHead.Attributes.Add("style", "display:none");
            divApprovalDetail.Attributes.Add("style", "display:none");

            divIssueHead.Attributes.Add("style", "display:none");
            divIssueDetail.Attributes.Add("style", "display:none");

            divCorrespondanceHead.Attributes.Add("style", "display:none");
            divCorrespondanceDetails.Attributes.Add("style", "display:none");
            //linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
            //divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";

            //divReceiptHead.Attributes.Add("style", "display:none");
            //divReceiptDetail.Attributes.Add("style", "display:none");

            //divConsumptionHead.Attributes.Add("style", "display:none");
            //divConsumptionDetail.Attributes.Add("style", "display:none");

            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Approved")) > 0)
            // if (ddlStatus.SelectedIndex == 3)
            if (ddlStatus.SelectedIndex == 2 || ddlStatus.SelectedValue == "22" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "2" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "4" || ddlStatus.SelectedIndex == 21 || ddlStatus.SelectedIndex == 22 || ddlStatus.SelectedIndex == 4 )
            {
                divApprovalHead.Attributes.Add("style", "display:'';");
                divApprovalDetail.Attributes.Add("style", "display:'';");

                //linkRequest.Attributes["innerHTML"] = "Expand";
                linkRequest.Attributes["innerHTML"] = "Collapse";
                linkRequest.InnerText = "Expand";
                divRequestDetail.Attributes["class"] = "divDetailCollapse";

                //linkIssueDetail.Attributes["innerHTML"] = "Expand";
                //divIssueDetail.Attributes["class"] = "divDetailCollapse";
                linkIssueDetail.Attributes.Add("InnerHtml", "Expand");
                linkIssueDetail.InnerText = "Expand";
                divIssueDetail.Attributes.Add("class", "divDetailCollapse");

                divCorrespondanceHead.Attributes.Add("style", "display:''");
                divCorrespondanceDetails.Attributes.Add("style", "display:''");
                //linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
                //divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";
                linkCorrespondanceDetail.Attributes.Add("InnerHtml", "Expand");
                linkCorrespondanceDetail.InnerText = "Expand";
                divCorrespondanceDetails.Attributes.Add("class", "divDetailCollapse");

                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
                Toolbar1.SetEditRight(false, "Not Allowed");
            }
            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Fully Issued")) > 0)
            if (ddlStatus.SelectedIndex == 3 || ddlStatus.SelectedValue == "7" || ddlStatus.SelectedValue == "10" || ddlStatus.SelectedValue == "8" || ddlStatus.SelectedValue == "25" || ddlStatus.SelectedValue == "26" || ddlStatus.SelectedValue == "28")
            {
                divIssueHead.Attributes.Add("style", "display:'';");
                divIssueDetail.Attributes.Add("style", "display:'';");

                linkRequest.Attributes["innerHTML"] = "Expand";
                linkRequest.InnerText = "Expand";
                divRequestDetail.Attributes["class"] = "divDetailCollapse";

                divApprovalHead.Attributes.Add("style", "display:'';");
                divApprovalDetail.Attributes.Add("style", "display:'';");

                linkApprovalDetail.Attributes["innerHTML"] = "Expand";
                linkApprovalDetail.InnerText = "Expand";
                divApprovalDetail.Attributes["class"] = "divDetailCollapse";

                divCorrespondanceHead.Attributes.Add("style", "display:''");
                divCorrespondanceDetails.Attributes.Add("style", "display:''");

                linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
                linkCorrespondanceDetail.InnerText = "Expand";
                divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderHead');LoadingOff();", true);
                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
                Toolbar1.SetEditRight(false, "Not Allowed");
            }
            if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Received")) > 0)
            {
                //divReceiptHead.Attributes.Add("style", "display:'';");
                //divReceiptDetail.Attributes.Add("style", "display:'';");
            }
            if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Consumed")) > 0)
            {
                //divConsumptionHead.Attributes.Add("style", "display:'';");
                //divConsumptionDetail.Attributes.Add("style", "display:'';");
            }

            if (ddlStatus.Items.Count > 0)
            {
                //if (Convert.ToInt32(ddlStatus.SelectedItem.Value) >= 2)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);
                //    //ddlRequestByUserID.Enabled = false;
                //    //ddlStatus.Enabled = false;
                //    //chkSelectTemplate.Visible = false;
                //    //btnSaveAsTemplate.Enabled = false;
                //    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                //    //ddlCompany.Enabled = false;
                //    //ddlSites.Enabled = false;
                //    //disabled
                //    //Toolbar1.SetSaveRight(false, "Not Allowed");
                //    Toolbar1.SetClearRight(false, "Not Allowed");
                //    Toolbar1.SetEditRight(true, "Allowed");
                //}
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 3 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 4 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 21 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 22 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 2)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderHead');LoadingOff();", true);
                    //ddlRequestByUserID.Enabled = false;
                    //ddlStatus.Enabled = false;
                    //chkSelectTemplate.Visible = false;
                    //btnSaveAsTemplate.Enabled = false;
                    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                    //ddlCompany.Enabled = false;
                    //ddlSites.Enabled = false;
                    //disabled
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                    Toolbar1.SetEditRight(false, "Not Allowed");
                }
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 1)
                {
                   // ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(false, 'divRequestDetail');LoadingOff();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(false, 'OrderHead');LoadingOff();", true);
                    //ddlRequestByUserID.Enabled = false;
                    //ddlStatus.Enabled = false;
                    //chkSelectTemplate.Visible = false;
                    //btnSaveAsTemplate.Enabled = false;
                    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                    //ddlCompany.Enabled = false;
                    //ddlSites.Enabled = false;
                    //disabled
                    Toolbar1.SetSaveRight(true, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                    // Toolbar1.SetEditRight(true, "Not Allowed");
                }


                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemode" + Session.SessionID, "changemode(false, 'divRequestDetail');LoadingOff();", true); }

                //if (Convert.ToInt32(ddlStatus.SelectedItem.Value) >= 3)
                //{
                //    CheckBoxApproved.Enabled = false;
                //    CheckBoxRejected.Enabled = false;
                //    txtApprovalRemark.Enabled = false;
                //    btnSaveApproval.Attributes.Add("onclick", "showAlert('Not Allowed');");
                //    btnSaveApproval.Attributes.Add("class", "Off buttonON");
                //}
                //else
                //{
                //    CheckBoxApproved.Enabled = true;
                //    CheckBoxRejected.Enabled = true;
                //    txtApprovalRemark.Enabled = true;
                //    btnSaveApproval.Attributes.Add("onclick", "jsSaveApproval();");
                //    btnSaveApproval.Attributes.Add("class", "buttonON");
                //}
            }

            if (Session["PORstate"] != null)
            {
                if (Session["PORstate"].ToString() == "Add")
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    // if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 1;

                    //if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 1;
                    if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 2;
                    lblRequestNo.Text = "Generate when Save";
                    UC_DateRequestDate.Date = DateTime.Now;

                    DateTime crntdt = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    txtRequestDate.Text = crntdt.ToString("dd-MMM-yyyy");// .ToShortDateString();

                    //UC_ExpDeliveryDate.Date = DateTime.Now.AddDays(7);
                    //  ddlRequestType.SelectedIndex = 2;
                    ddlRequestByUserID.DataSource = null;
                    ddlRequestByUserID.DataBind();
                    // ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                    if (hdnselectedDept.Value == "") hdnselectedDept.Value = "0";
                    //ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(hdnselectedDept.Value));
                    ddlRequestByUserID.DataSource = FillCurrentUserList(Convert.ToInt64(hdnselectedDept.Value));
                    ddlRequestByUserID.DataBind();
                    ddlRequestByUserID.SelectedIndex = ddlRequestByUserID.Items.IndexOf(ddlRequestByUserID.Items.FindByValue(profile.Personal.UserID.ToString()));
                    if (profile.Personal.UserType == "User")
                    {
                        ddlRequestByUserID.Enabled = false;
                    }

                    //ddlContainer.DataSource = null;
                    //ddlContainer.DataBind();

                    //ddlContainer.DataSource = WMFillEnginList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                    //ddlContainer.DataBind();
                }
            }
        }
        #endregion

        #region Request Part Detail
        protected void FillGrid1ByRequestID(long RequestID, long SiteID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<POR_SP_GetPartDetail_ForRequest_Result> RequestPartList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                RequestPartList = objService.GetRequestPartDetailByRequestID(RequestID, SiteID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                Grid1.DataSource = RequestPartList;
                Grid1.DataBind();
                // if (long.Parse(Session["PORRequestID"].ToString()) > 0) { Grid1.Columns[16].Visible = true; } else { Grid1.Columns[16].Visible = false; }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "FillGrid1ByRequestID"); }
            finally { objService.Close(); }
        }

        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                List<POR_SP_GetPartDetail_ForRequest_Result> RequestPartList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                if (hdn.Value == "")
                {
                    RequestPartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    // RequestPartList = objService.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(profile.Personal.DepartmentID), profile.DBConnection._constr).ToList();
                    //RequestPartList = objService.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, 18, profile.DBConnection._constr).ToList();
                    RequestPartList = objService.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(hdnselectedDept.Value), profile.DBConnection._constr).ToList();
                    //  RequestPartList = objService.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                }

                //Add by Suresh
                if (hdnprodID.Value != "")
                {
                    RequestPartList = objService.AddPartIntoRequest_TempData(hdnprodID.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                    hdnprodID.Value = "";
                }

                if (hdnChngDept.Value == "0x00x0")
                {
                    objService.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    RequestPartList = null;
                }
                hdnChngDept.Value = "";
                var chngdpt = "1x1";
                hdnChngDept.Value = chngdpt;

                if (hdnChangePrdQtyPrice.Value == "1")
                {
                    RequestPartList = objService.GetRequestPartDetailByRequestID(long.Parse(Session["PORRequestID"].ToString()), long.Parse(hdnselectedDept.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    //tOrderHead RequestHead = new tOrderHead();
                    //RequestHead = objService.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                    //txtTotalQty.Text = RequestHead.TotalQty.ToString();
                    //txtGrandTotal.Text = RequestHead.GrandTotal.ToString();
                }

                Grid1.DataSource = RequestPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "Grid1_OnRebind"); }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WmUpdateRequestPartUOM(object objRequest)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);

                objService.UpdatePartRequest_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WmUpdateRequestPartUOM"); }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WMUpdateRequestQty(object objRequest)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]);

                objService.UpdatePartRequest_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMUpdateRequestQty"); }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WMUpdRequestPart(object objRequest)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                string uom = objService.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);

                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); //PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);

                objService.UpdatePartRequest_TempData1(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMUpdRequestPart"); }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WMUpdRequestPartPrice(object objRequest, int ProdID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();
                string uom = objService.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);
                PartRequest.Price = Convert.ToDecimal(dictionary["Price"]);
                // PartRequest.IsPriceChange = Convert.ToInt16(dictionary["IsPriceChange"]);
                decimal price = Convert.ToDecimal(dictionary["Price"]);
                int ISPriceChangedYN = objService.IsPriceChanged(ProdID, price, profile.DBConnection._constr);
                if (ISPriceChangedYN == 0) { PartRequest.IsPriceChange = 0; }
                else { PartRequest.IsPriceChange = 1; }
                objService.UpdatePartRequest_TempData12(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMUpdRequestPart"); }
            finally { objService.Close(); }
        }
        [WebMethod]
        public static int WMRemovePartFromRequest(Int32 Sequence)
        {
            int editOrder = 0;
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (long.Parse(HttpContext.Current.Session["PORRequestID"].ToString()) > 0)
                {
                    tOrderHead RequestHead = new tOrderHead();
                    long ReqID = long.Parse(HttpContext.Current.Session["PORRequestID"].ToString());
                    RequestHead = objService.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                   string Status = RequestHead.Status.ToString();
                   if (Status == "1")
                   {
                       Dictionary<string, object> dictionary = new Dictionary<string, object>();                       
                       objService.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                       editOrder = 1;
                   }
                   else { editOrder = 0; }
                }
                else
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();                   
                    objService.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                    editOrder = 1;
                }
            }
            catch { }
            finally { objService.Close(); }
            return editOrder;
        }

        #endregion

        #region Approval Code
        [WebMethod]
        public static string WMSaveApproval(string ApprovalStatus, string ApprovalRemark)
        {
            iPartRequestClient objService = new iPartRequestClient();
            string result = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                result = objService.SaveApprovalStatus(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), ApprovalStatus, ApprovalRemark, profile.Personal.UserID, profile.DBConnection._constr);

                if (result == "true")
                {
                    objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMSaveApproval"); }
            finally
            { objService.Close(); }
            return result;
        }

        protected void GetApprovalDetails()
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tApprovalDetail ApprovalRec = new tApprovalDetail();
                ApprovalRec = objService.GetApprovalDetailsByReqestID(Convert.ToInt64(Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                if (ApprovalRec != null)
                {
                    CheckBoxApproved.Checked = false; CheckBoxRejected.Checked = false;
                    if (ApprovalRec.Status == "Approved") { CheckBoxApproved.Checked = true; }
                    else if (ApprovalRec.Status == "Rejected") { CheckBoxRejected.Checked = true; }
                    lblApprovalDate.Text = ApprovalRec.ApprovedDate.Value.ToString("dd-MMM-yyyy hh:mm tt");
                    txtApprovalRemark.Text = ApprovalRec.Remark = ApprovalRec.Remark;

                    if (ApprovalRec.ApproverUserID != profile.Personal.UserID)
                    {
                        divApprovalDetail.Disabled = true;
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "GetApprovalDetails"); }
            finally { objService.Close(); }
        }
        #endregion

        #region Add by Suresh

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();

            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                    //Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[6] as Obout.Grid.GridDataControlFieldCell;
                    // Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[7] as Obout.Grid.GridDataControlFieldCell;
                    Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[10] as Obout.Grid.GridDataControlFieldCell;

                    DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;
                    HiddenField hdnUOM = cell.FindControl("hdnMyUOM") as HiddenField;
                    //Label rowQtySpn = e.Row.Cells[9].FindControl("rowQtyTotal") as Label;
                    Label rowQtySpn = e.Row.Cells[12].FindControl("rowQtyTotal") as Label;

                    //TextBox txtUsrQty = e.Row.Cells[8].FindControl("txtUsrQty") as TextBox;

                    //TextBox txtUsrQty = e.Row.Cells[6].FindControl("txtUsrQty") as TextBox;
                    TextBox txtUsrQty = e.Row.Cells[9].FindControl("txtUsrQty") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);
                    // decimal CrntStock = Convert.ToDecimal(e.Row.Cells[10].Text);
                    decimal CrntStock = Convert.ToDecimal(e.Row.Cells[7].Text);
                    decimal moq = Convert.ToDecimal(e.Row.Cells[5].Text);

                    /*New Price Added*/
                    TextBox txtUsrPrice = e.Row.Cells[13].FindControl("txtUsrPrice") as TextBox; //txtUsrPrice.Enabled = false;   //Product Price
                    Label rowPriceTotal = e.Row.Cells[14].FindControl("rowPriceTotal") as Label;

                    int pricechange = objService.GetDeptPriceChange(long.Parse(hdnselectedDept.Value), profile.DBConnection._constr);
                    if (pricechange == 1) { txtUsrPrice.Enabled = true;           
                    }
                    else { txtUsrPrice.Enabled = false; }
                    /*New Price Added*/
                    int isprichange = Convert.ToInt32(e.Row.Cells[17].Text);
                    if (isprichange == 1)
                    {
                        e.Row.BackColor = System.Drawing.Color.DarkCyan;
                        e.Row.ForeColor = System.Drawing.Color.Red;
                        
                    }

                    DataSet dsUOM = new DataSet();
                    dsUOM = objService.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);

                    ddl.DataSource = dsUOM;
                    ddl.DataTextField = "Description";
                    ddl.DataValueField = "UMOGroup";
                    ddl.DataBind();
                    //ddl.SelectedValue = e.Row.Cells[6].Text;

                    //  string SelTmplt = Session["TemplateID"].ToString();

                    //Grid1.Columns[16].Visible = false;

                    if (Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()) > 0)
                    {
                        long ReqId = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());

                        string selectedUomTmpl = objService.GetSelectedUom(ReqId, ProdID, profile.DBConnection._constr);
                        if (selectedUomTmpl != "0")
                        {
                            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(selectedUomTmpl.ToString()));
                        }
                        else
                        {
                            ddl.SelectedIndex = 2;
                        }
                        rowQtySpn.Text = txtUsrQty.Text;
                        decimal UserQty = decimal.Parse(txtUsrQty.Text.ToString());
                        int SelInd = 0;
                        SelInd = ddl.SelectedIndex;
                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[SelInd]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[SelInd]["UOMID"].ToString());
                       
                        rowPriceTotal.Text = e.Row.Cells[14].Text;
                        if (hdnOrderStatus.Value == "1")
                        {
                            UCProductSearch1.Visible = true;
                                                        
                            decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                            decimal UsrQty = decimal.Parse(txtUsrQty.Text.ToString()); //SelectedQty * rowQty;
                            decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());
                           
                            hdnSelectedQty.Value = SelectedQty.ToString();
                            rowQtySpn.Text = UsrQty.ToString();

                            if (UsrQty > CrntStock)
                            { rowQtySpn.Text = "0"; }
                            else
                            {
                                rowQtySpn.Text = UsrQty.ToString();
                                txtUsrQty.Text = Convert.ToString(UsrQty / SelectedQty);
                            }
                                                   
                            ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");                            
                            txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");
                            txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                        }
                        else
                        {
                            txtUsrQty.Text = Convert.ToString(UserQty / SelectedQty);
                            UCProductSearch1.Visible = false;
                            txtUsrQty.Enabled = false;
                            txtUsrPrice.Enabled = false;
                            ddl.Enabled = false;
                        }
                        //  Grid1.Columns[16].Visible = true;
                    }
                    else if (Convert.ToInt64(HttpContext.Current.Session["TemplateID"].ToString()) > 0)
                    {
                        long TemplID = Convert.ToInt64(HttpContext.Current.Session["TemplateID"].ToString());
                        string selectedUom = objService.GetSelectedUomTemplate(TemplID, ProdID, profile.DBConnection._constr);
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(selectedUom.ToString()));
                        rowQtySpn.Text = txtUsrQty.Text;

                        int SelIndx = 0;
                        if (selectedUom == "0")
                        {
                            SelIndx = 2;
                        }
                        else
                        {
                            SelIndx = ddl.SelectedIndex;
                        }

                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[SelIndx]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[SelIndx]["UOMID"].ToString());
                        decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                         decimal UsrQty = decimal.Parse(txtUsrQty.Text.ToString()); //SelectedQty * rowQty;
                        decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());
                        rowPriceTotal.Text = e.Row.Cells[14].Text;

                        hdnSelectedQty.Value = SelectedQty.ToString();
                       rowQtySpn.Text = UsrQty.ToString();
                       
                        if (UsrQty > CrntStock)
                        { rowQtySpn.Text = "0"; }
                        else
                        {
                            rowQtySpn.Text = UsrQty.ToString();
                            txtUsrQty.Text = Convert.ToString(UsrQty / SelectedQty);
                        }

                       // ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ")"); //old
                        ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");
                        //txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ")"); //old
                        txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");
                        txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                    }
                    else
                    {
                        ddl.SelectedIndex = 2;

                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[2]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[2]["UOMID"].ToString());

                        decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                        decimal UsrQty = SelectedQty * rowQty;
                        decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());

                        hdnSelectedQty.Value = SelectedQty.ToString();
                        rowQtySpn.Text = UsrQty.ToString();

                        if (UsrQty > CrntStock)
                        { rowQtySpn.Text = "0"; }
                        else
                        {
                            rowQtySpn.Text = UsrQty.ToString();
                            //Price = decimal.Parse(rowQtySpn.Text.ToString()) * decimal.Parse(txtUsrPrice.Text.ToString());
                            //rowPriceTotal.Text = Price.ToString();

                        }
                        ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");

                        txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");
                      //  txtUsrQty.Attributes.Add("onkeydown", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");
                    //    txtUsrQty.Attributes.Add("onkeypress", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");

                        txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                    //    txtUsrPrice.Attributes.Add("onkeydown", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                    //    txtUsrPrice.Attributes.Add("onkeypress", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "Grid1_OnRowDataBound"); }
            finally { objService.Close(); }
        }

        protected void Grid1_RowCommand(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();

            try
            {
                Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[6] as Obout.Grid.GridDataControlFieldCell;
                DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;

                ddl.Attributes.Add("onchange", "javascript:GetIndex('" + ddl.SelectedIndex + "'," + e.Row.RowIndex + ")");
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void gvApprovalRemarkBind(long RequestID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            gvApprovalRemark.DataSource = null;
            gvApprovalRemark.DataBind();
            DataSet dsGetApprovalDetail = new DataSet();
            if (profile.Personal.UserType == "Admin")
            {
                dsGetApprovalDetail = objService.GetApprovalDetailsNewAdmin(RequestID, profile.DBConnection._constr);
            }
            else
            {
                dsGetApprovalDetail = objService.GetApprovalDetailsNew(RequestID, profile.Personal.UserID, profile.DBConnection._constr);
            }
            gvApprovalRemark.DataSource = dsGetApprovalDetail;
            gvApprovalRemark.DataBind();
        }

        protected void gvApprovalRemark_OnRebind(object sender, EventArgs e)
        {
            gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()));
        }

        protected void GetTemplateDetails(string hdnSelTemplateID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            mRequestTemplateHead ReqTemHead = new mRequestTemplateHead();
            try
            {
                /*Bind Template Details*/
                CustomProfile profile = CustomProfile.GetProfile();
                ReqTemHead = objService.GetTemplateOrderHead(Convert.ToInt64(hdnSelTemplateID), profile.DBConnection._constr);
                if (ReqTemHead != null)
                {
                    // lblSelectedTemplateTitle.Text = ReqTemHead.TemplateTitle;

                    long SiteID = long.Parse(ReqTemHead.Department.ToString());
                    ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(ReqTemHead.Department.ToString()));
                    hdnselectedDept.Value = SiteID.ToString(); Session["DeptID"] = SiteID.ToString();

                    txtTemplateTitleNew.Text = ReqTemHead.TemplateTitle;
                    ddlAccessTypeNew.SelectedIndex = ddlAccessTypeNew.Items.IndexOf(ddlAccessTypeNew.Items.FindByValue(ReqTemHead.Accesstype.ToString()));
                    // ddlAccessType.SelectedValue = ReqTemHead.Accesstype;
                    //    ddlContact1.SelectedIndex = ddlContact1.Items.IndexOf(ddlContact1.Items.FindByValue(ReqTemHead.Contact1.ToString()));
                    //  ddlContact2.DataSource = WMGetContactPerson2Lst(Convert.ToInt64(ReqTemHead.Department), Convert.ToInt64(ddlContact1.SelectedIndex));
                    //ddlContact2.DataBind();
                    //ddlContact2.SelectedIndex = ddlContact2.Items.IndexOf(ddlContact2.Items.FindByValue(ReqTemHead.Contact2.ToString()));
                    //ddlAddress.SelectedIndex = ddlAddress.Items.IndexOf(ddlAddress.Items.FindByValue(ReqTemHead.Address.ToString()));
                    //lblAddress.Text = ddlAddress.SelectedItem.ToString();
                    /*Bind Template Details*/

                    /*Bind Template Product*/
                    DataSet dsTemplatePartLst = new DataSet();
                    dsTemplatePartLst = objService.GetTemplatePartLstByTemplateID(Convert.ToInt64(hdnSelTemplateID), profile.DBConnection._constr);
                    List<POR_SP_GetPartDetail_ForRequest_Result> TemplatePartList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                    if (dsTemplatePartLst.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dsTemplatePartLst.Tables[0].Rows.Count - 1; i++)
                        {
                            //TemplatePartList = objService.AddPartIntoRequest_TempData(dsTemplatePartLst.Tables[0].Rows[i]["PrdID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt64(hdnselectedDept.Value), profile.DBConnection._constr).ToList();
                            TemplatePartList = objService.AddPartIntoRequest_TempData(dsTemplatePartLst.Tables[0].Rows[i]["PrdID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, SiteID, profile.DBConnection._constr).ToList();
                            string uom = objService.GetUOMName(Convert.ToInt64(dsTemplatePartLst.Tables[0].Rows[i]["UOMID"].ToString()), profile.DBConnection._constr);
                            POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                            PartRequest.Sequence = i + 1;
                            PartRequest.RequestQty = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Qty"].ToString()); // Convert.ToDecimal(dictionary["RequestQty"]);
                            PartRequest.Price = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Price"].ToString());
                            PartRequest.Total = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Total"].ToString());
                            PartRequest.IsPriceChange = Convert.ToInt16(dsTemplatePartLst.Tables[0].Rows[i]["IsPriceChange"].ToString());
                            PartRequest.UOMID = Convert.ToInt64(dsTemplatePartLst.Tables[0].Rows[i]["UOMID"].ToString());
                            PartRequest.UOM = uom;
                            
                            objService.UpdatePartRequest_TempData12(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
                            //objService.UpdatePartRequest_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
                            // TemplatePartList = objService.AddPartIntoRequest_TempData(dsTemplatePartLst.Tables[0].Rows[i]["PrdID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt64(hdnselectedDept.Value), profile.DBConnection._constr).ToList();

                            TemplatePartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                        }
                    }

                    Grid1.DataSource = TemplatePartList;
                    Grid1.DataBind();
                    /*Bind Template Product*/
                    decimal totQty = WMGetTotalQty(); txtTotalQty.Text = totQty.ToString();
                    decimal GrandTot = WMGetTotal(); txtGrandTotal.Text = GrandTot.ToString();
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "GetTemplateDetails"); }
            finally { objService.Close(); }

        }

        protected void ddlUOM_SelectedIndexChanged(long selid)
        {

        }


        [WebMethod]
        public static string WMSaveTemplateHead(object obj1)
        {
            string result = "";
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mRequestTemplateHead ReqTemplHead = new mRequestTemplateHead();

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)obj1;

                ReqTemplHead.TemplateTitle = dictionary["TemplateTitle"].ToString();
                long TemplTitle = objService.ChkTemplateTitle(ReqTemplHead.TemplateTitle, profile.DBConnection._constr);
                if (TemplTitle == 0)
                {
                    ReqTemplHead.Accesstype = dictionary["Accesstype"].ToString();
                    ReqTemplHead.Department = Convert.ToInt64(dictionary["StoreId"].ToString());
                    //ReqTemplHead.Customer = Convert.ToInt64(ddlCompany.SelectedValue.ToString());
                    ReqTemplHead.Active = "Yes";
                    ReqTemplHead.CreatedBy = profile.Personal.UserID;
                    ReqTemplHead.CreatedDate = DateTime.Now;
                    ReqTemplHead.Remark = dictionary["Remark"].ToString();


                    long TemplateHeadID = objService.InsertIntomRequestTemplateHead(ReqTemplHead, profile.DBConnection._constr);

                    if (TemplateHeadID > 0)
                    {
                        objService.FinalSavemRequestTemplateDetail(HttpContext.Current.Session.SessionID, ObjectName, TemplateHeadID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    }

                    result = "Template Saved Successfully";
                }
                else
                {
                    result = "Title Already Available";
                }
            }
            catch (System.Exception ex) { result = "Some error occurred"; Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMSaveTemplateHead"); }
            finally { objService.Close(); }

            return result;
        }
        //protected void btnSubmitTemplate_Onclick(object sender, EventArgs e)
        //{
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iPartRequestClient objService = new iPartRequestClient();

        //    mRequestTemplateHead ReqTemplHead = new mRequestTemplateHead();

        //    try
        //    {
        //        if (txtTemplateTitle.Text == string.Empty)
        //        {
        //            WebMsgBox.MsgBox.Show("Please Enter Template Title.");
        //        }
        //        else if (ddlAccessType.SelectedIndex == 0)
        //        {
        //            WebMsgBox.MsgBox.Show("Please Select Priority.");
        //        }
        //        //else if (txtRemark.Text == string.Empty)
        //        //{
        //        //    WebMsgBox.MsgBox.Show("Please Enter Remark.");
        //        //    txtRemark.Focus();
        //        //}
        //        ////else if (ddlContact1.SelectedIndex == 0)
        //        //else if (hdnselectedCont1.Value == "")
        //        //{
        //        //    WebMsgBox.MsgBox.Show("Please Select Contact 1.");
        //        //    ddlContact1.Focus();
        //        //}
        //        //else if (hdnselectedCont2.Value == "")
        //        //{
        //        //    WebMsgBox.MsgBox.Show("Please Select Contact 2.");
        //        //    ddlContact2.Focus();
        //        //}
        //        //// else if (ddlAddress.SelectedIndex == 0)
        //        //else if (hdnSelAddress.Value == "")
        //        //{
        //        //    WebMsgBox.MsgBox.Show("Please Select Address.");
        //        //    ddlAddress.Focus();
        //        //}

        //        else
        //        {
        //            ReqTemplHead.TemplateTitle = txtTemplateTitle.Text;
        //            ReqTemplHead.Accesstype = ddlAccessType.SelectedItem.ToString();

        //            if (profile.Personal.UserType == "User" || profile.Personal.UserType == "Requester And Approver" || profile.Personal.UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
        //            { ReqTemplHead.Department = Convert.ToInt64(ddlSites.SelectedValue.ToString()); }
        //            else { ReqTemplHead.Department = Convert.ToInt64(hdnselectedCompany.Value); }
        //            ReqTemplHead.Customer = Convert.ToInt64(ddlCompany.SelectedValue.ToString());
        //            ReqTemplHead.Active = "Yes";
        //            ReqTemplHead.CreatedBy = profile.Personal.UserID;
        //            ReqTemplHead.CreatedDate = DateTime.Now;
        //            ReqTemplHead.Remark = txtRemark.Text;
        //            //if (profile.Personal.UserType == "User")
        //            if (profile.Personal.UserType == "User" || profile.Personal.UserType == "Requester And Approver" || profile.Personal.UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
        //            {
        //                ReqTemplHead.Contact1 = Convert.ToInt64(ddlContact1.SelectedValue.ToString());
        //            }
        //            else
        //            {
        //                ReqTemplHead.Contact1 = Convert.ToInt64(hdnselectedCont1.Value);
        //            }
        //            if (ReqTemplHead.Contact1 > 0)
        //            {
        //                ReqTemplHead.Contact2 = Convert.ToInt64(hdnselectedCont2.Value);
        //            }
        //            else { ReqTemplHead.Contact2 = 0; }
        //            //if (profile.Personal.UserType == "User")
        //            if (profile.Personal.UserType == "User" || profile.Personal.UserType == "Requester And Approver" || profile.Personal.UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
        //            { ReqTemplHead.Address = Convert.ToInt64(ddlAddress.SelectedValue.ToString()); }
        //            else { ReqTemplHead.Address = Convert.ToInt64(hdnSelAddress.Value); }

        //            //long TemplateHeadID = objService.InsertIntomRequestTemplateHead(ReqTemplHead, profile.DBConnection._constr);

        //            //if (TemplateHeadID > 0)
        //            //{
        //            //    objService.FinalSavemRequestTemplateDetail(HttpContext.Current.Session.SessionID, ObjectName, TemplateHeadID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
        //            //    WebMsgBox.MsgBox.Show("Template Saved Successfully");
        //            //    txtTemplateTitle.Text = "";
        //            //    ddlAccessType.SelectedIndex = 0;
        //            //}
        //        }
        //    }
        //    catch { }
        //    finally { objService.Close(); }
        //}

        protected void GVInboxPOR_OnRebind(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();

            long RequestID = long.Parse(Session["PORRequestID"].ToString());

            GVInboxPOR.DataSource = objService.GetCorrespondance(RequestID, profile.DBConnection._constr);
            GVInboxPOR.DataBind();
        }

        [WebMethod]
        public static string WMGetDepartmentSession(string Dept)
        {
            Page objp = new Page();
            objp.Session["DeptID"] = Dept;
            return Dept;
        }

        [WebMethod]
        public static string WMGetApproverForApprove(long AprvlID, long requestID, long DeligateID)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            long UsrID = profile.Personal.UserID;
            if (DeligateID == UsrID)
            {
                result = requestID.ToString();
            }
            else if (AprvlID == UsrID)
            {
                result = requestID.ToString();
            }
            else
            {
                result = "AccessDenied";
            }
            return result;
        }

        [WebMethod]
        public static string WMGetApproverForReject(long AprvlID, long requestID)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            long UsrID = profile.Personal.UserID;
            if (AprvlID == UsrID)
            {
                result = requestID.ToString();
            }
            else
            {
                result = "AccessDenied";
            }
            return result;
        }

        protected void imgBtnView_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;

            var CorID = imgbtn.ToolTip.ToString();

            Session["CORID"] = CorID.ToString();

            //string path = "../PowerOnRent/Correspondance.aspx?CORID='" + CorID + "'";
            //string s = "window.open('" + path + "','width=300,height=100,left=100,top=100,resizable=yes,toolbar=no,scrollbars=no,');";
            //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            // ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('../PowerOnRent/Correspondance.aspx?CORID='" + CorID + "'', 'popup_window', 'height=550px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');", true);
            // ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            //  Response.Write("<script type='text/javascript'> window.open('../PowerOnRent/Correspondance.aspx?CORID='" + CorID + "'', null, 'height=550px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');</script>");
            //Response.Write("window.open('../PowerOnRent/Correspondance.aspx?CORID='"+ CorID +"'', null, 'height=550px, width=825px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50');");            
            //Response.Write("</script>");
        }

        public List<mTerritory> WMGetSelDept(int Cmpny, long UserID)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            SiteLst = UCCommonFilter.GetAddedDepartmentList(Cmpny, UserID, profile.DBConnection._constr).ToList();

            return SiteLst;
        }

        //protected void btnSubmit_Onclick(object sender, EventArgs e)
        //{
        //    //if (txtProductCode.Text == string.Empty)
        //    //{
        //    //    WebMsgBox.MsgBox.Show("Please Enter Product Code");
        //    //}
        //    if (txtProductName.Text == string.Empty)
        //    {
        //        WebMsgBox.MsgBox.Show("Please Enter Product Name");
        //    }

        //    try
        //    {
        //        string state;
        //        CustomProfile profile = CustomProfile.GetProfile();
        //        //if (checkduplicate() == "")
        //        //{
        //            iProductMasterClient productclient = new iProductMasterClient();
        //            mProduct obj = new mProduct();

        //            state = "AddNew";
        //            obj.CreatedBy = profile.Personal.UserID.ToString();
        //            obj.CreationDate = DateTime.Now;

        //            obj.ProductTypeID = 1;
        //            obj.ProductCategoryID = 2;
        //            obj.ProductSubCategoryID = 6;
        //           // obj.ProductCode = txtProductCode.Text.ToString().Trim();
        //            obj.ProductCode = "New Product"+ " " + DateTime.Now.ToString("ddMMyy HHmmss")+" " + profile.Personal.UserID.ToString();

        //            obj.Name = txtProductName.Text.ToString().Trim();
        //            obj.Description = txtDesc.Text.ToString().Trim();
        //            obj.UOMID = 17;
        //            obj.PrincipalPrice = 1;
        //            obj.FixedDiscount = 0;
        //            obj.FixedDiscountPercent =Convert.ToBoolean(0);
        //            obj.Installable = Convert.ToBoolean(1);
        //            obj.AMC = Convert.ToBoolean(0);
        //            obj.WarrantyDays = 0;
        //            obj.GuaranteeDays = 0;
        //            obj.Active = "Y";

        //            hdnprodID.Value = productclient.FinalSaveProductDetailByProductID(obj, profile.DBConnection._constr).ToString();

        //            productclient.Close();

        //            Grid1_OnRebind(sender,e);
        //        //}
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Login.Profile.ErrorHandling(ex, this, "PartRequestEntry", "btnSubmit_Onclick");
        //    }
        //    finally
        //    {
        //    }
        //    //txtProductCode.Text = "";
        //    txtProductName.Text = "";
        //    txtDesc.Text = "";
        //}


        //public string checkduplicate()
        //{
        //    try
        //    {
        //        CustomProfile profile = CustomProfile.GetProfile();
        //        iProductMasterClient productclient = new iProductMasterClient();
        //        string result="";

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
        #endregion

        #region GWCVer2
        public void LstStatutoryInfo_OnLoad(object sender, EventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdnSelPaymentMethod.Value != "")
            {
                long SelpaymentMethod = long.Parse(hdnSelPaymentMethod.Value);
                if (SelpaymentMethod > 0)
                {
                    if (Convert.ToInt64(Session["PORRequestID"].ToString()) > 0) 
                    {
                        if (hdnOrderStatus.Value == "1") 
                        {
                            if (hdnPmethodChng.Value == "1")
                            {
                                LstStatutoryInfo.DataSource = objService.GetPaymentMethodFields(SelpaymentMethod, profile.DBConnection._constr);
                                LstStatutoryInfo.DataBind();
                            }
                        }
                    }
                    else
                    {
                        LstStatutoryInfo.DataSource = objService.GetPaymentMethodFields(SelpaymentMethod, profile.DBConnection._constr);
                        LstStatutoryInfo.DataBind();
                    }
                }
            }
            objService.Close();
        }

        [WebMethod]
        public static List<VW_DeptWisePaymentMethod> WMGetPaymentMethod(long Dept)
        {
            List<VW_DeptWisePaymentMethod> AdrsLst = new List<VW_DeptWisePaymentMethod>();

            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            AdrsLst = objService.GEtDeptPaymentmethod(Dept, profile.DBConnection._constr).ToList();
            return AdrsLst;
        }

        [WebMethod]
        public static List<mCostCenterMain> WMGetCostCenter(long Dept)
        {
            List<mCostCenterMain> costcenter = new List<mCostCenterMain>();
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            costcenter = objService.GetCostCenter(Dept, profile.DBConnection._constr).ToList();
            return costcenter;
        }

        [WebMethod]
        public static decimal WMGetTotal()
        {
            iPartRequestClient objService = new iPartRequestClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                tot = objService.GetTotalFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetTotal"); }
            finally { objService.Close(); }
            return tot;
        }

        [WebMethod]
        public static decimal WMGetTotalQty()
        {
            iPartRequestClient objService = new iPartRequestClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                tot = objService.GetTotalQTYFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetTotalQty"); }
            finally { objService.Close(); }
            return tot;
        }

        [WebMethod]
        public static long WMGetMaxDeliveryDays(long Dept)
        {
            iPartRequestClient objService = new iPartRequestClient();
            long mdd = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mdd = objService.GetMaxDeliveryDaysofDept(Dept, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetMaxDeliveryDays"); }
            finally { objService.Close(); }
            return mdd;
        }

        public void UC_ExpDeliveryDate_OnLoad(object sender, EventArgs e)
        {
            string mdd = hdnMaxDeliveryDays.Value;
            if (mdd == "") { }
            else
            {
                UC_ExpDeliveryDate.enddate(DateTime.Now.AddDays(int.Parse(mdd)));
            }
        }

        [WebMethod]
        public static string WMGetMandatoryDetails(long pm)
        {
            iPartRequestClient objService = new iPartRequestClient();
            string seq = "0";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                seq = objService.GetMandatoryFields(pm, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetMandatoryDetails"); }
            finally { objService.Close(); }
            return seq;
        }

        [WebMethod]
        public static void WmGetPaymentMethodLabelText(string PMLabel, string PMText, long pmID, int Seq, long OrderID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long ApproverID = 0;
            try
            {
                long StatutoryID = objService.GetStatutoryID(PMLabel, pmID, profile.DBConnection._constr);

                if (pmID == 5) { ApproverID = objService.GetCostCenterApproverID(Convert.ToInt64(PMText), profile.DBConnection._constr); }
                
                tStatutoryDetail pmd = new tStatutoryDetail();
                pmd.ObjectName = "RequestPartDetail";
                pmd.ReferenceID = OrderID;
                pmd.StatutoryID = StatutoryID;
                pmd.StatutoryValue = PMText;
                pmd.Active = "1";
                pmd.CreatedBy = profile.Personal.UserID.ToString();
                pmd.CreatedDate = DateTime.Now;
                pmd.CompanyID = 0;
                pmd.Sequence = Seq;
                pmd.ApproverID = ApproverID;

                objService.AddIntotStatutory(pmd, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WmGetPaymentMethodLabelText"); }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static int WMGetAccessOfProductChange(int Seq)
        {
            iPartRequestClient objService = new iPartRequestClient(); HttpContext.Current.Session["SEQ"] = Seq;
            CustomProfile profile = CustomProfile.GetProfile();
            int YN = objService.GetPartAccessofUser(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.Personal.UserID, profile.DBConnection._constr);
            return YN;
        }
        [WebMethod]
        public static List<tOrderHead> WMGetTotalQtyGrandTotal()
        {
            iPartRequestClient objService = new iPartRequestClient();
            List<tOrderHead> RequestHead = new List<tOrderHead>();
            CustomProfile profile = CustomProfile.GetProfile();
            RequestHead = objService.GetOrderHeadByOrderIDQTYTotal(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), profile.DBConnection._constr).ToList();
            return RequestHead;
        }
        [WebMethod]
        public static void WMPaymentMethodNone(long pm, long OrderID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            objService.RemoveFromTStatutory(OrderID, profile.DBConnection._constr);
        }
        #endregion

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            //lblFrmTemplate.Text = rm.GetString("GenerateFromTemplate", ci);
            //lblTemplateTitleField.Text = rm.GetString("TemplateTitle", ci);
            //lblSelectedTemplateTitle.Text = rm.GetString("NotAvailable", ci);

            //UCFormHeader1.FormHeaderText = rm.GetString("MaterialRequest", ci);

            //btnSaveAsTemplate.Text = rm.GetString("SaveAsTemplate", ci);
            //lbltemplate.Text = rm.GetString("NewTemplate", ci);
            //btnSubmitTemplate.Text = rm.GetString("Submit", ci);
            //btnCloseNewTemplate.Text = rm.GetString("Cancel", ci);
            //lblTemplateTitle.Text = rm.GetString("TemplateTitle", ci);
            //lblAccessType.Text = rm.GetString("AccessType", ci);
            lblTitle.Text = rm.GetString("Title", ci);
            lblCustomerOrderRefNo.Text = rm.GetString("CustomerOrderRefNo", ci);
            lblRequestNumber.Text = rm.GetString("RequestNo", ci);
            lblStatus.Text = rm.GetString("Status", ci);
            lblRequestDate.Text = rm.GetString("RequestDate", ci);
            //   lblPriority.Text = rm.GetString("Priority", ci);
            lblRequestedBy.Text = rm.GetString("RequestedBy", ci);
            lblExpDeliveryDate.Text = rm.GetString("ExpDeliveryDate", ci);
            lblCustomerName.Text = rm.GetString("CustomerName", ci);
            lblDepartment.Text = rm.GetString("Department", ci);
            lblContact1.Text = rm.GetString("conatact1", ci);
            lblContact2.Text = rm.GetString("conatact2", ci);
            lblCustomerAddress.Text = rm.GetString("CustomerAddress", ci);
            lblAddressLabel.Text = rm.GetString("Address", ci);
            lblRemark.Text = rm.GetString("Remark", ci);
            lblrequestpartlist.Text = rm.GetString("RequestPartList", ci);
            btnNewPrduct.Text = rm.GetString("AddNewProduct", ci);
            lblApproval.Text = rm.GetString("Approval", ci);
            lblApprovalHistory.Text = rm.GetString("ApprovalHistory", ci);
            lblDispatch.Text = rm.GetString("Dispatch", ci);
            //  lblIssueNo1.Text = rm.GetString("IssueNo", ci);
            //   lbltranfersite.Text = rm.GetString("transferfromsite", ci);
            //   lblcreatereceipt.Text = rm.GetString("creategoodsreceipt", ci);
            lblshipeddate.Text = rm.GetString("ShippedDate", ci);
            //   lblReceivedDate.Text = rm.GetString("ReceivedDate", ci);
            lblCloseDate.Text = rm.GetString("CloseDate", ci);
            lblRemark1.Text = rm.GetString("Remark", ci);
            lblCorrespondance.Text = rm.GetString("Correspondance", ci);
            lblinbox.Text = rm.GetString("Inbox", ci);
            btnAddNewCorrespondance.Value = rm.GetString("AddNew", ci);
            lblOperationApproval.Text = rm.GetString("OperationApproval", ci);
            lblApproved1.Text = rm.GetString("Approved", ci);
            lblCancelled.Text = rm.GetString("Cancelled", ci);
            lblapprovrevision.Text = rm.GetString("ApproveWithRevision", ci);
            lblDate.Text = rm.GetString("Date", ci);
            lblApprovRemark.Text = rm.GetString("Remark", ci);
            lblrequest.Text = rm.GetString("Request", ci);
            // lblcollapse.Text = rm.GetString("Collapse", ci);

            
            btnGenerateFromTemplate.Text = rm.GetString("GenerateFromTemplate", ci);
            btnSaveAsTemplateNew.Text = rm.GetString("SaveAsTemplate", ci);
            lblRequestNo.Text = rm.GetString("Generatewhensave", ci);
            lblTemplateTitleNew.Text = rm.GetString("TemplateTitle", ci);
            lblAccessTypeNew.Text = rm.GetString("AccessType", ci);
            lblReceivedDate.Text = rm.GetString("DispatchDate", ci);

            lblPaymentMethod.Text = rm.GetString("PaymentMethod", ci);
            lblDocument.Text = rm.GetString("Document", ci);
            lblTQty.Text = rm.GetString("TotalQuantity", ci);
            lblGTotal.Text = rm.GetString("GrandTotal", ci);


            lblDispatchDetails.Text = rm.GetString("DispatchDetails", ci);
            lblCustomerDetails.Text = rm.GetString("CustomerDetails", ci);
            lblCustName.Text = rm.GetString("CustomerName", ci);
            lblContactNo.Text = rm.GetString("ContactNo", ci);
       
            lblCustAddress.Text = rm.GetString("Address", ci);

            lblEmail.Text = rm.GetString("EmailID", ci);
            lblLandmark.Text = rm.GetString("Landmark", ci);
            lblZipCode.Text = rm.GetString("ZIPCode", ci);
            lblPaymentDetail.Text = rm.GetString("PaymentDetails", ci);

            lblPaymentMode.Text = rm.GetString("PaymentMode", ci);
            lblCardNo.Text = rm.GetString("CardNo", ci);
            lblPaymentRemark.Text = rm.GetString("Remark", ci);
            lblBankName.Text = rm.GetString("BankName", ci);
            lblPaymentDate.Text = rm.GetString("PaymentDate", ci);
            lblVerified.Text = rm.GetString("Verified", ci);

            lblDecline.Text = rm.GetString("Decline", ci);
            lblPending.Text = rm.GetString("Pending", ci);
            lblDriverDetails.Text = rm.GetString("DriverDetails", ci);
            lblDriverName.Text = rm.GetString("DriverName", ci);
            lblDriverContactNo.Text = rm.GetString("ContactNo", ci);
            lblDriverEmail.Text = rm.GetString("EmailID", ci);
            lblTruckDetail.Text = rm.GetString("TruckDetail", ci);
            lblAssignDate.Text = rm.GetString("AssignDate", ci);
            lblDeliveryType.Text = rm.GetString("DeliveryType", ci);



        }
    }
}