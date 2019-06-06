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
    public partial class ClientMaster : System.Web.UI.Page
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
                UCFormHeader1.FormHeaderText = "Client Master";

                /*Set Values to UC_AddressInformation1*/

                UCAddress1.ClearAddress("Client");

                /*Set Values to UC_ContactPerson1*/
                UCContactPerson1.ClearContactPerson("Client");

                /*Set Values to UC_Document*/
                UC_AttachDocument1.ClearDocument("Client");

                /*Set Values to UC_StatutoryDetails1*/
               /* UC_StatutoryDetails1.BindGridStatutoryDetails(0, "Customer", profile.Personal.CompanyID);*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "ResetUserControl");
            }
            finally
            { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            UCFormHeader1.FormHeaderText = "Client Master";
            UCToolbar1.ParentPage = this;
            UCContactPerson1.ParentPage = this;

          /*  UC_StatutoryDetails1.ParentPage = this;*/

            if (!IsPostBack)
            {
                clr();
                bindDropdown();
                FillCompany();
                TabCustomerList.Focus();
                MainClientGridBind();
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
               // tabInvoice.Visible = false;
                //tabpaymenthistory.Visible = false;
            }
        }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
             iCustomerClient ServiceAccountMaster = new iCustomerClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tCustomerHead objCutomerHead = new tCustomerHead();
                tOpeningBalance objOpeningBal = new tOpeningBalance();
                mClient ObjClient = new mClient();
                int result;
                long Result1;
                if (checkDuplicate() == "")
                {
                    if (HdnAccountId.Value != "" && HdnAccountId.Value != "0")
                    {
                        //objCutomerHead = ServiceAccountMaster.GetCustomerHeadDetailByCustomerID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                        ObjClient = ServiceAccountMaster.GetClientDetailByClientID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                        objOpeningBal = ServiceAccountMaster.GetTOpeningBalanceDtls(Convert.ToInt64(HdnAccountId.Value),"Client", profile.DBConnection._constr);
                        //objCutomerHead.LastModifiedBy = profile.Personal.UserID.ToString();
                        //objCutomerHead.LastModifiedDate = DateTime.Now;
                    }
                    else
                    {
                       // objCutomerHead.CreatedBy = profile.Personal.UserID.ToString();
                       // objCutomerHead.CreationDate = DateTime.Now;
                    }

                    ObjClient.Name = txt_custname.Text;
                    ObjClient.CompanyID = long.Parse(ddlcompany.SelectedItem.Value);
                    ObjClient.Code = txt_custcode.Text;
                    ObjClient.Sector = Convert.ToInt64(Ddl_Sector.SelectedItem.Value);
                    ObjClient.Creditdays = Convert.ToInt64(txt_CreditDays.Text);
                    ObjClient.TurnOver = decimal.Parse(txt_turnOver.Text);
                    ObjClient.CompType = Convert.ToInt64(Ddl_CompanyType.SelectedItem.Value);
                    if (rbtnYes.Checked == true) ObjClient.Active = "Y";
                    if (rbtnNo.Checked == true) ObjClient.Active = "N";
                    ObjClient.CustomerID = long.Parse(hdncustomerid.Value);
                    ObjClient.Website = TxtWebsite.Text;
                    ObjClient.AccountType = "Client";




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

                    objCutomerHead.ConperID = UCContactPerson1.ContactPersonIDs; */

                    objOpeningBal.FinancialYear = null;
                    if (ddl_FinancialYr.SelectedIndex > 0) objOpeningBal.FinancialYear = ddl_FinancialYr.SelectedValue;

                    objOpeningBal.ObjectName = "Client";

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

                        ObjClient.CreatedBy = profile.Personal.UserID.ToString();
                        ObjClient.CreationDate = DateTime.Now;
                        //result = ServiceAccountMaster.SaveCustomerDetails(objCutomerHead, "AddNew", profile.DBConnection._constr);
                        result = ServiceAccountMaster.SaveClientDetails(ObjClient, "AddNew", profile.DBConnection._constr);

                    }
                    else
                    {
                        //objCutomerHead.LastModifiedBy = profile.Personal.UserID.ToString();
                        //objCutomerHead.LastModifiedDate = DateTime.Now;
                        ObjClient.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjClient.LastModifiedDate = DateTime.Now;
                        result = ServiceAccountMaster.SaveClientDetails(ObjClient, "Edit", profile.DBConnection._constr);

                       // result = ServiceAccountMaster.SaveCustomerDetails(objCutomerHead, "Edit", profile.DBConnection._constr);
                    }

                    objOpeningBal.ReferenceID = result;
                    HdnAccountId.Value = result.ToString();
                    if (HdnOpeningBalId.Value == "0" || HdnOpeningBalId.Value=="")
                    {
                        objOpeningBal.CreatedBy = profile.Personal.UserID.ToString();
                        objOpeningBal.CreatedDate = DateTime.Now;
                        //Result1 = ServiceAccountMaster.SaveOpeningBalance(objOpeningBal, "AddNew", profile.DBConnection._constr);
                        Result1 = ServiceAccountMaster.SaveAccountOpeningBal(objOpeningBal, profile.DBConnection._constr);
                    }
                    else
                    {
                        objOpeningBal.LastModifiedBy = profile.Personal.UserID.ToString();
                        objOpeningBal.LastModifiedDate = DateTime.Now;
                        //Result1 = ServiceAccountMaster.SaveOpeningBalance(objOpeningBal, "Edit", profile.DBConnection._constr);
                        Result1 = ServiceAccountMaster.SaveAccountOpeningBal(objOpeningBal, profile.DBConnection._constr);
                    }
                    HdnOpeningBalId.Value = Result1.ToString();
                    UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.Client, result);
                    UCContactPerson1.FinalSaveContactPerson("Client", result);
                    UC_AttachDocument1.FinalSaveDocument(result);
                 /*   UC_StatutoryDetails1.FinalSaveToStatutoryDetails(Convert.ToInt64(result), "Customer", profile.Personal.CompanyID);*/
                    if (result != 0)
                    {
                        WebMsgBox.MsgBox.Show("Record saved successfully");
                    }
                    clr();
                    MainClientGridBind();
                   // FillUserControl(result);
                    ActiveTab("Load");
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "pageSave");
            }
            finally
            {
                ServiceAccountMaster.Close();
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
        { clr(); ActiveTab(""); }

        public void clr()
        {
            try
            {
                txt_custcode.Text = txt_custname.Text = txt_turnOver.Text = TxtWebsite.Text = txt_CreditDays.Text = txt_OpeningBalance.Text = "";
                Ddl_CompanyType.SelectedIndex = Ddl_Sector.SelectedIndex = ddl_DrCr.SelectedIndex = ddl_FinancialYr.SelectedIndex = -1;
                rbtnNo.Checked = false; rbtnYes.Checked = true;
                ddlcompany.SelectedIndex = 0;
                ddlcustomer.SelectedIndex = -1;
                ResetUserControl();
                HdnAccountId.Value = null;
                grdratecard.DataSource = null;
                grdratecard.DataBind();

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "clr");
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
                UCAddress1.FillAddressByObjectNameReferenceID("Client", resultId, "Client");
                UCContactPerson1.FillContactPersonByObjectNameReferenceID("Client", resultId, "Client");
                UC_AttachDocument1.FillDocumentByObjectNameReferenceID(Convert.ToInt64(resultId), "Client", "Client");
              /*  UC_StatutoryDetails1.BindGridStatutoryDetails(Convert.ToInt64(resultId), "Customer", profile.Personal.CompanyID);*/
               // UCPaymentDetail1.FillOutstandingGrid(resultId.ToString());                                                            // Coomented in BrilliantWMS to check first
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "FillUserControl");
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
                tOpeningBalance objTOpeningBal = new tOpeningBalance();
                Hashtable selectedrec = (Hashtable)GvCustomer.SelectedRecords[0];
                HdnAccountId.Value = selectedrec["ID"].ToString();
                GetClientDetailByClientID();
                GetVendorOpeningBal();
                GetClientrateCard();

                Session.Add("PORRequestID", "Company");
                FillUserControl(Convert.ToInt64(HdnAccountId.Value));
                ActiveTab("Edit");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "GvCustomer_Select");
            }
            finally
            {
            }
        }

        public void GetClientDetailByClientID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCustomerClient ServiceAccountMaster = new iCustomerClient();
            try
            {
                mClient client = new mClient();

                client = ServiceAccountMaster.GetClientDetailByClientID(Convert.ToInt64(HdnAccountId.Value), profile.DBConnection._constr);
                FillCompany();
                if (client.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(client.CompanyID.ToString()));
                hdnCompanyid.Value = client.CompanyID.ToString();
                getCustomer(long.Parse(client.CompanyID.ToString()));
                if (client.CustomerID != null) ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(client.CustomerID.ToString()));
                hdncustomerid.Value = client.CustomerID.ToString();
                txt_custname.Text = client.Name.ToString();
                txt_custcode.Text = client.Code.ToString();
                txt_turnOver.Text = client.TurnOver.ToString();
                TxtWebsite.Text = client.Website.ToString();
                txt_CreditDays.Text = client.Creditdays.ToString();
                Ddl_CompanyType.SelectedValue = null;
                if (client.CompType != null)
                { Ddl_CompanyType.SelectedValue = (client.CompType).ToString(); }
                Ddl_Sector.SelectedValue = (client.Sector).ToString();
                if (client.Active == "Y")
                { rbtnYes.Checked = true; }
                else
                { rbtnNo.Checked = true; }
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "GetClientDetailByClientID");
            }
            finally
            {
                ServiceAccountMaster.Close();
            }

        }

        protected void GetVendorOpeningBal()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCustomerClient ServiceAccountMaster = new iCustomerClient();
            try
            {
                tOpeningBalance objTOpeningBal = new tOpeningBalance();
                objTOpeningBal = ServiceAccountMaster.GetTOpeningBalanceDtls(Convert.ToInt64(HdnAccountId.Value),"Client", profile.DBConnection._constr);
                txt_OpeningBalance.Text = objTOpeningBal.OpeningBalance.ToString();
                if (objTOpeningBal.FinancialYear != null)
                { ddl_FinancialYr.SelectedValue = objTOpeningBal.FinancialYear.ToString(); }
                if (objTOpeningBal.DrCr != null)
                { ddl_DrCr.SelectedValue = objTOpeningBal.DrCr.ToString(); }
                HdnOpeningBalId.Value = objTOpeningBal.ID.ToString();
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "GetVendorOpeningBal");
            }
            finally
            {
                ServiceAccountMaster.Close();
            }
        }

        protected void GetClientrateCard()
        {
            iCompanySetupClient Client = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string Type = "Client";
                grdratecard.DataSource = Client.GetVendorRateByVendorID(Convert.ToInt64(HdnAccountId.Value), Type, profile.DBConnection._constr);
                grdratecard.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "GetClientrateCard");
            }
            finally
            {
                Client.Close();
            }
        }


        protected void MainClientGridBind()
        {
             iCompanySetupClient client = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<V_WMS_GetClientDetails> clintlst = new List<V_WMS_GetClientDetails>();
                clintlst = client.GetClientList(profile.DBConnection._constr).ToList();
                clintlst = clintlst.Where(c => c.CompanyID == profile.Personal.CompanyID).ToList();
                GvCustomer.DataSource = clintlst;
                GvCustomer.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Client Master", "MainClientGridBind");
            }
            finally
            {
                client.Close();
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
    }
}