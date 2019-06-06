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
    /// Summary description for get_bin_status_by_location
    /// </summary>
    public class get_bin_status_by_location : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0; string lcode = "";

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            lcode= context.Request.QueryString["lcode"];

            if (lcode != null)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select V.ID,V.Name,V.ProductCode,V.Description,V.BatchCode,V.LocationID,V.LocationCode,V.ClosingBalance,V.Path,V.SkuImage,V.WarehouseID ,P.OMSSKUCode  from V_WMS_SKUSearch V left outer join mProduct P on V.ID=P.ID   where V.WarehouseID = " + WarehouseID + " and LocationCode='" + lcode + "'";
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
                        string id = dt.Rows[i]["ID"].ToString();
                        string productName = dt.Rows[i]["Name"].ToString();
                        string productCode = dt.Rows[i]["ProductCode"].ToString();                       
                        string systemQty = dt.Rows[i]["ClosingBalance"].ToString();
                        string batchCode = dt.Rows[i]["BatchCode"].ToString();
                        string omsSkuCode = dt.Rows[i]["OMSSKUCode"].ToString();   
                        string locationID= dt.Rows[i]["LocationID"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id + "\",\n";
                        jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                        jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCode\": \"" + CheckString(lcode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCodeImg\": \"" + CheckString(lcode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationDbId\": \"" + CheckString(locationID.Trim()) + "\",\n";
                        jsonString = jsonString + "\"qty\": \"" + CheckString(systemQty.Trim()) + "\",\n";                        
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