using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PowerOnRentwebapp
{
    /// <summary>
    /// Summary description for get_payment_method
    /// </summary>
    public class get_payment_method : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
            xmlString = xmlString + "<paymentMethod>";
            long pmId = Convert.ToInt64(context.Request.QueryString["pmId"]);
            long deptid = Convert.ToInt64(context.Request.QueryString["deptId"]);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select FieldName,ControlType from mPaymentMethodDetail where PMethodID=" + pmId + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl");
            dt = ds.Tables[0];

            xmlString = xmlString + "<pmId>" + pmId + "</pmId>";
            xmlString = xmlString + "<fieldGroup>";
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                String fieldlabel = dt.Rows[i]["FieldName"].ToString();
                String fieldtype = dt.Rows[i]["ControlType"].ToString();
                if (fieldtype == "TextBox")
                {
                 
                    xmlString = xmlString + "<fieldInfo>";
                    xmlString = xmlString + "<fieldlabel>" + fieldlabel + "</fieldlabel>";
                    xmlString = xmlString + "<fieldtype>Text</fieldtype>";
                    xmlString = xmlString + "</fieldInfo>";
                   
                    
                   
                }
                else if (fieldtype == "DropDownBox")
                {
                  
                    xmlString = xmlString + "<fieldInfo>";
                    xmlString = xmlString + "<fieldlabel>" + fieldlabel + "</fieldlabel>";
                    xmlString = xmlString + "<fieldtype>DropDown</fieldtype>";
                  

                   

                    SqlCommand cmd2 = new SqlCommand();
                    SqlDataAdapter da2 = new SqlDataAdapter();
                    DataSet ds2 = new DataSet();
                    DataTable dt2 = new DataTable();
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "select * from mTerritory where ID=" + deptid + "";
                    cmd2.Connection = conn;
                    cmd2.Parameters.Clear();

                    da2.SelectCommand = cmd2;
                    da2.Fill(ds2, "tbl2");
                    dt2 = ds2.Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        long companyid = Convert.ToInt64(dt2.Rows[0]["ParentID"].ToString());
                        SqlCommand cmd1 = new SqlCommand();
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        DataSet ds1 = new DataSet();
                        DataTable dt1 = new DataTable();
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = "select * from mcostcentermain where CompanyID=" + companyid + "";
                        cmd1.Connection = conn;
                        cmd1.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("param1", ResourceId);
                        da1.SelectCommand = cmd1;
                        da1.Fill(ds1, "tbl1");
                        dt1 = ds1.Tables[0];
                      
                        xmlString = xmlString + "<DropDownValueGroup>";
                        for (int j = 0; j <= dt1.Rows.Count - 1; j++)
                        {
                            string centername = dt1.Rows[j]["CenterName"].ToString();
                            long id = Convert.ToInt64(dt1.Rows[j]["ID"].ToString());
                            xmlString = xmlString + "<ddOption>";
                            xmlString = xmlString + "<ddLabel>" + CheckString(centername) + "</ddLabel>";
                            xmlString = xmlString + "<ddValue>" + id + "</ddValue>";
                            xmlString = xmlString + "</ddOption>";
                        }
                        xmlString = xmlString + "</DropDownValueGroup>";
                        
                        
                    }
                    xmlString = xmlString + "</fieldInfo>";
                }


            }
            xmlString = xmlString + "</fieldGroup>";
            xmlString = xmlString + "</paymentMethod>";
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