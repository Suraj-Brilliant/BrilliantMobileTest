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
    /// Summary description for get_product_billofmaterial
    /// </summary>
    public class get_product_billofmaterial : IHttpHandler
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

            String prdId = context.Request.QueryString["prdId"];

            String myQueryString = "select BD.SKUId,MP.ProductCode,MP.Name,MP.Description,MP.PrincipalPrice,MP.moq,BD.Quantity,img.Path from mBOMDetail BD left outer join mProduct MP on BD.SKUId=MP.ID left outer join tImage img on  BD.SKUId=img.ReferenceID where BD.BOMheaderId=" + prdId + "";
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
                    string ProductCode = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                    string Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    string Description = ds.Tables[0].Rows[i]["Description"].ToString();
                    decimal Quantity = Convert.ToDecimal(ds.Tables[0].Rows[i]["Quantity"].ToString());
                    string imgPath = ds.Tables[0].Rows[i]["Path"].ToString();
                    String price = dt.Rows[i]["PrincipalPrice"].ToString();
                    String moq = dt.Rows[i]["moq"].ToString();

                    xmlString = xmlString + "<bomItem>";
                    xmlString = xmlString + "<productCode>" + ProductCode + "</productCode>";
                    xmlString = xmlString + "<productName>" + CheckString(Name) + "</productName>";
                    xmlString = xmlString + "<productDescription>" + CheckString(Description) + "</productDescription>";
                    xmlString = xmlString + "<productQuantity>" + Quantity + "</productQuantity>";
                    //xmlString = xmlString + "<imgSrc>" + imgPath + "</imgSrc>";
                    xmlString = xmlString + "<imgSrc>get-image.ashx?imgID=" + prdId + "</imgSrc>";
                    xmlString = xmlString + "<price>" + price + "</price>";
                    xmlString = xmlString + "<MOQ>" + moq + "</MOQ>";
                    xmlString = xmlString + "</bomItem>";
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