using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.StatutoryService; // Add StatutoryService service
using Obout.Interface;
using System.Web.Services;
using System.Collections;
using System.Data;
using BrilliantWMS.Login;
using WebMsgBox;
using BrilliantWMS.CompanySetupService;

namespace BrilliantWMS.Tax
{
    public partial class StatutoryMaster : System.Web.UI.Page
    {
        int result = 0;
        StatutoryService.iStatutoryMasterClient StatutoryClient = new StatutoryService.iStatutoryMasterClient();
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Statutory Master";
            if (!IsPostBack)
            {
                BindGrid();
                hdnStatutoryID.Value = null;
               // FillCompany();
            }
           
            this.UCToolbar1.ToolbarAccess("TaxMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        //protected void FillCompany()
        //{
        //    ddlCompany.Items.Clear();
        //    iCompanySetupClient CompanyClient = new iCompanySetupClient();
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    ddlCompany.DataSource = CompanyClient.GetCompanyDropDown(profile.DBConnection._constr);
        //    ddlCompany.DataBind();
        //    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
        //    ddlCompany.Items.Insert(0, lst);
        //}

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvStatutoryM.DataSource = StatutoryClient.GetStatutoryRecordToBindGrid(profile.DBConnection._constr);
                gvStatutoryM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Statutory Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void clear()
        {
            txtName.Text = "";
            hdnStatutoryID.Value = null;
            txtRemark.Text = "";
            chkBLstGroupname.SelectedIndex = -1;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            chkBLstGroupname.Enabled = true;
                       
        }

        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //if (checkDuplicate() == "")
                //{
                mStatutory ObjStatutory = new mStatutory();
                if (hdnStatutoryID.Value == string.Empty)
                {
                    if (chkBLstGroupname.SelectedIndex >= 0)
                    {
                        for (int i = 0; i <= chkBLstGroupname.Items.Count - 1; i++)
                        {
                            if (chkBLstGroupname.Items[i].Selected == true)
                            {
                               
                               ObjStatutory.Name = txtName.Text.Trim();
                                ObjStatutory.ObjectName = chkBLstGroupname.Items[i].Value;
                                ObjStatutory.Sequence = 0;
                                if (rbtnYes.Checked == true)
                                { ObjStatutory.Active = "Y"; }
                                else
                                { ObjStatutory.Active = "N"; }
                                ObjStatutory.CreatedBy = profile.Personal.UserID.ToString();
                                ObjStatutory.CreationDate = DateTime.Now;
                                ObjStatutory.Remark = txtRemark.Text;
                                ObjStatutory.CompanyID = profile.Personal.CompanyID;                    
                                result = StatutoryClient.InsertmStatutory(ObjStatutory, profile.DBConnection._constr);
                            }
                        }
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");
                            BindGrid();
                            clear();
                        }
                    }
                    else
                    { WebMsgBox.MsgBox.Show("Please select Group"); }
                }
                else
                {
                    if (chkBLstGroupname.SelectedIndex >= 0)
                    {
                        for (int i = 0; i <= chkBLstGroupname.Items.Count - 1; i++)
                        {
                            if (chkBLstGroupname.Items[i].Selected == true)
                            {
                                if (checkDuplicate(chkBLstGroupname.Items[i].Value) == "")
                                {
                                    ObjStatutory = StatutoryClient.GetGetStatutoryListByID(Convert.ToInt32(hdnStatutoryID.Value), profile.DBConnection._constr);
                                    ObjStatutory.Name = txtName.Text.Trim();
                                    ObjStatutory.ObjectName = chkBLstGroupname.SelectedValue;
                                    ObjStatutory.Sequence = 0;
                                    if (rbtnYes.Checked == true)
                                    { ObjStatutory.Active = "Y"; }
                                    else
                                    { ObjStatutory.Active = "N"; }
                                    ObjStatutory.LastModifiedBy = profile.Personal.UserID.ToString();
                                    ObjStatutory.LastModifiedDate = DateTime.Now;
                                    ObjStatutory.Remark = txtRemark.Text;
                                    result = StatutoryClient.updatemStatutory(ObjStatutory, profile.DBConnection._constr);
                                }
                            }
                        }
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");
                            BindGrid();
                            clear();
                        }
                    }
                    else
                    { WebMsgBox.MsgBox.Show("Please select Group"); }
                }
                // }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Statutory Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate(string groupname)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";
                if (hdnStatutoryID.Value == string.Empty)
                {
                    //result = StatutoryClient.checkDuplicateRecord(txtName.Text.Trim(), chkBLstGroupname.SelectedItem.Text.ToString(), profile.DBConnection._constr);
                   /* result = StatutoryClient.checkDuplicateRecord(txtName.Text.Trim(), groupname, profile.DBConnection._constr);*/
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        //txtName.Text = "";
                    }
                    //txtName.Focus();
                }
                else
                {
                    /* result = StatutoryClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnStatutoryID.Value), txtName.Text.Trim(), groupname, profile.DBConnection._constr);*/
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtName.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                string result = "";
                Login.Profile.ErrorHandling(ex, this, "Statutory Master", "checkDuplicate");
                return result;
            }
            finally
            {
            }
        }

        protected void gvStatutoryM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                chkBLstGroupname.Enabled = false;
                Hashtable selectedrec = (Hashtable)gvStatutoryM.SelectedRecords[0];
                hdnStatutoryID.Value = selectedrec["ID"].ToString();
                chkBLstGroupname.SelectedIndex = chkBLstGroupname.Items.IndexOf(chkBLstGroupname.Items.FindByText(selectedrec["ObjectName"].ToString()));
                txtName.Text = selectedrec["Name"].ToString();
                txtRemark.Text = selectedrec["Remark"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Statutory Master", "gvStatutoryM_Select");
            }
            finally
            {
            }
        }

        [WebMethod]
        public static List<contact> GetDepartment(object objReq)
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
                Loc.Name = "Select Customer";
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