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


namespace BrilliantWMS.Warehouse
{
    public partial class LocationBySKU : System.Web.UI.Page
    {
        long ProductID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductID"] != null) ProductID = long.Parse(Request.QueryString["ProductID"].ToString());
           
            RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst(GridProductSearch.CurrentPageIndex, hdnFilterText.Value);
                GridProductSearch.DataSource = ds;
                GridProductSearch.GroupBy = hndgrupByGrid.Value;
                GridProductSearch.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SKUSearch.aspx.cs", "RebindGrid");
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
                str = "select * from V_WMS_getLocbyProdID where ProdID = '" + ProductID + "' and Active = 'Yes'";
               SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {

                str = "Select * from V_WMS_getLocbyProdID where ProdID = '" + ProductID + "' and Active = 'Yes'and (locationCode like '%" + filter + "%')";
               
                //str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct mp where StoreId = '" + StoreId + "' and (mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%') ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }
    }
}