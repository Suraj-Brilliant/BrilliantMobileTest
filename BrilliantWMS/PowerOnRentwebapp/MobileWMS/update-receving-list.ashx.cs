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
using BrilliantWMS.WMSInbound;
using System.Web.SessionState;
using static System.Web.SessionState.IRequiresSessionState;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for update_receving_list
    /// </summary>
    public class update_receving_list : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0;
      
        string objname = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long PTID = 0;
            try
            {
                if (context.Request.Form["oid"] != null)
                {
                    orderID = long.Parse(context.Request.Form["oid"]);
                }
                if (context.Request.Form["wrid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.Form["wrid"]);
                }
                if (context.Request.Form["objname"] != null)
                {
                    objname = context.Request.Form["objname"].ToString();
                }
                if (context.Request.Form["uid"] != null)
                {
                    UserID = long.Parse(context.Request.Form["uid"]);
                }
                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";
                int result = CheckOrderQty(orderID,objname);
                if(result==1)
                {
                    int save= UpdatePutInStatus(orderID,objname);
                    if(save>0)
                    {
                        jsonString = jsonString + "\"status\": \"success\",\n";
                        jsonString = jsonString + "\"reason\": \"\"\n";
                    }
                    else
                    {
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                    }
                }
                else
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"Please assign location to all SKU\"\n";
                }

                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-receving-list", "ProcessRequest"); }
            finally
            { }
        }

        public int UpdatePutInStatus(long oid,string obj)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdatePutInStatus";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-receving-list", "UpdateStatus"); }
            finally
            { }
            return result;
        }
        public int CheckOrderQty(long oid,string obj)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
            int result = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_CheckOrderQty";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dt1.Rows[0]["Result"]);
                }
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-receving-list", "CheckOrderQty"); }
            finally
            { }
            return result;
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