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
    /// Summary description for view_lottables
    /// </summary>
    public class view_lottables : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
       
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0,ProductID=0,UserID=0;

        string objname = "", page = "",gtype="";
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
                if (context.Request.QueryString["gtype"] != null)
                {
                    gtype = (context.Request.QueryString["gtype"]).ToString();
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/

                DataTable dtprd = new DataTable();
                if(page=="grn" || page=="pickup")
                {
                    dtprd = GetProductdetail(ProductID, orderID, objname);
                }
                if(page=="putin")
                {
                    dtprd = GetProductdetailPutIn(ProductID, orderID, objname);
                }
               
                string id = dtprd.Rows[0]["ProdID"].ToString();
                string prodname = dtprd.Rows[0]["ProdName"].ToString();
                string prodcode= dtprd.Rows[0]["ProdCode"].ToString();
                string totalqty= dtprd.Rows[0]["OQty"].ToString();
                string price= dtprd.Rows[0]["Price"].ToString();
                string calcqty = GetGRNQty(objname, orderID, ProductID,page);
                string islottable = IsProdLottable(ProductID,page);


                jsonString = jsonString + "\"product_id\": \"" + id.Trim() + "\",\n";
                jsonString = jsonString + "\"product_name\": \"" + CheckString(prodname.Trim()) + "\",\n";
                jsonString = jsonString + "\"product_code\": \"" + CheckString(prodcode.Trim()) + "\",\n";
                jsonString = jsonString + "\"calculated_qty\": \"" + CheckString(calcqty.Trim()) + "\",\n";
                jsonString = jsonString + "\"total_qty\": \"" + CheckString(totalqty.Trim()) + "\",\n";
                jsonString = jsonString + "\"price\": \"" + CheckString(price.Trim()) + "\",\n";
                jsonString = jsonString + "\"is_have_lottable\": \"" + CheckString(islottable.Trim()) + "\",\n"; 
                jsonString = jsonString + "\"arr_lottable_list\":[\n";

                string loccode = "";
                //if(page=="pickup")
                //{
                //    loccode=GetLocCode(orderID, ProductID, objname, page);
                //}
                DataTable dtlott = new DataTable();
                dtlott = GetLottable(ProductID, page, objname, orderID); 
                if (dtlott.Rows.Count>0)
                {
                    for (int i = 0; i < dtlott.Rows.Count; i++)
                    {
                        string lott1 = dtlott.Rows[i]["Lottable1"].ToString();
                        string lott2 = dtlott.Rows[i]["Lottable2"].ToString();
                        string lott3 = dtlott.Rows[i]["Lottable3"].ToString();
                        string qty = dtlott.Rows[i]["Qty"].ToString();
                        if (page=="pickup" || page=="putin")
                        {
                            loccode = dtlott.Rows[i]["LocCode"].ToString();
                        }

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"arr_lottable\": [\n";

                        DataTable dtprdlott = new DataTable();
                        dtprdlott = GetProdLottable(ProductID);
                        if(dtprdlott.Rows.Count>0)
                        {
                            for (int j = 0; j < dtprdlott.Rows.Count; j++)
                            {
                                string lottname = dtprdlott.Rows[j]["LottableDescription"].ToString();
                                jsonString = jsonString + "{\n";

                                if (j == 0)
                                {
                                    jsonString = jsonString + "\"LottableName\": \"" + CheckString(lottname.Trim()) + "\",\n";
                                    jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(lott1.Trim()) + "\"\n";
                                }
                                if (j == 1 && page == "pickup")
                                {
                                    jsonString = jsonString + "\"LottableName\": \"LOCATION\",\n";
                                    jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(loccode.Trim()) + "\"\n";
                                    if (islottable == "No")
                                    {
                                        jsonString = jsonString + "},\n";
                                        jsonString = jsonString + "{\n";
                                        jsonString = jsonString + "\"LottableName\": \"QTY\",\n";
                                        jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(qty.Trim()) + "\"\n";
                                    }
                                    jsonString = jsonString + "},\n";
                                    jsonString = jsonString + "{\n";
                                }
                                if (j == 1 && page == "putin")
                                {
                                    jsonString = jsonString + "\"LottableName\": \"LOCATION\",\n";
                                    jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(loccode.Trim()) + "\"\n";
                                    jsonString = jsonString + "},\n";
                                    jsonString = jsonString + "{\n";
                                }
                                if (j == 1)
                                {
                                    jsonString = jsonString + "\"LottableName\": \"" + CheckString(lottname.Trim()) + "\",\n";
                                    jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(lott2.Trim()) + "\"\n";
                                }
                                if (j == 2)
                                {
                                    jsonString = jsonString + "\"LottableName\": \"" + CheckString(lottname.Trim()) + "\",\n";
                                    if (CheckString(lottname.Trim()).ToUpper() == "GRADE")
                                    {
                                        jsonString = jsonString + "\"Lottablevalue\":[\n";
                                        DataTable dtGrade = new DataTable();
                                        dtGrade = GetWareGrade(WarehouseID);
                                        for (int k = 0; k < dtGrade.Rows.Count; k++)
                                        {
                                            string grade = dtGrade.Rows[k]["Grade"].ToString();
                                            jsonString = jsonString + "{\n";
                                            jsonString = jsonString + "\"ddOption\": \"" + CheckString(grade.Trim()) + "\",\n";
                                            jsonString = jsonString + "\"ddvalue\": \"" + CheckString(grade.Trim()) + "\",\n";
                                            if (lott3 == grade)
                                            {
                                                jsonString = jsonString + "\"ddIsSelected\": \"yes\"\n";
                                            }
                                            else
                                            {
                                                jsonString = jsonString + "\"ddIsSelected\": \"no\"\n";
                                            }
                                            if (k == dtGrade.Rows.Count - 1)
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
                                        jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(lott2.Trim()) + "\"\n";
                                    }
                                }
                                if (j == dtprdlott.Rows.Count - 1)
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
                            jsonString = jsonString + "{\n";
                            jsonString = jsonString + "\"LottableName\": \"SERIAL NO\",\n";
                            jsonString = jsonString + "\"Lottablevalue\": \"0\"\n";
                            jsonString = jsonString + "},\n";
                            jsonString = jsonString + "{\n";
                            jsonString = jsonString + "\"LottableName\": \"LOCATION\",\n";
                            jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(loccode.Trim()) + "\"\n";
                            jsonString = jsonString + "},\n";
                            jsonString = jsonString + "{\n";
                            jsonString = jsonString + "\"LottableName\": \"QTY\",\n";
                            jsonString = jsonString + "\"Lottablevalue\": \"" + CheckString(qty.Trim()) + "\"\n";
                            jsonString = jsonString + "}]\n";
                        }
                        if (i == dtlott.Rows.Count - 1)
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
            { Login.Profile.ErrorHandling(ex, "view_lottables", "ProcessRequest"); }
            finally
            { }
        }

        public DataTable GetProductdetailPutIn(long prodid, long oid, string obj)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetProductDetailforPutIn ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetProductdetail"); }
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

        public string GetLocCode(long oid, long prodid, string obj,string pg)
        {
            string LocCode = "";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetProdLoc";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@page", pg);
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    LocCode = dt1.Rows[0]["LocCode"].ToString();
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetLocCode"); }
            finally
            { }
            return LocCode;
        }
        public string GetGRNQty(string obj, long oid, long prodid,string pg)
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
                if (dt1.Rows.Count > 0)
                {
                    GRNQty = dt1.Rows[0]["GRNQty"].ToString();
                }
                if(GRNQty=="")
                {
                    GRNQty = "0.00";
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetGRNQty"); }
            finally
            { }
            return GRNQty;
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }

        public DataTable GetProductdetail(long prodid,long oid,string obj)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetProductDetailforGRN ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetProductdetail"); }
            finally
            { }
            return dt;
        }

        public string IsProdLottable(long prodid,string pg)
        {
            string result = "No";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_IsProdLottable ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@page", pg);
                cmd1.Connection.Open();
                int count=Convert.ToInt32(cmd1.ExecuteScalar());
                cmd1.Connection.Close();
                if(count>0)
                {
                    result = "Yes";
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "IsProdLottable"); }
            finally
            { }
            return result;
        }

        public DataTable GetLottable(long prodid,string page,string obj,long oid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetLottables";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                cmd1.Parameters.AddWithValue("@page", page);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@oid", oid);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetLottable"); }
            finally
            { }
            return dt;
        }

        public DataTable GetProdLottable(long prodid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetProdLottable";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@prdid", prodid);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetProdLottable"); }
            finally
            { }
            return dt;
        }

        public DataTable GetWareGrade(long wareid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetWareGrade";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                cmd1.Parameters.AddWithValue("@wareid", wareid);
                da1.Fill(dt);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "view_lottables", "GetWareGrade"); }
            finally
            { }
            return dt;
        }
    }
}