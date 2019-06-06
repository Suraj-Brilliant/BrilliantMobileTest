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
    /// Summary description for location_validation
    /// </summary>
    public class location_validation : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        long locationID = 0;
        string locationCode = "";


        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        long WarehouseID = 0, orderID = 0; string barcode = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;

            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            barcode = context.Request.QueryString["loccode"].ToString();

            jsonString = "{\n\"locationvalidation\": [\n";   /*json Loop Start*/

            if (context.Request.QueryString["wrid"] != null && context.Request.QueryString["loccode"] != null)
            {
                jsonString = jsonString + "{\n";
                string result = ValidLocation();
                jsonString = jsonString + "\"loccode\": \"" + locationCode + "\",\n";
                jsonString = jsonString + "\"locid\": \"" + locationID.ToString() + "\",\n";
                jsonString = jsonString + "\"result\": \"" + result.Trim() + "\"\n";
                jsonString = jsonString + "}\n";
            }


            jsonString = jsonString + "]\n}";
            context.Response.Write(jsonString);
        }

        //private string ValidLocation()
        //{
        //    SqlCommand cmd1 = new SqlCommand();
        //    SqlDataAdapter da1 = new SqlDataAdapter();
        //    DataSet ds1 = new DataSet();
        //    DataTable dt1 = new DataTable();
        //    SqlDataReader dr1;
        //    int count = 0;
        //    string result = "";

        //    SqlConnection conn = new SqlConnection(strcon);
        //    cmd1.CommandType = CommandType.Text;
        //    cmd1.CommandText = "select Count(*) from mlocation where warehouseid=" + WarehouseID + " and Code='" + barcode + "'";
        //    cmd1.Connection = conn;
        //    cmd1.Parameters.Clear();
        //    cmd1.Connection.Open();
        //    count = Convert.ToInt32(cmd1.ExecuteScalar());
        //    cmd1.Connection.Close();
        //    if (count > 0)
        //    {
        //        result = "success";
        //    }
        //    else
        //    {
        //        result = "failed";
        //    }
        //    return result;
        //}

        private string ValidLocation()
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            SqlDataReader dr1;
            int count = 0;
            string result = "";

            SqlConnection conn = new SqlConnection(strcon);
            cmd1.CommandType = CommandType.Text;
            //  cmd1.CommandText = "select Count(*) from mlocation where warehouseid=" + WarehouseID + " and Code='" + barcode + "'";
            cmd1.CommandText = "select top 1 * from mlocation where warehouseid=" + WarehouseID + " and Code='" + barcode + "' order by ID desc";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Connection.Open();
            // count = Convert.ToInt32(cmd1.ExecuteScalar());            

            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            count = dt1.Rows.Count;

            cmd1.Connection.Close();
            if (count > 0)
            {
                locationCode = ds1.Tables[0].Rows[0]["Code"].ToString();
                locationID = long.Parse(ds1.Tables[0].Rows[0]["ID"].ToString());
                result = "success";
            }
            else
            {
                result = "failed";
            }
            return result;
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