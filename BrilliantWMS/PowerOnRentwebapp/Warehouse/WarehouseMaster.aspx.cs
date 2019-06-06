using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Obout.Interface;
using System.Collections;
using Obout.Grid;
using BrilliantWMS.PopupMessages;
using WebMsgBox;
using System.Data;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.WarehouseService;
using System.Web.Services;
//using BrilliantWMS.AddressInfoService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;

namespace BrilliantWMS.POR
{
    public partial class WarehouseMaster : System.Web.UI.Page
    {
        static string sessionID = "";

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void ResetUserControl()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                /*Set Values to UC_AddressInformation1*/
                UCAddress1.ClearAddress("Warehouse");

                /*Set Values to UC_ContactPerson1*/
                UCContactPerson1.ClearContactPerson("Warehouse");

                /*Set Values to UC_Document*/
               // UC_AttachDocument1.ClearDocument("Customer");

                /*Set Values to UC_StatutoryDetails1*/
               // UC_StatutoryDetails1.BindGridStatutoryDetails(0, "Customer", profile.Personal.CompanyID);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "ResetUserControl");
            }
            finally
            {
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                Session.Add("WarehouseName", "");
                ActiveTab("Load");
                WarehouseGridBind();
            }
            sessionID = Session.SessionID;
            UCFormHeader1.FormHeaderText = "Warehouse Master";
           // UCToolbar1.ParentPage = this;
           // this.UCToolbar1.ToolbarAccess("Account");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
 
        }

       


        protected void btncustomernext_Click(object sender, System.EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient WarehouseClient = new iWarehouseClient();
            try
            {
                mWarehouseMaster WHDetail = new mWarehouseMaster();
                tAddress address = new tAddress();
                WHDetail.Code = txtcode.Text.ToString();
                WHDetail.WarehouseName = txtwarehousename.Text.ToString();
                WHDetail.Type = ddltype.SelectedItem.Text;
                WHDetail.Description = txtdescription.Text.ToString();
                WHDetail.Remark = txtremark.Text.ToString();
                WHDetail.Active = "No";
                if (rbtYes.Checked == true) WHDetail.Active = "Yes";
                WHDetail.CompanyID = long.Parse(hdncompanyid.Value);
                WHDetail.CustomerID = long.Parse(hdncustomerid.Value);
                WHDetail.CreatedBy = profile.Personal.UserID;
                WHDetail.CreationDate = DateTime.Now;
                address.AddressLine1 = txtCAddress1.Text.ToString();
                address.AddressLine2 = txtAddress2.Text.ToString();
                address.Zipcode = txtZipCode.Text.ToString();
                address.County = hdnCountry.Value;
                address.State = hdncountryState.Value;
                address.City = txtCity.Text.ToString();
                address.ObjectName = "Warehouse";
                address.Active = "Y";
                address.CreatedBy = profile.Personal.UserID.ToString();
                address.CreationDate = DateTime.Now;
                address.CompanyID = long.Parse(hdncompanyid.Value);
                address.AddressType = "Warehouse";
                hdnWarehouseName.Value = txtwarehousename.Text.ToString();

                Session["WarehouseName"] = txtwarehousename.Text.ToString();
                long WarehouseID = 0;
                WarehouseID = WarehouseClient.SaveWarehouseMaster(WHDetail, profile.DBConnection._constr);
                address.ReferenceID = WarehouseID;
                long AddressID = WarehouseClient.SaveWarehouseAddress(address, profile.DBConnection._constr);
                hdnwarehouseID.Value = WarehouseID.ToString();
                Session.Add("CompanyID", hdncompanyid.Value);
                if (hdnwarehouseID.Value != "0")
                {
                    ActiveTab("Next");
                    btncustomernext.Visible = false;
                }
                else
                {
                    WebMsgBox.MsgBox.Show("Error Occured");
                }

            }
            catch(Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "Buttonnextsave");
            }
            finally
            {
 
            }
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            mWarehouseMaster WHDetail = new mWarehouseMaster();
            tAddress address = new tAddress();
            if (hdnstate.Value == "Edit")
            {
                WHDetail = Warehouseclient.GetWarehouseMasterByID(long.Parse(hdnwarehouseID.Value), profile.DBConnection._constr);
                address = Warehouseclient.GetWarehouseAddress(long.Parse(hdnwarehouseID.Value), profile.DBConnection._constr);
            }
            WHDetail.Code = txtcode.Text.ToString();
            WHDetail.WarehouseName = txtwarehousename.Text.ToString();
            WHDetail.Type = ddltype.SelectedItem.Text;
            WHDetail.Description = txtdescription.Text.ToString();
            WHDetail.Remark = txtremark.Text.ToString();
            WHDetail.Active = "No";
            if (rbtYes.Checked == true) WHDetail.Active = "Yes";
            WHDetail.CompanyID = long.Parse(hdncompanyid.Value);
            if (hdncustomerid.Value != "")
            {
                WHDetail.CustomerID = long.Parse(hdncustomerid.Value);
            }
            else
            {
                WHDetail.CustomerID = long.Parse(hdnNewCustomerID.Value);
            }
            WHDetail.CreatedBy = profile.Personal.UserID;
            WHDetail.CreationDate = DateTime.Now;
            address.AddressLine1 = txtCAddress1.Text.ToString();
            address.AddressLine2 = txtAddress2.Text.ToString();
            address.Zipcode = txtZipCode.Text.ToString();
            address.County = hdnCountry.Value;
            address.State = hdncountryState.Value;
            address.City = txtCity.Text.ToString();
            address.ObjectName = "Warehouse";
            address.Active = "Y";
            address.CreatedBy = profile.Personal.UserID.ToString();
            address.CreationDate = DateTime.Now;
            address.CompanyID = long.Parse(hdncompanyid.Value);
            address.AddressType = "Warehouse";
            if (hdnstate.Value == "Edit")
            {
                WHDetail.ModifiedBy = profile.Personal.UserID;
                WHDetail.ModifiedDate = DateTime.Now;
                address.LastModifiedBy = profile.Personal.UserID.ToString();
                address.LastModifiedDate = DateTime.Now;
                long WarehouseID = Warehouseclient.SaveWarehouseMaster(WHDetail, profile.DBConnection._constr);
                address.ReferenceID = WarehouseID;
                long AddressID = Warehouseclient.SaveWarehouseAddress(address, profile.DBConnection._constr);
                UCContactPerson1.FinalSaveContactPerson("Warehouse", long.Parse(hdnwarehouseID.Value));
                UCContactPerson1.ClearContactPerson("Warehouse");
                WebMsgBox.MsgBox.Show("Record Updated successfully");
            }
            else
            {
                long WarehouseID = Warehouseclient.SaveWarehouseMaster(WHDetail, profile.DBConnection._constr);
                address.ReferenceID = WarehouseID;
                hdnwarehouseID.Value = WarehouseID.ToString();
                long AddressID = Warehouseclient.SaveWarehouseAddress(address, profile.DBConnection._constr);
                UCContactPerson1.FinalSaveContactPerson("Warehouse", long.Parse(hdnwarehouseID.Value));
                UCContactPerson1.ClearContactPerson("Warehouse");
                WebMsgBox.MsgBox.Show("Record saved successfully");
            }
            clear();
            Response.Redirect("WarehouseMaster.aspx");
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            FillCompany();
            clear();
            btncustomernext.Visible = true;
            ActiveTab("Add");
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
        }

       
        protected void GvCustomer_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            this.UCToolbar1.ToolbarAccess("Edit");
            Hashtable selectedrec = (Hashtable)grdWarehouseList.SelectedRecords[0];
            btncustomernext.Visible = false;
            hdnwarehouseID.Value = selectedrec["ID"].ToString();
            hdnNewCustomerID.Value = selectedrec["ID"].ToString();
            long reuslt = long.Parse(hdnwarehouseID.Value);
            GetWarehouseDetailByID();
            GetWarehouseAddress();
            FillUserControl(reuslt);
            FillLocationGrid(reuslt);
            hdnstate.Value = "Edit";
            ActiveTab("Edit");
        }



        public void GetWarehouseDetailByID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            V_WMS_GetWarehouseDetails Wdetail = new V_WMS_GetWarehouseDetails();
            Wdetail = Warehouseclient.GetWarehouseDetailByID(long.Parse(hdnwarehouseID.Value), profile.DBConnection._constr);
            FillCompany();
            if (Wdetail.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(Wdetail.CompanyID.ToString()));
            GetCustomerddl(long.Parse(Wdetail.CompanyID.ToString()));
            if (Wdetail.CustomerID != null) ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(Wdetail.CustomerID.ToString()));
            if (Wdetail.Code != null) txtcode.Text = Wdetail.Code.ToString();
            if (Wdetail.WarehouseName != null) txtwarehousename.Text = Wdetail.WarehouseName.ToString();
            if (Wdetail.Type != null) ddltype.SelectedIndex = ddltype.Items.IndexOf(ddltype.Items.FindByText(Wdetail.Type.ToString()));
            if (Wdetail.Description != null) txtdescription.Text = Wdetail.Description.ToString();
            if (Wdetail.Remark != null) txtremark.Text = Wdetail.Remark.ToString();
            hdnNewCustomerID.Value = Wdetail.CustomerID.ToString();
            hdncompanyid.Value = Wdetail.CompanyID.ToString();
            Session.Add("CompanyID", Wdetail.CompanyID.ToString());
            hdnWarehouseName.Value = Wdetail.WarehouseName.ToString();
            Session["WarehouseName"] = Wdetail.WarehouseName.ToString();
            Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + Wdetail.County + "','" + Wdetail.State + "');", true);
            string RActive = Wdetail.Active.ToString();
            if (RActive == "Yes")
            {
                rbtYes.Checked = true;
            }
            else
            {
                rbtNo.Checked = true;
            }
            hdncompanyid.Value = Wdetail.CompanyID.ToString();
            hdncustomerid.Value = Wdetail.CustomerID.ToString();
           // UCAddress1.FillAddressByObjectNameReferenceID("Warehouse", long.Parse(hdnwarehouseID.Value), "Warehouse");
           // UCContactPerson1.FillContactPersonByObjectNameReferenceID("Warehouse", long.Parse(hdnwarehouseID.Value), "Warehouse");
        }

        public void GetWarehouseAddress()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            tAddress getaddr = new tAddress();
            getaddr = Warehouseclient.GetWarehouseAddress(long.Parse(hdnwarehouseID.Value), profile.DBConnection._constr);
            txtCAddress1.Text = getaddr.AddressLine1;
            txtAddress2.Text = getaddr.AddressLine2;
            txtZipCode.Text = getaddr.Zipcode;
            hdnCountry.Value = getaddr.County;
            hdncountryState.Value = getaddr.State;
            txtCity.Text = getaddr.City;
            //Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + getaddr.County + "','" + getaddr.State + "');", true);
        }

        protected void FillUserControl(long resultId)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // UCAddress1.FillAddressByObjectNameReferenceID("Account", resultId, "");
                // UCContactPerson1.FillContactPersonByObjectNameReferenceID("Contact", resultId, "");
                UCContactPerson1.FillContactPersonByObjectNameReferenceID("Warehouse", resultId, "Warehouse");
               // UC_StatutoryDetails1.BindGridStatutoryDetails(resultId, "Customer", profile.Personal.CompanyID);
               // UC_AttachDocument1.FillDocumentByObjectNameReferenceID(resultId, "Customer", "Customer");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Customer Master", "FillUserControl");
            }
            finally { }
        }

        public void clear()
        {
            ResetUserControl();
           // ddlcompany.SelectedItem.Value = "0";
            ddlcustomer.SelectedIndex = -1;
            hdncompanyid.Value = null;
            hdncustomerid.Value = null;
            hdnwarehouseID.Value = null;
            hdnstate.Value = null;
            txtcode.Text = "";
            txtwarehousename.Text = "";
            txtCAddress1.Text = "";
            txtAddress2.Text = "";
            txtZipCode.Text = "";
            txtCity.Text = "";
            hdnNewCustomerID.Value = null;
            ddlCountry.SelectedIndex = -1;
            ddlState.SelectedIndex = -1;
            ddltype.SelectedIndex = -1;
          //  ddltype.SelectedItem.Value = "0";
          //  GVLocation.DataSource = null;
           // GVLocation.DataBind();
        }

        protected void ActiveTab(string state)
        {
            if (state == "Add")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                TabCustomerList.Visible = true;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = false;
                tabLocation.Visible = false;
            }
            else if (state == "Next")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 3;
                TabCustomerList.Visible = true;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = true;
                tabLocation.Visible = true;
            }
            else if (state == "Edit")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                TabCustomerList.Visible = true;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = true;
                tabLocation.Visible = true;
            }
            else
            {
                TabPanelWarehouseList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 0;
                TabCustomerList.Visible = false;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = false;
                tabLocation.Visible = false;
            }
        }

        protected void WarehouseGridBind()
        {
            iWarehouseClient WarehouseClient = new iWarehouseClient();
            try
            {               
                CustomProfile profile = CustomProfile.GetProfile();
                long CompanyID = profile.Personal.CompanyID;
                List<V_WMS_GetWarehouseDetails> warehouselst = new List<V_WMS_GetWarehouseDetails>();
                warehouselst = WarehouseClient.GetWarehouseList(CompanyID, profile.DBConnection._constr).ToList();
                warehouselst = warehouselst.Where(w => w.CompanyID == CompanyID).ToList();
                grdWarehouseList.DataSource = warehouselst;
                grdWarehouseList.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "MainCustomerGridBind");
            }
            finally
            {
                WarehouseClient.Close();
            }
        }


        #region   All  Drop Down Bind Code

        protected void FillCompany()
        {
            ddlcompany.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompany.Items.Insert(0, lst);
        }

        public void GetCustomerddl(long CompanyID)
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

        #region  Location Related Code
        public void FillLocationGrid(long WarehouseID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            DataSet ds = new DataSet();
            ds = Warehouseclient.GetWarehouseLocationByID(WarehouseID, profile.DBConnection._constr);
            GVLocation.DataSource = ds;
           // GVLocation.DataSource = Warehouseclient.GetWarehouseLocation(WarehouseID, profile.DBConnection._constr);
            GVLocation.DataBind();
        }

        protected void GVLocation_OnRebind(object sender, EventArgs e)
        {
            FillLocationGrid(long.Parse(hdnwarehouseID.Value));
        }
        #endregion


    }
}