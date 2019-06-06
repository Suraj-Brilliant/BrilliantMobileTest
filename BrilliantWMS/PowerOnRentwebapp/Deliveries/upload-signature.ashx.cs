using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace BrilliantWMS.Deliveries
{
    /// <summary>
    /// Summary description for upload_signature
    /// </summary>
    public class upload_signature : IHttpHandler
    {
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            string fname;

            String OdrID = context.Request.QueryString["odrId"];

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

            if (context.Request.Files.Count > 0)
            {
                context.Response.ContentType = "text/xml";
                string xmlString = string.Empty;
                xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xmlString = xmlString + "<gwcInfo>";
                xmlString = xmlString + "<filedata>";
                HttpFileCollection files = context.Request.Files;

                HttpPostedFile file = files[0];

                fname = file.FileName;
                string dirattachment = context.Server.MapPath("~/Deliveries/Attachment/" + OrderID);
                if (!Directory.Exists(dirattachment))
                {
                    Directory.CreateDirectory(dirattachment);
                }

                fname = Path.Combine(context.Server.MapPath("~/Deliveries/Attachment/" + OrderID + "/"), "signature.png");
                file.SaveAs(fname);
                string AttachfileName = Path.GetFileName(fname);
                if (File.Exists(fname))
                {
                    xmlString = xmlString + "<status>uploaded</status>";
                    xmlString = xmlString + "<filename>" + AttachfileName + "</filename>";

                }
                else
                {
                    xmlString = xmlString + "<status>failed</status>";
                }
                xmlString = xmlString + "</filedata>";
                xmlString = xmlString + "</gwcInfo>";

                context.Response.Write(xmlString);
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