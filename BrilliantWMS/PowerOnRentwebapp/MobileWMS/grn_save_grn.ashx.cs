using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for grn_save_grn
    /// </summary>
    public class grn_save_grn : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        string  batchcode = "", remark = "", airwaybill = "", shippingtype = "", dockno = "", lrno = "", intime = "", outtime = "", udr = "";

        long orderID = 0, UserID = 0, CustomerID = 0, CompanyID = 0;
        string objectName = "";
        string objname = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long GRNID = 0;
            try
            {
                DateTime shippingdate = DateTime.Now;
                DateTime expecteddeliverydate = DateTime.Now;
                if (context.Request.QueryString["userid"] != null)
                {
                    UserID = long.Parse(context.Request.QueryString["userid"]);
                }
                else if (context.Request.Form["userid"] != null)
                {
                    UserID = long.Parse(context.Request.Form["userid"]);
                }
                if (context.Request.QueryString["oid"] != null)
                {
                    orderID = long.Parse(context.Request.QueryString["oid"]);
                }
                else if (context.Request.Form["oid"] != null)
                {
                    orderID = long.Parse(context.Request.Form["oid"]);
                }
                if (context.Request.QueryString["objectname"] != null)
                {
                    objname = context.Request.QueryString["objectname"];
                }
                else if (context.Request.Form["objectname"] != null)
                {
                    objname = context.Request.Form["objectname"];
                }
                if (context.Request.QueryString["batchcode"] != null)
                {
                    batchcode = context.Request.QueryString["batchcode"];
                }
                else if (context.Request.Form["batchcode"] != null)
                {
                    batchcode = context.Request.Form["batchcode"];
                }
                if (context.Request.QueryString["remark"] != null)
                {
                    remark = context.Request.QueryString["remark"];
                }
                else if (context.Request.Form["remark"] != null)
                {
                    remark = context.Request.Form["remark"];
                }
                if (context.Request.QueryString["airwaybill"] != null)
                {
                    airwaybill = context.Request.QueryString["airwaybill"];
                }
                else if (context.Request.Form["airwaybill"] != null)
                {
                    airwaybill = context.Request.Form["airwaybill"];
                }
                if (context.Request.QueryString["shippingtype"] != null)
                {
                    shippingtype = context.Request.QueryString["shippingtype"];
                }
                else if (context.Request.Form["shippingtype"] != null)
                {
                    shippingtype = context.Request.Form["shippingtype"];
                }

                if (context.Request.QueryString["shippingdate"] != null)
                {
                    if (Convert.ToString(context.Request.QueryString["shippingdate"]) == "N/A")
                    { }
                    else
                    {
                        shippingdate = Convert.ToDateTime(context.Request.QueryString["shippingdate"]);
                    }
                }
                else if (context.Request.Form["shippingdate"] != null)
                {
                    if (context.Request.Form["shippingdate"] == "N/A")
                    {
                    }
                    else
                    {
                        shippingdate = Convert.ToDateTime(context.Request.Form["shippingdate"]);
                    }

                }

                if (context.Request.QueryString["expecteddeliverydate"] != null)
                {
                    if (Convert.ToString(context.Request.QueryString["expecteddeliverydate"]) == "N/A")
                    { }
                    else
                    {
                        expecteddeliverydate = Convert.ToDateTime(context.Request.QueryString["expecteddeliverydate"]);
                    }
                }
                else if (context.Request.Form["expecteddeliverydate"] != null)
                {
                    if (context.Request.Form["expecteddeliverydate"] == "N/A")
                    { }
                    else
                    {
                        expecteddeliverydate = Convert.ToDateTime(context.Request.Form["expecteddeliverydate"]);
                    }
                }
                if (context.Request.QueryString["dockno"] != null)
                {
                    dockno = context.Request.QueryString["dockno"];
                }
                else if (context.Request.Form["dockno"] != null)
                {
                    dockno = context.Request.Form["dockno"];
                }
                if (context.Request.QueryString["lrno"] != null)
                {
                    lrno = context.Request.QueryString["lrno"];
                }
                else if (context.Request.Form["lrno"] != null)
                {
                    lrno = context.Request.Form["lrno"];
                }
                if (context.Request.QueryString["intime"] != null)
                {
                    intime = context.Request.QueryString["intime"];
                }
                else if (context.Request.Form["intime"] != null)
                {
                    intime = context.Request.Form["intime"];
                }
                if (context.Request.QueryString["outtime"] != null)
                {
                    outtime = context.Request.QueryString["outtime"];
                }
                else if (context.Request.Form["outtime"] != null)
                {
                    outtime = context.Request.Form["outtime"];
                }
                if (context.Request.QueryString["other"] != null)
                {
                    udr = context.Request.QueryString["other"];
                }
                else if (context.Request.Form["other"] != null)
                {
                    udr = context.Request.Form["other"];
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";

                iInboundClient Inbound = new iInboundClient();
                string userName = GetUserID(UserID);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                DataSet dsUserDetail = new DataSet();
                dsUserDetail = GetUserDetails(UserID);
                CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
                CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());
                tGRNHead GRNHead = new tGRNHead();
                GRNHead.ObjectName = objname;
                GRNHead.OID = orderID;
                GRNHead.GRNDate = DateTime.Now;
                GRNHead.ReceivedBy = UserID;
                GRNHead.CreatedBy = UserID;
                GRNHead.Creationdate = DateTime.Now;
                GRNHead.CustomerID = CustomerID;
                GRNHead.CompanyID = CompanyID;
                string WorkFlow = "", NextObject = "";
                int count = 0;
                if (objname == "PurchaseOrder")
                {
                    WorkFlow = "Inbound";
                    NextObject = "QC";
                }
                if (objname == "Transfer")
                {
                    WorkFlow = "Transfer";
                    NextObject = "QC";
                }
                if (objname == "SalesReturn")
                {
                    WorkFlow = "Return";
                    NextObject = "QC";
                }
                count = GetWorkFlow(profile.Personal.CompanyID, profile.Personal.CustomerId, WorkFlow, NextObject, profile.DBConnection._constr);
                if(objname== "PurchaseOrder")
                {
                    if(count==0)
                    {
                        GRNHead.Status = 33;
                    }
                    else
                    {
                        GRNHead.Status = 31;
                    }
                }
                if (objname == "Transfer")
                {
                    if (count == 0)
                    {
                        GRNHead.Status = 71;
                    }
                    else
                    {
                        GRNHead.Status = 60;
                    }
                }
                if (objname == "SalesReturn")
                {
                    if (count == 0)
                    {
                        GRNHead.Status = 53;
                    }
                    else
                    {
                        GRNHead.Status = 52;
                    }
                }
                GRNHead.OrderFrom = "Mobile";
                GRNHead.ShipID = "";
                if (remark == "N/A")
                {
                    remark = "";
                }
                GRNHead.Remark = remark;
                if (airwaybill == "N/A")
                {
                    airwaybill = "";
                }
                GRNHead.AirwayBill = airwaybill;
                if (shippingtype == "N/A")
                {
                    shippingtype = "";
                }
                GRNHead.ShippingType = shippingtype;
                GRNHead.ShippingDate = shippingdate;
                GRNHead.TransporterRemark = "";
                if (lrno == "N/A")
                {
                    lrno = "";
                }
                GRNHead.LRNo = lrno;
                if (intime == "N/A")
                {
                    intime = "";
                }
                GRNHead.InTime = intime;
                if (outtime == "N/A")
                {
                    outtime = "";
                }
                GRNHead.OutTime = outtime;
                GRNHead.ExpDeliveryDate = expecteddeliverydate;
                GRNHead.IsRead = false;
                GRNHead.Other = udr;
                GRNHead.IsGRN = 1;
                DataTable dtgrn = new DataTable();
                dtgrn = GetGRNStatus(orderID, objname, UserID);
                if (dtgrn.Rows.Count>0)
                {
                    batchcode = dtgrn.Rows[0]["BatchNo"].ToString();
                    GRNID = Convert.ToInt64(dtgrn.Rows[0]["ID"]);
                }
                GRNHead.ID = GRNID;
                GRNHead.BatchNo = batchcode;
                UpdateRemQty(orderID, objname);
                GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);
                if (GRNID>0)
                {
                    jsonString = jsonString + "\"status\": \"success\",\n";
                    jsonString = jsonString + "\"reason\": \"\"\n";
                }
                else
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                }
                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn_save_grn", "ProcessRequest"); }
            finally
            { }
        }

        public int GetWorkFlow(long CompID, long CustID, string WorkFlow, string Obj, string[] conn)
        {
            int count = 0;
            try
            {
                SqlConnection conn1 = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_InboundWorkFolw";
                cmd.Connection = conn1;
                cmd.Parameters.AddWithValue("@CompanyID", CompID);
                cmd.Parameters.AddWithValue("@CustomerID", CustID);
                cmd.Parameters.AddWithValue("@WorkFlow", WorkFlow);
                cmd.Parameters.AddWithValue("@Object", Obj);
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetWorkFlow"); }
            finally
            { }
            return count;
        }
        public long getStatus(string objNM)
        {
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();

            long StatusID = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%GRN%' ";
                cmd2.Connection = conn;
                cmd2.Parameters.Clear();
                da2.SelectCommand = cmd2;
                da2.Fill(ds2, "tbl1");
                dt2 = ds2.Tables[0];
                StatusID = long.Parse(dt2.Rows[0]["ID"].ToString());
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "getStatus"); }
            finally
            { }
            return StatusID;
        }
        public DataSet GetUserDetails(long UserID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_getUserDetail";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@userid", UserID);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn_save_grn", "GetUserDetails"); }
            finally
            { }
            return ds1;
        }
        //private string GEtBatchcode(long orderID)
        //{
        //    string batchcode = "";
        //    try
        //    {
        //        SqlCommand cmd1 = new SqlCommand();
        //        SqlDataAdapter da1 = new SqlDataAdapter();
        //        DataSet ds1 = new DataSet();
        //        DataTable dt1 = new DataTable();
        //        SqlDataReader dr1;
        //        SqlConnection conn = new SqlConnection(strcon);
        //        string comapanyid = "0";
        //        string customerid = "0";
        //        cmd1.CommandType = CommandType.Text;
        //        //cmd1.CommandText = "select companyid ,customerid from tpurchaseorderhead where id="+ orderID + " ";
        //        cmd1.CommandText = "select companyid ,customerid,Object as Objectname from tpurchaseorderhead where id=" + orderID + " union all select companyid, customerid, Objectname from tTransferhead where id = " + orderID + " union all select companyid, customerid, Object as Objectname from tOrderhead where id = " + orderID + "";
        //        cmd1.Connection = conn;
        //        cmd1.Parameters.Clear();
        //        da1.SelectCommand = cmd1;
        //        da1.Fill(ds1, "tbl1");
        //        dt1 = ds1.Tables[0];
        //        if (dt1.Rows.Count > 0)
        //        {
        //            comapanyid = dt1.Rows[0]["companyid"].ToString();
        //            customerid = dt1.Rows[0]["customerid"].ToString();
        //            objectName = dt1.Rows[0]["Objectname"].ToString();
        //        }
        //        batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
        //    }
        //    catch (Exception ex)
        //    { Login.Profile.ErrorHandling(ex, "grn_save_grn", "GEtBatchcode"); }
        //    finally
        //    { }


        //    return batchcode;
        //}
        public void UpdateRemQty(long oid,string obj)
        {
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdateRemQty";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Connection.Open();
                cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "grn_save_grn", "UpdateRemQty"); }
            finally
            { }
        }
        public string GetUserID(long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string username = "";
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select top (1) UserName from mPassWordDetails where UserProfileID=" + uid + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    username = dt1.Rows[0]["UserName"].ToString();
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
        }
        public DataTable GetGRNStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetGRNStatusNew";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "getStatus"); }
            finally
            { }
            return dt1;
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