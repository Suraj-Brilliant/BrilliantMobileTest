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
    /// Summary description for receving_product_list
    /// </summary>
    public class receving_product_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0;
        string ObjectName = "";

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            orderID = long.Parse(context.Request.QueryString["orid"]);
            ObjectName = context.Request.QueryString["objname"].ToString();

            if (context.Request.QueryString["orid"] != null)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "exec WMS_SP_PutInList "+ orderID +" ";
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
                        string id = dt.Rows[i]["ProdID"].ToString();
                        string productName = dt.Rows[i]["Name"].ToString();
                        string productCode = dt.Rows[i]["ProductCode"].ToString();
                        string locationCode = dt.Rows[i]["Code"].ToString();
                        string qty = dt.Rows[i]["LocQty"].ToString();
                        string sequence = dt.Rows[i]["Sequence"].ToString();
                        string batchCode = dt.Rows[i]["BatchNo"].ToString();
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
                        jsonString = jsonString + "\"qty\": \"" + CheckString(qty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                        jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                        jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\",\n";

                        DataSet dslott = new DataSet();
                        DataTable dtlott = new DataTable();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT ID, ProductID, LottableTitle, LottableDescription, Sequence, LottableFormat, Active,'' as LottableValue  FROM  mProductLottable where ProductID=" + id + "";
                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        da.SelectCommand = cmd;
                        da.Fill(dslott, "tbl1");
                        dtlott = dslott.Tables[0];
                        int lottcnt = dtlott.Rows.Count;
                        if(lottcnt>0)
                        {
                            jsonString = jsonString + "\"is_have_lottable\": \"Yes\",\n";
                        }
                        else
                        {
                            jsonString = jsonString + "\"is_have_lottable\": \"No\",\n";
                        }

                        jsonString = jsonString + "\"arr_lottable\": [\n";
                        DataSet dsreason = new DataSet();
                        dsreason = GetSerialNumber(orderID, Convert.ToInt64(id));
                        decimal ProdQty = Convert.ToDecimal(qty);
                        if (dsreason.Tables[0].Rows.Count > 0)
                        {
                            int cnt = 0;
                            cnt = dsreason.Tables[0].Rows.Count;

                            for (int p = 0; p <= ProdQty - 1; p++)
                            {

                                string product_serial_number = dsreason.Tables[0].Rows[p]["Lottable1"].ToString();
                                string mfgdate = dsreason.Tables[0].Rows[p]["Lottable2"].ToString();
                                string grade = dsreason.Tables[0].Rows[p]["Lottable3"].ToString();

                                string lott1 = "";
                                string lott2 = "";
                                string lott3 = "";
                                jsonString = jsonString + "{\n";
                                for (int k=0;k<dtlott.Rows.Count;k++)
                                {
                                    if(k==0)
                                    {
                                        lott1 = dtlott.Rows[0]["LottableDescription"].ToString();
                                        jsonString = jsonString + "\"" + lott1 + "\": \"" + product_serial_number.Trim() + "\",\n";
                                    }
                                    if(k==1)
                                    {
                                        lott2 = dtlott.Rows[1]["LottableDescription"].ToString();
                                        jsonString = jsonString + "\"" + lott2 + "\": \"" + mfgdate.Trim() + "\",\n";
                                    }
                                    if(k==2)
                                    {
                                        lott3 = dtlott.Rows[2]["LottableDescription"].ToString();
                                        jsonString = jsonString + "\"" + lott3 + "\": \"" + grade.Trim() + "\"\n";
                                    }
                                }

                                if (p == cnt - 1)
                                {
                                    jsonString = jsonString + "}\n";
                                }
                                else
                                {
                                    jsonString = jsonString + "},\n";
                                }
                            }
                        }

                        jsonString = jsonString + "]\n";

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

        private DataSet GetSerialNumber(long orderID,long ProdID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from tskutransactionhistory where finalzone='QC' and object='"+ ObjectName +"' and oid=" + orderID + " and skuid='"+ProdID+"'";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
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