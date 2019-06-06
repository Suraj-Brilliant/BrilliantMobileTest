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
    /// Summary description for shipping_product_list
    /// </summary>
    public class shipping_product_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        DataSet dslott = new DataSet();
        DataTable dtlott = new DataTable();

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0;
        string objectname = "";

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            orderID = long.Parse(context.Request.QueryString["orid"]);
            if(context.Request.QueryString["objname"].ToString() != null)
            {
                objectname = context.Request.QueryString["objname"].ToString();
            }
            string date = ""; string prdBarcode = "";

            if (context.Request.QueryString["orid"] != null)
            {
                string objNM = "";

                DataTable dtobjnm = new DataTable();
                dtobjnm = GetDatatableDetails("select Object as Objectname from tOrderHead where id="+ orderID +" union all select Objectname from tTransferHead where id="+ orderID +"");
              //  objNM= dtobjnm.Rows[0]["ObjectName"].ToString();

                cmd.CommandType = CommandType.Text;
                if (objectname == "SalesOrder")
                {
                    cmd.CommandText = "exec WMS_SP_PickUpList '" + orderID + "',''";
                }
                else if(objectname == "Transfer")
                {
                    cmd.CommandText = "exec WMS_SP_PickUpList '','" + orderID + "'";
                }
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
                        string id = dt.Rows[i]["SkuId"].ToString();
                        string productName = dt.Rows[i]["Name"].ToString();
                        string productCode = dt.Rows[i]["ProductCode"].ToString();
                        string locationCode = dt.Rows[i]["Code"].ToString();
                        string qty = dt.Rows[i]["LocQty"].ToString();
                        string sequence = dt.Rows[i]["Sequence"].ToString();
                        string batchCode = dt.Rows[i]["BatchNo"].ToString();
                        string omsSkuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                        string locdbid = dt.Rows[i]["LocationID"].ToString();
                        string price= dt.Rows[i]["Price"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                        jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCode\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCodeImg\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationDbId\": \"" + CheckString(locdbid.Trim()) + "\",\n";
                        jsonString = jsonString + "\"qty\": \"" + CheckString(qty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"price\": \"" + CheckString(price.Trim()) + "\",\n";
                        jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                        jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                        jsonString = jsonString + "\"batchCode\": \"" +CheckString( batchCode.Trim()) + "\",\n";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT ID, ProductID, LottableTitle, LottableDescription, Sequence, LottableFormat, Active,'' as LottableValue  FROM  mProductLottable where ProductID=" + id + "";
                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        da.SelectCommand = cmd;
                        da.Fill(dslott, "tbl2");
                        dtlott = dslott.Tables[0];
                        int cntr1 = dtlott.Rows.Count;
                        if (cntr1 > 0)
                        {
                            jsonString = jsonString + "\"is_have_lottable\":\"Yes\",\n";
                            jsonString = jsonString + "\"arr_lottable\":[\n";
                            for (int j = 0; j <= cntr1 - 1; j++)
                            {
                                date = omsSkuCode.Substring(9, 4);
                                string LottableDescription = dtlott.Rows[j]["LottableDescription"].ToString();
                                string LottableValue = dtlott.Rows[j]["LottableValue"].ToString();
                                if (j == 0)
                                {
                                    LottableValue = productCode;
                                }
                                if (j == 1)
                                {
                                    LottableValue = date;
                                }

                                jsonString = jsonString + "{\n";
                                jsonString = jsonString + "\"LottableName\": \"" + LottableDescription.Trim() + "\",\n";
                                jsonString = jsonString + "\"Lottablevalue\": \"" + LottableValue.Trim() + "\"\n";
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
                        {
                            jsonString = jsonString + "\n \"is_have_lottable\":\"No\",\n";
                            jsonString = jsonString + "\"arr_lottable\":[]\n";
                        }
                       // jsonString = jsonString + "]\n}";
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

        private DataTable GetDatatableDetails(string Query)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection conn = new SqlConnection(strcon);
                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }

            }
            catch
            { }
            finally { }
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