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

namespace BrilliantWMS.CommonControls
{
    public partial class SKUSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillWarehouse();
            }
            hndgrupByGrid.Value = GridProductSearch.GroupBy;
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
                Login.Profile.ErrorHandling(ex, this, "SKUSearch.aspx.cs", "RebindGrid");
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
               // str = "Select * from V_WMS_SKUSearch where CustomerID = '" + CustomerID + "' and Active = 'Yes'";
                str = "Select * from V_WMS_SKUSearch where WarehouseID = '"+ WarehouseID +"'";
                //str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct where StoreId = '" + StoreId + "'";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "Select * from V_WMS_SKUSearch where WarehouseID = '"+ WarehouseID +"' and (ProductCode like '%" + filter + "%' or Name like '%" + filter + "%')";
                //str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct mp where StoreId = '" + StoreId + "' and (mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%') ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }



      /*  protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                BrilliantWMS.UCProductSearchService.iUCProductSearchClient productSearchService = new BrilliantWMS.UCProductSearchService.iUCProductSearchClient();
                CustomProfile profile = CustomProfile.GetProfile();

                //List<GetProductDetail> ProductList = new List<GetProductDetail>();
                //ProductList = productSearchService.GetProductList1(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, profile.DBConnection._constr).ToList();

                List<VW_GetSKUDetailsWithPack> ProductList = new List<VW_GetSKUDetailsWithPack>();
                long DeptID = long.Parse(Session["DeptID"].ToString());
                //if (profile.Personal.UserType == "Requester And Approver" || profile.Personal.UserType == "Requester" || profile.Personal.UserType == "User" || profile.Personal.UserType == "Admin")
                //{
                long UserID = profile.Personal.UserID;
                ProductList = productSearchService.GetSKUListDeptWise(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, UserID, DeptID, profile.DBConnection._constr).ToList();
                //}
                //else
                //{
                //    ProductList = productSearchService.GetSKUList(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, DeptID, profile.DBConnection._constr).ToList();
                //}                

                GridProductSearch.DataSource = ProductList;
                GridProductSearch.GroupBy = hndgrupByGrid.Value;
                // if (!Page.IsPostBack) { GridProductSearch.GroupBy = "ProductType"; }
                GridProductSearch.DataBind();
                productSearchService.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
            }
        }*/



    }
}