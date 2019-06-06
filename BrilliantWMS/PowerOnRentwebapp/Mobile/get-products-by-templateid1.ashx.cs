using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for get_products_by_templateid
    /// </summary>
    public class get_products_by_templateid : IHttpHandler
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

            long tmplId = Convert.ToInt64(context.Request.QueryString["tmplId"]);

            cmd.CommandType = CommandType.Text;
            //cmd.CommandText = " select TD.TemplateHeadID,TD.PrdId,TD.UOMID,TD.Qty,  MP.ProductCode,MP.Name,MP.Description,um.Description UOM from mRequestTemplateDetail TD  left outer join mProduct MP on TD.PrdId=MP.ID left outer join muom um on TD.UOMID=um.ID where TD.TemplateHeadID="+ tmplId +"";
            cmd.CommandText = " select TD.TemplateHeadID,TD.PrdId,TD.UOMID,TD.Qty,  MP.ProductCode,MP.Name,case when MP.OMSSKUCode is NULL then 'N/A' else MP.OMSSKUCode  End  OMSSKUCode,MP.PrincipalPrice,MP.moq,psd.AvailableBalance,im.Path,MP.GroupSet,um.Description UOM,MT.PriceChange from mRequestTemplateDetail TD  left outer join mProduct MP on TD.PrdId=MP.ID left outer join muom um on TD.UOMID=um.ID left outer join timage im on TD.PrdId=im.ReferenceID left outer join tproductstockdetails psd on TD.PrdId=psd.ProdID right outer join mTerritory MT on MP.StoreId=MT.ID where TD.TemplateHeadID=" + tmplId + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            /*cmd.Parameters.AddWithValue("SiteIDs", 0);
            cmd.Parameters.AddWithValue("UserID", uId);*/
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
                    string iseditableprice = "";
                    String prId = dt.Rows[i]["PrdId"].ToString();
                    string Prod_Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    string OMSSKUCode = ds.Tables[0].Rows[i]["OMSSKUCode"].ToString();
                    string Prod_Code = ds.Tables[0].Rows[i]["ProductCode"].ToString();
                    String stock = dt.Rows[i]["AvailableBalance"].ToString();
                    String price = dt.Rows[i]["PrincipalPrice"].ToString();
                    String moq = dt.Rows[i]["moq"].ToString();
                    String imgSrc = dt.Rows[i]["Path"].ToString();
                    String bomPrd = dt.Rows[i]["GroupSet"].ToString();
                    string OrderQty = ds.Tables[0].Rows[i]["Qty"].ToString();
                    string UOM = ds.Tables[0].Rows[i]["UOM"].ToString();
                    string pricechange = dt.Rows[i]["PriceChange"].ToString();
                    if (pricechange == "True")
                    {
                        iseditableprice = "Yes";
                    }
                    else if (pricechange == "False")
                    {
                        iseditableprice = "No";
                    }
                    string uomstring = GetUOM(prId);

                    xmlString = xmlString + "<templateProdDetails>";
                    xmlString = xmlString + "<prId>" + prId + "</prId>";
                    xmlString = xmlString + "<itemTitle>" + Prod_Code + "</itemTitle>";
                    xmlString = xmlString + "<productTitle>" + CheckString(Prod_Name) + "</productTitle>";
                    xmlString = xmlString + "<skuCode>" + OMSSKUCode + "</skuCode>";
                    xmlString = xmlString + "<price>" + price + "</price>";
                    xmlString = xmlString + "<iseditableprice>" + iseditableprice + "</iseditableprice>";
                    xmlString = xmlString + "<stock>" + stock + "</stock>";
                  //  xmlString = xmlString + "<imgSrc>" + imgSrc + "</imgSrc>";
                    xmlString = xmlString + "<imgSrc>get-image.ashx?imgID=" + prId + "</imgSrc>";
                    xmlString = xmlString + "<prodQty>" + OrderQty + "</prodQty>";
                    xmlString = xmlString + "<selectedProdUnit>" + UOM + "</selectedProdUnit>";
                    xmlString = xmlString + "<prodUnit>" + uomstring + "</prodUnit>";
                    xmlString = xmlString + "<isBom>" + bomPrd + "</isBom>";
                    xmlString = xmlString + "<minimumorderqty>" + moq + "</minimumorderqty>";
                    xmlString = xmlString + "</templateProdDetails>";
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

        protected string GetUOM(string prId)
        {
            string uom = "", FilterUOM = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SqlDataReader dr1;

            String myQueryString1 = "select * from VW_SkuUOMDetails where SkuId=" + prId + "";
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = myQueryString1;
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl5");
            dt1 = ds1.Tables[0];
            int cntr = dt1.Rows.Count;
            if (cntr > 0)
            {
                for (int i = 0; i <= cntr - 1; i++)
                {
                    string Dscr = dt1.Rows[i]["Description"].ToString();
                    if (Dscr == "" || Dscr == "NULL") { }
                    else
                    {
                        uom = uom + Dscr;
                        uom = uom + ":";
                        string uId = dt1.Rows[i]["UOMID"].ToString();
                        uom = uom + uId;
                        uom = uom + ":";
                        string qty = dt1.Rows[i]["Quantity"].ToString();
                        uom = uom + qty;
                        uom = uom + ":";
                    }
                }
                FilterUOM = uom.Substring(0, (uom.Length - 1));
            }
            return FilterUOM;
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