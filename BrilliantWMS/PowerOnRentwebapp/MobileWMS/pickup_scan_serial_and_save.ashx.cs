using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for pickup_scan_serial_and_save
    /// </summary>
    public class pickup_scan_serial_and_save : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, CustomerID = 0, CompanyID = 0,LocID=0,IsBatch=0;

        string objectName = "";
        string objname = "", serialno = "", page = "",batchflag="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long PKID = 0;
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
                if(PrdID==0)
                {
                    string[] prodid = serialno.Split('_');
                    string batch = "Batch-"+prodid[1].ToString();
                    PrdID = GetPrdIDNewCodeMarvel(Convert.ToInt64(prodid[0]), CompanyID, CustomerID);
                    if(PrdID>0)
                    {
                        batchflag = "yes";
                        if (objname == "SalesOrder")
                        {
                            IsBatch = CheckSKUInBatch(orderID.ToString(), "", PrdID, batch);
                        }
                        else
                        {
                            IsBatch = CheckSKUInBatch("", orderID.ToString(), PrdID, batch);
                        }
                    }
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
                else if(IsBatch==0 && batchflag=="yes")
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"SKU Not Available in Batch\"\n";
                }
                else
                {
                    DataTable dtorder = new DataTable();
                    dtorder = CheckSKUInOrder(PrdID, orderID, objname);
                    if(dtorder.Rows.Count > 0)
                    {
                        DataTable dtstock = new DataTable();
                        dtstock = SrNoInStock(serialno, CustomerID,LocID,PrdID);
                        if(dtstock.Rows.Count>0)
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
                                decimal pkqty = GetPkQty(orderID, objname, PrdID);
                                decimal oqty = GetOrderQty(orderID, objname, PrdID);
                                if (pkqty >= oqty)
                                {
                                    jsonString = jsonString + "\"status\": \"failed\",\n";
                                    jsonString = jsonString + "\"reason\": \"You have already reached max count\"\n";
                                }
                                else
                                {
                                    iOutboundClient outbound = new iOutboundClient();
                                    string userName = GetUserID(UserID);
                                    CustomProfile profile = CustomProfile.GetProfile(userName);
                                    tPickUpHead pk = new tPickUpHead();
                                    pk.ObjectName = objname;
                                    pk.OID = orderID;
                                    pk.PickUpDate = DateTime.Now;
                                    pk.PickUpBy = UserID;
                                    pk.Remark = "";
                                    pk.CreatedBy = UserID;
                                    pk.CreationDate = DateTime.Now;
                                    pk.Status = getStatus(objname);
                                    pk.CompanyID = CompanyID;
                                    pk.CustomerID = CustomerID;
                                    pk.PickUpStatus = true;
                                    PKID = GetPickUpStatus(orderID,objname,UserID);
                                    if(PKID==0)
                                    {
                                        PKID=outbound.SavetPickUpHead(pk, profile.DBConnection._constr);
                                    }
                                    if(PKID>0)
                                    {
                                        int save = SavePickUpDetail(orderID, objname, PKID, PrdID, 1, LocID, serialno);
                                        SaveLottablePickUp(serialno, objname, PKID, PrdID, 1, CompanyID, CustomerID,LocID);
                                        iInboundClient Inbound = new iInboundClient();
                                        string WorkFlow = "", NextObject = "";
                                        int count = 0;
                                        long QCID = 0;
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
                                        count = GetWorkFlow(profile.Personal.CompanyID, profile.Personal.CustomerId, WorkFlow, NextObject, profile.DBConnection._constr);
                                        if(count==0)
                                        {
                                            WMSOutbound.tQualityControlHead QCHead = new WMSOutbound.tQualityControlHead();
                                            QCHead.CreatedBy = profile.Personal.UserID;
                                            QCHead.Creationdate = DateTime.Now;
                                            QCHead.ObjectName = objname;
                                            QCHead.OID = PKID;
                                            QCHead.QCDate = DateTime.Now;
                                            QCHead.QCBy = profile.Personal.UserID;
                                            QCHead.Remark = "";

                                            if (objname == "SalesOrder") { QCHead.Status = 32; }
                                            else if (objname == "Transfer") { QCHead.Status = 58; }

                                            QCHead.Company = profile.Personal.CompanyID;
                                            QCHead.CustomerID = profile.Personal.CustomerId;
                                            QCID = GetQCStatus(PKID, objname, UserID);
                                            if(QCID==0)
                                            {
                                                QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                                            }
                                            if(QCID>0)
                                            {
                                                SaveQCDetails(PKID, QCID,PrdID,1);
                                            }
                                        }
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
                            jsonString = jsonString + "\"reason\": \"Serial No. not available in Stock OR Location\"\n";
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "ProcessRequest"); }
            finally
            { }
        }

        private long CheckSKUInBatch(string soid,string trid, long prdid,string batch)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "WMS_SP_PickUpListMarvel";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@soid", soid);
                cmd1.Parameters.AddWithValue("@trid", trid);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@batch", batch);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    id = Convert.ToInt64(dt1.Rows[0]["ID"].ToString());
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "CheckSKUInBatch"); }
            finally
            { }
            return id;
        }
        private long GetPrdIDNewCodeMarvel(long prdid, long companyID, long customerID)
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
                cmd1.CommandText = "SP_ProdIDByCodeMarvel";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdid", prdid);
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPrdIDNewCode"); }
            finally
            { }
            return id;
        }
        public void SaveQCDetails(long pkid,long qcid,long prdid,decimal qty)
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
                cmdDetail.CommandText = "SP_SaveQCDetails";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@pkid", pkid);
                cmdDetail.Parameters.AddWithValue("@qcid", qcid);
                cmdDetail.Parameters.AddWithValue("@prdid", prdid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "SaveQCDetails"); }
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetWorkFlow"); }
            finally
            { }
            return count;
        }
        public int SaveLottablePickUp(string serialno, string obj, long grnid, long prodid, decimal qty, long compid, long custid,long locid)
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
                cmdDetail.CommandText = "SP_SaveLottablePickUp";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@serialno", serialno);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "SaveLottable"); }
            finally
            { }
            return result;
        }
        public int SavePickUpDetail(long oid, string obj, long grnid, long prodid, decimal qty,long locid,string srno)
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
                cmdDetail.CommandText = "SP_SavePickUpDetails";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@pkid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@locid", locid);
                cmdDetail.Parameters.AddWithValue("@srno", srno);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "SaveGRNDetail"); }
            finally
            { }
            return result;
        }

        public long GetQCStatus(long oid, string objname, long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();

            long grnid = 0;
            try
            {
                SqlConnection conn = new SqlConnection(strcon);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetQCStatus";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@objname", objname);
                cmd1.Parameters.AddWithValue("@uid", uid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    grnid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPickUpStatus"); }
            finally
            { }
            return grnid;
        }
        public long GetPickUpStatus(long oid, string objname, long uid)
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
                if (dt1.Rows.Count > 0)
                {
                    grnid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPickUpStatus"); }
            finally
            { }
            return grnid;
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
                cmd1.CommandText = "SP_GetOrderQty";
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetOrderQty"); }
            finally
            { }
            return OQty;
        }
        public decimal GetPkQty(long oid,string obj,long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            decimal PkQty = 0;
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetPkQty";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
                if(dt1.Rows.Count>0)
                {
                    if(dt1.Rows[0]["PKQty"].ToString()!="")
                    {
                        PkQty = Convert.ToDecimal(dt1.Rows[0]["PKQty"]);
                    }
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPkQty"); }
            finally
            { }
            return PkQty;
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "CheckSKUInOrder"); }
            finally
            { }
            return dt1;
        }
        public DataTable SrNoInStock(string srno,long custid,long locid,long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_SrNoInStock";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@serialno", srno);
                cmd1.Parameters.AddWithValue("@custid", custid);
                cmd1.Parameters.AddWithValue("@locid", locid);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "SrNoInStock"); }
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
                cmd1.CommandText = "SP_CheckSKUInOrder";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "CheckSKUInOrder"); }
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPrdIDNewCode"); }
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPrdIDNew"); }
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetPrdID"); }
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
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "GetUserDetails"); }
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