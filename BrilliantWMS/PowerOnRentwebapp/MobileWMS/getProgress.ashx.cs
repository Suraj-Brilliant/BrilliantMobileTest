using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;
using System.Web.SessionState;
using static System.Web.SessionState.IRequiresSessionState;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for getProgress
    /// </summary>
    public class getProgress : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long oid, wid, uid;
        string objname="",page="";
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["oid"] != null)
            {
                oid = Convert.ToInt64(context.Request.QueryString["oid"]);
            }
            if (context.Request.QueryString["wrid"] != null)
            {
                wid = Convert.ToInt64(context.Request.QueryString["wrid"]);
            }
            if (context.Request.QueryString["uid"] != null)
            {
                uid = Convert.ToInt64(context.Request.QueryString["uid"]);
            }
            if (context.Request.QueryString["objname"] != null)
            {
                objname = context.Request.QueryString["objname"].ToString();
            }
            if (context.Request.QueryString["page"] != null)
            {
                page = context.Request.QueryString["page"].ToString();
            }

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;

            string OrderID = oid.ToString();
            string WareID = wid.ToString();
            string UserID = uid.ToString();
            string CurrentCnt = "";
            try
            {
                jsonString = "{\n\"progress\": [{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"oid\": \"" + OrderID.Trim() + "\",\n";
                jsonString = jsonString + "\"wid\": \"" + WareID.Trim() + "\",\n";
                jsonString = jsonString + "\"uid\": \"" + UserID.Trim() + "\",\n";
                jsonString = jsonString + "\"objname\": \"" + CheckString(objname.Trim()) + "\",\n";
                CurrentCnt = GetCurrentCount(oid, objname,page).ToString();
                jsonString = jsonString + "\"currentCount\": \"" + CurrentCnt.Trim() + "\"\n";
                jsonString = jsonString + "}]\n}";  /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            {
                jsonString = String.Empty;
                jsonString = "{\n\"progress\": [{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"oid\": \"" + OrderID.Trim() + "\",\n";
                jsonString = jsonString + "\"wid\": \"" + WareID.Trim() + "\",\n";
                jsonString = jsonString + "\"uid\": \"" + UserID.Trim() + "\",\n";
                jsonString = jsonString + "\"objname\": \"" + CheckString(objname.Trim()) + "\",\n";
                jsonString = jsonString + "\"currentCount\": \"na\"\n";
                jsonString = jsonString + "}]\n}";  /*json Loop End*/
                context.Response.Write(jsonString);
            }
            finally
            {
            }
        }

        private decimal GetCurrentCount(long oid,string objname,string page)
        {
            decimal Qty = 0;
            //int ActQty = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "SP_GetCurrentStatus";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@oid", oid);
            cmd1.Parameters.AddWithValue("@objname", objname);
            cmd1.Parameters.AddWithValue("@page", page);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                Qty = Convert.ToDecimal(ds1.Tables[0].Rows[0]["PickUpQty"].ToString());
                //ActQty = (int)Qty;
            }
            return Qty;
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