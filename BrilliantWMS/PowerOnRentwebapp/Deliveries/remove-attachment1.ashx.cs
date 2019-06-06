using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>
    /// Summary description for remove_attachment
    /// </summary>
    public class remove_attachment : IHttpHandler
    {
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            string fname;


            String OdrID = context.Request.Form["txtOrderId"];
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd3 = new SqlCommand();
            SqlDataAdapter da3 = new SqlDataAdapter();
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "select Id from tOrderHead where OrderNo='" + OdrID + "'";
            cmd3.Connection = conn;
            cmd3.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);
            da3.SelectCommand = cmd3;
            da3.Fill(ds3, "tbl4");
            dt3 = ds3.Tables[0];

            long OrderID = Convert.ToInt64(dt3.Rows[0]["Id"].ToString());

            String txtappsessionid = context.Request.Form["txtAppSessionId"];
          
                context.Response.ContentType = "text/xml";
                string xmlString = string.Empty;
                xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xmlString = xmlString + "<gwcInfo>";
                xmlString = xmlString + "<filedata>";

                fname = context.Server.MapPath("~/Deliveries/Attachment/" + OrderID + "/" + context.Request.Form["txtFileName"]);
                if (File.Exists(fname))
                {
                    File.Delete(fname);
                    xmlString = xmlString + "<status>deleted</status>";                   
                }
                else
                {
                    xmlString = xmlString + "<status>notexist</status>";
                }
                xmlString = xmlString + "<filename>" + fname + "</filename>";
                xmlString = xmlString + "<appsessionid>" + txtappsessionid + "</appsessionid>";
                xmlString = xmlString + "</filedata>";
                xmlString = xmlString + "</gwcInfo>";

                context.Response.Write(xmlString);
            
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