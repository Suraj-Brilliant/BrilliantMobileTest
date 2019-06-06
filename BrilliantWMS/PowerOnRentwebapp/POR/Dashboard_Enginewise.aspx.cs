using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BrilliantWMS.DashBoard;
using BrilliantWMS.UCProductSearchService;
using System.Web.UI.DataVisualization.Charting;
using BrilliantWMS.Login;
using BrilliantWMS.PORServiceSiteMaster;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServicePartRequest;
//using PowerOnRentwebapp.ServicePRSReports;
using BrilliantWMS.UserCreationService;
using Microsoft.Reporting.WebForms;

namespace BrilliantWMS.POR
{
    public partial class Dashboard_Enginewise : System.Web.UI.Page
    {
        string sqlConn, prd, eng, sit;
        string str;
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
                frmdate.Date = DateTime.Now.Date;
                todate.Date = DateTime.Now.Date;
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

        public void fillengine()
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
            //iPartRequisitionClient RequsitonClient = new iPartRequisitionClient();

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

            string fdt = frmdate.Date.Value.ToString("yyyy-MM-dd");
            string tdt = todate.Date.Value.ToString("yyyy-MM-dd");

            iEngineMasterClient EngineClient = new iEngineMasterClient();
            iProductMasterClient productClient = new iProductMasterClient();
            //iPartRequisitionClient RequsitonClient = new iPartRequisitionClient();

            if (q != null)
            {
                chkProductLst.DataSource = productClient.GetProductofEngine(fdt, tdt, p, q, profile.DBConnection._constr);
                chkProductLst.DataBind();
            }
        }

        private void ChartBind()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            objclsChartProperty.ConnectionString = sqlConn.ToString();
            objclsChartProperty.ReportID = 48;
            objclsChartProperty.QueryParameter = "";

            iUserCreationClient UserCreationClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            str = hfCount.Value;
            //if (ddlSite.SelectedIndex == 0) { sit = UserCreationClient.GetTerritoryID_FromUserId(profile.Personal.UserID, profile.DBConnection._constr); }
            //else { sit = Convert.ToString(ddlSite.SelectedValue); }

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

            string frmd = frmdate.Date.Value.ToString("yyyy-MM-dd");
            string tod = todate.Date.Value.ToString("yyyy-MM-dd");

            objclsChartProperty.QueryParameter = "declare @Snm varchar(5000);declare @Eng varchar (5000);declare @Pid varchar (5000);declare @sdt datetime;declare @edt datetime;set @Snm='" + sit + "' set @Eng='" + eng + "' set @Pid='" + prd + "' set @sdt='" + frmd + "' set @edt='" + tod + "'   ";

            Dashboard1.BindChart(objclsChartProperty);
            Chart chart1 = new Chart();
            chart1 = (Chart)Dashboard1.FindControl("chart1");
            if (chart1.Series.Count == 0)
            { Dashboard1.Visible = false; }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
          fillProduct();            
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            // fillengine();
        }

        protected void btnSubmitFinal_Click(object sender, EventArgs e)
        {
            ChartBind();
        }

        protected void ddlSite_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            fillengine();
        }
    }
}