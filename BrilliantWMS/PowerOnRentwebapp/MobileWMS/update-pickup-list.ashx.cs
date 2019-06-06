using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;
using System.Web.SessionState;
using static System.Web.SessionState.IRequiresSessionState;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for update_shipping_list
    /// </summary>
    public class update_shipping_list : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string objname = "SalesOrder";
        long OrderID = 5528, WarehouseID = 1, UserID = 10639;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long PKID = 0;
            try
            {
                if (context.Request.Form["oid"] != null)
                {
                    OrderID = long.Parse(context.Request.Form["oid"]);
                }
                if (context.Request.Form["wrid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.Form["wrid"]);
                }
                if (context.Request.Form["uid"] != null)
                {
                    UserID = long.Parse(context.Request.Form["uid"]);
                }
                if (context.Request.Form["objname"] != null)
                {
                    objname = context.Request.Form["objname"].ToString();
                }

                DataSet dsUserDetail = new DataSet();
                long CustomerID, CompanyID;
                dsUserDetail = GetUserDetails(UserID);
                CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
                CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

                iOutboundClient outbound = new iOutboundClient();
                string userName = GetUserID(UserID);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                tPickUpHead pk = new tPickUpHead();
                pk.ObjectName = objname;
                pk.OID = OrderID;
                pk.PickUpDate = DateTime.Now;
                pk.PickUpBy = UserID;
                pk.Remark = "";
                pk.CreatedBy = UserID;
                pk.CreationDate = DateTime.Now;
                string WorkFlow = "", NextObject = "";
                if (objname == "SalesOrder")
                {
                    WorkFlow = "Outbound";
                    NextObject = "QC";
                }
                if (objname == "Transfer")
                {
                    WorkFlow = "Transfer";
                    NextObject = "QCOut";
                }
                int count=GetWorkFlow(CompanyID, CustomerID, WorkFlow, NextObject, profile.DBConnection._constr);
                if (objname=="SalesOrder")
                {
                    if(count == 0)
                    {
                        pk.Status = 32;
                    }
                    else
                    {
                        pk.Status = 38;
                    }
                   
                }
                if(objname=="Transfer")
                {
                    if (count == 0)
                    {
                        pk.Status = 58;
                    }
                    else
                    {
                        pk.Status = 57;
                    }
                }
                pk.CompanyID = CompanyID;
                pk.CustomerID = CustomerID;
                pk.PickUpStatus = true;
                pk.IsPick = 1;
                //pk.IsRead = true;
                DataTable dtpkup = new DataTable();
                dtpkup = GetPickUpStatus(OrderID, objname, UserID);
                if(dtpkup.Rows.Count>0)
                {
                    pk.ID=Convert.ToInt64(dtpkup.Rows[0]["ID"]);
                }
                UpdateRemQtyPKUP(OrderID, objname,CompanyID,CustomerID);
                UpdateStock(OrderID, objname, CompanyID, CustomerID);
                PKID = outbound.SavetPickUpHead(pk, profile.DBConnection._constr);
                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";
                if (PKID>0)
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
            { Login.Profile.ErrorHandling(ex, "update-shiping-list", "GetPickUpStatus"); }
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
            { Login.Profile.ErrorHandling(ex, "update-pickup-list", "GetWorkFlow"); }
            finally
            { }
            return count;
        }

        public void UpdateStock(long oid, string obj, long compid, long custid)
        {
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdateStock";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@CompanyID", compid);
                cmdDetail.Parameters.AddWithValue("@CustomerID", custid);
                cmdDetail.Connection.Open();
                cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-shipping-list", "UpdateRemQtyPKUP"); }
            finally
            { }
        }
        public void UpdateRemQtyPKUP(long oid, string obj,long compid,long custid)
        {
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_UpdateRemQtyPKUP";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@CompanyID", compid);
                cmdDetail.Parameters.AddWithValue("@CustomerID", custid);
                cmdDetail.Connection.Open();
                cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-shipping-list", "UpdateRemQtyPKUP"); }
            finally
            { }
        }
        public DataTable GetPickUpStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();

            long grnid = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetPickUpStatusNew";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "update-shipping-list", "GetPickUpStatus"); }
            finally
            { }
            return dt1;
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
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%Pick Up%' ";
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
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
            { Login.Profile.ErrorHandling(ex, "update-shiping-list", "GetUserDetails"); }
            finally
            { }
            return ds1;
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