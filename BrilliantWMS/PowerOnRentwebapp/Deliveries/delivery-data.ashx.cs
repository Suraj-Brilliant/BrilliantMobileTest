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

namespace BrilliantWMS.Deliveries
{
    /// <summary>
    /// Summary description for delivery_data
    /// </summary>
    public class delivery_data : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        SqlCommand cmd1 = new SqlCommand();
        SqlDataAdapter da1 = new SqlDataAdapter();
        DataSet ds1 = new DataSet();
        DataTable dt1 = new DataTable();
        SqlDataReader dr1;

        long ResourceId = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            string driverid = context.Request.QueryString["drId"];
            string OdrID = context.Request.QueryString["odrId"];

            if (context.Request.QueryString["drId"] != null && context.Request.QueryString["odrId"] != null)
            {
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


                long Orderid = Convert.ToInt64(dt3.Rows[0]["Id"].ToString());

                SqlCommand cmd2 = new SqlCommand();
                SqlDataAdapter da2 = new SqlDataAdapter();
                DataTable dt2 = new DataTable();
                DataSet ds2 = new DataSet();

                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select OH.Id,OH.orderType,OH.LocationID,TDAD.DriverID from tOrderHead OH right outer join tDriverAssignDetail TDAD on OH.Id=TDAD.OrderID where TDAD.DriverID=" + driverid + " and TDAD.OrderID=" + Orderid + "";
                cmd2.Connection = conn;
                cmd2.Parameters.Clear();
                //cmd.Parameters.AddWithValue("param1", ResourceId);
                da2.SelectCommand = cmd2;
                da2.Fill(ds2, "tbl3");
                dt2 = ds2.Tables[0];
                string PaymentMode = "";
                decimal Amount = 0;
                if (dt2.Rows.Count > 0)
                {
                    string OrderType = dt2.Rows[0]["orderType"].ToString();

                    cmd.CommandType = CommandType.Text;
                    if (OrderType == "Ecommerce")
                    {
                        cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.GrandTotal,OH.OrderNo,ST.Status OrderStatus,CD.Name CustomerName,CD.Address,CD.ContactNo,CD.PhotoIdtype,CD.DeliveryType,PD.PaymentMode,PD.Remark from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mCustomerDetail CD on OH.Id=CD.OrderID left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentDetail PD on OH.Id=PD.OrderID where DA.DriverID=" + driverid + " and OH.Id=" + Orderid + "";
                    }
                    else
                    {
                        cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.GrandTotal,OH.OrderNo ,ST.Status OrderStatus,OH.ContactId1,CASE TA.AddressType when 'Location' then TA.ContactName when 'none' then TCPD.Name when 'General Address' then TCPD.Name End CustomerName,CASE TA.AddressType when 'none' then TA.AddressLine1 when 'Location' then TA.AddressLine1 when 'General Address' then TA.AddressLine1 End Address,CASE TA.AddressType when 'Location' then TA.MobileNo when 'none' then TCPD.MobileNo when 'General Address' then TCPD.MobileNo End ContactNo,'Regular' as DeliveryType,PMM.MethodName PaymentMode,SD.StatutoryValue StatutoryValue1,PMD.FieldName FieldName1,SD2.StatutoryValue StatutoryValue2,PMD2.FieldName FieldName2,MP.Remark,TD.DocumentType as PhotoIdType from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mPaymentMethodmain PMM ON OH.PaymentID=PMM.ID left outer join tAddress TA on OH.AddressId=TA.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tStatutoryDetail SD ON OH.Id=SD.ReferenceID and SD.Sequence=1 left outer join mPaymentMethodDetail PMD on SD.StatutoryID =PMD.ID and PMD.Sequence=1 left outer join tStatutoryDetail SD2 on OH.Id=SD2.ReferenceID and SD2.Sequence=2 left outer join mPaymentMethodDetail PMD2 on SD2.StatutoryID =PMD2.ID and PMD2.Sequence=2 left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentDetail MP on DA.OrderId=MP.OrderID left outer join tDocument TD on OH.Id=TD.ReferenceID and TD.Sequence=1 where DA.DriverID=" + driverid + " and OH.Id=" + Orderid + " ";
                        //cmd.CommandText = "select OH.Id,OH.Orderdate,OH.DeliveryDate,OH.Status,OH.CompletedDate,OH.GrandTotal,OH.OrderNo,ST.Status OrderStatus,OH.ContactId1,TCPD.Name as CustomerName,TA.AddressLine1 as Address, TCPD.MobileNo as ContactNo,'Regular' as DeliveryType,PMM.MethodName PaymentMode,SD.StatutoryValue StatutoryValue1,PMD.FieldName FieldName1,SD2.StatutoryValue StatutoryValue2,PMD2.FieldName FieldName2,MP.Remark,TD.DocumentType as PhotoIdType from torderhead OH left outer join mStatus ST on OH.Status=ST.ID left outer join mPaymentMethodmain PMM ON OH.PaymentID=PMM.ID left outer join tAddress TA on OH.AddressId=TA.ID left outer join tcontactpersondetail TCPD on OH.ContactId1=TCPD.ID left outer join tStatutoryDetail SD ON OH.Id=SD.ReferenceID and SD.Sequence=1 left outer join mPaymentMethodDetail PMD on SD.StatutoryID =PMD.ID and PMD.Sequence=1 left outer join tStatutoryDetail SD2 on OH.Id=SD2.ReferenceID and SD2.Sequence=2 left outer join mPaymentMethodDetail PMD2 on SD2.StatutoryID =PMD2.ID and PMD2.Sequence=2 left outer join tDriverAssignDetail DA on OH.Id=DA.OrderId left outer join mPaymentDetail MP on DA.OrderId=MP.OrderID left outer join tDocument TD on OH.Id=TD.ReferenceID and TD.Sequence=1 where DA.DriverID=" + driverid + " and OH.Id=" + Orderid + " ";
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
                        String OrderId = dt.Rows[0]["Id"].ToString();
                        String CustomerName = dt.Rows[0]["CustomerName"].ToString();
                        String Address = dt.Rows[0]["Address"].ToString();
                        String ContactNo = dt.Rows[0]["ContactNo"].ToString();
                        String OrderDate = dt.Rows[0]["Orderdate"].ToString();
                        String ExpOrderDate = dt.Rows[0]["DeliveryDate"].ToString();
                        String DeliveredDate = dt.Rows[0]["CompletedDate"].ToString();
                        String DeliveryType = dt.Rows[0]["DeliveryType"].ToString();
                        String DeliveryStatus = dt.Rows[0]["OrderStatus"].ToString();
                        String StatusID = dt.Rows[0]["Status"].ToString();
                        String Remark = dt.Rows[0]["Remark"].ToString();
                        String PhotoIdtype = dt.Rows[0]["PhotoIdtype"].ToString();

                        PaymentMode = dt.Rows[0]["PaymentMode"].ToString();
                        if (PaymentMode == "Credit Card" || PaymentMode == "Debit Card")
                        {
                            string[] ar = PaymentMode.Split(' ').ToArray();
                            PaymentMode = ar[1];
                        }
                        Amount = Convert.ToDecimal(dt.Rows[0]["GrandTotal"].ToString());
                        String OrderNo = dt.Rows[0]["OrderNo"].ToString();

                        // if (skuCode == "") skuCode = "N/A";

                        // RECORD LOOP
                        xmlString = xmlString + "<customerdetails>";
                        xmlString = xmlString + "<orderId>" + OrderNo + "</orderId>";
                        xmlString = xmlString + "<customerName>" + CustomerName + "</customerName>";
                        xmlString = xmlString + "<customerAddress>" + Address + "</customerAddress>";
                        xmlString = xmlString + "<contactNumber>" + ContactNo + "</contactNumber>";
                        xmlString = xmlString + "<orderDate>" + OrderDate + "</orderDate>";
                        if (StatusID == "8")
                        {
                            xmlString = xmlString + "<deliveredDate>" + DeliveredDate + "</deliveredDate>";

                        }
                        else
                        {
                            xmlString = xmlString + "<expOrderDate>" + ExpOrderDate + "</expOrderDate>";
                        }
                        xmlString = xmlString + "<deliveryMode>" + DeliveryType + "</deliveryMode>";
                        xmlString = xmlString + "<deliveryStatus>" + DeliveryStatus + "</deliveryStatus>";
                        xmlString = xmlString + "<photoIdType>" + PhotoIdtype + "</photoIdType>";
                        xmlString = xmlString + "<remark>" + Remark + "</remark>";
                        xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";
                        xmlString = xmlString + "</customerdetails>";
                        // RECORD LOOP         
                    }
                }

                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select OD.SkuId,OD.Prod_Name,OD.OrderQty,OD.UOMID,UM.Description from tOrderDetail OD left join mUOM UM on OD.UOMID=UM.Id where OrderHeadID=" + Orderid + " ";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                //cmd.Parameters.AddWithValue("param1", ResourceId);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl2");
                dt1 = ds1.Tables[0];
                int cntr1 = dt1.Rows.Count;
                if (cntr1 > 0)
                {
                    xmlString = xmlString + "<materialdetails>";
                    for (int i = 0; i < cntr1; i++)
                    {

                        String SkuId = dt1.Rows[i]["SkuId"].ToString();
                        String Prod_Name = dt1.Rows[i]["Prod_Name"].ToString();
                        String OrderQty = dt1.Rows[i]["OrderQty"].ToString();
                        String UOMID = dt1.Rows[i]["UOMID"].ToString();
                        String Description = dt1.Rows[i]["Description"].ToString();

                        xmlString = xmlString + "<productItem>";
                        xmlString = xmlString + "<prodId>" + SkuId + "</prodId>";
                        xmlString = xmlString + "<prodTitle>" + Prod_Name + "</prodTitle>";
                        xmlString = xmlString + "<qty>" + OrderQty + "</qty>";
                        xmlString = xmlString + "<unitId>" + UOMID + "</unitId>";
                        xmlString = xmlString + "<unitLabel>" + Description + "</unitLabel>";
                        xmlString = xmlString + "</productItem>";
                    }
                    xmlString = xmlString + "</materialdetails>";
                }

                xmlString = xmlString + " <paymentdetails>";
                xmlString = xmlString + "<paymentMode>" + PaymentMode + "</paymentMode>";
                xmlString = xmlString + "<paymentAmount>" + Amount + "</paymentAmount>";
                xmlString = xmlString + " </paymentdetails>";

            }
            else
            {
                // throw new ArgumentException("No Parameter Specified");
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