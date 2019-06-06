using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for get_address
    /// </summary>
    public class get_address : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            //String dptId = context.Request.QueryString["dptId"];
            String cmpId = context.Request.QueryString["cmpId"];

            String myQueryString = "select * from tAddress where CompanyId=" + cmpId + "";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = myQueryString;
            cmd.Connection = conn;
            cmd.Parameters.Clear();

            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            if (cntr > 0)
            {
                for (int i = 0; i < cntr; i++)
                {
                    long CID = Convert.ToInt64(ds.Tables[0].Rows[i]["ID"].ToString());
                    string Cname = ds.Tables[0].Rows[i]["AddressLine1"].ToString();

                    xmlString = xmlString + "<addressitem>";
                    xmlString = xmlString + "<adid>" + CID + "</adid>";
                    xmlString = xmlString + "<addressname>" + CheckString(Cname) + "</addressname>";
                    xmlString = xmlString + "</addressitem>";
                }
            }

            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
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