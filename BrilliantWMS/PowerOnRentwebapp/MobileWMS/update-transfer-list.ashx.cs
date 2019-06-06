using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.CycleCountService;
using BrilliantWMS.Login;
using BrilliantWMS.WarehouseService;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for update_transfer_list
    /// </summary>
    public class update_transfer_list : IHttpHandler
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
            CustomProfile profile = CustomProfile.GetProfile();

            long wid = long.Parse(context.Request.Form["wid"]);
            long transferID = long.Parse(context.Request.Form["oid"]);
            long userID = long.Parse(context.Request.Form["uid"]);
            long mySeq = 0;

            SqlCommand cmd6 = new SqlCommand();
            SqlDataAdapter da6 = new SqlDataAdapter();
            DataSet ds6 = new DataSet();
            DataTable dt6 = new DataTable();
            cmd6.CommandType = CommandType.Text;
            cmd6.CommandText = "select Status from tInternalTransferHead where ID=" + transferID + "";
            cmd6.Connection = conn;
            cmd6.Parameters.Clear();
            da6.SelectCommand = cmd6;
            da6.Fill(ds6, "tbl1");
            dt6 = ds6.Tables[0];
            long transferStatus = long.Parse(dt6.Rows[0]["Status"].ToString());
            if (transferStatus == 56)
            {
                if (transferID > 0)
                {
                    string transferDetails = context.Request.Form["transferDetails"];
                    string myquerystring = "select * from  SplitString('" + transferDetails + "',':')";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = myquerystring;
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
                            long skuid = long.Parse(ds.Tables[0].Rows[i]["part"].ToString());
                            i = i + 1;
                            decimal qty = decimal.Parse(ds.Tables[0].Rows[i]["part"].ToString());
                            i = i + 1;
                            long frmLocID = GetLocationID(ds.Tables[0].Rows[i]["part"].ToString());
                            i = i + 1;
                            long toLocID = GetLocationID(ds.Tables[0].Rows[i]["part"].ToString());
                            i = i + 1;
                            string batchCode = ds.Tables[0].Rows[i]["part"].ToString();

                            SqlCommand cmd1 = new SqlCommand();
                            SqlDataAdapter da1 = new SqlDataAdapter();
                            DataSet ds1 = new DataSet();
                            DataTable dt1 = new DataTable();
                            cmd1.CommandType = CommandType.StoredProcedure;
                            //cmd1.CommandText = "exec dbo.WMS_SP_InternalTransfer " + frmLocID + "," + toLocID + "," + skuid + "," + qty + ",'" + batchCode + "'," + userID + "";
                            cmd1.CommandText = "WMS_SP_InternalTransfer";
                            cmd1.Connection = conn;
                            conn.Open();
                            cmd1.Parameters.Clear();
                            cmd1.Parameters.AddWithValue("frmLocID", frmLocID);
                            cmd1.Parameters.AddWithValue("toLocID", toLocID);
                            cmd1.Parameters.AddWithValue("prdID", skuid);
                            cmd1.Parameters.AddWithValue("qty", qty);
                            cmd1.Parameters.AddWithValue("batchCode", batchCode);
                            cmd1.Parameters.AddWithValue("createdBy", userID);
                            cmd1.ExecuteNonQuery();
                            conn.Close();
                            mySeq = mySeq + 1;
                        }
                        SqlCommand cmd2 = new SqlCommand();
                        SqlDataAdapter da2 = new SqlDataAdapter();
                        DataSet ds2 = new DataSet();
                        DataTable dt2 = new DataTable();
                        string dsskudetails = "update tInternalTransferHead set Status=64 where ID=" + transferID + "";
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = dsskudetails;
                        cmd2.Connection = conn;
                        cmd2.Parameters.Clear();
                        da2.SelectCommand = cmd2;
                        da2.Fill(ds2, "tbl3");
                    }
                }
                else { }
            }
            else { }
            String xmlString = String.Empty;
            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            if (mySeq > 0)
            {
                jsonString = jsonString + "{\n\"status\":\"success\"\n}\n";
            }
            else
            {
                jsonString = jsonString + "{\n\"status\":\"failed\"\n}\n";
            }

            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        public long GetLocationID(string locationcode)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd5 = new SqlCommand();
            SqlDataAdapter da5 = new SqlDataAdapter();
            DataSet ds5 = new DataSet();
            DataTable dt5 = new DataTable();
            cmd5.CommandType = CommandType.Text;
            cmd5.CommandText = "select ID from mlocation where Code=ltrim(rtrim('" + locationcode + "'))";
            cmd5.Connection = conn;
            cmd5.Parameters.Clear();
            da5.SelectCommand = cmd5;
            da5.Fill(ds5, "tbl1");
            dt5 = ds5.Tables[0];
            long locID = 0;
            if (dt.Rows.Count > 0)
            {
                locID = long.Parse(dt5.Rows[0]["ID"].ToString());
            }
            return locID;
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