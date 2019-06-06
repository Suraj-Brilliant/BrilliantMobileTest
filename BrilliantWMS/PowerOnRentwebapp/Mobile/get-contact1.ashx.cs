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
    /// Summary description for get_contact
    /// </summary>
    public class get_contact : IHttpHandler
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

          //  String dptId = context.Request.QueryString["dptId"];
            String cmpId = context.Request.QueryString["cmpId"];

            String myQueryString = "select * from tContactPersonDetail where CompanyID=" + cmpId + "";           
             cmd.CommandType = CommandType.Text;
            cmd.CommandText =  myQueryString;
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
                    string Cname = ds.Tables[0].Rows[i]["Name"].ToString();

                    xmlString = xmlString + "<contactitem>";
                    xmlString = xmlString + "<conid>" + CID + "</conid>";
                    xmlString = xmlString + "<contactname>" + Cname + "</contactname>";
                    xmlString = xmlString + "</contactitem>";
                }
            }

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