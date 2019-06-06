using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DashBoard;
using BrilliantWMS.Login;
using System.Web.UI.DataVisualization.Charting;

namespace BrilliantWMS.Inbox
{
    public partial class Inbox : System.Web.UI.Page
    {
        string sqlConn;
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Inbox";
            CustomProfile profile = CustomProfile.GetProfile();
            sqlConn = "Data Source=" + profile.DBConnection._constr[0] + ";Initial Catalog=" + profile.DBConnection._constr[1] + "; User ID=" + profile.DBConnection._constr[3] + "; Password=" + profile.DBConnection._constr[2] + ";";
            ChartBind();
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        private void ChartBind()
        {
            ChartProperty objclsChartProperty = new ChartProperty();
            objclsChartProperty.QueryParameter = "";
            objclsChartProperty.ConnectionString = sqlConn.ToString();

            //For Engine
            objclsChartProperty.ReportID = 38;
            DashBoard1.BindChart(objclsChartProperty);
            Chart chart2 = new Chart();
            chart2 = (Chart)DashBoard1.FindControl("Chart1");
            if (chart2.Series.Count == 0)
            { DashBoard1.Visible = false; }
        }
    }
}