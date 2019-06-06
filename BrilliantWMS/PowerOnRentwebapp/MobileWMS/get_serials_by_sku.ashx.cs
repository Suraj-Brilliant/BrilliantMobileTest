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
    /// Summary description for get_serials_by_sku
    /// </summary>
    public class get_serials_by_sku : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, ProductID = 0, UserID = 0;

        string objname = "", page = "", gtype = "";
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
                if (context.Request.QueryString["page"] != null)
                {
                    page = (context.Request.QueryString["page"]).ToString();
                }
                if (context.Request.QueryString["pid"] != null)
                {
                    ProductID = Convert.ToInt64(context.Request.QueryString["pid"]);
                }
                if (context.Request.QueryString["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"seriallist\": [\n";
                DataTable dtsrno = new DataTable();
                dtsrno = GetSerialForPutIn(orderID, ProductID, objname);
                if(dtsrno.Rows.Count>0)
                {
                    for(int i=0;i<dtsrno.Rows.Count;i++)
                    {
                        string srno = dtsrno.Rows[i]["Lottable1"].ToString();
                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"serial\": \"" + CheckString(srno.Trim()) + "\"\n";
                        if (i == dtsrno.Rows.Count - 1)
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
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "get_serials_by_sku", "ProcessRequest"); }
            finally
            { }
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }
        public DataTable GetSerialForPutIn(long oid, long prodid, string obj)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetSerialForPutIn";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "get_serials_by_sku", "GetSerialForPutIn"); }
            finally
            { }
            return dt;
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