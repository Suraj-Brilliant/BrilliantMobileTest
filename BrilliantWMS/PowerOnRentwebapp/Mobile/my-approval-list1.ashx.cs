using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using PowerOnRentwebapp.Login;
using PowerOnRentwebapp.PORServicePartRequest;
using PowerOnRentwebapp.PORServiceUCCommonFilter;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for my_approval_list
    /// </summary>
    public class my_approval_list : IHttpHandler
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

            long uId = Convert.ToInt64(context.Request.QueryString["uId"]);
            string dptId = context.Request.QueryString["dptId"];

           
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = "select  VW.OrderId,Replace(OH.OrderNumber,'&','&amp;')OrderNumber,Replace(OH.Title,'&','&amp;')Title,Replace(OH.Remark,'&','&amp;')Remark,VW.ImgApproval,ST.Status OrderStatusName ,VW.StoreId,VW.ApproverID,OH.OrderNo from VW_ApprovalTransDetails VW left outer join tOrderHead OH on VW.OrderId=OH.Id left outer join mStatus ST on OH.Status =ST.ID where (VW.DeligateTo=" + uId + " or VW.ApproverID=" + uId + ")  and VW.StoreId="+ dptId +"  Order by VW.OrderId desc  ";
            //cmd.Connection = conn;
            //cmd.Parameters.Clear();
            ///*cmd.Parameters.AddWithValue("SiteIDs", 0);
            //cmd.Parameters.AddWithValue("UserID", uId);*/
            //da.SelectCommand = cmd;
            //da.Fill(ds, "tbl1");
            //dt = ds.Tables[0];
            //int cntr = dt.Rows.Count;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "POR_SP_GetRequestBySiteIDsOrUserIDForMOBILE";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SiteIDs", 0);
            cmd.Parameters.AddWithValue("UserID", uId);
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
  
            int cntr = dt.Rows.Count;

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            if (cntr > 0)
            {
                for (int i = 0; i < cntr; i++)
                {
                    DateTime?  OrderDateTime = null;
                    string datetime = "";
                    long OID = Convert.ToInt64(ds.Tables[0].Rows[i]["Id"].ToString());
                    string OrderNumber = ds.Tables[0].Rows[i]["OrderNumber"].ToString();
                    string Title = ds.Tables[0].Rows[i]["Title"].ToString();
                   // string Remark = ds.Tables[0].Rows[i]["Remark"].ToString();
                    string ImgApproval = ds.Tables[0].Rows[i]["ImgApproval"].ToString();
                    string OrderStatusName = ds.Tables[0].Rows[i]["RequestStatus"].ToString();
                    string OrderNo = ds.Tables[0].Rows[i]["OrderNo"].ToString();
                    datetime = ds.Tables[0].Rows[i]["ApprovalDate"].ToString();
                    if (datetime !="")
                    {
                       OrderDateTime = DateTime.Parse(ds.Tables[0].Rows[i]["ApprovalDate"].ToString());
                    }



                    //OrderStatusName
                    xmlString = xmlString + "<requestItem>";

                    xmlString = xmlString + "<reId>" + OrderNo + "</reId>";
                    xmlString = xmlString + "<reNo>" + CheckString(OrderNumber) + "</reNo>";
                    xmlString = xmlString + "<itemTitle>" + CheckString(Title) + "</itemTitle>";
                   // xmlString = xmlString + "<itemNote>" + Remark + "</itemNote>";
                    xmlString = xmlString + "<statusColor>" + ImgApproval + "</statusColor>";
                    xmlString = xmlString + "<itemStatus>" + OrderStatusName + "</itemStatus>";
                    xmlString = xmlString + "<itemDateTime>" + OrderDateTime + "</itemDateTime>";
                    xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";
                    xmlString = xmlString + "</requestItem>";
                }
            }

            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "&amp;");
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
