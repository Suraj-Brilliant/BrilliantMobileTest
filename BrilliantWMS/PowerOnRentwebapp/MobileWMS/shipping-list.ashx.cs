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
    /// Summary description for shipping_list
    /// </summary>
    public class shipping_list : IHttpHandler
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
            if(context.Request.QueryString["skey"].ToString()!=null)
            {
                skey = context.Request.QueryString["skey"].ToString();
            }
            if (context.Request.QueryString["uid"] != null)
            {
                cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "SELECT   T.TaskID, T.ObjectName, T.AssignTo, T.TaskDate, T.TaskECD, case when T.TaskRemark='' then 'N/A' else T.TaskRemark end TaskRemark,  T.CreatedBy, U.FirstName+' '+U.LastName AssignBy,T.CreationDate, T.Active, T.Priority, T.JobCardName,J.OID SalesOrderID,O.Status,S.Status StatusName,Case When O.Status=37 then 'Pending'  when O.Status in(38,32,40) then 'Completed' end PickUpStatus FROM  tTaskDetail T left outer join tJobCardDetail J on T.TaskID=J.TaskID left outer join tOrderHead O on J.OID=O.ID left outer join mStatus S on O.Status=S.ID left outer join mUserProfilehead U on T.CreatedBy=U.ID where   T.ObjectName=10004 and T.AssignTo="+ UserID + " and O.Status=37";
                if(skey!="")
                {
                    cmd.CommandText = "exec MobilePickList1 " + UserID + "," + WarehouseID + "," + skey + "";
                }
                else
                {
                    cmd.CommandText = "exec MobilePickList " + UserID + "," + WarehouseID +"";
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
                        string id = dt.Rows[i]["SalesOrderID"].ToString();
                        string title = dt.Rows[i]["JobCardName"].ToString();
                        string assignDate = dt.Rows[i]["TaskDate"].ToString();
                        string assignBy = dt.Rows[i]["AssignBy"].ToString();
                        string expCompletionDate = dt.Rows[i]["TaskECD"].ToString();
                        string remark = dt.Rows[i]["TaskRemark"].ToString();
                        string status = dt.Rows[i]["PickUpStatus"].ToString();
                        string obj = dt.Rows[i]["MainObject"].ToString();
                        string IDB_ODB = dt.Rows[i]["IDB_ODB"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"title\": \"" + title.Trim() + "\",\n";
                        jsonString = jsonString + "\"assignDate\": \"" + assignDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"assignBy\": \"" + assignBy.Trim() + "\",\n";
                        jsonString = jsonString + "\"expCompletionDate\": \"" + expCompletionDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"remark\": \"" + remark.Trim() + "\",\n";
                        jsonString = jsonString + "\"objectname\": \"" + obj.Trim() + "\",\n";
                        jsonString = jsonString + "\"status\": \"" + status.Trim() + "\",\n";
                        jsonString = jsonString + "\"IDB_ODB\": \"" + IDB_ODB.Trim() + "\"\n";
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