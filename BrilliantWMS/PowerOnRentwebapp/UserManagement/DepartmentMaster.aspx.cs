using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DepartmentService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using WebMsgBox;
using System.Data;
using System.Web.Services;

namespace BrilliantWMS.UserManagement
{
    public partial class DepartmentMaster : System.Web.UI.Page
    {
        //DepartmentService.iDepartmentMasterClient DepartmentClient = new DepartmentService.iDepartmentMasterClient();
        iDepartmentMasterClient DepartmentClient = new iDepartmentMasterClient();

        //PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Department Master";
            if (!IsPostBack)
            {
                BindGrid();
                FillCompany();
                hdnDepartmentID.Value = null;
               
            }
            this.UCToolbar1.ToolbarAccess("DepartmentMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            try
            {
                long userId = 540;
                CustomProfile profile = CustomProfile.GetProfile();
                //gvDepartmentM.DataSource = DepartmentClient.GetDepartmentRecordToBind(profile.DBConnection._constr);
                gvDepartmentM.DataSource = DepartmentClient.GetDepartmentToBindGrid(userId, profile.DBConnection._constr);
                gvDepartmentM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Department Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void clear()
        {
            txtDepartment.Text = "";
            txtSequence.Text = "";
            hdnDepartmentID.Value = null;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            txtstorecode.Text = "";
            ddlcompanymain.SelectedIndex = 0;
            ddlcustomer.SelectedIndex = 0;
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            if (checkDuplicate() == "")
            {
                try
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    mDepartment ObjDepartment = new mDepartment();
                    if (hdnDepartmentID.Value == string.Empty)
                    {
                        ObjDepartment.Name = txtDepartment.Text;
                        if (txtSequence.Text != string.Empty)
                        { ObjDepartment.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjDepartment.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjDepartment.Active = "Y"; }
                        else
                        { ObjDepartment.Active = "N"; }
                        ObjDepartment.CreatedBy = profile.Personal.UserID.ToString();
                        ObjDepartment.CreationDate = DateTime.Now;
                        ObjDepartment.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjDepartment.CustomerID = long.Parse(hdncustomerid.Value);
                        ObjDepartment.DeptCode = txtstorecode.Text;
                        int result = DepartmentClient.InsertmDepartment(ObjDepartment, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");      
                        }
                        BindGrid();
                        clear();
                    }
                    else
                    {
                        ObjDepartment = DepartmentClient.GetDepartmentListByID(Convert.ToInt32(hdnDepartmentID.Value), profile.DBConnection._constr);
                        ObjDepartment.Name = txtDepartment.Text;
                        if (txtSequence.Text != string.Empty)
                        { ObjDepartment.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjDepartment.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjDepartment.Active = "Y"; }
                        else
                        { ObjDepartment.Active = "N"; }
                        ObjDepartment.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjDepartment.LastModifiedDate = DateTime.Now;
                        ObjDepartment.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjDepartment.CustomerID = long.Parse(hdncustomerid.Value);
                        ObjDepartment.DeptCode = txtstorecode.Text;
                        int result = DepartmentClient.updatemDepartment(ObjDepartment, profile.DBConnection._constr);
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
                    Login.Profile.ErrorHandling(ex, this, "Department Master", "pageSave");
                }
                finally
                {
                }
            }
        }

        public string checkDuplicate()
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                if (hdnDepartmentID.Value == string.Empty)
                {
                    
                    result = DepartmentClient.checkDuplicateRecord(txtDepartment.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDepartment.Text = "";
                    }
                    //txtSequence.Focus();
                }
                else
                {
                    result = DepartmentClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnDepartmentID.Value), txtDepartment.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDepartment.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                string result = "";
                Login.Profile.ErrorHandling(ex, this, "Department Master", "checkDuplicate");
                return result;
            }
            finally
            {
            }

        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void gvDepartmentM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvDepartmentM.SelectedRecords[0];
                hdnDepartmentID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                txtDepartment.Text = selectedrec["Name"].ToString();
                txtstorecode.Text = selectedrec["DeptCode"].ToString();
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

        // Company Customer DropDown Code

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