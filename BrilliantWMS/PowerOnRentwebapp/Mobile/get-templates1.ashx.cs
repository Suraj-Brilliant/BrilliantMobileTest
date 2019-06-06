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
using PowerOnRentwebapp.Login;
using PowerOnRentwebapp.PORServicePartRequest;
using PowerOnRentwebapp.PORServiceUCCommonFilter;
namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for get_templates
    /// </summary>
    public class get_templates : IHttpHandler
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

            long uId =Convert.ToInt64(context.Request.QueryString["uId"]);
            String dptId = context.Request.QueryString["dptId"];

            String myQueryString = "select * from VW_GetTemplateDetails where createdBy=" + uId + " and Department=" + dptId + "  union select * from VW_GetTemplateDetails where createdBy!=" + uId + "  and Accesstype='Public' and Department=" + dptId + "";
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
                    long TID = Convert.ToInt64(ds.Tables[0].Rows[i]["ID"].ToString());
                    string Title = ds.Tables[0].Rows[i]["TemplateTitle"].ToString();

                    xmlString = xmlString + "<templateItems>";
                    xmlString = xmlString + "<teid>" + TID + "</teid>";
                    xmlString = xmlString + "<templatename>" + CheckString(Title) + "</templatename>";
                    xmlString = xmlString + "</templateItems>";
                }
            }

            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "&amp;");
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