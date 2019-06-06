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
    /// Summary description for grn_get_product_detail
    /// </summary>
    public class grn_get_product_detail : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string objectName = "";
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0;

        string objname = "",page="";
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
                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                if (context.Request.QueryString["oid"] != null)
                {

                    //get company id and customer id and get batch code value 
                    string batchCode = "";
                    if(page=="grn")
                    {
                        batchCode = GEtBatchcode(orderID);
                    }
                    if(page=="pickup")
                    {
                        batchCode = "";
                    }
                    if(page=="putin")
                    {
                        batchCode = GetBatchPutIn(orderID);
                    }
                    string dashbordtotqty = DashBordTotalQty(orderID,objname,page).ToString();
                    DataTable dtDashCalcQty = new DataTable();
                    string dashcalcqty = "0";
                    string dashseqcnt = "0";
                    string dashloccnt = "0";
                    dtDashCalcQty=DashBordCalcQty(orderID, objname,page);
                    if(dtDashCalcQty.Rows.Count>0)
                    {
                        if(page=="grn")
                        {
                            dashcalcqty = (Convert.ToInt32(dtDashCalcQty.Rows[0]["GRNQty"])).ToString();
                            dashseqcnt = dtDashCalcQty.Rows[0]["DashSeqCnt"].ToString();
                            dashloccnt = "";
                        }
                        if(page=="pickup")
                        {
                            dashcalcqty = (Convert.ToInt32(dtDashCalcQty.Rows[0]["GRNQty"])).ToString();
                            dashseqcnt = dtDashCalcQty.Rows[0]["DashSeqCnt"].ToString();
                            dashloccnt = dtDashCalcQty.Rows[0]["LocCnt"].ToString();
                        }
                        if(page=="putin")
                        {
                            dashcalcqty = (Convert.ToInt32(dtDashCalcQty.Rows[0]["GRNQty"])).ToString();
                            dashseqcnt = dtDashCalcQty.Rows[0]["DashSeqCnt"].ToString();
                            dashloccnt = dtDashCalcQty.Rows[0]["LocCnt"].ToString();
                        }
                    }

                    cmd.CommandType = CommandType.Text;
                    if(page=="grn")
                    {
                        //if (objname == "PurchaseOrder") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN " + orderID + ",'', '', 0"; }
                        //if (objname == "Transfer") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN '',''," + orderID + ", 0"; }
                        //if (objname == "SalesReturn") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN ''," + orderID + ",'', 0"; }
                        //if (objname == "SalesOrder") { cmd.CommandText = "exec WMS_SP_GetPartDetails_ForGRN ''," + orderID + ",'', 0"; }
                        if (objname == "PurchaseOrder") { cmd.CommandText = "exec SP_GRNListMobile " + orderID + ",0,0"; }
                        if (objname == "Transfer") { cmd.CommandText = "exec SP_GRNListMobile 0," + orderID + ", 0"; }
                        if (objname == "SalesReturn") { cmd.CommandText = "exec SP_GRNListMobile 0,0," + orderID + ""; }
                        if (objname == "SalesOrder") { cmd.CommandText = "exec SP_GRNListMobile 0,0," + orderID + ""; }
                    }
                    if (page=="pickup")
                    {
                        if (objname == "SalesOrder") { cmd.CommandText = "exec WMS_SP_PickUpListMobile '" + orderID + "',''"; }
                        else if (objname == "Transfer") { cmd.CommandText = "exec WMS_SP_PickUpListMobile '','" + orderID + "'"; }
                    }
                    if(page=="putin")
                    {
                        cmd.CommandText = "exec WMS_SP_PutInListNew " + orderID + "";
                    }
                   
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tbl1");
                    dt = ds.Tables[0];
                    int cntr = dt.Rows.Count;
                    if (cntr > 0)
                    {
                        jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"dashboardCalculatedQty\": \"" + CheckString(dashcalcqty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"dashboardTotalQty\": \"" + CheckString(dashbordtotqty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"dashboardSequenceCount\": \"" + CheckString(dashseqcnt.Trim()) + "\",\n";
                        jsonString = jsonString + "\"dashboardLocationCount\": \"" + CheckString(dashloccnt.Trim()) + "\",\n";
                        jsonString = jsonString + "\n\"arr_product_list\": [\n";
                        for (int i = 0; i <= cntr - 1; i++)
                        {
                            string id = "", productName = "", productCode = "", qty = "", sequence = "", price = "",batch="";
                            if (page=="grn")
                            {
                                id = dt.Rows[i]["Prod_ID"].ToString();
                                productName = dt.Rows[i]["Prod_Name"].ToString();
                                productCode = dt.Rows[i]["Prod_Code"].ToString();
                                qty = dt.Rows[i]["GRNQty"].ToString();
                                sequence = dt.Rows[i]["Sequence"].ToString();
                                price = dt.Rows[i]["Price"].ToString();
                            }
                            if (page == "pickup")
                            {
                                id = dt.Rows[i]["SkuId"].ToString();
                                productName = dt.Rows[i]["Name"].ToString();
                                productCode = dt.Rows[i]["ProductCode"].ToString();
                                qty = dt.Rows[i]["LocQty"].ToString();
                                sequence = dt.Rows[i]["Sequence"].ToString();
                                price = dt.Rows[i]["Price"].ToString();
                            }
                            if(page=="putin")
                            {
                                id = dt.Rows[i]["ProdID"].ToString();
                                productName = dt.Rows[i]["Name"].ToString();
                                productCode = dt.Rows[i]["ProductCode"].ToString();
                                qty = dt.Rows[i]["LocQty"].ToString();
                                sequence = dt.Rows[i]["Sequence"].ToString();
                                price = dt.Rows[i]["Price"].ToString();
                            }

                            string omsSkuCode = GetOMSSKUCODE(id);
                            string calcqty = GetGRNQty(objname, orderID, Convert.ToInt64(id),page);
                            string IsLott = CheckLottable(Convert.ToInt64(id), page);

                            jsonString = jsonString + "{\n";
                            jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                            jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                            jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                            jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                            jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                            jsonString = jsonString + "\"calculatedQty\": \"" + CheckString(calcqty.Trim()) + "\",\n";
                            jsonString = jsonString + "\"totalQty\": \"" + CheckString(qty.Trim()) + "\",\n";
                            jsonString = jsonString + "\"price\": \"" + CheckString(price.Trim()) + "\",\n";
                            jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                            jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                            jsonString = jsonString + "\"batchCode\": \"" + CheckString(batch.Trim()) + "\",\n";
                            jsonString = jsonString + "\"is_have_lottable\":\""+ IsLott.Trim() + "\"\n";

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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "ProcessRequest"); }
            finally
            {

            }
        }

        public string CheckLottable(long prdid, string pg)
        {
            DataTable dt = new DataTable();
            string result = "No";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_CheckLottable";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@page", pg);
                da1.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    result = "Yes";
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "CheckLottable"); }
            finally
            { }
            return result;
        }

        public string GetBatchPutIn(long oid)
        {
            string result ="";
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GetBatchPutIn";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@oid", oid);
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    result = obj.ToString();
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GetBatchPutIn"); }
            finally
            { }
            return result;
        }
        public DataTable DashBordCalcQty(long oid,string obj, string pg)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_DashbordCalcQty ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@page", pg);
                da1.Fill(dt);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "DashBordCalcQty"); }
            finally
            { }
            return dt;
        }

        public int DashBordTotalQty(long oid,string ob,string pg)
        {
            int result = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DashbordTotalQty";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@oid", oid);
                cmd.Parameters.AddWithValue("@obj", ob);
                cmd.Parameters.AddWithValue("@page", pg);
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "DashBordTotalQty"); }
            finally
            { }
            return result;
        }

        public string GetGRNQty(string obj,long oid,long prodid,string pg)
        {
            string GRNQty = "0.00";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetGRNQty ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@page", pg);
                da1.Fill(dt1);
               // dt1 = ds1.Tables[0];
                if(dt1.Rows.Count>0)
                {
                    GRNQty = dt1.Rows[0]["GRNQty"].ToString();
                }
                if(GRNQty=="")
                {
                    GRNQty = "0.00";
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GetGRNQty"); }
            finally
            { }
            return GRNQty;
        }

        public string GetOMSSKUCODE(string PrdID)
        {
            string omsSkuCode = "";
            try
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
                omsSkuCode = dt1.Rows[0]["OMSSKUCode"].ToString();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GetOMSSKUCODE"); }
            finally
            { }
            return omsSkuCode;
        }

        private string GEtBatchcode(long orderID)
        {
            string batchcode = "";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                SqlConnection conn = new SqlConnection(strcon);
                string comapanyid = "0";
                string customerid = "0";
                cmd1.CommandType = CommandType.Text;
                //cmd1.CommandText = "select companyid ,customerid from tpurchaseorderhead where id="+ orderID + " ";
                cmd1.CommandText = "select companyid ,customerid,Object as Objectname from tpurchaseorderhead where id=" + orderID + " union all select companyid, customerid, Objectname from tTransferhead where id = " + orderID + " union all select companyid, customerid, Object as Objectname from tOrderhead where id = " + orderID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    comapanyid = dt1.Rows[0]["companyid"].ToString();
                    customerid = dt1.Rows[0]["customerid"].ToString();
                    objectName = dt1.Rows[0]["Objectname"].ToString();
                }
                batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GEtBatchcode"); }
            finally
            { }
           
           
            return batchcode;
        }

        private string GetBatchNoForGRN(long CustomerID, long CompanyID)
        {
            string Batch = "";
            string batchformat = "";
            try
            {
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
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GetBatchNoForGRN"); }
            finally
            { }
            return batchformat;
        }
        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }

        public long GetNextBatchNo(long CustID, long CompID)
        {
            string Batch = "";
            long Number = 0;
            try
            {
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
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn-get-product-detail", "GetNextBatchNo"); }
            finally
            { }
            return Number;
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