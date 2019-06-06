using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for save_product_quantityPutIn
    /// </summary>
    public class save_product_quantityPutIn : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        
        long PTID = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0,orderID=0, UserID = 0, CustomerID = 0, CompanyID = 0, ProductID = 0, LocID = 0;
        decimal Qty = 0;
      
        string objname = "", page = "", finalzone = "", Batch = "";

        public void ProcessRequest(HttpContext context)
        {
          
            SqlConnection conn = new SqlConnection(strcon);

            if (context.Request.QueryString["objname"] != null)
            {
                objname = context.Request.QueryString["objname"].ToString();
            }
            if (context.Request.QueryString["wrid"] != null)
            {
                WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            }
            if (context.Request.QueryString["oid"] != null)
            {
                orderID = long.Parse(context.Request.QueryString["oid"]);
            }
            if (context.Request.QueryString["pid"] != null)
            {
                ProductID = Convert.ToInt64(context.Request.QueryString["pid"]);
            }
            if (context.Request.QueryString["locid"] != null)
            {
                LocID = Convert.ToInt64(context.Request.QueryString["locid"]);
            }
            if (context.Request.QueryString["batch"] != null)
            {
                Batch = (context.Request.QueryString["batch"]).ToString();
            }
            if (context.Request.QueryString["uid"] != null)
            {
                UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
            }
            if (context.Request.QueryString["qty"] != null)
            {
                Qty = Convert.ToDecimal(context.Request.QueryString["qty"]);
            }
            if (context.Request.QueryString["page"] != null)
            {
                page = (context.Request.QueryString["page"]).ToString();
            }

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n";   /*json Loop Start*/
            jsonString = jsonString + "\"result\":[{\n";

            PTID = GetPTINStatus(orderID, objname, UserID);
            if (PTID > 0)
            {
                decimal orderqty = ProdOrderQty(orderID, ProductID, page,objname);
                decimal putinqty = ProdPutInQty(PTID, ProductID,LocID);
                decimal RemQty = orderqty - putinqty;
                if (Qty - putinqty > RemQty)
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"Quantity Greater than Order Quantity\"\n";
                }
                else
                {
                    int result = UpdatePutInQty(PTID,ProductID,Qty,LocID,objname);
                    if (result > 0)
                    {
                        jsonString = jsonString + "\"status\": \"success\",\n";
                        jsonString = jsonString + "\"reason\": \"\"\n";
                    }
                    else
                    {
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"Failed to Update Quantity\"\n";
                    }
                }
            }

            jsonString = jsonString + "}]\n";
            jsonString = jsonString + "}\n"; /*json Loop End*/
            context.Response.Write(jsonString);
        }

        public int UpdatePutInQty(long grnid, long prodid, decimal qty,long locid, string obj)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdatePutInQty";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "SaveGRNDetail"); }
            finally
            { }
            return result;
        }
        public decimal ProdPutInQty(long ptid, long prdid,long locid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            decimal orderqty = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_ProdPutInQty";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@ptid", ptid);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@locid", locid);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    orderqty = Convert.ToDecimal(dt1.Rows[0]["PutInQty"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "save_product_quantityPutIn", "ProdPutInQty"); }
            finally
            { }
            return orderqty;
        }
        public decimal ProdOrderQty(long oid, long prdid, string page,string obj)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            decimal orderqty = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_ProdOrderQty";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@page", page);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    orderqty = Convert.ToDecimal(dt1.Rows[0]["OrderQty"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "save_product_quantity", "ProdOrderQty"); }
            finally
            { }
            return orderqty;
        }

        public Boolean executeqty(string sp_name)
        {
            Boolean isSucess = false;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_name;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@obj", objname);
                cmd.Parameters.AddWithValue("@oid", orderID);
                cmd.Parameters.AddWithValue("@prdid", ProductID);
                cmd.Parameters.AddWithValue("@locid", LocID);
                cmd.Parameters.AddWithValue("@batch", Batch);
                cmd.Parameters.AddWithValue("@ptid", PTID);
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@finalZone", finalzone);
                cmd.Parameters.AddWithValue("@compid", CompanyID);
                cmd.Parameters.AddWithValue("@custid", CustomerID);
                da.SelectCommand = cmd;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                isSucess = true;
            }
            catch (Exception ex)
            {
                isSucess = false;
            }
            return isSucess;
        }

        public long gettbltype()
        {
            long currentrowcount = 0;
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(strcon);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_checkPutInProductRecord";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@obj", objname);
            cmd.Parameters.AddWithValue("@oid", orderID);
            cmd.Parameters.AddWithValue("@prdid", ProductID);
            cmd.Parameters.AddWithValue("@ptid", PTID);
            cmd.Parameters.AddWithValue("@locid", LocID);
            cmd.Parameters.AddWithValue("@qty", Qty);
            cmd.Parameters.AddWithValue("@finalZone", finalzone);
            cmd.Parameters.AddWithValue("@compid", CompanyID);
            cmd.Parameters.AddWithValue("@custid", CustomerID);
            da.SelectCommand = cmd;
            cmd.Connection.Open();
            //   da.Fill(ds, "tbl1");
            da.Fill(dt);
            currentrowcount = dt.Rows.Count;
            cmd.Connection.Close();
            return currentrowcount;
        }
        public DataSet GetUserDetails(long UserID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_getUserDetail";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@userid", UserID);
                da1.SelectCommand = cmd1;
                cmd.Connection.Open();
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetUserDetails");
            }
            finally
            {
            }
            return ds1;
        }

        public string GetUserID(long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string username = "";
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select top (1) UserName from mPassWordDetails where UserProfileID=" + uid + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    username = dt1.Rows[0]["UserName"].ToString();
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
        }
        public long GetPTINStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();

            long ptid = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetPTINStatus";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    ptid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPickUpStatus"); }
            finally
            { }
            return ptid;
        }
        public long getStatus(string objNM)
        {
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();

            long StatusID = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%PutIn%' ";
                cmd2.Connection = conn;
                cmd2.Parameters.Clear();
                da2.SelectCommand = cmd2;
                da2.Fill(ds2, "tbl1");
                dt2 = ds2.Tables[0];
                StatusID = long.Parse(dt2.Rows[0]["ID"].ToString());
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "getStatus"); }
            finally
            { }
            return StatusID;
        }

        public int SavePutInDetail(long oid, long ptid, long prodid, long locid, decimal qty, string batch)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_SavePutInDetail";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@ptid", ptid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@batch", batch);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "SavePutInDetail"); }
            finally
            { }
            return result;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}