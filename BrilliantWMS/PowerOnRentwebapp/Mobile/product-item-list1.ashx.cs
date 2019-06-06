using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for product_item_list1
    /// </summary>
    public class product_item_list1 : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select MP.ID,MP.ProductCode,MP.Name,case when MP.OMSSKUCode is NULL then 'N/A' else MP.OMSSKUCode  End  OMSSKUCode,MP.PrincipalPrice,psd.AvailableBalance,im.Path from mproduct MP left outer join tproductstockdetails psd on MP.ID=psd.ProdID left outer join timage im on MP.ID=im.ReferenceID ";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);

            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;

            context.Response.ContentType = "text/xml";
            String xmlString = String.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            for (int i = 0; i < cntr; i++)
            {
                String prId = dt.Rows[i]["ID"].ToString();
                String itemTitle = dt.Rows[i]["ProductCode"].ToString();
                String productTitle = dt.Rows[i]["Name"].ToString();
                String skuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                String stock = dt.Rows[i]["AvailableBalance"].ToString();
                Decimal price = Convert.ToDecimal(dt.Rows[i]["PrincipalPrice"].ToString());
                String imgSrc = dt.Rows[i]["Path"].ToString();

                // if (skuCode == "") skuCode = "N/A";

                // RECORD LOOP
                xmlString = xmlString + "<productItem>";
                xmlString = xmlString + "<prId>" + prId + "</prId>";
                xmlString = xmlString + "<itemTitle>Request No. " + CheckString(itemTitle) + "</itemTitle>";
                xmlString = xmlString + "<productTitle>" + CheckString(productTitle) + "</productTitle>";
                xmlString = xmlString + "<skuCode>" + skuCode + "</skuCode>";
                xmlString = xmlString + "<stock>" + stock + "</stock>";
                xmlString = xmlString + "<price>" + price + "</price>";
              //  xmlString = xmlString + "<imgSrc>" + imgSrc + "</imgSrc>";
                xmlString = xmlString + "<imgSrc>get-image.ashx?imgID=" + prId + "</imgSrc>";

                xmlString = xmlString + "<imgSrc>" + imgSrc + "</imgSrc>";
                xmlString = xmlString + "</productItem>";
                // RECORD LOOP


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