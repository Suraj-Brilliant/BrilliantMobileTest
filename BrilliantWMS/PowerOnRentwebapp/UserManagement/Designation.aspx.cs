using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DepartmentService;
using BrilliantWMS.DesignationService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;
using System.Data;
using System.Web.Services;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;

namespace BrilliantWMS.UserManagement
{
    public partial class Designation : System.Web.UI.Page
    {
        BrilliantWMS.DepartmentService.iDepartmentMasterClient DepartmentClient = new BrilliantWMS.DepartmentService.iDepartmentMasterClient();
        BrilliantWMS.DesignationService.iDesignationMasterClient DesignationClient = new BrilliantWMS.DesignationService.iDesignationMasterClient();
        PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UCFormHeader1.FormHeaderText = "Designation Master";
            if (!IsPostBack)
            {
                // BinddlDepartment();
                FillCompany();
                BindGrid();
                hdnDesignationID.Value = null;
                if (profile.Personal.CompanyID == 14)
                {
                    Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                    btnExport.Visible = false;
                    Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                    btnImport.Visible = false;
                    Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                    btmMail.Visible = false;
                    Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                    btnPrint.Visible = false;
                }
            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            try
            {
                long UserID = 540;
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = DesignationClient.GetDesignationRecordToBind(UserID, profile.DBConnection._constr);
                //gvDesignationM.DataSource = DesignationClient.GetDesignationRecordToBind(UserID, profile.DBConnection._constr);
                gvDepartmentM.DataSource = DesignationClient.GetDesignationListTOBindgrid(profile.DBConnection._constr);
                gvDepartmentM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void BinddlDepartment()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ddlDepartment.DataSource = DepartmentClient.GetDeparmentList(long.Parse(hdncustomerid.Value), profile.DBConnection._constr);
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "ID";
                ddlDepartment.DataBind();
                ListItem lst = new ListItem();
                lst.Text = "-Select-";
                lst.Value = "0";
                ddlDepartment.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "BinddlDepartment");
            }
            finally
            {
            }
        }

        public void clear()
        {
            try
            {
                txtDesignation.Text = "";
                txtSequence.Text = "";
                hdnDesignationID.Value = null;
                ddlDepartment.SelectedIndex = -1;
                rbtnYes.Checked = true;
                rbtnNo.Checked = false;
                ddlcompanymain.SelectedIndex = 0;
                ddlcustomer.SelectedIndex = -1;
                hdnCompanyid.Value = "";
                hdncustomerid.Value = "";
                hdndepartment.Value = "";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "BinddlDepartment");
            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            BrilliantWMS.DesignationService.mDesignation ObjDesignation = new BrilliantWMS.DesignationService.mDesignation();
            if (checkDuplicate() == "")
            {
                try
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    if (hdnDesignationID.Value == string.Empty)
                    {
                        ObjDesignation.Name = txtDesignation.Text.Trim();
                        //ObjDesignation.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                        if (txtSequence.Text != string.Empty)
                        { ObjDesignation.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjDesignation.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjDesignation.Active = "Y"; }
                        else
                        { ObjDesignation.Active = "N"; }
                        ObjDesignation.CreatedBy = profile.Personal.UserID.ToString();
                        ObjDesignation.CreationDate = DateTime.Now;
                        ObjDesignation.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjDesignation.CustomerID = long.Parse(hdncustomerid.Value);
                        ObjDesignation.DepartmentID = long.Parse(hdndepartment.Value);

                        int result = DesignationClient.InsertmDesignation(ObjDesignation, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");
                        }
                        BindGrid();
                        clear();
                    }
                    else
                    {
                        ObjDesignation = DesignationClient.GetDesignationListByID(Convert.ToInt32(hdnDesignationID.Value), profile.DBConnection._constr);
                        ObjDesignation.Name = txtDesignation.Text.Trim();
                        // ObjDesignation.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);
                        if (txtSequence.Text != string.Empty)
                        { ObjDesignation.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjDesignation.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjDesignation.Active = "Y"; }
                        else
                        { ObjDesignation.Active = "N"; }
                        ObjDesignation.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjDesignation.LastModifiedDate = DateTime.Now;
                        ObjDesignation.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjDesignation.CustomerID = long.Parse(hdncustomerid.Value);
                        ObjDesignation.DepartmentID = long.Parse(hdndepartment.Value);
                        int result = DesignationClient.updatemDesignation(ObjDesignation, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");
                        }
                        BindGrid();
                        clear();
                    }
                }
                catch (System.Exception ex)
                {
                    Login.Profile.ErrorHandling(ex, this, "Designation Master", "pageSave");
                }
                finally
                {
                }
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnDesignationID.Value == string.Empty)
                {
                    result = DesignationClient.checkDuplicateRecord(txtDesignation.Text.Trim(), Convert.ToInt32(ddlDepartment.SelectedValue), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDesignation.Text = "";
                    }
                    txtSequence.Focus();
                }
                else
                {
                    result = DesignationClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnDesignationID.Value), txtDesignation.Text.Trim(), Convert.ToInt32(ddlDepartment.SelectedValue), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDesignation.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

       /* protected void gvDesignationM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                //clear();
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvDesignationM.SelectedRecords[0];
                hdnDesignationID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByText(selectedrec["Department"].ToString()));
                txtDesignation.Text = selectedrec["Name"].ToString();
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByValue(selectedrec["CompanyID"].ToString()));
                getCustomer(long.Parse(selectedrec["CompanyID"].ToString()));
                ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(selectedrec["CustomerID"].ToString()));
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
                txtDesignation.Focus();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "gvDesignationM_Select");
            }
            finally
            {
            }
        }*/

        protected void gvDepartmentM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvDepartmentM.SelectedRecords[0];
                //hdnDepartmentID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                //txtDepartment.Text = selectedrec["Name"].ToString();
                //txtstorecode.Text = selectedrec["DeptCode"].ToString();
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByValue(selectedrec["CompanyID"].ToString()));
                getCustomer(long.Parse(selectedrec["CompanyID"].ToString()));
                ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(selectedrec["CustomerID"].ToString()));
                hdncustomerid.Value = selectedrec["CustomerID"].ToString();
                if (selectedrec["Active"].ToString() == "N")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
                //txtDepartment.Focus();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Department Master", "gvDepartmentM_Select");
            }
            finally
            {
            }
        }

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
            ddlcustomer.Items.Clear();
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcustomer.DataSource = StatutoryClient.GetCustomerList(CompanyID, profile.DBConnection._constr);
            ddlcustomer.DataTextField = "Name";
            ddlcustomer.DataValueField = "ID";
            ddlcustomer.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcustomer.Items.Insert(0, lst);
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