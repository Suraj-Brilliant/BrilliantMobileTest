using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.ApprovalLevelMasterService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;
using System.Web.Services;
using BrilliantWMS.ServiceTerritory;
using BrilliantWMS.Territory;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.UserCreationService;
using System.Data;
using System.Data.SqlClient;
using BrilliantWMS.ProductMasterService;

namespace BrilliantWMS.Approval
{
    public partial class ApprovalMaster : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        BrilliantWMS.ApprovalLevelMasterService.iApprovalLevelMasterClient ApprovalClient = new BrilliantWMS.ApprovalLevelMasterService.iApprovalLevelMasterClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            //loadstring();
            //UCFormHeader1.FormHeaderText = "Approval Master";
            if (!IsPostBack)
            {
                GetApprovalgridList();
                BindCustomer();
                // BindApprovalGrid();
                BindUserGrid();
                BinddlObjectNameDDL();
                hdnApprovalID.Value = null;
                hdnUserIDs.Value = null;
                setActiveTab(0);
                Session["Userlist"] = null;
                Session.Add("InsertData", "");
            }

            this.UCToolbar1.ToolbarAccess("ApprovalLevelMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
            if (btnSave != null)
                //  btnSave.Enabled = false;

                if (ActiveTab == 0)
                {
                    tabapprovallist.Visible = true;
                    tabapprolevel.Visible = false;
                    panApprovalDetail.Visible = false;
                    tabApprovalLevelMaster.ActiveTabIndex = 0;
                    // btnSave.Enabled = false;//added by vishal
                }
                else
                {
                    tabapprovallist.Visible = true;
                    tabapprolevel.Visible = true;
                    panApprovalDetail.Visible = true;
                    tabApprovalLevelMaster.ActiveTabIndex = 1;
                    //if (btnSave != null) btnSave.Enabled = true;
                }
        }

        protected void BindApprovalGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //gvApprovalLevelM.DataSource = ApprovalClient.GetApprovalRecordToBindGrid(profile.DBConnection._constr);
                gvApprovalLevelM.DataSource = ApprovalClient.GetGWCApprovalRecordToBindGrid(profile.DBConnection._constr);

                gvApprovalLevelM.DataBind();

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "BindApprovalGrid");
            }
            finally
            {
            }
        }
        public void BindCustomer()
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
        protected void BindUserGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //gvUserCreation.DataSource = ApprovalClient.GetUserListForEditbySP(0, profile.DBConnection._constr);
                //gvUserCreation.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "BindUserGrid");
            }
            finally
            {
            }
        }

        protected void BinddlObjectNameDDL()
        {
            try
            {
                //CustomProfile profile = CustomProfile.GetProfile();
                //ddlObjectName.DataSource = ApprovalClient.GetObjectList(profile.DBConnection._constr);
                //ddlObjectName.DataBind();
                //ListItem lst = new ListItem();
                //lst.Text = "-Select-";
                //lst.Value = "0";
                //ddlObjectName.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "BinddlObjectNameDDL");
            }
            finally
            {
            }
        }

        [WebMethod]
        public static string PMGetApprovalLevelByObjectName(string objectName, long territoryID)
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            string NewLevel = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                NewLevel = ApprovalClient.GetApprovalLevelMax(objectName, territoryID, profile.DBConnection._constr).ToString();
            }
            catch (System.Exception ex)
            { Login.Profile.ErrorHandling(ex, "Approval Level Master", "PMGetApprovalLevelByObjectName"); }
            finally
            { ApprovalClient.Close(); }
            return NewLevel;
        }

        protected void clear()
        {
            ddlcompany.SelectedIndex = -1;
           // ddldepartment.SelectedIndex = -1;
            ddlcurrntlevel.SelectedIndex = -1;
            txtnoapprovar.Text = "";
            hdnApprovalID.Value = null;
            //lblApprovalLevel.Text = "";
            hdnStatus.Value = "";
            setActiveTab(1);
            BindUserGrid();
            DeleteZeroDetailaproval();
            gvUserCreation.DataSource = null;
            gvUserCreation.DataBind();

        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
            hdnStatus.Value = "AddNew";
            gvUserCreation.DataSource = null;
            gvUserCreation.DataBind();
            ddlcompany.Enabled = true;
           // ddldepartment.Enabled = true;

            //tabApprovalLevelMaster.ActiveTabIndex = 1;
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                if (hdnSelectedCompany.Value != "" || hdnSelectedDepartment.Value != "" || hdnSelectedLevel.Value != "" || txtnoapprovar.Text != "")
                {

                    //CustomProfile profile = CustomProfile.GetProfile();
                    //mApprovalLevel ObjApproval = new mApprovalLevel();
                    //ObjApproval.ObjectName = "MaterialRequest";
                    //ObjApproval.ApprovalLevel = Convert.ToByte(hdnSelectedLevel.Value);
                    //ObjApproval.NoOfApprovers = Convert.ToByte(txtnoapprovar.Text);
                    //ObjApproval.Active = "Y";
                    //ObjApproval.TerritoryID = Convert.ToInt64(hdnSelectedDepartment.Value);
                    //ObjApproval.MinApprovalReq = 0;
                    //ObjApproval.MinApprovalReq = 0;
                    //ObjApproval.MaxAmount = 0;
                    //ObjApproval.IsLowerLevelApprovalReq = true;
                    //if (hdnSelectedDepartment.Value != "" || hdnSelectedDepartment.Value != "0")
                    //{
                    //    ObjApproval.OrderCancelInDays = ApprovalClient.GetCancelDays(Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr);
                    //}

                    //mApprovalLevelDetail ObjApprovalDetail = new mApprovalLevelDetail();
                    //ObjApprovalDetail.Active = "Y";
                    //ObjApprovalDetail.CreatedBy = profile.Personal.UserID.ToString();
                    //ObjApprovalDetail.CreationDate = DateTime.Now;
                    //ObjApprovalDetail.CompanyID = Convert.ToInt64(hdnSelectedCompany.Value);
                    //ObjApprovalDetail.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
                    //if (chkand.Checked == true)
                    //{ ObjApprovalDetail.ApproverLogic = "AND"; }
                    //else
                    //{ ObjApprovalDetail.ApproverLogic = "OR"; }

                    //if (hdnStatus.Value != "Edit")
                    //{
                    //    ObjApproval.CreatedBy = profile.Personal.UserID.ToString();
                    //    ObjApproval.CreationDate = DateTime.Now;
                    //    int result = ApprovalClient.InsertmApprovalLevel(ObjApproval, profile.DBConnection._constr);
                    //    ObjApprovalDetail.ApprovalLevelID = result;
                    //    ApprovalClient.UpdateApprovallevelID(result, profile.DBConnection._constr);
                    //    if (result != 0)
                    //    {
                    //        WebMsgBox.MsgBox.Show("Record saved successfully");
                    //    }
                    //}
                    //if (hdnStatus.Value == "Edit")
                    //{
                    //    ObjApproval.LastModifiedBy = profile.Personal.UserID.ToString();
                    //    ObjApproval.LastModifiedDate = DateTime.Now;
                    //    // int result = ApprovalClient.updatemApprovalLevel(ObjApproval, profile.DBConnection._constr);
                    //    ObjApprovalDetail.ApprovalLevelID = Convert.ToInt32(hdnApprovalID.Value);
                    //    long headerId = long.Parse(hdnApprovalID.Value);
                    //    long canceldays = ApprovalClient.GetCancelDays(Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr);
                    //    ApprovalClient.UpdateMApproveHeader(headerId, Convert.ToByte(hdnSelectedLevel.Value), Convert.ToByte(txtnoapprovar.Text), profile.Personal.UserID.ToString(), DateTime.Now, Convert.ToInt64(hdnSelectedDepartment.Value), canceldays, profile.DBConnection._constr);
                    //    ApprovalClient.UpdateApprovallevelID(headerId, profile.DBConnection._constr);
                    //    if (hdnApprovalUserID.Value != "")
                    //    {
                    //        // ApprovalClient.SaveApprovalLevelDetail(hdnApprovalUserID.Value, Convert.ToInt32(hdnApprovalID.Value), ObjApprovalDetail);
                    //        ////ApprovalClient.SaveApprovalLevelDetail(UserId, Convert.ToInt32(hdnApprovalID.Value), ObjApprovalDetail, profile.DBConnection._constr);
                    //    }

                    //    if (hdnApprovalUserID.Value == "")
                    //    {
                    //        // ApprovalClient.SaveApprovalLevelDetail("0", Convert.ToInt32(hdnApprovalID.Value), ObjApprovalDetail, profile.DBConnection._constr);
                    //    }
                    //    //if (result == 1)
                    //    //{
                    //    WebMsgBox.MsgBox.Show("Record updated successfully");
                    //    //}
                    //}
                    clear();
                    GetApprovalgridList();
                    // BindApprovalGrid();
                    hdnStatus.Value = "";
                    hdnApprovalUserID.Value = "";
                    setActiveTab(0);
                    gvUserCreation.DataSource = null;
                    gvUserCreation.DataBind();
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "pageSave");
            }
            finally
            {
                ApprovalClient.Close();
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }


        protected void gvApprovalLevelM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ddlcompany.Enabled = false;
               // ddldepartment.Enabled = false;
                Hashtable selectedrec = (Hashtable)gvApprovalLevelM.SelectedRecords[0];
                hdnApprovalID.Value = selectedrec["ID"].ToString();
                BindCustomer();
                ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(selectedrec["CompanyID"].ToString()));
                long CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                hdnSelectedCompany.Value = CompanyID.ToString();
                FillDept(CompanyID);
                //ddldepartment.SelectedValue = selectedrec["DepartmentID"].ToString();
                //long DepartmentID = Convert.ToInt64(ddldepartment.SelectedValue);
               // hdnSelectedDepartment.Value = DepartmentID.ToString();
               // GetGWCApprovalLevel(CompanyID, DepartmentID);

                for (int i = 0; i <= Convert.ToInt32(txtnoapprovar.Text); i++)
                {
                    string j = i.ToString();
                    if (j == "0")
                    {
                        ddlcurrntlevel.Items.Add(new ListItem("--Select--", j));
                    }
                    else
                    {
                        ddlcurrntlevel.Items.Add(new ListItem(j, j));
                    }
                }
                ddlcurrntlevel.SelectedIndex = ddlcurrntlevel.Items.IndexOf(ddlcurrntlevel.Items.FindByValue(selectedrec["ApprovalLevel"].ToString()));

                hdnSelectedLevel.Value = selectedrec["ApprovalLevel"].ToString();

                txtnoapprovar.Text = selectedrec["NoOfApprovers"].ToString();

                //if (selectedrec["ApproverLogic"].ToString() == "AND")
                //{
                //    chkand.Checked = true;
                //}
                //else
                //{
                //    chkor.Checked = true;
                //}

                hdnStatus.Value = "Edit";
                gvUserCreation_OnRebind(sender, e);
                setActiveTab(1);
                //List<SP_GetUserForApprovalMaster_Result> getlst = new List<SP_GetUserForApprovalMaster_Result>();
                //getlst = ApprovalClient.GetUserListForEditbySP(Convert.ToInt32(hdnApprovalID.Value), profile.DBConnection._constr).ToList();
                //gvUserCreation.DataSource = getlst;
                //gvUserCreation.DataBind();

                //foreach (SP_GetUserForApprovalMaster_Result item in getlst)
                //{
                //    hdnApprovalUserID.Value = item.SelectedRec;
                //    break;
                //}
                //tabApprovalLevelMaster.ActiveTabIndex = 1;

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "gvApprovalLevelM_Select");
            }
            finally
            {
            }
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
        #endregion

        private void loadstring()
        {
            try
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                //rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                //ci = Thread.CurrentThread.CurrentCulture;
                //lblheader.Text = rm.GetString("ApprovalLevels", ci);
                //lblheadtxt.Text = rm.GetString("ApprovalLevelList", ci);
                //lbltabhead.Text = rm.GetString("ApprovalLevelDetail", ci);
                //lblcustomer.Text = rm.GetString("Customer", ci);
                //lbldept.Text = rm.GetString("Department", ci);
                //lblnolevel.Text = rm.GetString("noapprovallevel", ci);
                //lblcurrentlevel.Text = rm.GetString("CurrentApprovalLevel", ci);
                //lblapprovarno.Text = rm.GetString("NoOfApprovar", ci);
                //lblapprovallogic.Text = rm.GetString("ApprovalLogic", ci);
                //chkand.Text = rm.GetString("And", ci);
                //chkor.Text = rm.GetString("Or", ci);
                //btnaddi.Value = rm.GetString("AddUser", ci);
                //lblselectapprovar.Text = rm.GetString("SelectApprovers", ci);
                //UCFormHeader1.FormHeaderText = rm.GetString("ApprovalMaster", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Approval Level Master", "loadstring");
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


        [WebMethod]
        public static string PMApprovalLevel(long CompanyID, long DeptID)
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            string AppLevel = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                AppLevel = ApprovalClient.GetApprovalLevel(CompanyID, DeptID, profile.DBConnection._constr).ToString();
            }
            catch (System.Exception ex)
            { Login.Profile.ErrorHandling(ex, "Approval Level Master", "PMGetApprovalLevelByObjectName"); }
            finally
            { ApprovalClient.Close(); }

            return AppLevel;


        }

        protected void gvUserCreation_OnRebind(object sender, EventArgs e)
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            try
            {

                string insertgata = Session["InsertData"].ToString();
                if (hdnStatus.Value == "Edit" && Session["InsertData"].ToString() == "")
                {
                    //List<vGWCGetApprovalDetailsForEdit> getlst = new List<vGWCGetApprovalDetailsForEdit>();
                    //DataSet ds = new DataSet();
                    gvUserCreation.DataSource = ApprovalClient.GWCGetApprovalDetailsForEdit(Convert.ToInt64(hdnApprovalID.Value), profile.DBConnection._constr);
                    //getlst = ApprovalClient.GWCGetApprovalDetailsForEdit(Convert.ToInt64(hdnApprovalID.Value), profile.DBConnection._constr).ToList();
                    //gvUserCreation.DataSource = getlst;
                    gvUserCreation.DataBind();

                    // getlst = ApprovalClient.GWCGetUserForApprovalMaster(Convert.ToInt64(hdnSelectedLevel.Value), stringss[i], profile.DBConnection._constr).ToList();


                    GetApproverList();
                }
                else if (hdnStatus.Value == "AddNew" && Hdn1.Value == "1" && Session["InsertData"].ToString() == "")
                {

                    GetApproverList();
                }
                else if (Session["InsertData"].ToString() == "Insert")
                {
                    /*Add By Suresh*/
                    int AvailableApprovalLevel = ApprovalClient.GetApprovalRecordByApvrlLevelID(Convert.ToInt16(hdnSelectedLevel.Value), Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr);
                    if (AvailableApprovalLevel > 0)
                    {
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('Approval For This Level Are Already Available','Error','#')", true);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Approval For This Level Are Already Available')", true);
                    }
                    else
                    {
                        mApprovalLevel ObjApproval = new mApprovalLevel();
                        ObjApproval.ObjectName = "MaterialRequest";
                        ObjApproval.ApprovalLevel = Convert.ToByte(hdnSelectedLevel.Value);
                        ObjApproval.NoOfApprovers = Convert.ToByte(txtnoapprovar.Text);
                        ObjApproval.Active = "Y";
                        ObjApproval.TerritoryID = Convert.ToInt64(hdnSelectedDepartment.Value);
                        ObjApproval.MinApprovalReq = 0;
                        ObjApproval.MinApprovalReq = 0;
                        ObjApproval.MaxAmount = 0;
                        ObjApproval.IsLowerLevelApprovalReq = true;
                        if (hdnSelectedDepartment.Value != "" || hdnSelectedDepartment.Value != "0")
                        {
                            ObjApproval.OrderCancelInDays = ApprovalClient.GetCancelDays(Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr);
                        }

                        int result = 0;

                        //if (hdnStatus.Value != "Edit")
                        //{
                        ObjApproval.CreatedBy = profile.Personal.UserID.ToString();
                        ObjApproval.CreationDate = DateTime.Now;
                        result = ApprovalClient.InsertmApprovalLevel(ObjApproval, profile.DBConnection._constr);
                        // ObjApprovalDetail.ApprovalLevelID = result;
                        // ApprovalClient.UpdateApprovallevelID(result, profile.DBConnection._constr);
                        if (result != 0)
                        {
                            // WebMsgBox.MsgBox.Show("Record saved successfully");
                        }
                        //}
                        //else if (hdnStatus.Value == "Edit")
                        //{
                        //    ObjApproval.LastModifiedBy = profile.Personal.UserID.ToString();
                        //    ObjApproval.LastModifiedDate = DateTime.Now;
                        //    result= Convert.ToInt32(hdnApprovalID.Value);
                        //}
                        /*Add By Suresh*/

                        mApprovalLevelDetail ObjApprovalDetail = new mApprovalLevelDetail();
                        string[] stringss = new string[] { };
                        hdnUserIDs.Value = Session["Userlist"].ToString();
                        stringss = hdnUserIDs.Value.Split(',');

                        ObjApprovalDetail.ApprovalLevelID = result;  //ObjApprovalDetail.ApprovalLevelID = 0;
                        ObjApprovalDetail.Active = "Y";
                        ObjApprovalDetail.CreatedBy = profile.Personal.UserID.ToString();
                        ObjApprovalDetail.CreationDate = DateTime.Now;
                        ObjApprovalDetail.CompanyID = Convert.ToInt64(hdnSelectedCompany.Value);
                        ObjApprovalDetail.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
                        if (chkand.Checked == true)
                        { ObjApprovalDetail.ApproverLogic = "AND"; }
                        else
                        { ObjApprovalDetail.ApproverLogic = "OR"; }

                        for (int i = 0; i < stringss.Length; i++)
                        {
                            ObjApprovalDetail.UserID = Convert.ToInt64(stringss[i].ToString());
                            ApprovalClient.InsertGWCApprovalDetails(ObjApprovalDetail, profile.DBConnection._constr);
                        }
                    }
                    GetApproverList();
                    Session["InsertData"] = "";
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Approval Level Master", "gvUserCreation_OnRebind");
            }
            finally
            {
                ApprovalClient.Close();
            }
        }
        //gvUserCreation.DataSource = null;
        //List<SP_GWCGetUserForApprovalMaster_Result> getlst = new List<SP_GWCGetUserForApprovalMaster_Result>();
        //if (hdnUserIDs.Value != "" || hdnUserIDs.Value != "0")
        //{
        //    getlst = ApprovalClient.GWCGetUserForApprovalMaster(Convert.ToInt64(hdnSelectedLevel.Value), hdnUserIDs.Value, profile.DBConnection._constr).ToList();
        //    gvUserCreation.DataSource = getlst;
        //    gvUserCreation.DataBind();
        //}

        public void GetApproverList()
        {

            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvUserCreation.DataSource = null;
                gvUserCreation.DataBind();
                DataSet dsApproverList = new DataSet();
                List<SP_GWCGetApproverListBySp_Result> getlst = new List<SP_GWCGetApproverListBySp_Result>();
                //if (hdnStatus.Value == "AddNew")
                //{
                // getlst = ApprovalClient.GetApproverListBySp(Convert.ToInt64(hdnSelectedLevel.Value), profile.DBConnection._constr).ToList();
                getlst = ApprovalClient.GetApproverListBySp(Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr).ToList();
                dsApproverList = ApprovalClient.GetApproverList(profile.DBConnection._constr);
                // gvUserCreation.DataSource = dsApproverList;
                gvUserCreation.DataSource = getlst;
                gvUserCreation.DataBind();
                //}
                //else
                //{
                //    gvUserCreation.DataSource = ApprovalClient.GWCGetApprovalDetailForEditWithZero(Convert.ToInt64(hdnApprovalID.Value), profile.DBConnection._constr);
                //    gvUserCreation.DataBind();
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Approval Level Master", "GetApproverList");
            }
            finally
            {
                ApprovalClient.Close();
            }


        }


        public void FillDept(long CompanyID)
        {
            //List<mTerritory> TerritoryList = new List<mTerritory>();
            //try
            //{
            //    UC_Territory uc_territory = new UC_Territory();
            //    TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
            //    ddldepartment.DataSource = TerritoryList;
            //    ddldepartment.DataBind();
            //    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            //    ddldepartment.Items.Insert(0, lst);

            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDept");

            //}
            //finally
            //{
            //}
        }

        public void GetGWCApprovalLevel(long CompanyID, long DeptID)
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            string AppLevel = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                AppLevel = ApprovalClient.GetApprovalLevel(CompanyID, DeptID, profile.DBConnection._constr).ToString();
               // lblApprovalLevel.Text = AppLevel;
            }
            catch (System.Exception ex)
            { Login.Profile.ErrorHandling(ex, "Approval Level Master", "GetGWCApprovalLevel"); }
            finally
            { ApprovalClient.Close(); }

            // return AppLevel;


        }
        [WebMethod]
        public static void WMRemoveUserFromUserList(string ID)
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                ApprovalClient.DeleteApproverFromGrid(Convert.ToInt64(ID), profile.DBConnection._constr);

            }
            catch { }
            finally { }
        }

        public void DeleteZeroDetailaproval()
        {
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ApprovalClient.DeleteZeroDetailaproval(profile.DBConnection._constr);
        }

        public void GetApprovalgridList()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            iApprovalLevelMasterClient ApprovalClient = new iApprovalLevelMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ds = ApprovalClient.GetApprovalMasterGridList(profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                gvApprovalLevelM.DataSource = ds.Tables[0];
                gvApprovalLevelM.DataBind();
            }
            else
            {
                gvApprovalLevelM.DataSource = null;
                gvApprovalLevelM.DataBind();

            }
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
                Login.Profile.ErrorHandling(ex, "Role Master", "GetDepartment");
            }
            finally
            {
                productClient.Close();
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