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
using BrilliantWMS.WarehouseService;
using BrilliantWMS.CycleCountService;
using System.Web.Services;

namespace BrilliantWMS.Warehouse
{
    public partial class WSKUSearch : System.Web.UI.Page
    {
        static string sessionID;
        long WarehouseID = 0;
        long LocationID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["WarehouseID"] != null) WarehouseID = long.Parse(Request.QueryString["WarehouseID"].ToString());
            if (Request.QueryString["LocationID"] != null) LocationID = long.Parse(Request.QueryString["LocationID"].ToString());
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
           // long WarehouseID = long.Parse(hdnwarehouseID.Value);
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {

                str = "select * from V_WMS_GetProductByLocID where WarehouseID = '" + WarehouseID + "' and LocationId = '" + LocationID + "'";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "select * from V_WMS_GetProductByLocID where WarehouseID = '" + WarehouseID + "' and LocationId = '" + LocationID + "' and (ProductCode like '%" + filter + "%' or Name like '%" + filter + "%')";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }
    }
}