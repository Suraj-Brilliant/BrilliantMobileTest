using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartRequest;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for set_approval
    /// </summary>
    public class set_approval : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            long aprId = Convert.ToInt64(context.Request.Form["aprId"]);
            string OdrID = context.Request.Form["reqId"];
            String reqAct = context.Request.Form["reqAct"];

            SqlCommand cmd3 = new SqlCommand();
            SqlDataAdapter da3 = new SqlDataAdapter();
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "select Id from tOrderHead where OrderNo='" + OdrID + "'";
            cmd3.Connection = conn;
            cmd3.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);
            da3.SelectCommand = cmd3;
            da3.Fill(ds3, "tbl4");
            dt3 = ds3.Tables[0];

            long reqId = Convert.ToInt32(dt3.Rows[0]["Id"].ToString());


            long statusId = 0;
            if (reqAct == "Approved")
            { statusId = 3; }
            else if (reqAct == "Rejected") { statusId = 4; }

            string reqRemark = context.Request.Form["reqRemark"];
            string aprUserName = context.Request.Form["aprUserName"];
            string InvNo = "";
            InvNo = context.Request.Form["reqInvoiceNo"];

            CustomProfile profile = CustomProfile.GetProfile(aprUserName);


            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
            xmlString = xmlString + "<approvalAction>";

            iPartRequestClient objService = new iPartRequestClient();
            try
            {

                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataTable dt1 = new DataTable();
                DataSet ds1 = new DataSet();
                cmd1.Connection = conn;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select * from VW_ApprovalTransDetails where OrderID=" + reqId + " and approverID=" + aprId + " and Status !=3";
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "dtl");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    long aprovalid = Convert.ToInt64(dt1.Rows[0]["ApprovalID"].ToString());
                    if (statusId == 3)
                    {
                        objService.UpdatetApprovalTransAfterApproval(aprovalid, reqId, statusId, CheckString(reqRemark), aprId, InvNo, profile.DBConnection._constr);
                    }
                    else if (statusId == 4)
                    {
                        objService.UpdatetApprovalTransAfterReject(aprovalid, reqId, statusId, CheckString(reqRemark), aprId, profile.DBConnection._constr);
                    }
                    xmlString = xmlString + "<authmsg>success</authmsg>";
                }
            }
            catch { xmlString = xmlString + "<authmsg>failed</authmsg>"; }

            xmlString = xmlString + "</approvalAction>";
            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "&amp");
            return value;
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