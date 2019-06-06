using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using BrilliantWMS.Login;
using System.IO;
using System.Web.Security;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>
    /// Summary description for my_order_history_list
    /// </summary>
    public class my_order_history_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            string skey = context.Request.QueryString["skey"];
            string driverid = context.Request.QueryString["drId"];

            if (context.Request.QueryString["drId"] != null)
            {

                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataTable dt1 = new DataTable();
                DataSet ds1 = new DataSet();

                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select Top(15) DAD.OrderId,OH.orderType from tDriverAssignDetail DAD left outer join tOrderHead OH on DAD.OrderId =OH.Id where DAD.DriverId=" + driverid + " and OH.Status in(8,26) Order by OrderId desc";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                //cmd.Parameters.AddWithValue("param1", ResourceId);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl2");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        long OrderId = Convert.ToInt64(dt1.Rows[i]["OrderId"].ToString());
                        string Ordertype = dt1.Rows[i]["orderType"].ToString();
                        cmd.CommandType = CommandType.Text;
                        if ((context.Request.QueryString["skey"] != null) && (context.Request.QueryString["skey"] != ""))
                        {
                            if (Ordertype == "Ecommerce")
                            {

                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CD.Name CustomerName,CD.Address,CD.ContactNo,CD.DeliveryType,DA.DriverID from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + " and (CD.Name like '%" + skey + "%' OR CD.ContactNo like '%" + skey + "%' OR  OH.Id like '%" + skey + "%' OR CD.Address like '%" + skey + "%'OR CD.DeliveryType like '%" + skey + "%' OR OH.OrderNo like '%"+ skey +"%')";
                            }
                            else
                            {
                                cmd.CommandText = "Select * from(select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CASE TA.AddressType when 'Location' then TA.ContactName when 'none' then TCPD.Name when 'General Address' then TCPD.Name End CustomerName,CASE TA.AddressType when 'none' then TA.AddressLine1 when 'Location' then TA.AddressLine1 when 'General Address' then TA.AddressLine1 End Address,CASE TA.AddressType when 'Location' then TA.MobileNo when 'none' then TCPD.MobileNo when 'General Address' then TCPD.MobileNo End ContactNo,' ' as DeliveryType,DA.DriverID from tOrderHead OH left outer join tAddress TA on OH.AddressId=TA.ID left outer join mStatus ST on OH.Status=ST.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + ")aaa where (CustomerName like '%" + skey + "%' OR Address like '%" + skey + "%' OR Id like '%" + skey + "%' OR ContactNo like '%" + skey + "%' OR DeliveryType like '%" + skey + "%' OR OrderNo like '%"+ skey +"%')";
                            }
                        }

                        else
                        {
                            if (Ordertype == "Ecommerce")
                            {

                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo ,ST.Status OrderStatus,CD.Name CustomerName,CD.Address,CD.ContactNo,CD.DeliveryType,DA.DriverID from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + "";
                            }
                            else
                            {
                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CASE TA.AddressType when 'Location' then TA.ContactName when 'none' then TCPD.Name when 'General Address' then TCPD.Name End CustomerName,CASE TA.AddressType when 'none' then TA.AddressLine1 when 'Location' then TA.AddressLine1 when 'General Address' then TA.AddressLine1 End Address,CASE TA.AddressType when 'Location' then TA.MobileNo when 'none' then TCPD.MobileNo when 'General Address' then TCPD.MobileNo End ContactNo,' ' as DeliveryType,DA.DriverID from tOrderHead OH left outer join tAddress TA on OH.AddressId=TA.ID left outer join mStatus ST on OH.Status=ST.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId where DA.DriverID=" + driverid + " and OH.Status in(8,26)and OH.Id=" + OrderId + " ";
                            }
                        }

                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("param1", ResourceId);
                        da.SelectCommand = cmd;
                        da.Fill(ds, "tbl1");
                        dt = ds.Tables[0];
                        int cntr = dt.Rows.Count;

                        if (cntr > 0)
                        {
                            if ((context.Request.QueryString["skey"] != null) && (context.Request.QueryString["skey"] != ""))
                            {
                                String OrderIdd = dt.Rows[0]["Id"].ToString();
                                String CustomerName = dt.Rows[0]["CustomerName"].ToString();
                                String Address = dt.Rows[0]["Address"].ToString();
                                String ContactNo = dt.Rows[0]["ContactNo"].ToString();
                                String OrderDate = dt.Rows[0]["Orderdate"].ToString();
                                String DeliveredDate = dt.Rows[0]["CompletedDate"].ToString();
                                String DeliveryType = dt.Rows[0]["DeliveryType"].ToString();
                                String DeliveryStatus = dt.Rows[0]["OrderStatus"].ToString();
                                String OrderNo = dt.Rows[0]["OrderNo"].ToString();

                            
                                xmlString = xmlString + "<orderItem>";
                                xmlString = xmlString + "<orderId>" + OrderNo + "</orderId>";
                                xmlString = xmlString + "<customerName>" + CustomerName + "</customerName>";
                                xmlString = xmlString + "<customerAddress>" + Address + "</customerAddress>";
                                xmlString = xmlString + "<contactNumber>" + ContactNo + "</contactNumber>";
                                xmlString = xmlString + "<orderDate>" + OrderDate + "</orderDate>";
                                xmlString = xmlString + "<deliveredDate>" + DeliveredDate + "</deliveredDate>";
                                xmlString = xmlString + "<deliveryMode>" + DeliveryType + "</deliveryMode>";
                                xmlString = xmlString + "<deliveryStatus>" + DeliveryStatus + "</deliveryStatus>";
                                xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";
                                xmlString = xmlString + "</orderItem>";
                                dt.Clear();
                            }
                            else
                            {
                                String OrderIdd = dt.Rows[i]["Id"].ToString();
                                String CustomerName = dt.Rows[i]["CustomerName"].ToString();
                                String Address = dt.Rows[i]["Address"].ToString();
                                String ContactNo = dt.Rows[i]["ContactNo"].ToString();
                                String OrderDate = dt.Rows[i]["Orderdate"].ToString();
                                String DeliveredDate = dt.Rows[i]["CompletedDate"].ToString();
                                String DeliveryType = dt.Rows[i]["DeliveryType"].ToString();
                                String DeliveryStatus = dt.Rows[i]["OrderStatus"].ToString();
                                String OrderNo = dt.Rows[i]["OrderNo"].ToString();

                              
                                xmlString = xmlString + "<orderItem>";
                                xmlString = xmlString + "<orderId>" + OrderNo + "</orderId>";
                                xmlString = xmlString + "<customerName>" + CustomerName + "</customerName>";
                                xmlString = xmlString + "<customerAddress>" + Address + "</customerAddress>";
                                xmlString = xmlString + "<contactNumber>" + ContactNo + "</contactNumber>";
                                xmlString = xmlString + "<orderDate>" + OrderDate + "</orderDate>";
                                xmlString = xmlString + "<deliveredDate>" + DeliveredDate + "</deliveredDate>";
                                xmlString = xmlString + "<deliveryMode>" + DeliveryType + "</deliveryMode>";
                                xmlString = xmlString + "<deliveryStatus>" + DeliveryStatus + "</deliveryStatus>";
                                xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";
                                xmlString = xmlString + "</orderItem>";
                            }
                        }
                    }
                }
            }
            else
            {
               
            }

            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
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