using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PowerOnRentwebapp.Login;
using System.IO;
using System.Web.Security;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>532
    /// Summary description for download_my_order_history_list
    /// </summary>
    public class download_my_order_history_list : IHttpHandler
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

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"orderlist\": [\n";

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

                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CD.Name CustomerName,CD.Address,CD.ContactNo,CD.DeliveryType,DA.DriverID ,PD.PaymentMode,PD.CardNo,PD.Amount ,CD.PhotoIdtype, PD.Remark  from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentDetail PD on OH.Id=PD.OrderID where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + " and (CD.Name like '%" + skey + "%' OR CD.ContactNo like '%" + skey + "%' OR  OH.Id like '%" + skey + "%' OR CD.Address like '%" + skey + "%' OR CD.DeliveryType like '%" + skey + "%' OR OH.OrderNo like '%" + skey + "%')";
                            }
                            else
                            {
                                cmd.CommandText = "Select * from(select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CASE TA.AddressType when 'Location' then TA.ContactName when 'none' then TCPD.Name when 'General Address' then TCPD.Name End CustomerName,CASE TA.AddressType when 'none' then TA.AddressLine1 when 'Location' then TA.AddressLine1 when 'General Address' then TA.AddressLine1 End Address,CASE TA.AddressType when 'Location' then TA.MobileNo when 'none' then TCPD.MobileNo when 'General Address' then TCPD.MobileNo End ContactNo,' ' as DeliveryType,DA.DriverID ,PMM.MethodName PaymentMode,PMD.CardNo,OH.GrandTotal as Amount ,CD.PhotoIdtype,PD.Remark  from tOrderHead OH left outer join tAddress TA on OH.AddressId=TA.ID left outer join mStatus ST on OH.Status=ST.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentMethodmain PMM ON OH.PaymentID=PMM.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join mPaymentDetail PD on OH.Id=PD.OrderID left outer join mPaymentDetail PMD  ON OH.ID=PMD.OrderId  where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + ")aaa where (CustomerName like '%" + skey + "%' OR Address like '%" + skey + "%' OR Id like '%" + skey + "%' OR ContactNo like '%" + skey + "%' OR DeliveryType like '%" + skey + "%' OR OrderNo like '%" + skey + "%')";
                            }
                        }

                        else
                        {
                            if (Ordertype == "Ecommerce")
                            {

                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo ,ST.Status OrderStatus,CD.Name CustomerName,CD.Address,CD.ContactNo,CD.DeliveryType,DA.DriverID ,PD.PaymentMode,PD.CardNo,PD.Amount,CD.PhotoIdtype, PD.Remark from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentDetail PD on OH.Id=PD.OrderID  where DA.DriverID=" + driverid + " and OH.Status in(8,26) and OH.Id=" + OrderId + "";
                            }
                            else
                            {
                                cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.OrderNo,ST.Status OrderStatus,CASE TA.AddressType when 'Location' then TA.ContactName when 'none' then TCPD.Name when 'General Address' then TCPD.Name End CustomerName,CASE TA.AddressType when 'none' then TA.AddressLine1 when 'Location' then TA.AddressLine1 when 'General Address' then TA.AddressLine1 End Address,CASE TA.AddressType when 'Location' then TA.MobileNo when 'none' then TCPD.MobileNo when 'General Address' then TCPD.MobileNo End ContactNo,' ' as DeliveryType,DA.DriverID ,PMM.MethodName PaymentMode,PMD.CardNo,OH.GrandTotal as Amount ,CD.PhotoIdtype,PD.Remark  from tOrderHead OH left outer join tAddress TA on OH.AddressId=TA.ID left outer join mStatus ST on OH.Status=ST.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentMethodmain PMM ON OH.PaymentID=PMM.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join mPaymentDetail PD on OH.Id=PD.OrderID left outer join mPaymentDetail PMD  ON OH.ID=PMD.OrderId  where DA.DriverID=" + driverid + " and OH.Status in(8,26)and OH.Id=" + OrderId + " ";
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
                                String DeliveredDate = dt.Rows[0]["DeliveryDate"].ToString();
                                String DeliveryType = dt.Rows[0]["DeliveryType"].ToString();
                                String DeliveryStatus = dt.Rows[0]["OrderStatus"].ToString();
                                String OrderNo = dt.Rows[0]["OrderNo"].ToString();
                                string photoIdType = dt.Rows[0]["PhotoIdtype"].ToString();
                                string remark = dt.Rows[0]["Remark"].ToString();
                                string paymentMode = dt.Rows[0]["PaymentMode"].ToString();
                                string paymentAmount = dt.Rows[0]["Amount"].ToString();
                                string cardNo = dt.Rows[0]["CardNo"].ToString();

                                /*Coading For Document*/
                                string attachment1 = "";
                                string attachment2 = "";
                                string attachment3 = "";
                                string attachment4 = "";
                                string signature = "";

                                SqlCommand cmddoc = new SqlCommand();
                                SqlDataAdapter dadoc = new SqlDataAdapter();
                                DataTable dtdoc = new DataTable();
                                DataSet dsdoc = new DataSet();
                                cmddoc.CommandType = CommandType.Text;
                                cmddoc.CommandText = "select ID,ReferenceID,DocumentName+'.'+FileType DocumentName,Sequence,DocumentType from tDocument where ReferenceID=" + OrderIdd + "";
                                cmddoc.Connection = conn;
                                cmddoc.Parameters.Clear();
                                dadoc.SelectCommand = cmddoc;
                                dadoc.Fill(dsdoc, "tbl3");
                                dtdoc = dsdoc.Tables[0];
                                if (dt1.Rows.Count > 0)
                                {
                                    for (int d = 0; d <= dtdoc.Rows.Count - 1; d++)
                                    {
                                        if (dtdoc.Rows[d]["DocumentType"].ToString() == "Signature")
                                        {
                                            signature = dtdoc.Rows[d]["DocumentName"].ToString();
                                        }
                                        else
                                        {
                                            if (d == 0) { attachment1 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 1) { attachment2 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 2) { attachment3 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 3) { attachment4 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                        }
                                    }
                                }
                                /*Coading For Document*/


                                jsonString = jsonString + "{\n";
                                jsonString = jsonString + "\"orderId\": \"" + OrderIdd.Trim() + "\",\n";
                                jsonString = jsonString + "\"orderLabel\": \"" + OrderNo.Trim() + "\",\n";
                                jsonString = jsonString + "\"customerName\": \"" + CustomerName.Trim() + "\",\n";
                                jsonString = jsonString + "\"customerAddress\": \"" + Address.Trim() + "\",\n";
                                jsonString = jsonString + "\"contactNumber\": \"" + ContactNo.Trim() + "\",\n";
                                jsonString = jsonString + "\"orderDate\": \"" + OrderDate.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveredDate\": \"" + DeliveredDate.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveryMode\": \"" + DeliveryType.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveryStatus\": \"" + DeliveryStatus.Trim() + "\",\n";
                                jsonString = jsonString + "\"photoIdType\": \"" + photoIdType + "\",\n";
                                jsonString = jsonString + "\"remark\": \"" + remark + "\",\n";
                                jsonString = jsonString + "\"paymentMode\": \"" + paymentMode.Trim() + "\",\n";
                                jsonString = jsonString + "\"paymentAmount\": \"" + paymentAmount.Trim() + "\",\n";
                                jsonString = jsonString + "\"cardNo\": \"" + cardNo.Trim() + "\",\n";

                                //jsonString = jsonString + "\"attachment1\": \"" + attachment1.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment2\": \"" + attachment2.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment3\": \"" + attachment3.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment4\": \"" + attachment4.Trim() + "\",\n";
                                //jsonString = jsonString + "\"signature\": \"" + signature.Trim() + "\"\n";

                                if (attachment1 == "") { jsonString = jsonString + "\"attachment1\": \"\",\n"; }
                                else { jsonString = jsonString + "\"attachment1\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment1.Trim() + "\",\n"; }
                                if (attachment2 == "") { jsonString = jsonString + "\"\",\n";  }
                                else { jsonString = jsonString + "\"attachment2\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment2.Trim() + "\",\n"; }
                                if (attachment3 == "") { jsonString = jsonString + "\"\",\n"; }
                                else { jsonString = jsonString + "\"attachment3\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment3.Trim() + "\",\n"; }
                                if (attachment4 == "") { jsonString = jsonString + "\"\",\n"; }
                                else { jsonString = jsonString + "\"attachment4\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment4.Trim() + "\",\n"; }
                                if (signature == "") { jsonString = jsonString + "\"\"\n"; }
                                else { jsonString = jsonString + "\"signature\": \"Attachment/" + OrderIdd.Trim() + "/" + signature.Trim() + "\"\n"; }

                                //if (i == dt1.Rows.Count - 1)
                                //{
                                jsonString = jsonString + "}\n";
                                //}
                                //else
                                //{
                                //    jsonString = jsonString + "},\n";
                                //}

                                dt.Clear();
                            }
                            else
                            {
                                String OrderIdd = dt.Rows[i]["Id"].ToString();
                                String CustomerName = dt.Rows[i]["CustomerName"].ToString();
                                String Address = dt.Rows[i]["Address"].ToString();
                                String ContactNo = dt.Rows[i]["ContactNo"].ToString();
                                String OrderDate = dt.Rows[i]["Orderdate"].ToString();
                                String DeliveredDate = dt.Rows[i]["DeliveryDate"].ToString();
                                String DeliveryType = dt.Rows[i]["DeliveryType"].ToString();
                                String DeliveryStatus = dt.Rows[i]["OrderStatus"].ToString();
                                String OrderNo = dt.Rows[i]["OrderNo"].ToString();
                                string photoIdType = dt.Rows[i]["PhotoIdtype"].ToString();
                                string remark = dt.Rows[i]["Remark"].ToString();
                                string paymentMode = dt.Rows[i]["PaymentMode"].ToString();
                                string paymentAmount = dt.Rows[i]["Amount"].ToString();
                                string cardNo = dt.Rows[i]["CardNo"].ToString();

                                /*Coading For Document*/
                                string attachment1 = "";
                                string attachment2 = "";
                                string attachment3 = "";
                                string attachment4 = "";
                                string signature = "";

                                SqlCommand cmddoc = new SqlCommand();
                                SqlDataAdapter dadoc = new SqlDataAdapter();
                                DataTable dtdoc = new DataTable();
                                DataSet dsdoc = new DataSet();
                                cmddoc.CommandType = CommandType.Text;
                                cmddoc.CommandText = "select ID,ReferenceID,DocumentName+'.'+FileType DocumentName,Sequence,DocumentType from tDocument where ReferenceID=" + OrderIdd + "";
                                cmddoc.Connection = conn;
                                cmddoc.Parameters.Clear();
                                dadoc.SelectCommand = cmddoc;
                                dadoc.Fill(dsdoc, "tbl3");
                                dtdoc = dsdoc.Tables[0];
                                if (dt1.Rows.Count > 0)
                                {
                                    for (int d = 0; d <= dtdoc.Rows.Count - 1; d++)
                                    {
                                        if (dtdoc.Rows[d]["DocumentType"].ToString() == "Signature")
                                        {
                                            signature = dtdoc.Rows[d]["DocumentName"].ToString();
                                        }
                                        else
                                        {
                                            if (d == 0) { attachment1 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 1) { attachment2 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 2) { attachment3 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                            if (d == 3) { attachment4 = dtdoc.Rows[d]["DocumentName"].ToString(); }
                                        }
                                    }
                                }
                                /*Coading For Document*/


                                jsonString = jsonString + "{\n";
                                jsonString = jsonString + "\"orderId\": \"" + OrderIdd.Trim() + "\",\n";
                                jsonString = jsonString + "\"orderLabel\": \"" + OrderNo.Trim() + "\",\n";
                                jsonString = jsonString + "\"customerName\": \"" + CustomerName.Trim() + "\",\n";
                                jsonString = jsonString + "\"customerAddress\": \"" + Address.Trim() + "\",\n";
                                jsonString = jsonString + "\"contactNumber\": \"" + ContactNo.Trim() + "\",\n";
                                jsonString = jsonString + "\"orderDate\": \"" + OrderDate.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveredDate\": \"" + DeliveredDate.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveryMode\": \"" + DeliveryType.Trim() + "\",\n";
                                jsonString = jsonString + "\"deliveryStatus\": \"" + DeliveryStatus.Trim() + "\",\n";
                                jsonString = jsonString + "\"photoIdType\": \"" + photoIdType + "\",\n";
                                jsonString = jsonString + "\"remark\": \"" + remark + "\",\n";
                                jsonString = jsonString + "\"paymentMode\": \"" + paymentMode.Trim() + "\",\n";
                                jsonString = jsonString + "\"paymentAmount\": \"" + paymentAmount.Trim() + "\",\n";
                                jsonString = jsonString + "\"cardNo\": \"" + cardNo.Trim() + "\",\n";

                                //jsonString = jsonString + "\"attachment1\": \"" + attachment1.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment2\": \"" + attachment2.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment3\": \"" + attachment3.Trim() + "\",\n";
                                //jsonString = jsonString + "\"attachment4\": \"" + attachment4.Trim() + "\",\n";
                                //jsonString = jsonString + "\"signature\": \"" + signature.Trim() + "\"\n"; 

                                if (attachment1 == "") { jsonString = jsonString + "\"attachment1\": \"\",\n"; }
                                else { jsonString = jsonString + "\"attachment1\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment1.Trim() + "\",\n"; }
                                if (attachment2 == "") { jsonString = jsonString + "\"attachment2\": \"\",\n"; }
                                else { jsonString = jsonString + "\"attachment2\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment2.Trim() + "\",\n"; }
                                if (attachment3 == "") { jsonString = jsonString + "\"attachment3\": \"\",\n"; }
                                else { jsonString = jsonString + "\"attachment3\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment3.Trim() + "\",\n"; }
                                if (attachment4 == "") { jsonString = jsonString + "\"attachment4\": \"\",\n"; }
                                else { jsonString = jsonString + "\"attachment4\": \"Attachment/" + OrderIdd.Trim() + "/" + attachment4.Trim() + "\",\n"; }
                                if (signature == "") { jsonString = jsonString + "\"signature\": \"\"\n"; }
                                else { jsonString = jsonString + "\"signature\": \"Attachment/" + OrderIdd.Trim() + "/" + signature.Trim() + "\"\n"; }

                                if (i == dt1.Rows.Count - 1)
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
                }
            }
            else
            {

            }

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