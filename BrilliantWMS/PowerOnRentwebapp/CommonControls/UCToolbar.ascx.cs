using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.BindMenuService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.CommonControls
{
    public partial class UCToolbar : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        public Page ParentPage { get; set; }
        CustomProfile profile = CustomProfile.GetProfile();
        iUCToolbarClient ObjToolbarService = new iUCToolbarClient();
        PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();
        public delegate void OprClickhandler(Object sender, iUCToolbarClient e);
        BrilliantWMS.BindMenuService.iBindMenuClient objiBindMenuClient = new BrilliantWMS.BindMenuService.iBindMenuClient();
        ProBindMenu objProBindMenu = new ProBindMenu();

        public event OprClickhandler evClickAddNew;
        public event OprClickhandler evClickSave;
        public event OprClickhandler evClickClear;
       // public event OprClickhandler evClickPrint;
        public event OprClickhandler evClickSearch;
       // public event OprClickhandler evClickImport;
       // public event OprClickhandler evClickConvertTo;
        public long RefID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
        }


        public void ToolbarAccess(string ObjectName, String eventID)
        {

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
                btnAddNew.Enabled = objProBindMenu.BtnAdd;
                btnEdit.Disabled = !(objProBindMenu.BtnEdit);
                //btnExport.Enabled = objProBindMenu.BtnExport;
                //btnImport.Enabled = objProBindMenu.BtnImport;
                //btmMail.Enabled = objProBindMenu.BtnMail;
                //btnPrint.Enabled = objProBindMenu.BtnPrint;
                btnSave.Enabled = objProBindMenu.BtnSave;
                btnClear.Enabled = objProBindMenu.BtnClear;
                ButtonOffOn(btnAddNew); ButtonOffOn(btnEdit); 
                //ButtonOffOn(btnExport); 
                //ButtonOffOn(btnImport);
                //ButtonOffOn(btmMail); ButtonOffOn(btnPrint); 
                ButtonOffOn(btnSave); ButtonOffOn(btnClear);
               // tdConvertTo.Visible = false;
               // FlybtnConvertTo.AttachTo = "";
                if (objProBindMenu.ObjectCode == "Lead" || objProBindMenu.ObjectCode == "Opportunity" || objProBindMenu.ObjectCode == "Quotation" || objProBindMenu.ObjectCode == "SalesOrder" || objProBindMenu.ObjectCode == "AddNew")
                {
                    //FlybtnConvertTo.AttachTo = "tdConvertTo";
                   // tdConvertTo.Visible = true;
                    btnSave.Visible = false;
                    btnEdit.Visible = false;
                }
            }

        }

        public ProBindMenu ToolbarAccess1(string ObjectName, String eventID)
        {

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


        public Boolean ConvertToFill(String ObjectName)
        {
            Boolean flg = false;

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

        private void ButtonOffOn(System.Web.UI.HtmlControls.HtmlInputButton btn)
        {
            if (btn.Disabled == true) { btn.Attributes.Add("class", "Off FixWidth"); } else { btn.Attributes.Add("class", "FixWidth"); }
        }

        public void ButtonOffOn(Button btn)
        {
            if (btn.Enabled == false) { btn.CssClass = "Off FixWidth"; } else { btn.CssClass = "FixWidth"; }
        }

        protected void ImgbtnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (evClickAddNew != null)
                {
                    evClickAddNew(this, ObjToolbarService);
                }
                ObjToolbarService = null;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ImgbtnAddNew_Click");
            }
            finally
            {
            }
        }

        public void SetImgbtnAddNewRight(bool val, string msg, string RedirectTo = "#")
        {
            btnAddNew.Attributes.Add("class", "Off buttonON");
            btnAddNew.Attributes.Add("onclick", "showAlert('" + msg + "','orange','" + RedirectTo + "')");

            if (val == true)
            {
                btnAddNew.Attributes.Add("class", "buttonON");
                btnAddNew.Attributes.Add("onclick", "jsAddNew()");
            }
        }

        protected void ImgbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (evClickSave != null)
                {
                    evClickSave(this, ObjToolbarService);
                }
                ObjToolbarService = null;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ImgbtnSave_Click");
            }
            finally
            {
            }
        }

        protected void ImgbtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (evClickClear != null)
                {
                    evClickClear(this, ObjToolbarService);
                }
                ObjToolbarService = null;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ImgbtnClear_Click");
            }
            finally
            {
            }
        }

        protected void ImgbtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (evClickSearch != null)
                {
                    evClickSearch(this, ObjToolbarService);
                }
                ObjToolbarService = null;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ImgbtnSearch_Click");
            }
            finally
            {
            }
        }

        protected void ImgbtnImport_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (evClickImport != null)
            //    {
            //        evClickImport(this, ObjToolbarService);
            //    }
            //    ObjToolbarService = null;
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ImgbtnImport_Click");
            //}
            //finally
            //{
            //}
        }

        public void ResetRefID(string objname, long id)
        {
            try
            {
                //hdnObjectName.Value = objname;
                //hdnRefID.Value = id.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "ResetRefID");
            }
            finally
            {
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //if (hdnRefID.Value != "0")
                //{
                //    if (ddlConvertTo.SelectedItem.Text == "Opportunity") { Response.Redirect("../Opportunity/OpportunityAddEdit.aspx?" + hdnObjectName.Value + "ID=" + hdnRefID.Value); }
                //    if (ddlConvertTo.SelectedItem.Text == "Quotation") { Response.Redirect("../Quotation/QuotationAddEdit.aspx?" + hdnObjectName.Value + "ID=" + hdnRefID.Value); }
                //    if (ddlConvertTo.SelectedItem.Text == "Sales Order") { Response.Redirect("../SalesOrder/SalesOrderAddEdit.aspx?" + hdnObjectName.Value + "ID=" + hdnRefID.Value); }
                //    if (ddlConvertTo.SelectedItem.Text == "Invoice") { Response.Redirect("../Invoice/InvoiceAddEdit.aspx?" + hdnObjectName.Value + "ID=" + hdnRefID.Value); }
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "btnOK_Click");
            }
            finally
            {
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (evClickPrint != null)
            //    {
            //        evClickPrint(this, ObjToolbarService);
            //    }
            //    ObjToolbarService = null;
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "btnPrint_Click");
            //}
            //finally
            //{
            //}
        }

        public void PrintReport(string ObjectName, string RefrenceID)
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UCToolbar", "PrintReport");
            }
            finally
            {
               // invoiceReportService.Close();
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

        internal void ToolbarAccess(string p)
        {

            if (p == "AddNew")
            {
                btnSave.Visible = true;
                btnEdit.Visible = false;
            }
           else if (p == "Edit")
            {
                btnSave.Visible = true;
                btnEdit.Visible = true;
            }
            else if (p == "Product")
            {
                btnSave.Visible = false;
                btnEdit.Visible = false;
            }

        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            btnAddNew.Text = rm.GetString("AddNew", ci);
            btnEdit.Value = rm.GetString("Edit", ci);
            btnSave.Text = rm.GetString("Save", ci);
            btnClear.Text = rm.GetString("Clear", ci);
            //btnExport.Text = rm.GetString("Export", ci);
            //btnImport.Text = rm.GetString("Import", ci);
            //btmMail.Text = rm.GetString("Mail", ci);
            //btnPrint.Text = rm.GetString("Print", ci);
            //btnConvertTo.Value = rm.GetString("ConvertTo", ci);
        }
    }
}