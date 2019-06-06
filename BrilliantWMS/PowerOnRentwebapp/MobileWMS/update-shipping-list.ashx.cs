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
    /// Summary description for update_shipping_list1
    /// </summary>
    public class update_shipping_list1 : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1, locationID = 0, sortCode = 0;
        decimal capacity = 0, avlBlnce = 0;
        string objectname = "";
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            long orderID = long.Parse(context.Request.Form["oid"]);
            long uid = long.Parse(context.Request.Form["uid"]);
            objectname = context.Request.Form["objname"].ToString();
            string WorkFlow = "Outbound";
            string NextObject = "QC";
            long sequence = 0, prdID = 0;
            decimal qty = 0;
            string lCode = "", batchCode = "";
            string lot1str = "", lot2str = "", lot3str = "", lot1 = "", lot2 = "", lot3 = "";
            string prdString, qtystring, loccode, pid, bcode;
            int rslt = 0;
            long pkupID = 0;
            string objNM = "";
            string userName = GetUserID(uid);
            CustomProfile profile = CustomProfile.GetProfile(userName);
            iOutboundClient Outbound = new iOutboundClient();

            /*First Save PickUp List Data in Temparary Table start*/
            List<WMS_SP_PickUpList_Result> pickupLst = new List<WMS_SP_PickUpList_Result>();
            // pickupLst = Outbound.GetPickUpList(orderID.ToString(), "", HttpContext.Current.Session.SessionID, uid.ToString(), "PickUp", profile.DBConnection._constr).ToList();
            //pickupLst = Outbound.GetPickUpList(orderID.ToString(), "", context.Session.SessionID, uid.ToString(), "PickUp", profile.DBConnection._constr).ToList();
            /*First Save PickUp List Data in Temparary Table start*/
            long wid = long.Parse(context.Request.Form["wid"]);
            string putinDetails = context.Request.Form["putinDetails"];
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from  SplitString('" + putinDetails + "','@')";
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

                    string myputinstring = "select * from  SplitString('" + ds1.Tables[0].Rows[p]["part"].ToString() + "','|')";
                    ds.Clear();
                    dt.Clear();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = myputinstring;
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
                            //sequence
                            prdString = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                            string[] spltprdString = prdString.Split(':');
                            sequence = long.Parse(spltprdString[1]);
                            i = i + 1;

                            //qty
                            qtystring = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                            string[] spltqty = qtystring.Split(':');
                            qty = decimal.Parse(spltqty[1]);
                            i = i + 1;

                            //lcode
                            loccode = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                            string[] lotsplit = loccode.Split(':');
                            lCode = Convert.ToString(lotsplit[1]);
                            i = i + 1;
                            GetLocationDetails(lCode, wid);
                            /*Get Location Details*/

                            //Prodid
                            pid = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                            string[] lotsplit1 = pid.Split(':');
                            prdID = long.Parse(lotsplit1[1]);
                            i = i + 1;

                            //batch code
                            bcode = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                            string[] lotsplit2 = bcode.Split(':');
                            batchCode = Convert.ToString(lotsplit2[1]);
                            // i = i + 1;


                            /*Update List Qty Start*/
                            WMS_SP_PickUpList_Result pkupLst = new WMS_SP_PickUpList_Result();
                            pkupLst.Sequence = sequence;
                            pkupLst.LocQty = qty;
                            // Outbound.UpdatePkupLstLocofSelRow(HttpContext.Current.Session.SessionID, "PickUp", uid.ToString(), pkupLst, profile.DBConnection._constr);
                            //Outbound.UpdatePickUPLstQtyofSelRow(HttpContext.Current.Session.SessionID, "PickUp", uid.ToString(), pkupLst, profile.DBConnection._constr);
                            /*Update List Qty End*/

                            /*Update List Location Details Start*/
                            WMS_SP_PickUpList_Result pkLst = new WMS_SP_PickUpList_Result();
                            pkLst.Sequence = sequence;
                            pkLst.LocationID = locationID;
                            pkLst.Code = lCode;
                            pkLst.SortCode = sortCode;
                            pkLst.Capacity = capacity;
                            pkLst.AvailableBalance = avlBlnce;
                           // Outbound.UpdatePkupLstLocofSelRow(HttpContext.Current.Session.SessionID, "PickUp", uid.ToString(), pkLst, profile.DBConnection._constr);
                            //Outbound.UpdatePickUPLstQtyofSelRow(HttpContext.Current.Session.SessionID, "PickUp", uid.ToString(), pkLst, profile.DBConnection._constr);
                            /*Update List Location Details End*/

                            //serial number if avaliable
                            if (cntr >= 6)
                            {
                                i = i + 1;
                                lot1str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lotsplit4 = lot1str.Split(':');
                                lot1 = lotsplit4[1];
                            }
                            if (cntr >= 7)
                            {
                                i = i + 1;
                                lot2str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lot2split = lot2str.Split(':');
                                lot2 = lot2split[1];
                            }
                            if (cntr >= 8)
                            {
                                i = i + 1;
                                lot3str = ds.Tables[0].Rows[i]["part"].ToString().Trim();
                                string[] lot3split = lot3str.Split(':');
                                lot3 = lot3split[1];
                            }

                            //string objNM = "";

                            DataTable dtobjnm = new DataTable();
                            dtobjnm = GetDatatableDetails("select Object as Objectname from tOrderHead where id=" + orderID + " union all select Objectname from tTransferHead where id=" + orderID + "");
                            objNM = dtobjnm.Rows[0]["ObjectName"].ToString();

                            tPickUpHead ph = new tPickUpHead();
                            //ph.ObjectName = "SalesOrder";
                            ph.ObjectName = objectname;
                            ph.OID = orderID;
                            ph.PickUpDate = DateTime.Now;
                            ph.PickUpBy = uid;
                            ph.CreatedBy = uid;
                            ph.CreationDate = DateTime.Now;
                            int countnew = GetWorkFlow(profile.Personal.CompanyID, profile.Personal.CustomerId, WorkFlow, NextObject, profile.DBConnection._constr);
                            if (countnew > 0)
                            {
                                if (objectname == "SalesOrder") { ph.Status = 38; }
                                else if (objectname == "Transfer") { ph.Status = 57; }
                            }
                            else
                            {
                                if (objectname == "SalesOrder") { ph.Status = 32; }
                                else if (objectname == "Transfer") { ph.Status = 58; }
                            }
                            //if (objectname == "SalesOrder")
                            //{
                            //    ph.Status = 38;
                            //}
                            // else if(objectname == "Transfer")
                            //{
                            //    ph.Status = 57;
                            //}
                            ph.CompanyID = profile.Personal.CompanyID;
                            ph.CustomerID = profile.Personal.CustomerId;
                            ph.PickUpStatus = true;
                            // DataTable dtnew = new DataTable();
                            //   dtnew = GetDatatableDetails("select * from tPutInHead where oid=" + orderID + "");
                            //if (objectname == "SalesOrder")
                            //{   
                            //    dtnew = GetDatatableDetails("select * from tPickUpHead where oid=" + orderID + " and Status=32");
                            //}
                            //else if (objectname == "Transfer")
                            //{
                            //    dtnew = GetDatatableDetails("select * from tPickUpHead where oid=" + orderID + " and Status=58");  
                            //}

                            //if (dtnew.Rows.Count > 0)
                            //{   
                            //    pkupID = Convert.ToInt64(dtnew.Rows[0]["id"].ToString());
                            //}
                            //else
                            //{
                            //    pkupID = Outbound.SavetPickUpHead(ph, profile.DBConnection._constr);
                            //}
                            if (pkupID == 0)
                            {
                                pkupID = Outbound.SavetPickUpHead(ph, profile.DBConnection._constr);
                            }
                            if (pkupID > 0)
                            {
                                // rslt = Outbound.FinalSavePickUpDetail(orderID, HttpContext.Current.Session.SessionID, "PickUp", pkupID, uid.ToString(), Convert.ToInt32(ph.Status), profile.DBConnection._constr);
                                rslt = SavePickUpDetail(pkupID, prdID, qty, locationID, orderID, batchCode);
                            }

                            SavePickUpLottable(prdID, batchCode, locationID, qty, profile.Personal.CompanyID, profile.Personal.CustomerId, profile.Personal.UserID, lot1, lot2, lot3, pkupID);
                        }
                    }
                }
            }

            if (pkupID > 0)
            {
                // SaveInSkutransction();
                // INSERT INTO tskutransaction(skuid, batchcode, locationid, transtype, inqty, outqty, closingbalance, companyid, cutomerid, createdby, creationdate, lottable1, lottable2, lottable3)
                //SELECT tpd.Prodid,tpd.batchcode, tpd.locationid, 'Outbound',0,tpd.pickupqty,0,tph.companyid,tph.cutomerid,user,getdate(),
                //FROM tpickupdetail tpd left outer join tpickuphead tph on tpd.pickupid = tph.id
                //WHERE tph.id =
                iInboundClient Inbound = new iInboundClient();
                int count = 0;
                long QCID = 0;
                if (objectname == "SalesOrder")
                {
                    WorkFlow = "Outbound";
                    NextObject = "QC";
                }
                if (objectname == "Transfer")
                {
                    WorkFlow = "Transfer";
                    NextObject = "QCOut";
                }
                count = GetWorkFlow(profile.Personal.CompanyID, profile.Personal.CustomerId, WorkFlow, NextObject, profile.DBConnection._constr);
                if (count == 0)
                {
                    WMSOutbound.tQualityControlHead QCHead = new WMSOutbound.tQualityControlHead();
                    QCHead.CreatedBy = profile.Personal.UserID;
                    QCHead.Creationdate = DateTime.Now;
                    // QCHead.ObjectName = "SalesOrder";
                    QCHead.ObjectName = objectname;
                    QCHead.OID = pkupID;
                    QCHead.QCDate = DateTime.Now;
                    QCHead.QCBy = profile.Personal.UserID;
                    QCHead.Remark = "";

                    if (objectname == "SalesOrder") { QCHead.Status = 32; }
                    else if (objectname == "Transfer") { QCHead.Status = 58; }
                    //QCHead.Status = 32;

                    QCHead.Company = profile.Personal.CompanyID;
                    QCHead.CustomerID = profile.Personal.CustomerId;
                    QCID = Inbound.SavetQualityControlHead(QCHead, profile.DBConnection._constr);
                    if (QCID > 0)
                    {
                        DataSet dsQC = new DataSet();
                        dsQC = GetPickUpCount(pkupID, profile.DBConnection._constr);
                        if (dsQC.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsQC.Tables[0].Rows.Count; i++)
                            {
                                long prodid = Convert.ToInt64(dsQC.Tables[0].Rows[i]["ProdID"]);
                                long qtynew = Convert.ToInt64(dsQC.Tables[0].Rows[i]["PickUpQty"]);
                                long uom = Convert.ToInt64(dsQC.Tables[0].Rows[i]["UOMID"]);
                                //int RSLT1 = Inbound.FillQCDetail(QCID, PkUpID, profile.DBConnection._constr);
                                int RSLT1 = FillQCDetailNew(QCID, pkupID, prodid, qtynew, uom, profile.DBConnection._constr);
                            }
                        }

                    }
                    //update torderhead table
                    UpdateStatustorderhead(orderID);
                }

            }

            String xmlString = String.Empty;
            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            if (rslt > 0)
            {
                jsonString = jsonString + "{\n\"status\":\"success\"\n}\n";
            }
            else if (rslt == 0)
            {
                jsonString = jsonString + "{\n\"status\":\"failed\"\n}\n";
            }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        private void SavePickUpLottable(long prdID, string batchCode, long locationID, decimal qty, long companyID, long customerId, long userID, object lot1, object lot2, object lot3, long ptinID)
        {
            SqlConnection conn1 = new SqlConnection(strcon);
            SqlCommand cmdDetail = new SqlCommand();
            SqlDataAdapter daDetail = new SqlDataAdapter();
            DataSet dsDetail = new DataSet();
            DataTable dtDetail = new DataTable();
            cmdDetail.CommandType = CommandType.StoredProcedure;
            cmdDetail.CommandText = "SP_InsertintotskutransactionhistoryForPickUp";
            cmdDetail.Connection = conn1;
            cmdDetail.Parameters.Clear();
            cmdDetail.Parameters.AddWithValue("skuid", prdID);
            cmdDetail.Parameters.AddWithValue("lottable1", lot1);
            cmdDetail.Parameters.AddWithValue("qty", qty);
            cmdDetail.Parameters.AddWithValue("oid", ptinID);
            cmdDetail.Parameters.AddWithValue("companyid", companyID);
            cmdDetail.Parameters.AddWithValue("customerid", customerId);
            cmdDetail.Parameters.AddWithValue("batchcode", batchCode);
            cmdDetail.Parameters.AddWithValue("locationid", locationID);
            cmdDetail.Parameters.AddWithValue("createdby", userID);
            cmdDetail.Parameters.AddWithValue("lottable2", lot2);
            cmdDetail.Parameters.AddWithValue("lottable3", lot3);
            cmdDetail.Connection.Open();
            cmdDetail.ExecuteNonQuery();
            cmdDetail.Connection.Close();
        }

        private void UpdatePicjUpStatus(long pkid)
        {
            SqlConnection conn1 = new SqlConnection(strcon);
            SqlCommand cmdDetail = new SqlCommand();
            SqlDataAdapter daDetail = new SqlDataAdapter();
            DataSet dsDetail = new DataSet();
            DataTable dtDetail = new DataTable();
            cmdDetail.CommandType = CommandType.Text;
            if (objectname == "SalesOrder")
            {
                cmdDetail.CommandText = "Update tPickUphead set status=32";
            }
            else if (objectname == "Transfer")
            {
                cmdDetail.CommandText = "Update tPickUphead set status=58";
            }
            cmdDetail.Connection = conn1;
            cmdDetail.Parameters.Clear();
            cmdDetail.Connection.Open();
            cmdDetail.ExecuteNonQuery();
            cmdDetail.Connection.Close();
        }


        private DataTable GetOrderQty(long ID)
        {
            string str = "";
            DataTable dtqty = new DataTable();
            if (objectname == "SalesOrder")
            {
                str = "select ID from tOrderDetail where RemaningQty>0 and OrderHeadId='" + ID + "'";
            }
            else
            {
                str = "select ID from tTransferDetail where RemaningQty>0 and TransferID='" + ID + "'";
            }
            dtqty = GetDatatableDetails(str);
            return dtqty;

        }

        private void UpdateStatustorderhead(long orderID)
        {
            string objNM = "";

            DataTable dtobjnm = new DataTable();
            DataTable dtqty = new DataTable();
            dtobjnm = GetDatatableDetails("select Object as Objectname from tOrderHead where id=" + orderID + " union all select Objectname from tTransferHead where id=" + orderID + "");
            objNM = dtobjnm.Rows[0]["ObjectName"].ToString();

            SqlConnection conn33 = new SqlConnection(strcon);
            SqlCommand cmduom = new SqlCommand();
            SqlDataAdapter dauom = new SqlDataAdapter();
            DataSet dsuom = new DataSet();
            DataTable dtuom = new DataTable();
            cmduom.CommandType = CommandType.Text;
            dtqty = GetOrderQty(orderID);
            if (objectname == "SalesOrder")
            {
                int status = 0;
                if (dtqty.Rows.Count > 0)
                {
                    status = 69;
                }
                else
                {
                    status = 32;
                }
                cmduom.CommandText = "update torderhead set status='" + status + "' where id=" + orderID + "";
            }
            else if (objectname == "Transfer")
            {
                int status = 0;
                if (dtqty.Rows.Count > 0)
                {
                    status = 70;
                }
                else
                {
                    status = 58;
                }
                cmduom.CommandText = "update tTransferHead set status='" + status + "' where id=" + orderID + "";
            }
            cmduom.Connection = conn33;
            cmduom.Parameters.Clear();
            cmduom.Connection.Open();
            cmduom.ExecuteNonQuery();
            cmduom.Connection.Close();
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

        private int SavePickUpDetail(long pickupid, long prodid, decimal qty, long locid, long orderID, string Batchcode)
        {
            int count = 0;
            SqlConnection conn4 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SavePickUpDetailMobile";
            cmd.Connection = conn4;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@pickupid", pickupid);
            cmd.Parameters.AddWithValue("@prodid", prodid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@locid", locid);
            cmd.Parameters.AddWithValue("@orderID", orderID);
            cmd.Parameters.AddWithValue("@Batchcode", Batchcode);
            cmd.Connection.Open();
            count = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return count;
        }

        public int FillQCDetailNew(long qcid, long pickupid, long prodid, decimal qty, long uom, string[] conn)
        {
            int count = 0;
            SqlConnection conn4 = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_FillQCDetailNew";
            cmd.Connection = conn4;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@qcid", qcid);
            cmd.Parameters.AddWithValue("@pickupid", pickupid);
            cmd.Parameters.AddWithValue("@prodid", prodid);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@uom", uom);
            cmd.Connection.Open();
            count = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return count;
        }

        public DataSet GetPickUpCount(long pid, string[] conn)
        {
            SqlConnection conn3 = new SqlConnection(strcon);
            SqlCommand cmduom = new SqlCommand();
            SqlDataAdapter dauom = new SqlDataAdapter();
            DataSet dsuom = new DataSet();
            DataTable dtuom = new DataTable();
            cmduom.CommandType = CommandType.Text;
            cmduom.CommandText = "select * from  tpickupdetail where PickUpID=" + pid + "";
            cmduom.Connection = conn3;
            cmduom.Parameters.Clear();
            dauom.SelectCommand = cmduom;
            dauom.Fill(dsuom, "tbl1");
            dtuom = dsuom.Tables[0];
            return dsuom;
        }

        public int GetWorkFlow(long CompID, long CustID, string WorkFlow, string Obj, string[] conn)
        {
            int count = 0;
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
            return count;
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

        public void GetLocationDetails(string lCode, long wid)
        {
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();

            SqlConnection conn = new SqlConnection(strcon);
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "select ID,Code,SortCode,Capacity,AvailableBalance from mLocation where Code='" + lCode + "' and WarehouseID='" + wid + "'";
            cmd2.Connection = conn;
            cmd2.Parameters.Clear();
            da2.SelectCommand = cmd2;
            da2.Fill(ds2, "tbl2");
            dt2 = ds2.Tables[0];
            if (dt2.Rows.Count > 0)
            {
                locationID = long.Parse(dt2.Rows[0]["ID"].ToString());
                sortCode = long.Parse(dt2.Rows[0]["SortCode"].ToString());
                capacity = decimal.Parse(dt2.Rows[0]["Capacity"].ToString());
                avlBlnce = decimal.Parse(dt2.Rows[0]["AvailableBalance"].ToString());
            }
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