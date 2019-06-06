using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BrilliantWMS.DesignationService;
using BrilliantWMS.RoleMasterService;
using BrilliantWMS.UserCreationService;
using System.Web.Services;
using BrilliantWMS.Login;
using System.Web.Security;
using WebMsgBox;
using Obout.Grid;
using BrilliantWMS.ServiceTerritory;
using BrilliantWMS.Territory;
using System.Web.UI.HtmlControls;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Net.Mail;
using BrilliantWMS.ProductMasterService;
using System.Configuration;
using System.Data.SqlClient;



namespace BrilliantWMS.UserManagement
{

    public partial class UserCreation : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string sessionID = "";
        string editsession = "";
        string Usertyperoll = "";
        string CurrentPassword = "";
        static string EditUserName = "";
        #region UserForm
        #region PageCode
        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            txtConfirmPassword.Attributes.Add("onblur", "CheckPassword();");
            CustomProfile profile = CustomProfile.GetProfile();
            // UCAddress1.DefaultAddressColumn(true, false, "Default", "Shipping");
            // UCFormHeader1.FormHeaderText = "User Creation";
            sessionID = Session.SessionID;

            TextBox txtstartdate = (TextBox)UC_DateofBirth.FindControl("txtDate");
            TextBox txtenddate = (TextBox)UC_Dateofjoining.FindControl("txtDate");

            txtstartdate.Attributes.Add("onchange", "validateDate('" + txtstartdate.ClientID + "','" + txtenddate.ClientID + "','Start','Birth Date Should Not Be Less Than Registration Date')");
            txtenddate.Attributes.Add("onchange", "validateDate('" + txtstartdate.ClientID + "','" + txtenddate.ClientID + "','End','Registration Date Should Be Greater Than Birth Date')");
            if (IsPostBack != true)
            {
                Session.Add("hdnedit", editsession);
                ActiveTab("");
                BindCompany();
                // BindRole();
                BindGrid();
                BindReportingTo();
                ResetUserControl(0);


            }

            UC_Date1.startdate(DateTime.Now);
            UC_Date2.startdate(DateTime.Now);
            //this.UCToolbar1.ToolbarAccess("UserCreation");
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickClear += pageClear;

            //HtmlTable tbl = (HtmlTable)UC_Territory1.FindControl("tblUCTerritory_UserList");
            UC_Territory1.VisiableUserList = false;
            txtPassword1.Attributes["type"] = "password";
            txtConfirmPassword.Attributes["type"] = "password";
        }

        //protected void Page_LoadComplete(object sender, EventArgs e)
        //{
        //    if (ddlRoleList.Items.Count >= 1)
        //    {
        //        //ListItem lst1 = ddlRoleList.Items[1];
        //        // lst1.Attributes.Add("style", "color:red");
        //    }
        //}

        #endregion PageCode

        #region Toolbar Code
        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            ClearAll();
            ResetUserControl(0);
            //  FillRoleDropDown();

            BindRole();

            GridRoleConfiguration.DataSource = null;
            GridRoleConfiguration.DataBind();

            GridRoleConfiguration.GroupBy = "DisplayModuleName,DisplayPhaseName";
            GridRoleConfiguration.ShowHeader = true;
            //GridRoleConfiguration.HideColumnsWhenGrouping = true;
            lblPassword.Visible = lblConfirmPass.Visible = true;
            txtLoginId1.Visible = txtPassword1.Visible = txtConfirmPassword.Visible = true;
            req_txtLoginId.Visible = req_txtPassword.Visible = rfValtxtConfirmPassword.Visible = cmpValtxtPassword.Visible = true;
            ActiveTab("Add");
            UC_Territory1.BindListviewWithGroupTitle();
            hndstate.Value = "Add";
            Session["hdnedit"] = "";
            Session["hdnnewDelegateid"] = null;
            hdnSelectedLocation.Value = "";

        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            if (hndstate.Value == "Edit")
            {
                GetUserByID();
                ClearAll();
                ActiveTab("Add");

            }
            else
            {
                ClearAll();
                //  FillRoleDropDown(); 
                ResetUserControl(0);
                ActiveTab("Add");
            }
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                /*string text = "0";
                if (hndstate.Value == "Add")
                {
                    int val = Convert.ToInt32(hdnrollid.Value.ToString());
                    text = val.ToString();
                }
                else
                {
                    text = "0";
                }

                ;

                if (Usertyperoll != "Admin" && hdnSelectedDepartment.Value.ToString() == "0")
                {
                    MsgBox.Show("You Can't Select All Department for" + ddlRole.SelectedItem.Text + ".");
                }
                else if (Usertyperoll == "Admin" && Usertyperoll == "Admin")
                {
                    SaveForAdmin();
                }
                else if (Usertyperoll != "Super Admin" && hdnSelectedDepartment.Value.ToString() != "0")
                {
                    SaveForNormalUser();
                }*/

                SaveForNormalUser();
                // ActiveTab("List");

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "pageSave");
            }
            finally
            {
                userClient.Close();
            }
        }

        protected void ActiveTab(string state)
        {

            if (state == "Add")
            {
                //TabPanelUsersList.Visible = true;
                TabPanelUsersList.Visible = false;
                TabContainerUserCreation1.ActiveTabIndex = 1;
                TabPanelUserInformation.Visible = true;
                TabPanelAddressInfo.Visible = false;
                tabProductDetails.Visible = false;
                TabPanelRoleConfiguration.Visible = false;
                //  pnllocation.Visible = true;
                pnllocation.Visible = false; /*Temparary Change*/
                TabPanelWarehouse.Visible = true;
            }
            else if (state == "Edit")
            {
                //TabPanelUsersList.Visible = true;
                TabPanelUsersList.Visible = false;
                TabContainerUserCreation1.ActiveTabIndex = 1;
                TabPanelUserInformation.Visible = true;
                TabPanelAddressInfo.Visible = false;
                tabProductDetails.Visible = false;
                TabPanelRoleConfiguration.Visible = false;
                pnllocation.Visible = false;  /*Temparary Change*/
                TabPanelWarehouse.Visible = true;
            }

            else
            {
                TabPanelUsersList.Visible = true;
                TabContainerUserCreation1.ActiveTabIndex = 0;
                TabPanelUserInformation.Visible = false;
                TabPanelAddressInfo.Visible = false;
                tabProductDetails.Visible = false;
                TabPanelRoleConfiguration.Visible = false;
                pnllocation.Visible = false;
                TabPanelWarehouse.Visible = false;
            }
        }
        #endregion

        #region FillDropDown

        public void BindRole()
        {
            BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlRole.DataSource = roleMasterService.GetRoleList(profile.DBConnection._constr);
            ddlRole.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlRole.Items.Insert(0, lst);

        }

        public void bindRollbyDept(long DeptID)
        {
            DataSet ds;
            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {

                ds = userClient.GetRollNameById(DeptID, profile.DBConnection._constr);
                ddlRole.DataSource = ds;
                ddlRole.DataTextField = "Name";
                ddlRole.DataValueField = "Id";
                ddlRole.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlRole.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "bindRollbyDept");
            }
            finally { userClient.Close(); }
        }

        public void BindCompany()
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = userClient.GetCompanyName(profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlcompany.Items.Insert(0, lst);
        }
        public void BindReportingTo()
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlReportingTo.DataSource = userClient.GetUserCreationList(profile.DBConnection._constr);
            ddlReportingTo.DataBind();
            userClient.Close();

            ListItem lst = new ListItem();
            lst.Text = "-Select-";
            lst.Value = "0";
            ddlReportingTo.Items.Insert(0, lst);
        }
        public void BindDepartment()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.DepartmentService.iDepartmentMasterClient DepartmentService = new BrilliantWMS.DepartmentService.iDepartmentMasterClient();
                //ddlDepartment.DataSource = DepartmentService.GetDeparmentList(profile.DBConnection._constr);
                ddlDepartment.DataBind();
                ListItem lst = new ListItem();
                lst.Text = "-Select-";
                lst.Value = "0";
                ddlDepartment.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "BindDepartment");

            }
            finally
            {
            }
        }

        [WebMethod]
        public static List<BrilliantWMS.DesignationService.mDesignation> PMfillDesignation(int departmentID)
        {
            BrilliantWMS.DesignationService.iDesignationMasterClient DesignationService = new BrilliantWMS.DesignationService.iDesignationMasterClient();
            List<BrilliantWMS.DesignationService.mDesignation> DesignationList = new List<BrilliantWMS.DesignationService.mDesignation>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DesignationList = DesignationService.GetDesignationListByDepartmentID(departmentID, profile.DBConnection._constr).ToList();
                BrilliantWMS.DesignationService.mDesignation select = new BrilliantWMS.DesignationService.mDesignation() { ID = 0, Name = "-Select-" };
                DesignationList.Insert(0, select);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "User Creation", "PMfillDesignation");
            }
            finally
            { DesignationService.Close(); }
            return DesignationList;
        }
        #endregion

        #region DropDownSelectedIndexChanged
        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDesignation.DataSource = null;
                ddlDesignation.DataBind();
                int did = Convert.ToInt32(ddlDepartment.SelectedValue);
                //BindDesignation(did);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "ddlDepartment_SelectedIndexChanged");

            }
            finally
            {
            }
        }

        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FillRoleDropDown(); 
        }
        #endregion

        #region GVUserCreationCode

        //protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        //{
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iUserCreationClient userClient = new iUserCreationClient();

        //    hndRoleSate.Value = "Edit";
        //    ImageButton imgbtn = (ImageButton)sender;

        //    string prdSelValue = hdnUsrSelectedRec.Value.ToString();
        //    // hdnprodID.Value = imgbtn.ToolTip.ToString();
        //    hnduserID.Value = hdnUsrSelectedRec.Value.ToString();
        //    EditUserName = selectedrec["userIDPass"].ToString();
        //    // FillRoleDropDown();
        //    GetUserByID();
        //    ActiveTab("Edit");

        //}

        protected void gvUserCreationM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();              

                hndRoleSate.Value = "Edit";
                hdnSelectedLocation.Value = "";
                Hashtable selectedrec = (Hashtable)gvUserCreationM.SelectedRecords[0];
                hnduserID.Value = selectedrec["userID"].ToString();
                EditUserName = selectedrec["userIDPass"].ToString();
                // FillRoleDropDown();
                GetUserByID();
                RebindGrid(sender, e);
                ActiveTab("Edit");

                if (ddlcompany.SelectedValue != "0" || ddlcompany.SelectedValue != "")
                {
                    FillDept(Convert.ToInt64(ddlcompany.SelectedValue));
                    ddlDepartment.SelectedValue = selectedrec["DepartmentID"].ToString();
                    hdnSelectedDepartment.Value = selectedrec["DepartmentID"].ToString();

                    DataSet ds = new DataSet();
                    ds = userClient.GetUserDepartment(long.Parse(hnduserID.Value), profile.DBConnection._constr);
                    string DeptName = "";
                    long DeptID = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                        {
                            DeptName = ds.Tables[0].Rows[i]["Territory"].ToString();
                            DeptID = long.Parse(ds.Tables[0].Rows[i]["TerritoryID"].ToString());
                            var remove = "<a onclick=removeLocation('div" + hdnDivCount.Value.ToString() + "'," + DeptID + ") class='removeAL'>X</a>";
                            divSelectedLocation.InnerHtml = divSelectedLocation.InnerHtml.Replace("/^\\s+|\\s+$/g", "");
                            divSelectedLocation.InnerHtml += "<span class='newlocationdiv' id=div" + hdnDivCount.Value.ToString() + ">-" + DeptName + remove + "</span>";

                            if (i == 0) { hdnSelectedLocation.Value = DeptID.ToString(); }
                            else { hdnSelectedLocation.Value = hdnSelectedLocation.Value + ',' + DeptID; }
                        }
                    }
                    //GetUserByID();
                }
                lblPassword.Visible = lblConfirmPass.Visible = true;
                txtLoginId1.Visible = txtPassword1.Visible = txtConfirmPassword.Visible = true;
                req_txtLoginId.Visible = req_txtPassword.Visible = rfValtxtConfirmPassword.Visible = cmpValtxtPassword.Visible = true;
                //BindRoleDetailsGridView();

                /*New Code By Suresh for Warehouse Grid*/
                BindUserWarehouse(hnduserID.Value);
                /*New Code By Suresh*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "gvUserCreationM_Select");
            }
            finally
            {
                userClient.Close();
            }
        }

        private void BindUserWarehouse(string userID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {
                DataSet ds = new DataSet();
                grdwarehouse.DataSource = userClient.GetUserWarehouseDetails(userID, profile.DBConnection._constr);
                grdwarehouse.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "UserCreation", "BindUserWarehouse");
            }
            finally { userClient.Close();  }
        }

        private void GetUserByID()
        {
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {
                //string LocationList = ""; 
                Session["ProfileImg"] = null;
                //UC_Territory1.BindListviewWithGroupTitle(); Comment by vishal
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.UserCreationService.mUserProfileHead objuser = new BrilliantWMS.UserCreationService.mUserProfileHead();

                objuser = userClient.GetUserByID(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);

                //LocationList = userClient.GetLocationListByUserID(objuser.ID, profile.DBConnection._constr); Comment by vishal

                ResetUserControl(Convert.ToInt64(hnduserID.Value));
                //UC_AddressInformation1.FillAddressByObjectNameReferenceID(Convert.ToInt64(hnduserID.Value));
                //UCAddress1.FillAddressByObjectNameReferenceID("User", Convert.ToInt64(hnduserID.Value), "User");Comment by vishal

                txtFirstName.Text = objuser.FirstName;
                txtMiddleName.Text = objuser.MiddelName;
                txtLastName.Text = objuser.LastName;
                txtEmpNo.Text = objuser.EmployeeID;
                txtEmail.Text = objuser.EmailID;
                txtMobile.Text = objuser.MobileNo;

                ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(objuser.CompanyID.ToString()));

                if (ddlcompany.SelectedValue != "0" || ddlcompany.SelectedValue != "")
                {
                    UC_Territory uc_territory = new UC_Territory();
                    ddlDepartment.DataSource = null;
                    ddlDepartment.DataBind();
                    ddlDepartment.DataSource = uc_territory.GetDepartmentList(Convert.ToInt64(ddlcompany.SelectedValue)).ToList();
                    // ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByValue(objuser.DepartmentID.ToString()));
                }

                ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(objuser.UserType.ToString()));

                // ddlUserType.SelectedValue = objuser.UserType;
                ddlReportingTo.SelectedIndex = ddlReportingTo.Items.IndexOf(ddlReportingTo.Items.FindByValue(objuser.ReportingTo.ToString()));
                BindRole();

                // bindRollbyDept(long.Parse(objuser.DepartmentID.ToString()));

                ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(objuser.RoleID.ToString()));
                hdnrollid.Value = objuser.RoleID.ToString();

                ddlUserGender.SelectedValue = objuser.Gender;
                ddlmobile.SelectedValue = objuser.MobileInterface;


                hdnSelectedDepartment.Value = objuser.DepartmentID.ToString();
                //  getDelegateToList(long.Parse(hdnSelectedDepartment.Value));
                hndstate.Value = "Edit";
                BindAccessDelgrid(hndstate.Value, Convert.ToInt64(hnduserID.Value));
                DataTable dt = new DataTable();
                DataSet ds = userClient.GetUserLoginDetails(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtLoginId1.Text = dt.Rows[0]["UserName"].ToString();
                    hdncurrentpassword.Value = dt.Rows[0]["Password"].ToString();
                    txtPassword1.Text = dt.Rows[0]["Password"].ToString();
                    txtConfirmPassword.Text = dt.Rows[0]["Password"].ToString();
                }

                //if (ddlDepartment.SelectedValue != "0") Comment by vishal
                //{
                //    ddlDesignation.DataSource = null;
                //    ddlDesignation.DataBind();
                //    ddlDesignation.DataSource = PMfillDesignation(Convert.ToInt16(ddlDepartment.SelectedValue));
                //    ddlDesignation.DataBind();
                //    ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByValue(objuser.DesignationID.ToString()));
                //    FillRoleDropDown();
                //    ddlRoleList.SelectedIndex = ddlRoleList.Items.IndexOf(ddlRoleList.Items.FindByValue(objuser.RoleID.Value.ToString()));
                //    hndRoleSate.Value = "Edit";
                //    hndstate.Value = "Edit";
                //    BindRoleDetailsGridView();
                //}

                UC_Dateofjoining.Date = objuser.DateOfJoining;
                UC_DateofBirth.Date = objuser.DateOfBirth;
                if (objuser.Active.ToString() == "True")
                {
                    rbtnYes.Checked = true;
                    rbtnNo.Checked = false;
                }
                else
                {
                    rbtnYes.Checked = false;
                    rbtnNo.Checked = true;
                }

                if (objuser.ProfileImg != null)
                {
                    Session["ProfileImg"] = objuser.ProfileImg;
                    Img1.Src = "../Image.aspx";
                }
                else
                {
                    Img1.Src = "../App_Themes/Blue/img/Male.png";
                    if (objuser.Gender == "F")
                        Img1.Src = "../App_Themes/Blue/img/Female.png";

                }
            }
            catch (System.Exception ex)
            {
                // Login.Profile.ErrorHandling(ex, this, "User Creation", "GetUserByID");
            }
            finally
            {
                userClient.Close();
            }
        }

        protected void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iUserCreationClient userClient = new iUserCreationClient();
                List<vGWCGetUserProfileList> usrlst = new List<vGWCGetUserProfileList>();
                //usrlst = userClient.GetGWCUserList(profile.DBConnection._constr).ToList();
                usrlst = userClient.GetGWCUserListCompanyWise(profile.Personal.CompanyID, profile.DBConnection._constr).ToList();

                //gvUserCreationM.DataSource = userClient.GetUserCreationList(profile.DBConnection._constr);
                //gvUserCreationM.DataSource = userClient.GetGWCUserList(profile.DBConnection._constr);
                gvUserCreationM.DataSource = usrlst;
                gvUserCreationM.DataBind();
                userClient.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "GetUserByID");
            }

        }
        #endregion
        #endregion

        private void ResetUserControl(long referenceID)
        {
            try
            {
                if (hnduserID.Value.ToString() == "")
                {
                    hnduserID.Value = "0";
                }
                CustomProfile profile = CustomProfile.GetProfile();
                //UC_AddressInformation1.ResetAddress("User", Convert.ToInt64(hnduserID.Value), profile.Personal.UserID.ToString(), Session.SessionID, "UserCreation");
                UCAddress1.ClearAddress("User");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "GetUserByID");

            }
            finally
            {
            }
        }

        public string checkDuplicate()
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                if (txtEmpNo.Text != "")
                {
                    iUserCreationClient userClient = new iUserCreationClient();
                    if (hnduserID.Value == string.Empty)
                    {
                        result = userClient.checkDuplicateRecord(txtEmpNo.Text, profile.DBConnection._constr);
                        userClient.Close();
                        if (result != string.Empty)
                        {
                            WebMsgBox.MsgBox.Show(result);
                            txtEmpNo.Text = "";
                        }

                    }
                    else
                    {
                        int id = Convert.ToInt32(hnduserID.Value);
                        result = userClient.checkDuplicateRecordEdit(txtEmpNo.Text, id, profile.DBConnection._constr);
                        userClient.Close();
                        if (result != string.Empty)
                        {
                            WebMsgBox.MsgBox.Show(result);
                            txtEmpNo.Text = "";
                        }
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        private void ClearAll()
        {
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtLastName.Text = "";
            txtEmpNo.Text = "";
            ddlDepartment.SelectedIndex = -1;
            //BindDesignation(Convert.ToInt16(ddlDepartment.SelectedValue));
            ddlDesignation.SelectedIndex = -1;
            UC_Dateofjoining.Date = null;
            UC_DateofBirth.Date = null;
            txtEmail.Text = ""; ;
            // txtPhone.Text = "";
            txtMobile.Text = "";
            GridRoleConfiguration.DataSource = null;
            GridRoleConfiguration.DataBind();
            rbtnYes.Checked = true;
            txtLoginId1.Text = "";
            hndstate.Value = "";
            hnduserID.Value = "";
            // txtHighestQuali.Text = "";
            // txtotherID.Text = "";
            ddlRole.SelectedIndex = -1;
            ddlcompany.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlmobile.SelectedIndex = -1;
            // txtinstrated.Text = "";
            ddlUserType.SelectedIndex = -1;
            ddlReportingTo.SelectedIndex = -1;
            ddlUserGender.SelectedIndex = -1;
            UC_Date1.Date = null;
            UC_Date2.Date = null;
            txtPassword1.Text = "";
            txtConfirmPassword.Text = "";
            grdaccessdele.DataSource = null;
            grdaccessdele.DataBind();
            //lblPassword.Text = "";
            ActiveTab("Add");
            Img1.Src = "~/App_Themes/Blue/img/Male.png";
            // Img1.Src = null;

            divSelectedLocation.InnerHtml = "";

        }

        #region RoleConfiguration

        protected void BindRoleDetailsGridView()
        {
            BrilliantWMS.UserCreationService.iUserCreationClient UserCreationService = new BrilliantWMS.UserCreationService.iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GridRoleConfiguration.DataSource = null;
                GridRoleConfiguration.DataBind();

                List<SP_GetUserRoleDetail_Result> sessionList = new List<SP_GetUserRoleDetail_Result>();
                BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
                if (hndRoleSate.Value != "Edit")
                {
                    if (Convert.ToInt32(hdnDDLRoleSelectedValue.Value) > 1)
                    {
                        sessionList = UserCreationService.GetDataToBindRoleMasterDetailsByRoleID(Convert.ToInt32(hdnDDLRoleSelectedValue.Value), 0, profile.Personal.CompanyID, profile.DBConnection._constr).ToList();
                    }
                    else if (Convert.ToInt32(hdnDDLRoleSelectedValue.Value) == 1)
                    {
                        sessionList = UserCreationService.GetDataToBindRoleMasterDetailsByRoleID(1, 0, profile.Personal.CompanyID, profile.DBConnection._constr).ToList();
                    }
                    GridRoleConfiguration.GroupBy = "DisplayModuleName,DisplayPhaseName";
                    GridRoleConfiguration.ShowHeader = true;

                }

                if (hndRoleSate.Value == "Edit")
                {
                    sessionList = UserCreationService.GetDataToBindRoleMasterDetailsByRoleID(0, Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, profile.DBConnection._constr).ToList();
                    hndRoleSate.Value = "";
                }


                if (sessionList != null)
                {
                    if (sessionList.Count > 0)
                    {
                        GridRoleConfiguration.DataSource = sessionList;
                        GridRoleConfiguration.DataBind();
                        Session.Add("sessionRoleList", sessionList);
                    }
                }
                roleMasterService.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "BindRoleDetailsGridView");
            }
            finally
            {
                UserCreationService.Close();
            }
        }

        [WebMethod]
        public static void UpdateRole(object role, object rowIndex)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)role;

            SP_GetUserRoleDetail_Result updateRole = new SP_GetUserRoleDetail_Result();
            updateRole.mSequence = Convert.ToInt64(dictionary["mSequence"]);
            updateRole.pSequence = Convert.ToInt64(dictionary["pSequence"]);
            updateRole.oSequence = Convert.ToInt64(dictionary["oSequence"]);

            //updateRole.Add = Convert.ToBoolean(dictionary["Add"]);
            //updateRole.Edit = Convert.ToBoolean(dictionary["Edit"]);
            //updateRole.View = Convert.ToBoolean(dictionary["View"]);
            //updateRole.Delete = Convert.ToBoolean(dictionary["Delete"]);
            //updateRole.Approval = Convert.ToBoolean(dictionary["Approval"]);
            //updateRole.AssignTask = Convert.ToBoolean(dictionary["AssignTask"]);
            updateRole.Add = Convert.ToBoolean(dictionary["Add"]);
            if (updateRole.Add == true) { updateRole.Edit = true; updateRole.View = true; }
            else { updateRole.Edit = false; }
            //updateRole.View = true;
            updateRole.Delete = false;
            updateRole.Approval = Convert.ToBoolean(dictionary["Approval"]);
            if (updateRole.Approval == true) { updateRole.View = true; }
            updateRole.AssignTask = Convert.ToBoolean(dictionary["AssignTask"]);
            if (updateRole.AssignTask == true) { updateRole.View = true; }
            if (updateRole.Add == true && updateRole.Approval == false && updateRole.AssignTask == false && Convert.ToBoolean(dictionary["View"]) == true)
            { updateRole.View = true; }

            BrilliantWMS.UserCreationService.iUserCreationClient UserCreationService = new BrilliantWMS.UserCreationService.iUserCreationClient();
            HttpContext.Current.Session["sessionRoleList"] = UserCreationService.UpdateRoleIntoSessionList(getSessionList().ToArray(), updateRole, Convert.ToInt32(rowIndex)).ToList();
            UserCreationService.Close();
        }

        protected static List<SP_GetUserRoleDetail_Result> getSessionList()
        {
            List<SP_GetUserRoleDetail_Result> sessionList = new List<SP_GetUserRoleDetail_Result>();
            if (HttpContext.Current.Session["sessionRoleList"] != null) sessionList = (List<SP_GetUserRoleDetail_Result>)HttpContext.Current.Session["sessionRoleList"];
            return sessionList;
        }

        protected static List<SP_GWCGetUserRoleDetail_Result> getSessionList1()
        {
            List<SP_GWCGetUserRoleDetail_Result> sessionList = new List<SP_GWCGetUserRoleDetail_Result>();
            if (HttpContext.Current.Session["sessionRoleList"] != null) sessionList = (List<SP_GWCGetUserRoleDetail_Result>)HttpContext.Current.Session["sessionRoleList"];
            return sessionList;
        }


        [WebMethod]
        public static List<VGetRollNameByDeptID> PMfillRoleDDL(long DepartmentID, long DesignationID)
        {
            BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            List<VGetRollNameByDeptID> rolelist = new List<VGetRollNameByDeptID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // rolelist = roleMasterService.GetRoleMasterListByDepartmentIDDesignationID(DepartmentID, DesignationID, profile.DBConnection._constr).ToList();
                rolelist = roleMasterService.GetRoleMasterListByDepartmentID(DepartmentID, DesignationID, profile.DBConnection._constr).ToList();

            }
            catch { }
            finally { roleMasterService.Close(); }
            return rolelist;
        }

        protected void FillRoleDropDown()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ddlRoleList.DataSource = null;
                ddlRoleList.DataBind();
                ddlRoleList.Items.Clear();

                if (ddlDepartment.SelectedIndex > 0 && ddlDesignation.SelectedIndex > 0)
                {

                    BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
                    ddlRoleList.DataSource = roleMasterService.GetRoleMasterListByDepartmentIDDesignationID(Convert.ToInt64(ddlDepartment.SelectedItem.Value), Convert.ToInt64(ddlDesignation.SelectedItem.Value), profile.DBConnection._constr);
                    ddlRoleList.DataBind();
                    // roleMasterService.Close();
                    //ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlRoleList.Items.Insert(0, lst);
                    //ListItem lst1 = new ListItem { Text = "Custom", Value = "0.1" };
                    //ddlRoleList.Items.Insert(1, lst1);

                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "FillRoleDropDown");

            }
            finally
            {
            }
        }

        protected void ddlRoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRoleDetailsGridView();
        }

        #endregion RoleConfiguration

        protected void CreateProfile(string LoginName, long userID)
        {
            CustomProfile Cprofile = CustomProfile.GetProfile();
            CustomProfile profile = CustomProfile.GetProfile(LoginName);
            iUserCreationClient userClient = new iUserCreationClient();
            //profile.DBConnection._constr[0] = Cprofile.DBConnection._constr[0];
            //profile.DBConnection._constr[1] = Cprofile.DBConnection._constr[1];
            //profile.DBConnection._constr[2] = Cprofile.DBConnection._constr[2];
            //profile.DBConnection._constr[3] = Cprofile.DBConnection._constr[3];

            profile.DBConnection._constr[0] = "BWMSTest.db.11040877.c93.hostedresource.net";
            profile.DBConnection._constr[1] = "BWMSTest";
            profile.DBConnection._constr[2] = "Password123#";
            profile.DBConnection._constr[3] = "BWMSTest";



            //profile.Personal.UserID = userID;
            profile.Personal.UserID = userID;
            profile.Personal.UserName = txtFirstName.Text + " " + txtLastName.Text;
            profile.Personal.Gender = "";
            profile.Personal.UserType = Usertyperoll;
            if (UC_DateofBirth.Date != null)
            { profile.Personal.DateOfBirth = Convert.ToDateTime(UC_DateofBirth.Date); }
            //else { profile.Personal.DateOfBirth = null; }

            profile.Personal.EmailID = txtEmail.Text;
            profile.Personal.MobileNo = txtMobile.Text;

            profile.Personal.ProfileImageURL = "";
            profile.Personal.HeaderMenu = "";

            profile.Personal.ReportingTo = ddlReportingTo.SelectedValue;
            profile.Personal.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
            profile.Personal.Department = hdnDepartmentName.Value;
            profile.Personal.DesignationID = 0;
            profile.Personal.Designation = "";
            //profile.Personal.Department = ddlDepartment.SelectedItem.Text; comment by vishal

            //profile.Personal.DesignationID = Convert.ToInt64(ddlDesignation.SelectedValue);
            // profile.Personal.DesignationID = Convert.ToInt64(hndDesignationIndex.Value); comment by vishal

            //profile.Personal.Designation = hndDesignationValue.Value; comment by vishal
            //profile.Personal.Designation = ddlDesignation.SelectedItem.Text; Comment by vishal


            //profile.Personal.Designation = ddlDesignation.Items

            // string txt = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByValue(ddlDepartment.SelectedValue).ToString();
            //string txt = ddlDepartment.Items[ddlDepartment.SelectedIndex].Text;


            profile.Personal.IPAddress = "";
            profile.Personal.MachineID = "";


            profile.Personal.Gender = ddlUserGender.SelectedValue;
            profile.Personal.ProfileImg = (byte[])Session["ProfileImg"];

            /*Company Details*/
            profile.Personal.CompanyID = Convert.ToInt64(hdnSelectedCompany.Value);
            string logopath = userClient.GetLogoPath(Convert.ToInt64(hdnSelectedCompany.Value), profile.DBConnection._constr);
            profile.Personal.CName = "";
            // profile.Personal.CLogoURL = Cprofile.Personal.CLogoURL;
            profile.Personal.CLogoURL = logopath;
            profile.Personal.CRMUrl = "";



            /*Preferences*/
            profile.Personal.Theme = "";
            profile.Personal.TimeZone = "";
            profile.Personal.DateTime = "";

            profile.Save();
        }

        protected void GridRoleConfiguration_ColumnsCreated(object sender, EventArgs e)
        {
            int width = 700;
            int count = GridRoleConfiguration.Columns.Count;
            int average = width / count;
            int i = 0;

            //     foreach (Column column in GridRoleConfiguration.Columns)
            //     {
            //         column.Width = 140 + "px";
            //         i++;
            //    }
        }

        protected void lnkUpdateProfileImg_OnClick(object sender, EventArgs e)
        {
            //try
            //{

            //    Session["ProfileImg"] = FileUploadProfileImg.FileBytes;

            //    Img1.Src = "../Image.aspx";
            //    //hdnSelectedLocation.Value = "";
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, this, "User Creation", "lnkUpdateProfileImg_OnClick");
            //}
        }

        [WebMethod]
        public string Get_image(string file_content, string file_name)
        {
            string result = "";
            HttpContext.Current.Session["ProfileImg"] = file_content;
            return result;

        }


        [WebMethod]
        public static void PMMoveAddressToArchive(string Ids, string IsArchive)
        {
            BrilliantWMS.Address.UCAddress ucAddress = new Address.UCAddress();
            ucAddress.MoveAddressToArchive(Ids, IsArchive, "User");
        }

        #region Fill Territory
        [WebMethod]
        public static List<mTerritory> PMFillddlLevel(long level, long parentID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                TerritoryList = uc_territory.GetTerritoryList(level, parentID).ToList();
            }
            catch { }
            finally { }
            return TerritoryList;
        }

        [WebMethod]
        public static List<BrilliantWMS.ServiceTerritory.vGetUserProfileList> PMFillddlUserListByTerritory(long level, long parentID)
        {
            List<BrilliantWMS.ServiceTerritory.vGetUserProfileList> UserList = new List<BrilliantWMS.ServiceTerritory.vGetUserProfileList>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                UserList = uc_territory.GetUserListByTerritory(level, parentID).ToList();
            }
            catch { }
            finally { }
            return UserList;
        }

        #endregion

        protected void GridRoleConfiguration_OnRebind(object sender, EventArgs e)
        {
            BindRoleDetailsGridView();
        }


        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            lblheader.Text = rm.GetString("UserList", ci);
            TabPanelUsersList.HeaderText = rm.GetString("UserList", ci);
            lblmobileinter.Text = rm.GetString("MobileInterfaceApproval", ci);
            lblemployeeno.Text = rm.GetString("EmployeeNo", ci);
            lblfname.Text = rm.GetString("FirstName", ci);
            lblmname.Text = rm.GetString("Middlename", ci);
            lbllname.Text = rm.GetString("lastname", ci);
            //lblusertype.Text = rm.GetString("UserType", ci); comment by vishal
            lblrole.Text = rm.GetString("Role", ci);
            lblgender.Text = rm.GetString("Gender", ci);
            lbldept.Text = rm.GetString("Department", ci);
            lbldesig.Text = rm.GetString("Designation", ci);
            lbldoj.Text = rm.GetString("DateOfJoining", ci);
            lbldob.Text = rm.GetString("DateOfBirth", ci);
            lblemail.Text = rm.GetString("EmailID", ci);
            lblmobno.Text = rm.GetString("MobileNo", ci);
            lblactive.Text = rm.GetString("Active", ci);
            rbtnYes.Text = rm.GetString("Yes", ci);
            rbtnNo.Text = rm.GetString("No", ci);
            lblreporting.Text = rm.GetString("ReportingTo", ci);
            lnkUpdateProfileImg.Text = rm.GetString("Upload", ci);
            lblloginheader.Text = rm.GetString("LoginDetails", ci);
            lblusername.Text = rm.GetString("UserName", ci);
            lblPassword.Text = rm.GetString("Password", ci);
            lblConfirmPass.Text = rm.GetString("ConfirmPassword", ci);
            lbladdress.Text = rm.GetString("AddressInfo", ci);
            tabProductDetails.HeaderText = rm.GetString("AccessDelegation", ci);
            lblfrmdate.Text = rm.GetString("FromDate", ci);
            lbltodate.Text = rm.GetString("ToDate", ci);
            lblrightsto.Text = rm.GetString("Delegation", ci);
            lblremark.Text = rm.GetString("Remark", ci);
            lblaccdele.Text = rm.GetString("AccessDelegation", ci);
            TabPanelUserInformation.HeaderText = rm.GetString("UserInformation", ci);
            UCFormHeader1.FormHeaderText = rm.GetString("UserCreation", ci);
            lblcompany.Text = rm.GetString("company", ci);
            btnsumit.Value = rm.GetString("Submit", ci);
            btnAllocate.Value = rm.GetString("LockUnlock", ci);
            //lblassigndept.Text = rm.GetString("AssignDepartment", ci);
            // pnllocation.HeaderText = rm.GetString("Location", ci);
            btnContactPerson.Value = rm.GetString("AddLocation", ci);
            lbldisrtibution.Text = rm.GetString("LocationList", ci);


        }


        [WebMethod]
        public static List<mTerritory> PMGetDepartmentList(long CompanyID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
            }
            catch { }
            finally { }
            return TerritoryList;
        }


        [WebMethod]
        public static List<contact> GetDepartment(object objReq)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
                long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());
                ds = productClient.GetDepartment(ddlcompanyId, profile.DBConnection._constr);
                dt = ds.Tables[0];
                contact Loc = new contact();
                Loc.Name = "Select Department";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Territory"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "User Creation", "lnkUpdateProfileImg_OnClick");
            }
            finally
            {
                productClient.Close();
            }
            return LocList;
        }

        //[WebMethod]
        //  public static List<contact> GetDepartmentfrmSelCmpny(string hdnSelectedCompany)
        public void GetDepartmentfrmSelCmpny(string hdnSelectedCompany)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                long ddlcompanyId = long.Parse(hdnSelectedCompany.ToString());
                ds = productClient.GetDepartment(ddlcompanyId, profile.DBConnection._constr);

                ddlDepartment.DataSource = ds;
                ddlDepartment.DataBind();
                ListItem lst = new ListItem();
                lst.Text = "Select Department";
                lst.Value = "0";
                ddlDepartment.Items.Insert(0, lst);

                dt = ds.Tables[0];
                contact Loc = new contact();
                Loc.Name = "Select Department";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Territory"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "User Creation", "lnkUpdateProfileImg_OnClick");
            }
            finally
            {
                productClient.Close();
            }
            //  return LocList;
        }

        [WebMethod]
        public static List<BrilliantWMS.DesignationService.mDesignation> PMGetDesignation(long CompanyID, long DeptID)
        {
            BrilliantWMS.DesignationService.iDesignationMasterClient DesignationService = new BrilliantWMS.DesignationService.iDesignationMasterClient();
            List<BrilliantWMS.DesignationService.mDesignation> DesignationList = new List<BrilliantWMS.DesignationService.mDesignation>();
            try
            {

                CustomProfile profile = CustomProfile.GetProfile();
                DesignationList = DesignationService.GetDesignationListBycompanddept(CompanyID, DeptID, profile.DBConnection._constr).ToList();
                //DesignationService.mDesignation select = new DesignationService.mDesignation() { ID = 0, Name = "-Select-" };
                //DesignationList.Insert(0, select);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "User Creation", "PMfillDesignation");
            }
            finally
            { DesignationService.Close(); }
            return DesignationList;
        }


        public void GetRoleDetails(long RoleID)
        {
            BrilliantWMS.UserCreationService.iUserCreationClient UserCreationService = new BrilliantWMS.UserCreationService.iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            List<SP_GWCGetUserRoleDetail_Result> sessionList = new List<SP_GWCGetUserRoleDetail_Result>();
            try
            {

                sessionList = UserCreationService.GetRoleDetails(RoleID, 0, 0, profile.DBConnection._constr).ToList();

                if (sessionList != null)
                {
                    if (sessionList.Count > 0)
                    {

                        Session.Add("sessionRoleList", sessionList);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "User Creation", "GetRoleDetails");
            }
            finally
            {
                UserCreationService.Close();
            }
        }

        public void FillDept(long CompanyID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
                ddlDepartment.DataSource = TerritoryList;
                ddlDepartment.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlDepartment.Items.Insert(0, lst);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDept");

            }
            finally
            {
            }
        }

        # region new WMS User Code Addition Access Deligation
        protected void grdaccessdele_RebindGrid(object sender, EventArgs e)
        {
            if (hndstate.Value == "Edit")
            {
                BindAccessDelgrid(hndstate.Value, Convert.ToInt64(hnduserID.Value));
            }
            else
            {
                BindAccessDelgrid(hndstate.Value, 0);
            }
        }

        public void BindAccessDelgrid(string state, long UserId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            ds = userClient.GetUserDelegationDetail(state, UserId, profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                grdaccessdele.DataSource = ds.Tables[0];
                grdaccessdele.DataBind();
            }
            else
            {
                grdaccessdele.DataSource = null;
                grdaccessdele.DataBind();

            }
        }

        [WebMethod]
        public static string SaveAccessDelegation(object objReq)
        {
            string result = "";
            try
            {
                string hiddneedit = "";
                iUserCreationClient userClient = new iUserCreationClient();
                CustomProfile profile = CustomProfile.GetProfile();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long Delegateto = long.Parse(dictionary["Delegateto"].ToString());
                string Remark = dictionary["Remark"].ToString();
                string hndstate = dictionary["hndstate"].ToString();
                //long UserId = long.Parse(dictionary["UserId"].ToString());
                DateTime FromDate = Convert.ToDateTime(dictionary["FromDate"].ToString());
                DateTime ToDate = Convert.ToDateTime(dictionary["ToDate"].ToString());
                // long newdelegate = long.Parse(dictionary["newDelegate"].ToString());
                long DelegateId = 0;
                long delegatefrom = 0;
                if (HttpContext.Current.Session["hdnedit"].ToString() != "" || HttpContext.Current.Session["hdnedit"].ToString() != null)
                {
                    hiddneedit = HttpContext.Current.Session["hdnedit"].ToString();
                }

                string hdndelegatestateedit = dictionary["hdndeligateeditstate"].ToString();
                if (hndstate == "Edit")
                {

                    if (hiddneedit == "Edit")
                    {
                        string hdndeligateeditstate = hiddneedit.ToString();
                        hndstate = hdndeligateeditstate.ToString();
                        string delid = HttpContext.Current.Session["hdnnewDelegateid"].ToString();
                        DelegateId = long.Parse(delid.ToString());
                        HttpContext.Current.Session["hdnnewDelegateid"] = "";
                        HttpContext.Current.Session["hdnedit"] = "";
                    }
                    else
                    {
                        hndstate = "Add";
                    }
                }
                else
                {
                    if (hiddneedit == "Edit")
                    {
                        string hdndeligateeditstate = hiddneedit.ToString();
                        hndstate = hdndeligateeditstate.ToString();
                        string delid = HttpContext.Current.Session["hdnnewDelegateid"].ToString();
                        DelegateId = long.Parse(delid.ToString());
                        HttpContext.Current.Session["hdnnewDelegateid"] = "";
                        HttpContext.Current.Session["hdnedit"] = "";

                    }
                }

                //  userClient.SaveEditUserDelegation(DelegateId, delegatefrom, FromDate, ToDate, Delegateto, Remark, hndstate, profile.Personal.UserID, DateTime.Now, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Access Delegation", "SaveAccessDelegation");

            }
            finally
            {
            }

            return result;
        }

        public void GetDelegationDetail(long Delegateid)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                DataSet ds;
                DataTable dt;
                //ds = productClient.GetBOMDetailById(BOMDetailId, profile.DBConnection._constr);
                ds = userClient.getUserDelegateDetail(Delegateid, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    long deptId = long.Parse(hdnSelectedDepartment.Value);
                    getDelegateToList(deptId);
                    if (ddlUOM.Items.Count >= 1) ddlUOM.SelectedIndex = 0;
                    ddlUOM.SelectedIndex = ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(dt.Rows[0]["DeligateTo"].ToString()));
                    ddlUOM.SelectedItem.Value = dt.Rows[0]["DeligateTo"].ToString();
                    //ddlUOM.SelectedItem.Text = dt.Rows[0]["Name"].ToString();

                    hdndegateId.Value = Delegateid.ToString();

                    txtPrincipalPrice.Text = dt.Rows[0]["Remark"].ToString();
                    UC_Date1.Date = Convert.ToDateTime(dt.Rows[0]["FromDate"].ToString());
                    UC_Date2.Date = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString());

                    hdndeligateeditstate.Value = "Edit";
                    hdnnewDelegateid.Value = dt.Rows[0]["ID"].ToString();
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Access Delegation", "SaveAccessDelegation");
            }
            finally
            {
                userClient.Close();
            }


        }

        protected void imgBtnEditbom_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            hdnnewDelegateid.Value = imgbtn.ToolTip.ToString();
            hdndegateId.Value = imgbtn.ToolTip.ToString();
            hdndeligateeditstate.Value = "Edit";
            GetDelegationDetail(long.Parse(hdndegateId.Value));
            //string editsession = "Edit";

            Session["hdnedit"] = "Edit";
            Session.Add("hdnnewDelegateid", hdnnewDelegateid.Value);
        }

        public void getDelegateToList(long DeptId)
        {

            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            DataSet ds;
            //ds = userClient.getDelegateToList(DeptId,profile.DBConnection._constr);
            ds = userClient.getDelegateToListMultipleDept(hdnSelectedLocation.Value, profile.DBConnection._constr);
            ddlUOM.DataSource = ds;
            ddlUOM.DataTextField = "Name";
            ddlUOM.DataValueField = "ID";
            ddlUOM.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlUOM.Items.Insert(0, lst);

        }

        [WebMethod]
        public static List<contact> Getdelegate(object objReq)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
                long ddlDepartment = long.Parse(dictionary["ddlDepartment"].ToString());
                long UserID = long.Parse(profile.Personal.UserID.ToString());
                ds = userClient.getDelegateToList(ddlDepartment, UserID, profile.DBConnection._constr);

                dt = ds.Tables[0];


                contact Loc = new contact();
                Loc.Name = "--Select--";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Name"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();

                    }

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Access Delegation", "Getdelegate");
            }
            finally
            {
                userClient.Close();

            }
            return LocList;
        }


        [WebMethod]
        public static List<contact> GetRollById(object objReq)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
                long ddlDepartment = long.Parse(dictionary["ddlDepartment"].ToString());

                // ds = userClient.getDelegateToList(ddlDepartment, profile.DBConnection._constr);
                ds = userClient.GetRollNameById(ddlDepartment, profile.DBConnection._constr);

                dt = ds.Tables[0];


                contact Loc = new contact();
                Loc.Name = "--Select--";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["Id"].ToString();
                        Loc.Name = dt.Rows[i]["Name"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();

                    }

                }
            }
            catch
            {
            }
            finally
            {
                userClient.Close();

            }
            return LocList;
        }




        public class contact
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        #endregion


        public void SaveForAdmin()
        {
            bool chkDuplicate_aspnetmember = false;

            if (hndstate.Value != "Edit")
            {
                if (Membership.GetUser(txtLoginId1.Text.Trim()) != null) { chkDuplicate_aspnetmember = true; }
            }

            if (chkDuplicate_aspnetmember == true)
            {
                txtLoginId1.Text = "";
                MsgBox.Show("Login name already exist");
            }
            else if (chkDuplicate_aspnetmember == false)
            {
                iUserCreationClient userClient = new iUserCreationClient();
                //if (checkDuplicate() == "")
                //{

                BrilliantWMS.UserCreationService.mUserProfileHead objuser = new BrilliantWMS.UserCreationService.mUserProfileHead();
                CustomProfile profile = CustomProfile.GetProfile();


                if (hndstate.Value == "Edit")
                {
                    objuser = userClient.GetUserByID(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);
                }
                objuser.RoleID = 0;
                // if (ddlRoleList.SelectedIndex > 1)
                if (ddlRole.SelectedIndex > 1)
                {
                    objuser.RoleID = Convert.ToInt64(ddlRole.SelectedItem.Value);
                    GetRoleDetails(Convert.ToInt64(ddlRole.SelectedItem.Value));
                }

                objuser.CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                hdnSelectedCompany.Value = objuser.CompanyID.ToString();
                // hdnSelectedDepartment.Value = "0";
                objuser.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
                objuser.EmployeeID = txtEmpNo.Text.Trim();
                objuser.FirstName = txtFirstName.Text.Trim();
                objuser.MiddelName = txtMiddleName.Text.Trim();
                objuser.LastName = txtLastName.Text.Trim();

                //objuser.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                //iDesignationMasterClient DesignationMasterClient = new iDesignationMasterClient();
                //long i = DesignationMasterClient.GetDesignationIDByName((hndDesignationValue.Value), profile.DBConnection._constr);

                //Add By Suresh
                //if (hndDesignationValue.Value == "")//Comment by vishal
                if (hdnSelectedDesignation.Value == "")
                {
                    ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByValue(objuser.DesignationID.ToString()));
                    objuser.DesignationID = 1;
                }
                else
                {
                    //objuser.DesignationID = Convert.ToInt64(hndDesignationValue.Value); Comment by vishal
                    objuser.DesignationID = 1;
                }

                objuser.DateOfJoining = UC_Dateofjoining.Date;
                objuser.DateOfBirth = UC_DateofBirth.Date;
                //objuser.PhoneNo = txtPhone.Text.Trim();
                objuser.MobileNo = txtMobile.Text.Trim();
                objuser.EmailID = txtEmail.Text.Trim();
                objuser.ReportingTo = ddlReportingTo.SelectedValue;

                string Usertype = userClient.GetUserTypeByRoll(Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);

                //objuser.UserType = ddlRole.SelectedItem.Text;
                objuser.UserType = Usertype;
                objuser.Gender = ddlUserGender.SelectedValue;
                //objuser.InterestedIn = txtinstrated.Text.Trim();
                objuser.Hobbies = "";
                // objuser.OtherID = txtotherID.Text.Trim();
                //objuser.HighestQualification = txtHighestQuali.Text.Trim();
                objuser.CollegeOrUniversity = "";
                objuser.HighSchool = "";
                objuser.Remark = "";
                objuser.Active = true;
                if (rbtnNo.Checked == true) objuser.Active = false;
                //objuser.CompanyID = profile.Personal.CompanyID;
                objuser.ProfileImg = (byte[])Session["ProfileImg"];
                objuser.MobileInterface = ddlmobile.SelectedValue;
                objuser.DefaultAddress = UCAddress1.BillingSeq.Trim();



                if (hndstate.Value != "Edit")
                {
                    hndRoleSate.Value = "";
                    objuser.CreatedBy = profile.Personal.UserID.ToString();
                    objuser.CreationDate = DateTime.Now;
                    hnduserID.Value = userClient.InsertUserCreation(objuser, profile.DBConnection._constr).ToString();
                    Membership.CreateUser(txtLoginId1.Text, txtPassword1.Text, txtEmail.Text);
                    CreateProfile(txtLoginId1.Text, Convert.ToInt64(hnduserID.Value));

                    long ProfileID = Convert.ToInt64(hnduserID.Value);
                    userClient.SavePasswordDetails(ProfileID, txtEmail.Text, txtLoginId1.Text, txtPassword1.Text, profile.Personal.UserName, profile.DBConnection._constr);
                }
                else if (hndstate.Value == "Edit")
                {
                    hndRoleSate.Value = "Edit";
                    objuser.LastModifiedBy = profile.Personal.UserID.ToString();
                    objuser.LastModifiedDate = DateTime.Now;
                    hnduserID.Value = userClient.UpdateUserProfile(objuser, profile.DBConnection._constr).ToString();

                    CreateProfile(EditUserName, Convert.ToInt64(hnduserID.Value));//New Add By Suresh
                    profile.Personal.ProfileImg = (byte[])Session["ProfileImg"];
                    profile.Save();

                    //var u = Membership.GetUser(txtLoginId.Text.Trim());
                    //u.Email = txtEmail.Text;
                    //u.ChangePassword(u.GetPassword(), txtPassword.Text);                      
                }

                if (hnduserID.Value != "0" && hnduserID.Value != "")
                {
                    UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.User, Convert.ToInt64(hnduserID.Value));
                    bool roelSaveResult = false;
                    userClient.UpdateDelegateFrom(long.Parse(hnduserID.Value), profile.DBConnection._constr);
                    //if (ddlRoleList.SelectedIndex == -1 || ddlRoleList.SelectedIndex == 0) Comment by vishal
                    //    roelSaveResult = userClient.FinalSaveUserRoles(getSessionList().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, 0, profile.DBConnection._constr);
                    //if (ddlRoleList.SelectedIndex > 1) Comment by vishal
                    //    roelSaveResult = userClient.FinalSaveUserRoles(getSessionList().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, Convert.ToInt64(ddlRoleList.SelectedItem.Value), profile.DBConnection._constr);
                    if (ddlRole.SelectedIndex == -1 || ddlRole.SelectedIndex == 0 || hdnrollid.Value == "")
                    {
                        //roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, 0, profile.DBConnection._constr);
                        roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), "0", Convert.ToInt64(hnduserID.Value), Convert.ToInt64(hdnSelectedCompany.Value), Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);
                    }
                    if (ddlRole.SelectedIndex > 1 || hdnrollid.Value != "")
                    {
                        roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), "0", Convert.ToInt64(hnduserID.Value), Convert.ToInt64(hdnSelectedCompany.Value), Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);
                    }
                    if (hdnSelectedCompany.Value != "")
                    {
                        long companyId = long.Parse(hdnSelectedCompany.Value);

                        DataSet ds = new DataSet();
                        DataTable dt;
                        string ID = "";
                        string Ids = "";
                        long level = 999999;
                        ds = userClient.GetDepartmentforUsersave(companyId, profile.DBConnection._constr);
                        dt = ds.Tables[0];
                        //if (dt.Rows.Count > 0)
                        //{ 
                        //     for (int i = 0; i < dt.Rows.Count; i++)
                        //     {
                        //        ID = ID +','+ dt.Rows[i]["ID"].ToString();

                        //     }
                        //     Ids = ID.Substring(1);
                        //     userClient.SaveUsersLocationDetails(Convert.ToInt64(hnduserID.Value), level, Ids, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                        //}

                        userClient.SaveUsersLocationDetails(Convert.ToInt64(hnduserID.Value), level, hdnSelectedLocation.Value, profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                        //userClient.SaveUsersLocationDetails(Convert.ToInt64(hnduserID.Value), (ddlDesignation.SelectedIndex + 1), hdnSelectedLocation.Value, profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                    }
                    if (roelSaveResult == true)
                    {
                        if (hndstate.Value != "Edit")
                            MsgBox.Show("Record save successfully");
                    }
                    if (hndstate.Value == "Edit")
                    {
                        MsgBox.Show("Record update successfully");
                    }
                    ClearAll();
                    hndstate.Value = "Edit";
                    BindGrid();
                    lblPassword.Visible = lblConfirmPass.Visible = false;
                    txtLoginId1.Visible = txtPassword1.Visible = txtConfirmPassword.Visible = false;
                    req_txtLoginId.Visible = req_txtPassword.Visible = rfValtxtConfirmPassword.Visible = cmpValtxtPassword.Visible = false;
                }
                //}
                //ActiveTab("Add");
                userClient.Close();
            }
        }


        public void SaveForNormalUser()
        {
            bool chkDuplicate_aspnetmember = false;
            string str = "";
            int Approvallst = 0;

            if (hndstate.Value != "Edit")
            {
                if (Membership.GetUser(txtLoginId1.Text.Trim()) != null) { chkDuplicate_aspnetmember = true; }
            }

            if (chkDuplicate_aspnetmember == true)
            {
                txtLoginId1.Text = "";
                MsgBox.Show("Login name already exist");
            }
            else if (chkDuplicate_aspnetmember == false)
            {
                iUserCreationClient userClient = new iUserCreationClient();

                try
                {

                    //if (checkDuplicate() == "")
                    //{
                    hdnSelectedLocation.Value = "10337"; hdnSelectedDepartment.Value = "10337";//TEmparary Value
                    if (hdnSelectedLocation.Value == "")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showAlert('Please Click on Assign Department...','Error','#');", true);
                        GetDepartmentfrmSelCmpny(hdnSelectedCompany.Value);
                    }
                    else if (ddlRole.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showAlert('Please Select Role...','Error','#');", true); ddlRole.Focus();
                        GetDepartmentfrmSelCmpny(hdnSelectedCompany.Value);
                    }
                    else
                    {
                        BrilliantWMS.UserCreationService.mUserProfileHead objuser = new BrilliantWMS.UserCreationService.mUserProfileHead();
                        CustomProfile profile = CustomProfile.GetProfile();
                        Usertyperoll = userClient.GetUserTypeByRoll(Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);
                        if (hndstate.Value == "Edit")
                        {
                            objuser = userClient.GetUserByID(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);
                        }
                        objuser.RoleID = 0;
                        // if (ddlRoleList.SelectedIndex > 1)
                        if (ddlRole.SelectedIndex > 1 || hdnrollid.Value != "")
                        {
                            objuser.RoleID = Convert.ToInt64(hdnrollid.Value);
                            GetRoleDetails(Convert.ToInt64(hdnrollid.Value));
                        }

                        objuser.CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                        hdnSelectedCompany.Value = objuser.CompanyID.ToString();
                        objuser.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
                        objuser.EmployeeID = txtEmpNo.Text.Trim();
                        objuser.FirstName = txtFirstName.Text.Trim();
                        objuser.MiddelName = txtMiddleName.Text.Trim();
                        objuser.LastName = txtLastName.Text.Trim();

                        //objuser.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                        //iDesignationMasterClient DesignationMasterClient = new iDesignationMasterClient();
                        //long i = DesignationMasterClient.GetDesignationIDByName((hndDesignationValue.Value), profile.DBConnection._constr);

                        //Add By Suresh
                        //if (hndDesignationValue.Value == "")//Comment by vishal
                        if (hdnSelectedDesignation.Value == "")
                        {
                            ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByValue(objuser.DesignationID.ToString()));
                            objuser.DesignationID = 1;
                        }
                        else
                        {
                            //objuser.DesignationID = Convert.ToInt64(hndDesignationValue.Value); Comment by vishal
                            objuser.DesignationID = 1;
                        }

                        objuser.DateOfJoining = UC_Dateofjoining.Date;
                        objuser.DateOfBirth = UC_DateofBirth.Date;
                        //objuser.PhoneNo = txtPhone.Text.Trim();
                        objuser.MobileNo = txtMobile.Text.Trim();
                        objuser.EmailID = txtEmail.Text.Trim();
                        objuser.ReportingTo = ddlReportingTo.SelectedValue;

                        string Usertype = userClient.GetUserTypeByRoll(Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);

                        //objuser.UserType = ddlRole.SelectedItem.Text;
                        objuser.UserType = Usertype;
                        objuser.Gender = ddlUserGender.SelectedValue;
                        //objuser.InterestedIn = txtinstrated.Text.Trim();
                        objuser.Hobbies = "";
                        // objuser.OtherID = txtotherID.Text.Trim();
                        //objuser.HighestQualification = txtHighestQuali.Text.Trim();
                        objuser.CollegeOrUniversity = "";
                        objuser.HighSchool = "";
                        objuser.Remark = "";

                        //objuser.CompanyID = profile.Personal.CompanyID;
                        Session["ProfileImg"] = FileUploadProfileImg.FileBytes;
                        objuser.ProfileImg = (byte[])Session["ProfileImg"];

                        objuser.MobileInterface = ddlmobile.SelectedValue;
                        objuser.DefaultAddress = UCAddress1.BillingSeq.Trim();

                        objuser.Active = true;
                        if (rbtnNo.Checked == true)
                        {
                            objuser.Active = false;
                            if (hndstate.Value == "Edit")
                            {
                                Approvallst = userClient.GetApprovalDetailsOfUser(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);
                            }
                        }

                        if (hndstate.Value != "Edit")
                        {
                            hndRoleSate.Value = "";
                            objuser.CreatedBy = profile.Personal.UserID.ToString();
                            objuser.CreationDate = DateTime.Now;
                            hnduserID.Value = userClient.InsertUserCreation(objuser, profile.DBConnection._constr).ToString();
                            Membership.CreateUser(txtLoginId1.Text, txtPassword1.Text, txtEmail.Text);
                            CreateProfile(txtLoginId1.Text, Convert.ToInt64(hnduserID.Value));

                            long ProfileID = Convert.ToInt64(hnduserID.Value);
                            userClient.SavePasswordDetails(ProfileID, txtEmail.Text, txtLoginId1.Text, txtPassword1.Text, profile.Personal.UserName, profile.DBConnection._constr);
                        }
                        else if (hndstate.Value == "Edit" && Approvallst == 0)
                        {
                            hndRoleSate.Value = "Edit";
                            objuser.LastModifiedBy = profile.Personal.UserID.ToString();
                            objuser.LastModifiedDate = DateTime.Now;
                            hnduserID.Value = userClient.UpdateUserProfile(objuser, profile.DBConnection._constr).ToString();
                            CreateProfile(EditUserName, Convert.ToInt64(hnduserID.Value));//New Add By Suresh
                            //profile.Personal.ProfileImg = (byte[])Session["ProfileImg"];
                            //profile.Save();

                            if (hdncurrentpassword.Value != txtPassword1.Text.Trim())
                            {
                                DataSet ds = new DataSet();

                                ds = userClient.CheckPasswordHistory(Convert.ToInt64(hnduserID.Value), txtPassword1.Text, profile.DBConnection._constr);
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["Password"].ToString() == txtPassword1.Text)
                                    {
                                        str = "PasswordFound";
                                    }
                                }
                                if (str == "")
                                {
                                    string OneDayValidation = userClient.CheckOneDayValidation(Convert.ToInt64(hnduserID.Value), profile.DBConnection._constr);
                                    str = OneDayValidation;
                                }

                                if (str != "PasswordFound")
                                {
                                    if (str != "SameDay")
                                    {
                                        MembershipUser u = Membership.GetUser(txtLoginId1.Text.Trim());
                                        u.ChangePassword(hdncurrentpassword.Value, txtPassword1.Text.Trim());
                                        userClient.SavePasswordDetails(Convert.ToInt64(hnduserID.Value), txtEmail.Text, txtLoginId1.Text, txtPassword1.Text, profile.Personal.UserName, profile.DBConnection._constr);
                                        SendInstituteMail(txtEmail.Text, txtPassword1.Text);
                                    }
                                    else
                                    {
                                        MsgBox.Show("You can not change password within 1 day.");
                                    }
                                }
                                else
                                {
                                    MsgBox.Show("Password already exist");
                                }
                            }

                        }

                        if (hnduserID.Value != "0" && hnduserID.Value != "")
                        {
                            UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.User, Convert.ToInt64(hnduserID.Value));
                            bool roelSaveResult = false;
                            userClient.UpdateDelegateFrom(long.Parse(hnduserID.Value), profile.DBConnection._constr);
                            //if (ddlRoleList.SelectedIndex == -1 || ddlRoleList.SelectedIndex == 0) Comment by vishal
                            //    roelSaveResult = userClient.FinalSaveUserRoles(getSessionList().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, 0, profile.DBConnection._constr);
                            //if (ddlRoleList.SelectedIndex > 1) Comment by vishal
                            //    roelSaveResult = userClient.FinalSaveUserRoles(getSessionList().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, Convert.ToInt64(ddlRoleList.SelectedItem.Value), profile.DBConnection._constr);
                            if (ddlRole.SelectedIndex == -1 || ddlRole.SelectedIndex == 0 || hdnrollid.Value == "")
                            {
                                //roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), profile.Personal.UserID.ToString(), Convert.ToInt64(hnduserID.Value), profile.Personal.CompanyID, 0, profile.DBConnection._constr);
                                roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), "0", Convert.ToInt64(hnduserID.Value), Convert.ToInt64(hdnSelectedCompany.Value), Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);
                            }
                            if (ddlRole.SelectedIndex > 1 || hdnrollid.Value != "")
                            {
                                roelSaveResult = userClient.FinalSaveGWCUserRoles(getSessionList1().ToArray(), "0", Convert.ToInt64(hnduserID.Value), Convert.ToInt64(hdnSelectedCompany.Value), Convert.ToInt64(hdnrollid.Value), profile.DBConnection._constr);
                            }
                            if (hdnSelectedCompany.Value != "")
                            {
                                long level = 0;
                                //userClient.SaveUsersLocationDetails(Convert.ToInt64(hnduserID.Value), level, hdnSelectedDepartment.Value, profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                                userClient.SaveUsersLocationDetails(Convert.ToInt64(hnduserID.Value), level, hdnSelectedLocation.Value, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                            }
                            if (roelSaveResult == true)
                            {
                                if (hndstate.Value != "Edit")
                                    MsgBox.Show("Record save successfully");
                            }
                            if (hndstate.Value == "Edit" && Approvallst == 0 && str == "")
                            {
                                MsgBox.Show("Record update successfully");
                            }
                            else if (Approvallst > 0)
                            {
                                MsgBox.Show("This User Is In Approval Level. First Remove the User From Approval Level Then Set This User To Not Active");
                            }
                            if (str == "")
                            {
                                ClearAll();
                                hndstate.Value = "Edit";
                                BindGrid();
                                lblPassword.Visible = lblConfirmPass.Visible = false;
                                txtLoginId1.Visible = txtPassword1.Visible = txtConfirmPassword.Visible = false;
                                req_txtLoginId.Visible = req_txtPassword.Visible = rfValtxtConfirmPassword.Visible = cmpValtxtPassword.Visible = false;
                                ActiveTab("List");
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Login.Profile.ErrorHandling(ex, "Save", "SaveforNormaluser");
                }
                finally
                {
                    userClient.Close();
                }

            }
        }

        [WebMethod]
        public static int WMlockunlock(string UserID, string Lockvalue)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            byte lockunlock;
            if (Lockvalue == "No")
            {
                lockunlock = 1;
            }
            else
            {
                lockunlock = 0;
            }

            userClient.updatelockunlock(UserID, lockunlock, profile.DBConnection._constr);
            //int aspt = admn.AssignPartner(long.Parse(hdnResourceID), SelectedCPN);
            return 1;
        }

        protected void gvUserCreationM_RebindGrid(object sender, EventArgs e)
        {
            BindGrid();
        }

        public static void SendInstituteMail(string EmailId, string Password)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("mail.domain.com");

            try
            {

                mail.To.Add(EmailId);
                mail.From = new MailAddress("admin@elegantcrm.com");
                mail.Subject = "Your Password has ChangedBy OMS Super Admin";
                mail.Body += "<p style='color:black;font-size:17px;font-family:Times New Roman;'> Your Password has changed by Super Admin.You need have to login with new Password" + "<br/>";// +"<br/>";
                mail.Body += "<p style='color:Navy;font-size:16px;font-family:Times New Roman;'>Your New Password :" + "&nbsp;&nbsp;" + Password + "<p>";
                mail.Body += "<b style='font-family:Times New Roman;font-size:15px;'>Thank You !!!</b>" + "<br/>"; //+ "<br/>";
                mail.Body += "<b style='font-family:Times New Roman;font-size:15px;'>OMS Notification Team</b>" + "<br/>" + "<br/>";
                mail.IsBodyHtml = true;
                smtp.Host = "smtpout.asia.secureserver.net";
                smtp.Port = 80;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential("admin@elegantcrm.com", "fortis11");

                smtp.Send(mail);
            }

            catch (Exception ex)
            {
                mail = null;
                smtp = null;
            }
        }

        #region  code for Location add tab
        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst();

                Grid1.DataSource = ds;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst()
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            if (hndstate.Value == "Edit")
            {
                // str = "select ad.Id,cp.ID as contactid ,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID inner join mAddDistribution ad on cp.ID = ad.ContactID where cp.ID in (Select ContactID from mAddDistribution where ad.TemplateID = 0 or ad.TemplateID = " + long.Parse(hdnTemplateID.Value) + ")";
                str = "select UL.Id,A.LocationCode,A.AddressLine1, A.City, A.State, A.ContactName, A.ContactEmail from tAddress A inner join mUserLocation UL on A.ID = UL.LocationID where UserID = '" + long.Parse(hnduserID.Value) + "'";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);

            }
            else
            {
                str = "select ad.Id,cp.ID as contactid ,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID inner join mAddDistribution ad on cp.ID = ad.ContactID where cp.ID in (Select ContactID from mAddDistribution where ad.TemplateID = 0)";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        [WebMethod]
        public static string RemoveSku(object objReq)
        {
            string result = "";
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;
            long userlocid = long.Parse(dictionary["distribId"].ToString());
            userClient.RemoveUserLoc(userlocid, profile.DBConnection._constr);
            result = "success";
            return result;
        }

        #endregion

    }
}
