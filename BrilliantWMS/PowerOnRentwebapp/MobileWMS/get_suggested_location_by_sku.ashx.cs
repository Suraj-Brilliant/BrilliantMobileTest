using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for get_suggested_location_by_sku
    /// </summary>
    public class get_suggested_location_by_sku : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, ProductID = 0;
       
        string objname = "", page = "";
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
                if (context.Request.QueryString["wrid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
                }
                if (context.Request.QueryString["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
                }
                if (context.Request.QueryString["page"] != null)
                {
                    page = (context.Request.QueryString["page"]).ToString();
                }
                if (context.Request.QueryString["pid"] != null)
                {
                    ProductID = Convert.ToInt64(context.Request.QueryString["pid"]);
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/

                jsonString = jsonString + "\"seriallist\":[\n";

                DataTable dtloc = new DataTable();
                dtloc = GetSuggestLoc(ProductID, WarehouseID);
                if(dtloc.Rows.Count>0)
                {
                    for (int i = 0; i < dtloc.Rows.Count; i++)
                    {
                        jsonString = jsonString + "{\n";
                        string loccode = dtloc.Rows[i]["LocCode"].ToString();
                        string locid = dtloc.Rows[i]["LocID"].ToString();

                        jsonString = jsonString + "\"locationCode\": \"" + CheckString(loccode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationId\": \"" + CheckString(locid.Trim()) + "\"\n";

                        if (i == dtloc.Rows.Count - 1)
                        {
                            jsonString = jsonString + "}]\n";
                        }
                        else
                        {
                            jsonString = jsonString + "},\n";
                        }
                    }
                }
                else
                {
                    jsonString = jsonString + "]\n";
                }
               
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "get_suggested_location_by_sku", "ProcessRequest"); }
            finally
            { }
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }
        public DataTable GetSuggestLoc(long prdid,long wareid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetSuggestLoc";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@wareid", wareid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "get_suggested_location_by_sku", "GetSuggestLoc"); }
            finally
            { }
            return dt1;
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