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
    /// Summary description for mark_status
    /// </summary>
    public class mark_status : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string orderid = ""; string status = "";
        int rslt = 0;

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            orderid =context.Request.Form["orid"];
            status = context.Request.Form["status"];
            try
            {
                //if (context.Request.Form["orid"] != null || context.Request.Form["orid"] != "")
                if(orderid != "")
                {
                    cmd.CommandType = CommandType.Text;
                    if (status == "dispatch")
                    {
                        cmd.CommandText = "update tOrderHead set Status=40 where OrderNumber in (Select Part from dbo.SplitString('" + orderid +"','|'))";  
                    }
                    else if (status == "return")
                    {
                        cmd.CommandText = "update tOrderHead set Status=48 where OrderNumber in (Select Part from dbo.SplitString('" + orderid + "','|'))"; 
                    }
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tbl1");
                                        
                    rslt = 1;                    
                }
                else { rslt = 0; }
                
            }
            catch { rslt = 0; }
            finally
            {
                String xmlString = String.Empty;
                jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
                if (rslt > 0)
                {
                    jsonString = jsonString + "{\n\"status\":\"success\"\n}\n";
                }
                else if (rslt == 0)
                {
                    jsonString = jsonString + "{\n\"status\":\"failed\"\n}\n";
                }

                jsonString = jsonString + "]\n}";  /*json Loop End*/
                context.Response.Write(jsonString);
            }
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