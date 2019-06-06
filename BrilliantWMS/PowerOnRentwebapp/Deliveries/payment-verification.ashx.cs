using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.IO;

namespace BrilliantWMS.Deliveries
{
    /// <summary>
    /// Summary description for payment_verification
    /// </summary>
    public class payment_verification : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd1 = new SqlCommand();
        SqlDataAdapter da1 = new SqlDataAdapter();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            string OdrID = context.Request.QueryString["odrId"];
            string CardNo = context.Request.QueryString["cardno"];
            string Paymentmode = context.Request.QueryString["paymentmode"];


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

            if (dt3.Rows.Count > 0)
            {
                long OrderId = Convert.ToInt64(dt3.Rows[0]["Id"].ToString());

                if (context.Request.QueryString["odrId"] != null)
                {
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    //if (Paymentmode == "Card")
                    //{
                    cmd.CommandText = "select CardNo from mPaymentDetail where OrderId=" + OrderId + "";

                    //}
                    //else
                    //{
                    //    cmd.CommandText = "select StatutoryValue from tStatutoryDetail where ReferenceID=" + OrderId + " and StatutoryID=8";
                    //}

                    string crdno = "";
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tbl1");
                    dt = ds.Tables[0];
                    int cnt = dt.Rows.Count;

                    context.Response.ContentType = "text/xml";
                    string xmlString = string.Empty;
                    xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                    xmlString = xmlString + "<gwcInfo>";
                    xmlString = xmlString + "<paymentverification>";
                    if (cnt > 0)
                    {
                        //if (Paymentmode == "Card")
                        //{
                        crdno = dt.Rows[0]["CardNo"].ToString();
                        string CardFourDNumber = crdno.Substring(crdno.Length - 4, 4);

                        if (CardFourDNumber == CardNo)
                        {

                            string verify = "Verified";
                            cmd1.CommandType = CommandType.Text;

                            cmd1.CommandText = "update mPaymentDetail set Card_Verified='" + verify + "' where OrderId=" + OrderId + "";
                            cmd1.Connection = conn;
                            cmd1.Parameters.Clear();
                            da1.SelectCommand = cmd1;
                            da1.Fill(ds1, "tbl2");
                            xmlString = xmlString + "<status>Verified</status>";
                        }
                        else
                        {
                            xmlString = xmlString + "<status>failed</status>";
                        }

                        //}
                        //else if (Paymentmode == "Credit Card")
                        //{
                        //     crdno = dt.Rows[0]["StatutoryValue"].ToString();
                        //    if (crdno == CardNo)
                        //    {
                        //        xmlString = xmlString + "<status>Verified</status>";
                        //    }
                        //    else
                        //    {
                        //        xmlString = xmlString + "<status>failed</status>";
                        //    }
                        //}                   

                    }
                    else if (cnt == 0)
                    {
                        xmlString = xmlString + "<status>failed</status>";
                    }

                    xmlString = xmlString + "</paymentverification>";
                    xmlString = xmlString + "</gwcInfo>";
                    context.Response.Write(xmlString);
                }
            }
            else
            {
                context.Response.ContentType = "text/xml";
                string xmlString = string.Empty;
                xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xmlString = xmlString + "<gwcInfo>";
                xmlString = xmlString + "<paymentverification>";
                xmlString = xmlString + "<status>failed</status>";
                xmlString = xmlString + "</paymentverification>";
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