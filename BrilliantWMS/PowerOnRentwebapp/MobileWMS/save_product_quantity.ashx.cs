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
    /// Summary description for save_product_quantity
    /// </summary>
    public class save_product_quantity : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long GRNID = 0;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, CustomerID = 0, CompanyID = 0, ProductID = 0;
        decimal Qty = 0;
       
        string objname = "", page = "",finalzone="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
          
            if (context.Request.QueryString["objname"] != null)
            {
                objname = context.Request.QueryString["objname"].ToString();
            }
            if (context.Request.QueryString["oid"] != null)
            {
                orderID = long.Parse(context.Request.QueryString["oid"]);
            }
            if (context.Request.QueryString["wrid"] != null)
            {
                WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            }
            if (context.Request.QueryString["pid"] != null)
            {
                ProductID = Convert.ToInt64(context.Request.QueryString["pid"]);
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

            GRNID = GetGRNStatus(orderID, objname, UserID);

            if (GRNID > 0)
            {
                decimal orderqty = ProdOrderQty(orderID, ProductID, page,objname);
                if(Qty>orderqty)
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"Quantity Greater than Order Quantity\"\n";
                }
                else
                {
                    int result = UpdateGRNQty(GRNID, ProductID, Qty, objname);
                    if(result>0)
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

        public int UpdateGRNQty(long grnid, long prodid, decimal qty,string obj)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdateGRNQty";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
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
        public decimal ProdOrderQty(long oid,long prdid, string page,string obj)
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
        public Boolean executeqty(string sp_name, string tbltype)
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
                cmd.Parameters.AddWithValue("@grnid", orderID);
                cmd.Parameters.AddWithValue("@prdid", ProductID);
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@finalZone", finalzone);
                cmd.Parameters.AddWithValue("@compid", CompanyID);
                cmd.Parameters.AddWithValue("@custid", CustomerID);
                cmd.Parameters.AddWithValue("@tbltype", tbltype);
                da.SelectCommand = cmd;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                isSucess = true;
            }
            catch(Exception ex)
            {
                isSucess = false;
            }
            return isSucess;
        }

        public long gettbltype(string tbltype)
        {
            long currentrowcount = 0;
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(strcon);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_checkGrnProductRecord";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@obj", objname);
            cmd.Parameters.AddWithValue("@grnid", orderID);
            cmd.Parameters.AddWithValue("@prdid", ProductID);
            cmd.Parameters.AddWithValue("@qty", Qty);
            cmd.Parameters.AddWithValue("@finalZone", finalzone);
            cmd.Parameters.AddWithValue("@compid", CompanyID);
            cmd.Parameters.AddWithValue("@custid", CustomerID);
            cmd.Parameters.AddWithValue("@tbltype", tbltype);
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

        public int SaveGRNDetail(long oid, string obj, long grnid, long prodid, decimal qty)
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
                cmdDetail.CommandText = "SP_SaveLiveGRNDetails";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
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
        public long GetGRNStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            long grnid = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetGRNStatusNew";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    grnid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetGRNStatus"); }
            finally
            { }
            return grnid;
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
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%GRN%' ";
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
        public long GetNextBatchNo(long CustID, long CompID)
        {
            string Batch = "";
            long Number = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select top 1 BatchNo from tgrnhead where CompanyId='" + CompID + "' and CustomerID='" + CustID + "' order by ID desc ";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    Batch = obj.ToString();
                }
                if (Batch != "")
                {
                    string[] format = Batch.Split('-');
                    long No = Convert.ToInt64(format[2]);
                    Number = No + 1;
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetNextBatchNo"); }
            finally
            { }
            return Number;
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
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
        }

        private string GEtBatchcode(long orderID)
        {
            string batchcode = "";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                SqlConnection conn = new SqlConnection(strcon);
                string comapanyid = "0";
                string customerid = "0";
                cmd1.CommandType = CommandType.Text;
                //cmd1.CommandText = "select companyid ,customerid from tpurchaseorderhead where id="+ orderID + " ";
                cmd1.CommandText = "select companyid ,customerid,Object as Objectname from tpurchaseorderhead where id=" + orderID + " union all select companyid, customerid, Objectname from tTransferhead where id = " + orderID + " union all select companyid, customerid, Object as Objectname from tOrderhead where id = " + orderID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    comapanyid = dt1.Rows[0]["companyid"].ToString();
                    customerid = dt1.Rows[0]["customerid"].ToString();
                    objname = dt1.Rows[0]["Objectname"].ToString();
                }
                batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GEtBatchcode"); }
            finally
            { }


            return batchcode;
        }
        private string GetBatchNoForGRN(long CustomerID, long CompanyID)
        {
            string Batch = "";
            string batchformat = "";
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select BatchNoFormat from mCustomer where ID='" + CustomerID + "' and ParentID='" + CompanyID + "'";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    Batch = obj.ToString();
                }
                if (Batch != "")
                {
                    string[] format = Batch.Split('-');
                    string batchname = format[0];
                    string date = format[1];
                    long number = Convert.ToInt64(format[2]);
                    long nextno = 0;
                    if (date == "Number")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        batchformat = batchname + "-" + "01" + "-" + nextno;
                    }
                    if (date == "YYMM")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        string datetime = DateTime.Now.ToString("yyMM");
                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                    if (date == "YYQ")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        int month = DateTime.Now.Month;
                        string datetime = "";
                        if (month >= 1 && month <= 3) { datetime = "01"; }

                        else if (month >= 4 && month <= 6) { datetime = "02"; }

                        else if (month >= 7 && month <= 9) { datetime = "03"; }

                        else { datetime = "04"; }

                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                    if (date == "DDMMYY")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        string datetime = DateTime.Now.ToString("ddMMyy");
                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetBatchNoForGRN"); }
            finally
            { }
            return batchformat;
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