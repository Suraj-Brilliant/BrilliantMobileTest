using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UCProductSearchService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebMsgBox;

namespace BrilliantWMS.Product
{
    public partial class SearchProduct : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long StoreId = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Request.QueryString["deptid"] != null)
                {
                    StoreId = long.Parse(Request.QueryString["deptid"].ToString());
                    hndgrupByGrid.Value = GridProductSearch.GroupBy;
                    RebindGrid(sender, e);
                    if (Session["Lang"] == "")
                    {
                        Session["Lang"] = Request.UserLanguages[0];
                    }
                    loadstring();
                }
                else
                {
                    WebMsgBox.MsgBox.Show("");
                }
               
               
               
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SearchProduct.aspx.cs", "Page_Load");
            }
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                //UCProductSearchService.iUCProductSearchClient productSearchService = new UCProductSearchService.iUCProductSearchClient();
                //CustomProfile profile = CustomProfile.GetProfile();
                ////List<GetProductDetail> ProductList = new List<GetProductDetail>();
                ////ProductList = productSearchService.GetProductList1(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, profile.DBConnection._constr).ToList();

                //List<VW_GetSKUDetailsWithPack> ProductList = new List<VW_GetSKUDetailsWithPack>();
                //ProductList = productSearchService.GetSKUList(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, profile.DBConnection._constr).ToList();

                //GridProductSearch.DataSource = ProductList;
                //GridProductSearch.GroupBy = hndgrupByGrid.Value;
                //// if (!Page.IsPostBack) { GridProductSearch.GroupBy = "ProductType"; }
                //GridProductSearch.DataBind();
                //productSearchService.Close();
                DataSet ds = new DataSet();
                ds = GetPrdLst(GridProductSearch.CurrentPageIndex, hdnFilterText.Value);

                GridProductSearch.DataSource = ds;
                GridProductSearch.GroupBy = hndgrupByGrid.Value;
               // if (!Page.IsPostBack) { GridProductSearch.GroupBy = "ProductCode"; }
                GridProductSearch.DataBind();



            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SearchProduct.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst(int pageIndex, string filter)
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct where StoreId = '"+ StoreId +"'";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct mp where StoreId = '" + StoreId + "' and (mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%') ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblheader.Text = rm.GetString("SKUList", ci);
            //lblwithbom.Text = rm.GetString("WithBOM", ci);
            btnSubmitProductSearch1.Value = rm.GetString("Submit", ci);
            btnSubmitProductSearch2.Value = rm.GetString("Submit", ci);
        }
    }
}