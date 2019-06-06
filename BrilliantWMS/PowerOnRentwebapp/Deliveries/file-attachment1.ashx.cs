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
    /// Summary description for file_attachment
    /// </summary>
    public class file_attachment : IHttpHandler
    {
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            string fname;

            String OrderID = context.Request.QueryString["odrId"];
            String uploadedfilename = context.Request.QueryString["uploadfilename"];

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

                fname = Path.Combine(context.Server.MapPath("~/Deliveries/Attachment/" + OrderID + "/"), uploadedfilename);
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

        public decimal d { get; set; }
    }
}