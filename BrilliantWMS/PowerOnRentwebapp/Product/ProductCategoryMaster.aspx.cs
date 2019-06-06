using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.ProductCategoryService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Data;
using WebMsgBox;

namespace BrilliantWMS.Product
{
    public partial class ProductCategoryMaster : System.Web.UI.Page
    {
        
        BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient ProductCategoryClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
        //PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Product Category Master";
            if (!IsPostBack)
            { 
                BindGrid();
                FillCompany();
                hdnPrdCategoryID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("ProductCategoryMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear; 
        }

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //gvPrdCat.DataSource = ProductCategoryClient.GetPrdCategoryRecordToBindGrid(profile.DBConnection._constr);
                gvPrdCat.DataSource = ProductCategoryClient.GetCustomerList(profile.DBConnection._constr);
                gvPrdCat.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Category Master", "BindGrid");
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

        public void clear()
        {
            txtPrdCategory.Text = "";
            txtSequence.Text = "";
            hdnPrdCategoryID.Value = null;
            txtPrdCategory.Focus();  
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            FillCompany();
            ddlcompanymain.SelectedItem.Value = "";
            ddlcustomer.SelectedItem.Value = "";
            ddlcustomer.SelectedIndex = -1;
            
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear();}

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (checkDuplicate() == "")
                {
                    mProductCategory ObjPrdCategory = new mProductCategory();
                    if (hdnPrdCategoryID.Value == string.Empty)
                    {
                        ObjPrdCategory.Name = txtPrdCategory.Text.Trim();
                        if (txtSequence.Text != string.Empty)
                        { ObjPrdCategory.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjPrdCategory.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjPrdCategory.Active = "Y"; }
                        else
                        { ObjPrdCategory.Active = "N"; }
                        ObjPrdCategory.CreatedBy = profile.Personal.UserID.ToString();
                        ObjPrdCategory.CreationDate = DateTime.Now;

                        //ObjPrdCategory.CompanyID = profile.Personal.CompanyID;
                        ObjPrdCategory.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjPrdCategory.CustomerID = long.Parse(hdncustomerid.Value);
                        int result = ProductCategoryClient.InsertmProductCategory(ObjPrdCategory, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");                           
                        }
                        BindGrid();
                        clear();
                    }

                    else
                    {
                        ObjPrdCategory = ProductCategoryClient.GetProductCategoryListByID(Convert.ToInt32(hdnPrdCategoryID.Value), profile.DBConnection._constr);
                        long customerID = long.Parse(ObjPrdCategory.CustomerID.ToString());
                        ObjPrdCategory.Name = txtPrdCategory.Text.Trim();
                        if (txtSequence.Text != string.Empty)
                        { ObjPrdCategory.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else
                        { ObjPrdCategory.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjPrdCategory.Active = "Y"; }
                        else
                        { ObjPrdCategory.Active = "N"; }
                        ObjPrdCategory.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjPrdCategory.LastModifiedDate = DateTime.Now;
                        ObjPrdCategory.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        if (hdncustomerid.Value == "")
                        {
                            ObjPrdCategory.CustomerID = customerID;
                        }
                        else
                        {
                            ObjPrdCategory.CustomerID = long.Parse(hdncustomerid.Value);
                        }
                       
                        int result = ProductCategoryClient.updatemProductCategory(ObjPrdCategory, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");                              
                        }
                        BindGrid();
                        clear();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Category Master", "pageSave");
            }
            finally
            {
            }
        }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnPrdCategoryID.Value == string.Empty)
                {
                    result = ProductCategoryClient.checkDuplicateRecord(txtPrdCategory.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);   
                        txtPrdCategory.Text = "";
                    }
                    txtSequence.Focus();
                }
                else
                {
                    result = ProductCategoryClient.checkDuplicateRecordEdit(txtPrdCategory.Text.Trim(), Convert.ToInt32(hdnPrdCategoryID.Value), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);   
                        txtPrdCategory.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Category Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear();}

        protected void gvPrdCat_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvPrdCat.SelectedRecords[0];
                long CompanyID = long.Parse(selectedrec["CompanyID"].ToString());
                getCustomer(CompanyID);
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByText(selectedrec["Company"].ToString()));
                ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByText(selectedrec["Customer"].ToString()));
             //   hdncustomerid.Value = selectedrec["CustomerID"].ToString();
                hdnPrdCategoryID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                txtPrdCategory.Text = selectedrec["Category"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Category Master", "gvPrdCat_Select");
               
            }
            finally
            {
            }
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