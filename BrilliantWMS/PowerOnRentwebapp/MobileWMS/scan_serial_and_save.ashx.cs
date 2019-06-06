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
    /// Summary description for scan_serial_and_save
    /// </summary>
    public class scan_serial_and_save : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable(); 
         SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, orderID = 0, UserID = 0, CustomerID=0, CompanyID=0;

        string objectName = "";
        string objname = "", serialno = "",page="";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long GRNID = 0;
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

                DataSet dsUserDetail = new DataSet();
                dsUserDetail = GetUserDetails(UserID);
                CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
                CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

                long PrdID = 0;
                PrdID = GetPrdID(serialno, CompanyID, CustomerID);
                if(PrdID==0)
                {
                    PrdID = GetPrdIDNew(serialno, CompanyID, CustomerID);
                }
                if(PrdID==0)
                {
                    PrdID = GetPrdIDNewCode(serialno, CompanyID, CustomerID);
                }

                context.Response.ContentType = "text/plain";
                String jsonString = String.Empty;
                jsonString = "{\n";   /*json Loop Start*/
                jsonString = jsonString + "\"result\":[{\n";
                if(PrdID==0)
                {
                    jsonString = jsonString + "\"status\": \"failed\",\n";
                    jsonString = jsonString + "\"reason\": \"SKU Not Available\"\n";
                }
                else
                {
                    DataTable dtorder = new DataTable();
                    dtorder = CheckSKUInOrder(PrdID, orderID, objname);
                    if(dtorder.Rows.Count>0)
                    {
                        DataTable dtsrno = new DataTable();
                        dtsrno = CheckDuplicateSrNo(serialno, CompanyID, CustomerID, page, objname);
                        if(dtsrno.Rows.Count>0)
                        {
                            jsonString = jsonString + "\"status\": \"failed\",\n";
                            jsonString = jsonString + "\"reason\": \"Serial number already used\"\n";
                        }
                        else
                        {
                            DataTable dtcurcnt = new DataTable();
                            dtcurcnt = GetCurrentCount(orderID, objname, PrdID);
                            decimal grnqty = 0;
                            decimal oqty = 1;
                            if(dtcurcnt.Rows.Count>0)
                            {
                                grnqty = Convert.ToDecimal(dtcurcnt.Rows[0]["GRNQty"]);
                                oqty= Convert.ToDecimal(dtcurcnt.Rows[0]["OQty"]);
                            }
                            if(grnqty>=oqty)
                            {
                                jsonString = jsonString + "\"status\": \"failed\",\n";
                                jsonString = jsonString + "\"reason\": \"You have already reached max count\"\n";
                            }
                            else
                            {
                                iInboundClient Inbound = new iInboundClient();
                                string userName = GetUserID(UserID);
                                CustomProfile profile = CustomProfile.GetProfile(userName);
                                tGRNHead GRNHead = new tGRNHead();
                                GRNHead.ObjectName = objname;
                                GRNHead.OID = orderID;
                                GRNHead.GRNDate = DateTime.Now;
                                GRNHead.ReceivedBy = UserID;
                                GRNHead.BatchNo = GEtBatchcode(orderID);
                                GRNHead.CreatedBy = UserID;
                                GRNHead.Creationdate = DateTime.Now;
                                GRNHead.CustomerID = CustomerID;
                                GRNHead.CompanyID = CompanyID;
                                GRNHead.Status = getStatus(objname);
                                GRNHead.OrderFrom = "Mobile";
                                GRNID = GetGRNStatus(orderID, objname, UserID);
                                if (GRNID == 0)
                                {
                                    GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);
                                }
                                if (GRNID > 0)
                                {
                                    int save = SaveGRNDetail(orderID, objname, GRNID, PrdID, 1);
                                    SaveLottable(serialno, objname, GRNID, PrdID, 1, CompanyID, CustomerID, WarehouseID);
                                    string WorkFlow = "", NextObject = "";
                                    int count = 0;
                                    long QCID = 0;
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
                                    if (count == 0)
                                    {
                                        tQualityControlHead QCHead = new tQualityControlHead();
                                        QCHead.CreatedBy = profile.Personal.UserID;
                                        QCHead.Creationdate = DateTime.Now;
                                        QCHead.ObjectName = objname;
                                        QCHead.OID = GRNID;
                                        QCHead.QCDate = DateTime.Now;
                                        QCHead.QCBy = profile.Personal.UserID;
                                        QCHead.Remark = "";
                                        if (objname == "PurchaseOrder")
                                        {
                                            WorkFlow = "Inbound";
                                            NextObject = "LabelPrinting";
                                        }
                                        if (objname == "Transfer")
                                        {
                                            WorkFlow = "Transfer";
                                            NextObject = "LabelPrinting";
                                        }
                                        if (objname == "SalesReturn")
                                        {
                                            WorkFlow = "Return";
                                            NextObject = "LabelPrinting";
                                        }
                                        int count1=GetWorkFlow(profile.Personal.CompanyID, profile.Personal.CustomerId, WorkFlow, NextObject, profile.DBConnection._constr);

                                        if (objname == "PurchaseOrder") { if (count1 == 0) { QCHead.Status = 33; } else { QCHead.Status = 32; } }
                                        else if (objname == "Transfer") { if (count1 == 0) { QCHead.Status = 71; } else { QCHead.Status = 60; } }
                                        else if (objname == "SalesReturn") { if (count1 == 0) { QCHead.Status = 53; } else { QCHead.Status = 52; } }

                                        QCHead.Company = profile.Personal.CompanyID;
                                        QCHead.CustomerID = profile.Personal.CustomerId;
                                        QCHead.OrderFrom = "Mobile";
                                        QCID = GetQCStatus(GRNID, objname, UserID);
                                        if (QCID == 0)
                                        {
                                            QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                                        }
                                        if (QCID > 0)
                                        {
                                            SaveQCDetails(GRNID, QCID, PrdID, 1);
                                            SaveLottableQC(serialno, objname, QCID, PrdID, 1, CompanyID, CustomerID, WarehouseID);
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
                        jsonString = jsonString + "\"reason\": \"SKU Not Available In Order\"\n";
                    }
                }
               
                jsonString = jsonString + "}]\n";
                jsonString = jsonString + "}\n"; /*json Loop End*/
                context.Response.Write(jsonString);
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "ProcessRequest"); }
            finally
            { }
        }

        public int SaveLottableQC(string serialno, string obj, long grnid, long prodid, decimal qty, long compid, long custid, long wareid)
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
                cmdDetail.CommandText = "SP_SaveLottablesQC";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@serialno", serialno);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@wareid", wareid);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "SaveLottable"); }
            finally
            { }
            return result;
        }
        public void SaveQCDetails(long pkid, long qcid, long prdid, decimal qty)
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
                cmdDetail.CommandText = "SP_SaveQCDetailsGRN";
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
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "pickup_scan_serial_and_save", "SaveQCDetails"); }
            finally
            { }
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
                cmd1.CommandText = "SP_GetQCStatusGRN";
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
        public DataTable GetCurrentCount(long oid,string obj,long prdid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            try
            {
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "SP_GetCurrentCount";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@oid", oid);
                cmd1.Parameters.AddWithValue("@obj", obj);
                cmd1.Parameters.AddWithValue("@prdid", prdid);
                da1.SelectCommand = cmd1;
                da1.Fill(dt1);
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetCurrentCount"); }
            finally
            { }
            return dt1;
        }
        public int SaveLottable(string serialno, string obj, long grnid, long prodid, decimal qty,long compid,long custid,long wareid)
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
                cmdDetail.CommandText = "SP_SaveLottables";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@serialno", serialno);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Parameters.AddWithValue("@compid", compid);
                cmdDetail.Parameters.AddWithValue("@custid", custid);
                cmdDetail.Parameters.AddWithValue("@wareid", wareid);
                cmdDetail.Connection.Open();
                result = cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "SaveLottable"); }
            finally
            { }
            return result;
        }

        public int SaveGRNDetail(long oid,string obj,long grnid,long prodid,decimal qty)
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
                cmdDetail.CommandText = "SP_SaveGRNDetails";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                cmdDetail.Parameters.AddWithValue("@oid", oid);
                cmdDetail.Parameters.AddWithValue("@obj", obj);
                cmdDetail.Parameters.AddWithValue("@grnid", grnid);
                cmdDetail.Parameters.AddWithValue("@prdid", prodid);
                cmdDetail.Parameters.AddWithValue("@qty", qty);
                cmdDetail.Connection.Open();
                result=cmdDetail.ExecuteNonQuery();
                cmdDetail.Connection.Close();
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "SaveGRNDetail"); }
            finally
            { }
            return result;
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetUserID"); }
            finally
            { }
            return username;
        }
        public long GetGRNStatus(long oid, string objname,long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            long grnid = 0;
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
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    grnid = Convert.ToInt64(dt1.Rows[0]["ID"]);
                }
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetGRNStatus"); }
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
                cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%GRN%' ";
                cmd2.Connection = conn;
                cmd2.Parameters.Clear();
                da2.SelectCommand = cmd2;
                da2.Fill(ds2, "tbl1");
                dt2 = ds2.Tables[0];
                StatusID = long.Parse(dt2.Rows[0]["ID"].ToString());
            }
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "getStatus"); }
            finally
            { }
            return StatusID;
        }
        public long GetNextBatchNo(long CustID, long CompID)
        {
            string Batch = "";
            long Number = 0;
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select top 1 BatchNo from tgrnhead where CompanyId='" + CompID + "' and CustomerID='" + CustID + "' order by ID desc ";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    Batch = obj.ToString();
                }
                if (Batch != "")
                {
                    string[] format = Batch.Split('-');
                    long No = Convert.ToInt64(format[2]);
                    Number = No + 1;
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetNextBatchNo"); }
            finally
            { }
            return Number;
        }
        private string GetBatchNoForGRN(long CustomerID, long CompanyID)
        {
            string Batch = "";
            string batchformat = "";
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select BatchNoFormat from mCustomer where ID='" + CustomerID + "' and ParentID='" + CompanyID + "'";
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                cmd.Connection.Close();
                if (obj != null)
                {
                    Batch = obj.ToString();
                }
                if (Batch != "")
                {
                    string[] format = Batch.Split('-');
                    string batchname = format[0];
                    string date = format[1];
                    long number = Convert.ToInt64(format[2]);
                    long nextno = 0;
                    if (date == "Number")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        batchformat = batchname + "-" + "01" + "-" + nextno;
                    }
                    if (date == "YYMM")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        string datetime = DateTime.Now.ToString("yyMM");
                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                    if (date == "YYQ")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        int month = DateTime.Now.Month;
                        string datetime = "";
                        if (month >= 1 && month <= 3) { datetime = "01"; }

                        else if (month >= 4 && month <= 6) { datetime = "02"; }

                        else if (month >= 7 && month <= 9) { datetime = "03"; }

                        else { datetime = "04"; }

                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                    if (date == "DDMMYY")
                    {
                        nextno = GetNextBatchNo(CustomerID, CompanyID);
                        string datetime = DateTime.Now.ToString("ddMMyy");
                        batchformat = batchname + "-" + datetime + "-" + nextno;
                    }
                }
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetBatchNoForGRN"); }
            finally
            { }
            return batchformat;
        }
        private string GEtBatchcode(long orderID)
        {
            string batchcode = "";
            try
            {
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                SqlConnection conn = new SqlConnection(strcon);
                string comapanyid = "0";
                string customerid = "0";
                cmd1.CommandType = CommandType.Text;
                //cmd1.CommandText = "select companyid ,customerid from tpurchaseorderhead where id="+ orderID + " ";
                cmd1.CommandText = "select companyid ,customerid,Object as Objectname from tpurchaseorderhead where id=" + orderID + " union all select companyid, customerid, Objectname from tTransferhead where id = " + orderID + " union all select companyid, customerid, Object as Objectname from tOrderhead where id = " + orderID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    comapanyid = dt1.Rows[0]["companyid"].ToString();
                    customerid = dt1.Rows[0]["customerid"].ToString();
                    objectName = dt1.Rows[0]["Objectname"].ToString();
                }
                batchcode = GetBatchNoForGRN(Convert.ToInt64(customerid), Convert.ToInt64(comapanyid));
            }
            catch (Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GEtBatchcode"); }
            finally
            { }


            return batchcode;
        }

        public DataTable CheckDuplicateSrNo(string srno,long compid, long custid,string page, string obj)
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
        public DataTable CheckSKUInOrder(long prdid,long oid,string obj)
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetPrdIDNewCode"); }
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetPrdIDNew"); }
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetPrdID"); }
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
            catch(Exception ex)
            { Login.Profile.ErrorHandling(ex, "scan_serial_and_save", "GetUserDetails"); }
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