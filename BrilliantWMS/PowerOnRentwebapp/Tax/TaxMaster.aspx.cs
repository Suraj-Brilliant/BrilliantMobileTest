using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.TaxMasterService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;
using System.Web.Services;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using System.Data;


namespace BrilliantWMS.Tax
{
    public partial class TaxMaster : System.Web.UI.Page
    {
       TaxMasterService.iTaxMasterClient TaxClient = new TaxMasterService.iTaxMasterClient();

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Tax Master";
            if (!IsPostBack)
            {
                BindGrid();
                FillCompany();
                ddlTaxType.Focus();
                hdnTaxID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("TaxMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            lblTaxMapping.Text = "";
            if (txtTaxMappingID1.Text.ToString() != "") { lblTaxMapping.Text = "Tax Mapping :"; }
        }

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               gvTaxM.DataSource = TaxClient.GetTaxRecordToBindGrid(profile.DBConnection._constr);
                gvTaxM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Tax Master", "BindGrid");

            }
            finally
            {
            }
        }

        public void BindTaxMappingGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                /*gvTaxMappingM.DataSource = TaxClient.GetTaxRecordToBindTaxMappingGrid(profile.DBConnection._constr);
                gvTaxMappingM.DataBind();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Tax Master", "BindTaxMappingGrid");

            }
            finally
            {
            }
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            BindTaxMappingGrid();
        }

        public void clear()
        {
            txtTaxName.Enabled = true;
            ddlTaxType.Enabled = true;
            txtTaxPercent.Enabled = true;
            hdnTaxIDs.Value = null;
            txtTaxName.Text = "";
            txtSequence.Text = "";
            hdnTaxID.Value = null;
            txtTaxPercent.Text = "";
            txtTaxMappingID1.Text = "";
            txtDescription.Text = "";
            ddlTaxType.SelectedIndex = -1;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            ddlcompanymain.SelectedIndex = 0;
            ddlcustomer.SelectedIndex = -1;
        }
        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
            if (checkDuplicate() == "")
            {
                try
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    mTaxSetup ObjTax = new mTaxSetup();

                    if (hdnTaxID.Value == string.Empty)
                    {
                        if (ddlTaxType.SelectedValue == "Tax On Tax" && hdnTaxIDs.Value.ToString() == "")
                        {
                            WebMsgBox.MsgBox.Show("Please select Tax from list for Tax on Principal"); ddlTaxType.SelectedIndex = 0;
                        }
                        else
                        {
                           ObjTax.Name = txtTaxName.Text;
                            ObjTax.Description = txtDescription.Text;
                            ObjTax.Type = ddlTaxType.SelectedValue;
                            if (txtSequence.Text != string.Empty)
                            { ObjTax.Sequence = Convert.ToInt64(txtSequence.Text); }
                            else
                            { ObjTax.Sequence = 0; }
                            if (rbtnYes.Checked == true)
                            { ObjTax.Active = "Y"; }
                            else
                            { ObjTax.Active = "N"; }
                            ObjTax.CreatedBy = profile.Personal.UserID.ToString();
                            ObjTax.CreatedDate = DateTime.Now;
                            ObjTax.Percent = Convert.ToDecimal(txtTaxPercent.Text);
                            ObjTax.TaxMappingID = hdnTaxIDs.Value.ToString();
                           // ObjTax.CompanyID = profile.Personal.CompanyID;
                            ObjTax.CompanyID =  long.Parse(ddlcompanymain.SelectedItem.Value);
                            ObjTax.CustomerID = long.Parse(hdncustomerid.Value);
                            int result = TaxClient.InsertmTaxSetup(ObjTax, profile.DBConnection._constr);
                            if (result == 1)
                            {
                                WebMsgBox.MsgBox.Show("Record saved successfully");
                            }
                            BindGrid();
                            clear();
                        }
                    }
                    else
                    {
                       ObjTax = TaxClient.GetTaxListByID(Convert.ToInt32(hdnTaxID.Value), profile.DBConnection._constr);

                        ObjTax.Name = txtTaxName.Text;
                        ObjTax.Description = txtDescription.Text;
                        ObjTax.Type = ddlTaxType.SelectedValue;
                        if (txtSequence.Text != string.Empty)
                        { ObjTax.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else { ObjTax.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjTax.Active = "Y"; }
                        else
                        { ObjTax.Active = "N"; }
                        ObjTax.LastEditBy = profile.Personal.UserID.ToString();
                        ObjTax.Description = txtDescription.Text;
                        ObjTax.LastEditDate = DateTime.Now;
                        ObjTax.Percent = Convert.ToDecimal(txtTaxPercent.Text);
                        ObjTax.CustomerID = long.Parse(hdncustomerid.Value);
                        int result = TaxClient.updatemTaxSetup(ObjTax, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");
                        }
                        BindGrid();
                        clear();
                    }
                    //}
                }
                catch (System.Exception ex)
                {
                    Login.Profile.ErrorHandling(ex, this, "Tax Master", "pageSave");
                }
                finally
                {
                }
            }
        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnTaxID.Value == string.Empty)
                {
                    result = TaxClient.checkDuplicateRecord(txtTaxName.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtTaxName.Text = "";
                    }
                    txtSequence.Focus();
                }
                else
                {
                    int id = Convert.ToInt32(hdnTaxID.Value);
                    result = TaxClient.checkDuplicateRecordEdit(id, txtTaxName.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtTaxName.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Tax Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            txtTaxName.Enabled = false;
            ddlTaxType.Enabled = false;
            txtTaxPercent.Enabled = false;
        }

        protected void gvTaxM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvTaxM.SelectedRecords[0];
                FillCompany();
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByValue(selectedrec["CompanyID"].ToString()));
                getCustomer(long.Parse(selectedrec["CompanyID"].ToString()));
                ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(selectedrec["CustomerID"].ToString()));
                hdncustomerid.Value = selectedrec["CustomerID"].ToString();
                hdnTaxID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                ddlTaxType.SelectedIndex = ddlTaxType.Items.IndexOf(ddlTaxType.Items.FindByText(selectedrec["Type"].ToString()));
                txtTaxName.Text = selectedrec["Name"].ToString();
                txtTaxPercent.Text = selectedrec["Percent"].ToString();
               // txtTaxMappingID1.Text = selectedrec["TaxMapping"].ToString();
                txtDescription.Text = selectedrec["Description"].ToString();

                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
                /*mTaxSetup ObjTax = new mTaxSetup();
                ObjTax = TaxClient.GetTaxListByID(Convert.ToInt32(hdnTaxID.Value), profile.DBConnection._constr);
                txtDescription.Text = ObjTax.Description;*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Tax Master", "gvTaxM_Select");
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