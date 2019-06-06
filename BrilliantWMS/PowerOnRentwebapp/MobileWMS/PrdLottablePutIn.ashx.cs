using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using BrilliantWMS.Login;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for PrdLottablePutIn
    /// </summary>
    public class PrdLottablePutIn : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string prdBarcode = "";
        long UserID = 0, Oid = 0;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            string Chklottable = "";
            prdBarcode = context.Request.QueryString["barcode"];
            UserID = long.Parse(context.Request.QueryString["user_id"]);
            Oid = long.Parse(context.Request.QueryString["oid"]);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;

            long CustomerID, CompanyID;
            DataSet dsUserDetail = new DataSet();
            dsUserDetail = GetUserDetails(UserID);
            CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
            CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

            long PrdID = 0;
            PrdID = GetPrdID(prdBarcode, CompanyID, CustomerID);
            //barcode not match
            if (PrdID == 0)
            {
                PrdID = GetPrdIDNew(prdBarcode, CompanyID, CustomerID);
            }

            //Get Product all Details.
            DataSet dsProdDetails = new DataSet();
            string ProductName = "", ProductDess = "", ProductCode = "";
            dsProdDetails = GetProductDetails(PrdID);
            if (dsProdDetails.Tables[0].Rows.Count > 0)
            {
                ProductName = dsProdDetails.Tables[0].Rows[0]["Name"].ToString();
                ProductDess = dsProdDetails.Tables[0].Rows[0]["Description"].ToString();
                ProductCode = dsProdDetails.Tables[0].Rows[0]["ProductCode"].ToString();
            }
            string date = "";

            //get lottable description
            string LottableDescription = "", LottableDescription1 = "", LottableDescription2 = "";
            DataSet dslottable = new DataSet();
            dslottable = GetLottableDescription(PrdID);
            int counter = dslottable.Tables[0].Rows.Count;
            if (counter > 0)
            {
                LottableDescription = dslottable.Tables[0].Rows[0]["LottableDescription"].ToString();
                if (counter > 1)
                {
                    LottableDescription1 = dslottable.Tables[0].Rows[1]["LottableDescription"].ToString();
                }
                if (counter > 2)
                {
                    LottableDescription2 = dslottable.Tables[0].Rows[2]["LottableDescription"].ToString();
                }
            }

            //jsonString = "{\"product_id\":\"" + PrdID + "\",";
            jsonString = "{\n \"product_id\":\"" + PrdID + "\",\"product_name\":\"" + ProductName + "\",\"product_description\":\"" + ProductDess + "\",\"product_code\":\"" + ProductCode + "\", \n";
            cmd.CommandType = CommandType.Text;
            // cmd.CommandText = "SELECT ID, ProductID, LottableTitle, LottableDescription, Sequence, LottableFormat, Active,'' as LottableValue  FROM  mProductLottable where ProductID=" + PrdID + "";
            cmd.CommandText = "select * from tskutransactionhistory where finalzone='QC' and oid=" + Oid + " and lottable1='" + prdBarcode + "'";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;
            if (cntr > 0)
            {
                jsonString = jsonString + "\n \"is_have_lottable\":\"Yes\",\n";
                jsonString = jsonString + "\"arr_lottable\":[\n";
                //for (int i = 0; i <= cntr - 1; i++)
                //{
               // date = prdBarcode.Substring(9, 4);


                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"LottableName\": \"" + LottableDescription.Trim() + "\",\n";
                jsonString = jsonString + "\"Lottablevalue\": \"" + Convert.ToString(ds.Tables[0].Rows[0]["Lottable1"]) + "\"\n";
                jsonString = jsonString + "},\n";

                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"LottableName\": \"" + LottableDescription1.Trim() + "\",\n";
                jsonString = jsonString + "\"Lottablevalue\": \"" + Convert.ToString(ds.Tables[0].Rows[0]["Lottable2"]) + "\"\n";
                jsonString = jsonString + "},\n";

                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"LottableName\": \"" + LottableDescription2.Trim() + "\",\n";
                jsonString = jsonString + "\"Lottablevalue\": \"" + Convert.ToString(ds.Tables[0].Rows[0]["Lottable3"]) + "\"\n";
                jsonString = jsonString + "}\n";

                // }
            }
            else
            {
                jsonString = jsonString + "\n \"is_have_lottable\":\"No\",\n";
                jsonString = jsonString + "\"arr_lottable\":[\n";
            }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        private DataSet GetLottableDescription(long prdID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "SELECT ID, ProductID, LottableTitle, LottableDescription, Sequence, LottableFormat, Active,'' as LottableValue  FROM  mProductLottable where ProductID=" + prdID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        private long GetPrdIDNew(string prdBarcode, long companyID, long customerID)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select ID from mProduct where ProductCode='" + prdBarcode + "' and CompanyID=" + companyID + " and CustomerID=" + customerID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                id = Convert.ToInt64(ds1.Tables[0].Rows[0]["ID"].ToString());
            }
            return id;
        }


        private DataSet GetProductDetails(long prdID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from mproduct where id =" + prdID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
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

        public long GetPrdID(string prdBarcode, long CompanyID, long CustomerID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();
            long PrdID = 0;
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "SP_GetGRNBarcode";
            cmd2.Connection = conn;
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@Barcode", prdBarcode);
            cmd2.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd2.Parameters.AddWithValue("@CustomerID", CustomerID);
            da2.SelectCommand = cmd2;
            da2.Fill(ds2, "tbl1");
            dt2 = ds2.Tables[0];
            if (ds2.Tables[0].Rows.Count > 0)
            {
                PrdID = long.Parse(ds2.Tables[0].Rows[0]["ID"].ToString());
            }
            return PrdID;
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