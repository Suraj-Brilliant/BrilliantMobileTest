using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using System.Web.Services;
using System.Configuration;
using System.IO;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Drawing;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartRequest;

namespace BrilliantWMS.PowerOnRent
{
    public partial class DirectImportV : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        DataTable dt;
        DataSet ds = new DataSet();
        long value = 1;
        long companyID = 0, DeptID = 0,locationId=0, paymentId = 0;
        DateTime Expecteddate;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            //companyID = long.Parse(Session["CompanyIdPI"].ToString());
            //DeptID = long.Parse(Session["DepartmentIDPI"].ToString());
            //locationId = long.Parse(Session["LocationID"].ToString());
            //paymentId = long.Parse(Session["PaymentID"].ToString());
            //Expecteddate = Convert.ToDateTime(Session["ExpDate"].ToString());
            DisplayGrid();
        }

        public void DisplayGrid()
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ds = productClient.GetDirectOrderData(profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                GVImportView.DataSource = ds.Tables[0];
                GVImportView.DataBind();
            }
            else
            {
                value = 3;
            }
            if (value == 0)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Colored Row SKU not Available in OMS.Please click on Back Button";
            }
            else if (value == 2)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Characters and blank values not asccepted in Price columns";
            }
            else if (value == 3)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Please Select Correct Company and department for Uploading SKU price";
            }
            else if (value == 4)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Current Stock is Less than requested Qty";
            }
            else if (value == 5)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Requested Qty is not according to MOQ";
            }
            else if(value == 6)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Quantity Should not be blank,Zero or Charecters";
            }
            else if (value == 7)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "SKU not available in Given Department";
            }
            else if (value == 8)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Department Not Available";
            }
            else if (value == 9)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Location Not Available";
            }
            else
            {
                lblOkMessage.Text = "All data are verified.Please click on Next Button ";
                btnnext.Enabled = true;
                btnnext.CssClass = "class2";
            }
        }

        protected void GVImportView_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            try
            {
                decimal price = 0m;
                long CurrentStock = 0;
                string ProductCode = e.Row.Cells[12].Text;
                string ProdDept = e.Row.Cells[13].Text;
                string Department = e.Row.Cells[14].Text;
                string Location = e.Row.Cells[15].Text;
                
               
                long MOQchk = long.Parse(e.Row.Cells[17].Text);
                if (e.Row.Cells[10].Text != "")
                {
                    price = decimal.Parse(e.Row.Cells[10].Text);
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[10].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[10].ToolTip = "Price Should not be blank or Charecters";
                    value = 2;
                }

                if (e.Row.Cells[7].Text != "" && double.Parse(e.Row.Cells[7].Text) != 0.00)
                {
                    CurrentStock = long.Parse(e.Row.Cells[16].Text);
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[7].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[7].ToolTip = "Quantity Should not be blank, Zero or Charecters";
                    value = 6;
                }

                if (ProductCode == "NotAvailable")
                {
                    value = 0;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[1].ToolTip = "SKU not available";
                }
                if (long.Parse(e.Row.Cells[16].Text) == 0)
                {
                    value = 4;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[5].ToolTip = "Current Stock is Less than requested Qty";
                }
                if (MOQchk == 1)
                {
                    value = 5;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[4].ToolTip = "Requested Qty is not according to MOQ";
                    e.Row.Cells[7].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[7].ToolTip = "Requested Qty is not according to MOQ";
                }
                if (ProdDept == "NotInDepartment")
                {
                    value = 7;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[1].ToolTip = "SKU not available in Given Department";
                }
                if (Department == "NotAvailable")
                {
                    value = 8;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[18].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[18].ToolTip = "Department Not Available";
                }
                if (Location == "NotAvailable")
                {
                    value = 9;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[19].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[19].ToolTip = "Location Not Available";
                }
                 

            }
            catch (Exception ex)
            {
                value = 2;
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Only Numbers are allowed in Price columns');", true);

            }

        }





        protected void btnnext_Click(object sender, EventArgs e)
        {
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable(); 
            iProductMasterClient productClient = new iProductMasterClient();
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            decimal TotalProdQty = 0m, totalPrice = 0m;
            string Title = "Direct Order Import";
            string OrderNumber = "";
            DateTime orderdate = DateTime.Now;
            try
            {
                ds = productClient.GetDistinctPLCodes(profile.DBConnection._constr);
                //ds = productClient.GetDirectOrderData(profile.DBConnection._constr);
                dt = ds.Tables[0];
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    DataSet ds2 = new DataSet();
                    DataTable dt2 = new DataTable(); 
                    long disDeptid = long.Parse(ds.Tables[0].Rows[i]["deptId"].ToString());
                    long DisLocId = long.Parse(ds.Tables[0].Rows[i]["LocID"].ToString());
                    long CompanyID = long.Parse(ds.Tables[0].Rows[i]["ParentID"].ToString());
                    long Orderheadid = 0;
                    ds2 = productClient.GetTotalForOrderHead(disDeptid, DisLocId, profile.DBConnection._constr);
                    dt2 = ds2.Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        TotalProdQty = decimal.Parse(dt2.Rows[0]["TotalOrderQty"].ToString());
                        totalPrice = decimal.Parse(dt2.Rows[0]["GrandTotalPrice"].ToString());
                    }
                    OrderNumber = productClient.getOrderFormatNumber(disDeptid,profile.DBConnection._constr);
                    ds1 = productClient.GetImportDatabyDisDeptLocId(disDeptid, DisLocId, profile.DBConnection._constr);
                    dt1 = ds1.Tables[0];
                    long sequence = 0;
                    for (int j = 0; j <= ds1.Tables[0].Rows.Count - 1; j++)
                    {
                        long UOMID = 16;
                     
                        string skucode = ds1.Tables[0].Rows[j]["SKUCode"].ToString();
                        string SkuName = ds1.Tables[0].Rows[j]["Name"].ToString();
                        string Description = ds1.Tables[0].Rows[j]["Description"].ToString();
                        decimal AvailableBalance = decimal.Parse(ds1.Tables[0].Rows[j]["AvailableBalance"].ToString());
                        decimal RequestQty = decimal.Parse(ds1.Tables[0].Rows[j]["asRequestQty"].ToString());
                        decimal OrderQty = decimal.Parse(ds1.Tables[0].Rows[j]["OrderQty"].ToString());
                        decimal Price = decimal.Parse(ds1.Tables[0].Rows[j]["Price"].ToString());
                        decimal Total = decimal.Parse(ds1.Tables[0].Rows[j]["Total"].ToString());
                        long locationid = long.Parse(ds1.Tables[0].Rows[j]["locationid"].ToString());
                        long prodID = long.Parse(ds1.Tables[0].Rows[j]["prodID"].ToString());
                        long storeID = long.Parse(ds1.Tables[0].Rows[j]["ID"].ToString());
                        decimal Dispatch = decimal.Parse(ds1.Tables[0].Rows[j]["TotalDispatchQty"].ToString());
                        //TotalProdQty = TotalProdQty + OrderQty;
                        //totalPrice = totalPrice + Total;
                        long maxdeldays = productClient.GetmaxDeliverydays(storeID, profile.DBConnection._constr);
                        DateTime Deliveryday = DateTime.Now.AddDays(maxdeldays);
                        decimal CurrentAvailBalance = AvailableBalance - OrderQty;
                        decimal CurrentDispatchQty = Dispatch + OrderQty;
                        
                        sequence = sequence + 1;
                        if (j == 0)
                        {
                            Orderheadid = productClient.SaveOrderHeaderImport(storeID, orderdate, Deliveryday, 0, 3, profile.Personal.UserID, DateTime.Now, Title, DateTime.Now, TotalProdQty, totalPrice, OrderNumber,locationid, profile.DBConnection._constr);
                        }
                        productClient.SaveOrderDetailImport(Orderheadid, prodID, OrderQty, UOMID, sequence, SkuName, Description, skucode, Price, Total, profile.DBConnection._constr);
                        productClient.updateproductstockdetailimport(storeID, prodID, CurrentAvailBalance, CurrentDispatchQty, profile.DBConnection._constr);
                    }
                    productClient.ImportMsgTransHeader(Orderheadid, profile.DBConnection._constr);
                    int x= objService.EmailSendWhenRequestSubmit(Orderheadid, profile.DBConnection._constr);

                    DataSet dss = new DataSet();
                    dss = objService.GetApproverDepartmentWise(disDeptid, profile.DBConnection._constr);
                    for (int t = 0; t <= dss.Tables[0].Rows.Count - 1; t++)
                    {
                        long approverid = Convert.ToInt64(dss.Tables[0].Rows[t]["UserId"].ToString());

                        objService.EmailSendofApproved(approverid, Orderheadid, profile.DBConnection._constr);
                    }
                }

                productClient.DeleteOrderImport(profile.DBConnection._constr);
                Response.Redirect("ImportOrderF.aspx");
            }
            catch
            {
                productClient.DeleteSKUPricetemp(profile.DBConnection._constr);
            }
            finally
            {
                productClient.Close();
            }
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            productClient.DeleteOrderImport(profile.DBConnection._constr);
            productClient.Close();
            Response.Redirect("DirectImportD.aspx");
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblHeading.Text = rm.GetString("ImportDirectOrder", ci);
            lblstep1.Text = rm.GetString("UploadFile", ci);
            lblstep2.Text = rm.GetString("DataValidationVerification", ci);
            lblstep3.Text = rm.GetString("Finished", ci);

            lbladdresslist.Text = rm.GetString("SKUList", ci);
            btnnext.Text = rm.GetString("Next", ci);

            btnback.Text = rm.GetString("back", ci);
        }

    }
}