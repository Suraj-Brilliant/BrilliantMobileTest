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
    /// Summary description for ws_get_qc_list
    /// </summary>
    public class ws_get_qc_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, UserID = 0;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"arr_qc_list\":[\n";

            UserID = long.Parse(context.Request.QueryString["user_id"]);
            if(context.Request.QueryString["wrid"]!=null)
            {
                WarehouseID = Convert.ToInt64(context.Request.QueryString["wrid"]);
            }
            if (context.Request.QueryString["user_id"] != null)
            {
                cmd.CommandType = CommandType.Text;
               // cmd.CommandText = "select T.TaskID, T.ObjectName, T.AssignTo, T.TaskDate, T.TaskECD, case when T.TaskRemark='' then 'N/A' else T.TaskRemark end TaskRemark, J.OrderObjectName, T.CreatedBy,U.FirstName + ' ' + U.LastName AssignBy, T.CreationDate, T.Active, T.Priority, T.JobCardName,J.OID POID, case when J.OrderObjectName = 'GRN' then J.OID when J.OrderObjectName = 'PurchaseOrder' then(select top(1) ID from tGRNHead where OID = J.OID and status in(38, 31, 52, 60)) end GRNID,  case when J.OrderObjectName = 'GRN' then(case when GH.Status in (38, 31, 52, 60) then 'Pending' when GH.Status in (32,55,58,61) then 'Completed' end)  when J.OrderObjectName = 'PurchaseOrder' then(Case When PH.Status in (38, 31, 52, 60) then 'Pending'  when PH.Status in (32,55,58,61) then 'Completed' end) end POStatus, case when J.OrderObjectName = 'GRN' then GH.objectname else case when J.OrderObjectName = 'PurchaseOrder' then PH.Object end end objNM FROM tTaskDetail T left outer join tJobCardDetail J on T.TaskID = J.TaskID left outer join tPurchaseOrderHead PH on J.OID = PH.ID left outer join tGRNHead GH on J.OID = GH.ID left outer join mStatus S on PH.Status = S.ID left outer join mUserProfilehead U on T.CreatedBy = U.ID where T.AssignTo = " + UserID +" and T.ObjectName = 17 and(GH.Status in (31, 38, 52, 60) OR PH.Status in (31,38,52,60))";
                cmd.CommandText = "exec qcListMobile " + UserID + ","+ WarehouseID +"";
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
                        string id = dt.Rows[i]["GRNID"].ToString();
                        string title = dt.Rows[i]["JobCardName"].ToString();
                        string assignDate = dt.Rows[i]["TaskDate"].ToString();
                        string assignBy = dt.Rows[i]["AssignBy"].ToString();
                        string expCompletionDate = dt.Rows[i]["TaskECD"].ToString();
                        string remark = dt.Rows[i]["TaskRemark"].ToString();
                        string status = dt.Rows[i]["POStatus"].ToString();
                        string objNM = dt.Rows[i]["objNM"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"objectname\": \"" + objNM.Trim() + "\",\n";
                        jsonString = jsonString + "\"title\": \"" + title.Trim() + "\",\n";
                        jsonString = jsonString + "\"assigned_date\": \"" + assignDate.Trim() + "\",\n";
                        jsonString = jsonString + "\"assigned_by\": \"" + assignBy.Trim() + "\",\n";
                        jsonString = jsonString + "\"ex_complited_date\": \"" + expCompletionDate.Trim() + "\",\n";
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