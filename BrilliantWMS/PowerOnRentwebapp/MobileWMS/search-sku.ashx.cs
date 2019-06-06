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
    /// Summary description for search_sku1
    /// </summary>
    public class search_sku1 : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0;
        string skey = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            skey = context.Request.QueryString["skey"];
            try
            {
                if (context.Request.QueryString["skey"] != null)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "	Select distinct V.ID,V.Name,V.ProductCode,V.Description,V.omsskucode,W.WarehouseName from V_WMS_SKUSearch V left outer join mWarehouseMaster W on V.WarehouseID=W.ID  left outer join mProduct P on V.ID=P.ID  where V.WarehouseID = " + WarehouseID + " and (V.ProductCode like '%" + skey + "%' or V.Name like '%" + skey + "%') ";
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
                            string prodid = dt.Rows[i]["ID"].ToString();
                            string productName = dt.Rows[i]["Name"].ToString();
                            string omsSkuCode = dt.Rows[i]["omsskucode"].ToString();
                            string productCode = dt.Rows[i]["ProductCode"].ToString();
                            string warehouse = dt.Rows[i]["WarehouseName"].ToString();
                            string description = dt.Rows[i]["Description"].ToString();

                            jsonString = jsonString + "{\n";
                            jsonString = jsonString + "\"id\": \"" + prodid.Trim() + "\",\n";
                            jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                            jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                            jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                            jsonString = jsonString + "\"warehouse\": \"" + CheckString(warehouse.Trim()) + "\",\n";
                            jsonString = jsonString + "\"description\": \"" + CheckString(description.Trim()) + "\",\n";

                            jsonString = jsonString + "\"locationDetails\":[\n";


                            SqlCommand cmd1 = new SqlCommand();
                            SqlDataAdapter da1 = new SqlDataAdapter();
                            DataSet ds1 = new DataSet();
                            DataTable dt1 = new DataTable();
                            cmd1.CommandType = CommandType.Text;
                            cmd1.CommandText = "Select V.LocationID,V.LocationCode,sum(V.ClosingBalance) as ClosingBalance from V_WMS_SKUSearch V left outer join mWarehouseMaster W on V.WarehouseID = W.ID left outer join mProduct P on V.ID = P.ID  where V.ClosingBalance>0 and V.WarehouseID = " + WarehouseID + " and V.ProductCode= '"+ productCode +"' group by V.LocationID,V.LocationCode ";
                            cmd1.Connection = conn;
                            cmd1.Parameters.Clear();
                            da1.SelectCommand = cmd1;
                            da1.Fill(ds1, "tbl2");
                            dt1 = ds1.Tables[0];
                            int cntr1 = dt1.Rows.Count;
                            if (cntr1 > 0)
                            {
                                for (int j = 0; j <= cntr1 - 1; j++)
                                {

                                    string locationCode = dt1.Rows[j]["LocationCode"].ToString();
                                    string locationDbId = dt1.Rows[j]["LocationID"].ToString(); ;
                                    string skuQty = dt1.Rows[j]["ClosingBalance"].ToString(); ;

                                    jsonString = jsonString + "{\n";
                                    jsonString = jsonString + "\"locationCode\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                                    jsonString = jsonString + "\"locationDbId\": \"" + CheckString(locationDbId.Trim()) + "\",\n";
                                    jsonString = jsonString + "\"skuQty\": \"" + CheckString(skuQty.Trim()) + "\",\n";

                                    jsonString = jsonString + "\"searialCode\":[\n";


                                    SqlCommand cmd2 = new SqlCommand();
                                    SqlDataAdapter da2 = new SqlDataAdapter();
                                    DataSet ds2 = new DataSet();
                                    DataTable dt2 = new DataTable();
                                    cmd2.CommandType = CommandType.Text;
                                    cmd2.CommandText = "select Lottable1 as Code,BatchCode from tskutransaction where skuid=" + Convert.ToInt64(prodid) + " and LocationID=" + Convert.ToInt64(locationDbId) + " and ClosingBalance>0";
                                    cmd2.Connection = conn;
                                    cmd2.Parameters.Clear();
                                    da2.SelectCommand = cmd2;
                                    da2.Fill(ds2, "tbl3");
                                    dt2 = ds2.Tables[0];
                                    int cntr2 = dt2.Rows.Count;
                                    if (cntr2 > 0)
                                    {
                                        for (int k = 0; k <= cntr2 - 1; k++)
                                        {
                                            string Code = dt2.Rows[k]["Code"].ToString();
                                            string batchcode = dt2.Rows[k]["BatchCode"].ToString();

                                            jsonString = jsonString + "{\n";
                                            jsonString = jsonString + "\"code\": \"" + CheckString(Code.Trim()) + "\",\n";
                                            jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchcode.Trim()) + "\"\n";

                                            if (k == cntr2 - 1)
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
                                    { }


                                    if (j == cntr1 - 1)
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
                            { }
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
            catch (Exception ex)
            { }
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

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }
    }
}