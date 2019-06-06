using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.Login;
using System.Data;
/*using WebClientElegantCRM.PaymentDetailService;*/

namespace BrilliantWMS.Invoice
{
    public partial class UCPaymentDetail : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
       public DataSet FillOutstandingGrid(string IDs = "0")
        {
            hdnPaymentDetailAccountID.Value = IDs;
           /* iPaymentDetailClient paymentDetailClient = new iPaymentDetailClient();*/
            DataSet dsPaymentDetail = new DataSet();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVDisply.DataSourceID = null;
                GVDisply.DataBind();
                /*dsPaymentDetail = paymentDetailClient.GetPaymentDetails(IDs, ddlFY.SelectedItem.Value, profile.Personal.CompanyID, profile.DBConnection._constr);
                GVDisply.DataSource = dsPaymentDetail;
                GVDisply.DataBind();*/
                if (dsPaymentDetail.Tables.Count > 0) { dsPaymentDetail.Tables[0].TableName = "rptDSOutstandingDetail"; }
                return dsPaymentDetail;
            }
            catch (Exception ex) { return dsPaymentDetail; }
            finally { /*paymentDetailClient.Close();*/ }
        }

        protected void BtnExportToExcel_OnClick(object sender, EventArgs e)
        {
            ShowReport("ReportBuilder/rptOutstandingDetail.rdlc", "OutstandingDetails_" + ddlFY.SelectedItem.Value, FillOutstandingGrid(), "Excel");
        }

        protected void BtnExportToPDF_OnClick(object sender, EventArgs e)
        {
            ShowReport("ReportBuilder/rptOutstandingDetail.rdlc", "OutstandingDetails_" + ddlFY.SelectedItem.Value, FillOutstandingGrid(), "PDF");
        }

        protected void BtnReload_Click(object sender, EventArgs e)
        {
            FillOutstandingGrid();
        }

        protected void ShowReport(string rptPath_RDLC, string dispName, DataSet rptDatasrc, string reportType)
        {
            try
            {
                //ShowReport(Me, "Report_Kaveri\RptPaySlip_New.rdlc", DisplayName, New ReportDataSource("DSReports_DT_SALARYSLIP", DS.Tables(0)), "PDF")
                LocalReport lr = new LocalReport();
                lr.EnableExternalImages = true;
                lr.ReportPath = rptPath_RDLC;
                lr.DisplayName = dispName;

                for (Int16 I = 0; I <= rptDatasrc.Tables.Count - 1; I++)
                {
                    lr.DataSources.Add(new ReportDataSource(rptDatasrc.Tables[I].TableName, rptDatasrc.Tables[I]));
                }
                lr.Refresh();

                string extension = "";
                switch (reportType)
                {
                    case "Excel": extension = "xlsx"; break;
                    case "PDF": extension = "pdf"; break;
                    default: extension = "pdf"; break;
                }

                string mimeType = null;
                string encoding = null;
                string fileNameExtension = null;
                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn.microsoft.com/en-us/library/ms155397.aspx
                string deviceInfo = "<DeviceInfo>" + " <OutputFormat>PDF</OutputFormat>" + " <PageWidth>8.5in</PageWidth>" + " <PageHeight>11in</PageHeight>" + " <MarginTop>0.5in</MarginTop>" + " <MarginLeft>0.25in</MarginLeft>" + " <MarginRight>0.25in</MarginRight>" + " <MarginBottom>0.5in</MarginBottom>" + "</DeviceInfo>";
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                string[] streams = null;
                byte[] renderedBytes = null;
                //Render the report
                renderedBytes = lr.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
                //Clear the response stream and write the bytes to the outputstream
                //Set content-disposition to "attachment" so that user is prompted to take an action
                //on the file (open or save)
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment;open; filename=" + lr.DisplayName + "." + extension);
                Response.BinaryWrite(renderedBytes);
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ShowReport");
                Response.Write(ex);
            }
            finally
            {
            }
        }
    }
}