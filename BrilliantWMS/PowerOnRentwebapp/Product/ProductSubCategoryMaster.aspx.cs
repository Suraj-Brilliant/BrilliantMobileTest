using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.ProductSubCategoryService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;
using System.Web.Services;
using System.Data;

namespace BrilliantWMS.Product
{
    public partial class ProductSubCategoryMaster : System.Web.UI.Page
    {
        
        BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient ProductCategoryClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
        BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient ProductSubCategoryClient = new BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient();
        //ProductCategoryService.connectiondetails profile.DBConnection._constr1 = new ProductCategoryService.connectiondetails();
        
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //profile.DBConnection._constr1.DataBaseName = Profile.DataBase;
            //profile.DBConnection._constr1.DataSource = Profile.DataSource;
            //profile.DBConnection._constr1.DBPassword = Profile.DBPassword;
            UCFormHeader1.FormHeaderText = "Product Sub-Category Master";
            if (!IsPostBack)
            {
                BinddlProductCategory();
                BindGrid();
                FillCompany();
                hdnPrdSubCategoryID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("ProductSubCategoryMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvPrdSubCategoryM.DataSource = ProductSubCategoryClient.GetPrdSubCategoryRecordToBind(profile.DBConnection._constr);
                gvPrdSubCategoryM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Sub Category Master", "BindGrid");
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

        public void BinddlProductCategory()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ddlPrdCategory.DataSource = ProductCategoryClient.GetProductCategoryList(profile.DBConnection._constr);
                ddlPrdCategory.DataBind();
                ListItem lst = new ListItem();
                lst.Text = "-Select-";
                lst.Value = "0";
                ddlPrdCategory.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Sub Category Master", "BinddlProductCategory");
            }
            finally
            {
            }
        }

        public void clear()
        {
            txtPrdSubCategory.Text = "";
            txtSequence.Text = "";
            hdnPrdSubCategoryID.Value = null;
            ddlPrdCategory.SelectedIndex = -1;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            ddlcompanymain.SelectedItem.Value = "0";
            ddlcustomer.SelectedItem.Value = "";
            FillCompany();
            ddlcustomer.SelectedIndex = -1;
            hdncategoryid.Value = "";
            hdncompanyid.Value = "";
            hdncustomerid.Value = "";
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (checkDuplicate() == "")
                {
                    BrilliantWMS.ProductSubCategoryService.mProductSubCategory ObjPrdSubCategory = new BrilliantWMS.ProductSubCategoryService.mProductSubCategory();
                    if (hdnPrdSubCategoryID.Value == string.Empty)
                    {
                        ObjPrdSubCategory.Name = txtPrdSubCategory.Text;
                        if (hdncategoryid.Value == "")
                        { ObjPrdSubCategory.ProductCategoryID = 0; }
                        else { ObjPrdSubCategory.ProductCategoryID = Convert.ToInt64(hdncategoryid.Value); }
                        if (txtSequence.Text != string.Empty) { ObjPrdSubCategory.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else { ObjPrdSubCategory.Sequence = 0; }
                        if (rbtnYes.Checked == true) { ObjPrdSubCategory.Active = "Y"; }
                        else { ObjPrdSubCategory.Active = "N"; }
                        ObjPrdSubCategory.CreatedBy = profile.Personal.UserID.ToString();
                        ObjPrdSubCategory.CreationDate = DateTime.Now;
                       // ObjPrdSubCategory.Companyid = profile.Personal.CompanyID;
                        ObjPrdSubCategory.Companyid = long.Parse(ddlcompanymain.SelectedItem.Value);
                        ObjPrdSubCategory.CustomerID = long.Parse(hdncustomerid.Value);

                        int result = ProductSubCategoryClient.InsertmProductSubCategory(ObjPrdSubCategory, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully"); 
                        }
                        BindGrid();
                        clear();
                    }
                    else
                    {
                        ObjPrdSubCategory = ProductSubCategoryClient.GetProductSubCategoryListByID(Convert.ToInt32(hdnPrdSubCategoryID.Value), profile.DBConnection._constr);
                        long customerID = long.Parse(ObjPrdSubCategory.CustomerID.ToString());
                        long CategoryID = long.Parse(ObjPrdSubCategory.ProductCategoryID.ToString());
                        ObjPrdSubCategory.Name = txtPrdSubCategory.Text;
                        if (hdncategoryid.Value == "")
                        {
                            ObjPrdSubCategory.ProductCategoryID = CategoryID;
                        }
                        else
                        {
                            ObjPrdSubCategory.ProductCategoryID = Convert.ToInt64(hdncategoryid.Value);
                        }
                        
                        if (txtSequence.Text != string.Empty) { ObjPrdSubCategory.Sequence = Convert.ToInt64(txtSequence.Text); }
                        else { ObjPrdSubCategory.Sequence = 0; }
                        if (rbtnYes.Checked == true) { ObjPrdSubCategory.Active = "Y"; }
                        else { ObjPrdSubCategory.Active = "N"; }
                        ObjPrdSubCategory.LastModifiedBy =profile.Personal.UserID.ToString();
                        ObjPrdSubCategory.LastModifiedDate = DateTime.Now;
                        ObjPrdSubCategory.Companyid = long.Parse(ddlcompanymain.SelectedItem.Value);
                        if (hdncustomerid.Value == "")
                        {
                            ObjPrdSubCategory.CustomerID = customerID;
                        }
                        else
                        {
                            ObjPrdSubCategory.CustomerID = long.Parse(hdncustomerid.Value);
                        }

                        int result = ProductSubCategoryClient.updatemProductSubCategory(ObjPrdSubCategory, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully"); 
                        }
                        BindGrid();
                        clear();
                    }
                }
                clear();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Sub Category Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
        }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnPrdSubCategoryID.Value == string.Empty)
                {
                    result = ProductSubCategoryClient.checkDuplicateRecord(txtPrdSubCategory.Text, Convert.ToInt32(hdncategoryid.Value), long.Parse(hdncustomerid.Value), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);   
                        txtPrdSubCategory.Text = "";
                    }
                    txtSequence.Focus();
                }
                else
                {
                    result = ProductSubCategoryClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnPrdSubCategoryID.Value), txtPrdSubCategory.Text, Convert.ToInt32(ddlPrdCategory.SelectedValue), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);                          
                        txtPrdSubCategory.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Sub Category Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void gvPrdSubCategoryM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvPrdSubCategoryM.SelectedRecords[0];
                long CompanyID = long.Parse(selectedrec["Companyid"].ToString());
                long CustomerID = long.Parse(selectedrec["CustomerID"].ToString());
                getCustomer(CompanyID);
                GetCategory(CustomerID);
                hdnPrdSubCategoryID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByValue(selectedrec["Companyid"].ToString()));
                ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByText(selectedrec["Customer"].ToString()));
                ddlPrdCategory.SelectedIndex = ddlPrdCategory.Items.IndexOf(ddlPrdCategory.Items.FindByText(selectedrec["PrdCategoryName"].ToString()));
                txtPrdSubCategory.Text = selectedrec["Name"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product Sub Category Master", "gvPrdSubCategoryM_Select");
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

        public void GetCategory(long CustomerID)
        {
            ddlPrdCategory.Items.Clear();
            iProductSubCategoryMasterClient SubCategoryClient = new iProductSubCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlPrdCategory.DataSource = SubCategoryClient.GetCategoryListByCustomer(CustomerID, profile.DBConnection._constr); ;
            ddlPrdCategory.DataTextField = "Name";
            ddlPrdCategory.DataValueField = "ID";
            ddlPrdCategory.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlPrdCategory.Items.Insert(0, lst);
            SubCategoryClient.Close();
        }

        [WebMethod]
        public static List<contact> GetCustomer(object objReq)
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
        public static List<contact> GetCategory(object objReq)
        {
            iProductSubCategoryMasterClient SubCategoryClient = new iProductSubCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long ddlcustomer = long.Parse(dictionary["ddlcustomer"].ToString());
                ds = SubCategoryClient.GetCategoryListByCustomer(ddlcustomer, profile.DBConnection._constr);
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
                SubCategoryClient.Close();
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