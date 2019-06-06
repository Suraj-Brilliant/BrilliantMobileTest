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
    /// Summary description for ws_get_grn_list
    /// </summary>
    public class ws_get_grn_list : IHttpHandler
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
            jsonString = "{\n\"arr_grn_list\": [\n";

            UserID = long.Parse(context.Request.QueryString["user_id"]);
            if(context.Request.QueryString["wrid"].ToString()!=null)
            {
                WarehouseID= long.Parse(context.Request.QueryString["wrid"]);
            }
            if(context.Request.QueryString["skey"].ToString() != null)
            {
                skey = context.Request.QueryString["skey"].ToString();
            }
            if (context.Request.QueryString["user_id"] != null)
            {
                cmd.CommandType = CommandType.Text;
                //  cmd.CommandText = "select T.TaskID, T.ObjectName, T.AssignTo, T.TaskDate, T.TaskECD, case when T.TaskRemark='' then 'N/A' else T.TaskRemark end TaskRemark, T.CreatedBy,U.FirstName + ' ' + U.LastName AssignBy, T.CreationDate, T.Active, T.Priority, T.JobCardName,J.OID POID, PH.Status,S.Status StatusName,      Case When PH.Status = 41 then 'Pending'  when PH.Status = 31 then 'Completed' end POStatus ,PH.Object ObjNM FROM tTaskDetail T left outer join tJobCardDetail J on T.TaskID = J.TaskID left outer join tPurchaseOrderHead PH on J.OID = PH.ID left outer join mStatus S on PH.Status = S.ID left outer join mUserProfilehead U on T.CreatedBy = U.ID where T.ObjectName = 16 and PH.Status = 41 and T.AssignTo = " + UserID +"";
                //cmd.CommandText = " select T.TaskID, T.ObjectName, T.AssignTo, T.TaskDate, T.TaskECD, case when T.TaskRemark = '' then 'N/A' else T.TaskRemark end TaskRemark,T.CreatedBy,U.FirstName + ' ' + U.LastName AssignBy, T.CreationDate, T.Active, T.Priority, T.JobCardName,J.OID POID, PH.Status,S.Status StatusName,Case When PH.Status = 41 then 'Pending'  when PH.Status = 31 then 'Completed' when PH.Status = 63 then 'Partially Completed'   end POStatus, PH.Object ObjNM FROM tTaskDetail T left outer join tJobCardDetail J on T.TaskID = J.TaskID left outer join tPurchaseOrderHead PH on J.OID = PH.ID left outer join mStatus S on PH.Status = S.ID left outer join mUserProfilehead U on T.CreatedBy = U.ID where T.ObjectName = 16 and PH.Status in(41,63) and T.AssignTo  = " + UserID + "";
                if(skey!="")
                {
                    cmd.CommandText = "exec grnListMobile1 " + UserID + "," + WarehouseID + "," + skey + "";
                }
                else
                {
                    cmd.CommandText = "exec grnListMobile " + UserID + "," + WarehouseID + "";
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
                        string id = dt.Rows[i]["POID"].ToString();
                        string title = dt.Rows[i]["JobCardName"].ToString();
                        string assignDate = dt.Rows[i]["TaskDate"].ToString();
                        string assignBy = dt.Rows[i]["AssignBy"].ToString();
                        string expCompletionDate = dt.Rows[i]["TaskECD"].ToString();
                        string remark = dt.Rows[i]["TaskRemark"].ToString();
                        string status = dt.Rows[i]["POStatus"].ToString();
                        string objectname= dt.Rows[i]["ObjNM"].ToString();
                        string IDB_ODB= dt.Rows[i]["IDB_ODB"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"objectname\": \"" + objectname.Trim() + "\",\n";
                        jsonString = jsonString + "\"title\": \"" + title.Trim() + "\",\n";
                        jsonString = jsonString + "\"assigned_date\": \"" + assignDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"assigned_by\": \"" + assignBy.Trim() + "\",\n";
                        jsonString = jsonString + "\"ex_complited_date\": \"" + expCompletionDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"remark\": \"" + remark.Trim() + "\",\n";
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