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
namespace BrilliantWMS.POR
{
    public partial class DashboardPOR : System.Web.UI.Page
    {

        string sqlConn, QueryParameter;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            QueryParameter = Request.QueryString["invoker"];

            CustomProfile profile = CustomProfile.GetProfile();
            sqlConn = "Data Source=" + profile.DBConnection._constr[0] + ";Initial Catalog=" + profile.DBConnection._constr[1] + "; User ID=" + profile.DBConnection._constr[3] + "; Password=" + profile.DBConnection._constr[2] + ";";

            if (QueryParameter.ToLower() == "purchaseorder")
            {
                ChartBind(52);
            }
            else if (QueryParameter.ToLower() == "sostatus")
            {
                ChartBind(50);
            }
            else if (QueryParameter.ToLower() == "postatus")
            {
                ChartBind(49);
            }
            else if (QueryParameter.ToLower() == "salesorder")
            {
                ChartBind(53);
            }
            else if (QueryParameter.ToLower() == "utilization")
            {
                ChartBind(57);
            }
            else if (QueryParameter.ToLower() == "all")
            {
                ChartBind();
            }

        }
        private void ChartBind(int chartID)
        {
            ChartProperty objclsChartProperty = new ChartProperty();

            objclsChartProperty.QueryParameter = "";
            objclsChartProperty.ConnectionString = sqlConn.ToString();
            objclsChartProperty.ReportID = chartID;
            DashBoard1.BindChart(objclsChartProperty);
            Chart chart1 = new Chart();
            chart1 = (Chart)DashBoard1.FindControl("Chart1");
            if (chart1.Series.Count == 0)
            { DashBoard1.Visible = false; }


        }


        private void ChartBind()
        {
            ChartProperty objclsChartProperty = new ChartProperty();

            objclsChartProperty.QueryParameter = "";
            objclsChartProperty.ConnectionString = sqlConn.ToString();

            //For Porduct
            objclsChartProperty.ReportID = 41;
            //DashBoard1.BindChart(objclsChartProperty);
            Chart chart1 = new Chart();
            chart1 = (Chart)DashBoard1.FindControl("Chart1");
            if (chart1.Series.Count == 0)
            { DashBoard1.Visible = false; }



            //For Engine
            //objclsChartProperty.ReportID = 38;
            //DashBoard2.BindChart(objclsChartProperty);
            //Chart chart2 = new Chart();
            //chart2 = (Chart)DashBoard2.FindControl("Chart1");
            //if (chart1.Series.Count == 0)
            //{ DashBoard2.Visible = false; }

            //  // For Site
            //objclsChartProperty.ReportID = 39;
            //DashBoard3.BindChart(objclsChartProperty);
            //Chart chart3 = new Chart();
            //chart3 = (Chart)DashBoard3.FindControl("Chart1");
            //if (chart1.Series.Count == 0)
            //{ DashBoard3.Visible = false; }

        }
    }
}