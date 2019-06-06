using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.TermsConditionMasterService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using BrilliantWMS.Login;
using WebMsgBox;
using System.Data;
using System.Web.Services;

namespace BrilliantWMS.Company
{
    public partial class TermsAndConditionMaster : System.Web.UI.Page
    {
       /* iTermConditionMasterClient TermsClient = new iTermConditionMasterClient(); */
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Terms and Conditions Master";
            if (!IsPostBack)
            {
                BindddlGroupName();
                BindGrid();
                FillCompany();
                hdnTermID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("TermsConditionsMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            iCompanySetupClient TermsClient = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               // gvTerm.DataSource = TermsClient.GetTermRecordToBindGrid(profile.DBConnection._constr);
                gvTerm.DataSource = TermsClient.GetTermsnConditionList(profile.DBConnection._constr);
                gvTerm.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, " Terms and Conditions Master", "BindGrid");
            }
            finally
            {
                TermsClient.Close();
            }
        }

        protected void FillCompany()
        {
            ddlCompany.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlCompany.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlCompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlCompany.Items.Insert(0, lst);
            CompanyClient.Close();
        }

        public void BindddlGroupName()
        {
            iTermConditionMasterClient TermsClient = new iTermConditionMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               //chkBLstGroupname.DataSource = TermsClient.GetGroupListToBindDDL(profile.DBConnection._constr);
                chkBLstGroupname.DataSource = TermsClient.GetGroupListToBindDDLDropdown(profile.DBConnection._constr);
                chkBLstGroupname.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Terms & Conditions Master", "BindddlGroupName");
            }
            finally
            {
                TermsClient.Close();
            }
        }

        public void clear()
        {
            txtTermName.Text = "";
            txtCondition.Text = "";
            hdnTermID.Value = null;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            chkBLstGroupname.SelectedIndex = -1;
            chkBLstGroupname.Enabled = true;
            ddlCompany.Enabled = true;
            ddlcutomer.Enabled = true;
            ddlCompany.SelectedItem.Value = "0";
            hdncustomerid.Value = "";
            ddlcutomer.SelectedIndex = -1;
            FillCompany();
           
        }

        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
            iTermConditionMasterClient TermsClient = new iTermConditionMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mTermsCondition objTerms = new mTermsCondition();

                if (hdnTermID.Value == string.Empty)
                {
                    if (chkBLstGroupname.SelectedIndex >= 0)
                    {
                        for (int i = 0; i <= chkBLstGroupname.Items.Count - 1; i++)
                        {
                            if (chkBLstGroupname.Items[i].Selected == true)
                            {
                                objTerms.Term = txtTermName.Text.Trim();
                                objTerms.Condition = txtCondition.Text;
                                objTerms.TCGroupID = Convert.ToInt64(chkBLstGroupname.Items[i].Value);
                                objTerms.Sequence = 0;
                                if (rbtnYes.Checked == true)
                                { objTerms.Active = "Y"; }
                                else
                                { objTerms.Active = "N"; }
                                objTerms.CreatedBy = profile.Personal.UserID.ToString();
                                objTerms.CreationDate = DateTime.Now;
                                objTerms.CompanyID = profile.Personal.CompanyID;
                                objTerms.CompanyID = long.Parse(ddlCompany.SelectedItem.Value);
                                objTerms.CustomerID = long.Parse(hdncustomerid.Value);
                                int result = TermsClient.InsertmTermsCondition(objTerms, profile.DBConnection._constr);
                            }
                        }
                        WebMsgBox.MsgBox.Show("Record saved successfully");
                        BindGrid();
                        clear();
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
                                objTerms = TermsClient.GetTermConditionListByID(Convert.ToInt32(hdnTermID.Value), profile.DBConnection._constr);
                                objTerms.Term = txtTermName.Text.Trim();
                                objTerms.Condition = txtCondition.Text;
                                objTerms.TCGroupID = Convert.ToInt64(chkBLstGroupname.Items[i].Value);
                                objTerms.Sequence = 0;
                                if (rbtnYes.Checked == true)
                                { objTerms.Active = "Y"; }
                                else { objTerms.Active = "N"; }
                                objTerms.LastModifiedBy = profile.Personal.UserID.ToString();
                                objTerms.LastModifiedDate = DateTime.Now;
                                objTerms.CompanyID = long.Parse(ddlCompany.SelectedItem.Value);
                                int result = TermsClient.updatemTermsCondition(objTerms, profile.DBConnection._constr);
                            }
                        }
                        WebMsgBox.MsgBox.Show("Record updated successfully");
                        BindGrid();
                        clear();
                    }
                    else
                    { WebMsgBox.MsgBox.Show("Please select Group"); }
                }
            }
            //}
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Terms & Conditions Master", "pageSave");
            }
            finally
            {

            }

        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate(int groupid)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnTermID.Value == string.Empty)
                {
                    //result = TermsClient.checkDuplicateRecord(txtTermName.Text.Trim(), Convert.ToInt32(ddlGroupName.SelectedValue), profile.DBConnection._constr);
                   /* result = TermsClient.checkDuplicateRecord(txtTermName.Text.Trim(), groupid, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        //txtTermName.Text = "";
                    }*/
                }
                else
                {
                    //result = TermsClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnTermID.Value), txtTermName.Text.Trim(), Convert.ToInt32(ddlGroupName.SelectedValue), profile.DBConnection._constr);
                    /*result = TermsClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnTermID.Value), txtTermName.Text.Trim(), groupid, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtTermName.Text = "";
                    }*/
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Terms & Conditions Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void gvTerm_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                long CompanyID = 0;
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                chkBLstGroupname.Enabled = false;
                ddlCompany.Enabled = false;
                ddlcutomer.Enabled = false;
                ddlcutomer.SelectedIndex = 0;
                Hashtable selectedrec = (Hashtable)gvTerm.SelectedRecords[0];
                CompanyID = long.Parse(selectedrec["CompanyID"].ToString());
                getCustomer(CompanyID);
                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(selectedrec["Company"].ToString()));
                ddlcutomer.SelectedIndex = ddlcutomer.Items.IndexOf(ddlcutomer.Items.FindByText(selectedrec["Customer"].ToString()));
                hdnTermID.Value = selectedrec["ID"].ToString();
                chkBLstGroupname.SelectedIndex = chkBLstGroupname.Items.IndexOf(chkBLstGroupname.Items.FindByText(selectedrec["Groupname"].ToString()));
                txtTermName.Text = selectedrec["Term"].ToString();
                txtCondition.Text = selectedrec["Condition"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else { rbtnYes.Checked = true; }
                hdncustomerid.Value = selectedrec["CustomerID"].ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Terms & Conditions Master", "gvTerm_Select");
            }
            finally
            {
            }
        }

        public void getCustomer(long CustomerID)
        {
            ddlcutomer.Items.Clear();
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcutomer.DataSource = StatutoryClient.GetCustomerList(CustomerID, profile.DBConnection._constr);
            ddlcutomer.DataTextField = "Name";
            ddlcutomer.DataValueField = "ID";
            ddlcutomer.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcutomer.Items.Insert(0, lst);
            StatutoryClient.Close();
            
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