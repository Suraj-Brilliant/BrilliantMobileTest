using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for batch_code_detail
    /// </summary>
    public class batch_code_detail : IHttpHandler
    {
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string comapanyid = "", customerid = "", Batchcode = "";

        public void ProcessRequest(HttpContext context)
        {
            comapanyid = context.Request.QueryString["company_id"];
            customerid = context.Request.QueryString["customer_id"];
            if (string.IsNullOrEmpty(comapanyid)) { comapanyid = "0"; }
            if (string.IsNullOrEmpty(customerid)) { customerid = "0"; }

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"batchcode\": [\n";   /*json Loop Start*/
            Batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
            if (Batchcode == "")
            {
                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"batch_code\": \"" + "" + "\"\n";
            }
            else
            {
                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"batch_code\": \"" + Batchcode + "\"\n";
            }
            jsonString = jsonString + "}]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }


        public string GetBatchNoForGRN(long CustomerID, long CompanyID)
        {
            string Batch = "";
            string batchformat = "";
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select BatchNoFormat from mCustomer where ID='" + CustomerID + "' and ParentID='" + CompanyID + "'";
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (obj != null)
            {
                Batch = obj.ToString();
            }
            if (Batch != "")
            {
                string[] format = Batch.Split('-');
                string batchname = format[0];
                string date = format[1];
                long number = Convert.ToInt64(format[2]);
                long nextno = 0;
                if (date == "Number")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    batchformat = batchname + "-" + "01" + "-" + nextno;
                }
                if (date == "YYMM")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    string datetime = DateTime.Now.ToString("yyMM");
                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
                if (date == "YYQ")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    int month = DateTime.Now.Month;
                    string datetime = "";
                    if (month >= 1 && month <= 3) { datetime = "01"; }

                    else if (month >= 4 && month <= 6) { datetime = "02"; }

                    else if (month >= 7 && month <= 9) { datetime = "03"; }

                    else { datetime = "04"; }

                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
                if (date == "DDMMYY")
                {
                    nextno = GetNextBatchNo(CustomerID, CompanyID);
                    string datetime = DateTime.Now.ToString("ddMMyy");
                    batchformat = batchname + "-" + datetime + "-" + nextno;
                }
            }
            return batchformat;
        }

        public long GetNextBatchNo(long CustID, long CompID)
        {
            string Batch = "";
            long Number = 0;
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select top 1 BatchNo from tgrnhead where CompanyId='" + CompID + "' and CustomerID='" + CustID + "' order by ID desc ";
            cmd.Connection = con;
            cmd.Parameters.Clear();
            cmd.Connection.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (obj != null)
            {
                Batch = obj.ToString();
            }
            if (Batch != "")
            {
                string[] format = Batch.Split('-');
                long No = Convert.ToInt64(format[2]);
                Number = No + 1;
            }
            return Number;
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