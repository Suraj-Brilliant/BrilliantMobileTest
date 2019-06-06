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
using BrilliantWMS.WMSInbound;
using System.Web.SessionState;
using static System.Web.SessionState.IRequiresSessionState;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for ws_save_grn
    /// </summary>
    public class ws_save_grn : IHttpHandler
    {

        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string prdBarcode = "", objectname = "", batchcode = "", remark = "", airwaybill = "", shippingtype="", dockno="", lrno="", intime="", outtime="", productdetail="",udr="";
        long UserID = 0, Status = 0, uid=0, oid=0;
        
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long GRNID = 0;
            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"save_grn\": [\n";

            try
            {
                DateTime shippingdate = DateTime.Now;
                DateTime expecteddeliverydate = DateTime.Now;
                if(context.Request.QueryString["userid"]!=null)
                {
                    uid = long.Parse(context.Request.QueryString["userid"]);
                }
                else if(context.Request.Form["userid"]!=null)
                {
                    uid = long.Parse(context.Request.Form["userid"]);
                }
                if(context.Request.QueryString["oid"]!=null)
                {
                    oid = long.Parse(context.Request.QueryString["oid"]);
                }
                else if(context.Request.Form["oid"]!=null)
                {
                    oid = long.Parse(context.Request.Form["oid"]);
                }
                if(context.Request.QueryString["objectname"]!=null)
                {
                    objectname = context.Request.QueryString["objectname"];
                }
                else if(context.Request.Form["objectname"]!=null)
                {
                    objectname = context.Request.Form["objectname"];
                }
                if(context.Request.QueryString["batchcode"]!=null)
                {
                    batchcode = context.Request.QueryString["batchcode"];
                }
                else if(context.Request.Form["batchcode"]!=null)
                {
                    batchcode = context.Request.Form["batchcode"];
                }
                if(context.Request.QueryString["remark"]!=null)
                {
                    remark = context.Request.QueryString["remark"];
                }
                else if(context.Request.Form["remark"]!=null)
                {
                    remark = context.Request.Form["remark"];
                }
                if(context.Request.QueryString["airwaybill"]!=null)
                {
                    airwaybill = context.Request.QueryString["airwaybill"];
                }
                else if(context.Request.Form["airwaybill"]!=null)
                {
                    airwaybill = context.Request.Form["airwaybill"];
                }
                if(context.Request.QueryString["shippingtype"]!=null)
                {
                    shippingtype = context.Request.QueryString["shippingtype"];
                }
                else if(context.Request.Form["shippingtype"]!=null)
                {
                    shippingtype = context.Request.Form["shippingtype"];
                }
                
                if(context.Request.QueryString["shippingdate"]!=null)
                {
                    if (Convert.ToString(context.Request.QueryString["shippingdate"]) == "N/A")
                    { }
                    else
                    {
                        shippingdate = Convert.ToDateTime(context.Request.QueryString["shippingdate"]);
                    }
                }
                else if(context.Request.Form["shippingdate"]!=null)
                {
                    if(context.Request.Form["shippingdate"]=="N/A")
                    {
                    }
                    else
                    {
                        shippingdate = Convert.ToDateTime(context.Request.Form["shippingdate"]);
                    }
                    
                }
                
                if(context.Request.QueryString["expecteddeliverydate"]!=null)
                {
                    if (Convert.ToString(context.Request.QueryString["expecteddeliverydate"]) == "N/A")
                    { }
                    else
                    {
                        expecteddeliverydate = Convert.ToDateTime(context.Request.QueryString["expecteddeliverydate"]);
                    }
                }
                else if(context.Request.Form["expecteddeliverydate"]!=null)
                {
                    if(context.Request.Form["expecteddeliverydate"]=="N/A")
                    { }
                    else
                    {
                        expecteddeliverydate = Convert.ToDateTime(context.Request.Form["expecteddeliverydate"]);
                    }
                }
                if(context.Request.QueryString["dockno"]!=null)
                {
                    dockno = context.Request.QueryString["dockno"];
                }
                else if(context.Request.Form["dockno"]!=null)
                {
                    dockno = context.Request.Form["dockno"];
                }
                if(context.Request.QueryString["lrno"]!=null)
                {
                    lrno = context.Request.QueryString["lrno"];
                }
                else if(context.Request.Form["lrno"]!=null)
                {
                    lrno = context.Request.Form["lrno"];
                }
                if(context.Request.QueryString["intime"]!=null)
                {
                    intime = context.Request.QueryString["intime"];
                }
                else if(context.Request.Form["intime"]!=null)
                {
                    intime = context.Request.Form["intime"];
                }
                if(context.Request.QueryString["outtime"]!=null)
                {
                    outtime = context.Request.QueryString["outtime"];
                }
                else if(context.Request.Form["outtime"]!=null)
                {
                    outtime = context.Request.Form["outtime"];
                }
                if(context.Request.QueryString["productdetail"]!=null)
                {
                    productdetail = context.Request.QueryString["productdetail"];
                }
                else if(context.Request.Form["productdetail"]!=null)
                {
                    productdetail = context.Request.Form["productdetail"];
                }

                if (context.Request.QueryString["other"] != null)
                {
                    udr = context.Request.QueryString["other"];
                }
                else if (context.Request.Form["other"] != null)
                {
                    udr = context.Request.Form["other"];
                }

                //context.Response.ContentType = "text/plain";
                //jsonString = String.Empty;
                long CustomerID, CompanyID;
                string DuplicateLottable = "";
                string userName = GetUserID(uid);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                iInboundClient Inbound = new iInboundClient();
                tGRNHead GRNHead = new tGRNHead();
                GRNHead.CreatedBy = uid;
                GRNHead.Creationdate = DateTime.Now;
                GRNHead.ObjectName = objectname;
                GRNHead.OID = oid;
                GRNHead.ShipID = "";
                GRNHead.GRNDate = DateTime.Now;
                GRNHead.ReceivedBy = uid;
                GRNHead.BatchNo = batchcode;
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
                GRNHead.CompanyID = profile.Personal.CompanyID;
                GRNHead.CustomerID = profile.Personal.CustomerId;
                GRNHead.Status = getStatus(objectname);
                Status = getStatus(objectname);
                GRNHead.ShippingDate = shippingdate;
                GRNHead.TransporterRemark = "";
                //GRNHead.TransporterID = ;
                // GRNHead.DockNo = dockno;
                //GRNHead.TruckNo = "";
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

                //DataTable dtnew = new DataTable();
                //dtnew = GetDatatableDetails("select * from tgrnhead where oid=" + oid + "");
                //if (dtnew.Rows.Count > 0)
                //{
                //    GRNID = Convert.ToInt64(dtnew.Rows[0]["ID"].ToString());
                //}
                //else
                //{
                //    GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);
                //}
                GRNID = GetGRNStatus(oid, objectname);
                if (GRNID == 0)
                {
                    GRNID = Inbound.SavetGRNHead(GRNHead, profile.DBConnection._constr);
                }

                /* GRNDetail */
                string mygrnPrdString1 = "select * from SplitString('" + productdetail + "','@')";
                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                SqlDataReader dr1;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = mygrnPrdString1;
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl2");
                dt1 = ds1.Tables[0];
                int cntr1 = dt1.Rows.Count;
                if (cntr1 > 0)
                {
                    for (int P = 0; P <= cntr1 - 1; P++)
                    {
                        string productdetail1 = ds1.Tables[0].Rows[P]["part"].ToString().Trim();
                        string prdString, qtystring; long prdID;

                        decimal qty = 0;
                        string lot1str, lot2str, lot3str, lot1 = "", lot2 = "", lot3 = "";
                        dt.Clear();
                        string mygrnPrdString = "select * from SplitString('" + productdetail1 + "','|')";
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = mygrnPrdString;
                        cmd.Connection = conn;
                        cmd.Parameters.Clear();
                        da.SelectCommand = cmd;
                        da.Fill(ds, "tbl1");
                        dt = ds.Tables[0];
                        int cntr = dt.Rows.Count;
                        if (cntr > 0)
                        {
                            for (int i = 0; i <= cntr - 1; i++)
                            {
                                prdString = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] spltprdString = prdString.Split(':');
                                prdID = long.Parse(spltprdString[1]);
                                i = i + 1;
                                qtystring = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] spltqty = qtystring.Split(':');
                                qty = decimal.Parse(spltqty[1]);
                                if (cntr > 2)
                                {
                                    i = i + 1;
                                    lot1str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lotsplit = lot1str.Split(':');
                                    lot1 = lotsplit[1];
                                    //DuplicateLottable = ChkDuplicateLottable(lot1, profile.Personal.CompanyID, profile.Personal.CustomerId);
                                    DuplicateLottable = "NO";
                                    //if (objectname == "SalesReturn")
                                    //{
                                    //    DuplicateLottable = "No";
                                    //}
                                    //if (DuplicateLottable == "Yes")
                                    //{
                                    //    break;
                                    //}
                                    //else { }
                                }
                                if (cntr > 3)
                                {
                                    i = i + 1;
                                    lot2str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lot2split = lot2str.Split(':');
                                    lot2 = lot2split[1];
                                }
                                if (cntr > 4)
                                {
                                    i = i + 1;
                                    lot3str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lot3split = lot3str.Split(':');
                                    lot3 = lot3split[1];
                                }
                                //i = i + 1;
                                DataSet dsOrder = new DataSet();
                                dsOrder = GetUOMofPrd(oid, objectname, prdID);

                                decimal poqty = 0;
                                long UOMID = long.Parse(dsOrder.Tables[0].Rows[0]["UOMID"].ToString());

                                if (objectname == "PurchaseOrder")
                                { poqty = decimal.Parse(dsOrder.Tables[0].Rows[0]["OrderQty"].ToString()); }
                                else if (objectname == "Transfer")
                                { poqty = decimal.Parse(dsOrder.Tables[0].Rows[0]["Qty"].ToString()); }
                                else if (objectname == "SalesReturn")
                                { poqty = decimal.Parse(dsOrder.Tables[0].Rows[0]["OrderQty"].ToString()); }
                                decimal totalgrnqty = 0;

                                DataTable dtgrn = new DataTable();
                                dtgrn = GetDatatableDetails("select isnull(sum(grnqty),0) as grnqty from tgrndetail where grnid=" + GRNID + " and prodid=" + prdID + "");
                                if (dtgrn.Rows.Count > 0)
                                {
                                    totalgrnqty = Convert.ToDecimal(dtgrn.Rows[0]["grnqty"].ToString());
                                }
                                decimal ShortQty = 0, ExcessQty = 0; decimal qtyCalculation = 0;
                                // decimal qtyCalculation = poqty - qty;
                                if (totalgrnqty == 0)
                                {
                                    qtyCalculation = poqty - qty;
                                }
                                else
                                {
                                    qtyCalculation = poqty - totalgrnqty;
                                }
                                if (qtyCalculation == 0) { ShortQty = 0; ExcessQty = 0; }
                                else if (qtyCalculation > 0) { ShortQty = qtyCalculation; ExcessQty = 0; }
                                else if (qtyCalculation < 0) { ShortQty = 0; ExcessQty = Math.Abs(qtyCalculation); }

                                SqlCommand cmdDetail = new SqlCommand();
                                SqlDataAdapter daDetail = new SqlDataAdapter();
                                DataSet dsDetail = new DataSet();
                                DataTable dtDetail = new DataTable();
                                // SqlConnection conn = new SqlConnection(strcon);
                                cmdDetail.CommandType = CommandType.StoredProcedure;
                                cmdDetail.CommandText = "SP_MobileInsertGRNDetails";
                                cmdDetail.Connection = conn;
                                cmdDetail.Parameters.Clear();
                                cmdDetail.Parameters.AddWithValue("GRNID", GRNID);
                                cmdDetail.Parameters.AddWithValue("prdID", prdID);
                                cmdDetail.Parameters.AddWithValue("qty", qty);
                                cmdDetail.Parameters.AddWithValue("poqty", poqty);
                                cmdDetail.Parameters.AddWithValue("ShortQty", ShortQty);
                                cmdDetail.Parameters.AddWithValue("ExcessQty", ExcessQty);
                                cmdDetail.Parameters.AddWithValue("UOMID", UOMID);
                                cmdDetail.Parameters.AddWithValue("lot1", lot1);
                                cmdDetail.Parameters.AddWithValue("lot2", lot2);
                                cmdDetail.Parameters.AddWithValue("lot3", lot3);
                                cmdDetail.Parameters.AddWithValue("obj", objectname);
                                cmdDetail.Parameters.AddWithValue("CompanyId", profile.Personal.CompanyID);
                                cmdDetail.Parameters.AddWithValue("CustomerId", profile.Personal.CustomerId);
                                cmdDetail.Connection.Open();
                                cmdDetail.ExecuteNonQuery();
                                cmdDetail.Connection.Close();
                            }

                        }
                        if (DuplicateLottable == "Yes")
                        {
                            break;
                        }

                        // jsonString = "{\n\"arr_qc_list\":[\n";
                    }
                    updateGRNStatus(GRNID);
                    if (DuplicateLottable == "Yes")
                    {
                        RecordUpdateStatus(GRNID, objectname, Status,oid);
                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"duplicateserial\"\n";
                        jsonString = jsonString + "}\n";
                        jsonString = jsonString + "]\n}";
                        context.Response.Write(jsonString);
                    }
                    else
                    {
                        RecordUpdateStatus(GRNID, objectname, Status,oid);
                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"status\": \"success\",\n";
                        jsonString = jsonString + "\"reason\": \"\"\n";
                        jsonString = jsonString + "}\n";
                        jsonString = jsonString + "]\n}";
                        context.Response.Write(jsonString);
                    }

                }

            }
            catch (System.Exception ex)
            {
                ErrorLog(ex.ToString(), "Save_GRN");
                Login.Profile.ErrorHandling(ex, "ws-save-grn", "ProcessRequest");
                // throw ex;
                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"status\": \"failed\",\n";
                jsonString = jsonString + "\"reason\": \"servererror\"\n";
                jsonString = jsonString + "}\n";
                jsonString = jsonString + "]\n}";
                context.Response.Write(jsonString);
               
            }
            finally
            { }
        }

        private void ErrorLog(string msg,string method)
        {
            try
            {
                SqlConnection conn33 = new SqlConnection(strcon);
                SqlCommand cmduom = new SqlCommand();
                SqlDataAdapter dauom = new SqlDataAdapter();
                DataSet dsuom = new DataSet();
                DataTable dtuom = new DataTable();
                cmduom.CommandType = CommandType.StoredProcedure;
                cmduom.CommandText = "sp_errorlog";
                cmduom.Connection = conn33;
                cmduom.Parameters.Clear();
                cmduom.Connection.Open();
                cmduom.ExecuteNonQuery();
                cmduom.Connection.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Update-Shipping-List", "updatePickUpStatus");
            }
            finally
            { }
        }

        private string ChkDuplicateLottable(string lot1, long companyid, long customerid)
        {
            string result = "";
            DataTable dtnew = new DataTable();
            dtnew = GetDatatableDetails("select * from tSkuTransactionHistory where companyid=" + companyid + " and customerid=" + customerid + " and lottable1='" + lot1 + "'");
            if (dtnew.Rows.Count > 0)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

        private string CHKGrnHeadidcreateornot(long oid)
        {
            string result = "";
            DataTable dtnew = new DataTable();
            dtnew = GetDatatableDetails("select * from tgrnhead where oid=" + oid + "");
            if (dtnew.Rows.Count > 0)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

        private DataTable GetDatatableDetails(string Query)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection conn = new SqlConnection(strcon);
                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }

            }
            catch
            { }
            finally { }
            return dt;
        }

        private void RecordUpdateStatus(long GRNID, string objectname, long Status,long orderid)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmdDetail = new SqlCommand();
            SqlDataAdapter daDetail = new SqlDataAdapter();
            DataSet dsDetail = new DataSet();
            DataTable dtDetail = new DataTable();
            SqlCommand cmdDetail2 = new SqlCommand();
            SqlDataAdapter daDetail2 = new SqlDataAdapter();
            DataSet dsDetail2 = new DataSet();
            DataTable dtDetail2 = new DataTable();
            cmdDetail.CommandType = CommandType.Text;
            if (objectname == "PurchaseOrder")
            {
                ////cmdDetail.CommandText = "select GD.Prodid,GD.GRNQty,PD.Skuid,PD.OrderQty  from tgrnhead GH  left outer join tgrndetail GD on GH.Id=GD.grnid  left outer join tpurchaseorderdetail PD on PD.poorderheadid=GH.Oid where GH.id=" + GRNID + "";
                //cmdDetail.CommandText = " select Sum(GD.GRNQty) as totalgrnqty,Sum(PD.OrderQty) as totalpoqty  from tgrnhead GH left outer join tgrndetail GD on GH.Id=GD.grnid  left outer join tpurchaseorderdetail PD on PD.poorderheadid=GH.Oid where GH.id=" + GRNID + " ";
                //cmdDetail.Connection = conn;
                //cmdDetail.Parameters.Clear();
                //daDetail.SelectCommand = cmdDetail;
                //daDetail.Fill(dsDetail, "tbl1");
                //dtDetail = dsDetail.Tables[0];

                cmdDetail.CommandText = "select ISNULL(Sum(GD.GRNQty),0.00) as totalgrnqty from tgrndetail GD left outer join tgrnhead GH  on GH.Id = GD.grnid left outer join tPurchaseOrderHead TH on GH.OID = TH.ID where GD.GRNID in(select ID from tGRNHead where OID = '" + orderid + "')";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                daDetail.SelectCommand = cmdDetail;
                daDetail.Fill(dsDetail, "tbl1");
                dtDetail = dsDetail.Tables[0];

                cmdDetail2.CommandText = "select ISNULL(Sum(PD.OrderQty),0.00) as totalpoqty  from tPurchaseOrderDetail PD where PD.POOrderHeadId='" + orderid + "'";
                cmdDetail2.Connection = conn;
                cmdDetail2.Parameters.Clear();
                daDetail2.SelectCommand = cmdDetail2;
                daDetail2.Fill(dsDetail2, "tbl1");
                dtDetail2 = dsDetail2.Tables[0];
                string result = "";
                if (dtDetail.Rows.Count > 0)
                {

                    SqlCommand cmdDetail1 = new SqlCommand();
                    SqlDataAdapter daDetail1 = new SqlDataAdapter();
                    DataSet dsDetail1 = new DataSet();
                    DataTable dtDetail1 = new DataTable();
                    cmdDetail1.CommandType = CommandType.StoredProcedure;
                    cmdDetail1.CommandText = "SP_MobileInsertGRNDetailsUpdate";
                    cmdDetail1.Connection = conn;
                    if (Convert.ToDecimal(dtDetail.Rows[0]["totalgrnqty"].ToString()) >= Convert.ToDecimal(dtDetail2.Rows[0]["totalpoqty"].ToString()))
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 31);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    else
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 63);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    cmdDetail1.Connection.Open();
                    cmdDetail1.ExecuteNonQuery();
                    cmdDetail1.Connection.Close();
                }
            }
            else if (objectname == "Transfer")
            {
                //  cmdDetail.CommandText = "select Sum(GD.GRNQty) as totalgrnqty,Sum(PD.Qty) as totalpoqty  from tgrnhead GH left outer join tgrndetail GD on GH.Id=GD.grnid left outer join tTransferDetail PD on PD.TransferID=GH.Oid where GH.id="+ GRNID + "";
                cmdDetail.CommandText = "select ISNULL(Sum(GD.GRNQty),0.00) as totalgrnqty from tgrndetail GD left outer join tgrnhead GH  on GH.Id = GD.grnid left outer join ttransferhead TH on GH.OID = TH.ID where GD.GRNID in(select ID from tGRNHead where OID = '"+orderid+"')";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                daDetail.SelectCommand = cmdDetail;
                daDetail.Fill(dsDetail, "tbl1");
                dtDetail = dsDetail.Tables[0];

                cmdDetail2.CommandText = "select ISNULL(Sum(PD.Qty),0.00) as totaltrqty  from ttransferdetail PD where PD.TransferID='" + orderid + "'";
                cmdDetail2.Connection = conn;
                cmdDetail2.Parameters.Clear();
                daDetail2.SelectCommand = cmdDetail2;
                daDetail2.Fill(dsDetail2, "tbl1");
                dtDetail2 = dsDetail2.Tables[0];

                if (dtDetail.Rows.Count > 0)
                {
                    SqlCommand cmdDetail1 = new SqlCommand();
                    SqlDataAdapter daDetail1 = new SqlDataAdapter();
                    DataSet dsDetail1 = new DataSet();
                    DataTable dtDetail1 = new DataTable();
                    cmdDetail1.CommandType = CommandType.StoredProcedure;
                    cmdDetail1.CommandText = "SP_MobileInsertGRNDetailsUpdate";
                    cmdDetail1.Connection = conn;
                    if (Convert.ToDecimal(dtDetail.Rows[0]["totalgrnqty"].ToString()) >= Convert.ToDecimal(dtDetail2.Rows[0]["totaltrqty"].ToString()))
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 60);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    else
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 67);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    cmdDetail1.Connection.Open();
                    cmdDetail1.ExecuteNonQuery();
                    cmdDetail1.Connection.Close();
                }
            }
            else if (objectname == "SalesReturn")
            {
                //cmdDetail.CommandText = "select Sum(GD.GRNQty) as totalgrnqty,Sum(PD.ReturnQty) as totalpoqty  from tgrnhead GH left outer join tgrndetail GD on GH.Id=GD.grnid left outer join tOrderDetail PD on PD.OrderHeadId=GH.Oid where GH.id="+ GRNID + "";
                //cmdDetail.Connection = conn;
                //cmdDetail.Parameters.Clear();
                //daDetail.SelectCommand = cmdDetail;
                //daDetail.Fill(dsDetail, "tbl1");
                //dtDetail = dsDetail.Tables[0];

              //  cmdDetail.CommandText = "select ISNULL(Sum(GD.GRNQty),0.00) as totalgrnqty from tgrndetail GD left outer join tgrnhead GH  on GH.Id = GD.grnid left outer join tOrderHead TH on GH.OID = TH.ID where GD.GRNID in(select ID from tGRNHead where OID = '" + orderid + "')";
                cmdDetail.CommandText = "select ISNULL(Sum(GD.GRNQty),0.00) as totalgrnqty from tgrndetail GD left outer join tgrnhead GH  on GH.Id = GD.grnid left outer join tOrderHead TH on GH.OID = TH.ID where GD.GRNID in(select ID from tGRNHead where OID =(select SONo from tReturnHead where ID="+orderid+"))";
                cmdDetail.Connection = conn;
                cmdDetail.Parameters.Clear();
                daDetail.SelectCommand = cmdDetail;
                daDetail.Fill(dsDetail, "tbl1");
                dtDetail = dsDetail.Tables[0];

              //  cmdDetail2.CommandText = "select ISNULL(Sum(PD.ReturnQty),0.00) as totalrtqty  from tOrderDetail PD where PD.OrderHeadId='" + orderid + "'";
                cmdDetail2.CommandText = "select ISNULL(Sum(PD.ReturnQty),0.00) as totalrtqty  from tOrderDetail PD where PD.OrderHeadId=(select SONo from tReturnHead where ID=" + orderid + ")";
                cmdDetail2.Connection = conn;
                cmdDetail2.Parameters.Clear();
                daDetail2.SelectCommand = cmdDetail2;
                daDetail2.Fill(dsDetail2, "tbl1");
                dtDetail2 = dsDetail2.Tables[0];

                if (dtDetail.Rows.Count > 0)
                {
                    SqlCommand cmdDetail1 = new SqlCommand();
                    SqlDataAdapter daDetail1 = new SqlDataAdapter();
                    DataSet dsDetail1 = new DataSet();
                    DataTable dtDetail1 = new DataTable();
                    cmdDetail1.CommandType = CommandType.StoredProcedure;
                    cmdDetail1.CommandText = "SP_MobileInsertGRNDetailsUpdate";
                    cmdDetail1.Connection = conn;
                    if (Convert.ToDecimal(dtDetail.Rows[0]["totalgrnqty"].ToString()) >= Convert.ToDecimal(dtDetail2.Rows[0]["totalrtqty"].ToString()))
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 52);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    else
                    {
                        cmdDetail1.Parameters.Clear();
                        cmdDetail1.Parameters.AddWithValue("Status", 68);
                        cmdDetail1.Parameters.AddWithValue("ID", GRNID);
                        cmdDetail1.Parameters.AddWithValue("objectname", objectname);
                    }
                    cmdDetail1.Connection.Open();
                    cmdDetail1.ExecuteNonQuery();
                    cmdDetail1.Connection.Close();
                }
               
            }
        }



        public DataSet GetUOMofPrd(long oid, string objectname, long prdID)
        {
            SqlCommand cmduom = new SqlCommand();
            SqlDataAdapter dauom = new SqlDataAdapter();
            DataSet dsuom = new DataSet();
            DataTable dtuom = new DataTable();

            SqlConnection conn = new SqlConnection(strcon);
            cmduom.CommandType = CommandType.Text;
            if (objectname == "PurchaseOrder") { cmduom.CommandText = "select * from tPurchaseOrderDetail where POOrderHeadID=" + oid + " and SkuId=" + prdID + ""; }
            else if (objectname == "Transfer") { cmduom.CommandText = "select * from ttransferdetail where TransferID=" + oid + " and SkuId=" + prdID + ""; }
            else if (objectname == "SalesReturn") { cmduom.CommandText = "select * from tOrderDetail  where OrderHeadID=" + oid + " and SkuId=" + prdID + ""; }

            cmduom.Connection = conn;
            cmduom.Parameters.Clear();
            dauom.SelectCommand = cmduom;
            dauom.Fill(dsuom, "tbl1");
            dtuom = dsuom.Tables[0];

            return dsuom;
        }

        public long getStatus(string objNM)
        {
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();

            long StatusID = 0;
            SqlConnection conn = new SqlConnection(strcon);
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select * from mstatus where Remark='" + objNM + "' and status like '%GRN%' ";
            cmd2.Connection = conn;
            cmd2.Parameters.Clear();
            da2.SelectCommand = cmd2;
            da2.Fill(ds2, "tbl1");
            dt2 = ds2.Tables[0];
            StatusID = long.Parse(dt2.Rows[0]["ID"].ToString());
            return StatusID;
        }

        public string GetUserID(long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            string username = "";
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
            return username;
        }

        public long GetGRNStatus(long oid,string objname)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            long grnid =0;
            SqlConnection conn = new SqlConnection(strcon);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "SP_GetGRNStatus";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@oid", oid);
            cmd1.Parameters.AddWithValue("@objname", objname);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                grnid = Convert.ToInt64(dt1.Rows[0]["ID"]);
            }
            return grnid;
        }

        private void updateGRNStatus(long GRNID)
        {
            try
            {
                SqlConnection conn33 = new SqlConnection(strcon);
                SqlCommand cmduom = new SqlCommand();
                cmduom.CommandType = CommandType.StoredProcedure;
               // cmduom.CommandText = "update tGRNHead set IsGRN=1 where ID=" + GRNID + "";
                cmduom.CommandText = "sp_UpdateGRNStatus";
                cmduom.Connection = conn33;
                cmduom.Parameters.Clear();
                cmduom.Parameters.AddWithValue("@grnid", GRNID);
                cmduom.Connection.Open();
                cmduom.ExecuteNonQuery();
                cmduom.Connection.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "ws_save_grn", "updateGRNStatus");
            }
            finally
            { }
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