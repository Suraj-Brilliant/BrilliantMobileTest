using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.DocumentService;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.WMS
{
    public partial class PurchaseOrder : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string ObjectName = "PurchaseOrder";       
        static long UOMID = 0;

        public Page ParentPage { get; set; }

        public decimal SubTotal { get; set; }
        public decimal DiscountOnSubTotal { get; set; }
        public string DiscountOnSubTotalPercent { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal ShippingCharges { get; set; }
        public string OtherChargesDescription { get; set; }
        public decimal OtherCharges { get; set; }
        public decimal TotalAmount { get; set; }

        #region Page Events

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillWarehouse();
                UC_AttachmentDocument1.ClearDocument("PurchaseOrder");
                if (Session["POID"] != null)
                {
                    if (Session["POID"].ToString() != "0")
                    {
                        lblApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                        GetPOHead();
                        gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()));
                        GVInboxPOR_OnRebind(sender, e);
                    }
                    else
                    {
                        /* For New PO*/
                        WMpageAddNew();
                        setFooterValuesToProperty();
                        UC_ExpDeliveryDate.startdate(DateTime.Now);
                        ddlStatus.DataSource = WMFillStatus();
                        ddlStatus.DataBind();
                    }
                }
                divVisibility();
            }
            UC_ExpDeliveryDate.DateIsRequired(true, "", "");
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }

            //loadstring();

            if (!IsPostBack)
            {
                iInboundClient Inbound = new iInboundClient();      
                CustomProfile profile = CustomProfile.GetProfile();
                //  ddlSites.Attributes.Add("onchange", "jsFillUsersList();jsFillEnginList();");
                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");
                Inbound.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }
        }

        public void setFooterValuesToProperty()
        {
            try
            {
                //SubTotal = Convert.ToDecimal(lblSubTotal.Text);
                if (hdnCartSubTotal.Value == "") hdnCartSubTotal.Value = "0.00";
                SubTotal = Convert.ToDecimal(hdnCartSubTotal.Value);
                lblSubTotal.Text = SubTotal.ToString("0.00");

                if (txtDiscountOnSubTotal.Text == "") txtDiscountOnSubTotal.Text = "0.00";
                DiscountOnSubTotal = Convert.ToDecimal(txtDiscountOnSubTotal.Text);
                DiscountOnSubTotalPercent = chkboxDiscountOnSubTotal.Checked.ToString();
                txtDiscountOnSubTotal.Text = DiscountOnSubTotal.ToString("0.00");

                if (hdnCartDiscountOnSubTotal.Value == "") hdnCartDiscountOnSubTotal.Value = "0.00";
                TotalDiscount = Convert.ToDecimal(hdnCartDiscountOnSubTotal.Value);
                lblDiscountOnSubTotal.Text = TotalDiscount.ToString("0.00");

                if (hdnCartSubTotal2.Value == "") hdnCartSubTotal2.Value = "0.00";
                TotalAfterDiscount = Convert.ToDecimal(hdnCartSubTotal2.Value);
                lblSubTotal2.Text = TotalAfterDiscount.ToString();

                if (hdnCartTaxOnSubTotal.Value == "") hdnCartTaxOnSubTotal.Value = "0.00";
                TotalTax = Convert.ToDecimal(hdnCartTaxOnSubTotal.Value);
                lblTaxOnSubTotal.Text = TotalTax.ToString();

                if (txtShippingCharges.Text == "") txtShippingCharges.Text = "0.00";
                ShippingCharges = Convert.ToDecimal(txtShippingCharges.Text);
                txtShippingCharges.Text = ShippingCharges.ToString("0.00");

                OtherChargesDescription = txtAdditionalChargeDescription.Text;

                if (txtAdditionalCharges.Text == "") txtAdditionalCharges.Text = "0.00";
                OtherCharges = Convert.ToDecimal(txtAdditionalCharges.Text);

                if (hdnCartGrandTotal.Value == "") hdnCartGrandTotal.Value = "0.00";
                TotalAmount = Convert.ToDecimal(hdnCartGrandTotal.Value);
                lblGrandTotal.Text = TotalAmount.ToString("0.00");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "PurchaseOrder.aspx", "setFooterValuesToProperty");
            }
            finally
            {
            }
        }
        #endregion

        #region FillDropdown

        protected void fillWarehouse()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<mWarehouseMaster> WarehouseList = new List<mWarehouseMaster>();
                long UserID = profile.Personal.UserID;
                WarehouseList = Inbound.GetUserWarehouse(UserID, profile.DBConnection._constr).ToList();
                ddlSites.DataSource = WarehouseList;
                ddlSites.DataBind();
                ListItem lstW = new ListItem { Text = "-Select-", Value = "0" };
                ddlSites.Items.Insert(0, lstW);

                /*Fill Vendor*/
                List<mVendor> VendorLst = new List<mVendor>();
                VendorLst = Inbound.GetVendor(profile.Personal.CompanyID,profile.DBConnection._constr).ToList();
                ddlCompany.DataSource = VendorLst;
                ddlCompany.DataBind();
                ListItem lstV = new ListItem { Text = "-Select-", Value = "0" };
                ddlCompany.Items.Insert(0, lstV);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "fillWarehouse");
            }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static List<tAddress> WMGetWarehouseAddress(long WarehouseID)
        {
            List<tAddress> AdrsLst = new List<tAddress>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            AdrsLst = UCCommonFilter.GetWarehouseAddressList(WarehouseID, profile.DBConnection._constr).ToList();

            return AdrsLst;
        }

        [WebMethod]
        public static string WMGetWarehouseSession(string Warehouse)
        {
            Page objp = new Page();
            objp.Session["WarehouseID"] = Warehouse; objp.Session["DeptID"] = null;
            return Warehouse;
        }

        [WebMethod]
        public static string WMGetVendorSession(string Vendor)
        {
            Page objp = new Page();
            objp.Session["VendorID"] = Vendor;
            return Vendor;
        }

        [WebMethod]
        public static List<BrilliantWMS.WMSInbound.mStatu> WMFillStatus()
        {
            string state = HttpContext.Current.Session["POstate"].ToString();
            iInboundClient Inbound = new iInboundClient();

            List<BrilliantWMS.WMSInbound.mStatu> StatusList = new List<BrilliantWMS.WMSInbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                if (HttpContext.Current.Session["POID"].ToString() == "0" && state == "Add")
                {
                    StatusList = Inbound.GetStatusListForInbound(ObjectName,"", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();

                    long CompanyID = profile.Personal.CompanyID;
                    int Sequence = Inbound.GetWorkflowSequenceOfPO("PurchaseOrder", CompanyID, profile.DBConnection._constr);
                    StatusList = StatusList.Where(s => s.Sequence==Sequence || s.Sequence==1).ToList();
                    //StatusList = StatusList.Where(s => s.ID == 1 || s.ID == 2).ToList();
                }
                else if (HttpContext.Current.Session["POID"].ToString() != "0" && state == "Edit")
                {
                    if (HttpContext.Current.Session["OrderStatus"].ToString() == "45")
                    {
                        StatusList = Inbound.GetStatusListForInbound(ObjectName, "PurchaseOrder", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        long CompanyID = profile.Personal.CompanyID;
                        int Sequence = Inbound.GetWorkflowSequenceOfPO("PurchaseOrder", CompanyID, profile.DBConnection._constr);
                        StatusList = StatusList.Where(s => s.Sequence == Sequence || s.Sequence == 1).ToList();
                    }
                    else
                    {
                        StatusList = Inbound.GetStatusListForInbound(ObjectName, "PurchaseOrder", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                }
                else if (HttpContext.Current.Session["POID"].ToString() != "0" && state == "View")
                {
                    StatusList = Inbound.GetStatusListForInbound("","PurchaseOrder,POSO", "", 0, profile.DBConnection._constr).ToList();
                }

                BrilliantWMS.WMSInbound.mStatu select = new BrilliantWMS.WMSInbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMFillStatus");
            }
            finally { Inbound.Close(); }
            return StatusList;
        }
        #endregion

        #region ToolbarCode
        [WebMethod]
        public static void WMpageAddNew()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["POID"] = 0;
                HttpContext.Current.Session["POstate"] = "Add";
                Inbound.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMpageAddNew");
            }
            finally { Inbound.Close(); }
        }
        #endregion

        #region POHead
        protected void GetPOHead()
        {
            iInboundClient Inbound = new iInboundClient();
            tPurchaseOrderHead POHead = new tPurchaseOrderHead();            
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();                                
                POHead = Inbound.GetPoHeadByPOID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                hdnOrderStatus.Value = POHead.Status.ToString(); Session["OrderStatus"] = hdnOrderStatus.Value;
                FillGrid1ByRequestID(POHead.Id, Convert.ToInt64(POHead.Warehouse));
                
                txtTitle.Text = POHead.Title;
                long SiteID = long.Parse(POHead.Warehouse.ToString());

                iUCCommonFilterClient objCommon = new iUCCommonFilterClient();
               // long CompanyID = objCommon.GetCompanyIDFromSiteID(SiteID, profile.DBConnection._constr);
                                
                if (profile.Personal.UserType == "Super Admin" || profile.Personal.UserType == "Admin")
                {
                    List<mVendor> VendorList = new List<mVendor>();
                    VendorList = Inbound.GetVendor(profile.Personal.CompanyID,profile.DBConnection._constr).ToList();

                    ddlCompany.DataSource = VendorList;
                    ddlCompany.DataBind();


                    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(POHead.VendorID.ToString())); Session["VendorID"] = POHead.VendorID.ToString();
                    //ddlCompany.Enabled = false;

                    long UID = profile.Personal.UserID;
                    List<mWarehouseMaster> WarehouseList = new List<mWarehouseMaster>();                   
                    if (profile.Personal.UserType == "Admin")
                    {
                        WarehouseList = Inbound.GetWarehouseNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();                       
                    }
                    else if (profile.Personal.UserType == "Super Admin")
                    {
                        WarehouseList = Inbound.GetAllWarehouseList(profile.DBConnection._constr).ToList();                       
                    }
                    ddlSites.DataSource = WarehouseList; 
                    ddlSites.DataBind();Session["WarehouseID"] = POHead.Warehouse.ToString();
                    //  ddlSites.Enabled = false;
                }

                ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(POHead.Warehouse.ToString())); hdnSelectedWarehouse.Value = POHead.Warehouse.ToString();
                if (hdnOrderStatus.Value == "45") { lblRequestNo.Text = "Generate when Save"; }
                else
                {
                    lblRequestNo.Text = POHead.Id.ToString(); 
                }
                ddlStatus.DataSource = WMFillStatus();
                ddlStatus.DataBind();
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "divVisibility123", "divVisibility()");

                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(POHead.Status.ToString()));

                UC_DateRequestDate.Date = POHead.POdate;
                txtRequestDate.Text = Convert.ToString(POHead.POdate.Value.ToString("dd-MMM-yyyy"));

                //      ddlRequestType.SelectedIndex = ddlRequestType.Items.IndexOf(ddlRequestType.Items.FindByValue(RequestHead.Priority.ToString()));
                if (hdnOrderStatus.Value == "45")
                {
                    ddlRequestByUserID.DataSource = FillCurrentUserList(Convert.ToInt64(hdnSelectedWarehouse.Value));
                }
                else
                {
                    ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(POHead.Warehouse)); hdnselectedDept.Value = POHead.Warehouse.ToString();
                }
                ddlRequestByUserID.DataBind();
                ddlRequestByUserID.SelectedIndex = ddlRequestByUserID.Items.IndexOf(ddlRequestByUserID.Items.FindByValue(POHead.CreatedBy.ToString()));

                txtRemark.Text = POHead.Remark;

                txtCustOrderRefNo.Text = POHead.OrderNumber;
                UC_ExpDeliveryDate.Date = POHead.Deliverydate; //if (RequestHead.Status >= 2) { Page.ClientScript.RegisterStartupScript(this.GetType(), "reset", " disableExpDeliveryDate();", true); }

                
                ddlAddress.SelectedIndex = ddlAddress.Items.IndexOf(ddlAddress.Items.FindByValue(POHead.AddressId.ToString())); hdnSelAddress.Value = POHead.AddressId.ToString();
                ddlContact1.SelectedIndex = ddlContact1.Items.IndexOf(ddlContact1.Items.FindByValue(POHead.ContactId1.ToString())); hdnselectedCont1.Value = POHead.ContactId1.ToString();
                
                /*New Change Code*/
                long EdtCon1 = long.Parse(POHead.ContactId1.ToString());
                long EdtAddress = long.Parse(POHead.AddressId.ToString());

             //   long LocAddress = long.Parse(POHead.LocationID.ToString());

               // string EdtCon2 = RequestHead.Con2; hdnselectedCont2.Value = RequestHead.Con2.ToString();
                if (EdtCon1 != 0)
                {
                    txtContact1.Text = objCommon.getContact1NameByID(EdtCon1, profile.DBConnection._constr);
                }
               
                if (EdtAddress != 0)
                {
                    txtAddress.Text = objCommon.GetAddressLineByAdrsID(EdtAddress, profile.DBConnection._constr);
                    lblAddress.Text = objCommon.GetAddressLineByAdrsID(EdtAddress, profile.DBConnection._constr);
                }

                /*New Change Code*/

                txtTotalQty.Text = POHead.TotalQty.ToString();
                txtGrandTotal.Text = POHead.GrandTotal.ToString();

                lblSubTotal.Text=POHead.SubTotal.ToString();
                txtDiscountOnSubTotal.Text=POHead.DiscountOnSubTotal.ToString();
                if (POHead.DiscountOnSubTotalPercent == true) { chkboxDiscountOnSubTotal.Checked = true; }
                else { chkboxDiscountOnSubTotal.Checked=false; }
                lblDiscountOnSubTotal.Text= POHead.TotalDiscount.ToString();
                lblSubTotal2.Text=POHead.TotalAfterDiscount.ToString();
                lblTaxOnSubTotal.Text=POHead.TotalTax.ToString();
                txtAdditionalChargeDescription.Text=POHead.OtherChargesDescription.ToString();
                txtAdditionalCharges.Text=POHead.OtherCharges.ToString();
                txtShippingCharges.Text=POHead.ShippingCharges.ToString();
                lblGrandTotal.Text=POHead.TotalAmount.ToString();

                //fillPaymentMethod(long.Parse(hdnselectedDept.Value));
                //ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByValue(RequestHead.PaymentID.ToString()));
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "reset", " GetPaymentMethodID();", true);
                //if (RequestHead.PaymentID == 5)
                //{
                //    WMGetCostCenter(long.Parse(hdnselectedDept.Value));
                //    DataSet dsCostCenter = new DataSet();
                //    dsCostCenter = objService.GetSelectedCostCenter(long.Parse(RequestHead.Id.ToString()), profile.DBConnection._constr);
                //    long SelectedCostCenter = long.Parse(dsCostCenter.Tables[0].Rows[0]["StatutoryValue"].ToString());
                //    //ddlFOC.SelectedIndex = ddlFOC.Items.IndexOf(ddlFOC.Items.FindByValue(SelectedCostCenter.ToString()));
                //    ddlFOC.SelectedValue = SelectedCostCenter.ToString();
                //}
                //else
                //{
                //    //LstStatutoryInfo.Enabled = false;
                //    if (RequestHead.Status >= 2) { LstStatutoryInfo.Enabled = false; }
                //    DataSet dsAdditionalFields = new DataSet();
                //    dsAdditionalFields = objService.GetAddedAdditionalFields(long.Parse(RequestHead.Id.ToString()), profile.DBConnection._constr);
                //    LstStatutoryInfo.DataSource = dsAdditionalFields;
                //    LstStatutoryInfo.DataBind();
                //}
                GetDocumentByPOID(long.Parse(POHead.Id.ToString()));
                //GetApprovalDetails();
                //GetDeliveryDetails(RequestHead.Id);

                /*ASN Details Coading*/
                GetASNDetails(long.Parse(POHead.Id.ToString()));
                /*ASN Details Coading*/
            }
            catch { }
            finally { Inbound.Close(); }
        }

        protected void GetASNDetails(long poID)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdASN.DataSource = Inbound.GetASNHead(poID,profile.DBConnection._constr);
                grdASN.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "GetASNDetails"); }
            finally { Inbound.Close(); }
        }

        protected void divVisibility()
        {
            divApprovalHead.Attributes.Add("style", "display:none");
            divApprovalDetail.Attributes.Add("style", "display:none");

            divIssueHead.Attributes.Add("style", "display:none");
            divIssueDetail.Attributes.Add("style", "display:none");

            divCorrespondanceHead.Attributes.Add("style", "display:none");
            divCorrespondanceDetails.Attributes.Add("style", "display:none");

            divHeadASNDetail.Attributes.Add("style", "display:none");
            divASNDetail.Attributes.Add("style", "display:none");
            //linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
            //divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";

            //divReceiptHead.Attributes.Add("style", "display:none");
            //divReceiptDetail.Attributes.Add("style", "display:none");

            //divConsumptionHead.Attributes.Add("style", "display:none");
            //divConsumptionDetail.Attributes.Add("style", "display:none");

            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Approved")) > 0)
            // if (ddlStatus.SelectedIndex == 3)
            if (ddlStatus.SelectedIndex == 2 || ddlStatus.SelectedValue == "22" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "2" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "4" || ddlStatus.SelectedIndex == 21 || ddlStatus.SelectedIndex == 22 || ddlStatus.SelectedIndex == 4 || ddlStatus.SelectedIndex == 41 || ddlStatus.SelectedIndex == 31 || ddlStatus.SelectedIndex == 32 || ddlStatus.SelectedIndex == 33 || ddlStatus.SelectedIndex == 35)
            {
                 iInboundClient Inbound = new iInboundClient();          
                CustomProfile profile = CustomProfile.GetProfile();
                long CmpnyID = profile.Personal.CompanyID;
                int IsApproval = Inbound.GetApprovalInWorkFlow(CmpnyID, profile.DBConnection._constr);
                if (IsApproval > 0)
                {
                    divApprovalHead.Attributes.Add("style", "display:'';");
                    divApprovalDetail.Attributes.Add("style", "display:'';");
                }
                else
                {
                    divApprovalHead.Attributes.Add("style", "display:none;");
                    divApprovalDetail.Attributes.Add("style", "display:none;");
                }

                //linkRequest.Attributes["innerHTML"] = "Expand";
                linkRequest.Attributes["innerHTML"] = "Collapse";
                linkRequest.InnerText = "Expand";
                divRequestDetail.Attributes["class"] = "divDetailCollapse";

                //linkIssueDetail.Attributes["innerHTML"] = "Expand";
                //divIssueDetail.Attributes["class"] = "divDetailCollapse";
                linkIssueDetail.Attributes.Add("InnerHtml", "Expand");
                linkIssueDetail.InnerText = "Expand";
                divIssueDetail.Attributes.Add("class", "divDetailCollapse");

                divCorrespondanceHead.Attributes.Add("style", "display:''");
                divCorrespondanceDetails.Attributes.Add("style", "display:''");
                //linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
                //divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";
                linkCorrespondanceDetail.Attributes.Add("InnerHtml", "Expand");
                linkCorrespondanceDetail.InnerText = "Expand";
                divCorrespondanceDetails.Attributes.Add("class", "divDetailCollapse");

                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
                Toolbar1.SetEditRight(false, "Not Allowed");
            }
            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Fully Issued")) > 0)
            if (ddlStatus.SelectedIndex == 3 || ddlStatus.SelectedValue == "7" || ddlStatus.SelectedValue == "10" || ddlStatus.SelectedValue == "8" || ddlStatus.SelectedValue == "25" || ddlStatus.SelectedValue == "26" || ddlStatus.SelectedValue == "28")
            {
                divHeadASNDetail.Attributes.Add("style", "display:'';");
                divASNDetail.Attributes.Add("style", "display:'';");

                divIssueHead.Attributes.Add("style", "display:'';");
                divIssueDetail.Attributes.Add("style", "display:'';");

                linkRequest.Attributes["innerHTML"] = "Expand";
                linkRequest.InnerText = "Expand";
                divRequestDetail.Attributes["class"] = "divDetailCollapse";

                divApprovalHead.Attributes.Add("style", "display:'';");
                divApprovalDetail.Attributes.Add("style", "display:'';");

                linkApprovalDetail.Attributes["innerHTML"] = "Expand";
                linkApprovalDetail.InnerText = "Expand";
                divApprovalDetail.Attributes["class"] = "divDetailCollapse";

                divCorrespondanceHead.Attributes.Add("style", "display:''");
                divCorrespondanceDetails.Attributes.Add("style", "display:''");

                linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
                linkCorrespondanceDetail.InnerText = "Expand";
                divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderHead');LoadingOff();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderCalculation');LoadingOff();", true);
                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
                Toolbar1.SetEditRight(false, "Not Allowed");
            }
            if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Received")) > 0)
            {
                //divReceiptHead.Attributes.Add("style", "display:'';");
                //divReceiptDetail.Attributes.Add("style", "display:'';");
            }
            if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Consumed")) > 0)
            {
                //divConsumptionHead.Attributes.Add("style", "display:'';");
                //divConsumptionDetail.Attributes.Add("style", "display:'';");
            }

            if (ddlStatus.Items.Count > 0)
            {
                //if (Convert.ToInt32(ddlStatus.SelectedItem.Value) >= 2)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);
                //    //ddlRequestByUserID.Enabled = false;
                //    //ddlStatus.Enabled = false;
                //    //chkSelectTemplate.Visible = false;
                //    //btnSaveAsTemplate.Enabled = false;
                //    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                //    //ddlCompany.Enabled = false;
                //    //ddlSites.Enabled = false;
                //    //disabled
                //    //Toolbar1.SetSaveRight(false, "Not Allowed");
                //    Toolbar1.SetClearRight(false, "Not Allowed");
                //    Toolbar1.SetEditRight(true, "Allowed");
                //}
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 3 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 4 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 21 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 22 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 2 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 41 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 31 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 32 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 33 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 35)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);
                    divHeadASNDetail.Attributes.Add("style", "display:'';");
                    divASNDetail.Attributes.Add("style", "display:'';");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderHead');LoadingOff();", true);
                    
                    txtDiscountOnSubTotal.Enabled = false;
                    chkboxDiscountOnSubTotal.Enabled = false;
                    txtAdditionalChargeDescription.Enabled = false;
                    txtShippingCharges.Enabled = false;
                    txtAdditionalCharges.Enabled = false;
                    //ddlRequestByUserID.Enabled = false;
                    //ddlStatus.Enabled = false;
                    //chkSelectTemplate.Visible = false;
                    //btnSaveAsTemplate.Enabled = false;
                    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                    //ddlCompany.Enabled = false;
                    //ddlSites.Enabled = false;
                    //disabled
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                    Toolbar1.SetEditRight(false, "Not Allowed");
                }
               // if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 1)
                    if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 45)
                {
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(false, 'divRequestDetail');LoadingOff();", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(false, 'OrderHead');LoadingOff();", true);
                   
                    //ddlRequestByUserID.Enabled = false;
                    //ddlStatus.Enabled = false;
                    //chkSelectTemplate.Visible = false;
                    //btnSaveAsTemplate.Enabled = false;
                    //UC_DateRequestDate.Attributes.Add("style", "display:none");
                    //ddlCompany.Enabled = false;
                    //ddlSites.Enabled = false;
                    //disabled
                    Toolbar1.SetSaveRight(true, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                    // Toolbar1.SetEditRight(true, "Not Allowed");
                }


                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemode" + Session.SessionID, "changemode(false, 'divRequestDetail');LoadingOff();", true); }

                //if (Convert.ToInt32(ddlStatus.SelectedItem.Value) >= 3)
                //{
                //    CheckBoxApproved.Enabled = false;
                //    CheckBoxRejected.Enabled = false;
                //    txtApprovalRemark.Enabled = false;
                //    btnSaveApproval.Attributes.Add("onclick", "showAlert('Not Allowed');");
                //    btnSaveApproval.Attributes.Add("class", "Off buttonON");
                //}
                //else
                //{
                //    CheckBoxApproved.Enabled = true;
                //    CheckBoxRejected.Enabled = true;
                //    txtApprovalRemark.Enabled = true;
                //    btnSaveApproval.Attributes.Add("onclick", "jsSaveApproval();");
                //    btnSaveApproval.Attributes.Add("class", "buttonON");
                //}
            }

            if (Session["POstate"] != null)
            {
                if (Session["POstate"].ToString() == "Add")
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    // if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 1;

                    //if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 1;
                    if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 2;
                    lblRequestNo.Text = "Generate when Save";
                    UC_DateRequestDate.Date = DateTime.Now;

                    DateTime crntdt = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    txtRequestDate.Text = crntdt.ToString("dd-MMM-yyyy");// .ToShortDateString();

                    //UC_ExpDeliveryDate.Date = DateTime.Now.AddDays(7);
                    //  ddlRequestType.SelectedIndex = 2;
                    ddlRequestByUserID.DataSource = null;
                    ddlRequestByUserID.DataBind();
                    // ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                    if (hdnselectedDept.Value == "") hdnSelectedWarehouse.Value = "0";
                    //ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(hdnselectedDept.Value));
                    ddlRequestByUserID.DataSource = FillCurrentUserList(Convert.ToInt64(hdnSelectedWarehouse.Value));
                    ddlRequestByUserID.DataBind();
                    ddlRequestByUserID.SelectedIndex = ddlRequestByUserID.Items.IndexOf(ddlRequestByUserID.Items.FindByValue(profile.Personal.UserID.ToString()));
                    if (profile.Personal.UserType == "User")
                    {
                        ddlRequestByUserID.Enabled = false;
                    }                    
                }
            }
        }

        public List<vGetUserProfileByUserID> FillCurrentUserList(long WarehouseID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();               
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }

        public void GetDocumentByPOID(long OrderID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UC_AttachmentDocument1.FillDocumentByObjectNameReferenceID(OrderID, "PurchaseOrder", "PurchaseOrder");
        }

        [WebMethod]
        public static long WMSaveRequestHead(object objPOHead)
        {
            long result = 0;           
            int RSLT = 0; long POID = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objPOHead;
                if (HttpContext.Current.Session["POID"] != null)
                {
                    if (HttpContext.Current.Session["POID"].ToString() == "0")
                    {
                        POHead.CreatedBy = profile.Personal.UserID;
                        POHead.Creationdate = DateTime.Now;
                    }
                    else
                    {
                        POHead.Id = Convert.ToInt64(HttpContext.Current.Session["POID"].ToString());
                        POHead.LastModifiedBy = profile.Personal.UserID;
                        POHead.LastModifiedDt = DateTime.Now;
                    }
                    POHead.Warehouse=Convert.ToInt64(d["StoreId"]);
                    POHead.OrderNumber = d["OrderNumber"].ToString();
                    POHead.Priority = d["Priority"].ToString();
                    POHead.Status = Convert.ToInt64(d["Status"]);
                    POHead.Title = d["Title"].ToString();
                    POHead.POdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    POHead.CreatedBy = Convert.ToInt64(d["RequestBy"]);
                    POHead.Remark = d["Remark"].ToString();
                    POHead.Deliverydate = Convert.ToDateTime(d["Deliverydate"]);
                    POHead.ContactId1 = Convert.ToInt64(d["ContactId1"].ToString());                    
                    POHead.AddressId = Convert.ToInt64(d["AddressId"].ToString());
                    //POHead.Con2 = d["ContactId2"].ToString();
                    //POHead.PaymentID = Convert.ToInt64(dictionary["PaymentID"].ToString());
                    POHead.TotalQty = Convert.ToDecimal(d["TotalQty"].ToString());
                    POHead.GrandTotal = Convert.ToDecimal(d["GrandTotal"].ToString());
                    POHead.VendorID = Convert.ToInt64(d["VendorID"]);
                    POHead.CompanyID = profile.Personal.CompanyID;

                    long CustID = Inbound.getCustomerofUser(profile.Personal.UserID, profile.Personal.CompanyID, profile.DBConnection._constr);
                    POHead.CustomerID = CustID;

                    //if (Convert.ToInt64(d["Status"]) == 1) { }  //For User Defined Order Number
                    //else
                    //{ RequestHead.OrderNo = objService.GetNewOrderNo(Convert.ToInt64(dictionary["StoreId"]), profile.DBConnection._constr); }

                    POHead.orderType = "WMS";
                    POHead.SubTotal = Convert.ToDecimal(d["SubTotal"].ToString());
                    POHead.DiscountOnSubTotal = Convert.ToDecimal(d["DiscountOnSubTotal"].ToString());
                    POHead.DiscountOnSubTotalPercent = Convert.ToBoolean(d["DiscountOnSubTotalPercent"].ToString());
                    POHead.TotalDiscount = Convert.ToDecimal(d["TotalDiscount"].ToString());
                    POHead.TotalAfterDiscount = Convert.ToDecimal(d["TotalAfterDiscount"].ToString());
                    POHead.TotalTax = Convert.ToDecimal(d["TotalTax"].ToString());
                    POHead.OtherChargesDescription = d["OtherChargesDescription"].ToString();
                    POHead.OtherCharges = Convert.ToDecimal(d["OtherCharges"].ToString());
                    POHead.ShippingCharges = Convert.ToDecimal(d["ShippingCharges"].ToString());
                    POHead.TotalAmount = Convert.ToDecimal(d["TotalAmount"].ToString());

                    POHead.Object = "PurchaseOrder";

                    POID = Inbound.SetIntotPurchaseOrderHead(POHead, profile.DBConnection._constr);

                    if (POID > 0)
                    {
                        RSLT = Inbound.FinalSavePODetail(HttpContext.Current.Session.SessionID, ObjectName, POID, profile.Personal.UserID.ToString(), Convert.ToInt16(POHead.Status), Convert.ToInt64(POHead.Warehouse), profile.DBConnection._constr);
                        if (RSLT == 1 || RSLT == 2) { result = POID; }  //"Request saved successfully";
                        else if (RSLT == 3) { result = -3; } //"Request saved successfully. Email Notification Failed..."
                        else if (RSLT == 0) { result = 0; }  //"Some error occurred";
                        iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                        DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, POID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMSaveRequestHead");
                result = 0; // "Some error occurred";
            }
            finally { 
                Inbound.Close();
                //HttpContext.Current.Session["POID"] = RequestID; 
            }
            return result;
        }
        #endregion

        #region PODetail

        protected void FillGrid1ByRequestID(long RequestID, long WarehouseID)
        {
            iInboundClient Inbound = new iInboundClient();              
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<WMS_SP_GetPartDetail_ForPO_Result> RequestPartList = new List<WMS_SP_GetPartDetail_ForPO_Result>();                
                RequestPartList = Inbound.GetRequestPartDetailByRequestID(RequestID, WarehouseID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();                
                Grid1.DataSource = RequestPartList;
                Grid1.DataBind();
               
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "FillGrid1ByRequestID"); }
            finally { Inbound.Close(); }
        }

        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();            
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                List<WMS_SP_GetPartDetail_ForPO_Result> POPartList = new List<WMS_SP_GetPartDetail_ForPO_Result>();               
                if (hdn.Value == "")
                {
                    POPartList = Inbound.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    POPartList = Inbound.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(hdnSelectedWarehouse.Value), profile.DBConnection._constr).ToList();                    
                }

                //Add by Suresh
                if (hdnprodID.Value != "")
                {
                    POPartList = Inbound.AddPartIntoRequest_TempData(hdnprodID.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                    hdnprodID.Value = "";
                }

                if (hdnChngDept.Value == "0x00x0")
                {
                    Inbound.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    POPartList = null;
                }
                hdnChngDept.Value = "";
                var chngdpt = "1x1";
                hdnChngDept.Value = chngdpt;

                if (hdnChangePrdQtyPrice.Value == "1")
                {
                    POPartList = Inbound.GetRequestPartDetailByRequestID(long.Parse(Session["POID"].ToString()), long.Parse(hdnSelectedWarehouse.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();                    
                }

                Grid1.DataSource = POPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PurchaseOrder.aspx", "Grid1_OnRebind"); }
            finally { Inbound.Close(); }
        }

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();             
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                   Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[8] as Obout.Grid.GridDataControlFieldCell;

                    DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;
                    HiddenField hdnUOM = cell.FindControl("hdnMyUOM") as HiddenField;
                    Label rowQtySpn = e.Row.Cells[10].FindControl("rowQtyTotal") as Label;                                        
                    TextBox txtUsrQty = e.Row.Cells[7].FindControl("txtUsrQty") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);                   
                    decimal moq = Convert.ToDecimal(e.Row.Cells[5].Text);

                    /*New Price Added*/
                    TextBox txtUsrPrice = e.Row.Cells[11].FindControl("txtUsrPrice") as TextBox; //txtUsrPrice.Enabled = false;   //Product Price
                    Label rowPriceTotal = e.Row.Cells[12].FindControl("rowPriceTotal") as Label;

                    /*New Code In WMS Start*/
                    Label rowPriceTaxTotal = e.Row.Cells[17].FindControl("rowPriceTaxTotal") as Label;
                    Label rowTotalTax = e.Row.Cells[15].FindControl("rowTotalTax") as Label;
                    hdnSelectedRowTax.Value = rowTotalTax.Text;

                    hdnSelectedRowPercnt.Value = "";
                    if (hdnSelectedRowTax.Value == "0.00")
                    {
                        decimal percent = Inbound.GetTaxofProduct(ProdID, profile.DBConnection._constr);
                        hdnSelectedRowPercnt.Value = percent.ToString();
                    }

                    long sequence = Convert.ToInt64(e.Row.Cells[1].Text);
                    /*New Code In WMS End*/

                   // int pricechange = Inbound.GetDeptPriceChange(long.Parse(hdnselectedDept.Value), profile.DBConnection._constr);
                  //  if (pricechange == 1)
                    //{
                        txtUsrPrice.Enabled = true;
                    //}
                    //else { txtUsrPrice.Enabled = false; }
                    
                    DataSet dsUOM = new DataSet();
                    dsUOM = Inbound.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);

                    ddl.DataSource = dsUOM;
                    ddl.DataTextField = "Description";
                    ddl.DataValueField = "UMOGroup";
                    ddl.DataBind();
                    //ddl.SelectedValue = e.Row.Cells[6].Text;

                    //  string SelTmplt = Session["TemplateID"].ToString();

                    //Grid1.Columns[16].Visible = false;

                    if (Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()) > 0)
                    {
                        long ReqId = Convert.ToInt64(HttpContext.Current.Session["POID"].ToString());

                        string selectedUomTmpl = Inbound.GetSelectedUom(ReqId, ProdID, sequence,profile.DBConnection._constr);
                        if (selectedUomTmpl != "0")
                        {
                            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(selectedUomTmpl.ToString()));
                        }
                        else
                        {
                            ddl.SelectedIndex = 2;
                        }
                        rowQtySpn.Text = txtUsrQty.Text;
                        decimal UserQty = decimal.Parse(txtUsrQty.Text.ToString());
                        int SelInd = 0;
                        SelInd = ddl.SelectedIndex;
                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[SelInd]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[SelInd]["UOMID"].ToString());

                        rowPriceTotal.Text = e.Row.Cells[14].Text;
                        if (hdnOrderStatus.Value == "45")
                        {
                            UCProductSearch1.Visible = true;

                            decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                            decimal UsrQty = decimal.Parse(txtUsrQty.Text.ToString()); //SelectedQty * rowQty;
                            decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());

                            hdnSelectedQty.Value = SelectedQty.ToString();
                            rowQtySpn.Text = UsrQty.ToString();

                            //if (UsrQty > CrntStock)
                            //{ rowQtySpn.Text = "0"; }
                            //else
                            //{
                                rowQtySpn.Text = UsrQty.ToString();
                                txtUsrQty.Text = Convert.ToString(UsrQty / SelectedQty);
                            //}

                                ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");
                                txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");
                                txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + ProdID + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");
                        }
                        else
                        {
                            txtUsrQty.Text = Convert.ToString(UserQty / SelectedQty);
                            UCProductSearch1.Visible = false;
                            txtUsrQty.Enabled = false;
                            txtUsrPrice.Enabled = false;
                            ddl.Enabled = false;
                        }
                        //  Grid1.Columns[16].Visible = true;
                    }                    
                    else
                    {
                        ddl.SelectedIndex = 0;

                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[0]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[0]["UOMID"].ToString());

                        decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                        decimal UsrQty = SelectedQty * rowQty;
                        decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());

                        hdnSelectedQty.Value = SelectedQty.ToString();
                        rowQtySpn.Text = UsrQty.ToString();

                        //if (UsrQty > CrntStock)
                        //{ rowQtySpn.Text = "0"; }
                        //else
                        //{
                            rowQtySpn.Text = UsrQty.ToString();
                            //Price = decimal.Parse(rowQtySpn.Text.ToString()) * decimal.Parse(txtUsrPrice.Text.ToString());
                            //rowPriceTotal.Text = Price.ToString();

                        //}
                            ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");
                            txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");
                            txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + ProdID + ",'" + hdnSelectedRowPercnt.Value + "','" + rowTotalTax.ClientID.ToString() + "')");                        
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PurchaseOrder.aspx", "Grid1_OnRowDataBound"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static int WMRemovePartFromRequest(Int32 Sequence)
        {
            int editOrder = 0;
            iInboundClient Inbound = new iInboundClient();             
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (long.Parse(HttpContext.Current.Session["POID"].ToString()) > 0)
                {
                    tPurchaseOrderHead RequestHead = new tPurchaseOrderHead();                    
                    long ReqID = long.Parse(HttpContext.Current.Session["POID"].ToString());
                    RequestHead = Inbound.GetOrderHeadByOrderID(Convert.ToInt64(HttpContext.Current.Session["POID"].ToString()), profile.DBConnection._constr);
                    string Status = RequestHead.Status.ToString();
                    if (Status == "1")
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        Inbound.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                        editOrder = 1;
                    }
                    else { editOrder = 0; }
                }
                else
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    Inbound.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                    editOrder = 1;
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMRemovePartFromRequest");
            }
            finally { Inbound.Close(); }
            return editOrder;
        }

        [WebMethod]
        public static void WMUpdRequestPart(object objRequest)
        {
            iInboundClient Inbound = new iInboundClient();  
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                string uom = Inbound.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);

                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); //PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);
                PartRequest.AmountAfterTax = Convert.ToDecimal(dictionary["AmountAfterTax"]);

                Inbound.UpdatePartRequest_TempData1(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMUpdRequestPart"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static decimal WMGetTotal()
        {
            iInboundClient Inbound = new iInboundClient();  
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                tot = Inbound.GetTotalFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMGetTotal"); }
            finally { Inbound.Close(); }
            return tot;
        }

        [WebMethod]
        public static decimal WMGetTotalQty()
        {
            iInboundClient Inbound = new iInboundClient();  
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                tot = Inbound.GetTotalQTYFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMGetTotalQty"); }
            finally { Inbound.Close(); }
            return tot;
        }

        [WebMethod]
        public static void WMUpdRequestPartPrice(object objRequest, int ProdID)
        {
            iInboundClient Inbound = new iInboundClient();  
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();
                string uom = Inbound.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);
                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                 PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);
                PartRequest.Price = Convert.ToDecimal(dictionary["Price"]);
                PartRequest.AmountAfterTax = Convert.ToDecimal(dictionary["AmountAfterTax"]);
                // PartRequest.IsPriceChange = Convert.ToInt16(dictionary["IsPriceChange"]);
                decimal price = Convert.ToDecimal(dictionary["Price"]);
                int ISPriceChangedYN = Inbound.IsPriceChanged(ProdID, price, profile.DBConnection._constr);
                if (ISPriceChangedYN == 0) { PartRequest.IsPriceChange = 0; }
                else { PartRequest.IsPriceChange = 1; }
                Inbound.UpdatePartRequest_TempData12(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMUpdRequestPart"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static string[] webGetFooterTotal(decimal discountonsubtotal, string discountonsubtotalchk)
        {
            iInboundClient Inbound = new iInboundClient(); 
            CustomProfile profile = CustomProfile.GetProfile();
            string[] returnresult;
            returnresult = new string[4];
            returnresult[0] = "0.00"; /* Sub Total 1*/
            returnresult[1] = "0.00"; /* Discount on SubTotal 1*/
            returnresult[2] = "0.00"; /* Footer Taxable Amount or SubTotal 2*/
            returnresult[3] = "0.00"; /* Footer Tax Amount*/

            try
            {
                decimal taxableamount = 0;
                decimal tot=0;
                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                tot = Inbound.GetTotalFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                returnresult[0] = Convert.ToString(tot);
                if (discountonsubtotalchk == "true")
                {
                    taxableamount = Convert.ToDecimal(returnresult[0]) - (Convert.ToDecimal(returnresult[0]) * (discountonsubtotal / Convert.ToDecimal(100.00)));
                    returnresult[1] = (Convert.ToDecimal(returnresult[0]) * (discountonsubtotal / Convert.ToDecimal(100.00))).ToString();
                }
                if (discountonsubtotalchk == "false")
                {
                    taxableamount = Convert.ToDecimal(returnresult[0]) - discountonsubtotal;
                    returnresult[1] = discountonsubtotal.ToString();
                }
                returnresult[2] = taxableamount.ToString();

                UCApplyTaxService.iUCApplyTaxClient taxserviceclient = new UCApplyTaxService.iUCApplyTaxClient();
                string FooterTaxAmount = "0";
                FooterTaxAmount = taxserviceclient.UpdateCalculatedTaxList("PurchaseOrderTotalTax", HttpContext.Current.Session.SessionID, 0, Convert.ToDecimal(taxableamount), "*", profile.DBConnection._constr).ToString();
                taxserviceclient.Close();

                returnresult[3] = FooterTaxAmount;

                return returnresult;
            }
            catch { return returnresult; }
            finally { Inbound.Close(); }

        }
        
        [WebMethod]
        public static string WMGetTotalForTax(int Sequence)
        {
            iInboundClient Inbound = new iInboundClient();
            decimal tot = 0; long PrdID = 0; string totPrdID = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                tot = Inbound.GetTotalQTYofSequenceFromTempData(Sequence,HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                PrdID = Inbound.GetPrdIDofSequenceFromTempData(Sequence, HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMGetTotalQty"); }
            finally { Inbound.Close(); }
            return totPrdID = tot + "," + PrdID;
        }

        [WebMethod]
        public static List<vGetUserProfileByUserID> WMFillUserList(long WarehouseID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListByWarehouse(WarehouseID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.GroupBy(x => x.userID).Select(x => x.FirstOrDefault()).ToList();
                //  UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }

        protected void gvApprovalRemarkBind(long RequestID)
        {
            //iPartRequestClient objService = new iPartRequestClient();    //Currently Not Working
            //CustomProfile profile = CustomProfile.GetProfile();
            //gvApprovalRemark.DataSource = null;
            //gvApprovalRemark.DataBind();
            //DataSet dsGetApprovalDetail = new DataSet();
            //if (profile.Personal.UserType == "Admin")
            //{
            //    dsGetApprovalDetail = objService.GetApprovalDetailsNewAdmin(RequestID, profile.DBConnection._constr);
            //}
            //else
            //{
            //    dsGetApprovalDetail = objService.GetApprovalDetailsNew(RequestID, profile.Personal.UserID, profile.DBConnection._constr);
            //}
            //gvApprovalRemark.DataSource = dsGetApprovalDetail;
            //gvApprovalRemark.DataBind();
        }

        protected void gvApprovalRemark_OnRebind(object sender, EventArgs e)
        {
            gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()));
        }

        protected void GVInboxPOR_OnRebind(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iInboundClient Inbound = new iInboundClient();

            long RequestID = long.Parse(Session["POID"].ToString());

            GVInboxPOR.DataSource = Inbound.GetCorrespondance(RequestID, profile.DBConnection._constr);
            GVInboxPOR.DataBind();
        }
        #endregion

        #region ASN
        protected void imgBtnViewASN_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            var asnID = imgbtn.ToolTip.ToString();
            Session["ASNID"] = asnID.ToString();

           this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "window.open('../WMS/ASNPoDetails.aspx', null, 'height=380px, width=1010px,status= 0, resizable= 1, scrollbars=1, toolbar=0,location=center,menubar=0, screenX=300; screenY=50')", true);
        }
        #endregion
    }
}