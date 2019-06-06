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
    /// Summary description for update_cycle_count
    /// </summary>
    public class update_cycle_count : IHttpHandler
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
            CustomProfile profile = CustomProfile.GetProfile();
            iCycleCountClient cycleclient = new iCycleCountClient();
            tCycleCountDetail cycledetail = new tCycleCountDetail();

            SqlConnection conn = new SqlConnection(strcon);

            long wid = long.Parse(context.Request.Form["wid"]);
            long uid = long.Parse(context.Request.Form["uid"]);
            long cyclecountid = long.Parse(context.Request.Form["cid"]);
            string locationcode = "";
            long locid = 0;
            string prdomsskucode = ""; decimal skuqty = 0;
            //long locid = GetLocationID(locationcode);  //LocationID

            String cycleCountDetails = context.Request.Form["cycleCountDetails"];
            //string myquerystring = "select * from  SplitString('" + cycleCountDetails + "',':')";
            //string myquerystring = "select * from  SplitString('" + cycleCountDetails + "','@')";

            string myquerystring = "select * from  SplitString('" + cycleCountDetails + "',':')";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = myquerystring;
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;
            long mySeq = 0;
            if (cntr > 0)
            {
                for (int i = 0; i <= cntr - 1; i++)
                {
                    locationcode = ds.Tables[0].Rows[i]["part"].ToString();
                    locid = GetLocationID(ds.Tables[0].Rows[i]["part"].ToString());
                    i = i + 1;
                    prdomsskucode = ds.Tables[0].Rows[i]["part"].ToString();
                    i = i + 1;
                    skuqty = Convert.ToDecimal(ds.Tables[0].Rows[i]["part"].ToString());

                    SqlCommand cmd1 = new SqlCommand();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    string dsprdDetails = "select ID,ProductCode,Name , Description  from mProduct where OMSSKUCode=ltrim(rtrim('" + prdomsskucode + "'))";
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = dsprdDetails;
                    cmd1.Connection = conn;
                    cmd1.Parameters.Clear();
                    da1.SelectCommand = cmd1;
                    da1.Fill(ds1, "tbl2");
                    dt1 = ds1.Tables[0];

                    string productCode = "";
                    long prdID = 0;
                    if (dt1.Rows.Count > 0)
                    {
                        productCode = ds1.Tables[0].Rows[0]["ProductCode"].ToString();
                        prdID = long.Parse(ds1.Tables[0].Rows[0]["ID"].ToString());
                    }

                    SqlCommand cmd3 = new SqlCommand();
                    SqlDataAdapter da3 = new SqlDataAdapter();
                    DataSet ds3 = new DataSet();
                    DataTable dt3 = new DataTable();

                    string insPrdDetails = "insert into tCycleCountDetail(CountHeadID,ProductCode, LocationCode,ActualQty,CreatedBy, CreationDate,SKUID,LocationID) values(" + cyclecountid + ",'" + productCode + "','" + locationcode + "'," + skuqty + "," + uid + ",'" + DateTime.Now + "'," + prdID + "," + locid + ")";
                    cmd3.CommandType = CommandType.Text;
                    cmd3.CommandText = insPrdDetails;
                    cmd3.Connection = conn;
                    cmd3.Parameters.Clear();
                    da3.SelectCommand = cmd3;
                    da3.Fill(ds3, "tbl3");

                    mySeq = mySeq + 1;                    
                }
            }
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