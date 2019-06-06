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

namespace BrilliantWMS.POR.Reports
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
                if (ds.Tables[0].TableName == "dsPartRequisitions")
                {

                    ReportDataSource rds = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);

                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds);


                    //if (Teststr == "PartRequisition")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/PartRequestList.rdlc";
                    //}
                    //else if (Session["SelObject"].ToString() == "PartIssue")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/PartIssueList.rdlc";
                    //}
                    //else if (Session["SelObject"].ToString() == "PartReceipt")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/PartReceiptList.rdlc";
                    //}
                    //else if (Session["SelObject"].ToString() == "PartConsumption")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/PartConsumptionList.rdlc";
                    //}
                    //else if (Session["SelObject"].ToString() == "PartStock")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/PartStockList.rdlc";
                    //}
                    //else if (Session["SelObject"].ToString() == "SiteWiseConsumption")
                    //{
                    //    RptViewer1.LocalReport.ReportPath = "POR/Reports/SiteWiseConsumption.rdlc";
                    //}

                    RptViewer1.LocalReport.SetParameters(RptParameters());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsSiteConsumption")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);

                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);

                    if (Session["SelObject"].ToString() == "partconsumption")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/SiteWiseConsumption.rdlc";
                    }

                    else if (Session["SelObject"].ToString() == "partrequest")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/PartRequestList.rdlc";
                    }

                    else if (Session["SelObject"].ToString() == "partissue")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/PartIssueList.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "partreceipt")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/PartReceiptList.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "monthly")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/PrReportMonthly.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "weeklylst")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/WeeklyAnalysis.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "consumabletracker")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/ConsumableTracker.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "productdtl")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/ProductBalance.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "transfer")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/TransferRpt.rdlc";
                    }
                    else if (Session["SelObject"].ToString() == "asset")
                    {
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/AssetTransferRpt.rdlc";
                    }


                    if (Session["SelObject"].ToString() == "productdtl")
                    {
                    }
                    else if (Session["SelObject"].ToString() == "partconsumption")
                    {
                        RptViewer1.LocalReport.SetParameters(RptParameters1());
                    }
                    else
                    {
                        RptViewer1.LocalReport.SetParameters(RptParameters());
                    }
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }

                else if (ds.Tables[0].TableName == "dsSKU")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSKUNEWArabic.rdlc";
                        RptViewer1.LocalReport.EnableExternalImages = true;
                        RptViewer1.LocalReport.SetParameters(RptParameters2());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSKU.rdlc";
                        RptViewer1.LocalReport.EnableExternalImages = true;                     
                        RptViewer1.LocalReport.SetParameters(RptParameters2());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                        
                    }
                   
                
                }
                else if (ds.Tables[0].TableName == "dsUser")//c
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptUserNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters3());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptUser.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters3());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                 
                }
                else if (ds.Tables[0].TableName == "dsOrder")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptOrderNEWArabiic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else 
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptOrder.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    

                }
                else if (ds.Tables[0].TableName == "dsfailed")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "POR/Reports/Importfailed.rdlc";
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();

                }

                else if (ds.Tables[0].TableName == "dsOrderLeadTime")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptOrderLeadTimeNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptOrderLeadTime.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();    
                    }
                    
                }

                else if (ds.Tables[0].TableName == "dsOrderDetails")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/OrderDetailsNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/OrderDetails.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters5());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    

                }
                else if (ds.Tables[0].TableName == "dsBOMDetails")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptBOMDetailsNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters6());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {

                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptBOMDetails.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters6());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }
                else if (ds.Tables[0].TableName == "dsSKUTrans")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/SkuTransactionReportNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters7());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/SkuTransactionReport.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters7());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }
                else if (ds.Tables[0].TableName == "dsSKUDetails")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSKUDetailNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters7());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSKUDetail.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters7());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }

                }
                else if (ds.Tables[0].TableName == "dsImageAudit")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptImageAudit.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParametersImg());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptImageAudit.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParametersImg());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }
                else if (ds.Tables[0].TableName == "dsImageAuditFail")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptImageAuditFailNEWARABIC.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParametersImg());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptImageAuditFail.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParametersImg());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }
                else if (ds.Tables[0].TableName == "dsUserTrans")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/UserTransactionReportNEWArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters3());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/UserTransactionReport.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters3());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }
                else if (ds.Tables[0].TableName == "dsOrderDelivery")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/OrderDeliveryReportArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters8());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/OrderDeliveryReport.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters8());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }


                }
                else if (ds.Tables[0].TableName == "dsSla")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSLAArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters9());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/RptSLA.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters9());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }


                }

                else if (ds.Tables[0].TableName == "dsTDVSTR")
                {

                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/TotalDeliveryvsTotalRequestReportNewArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters10());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/TotalDeliveryvsTotalRequestReport.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters10());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }


                }

                else if (ds.Tables[0].TableName == "dsPOR")
                {
                    if (language == "ar-QA")
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/TotalDeliveryvsTotalRequestReportNewArabic.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters11());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                    else
                    {
                        ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                        RptViewer1.LocalReport.DataSources.Clear();
                        RptViewer1.LocalReport.DataSources.Add(rds1);
                        RptViewer1.LocalReport.ReportPath = "POR/Reports/PurchaseOrderReport.rdlc";
                        RptViewer1.LocalReport.SetParameters(RptParameters11());
                        RptViewer1.ShowParameterPrompts = false;
                        RptViewer1.ShowPromptAreaButton = false;
                        RptViewer1.LocalReport.Refresh();
                    }
                }

                /*New PO Reports*/
                else if (ds.Tables[0].TableName == "dspolist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/POLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsgrnlist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/GRNLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsgrndetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/GRNDETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsqclist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/QCLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsqcdetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/QCDETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsputinlist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/PUTINLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsputindetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/PUTINDETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName=="dsorderlist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/SOLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsorderdetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/SODETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName =="dspickuplist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/PICKUPLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dspickupdetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/PICKUPDETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                 else if (ds.Tables[0].TableName=="dsdispatchlist")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/DISPATCHLISTRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
                else if (ds.Tables[0].TableName == "dsdispatchdetail")
                {
                    ReportDataSource rds1 = new ReportDataSource(ds.Tables[0].TableName, ds.Tables[0]);
                    RptViewer1.LocalReport.DataSources.Clear();
                    RptViewer1.LocalReport.DataSources.Add(rds1);
                    RptViewer1.LocalReport.ReportPath = "WMS/WMSReport/DISPATCHDETAILRpt.rdlc";
                    RptViewer1.LocalReport.SetParameters(RptParameterspoList());
                    RptViewer1.ShowParameterPrompts = false;
                    RptViewer1.ShowPromptAreaButton = false;
                    RptViewer1.LocalReport.Refresh();
                }
            }
        }

        private List<ReportParameter> RptParameterspoList()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            return parameters;
        }
        private List<ReportParameter> RptParameters11()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("PurchaseOrderNo", Session["PurchaseOrderNo"].ToString()));
            parameters.Add(new ReportParameter("Vendor", Session["Vendor"].ToString()));
            parameters.Add(new ReportParameter("Status", Session["Status"].ToString()));
            parameters.Add(new ReportParameter("Product", Session["Product"].ToString()));
            parameters.Add(new ReportParameter("FrmDt", Session["FrmDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParameters10()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));          
            parameters.Add(new ReportParameter("FrmDt", Session["FrmDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParameters9()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
         
            parameters.Add(new ReportParameter("DriverName", Session["DriverName"].ToString()));
            parameters.Add(new ReportParameter("DeliveryType", Session["DeliveryType"].ToString()));
            parameters.Add(new ReportParameter("FrmDt", Session["FrmDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            return parameters;
        }
        private List<ReportParameter> RptParameters8()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("DriverName", Session["DriverName"].ToString()));
            parameters.Add(new ReportParameter("PaymentMode", Session["PaymentMode"].ToString()));
            parameters.Add(new ReportParameter("FrmDt", Session["FrmDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParameters7()
        {

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("BOM", Session["BOM"].ToString()));
            parameters.Add(new ReportParameter("Image", Session["Image"].ToString()));
            parameters.Add(new ReportParameter("CompanyLogo", Session["CompanyLogo"].ToString()));
            parameters.Add(new ReportParameter("CompanyAddress", Session["CompanyAddress"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParametersImg()
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("ImgStatus", Session["ImgStatus"].ToString()));
            parameters.Add(new ReportParameter("UsrName", Session["UsrName"].ToString()));
            parameters.Add(new ReportParameter("FrmDt", Session["FrmDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            
            return parameters;
        }

        private List<ReportParameter> RptParameters6()
        {

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            return parameters;
        }
        private List<ReportParameter> RptParameters4()
        {

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("FromDt", Session["FromDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            parameters.Add(new ReportParameter("Status", Session["Status"].ToString()));
            parameters.Add(new ReportParameter("User", Session["User"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParameters2()
        {
            
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
          //  parameters.Add(new ReportParameter("ImagePath", Session["ImagePath"].ToString()));
            parameters.Add(new ReportParameter("BOM", Session["BOM"].ToString()));
            parameters.Add(new ReportParameter("Image", Session["Image"].ToString()));
            //parameters.Add(new ReportParameter("Logo", Session["Logo"].ToString(),true));
            //parameters.Add(new ReportParameter("LogoMimeType", "image/jpg", true));
            parameters.Add(new ReportParameter("CompanyAddress", Session["CompanyAddress"].ToString()));
            return parameters;
        }
        private List<ReportParameter> RptParameters3()
        {

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("RoleName", Session["RoleName"].ToString()));
            parameters.Add(new ReportParameter("Active", Session["Active"].ToString()));
            return parameters;
        }
        private List<ReportParameter> RptParameters5()
        {

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Company", Session["Company"].ToString()));
            parameters.Add(new ReportParameter("Department", Session["Department"].ToString()));
            parameters.Add(new ReportParameter("Status", Session["Status"].ToString()));
            parameters.Add(new ReportParameter("User", Session["User"].ToString()));
            parameters.Add(new ReportParameter("FromDt", Session["FromDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            return parameters;
        }

        private List<ReportParameter> RptParameters()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<ReportParameter> parameters = new List<ReportParameter>();
            mCompany compdetails = new mCompany();
            iCompanySetupClient compclient = new iCompanySetupClient();
            compdetails = compclient.GetCompanyById(profile.Personal.CompanyID, profile.DBConnection._constr);
            parameters.Add(new ReportParameter("CompName", compdetails.Name));
            parameters.Add(new ReportParameter("CompAdd", compdetails.AddressLine1));
            parameters.Add(new ReportParameter("FromDt", Session["FromDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            parameters.Add(new ReportParameter("ReportHeading", Session["ReportHeading"].ToString()));
            // parameters.Add(new ReportParameter("CompLogo","../MasterPage/Logo/" + compdetails.LogoPath));
            return parameters;
        }
        private List<ReportParameter> RptParameters1()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<ReportParameter> parameters = new List<ReportParameter>();
            mCompany compdetails = new mCompany();
            iCompanySetupClient compclient = new iCompanySetupClient();
            compdetails = compclient.GetCompanyById(profile.Personal.CompanyID, profile.DBConnection._constr);
            parameters.Add(new ReportParameter("CompName", compdetails.Name));
            parameters.Add(new ReportParameter("CompAdd", compdetails.AddressLine1));
            parameters.Add(new ReportParameter("FromDt", Session["FromDt"].ToString()));
            parameters.Add(new ReportParameter("ToDt", Session["ToDt"].ToString()));
            parameters.Add(new ReportParameter("ReportHeading", Session["ReportHeading"].ToString()));
            parameters.Add(new ReportParameter("Generator", Session["Generator"].ToString()));
            // parameters.Add(new ReportParameter("CompLogo","../MasterPage/Logo/" + compdetails.LogoPath));
            return parameters;
        }

        protected void RptViewer1_Load(object sender, EventArgs e)
        {
            //string exportOption = "Excel";
            string exportOption = "Word";
            // string exportOption = "PDF";
            RenderingExtension extension = RptViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOption, StringComparison.CurrentCultureIgnoreCase));
            if (extension != null)
            {
                System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                fieldInfo.SetValue(extension, false);
            }


        }

        protected void bntncsv_Click(object sender, EventArgs e)
        {
            string fileName = "";
            DataSet ds = new DataSet();
            if (Session["ReportDS"] != null)
            {
                string Teststr;
                Teststr = Session["SelObject"].ToString();

                ds = (DataSet)Session["ReportDS"];
                DataTable dt = new DataTable();
                if (ds.Tables[0].TableName == "dsSKU")
                {
                    fileName = "SKU_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsSKUDetails")
                {
                    fileName = "SKU_DETAIL_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsUser")
                {
                    fileName = "USER_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsOrder")
                {
                    fileName = "ORDER_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsOrderDetails")
                {
                    fileName = "ORDER_DETAIL_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsOrderLeadTime")
                {
                    fileName = "ORDER_LEAD_TIME_REPORT";
                    dt = ds.Tables[0];
                }


                else if (ds.Tables[0].TableName == "dsBOMDetails")
                {
                    fileName = "BOM_DETAIL_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsSKUTrans")
                {
                    fileName = "SKU_TRANSACTION_REPORT";
                    dt = ds.Tables[0];
                }

                else if (ds.Tables[0].TableName == "dsImageAudit")
                {
                    fileName = "IMAGE_AUDIT_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsImageAuditFail")
                {
                    fileName = "IMAGE_AUDIT_FAIL_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsUserTrans")
                {
                    fileName = "USER_TRANSACTION_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsOrderDelivery")
                {
                    fileName = "ORDER_DELIVERY_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsSla")
                {
                    fileName = "SERVICE_LEVEL_AGGREMENT_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsTDVSTR")
                {
                    fileName = "TOTAL_DELIVERY_VS_TOTAL_REQUEST_REPORT";
                    dt = ds.Tables[0];
                }
                else if (ds.Tables[0].TableName == "dsfailed")
                {
                    fileName = "IMPORT_FAILED";
                    dt = ds.Tables[0];
                }
                string csv = string.Empty;

                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ';';
                }

                //Add new line.
                csv += "\r\n";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ';';
                    }

                    //Add new line.
                    csv += "\r\n";
                }

                //Download the CSV file.
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(csv);
                Response.Flush();
                Response.End();

            }  
        }
    }
}