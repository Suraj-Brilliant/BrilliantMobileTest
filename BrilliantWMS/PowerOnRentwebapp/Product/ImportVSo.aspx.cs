using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.ToolbarService;
using System.Web.Services;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.ProductMasterService;


namespace BrilliantWMS.POR
{
    public partial class ImportVSo : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        DataTable dt;
        DataSet ds = new DataSet();
        string value = "";
        protected void Page_Load(object sender, EventArgs e)
        {
               if (Session["Lang"] == "")
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();
                long DeptID = long.Parse(Session["DepartmentID"].ToString());
    
           // UCFormHeader1.FormHeaderText = "Import Images";
            GetImportStatus();
            //DisplayGrid();
        }

        public void GetImportStatus()
        {
            lbltotalimagetxt.Text = Session["TotalImages"].ToString();
            lblsuccessfultxt.Text = Session["SuccessImages"].ToString();
            lblfailedtxt.Text = Session["FailedImages"].ToString();
        }



        public void DisplayGrid()
        {
           // iSalesOrderClient Sales = new iSalesOrderClient();
            try
            {
                //ds = Sales.GetNotAvailable();
                //dt = ds.Tables[0];
                //if (dt.Rows.Count > 0)
                //{
                //    grdImportView.DataSource = ds.Tables[0];
                //    grdImportView.DataBind();
                //}
                //if (value != "")
                //{
                //    lblbackMessage.Text = "Colored row Products or Customer are not in the system.Please click on Back Button ";
                //    btnforword.Visible = false;
                //}
                //else
                //{
                //    lblOkMessage.Text = "All data are verified.Please click on Next Button ";
                //}
            }
            catch { }
            finally { 
               // Sales.Close(); 
            }
                       
              
        }

        protected void grdImportView_RowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            string prodcode = e.Row.Cells[7].Text;
            string Vendorname = e.Row.Cells[8].Text;
            string SOavailable = e.Row.Cells[9].Text;
            try
            {

                //if (prodcode == "NotAvailable" || Vendorname == "NotAvailable" || SOavailable == "Available")
                //{
                //    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                //    e.Row.ForeColor = System.Drawing.Color.White;
                //    value = "NotAvailable";
                //    if (prodcode == "NotAvailable")
                //    {
                //        e.Row.Cells[3].ForeColor = System.Drawing.Color.White;
                //        e.Row.Cells[3].ToolTip = "Product not available";
                //    }
                //    if (Vendorname == "NotAvailable")
                //    {
                //        e.Row.Cells[4].ForeColor = System.Drawing.Color.White;
                //        e.Row.Cells[4].ToolTip = "Customer Not Available";
                //    }
                //    if (SOavailable == "Available")
                //    {
                //        e.Row.Cells[1].ForeColor = System.Drawing.Color.White;
                //        e.Row.Cells[1].ToolTip = "SoNumber Allready Available";
                //    }
                //}
            }
            catch { }
            finally { }


        }


        protected void btnUploadPo_Click(object sender, EventArgs e)
        {
           // iSalesOrderClient Sales = new iSalesOrderClient();
            try
            {
                //Sales.DeleteImportSO();
                Server.Transfer("ImportDSo.aspx");
                //Response.Redirect("../ImportDSo.aspx");
            }
            catch { }
            finally { 
                //Sales.Close();
            }
        }

        protected void btnforword_Click(object sender, EventArgs e)
        {
            string[] selectedProducts = new string[] { "1" };
            char[] splitchar = { ',' };
          //  iSalesOrderClient Sales = new iSalesOrderClient();
            try
            {
                //string createdby = "1";
                //DateTime Creationdate = Convert.ToDateTime(DateTime.Now.Date.ToString());
                //Sales.InsertBulkSOHead(createdby, Creationdate);

                //ds = Sales.GetdistinctSO();
                //dt = ds.Tables[0];
                //object O = dt.Rows[0]["SoNumber"];

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    O = dt.Rows[i]["SoNumber"];
                //    string Prodvalue = Convert.ToString(O);
                //    string PrroductSOhead = Sales.GetSOHeadNumber(Prodvalue);
                //    Sales.UpdateTotalpriceSOHead(Prodvalue, PrroductSOhead);
                //}


                ////string SoProd = Sales.GetdistinctSO();

                ////selectedProducts = SoProd.Split(splitchar);
                ////for (int i = 0; i < selectedProducts.Length; i++)
                ////{
                ////    string ProdCode = selectedProducts[i].ToString();
                ////    Sales.UpdateTotalpriceSOHead(ProdCode);
                ////}
                //Sales.InsertBulkSODetail(createdby, Creationdate);
                //Sales.DeleteImportSO();
                Server.Transfer("ImportFSo.aspx");
            }
            catch { }
            finally {
               // Sales.Close();
            }

            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Working.');", true);  
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblHeading.Text = rm.GetString("ImportImages", ci);
           UCFormHeader1.FormHeaderText = rm.GetString("ImportImages", ci);
            //btnUploadPo.Text = rm.GetString("Upload", ci);
            lblstep1.Text = rm.GetString("UploadFile", ci);
            lblstep2.Text = rm.GetString("validaton", ci);
            btnfinish.Value = rm.GetString("Finished", ci);
            lbltotalnumber.Text = rm.GetString("TotalImages", ci);
            lblsuccessful.Text = rm.GetString("successimages", ci);
            lblfailedimages.Text = rm.GetString("failimages", ci);
        }

        [WebMethod]
        public static int WmGetCouponReport(string Coupon)
        {
            int result = 0;
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string ReportPath = string.Empty;
            string DisplayName = string.Empty;
            DataSet dsfailed = new DataSet();

           // ReportPath = "AdministratorPortal/Report/CouponRpt.rdlc";
           // DisplayName = "Coupon Report";

            dsfailed = productClient.GetFailedImageDetail(profile.DBConnection._constr);
            if (dsfailed.Tables.Count > 0)
            {
                dsfailed.Tables[0].TableName = "dsfailed";
            }
            HttpContext.Current.Session["ReportDS"] = dsfailed;
            HttpContext.Current.Session["SelObject"] = "imagefailed";

            result = Convert.ToInt16(dsfailed.Tables[0].Rows.Count);
            return result;
        }
    }
}