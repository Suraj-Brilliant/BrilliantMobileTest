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
    /// Summary description for ws_get_qc_product_detail
    /// </summary>
    public class ws_get_qc_product_detail : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        long WarehouseID = 0, orderID = 0;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"arr_product_list\": [\n";   /*json Loop Start*/
                                                     // WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            orderID = long.Parse(context.Request.QueryString["qc_order_number"]);

            if (context.Request.QueryString["qc_order_number"] != null)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "exec WMS_SP_GetpartDetails_ForQC "+ orderID + ",'','','','' ";
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
                        string id = dt.Rows[i]["Prod_ID"].ToString();
                        string productName = dt.Rows[i]["Prod_Name"].ToString();
                        string productCode = dt.Rows[i]["Prod_Code"].ToString();
                        string qty = dt.Rows[i]["QCQty"].ToString();
                        string sequence = dt.Rows[i]["Sequence"].ToString();

                        string omsSkuCode = GetOMSSKUCODE(id);

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                        jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        //   jsonString = jsonString + "\"locationCode\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        //   jsonString = jsonString + "\"locationCodeImg\": \"" + CheckString(locationCode.Trim()) + "\",\n";
                        //   jsonString = jsonString + "\"locationDbId\": \"" + CheckString(locdbid.Trim()) + "\",\n";
                        jsonString = jsonString + "\"qty\": \"" + CheckString(qty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                        jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                        // jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\"\n";
                        jsonString = jsonString + "\"serialnumber\": [\n";
                        DataSet dssrno = new DataSet();
                        dssrno = GetSerialNo();
                        if (dssrno.Tables[0].Rows.Count > 0)
                        {
                            int cnt = 0;
                            cnt = dssrno.Tables[0].Rows.Count;

                            for (int p = 0; p <= cnt - 1; p++)
                            {

                               // string Reasonid = dssrno.Tables[0].Rows[p]["ID"].ToString();
                                string srno = dssrno.Tables[0].Rows[p]["Lottable1"].ToString();

                                
                              //  jsonString = jsonString + "\"reasoonid\": \"" + Reasonid.Trim() + "\",\n";
                              if(srno!="")
                                {
                                    jsonString = jsonString + "{\n";
                                    jsonString = jsonString + "\"product_serial_number\": \"" + srno.Trim() + "\"\n";
                                }
                                
                                if (p == cnt - 1 && srno!="")
                                {
                                    jsonString = jsonString + "}\n";
                                }
                                else if(p != cnt - 1)
                                {
                                    jsonString = jsonString + "},\n";
                                }
                                else
                                { }
                            }
                        }

                        jsonString = jsonString + "]\n";

                        if (i != cntr - 1)
                        {
                            //jsonString = jsonString + "}\n";
                            jsonString = jsonString + "},\n";
                        }
                        else
                        {
                            jsonString = jsonString + ",\n";
                        }

                        jsonString = jsonString + "\"reasoncode\": [\n";
                        //add reason code by suraj
                        DataSet dsreason = new DataSet();
                        dsreason = GetReasoncode();
                        if (dsreason.Tables[0].Rows.Count > 0)
                        {
                            int cnt = 0;
                            cnt = dsreason.Tables[0].Rows.Count;
                         
                            for (int p = 0; p <= cnt - 1; p++)
                            {
                            
                                string Reasonid = dsreason.Tables[0].Rows[p]["ID"].ToString();
                                string Resoncode = dsreason.Tables[0].Rows[p]["ReasonCode"].ToString();
                               
                                jsonString = jsonString + "{\n";
                                jsonString = jsonString + "\"reasoonid\": \"" + Reasonid.Trim() + "\",\n";
                                jsonString = jsonString + "\"reasoncode\": \"" + Resoncode.Trim() + "\"\n";

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

        private DataSet GetSerialNo()
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select Lottable1 from tskutransactionhistory where finalzone='GRN' and oid="+ orderID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        private DataSet GetReasoncode()
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from mReasonCode";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }

        public string GetOMSSKUCODE(string PrdID)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SqlDataReader dr1;

            SqlConnection conn = new SqlConnection(strcon);
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select OMSSKUCode from mProduct where ID=" + PrdID + " ";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            string omsSkuCode = dt1.Rows[0]["OMSSKUCode"].ToString();
            return omsSkuCode;
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