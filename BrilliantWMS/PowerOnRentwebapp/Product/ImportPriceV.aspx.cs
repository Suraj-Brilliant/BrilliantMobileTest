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

namespace BrilliantWMS.Product
{
    public partial class ImportPriceV : System.Web.UI.Page
    {
        DataTable dt;
        DataSet ds = new DataSet();
        long value = 1;
        long companyID = 0, DeptID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            companyID = long.Parse(Session["CompanyIdPI"].ToString());
            DeptID = long.Parse(Session["DepartmentIDPI"].ToString());
            DisplayGrid();
        }

        public void DisplayGrid()
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ds = productClient.GetPriceImportData(DeptID,profile.DBConnection._constr);
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
            else if(value ==3)
            {
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
                lblbackMessage.Text = "Please Select Correct Company and department for Uploading SKU price";
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
                string ProductCode = e.Row.Cells[6].Text;
                if (e.Row.Cells[5].Text != "")
                {
                    price = decimal.Parse(e.Row.Cells[5].Text);
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[5].ToolTip = "Price Should not be blank or Charecters";
                    value = 2;
                }

                if (ProductCode == "NotAvailable")
                {
                    value = 0;
                    e.Row.BackColor = System.Drawing.Color.DarkCyan;
                    e.Row.ForeColor = System.Drawing.Color.White;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Tomato;
                    e.Row.Cells[1].ToolTip = "SKU not available";
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
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                ds = productClient.GetPriceImportData(DeptID,profile.DBConnection._constr);
                dt = ds.Tables[0];
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    string SkuCode = ds.Tables[0].Rows[i]["SKUCode"].ToString();
                    decimal Price = decimal.Parse(ds.Tables[0].Rows[i]["Price"].ToString());
                    productClient.UpdateImportSkuPrice(SkuCode, Price, DeptID, profile.DBConnection._constr);
                }
                productClient.DeleteSKUPricetemp(profile.DBConnection._constr);
                Response.Redirect("ImportPriceF.aspx");
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
            productClient.DeleteSKUPricetemp(profile.DBConnection._constr);
            productClient.Close();
            Response.Redirect("ImportPriceD.aspx");
        }
    }
}