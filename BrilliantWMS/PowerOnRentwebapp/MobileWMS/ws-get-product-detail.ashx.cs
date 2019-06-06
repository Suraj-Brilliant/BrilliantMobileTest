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
    /// Summary description for ws_get_product_detail
    /// </summary>
    public class ws_get_product_detail : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string objectName = "";
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0;

        string objname = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n";   /*json Loop Start*/
            // WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            orderID = long.Parse(context.Request.QueryString["grn_order_number"]);
            if(context.Request.QueryString["objname"]!=null)
            {
                objname = context.Request.QueryString["objname"].ToString();
            }
            if (context.Request.QueryString["grn_order_number"] != null)
            {

                //get company id and customer id and get batch code value 
                string batchCode = "";
                batchCode = GEtBatchcode(orderID);

               // string objNM = objectName;
                string objNM = objname;

                cmd.CommandType = CommandType.Text;
                if (objNM == "PurchaseOrder") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN " + orderID + ",'', '', 0"; }
                if (objNM == "Transfer") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN '',''," + orderID + ", 0"; }
                if (objNM == "SalesReturn") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN ''," + orderID + ",'', 0"; }
                if (objNM == "SalesOrder") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN ''," + orderID + ",'', 0"; }
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;              
                if (cntr > 0)
                {                  
                    jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\",\n";
                    jsonString = jsonString + "\n\"arr_product_list\": [\n";
                    for (int i = 0; i <= cntr - 1; i++)
                    {
                        string id = dt.Rows[i]["Prod_ID"].ToString();
                        string productName = dt.Rows[i]["Prod_Name"].ToString();
                        string productCode = dt.Rows[i]["Prod_Code"].ToString();
                        string qty = dt.Rows[i]["GRNQty"].ToString();
                        string sequence = dt.Rows[i]["Sequence"].ToString();
                        string price= dt.Rows[i]["Price"].ToString();
                        //string omsSkuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                        // string batchCode = dt.Rows[i]["batchNo"].ToString();

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
                        jsonString = jsonString + "\"price\": \"" + CheckString(price.Trim()) + "\",\n";
                        jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                        jsonString = jsonString + "\"serialCode\": \"N/A\"\n";
                        //   jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\"\n";

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

        private string GEtBatchcode(long orderID)
        {
            string batchcode = "";
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SqlDataReader dr1;
            SqlConnection conn = new SqlConnection(strcon);
            string comapanyid = "0";
            string customerid ="0";
            cmd1.CommandType = CommandType.Text;
            //cmd1.CommandText = "select companyid ,customerid from tpurchaseorderhead where id="+ orderID + " ";
            cmd1.CommandText = "select companyid ,customerid,Object as Objectname from tpurchaseorderhead where id=" + orderID + " union all select companyid, customerid, Objectname from tTransferhead where id = " + orderID + " union all select companyid, customerid, Object as Objectname from tOrderhead where id = " + orderID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if(dt1.Rows.Count>0)
            {
                comapanyid = dt1.Rows[0]["companyid"].ToString();
                customerid = dt1.Rows[0]["customerid"].ToString();
                objectName= dt1.Rows[0]["Objectname"].ToString();
            }
            batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
            return batchcode;
        }

        private string GetBatchNoForGRN(long CustomerID, long CompanyID)
        {
            string Batch = "";
            string batchformat = "";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select BatchNoFormat from mCustomer where ID='" + CustomerID + "' and ParentID='" + CompanyID + "'";
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (obj != null)
            {
                Batch = obj.ToString();
            }
            if (Batch != "")
            {
                string[] format = Batch.Split('-');
                string batchname = format[0];
                string date = format[1];
                long number = Convert.ToInt64(format[2]);
                long nextno = 0;
                if (date == "Number")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    batchformat = batchname + "-" + "01" + "-" + nextno;
                }
                if (date == "YYMM")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    string datetime = DateTime.Now.ToString("yyMM");
                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
                if (date == "YYQ")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    int month = DateTime.Now.Month;
                    string datetime = "";
                    if (month >= 1 && month <= 3) { datetime = "01"; }

                    else if (month >= 4 && month <= 6) { datetime = "02"; }

                    else if (month >= 7 && month <= 9) { datetime = "03"; }

                    else { datetime = "04"; }

                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
                if (date == "DDMMYY")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    string datetime = DateTime.Now.ToString("ddMMyy");
                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
            }
            return batchformat;
        }

        public long GetNextBatchNo(long CustID, long CompID)
        {
            string Batch = "";
            long Number = 0;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select top 1 BatchNo from tgrnhead where CompanyId='" + CompID + "' and CustomerID='" + CustID + "' order by ID desc ";
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (obj != null)
            {
                Batch = obj.ToString();
            }
            if (Batch != "")
            {
                string[] format = Batch.Split('-');
                long No = Convert.ToInt64(format[2]);
                Number = No + 1;
            }
            return Number;
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