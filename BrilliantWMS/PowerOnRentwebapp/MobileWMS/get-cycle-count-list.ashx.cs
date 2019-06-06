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
    /// Summary description for get_cycle_count_list
    /// </summary>
    public class get_cycle_count_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, UserID = 0;
        string skey = "";

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/

            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            UserID = long.Parse(context.Request.QueryString["uid"]);
            skey = context.Request.QueryString["skey"];

            if (UserID != 0)
            {
                cmd.CommandType = CommandType.Text;
                if (skey == "")
                {
                    cmd.CommandText = "SELECT ID, Title,  Status, WarehouseID, Frequency, CountBasis, CycleCountDate, Active FROM tCycleCountHead where Active='Yes' and WarehouseID=" + WarehouseID + "";
                }
                else
                {
                    cmd.CommandText = "SELECT ID, Title,  Status, WarehouseID, Frequency, CountBasis, CycleCountDate, Active FROM tCycleCountHead where Active='Yes' and WarehouseID=" + WarehouseID + " and Title like '%" + skey + "%'";
                }
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;
                if (cntr > 0)
                {
                    for (int c = 0; c <= cntr - 1; c++)
                    {
                        string id = dt.Rows[c]["ID"].ToString();
                        string title = dt.Rows[c]["Title"].ToString();
                        string date = dt.Rows[c]["CycleCountDate"].ToString();
                        string frequency = dt.Rows[c]["Frequency"].ToString();
                        string countbasis = dt.Rows[c]["CountBasis"].ToString();
                        string status = dt.Rows[c]["Status"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"title\": \"" + title.Trim() + "\",\n";
                        jsonString = jsonString + "\"date\": \"" + date.Trim() + "\",\n";
                        jsonString = jsonString + "\"frequency\": \"" + frequency.Trim() + "\",\n";
                        jsonString = jsonString + "\"countbasis\": \"" + countbasis.Trim() + "\",\n";
                        jsonString = jsonString + "\"status\": \"" + status.Trim() + "\"\n";

                        if (c == cntr - 1)
                        {
                            jsonString = jsonString + "}\n";
                        }
                        else
                        {
                            jsonString = jsonString + "},\n";
                        }
                    }
                }
            }
            else { }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
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