using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.IO;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for file_attachment
    /// </summary>
    public class file_attachment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string fname;

            // String OrderID = context.Request.Form["txtOrderId"];

            String uploadedfilename = context.Request.QueryString["uploadfilename"];
            //String txtappsessionid = context.Request.Form["txtAppSessionId"];
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
                string dirattachment = context.Server.MapPath("~/Mobile/Attachment");
                if (!Directory.Exists(dirattachment))
                {
                    Directory.CreateDirectory(dirattachment);
                }

                fname = Path.Combine(context.Server.MapPath("~/Mobile/Attachment/"), uploadedfilename);
                file.SaveAs(fname);

                if (File.Exists(fname))
                {
                    xmlString = xmlString + "<status>uploaded</status>";
                    xmlString = xmlString + "<filename>" + fname + "</filename>";

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