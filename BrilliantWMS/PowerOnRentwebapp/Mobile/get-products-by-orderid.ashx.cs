using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for get_products_by_orderid
    /// </summary>
    public class get_products_by_orderid : IHttpHandler
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

            string OdrID = context.Request.QueryString["reId"];


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

            long reId = Convert.ToInt64(dt3.Rows[0]["Id"].ToString());
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = " select OD.SkuId,OD.Prod_Name,OD.Prod_Description,OD.Prod_Code,OD.OrderQty,um.Description UOM,IM.Path,Mp.GroupSet,case when MP.PrincipalPrice is NULL then '0' else MP.PrincipalPrice End PrincipalPrice  from tOrderDetail OD left outer join tImage IM on OD.SkuId=IM.ReferenceID left outer join muom um on OD.UOMID=um.ID left outer join mProduct MP on OD.SkuId=MP.ID  where OD.OrderHeadId=" + reId + "";
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
                    string Prod_Name = ds.Tables[0].Rows[i]["Prod_Name"].ToString();
                    string Prod_Description = ds.Tables[0].Rows[i]["Prod_Description"].ToString();
                    string Prod_Code = ds.Tables[0].Rows[i]["Prod_Code"].ToString();
                    string OrderQty = ds.Tables[0].Rows[i]["OrderQty"].ToString();
                    string UOM = ds.Tables[0].Rows[i]["UOM"].ToString();
                    string Path = ds.Tables[0].Rows[i]["Path"].ToString();
                    string isBOM = ds.Tables[0].Rows[i]["GroupSet"].ToString();
                    long prID = long.Parse(ds.Tables[0].Rows[i]["SkuId"].ToString());
                    decimal Price = Convert.ToDecimal(ds.Tables[0].Rows[i]["PrincipalPrice"].ToString());
                    xmlString = xmlString + "<requestItemDetails>";
                    xmlString = xmlString + "<prodName>" + CheckString(Prod_Name) + "</prodName>";
                    xmlString = xmlString + "<prodDescription>" + CheckString(Prod_Description) + "</prodDescription>";
                    xmlString = xmlString + "<prodCode>" + Prod_Code + "</prodCode>";
                    xmlString = xmlString + "<prodQty>" + OrderQty + "</prodQty>";
                    xmlString = xmlString + "<unit>" + UOM + "</unit>";
                    xmlString = xmlString + "<price>" + Price + "</price>";
                    //xmlString = xmlString + "<imgPath>" + Path + "</imgPath>";
                    xmlString = xmlString + "<imgPath>get-image.ashx?imgID=" + prID + "</imgPath>";
                    xmlString = xmlString + "<isBom>" + isBOM + "</isBom>";
                    xmlString = xmlString + "<prId>" + prID + "</prId>";
                    xmlString = xmlString + "</requestItemDetails>";
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