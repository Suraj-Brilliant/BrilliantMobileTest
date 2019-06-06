using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for getLocationBySkuBarcode
    /// </summary>
    public class getLocationBySkuBarcode : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        long WarehouseID = 0, UserID = 0;
        string BarCode = "";


        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            UserID = long.Parse(context.Request.QueryString["user_id"]);
            BarCode = context.Request.QueryString["barcode"].ToString();

            DataSet dsUserDetail = new DataSet();
            dsUserDetail = GetUserDetails(UserID);
            long CustomerID, CompanyID;
            CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
            CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

            //jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "exec SP_ProductDetails '" + BarCode.Trim() + "'," + CompanyID + "," + CustomerID + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;
            if (cntr > 0)
            {
                for (int i = 0; i <= cntr - 1; i++)
                {
                    string id = dt.Rows[i]["ID"].ToString();
                    string productName = dt.Rows[i]["Name"].ToString();
                    string productCode = dt.Rows[i]["ProductCode"].ToString();
                    string desc = dt.Rows[i]["Description"].ToString();

                    jsonString = jsonString + "{\n";
                    jsonString = jsonString + "\"product_id\": \"" + id.Trim() + "\",\n";
                    jsonString = jsonString + "\"product_name\": \"" + CheckString(productName.Trim()) + "\",\n";
                    jsonString = jsonString + "\"product_description\": \"" + CheckString(desc.Trim()) + "\",\n";
                    jsonString = jsonString + "\"product_code\": \"" + CheckString(productCode.Trim()) + "\",\n";

                    DataSet dsLocation = new DataSet();
                    dsLocation = GetLocation(BarCode);
                    if (dsLocation.Tables[0].Rows.Count > 0)
                    {
                        string LocID = dsLocation.Tables[0].Rows[0]["ID"].ToString();
                        string Code = dsLocation.Tables[0].Rows[0]["Code"].ToString();

                        jsonString = jsonString + "\"locationcode\": \"" + CheckString(Code.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationid\": \"" + CheckString(LocID.Trim()) + "\"\n";
                    }
                }
                jsonString = jsonString + "}\n";
            }

            // jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public DataSet GetLocation(string barcode)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "WMS_SP_GetLocation";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@barcode", barcode);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }
        public DataSet GetUserDetails(long UserID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select CompanyID,CustomerID from mUserProfileHEad where id =" + UserID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }
        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
        }
    }
}