using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for delete_scan_lottable
    /// </summary>
    public class delete_scan_lottable : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, pid = 0, LocID = 0;

        string objname = "", page = "", gtype = "",serialno="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            try
            {
                if (context.Request.QueryString["objname"] != null)
                {
                    objname = context.Request.QueryString["objname"].ToString();
                }
                if (context.Request.QueryString["oid"] != null)
                {
                    orderID = long.Parse(context.Request.QueryString["oid"]);
                }
                if (context.Request.QueryString["oid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
                }
                if (context.Request.QueryString["page"] != null)
                {
                    page = (context.Request.QueryString["page"]).ToString();
                }
                if (context.Request.QueryString["serialno"] != null)
                {
                    serialno = (context.Request.QueryString["serialno"]).ToString();
                }
                if (context.Request.QueryString["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
                }
                if (context.Request.QueryString["gtype"] != null)
                {
                    gtype = (context.Request.QueryString["gtype"]).ToString();
                }
                if (context.Request.QueryString["pid"] != null)
                {
                    pid = Convert.ToInt64(context.Request.QueryString["pid"]);
                }
                if (context.Request.QueryString["locid"] != null)
                {
                    LocID = Convert.ToInt64(context.Request.QueryString["locid"]);
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";

                int result = 0;
                if(page=="grn")
                {
                    result = DeleteSKU(serialno, orderID, objname, pid, page);
                }
                if(page=="pickup")
                {
                    result= DeleteScanSKUPickUp(serialno, orderID, objname, pid, page, LocID);
                }
                if(page=="putin")
                {
                    result= DeleteScanSKUPutIn(serialno, orderID, objname, pid, page, LocID);
                }

                if (result>0)
                {
                    jsonString = jsonString + "\"status\": \"success\",\n";
                    jsonString = jsonString + "\"reason\": \"\"\n";
                }
                else
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                }

                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "delete_scan_lottable", "ProcessRequest"); }
            finally
            { }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private int DeleteScanSKUPutIn(string srno, long oid, string obj, long prdid, string page, long locid)
        {
            int result = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DeleteScanSKUPutIn";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@srno", srno);
                cmd.Parameters.AddWithValue("@oid", oid);
                cmd.Parameters.AddWithValue("@obj", obj);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@locid", locid);
                result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "delete_scan_lottable", "DeleteSKU"); }
            finally
            { }
            return result;
        }
        private int DeleteScanSKUPickUp(string srno, long oid, string obj, long prdid, string page,long locid)
        {
            int result = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DeleteScanSKUPickUp";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@srno", srno);
                cmd.Parameters.AddWithValue("@oid", oid);
                cmd.Parameters.AddWithValue("@obj", obj);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@locid", locid);
                result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "delete_scan_lottable", "DeleteSKU"); }
            finally
            { }
            return result;
        }
        private int DeleteSKU(string srno,long oid,string obj,long prdid,string page)
        {
            int result = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DeleteScanSKU";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@srno", srno);
                cmd.Parameters.AddWithValue("@oid", oid);
                cmd.Parameters.AddWithValue("@obj", obj);
                cmd.Parameters.AddWithValue("@prdid", prdid);
                cmd.Parameters.AddWithValue("@page", page);
                result = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "delete_scan_lottable", "DeleteSKU"); }
            finally
            { }
            return result;
        }
    }
}