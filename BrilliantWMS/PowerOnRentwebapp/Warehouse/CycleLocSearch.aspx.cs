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
    public partial class CycleLocSearch : System.Web.UI.Page
    {
        static string sessionID;
        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            hdnSessionID.Value = sessionID;
            if (Request.QueryString["Object"] != null) hdnobject.Value = Request.QueryString["Object"].ToString();
            if (Request.QueryString["WarehouseID"] != null) hdnwarehouseID.Value = Request.QueryString["WarehouseID"].ToString();
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
            long WarehouseID = long.Parse(hdnwarehouseID.Value);
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                if (WarehouseID != 0)
                {
                    str = "Select * from V_WMS_WarehouseLocation where WarehouseID = '" + WarehouseID + "' and Active = 'Yes'";
                }
                else
                {
                    //str = "Select * from V_WMS_WarehouseLocation where CustomerID = '" + CustomerID + "' and Active = 'Yes'";
                }

                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                if (WarehouseID != 0)
                {
                    str = "Select * from V_WMS_WarehouseLocation where WarehouseID = '" + WarehouseID + "' and Active = 'Yes'and (locationCode like '%" + filter + "%' or WarehouseName like '%" + filter + "%')";
                }
                else
                {
                   // str = "Select * from V_WMS_WarehouseLocation where CustomerID = '" + CustomerID + "' and Active = 'Yes' and (locationCode like '%" + filter + "%' or WarehouseName like '%" + filter + "%')";
                }
                //str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct mp where StoreId = '" + StoreId + "' and (mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%') ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        [WebMethod]
        public static void SaveCycleProductIds(string selectedIds, string Object, string SessionID)
        {
            iCycleCountClient CycleClient = new iCycleCountClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                CycleCountTemp cycletemp = new CycleCountTemp();
                cycletemp.Object = Object.ToString();
                cycletemp.SessionID = SessionID.ToString();
                cycletemp.CreatedBy = profile.Personal.UserID;
                cycletemp.CreationDate = DateTime.Now;
                string ids = selectedIds.ToString();
                string[] words = ids.Split(',');
                for (int i = 0; i < words.Length; i++)
                {
                    // long ProductID = long.Parse(words[i]);
                    cycletemp.ReferenceID = long.Parse(words[i]);
                    CycleClient.SaveCycleCounttemp(cycletemp, profile.DBConnection._constr);
                }
            }
            catch (Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, this, "CycleSkuSearch", "SaveCycleProductIds");
            }
            finally
            {
                CycleClient.Close();

            }

        }
    }
}