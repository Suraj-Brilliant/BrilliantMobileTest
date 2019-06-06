using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.BindMenuService;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace BrilliantWMS.CommonControls
{
    public partial class UCToolbarHTML : System.Web.UI.UserControl
    {
        ProBindMenu objProBindMenu = new ProBindMenu();
        public ProBindMenu ToolbarAccess1(string ObjectName, String eventID)
        {
            BrilliantWMS.BindMenuService.iBindMenuClient objiBindMenuClient = new BrilliantWMS.BindMenuService.iBindMenuClient();
            try
            {

                CustomProfile profile = CustomProfile.GetProfile();
                //objProBindMenu._constP = profile.DBConnection._constr;
                objProBindMenu.CompanyCode = profile.Personal.CompanyID;
                objProBindMenu.UserCode = profile.Personal.UserID;
                List<ProBindMenu> objLstProBindMenu = new List<ProBindMenu> { };
                objProBindMenu.ObjectCode = ObjectName;
                objLstProBindMenu = objiBindMenuClient.CheckOperaction(objProBindMenu).ToList();
                if (objLstProBindMenu.Count > 0)
                {
                    objProBindMenu = objLstProBindMenu[0];
                    objProBindMenu.ActiveButton = eventID;
                    objProBindMenu = objiBindMenuClient.ActiveButtonClickEvent(objProBindMenu);

                }
                return objProBindMenu;
            }
            catch (Exception ex) { Login.Profile.ErrorHandling(ex, "UCToolbarHTML", "ToolbarAccess1"); return objProBindMenu; }
            finally { objiBindMenuClient.Close(); }

        }

        public Boolean ConvertToFill(String ObjectName)
        {
            BrilliantWMS.BindMenuService.iBindMenuClient objiBindMenuClient = new BrilliantWMS.BindMenuService.iBindMenuClient();
            Boolean flg = false;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //objProBindMenu._constP = profile.DBConnection._constr;
                objProBindMenu.CompanyCode = profile.Personal.CompanyID;
                objProBindMenu.UserCode = profile.Personal.UserID;
                List<ProBindMenu> objLstProBindMenu = new List<ProBindMenu> { };
                objProBindMenu.ObjectCode = ObjectName;
                objLstProBindMenu = objiBindMenuClient.CheckOperaction(objProBindMenu).ToList();
                if (objLstProBindMenu.Count > 0)
                {
                    objProBindMenu = objLstProBindMenu[0];
                    flg = objProBindMenu.BtnAdd;
                }
                return flg;
            }
            catch (Exception ex) { Login.Profile.ErrorHandling(ex, "UCToolbarHTML", "ConvertToFill"); return flg; }
            finally { objiBindMenuClient.Close(); }
        }

        private void ButtonOffOn(System.Web.UI.HtmlControls.HtmlInputButton btn)
        {
            if (btn.Disabled == true) { btn.Attributes.Add("class", "Off FixWidth"); } else { btn.Attributes.Add("class", "FixWidth"); }
        }

        public void ButtonOffOn(Button btn)
        {
            if (btn.Enabled == false) { btn.CssClass = "Off FixWidth"; } else { btn.CssClass = "FixWidth"; }
        }

        public void PrintReport(string ObjectName, string RefrenceID)
        {
            try
            {


            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "UCToolbarHTML", "PrintReport");
            }
            finally
            {
            }

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
                List<ReportParameter> rptpara = new List<ReportParameter>();//("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath.ToString(), "") + @"UpLoadedFiles/photo/1077.jpeg");
                //rptpara.Add(new ReportParameter("Path", Server.MapPath(@"UpLoadedFiles/photo")));
                //rptpara.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + @"/UpLoadedFiles/photo"));

                //lr.SetParameters(rptpara);
                //lr.ReportPath = Server.MapPath(rptPath_RDLC);
                //lr.DisplayName = dispName;
                //lr.DataSources.Add(CType(rptDatasrc, ReportDataSource))

                for (Int16 I = 0; I <= rptDatasrc.Tables.Count - 1; I++)
                {
                    lr.DataSources.Add(new ReportDataSource(rptDatasrc.Tables[I].TableName, rptDatasrc.Tables[I]));
                }
                lr.Refresh();

                //Dim reportType As String = "Excel"
                // reportType = "Excel"
                // reportType = "PDF"
                // reportType = "HTML"
                string mimeType = null;
                string encoding = null;
                string fileNameExtension = null;
                //The DeviceInfo settings should be changed based on the reportType
                //http://msdn.microsoft.com/en-us/library/ms155397.aspx
                string deviceInfo = "<DeviceInfo>" + " <OutputFormat>PDF</OutputFormat>" + " <PageWidth>8.5in</PageWidth>" + " <PageHeight>11in</PageHeight>" + " <MarginTop>0.5in</MarginTop>" + " <MarginLeft>0.25in</MarginLeft>" + " <MarginRight>0.25in</MarginRight>" + " <MarginBottom>0.5in</MarginBottom>" + "</DeviceInfo>";
                //string deviceInfo = null;
                // = "<DeviceInfo>" + " <OutputFormat>PDF</OutputFormat>" + " <PageWidth>8.5in</PageWidth>" + " <PageHeight>11.5in</PageHeight>" + " <MarginTop>0.50cm</MarginTop>" + " <MarginLeft>1.25cm</MarginLeft>" + " <MarginRight>0.25cm</MarginRight>" + " <MarginBottom>0.50cm</MarginBottom>" + "</DeviceInfo>"
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
                //sender.Response.AddHeader("content-disposition", "attachment; filename=" & lr.DisplayName & "." & fileNameExtension)
                Response.AddHeader("content-disposition", "attachment;open; filename=" + lr.DisplayName + ".pdf");
                Response.BinaryWrite(renderedBytes);
                Response.End();
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

        //private void loadstring()
        //{
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
        //    rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
        //    ci = Thread.CurrentThread.CurrentCulture;

        //    btnAddNew.Value = rm.GetString("AddNew", ci);
        //    btnEdit.Value = rm.GetString("Edit", ci);
        //    btnSave.Value = rm.GetString("Save", ci);
        //    btnClear.Value = rm.GetString("Clear", ci);
        //    btnExport.Value = rm.GetString("Export", ci);
        //    btnImport.Value = rm.GetString("Import", ci);
        //    btnMail.Value = rm.GetString("Mail", ci);
        //    btnPrint.Value = rm.GetString("Print", ci);
        //    btnConvertTo.Value = rm.GetString("ConvertTo", ci);
        //}



    }
}