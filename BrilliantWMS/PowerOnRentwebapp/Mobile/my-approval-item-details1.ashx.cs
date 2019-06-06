﻿using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for my_approval_item_details
    /// </summary>
    public class my_approval_item_details : IHttpHandler
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

            string OdrID = context.Request.QueryString["reId"];

            long aprId = Convert.ToInt64(context.Request.QueryString["aprId"]);

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

            long reId = Convert.ToInt64(dt3.Rows[0]["Id"].ToString());

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select OH.ID,OH.OrderNumber,OH.OrderNo,OH.OrderDate,OH.DeliveryDate,Replace(OH.Remark,'&','&amp;')Remark,Replace(CPD1.Name,'&','&amp;')Con1,Replace(CPD2.Name,'&','&amp;') Con2,Replace(Adr.AddressLine1,'&','&amp;')AddressLine1,Replace(OH.Title,'&','&amp;')Title,ST.Status StatusName from tOrderHead OH left outer join tContactPersonDetail CPD1 on OH.ContactId1=CPD1.ID left outer join tContactPersonDetail CPD2 on OH.ContactId2=CPD2.ID left outer join tAddress Adr on OH.AddressId=Adr.ID left outer join mStatus ST on OH.Status =ST.ID where OH.ID=" + reId + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            /*cmd.Parameters.AddWithValue("SiteIDs", 0);
            cmd.Parameters.AddWithValue("UserID", uId);*/
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            string OrderNumber = ds.Tables[0].Rows[0]["OrderNumber"].ToString();
            string OrderDate = ds.Tables[0].Rows[0]["OrderDate"].ToString();
            string DeliveryDate = ds.Tables[0].Rows[0]["DeliveryDate"].ToString();
            string Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
            string Con1 = ds.Tables[0].Rows[0]["Con1"].ToString();
            string Con2 = ds.Tables[0].Rows[0]["Con2"].ToString();
            string AddressLine1 = ds.Tables[0].Rows[0]["AddressLine1"].ToString();
            string Title = ds.Tables[0].Rows[0]["Title"].ToString();
            string StatusName = ds.Tables[0].Rows[0]["StatusName"].ToString();
            string OrderNo = ds.Tables[0].Rows[0]["OrderNo"].ToString();

            xmlString = xmlString + "<requestItemDetails>";
            xmlString = xmlString + "<orderNumber>" + CheckString(OrderNumber) + "</orderNumber>";
            xmlString = xmlString + "<orderDate>" + OrderDate + "</orderDate>";
            xmlString = xmlString + "<deliveryDate>" + DeliveryDate + "</deliveryDate>";
            xmlString = xmlString + "<remark>" + CheckString(Remark) + "</remark>";
            xmlString = xmlString + "<contact1>" + Con1 + "</contact1>";
            xmlString = xmlString + "<contact2>" + Con2 + "</contact2>";
            xmlString = xmlString + "<addressLine1>" + CheckString(AddressLine1) + "</addressLine1>";
            xmlString = xmlString + "<title>" + CheckString(Title) + "</title>";
            xmlString = xmlString + "<statusName>" + StatusName + "</statusName>";
            xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";

            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select AT.Remark,ST.Status StatusName from tApprovalTrans AT left outer join mStatus ST on AT.Status=ST.ID where OrderId=" + reId + " and ApproverID=" + aprId + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();            
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl2");
            dt1 = ds1.Tables[0];
            int cntr1 = dt1.Rows.Count;
            string appRemark = "", appStatus = "";
            if (cntr1 > 0)
            {
                appRemark = ds1.Tables[0].Rows[0]["Remark"].ToString();
                appStatus = ds1.Tables[0].Rows[0]["StatusName"].ToString();
            }
            xmlString = xmlString + "<myRemark>" + CheckString(appRemark) + "</myRemark>";
            xmlString = xmlString + "<myStatus>" + CheckString(appStatus) + "</myStatus>";

            xmlString = xmlString + "</requestItemDetails>";

            xmlString = xmlString + "</gwcInfo>";

            context.Response.Write(xmlString);
        }


        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
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