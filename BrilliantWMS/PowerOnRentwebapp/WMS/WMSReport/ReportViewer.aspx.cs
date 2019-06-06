using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using BrilliantWMS.Login;
using BrilliantWMS.CompanySetupService;
using System.IO;

namespace BrilliantWMS.WMS.WMSReport
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowReport();
            }
        }

        protected void ShowReport()
        {
            string language = Session["Lang"].ToString();
            DataSet ds = new DataSet();
            if (Session["ReportDS"] != null)
            {
                string Teststr;
                Teststr = Session["SelObject"].ToString();

                ds = (DataSet)Session["ReportDS"];
                if (ds.Tables[0].TableName == "DataSet1")
                {
                    ReportDataSource rds = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);

                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds);

                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/LabelPrinting.rdlc";

                    RptViewer1.LocalReport.SetParameters(RptParameters1());

                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
            }
        }

        protected void RptViewer1_Load(object sender,EventArgs e)
        {
            string exportOption = "Word";
            RenderingExtension extension = RptViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOption, StringComparison.CurrentCultureIgnoreCase));
            if (extension != null)
            {
                System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                fieldInfo.SetValue(extension, false);
            }
        }

        private List<ReportParameter> RptParameters1()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<ReportParameter> parameters = new List<ReportParameter>();
            mCompany compdetails = new mCompany();
            iCompanySetupClient compclient = new iCompanySetupClient();
            compdetails = compclient.GetCompanyById(profile.Personal.CompanyID, profile.DBConnection._constr);
           // parameters.Add(new ReportParameter("CompName", compdetails.Name));
            //parameters.Add(new ReportParameter("CompAdd", compdetails.AddressLine1));
            //parameters.Add(new ReportParameter("FromDt", Session["FromDt"].ToString()));
            //parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            //parameters.Add(new ReportParameter("ReportHeading", Session["ReportHeading"].ToString()));
            //parameters.Add(new ReportParameter("Generator", Session["Generator"].ToString()));
            // parameters.Add(new ReportParameter("CompLogo","../MasterPage/Logo/" + compdetails.LogoPath));
            return parameters;
        }
    }
}