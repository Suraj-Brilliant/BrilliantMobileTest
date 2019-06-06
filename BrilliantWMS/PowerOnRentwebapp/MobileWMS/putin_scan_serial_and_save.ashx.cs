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
    /// Summary description for putin_scan_serial_and_save
    /// </summary>
    public class putin_scan_serial_and_save : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, CustomerID = 0, CompanyID = 0,LocID;

        string objectName = "";
        string objname = "", serialno = "", page = "",Batch="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long PTID = 0;
            try
            {
                if (context.Request.QueryString["objname"] != null)
                {
                    objname = context.Request.QueryString["objname"].ToString();
                }
                if (context.Request.QueryString["oid"] != null)
                {
                    orderID = long.Parse(context.Request.QueryString["oid"]);
                }
                if (context.Request.QueryString["wrid"] != null)
                {
                    WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
                }
                if (context.Request.QueryString["serialno"] != null)
                {
                    serialno = (context.Request.QueryString["serialno"]).ToString();
                }
                if (context.Request.QueryString["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.QueryString["uid"]);
                }
                if (context.Request.QueryString["page"] != null)
                {
                    page = (context.Request.QueryString["page"]).ToString();
                }
                if (context.Request.QueryString["loc"] != null)
                {
                    LocID = Convert.ToInt64(context.Request.QueryString["loc"]);
                }
                if (context.Request.QueryString["batch"] != null)
                {
                    Batch = (context.Request.QueryString["batch"]).ToString();
                }

                DataSet dsUserDetail = new DataSet();
                dsUserDetail = GetUserDetails(UserID);
                CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
                CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

                long PrdID = 0;
                PrdID = GetPrdID(serialno, CompanyID, CustomerID);
                if (PrdID == 0)
                {
                    PrdID = GetPrdIDNew(serialno, CompanyID, CustomerID);
                }
                if (PrdID == 0)
                {
                    PrdID = GetPrdIDNewCode(serialno, CompanyID, CustomerID);
                }
                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";
                if (PrdID == 0)
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"SKU Not Available\"\n";
                }
                else
                {
                    DataTable dtorder = new DataTable();
                    dtorder = CheckSKUInOrder(PrdID, orderID, objname);
                    if (dtorder.Rows.Count > 0)
                    {
                        DataTable dtsrorder = new DataTable();
                        dtsrorder = CheckSrNoInOrder(orderID, serialno, objname, PrdID);
                        if(dtsrorder.Rows.Count>0)
                        {
                            DataTable dtsrno = new DataTable();
                            dtsrno = CheckDuplicateSrNo(serialno, CompanyID, CustomerID, page, objname);
                            if (dtsrno.Rows.Count > 0)
                            {
                                jsonString = jsonString + "\"status\": \"failed\",\n";
                                jsonString = jsonString + "\"reason\": \"Serial number already used\"\n";
                            }
                            else
                            {
                                decimal ptqty = GetPtQty(orderID, objname, PrdID);
                                decimal oqty = GetOrderQty(orderID, objname, PrdID);
                                if (ptqty >= oqty)
                                {
                                    jsonString = jsonString + "\"status\": \"failed\",\n";
                                    jsonString = jsonString + "\"reason\": \"You have already reached max count\"\n";
                                }
                                else
                                {
                                    tPutInHead ph = new tPutInHead();
                                    string userName = GetUserID(UserID);
                                    CustomProfile profile = CustomProfile.GetProfile(userName);
                                    iInboundClient Inbound = new iInboundClient();
                                    ph.ObjectName = objname;
                                    ph.OID = orderID;
                                    ph.PutInDate = DateTime.Now;
                                    ph.PutInBy = UserID;
                                    ph.Remark = "";
                                    ph.CreatedBy = UserID;
                                    ph.CreationDate = DateTime.Now;
                                    ph.Status = getStatus(objname);
                                    ph.Company = CompanyID;
                                    ph.CustomerID = CustomerID;
                                    ph.OrderFrom = "Mobile";
                                    PTID= GetPTINStatus(orderID, objname, UserID);
                                    if(PTID==0)
                                    {
                                        PTID=Inbound.SavetPutInHead(ph, profile.DBConnection._constr);
                                    }
                                    if(PTID>0)
                                    {
                                        int save = SavePutInDetail(orderID, PTID, PrdID, LocID, 1, Batch);
                                        SaveLottablePutIn(orderID, serialno, objname, PTID, PrdID, 1, CompanyID, CustomerID, UserID, LocID, Batch);
                                        if (save > 0)
                                        {
                                            jsonString = jsonString + "\"status\": \"success\",\n";
                                            jsonString = jsonString + "\"reason\": \"\"\n";
                                        }
                                        else
                                        {
                                            jsonString = jsonString + "\"status\": \"failed\",\n";
                                            jsonString = jsonString + "\"reason\": \"Server error occured\"\n";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            jsonString = jsonString + "\"status\": \"failed\",\n";
                            jsonString = jsonString + "\"reason\": \"Serial No. Not Available In Order\"\n";
                        }
                    }
                    else
                    {
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"SKU Not Available In Order\"\n";
                    }
                }

                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "ProcessRequest"); }
            finally
            { }
        }

        public int SaveLottablePutIn(long oid,string srno,string obj, long ptid, long prodid,decimal qty,long compid,long custid,long uid, long locid,string batch)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_SaveLottablePutIn";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@serialno", srno);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@ptid", ptid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@uid", uid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@batch", batch);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "SavePutInDetail"); }
            finally
            { }
            return result;
        }
        public int SavePutInDetail(long oid,long ptid,long prodid,long locid,decimal qty,string batch)
        {
            int result = 0;
            try
            {
                SqlCommand cmdDetail = new SqlCommand();
                SqlDataAdapter daDetail = new SqlDataAdapter();
                DataSet dsDetail = new DataSet();
                DataTable dtDetail = new DataTable();
                SqlConnection conn = new SqlConnection(strcon);
                cmdDetail.CommandType = CommandType.StoredProcedure;
                cmdDetail.CommandText = "SP_SavePutInDetail";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@ptid", ptid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@batch", batch);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "SavePutInDetail"); }
            finally
            { }
            return result;
        }
        public long GetPTINStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();

            long ptid = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetPTINStatus";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    ptid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPickUpStatus"); }
            finally
            { }
            return ptid;
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
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%PutIn%' ";
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
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
        }
        public decimal GetOrderQty(long oid, string obj, long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            decimal OQty = 1;
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetOrderQtyPutin";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    OQty = Convert.ToDecimal(dt1.Rows[0]["OQty"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetOrderQty"); }
            finally
            { }
            return OQty;
        }
        public decimal GetPtQty(long oid, string obj, long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            decimal PtQty = 0;
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetPtQty";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["PtQty"].ToString() != "")
                    {
                        PtQty = Convert.ToDecimal(dt1.Rows[0]["PtQty"]);
                    }
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPtQty"); }
            finally
            { }
            return PtQty;
        }
        public DataTable CheckDuplicateSrNo(string srno, long compid, long custid, string page, string obj)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_CheckDuplicateSrNo";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@srno", srno);
                cmd1.Parameters.AddWithValue("@compid", compid);
                cmd1.Parameters.AddWithValue("@custid", custid);
                cmd1.Parameters.AddWithValue("@page", page);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "CheckSKUInOrder"); }
            finally
            { }
            return dt1;
        }
        public DataTable CheckSrNoInOrder(long oid,string srno, string obj, long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_CheckSrNoInOrder";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@srno", srno);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
               
                
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "CheckSKUInOrder"); }
            finally
            { }
            return dt1;
        }
        public DataTable CheckSKUInOrder(long prdid, long oid, string obj)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_CheckSKUInOrderPutIn";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "CheckSKUInOrder"); }
            finally
            { }
            return dt1;
        }
        private long GetPrdIDNewCode(string prdcode, long companyID, long customerID)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_ProdIDByCode";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdcode", prdcode);
                cmd1.Parameters.AddWithValue("@compid", companyID);
                cmd1.Parameters.AddWithValue("@custid", customerID);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    id = Convert.ToInt64(ds1.Tables[0].Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPrdIDNewCode"); }
            finally
            { }
            return id;
        }
        private long GetPrdIDNew(string prdBarcode, long companyID, long customerID)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetProductCode";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@code", prdBarcode);
                cmd1.Parameters.AddWithValue("@compid", companyID);
                cmd1.Parameters.AddWithValue("@custid", customerID);
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    id = Convert.ToInt64(ds1.Tables[0].Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPrdIDNew"); }
            finally
            { }
            return id;
        }
        public long GetPrdID(string prdBarcode, long CompanyID, long CustomerID)
        {
            long PrdID = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                SqlCommand cmd2 = new SqlCommand();
                SqlDataAdapter da2 = new SqlDataAdapter();
                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "SP_GetGRNBarcode";
                cmd2.Connection = conn;
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@Barcode", prdBarcode);
                cmd2.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd2.Parameters.AddWithValue("@CustomerID", CustomerID);
                da2.SelectCommand = cmd2;
                da2.Fill(ds2, "tbl1");
                dt2 = ds2.Tables[0];
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    PrdID = long.Parse(ds2.Tables[0].Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetPrdID"); }
            finally
            { }
            return PrdID;
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
            { Login.Profile.ErrorHandling(ex, "putin_scan_serial_and_save", "GetUserDetails"); }
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