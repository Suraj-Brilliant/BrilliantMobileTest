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
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.WMS
{
    public partial class SalesOrder : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string ObjectName = "SalesOrder";
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
                UC_AttachmentDocument1.ClearDocument("SalesOrder");
                if (Session["SOID"] != null)
                {
                    if (Session["SOID"].ToString() != "0")
                    {
                        lblApprovalDate.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
                        GetSOHead();
                        gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()));
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
            if (!IsPostBack)
            {
                BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
                //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
                CustomProfile profile = CustomProfile.GetProfile();
                //  ddlSites.Attributes.Add("onchange", "jsFillUsersList();jsFillEnginList();");
                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");
                Outbound.ClearTempDataFromDBNEWSO(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
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
        {//Customer 
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<BrilliantWMS.WMSOutbound.mWarehouseMaster> WarehouseList = new List<WMSOutbound.mWarehouseMaster>(); //= new List<mWarehouseMaster>();
                long UserID = profile.Personal.UserID;
                WarehouseList = Outbound.GetUserWarehouseSO(UserID, profile.DBConnection._constr).ToList();
                ddlSites.DataSource = WarehouseList;
                ddlSites.DataBind();
                ListItem lstW = new ListItem { Text = "-Select-", Value = "0" };
                ddlSites.Items.Insert(0, lstW);

                /*Fill Client*/
                List<BrilliantWMS.WMSOutbound.mClient> ClientList = new List<WMSOutbound.mClient>();
                ClientList = Outbound.GetCompanyWiseClient(profile.Personal.CompanyID, profile.DBConnection._constr).ToList();
                //ddlCompany.DataSource = ClientList;
                //ddlCompany.DataBind();
                //ListItem lstV = new ListItem { Text = "-Select-", Value = "0" };
                //ddlCompany.Items.Insert(0, lstV);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "fillWarehouse");
            }
            finally { Outbound.Close(); }
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
        {//Customer
            Page objp = new Page();
            objp.Session["ClientID"] = Vendor;
            return Vendor;
        }

        [WebMethod]
        public static List<BrilliantWMS.WMSOutbound.mStatu> WMFillStatus()
        {
            string state = HttpContext.Current.Session["SOstate"].ToString();
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();

            List<BrilliantWMS.WMSOutbound.mStatu> StatusList = new List<WMSOutbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                if (HttpContext.Current.Session["SOID"].ToString() == "0" && state == "Add")
                {
                    StatusList = Outbound.GetStatusListForOutbound(ObjectName, "SalesOrder", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();

                    long CompanyID = profile.Personal.CompanyID;
                    int Sequence = Outbound.GetWorkflowSequenceOfSO("SalesOrder", CompanyID, profile.DBConnection._constr);
                    StatusList = StatusList.Where(s => s.Sequence == Sequence || s.Sequence == 1).ToList();
                    //StatusList = StatusList.Where(s => s.ID == 1 || s.ID == 2).ToList();
                }
                else if (HttpContext.Current.Session["SOID"].ToString() != "0" && state == "Edit")
                {
                    if (HttpContext.Current.Session["OrderStatus"].ToString() == "46")//ChangeStatus 
                    {
                        StatusList = Outbound.GetStatusListForOutbound(ObjectName, "SalesOrder", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                        long CompanyID = profile.Personal.CompanyID;
                        int Sequence = Outbound.GetWorkflowSequenceOfSO("SalesOrder", CompanyID, profile.DBConnection._constr);
                        StatusList = StatusList.Where(s => s.Sequence == Sequence || s.Sequence == 1).ToList();
                    }
                    else
                    {
                        StatusList = Outbound.GetStatusListForOutbound("", "SalesOrder,POSO,", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                    }
                }
                else if (HttpContext.Current.Session["SOID"].ToString() != "0" && state == "View")
                {
                    StatusList = Outbound.GetStatusListForOutbound("", "SalesOrder,POSO,Return", "", 0, profile.DBConnection._constr).ToList();
                }

                BrilliantWMS.WMSOutbound.mStatu select = new WMSOutbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMFillStatus");
            }
            finally { Outbound.Close(); }
            return StatusList;
        }

        #endregion

        #region ToolbarCode
        [WebMethod]
        public static void WMpageAddNew()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["SOID"] = 0;
                HttpContext.Current.Session["SOstate"] = "Add";
                HttpContext.Current.Session["ClientID"] = "0";
                Outbound.ClearTempDataFromDBNEWSO(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMpageAddNew");
            }
            finally { Outbound.Close(); }
        }
        #endregion
        #region SOHead
        protected void GetSOHead()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            // iOutboundClient Outbound = new WMSInbound.iOutboundClient();// iInboundClient Inbound = new iInboundClient();
            BrilliantWMS.WMSOutbound.tOrderHead SOHead = new WMSOutbound.tOrderHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                SOHead = Outbound.GetSoHeadBySOID(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), profile.DBConnection._constr);
                hdnOrderStatus.Value = SOHead.Status.ToString(); Session["OrderStatus"] = hdnOrderStatus.Value;
                FillGrid1ByRequestID(SOHead.Id, Convert.ToInt64(SOHead.StoreId));

                txtTitle.Text = SOHead.Title;
                long SiteID = long.Parse(SOHead.StoreId.ToString());

                iUCCommonFilterClient objCommon = new iUCCommonFilterClient();
                // long CompanyID = objCommon.GetCompanyIDFromSiteID(SiteID, profile.DBConnection._constr);

                if (profile.Personal.UserType == "Super Admin" || profile.Personal.UserType == "Admin")
                {
                    List<BrilliantWMS.WMSOutbound.mClient> ClientList = new List<WMSOutbound.mClient>();
                    ClientList = Outbound.GetCompanyWiseClient(profile.Personal.CompanyID, profile.DBConnection._constr).ToList();
                    //ddlCompany.DataSource = ClientList;
                    //ddlCompany.DataBind();
                    //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(SOHead.ClientID.ToString()));

                    BrilliantWMS.WMSOutbound.mClient cl = new WMSOutbound.mClient();
                    cl = Outbound.GetClientNameByID(long.Parse(SOHead.ClientID.ToString()), profile.DBConnection._constr);
                    txtClientName.Text = cl.Name.ToString();
                    Session["ClientID"] = SOHead.ClientID.ToString();
                    //ddlCompany.Enabled = false;

                    long UID = profile.Personal.UserID;
                    List<BrilliantWMS.WMSOutbound.mWarehouseMaster> WarehouseList = new List<WMSOutbound.mWarehouseMaster>();
                    if (profile.Personal.UserType == "Admin")
                    {
                        WarehouseList = Outbound.GetWarehouseNameByUserIDSO(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    }
                    else if (profile.Personal.UserType == "Super Admin")
                    {
                        WarehouseList = Outbound.GetAllWarehouseListSO(profile.DBConnection._constr).ToList();
                    }
                    ddlSites.DataSource = WarehouseList;
                    ddlSites.DataBind(); Session["WarehouseID"] = SOHead.StoreId.ToString();
                    //  ddlSites.Enabled = false;
                }

                ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(SOHead.StoreId.ToString())); hdnSelectedWarehouse.Value = SOHead.StoreId.ToString();
                if (hdnOrderStatus.Value == "46") { lblRequestNo.Text = "Generate when Save"; }
                else
                {
                    lblRequestNo.Text = SOHead.Id.ToString();
                }
                ddlStatus.DataSource = WMFillStatus();
                ddlStatus.DataBind();
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "divVisibility123", "divVisibility()");

                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(SOHead.Status.ToString()));

                UC_DateRequestDate.Date = SOHead.Orderdate;
                txtRequestDate.Text = Convert.ToString(SOHead.Orderdate.Value.ToString("dd-MMM-yyyy"));

                //      ddlRequestType.SelectedIndex = ddlRequestType.Items.IndexOf(ddlRequestType.Items.FindByValue(RequestHead.Priority.ToString()));
                if (hdnOrderStatus.Value == "46")
                {
                    ddlRequestByUserID.DataSource = FillCurrentUserList(Convert.ToInt64(hdnSelectedWarehouse.Value));
                }
                else
                {
                    ddlRequestByUserID.DataSource = WMFillUserList(Convert.ToInt64(SOHead.StoreId)); hdnselectedDept.Value = SOHead.StoreId.ToString();
                }
                ddlRequestByUserID.DataBind();
                ddlRequestByUserID.SelectedIndex = ddlRequestByUserID.Items.IndexOf(ddlRequestByUserID.Items.FindByValue(SOHead.CreatedBy.ToString()));

                txtRemark.Text = SOHead.Remark;

                txtCustOrderRefNo.Text = SOHead.OrderNumber;
                UC_ExpDeliveryDate.Date = SOHead.Deliverydate; //if (RequestHead.Status >= 2) { Page.ClientScript.RegisterStartupScript(this.GetType(), "reset", " disableExpDeliveryDate();", true); }


                ddlAddress.SelectedIndex = ddlAddress.Items.IndexOf(ddlAddress.Items.FindByValue(SOHead.AddressId.ToString())); hdnSelAddress.Value = SOHead.AddressId.ToString();
                ddlContact1.SelectedIndex = ddlContact1.Items.IndexOf(ddlContact1.Items.FindByValue(SOHead.ContactId1.ToString())); hdnselectedCont1.Value = SOHead.ContactId1.ToString();

                /*New Change Code*/
                long EdtCon1 = long.Parse(SOHead.ContactId1.ToString());
                long EdtAddress = long.Parse(SOHead.AddressId.ToString());

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

                txtTotalQty.Text = SOHead.TotalQty.ToString();
                txtGrandTotal.Text = SOHead.GrandTotal.ToString();

                lblSubTotal.Text = SOHead.SubTotal.ToString();
                txtDiscountOnSubTotal.Text = SOHead.DiscountOnSubTotal.ToString();
                if (SOHead.DiscountOnSubTotalPercent == true) { chkboxDiscountOnSubTotal.Checked = true; }
                else { chkboxDiscountOnSubTotal.Checked = false; }
                lblDiscountOnSubTotal.Text = SOHead.TotalDiscount.ToString();
                lblSubTotal2.Text = SOHead.TotalAfterDiscount.ToString();
                lblTaxOnSubTotal.Text = SOHead.TotalTax.ToString();
                txtAdditionalChargeDescription.Text = SOHead.OtherChargesDescription.ToString();
                txtAdditionalCharges.Text = SOHead.OtherCharges.ToString();
                txtShippingCharges.Text = SOHead.ShippingCharges.ToString();
                lblGrandTotal.Text = SOHead.TotalAmount.ToString();

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

                GetDocumentBySOID(long.Parse(SOHead.Id.ToString()));
                //GetApprovalDetails();

                //GetDeliveryDetails(RequestHead.Id);

            }
            catch { }
            finally { Outbound.Close(); }
        }

        protected void divVisibility()
        {
            divApprovalHead.Attributes.Add("style", "display:none");
            divApprovalDetail.Attributes.Add("style", "display:none");

            divIssueHead.Attributes.Add("style", "display:none");
            divIssueDetail.Attributes.Add("style", "display:none");

            divCorrespondanceHead.Attributes.Add("style", "display:none");
            divCorrespondanceDetails.Attributes.Add("style", "display:none");
            //linkCorrespondanceDetail.Attributes["innerHTML"] = "Expand";
            //divCorrespondanceDetails.Attributes["class"] = "divDetailCollapse";

            //divReceiptHead.Attributes.Add("style", "display:none");
            //divReceiptDetail.Attributes.Add("style", "display:none");

            //divConsumptionHead.Attributes.Add("style", "display:none");
            //divConsumptionDetail.Attributes.Add("style", "display:none");

            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Approved")) > 0)
            // if (ddlStatus.SelectedIndex == 3)
            if (ddlStatus.SelectedIndex == 2 || ddlStatus.SelectedValue == "22" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "2" || ddlStatus.SelectedValue == "21" || ddlStatus.SelectedValue == "4" || ddlStatus.SelectedIndex == 21 || ddlStatus.SelectedIndex == 22 || ddlStatus.SelectedIndex == 4 || ddlStatus.SelectedIndex == 38 || ddlStatus.SelectedIndex == 40 || ddlStatus.SelectedIndex == 32 || ddlStatus.SelectedIndex == 48 )
            {
                BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();

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
            if (ddlStatus.SelectedValue == "49")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderHead');LoadingOff();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'OrderCalculation');LoadingOff();", true);
                
                Toolbar1.SetClearRight(false, "Not Allowed");
                Toolbar1.SetEditRight(false, "Not Allowed");
                Toolbar1.SetSaveRight(true, "Not Allowed");
            }
            // if (ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Fully Issued")) > 0)
            if (ddlStatus.SelectedValue == "7" || ddlStatus.SelectedValue == "10" || ddlStatus.SelectedValue == "8" || ddlStatus.SelectedValue == "25" || ddlStatus.SelectedValue == "26" || ddlStatus.SelectedValue == "28" ||ddlStatus.SelectedValue == "37" || ddlStatus.SelectedValue == "38" || ddlStatus.SelectedValue == "40" || ddlStatus.SelectedValue == "48" || ddlStatus.SelectedValue == "32")
            {
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
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 3 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 4 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 21 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 22 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 2 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 38 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 40 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 32 || Convert.ToInt32(ddlStatus.SelectedItem.Value) == 48)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divRequestDetail');LoadingOff();", true);

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
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) == 46)
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

            if (Session["SOstate"] != null)
            {
                if (Session["SOstate"].ToString() == "Add")
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
        public void GetDocumentBySOID(long OrderID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UC_AttachmentDocument1.FillDocumentByObjectNameReferenceID(OrderID, "SalesOrder", "SalesOrder");
        }

        [WebMethod]
        public static long WMSaveRequestHead(object objSOHead, string clientName)
        {
            long result = 0;
            int RSLT = 0; long SOID = 0;
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            // iOutboundClient Outbound = new WMSInbound.iOutboundClient(); //iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.tOrderHead SOHead = new WMSOutbound.tOrderHead();

                /*New Change For Client Name */
                BrilliantWMS.WMSOutbound.mClient objCl = new WMSOutbound.mClient();
                objCl = Outbound.GetClientDetailByClientName(clientName, profile.DBConnection._constr);
                //if (objCl == null)
                //{
                //    result = -5;
                //}
                //else
                //{
                    long clientID=long.Parse(HttpContext.Current.Session["ClientID"].ToString());
                    if (clientID != null && clientID != 0)
                    {
                        objCl = Outbound.GetClientNameByID(clientID, profile.DBConnection._constr);
                        SOHead.ClientID = objCl.ID;
                    }
                    else
                    {
                        BrilliantWMS.WMSOutbound.mClient cl = new WMSOutbound.mClient();

                        cl.Name = clientName.ToString();
                        cl.Active = "Y";
                        cl.CreatedBy = profile.Personal.UserID.ToString();
                        cl.CreationDate = DateTime.Now;
                        cl.CompanyID = profile.Personal.CompanyID;
                        cl.Code = clientName.ToString();
                        cl.Sector = 54;
                        cl.Creditdays = 30;
                        cl.TurnOver = 10000;
                        cl.CompType = 1;
                        //cl.CustomerID = profile.Personal.CustomerID;
                        cl.AccountType = "Client";
                        long clID = Outbound.SaveNewClientDetails(cl, profile.DBConnection._constr);
                        SOHead.ClientID = clID;
                    }
                    /*New Change For Client Name */

                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d = (Dictionary<string, object>)objSOHead;
                    if (HttpContext.Current.Session["SOID"] != null)
                    {
                        if (HttpContext.Current.Session["SOID"].ToString() == "0")
                        {
                            SOHead.CreatedBy = profile.Personal.UserID;
                            SOHead.Creationdate = DateTime.Now;
                        }
                        else
                        {
                            SOHead.Id = Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString());
                            SOHead.LastModifiedBy = profile.Personal.UserID;
                            SOHead.LastModifiedDt = DateTime.Now;
                        }
                        SOHead.StoreId = Convert.ToInt64(d["StoreId"]);
                        SOHead.OrderNumber = d["OrderNumber"].ToString();
                        SOHead.Priority = d["Priority"].ToString();
                        SOHead.Status = Convert.ToInt64(d["Status"]);
                        SOHead.Title = d["Title"].ToString();
                        SOHead.Orderdate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        SOHead.CreatedBy = Convert.ToInt64(d["RequestBy"]);
                        SOHead.Remark = d["Remark"].ToString();
                        SOHead.Deliverydate = Convert.ToDateTime(d["Deliverydate"]);
                        SOHead.ContactId1 = Convert.ToInt64(d["ContactId1"].ToString());
                        SOHead.AddressId = Convert.ToInt64(d["AddressId"].ToString());
                        //POHead.Con2 = d["ContactId2"].ToString();
                        //POHead.PaymentID = Convert.ToInt64(dictionary["PaymentID"].ToString());
                        SOHead.TotalQty = Convert.ToDecimal(d["TotalQty"].ToString());
                        SOHead.GrandTotal = Convert.ToDecimal(d["GrandTotal"].ToString());
                        //SOHead.ClientID = Convert.ToInt64(d["ClntID"]);    /* New Change */
                        SOHead.CompanyID = profile.Personal.CompanyID;

                        //if (Convert.ToInt64(d["Status"]) == 1) { }  //For User Defined Order Number
                        //else
                        //{ RequestHead.OrderNo = objService.GetNewOrderNo(Convert.ToInt64(dictionary["StoreId"]), profile.DBConnection._constr); }

                        SOHead.orderType = "WMS";
                        SOHead.SubTotal = Convert.ToDecimal(d["SubTotal"].ToString());
                        SOHead.DiscountOnSubTotal = Convert.ToDecimal(d["DiscountOnSubTotal"].ToString());
                        SOHead.DiscountOnSubTotalPercent = Convert.ToBoolean(d["DiscountOnSubTotalPercent"].ToString());
                        SOHead.TotalDiscount = Convert.ToDecimal(d["TotalDiscount"].ToString());
                        SOHead.TotalAfterDiscount = Convert.ToDecimal(d["TotalAfterDiscount"].ToString());
                        SOHead.TotalTax = Convert.ToDecimal(d["TotalTax"].ToString());
                        SOHead.OtherChargesDescription = d["OtherChargesDescription"].ToString();
                        SOHead.OtherCharges = Convert.ToDecimal(d["OtherCharges"].ToString());
                        SOHead.ShippingCharges = Convert.ToDecimal(d["ShippingCharges"].ToString());
                        SOHead.TotalAmount = Convert.ToDecimal(d["TotalAmount"].ToString());
                        SOHead.Object = "SalesOrder";

                        long MrkRtrnSts = Convert.ToInt64(d["Status"]);
                        if (MrkRtrnSts == 49) { SOID = long.Parse(HttpContext.Current.Session["SOID"].ToString()); }
                        else
                        {
                            SOID = Outbound.SetIntotSalesOrderHead(SOHead, profile.DBConnection._constr);
                        }

                        if (SOID > 0)
                        {
                            RSLT = Outbound.FinalSavSODetail(HttpContext.Current.Session.SessionID, ObjectName, SOID, profile.Personal.UserID.ToString(), Convert.ToInt16(SOHead.Status), Convert.ToInt64(SOHead.StoreId), profile.DBConnection._constr);
                            if (RSLT == 1 || RSLT == 2) { result = SOID; }  //"Request saved successfully";
                            else if (RSLT == 3) { result = -3; } //"Request saved successfully. Email Notification Failed..."
                            else if (RSLT == 0) { result = 0; }  //"Some error occurred";
                            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();//Document Save
                            DocumentSourceClient.FinalSaveToDBtDocument(HttpContext.Current.Session.SessionID, SOID, profile.Personal.UserID.ToString(), ObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                        }
                    }
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMSaveRequestHead");
                result = 0; // "Some error occurred";
            }
            finally
            {
                Outbound.Close();
                //HttpContext.Current.Session["POID"] = RequestID; 
            }
            return result;
        }
        #endregion

        #region SODetail
        protected void FillGrid1ByRequestID(long RequestID, long WarehouseID)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();  //iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result> RequestPartList = new List<WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result>();
                RequestPartList = Outbound.GetRequestPartDetailByRequestIDSO(RequestID, WarehouseID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();

                if (hdnOrderStatus.Value == "48" || hdnOrderStatus.Value == "49")
                {
                    Grid1.Columns[10].Visible = true;
                    Grid1.Columns[10].Width = "13%";
                }
                else
                {
                    Grid1.Columns[10].Visible = false;
                    Grid1.Columns[10].Width = "0%";
                }
                Grid1.DataSource = RequestPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "PartRequestEntry.aspx", "FillGrid1ByRequestID"); }
            finally { Outbound.Close(); }
        }
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            // iOutboundClient Outbound = new WMSInbound.iOutboundClient(); //iInboundClient Inbound = new iInboundClient();
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                List<BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result> SOPartList = new List<WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result>();
                if (hdn.Value == "")
                {
                    SOPartList = Outbound.GetExistingTempDataBySessionIDObjectNameSO(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    SOPartList = Outbound.AddPartIntoRequest_TempDataSO(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(hdnSelectedWarehouse.Value), profile.DBConnection._constr).ToList();
                }

                //Add by Suresh
                if (hdnprodID.Value != "")
                {
                    SOPartList = Outbound.AddPartIntoRequest_TempDataSO(hdnprodID.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                    hdnprodID.Value = "";
                }

                if (hdnChngDept.Value == "0x00x0")
                {
                    Outbound.ClearTempDataFromDBNEWSO(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    SOPartList = null;
                }
                hdnChngDept.Value = "";
                var chngdpt = "1x1";
                hdnChngDept.Value = chngdpt;

                if (hdnChangePrdQtyPrice.Value == "1")
                {
                    SOPartList = Outbound.GetRequestPartDetailByRequestIDSO(long.Parse(Session["SOID"].ToString()), long.Parse(hdnSelectedWarehouse.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }

                if (Session["OID"] != null)
                {
                    if (Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()) > 0)
                    {
                        if (hdnOrderStatus.Value == "48" || hdnOrderStatus.Value == "49")
                        {
                            Grid1.Columns[10].Visible = true;
                            Grid1.Columns[10].Width = "13%";
                        }
                        else
                        {
                            Grid1.Columns[10].Visible = false;
                            Grid1.Columns[10].Width = "0%";
                        }
                    }
                }
                Grid1.DataSource = SOPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "SalesOrder.aspx", "Grid1_OnRebind"); }
            finally { Outbound.Close(); }
        }

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            //iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                    Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[11] as Obout.Grid.GridDataControlFieldCell;

                    DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;
                    HiddenField hdnUOM = cell.FindControl("hdnMyUOM") as HiddenField;
                    Label rowQtySpn = e.Row.Cells[13].FindControl("rowQtyTotal") as Label;
                    TextBox txtUsrQty = e.Row.Cells[9].FindControl("txtUsrQty") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);
                    decimal CrntStock = Convert.ToDecimal(e.Row.Cells[7].Text);
                    decimal moq = Convert.ToDecimal(e.Row.Cells[5].Text);

                    /*New Price Added*/
                    TextBox txtUsrPrice = e.Row.Cells[14].FindControl("txtUsrPrice") as TextBox; //txtUsrPrice.Enabled = false;   //Product Price
                    Label rowPriceTotal = e.Row.Cells[15].FindControl("rowPriceTotal") as Label;

                    /*New Code In WMS Start*/
                    Label rowPriceTaxTotal = e.Row.Cells[19].FindControl("rowPriceTaxTotal") as Label;
                    Label rowTotalTax = e.Row.Cells[17].FindControl("rowTotalTax") as Label;
                    hdnSelectedRowTax.Value = rowTotalTax.Text;

                    TextBox txtReturnQty = e.Row.Cells[10].FindControl("txtReturnQty") as TextBox;

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

                    if (Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()) > 0)
                    {
                        long ReqId = Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString());

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
                        if (hdnOrderStatus.Value == "46")
                        {
                            UCProductSearch1.Visible = true;

                            decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                            decimal UsrQty = decimal.Parse(txtUsrQty.Text.ToString()); //SelectedQty * rowQty;
                            decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());

                            hdnSelectedQty.Value = SelectedQty.ToString();
                            rowQtySpn.Text = UsrQty.ToString();

                            if (UsrQty > CrntStock)
                            { rowQtySpn.Text = "0"; }
                            else
                            {
                                rowQtySpn.Text = UsrQty.ToString();
                                txtUsrQty.Text = Convert.ToString(UsrQty / SelectedQty);
                            }

                            ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ")");
                            txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ")");
                            txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + ProdID + ")");
                        }
                        else if (hdnOrderStatus.Value == "49")
                        {
                            txtReturnQty.Enabled = true;
                            txtUsrQty.Text = Convert.ToString(UserQty / SelectedQty);
                            UCProductSearch1.Visible = false;
                            txtUsrQty.Enabled = false;
                            txtUsrPrice.Enabled = false;
                            ddl.Enabled = false;

                            txtReturnQty.Attributes.Add("onblur", "javascript:GetReturnQty(this,'" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + ")");

                        }
                        else
                        {
                            txtUsrQty.Text = Convert.ToString(UserQty / SelectedQty);
                            UCProductSearch1.Visible = false;
                            txtUsrQty.Enabled = false;
                            txtUsrPrice.Enabled = false;
                            ddl.Enabled = false;
                            txtReturnQty.Enabled = false;
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

                        if (UsrQty > CrntStock)
                        { rowQtySpn.Text = "0"; }
                        else
                        {
                            rowQtySpn.Text = UsrQty.ToString();
                            //Price = decimal.Parse(rowQtySpn.Text.ToString()) * decimal.Parse(txtUsrPrice.Text.ToString());
                            //rowPriceTotal.Text = Price.ToString();

                        }
                        ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ")");
                        txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + moq + ")");
                        txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "','" + rowPriceTaxTotal.ClientID.ToString() + "','" + hdnSelectedRowTax.Value + "'," + ProdID + ")");
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
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iInboundClient Inbound = new iInboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (long.Parse(HttpContext.Current.Session["SOID"].ToString()) > 0)
                {
                    BrilliantWMS.WMSOutbound.tOrderHead RequestHead = new WMSOutbound.tOrderHead();
                    long ReqID = long.Parse(HttpContext.Current.Session["SOID"].ToString());
                    RequestHead = Outbound.GetOrderHeadByOrderIDSO(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), profile.DBConnection._constr);
                    string Status = RequestHead.Status.ToString();
                    if (Status == "1")
                    {
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        Outbound.RemovePartFromRequest_TempDataSO(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                        editOrder = 1;
                    }
                    else { editOrder = 0; }
                }
                else
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    Outbound.RemovePartFromRequest_TempDataSO(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                    editOrder = 1;
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMRemovePartFromRequest");
            }
            finally { Outbound.Close(); }
            return editOrder;
        }
        [WebMethod]
        public static void WMUpdRequestPart(object objRequest)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            // iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            // iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                string uom = Inbound.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);

                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();

                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); //PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);
                PartRequest.AmountAfterTax = Convert.ToDecimal(dictionary["AmountAfterTax"]);

                Outbound.UpdatePartRequest_TempData1SO(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMUpdRequestPart"); }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdReturnQty(object objRequest)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();

                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.ReturnQty = Convert.ToDecimal(dictionary["ReturnQty"]); 
                
                Outbound.UpdatePartRequest_TempDataRtrn(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PurchaseOrder.aspx", "WMUpdRequestPart"); }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static decimal WMGetTotal()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();
                tot = Outbound.GetTotalFromTempDataSO(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMGetTotal"); }
            finally { Outbound.Close(); }
            return tot;
        }

        [WebMethod]
        public static decimal WMGetTotalQty()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();
                tot = Outbound.GetTotalQTYFromTempDataSO(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMGetTotalQty"); }
            finally { Outbound.Close(); }
            return tot;
        }

        [WebMethod]
        public static void WMUpdRequestPartPrice(object objRequest, int ProdID)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            //iInboundClient Inbound = new iInboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();
                string uom = Inbound.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);
                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();
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
                Outbound.UpdatePartRequest_TempData12SO(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMUpdRequestPart"); }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static string[] webGetFooterTotal(decimal discountonsubtotal, string discountonsubtotalchk)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iInboundClient Inbound = new iInboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
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
                decimal tot = 0;
                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();
                tot = Outbound.GetTotalFromTempDataSO(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
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
                FooterTaxAmount = taxserviceclient.UpdateCalculatedTaxList("SalesOrderTotalTax", HttpContext.Current.Session.SessionID, 0, Convert.ToDecimal(taxableamount), "*", profile.DBConnection._constr).ToString();
                taxserviceclient.Close();

                returnresult[3] = FooterTaxAmount;

                return returnresult;
            }
            catch { return returnresult; }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static string WMGetTotalForTax(int Sequence)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            //iOutboundClient Outbound = new WMSInbound.iOutboundClient();
            // iInboundClient Inbound = new iInboundClient();
            decimal tot = 0; long PrdID = 0; string totPrdID = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRequest = new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result();
                tot = Outbound.GetTotalQTYofSequenceFromTempDataSO(Sequence, HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                PrdID = Outbound.GetPrdIDofSequenceFromTempDataSO(Sequence, HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "SalesOrder.aspx", "WMGetTotalQty"); }
            finally { Outbound.Close(); }
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
            gvApprovalRemarkBind(Convert.ToInt64(HttpContext.Current.Session["SORRequestID"].ToString()));
        }
        protected void GVInboxPOR_OnRebind(object sender, EventArgs e)
        {

            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();

            long RequestID = long.Parse(Session["SOID"].ToString());

            GVInboxPOR.DataSource = Inbound.GetCorrespondance(RequestID, profile.DBConnection._constr);
            GVInboxPOR.DataBind();
        }
        #endregion
    }
}