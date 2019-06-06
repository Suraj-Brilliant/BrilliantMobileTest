using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

namespace BrilliantWMS.WMS
{
    public partial class RptViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string val = Request.QueryString["Val"];
                string fromDt = Request.QueryString["fromDt"];
                string ToDt = Request.QueryString["ToDt"];
                string RptType = Request.QueryString["RptType"];
                string RptID = Request.QueryString["RptID"];
                ShowListRpt(val, fromDt, ToDt, RptType, RptID);
            }
        }

        public void ShowListRpt(string val, string fromDt, string ToDt, string RptType, string RptID)
        {
            DataSet ds = new DataSet();
            ds = BrilliantWMS.CommonControlReport.UC_RptFilter.GetFilterData(RptType, val, RptID, fromDt, ToDt);
            string path = ds.Tables[2].Rows[0][0].ToString();
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath(path);

            ReportDataSource datasource = new ReportDataSource(ds.Tables[1].Rows[0][0].ToString(), ds.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
        }
    }
}