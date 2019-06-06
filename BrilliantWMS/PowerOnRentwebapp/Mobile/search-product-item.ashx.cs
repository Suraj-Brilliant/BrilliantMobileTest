using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;
using BrilliantWMS.PORServicePartRequest;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for search_product_item
    /// </summary>
    public class search_product_item : IHttpHandler
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

            string skey = String.Empty;
            string sqlLimit = string.Empty;
            string dptId = context.Request.QueryString["dptId"];

            if ((context.Request.QueryString["skey"] != null) && (context.Request.QueryString["skey"] != ""))
            {
                skey = context.Request.QueryString["skey"].ToString();
            }
            else
            {
                skey = "";
            }

            String myQueryString = "select top (30)  MP.ID,MP.ProductCode,MP.Name,case when MP.OMSSKUCode is NULL then 'N/A' else MP.OMSSKUCode  End  OMSSKUCode,case when MP.PrincipalPrice is NULL then '0' else MP.PrincipalPrice End PrincipalPrice,MP.moq,psd.AvailableBalance,im.Path,MP.GroupSet,MT.PriceChange from mproduct MP left outer join tproductstockdetails psd on MP.ID=psd.ProdID left outer join timage im on MP.ID=im.ReferenceID right outer join mTerritory MT on MP.StoreId=MT.ID WHERE ";

            String myQueryStringAll = "select count(ID) CNT from( select  MP.ID,MP.ProductCode,MP.Name,case when MP.OMSSKUCode is NULL then 'N/A' else MP.OMSSKUCode  End  OMSSKUCode,case when MP.PrincipalPrice is NULL then '0' else MP.PrincipalPrice End PrincipalPrice,MP.moq,psd.AvailableBalance,im.Path,MP.GroupSet,MT.PriceChange from mproduct MP left outer join tproductstockdetails psd on MP.ID=psd.ProdID left outer join timage im on MP.ID=im.ReferenceID right outer join mTerritory MT on MP.StoreId=MT.ID WHERE ";

            int myLineCount = 0;
            string[] sResult = skey.Split(new string[] { " ", "%20" }, StringSplitOptions.None);
            foreach (string currentKey in sResult)
            {
                if (myLineCount == 0)
                {
                    myQueryString = myQueryString + "";
                    myLineCount = 1;
                    myQueryStringAll = myQueryStringAll + "";
                }
                else
                {
                    myQueryString = myQueryString + "OR ";
                    myQueryStringAll = myQueryStringAll + "OR";
                }
                myQueryString = myQueryString + "(MP.ProductCode like '%" + currentKey + "%' OR MP.Name like '%" + currentKey + "%' OR OMSSKUCode like '%" + currentKey + "%' OR MP.PrincipalPrice Like '%" + currentKey + "%' )";
                // MessageBox.Show(s);
                myQueryStringAll = myQueryStringAll + "(MP.ProductCode like '%" + currentKey + "%' OR MP.Name like '%" + currentKey + "%' OR OMSSKUCode like '%" + currentKey + "%' OR MP.PrincipalPrice Like '%" + currentKey + "%')";
            }

            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SqlDataReader dr1;
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = myQueryStringAll + " and MP.StoreId=" + dptId + ")xxx ";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl2");
            dt1 = ds1.Tables[0];
            long TotalCount = Convert.ToInt64(dt1.Rows[0]["CNT"].ToString());


            cmd.CommandType = CommandType.Text;
            cmd.CommandText = myQueryString + " and MP.StoreId=" + dptId + "";
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

            for (int i = 0; i <= cntr - 1; i++)
            {
                string iseditableprice = "";
                String prId = dt.Rows[i]["ID"].ToString();
                String itemTitle = dt.Rows[i]["ProductCode"].ToString();
                String productTitle = dt.Rows[i]["Name"].ToString();
                String skuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                String stock = dt.Rows[i]["AvailableBalance"].ToString();
                decimal price = Convert.ToDecimal(dt.Rows[i]["PrincipalPrice"].ToString());
                int moq = Convert.ToInt32(dt.Rows[i]["moq"].ToString());
                String imgSrc = dt.Rows[i]["Path"].ToString();
                String bomPrd = dt.Rows[i]["GroupSet"].ToString();
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
                // if (skuCode == "") skuCode = "N/A";

                // RECORD LOOP
                xmlString = xmlString + "<productItem>";
                xmlString = xmlString + "<prId>" + prId + "</prId>";
                xmlString = xmlString + "<itemTitle>" + CheckString(itemTitle) + "</itemTitle>";
                xmlString = xmlString + "<productTitle>" + CheckString(productTitle) + "</productTitle>";
                xmlString = xmlString + "<skuCode>" + skuCode + "</skuCode>";
                xmlString = xmlString + "<price>" + price + "</price>";
                xmlString = xmlString + "<iseditableprice>" + iseditableprice + "</iseditableprice>";
                xmlString = xmlString + "<stock>" + stock + "</stock>";
                // xmlString = xmlString + "<imgSrc>" + imgSrc + "</imgSrc>";

                xmlString = xmlString + "<imgSrc>get-image.ashx?imgID=" + prId + "</imgSrc>";
                xmlString = xmlString + "<prodUnit>" + uomstring + "</prodUnit>";
                xmlString = xmlString + "<minimumorderqty>" + moq + "</minimumorderqty>";
                xmlString = xmlString + "<isBom>" + bomPrd + "</isBom>";
                xmlString = xmlString + "</productItem>";
                // RECORD LOOP
            }
            xmlString = xmlString + "<productRecordCount>";
            xmlString = xmlString + "<valCount>" + TotalCount + "</valCount>";
            xmlString = xmlString + "</productRecordCount>";

            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
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