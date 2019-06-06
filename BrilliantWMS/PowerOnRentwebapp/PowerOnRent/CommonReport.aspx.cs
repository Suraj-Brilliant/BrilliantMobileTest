using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Data;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.CommonControls;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.PORServicePartRequest;

//namespace WebClientElegantCRM.PowerOnRent
namespace BrilliantWMS.PowerOnRent
{
    public partial class CommonReport : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        string QueryParameter, ObjectName;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            //loadstring();
            //UCCommonFilter1.fillDetail();
            if (!IsPostBack)
            {

                QueryParameter = Request.QueryString["invoker"];
            }
            if (QueryParameter == null) { UCCommonFilter1.GridVisible(); }

            
            if (QueryParameter != null) UCCommonFilter1.GridVisibleTF(QueryParameter);

            changecolor(QueryParameter);

            /*New Code For Two Reports Not Showing for Requestor*/
            CustomProfile profile = CustomProfile.GetProfile();
            string UsrType = profile.Personal.UserType.ToString();
            //if (UsrType == "Super Admin")
            //{
            //    imgadt1.Visible = true;
            //    imgadt2.Visible = true;
            //    usrrpt1.Visible = true;
            //    usrrpt2.Visible = true;
            //}
            //else
            //{
            //    imgadt1.Visible = false;
            //    imgadt2.Visible = false;
            //    usrrpt1.Visible = false;
            //    usrrpt2.Visible = false;
            //}
            /*New Code For Two Reports Not Showing for Requestor*/
            if (QueryParameter == "SkuDetails") { btnSKUTransaction.Visible = true; }
            else { btnSKUTransaction.Visible = false; }

            if (QueryParameter == "user") { btnusertransaction.Visible = true; }
            else { btnusertransaction.Visible = false; }

            if (QueryParameter == "purchaseorder") { btnViewReport.Visible = false; btnPOList.Visible = true; btnPODetail.Visible = true; }
            else { btnPOList.Visible = false; btnPODetail.Visible = false; }

            if (QueryParameter == "grn") { btnViewReport.Visible = false; btngrnList.Visible = true; btngrnDetail.Visible = true; }
            else { btngrnList.Visible = false; btngrnDetail.Visible = false;  }

            if (QueryParameter == "qc") { btnViewReport.Visible = false; btnqcList.Visible = true; btnqcDetail.Visible = true; }
            else { btnqcList.Visible = false; btnqcDetail.Visible = false; }

            if (QueryParameter == "putin") { btnViewReport.Visible = false; btnputinList.Visible = true; btnputinDetail.Visible = true; }
            else { btnputinList.Visible = false; btnputinDetail.Visible = false;  }

            if (QueryParameter == "order") { btnViewReport.Visible = false; btnOrderList.Visible = true; btnOrderDetail.Visible = true; }
            else { btnOrderList.Visible = false; btnOrderDetail.Visible = false; }

            if (QueryParameter == "pickup") { btnViewReport.Visible = false; btnpickupList.Visible = true; btnpickDetail.Visible = true; }
            else { btnpickupList.Visible = false; btnpickDetail.Visible = false; }

            if (QueryParameter == "dispatch") { btnViewReport.Visible = false; btndispatchList.Visible = true; btndispatchDetail.Visible = true; }
            else { btndispatchList.Visible = false; btndispatchDetail.Visible = false; }
        }


        protected void fillDataSet()
        {
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            QueryParameter = Request.QueryString["invoker"];
            try
            {
                if (QueryParameter.ToLower() == "partconsumption")
                {
                    ObjectName = "PartConsumption";
                    Session["ReportHeading"] = "Consumption Report";
                    dsCmnRpt = UCCommonFilterClient.GetReportData(UCCommonFilter1.hfCount_lcl, UCCommonFilter1.hdnEngineSelectedRec_lcl, UCCommonFilter1.hdnProductSelectedRec_lcl, UCCommonFilter1.frmdt_lcl, UCCommonFilter1.todt_lcl, ObjectName, profile.DBConnection._constr);
                }
                else if (QueryParameter.ToLower() == "partrequest")
                {
                    ObjectName = "PartRequisition";
                    Session["ReportHeading"] = "Part Requisition Register";
                    dsCmnRpt = UCCommonFilterClient.GetRequisitionData(UCCommonFilter1.hfCount_lcl, UCCommonFilter1.hdnRequestSelectedRec_lcl, UCCommonFilter1.hdnProductSelectedRec_lcl, UCCommonFilter1.frmdt_lcl, UCCommonFilter1.todt_lcl, ObjectName, profile.DBConnection._constr);
                }
                else if (QueryParameter.ToLower() == "partissue")
                {
                    ObjectName = "PartIssue";
                    Session["ReportHeading"] = "Issue Register";
                    dsCmnRpt = UCCommonFilterClient.GetIssueData(UCCommonFilter1.hfCount_lcl, UCCommonFilter1.hdnIssueSelectedRec_lcl, UCCommonFilter1.hdnProductSelectedRec_lcl, UCCommonFilter1.frmdt_lcl, UCCommonFilter1.todt_lcl, ObjectName, profile.DBConnection._constr);
                }
                else if (QueryParameter.ToLower() == "partreceipt")
                {
                    ObjectName = "partreceipt";
                    Session["ReportHeading"] = "Receipt Register";
                    dsCmnRpt = UCCommonFilterClient.GetReceiptData(UCCommonFilter1.hfCount_lcl, UCCommonFilter1.hdnReceiptSelectedRec_lcl, UCCommonFilter1.hdnProductSelectedRec_lcl, UCCommonFilter1.frmdt_lcl, UCCommonFilter1.todt_lcl, ObjectName, profile.DBConnection._constr);
                }

                Session["ReportDS"] = dsCmnRpt;
                Session["FromDt"] = UCCommonFilter1.frmdt_lcl;
                Session["ToDt"] = UCCommonFilter1.todt_lcl;
                Session["SelObject"] = QueryParameter;

                DataSet ds = new DataSet();
                ds = dsCmnRpt;
                ds.Tables[0].TableName = "dsSiteConsumption";
                ReportDataSource rds = new ReportDataSource
                    (ds.Tables[0].TableName, ds.Tables[0]);
                //Response.Redirect("<script>window.open('../POR/Reports/ReportViewer.aspx', null, 'height=510, width=990,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=0,menubar=0, screenX=0; screenY=0');</script>");
                Response.Redirect("../POR/Reports/ReportViewer.aspx");
            }
            catch (System.Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, this, "CommonReport", "fillDataSet");
            }
            finally
            {
                UCCommonFilterClient.Close();
            }
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            QueryParameter = Request.QueryString["invoker"];

            if (QueryParameter.ToLower() == "partrequest")
            {
                if (UCCommonFilter1.hdnRequestSelectedRec_lcl == "")
                {
                    WebMsgBox.MsgBox.Show("Please Select Request");
                }
                else if (UCCommonFilter1.hdnProductSelectedRec_lcl == "")
                {
                    WebMsgBox.MsgBox.Show("Please Select Product");
                }
                else
                {
                    fillDataSet();
                }
            }


        }

        protected void changecolor(string qrypara)
        {
            switch (qrypara)
            {
                //case "partrequest":
                //    partrequisition.Attributes.Add("style", "color:Navy");
                //    break;
                //case "partissue":
                //    partissue.Attributes.Add("style", "color:Navy");
                //    break;
                //case "partreceipt":
                //    partreceipt.Attributes.Add("style", "color:Navy");
                //    break;
                //case "rptsku":
                //    partreceipt.Attributes.Add("style", "color:Navy");
                //    break;
                //case "rptuser":
                //    partreceipt.Attributes.Add("style", "color:Navy");
                //    break;
                //case "rptorder":
                //    partreceipt.Attributes.Add("style", "color:Navy");
                //    break;
                //case "partconsumption":
                //  partconsumption.Attributes.Add("style", "color:Navy");
                //break;
                //case "monthly":
                //  monthly.Attributes.Add("style", "color:Navy");
                //break;
                //case "weeklylst":
                //  weeklylst.Attributes.Add("style", "color:Navy");
                //break;
                //case "consumabletracker":
                //  consumabletracker.Attributes.Add("style", "color:Navy");
                //break;
                // case "productdtl":
                //   productdtl.Attributes.Add("style", "color:Navy");
                // break;
            }
        }

        [WebMethod]
        public static int WMGetReportData(string invoker, string SeletedParts, string SeletedRefIDs, string FromDt, string ToDt, string Site, string ChkAll, string ChkPrd)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (invoker.ToLower() == "partconsumption")
            {
                HttpContext.Current.Session["ReportHeading"] = "Consumption Report";
                if (ChkAll != "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetReportData(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllReportData(Site, FromDt, ToDt, profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd != "1")
                {
                    //  dsCmnRpt = UCCommonFilterClient.GetReportDataAllEngine(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll != "1" && ChkPrd == "1")
                {
                    //    dsCmnRpt = UCCommonFilterClient.GetReportDataAllPrd(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }

            }
            else if (invoker.ToLower() == "partrequest")
            {
                HttpContext.Current.Session["ReportHeading"] = "Part Requisition Register";
                if (ChkAll != "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetRequisitionData(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllRequisitionData(Site, FromDt, ToDt, profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllRequisitionDataAllRequest(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll != "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllRequisitionDataAllPrd(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
            }
            else if (invoker.ToLower() == "partissue")
            {
                HttpContext.Current.Session["ReportHeading"] = "Issue Register";
                if (ChkAll != "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetIssueData(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllIssueData(Site, FromDt, ToDt, profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetIssueDataAllIssue(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll != "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetIssueDataAllPrd(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
            }
            else if (invoker.ToLower() == "partreceipt")
            {
                HttpContext.Current.Session["ReportHeading"] = "Receipt Register";
                if (ChkAll != "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetReceiptData(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllReceiptData(Site, FromDt, ToDt, profile.DBConnection._constr);
                }
                else if (ChkAll == "1" && ChkPrd != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetReceiptDataAllReceipt(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
                else if (ChkAll != "1" && ChkPrd == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetReceiptDataAllPrd(Site, SeletedRefIDs, SeletedParts, FromDt, ToDt, "", profile.DBConnection._constr);
                }
            }
            else if (invoker.ToLower() == "monthly")
            {
                HttpContext.Current.Session["ReportHeading"] = "PR-Report Monthly";
                dsCmnRpt = UCCommonFilterClient.GetPenComRequestData(Site, FromDt, ToDt, profile.DBConnection._constr);
            }
            else if (invoker.ToLower() == "weeklylst")
            {
                HttpContext.Current.Session["ReportHeading"] = "Weekly Analysis";
                dsCmnRpt = UCCommonFilterClient.GetWeeklyConsumption(Site, FromDt, ToDt, profile.DBConnection._constr);
            }
            else if (invoker.ToLower() == "consumabletracker")
            {
                HttpContext.Current.Session["ReportHeading"] = "Consumable Tracker";
                dsCmnRpt = UCCommonFilterClient.GetConsumableStock(SeletedRefIDs, Site, FromDt, ToDt, profile.DBConnection._constr);
            }

            else if (invoker.ToLower() == "productdtl")
            {
                HttpContext.Current.Session["ReportHeading"] = "productdtl";
                dsCmnRpt = UCCommonFilterClient.GetProductBalanceOfSite(Site, SeletedParts, ChkPrd, ChkAll, profile.DBConnection._constr);
            }

            else if (invoker.ToLower() == "transfer")
            {
                HttpContext.Current.Session["ReportHeading"] = "transfer";
                dsCmnRpt = UCCommonFilterClient.GetTransferRptData(SeletedParts, SeletedRefIDs, FromDt, ToDt, profile.DBConnection._constr);
            }

            else if (invoker.ToLower() == "asset")
            {
                HttpContext.Current.Session["ReportHeading"] = "asset";
                dsCmnRpt = UCCommonFilterClient.GetAssetRptData(SeletedParts, SeletedRefIDs, FromDt, ToDt, profile.DBConnection._constr);
            }

            dsCmnRpt.Tables[0].TableName = "dsSiteConsumption";
            HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
            HttpContext.Current.Session["FromDt"] = FromDt;
            HttpContext.Current.Session["ToDt"] = ToDt;
            HttpContext.Current.Session["SelObject"] = invoker;


            if (invoker.ToLower() == "partconsumption")
            {
                int EngCount;

                if (ChkAll == "1")
                {
                    EngCount = UCCommonFilterClient.GetEngineCountAll(profile.DBConnection._constr);
                    HttpContext.Current.Session["Generator"] = EngCount;
                }
                else
                {
                    EngCount = UCCommonFilterClient.GetEngineCount(SeletedRefIDs, profile.DBConnection._constr);
                    HttpContext.Current.Session["Generator"] = EngCount;
                }
            }

            //DataSet ds = new DataSet();
            //ds = dsCmnRpt;
            //ds.Tables[0].TableName = "dsSiteConsumption";
            //ReportDataSource rds = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
            result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            return result;
        }

        [WebMethod]
        public static List<mTerritory> WMGetFromSite(long FrmSiteID)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            SiteLst = UCCommonFilter.GetToSiteName_Transfer(FrmSiteID, profile.DBConnection._constr).ToList();

            return SiteLst;
        }


        [WebMethod]
        public static int WMGetGWCSKUReportData(string invoker, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage, string AllSkU, string WithZero)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();

            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (WithZero == "") { WithZero = "1"; }
            if (SelectedImage == "1") SelectedImage = "";

            if (invoker.ToLower() == "sku")
            {
                HttpContext.Current.Session["ReportHeading"] = "SKU Report";
                if (AllSkU == "1")
                {
                    //dsCmnRpt = UCCommonFilterClient.GetAllSKUData(SelectedCompany, SelectedDepartment, SelectedGroupSet, profile.DBConnection._constr);
                    dsCmnRpt = UCCommonFilterClient.GetAllSKUData(SelectedCompany, SelectedDepartment, SelectedGroupSet, SelectedImage, WithZero, profile.DBConnection._constr);
                }
                else if (AllSkU != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllSKUDataSelectedRow(SelectedProducts, SelectedImage, WithZero, profile.DBConnection._constr);
                }
            }
            dsCmnRpt.Tables[0].TableName = "dsSKU";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
               
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (SelectedCompany == "0") { Company = "All Company"; }
                if (SelectedDepartment == "0") { Department = "All Department"; }
                string BOM = SelectedGroupSet;
                if (SelectedGroupSet == "0") { BOM = "Yes / No"; } else if (SelectedGroupSet == "") { BOM = "Yes / No"; }
                string Image = SelectedImage;
                if (SelectedImage == "1")
                { Image = "Yes / No"; }
                else if (SelectedImage == "") { Image = "Yes / No"; }
                string Logo = dsCmnRpt.Tables[0].Rows[0]["Logo"].ToString();
                string CompanyAddress = dsCmnRpt.Tables[0].Rows[0]["AddressLine1"].ToString();

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["BOM"] = BOM;
                HttpContext.Current.Session["Image"] = Image;
                HttpContext.Current.Session["SelObject"] = invoker;
                //string imagePath = new Uri("c:\\RAH1012-18-18.png").AbsoluteUri;
                string imagePath = dsCmnRpt.Tables[0].Rows[0]["Path"].ToString();
                HttpContext.Current.Session["ImagePath"] = imagePath;
                //HttpContext.Current.Session["Logo"] = Logo;
                HttpContext.Current.Session["CompanyAddress"] = CompanyAddress;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;
        }




        [WebMethod]
        public static int WMGetGWCUserReportData(string invoker, string SelectedUser, string SelectedCompany, string SelectedDepartment, string AllUser, string Role, string Active)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (invoker.ToLower() == "user")
            {
                HttpContext.Current.Session["ReportHeading"] = "User Report";
                if (AllUser == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllUserData(SelectedCompany, SelectedDepartment, Role, Active, profile.DBConnection._constr);
                }
                else if (AllUser != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllUserDataSelectedRow(SelectedUser, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsUser";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["Company"].ToString();
                if (Company == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Department"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (Department == "0") Department = "All";
                string RoleName = dsCmnRpt.Tables[0].Rows[0]["RoleName"].ToString();
                if (RoleName == "0") RoleName = "All";
                string Active1 = dsCmnRpt.Tables[0].Rows[0]["Active"].ToString();


                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["RoleName"] = RoleName;
                HttpContext.Current.Session["Active"] = Active1;

                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;
        }

        /*New PO Report*/
        [WebMethod]
        public static int WMGetpoListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "Purchase Order Report";
            dsCmnRpt = UCCommonFilterClient.GetPOList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dspolist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "purchaseorder";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetgrnListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "GRN List Report";
            dsCmnRpt = UCCommonFilterClient.GetAllGRNList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsgrnlist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "grnlist";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetgrnDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "GRN List Report";
            dsCmnRpt = UCCommonFilterClient.GetGRNDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsgrndetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "grndetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetqcListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "QC List Report";
            dsCmnRpt = UCCommonFilterClient.GetAllqcList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsqclist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "qcdetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetqcDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "QC Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetqcDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsqcdetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "qcdetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetputinListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "PutIn List Report";
            dsCmnRpt = UCCommonFilterClient.GetAllputinList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsputinlist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsputinlist";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetputinDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "PutIn Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetputinDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsputindetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsputindetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetorderListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "Order Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetAllsalesorderList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsorderlist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsorderlist";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetorderDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "Order List Report";
            dsCmnRpt = UCCommonFilterClient.GetsalesorderDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsorderdetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsorderdetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetpickupListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "PickUP Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetAllpickupList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dspickuplist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dspickuplist";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetpickupDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "PickUP Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetpickupDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dspickupdetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dspickupdetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetdispatchListALL()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "Dispatch Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetAlldispatchList(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsdispatchlist";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsdispatchlist";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetdispatchDetail()
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["ReportHeading"] = "Dispatch Detail Report";
            dsCmnRpt = UCCommonFilterClient.GetdispatchDetail(profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsdispatchdetail";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "dsdispatchdetail";
                HttpContext.Current.Session["Company"] = "Supreme Logistics Solutions";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            return result;
        }

        [WebMethod]
        public static int WMGetGWCOrderReportData(string invoker, string FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string SelectedCompany, string SelectedDepartment, string AllOrder, string AllProduct, string SelectedUser, string SelectedStatus)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string frmdt_lcl, todt_lcl;
            frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToString("yyyy/MM/dd"); // Value.ToString("yyyy/MM/dd");
            todt_lcl = Convert.ToDateTime(ToDate).Date.ToString("yyyy/MM/dd"); //.Value.ToString("yyyy/MM/dd");

            if (invoker.ToLower() == "order")
            {
                HttpContext.Current.Session["ReportHeading"] = "Order Report";
                if (AllOrder == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.AllOrderReports(FromDate, ToDate, SelectedCompany, SelectedDepartment, SelectedUser, SelectedStatus, profile.DBConnection._constr);
                    
                }
                else if (AllOrder != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllOrderDataSelectedRow(SelectedOrder, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsOrder";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string User = dsCmnRpt.Tables[0].Rows[0]["UserName"].ToString();
                if (SelectedUser == "0") User = "All";
                string Status = dsCmnRpt.Tables[0].Rows[0]["StatusName"].ToString();
                if (SelectedStatus == "0") Status = "All";


                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FromDt"] = frmdt_lcl;
                HttpContext.Current.Session["ToDt"] = todt_lcl;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["Status"] = Status;
                HttpContext.Current.Session["User"] = User;



                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;

        }

        [WebMethod]
        public static int WMGetGWCOrderLeadReportData(string invoker, string FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string SelectedCompany, string SelectedDepartment, string AllOrder, string AllProduct, string SelectedUser, string SelectedStatus)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string frmdt_lcl, todt_lcl;
            frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToShortDateString(); // Value.ToString("yyyy/MM/dd");
            todt_lcl = Convert.ToDateTime(ToDate).Date.ToShortDateString(); 

            if (invoker.ToLower() == "orderlead")
            {
                HttpContext.Current.Session["ReportHeading"] = "Order Lead Report";
                if (AllOrder == "1")
                {
                    dsCmnRpt = dsCmnRpt = UCCommonFilterClient.GetAllOrderLeadReprtData(SelectedCompany, SelectedDepartment, SelectedStatus, SelectedUser, profile.DBConnection._constr);
                }
                else if (AllOrder != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllOrderSelectedOrderRpt(SelectedOrder, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsOrderLeadTime";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (Company == "0") Company = "All Company";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (Department == "0") Department = "All Department";
                string User = dsCmnRpt.Tables[0].Rows[0]["UserName"].ToString();
                if (User == "0") User = "All Users";
                string Status = dsCmnRpt.Tables[0].Rows[0]["StatusName"].ToString();
                if (Status == "0") Status = "All";

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FromDt"] = frmdt_lcl;
                HttpContext.Current.Session["ToDt"] = todt_lcl;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["Status"] = Status;
                HttpContext.Current.Session["User"] = User;
                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else
            {
                result = 0;
            }
            return result;

        }


        [WebMethod]
        public static int WMGetGWCOrderDetailsReportData(string invoker, string  FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string SelectedCompany, string SelectedDepartment, string AllOrder, string AllProduct, string SelectedUser, string SelectedStatus)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string frmdt_lcl, todt_lcl;
            frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToShortDateString(); // Value.ToString("yyyy/MM/dd");
            todt_lcl = Convert.ToDateTime(ToDate).Date.ToShortDateString(); 

            if (invoker.ToLower() == "orderdetail")
            {
                HttpContext.Current.Session["ReportHeading"] = "Order Details Report";
                if (AllOrder == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetOrderDetailsReprtData(SelectedCompany, SelectedDepartment, SelectedUser, SelectedStatus, profile.DBConnection._constr);
                }
                else if (AllOrder != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllOrderDetailsDataSelectedRow(SelectedOrder, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsOrderDetails";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string User = dsCmnRpt.Tables[0].Rows[0]["UserName"].ToString();
                if (SelectedUser == "0") User = "All";
                string Status = dsCmnRpt.Tables[0].Rows[0]["Status"].ToString();
                if (SelectedStatus == "0") Status = "All";


                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FromDt"] = frmdt_lcl;
                HttpContext.Current.Session["ToDt"] = todt_lcl;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["Status"] = Status;
                HttpContext.Current.Session["User"] = User;
                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else
            {
                result = 0;
            }
            return result;

        }


        [WebMethod]
        public static int WMGetGWCSKUDetailsReportData(string invoker, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage, string AllSkU)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (invoker.ToLower() == "skudetails")
            {
                HttpContext.Current.Session["ReportHeading"] = "SKU Details Report";
                if (AllSkU == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetSKUDetailsReprtData(SelectedCompany, SelectedDepartment, SelectedGroupSet, profile.DBConnection._constr);
                }
                else if (AllSkU != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetSKUDetailsSelectedRow(SelectedProducts, profile.DBConnection._constr);
                }
            }
            dsCmnRpt.Tables[0].TableName = "dsSKUDetails";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string BOM = SelectedGroupSet;
                if (SelectedGroupSet == "0") BOM = "Yes / No";
                string Image = SelectedImage;
                if (SelectedImage == "1")
                { Image = "Yes / No"; }
                string CompanyLogo = dsCmnRpt.Tables[0].Rows[0]["Logo"].ToString();
                string CompanyAddress = dsCmnRpt.Tables[0].Rows[0]["AddressLine1"].ToString();

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["BOM"] = BOM;
                HttpContext.Current.Session["Image"] = Image;
                HttpContext.Current.Session["CompanyLogo"] = CompanyLogo;
                HttpContext.Current.Session["CompanyAddress"] = CompanyAddress;
                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;

        }

        [WebMethod]
        public static int WMGetGWCBOMDetailsReportData(string invoker, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage, string AllSkU)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (invoker.ToLower() == "bomdetail")
            {
                HttpContext.Current.Session["ReportHeading"] = "Order Details Report";
                if (AllSkU == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetBOMDetailsReprtData(SelectedCompany, SelectedDepartment, SelectedGroupSet, profile.DBConnection._constr);
                }
                else if (AllSkU != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetBOMDetailsSelectedRow(SelectedProducts, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsBOMDetails";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["territory"].ToString() + "-" + dsCmnRpt.Tables[0].Rows[0]["StoreCode"].ToString();
                if (SelectedDepartment == "0") Department = "All";

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["SelObject"] = invoker;

                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else
            {
                result = 0;
            }
            return result;
        }

        [WebMethod]
        public static int WMGetImageAudit(string invoker, string FromDate, string ToDate, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedUser, string AllSkU, string ImgStatus)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            if (invoker.ToLower() == "imgaudit")
            {
                HttpContext.Current.Session["ReportHeading"] = "Image Audit Trails";
                if (ImgStatus == "Success")
                {
                    if (AllSkU == "1")
                    {
                        dsCmnRpt = UCCommonFilterClient.GetImageAuditAllPrd(FromDate, ToDate, SelectedCompany, SelectedDepartment, SelectedUser, profile.DBConnection._constr);
                    }
                    else if (AllSkU != "1")
                    {
                        dsCmnRpt = UCCommonFilterClient.GetImageAuditSelectedPrd(FromDate, ToDate, SelectedProducts, SelectedUser, profile.DBConnection._constr);
                    }
                    dsCmnRpt.Tables[0].TableName = "dsImageAudit";
                }
                else
                {
                    if (AllSkU == "1")
                    {
                        dsCmnRpt = UCCommonFilterClient.GetImageAuditFail(FromDate, ToDate, SelectedCompany, SelectedDepartment, SelectedUser, profile.DBConnection._constr);
                    }
                    else
                    {
                        dsCmnRpt = UCCommonFilterClient.GetImageAuditFailSelectedProduct(FromDate, ToDate, SelectedProducts, SelectedCompany, SelectedDepartment, SelectedUser, profile.DBConnection._constr);
                    }
                    dsCmnRpt.Tables[0].TableName = "dsImageAuditFail";
                }

            }

            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["territory"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string UsrName = dsCmnRpt.Tables[0].Rows[0]["UserName"].ToString();

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["ImgStatus"] = ImgStatus;
                HttpContext.Current.Session["UsrName"] = UsrName;
                HttpContext.Current.Session["SelObject"] = invoker;
                HttpContext.Current.Session["FrmDt"] = FromDate;
                HttpContext.Current.Session["ToDt"] = ToDate;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else
            {
                result = 0;
            }


            return result;
        }


        [WebMethod]
        public static int WMGetGWCSKUTransactionReportData(string FromDate, string ToDate, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string SelectedGroupSet, string SelectedImage)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            dsCmnRpt = UCCommonFilterClient.GetSKUTransaction(SelectedProducts, profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsSKUTrans";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string BOM = SelectedGroupSet;
                if (SelectedGroupSet == "0") BOM = "Yes / No";
                string Image = SelectedImage;
                if (SelectedImage == "1")
                { Image = "Yes / No"; }

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;
                HttpContext.Current.Session["BOM"] = BOM;
                HttpContext.Current.Session["Image"] = Image;
                HttpContext.Current.Session["SelObject"] = "SkuDetails";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;
        }

        [WebMethod]
        public static int WMGetGWCUserTransactionReportData(string hdnUserSelectedRec)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            dsCmnRpt = UCCommonFilterClient.GetUserTransaction(hdnUserSelectedRec, profile.DBConnection._constr);
            dsCmnRpt.Tables[0].TableName = "dsUserTrans";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["Company"] = "Vodafone";
                HttpContext.Current.Session["Department"] = "Vodafone";
                HttpContext.Current.Session["RoleName"] = "User";
                HttpContext.Current.Session["Active"] = "Yes";

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["SelObject"] = "user";
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;
        }
/**********/
        [WebMethod]
        public static int WMGetGWCOrderDeliveryReport(string invoker, string FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string SelectedCompany, string SelectedDepartment, string AllOrder, string AllProduct, string SelectedDriver, string SelectedPaymentMode)
        {
            int result = 0;

         
            DataSet dsCmnRpt = new DataSet();
                iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
                CustomProfile profile = CustomProfile.GetProfile();

                string frmdt_lcl, todt_lcl;
                frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToString("yyyy/MM/dd"); // Value.ToString("yyyy/MM/dd");
                todt_lcl = Convert.ToDateTime(ToDate).Date.ToString("yyyy/MM/dd"); //.Value.ToString("yyyy/MM/dd");

                if (invoker.ToLower() == "orderdelivery")
                {
                    HttpContext.Current.Session["ReportHeading"] = "Order Deilvery Report";
                    if (AllOrder == "1")
                    {
                        
                        dsCmnRpt = UCCommonFilterClient.GetAllOrderDelivery(FromDate, ToDate, SelectedCompany, SelectedDepartment, SelectedDriver, SelectedPaymentMode, profile.DBConnection._constr);

                    }
                    else if (AllOrder != "1")
                    {
                        dsCmnRpt = UCCommonFilterClient.GetAllOrderDeliveryDataSelectedRow(SelectedOrder, profile.DBConnection._constr);
                    }

                
            }
                dsCmnRpt.Tables[0].TableName = "dsOrderDelivery";
                if (dsCmnRpt.Tables[0].Rows.Count > 0)
                {
                    string Company = dsCmnRpt.Tables[0].Rows[0]["Company"].ToString();
                    if (SelectedCompany == "0") Company = "All";
                    string Department = dsCmnRpt.Tables[0].Rows[0]["Department"].ToString();// +"-" + dsCmnRpt.Tables[0].Rows[0]["DepartmentID"].ToString();
                    if (SelectedDepartment == "0") Department = "All";
                    string DriverName = dsCmnRpt.Tables[0].Rows[0]["DriverName"].ToString();
                    if (SelectedDriver == "0") DriverName = "All";
                    string PaymentMode = dsCmnRpt.Tables[0].Rows[0]["PaymentMode"].ToString();
                    if (SelectedPaymentMode == "0") PaymentMode = "All";


                    HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                    HttpContext.Current.Session["FrmDt"] = frmdt_lcl;
                    HttpContext.Current.Session["ToDt"] = todt_lcl;
                    HttpContext.Current.Session["Company"] = Company;
                    HttpContext.Current.Session["Department"] = Department;
                    HttpContext.Current.Session["DriverName"] = DriverName;
                    HttpContext.Current.Session["PaymentMode"] = PaymentMode;



                    HttpContext.Current.Session["SelObject"] = invoker;
                    result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
                }
                else { result = 0; }
        
                return result;
            
        }

        [WebMethod]
        public static int WMGetGWCSLAReport(string invoker, string FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string SelectedCompany, string SelectedDepartment, string AllOrder, string AllProduct, string SelectedStatus, string SelectedDriver, string SelectedDeliveryType)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string frmdt_lcl, todt_lcl;
            frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToString("yyyy/MM/dd"); // Value.ToString("yyyy/MM/dd");
            todt_lcl = Convert.ToDateTime(ToDate).Date.ToString("yyyy/MM/dd"); //.Value.ToString("yyyy/MM/dd");

            if (invoker.ToLower() == "sla")
            {
                HttpContext.Current.Session["ReportHeading"] = "Service Level Agreement Report";
                if (AllOrder == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllSlaData(FromDate, ToDate, SelectedCompany, SelectedDepartment, SelectedStatus, SelectedDriver, SelectedDeliveryType, profile.DBConnection._constr);

                }
                else if (AllOrder != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllSlaDataSelectedRow(SelectedOrder, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsSla";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["Company"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Department"].ToString();// +"-" + dsCmnRpt.Tables[0].Rows[0]["DepartmentID"].ToString();
                if (SelectedDepartment == "0") Department = "All";
                string Status = dsCmnRpt.Tables[0].Rows[0]["Status"].ToString();
                if (SelectedStatus == "0") Status = "All";
                string DriverName = dsCmnRpt.Tables[0].Rows[0]["DriverName"].ToString();
                if (SelectedDriver == "0") DriverName = "All";
                string DeliveryType = dsCmnRpt.Tables[0].Rows[0]["DeliveryType"].ToString();
                if (SelectedDeliveryType == "0") DeliveryType = "All";


                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FrmDt"] = frmdt_lcl;
                HttpContext.Current.Session["ToDt"] = todt_lcl;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;              
                HttpContext.Current.Session["DriverName"] = DriverName;
                HttpContext.Current.Session["DeliveryType"] = DeliveryType;



                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;

        }


        [WebMethod]
        public static int WMGetGWCTotalDeliveryVsTotalRequestReport(string invoker, string FromDate, string ToDate, string SelectedProducts, string SelectedCompany, string SelectedDepartment, string AllSkU)
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            if (invoker.ToLower() == "totaldeliveryvstotalrequest")
            {
                HttpContext.Current.Session["ReportHeading"] = "Total Delivery Vs Total Request";
                if (AllSkU == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetToTalDeliveryVSTotalReq(FromDate, ToDate, SelectedCompany, SelectedDepartment, profile.DBConnection._constr);
                }
                else if (AllSkU != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetToTalDeliveryVSTotalReqDataSelectedRow(FromDate, ToDate, SelectedProducts, profile.DBConnection._constr);
                }
                
            }

            dsCmnRpt.Tables[0].TableName = "dsTDVSTR";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string Company = dsCmnRpt.Tables[0].Rows[0]["CompanyName"].ToString();
                if (SelectedCompany == "0") Company = "All";
                string Department = dsCmnRpt.Tables[0].Rows[0]["Territory"].ToString();// +"-" + dsCmnRpt.Tables[0].Rows[0]["DepartmentID"].ToString();
                if (SelectedDepartment == "0") Department = "All";

                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FrmDt"] = FromDate;
                HttpContext.Current.Session["ToDt"] = ToDate;
                HttpContext.Current.Session["Company"] = Company;
                HttpContext.Current.Session["Department"] = Department;


                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;
        }


        [WebMethod]
        public static int WMGetPurchaseOrderReport(string invoker, string FromDate, string ToDate, string SelectedOrder, string SelectedProduct, string AllOrder, string AllProduct, string SelectedStatus, string Selectedvendor )
        {
            int result = 0;
            DataSet dsCmnRpt = new DataSet();
            iUCCommonFilterClient UCCommonFilterClient = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string frmdt_lcl, todt_lcl;
            frmdt_lcl = Convert.ToDateTime(FromDate).Date.ToString("yyyy/MM/dd"); // Value.ToString("yyyy/MM/dd");
            todt_lcl = Convert.ToDateTime(ToDate).Date.ToString("yyyy/MM/dd"); //.Value.ToString("yyyy/MM/dd");

            if (invoker.ToLower() == "purchaseorder")
            {
                HttpContext.Current.Session["ReportHeading"] = "Purchase Order";
                if (AllOrder == "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllPurchaseOrderReport(FromDate, ToDate, SelectedStatus, Selectedvendor, profile.DBConnection._constr);

                }
                else if (AllOrder != "1")
                {
                    dsCmnRpt = UCCommonFilterClient.GetAllPurchaseOrderSelectedRowReport(FromDate, ToDate, SelectedOrder, profile.DBConnection._constr);
                }

            }
            dsCmnRpt.Tables[0].TableName = "dsPOR";
            if (dsCmnRpt.Tables[0].Rows.Count > 0)
            {
                string PurchaseOrderNo = dsCmnRpt.Tables[0].Rows[0]["POOrderNo"].ToString();
                string Product = dsCmnRpt.Tables[0].Rows[0]["Name"].ToString();
                string Status = dsCmnRpt.Tables[0].Rows[0]["StatusName"].ToString();
                if (SelectedStatus == "0") Status = "All";
                string Vendor = dsCmnRpt.Tables[0].Rows[0]["Vendor"].ToString();
                if (Selectedvendor == "0") Vendor = "All";


                HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
                HttpContext.Current.Session["FrmDt"] = frmdt_lcl;
                HttpContext.Current.Session["ToDt"] = todt_lcl;
                HttpContext.Current.Session["Status"] = Status;
                HttpContext.Current.Session["PurchaseOrderNo"] = PurchaseOrderNo;
                HttpContext.Current.Session["Vendor"] = Vendor;
                HttpContext.Current.Session["Product"] = Product;



                HttpContext.Current.Session["SelObject"] = invoker;
                result = Convert.ToInt16(dsCmnRpt.Tables[0].Rows.Count);
            }
            else { result = 0; }
            return result;

        }
/**********/
        #region [GWC]all report Dropdown
        [WebMethod]
        public static List<mTerritory> PMGetDepartmentList(int CompanyID)
        {
            List<mTerritory> DeptLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (profile.Personal.UserType == "Super Admin")
            {
                DeptLst = UCCommonFilter.GetDepartmentList(CompanyID, profile.DBConnection._constr).ToList();
            }
            else
            {
                DeptLst = UCCommonFilter.GetAddedDepartmentList(CompanyID, profile.Personal.UserID, profile.DBConnection._constr).ToList();
            }
            return DeptLst;
        }

        [WebMethod]
        public static List<mTerritory> PMGetAllDepartment()
        {
            List<mTerritory> DeptLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            DeptLst = UCCommonFilter.GetAllDepartmentList(profile.DBConnection._constr).ToList();

            return DeptLst;
        }

        [WebMethod]
        public static List<mBOMHeader> PMGetGroupsetList(int CompanyID, int DeptID)
        {
            List<mBOMHeader> GroupList = new List<mBOMHeader>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            GroupList = UCCommonFilter.GetGroupSet(CompanyID, DeptID, profile.DBConnection._constr).ToList();

            return GroupList;
        }

        [WebMethod]
        public static List<mBOMHeader> PMGetAllGroupsetList()
        {
            List<mBOMHeader> GroupList = new List<mBOMHeader>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            GroupList = UCCommonFilter.GetAllGroupsetList(profile.DBConnection._constr).ToList();

            return GroupList;
        }

        [WebMethod]
        public static List<mBOMHeader> PMGetGroupsetListByDept(int DeptID)
        {
            List<mBOMHeader> GroupList = new List<mBOMHeader>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            GroupList = UCCommonFilter.GetGroupSetByDept(DeptID, profile.DBConnection._constr).ToList();

            return GroupList;
        }


        [WebMethod]
        public static List<mBOMHeader> PMGetGroupSetListByCompanyId(int CompanyID)
        {
            List<mBOMHeader> GroupList = new List<mBOMHeader>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            GroupList = UCCommonFilter.GetGroupSetByCompany(CompanyID, profile.DBConnection._constr).ToList();

            return GroupList;
        }

        [WebMethod]
        public static List<VW_GetUserInformation> PMGetUserList(string CompanyID, string DeptID)
        {
            List<VW_GetUserInformation> UserList = new List<VW_GetUserInformation>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            UserList = UCCommonFilter.GetUser(Convert.ToInt32(CompanyID), Convert.ToInt32(DeptID), profile.DBConnection._constr).ToList();

            return UserList;
        }

        [WebMethod]
        public static List<SP_GWC_GetUserInfo_Result> WMGetUserList(string SelectedDepartment, string selectedCompany)
        {
            DataSet ds = new DataSet();
            List<SP_GWC_GetUserInfo_Result> UserList = new List<SP_GWC_GetUserInfo_Result>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            UserList = UCCommonFilter.GetUsrLst1(selectedCompany, SelectedDepartment, profile.DBConnection._constr).ToList();

            return UserList;
        }

        #endregion


        private void loadstring()
        {
            QueryParameter = Request.QueryString["invoker"];
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            
            //else if (QueryParameter == "imgaudit")
            //{
            //    lblRptName.Text = rm.GetString("ImageAuditTrails", ci);
            //}
        }

        [WebMethod]
        public static List<VW_DeptWisePaymentMethod> WMGetPaymentMethod(long Dept)
        {
            List<VW_DeptWisePaymentMethod> AdrsLst = new List<VW_DeptWisePaymentMethod>();

            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            AdrsLst = objService.GEtDeptPaymentmethod(Dept, profile.DBConnection._constr).ToList();
            return AdrsLst;
        }
    }
}