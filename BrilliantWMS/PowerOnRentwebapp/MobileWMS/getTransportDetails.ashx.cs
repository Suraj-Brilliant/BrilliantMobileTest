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
    /// Summary description for getTransportDetails
    /// </summary>
    public class getTransportDetails : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long OrderID=0,WareID=0,UserID= 0;
        string ObjName = "";
        public void ProcessRequest(HttpContext context)
        {
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(strcon);

                if (context.Request.QueryString["oid"] != null)
                {
                    OrderID = Convert.ToInt64(context.Request.QueryString["oid"]);
                }
                if (context.Request.QueryString["wrid"] != null)
                {
                    WareID = Convert.ToInt64(context.Request.QueryString["wrid"]);
                }
                if (context.Request.QueryString["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
                }
                if (context.Request.QueryString["objname"] != null)
                {
                    ObjName = context.Request.QueryString["objname"].ToString();
                }

                result=GetTransFlag(OrderID,ObjName);
                if(result=="Yes")
                {
                    DataTable dtTrans = new DataTable();
                    context.Response.ContentType = "text/plain";
                    String jsonString = String.Empty;
                    jsonString = "{\n\"transport_details\": [\n";   /*json Loop Start*/
                    jsonString = jsonString + "{\n";

                    string airwaybill ="", shippingtype ="", shippingdate ="", expecteddeliverydate ="", dockno ="", lrno ="", intime ="", outtime ="", other = "";

                    dtTrans=GetTransDetail(OrderID,ObjName);
                    if(dtTrans.Rows.Count>0)
                    {
                        if(dtTrans.Rows[0]["AirwayBill"]!=null)
                        {
                            airwaybill = dtTrans.Rows[0]["AirwayBill"].ToString();
                        }
                        if (dtTrans.Rows[0]["ShippingType"] != null)
                        {
                            shippingtype = dtTrans.Rows[0]["ShippingType"].ToString();
                        }
                        if (dtTrans.Rows[0]["ShippingDate"] != null)
                        {
                            shippingdate = dtTrans.Rows[0]["ShippingDate"].ToString();
                        }
                        if (dtTrans.Rows[0]["ExpDeliveryDate"] != null)
                        {
                            expecteddeliverydate = dtTrans.Rows[0]["ExpDeliveryDate"].ToString();
                        }
                        if (dtTrans.Rows[0]["DockNo"] != null)
                        {
                            dockno = dtTrans.Rows[0]["DockNo"].ToString();
                        }
                        if (dtTrans.Rows[0]["LRNo"] != null)
                        {
                            lrno = dtTrans.Rows[0]["LRNo"].ToString();
                        }
                        if (dtTrans.Rows[0]["InTime"] != null)
                        {
                            intime = dtTrans.Rows[0]["InTime"].ToString();
                        }
                        if (dtTrans.Rows[0]["OutTime"] != null)
                        {
                            outtime = dtTrans.Rows[0]["OutTime"].ToString();
                        }
                        if (dtTrans.Rows[0]["Other"] != null)
                        {
                            other = dtTrans.Rows[0]["Other"].ToString();
                        }

                        jsonString = jsonString + "\"airwaybill\": \"" + CheckString(airwaybill.Trim()) + "\",\n";
                        jsonString = jsonString + "\"shippingtype\": \"" + CheckString(shippingtype.Trim()) + "\",\n";
                        jsonString = jsonString + "\"shippingdate\": \"" + CheckString(shippingdate.Trim()) + "\",\n";
                        jsonString = jsonString + "\"expecteddeliverydate\": \"" + CheckString(expecteddeliverydate.Trim()) + "\",\n";
                        jsonString = jsonString + "\"dockno\": \"" + CheckString(dockno.Trim()) + "\",\n";
                        jsonString = jsonString + "\"lrno\": \"" + CheckString(lrno.Trim()) + "\",\n";
                        jsonString = jsonString + "\"intime\": \"" + CheckString(intime.Trim()) + "\",\n";
                        jsonString = jsonString + "\"outtime\": \"" + CheckString(outtime.Trim()) + "\",\n";
                        jsonString = jsonString + "\"other\": \"" + CheckString(other.Trim()) + "\"\n";

                        jsonString = jsonString + "}]\n}";  /*json Loop End*/
                        context.Response.Write(jsonString);
                    }

                }
            }
            catch (Exception ex)
            { }
            finally
            { }
        }

        public string GetTransFlag(long OID,string ObjName)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string result = "No";

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "SP_TransFlag";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@oid", OID);
            cmd1.Parameters.AddWithValue("@objname", ObjName);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if(dt1.Rows.Count>0)
            {
                if(dt1.Rows[0]["TransFlag"]!=null)
                {
                    result = dt1.Rows[0]["TransFlag"].ToString();
                }
            }
            return result;
        }

        public DataTable GetTransDetail(long OID, string ObjName)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "SP_GetTransDetail";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@oid", OID);
            cmd1.Parameters.AddWithValue("@objname", ObjName);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return dt1;
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