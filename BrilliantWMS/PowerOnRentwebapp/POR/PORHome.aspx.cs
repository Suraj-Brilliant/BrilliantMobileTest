using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.InboxService;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BrilliantWMS.DashBoard;
using BrilliantWMS.UCProductSearchService;
using System.Web.UI.DataVisualization.Charting;

namespace BrilliantWMS.POR
{
    public partial class Home : System.Web.UI.Page
    {
        string sqlConn;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            iInboxClient InboxService = new iInboxClient();
            try
            {

                CustomProfile profile = CustomProfile.GetProfile();

                //if (hndLinkValue.Value == "") hndLinkValue.Value = "All";
                GVInboxPOR.DataSource = InboxService.GetInboxDetailBySiteUserID(profile.Personal.UserID, "All", profile.DBConnection._constr).ToList();
                GVInboxPOR.DataBind();

                sqlConn = "Data Source=" + profile.DBConnection._constr[0] + ";Initial Catalog=" + profile.DBConnection._constr[1] + "; User ID=" + profile.DBConnection._constr[3] + "; Password=" + profile.DBConnection._constr[2] + ";";
                ChartBind();
            }
            catch { }
            finally { InboxService.Close(); }
        }


        private void ChartBind()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            //objclsChartProperty.ReportID = 1;
            objclsChartProperty.QueryParameter = "";
            objclsChartProperty.ConnectionString = sqlConn.ToString();


            // For Site
            objclsChartProperty.ReportID = 39;
            DashBoard1.BindChart(objclsChartProperty);
            Chart chart1 = new Chart();
            chart1 = (Chart)DashBoard1.FindControl("Chart1");
            if (chart1.Series.Count == 0)
            { DashBoard1.Visible = false; }

            //For Engine
            objclsChartProperty.ReportID = 38;
            DashBoard2.BindChart(objclsChartProperty);
            Chart chart2 = new Chart();
            chart1 = (Chart)DashBoard1.FindControl("Chart1");
            if (chart1.Series.Count == 0)
            { DashBoard2.Visible = false; }

            //For Porduct
            // objclsChartProperty.ReportID = 41;
            //// DashBoard3.BindChart(objclsChartProperty);
            // Chart chart3 = new Chart();
            // chart1 = (Chart)DashBoard1.FindControl("Chart1");
            // if (chart1.Series.Count == 0)
            // { DashBoard3.Visible = false; }


        }

    }
}