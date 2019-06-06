using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BrilliantWMS.RoleMasterService;
using Obout.Grid;
using System.Collections;
using BrilliantWMS.Login;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.UserCreationService;
using BrilliantWMS.ServiceTerritory;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.Territory;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using BrilliantWMS.DepartmentService;
using BrilliantWMS.DesignationService;


namespace BrilliantWMS.UserManagement
{
    public partial class RoleMaster : System.Web.UI.Page
    {
        //connectiondetails AppConnectionDetails = new connectiondetails();
        ResourceManager rm;
        CultureInfo ci;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            //UCFormHeader1.FormHeaderText = "Role Master";
            if (!IsPostBack) 
            {
                FillCompany();
                FillUserType();
                //FillDepartment(); 
                Session["sessionRoleList"] = null;
                BindRoleMasterGridView();
                clear();
                setActiveTab(0); 
            }
            //this.UCToolbar1.ToolbarAccess("RoleMaster","");
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickClear += pageClear;

            CustomProfile profile = CustomProfile.GetProfile();
            if (!IsPostBack)
            {
                
                //if (profile.Personal.CompanyID == 14)
                //{
                    //Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                    //btnExport.Visible = false;
                    //Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                    //btnImport.Visible = false;
                    //Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                    //btmMail.Visible = false;
                    //Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                    //btnPrint.Visible = false;
                //}
            }
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        public void FillUserType()
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlUserType.DataSource = userClient.GetUserType(profile.DBConnection._constr);
            ddlUserType.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlUserType.Items.Insert(0, lst);
        }
        //public void FillCompany()
        //{
        //    iUserCreationClient userClient = new iUserCreationClient();
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    ddlcompany.DataSource = userClient.GetCompanyName(profile.DBConnection._constr);
        //    ddlcompany.DataBind();
        //    ListItem lst = new ListItem();
        //    lst.Text = "--Select--";
        //    lst.Value = "0";
        //    ddlcompany.Items.Insert(0, lst);
        //}
        
        protected void FillDepartment()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.DepartmentService.iDepartmentMasterClient DeptService = new BrilliantWMS.DepartmentService.iDepartmentMasterClient();
               // ddlDeartment.DataSource = DeptService.GetDeparmentList(profile.DBConnection._constr);
                ddlDeartment.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlDeartment.Items.Insert(0, lst);
                DeptService.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDepartment");

            }
            finally
            {
            }
        }

        protected void FillDesignation()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //DesignationService.connectiondetails AppConnectionDetailsDeSig = new DesignationService.connectiondetails();
                ddlDesignation.DataSource = null;
                ddlDesignation.DataBind();
                if (ddlDeartment.SelectedIndex > 0)
                {
                    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                    BrilliantWMS.DesignationService.iDesignationMasterClient DesignationService = new BrilliantWMS.DesignationService.iDesignationMasterClient();
                    ddlDesignation.DataSource = DesignationService.GetDesignationListByDepartmentID(Convert.ToInt32(ddlDeartment.SelectedItem.Value), profile.DBConnection._constr);
                    ddlDesignation.DataBind();
                    ddlDesignation.Items.Insert(0, lst);
                    DesignationService.Close();

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDesignation");

            }
            finally
            {
            }
        }

        protected void BindRoleDetailsGridView(long RoleID)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //List<SP_GetDataToBindRoleMaster_Result> sessionList = new List<SP_GetDataToBindRoleMaster_Result>();Comment by vishal
                List<SP_GWCGetDataToBindRoleMaster_Result> sessionList = new List<SP_GWCGetDataToBindRoleMaster_Result>();
                BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
               // sessionList = roleMasterService.GetDataToBindRoleMasterDetailsByRoleID(RoleID, profile.Personal.CompanyID, profile.DBConnection._constr).ToList();Comment by vishal
                sessionList = roleMasterService.GetGWCDataToBindRoleMasterDetailsByRoleID(RoleID, profile.Personal.CompanyID, profile.DBConnection._constr).ToList();// Added by vishal
                //GridRoleConfiguration.GroupBy = "DisplayModuleName,DisplayPhaseName";Comment by vishal
                GridRoleConfiguration.ShowHeader = true;
                GridRoleConfiguration.DataSource = sessionList;
                GridRoleConfiguration.DataBind();
                Session.Add("sessionRoleList", sessionList);
                roleMasterService.Close();
                //TabContainerRoleMaster.ActiveTabIndex = 1;
                //TabPanelRoleDetails.Visible = true;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "BindRoleDetailsGridView");

            }
            finally
            {
            }
        }

        protected void BindRoleMasterGridView()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
               // GridRoleMaster.DataSource = roleMasterService.BindRoleMasterSummary(profile.DBConnection._constr); Comment by vishal
                GridRoleMaster.DataSource = roleMasterService.BindGWCRoleMasterSummary(profile.DBConnection._constr);
                
                GridRoleMaster.DataBind();
                roleMasterService.Close();
               // TabContainerRoleMaster.ActiveTabIndex = 0;
                
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "BindRoleMasterGridView");

            }
            finally
            {
            }
        }

        [WebMethod]
        public static void UpdateRole(object role, object rowIndex)
        {
            BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)role;
                //connectiondetails AppConnectionDetails = new connectiondetails();
                //SP_GetDataToBindRoleMaster_Result updateRole = new SP_GetDataToBindRoleMaster_Result(); comment by vishal
                SP_GWCGetDataToBindRoleMaster_Result updateRole = new SP_GWCGetDataToBindRoleMaster_Result();
                updateRole.mSequence = Convert.ToInt64(dictionary["mSequence"]);
                updateRole.pSequence = Convert.ToInt64(dictionary["pSequence"]);
                updateRole.oSequence = Convert.ToInt64(dictionary["oSequence"]);

                updateRole.Add = Convert.ToBoolean(dictionary["Add"]);

                //if (updateRole.Add == true) { updateRole.Edit = true; updateRole.View = true; }
                //else { updateRole.Edit = false; }

                //Add By Suresh
                if (updateRole.Add == true)
                {
                    updateRole.Edit = true;
                }
                else
                {
                    updateRole.Edit = false;
                }

                if (updateRole.View == true)
                {
                    updateRole.View = true;
                }
                else
                {
                    updateRole.View = false;
                }

                //updateRole.View = true;
                updateRole.Delete = false;
                updateRole.Approval = Convert.ToBoolean(dictionary["Approval"]);

                if (updateRole.Approval == true)
                {
                    updateRole.View = true;
                }
                //updateRole.AssignTask = Convert.ToBoolean(dictionary["AssignTask"]);comment by vishal

                //if (updateRole.AssignTask == true) comment bi vishal
                //{ 
                //    updateRole.View = true; 
                //}
                //if (updateRole.Add == true && updateRole.Approval == false && updateRole.AssignTask == false && Convert.ToBoolean(dictionary["View"]) == true) comment by vishal
                //{ 
                //    updateRole.View = true; 
                //}  

                //HttpContext.Current.Session["sessionRoleList"] = roleMasterService.UpdateRoleIntoSessionList(getSessionList().ToArray(), updateRole, Convert.ToInt32(rowIndex), profile.DBConnection._constr).ToList();Comment by vishal
                HttpContext.Current.Session["sessionRoleList"] = roleMasterService.GWCUpdateRoleIntoSessionList(getSessionList1().ToArray(), updateRole, Convert.ToInt32(rowIndex), profile.DBConnection._constr).ToList();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Role Master", "UpdateRole");
            }
            finally
            {
                roleMasterService.Close();
            }
        }
        
        protected void pageAdd(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
          

        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
             BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            try
            {  if (checkDuplicate() == "")
                {
                 
                CustomProfile profile = CustomProfile.GetProfile();
               
                mRole obj = new mRole();
                if (hdnRoleID.Value != "0" && hdnRoleID.Value != string.Empty) // for edit
                {
                    obj = roleMasterService.GetmRoleByID(Convert.ToInt32(hdnRoleID.Value), profile.DBConnection._constr);
                    obj.LastModifiedBy = profile.Personal.UserID.ToString();
                    obj.LastModifiedDate = DateTime.Now;
                    obj.TerritoryID = Convert.ToInt64(ddlDeartment.SelectedValue);

                }
                else  //add new role
                {
                    obj.CreatedBy = profile.Personal.UserID.ToString();
                    obj.CreationDate = DateTime.Now;
                    obj.TerritoryID = Convert.ToInt64(hdnSelectedDepartment.Value);
                }
                obj.RoleName = txtRoleName.Text.Trim();
               
                obj.DepartmentID = 1;
                obj.CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                obj.CommonValueID = Convert.ToInt64(ddlUserType.SelectedValue);
             
                obj.DesignationID = 1;
                obj.Sequence = 0;
                //if (txtSequence.Text != string.Empty) obj.Sequence = Convert.ToInt32(txtSequence.Text);
                obj.Active = "Y";
                if (rbtnNo.Checked == true) obj.Active = "N";
                //obj.CompanyID = profile.Personal.CompanyID;
               // roleMasterService.FinalSave(getSessionList().ToArray(), obj, profile.DBConnection._constr);//Comment by vishal
                roleMasterService.FinalSaveRole(getSessionList1().ToArray(), obj, profile.DBConnection._constr);
                roleMasterService.Close();
                TabContainerRoleMaster.ActiveTabIndex = 0;
                TabPanelRoleDetails.Visible = false;
                BindRoleMasterGridView();
                clear();
                setActiveTab(0);
                WebMsgBox.MsgBox.Show("Record saved successfully");
            }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "pageSave");

            }
            finally
            {
                
            }

        }

        protected void clear()
        {
            hdnRoleID.Value = "0";
            txtRoleName.Text = "";
           // txtSequence.Text = "";
            ddlDeartment.SelectedIndex = -1;
            ddlDesignation.SelectedIndex = -1;
            ddlcompany.SelectedIndex = -1;
            ddlUserType.SelectedIndex = -1;
            rbtnNo.Checked = false;
            rbtnYes.Checked = true;
            GridRoleConfiguration.DataSource = null;
            GridRoleConfiguration.DataBind();
            Session["sessionRoleList"] = null;
        }

        protected static List<SP_GetDataToBindRoleMaster_Result> getSessionList()
        {

            List<SP_GetDataToBindRoleMaster_Result> sessionList = new List<SP_GetDataToBindRoleMaster_Result>();
            if (HttpContext.Current.Session["sessionRoleList"] != null) sessionList = (List<SP_GetDataToBindRoleMaster_Result>)HttpContext.Current.Session["sessionRoleList"];
            return sessionList;
        }

        protected static List<SP_GWCGetDataToBindRoleMaster_Result> getSessionList1()
        {

            List<SP_GWCGetDataToBindRoleMaster_Result> sessionList = new List<SP_GWCGetDataToBindRoleMaster_Result>();
            if (HttpContext.Current.Session["sessionRoleList"] != null) sessionList = (List<SP_GWCGetDataToBindRoleMaster_Result>)HttpContext.Current.Session["sessionRoleList"];
            return sessionList;
        }



        protected void ddlDeartment_SelectedIndexChanged(object sender, EventArgs e)
        { FillDesignation(); }

        //protected void GridRoleMaster_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        //{

        //    try
        //    {
        //        clear();

        //        Hashtable selectedrec = (Hashtable)GridRoleMaster.SelectedRecords[0];

        //        hdnRoleID.Value = selectedrec["mrID"].ToString();
        //        txtRoleName.Text = selectedrec["RoleName"].ToString();
        //       // txtSequence.Text = selectedrec["mrSequence"].ToString();
        //        if (selectedrec["Active"].ToString() == "Yes") { rbtnYes.Checked = true; rbtnNo.Checked = false; }
        //        else { rbtnNo.Checked = true; rbtnYes.Checked = false; }

        //        if (ddlDeartment.Items.Count <= 1) FillDepartment();
        //        ddlDeartment.SelectedIndex = ddlDeartment.Items.IndexOf(ddlDeartment.Items.FindByValue(selectedrec["DepartmentID"].ToString()));
        //        FillDesignation();
        //        ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByValue(selectedrec["DesignationID"].ToString()));
        //        BindRoleDetailsGridView(Convert.ToInt32(hdnRoleID.Value));
        //        //TabContainerRoleMaster.ActiveTabIndex = 1;
        //        setActiveTab(1);
        //        this.UCToolbar1.ToolbarAccess("RoleMaster", "btnEdit");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Login.Profile.ErrorHandling(ex, this, "Role Master", "GridRoleMaster_Select");

        //    }
        //    finally
        //    {
        //    }
        //}

        protected void GridRoleMaster_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                clear();
                getrolebyID();
                setActiveTab(1);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "GridRoleMaster_Select");
            }
            finally 
            {
              
            }
        }

        private void getrolebyID()
        {
            try
            {
                Hashtable selectedrec = (Hashtable)GridRoleMaster.SelectedRecords[0];
                hdnRoleID.Value = selectedrec["ID"].ToString();
                txtRoleName.Text = selectedrec["RoleName"].ToString();

                if (selectedrec["Active"].ToString() == "Yes") 
                { 
                    rbtnYes.Checked = true; rbtnNo.Checked = false; 
                }
                else 
                { 
                    rbtnNo.Checked = true; 
                    rbtnYes.Checked = false; 
                }

                //if (ddlDeartment.Items.Count <= 1) 
                //    FillDepartment();

                //    ddlDeartment.SelectedIndex = ddlDeartment.Items.IndexOf(ddlDeartment.Items.FindByValue(selectedrec["DepartmentID"].ToString()));
                //    FillDesignation();
                //   ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByValue(selectedrec["DesignationID"].ToString()));

              
                   FillCompany();
                   ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(selectedrec["CompanyID"].ToString()));

                   long CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                    
                   FillUserType();
                   ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlUserType.Items.FindByValue(selectedrec["UserID"].ToString()));

                   FillDept(CompanyID);
                   //ddlDeartment.SelectedIndex = ddlDeartment.Items.IndexOf(ddlUserType.Items.FindByValue(selectedrec["DepartmentID"].ToString()));
                   ddlDeartment.SelectedValue = selectedrec["DepartmentID"].ToString();

                if (ddlDeartment.Items.Count <= 1)
                {
                   
                    ddlUserType.SelectedIndex = ddlUserType.Items.IndexOf(ddlcompany.Items.FindByValue(selectedrec["UserID"].ToString()));
                }

                
                  BindRoleDetailsGridView(Convert.ToInt32(hdnRoleID.Value));

                hndstate.Value = "Edit";
               // setActiveTab(1);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "GridRoleMaster_Select");
            }
            finally { }
        }


        public void FillDept(long CompanyID)
        {
             List<mTerritory> TerritoryList = new List<mTerritory>();
                try
                {
                     CustomProfile profile = CustomProfile.GetProfile();
                    UC_Territory uc_territory = new UC_Territory();
                    TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
                    ddlDeartment.DataSource = TerritoryList;
                    ddlDeartment.DataBind();
                    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                    ddlDeartment.Items.Insert(0, lst);

                }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDepartment");

            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                clear();
                BindRoleDetailsGridView(0);
                setActiveTab(1);
                //TabPanelRoleDetails.Enabled = true;
                //TabContainerRoleMaster.ActiveTabIndex = 1;              
                //GridRoleConfiguration.GroupBy = "DisplayModuleName,DisplayPhaseName"; comment by vishal
                //GridRoleConfiguration.Width =100;
                //GridRoleConfiguration.ShowHeader = true; comment by vishal
               // GridRoleConfiguration_ColumnsCreated(sender, e);
                    
           }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "pageAddNew");
            }
            finally
            {
            }
        }

        public string checkDuplicate()
        {
            BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            //PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();
            try
            {                 
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                if (hdnRoleID.Value == string.Empty)
                {
                    result = roleMasterService.checkDuplicateRecord(txtRoleName.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtRoleName.Text = "";
                    }
                    txtRoleName.Focus();
                }
                else
                {
                    int id = Convert.ToInt32(hdnRoleID.Value);
                    result = roleMasterService.checkDuplicateRecordEdit(id, txtRoleName.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtRoleName.Text = "";
                    }
                }
                return result;

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
                roleMasterService.Close();
            }


        }

        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
             
            if (ActiveTab == 0)
            {
                TabPanelRoleDetails.Visible = false;
                TabContainerRoleMaster.ActiveTabIndex = 0;
                 //btnSave.Enabled = false;
                //btnSave.Visible = false;
            }
            else
            {              
                 TabPanelRoleDetails.Visible = true;
                 TabContainerRoleMaster.ActiveTabIndex = 1; 
                 //btnSave.Enabled = true;
                // btnSave.Visible = true;
                
            }
        }

        protected void GridRoleConfiguration_ColumnsCreated(object sender, EventArgs e)
        {
            int width = 700;
            int count = GridRoleConfiguration.Columns.Count;
            int average = width / count;
            int i = 0;

            foreach (Column column in GridRoleConfiguration.Columns)
            {
                //if (i < count - 1)
                //{
                //    column.Width = average.ToString() + "%";
                //}
                //else
                //{
                //    column.Width = width.ToString() + "%";
                //}
                
                //width -= average;
                column.Width = 140 + "px";
                i++;
            }
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;
                TabPanelRoleMaster.HeaderText = rm.GetString("RoleList", ci);
                lblrolllist.Text = rm.GetString("RoleList", ci);
                TabPanelRoleDetails.HeaderText = rm.GetString("RoleConfiguration", ci);
                lbldepartment.Text = rm.GetString("Department", ci);
                //lbldesignation.Text = rm.GetString("Designation", ci);
                lblactive.Text = rm.GetString("Active", ci);
                rbtnYes.Text = rm.GetString("Yes", ci);
                rbtnNo.Text = rm.GetString("No", ci);
                lblrollname.Text = rm.GetString("RollName", ci);
                lblrollconfig.Text = rm.GetString("RoleConfiguration", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("RollMaster", ci);
                lblcompany.Text = rm.GetString("Customer", ci);
               // lblUserType.Text = rm.GetString("UserType", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "loadstring");
            }
            finally
            {
 
            }

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

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            ddlcompany.SelectedIndex = -1;
            ddlDeartment.SelectedIndex = -1;
            ddlUserType.SelectedIndex = -1;
            txtRoleName.Text = "";
            BindRoleDetailsGridView(0);
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
            catch(System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Role Master", "GetDepartment");
            }
            finally
            {
                productClient.Close();
            }
            return LocList;
        }

        //public class contact
        //{
        //    private string _name;
        //    public string Name
        //    {
        //        get { return _name; }
        //        set { _name = value; }
        //    }

        //    private string _id;
        //    public string Id
        //    {
        //        get { return _id; }
        //        set { _id = value; }
        //    }
        //}

        protected void FillCompany()
        {
            ddlcompanymain.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompanymain.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlcompanymain.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompanymain.Items.Insert(0, lst);
            CompanyClient.Close();
        }

        public void getCustomer(long CompanyID)
        {
            ddlcompany.Items.Clear();
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = StatutoryClient.GetCustomerList(CompanyID, profile.DBConnection._constr);
            ddlcompany.DataTextField = "Name";
            ddlcompany.DataValueField = "ID";
            ddlcompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompany.Items.Insert(0, lst);
            StatutoryClient.Close();
        }

        [WebMethod]
        public static List<contact> GetCustomerByComp(object objReq)
        {
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());
                ds = StatutoryClient.GetCustomerList(ddlcompanyId, profile.DBConnection._constr);
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
            catch
            {
            }
            finally
            {
                StatutoryClient.Close();
            }
            return LocList;
        }

        [WebMethod]
        public static List<contact> GetDepartmentByCustID(object objReq)
        {
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            iDepartmentMasterClient Department = new iDepartmentMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long ddlcustomerID = long.Parse(dictionary["ddlcustomerId"].ToString());
                //ds = StatutoryClient.GetCustomerList(ddlcompanyId, profile.DBConnection._constr);
                ds = Department.GetDeptByCustomerID(ddlcustomerID, profile.DBConnection._constr);
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
            catch
            {
            }
            finally
            {
                StatutoryClient.Close();
            }
            return LocList;
        }



        [WebMethod]
        public static List<contact> GetDesignationByDeptID(object objReq)
        {
            iDesignationMasterClient designation = new iDesignationMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long ddldepartment = long.Parse(dictionary["ddldepartment"].ToString());
                //ds = StatutoryClient.GetCustomerList(ddlcompanyId, profile.DBConnection._constr);
                ds = designation.GetDesignationByDeptID(ddldepartment, profile.DBConnection._constr);
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
            catch
            {
            }
            finally
            {
                designation.Close();
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

    }
}
