using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PowerOnRentwebapp.DashBoard;
using BrilliantWMS.UCProductSearchService;
using System.Web.UI.DataVisualization.Charting;
using PowerOnRentwebapp.Login;
using BrilliantWMS.PORServiceSiteMaster;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServicePartRequest;
//using PowerOnRentwebapp.ServicePRSReports;
//using PowerOnRentwebapp.UserCreationService;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.UserCreationService;

namespace WebClientElegantCRM.POR
{
    public partial class Report_SiteConsumption : System.Web.UI.Page
    {
        string sqlConn, prd, eng, sit;
        string str;
        public DataSet dsLstRpt = new DataSet();
        string ObjectName, QueryParameter;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            sqlConn = "Data Source=" + profile.DBConnection._constr[0] + ";Initial Catalog=" + profile.DBConnection._constr[1] + "; User ID=" + profile.DBConnection._constr[3] + "; Password=" + profile.DBConnection._constr[2] + ";";
            if (!IsPostBack)
            {
                FrmDate.Date = DateTime.Now.Date;
                ToDate.Date = DateTime.Now.Date;
                fillSite();

            }

        }
        public void fillSite()
        {
            string sid;
            iUserCreationClient UserCreationClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            sid = UserCreationClient.GetTerritoryID_FromUserId(profile.Personal.UserID, profile.DBConnection._constr);

            ddlSite.DataSource = UserCreationClient.GetSiteNameFromId(sid, profile.DBConnection._constr);
            ddlSite.DataBind();

            ListItem lst1 = new ListItem();
            lst1.Text = "Select All";
            lst1.Value = "0";
            ddlSite.Items.Insert(0, lst1);
        }

        public void fillEngine()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            iUserCreationClient UserCreationClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            str = hfCount.Value;
            
            if (ddlSite.SelectedIndex == 0) { eng = str; }
            else { eng = str; }

            string p = eng;
            iEngineMasterClient EngineClient = new iEngineMasterClient();
            iProductMasterClient productClient = new iProductMasterClient();
            iPartRequisitionClient RequsitonClient = new iPartRequisitionClient();

            if (p != "")
            {
                chkEngineList.DataSource = EngineClient.GetEngineOfSite(p, profile.DBConnection._constr);
                chkEngineList.DataBind();
            }
        }

        public void fillProduct()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            objclsChartProperty.QueryParameter = "";
            iUserCreationClient UserCreationClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            string abc = hfEng.Value;
            str = hfCount.Value;
            
            if (ddlSite.SelectedIndex == 0) { eng = str; }
            else { eng = str; }

            string p = eng;

            for (int y = 0; y <= chkEngineList.Items.Count - 1; y++)
            {
                if (prd == null)
                { if (chkEngineList.Items[y].Selected == true) { prd = prd + chkEngineList.Items[y].Value; } }
                else { if (chkEngineList.Items[y].Selected == true) { prd = prd + "," + chkEngineList.Items[y].Value; } }
            }
            string q = prd;

            string fdt = FrmDate.Date.Value.ToString("yyyy-MM-dd");
            string tdt = ToDate.Date.Value.ToString("yyyy-MM-dd");

            iEngineMasterClient EngineClient = new iEngineMasterClient();
            iProductMasterClient productClient = new iProductMasterClient();
            iPartRequisitionClient RequsitonClient = new iPartRequisitionClient();

            if (q != null)
            {
                chkProductLst.DataSource = productClient.GetProductofEngine(fdt, tdt, p, q, profile.DBConnection._constr);
                chkProductLst.DataBind();
            }
        }

        protected void QueryBind()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            objclsChartProperty.ConnectionString = sqlConn.ToString();
            iUserCreationClient UserCreationClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            str = hfCount.Value;
            if (ddlSite.SelectedIndex == 0) { sit = str; }
            else { sit = str; }
            
            for (int y = 0; y <= chkEngineList.Items.Count - 1; y++)
            {
                if (eng == null)
                { if (chkEngineList.Items[y].Selected == true) { eng = eng + chkEngineList.Items[y].Value; } }
                else { if (chkEngineList.Items[y].Selected == true) { eng = eng + "," + chkEngineList.Items[y].Value; } }
            }

            for (int z = 0; z <= chkProductLst.Items.Count - 1; z++)
            {
                if (prd == null)
                { if (chkProductLst.Items[z].Selected == true) { prd = prd + chkProductLst.Items[z].Value; } }
                else { if (chkProductLst.Items[z].Selected == true) { prd = prd + "," + chkProductLst.Items[z].Value; } }
            }

            string frmd = FrmDate.Date.Value.ToString("yyyy-MM-dd");
            string tod = ToDate.Date.Value.ToString("yyyy-MM-dd");


        }

        protected void FillDataSet()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ServicePRSReports.iPRSReportsClient objPRSReports = new ServicePRSReports.iPRSReportsClient();
            
            ObjectName = "SiteWiseConsumption";
            QueryParameter  = "SiteWiseConsumption";
            Session["ReportHeading"] = "Site Wise Consumption";

            sit = str;
           string  eng1 = eng;
           string  prd1 = prd;
           string frmd = FrmDate.Date.Value.ToString("yyyy-MM-dd");
           string tod = ToDate.Date.Value.ToString("yyyy-MM-dd");

            dsLstRpt=objPRSReports.GetQueryData(sit,eng1,prd1,frmd,tod,profile.Personal.UserID,profile.DBConnection._constr);

            Session["ReportDS"]=dsLstRpt;
            Session["FromDt"] = FrmDate.Date.Value.ToString("yyyy-MM-dd");
            Session["ToDt"] = ToDate.Date.Value.ToString("yyyy-MM-dd");
            Session["SelObject"] = QueryParameter;

            DataSet ds = new DataSet();
            ds = dsLstRpt;
            ds.Tables[0].TableName = "dsSiteConsumption";
            ReportDataSource rds = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
            Response.Write("<script>window.open('../POR/Reports/ReportViewer.aspx', null, 'height=510, width=990,status= 0, resizable= 0, scrollbars=0, toolbar=0,location=0,menubar=0, screenX=0; screenY=0');</script>");

        }
        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillEngine();
        }
        protected void btnPrd_Click(object sender,EventArgs e)
        {
            fillProduct();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            QueryBind();

            FillDataSet();
        }
    }
}