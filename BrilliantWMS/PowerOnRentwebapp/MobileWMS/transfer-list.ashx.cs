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
    /// Summary description for transfer_list
    /// </summary>
    public class transfer_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0;
        string skey = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            skey = context.Request.QueryString["skey"];

            if (context.Request.QueryString["wrid"] != null)
            {
                cmd.CommandType = CommandType.Text;
                if (skey == "")
                {
                    cmd.CommandText = "select th.ID,th.TransferDate,th.TransferBy,U.FirstName+' '+U.LastName TransferByName ,th.Status, th.WarehouseID, th.Remark, th.CompanyID, th.CustomerID, case when th.Status=56 then 'Pending' else 'Completed' End StatusName from tInternalTransferHead th left outer join mUserProfileHead U on th.TransferBy=U.ID where status=56 and warehouseID=" + WarehouseID + "";
                }
                else if (skey != "")
                {
                    cmd.CommandText = "select th.ID,th.TransferDate,th.TransferBy,U.FirstName+' '+U.LastName TransferByName ,th.Status, th.WarehouseID, th.Remark, th.CompanyID, th.CustomerID, case when th.Status=56 then 'Pending' else 'Completed' End StatusName from tInternalTransferHead th left outer join mUserProfileHead U on th.TransferBy=U.ID where status=56 and warehouseID=" + WarehouseID + " and (th.Remark like '%" + skey + "%' or th.ID like '%" + skey + "%')";
                }
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
                        string transferDate = dt.Rows[i]["TransferDate"].ToString();
                        string transferBy = dt.Rows[i]["TransferByName"].ToString();
                        string remark = dt.Rows[i]["Remark"].ToString();
                        string status = dt.Rows[i]["StatusName"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"title\": \"Transfer No." + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"transferDate\": \"" + transferDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"transferBy\": \"" + transferBy.Trim() + "\",\n";
                        jsonString = jsonString + "\"remark\": \"" + remark.Trim() + "\",\n";
                        jsonString = jsonString + "\"status\": \"" + status.Trim() + "\"\n";

                        if (i == cntr - 1)
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