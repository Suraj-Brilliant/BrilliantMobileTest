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
    /// Summary description for get_cycle_count_product_list
    /// </summary>
    public class get_cycle_count_product_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, UserID = 0, ccID = 0;
        string skey = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/

            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            UserID = long.Parse(context.Request.QueryString["uid"]);
            ccID = long.Parse(context.Request.QueryString["cycleCountID"]);

            if (ccID != 0)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "exec dbo.SP_WMS_CCPlnDetail " + ccID + "";
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;
                if (cntr > 0)
                {
                    for (int i = 0; i <= cntr - 1; i++)
                    {
                        string id= dt.Rows[i]["SKUId"].ToString();
                        string productName = dt.Rows[i]["Name"].ToString();
                        string productCode = dt.Rows[i]["ProductCode"].ToString();
                        string locationCode = dt.Rows[i]["Code"].ToString();                                                
                        string batchCode = dt.Rows[i]["BatchCode"].ToString();
                        string omsSkuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                        string locdbid = dt.Rows[i]["LocationID"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                        jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCode\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCodeImg\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationDbId\": \"" + CheckString(locdbid.Trim()) + "\",\n";                        
                        jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                        jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\"\n";
                        if (i == cntr - 1)
                        {
                            jsonString = jsonString + "}\n";
                        }
                        else
                        {
                            jsonString = jsonString + "},\n";
                        }
                    }
                }
            }
            else { }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);

        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
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