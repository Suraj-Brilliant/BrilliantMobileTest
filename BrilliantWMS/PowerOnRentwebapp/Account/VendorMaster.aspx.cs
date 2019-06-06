using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using Obout.Grid;
using BrilliantWMS.PopupMessages;
using System.Web.Services;
using BrilliantWMS.AccountSearchService;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.DocumentService;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using System.Data;

namespace BrilliantWMS.Account
{
    public partial class VendorMaster : System.Web.UI.Page
    {
        string ObjectName = "Account";
        long CustHeadID = 0;
        static string sessionID { get; set; }
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
                /*Set Values to UCFormHeader1*/
                UCFormHeader1.FormHeaderText = "Vendor Master";

                /*Set Values to UC_AddressInformation1*/
                UCAddress1.ClearAddress("Vendor");

                /*Set Values to UC_ContactPerson1*/
                UCContactPerson1.ClearContactPerson("Vendor");

                /*Set Values to UC_Document*/
                UC_AttachDocument1.ClearDocument("Vendor");

                /*Set Values to UC_StatutoryDetails1*/
                /* UC_StatutoryDetails1.BindGridStatutoryDetails(0, "Customer", profile.Personal.CompanyID);*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Vendor Master", "ResetUserControl");
            }
            finally
            { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            UCFormHeader1.FormHeaderText = "Account Master";
            UCToolbar1.ParentPage = this;
            UCContactPerson1.ParentPage = this;

            /*  UC_StatutoryDetails1.ParentPage = this;*/

            if (!IsPostBack)
            {
                clr();
                bindDropdown();
                FillCompany();
                GetRateType();
                TabCustomerList.Focus();
                MainCustomerGridBind();
                HdnAccountId.Value = HdnOpeningBalId.Value = null;
                ActiveTab("Load");
            }
            this.UCToolbar1.ToolbarAccess("Accounts");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
            /*  this.UCToolbar1.evClickImport += pageImport;*/
        }

        protected void ActiveTab(string state)
        {

            if (state == "Add" || state == "Edit")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                tabAccountInfo.Visible = true;
                tabAddressInfo.Visible = true;
                tabContactInfo.Visible = true;
                tabAttachedDocumentInfo.Visible = true;
                tabRateCard.Visible = true;
                tabInvoice.Visible = true;
                tabpaymenthistory.Visible = true;
            }
            else
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 0;
                tabAccountInfo.Visible = false;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = false;
                tabAttachedDocumentInfo.Visible = false;
                tabRateCard.Visible = false;
                tabInvoice.Visible = false;
                tabpaymenthistory.Visible = false;
            }
        }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iCustomerClient ServiceAccountMaster = new iCustomerClient();
                mVendor Objvendor = new mVendor();
                tCustomerHead objCutomerHead = new tCustomerHead();
                tOpeningBalance objOpeningBal = new tOpeningBalance();
                int result;
                long Result1;
                if (checkDuplicate() == "")
                {
                    if (HdnAccountId.Value != "" && HdnAccountId.Value != "0")
                    {
                        //objCutomerHead = ServiceAccountMaster.GetCustomerHeadDetailByCustomerID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                        Objvendor = ServiceAccountMaster.GetVendorDetailByVendorID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                        objOpeningBal = ServiceAccountMaster.GetTOpeningBalanceDtls(Convert.ToInt64(HdnAccountId.Value),"Vendor", profile.DBConnection._constr);
                        objCutomerHead.LastModifiedBy = profile.Personal.UserID.ToString();
                        objCutomerHead.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        objCutomerHead.CreatedBy = profile.Personal.UserID.ToString();
                        objCutomerHead.CreationDate = DateTime.Now;
                    }

                    Objvendor.Name = txt_custname.Text;
                    Objvendor.CompanyID = long.Parse(ddlcompany.SelectedItem.Value);
                    Objvendor.Code = txt_custcode.Text;
                    Objvendor.Sector = Convert.ToInt64(Ddl_Sector.SelectedItem.Value);
                    Objvendor.VCType = Convert.ToInt64(ddlvendortype.SelectedItem.Value);
                    Objvendor.Creditdays = Convert.ToInt64(txt_CreditDays.Text);
                    Objvendor.TurnOver = decimal.Parse(txt_turnOver.Text);
                    Objvendor.CompType = Convert.ToInt64(Ddl_CompanyType.SelectedItem.Value);
                    if (rbtnYes.Checked == true) Objvendor.Active = "Y";
                    if (rbtnNo.Checked == true) Objvendor.Active = "N";
                    Objvendor.CustomerID = long.Parse(hdncustomerid.Value);
                    Objvendor.Website = TxtWebsite.Text;
                    Objvendor.AccountType = "Vendor";

                   /* if (Ddl_CompanyType.SelectedIndex > 0) objCutomerHead.CustomerTypeID = Convert.ToInt64(Ddl_CompanyType.SelectedItem.Value);

                    objCutomerHead.CustomerCode = null;
                    if (txt_custcode.Text.ToString().Trim() != "") objCutomerHead.CustomerCode = (txt_custcode.Text).ToString();

                    if (Ddl_Sector.SelectedIndex > 0) objCutomerHead.SectorID = Convert.ToInt64(Ddl_Sector.SelectedItem.Value);

                    objCutomerHead.Name = null;
                    if (txt_custname.Text.ToString().Trim() != "") objCutomerHead.Name = (txt_custname.Text).ToString();

                    objCutomerHead.WebSite = null;
                    if (TxtWebsite.Text.ToString().Trim() != "") objCutomerHead.WebSite = (TxtWebsite.Text).ToString();

                    objCutomerHead.TurnOver = null;
                    if (txt_turnOver.Text.ToString().Trim() != "") objCutomerHead.TurnOver = (txt_turnOver.Text).ToString();


                    objCutomerHead.CreditDays = null;
                    if (txt_CreditDays.Text.ToString().Trim() != "") objCutomerHead.CreditDays = Convert.ToInt32(txt_CreditDays.Text);

                    if (rbtnYes.Checked == true) objCutomerHead.Active = "Y";
                    if (rbtnNo.Checked == true) objCutomerHead.Active = "N";

                    objCutomerHead.BillingAddressID = Convert.ToInt64(UCAddress1.BillingSeq);
                    objCutomerHead.ShippingAddressID = Convert.ToInt64(UCAddress1.ShippingSeq);

                    objCutomerHead.ConperID = UCContactPerson1.ContactPersonIDs;*/

                    objOpeningBal.FinancialYear = null;
                    if (ddl_FinancialYr.SelectedIndex > 0) objOpeningBal.FinancialYear = ddl_FinancialYr.SelectedValue;

                    objOpeningBal.ObjectName = "Vendor";

                    objOpeningBal.OpeningBalance = null;
                    if (txt_OpeningBalance.Text.ToString().Trim() != "") objOpeningBal.OpeningBalance = Convert.ToDecimal((txt_OpeningBalance.Text).ToString());

                    objOpeningBal.DrCr = null;
                    if (ddl_DrCr.SelectedIndex > 0) objOpeningBal.DrCr = ddl_DrCr.SelectedValue;

                    //objOpeningBal.ID = Convert.ToInt64(HdnOpeningBalId.Value.ToString());                   

                    if (HdnAccountId.Value == string.Empty)
                    {
                        //objCutomerHead.CreatedBy = profile.Personal.UserID.ToString();
                        //objCutomerHead.CreationDate = DateTime.Now;
                        //objCutomerHead.CompanyID = profile.Personal.CompanyID;
                        Objvendor.CreatedBy = profile.Personal.UserID.ToString();
                        Objvendor.CreationDate = DateTime.Now;
                       // result = ServiceAccountMaster.SaveCustomerDetails(objCutomerHead, "AddNew", profile.DBConnection._constr);
                        result = ServiceAccountMaster.SaveVendorDetails(Objvendor, "AddNew", profile.DBConnection._constr);

                    }
                    else
                    {
                        //objCutomerHead.LastModifiedBy = profile.Personal.UserID.ToString();
                        //objCutomerHead.LastModifiedDate = DateTime.Now;
                        Objvendor.LastModifiedBy = profile.Personal.UserID.ToString();
                        Objvendor.LastModifiedDate = DateTime.Now;
                        //result = ServiceAccountMaster.SaveCustomerDetails(objCutomerHead, "Edit", profile.DBConnection._constr);
                        result = ServiceAccountMaster.SaveVendorDetails(Objvendor, "Edit", profile.DBConnection._constr);
                    }

                    objOpeningBal.ReferenceID = result;
                    HdnAccountId.Value = result.ToString();
                    if (HdnOpeningBalId.Value == "0" || HdnOpeningBalId.Value == "")
                    {
                        objOpeningBal.CreatedBy = profile.Personal.UserID.ToString();
                        objOpeningBal.CreatedDate = DateTime.Now;
                       // Result1 = ServiceAccountMaster.SaveOpeningBalance(objOpeningBal, "AddNew", profile.DBConnection._constr);
                     Result1 = ServiceAccountMaster.SaveAccountOpeningBal(objOpeningBal, profile.DBConnection._constr);
                        
                    }
                    else
                    {
                        objOpeningBal.LastModifiedBy = profile.Personal.UserID.ToString();
                        objOpeningBal.LastModifiedDate = DateTime.Now;
                       // Result1 = ServiceAccountMaster.SaveOpeningBalance(objOpeningBal, "Edit", profile.DBConnection._constr);
                       Result1 = ServiceAccountMaster.SaveAccountOpeningBal(objOpeningBal, profile.DBConnection._constr);
                    }
                    HdnOpeningBalId.Value = Result1.ToString();
                    UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.Vendor, result);
                    UCContactPerson1.FinalSaveContactPerson("Vendor", result);
                    UC_AttachDocument1.FinalSaveDocument(result);
                    /*   UC_StatutoryDetails1.FinalSaveToStatutoryDetails(Convert.ToInt64(result), "Customer", profile.Personal.CompanyID);*/
                    if (result != 0)
                    {
                        WebMsgBox.MsgBox.Show("Record saved successfully");
                    }
                    clr();
                    MainCustomerGridBind();
                   // FillUserControl(result);
                    ActiveTab("Load");
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        {
            clr();
            HdnAccountId.Value = null;
            Session.Add("PORRequestID", "Company");
            ActiveTab("Add"); //UC_AttachDocument1.FillDocumentByObjectNameReferenceID(0, "Account", "Account");

        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        {
            clr();
            ActiveTab("Add"); 
        }

        public void clr()
        {
            try
            {
                txt_custcode.Text = txt_custname.Text = txt_turnOver.Text = TxtWebsite.Text = txt_CreditDays.Text = txt_OpeningBalance.Text = "";
                Ddl_CompanyType.SelectedIndex = Ddl_Sector.SelectedIndex = ddl_DrCr.SelectedIndex = ddl_FinancialYr.SelectedIndex = -1;
                rbtnNo.Checked = false; rbtnYes.Checked = true;
                ddlcompany.SelectedIndex = 0;
                ddlcustomer.SelectedIndex = -1;
                ddlvendortype.SelectedIndex = 0;
                ResetUserControl();
                HdnAccountId.Value = null;
                grdratecard.DataSource = null;
                grdratecard.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "clr");
            }
            finally
            {
            }
        }

        public void bindDropdown()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iCustomerClient ServiceAccountMaster = new iCustomerClient();
                Ddl_CompanyType.DataSource = ServiceAccountMaster.GetCompanyType(profile.DBConnection._constr);
                Ddl_CompanyType.DataBind();
                ListItem lst1 = new ListItem();
                lst1.Text = "-Select-";
                lst1.Value = "0";
                Ddl_CompanyType.Items.Insert(0, lst1);

                Ddl_Sector.DataSource = ServiceAccountMaster.GetLeadSector(profile.DBConnection._constr);
                Ddl_Sector.DataBind();
                Ddl_Sector.Items.Insert(0, lst1);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "bindDropdown");
            }
            finally
            {
            }
        }

        protected void FillUserControl(long resultId)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UCAddress1.FillAddressByObjectNameReferenceID("Vendor", resultId, "Vendor");
                UCContactPerson1.FillContactPersonByObjectNameReferenceID("Vendor", resultId, "Vendor");
                UC_AttachDocument1.FillDocumentByObjectNameReferenceID(Convert.ToInt64(resultId), "Vendor", "Vendor");
                //UCPaymentDetail1.FillOutstandingGrid(resultId.ToString());
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Vendor Master", "FillUserControl");
            }
            finally
            {
            }
        }

        public string checkDuplicate()
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                //AccountSearchService.AccountSearchService ServiceAccountMaster = new AccountSearchService.AccountSearchService();
                iCustomerClient ServiceAccountMaster = new iCustomerClient();
                if (HdnAccountId.Value == string.Empty)
                {
                    result = ServiceAccountMaster.checkDuplicateRecord(txt_custname.Text, txt_custcode.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txt_custname.Text = "";
                        txt_custcode.Text = "";
                    }
                    txt_custname.Focus();
                }
                else
                {
                    int id = Convert.ToInt32(HdnAccountId.Value);
                    result = ServiceAccountMaster.checkDuplicateRecordEdit(id, txt_custname.Text, txt_custcode.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txt_custname.Text = "";
                        txt_custcode.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void GvCustomer_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iCustomerClient ServiceAccountMaster = new iCustomerClient();
                tCustomerHead objCutomerHead = new tCustomerHead();
                clr();
                Hashtable selectedrec = (Hashtable)GvCustomer.SelectedRecords[0];
                HdnAccountId.Value = selectedrec["ID"].ToString();
                GetVendorDetailByID();
                GetVendorOpeningBal();
                GetVendorRatecard();
                Session.Add("PORRequestID", "Company");
                FillUserControl(Convert.ToInt64(HdnAccountId.Value));
                ActiveTab("Edit");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "GvCustomer_Select");
            }
            finally
            {
            }
        }


        protected void GetVendorDetailByID()
        {
                CustomProfile profile = CustomProfile.GetProfile();
                iCustomerClient ServiceAccountMaster = new iCustomerClient();
                mVendor vendor = new mVendor();

                vendor = ServiceAccountMaster.GetVendorDetailByVendorID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                //objCutomerHead = ServiceAccountMaster.GetCustomerHeadDetailByCustomerID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);


                FillCompany();
                if (vendor.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(vendor.CompanyID.ToString()));
                hdnCompanyid.Value = vendor.CompanyID.ToString();
                getCustomer(long.Parse(vendor.CompanyID.ToString()));
                if (vendor.CustomerID != null) ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(vendor.CustomerID.ToString()));
                hdncustomerid.Value = vendor.CustomerID.ToString();
                if (vendor.VCType != null) ddlvendortype.SelectedIndex = ddlvendortype.Items.IndexOf(ddlvendortype.Items.FindByValue(vendor.VCType.ToString()));

                txt_custname.Text = vendor.Name.ToString();
                txt_custcode.Text = vendor.Code.ToString();
                txt_turnOver.Text = vendor.TurnOver.ToString();
                TxtWebsite.Text = vendor.Website.ToString();
                txt_CreditDays.Text = vendor.Creditdays.ToString();
                Ddl_CompanyType.SelectedValue = null;
                if (vendor.CompType != null)
                { Ddl_CompanyType.SelectedValue = (vendor.CompType).ToString(); }
                Ddl_Sector.SelectedValue = (vendor.Sector).ToString();
                if (vendor.Active == "Y")
                { rbtnYes.Checked = true; }
                else
                { rbtnNo.Checked = true; }
        }

        protected void GetVendorOpeningBal()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCustomerClient ServiceAccountMaster = new iCustomerClient();
            tOpeningBalance objTOpeningBal = new tOpeningBalance();
            objTOpeningBal = ServiceAccountMaster.GetTOpeningBalanceDtls(Convert.ToInt64(HdnAccountId.Value), "Vendor", profile.DBConnection._constr);
            txt_OpeningBalance.Text = objTOpeningBal.OpeningBalance.ToString();
            if (objTOpeningBal.FinancialYear != null)
            { ddl_FinancialYr.SelectedValue = objTOpeningBal.FinancialYear.ToString(); }
            if (objTOpeningBal.DrCr != null)
            { ddl_DrCr.SelectedValue = objTOpeningBal.DrCr.ToString(); }
            HdnOpeningBalId.Value = objTOpeningBal.ID.ToString();
        }

        protected void GetVendorRatecard()
        {
            iCompanySetupClient vendor = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string Type = "Vendor";
                grdratecard.DataSource = vendor.GetVendorRateByVendorID(Convert.ToInt64(HdnAccountId.Value), Type, profile.DBConnection._constr);
                grdratecard.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Vendor Master", "GetVendorRatecard");
            }
            finally
            {
                vendor.Close();
            }
        }

        protected void MainCustomerGridBind()
        {
            iCompanySetupClient vendor = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<V_WMS_GetVendorDetails> vend = new List<V_WMS_GetVendorDetails>();
                vend = vendor.GetVendorList(profile.DBConnection._constr).ToList();
                vend = vend.Where(v => v.CompanyID == profile.Personal.CompanyID).ToList();
                GvCustomer.DataSource = vend;
                 GvCustomer.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "MainCustomerGridBind");
            }
            finally
            {
                vendor.Close();
            }

        }

        protected void pageImport(Object sender, ToolbarService.iUCToolbarClient e)
        {
            try
            {
                Response.Redirect("../Import/Import.aspx?Objectname=" + "Customer");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "pageImport");
            }
            finally
            {
            }
        }

        [WebMethod]
        public static string PMDeleteDocument(string Sequence)
        {
            iUC_AttachDocumentClient DocumentClient = new iUC_AttachDocumentClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DocumentClient.DeleteDocumentFormTemp(Convert.ToInt64(Sequence), sessionID, profile.Personal.UserID.ToString(), "Account", profile.DBConnection._constr);
                return "true";
            }
            catch (Exception ex) { return "false"; }
            finally { DocumentClient.Close(); }
        }

        /* [WebMethod]
         public static void PMMoveAddressToArchive(string Ids, string IsArchive)
         {
             WebClientElegantCRM.Address.UCAddress ucAddress = new Address.UCAddress();
             ucAddress.MoveAddressToArchive(Ids, IsArchive, "Account");
         }*/


        // Get Company and Customer Code
        protected void FillCompany()
        {
            ddlcompany.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompany.Items.Insert(0, lst);
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

        protected void GetRateType()
        {
            ddlvendortype.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlvendortype.DataSource = CompanyClient.GetRateTypeDropdown("Vendor", profile.DBConnection._constr);
            ddlvendortype.DataTextField = "Value";
            ddlvendortype.DataValueField = "Id";
            ddlvendortype.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlvendortype.Items.Insert(0, lst);
            CompanyClient.Close();
        }
    }
}