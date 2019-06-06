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


//MobileWMS/ws-save-qc.ashx?oid=50387&objectname=PurchaseOrder&userid=10542&remark=hhj&productdetail=prouct_id:25079|qty:1|acceptedqty:1|rejectedqty:0|reasoncode:123

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description forMobileWMS/ws_get_qc_list.ashx?oid=50918&objectname=PurchaseOrder&userid=10541&remark=test&productdetail=prouct_id:25364|qty:2|acceptedqty:2|rejectedqty:0|ReasonID:6   
    /// </summary>
    public class ws_save_qc : IHttpHandler
    {

        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string prdBarcode = "";
        long UserID = 0;

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"save_qc\": [\n";
            try
            {

                long uid = long.Parse(context.Request.QueryString["userid"]);
                long oid = long.Parse(context.Request.QueryString["oid"]);
                string objectname = context.Request.QueryString["objectname"];
                string productdetail = context.Request.QueryString["productdetail"];
                string remark = context.Request.QueryString["remark"];
                string userName = GetUserID(uid);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                iInboundClient Inbound = new iInboundClient();
                int count1 = Workflow(profile.Personal.CompanyID, profile.Personal.CustomerId, objectname);
                long status = 0;
                if (count1 > 0)
                {
                    status = getStatus(objectname);
                }
                else
                {
                    status = 33;
                }
                long CustomerID, CompanyID, reasonid;
                string lottable1;
                string lot1 = "", lot2 = "", lot3 = "";
                long QCID = 0;
                /* QualityControlHead */
                tQualityControlHead QCHead = new tQualityControlHead();
                QCHead.ObjectName = objectname;
                QCHead.OID = oid;
                QCHead.QCDate = DateTime.Now;
                QCHead.QCBy = uid;
                QCHead.Remark = remark;
                QCHead.CreatedBy = uid;
                QCHead.Creationdate = DateTime.Now;
                QCHead.Status = status;
                QCHead.Company = profile.Personal.CompanyID;
                QCHead.CustomerID = profile.Personal.CustomerId;
                DataTable dtnew = new DataTable();
                dtnew = GetDatatableDetails(" select * from tQualityControlHead where oid=" + oid + "");
                if (dtnew.Rows.Count > 0)
                {
                    QCID = Convert.ToInt64(dtnew.Rows[0]["ID"].ToString());
                }
                else
                {
                    QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                }

                /* QualityControlDetail */

                SqlCommand cmd1 = new SqlCommand();
                SqlDataAdapter da1 = new SqlDataAdapter();
                DataSet ds1 = new DataSet();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "select * from  SplitString('" + productdetail + "','@')";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                int cntr1 = 0;
                cntr1 = ds1.Tables[0].Rows.Count;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p <= cntr1 - 1; p++)
                    {
                        string prdString, qtystring; long prdID, oqty, uomid;
                        decimal qty = 0, grnqty = 0, qcqty = 0, rejectqty = 0;
                        string lot1str, lot2str, lot3str, serialno, lot4str, lot5str, lot6str;
                        string mygrnPrdString = "select * from SplitString('" + ds1.Tables[0].Rows[p]["part"].ToString() + "','|')";
                        ds.Clear();
                        dt.Clear();
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
                                //     ID QCID    ProdID OQty    GRNQty QCQty   RejectedQty ExcessQty   Status Reason  Company CustomerID  UOMID PackingStyle    ReasonID

                                //ProdID
                                prdString = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] spltprdString = prdString.Split(':');
                                prdID = long.Parse(spltprdString[1]);
                                i = i + 1;

                                //qty=OQty
                                qtystring = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] spltqty = qtystring.Split(':');
                                qty = decimal.Parse(spltqty[1]);
                                i = i + 1;

                                //GRNqty
                                DataSet dsqty = new DataSet();
                                // dsqty = GetAllDetails(oid, prdID, objectname);
                                grnqty = GetQtyAllDetails(oid, prdID, objectname);
                                // grnqty = decimal.Parse(dsqty.Tables[0].Rows[0]["GRNQty"].ToString());


                                //QCQTY=acceptedqty
                                lot1str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lotsplit = lot1str.Split(':');
                                qcqty = decimal.Parse(lotsplit[1]);
                                i = i + 1;

                                //RejectedQty=rejectedqty
                                lot2str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lot2split = lot2str.Split(':');
                                rejectqty = decimal.Parse(lot2split[1]);
                                i = i + 1;

                                //reasoncode=ReasonID
                                lot3str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lot3split = lot3str.Split(':');
                                reasonid = long.Parse(lot3split[1]);

                                //serial number if avaliable
                                if (cntr > 5)
                                {
                                    i = i + 1;
                                    lot4str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lotsplit4 = lot4str.Split(':');
                                    lot1 = lotsplit4[1];
                                }
                                if (cntr > 6)
                                {
                                    i = i + 1;
                                    lot5str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lotsplit4 = lot5str.Split(':');
                                    lot2 = lotsplit4[1];
                                }
                                if (cntr > 7)
                                {
                                    i = i + 1;
                                    lot6str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                    string[] lotsplit4 = lot6str.Split(':');
                                    lot3 = lotsplit4[1];
                                }
                                                              

                                DataSet dsOrder = new DataSet();
                                dsOrder = GetUOMofPrd(oid, objectname, prdID);
                                decimal poqty = 0;
                                long UOMID = long.Parse(dsOrder.Tables[0].Rows[0]["UOMID"].ToString());
                                //long UOMID = 16;
                                SqlCommand cmdDetail = new SqlCommand();
                                SqlDataAdapter daDetail = new SqlDataAdapter();
                                DataSet dsDetail = new DataSet();
                                DataTable dtDetail = new DataTable(); ;
                                cmdDetail.CommandType = CommandType.StoredProcedure;
                                cmdDetail.CommandText = "SP_MobileInsertQCDetails";
                                cmdDetail.Connection = conn;
                                cmdDetail.Parameters.Clear();
                                cmdDetail.Parameters.AddWithValue("QCID", QCID);
                                cmdDetail.Parameters.AddWithValue("ProdID", prdID);
                                cmdDetail.Parameters.AddWithValue("OQty", qty);
                                cmdDetail.Parameters.AddWithValue("GRNQty", grnqty);
                                cmdDetail.Parameters.AddWithValue("QCQty", qcqty);
                                cmdDetail.Parameters.AddWithValue("RejectedQty", rejectqty);
                                cmdDetail.Parameters.AddWithValue("UOMID", UOMID);
                                cmdDetail.Parameters.AddWithValue("ReasonID", reasonid);
                                cmdDetail.Connection.Open();
                                cmdDetail.ExecuteNonQuery();
                                cmdDetail.Connection.Close();
                                SaveQCLottable(lot1,profile.Personal.CompanyID,profile.Personal.CustomerId, prdID, QCID,lot2,lot3);
                            }


                        }
                    }
                    //Upadte status from tgrnhead and tpurchasehead
                    int count = 0;
                    count = Workflow(profile.Personal.CompanyID, profile.Personal.CustomerId, objectname);
                    if (count > 0)
                    {
                        RecordUpdateStatus(oid, 32, objectname);
                    }
                    else
                    {
                        RecordUpdateStatus(oid, 33, objectname);
                    }

                    jsonString = jsonString + "{\n";
                    jsonString = jsonString + "\"status\": \"success\"\n";
                    jsonString = jsonString + "}\n";
                    jsonString = jsonString + "]\n}";
                    context.Response.Write(jsonString);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"status\": \"failed\"\n";
                jsonString = jsonString + "}\n";
                jsonString = jsonString + "]\n}";
                context.Response.Write(jsonString);
            }

        }

        private void SaveQCLottable(string lottable1, long companyID, long customerId, long prdID, long oid,string lot2,string lot3)
        {
            SqlConnection conn1 = new SqlConnection(strcon);
            SqlCommand cmdDetail = new SqlCommand();
            SqlDataAdapter daDetail = new SqlDataAdapter();
            DataSet dsDetail = new DataSet();
            DataTable dtDetail = new DataTable();
            cmdDetail.CommandType = CommandType.StoredProcedure;
            cmdDetail.CommandText = "SP_InsertintotskutransactionhistoryForQC";
            cmdDetail.Connection = conn1;
            cmdDetail.Parameters.Clear();
            cmdDetail.Parameters.AddWithValue("skuid", prdID);
            cmdDetail.Parameters.AddWithValue("lottable1", lottable1);
            cmdDetail.Parameters.AddWithValue("qty", 1);
            cmdDetail.Parameters.AddWithValue("oid", oid);
            cmdDetail.Parameters.AddWithValue("companyid", companyID);
            cmdDetail.Parameters.AddWithValue("customerid", customerId);
            cmdDetail.Parameters.AddWithValue("lottable2", lot2);
            cmdDetail.Parameters.AddWithValue("lottable3", lot3);
            cmdDetail.Connection.Open();
            cmdDetail.ExecuteNonQuery();
            cmdDetail.Connection.Close();
        }

        private int Workflow(long companyID, long customerId, string objectname)
        {
            int i = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmdDetail1 = new SqlCommand();
            SqlDataAdapter daDetail1 = new SqlDataAdapter();
            DataSet dsDetail1 = new DataSet();
            DataTable dtDetail1 = new DataTable(); ;
            cmdDetail1.CommandType = CommandType.StoredProcedure;
            cmdDetail1.CommandText = "SP_InboundWorkFolwForMobile";
            cmdDetail1.Connection = conn;
            cmdDetail1.Parameters.Clear();
            cmdDetail1.Parameters.AddWithValue("@CompanyID", companyID);
            cmdDetail1.Parameters.AddWithValue("@CustomerID", customerId);
            cmdDetail1.Parameters.AddWithValue("@WorkFlow", "Inbound");
            cmdDetail1.Parameters.AddWithValue("@Object", objectname);
            cmdDetail1.Connection.Open();
            i = cmdDetail1.ExecuteNonQuery();
            cmdDetail1.Connection.Close();
            return i;
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

        private decimal GetQtyAllDetails(long oid, long prdID, string objectname)
        {
            decimal qty = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            if (objectname == "PurchaseOrder")
            {
                cmd1.CommandText = "select * from tgrndetail where grnid=" + oid + " and prodid=" + prdID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                qty = decimal.Parse(dt1.Rows[0]["GRNQty"].ToString());
            }
            else if (objectname == "SalesOrder")
            {
                cmd1.CommandText = "select * from tpickupdetail where pickupid=" + oid + "  and prodid=" + prdID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                qty = decimal.Parse(dt1.Rows[0]["pickupqty"].ToString());
            }
            else if (objectname == "Transfer")
            {
                cmd1.CommandText = "select * from tTransferDetail where transferid=" + oid + " and skuid=" + prdID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                qty = decimal.Parse(dt1.Rows[0]["Qty"].ToString());
            }
            else if (objectname == "SalesReturn")
            {
                cmd1.CommandText = "select * from tgrndetail where grnid=" + oid + "  and prodid=" + prdID + "";
                cmd1.Connection = conn;
                cmd1.Parameters.Clear();
                da1.SelectCommand = cmd1;
                da1.Fill(ds1, "tbl1");
                dt1 = ds1.Tables[0];
                qty = decimal.Parse(dt1.Rows[0]["GRNQty"].ToString());
            }

            return qty;
        }

        private void RecordUpdateStatus(long oid, long status, string objectname)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmdDetail = new SqlCommand();
            SqlDataAdapter daDetail = new SqlDataAdapter();
            DataSet dsDetail = new DataSet();
            DataTable dtDetail = new DataTable(); ;
            cmdDetail.CommandType = CommandType.StoredProcedure;
            cmdDetail.CommandText = "SP_MobileInsertQCDetailsUpdate";
            cmdDetail.Connection = conn;
            cmdDetail.Parameters.Clear();
            cmdDetail.Parameters.AddWithValue("Status", status);
            cmdDetail.Parameters.AddWithValue("Oid", oid);
            cmdDetail.Parameters.AddWithValue("objectname", objectname);
            cmdDetail.Connection.Open();
            cmdDetail.ExecuteNonQuery();
            cmdDetail.Connection.Close();
        }

        public DataSet GetUOMofPrd(long oid, string objectname, long prdID)
        {
            SqlCommand cmduom = new SqlCommand();
            SqlDataAdapter dauom = new SqlDataAdapter();
            DataSet dsuom = new DataSet();
            DataTable dtuom = new DataTable();

            SqlConnection conn = new SqlConnection(strcon);
            cmduom.CommandType = CommandType.Text;
            if (objectname == "PurchaseOrder") { cmduom.CommandText = "select * from  tgrndetail  where grnid=" + oid + " and prodid=" + prdID + ""; }
            else if (objectname == "Transfer") { cmduom.CommandText = "select * from ttransferdetail where TransferID=" + oid + " and SkuId=" + prdID + ""; }
            else if (objectname == "SalesReturn") { cmduom.CommandText = "select * from  tgrndetail  where grnid=" + oid + " and prodid=" + prdID + ""; }
            else if (objectname == "SalesOrder") { cmduom.CommandText = "select * from  tpickupdetail  where pickupid=" + oid + " and prodid=" + prdID + ""; }

            cmduom.Connection = conn;
            cmduom.Parameters.Clear();
            dauom.SelectCommand = cmduom;
            dauom.Fill(dsuom, "tbl1");
            dtuom = dsuom.Tables[0];

            return dsuom;
        }

        private DataSet GetAllDetails(long oid, long prdID, string objectname)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            cmd1.CommandType = CommandType.Text;
            if (objectname == "PurchaseOrder")
            {
                cmd1.CommandText = "select * from tgrndetail where grnid=" + oid + "  and prodid=" + prdID + "";
            }
            else if (objectname == "SalesOrder")
            {
                cmd1.CommandText = "select * from tpickupdetail where pickupid=" + oid + "  and prodid=" + prdID + "";
            }
            else if (objectname == "Transfer")
            {
                cmd1.CommandText = "select * from tTransferDetail where transferid=" + oid + "  and skuid=" + prdID + "";
            }
            else if (objectname == "SalesReturn")
            {
                cmd1.CommandText = "select * from tgrndetail where grnid=" + oid + "  and prodid=" + prdID + "";
            }

            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
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
            if (objNM == "PurchaseOrder" || objNM == "SalesOrder")
            {
                cmd2.CommandText = "select * from mstatus where Remark='POSO' and status like '%Quality Check%' ";
            }
            else if (objNM == "Transfer")
            {
                cmd2.CommandText = "select * from mstatus where Remark='Transfer' and status like '%QC%' ";
            }
            else if (objNM == "SalesReturn")
            {
                cmd2.CommandText = "select * from mstatus where Remark='Return' and status like '%QC%' ";
            }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}