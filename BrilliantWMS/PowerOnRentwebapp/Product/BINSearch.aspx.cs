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
using BrilliantWMS.WarehouseService;

namespace BrilliantWMS.Product
{
    public partial class BINSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillWarehouse();
            }
            RebindGrid(sender, e);
        }



        protected void FillWarehouse()
        {
            ddlwarehouse.Items.Clear();
            iWarehouseClient Warehouse = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long UserID = profile.Personal.UserID;
            ddlwarehouse.DataSource = Warehouse.GetWarehousebyUserID(UserID, profile.DBConnection._constr);
            ddlwarehouse.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlwarehouse.Items.Insert(0, lst);
            Warehouse.Close();
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
                Login.Profile.ErrorHandling(ex, this, "BINSearch.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst(int pageIndex, string filter)
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            long WarehouseID = long.Parse(hdnwarehouseID.Value);
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                str = "Select * from V_WMS_SKUSearch where WarehouseID = '" + WarehouseID + "'";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "Select * from V_WMS_SKUSearch where WarehouseID = '" + WarehouseID + "' and (LocationCode like '%" + filter + "%' or LocAliasCode like '%" + filter + "%')";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }
    }
}