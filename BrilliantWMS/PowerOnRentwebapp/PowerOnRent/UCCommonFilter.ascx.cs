using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DashBoard;
using System.Web.UI.DataVisualization.Charting;
using BrilliantWMS.Login;
//using PowerOnRentwebapp.PORServiceSiteMaster;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.UserCreationService;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.PORServicePartIssue;
using BrilliantWMS.PORServicePartReceipts;
using BrilliantWMS.ProductCategoryService;
using BrilliantWMS.UCProductSearchService;
using System.Data;
using System.Collections;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using System.Linq.Expressions;
//using System.Data.Linq;
//using System.Data.Linq.SqlClient;

//namespace WebClientElegantCRM.PowerOnRent
namespace BrilliantWMS.PowerOnRent
{
    public partial class UCCommonFilter : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        public string hfCount_lcl, hdnEngineSelectedRec_lcl, hdnProductSelectedRec_lcl, frmdt_lcl, todt_lcl, hdnRequestSelectedRec_lcl, hdnIssueSelectedRec_lcl, hdnReceiptSelectedRec_lcl, hdnProductCategory_lcl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == null)
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
           // loadstring();
            hndGroupByGrid.Value = GVEngineInfo.GroupBy;
            hndGroupByPrd.Value = GVProductInfo.GroupBy;



            if (!IsPostBack)
            {

                FillDriver();
              //  FillVendor();
                fillsite();
                fillCategory();
                fillCompany();
                fillStatus();
                BindRole();
               


                GVRequestInfo_OnRebind(sender, e);
                GVEngineInfo_OnRebind(sender, e);
                GVIssueInfo_OnRebind(sender, e);
                GVReceiptInfo_OnRebind(sender, e);
                GVProductInfo_OnRebind(sender, e);
                GVUserInfo_OnRebind(sender, e);
                GVpurchaseInfo_Rebind(sender, e);
                gvGrnInfo_OnRebind(sender, e);
                gvQC_OnRebind(sender, e);
                gvPutIn_OnRebind(sender,e);
                gvPickUp_OnRebind(sender, e);
                gvDispatch_OnRebind(sender, e);
                FrmDate.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                To_Date.Date = DateTime.Now.Date;

                if (Request.QueryString["invoker"] == "orderdetail") { lblselectallorder.Visible = false; chkSelectAll.Visible = false; } else { lblselectallorder.Visible = true; chkSelectAll.Visible = true; }
                if (Request.QueryString["invoker"] == "SkuDetails") { lblallsku.Visible = false; chkSelectProduct.Visible = false; } else { lblallsku.Visible = true; chkSelectProduct.Visible = true; }
            }

            hfCount_lcl = hfCount.Value;
            hdnEngineSelectedRec_lcl = hdnEngineSelectedRec.Value;
            hdnProductSelectedRec_lcl = hdnProductSelectedRec.Value;
            frmdt_lcl = FrmDate.Date.Value.ToString("yyyy/MM/dd"); hdnNewFDt.Value = frmdt_lcl;
            todt_lcl = To_Date.Date.Value.ToString("yyyy/MM/dd"); hdnNewTDt.Value = todt_lcl;
            hdnRequestSelectedRec_lcl = hdnRequestSelectedRec.Value;
            hdnIssueSelectedRec_lcl = hdnIssueSelectedRec.Value;
            hdnReceiptSelectedRec_lcl = hdnReceiptSelectedRec.Value;
            hdnProductCategory_lcl = hdnProductCategory.Value;

        }

        public void BindRole()
        {
            BrilliantWMS.RoleMasterService.iRoleMasterClient roleMasterService = new BrilliantWMS.RoleMasterService.iRoleMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlrole.DataSource = roleMasterService.GetRoleList(profile.DBConnection._constr);
            ddlrole.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--SelectAll--";
            lst.Value = "0";
            ddlrole.Items.Insert(0, lst);

        }
        public void fillStatus()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlStatus.DataSource = UCCommonFilter.GetStatus(profile.DBConnection._constr);
            ddlStatus.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "-Select All-";
            lst.Value = "0";
            ddlStatus.Items.Insert(0, lst);
        }

        public void fillCompany()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            long UID = profile.Personal.UserID;

            string UsrType = profile.Personal.UserType.ToString();
            if (UsrType == "Super Admin")
            {
                //ddlcompany.DataSource = UCCommonFilter.GetCompanyName(profile.DBConnection._constr);
                ddlcompany.DataSource = UCCommonFilter.GetUserCompanyNameNEW(UID, profile.DBConnection._constr);
            }
            else
            {
                //ddlcompany.DataSource = UCCommonFilter.GetUserCompanyName(UID, profile.DBConnection._constr);     //UCCommonFilter.GetCompanyName(profile.DBConnection._constr);
                ddlcompany.DataSource = UCCommonFilter.GetUserCompanyNameNEW(UID, profile.DBConnection._constr);
            }

            //  ddlcompany.DataSource = UCCommonFilter.GetUserCompanyName(UID, profile.DBConnection._constr);     //UCCommonFilter.GetCompanyName(profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "-Select All-";
            lst.Value = "0";
            ddlcompany.Items.Insert(0, lst);

            ddlcompany.SelectedIndex = 1; hdnSelectedCompany.Value = ddlcompany.SelectedValue.ToString();

            List<mTerritory> SiteLst = new List<mTerritory>();

           // SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

            SiteLst = UCCommonFilter.GetAddedDepartmentList(Convert.ToInt16(hdnSelectedCompany.Value), Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            List<BrilliantWMS.WMSOutbound.mCustomer> CustList = new List<WMSOutbound.mCustomer>();
            CustList = Outbound.GetCustomerByCompanyID(Convert.ToInt16(hdnSelectedCompany.Value), profile.DBConnection._constr).ToList();
            ddlCustomer.DataSource = CustList;
            ddlCustomer.DataBind();
            ListItem lstCust = new ListItem();
            lstCust.Text = "--Select--";
            lstCust.Value = "0";
            ddlCustomer.Items.Insert(0, lstCust);
            ListItem lCust = new ListItem();
            lCust.Text = "Select All";
            lCust.Value = "1";
            ddlCustomer.Items.Insert(1, lCust);
            if (ddlCustomer.Items.Count > 0) ddlCustomer.SelectedIndex = 2;

            /*Vendor Client Fill*/
            FillVendor(hdnSelectedCompany.Value);

            FillClient(hdnSelectedCompany.Value);
            /**/

            ddldepartment.DataSource = SiteLst;
            ddldepartment.DataBind();
            ListItem lst1 = new ListItem();
            lst1.Text = "--Select--";
            lst1.Value = "0";
            ddldepartment.Items.Insert(0, lst1);
            ListItem l = new ListItem();
            l.Text = "Select All";
            l.Value = "1";
            ddldepartment.Items.Insert(1, l);
            if (ddldepartment.Items.Count > 0) ddldepartment.SelectedIndex = 2;

            long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnSelectedDepartment.Value = DeptID.ToString();
            //long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnSelectedCompany.Value = CompanyID.ToString();
            fillPaymentMethod(DeptID);

            string invkr = Request.QueryString["invoker"].ToString();

            if (invkr == "sku" || invkr == "SkuDetails" || invkr == "BomDetail")
            {
                ddlgset.SelectedIndex = 0;
                hdnSelectedGroupSet.Value = "0";

                ddlImage.SelectedIndex = 1;
                hdnSelectedImage.Value = "1";
            }
            else if (invkr == "order" || invkr == "orderdetail" || invkr == "orderlead")
            {
                DataSet ds = new DataSet();
                //List<SP_GWC_GetUserInfo_Result> UserList = new List<SP_GWC_GetUserInfo_Result>();
                //UserList = UCCommonFilter.GetUsrLst1(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, profile.DBConnection._constr).ToList();
                List<SP_GetUsers_Result> UsersList = new List<SP_GetUsers_Result>();
                UsersList = UCCommonFilter.GetUsersDepartmentWise(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, profile.DBConnection._constr).ToList();
                ddlUser.DataSource = UsersList;
                ddlUser.DataBind();
                ListItem lusr = new ListItem();
                lusr.Text = "Select All";
                lusr.Value = "0";
                ddlUser.Items.Insert(0, lusr);
                if (ddlUser.Items.Count > 0) ddlUser.SelectedIndex = 0;
                hdnSelectedUser.Value = ddlUser.SelectedItem.Value.ToString();

                // ddlStatus.SelectedIndex = 0;
                hdnSelectedStatus.Value = "0";// ddlStatus.SelectedValue.ToString();// ddlStatus.SelectedItem.Value.ToString();
            }
            else if (invkr == "user")
            {
                hdnSelectedRole.Value = "0";

                ddlActive.SelectedIndex = 1;
                hdnSelectedActive.Value = "Yes";
            }
            else if (invkr == "imgaudit")
            {
                DataSet ds = new DataSet();
                List<SP_GWC_GetUserInfo_Result> UserList = new List<SP_GWC_GetUserInfo_Result>();
                UserList = UCCommonFilter.GetUsrLst1(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, profile.DBConnection._constr).ToList();

                ddlUser.DataSource = UserList;
                ddlUser.DataBind();
                ListItem lusr = new ListItem();
                lusr.Text = "Select All";
                lusr.Value = "0";
                ddlUser.Items.Insert(0, lusr);
                if (ddlUser.Items.Count > 0) ddlUser.SelectedIndex = 0;
                hdnSelectedUser.Value = ddlUser.SelectedItem.Value.ToString();

                ddlImgstatus.SelectedIndex = 1;
                hdnImgStatus.Value = "Success";
            }
            else if (invkr == "orderdelivery")
            {
                ddlDriver.SelectedIndex=0;
                hdnSelectedDriver.Value = ddlDriver.SelectedValue.ToString();

                ddlPytMode.SelectedIndex = 0;
                hdnSelectedPaymentMode.Value = ddlPytMode.SelectedValue.ToString();
               
            }

            else if (invkr == "sla")
            {
                ddlDriver.SelectedIndex = 0;
                hdnSelectedDriver.Value = ddlDriver.SelectedValue.ToString();

                ddlDlrytype.SelectedIndex = 0;
                hdnSelectedDeliveryType.Value = ddlDlrytype.SelectedValue.ToString();
            }

            else if (invkr == "purchaseorder")
            {
                ddlVendor.SelectedIndex = 0;
                hdnSelectedVendor.Value = ddlVendor.SelectedValue.ToString();
                hdnSelectedStatus.Value = "0";
               
            }

        }

        public void fillDepartment()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            // ddldepartment.DataSource = UCCommonFilter.GetDepartmentName(Convert.ToInt32(hdnSelectedCompany.Value), profile.DBConnection._constr);
            ddldepartment.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddldepartment.Items.Insert(0, lst);
            ListItem l = new ListItem();
            l.Text = "Select All";
            l.Value = "1";
            ddldepartment.Items.Insert(1, l);
        }


        public void fillsite()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string UsrType = profile.Personal.UserType.ToString();
            if (UsrType == "Super Admin")
            {
                ddlSite.DataSource = UCCommonFilter.GetAllSites(profile.DBConnection._constr);
            }
            else
            {
                ddlSite.DataSource = UCCommonFilter.GetSiteNameByUserID(profile.Personal.UserID, profile.DBConnection._constr);
            }
            ddlSite.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlSite.Items.Insert(0, lst);
            ListItem l = new ListItem();
            l.Text = "Select All";
            l.Value = "1";
            ddlSite.Items.Insert(1, l);

            ddlFrmSite.DataSource = UCCommonFilter.GetSiteNameByUserID_Transfer(profile.Personal.UserID, profile.DBConnection._constr);
            ddlFrmSite.DataBind();
            ListItem lstfrm = new ListItem();
            lstfrm.Text = "--Select--";
            lstfrm.Value = "0";
            ddlFrmSite.Items.Insert(0, lstfrm);
        }

        public void fillCategory()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            iProductCategoryMasterClient ProductCategory = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlCategory.DataSource = ProductCategory.GetProductCategoryList(profile.DBConnection._constr);
            ddlCategory.DataBind();
            ListItem lst1 = new ListItem();
            lst1.Text = "--Select--";
            lst1.Value = "0";
            ddlCategory.Items.Insert(0, lst1);
        }

        public void fillDetail()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            iPartRequestClient PartRequest = new iPartRequestClient();
            iPartIssueClient PartIssue = new iPartIssueClient();
            iPartReceiptClient PartReceipt = new iPartReceiptClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (ddlSite.SelectedIndex >= -1)
            {

                if (Request.QueryString["invoker"] == "partrequest")
                {
                    try
                    {
                        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
                        List<POR_SP_GetRequestBySiteIDsOrUserID_Result> FinalRequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();

                        //GVRequestInfo.DataSource = null;
                        //GVRequestInfo.DataBind();

                        RequestList = PartRequest.GetRequestSummayBySiteIDs(hfCount.Value, profile.DBConnection._constr).ToList();
                        RequestList = RequestList.Where(l => (l.OrderDate >= FrmDate.Date)).ToList();
                        RequestList = RequestList.Where(l => (l.OrderDate <= To_Date.Date)).ToList();
                        //if (txtRequestSearch.Text != "")
                        //{
                        //    FinalRequestList = RequestList.Where(e => e.RequestNo.Contains(txtRequestSearch.Text) || e.RequestByUserName.Contains(txtRequestSearch.Text)).ToList();
                        //    RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
                        //    RequestList = FinalRequestList;
                        //}
                        //GVRequestInfo.DataSource = RequestList;
                        //GVRequestInfo.DataBind();

                        if (hdnRequestSelectedRec.Value == "0")
                        {
                            GVRequestInfo.SelectedRecords = new ArrayList();
                            foreach (POR_SP_GetRequestBySiteIDsOrUserID_Result rec in RequestList)
                            {
                                Hashtable row = new Hashtable();
                                row["PRH_ID"] = rec.ID;
                                row["RequestDate"] = rec.OrderDate;
                                row["RequestByUserName"] = rec.RequestByUserName;
                                GVRequestInfo.SelectedRecords.Add(row);
                                if (hdnRequestSelectedRec.Value != "") { hdnRequestSelectedRec.Value = hdnRequestSelectedRec.Value + "," + rec.ID.ToString(); }
                                else if (hdnRequestSelectedRec.Value == "") { hdnRequestSelectedRec.Value = rec.ID.ToString(); }
                            }
                        }
                        GVRequestInfo.DataSource = RequestList;
                        GVRequestInfo.DataBind();
                    }
                    catch { }
                    finally { PartRequest.Close(); }
                }
                else
                    if (Request.QueryString["invoker"] == "partconsumption")
                    {
                        try
                        {
                            //  UCProductSearchService.iUCProductSearchClient productSearchService = new UCProductSearchService.iUCProductSearchClient();

                            // GVEngineInfo.DataSource = null;
                            // GVEngineInfo.DataBind();

                            List<v_GetEngineDetails> EngineList = new List<v_GetEngineDetails>();
                            List<v_GetEngineDetails> FinalEngineList = new List<v_GetEngineDetails>();
                            EngineList = UCCommonFilter.GetEngineOfSite(hfCount.Value, profile.DBConnection._constr).ToList();
                            //if (txtEngineSearch.Text != "")
                            //{
                            //    FinalEngineList = EngineList.Where(e => e.EngineSerial.Contains(txtEngineSearch.Text) || e.GeneratorSerial.Contains(txtEngineSearch.Text)).ToList();
                            //    EngineList = new List<v_GetEngineDetails>();
                            //    EngineList = FinalEngineList;
                            //}

                            //GVEngineInfo.DataSource = EngineList;
                            //GVEngineInfo.GroupBy = hndGroupByGrid.Value;
                            //if (!Page.IsPostBack) { GVEngineInfo.GroupBy = "ProductCategory"; }
                            //GVEngineInfo.DataBind();
                            //productSearchService.Close();
                            //ID EngineSerial Container  EngineModel  EngineSerial GeneratorModel Territory
                            if (hdnEngineSelectedRec.Value == "0")
                            {
                                GVEngineInfo.SelectedRecords = new ArrayList();
                                foreach (v_GetEngineDetails rec in EngineList)
                                {
                                    Hashtable row = new Hashtable();

                                    row["ID"] = rec.ID;
                                    row["EngineSerial"] = rec.EngineSerial;
                                    row["Container"] = rec.Container;
                                    row["EngineModel"] = rec.EngineModel;
                                    row["EngineSerial"] = rec.EngineSerial;
                                    row["GeneratorModel"] = rec.GeneratorModel;
                                    row["Territory"] = rec.Territory;
                                    GVEngineInfo.SelectedRecords.Add(row);
                                    if (hdnEngineSelectedRec.Value != "") { hdnEngineSelectedRec.Value = hdnEngineSelectedRec.Value + "," + rec.ID.ToString(); }
                                    else if (hdnEngineSelectedRec.Value == "") { hdnEngineSelectedRec.Value = rec.ID.ToString(); }
                                }
                            }
                            GVEngineInfo.DataSource = EngineList;
                            GVEngineInfo.DataBind();

                        }
                        catch { }
                        finally { UCCommonFilter.Close(); }
                    }
                    else
                        if (Request.QueryString["invoker"] == "partissue")
                        {
                            try
                            {
                                List<POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result> IssueList = new List<POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result>();
                                List<POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result> FinalList = new List<POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result>();

                                //GVIssueInfo.DataSource = null;
                                //GVIssueInfo.DataBind();

                                IssueList = PartIssue.GetIssueSummayBySiteIDs(hfCount.Value, profile.DBConnection._constr).ToList();
                                IssueList = IssueList.Where(i => (i.IssueDate >= FrmDate.Date)).ToList();
                                IssueList = IssueList.Where(i => (i.IssueDate <= To_Date.Date)).ToList();
                                //if (txtIssueSearch.Text != "")
                                //{
                                //    FinalList = IssueList.Where(e => e.IssueNo.Contains(txtIssueSearch.Text) || e.IssuedByUserName.Contains(txtIssueSearch.Text)).ToList();
                                //    IssueList = new List<POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result>();
                                //    IssueList = FinalList;
                                //}
                                //GVIssueInfo.DataSource = IssueList;
                                //GVIssueInfo.DataBind();

                                if (hdnIssueSelectedRec.Value == "0")
                                {
                                    GVIssueInfo.SelectedRecords = new ArrayList();
                                    foreach (POR_SP_GetIssueSummaryBySiteIDsOrUserIDOrRequestIDOrIssueIDs_Result rec in IssueList)
                                    {
                                        Hashtable row = new Hashtable();
                                        //MINH_ID IssueDate IssuedByUserName
                                        row["MINH_ID"] = rec.MINH_ID;
                                        row["IssueDate"] = rec.IssueDate;
                                        row["IssuedByUserName"] = rec.IssuedByUserName;
                                        GVIssueInfo.SelectedRecords.Add(row);
                                        if (hdnIssueSelectedRec.Value != "") { hdnIssueSelectedRec.Value = hdnIssueSelectedRec.Value + "," + rec.MINH_ID.ToString(); }
                                        else if (hdnIssueSelectedRec.Value == "") { hdnIssueSelectedRec.Value = rec.MINH_ID.ToString(); }
                                    }
                                }
                                GVIssueInfo.DataSource = IssueList;
                                GVIssueInfo.DataBind();

                            }
                            catch { }
                            finally { PartIssue.Close(); }
                        }
                        else if (Request.QueryString["invoker"] == "partreceipt")
                        {
                            try
                            {
                                List<POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result> ReceiptList = new List<POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result>();
                                List<POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result> FinalReceiptList = new List<POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result>();

                                //GVReceiptInfo.DataSource = null;
                                //GVReceiptInfo.DataBind();

                                ReceiptList = PartReceipt.GetReceiptSummaryBySiteIDs(hfCount.Value, profile.DBConnection._constr).ToList();
                                ReceiptList = ReceiptList.Where(i => (i.GRN_Date >= FrmDate.Date)).ToList();
                                ReceiptList = ReceiptList.Where(i => (i.GRN_Date <= To_Date.Date)).ToList();
                                //if (txtReceiptSearch.Text != "")
                                //{
                                //    FinalReceiptList = ReceiptList.Where(e => e.GRNNo.Contains(txtReceiptSearch.Text) || e.ReceiptByUserName.Contains(txtReceiptSearch.Text)).ToList();
                                //    ReceiptList = new List<POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result>();
                                //    ReceiptList = FinalReceiptList;
                                //}
                                //GVReceiptInfo.DataSource = ReceiptList;
                                //GVReceiptInfo.DataBind();

                                if (hdnReceiptSelectedRec.Value == "0")
                                {
                                    GVReceiptInfo.SelectedRecords = new ArrayList();
                                    foreach (POR_SP_GetReceiptSummaryBySiteIDsOrUserIDOrRequestID_Result rec in ReceiptList)
                                    {
                                        Hashtable row = new Hashtable();
                                        //GRNH_ID GRN_Date ReceiptByUserName
                                        row["GRNH_ID"] = rec.GRNH_ID;
                                        row["GRN_Date"] = rec.GRN_Date;
                                        row["ReceiptByUserName"] = rec.ReceiptByUserName;
                                        GVReceiptInfo.SelectedRecords.Add(row);
                                        if (hdnReceiptSelectedRec.Value != "") { hdnReceiptSelectedRec.Value = hdnReceiptSelectedRec.Value + "," + rec.GRNH_ID.ToString(); }
                                        else if (hdnReceiptSelectedRec.Value == "") { hdnReceiptSelectedRec.Value = rec.GRNH_ID.ToString(); }
                                    }
                                }
                                GVReceiptInfo.DataSource = ReceiptList;
                                GVReceiptInfo.DataBind();

                            }
                            catch { }
                            finally { PartReceipt.Close(); }

                        }

                        else if (Request.QueryString["invoker"] == "user")
                        {
                            List<SP_GWC_GetUserInfoRoleWise_Result> UsrLst = new List<SP_GWC_GetUserInfoRoleWise_Result>();
                            //List<VW_GetUserInformation> UserList = new List<VW_GetUserInformation>();
                            //DataSet UserList = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();

                            var SearchedValue = hdnFilterText.Value;

                            try
                            {
                                //if (hdnUser.Value == "1")
                                //{
                                //    UserList = UCCommonFilter.GetUserInformation(Convert.ToInt64(hdnSelectedCompany.Value), Convert.ToInt64(hdnSelectedDepartment.Value), Convert.ToInt64(hdnSelectedRole.Value), ddlActive.SelectedValue, profile1.DBConnection._constr).ToList();
                                //}
                                if (SearchedValue == "")
                                {
                                    UsrLst = UCCommonFilter.GetUserInformation(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedRole.Value, ddlActive.SelectedValue, profile1.DBConnection._constr).ToList();
                                }
                                else
                                {
                                    //UsrLst = UCCommonFilter.GetUserInformationSearched(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedRole.Value, ddlActive.SelectedValue, SearchedValue ,profile1.DBConnection._constr).ToList();
                                    UsrLst = UCCommonFilter.GetUserInformation(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedRole.Value, ddlActive.SelectedValue, profile1.DBConnection._constr).ToList();
                                    UsrLst = UsrLst.Where(r => r.Name.StartsWith(SearchedValue) || r.EmailID.StartsWith(SearchedValue) || r.EmployeeID.StartsWith(SearchedValue)).ToList();
                                }
                                GVUserInfo.DataSource = UsrLst;
                                GVUserInfo.DataBind();
                            }

                            catch { }
                            finally { }

                        }

                        else if (Request.QueryString["invoker"] == "order")
                        {
                            DataSet ReqLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                               // ReqLst = UCCommonFilter.GetAllOrderDetails(frmdt_lcl, todt_lcl, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value, ddlStatus.SelectedValue, profile1.DBConnection._constr);
                                ReqLst = UCCommonFilter.GetSalesOrderList(profile.DBConnection._constr);
                                GVRequestInfo.DataSource = ReqLst;
                                GVRequestInfo.DataBind();
                            }
                            catch { }
                            finally { }

                        }
                        else if (Request.QueryString["invoker"] == "orderdetail")
                        {
                            DataSet ReqDtLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                ReqDtLst = UCCommonFilter.GetAllOrderDetails(frmdt_lcl, todt_lcl, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value, ddlStatus.SelectedValue, profile.DBConnection._constr);
                                GVRequestInfo.DataSource = ReqDtLst;
                                GVRequestInfo.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "orderlead")
                        {
                            DataSet ReqDtLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                ReqDtLst = UCCommonFilter.GetAllOrderDetails(frmdt_lcl, todt_lcl, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value, ddlStatus.SelectedValue, profile.DBConnection._constr);
                                GVRequestInfo.DataSource = ReqDtLst;
                                GVRequestInfo.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                 /*******/
                        else if (Request.QueryString["invoker"] == "orderdelivery")
                        {
                            DataSet ReqDtLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                ReqDtLst = UCCommonFilter.GetAllOrderDelivery(frmdt_lcl, todt_lcl, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedDriver.Value, hdnSelectedPaymentMode.Value, profile.DBConnection._constr);
                                GVRequestInfo.DataSource = ReqDtLst;
                                GVRequestInfo.DataBind();
                              
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "sla")
                        {
                            DataSet ReqDtLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                ReqDtLst = UCCommonFilter.GetAllSlaData(frmdt_lcl, todt_lcl, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, ddlStatus.SelectedValue, hdnSelectedDriver.Value, hdnSelectedDeliveryType.Value, profile.DBConnection._constr);
                                GVRequestInfo.DataSource = ReqDtLst;
                                GVRequestInfo.DataBind();
                                
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "purchaseorder")
                        {
                            DataSet PODtLst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                PODtLst = UCCommonFilter.GetPurchaseOrder(frmdt_lcl, todt_lcl,hdnSelectedVendor.Value, ddlStatus.SelectedValue, profile.DBConnection._constr);
                                GVpurchaseInfo.DataSource = PODtLst;
                                GVpurchaseInfo.DataBind();

                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "grn")
                        {
                            DataSet grnlst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                grnlst = UCCommonFilter.GetGRNList(profile.DBConnection._constr);
                                gvGrnInfo.DataSource = grnlst;
                                gvGrnInfo.DataBind();

                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "qc")
                        {
                            DataSet qclst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                qclst = UCCommonFilter.GetqcList(profile.DBConnection._constr);
                                gvQC.DataSource = qclst;
                                gvQC.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "putin")
                        {
                            DataSet putinlst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                putinlst = UCCommonFilter.GetputinList(profile.DBConnection._constr);
                                gvPutIn.DataSource = putinlst;
                                gvPutIn.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "pickup")
                        {
                            DataSet pickuplst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                pickuplst = UCCommonFilter.GetpickupList(profile.DBConnection._constr);
                                gvPickUp.DataSource = pickuplst;
                                gvPickUp.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                        else if (Request.QueryString["invoker"] == "dispatch")
                        {
                            DataSet dispatchlst = new DataSet();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            try
                            {
                                dispatchlst = UCCommonFilter.GetdispatchList(profile.DBConnection._constr);
                                gvPickUp.DataSource = dispatchlst;
                                gvPickUp.DataBind();
                            }
                            catch { }
                            finally { }
                        }
                /*******/

            }
        }

        public void fillProduct()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVProductInfo.DataSource = null;
            GVProductInfo.DataBind();
            try
            {
                //UCProductSearchService.iUCProductSearchClient productSearchService = new UCProductSearchService.iUCProductSearchClient();
                //List<GetProductDetail> ProductList = new List<GetProductDetail>();
                //List<GetProductDetail> FinalProductList = new List<GetProductDetail>();
                DataSet prdlist = new DataSet();

                GVProductInfo.DataSource = null;
                GVProductInfo.DataBind();

                if (Request.QueryString["invoker"] == "partconsumption")
                {
                    if (hdnEngineSelectedRec.Value == "")
                    {
                        GVProductInfo.DataSource = null;
                        GVProductInfo.DataBind();
                    }
                    else
                    {
                        var frmdt = FrmDate.Date.ToString();
                        var todt = To_Date.Date.ToString();


                        prdlist = UCCommonFilter.GetProductOfSelectedEngine(hdnEngineSelectedRec.Value, hdnFilterText.Value, frmdt, todt, profile.DBConnection._constr);
                        if (hdnProductSelectedRec.Value == "0")
                        {
                            GVProductInfo.SelectedRecords = new ArrayList();
                            foreach (DataRow rec in prdlist.Tables[0].Rows)
                            {
                                Hashtable row = new Hashtable();
                                //row["ID"] = rec.ID;
                                row["ID"] = rec["ID"];
                                //row["ProductCode"] = rec.ProductCode;
                                row["ProductCode"] = rec["ProductCode"];
                                //row["Name"] = rec.Name;
                                row["Name"] = rec["Name"];
                                GVProductInfo.SelectedRecords.Add(row);
                                if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                                else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                            }
                        }
                        GVProductInfo.DataSource = prdlist;
                        GVProductInfo.DataBind();
                    }
                }
                else if (Request.QueryString["invoker"] == "partrequest")
                {
                    if (hdnRequestSelectedRec.Value == "")
                    {
                        GVProductInfo.DataSource = null;
                        GVProductInfo.DataBind();
                    }
                    else
                    {
                        prdlist = UCCommonFilter.GetProductofRequest(hdnRequestSelectedRec.Value, hdnFilterText.Value, profile.DBConnection._constr);

                        if (hdnProductSelectedRec.Value == "0")
                        {
                            GVProductInfo.SelectedRecords = new ArrayList();
                            foreach (DataRow rec in prdlist.Tables[0].Rows)
                            {
                                Hashtable row = new Hashtable();
                                //row["ID"] = rec.ID;
                                row["ID"] = rec["ID"];
                                //row["ProductCode"] = rec.ProductCode;
                                row["ProductCode"] = rec["ProductCode"];
                                //row["Name"] = rec.Name;
                                row["Name"] = rec["Name"];
                                GVProductInfo.SelectedRecords.Add(row);
                                if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                                else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                            }
                        }

                        GVProductInfo.DataSource = prdlist;
                        GVProductInfo.DataBind();
                    }
                }
                else if (Request.QueryString["invoker"] == "partissue")
                {
                    if (hdnIssueSelectedRec.Value == "")
                    {
                        GVProductInfo.DataSource = null;
                        GVProductInfo.DataBind();
                    }

                    else
                    {
                        prdlist = UCCommonFilter.GetProductofIssue(hdnIssueSelectedRec.Value, hdnFilterText.Value, profile.DBConnection._constr);
                        if (hdnProductSelectedRec.Value == "0")
                        {
                            GVProductInfo.SelectedRecords = new ArrayList();
                            foreach (DataRow rec in prdlist.Tables[0].Rows)
                            {
                                Hashtable row = new Hashtable();
                                //row["ID"] = rec.ID;
                                row["ID"] = rec["ID"];
                                //row["ProductCode"] = rec.ProductCode;
                                row["ProductCode"] = rec["ProductCode"];
                                //row["Name"] = rec.Name;
                                row["Name"] = rec["Name"];
                                GVProductInfo.SelectedRecords.Add(row);
                                if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                                else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                            }
                        }
                        GVProductInfo.DataSource = prdlist;
                        GVProductInfo.DataBind();
                    }
                }
                else if (Request.QueryString["invoker"] == "partreceipt")
                {
                    if (hdnReceiptSelectedRec.Value == "")
                    {
                        GVProductInfo.DataSource = null;
                        GVProductInfo.DataBind();
                    }
                    else
                    {
                        prdlist = UCCommonFilter.GetProductofReceipt(hdnReceiptSelectedRec.Value, hdnFilterText.Value, profile.DBConnection._constr);
                        if (hdnProductSelectedRec.Value == "0")
                        {
                            GVProductInfo.SelectedRecords = new ArrayList();
                            foreach (DataRow rec in prdlist.Tables[0].Rows)
                            {
                                Hashtable row = new Hashtable();
                                //row["ID"] = rec.ID;
                                row["ID"] = rec["ID"];
                                //row["ProductCode"] = rec.ProductCode;
                                row["ProductCode"] = rec["ProductCode"];
                                //row["Name"] = rec.Name;
                                row["Name"] = rec["Name"];
                                GVProductInfo.SelectedRecords.Add(row);
                                if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                                else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                            }
                        }
                        GVProductInfo.DataSource = prdlist;
                        GVProductInfo.DataBind();
                    }
                }

                else if (Request.QueryString["invoker"] == "productdtl")
                {
                    List<GetPrdDetail> PrdList = new List<GetPrdDetail>();


                    PrdList = UCCommonFilter.AllProductOnSite(profile.DBConnection._constr).ToList();

                    if (hdnProductSelectedRec.Value == "0")
                    {

                        GVProductInfo.SelectedRecords = new ArrayList();
                        foreach (GetPrdDetail rec in PrdList)
                        {
                            Hashtable row = new Hashtable();
                            row["ID"] = rec.ID;
                            row["ProductCode"] = rec.ProductCode;
                            row["Name"] = rec.Name;
                            GVProductInfo.SelectedRecords.Add(row);
                            if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                            else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                        }
                    }
                    if (hdnFilterText.Value != "")
                    {
                        try
                        {
                            iUCProductSearchClient productSearchService = new iUCProductSearchClient();
                            CustomProfile profile1 = CustomProfile.GetProfile();
                            List<GetProductDetail> SProductList = new List<GetProductDetail>();
                            SProductList = productSearchService.GetProductList1(GVProductInfo.CurrentPageIndex, hdnFilterText.Value, profile1.DBConnection._constr).ToList();

                            GVProductInfo.DataSource = SProductList;
                            GVProductInfo.DataBind();
                            productSearchService.Close();
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }
                    else
                    {

                        GVProductInfo.DataSource = PrdList;
                        GVProductInfo.DataBind();
                    }

                }

                else if (Request.QueryString["invoker"] == "sku")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {
                        //select MP.ProductCode,MP.Name,MP.Description,img.Path,MP.CompanyID ,MP.StoreId,MP.GroupSet  from mproduct MP left outer join tImage img on  MP.ID=img.ReferenceID   where MP.CompanyID like '%%' and MP.StoreId like '%%' and MP.GroupSet like '%%'  
                        //if((hdnSelectedCompany.Value != "0") && (hdnSelectedCompany.Value != "") && (hdnSelectedDepartment.Value != "0") && (hdnSelectedDepartment.Value != "") && (hdnSelectedGroupSet.Value != "0") && (hdnSelectedGroupSet.Value != ""))                        
                        DataSet dsSkuFltr = new DataSet();
                        var SearchedValue = hdnFilterText.Value;
                        if (hdnSKU.Value == "sku")
                        {
                            if (SearchedValue == "")
                            {
                                dsSkuFltr = CommonFltr.SkulistReport(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, profile.DBConnection._constr);
                            }
                            else
                            {
                                dsSkuFltr = CommonFltr.SkulistReportSearch(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value,SearchedValue, profile.DBConnection._constr);
                            }
                            GVProductInfo.DataSource = dsSkuFltr;
                            GVProductInfo.DataBind();
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }
                }
                else if (Request.QueryString["invoker"] == "SkuDetails")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {
                        DataSet dsSkuFltr = new DataSet();
                        var SearchedValue = hdnFilterText.Value;
                        if (hdnSKU.Value == "sku")
                        {
                            if (SearchedValue == "")
                            {
                                dsSkuFltr = CommonFltr.SkulistReport(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, profile.DBConnection._constr);
                            }
                            else
                            {
                                dsSkuFltr = CommonFltr.SkulistReportSearch(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, SearchedValue, profile.DBConnection._constr);
                            }
                            GVProductInfo.DataSource = dsSkuFltr;
                            GVProductInfo.DataBind();
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }
                }
                else if (Request.QueryString["invoker"] == "BomDetail")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {
                        DataSet dsSkuFltr = new DataSet();
                        var SearchedValue = hdnFilterText.Value;
                        //showAlert("Select BOM Value Yes For BOM Detail Report...", "Error", "#")
                        if (hdnSKU.Value == "sku")
                        {
                            if (hdnSelectedGroupSet.Value == "No" || hdnSelectedGroupSet.Value == "0")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('Select BOM Value Yes For BOM Detail Report...','Error','#')", true);
                            }
                            else
                            {
                                if (SearchedValue == "")
                                {
                                    dsSkuFltr = CommonFltr.SkulistReport(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, profile.DBConnection._constr);
                                }
                                else
                                {
                                    dsSkuFltr = CommonFltr.SkulistReportSearch(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value,SearchedValue, profile.DBConnection._constr);
                                }
                                    GVProductInfo.DataSource = dsSkuFltr;
                                GVProductInfo.DataBind();
                            }
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }
                }

                else if (Request.QueryString["invoker"] == "order")
                {
                    if (hdnRequestSelectedRec.Value == "")
                    {
                        GVProductInfo.DataSource = null;
                        GVProductInfo.DataBind();
                    }
                    else
                    {
                        prdlist = UCCommonFilter.SKUDetailsBySelectedRequestID(hdnRequestSelectedRec.Value, profile.DBConnection._constr);

                        if (hdnProductSelectedRec.Value == "0")
                        {
                            GVProductInfo.SelectedRecords = new ArrayList();
                            foreach (DataRow rec in prdlist.Tables[0].Rows)
                            {
                                Hashtable row = new Hashtable();
                                //row["ID"] = rec.ID;
                                row["ID"] = rec["ID"];
                                //row["ProductCode"] = rec.ProductCode;
                                row["ProductCode"] = rec["ProductCode"];
                                //row["Name"] = rec.Name;
                                row["Name"] = rec["Name"];
                                GVProductInfo.SelectedRecords.Add(row);
                                if (hdnProductSelectedRec.Value != "") { hdnProductSelectedRec.Value = hdnProductSelectedRec.Value + "," + ID.ToString(); }
                                else if (hdnProductSelectedRec.Value == "") { hdnProductSelectedRec.Value = ID.ToString(); }
                            }
                        }

                        GVProductInfo.DataSource = prdlist;
                        GVProductInfo.DataBind();
                    }
                }
                else if (Request.QueryString["invoker"] == "imgaudit")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {
                        var frmdt = FrmDate.Date.ToString();
                        var todt = To_Date.Date.ToString();

                        DataSet dsSkuFltr = new DataSet();
                        var SearchedValue = hdnFilterText.Value;
                        if (hdnSKU.Value == "sku")
                        {
                            if (hdnImgStatus.Value == "Success")
                            {
                                if (SearchedValue == "")
                                {
                                    dsSkuFltr = CommonFltr.GetImageAuditAllPrd(frmdt, todt, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value, profile.DBConnection._constr);
                                }
                                else
                                {
                                    dsSkuFltr = CommonFltr.GetImageAuditAllPrdSearched(frmdt, todt, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value,SearchedValue, profile.DBConnection._constr);
                                }
                                    //dsSkuFltr = CommonFltr.SkulistReport(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, profile.DBConnection._constr);
                            }
                            else
                            {
                                if (SearchedValue == "")
                                {
                                    dsSkuFltr = CommonFltr.GetImageAuditFailPrdLst(frmdt, todt, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value, profile.DBConnection._constr);
                                }
                                else
                                {
                                    dsSkuFltr = CommonFltr.GetImageAuditFailPrdLstSearched(frmdt, todt, hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedUser.Value,SearchedValue, profile.DBConnection._constr);
                                }
                            }
                            GVProductInfo.DataSource = dsSkuFltr;
                            GVProductInfo.DataBind();
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }
                }

                else if (Request.QueryString["invoker"] == "totaldeliveryvstotalrequest")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {
                        //select MP.ProductCode,MP.Name,MP.Description,img.Path,MP.CompanyID ,MP.StoreId,MP.GroupSet  from mproduct MP left outer join tImage img on  MP.ID=img.ReferenceID   where MP.CompanyID like '%%' and MP.StoreId like '%%' and MP.GroupSet like '%%'  
                        //if((hdnSelectedCompany.Value != "0") && (hdnSelectedCompany.Value != "") && (hdnSelectedDepartment.Value != "0") && (hdnSelectedDepartment.Value != "") && (hdnSelectedGroupSet.Value != "0") && (hdnSelectedGroupSet.Value != ""))                        
                        DataSet dsSkuFltr = new DataSet();
                        var SearchedValue = hdnFilterText.Value;
                        if (hdnSKU.Value == "sku")
                        {
                            if (SearchedValue == "")
                            {
                                dsSkuFltr = CommonFltr.SkulistReport(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, profile.DBConnection._constr);
                            }
                            else
                            {
                                dsSkuFltr = CommonFltr.SkulistReportSearch(hdnSelectedCompany.Value, hdnSelectedDepartment.Value, hdnSelectedGroupSet.Value, hdnSelectedImage.Value, SearchedValue, profile.DBConnection._constr);
                            }
                            GVProductInfo.DataSource = dsSkuFltr;
                            GVProductInfo.DataBind();
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }
                }
                else if (Request.QueryString["invoker"] == "purchaseorder")
                {
                    iUCCommonFilterClient CommonFltr = new iUCCommonFilterClient();
                    CustomProfile profile1 = CustomProfile.GetProfile();
                    try
                    {


                        if (hdnRequestSelectedRec.Value == "")
                        {
                            GVProductInfo.DataSource = null;
                            GVProductInfo.DataBind();
                        }

                        else
                        {



                            //select MP.ProductCode,MP.Name,MP.Description,img.Path,MP.CompanyID ,MP.StoreId,MP.GroupSet  from mproduct MP left outer join tImage img on  MP.ID=img.ReferenceID   where MP.CompanyID like '%%' and MP.StoreId like '%%' and MP.GroupSet like '%%'  
                            //if((hdnSelectedCompany.Value != "0") && (hdnSelectedCompany.Value != "") && (hdnSelectedDepartment.Value != "0") && (hdnSelectedDepartment.Value != "") && (hdnSelectedGroupSet.Value != "0") && (hdnSelectedGroupSet.Value != ""))                        
                            DataSet dsSkuFltr = new DataSet();
                            //var SearchedValue = hdnFilterText.Value;
                            //if (hdnSKU.Value == "sku")
                            //{
                            //    if (SearchedValue == "")
                            //    {
                            if (hdnRequestSelectedRec.Value != "0")
                            {
                            dsSkuFltr = CommonFltr.GetSelectedPurchaseOrderDetails(hdnRequestSelectedRec.Value, profile.DBConnection._constr);
                            }
                            else
                            {
                                dsSkuFltr = CommonFltr.GetAllPurchaseOrderDetails(frmdt_lcl, todt_lcl, hdnSelectedVendor.Value, ddlStatus.SelectedValue, profile.DBConnection._constr);
                            }
                            GVProductInfo.DataSource = dsSkuFltr;
                            GVProductInfo.DataBind();
                            //}
                        }
                    }
                    catch { }
                    finally { CommonFltr.Close(); }

                }
            }

            catch (SystemException ex)
            {

            }

        }

        protected void GVEngineInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "partconsumption")
            {
                fillDetail();
            }
        }

        protected void GVRequestInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "partrequest")
            {
                fillDetail();
            }
            else if (Request.QueryString["invoker"] == "order")
            {
                fillDetail();
            }
            else if (Request.QueryString["invoker"] == "orderdetail")
            {
                fillDetail();
            }
            else if (Request.QueryString["invoker"] == "orderlead")
            {
                fillDetail();
            }
            else if (Request.QueryString["invoker"] == "orderdelivery")
            {
                fillDetail();
            }
            else if (Request.QueryString["invoker"] == "sla")
            {
                fillDetail();
            }
          
        }

        protected void GVIssueInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "partissue")
            {
                fillDetail();
            }
        }

        protected void GVReceiptInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "partreceipt")
            {
                fillDetail();
            }
        }

        protected void gvGrnInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "grn")
            {
                fillDetail();
            }
        }

        protected void gvQC_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "qc")
            {
                fillDetail();
            }
        }

        protected void gvPutIn_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "putin")
            {
                fillDetail();
            }
        }

        protected void gvPickUp_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "pickup")
            {
                fillDetail();
            }
        }

        protected void gvDispatch_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "dispatch")
            {
                fillDetail();
            }
        }

        protected void GVProductInfo_OnRebind(object sender, EventArgs e)
        {
            fillProduct();
        }

        protected void GVUserInfo_OnRebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "user")
            {
                fillDetail();
            }
        }



        public void GridVisibleTF(string invoker)
        {
            tblProduct.Attributes.Add("style", "display:table;");
            if (invoker == "partconsumption")
            {
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblEngine.Attributes.Add("style", "display:table;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "partrequest")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "partissue")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:table;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "partreceipt")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:table;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "monthly")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "weeklylst")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                //SiteList.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "consumabletracker")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                //SiteList.Attributes.Add("style", "display:none;");

                ExcludeZero.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");

            }

            else if (invoker == "productdtl")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:table;");
                PrdCategory.Attributes.Add("style", "display:none;");
                FDate.Attributes.Add("style", "display:none;");
                TDate.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:table;"); tblDispatch.Attributes.Add("Style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                toSite.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "transfer")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:table;");
                toSite.Attributes.Add("style", "display:table;"); tblDispatch.Attributes.Add("Style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "asset")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:table;");
                toSite.Attributes.Add("style", "display:table;");
                SiteList.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); dvCustomer.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "sku")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                FDate.Attributes.Add("style", "display:none;");
                TDate.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:table;");
                SiteList.Attributes.Add("style", "display:none;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Groupset1.Attributes.Add("style", "display:table;");
                Image.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");
                ZeroBalance.Attributes.Add("style", "display:table;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }


            else if (invoker == "user")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                SiteList.Attributes.Add("style", "display:none;");

                tblRequest.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                FDate.Attributes.Add("style", "display:none;");
                TDate.Attributes.Add("style", "display:none;");
                Department.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:table;");

                Role.Attributes.Add("style", "display:table;");
                Active.Attributes.Add("style", "display:table;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "order")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:table;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:table;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "orderlead")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:table;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "orderdetail")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:table;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "SkuDetails")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:table;");
                SiteList.Attributes.Add("style", "display:none;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Groupset1.Attributes.Add("style", "display:table;");
                Image.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "BomDetail")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                FDate.Attributes.Add("style", "display:none;");
                TDate.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:table;");
                SiteList.Attributes.Add("style", "display:none;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Groupset1.Attributes.Add("style", "display:table;");
                Image.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "imgaudit")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:table;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:table;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
                tblDispatch.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "orderdelivery")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:table");
                PytMode.Attributes.Add("Style", "display:table");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");

                Vendor.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }

            else if (invoker == "sla")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:table;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:table;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:table");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:table");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                Vendor.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
            }
            else if (invoker == "totaldeliveryvstotalrequest")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:table;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:table;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                Vendor.Attributes.Add("style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                dvClient.Attributes.Add("Style", "display:none;"); tblGRN.Attributes.Add("style", "display:none;");
                tblQC.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "purchaseorder")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none;");
                PytMode.Attributes.Add("Style", "display:none;");
                DlryType.Attributes.Add("Style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:table;");

                dvCustomer.Attributes.Add("Style", "display:table;");
                dvClient.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:table;");
                tblGRN.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                tblQC.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "grn")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none;");
                PytMode.Attributes.Add("Style", "display:none;");
                DlryType.Attributes.Add("Style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                dvCustomer.Attributes.Add("Style", "display:table;");
                dvClient.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:table;");
                tblGRN.Attributes.Add("style", "display:table;"); tblDispatch.Attributes.Add("Style", "display:none;");
                tblQC.Attributes.Add("style", "display:none;"); tblPutIn.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "qc")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none;");
                PytMode.Attributes.Add("Style", "display:none;");
                DlryType.Attributes.Add("Style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                dvCustomer.Attributes.Add("Style", "display:table;");
                dvClient.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:table;");
                tblGRN.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                tblQC.Attributes.Add("style", "display:table;"); tblPutIn.Attributes.Add("style", "display:none;"); tblPickUP.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "putin")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:none;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none;");
                PytMode.Attributes.Add("Style", "display:none;");
                DlryType.Attributes.Add("Style", "display:none;");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                dvCustomer.Attributes.Add("Style", "display:table;");
                dvClient.Attributes.Add("Style", "display:none;");
                Vendor.Attributes.Add("style", "display:table;");
                tblGRN.Attributes.Add("style", "display:none;");
                tblQC.Attributes.Add("style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:none;");
                tblPutIn.Attributes.Add("style", "display:table;"); tblPickUP.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "pickup")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                Vendor.Attributes.Add("style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:table;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:table;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
                tblPickUP.Attributes.Add("Style", "display:table;"); tblDispatch.Attributes.Add("Style", "display:none;");
            }
            else if (invoker == "dispatch")
            {
                tblEngine.Attributes.Add("style", "display:none;");
                tblIssue.Attributes.Add("style", "display:none;");
                tblReceipt.Attributes.Add("style", "display:none;");
                PrdCategory.Attributes.Add("style", "display:none;");
                ExcludeZero.Attributes.Add("style", "display:none;");
                frmSite.Attributes.Add("style", "display:none;");
                toSite.Attributes.Add("style", "display:none;");

                Groupset1.Attributes.Add("style", "display:none;");
                Image.Attributes.Add("style", "display:none;");
                SiteList.Attributes.Add("style", "display:none;");

                FDate.Attributes.Add("style", "display:table;");
                TDate.Attributes.Add("style", "display:table;");
                Company.Attributes.Add("style", "display:table;");
                Department.Attributes.Add("style", "display:none;");
                Status.Attributes.Add("style", "display:none;");
                User.Attributes.Add("style", "display:table;");
                tblProduct.Attributes.Add("style", "display:none;");
                tblRequest.Attributes.Add("style", "display:none;");
                tblUserInfo.Attributes.Add("style", "display:none;");

                Role.Attributes.Add("style", "display:none;");
                Active.Attributes.Add("style", "display:none;");

                ZeroBalance.Attributes.Add("style", "display:none;");
                ImgStatus.Attributes.Add("style", "display:none;");
                Driver.Attributes.Add("Style", "display:none");
                PytMode.Attributes.Add("Style", "display:none");
                DlryType.Attributes.Add("Style", "display:none");
                tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");

                Vendor.Attributes.Add("style", "display:none;");
                dvCustomer.Attributes.Add("Style", "display:table;"); tblPutIn.Attributes.Add("style", "display:none;");
                dvClient.Attributes.Add("Style", "display:table;"); tblGRN.Attributes.Add("style", "display:none;"); tblQC.Attributes.Add("style", "display:none;");
                tblPickUP.Attributes.Add("Style", "display:none;"); tblDispatch.Attributes.Add("Style", "display:table;");
            }
        }

        public void GridVisible()
        {
            tblRequest.Attributes.Add("style", "display:none;");
            tblEngine.Attributes.Add("style", "display:none;");
            tblProduct.Attributes.Add("style", "display:none;");
            tblIssue.Attributes.Add("style", "display:none;");
            tblReceipt.Attributes.Add("style", "display:none;");
            tblPurchaseOrderInfo.Attributes.Add("style", "display:none;");
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblFromDate.Text = rm.GetString("FromDate", ci);
            lblToDate.Text = rm.GetString("ToDate", ci);
            lblcompany.Text = rm.GetString("company", ci);
            lbldept.Text = rm.GetString("Department", ci);
            lblGroupSet.Text = rm.GetString("BOM", ci);
            lblimage.Text = rm.GetString("Image", ci);
            lblStatus.Text = rm.GetString("Status", ci);
            lblUser.Text = rm.GetString("User", ci);
            lblRole.Text = rm.GetString("Role", ci);
            lblActive.Text = rm.GetString("Active", ci);

            lblorderlist.Text = rm.GetString("OrderList", ci);
            lblselectallorder.Text = rm.GetString("SelectAllOrder", ci);
            lblissuelist.Text = rm.GetString("IssueList", ci);
            lblallissue.Text = rm.GetString("SelectAllIssue", ci);

            lblskulist.Text = rm.GetString("SKUList", ci);
            lblallsku.Text = rm.GetString("SelectAllSKU", ci);

            lbluserlist.Text = rm.GetString("UserList", ci);
            lblalluser.Text = rm.GetString("SelectAllUser", ci);

            btnexQuery.Value = rm.GetString("executequery", ci);
            lblWithZeroBalance.Text = rm.GetString("withzerobalance", ci);

            lblDriver.Text = rm.GetString("Driver", ci);
            lblPytMode.Text = rm.GetString("PaymentMode", ci);

            lbldeliverytype.Text = rm.GetString("DeliveryType", ci);
            



        }
     
        public void FillDriver()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlDriver.DataSource = UCCommonFilter.GetAllDriverList(profile.DBConnection._constr);
            ddlDriver.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "-Select All-";
            lst.Value = "0";
            ddlDriver.Items.Insert(0, lst);
            
        }
        
        public void fillPaymentMethod(long selectedDept)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds = objService.GetDeptWisePaymentMethod(selectedDept, profile.DBConnection._constr);
                ddlPytMode.DataSource = ds;
                ddlPytMode.DataBind();
                ListItem lstpm = new ListItem { Text = "-Select All-", Value = "0" };
                ddlPytMode.Items.Insert(0, lstpm);
            }
            catch { }
            finally { objService.Close(); }
        }

        public void FillVendor(string CompanyID)
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlVendor.DataSource = UCCommonFilter.GetVendors(CompanyID,profile.DBConnection._constr);
            ddlVendor.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "-Select All-";
            lst.Value = "0";
            ddlVendor.Items.Insert(0, lst);
        }

        public void FillClient(string CompanyID)
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlClient.DataSource = UCCommonFilter.GetClients(CompanyID, profile.DBConnection._constr);
            ddlClient.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "-Select All-";
            lst.Value = "0";
            ddlClient.Items.Insert(0, lst);
        }

        protected void GVpurchaseInfo_Rebind(object sender, EventArgs e)
        {
            if (Request.QueryString["invoker"] == "purchaseorder")
            {
                fillDetail();
            }
        }

    }
}