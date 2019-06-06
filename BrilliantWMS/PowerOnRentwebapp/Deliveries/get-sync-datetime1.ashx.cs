using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>
    /// Summary description for get_sync_datetime
    /// </summary>
    public class get_sync_datetime : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String jsonString = String.Empty;
            context.Response.ContentType = "text/plain";
            jsonString = "{\n\"synctime\": [\n {\"datetime\": \""+ DateTime.Now +"\"\n}]}";
            context.Response.Write(jsonString);
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