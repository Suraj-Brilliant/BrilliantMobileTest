using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for get_document_type
    /// </summary>
    public class get_document_type : IHttpHandler
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
            //string param="DocumentType";
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "select Id,Value from mDropdownValues where Parameter='"+param+"' ";
            //cmd.Connection = conn;
            //cmd.Parameters.Clear();           
            //da.SelectCommand = cmd;
            //da.Fill(ds, "tbl");
            //dt = ds.Tables[0];
            //for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    String Id = dt.Rows[i]["Id"].ToString();
            //    String Value = dt.Rows[i]["Value"].ToString();

            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Photd ID</dtId>";
            xmlString = xmlString + "<doctypename>Photo ID</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Passport</dtId>";
            xmlString = xmlString + "<doctypename>Passport</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>LPO</dtId>";
            xmlString = xmlString + "<doctypename>LPO</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Purchase Order</dtId>";
            xmlString = xmlString + "<doctypename>Purchase Order</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Invoice</dtId>";
            xmlString = xmlString + "<doctypename>Invoice</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Signature</dtId>";
            xmlString = xmlString + "<doctypename>Signature</doctypename>";
            xmlString = xmlString + "</documentType>";
            xmlString = xmlString + "<documentType>";
            xmlString = xmlString + "<dtId>Other</dtId>";
            xmlString = xmlString + "<doctypename>Other</doctypename>";
            xmlString = xmlString + "</documentType>";

            // }
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