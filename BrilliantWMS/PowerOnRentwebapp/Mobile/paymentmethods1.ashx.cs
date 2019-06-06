using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for paymentmethods
    /// </summary>
    public class paymentmethods : IHttpHandler
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
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from mPaymentMethodMain";
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cnt = dt.Rows.Count;
                                              
               
            context.Response.ContentType = "text/xml";
            String xmlString = String.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
            
            if (cnt > 0)
            {
                for (int i = 0; i <= cnt - 1; i++)
                {
                    string pmId = dt.Rows[i]["ID"].ToString();


                    string paymentMethod = dt.Rows[i]["MethodName"].ToString();

                    // string PMstring = GetPaymentMethod();
                    xmlString = xmlString + "<paymentType>";    
                    xmlString = xmlString + "<pmId>" + pmId + "</pmId>";
                    xmlString = xmlString + "<paymentMethod>" + paymentMethod + "</paymentMethod>";
                    xmlString = xmlString + "</paymentType>"; 
                }
            }
                       
            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
        }



        //public string GetPaymentMethod()
        //{
        //   String pm = "",FilterPM = "";
        //    SqlConnection conn = new SqlConnection(strcon);
        //    cmd.Connection = conn;
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "select * from mPaymentMethodMain";
        //    da.SelectCommand = cmd;
        //    da.Fill(ds, "tbl1");
        //    dt = ds.Tables[0];
        //    int cnt = dt.Rows.Count;
        //    if (cnt > 0)
        //    {
        //        for (int i = 0; i <= cnt - 1; i++)
        //        {
        //            string pmId = dt.Rows[i]["ID"].ToString();
        //            pm=pm+pmId;
        //            pm = pm + ":";
        //            string paymentMethod = dt.Rows[i]["MethodName"].ToString();
        //            pm=pm+paymentMethod;
        //            pm = pm + ":";
        //        }
        //        FilterPM = pm.Substring(0, (pm.Length - 1));
        //    }
        //    return FilterPM;

        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}