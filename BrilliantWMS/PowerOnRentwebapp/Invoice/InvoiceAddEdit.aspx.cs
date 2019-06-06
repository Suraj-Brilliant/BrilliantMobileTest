using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/*using WebClientElegantCRM.LeadDetailsService;
using WebClientElegantCRM.ServiceOpportunity;
using WebClientElegantCRM.ServiceQuotation;
using WebClientElegantCRM.SalesOrderService;
using WebClientElegantCRM.InvoiceService;*/
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Collections;
/*using WebClientElegantCRM.Product;
using WebClientElegantCRM.LeadService;
using WebClientElegantCRM.MasterPage;
using WebClientElegantCRM.PopupMessages;*/
using BrilliantWMS.Login;
/*using WebClientElegantCRM.DocumentService;
using WebClientElegantCRM.TitleMasterService;*/
using System.Net;

namespace BrilliantWMS.Invoice
{
    public partial class InvoiceAddEdit : System.Web.UI.Page
    {
        static string ConvertObjectFrom = "Invoice";
        static string CurrentObjectName = "Invoice";
        long LeadID = 0;
        long OpportunityID = 0;
        long QuotationID = 0;
        long CustomerHeadID = 0;
        long SalesOrderID = 0;
        static long InvoiceID = 0;
        long ReferenceID = 0;
        /*static UC_AddToCart ucAddToCart = new UC_AddToCart();*/
        static string sessionID { get; set; }
        static Page ParentPage { get; set; }

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            UCToolbar1.ParentPage = this;
            UCContactPerson1.ParentPage = this;
            /* UC_AddToCart1.ParentPage = this;*/

            if (Request.QueryString["LeadID"] != null) { ReferenceID = Convert.ToInt32(Request.QueryString["LeadID"]); ConvertObjectFrom = "Lead"; CurrentObjectName = "Invoice"; }
            if (Request.QueryString["OpportunityID"] != null) { ReferenceID = Convert.ToInt32(Request.QueryString["OpportunityID"]); ConvertObjectFrom = "Opportunity"; CurrentObjectName = "Invoice"; }
            if (Request.QueryString["QuotationID"] != null) { ReferenceID = Convert.ToInt32(Request.QueryString["QuotationID"]); ConvertObjectFrom = "Quotation"; CurrentObjectName = "Invoice"; }
            if (Request.QueryString["SalesOrderID"] != null) { ReferenceID = Convert.ToInt32(Request.QueryString["SalesOrderID"]); ConvertObjectFrom = "SalesOrder"; CurrentObjectName = "Invoice"; }
            if (Request.QueryString["InvoiceID"] != null) { ReferenceID = Convert.ToInt32(Request.QueryString["InvoiceID"]); ConvertObjectFrom = "Invoice"; CurrentObjectName = "Invoice"; }
            SetValueToUserControl();

            //Payment Booking & schedule
            UC_PaymentBookingDate.DateIsRequired(true, "PaymentBooking", "Please select  Date");

            UC_PayemntDate.DateIsRequired(true, "PaymentSchedule", "Please select Date");
            //UC_PayemntDate.startdate(DateTime.Now);

            UCAlertDate.DateIsRequired(false, "x", "");
            //UCAlertDate.startdate(DateTime.Now);

            TextBox txtPayemntDate = (TextBox)UC_PayemntDate.FindControl("txtDate");
            TextBox txtAlertDate = (TextBox)UCAlertDate.FindControl("txtDate");

            txtPayemntDate.Attributes.Add("onchange", "validateDate('" + txtAlertDate.ClientID + "','" + txtPayemntDate.ClientID + "','End','Payment Date shoule not be less than Alert Date')");
            txtAlertDate.Attributes.Add("onchange", "validateDate('" + txtAlertDate.ClientID + "','" + txtPayemntDate.ClientID + "','Start','Alert Date should not be greater than payment Date')");

            if (!IsPostBack)
            {

                clr();
                if (ReferenceID != 0)
                {
                    BindData(Convert.ToInt32(ReferenceID));
                    if (ConvertObjectFrom == "Invoice" && Request.QueryString["CallBack"] == null) Page.ClientScript.RegisterStartupScript(GetType(), "MyKey", "changemode(true,'tabletest');", true);

                    if (ConvertObjectFrom == "Invoice" && Request.QueryString["CallBack"] != null)
                    {
                        if (Request.QueryString["CallBack"] == "Update") WebMsgBox.MsgBox.Show("Invoice updated successfully");
                        else if (Request.QueryString["CallBack"] == "Save") WebMsgBox.MsgBox.Show("Invoice saved successfully");

                        //add by Suresh
                        //HttpWebRequest

                    }
                }
                if (ReferenceID == 0) { bindDropdown(); }
            }

            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
            /* this.UCToolbar1.evClickPrint += pagePrint;*/
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        protected void pagePrint(Object sender, ToolbarService.iUCToolbarClient e)
        {
            try
            {
                if (Request.QueryString.Count > 0)
                {
                    if (ReferenceID != 0)
                    {
                        UCToolbar1.PrintReport("Invoice", ReferenceID.ToString());
                    }
                    else
                    {
                        WebMsgBox.MsgBox.Show("Please Select At Least One Record");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SalesOrderAddEdit", "pagePrint");
            }
            finally
            {
            }
        }
        #endregion

        #region ToolbarCode [Page Methods]

        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        {
            clr();
            bindDropdown();

        }

        protected void pageImport(Object sender, ToolbarService.iUCToolbarClient e)
        { Response.Redirect("../Import/Import.aspx?Objectname=" + "Lead"); }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {

            try
            {
                HiddenField hdnItemCount = (HiddenField)UC_AddToCart1.FindControl("hdnItemCount");
                if (hdnItemCount != null)
                {
                    if (hdnItemCount.Value == "") hdnItemCount.Value = "0";
                    if (Convert.ToInt64(hdnItemCount.Value) <= 0)
                    { WebMsgBox.MsgBox.Show("Atleast one product add into Cart"); }

                    else
                    {
                        CustomProfile profile = CustomProfile.GetProfile();
                        /* iLeadDetailsClient LeadClient = new iLeadDetailsClient();
                         LeadDetailsService.tCustomerHead objCustomerHead = new LeadDetailsService.tCustomerHead();
                         objCustomerHead = LeadClient.GetCustomerHeadDetailByCustname(txtAccountName.Text, profile.DBConnection._constr);*/
                        if ((hdnCustomerID.Value == null || hdnCustomerID.Value == ""))/* && (objCustomerHead != null))*/
                        {
                            WebMsgBox.MsgBox.Show("Customer with the same name already exist. Please select from customer list...");
                            txtAccountName.Text = "";

                        }
                        else
                        {
                            /*InvoiceService.iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
                            if (objCustomerHead == null) objCustomerHead = new LeadDetailsService.tCustomerHead();
                            if (hdnCustomerID.Value != "" && hdnCustomerID.Value != "0")
                            {
                                objCustomerHead = LeadClient.GetCustomerHeadDetailByCustomerID(Convert.ToInt64(hdnCustomerID.Value), profile.DBConnection._constr);
                                objCustomerHead.LastModifiedBy = profile.Personal.UserID.ToString();
                                objCustomerHead.LastModifiedDate = DateTime.Now;
                            }
                            else
                            {
                                objCustomerHead.DisplayName = txtAccountName.Text.ToString().Trim();
                                objCustomerHead.TurnOver = "0";
                                objCustomerHead.CreditDays = 0;

                                objCustomerHead.CreatedBy = profile.Personal.UserID.ToString();
                                objCustomerHead.CreationDate = DateTime.Now;
                            }

                            objCustomerHead.Name = txtAccountName.Text.ToString().Trim();
                            objCustomerHead.WebSite = txtWebSite.Text.ToString().Trim();
                            objCustomerHead.SectorID = Convert.ToInt64(ddlSector.SelectedItem.Value);
                            objCustomerHead.CustomerTypeID = null;
                            if (ddlCompanyType.SelectedIndex > 0) objCustomerHead.CustomerTypeID = Convert.ToInt64(ddlCompanyType.SelectedItem.Value);
                            objCustomerHead.Active = "Y";
                            objCustomerHead.CompanyID = profile.Personal.CompanyID;
                            //if (ddlCompanyType.SelectedIndex > 0) objCustomerHead.CustomerTypeID = Convert.ToInt64(ddlCompanyType.SelectedItem.Value);

                            if (hdnCustomerID.Value != "" && hdnCustomerID.Value != "0") { LeadClient.SaveCustomerDetails(objCustomerHead, "Edit", profile.DBConnection._constr); }
                            else { hdnCustomerID.Value = LeadClient.SaveCustomerDetails(objCustomerHead, "AddNew", profile.DBConnection._constr).ToString(); }
                            /*End*/


                            /*Insert or Update tOpportunityHead*/
                            /* InvoiceService.tInvoiceHead objInvoiceHead = new InvoiceService.tInvoiceHead();*/



                            /*  if (txtInvoiceNo.Text != "0") { objInvoiceHead = InvoiceClient.GetInvoiceDetailByID(Convert.ToInt32(txtInvoiceNo.Text), profile.DBConnection._constr); } // objSalesOrderHead.ReferenceID = Convert.ToInt64(txtSalesOrderNo.Text); }
                              else { objInvoiceHead.ObjectName = ConvertObjectFrom; objInvoiceHead.ReferenceID = ReferenceID != 0 ? ReferenceID : 0; }

                              objInvoiceHead.Title = null;
                              if (txtTitle.Text.ToString().Trim() != "") objInvoiceHead.Title = txtTitle.Text.ToString().Trim();

                              objInvoiceHead.CampaignID = null;
                              if (ddlCampaign.SelectedIndex > 0) { objInvoiceHead.CampaignID = Convert.ToInt64(ddlCampaign.SelectedItem.Value); }

                              objInvoiceHead.InvoiceNo = txtUserInvoiceNo.Text != null ? objInvoiceHead.InvoiceNo = txtUserInvoiceNo.Text : objInvoiceHead.InvoiceNo = "0";

                              objInvoiceHead.InvoiceDate = Convert.ToDateTime(UC_InvoiceDate.Date);

                              objInvoiceHead.LeadSourceID = 0;
                              if (ddlLeadSource.SelectedIndex > 0) { objInvoiceHead.LeadSourceID = Convert.ToInt64(ddlLeadSource.SelectedItem.Value); }

                              objInvoiceHead.Executive = null;
                              if (ddlExecutive.SelectedIndex > 0) { objInvoiceHead.Executive = ddlExecutive.SelectedItem.Value; }

                              objInvoiceHead.InvoiceStatus = ddlInvoiceStatus.SelectedItem.Text;

                              objInvoiceHead.InvoiceType = ddlInvoiceType.SelectedItem.Text;

                              if (UC_CustomerPODate.Date != null) { objInvoiceHead.CustomerPODate = Convert.ToDateTime(UC_CustomerPODate.Date); }

                              objInvoiceHead.CustomerPONo = txtCustomerPONo.Text;

                              if (txtParentInvoiceNo.Text.ToString().Trim() != "") { objInvoiceHead.ParentInvoiceID = Convert.ToInt64(txtParentInvoiceNo.Text); }

                              if (txtlDispatchThrough.Text.Trim() != "") { objInvoiceHead.DispatchThrough = txtlDispatchThrough.Text.Trim(); }

                              if (UC_ExpDispatchDate.Date != null) { objInvoiceHead.ExpDispatchDate = Convert.ToDateTime(UC_ExpDispatchDate.Date); }*/

                            //objLeadHead. = null; /*Approved By*/
                            /*  objInvoiceHead.Active = "Y";

                              objInvoiceHead.Remark = null;
                              if (txtRemark.Text.ToString().Trim() != "") objInvoiceHead.Remark = txtRemark.Text.ToString().Trim();

                              objInvoiceHead.CustomerHeadID = Convert.ToInt64(hdnCustomerID.Value);

                          
                              UC_AddToCart1.setFooterValuesToProperty();
                              objInvoiceHead.SubTotal = UC_AddToCart1.SubTotal;
                              objInvoiceHead.ProductLevelTotalDiscount = 0;
                              objInvoiceHead.DiscountOnSubTotal = UC_AddToCart1.DiscountOnSubTotal;
                              objInvoiceHead.DiscountOnSubTotalPercent = Convert.ToBoolean(UC_AddToCart1.DiscountOnSubTotalPercent.ToString());
                              objInvoiceHead.TotalDiscount = UC_AddToCart1.TotalDiscount;
                              objInvoiceHead.TotalAfterDiscount = UC_AddToCart1.TotalAfterDiscount;
                              objInvoiceHead.ProductLevelTotalTax = 0;
                              objInvoiceHead.TaxOnTotalAfterDiscount = 0;
                              objInvoiceHead.TotalTax = UC_AddToCart1.TotalTax;
                              objInvoiceHead.ShippingCharges = UC_AddToCart1.ShippingCharges;
                              objInvoiceHead.OtherChargesDescription = UC_AddToCart1.OtherChargesDescription;
                              objInvoiceHead.OtherCharges = UC_AddToCart1.OtherCharges;
                              objInvoiceHead.TotalAmount = UC_AddToCart1.TotalAmount;

                              objInvoiceHead.CompanyID = profile.Personal.CompanyID;
                              objInvoiceHead.BillingAddressID = Convert.ToInt64(UCAddress1.BillingSeq);
                              objInvoiceHead.ShippingAddressID = Convert.ToInt64(UCAddress1.ShippingSeq);

                              objInvoiceHead.ConperID = UCContactPerson1.ContactPersonIDs; */

                            /*End*/
                            string state = "";
                            if (txtInvoiceNo.Text == "0")
                            {
                                state = "AddNew";
                                /* objInvoiceHead.CreatedBy = profile.Personal.UserID.ToString();
                                 objInvoiceHead.CreationDate = DateTime.Now;
                                 ReferenceID = InvoiceClient.SaveInvoiceDetails(objInvoiceHead, "AddNew", profile.DBConnection._constr);*/

                            }
                            else
                            {
                                state = "Edit";
                                /* objInvoiceHead.ID = Convert.ToInt64(txtInvoiceNo.Text);
                                 objInvoiceHead.LastModifiedBy = profile.Personal.UserID.ToString();
                                 objInvoiceHead.LastModifiedDate = DateTime.Now;
                                 ReferenceID = InvoiceClient.SaveInvoiceDetails(objInvoiceHead, "Edit", profile.DBConnection._constr); */
                            }
                            txtInvoiceNo.Text = ReferenceID.ToString();
                            /*End*/

                            /*User Control Final Save*/
                            if (ReferenceID > 0)
                            {
                               /* InvoiceClient.FinalSaveOfPaymentBooking(ReferenceID, Session.SessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                                UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.Account, objInvoiceHead.CustomerHeadID);
                                UCContactPerson1.FinalSaveContactPerson("Account", objInvoiceHead.CustomerHeadID);
                                UC_AddToCart1.FinalSaveAddToCartProductDetail(Convert.ToInt64(ReferenceID));*/
                                UC_AttachDocument1.FinalSaveDocument(Convert.ToInt64(ReferenceID));
                                UC_TermsAndCondition1.FinalSaveTermConditionDetail(Convert.ToInt64(ReferenceID));//Uc_TermCondition                               
                                if (state == "AddNew")
                                {
                                    WebMsgBox.MsgBox.Show("Invoice saved sucessfully");
                                    Response.Redirect("../Invoice/InvoiceAddEdit.aspx?CallBack=Save&InvoiceID=" + ReferenceID, false);
                                }
                                if (state == "Edit")
                                {
                                    WebMsgBox.MsgBox.Show("Invoice updated sucessfully");
                                    Response.Redirect("../Invoice/InvoiceAddEdit.aspx?CallBack=Update&InvoiceID=" + ReferenceID, false);
                                }
                            }
                            /*End*/
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        {
            // clr();
            if (txtInvoiceNo.Text == "0")
            { clr(); }
            else
            { BindData(Convert.ToInt32(txtInvoiceNo.Text)); }
        }

        #endregion

        #region User Control Code

        protected void SetValueToUserControl()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UCFormHeader1.FormHeaderText = "Invoice";

            UC_TermsAndCondition1.groupName = "Invoice";
            UC_TermsAndCondition1.ParentPage = this;

            /*Set Values to UCLeadDate*/
            UC_InvoiceDate.DateIsRequired(true, "Save", "Select Invoice Date");


            /*Set Values to UC_CustomerPODate*/
            UC_CustomerPODate.DateIsRequired(false, "x", "");
            //UC_CustomerPODate.startdate(DateTime.Now);
            //UCExpOrderDate.Date = null;
            UC_ExpDispatchDate.DateIsRequired(false, "None", "");
            sessionID = Session.SessionID;

        }

        public void FillUserControl(int id)
        {
            try
            {
                if (hdnCustomerID.Value == "") { hdnCustomerID.Value = "0"; }

                UCAddress1.FillAddressByObjectNameReferenceID(ConvertObjectFrom, id, CurrentObjectName);
                UCContactPerson1.FillContactPersonByObjectNameReferenceID(ConvertObjectFrom, id, CurrentObjectName);
               /* UC_AddToCart1.GetAddToCartListByReferenceIDObjectName(id);
                UC_AttachDocument1.FillDocumentByObjectNameReferenceID(Convert.ToInt64(id), ConvertObjectFrom, CurrentObjectName);
                UC_ActionHistory1.GetActionHistorydetails(ConvertObjectFrom, id);
                UC_TermsAndCondition1.GetTermsConditionListByReferenceIDObjectName(Convert.ToInt64(id)); */

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "FillUserControl");
            }
            finally
            {
            }
        }

        public void ResetUserControl() /*ResetUserControl at once when lead first time load*/
        {
            try
            {
              /*  InvoiceService.iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
                CustomProfile profile = CustomProfile.GetProfile();
                UCAddress1.ClearAddress(CurrentObjectName);
                UCContactPerson1.ClearContactPerson(CurrentObjectName);
                UC_AddToCart1.ResetAddToCart(ConvertObjectFrom, ReferenceID, profile.Personal.UserID.ToString(), Session.SessionID, CurrentObjectName);
                UC_AttachDocument1.ClearDocument(CurrentObjectName);
                UC_TermsAndCondition1.ResetUCTermC(ConvertObjectFrom, ReferenceID, profile.Personal.UserID.ToString(), sessionID, CurrentObjectName);//Uc_TermCondition                 
                InvoiceClient.ClearTempDataOfPaymentBooking(sessionID, CurrentObjectName + "PB", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                InvoiceClient.ClearTempDataOfPaymentSchedule(sessionID, CurrentObjectName + "PS", profile.Personal.UserID.ToString(), profile.DBConnection._constr); */
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "ResetUserControl");
            }
            finally
            {
            }
        }

        [WebMethod]
        public static void UpdateUcTermC(object order)
        {
            CustomProfile profile = CustomProfile.GetProfile();
           /* WebClientElegantCRM.Company.UC_TermsAndCondition ucTermC = new WebClientElegantCRM.Company.UC_TermsAndCondition();
            ucTermC.ParentPage = ParentPage;
            ucTermC.UpdateOrder(sessionID, ConvertObjectFrom, profile.Personal.UserID.ToString(), order);*/
        }

        /// <summary>
        /// To Update Cart Changes
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
       /* [WebMethod]
        public static string[] UpdateOrder(object order)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            WebClientElegantCRM.Product.UC_AddToCart ucAddToCart = new WebClientElegantCRM.Product.UC_AddToCart();
            ucAddToCart.ParentPage = ParentPage;
            return ucAddToCart.UpdateOrder(sessionID, "Invoice", profile.Personal.UserID.ToString(), order); 
            }*/

        /// <summary>
        /// To Get Cart Footer Total
        /// </summary>
        /// <param name="discountonsubtotal"></param>
        /// <param name="discountonsubtotalchk"></param>
        /// <returns></returns>
       /* [WebMethod]
        public static string[] webGetFooterTotal(decimal discountonsubtotal, string discountonsubtotalchk)
        {
            CustomProfile profile = CustomProfile.GetProfile();
           /* WebClientElegantCRM.Product.UC_AddToCart ucAddToCart = new WebClientElegantCRM.Product.UC_AddToCart();
            ucAddToCart.ParentPage = ParentPage;
            return ucAddToCart.GetFooterTotal(sessionID, "Invoice", discountonsubtotal, discountonsubtotalchk); 
        }*/

       /* [WebMethod]
        public static WebClientElegantCRM.AccountSearchService.tCustomerHead webGetCustomerHeadDetailByCustomerID(long customerID)
        {
            WebClientElegantCRM.Account.UC_AccountSearch ucAccountSearch = new Account.UC_AccountSearch();
            WebClientElegantCRM.AccountSearchService.tCustomerHead customer = new AccountSearchService.tCustomerHead();
            customer = ucAccountSearch.GetCustomerHeadDetailByCustomerID(customerID);

            WebClientElegantCRM.Address.UCAddress ucAddress = new Address.UCAddress();
            ucAddress.FillAddressByObjectNameReferenceID("Account", customerID, CurrentObjectName);

            WebClientElegantCRM.ContactPerson.UcContactPerson ucContactPerson = new ContactPerson.UcContactPerson();
            ucContactPerson.FillContactPersonByObjectNameReferenceID("Account", customerID, CurrentObjectName);

            return customer;
        }*/

       /* [WebMethod]
        public static mTitle GetTitleRecordByTitleID(long TitleId)
        {
            mTitle titleService = new mTitle();
            WebClientElegantCRM.Product.Uc_Title ucTitle = new Product.Uc_Title();
            titleService = ucTitle.GetTitleRecordByTitleID(TitleId);
            return titleService;
        }

        [WebMethod]
        public static void PMMoveAddressToArchive(string Ids, string IsArchive)
        {
            WebClientElegantCRM.Address.UCAddress ucAddress = new Address.UCAddress();
            ucAddress.MoveAddressToArchive(Ids, IsArchive, CurrentObjectName);
        }

        [WebMethod]
        public static void PMMoveContactPersonToArchive(string Ids, string IsArchive)
        {
            WebClientElegantCRM.ContactPerson.UcContactPerson ucContactPerson = new ContactPerson.UcContactPerson();
            ucContactPerson.MoveToArchiveContactPerson(Ids, IsArchive, CurrentObjectName);
        }*/
        #endregion

        protected void fillCustomerDetails(long CustomerID)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               /* iLeadDetailsClient LeadClient = new iLeadDetailsClient();
                LeadDetailsService.tCustomerHead getCustomer = new LeadDetailsService.tCustomerHead();
                getCustomer = LeadClient.GetCustomerHeadDetailByCustomerID(CustomerID, profile.DBConnection._constr);
                LeadClient.Close();
                if (getCustomer != null)
                {
                    hdnCustomerID.Value = CustomerID.ToString();
                    txtAccountName.Text = getCustomer.Name.ToString();
                    txtWebSite.Text = getCustomer.WebSite.ToString();
                    ddlSector.SelectedIndex = ddlSector.Items.IndexOf(ddlSector.Items.FindByValue(getCustomer.SectorID.ToString()));
                    if (getCustomer.CustomerTypeID != null) ddlCompanyType.SelectedIndex = ddlCompanyType.Items.IndexOf(ddlCompanyType.Items.FindByValue(getCustomer.CustomerTypeID.ToString()));
                }*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "fillCustomerDetails");
            }
            finally
            {
            }
        }

        protected void ddlInvoiceStatus_Selectedindexchange(Object sender, EventArgs e)
        {

        }

        public void BindData(int RefID)
        {
           /* try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bindDropdown();
                if (ConvertObjectFrom == "Lead")
                {
                    iLeadDetailsClient LeadClient = new iLeadDetailsClient();
                    LeadDetailsService.tLeadHead data = new LeadDetailsService.tLeadHead();
                    data = LeadClient.GetLeadDetailByID(RefID, profile.DBConnection._constr);
                    LeadClient.Close();
                    if (data != null)
                    {
                        if (data.Title != null) txtTitle.Text = data.Title.ToString().Trim();
                        else { txtTitle.Text = null; }
                        if (data.CampaignID != null) ddlCampaign.SelectedIndex = ddlCampaign.Items.IndexOf(ddlCampaign.Items.FindByValue(data.CampaignID.Value.ToString()));
                        txtInvoiceNo.Text = "0";
                        txtUserInvoiceNo.Text = "0";
                        UC_InvoiceDate.Date = data.LeadDate;
                        if (data.LeadSourceID != null) ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(data.LeadSourceID.Value.ToString()));
                        if (data.Executive != null) ddlExecutive.SelectedIndex = ddlExecutive.Items.IndexOf(ddlExecutive.Items.FindByValue(data.Executive.ToString()));
                        fillCustomerDetails(data.CustomerHeadID);
                        
                        if (data.Remark != null) { txtRemark.Text = data.Remark; }
                        if (data.ConperID != null) UCContactPerson1.ContactPersonIDs = data.ConperID;
                        if (data.BillingAddressID != null) UCAddress1.BillingSeq = data.BillingAddressID.ToString();
                        if (data.ShippingAddressID != null) UCAddress1.ShippingSeq = data.ShippingAddressID.ToString();
                       
                        UC_AddToCart1.SubTotal = Convert.ToDecimal(data.SubTotal);
                        
                        UC_AddToCart1.DiscountOnSubTotal = Convert.ToDecimal(data.DiscountOnSubTotal);
                        UC_AddToCart1.DiscountOnSubTotalPercent = data.DiscountOnSubTotalPercent.Value.ToString().ToLower();
                        UC_AddToCart1.TotalDiscount = Convert.ToDecimal(data.TotalDiscount);
                        UC_AddToCart1.TotalAfterDiscount = Convert.ToDecimal(data.TotalAfterDiscount);
                       
                        UC_AddToCart1.TotalTax = Convert.ToDecimal(data.TotalTax);
                        UC_AddToCart1.ShippingCharges = Convert.ToDecimal(data.ShippingCharges);
                        UC_AddToCart1.OtherChargesDescription = data.OtherChargesDescription;
                        UC_AddToCart1.OtherCharges = Convert.ToDecimal(data.OtherCharges);
                        UC_AddToCart1.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                        if (Convert.ToDecimal(data.SubTotal) > 0) hdnCartCount.Text = "1";
                    }

                }
                if (ConvertObjectFrom == "Opportunity")
                {
                    ServiceOpportunity.iOpportunityDetailsClient OpportunityClient = new ServiceOpportunity.iOpportunityDetailsClient();
                    ServiceOpportunity.tOpportunityHead data = new ServiceOpportunity.tOpportunityHead();
                    data = OpportunityClient.GetOpportunityDetailByID(RefID, profile.DBConnection._constr);
                    OpportunityClient.Close();
                    if (data != null)
                    {
                        if (data.Title != null) txtTitle.Text = data.Title.ToString().Trim();
                        else { txtTitle.Text = null; }
                        if (data.CampaignID != null) ddlCampaign.SelectedIndex = ddlCampaign.Items.IndexOf(ddlCampaign.Items.FindByValue(data.CampaignID.Value.ToString()));
                        txtInvoiceNo.Text = "0";
                        txtUserInvoiceNo.Text = "0";
                        UC_InvoiceDate.Date = data.OpportunityDate;
                        if (data.LeadSourceID != null) ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(data.LeadSourceID.Value.ToString()));
                        if (data.Executive != null) ddlExecutive.SelectedIndex = ddlExecutive.Items.IndexOf(ddlExecutive.Items.FindByValue(data.Executive.ToString()));
                        fillCustomerDetails(data.CustomerHeadID);
                        //ddlQuotationStatus.SelectedValue = data.OpportunityStatus;
                        //ddlApprovedBy.SelectedValue = "";

                        if (data.Remark != null) { txtRemark.Text = data.Remark; }
                        if (data.BillingAddressID != null) UCAddress1.BillingSeq = data.BillingAddressID.ToString();
                        if (data.ShippingAddressID != null) UCAddress1.ShippingSeq = data.ShippingAddressID.ToString();
                        if (data.ConperID != null) UCContactPerson1.ContactPersonIDs = data.ConperID;
                      
                        UC_AddToCart1.SubTotal = Convert.ToDecimal(data.SubTotal);
                       
                        UC_AddToCart1.DiscountOnSubTotal = Convert.ToDecimal(data.DiscountOnSubTotal);
                        UC_AddToCart1.DiscountOnSubTotalPercent = data.DiscountOnSubTotalPercent.Value.ToString().ToLower();
                        UC_AddToCart1.TotalDiscount = Convert.ToDecimal(data.TotalDiscount);
                        UC_AddToCart1.TotalAfterDiscount = Convert.ToDecimal(data.TotalAfterDiscount);
                       
                        UC_AddToCart1.TotalTax = Convert.ToDecimal(data.TotalTax);
                        UC_AddToCart1.ShippingCharges = Convert.ToDecimal(data.ShippingCharges);
                        UC_AddToCart1.OtherChargesDescription = data.OtherChargesDescription;
                        UC_AddToCart1.OtherCharges = Convert.ToDecimal(data.OtherCharges);
                        UC_AddToCart1.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                        if (Convert.ToDecimal(data.SubTotal) > 0) hdnCartCount.Text = "1";
                    }
                }
                if (ConvertObjectFrom == "Quotation")
                {
                    ServiceQuotation.iQuotationDetailsClient QuotationClient = new ServiceQuotation.iQuotationDetailsClient();
                    ServiceQuotation.tQuotationHead data = new ServiceQuotation.tQuotationHead();
                    data = QuotationClient.GetQuotationDetailByID(RefID, profile.DBConnection._constr);
                    QuotationClient.Close();
                    if (data != null)
                    {
                        if (data.Title != null) txtTitle.Text = data.Title.ToString().Trim();
                        else { txtTitle.Text = null; }
                        if (data.CampaignID != null) ddlCampaign.SelectedIndex = ddlCampaign.Items.IndexOf(ddlCampaign.Items.FindByValue(data.CampaignID.Value.ToString()));
                        txtInvoiceNo.Text = "0";
                        txtUserInvoiceNo.Text = "0";
                        UC_InvoiceDate.Date = data.QuotationDate;
                        if (data.LeadSourceID != null) ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(data.LeadSourceID.ToString()));
                        if (data.Executive != null) ddlExecutive.SelectedIndex = ddlExecutive.Items.IndexOf(ddlExecutive.Items.FindByValue(data.Executive.ToString()));
                        fillCustomerDetails(data.CustomerHeadID);
                       
                        if (data.Remark != null) { txtRemark.Text = data.Remark; }
                        if (data.ConperID != null) UCContactPerson1.ContactPersonIDs = data.ConperID;
                        if (data.BillingAddressID != null) UCAddress1.BillingSeq = data.BillingAddressID.ToString();
                        if (data.ShippingAddressID != null) UCAddress1.ShippingSeq = data.ShippingAddressID.ToString();
                       
                        UC_AddToCart1.SubTotal = Convert.ToDecimal(data.SubTotal);
                       
                        UC_AddToCart1.DiscountOnSubTotal = Convert.ToDecimal(data.DiscountOnSubTotal);
                        UC_AddToCart1.DiscountOnSubTotalPercent = data.DiscountOnSubTotalPercent.Value.ToString().ToLower();
                        UC_AddToCart1.TotalDiscount = Convert.ToDecimal(data.TotalDiscount);
                        UC_AddToCart1.TotalAfterDiscount = Convert.ToDecimal(data.TotalAfterDiscount);
                       
                        UC_AddToCart1.TotalTax = Convert.ToDecimal(data.TotalTax);
                        UC_AddToCart1.ShippingCharges = Convert.ToDecimal(data.ShippingCharges);
                        UC_AddToCart1.OtherChargesDescription = data.OtherChargesDescription;
                        UC_AddToCart1.OtherCharges = Convert.ToDecimal(data.OtherCharges);
                        UC_AddToCart1.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                        if (Convert.ToDecimal(data.SubTotal) > 0) hdnCartCount.Text = "1";
                    }
                }

                if (ConvertObjectFrom == "SalesOrder")
                {
                    SalesOrderService.iSalesOrderDetailsClient SalesOrderClient = new SalesOrderService.iSalesOrderDetailsClient();
                    SalesOrderService.tSalesOrderHead data = new tSalesOrderHead();
                    data = SalesOrderClient.GetSalesOrderDetailByID(RefID, profile.DBConnection._constr);
                    SalesOrderClient.Close();
                    if (data != null)
                    {
                        if (data.Title != null) txtTitle.Text = data.Title.ToString().Trim();
                        else { txtTitle.Text = null; }
                        if (data.CampaignID != null) ddlCampaign.SelectedIndex = ddlCampaign.Items.IndexOf(ddlCampaign.Items.FindByValue(data.CampaignID.Value.ToString()));
                        txtInvoiceNo.Text = "0";
                        txtUserInvoiceNo.Text = "0";
                        UC_InvoiceDate.Date = data.SalesOrderDate;
                        if (data.LeadSourceID != null) ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(data.LeadSourceID.ToString()));
                        if (data.Executive != null) ddlExecutive.SelectedIndex = ddlExecutive.Items.IndexOf(ddlExecutive.Items.FindByValue(data.Executive.ToString()));
                        fillCustomerDetails(data.CustomerHeadID);
                        ddlInvoiceStatus.SelectedValue = data.SalesOrderStatus;

                        txtCustomerPONo.Text = data.CustomerPONo;
                        UC_CustomerPODate.Date = data.CustomerPODate;
                        if (data.ConperID != null) UCContactPerson1.ContactPersonIDs = data.ConperID;

                        if (data.Remark != null) { txtRemark.Text = data.Remark; }
                        if (data.BillingAddressID != null) UCAddress1.BillingSeq = data.BillingAddressID.ToString();
                        if (data.ShippingAddressID != null) UCAddress1.ShippingSeq = data.ShippingAddressID.ToString();
                       
                        UC_AddToCart1.SubTotal = Convert.ToDecimal(data.SubTotal);
                       
                        UC_AddToCart1.DiscountOnSubTotal = Convert.ToDecimal(data.DiscountOnSubTotal);
                        UC_AddToCart1.DiscountOnSubTotalPercent = data.DiscountOnSubTotalPercent.Value.ToString().ToLower();
                        UC_AddToCart1.TotalDiscount = Convert.ToDecimal(data.TotalDiscount);
                        UC_AddToCart1.TotalAfterDiscount = Convert.ToDecimal(data.TotalAfterDiscount);
                     
                        UC_AddToCart1.TotalTax = Convert.ToDecimal(data.TotalTax);
                        UC_AddToCart1.ShippingCharges = Convert.ToDecimal(data.ShippingCharges);
                        UC_AddToCart1.OtherChargesDescription = data.OtherChargesDescription;
                        UC_AddToCart1.OtherCharges = Convert.ToDecimal(data.OtherCharges);
                        UC_AddToCart1.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                        if (Convert.ToDecimal(data.SubTotal) > 0) hdnCartCount.Text = "1";

                    }
                }
                if (ConvertObjectFrom == "Invoice")
                {
                    InvoiceService.iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
                    InvoiceService.tInvoiceHead data = new InvoiceService.tInvoiceHead();
                 
                    data = InvoiceClient.GetInvoiceDetailByID(RefID, profile.DBConnection._constr);
                    InvoiceClient.Close();
                    if (data != null)
                    {
                        if (data.Title != null) txtTitle.Text = data.Title.ToString().Trim();
                        else { txtTitle.Text = null; }
                        if (data.CampaignID != null) ddlCampaign.SelectedIndex = ddlCampaign.Items.IndexOf(ddlCampaign.Items.FindByValue(data.CampaignID.Value.ToString()));
                        txtInvoiceNo.Text = RefID.ToString();
                        txtUserInvoiceNo.Text = data.InvoiceNo;
                        UC_InvoiceDate.Date = data.InvoiceDate;
                        if (data.LeadSourceID != null) ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(data.LeadSourceID.ToString()));
                        if (data.Executive != null) ddlExecutive.SelectedIndex = ddlExecutive.Items.IndexOf(ddlExecutive.Items.FindByValue(data.Executive.ToString()));
                        fillCustomerDetails(data.CustomerHeadID);
                        ddlInvoiceStatus.SelectedValue = data.InvoiceStatus;
                        ddlInvoiceType.SelectedValue = data.InvoiceType;
                        txtCustomerPONo.Text = data.CustomerPONo;
                        UC_CustomerPODate.Date = data.CustomerPODate;
                        txtParentInvoiceNo.Text = data.ParentInvoiceID.ToString();
                        if (data.DispatchThrough != null) { txtlDispatchThrough.Text = data.DispatchThrough; }
                        UC_ExpDispatchDate.Date = data.ExpDispatchDate;
                      

                        if (data.Remark != null) { txtRemark.Text = data.Remark; }
                        if (data.ConperID != null) UCContactPerson1.ContactPersonIDs = data.ConperID;

                        if (data.BillingAddressID != null) UCAddress1.BillingSeq = data.BillingAddressID.ToString();
                        if (data.ShippingAddressID != null) UCAddress1.ShippingSeq = data.ShippingAddressID.ToString();
                       
                        UC_AddToCart1.SubTotal = Convert.ToDecimal(data.SubTotal);
                      
                        UC_AddToCart1.DiscountOnSubTotal = Convert.ToDecimal(data.DiscountOnSubTotal);
                        UC_AddToCart1.DiscountOnSubTotalPercent = data.DiscountOnSubTotalPercent.Value.ToString().ToLower();
                        UC_AddToCart1.TotalDiscount = Convert.ToDecimal(data.TotalDiscount);
                        UC_AddToCart1.TotalAfterDiscount = Convert.ToDecimal(data.TotalAfterDiscount);
                       
                        UC_AddToCart1.TotalTax = Convert.ToDecimal(data.TotalTax);
                        UC_AddToCart1.ShippingCharges = Convert.ToDecimal(data.ShippingCharges);
                        UC_AddToCart1.OtherChargesDescription = data.OtherChargesDescription;
                        UC_AddToCart1.OtherCharges = Convert.ToDecimal(data.OtherCharges);
                        UC_AddToCart1.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                        if (Convert.ToDecimal(data.SubTotal) > 0) hdnCartCount.Text = "1";
                        bindgrvPaymentGrid(RefID, data.TotalAmount.ToString());


                    }
                }
                FillUserControl(RefID);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "BindData");
            }
            finally
            {
            }*/
        }

        public void bindDropdown()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
              /* LeadService.iLeadSourceMasterClient LeadService = new iLeadSourceMasterClient();

                LeadService.iLeadSourceMasterClient LeadSourceMasterService = new WebClientElegantCRM.LeadService.iLeadSourceMasterClient();
                ddlLeadSource.DataSource = LeadService.GetLeadRecordToBind(profile.DBConnection._constr);
                ddlLeadSource.DataBind();
                ListItem lst1 = new ListItem();
                lst1.Text = "-Select-";
                lst1.Value = "0";
                ddlLeadSource.Items.Insert(0, lst1);

                iLeadDetailsClient LeadClient = new iLeadDetailsClient();
                ddlCompanyType.DataSource = LeadClient.GetCompanyType(profile.DBConnection._constr);
                ddlCompanyType.DataBind();
                ListItem lst4 = new ListItem();
                lst4.Text = "-Select-";
                lst4.Value = "0";
                ddlCompanyType.Items.Insert(0, lst4);

                //ListItem lst5 = new ListItem();
                //lst5.Text = "-Select-";
                //lst5.Value = "0";
                //ddlDispatchThrough.Items.Insert(0, lst5);

                ddlSector.DataSource = LeadClient.GetLeadSector(profile.DBConnection._constr);
                ddlSector.DataBind();
                ListItem lst3 = new ListItem();
                lst3.Text = "-Select-";
                lst3.Value = "0";
                ddlSector.Items.Insert(0, lst3);

                UCAssignTaskService.iUCAssignTaskClient ucAssignTaskService = new UCAssignTaskService.iUCAssignTaskClient();
                ddlExecutive.DataSource = ucAssignTaskService.GetAssignToList("Invoice", profile.Personal.UserID, profile.DBConnection._constr).ToList();
                ddlExecutive.DataBind();
                ListItem lst6 = new ListItem();
                lst6.Text = "-Select-";
                lst6.Value = "0";
                ddlExecutive.Items.Insert(0, lst6);
                ucAssignTaskService.Close();

                LeadClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "bindDropdown");
            }
            finally
            {
            }
        }

        public void clr()
        {
            hdnCartCount.Text = "0";
            ddlCampaign.SelectedIndex = -1;
            ddlInvoiceStatus.SelectedIndex = -1;
            ddlSector.SelectedIndex = -1;
            ddlCompanyType.SelectedIndex = -1;
            ddlLeadSource.SelectedIndex = -1;
            ddlInvoiceType.SelectedIndex = -1;
            txtlDispatchThrough.Text = "";
            txtTitle.Text = "";
            txtInvoiceNo.Text = "0";
            txtUserInvoiceNo.Text = "0";
            txtAccountName.Text = "";
            txtWebSite.Text = "";
            txtCustomerPONo.Text = "0";
            txtParentInvoiceNo.Text = "";
            txtRemark.Text = "";
            lblTotalReceivedAmountF.Text = "";
            lblInvoiceAmountF.Text = "";
            lblOutstandingAmountF.Text = "";
            txtPayDetails.Text = "";
            txtPaymentScheduleRemarks.Text = "";
            //UC_InvoiceDate.Date = DateTime.Now;
            UC_InvoiceDate.Date = null;
            UC_ExpDispatchDate.Date = null;
            UC_CustomerPODate.Date = null;

            hdnCustomerID.Value = "";
            hdnPaymentScheduleID.Value = "";
            hdnPaymentBookingID.Value = "";
            ResetUserControl();
            grvPaymentSchedule.DataSource = null;
            grvPaymentSchedule.DataBind();
            grvPaymentBooking.DataSource = null;
            grvPaymentBooking.DataBind();
            ResetUserControl();
        }

        protected void fillAcDetails(string AccountName)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                txtAccountName.Text = AccountName;
                if (AccountName != "")
                {
                    /*iLeadDetailsClient LeadClient = new iLeadDetailsClient();
                    LeadDetailsService.tCustomerHead objCustomerHead = new LeadDetailsService.tCustomerHead();
                    objCustomerHead = LeadClient.GetCustomerHeadDetailByCustomerID(Convert.ToInt64(AccountName), profile.DBConnection._constr);
                    hdnCustomerID.Value = objCustomerHead.ID.ToString();
                    txtAccountName.Text = objCustomerHead.Name;
                    ddlSector.SelectedValue = objCustomerHead.SectorID.ToString();
                    txtWebSite.Text = objCustomerHead.WebSite;
                    ddlCompanyType.SelectedValue = objCustomerHead.CustomerTypeID.ToString();
                    LeadClient.Close();*/
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "fillAcDetails");
            }
            finally
            {
            }
        }

        public void bindgrvPaymentGrid(int id, string InvoiceAmount)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               /* InvoiceService.iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();

                grvPaymentSchedule.DataSource = InvoiceClient.GetInvoicePaymentScheduleDetail(id, Session.SessionID, "InvoicePS", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                grvPaymentSchedule.DataBind();

                List<tInvoicePaymentBooking> InvoicePBList = new List<tInvoicePaymentBooking>();
                InvoicePBList = InvoiceClient.GetInvoicePaymentBookingDetail(id, Session.SessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                grvPaymentBooking.DataSource = InvoicePBList;
                grvPaymentBooking.DataBind();
                lblInvoiceAmountF.Text = Convert.ToDecimal(InvoiceAmount).ToString("0.00");
                lblTotalReceivedAmountF.Text = Convert.ToDecimal(InvoicePBList.Sum(PB => PB.PaymentAmount)).ToString("0.00");
                lblOutstandingAmountF.Text = Convert.ToDecimal(Convert.ToDecimal(InvoiceAmount) - Convert.ToDecimal(lblTotalReceivedAmountF.Text)).ToString("0.00");
                InvoiceClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Invoice", "bindgrvPaymentGrid");
            }
            finally
            {
            }

        }

        #region Payment Schedule

       /* [WebMethod]
        public static string PMSaveTempDataOfPaymentSchedule(string Sequence, object ObjInvoicePS)
        {
            iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tInvoicePaymentSchedule InvoicePS = new tInvoicePaymentSchedule();

                if (Sequence != "" && Sequence != "0")
                {
                    InvoicePS = InvoiceClient.GetTempDataOfPaymentScheduleBySequenceNo(Convert.ToInt64(Sequence), sessionID, "InvoicePS", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    InvoicePS.Sequence = Convert.ToInt64(Sequence);
                }
                else { InvoicePS.Sequence = 0; }

                if (InvoicePS.ID == 0)
                {
                    InvoicePS.CreatedBy = profile.Personal.UserID.ToString();
                    InvoicePS.CreationDate = DateTime.Now;
                }
                else
                {
                    InvoicePS.LastModifiedBy = profile.Personal.UserID.ToString();
                    InvoicePS.LastModifiedDate = DateTime.Now;
                }

                InvoicePS.InvoiceID = 0;
                Dictionary<string, object> rec = new Dictionary<string, object>();
                rec = (Dictionary<string, object>)ObjInvoicePS;
                if (rec["PaymentScheduleDate"].ToString() != "") { InvoicePS.PaymentScheduleDate = Convert.ToDateTime(rec["PaymentScheduleDate"]); }

                InvoicePS.PaymentScheduleAmount = Convert.ToDecimal(rec["PaymentScheduleAmount"]);
                if (rec["AlertDate"].ToString() != "") { InvoicePS.AlertDate = Convert.ToDateTime(rec["AlertDate"]); }
                InvoicePS.AlertEmail = Convert.ToBoolean(rec["AlertEmail"]);
                InvoicePS.AlertSMS = Convert.ToBoolean(rec["AlertSMS"]);
                InvoicePS.CustomerAlert = Convert.ToBoolean(rec["CustomerAlert"]);
                InvoicePS.Remark = rec["Remark"].ToString();
                InvoicePS.Active = "Y";
                InvoicePS.CompanyID = profile.Personal.CompanyID;

                if (Sequence != "0" && Sequence != "")
                { InvoiceClient.UpdateTempDataOfPaymentSchedule(InvoicePS, sessionID, "InvoicePS", profile.Personal.UserID.ToString(), profile.DBConnection._constr); }
                else
                { InvoiceClient.CreateTempDataOfPaymentSchedule(InvoicePS, sessionID, "InvoicePS", profile.Personal.UserID.ToString(), profile.DBConnection._constr); }
                InvoiceClient.Close();
                return "true";

            }
            catch (System.Exception ex)
            {
                InvoiceClient.Close();
                //Login.Profile.ErrorHandling(ex, , "AddressInfo", "PMSaveAddress");
                return "false";
            }
        }*/

        protected void grvPaymentSchedule_OnRebind(object sender, EventArgs e)
        {
           /* iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grvPaymentSchedule.DataSource = null;
                grvPaymentSchedule.DataBind();

                grvPaymentSchedule.DataSource = InvoiceClient.GetTempDataOfPaymentSchedule(Session.SessionID, "InvoicePS", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                grvPaymentSchedule.DataBind();

            }
            catch (Exception ex) { }*/
        }
        #endregion

        #region PaymentBooking

        /* [WebMethod]
        public static string[] PMSaveTempDataOfPaymentBooking(string Sequence, object ObjInvoicePB)
        {
            string[] result;
            result = new string[2];
            result[0] = "false";
            result[1] = "0.00";
           iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tInvoicePaymentBooking InvoicePB = new tInvoicePaymentBooking();

                if (Sequence != "" && Sequence != "0")
                {
                    InvoicePB = InvoiceClient.GetTempDataOfPaymentBookingeBySequenceNo(Convert.ToInt64(Sequence), sessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    InvoicePB.Sequence = Convert.ToInt64(Sequence);
                }
                else { InvoicePB.Sequence = 0; }

                if (InvoicePB.ID == 0)
                {
                    InvoicePB.CreatedBy = profile.Personal.UserID.ToString();
                    InvoicePB.CreationDate = DateTime.Now;
                }
                else
                {
                    InvoicePB.LastModifiedBy = profile.Personal.UserID.ToString();
                    InvoicePB.LastModifiedDate = DateTime.Now;
                }

                InvoicePB.InvoiceID = 0;
                Dictionary<string, object> rec = new Dictionary<string, object>();
                rec = (Dictionary<string, object>)ObjInvoicePB;
                if (rec["PaymentReceivedDate"].ToString() != "") { InvoicePB.PaymentReceivedDate = Convert.ToDateTime(rec["PaymentReceivedDate"]); }
                InvoicePB.PaymentAmount = Convert.ToDecimal(rec["PaymentAmount"]);
                if (rec["PaymentMode"].ToString() != "") { InvoicePB.PaymentMode = Convert.ToString(rec["PaymentMode"]); }
                if (rec["PaymentDetails"].ToString() != "") { InvoicePB.PaymentDetails = Convert.ToString(rec["PaymentDetails"]); }
                InvoicePB.OutstandingAmount = 0;
                InvoicePB.Remark = rec["Remark"].ToString();
                InvoicePB.Active = "Y";
                InvoicePB.CompanyID = profile.Personal.CompanyID;
                List<tInvoicePaymentBooking> InvoicePBLst = new List<tInvoicePaymentBooking>();
                if (Sequence != "0" && Sequence != "")
                { InvoicePBLst = InvoiceClient.UpdateTempDataOfPaymentBooking(InvoicePB, sessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList(); }
                else
                { InvoicePBLst = InvoiceClient.CreateTempDataOfPaymentBooking(InvoicePB, sessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList(); }
                InvoiceClient.Close();
                result[0] = "true";
                result[1] = Convert.ToDecimal(InvoicePBLst.Sum(pb => pb.PaymentAmount)).ToString("0.00");
                return result;

            }
            catch (System.Exception ex)
            {
                InvoiceClient.Close();
                //Login.Profile.ErrorHandling(ex, , "AddressInfo", "PMSaveAddress");
                return result;
            }
        }*/

        protected void grvPaymentBooking_OnRebind(object sender, EventArgs e)
        {
           /* iInvoiceDetailsClient InvoiceClient = new iInvoiceDetailsClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grvPaymentBooking.DataSource = null;
                grvPaymentBooking.DataBind();

                grvPaymentBooking.DataSource = InvoiceClient.GetTempDataOfPaymentBooking(Session.SessionID, "InvoicePB", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                grvPaymentBooking.DataBind();

            }
            catch (Exception ex) { }*/
        }
        #endregion

      /*  [WebMethod]
        public static string PMCheckDuplicateInvoiceNo(string InvoiceNo)
        {
            iInvoiceDetailsClient invoiceClient = new iInvoiceDetailsClient();
            string result = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                result = invoiceClient.checkDuplicateInvoiceNo(InvoiceNo, InvoiceID, profile.DBConnection._constr);
                return result;
            }
            catch (Exception ex) { return result; Login.Profile.ErrorHandling(ex, "Invoice", "PMCheckDuplicateInvoiceNo"); }
            finally { invoiceClient.Close(); }
        }*/
    }
}