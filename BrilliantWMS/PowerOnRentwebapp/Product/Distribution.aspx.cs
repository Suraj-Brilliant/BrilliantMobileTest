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
using System.Web.Services;
using BrilliantWMS.ProductMasterService;

namespace BrilliantWMS.Product
{
    public partial class Distribution : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long StoreId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
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
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
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
                str = "select cp.ID,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "select cp.ID,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID where cp.Name like '%" + filter + "%' or c.Name like '%" + filter + "%' or t.Territory like '%" + filter + "%'";
               // str = "select ProductCode,OMSSKUCode,Name,Description,from mProduct mp where StoreId = '" + StoreId + "' and mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%' ";
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

            lblheader.Text = rm.GetString("ContactList", ci);
            //lblwithbom.Text = rm.GetString("WithBOM", ci);
            btnSubmitProductSearch1.Value = rm.GetString("Submit", ci);
            btnSubmitProductSearch2.Value = rm.GetString("Submit", ci);
        }

        [WebMethod]
        public static void PMSaveContactD(string selectedIds)
        {
            iProductMasterClient productClient = new iProductMasterClient();
             CustomProfile profile = CustomProfile.GetProfile();
            string ids = selectedIds.ToString();
            string[] words = ids.Split(',');
            for (int i = 1; i < words.Length; i++)
            {
                long ContactId = long.Parse(words[i]); 
                long templateid = 0;
                productClient.InsertIntoDistribution(templateid, ContactId, profile.DBConnection._constr);
            }
            //string selectedIds = rec["SelectedIds"].ToString();

        }
    }
}