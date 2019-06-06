using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DashBoard;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using BrilliantWMS.Login;
namespace BrilliantWMS.DashBoard
{
    public partial class DashboardInvoice : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); Page.Theme = profile.Personal.Theme; }

        DataSet dsdash = new DataSet();
        DataSet dsProduct = new DataSet();
        DataSet dsYear = new DataSet();
        ChartProperty objChartProperty = new ChartProperty();
        string connectionString = "";//= ConfigurationManager.ConnectionStrings["BISPL_CRMDBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            connectionString = "Data Source=" + profile.DBConnection._constr[0] + ";Initial Catalog=" + profile.DBConnection._constr[1] + "; User ID=" + profile.DBConnection._constr[1] + "; Password=" + profile.DBConnection._constr[2] + ";";
            if (IsPostBack != true)
            {
                //UCProductSearch1.Visible = false;

                string stryear = "select year(MAX(LeadDate))as [Year] from tLeadHead union select year(Min(LeadDate))as [Year] from tLeadHead";
                string Strproduct = "select ID,Name from mProduct where Active='Y'";

                dsYear = filldataset(stryear);
                rbtnyear.DataSource = dsYear;
                rbtnyear.DataBind();

                rbtnyear.SelectedValue = DateTime.Now.Year.ToString();
                BindInvoiceModule(rbtnyear.SelectedValue);

            }


        }

        private void BindInvoiceModule(string year)
        {
            dsdash.Reset();
            string str = "select * from mReportMaster where ReportType ='Dashboard' and ModuleName='InvoiceModule'";
            dsdash = filldataset(str);

            for (int i = 0; i <= dsdash.Tables[0].Rows.Count - 1; i++)
            {
                DashBoard DashBoard1 = (DashBoard)Page.LoadControl("DashBoard.ascx");
                objChartProperty.ConnectionString = connectionString;
                objChartProperty.QueryParameter = "declare @year varchar(50) set @year='" + year + "'";
                objChartProperty.ReportID = Convert.ToInt32(dsdash.Tables[0].Rows[i][0]);
                plsDashboard.Controls.Add(DashBoard1);
                updPnl_Dashboard.ContentTemplateContainer.Controls.Add(DashBoard1);
                DashBoard1.BindChart(objChartProperty);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected DataSet filldataset(String strquery)
        {
            DataSet dsfill = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strquery, connectionString);
            dsfill.Reset();
            da.Fill(dsfill);
            return dsfill;

        }

        protected void rbtnyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoiceModule(rbtnyear.SelectedValue);

        }
    }
}