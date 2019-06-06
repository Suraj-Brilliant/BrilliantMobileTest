using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using System.Text;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for bulk_assign_location
    /// </summary>
    public class bulk_assign_location : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, CustomerID = 0, CompanyID = 0, LocID=0,ProdID=0;

        string objectName = "";
        string objname = "", page = "", Batch = "", AllSelect="",BulkData="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long PTID = 0;
            try
            {
                if (context.Request.Form["oid"] != null)
                {
                    orderID = long.Parse(context.Request.Form["oid"]);
                }
                if (context.Request.Form["wrid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.Form["wrid"]);
                }
                if (context.Request.Form["objname"] != null)
                {
                    objname = context.Request.Form["objname"].ToString();
                }
                if (context.Request.Form["uid"] != null)
                {
                    UserID = long.Parse(context.Request.Form["uid"]);
                }
                if (context.Request.Form["page"] != null)
                {
                    page = context.Request.Form["page"].ToString();
                }
                if (context.Request.Form["isAllSelected"] != null)
                {
                    AllSelect = context.Request.Form["isAllSelected"].ToString();
                }
                if (context.Request.Form["Batch"] != null)
                {
                    Batch = context.Request.Form["Batch"].ToString();
                }
                if (context.Request.Form["loc"] != null)
                {
                    LocID = long.Parse(context.Request.Form["loc"]);
                }
                if (context.Request.Form["pid"] != null)
                {
                    ProdID = long.Parse(context.Request.Form["pid"]);
                }
                if (context.Request.Form["bulkdata"] != null)
                {
                    BulkData = context.Request.Form["bulkdata"].ToString();
                }

                iInboundClient Inbound = new iInboundClient();
                string userName = GetUserID(UserID);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                DataSet dsUserDetail = new DataSet();
                dsUserDetail = GetUserDetails(UserID);
                CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
                CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());
                int result = 0;
                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";
                if (AllSelect=="yes")
                {
                    result = BulkSavePutIn(orderID, objname, page, CompanyID, CustomerID, UserID, LocID, Batch, ProdID);
                    if(result>0)
                    {
                        jsonString = jsonString + "\"status\": \"success\",\n";
                        jsonString = jsonString + "\"reason\": \"\"\n";
                    }
                    else
                    {
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                    }
                }
                else
                {
                    tPutInHead ph = new tPutInHead();
                    ph.ObjectName = objname;
                    ph.OID = orderID;
                    ph.PutInDate = DateTime.Now;
                    ph.PutInBy = UserID;
                    ph.Remark = "";
                    ph.CreatedBy = UserID;
                    ph.CreationDate = DateTime.Now;
                    ph.Status = getStatus(objname);
                    ph.Company = CompanyID;
                    ph.CustomerID = CustomerID;
                    ph.OrderFrom = "Mobile";
                    PTID = GetPTINStatus(orderID, objname, UserID);
                    if (PTID == 0)
                    {
                        PTID = Inbound.SavetPutInHead(ph, profile.DBConnection._constr);
                    }
                    if(PTID>0)
                    {
                        int save = 0;
                        string[] srno = BulkData.Split('|');
                        decimal srcount = Convert.ToDecimal(srno.Length);
                        save = SavePutInDetail(orderID, PTID, ProdID, LocID, srcount, Batch);
                        for (int i=0;i<= srcount - 1;i++)
                        {
                            SaveLottablePutIn(orderID, srno[i].ToString(), objname, PTID, ProdID, 1, CompanyID, CustomerID, UserID, LocID, Batch);
                        }

                        if (save > 0)
                        {
                            jsonString = jsonString + "\"status\": \"success\",\n";
                            jsonString = jsonString + "\"reason\": \"\"\n";
                        }
                        else
                        {
                            jsonString = jsonString + "\"status\": \"failed\",\n";
                            jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                        }
                    }
                }

                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "ProcessRequest"); }
            finally
            { }
        }

        public int SaveLottablePutIn(long oid, string srno, string obj, long ptid, long prodid, decimal qty, long compid, long custid, long uid, long locid, string batch)
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
                cmdDetail.CommandText = "SP_SaveLottablePutIn";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@serialno", srno);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@ptid", ptid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@uid", uid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@batch", batch);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "SavePutInDetail"); }
            finally
            { }
            return result;
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
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "SavePutInDetail"); }
            finally
            { }
            return result;
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
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "GetPickUpStatus"); }
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
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "getStatus"); }
            finally
            { }
            return StatusID;
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
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "GetUserDetails"); }
            finally
            { }
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
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "GetUserID"); }
            finally
            { }
            return username;
        }
        public int BulkSavePutIn(long oid,string obj,string pg,long compid,long custid,long uid,long locid,string batch,long prdid)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_BulkPutInSave";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@page", pg);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@uid", uid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@batch", batch);
                cmdDetail.Parameters.AddWithValue("@prdid", prdid);
                cmdDetail.Connection.Open();
                result=cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "bulk_assign_location", "BulkSavePutIn"); }
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