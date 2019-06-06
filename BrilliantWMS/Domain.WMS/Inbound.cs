using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using System.ServiceModel;
using System.Data.Entity;
using Domain.Server;
using Domain.Tempdata;
using System.Xml.Linq;
using System.Data.Objects;
using System.Threading.Tasks;
using Interface.WMS;

namespace Domain.WMS
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public partial class WMS : Interface.WMS.iInbound
    {
        Domain.Server.Server svr = new Server.Server();
        DataHelper datahelper = new DataHelper();

        #region Inbound Grid

        protected DataSet fillds(string strquery, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            SqlDataAdapter da = new SqlDataAdapter(strquery, sqlConn);
            ds.Reset();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindInboundGrid(long userCompany, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_InboundList where CompanyID=" + userCompany + "  order by id desc";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet BindInboundGridbyUser(long userID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_InboundList where RequestBy=" + userID + "  order by id desc";
            ds = fillds(str, conn);
            return ds;
        }

        public int CheckSelectedPOStatusIsSameOrNot(string SelectedPO, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Count(*)Cnt,Status from tPurchaseOrderHead where Id in(" + SelectedPO + ") Group By Status";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows.Count;
        }

        public DataSet GetNextObject(string SelectedRecords, string ObjectName, long CompanyID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "";
            string WorkFlow = "";
            string WorkFlowID = "";
            if (ObjectName == "PurchaseOrder")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tPurchaseOrderHead where Id in(" + SelectedRecords + ") Group By Status))";
                WorkFlow = "Inbound";
                WorkFlowID = "1";
            }
            else if (ObjectName == "GRN")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tGRNHead where Id in(" + SelectedRecords + ") Group By Status))";
                WorkFlow = "Inbound";
                WorkFlowID = "1";
            }
            else if (ObjectName == "QC")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tQualityControlHead where Id in(" + SelectedRecords + ") Group By Status))";
                WorkFlow = "Inbound";
                WorkFlowID = "1";
            }
            else if (ObjectName == "LabelPrinting")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tQualityControlHead where Id in(" + SelectedRecords + ") Group By Status))";
                WorkFlow = "Inbound";
                WorkFlowID = "1";
            }
            else if (ObjectName == "SalesOrder")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tOrderHead where Id in(" + SelectedRecords + ") Group By Status))";
                WorkFlow = "Outbound";
                WorkFlowID = "2";
            }
            ds = fillds(str, conn);
            int ObjID = Convert.ToInt16(ds.Tables[0].Rows[0]["OBJECTID"].ToString());

            DataSet dsNewObject = new DataSet();
            dsNewObject.Reset();
            string st = "select Top(1)WH.WorkflowName,WD.ObjectID,WD.Sequence,o.ObjectName from mWorkFlowHead WH left outer join mWorkFlowDetail WD on WH.ID=WD.WorkflowID left outer join mObject o on WD.ObjectID=o.OBJECTID where WH.CompanyID=" + CompanyID + " and WH.WorkflowName='" + WorkFlow + "' and WD.ObjectID > " + ObjID + " and WD.Sequence > (select Sequence from mWorkFlowDetail where ObjectID=" + ObjID + " and CompanyID=" + CompanyID + " and WorkflowID=(select ID from mWorkFlowHead where CompanyID=" + CompanyID + " and WorkflowName='" + WorkFlow + "'))  order by WD.Sequence ";
            dsNewObject = fillds(st, conn);

            return dsNewObject;
        }

        public bool SaveAssignedTask(tTaskDetail objTask, string OrderObjectName, string OID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.tTaskDetails.AddObject(objTask);
            ce.SaveChanges();

            string[] TotOID = OID.Split(',');
            int OrderCount = TotOID.Count();
            for (int i = 0; i <= OrderCount - 1; i++)
            {
                tJobCardDetail objJob = new tJobCardDetail();
                objJob.TaskID = objTask.TaskID;
                objJob.OID = long.Parse(TotOID[i].ToString());
                objJob.OrderObjectName = OrderObjectName;

                ce.tJobCardDetails.AddObject(objJob);
                ce.SaveChanges();
            }
            return true;
        }

        public bool CheckJobCardofSelectedRecord(string SelectedPO, string Objectname, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from tJobCardDetail where OrderObjectName='" + Objectname + "' and OID in(" + SelectedPO + ")";
            ds = fillds(str, conn);
            int Cnt = ds.Tables[0].Rows.Count;
            if (Cnt > 0)
                return false;
            else
                return true;
        }

        public int CancelSelectedOrder(long SelectedOrder, long UserID, string[] conn)
        {
            int result = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tPurchaseOrderHead porder = new tPurchaseOrderHead();
            porder = db.tPurchaseOrderHeads.Where(o => o.Id == SelectedOrder && o.CreatedBy == UserID).FirstOrDefault();
            if (porder != null)
            {
                long OrderStatus = long.Parse(porder.Status.ToString());
                if (OrderStatus == 43 || OrderStatus == 41 || OrderStatus == 45)
                {
                    DataSet ds = new DataSet();
                    ds = fillds("update tPurchaseOrderHead set Status=28 where id=" + SelectedOrder + "", conn); // Update Order Status
                    //UpdateAvailableBalanceAfterRequestReject(SelectedOrder, conn); // Update Stock
                    //SendEmailWhenOrderCancelByRequestor(SelectedOrder, conn);                   //Email 
                    result = 1;
                }
                else
                { result = 0; }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        #endregion

        #region PO

        public List<mWarehouseMaster> GetUserWarehouse(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseMaster> UsrW = new List<mWarehouseMaster>();
            UsrW = (from w in ce.mWarehouseMasters
                    join u in ce.mUserWarehouses on w.ID equals u.WarehoueID
                    where u.UserID == UserID
                    select w).ToList();
            return UsrW;
        }

        public List<mVendor> GetVendor(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mVendor> Vndr = new List<mVendor>();
            Vndr = (from v in ce.mVendors
                    where v.CompanyID == CompanyID && v.VCType == 84
                    select v).ToList();
            return Vndr;
        }

        public void ClearTempDataFromDBNEW(string paraSessionID, string paraUserID, string CurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.UserID == paraUserID
                        && rec.ObjectName == CurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<mStatu> GetStatusListForInbound(string ObjectName, string Remark, string state, long UserID, string[] conn)
        {
            List<mStatu> statusdetail = new List<mStatu>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                string[] RemarkArr = Remark.Split(',');
                //  if (Remark != "" && ObjectName!="")
                if (Remark == "" && ObjectName != "")
                {
                    statusdetail = (from st in db.mStatus
                                    where (st.ObjectName == ObjectName) //&& RemarkArr.Contains(st.Remark))
                                    select st).OrderBy(st => st.Sequence).ToList();
                }
                else if (Remark != "" & ObjectName == "")
                {
                    statusdetail = (from st in db.mStatus
                                    where (RemarkArr.Contains(st.Remark))
                                    select st).OrderBy(st => st.Sequence).ToList();
                }
            }
            catch { }
            finally { }
            return statusdetail;
        }

        public int GetWorkflowSequenceOfPO(string ObjectName, long CompanyID, string[] conn)
        {
            int Seq = 0;
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                mWorkFlowDetail WD = new mWorkFlowDetail();
                WD = (from h in db.mWorkFlowHeads
                      join d in db.mWorkFlowDetails on h.ID equals d.WorkflowID
                      join o in db.mObjects on d.ObjectID equals o.OBJECTID
                      where h.CompanyID == CompanyID && h.WorkflowName == "Inbound" && o.ObjectName == ObjectName
                      select d).FirstOrDefault();
                Seq = Int16.Parse(WD.Sequence.ToString());
            }
            catch { }
            finally { }
            return Seq;
        }

        public List<WMS_SP_GetPartDetail_ForPO_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetail_ForPO_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public List<WMS_SP_GetPartDetail_ForPO_Result> AddPartIntoRequest_TempData(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForPO_Result> existingList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            existingList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            long MaxSequenceNo = 0;
            if (existingList.Count > 0)
            {
                MaxSequenceNo = Convert.ToInt64((from lst in existingList
                                                 select lst.Sequence).Max().Value);
            }

            /*Get Product Details*/
            List<WMS_SP_GetPartDetail_ForPO_Result> getnewRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getnewRec = (from view in db.WMS_SP_GetPartDetail_ForPO(paraProductIDs, MaxSequenceNo, WarehouseID, 0)
                         orderby view.Sequence
                         select view).ToList();
            /*End*/

            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            List<WMS_SP_GetPartDetail_ForPO_Result> mergedList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            mergedList.AddRange(existingList);
            mergedList.AddRange(getnewRec);
            /*End*/

            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDB(mergedList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return mergedList;
        }

        protected void SaveTempDataToDB(List<WMS_SP_GetPartDetail_ForPO_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(paraobjList);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObjectName.ToString();
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();
            /*End*/

        }

        public void ClearTempDataFromDB(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraCurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<WMS_SP_GetPartDetail_ForPO_Result> GetRequestPartDetailByRequestID(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> PartDetail = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            PartDetail = (from sp in db.WMS_SP_GetPartDetail_ForPO("0", 0, WarehouseID, RequestID)
                          select sp).ToList();
            SaveTempDataToDB(PartDetail, sessionID, userID, CurrentObject, conn);
            return PartDetail;
        }

        public DataSet GetUOMofSelectedProduct(int ProdID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select PackUomid,SkuId,ShortDescri,Description,Quantity,UOMID,Sequence, (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " order by Sequence ", conn);
            return ds;
        }

        public string GetSelectedUom(long OrderId, long ProdID, long sequence, string[] conn)
        {
            string uomid = "";
            DataSet ds = new DataSet();
            //ds = fillds("select UOMID from tOrderDetail where Orderheadid="+ OrderId +" and SkuId="+ ProdID +"", conn);
            ds = fillds("select (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " and UOMID=(select UOMID from tPurchaseOrderDetail where POOrderHeadId=" + OrderId + " and SkuId=" + ProdID + " and Sequence=" + sequence + ")", conn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                uomid = ds.Tables[0].Rows[0]["UMOGroup"].ToString();
            }
            else { uomid = "0"; }
            return uomid;
        }

        public tPurchaseOrderHead GetOrderHeadByOrderID(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tPurchaseOrderHead POH = new tPurchaseOrderHead();
            POH = db.tPurchaseOrderHeads.Where(p => p.Id == OrderID).FirstOrDefault();
            db.tPurchaseOrderHeads.Detach(POH);
            return POH;
        }

        public List<WMS_SP_GetPartDetail_ForPO_Result> RemovePartFromRequest_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForPO_Result> existingList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            existingList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Get Filter List [Filter By paraSequence]*/
            List<WMS_SP_GetPartDetail_ForPO_Result> filterList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/
            /*Save result to TempData*/
            SaveTempDataToDB(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<WMS_SP_GetPartDetail_ForPO_Result> NewList = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            NewList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForPO_Result UpdRec = new WMS_SP_GetPartDetail_ForPO_Result();

            if (cnt > 0)
            {
                for (int i = paraSequence; i <= cnt; i++)
                {
                    UpdRec = NewList.Where(u => u.Sequence == i + 1).FirstOrDefault();
                    UpdRec.Sequence = i;
                    SaveTempDataToDB(NewList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
                }
            }
            /*End*/
            if (cnt > 0)
            { return NewList; }
            else { return filterList; }
        }

        public string GetUOMName(long UOMID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select Description from muom where ID=" + UOMID + "", conn);
            string UOM = ds.Tables[0].Rows[0]["Description"].ToString();
            return UOM;
        }

        public void UpdatePartRequest_TempData1(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public decimal GetTotalFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            //totPrice = getRec.Sum(s => s.Total);
            totPrice = getRec.Sum(s => s.AmountAfterTax);
            return totPrice;
        }

        public decimal GetTotalQTYFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            totPrice = getRec.Sum(s => s.RequestQty);
            return totPrice;
        }

        public int IsPriceChanged(int ProdID, decimal price, string[] conn)
        {
            int chng = 0;
            DataSet ds = new DataSet();
            ds = fillds("select PrincipalPrice from mproduct where id= " + ProdID + "", conn);
            decimal pp = Convert.ToDecimal(ds.Tables[0].Rows[0]["PrincipalPrice"].ToString());
            if (pp == price) { chng = 0; }
            else { chng = 1; }
            return chng;
        }

        public void UpdatePartRequest_TempData12(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty; updateRec.UOM = Request.UOM;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            updateRec.Price = Request.Price;
            updateRec.IsPriceChange = Request.IsPriceChange;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public decimal GetTotalQTYofSequenceFromTempData(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            updateRec = getRec.Where(s => s.Sequence == Sequence).FirstOrDefault();
            totPrice = updateRec.Total; long PrdID = updateRec.Prod_ID;
            return totPrice;
        }

        public long GetPrdIDofSequenceFromTempData(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            long PrdID = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            updateRec = getRec.Where(s => s.Sequence == Sequence).FirstOrDefault();
            PrdID = updateRec.Prod_ID;
            return PrdID;
        }

        public void UpdatePartRequest_TempData13(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForPO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForPO_Result> getRec = new List<WMS_SP_GetPartDetail_ForPO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForPO_Result updateRec = new WMS_SP_GetPartDetail_ForPO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();
            updateRec.TotalTaxAmount = Request.TotalTaxAmount;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public long SetIntotPurchaseOrderHead(tPurchaseOrderHead POHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (POHead.Id == 0)
            {
                db.tPurchaseOrderHeads.AddObject(POHead);
            }
            else
            {
                db.tPurchaseOrderHeads.Attach(POHead);
                db.ObjectStateManager.ChangeObjectState(POHead, EntityState.Modified);
            }
            db.SaveChanges();
            return POHead.Id;
        }

        public int FinalSavePODetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var tPurchaseOrderDetails = 0;
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetail_ForPO_Result> fnlSaveLst = new List<WMS_SP_GetPartDetail_ForPO_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

                XElement xmlEle = new XElement("PO", from rec in fnlSaveLst
                                                     select new XElement("PartList",
                                                         new XElement("PRH_ID", paraReferenceID),
                                                       new XElement("Prod_ID", Convert.ToInt64(rec.Prod_ID)),
                                                       new XElement("Prod_Name", rec.Prod_Name),
                                                       new XElement("Prod_Description", rec.Prod_Description),
                                                       new XElement("RequestQty", Convert.ToDecimal(rec.RequestQty)),
                                                       new XElement("RemaningQty", Convert.ToDecimal(rec.RequestQty)),
                                                       new XElement("Sequence", Convert.ToInt64(rec.Sequence)),
                                                       new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                       new XElement("Price", Convert.ToDecimal(rec.Price)),
                                                       new XElement("Total", Convert.ToDecimal(rec.Total)),
                                                       new XElement("IsPriceChange", Convert.ToInt16(rec.IsPriceChange)),
                                                       new XElement("Prod_Code", rec.Prod_Code),
                                                       new XElement("ReceivedQty", 0)
                                                       ));

                ObjectParameter _PRH_ID = new ObjectParameter("PRH_ID", typeof(long));
                _PRH_ID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _PRH_ID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotPurchaseOrderDetail", obj);
                db.SaveChanges(); tPurchaseOrderDetails = 1; Result = 1;

                /*Add Record Of User into table tOrderWiseAccess*/
                DataSet dsChkApproval = new DataSet();
                dsChkApproval = fillds("select * FROM mWorkFlowDetail WHERE WorkflowID = 1 and ObjectID=10", conn);
                if (dsChkApproval.Tables[0].Rows.Count > 0)
                {//This code is not for Shop Drive
                    DataSet dsChkRecordOfUser = new DataSet();
                    dsChkRecordOfUser = fillds("select * from tOrderwiseAccess  where orderID=" + paraReferenceID + " and Usertype='User'", conn);
                    if (dsChkRecordOfUser.Tables[0].Rows.Count == 0)
                    {
                        int IsPriceChenged = 0;
                        DataSet dsIsPriceChange = new DataSet();
                        dsIsPriceChange = fillds("select * from torderdetail where orderheadid=" + paraReferenceID + " and IsPriceChange=1", conn);
                        int rowcount = dsIsPriceChange.Tables[0].Rows.Count;
                        if (rowcount > 0)
                        { IsPriceChenged = 1; }
                        else { IsPriceChenged = 0; }

                        //tOrderWiseAccess ODA = new tOrderWiseAccess();
                        //ODA.UserApproverID = long.Parse(paraUserID);
                        //ODA.ApprovalLevel = 0;
                        //if (IsPriceChenged == 1)
                        //{
                        //    ODA.PriceEdit = false;
                        //    ODA.SkuQtyEdit = false;
                        //}
                        //else
                        //{
                        //    ODA.PriceEdit = false;
                        //    ODA.SkuQtyEdit = true;
                        //}
                        //ODA.UserType = "User";
                        //ODA.OrderID = paraReferenceID;
                        //ODA.Date = DateTime.Now;
                        // db.AddTotOrderWiseAccesses(ODA);
                        db.SaveChanges();
                    }
                    /*Add Record Of User into table tOrderWiseAccess*/
                    if (StatusID == 45) { }
                    else
                    {
                        tOrderDetail ODT = new tOrderDetail();
                        ODT = db.tOrderDetails.Where(r => r.OrderHeadId == paraReferenceID).FirstOrDefault();
                        if (ODT != null)
                        {
                            //Result = SetApproverDataafterSave(paraCurrentObjectName, paraReferenceID, paraUserID, StatusID, DepartmentID, PreviousStatusID, conn);
                        }
                        else
                        {
                            long OrderID = paraReferenceID;
                            //RollBack(OrderID, conn);
                            Result = 0;
                        }
                    }
                }
                ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                /*Code For Add Product Wise Tax Details Start*/
                FinalSaveProductLeveltax("PurchaseOrderTax", paraReferenceID, paraSessionID, conn);
                FinalSaveTaxOnTotal("PurchaseOrderTotalTax", paraReferenceID, paraSessionID, conn);
                /*Code For Add Product Wise Tax Details End*/
            }
            catch (System.Exception ex)
            {
                if (tPurchaseOrderDetails == 0)
                {
                    long OrderID = paraReferenceID;
                    //RollBack(OrderID, conn);
                    Result = 0;
                }
            }
            finally { }
            return Result;
        }

        //public int SetApproverDataafterSave(string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, long PreviousStatusID, string[] conn)
        //{
        //    BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
        //    var ApprovalDetail = 0;
        //    int Rslt = 0, Suecc = 0;
        //    try
        //    {
        //        if (PreviousStatusID == 2) { }
        //        else
        //        {
        //            if (StatusID == 2)
        //            {
        //                /* Insert record of Approval Level 1 in tApprovalTrans Table===>>>> */

        //                /* If Price is change of product then add Financial Approver at Approval Level 1  START*/
        //                /*Check if Price is Changed or not*/
        //                int IsPriceChenged = 0;
        //                DataSet dsIsPriceChange = new DataSet();
        //                dsIsPriceChange = fillds("select * from torderdetail where orderheadid=" + paraReferenceID + " and IsPriceChange=1", conn);
        //                int rowcount = dsIsPriceChange.Tables[0].Rows.Count;
        //                if (rowcount > 0)
        //                {
        //                    IsPriceChenged = 1; long FinanAppId = 0;
        //                    DataSet dsFinanAppID = new DataSet();
        //                    dsFinanAppID = fillds("select FinApproverID from mterritory where id=" + DepartmentID + "", conn);
        //                    FinanAppId = Convert.ToInt64(dsFinanAppID.Tables[0].Rows[0]["FinApproverID"].ToString());

        //                    SqlCommand cmd = new SqlCommand();
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.CommandText = "SP_Insert_tapprovaltrans";
        //                    cmd.Connection = svr.GetSqlConn(conn);
        //                    cmd.Parameters.Clear();
        //                    cmd.Parameters.AddWithValue("OrderId", paraReferenceID);
        //                    cmd.Parameters.AddWithValue("StoreId", DepartmentID);
        //                    cmd.Parameters.AddWithValue("UserId", Convert.ToInt64(paraUserID));
        //                    cmd.Parameters.AddWithValue("ApprovalId", 1);
        //                    cmd.Parameters.AddWithValue("ApproverID", FinanAppId);
        //                    cmd.Parameters.AddWithValue("Status", StatusID);
        //                    cmd.ExecuteNonQuery();

        //                    Rslt = EmailSendToApprover(FinanAppId, paraReferenceID, conn);

        //                    /*Add Record Of User into table tOrderWiseAccess*/
        //                    tOrderWiseAccess ODA = new tOrderWiseAccess();
        //                    ODA.UserApproverID = FinanAppId;
        //                    ODA.ApprovalLevel = 1;
        //                    ODA.PriceEdit = true;
        //                    ODA.SkuQtyEdit = false;
        //                    ODA.UserType = "Financial Approver";
        //                    ODA.OrderID = paraReferenceID;
        //                    ODA.Date = DateTime.Now;
        //                    ODA.ApproverLogic = "AND";
        //                    db.AddTotOrderWiseAccesses(ODA);
        //                    db.SaveChanges();
        //                    /*Add Record Of User into table tOrderWiseAccess*/
        //                    AddAllApprovalLevel(IsPriceChenged, paraReferenceID, DepartmentID, conn);
        //                }
        //                else
        //                {
        //                    IsPriceChenged = 0;
        //                    /* If Price is change of product then add Financial Approver at Approval Level 1  END*/
        //                    /*Add Record Of User into table tOrderWiseAccess*/
        //                    AddAllApprovalLevel(IsPriceChenged, paraReferenceID, DepartmentID, conn);

        //                    /*New Code After tOrderWiseAccess able added for order wise approval level start*/
        //                    DataSet dsFirstApprover = new DataSet();
        //                    dsFirstApprover = fillds("select OW.ID,OW.UserApproverID,OW.ApprovalLevel,OW.PriceEdit,OW.SkuQtyEdit,OW.UserType,OW.OrderID,OW.ApproverLogic ,Dl.DeligateTo from tOrderWiseAccess  OW left outer join tOrderHead OH on OW.OrderID=OH.ID left outer join mDeligate AS Dl ON OW.UserApproverID = Dl.DeligateFrom and CONVERT(VARCHAR(10), GETDATE(), 111) <=Convert(VARCHAR(10), Dl.ToDate,111) and CONVERT(VARCHAR(10), GETDATE(), 111) >=Convert(VARCHAR(10), Dl.FromDate,111)  and OH.StoreId=Dl.DeptID where OW.OrderID=" + paraReferenceID + " and OW.UserType != 'User' and OW.ApprovalLevel=1", conn);
        //                    int CntFirstApprover = dsFirstApprover.Tables[0].Rows.Count;
        //                    if (CntFirstApprover > 0)
        //                    {
        //                        for (int i = 0; i <= CntFirstApprover - 1; i++)
        //                        {
        //                            SqlCommand cmd1 = new SqlCommand();
        //                            cmd1.CommandType = CommandType.StoredProcedure;
        //                            cmd1.CommandText = "SP_Insert_tapprovaltrans";
        //                            cmd1.Connection = svr.GetSqlConn(conn);
        //                            cmd1.Parameters.Clear();
        //                            cmd1.Parameters.AddWithValue("OrderId", paraReferenceID);
        //                            cmd1.Parameters.AddWithValue("StoreId", DepartmentID);
        //                            cmd1.Parameters.AddWithValue("UserId", Convert.ToInt64(paraUserID));
        //                            cmd1.Parameters.AddWithValue("ApprovalId", 1);
        //                            cmd1.Parameters.AddWithValue("ApproverID", Convert.ToInt64(dsFirstApprover.Tables[0].Rows[i]["UserApproverID"].ToString()));
        //                            cmd1.Parameters.AddWithValue("Status", StatusID);
        //                            cmd1.ExecuteNonQuery();

        //                            ApprovalDetail = 1;

        //                            /*Send Email to Approvers*/
        //                            Rslt = EmailSendToApprover(Convert.ToInt64(dsFirstApprover.Tables[0].Rows[i]["UserApproverID"].ToString()), paraReferenceID, conn);
        //                        }
        //                    }
        //                }
        //                /*New Code After tOrderWiseAccess able added for order wise approval level end*/

        //                tApprovalTran APRT = new tApprovalTran();
        //                APRT = db.tApprovalTrans.Where(rec => rec.OrderId == paraReferenceID).FirstOrDefault();
        //                if (APRT != null)
        //                {

        //                    Rslt = EmailSendWhenRequestSubmit(paraReferenceID, conn); //if Rslt =2 then mail sent to requestor else if Rslt=3 then mail not sent to requestoe
        //                    /*Insert record of Auto Cancellation Reminder & Approval reminder in tCorrespond table START*/
        //                    mTerritory Dept = new mTerritory();
        //                    Dept = db.mTerritories.Where(r => r.ID == DepartmentID).FirstOrDefault();
        //                    long OrderCancelDays = 0, ApprovalReminderSchedule = 0, AutoCancelReminderSchedule = 0;
        //                    if (Dept != null)
        //                    {
        //                        OrderCancelDays = long.Parse(Dept.cancelDays.ToString());
        //                        ApprovalReminderSchedule = long.Parse(Dept.ApproRemSchedul.ToString());
        //                        AutoCancelReminderSchedule = long.Parse(Dept.AutoRemSchedule.ToString());
        //                    }

        //                    DataSet dsGetOrderDate = new DataSet();
        //                    dsGetOrderDate = fillds("select OrderDate from torderhead where id=" + paraReferenceID + "", conn);
        //                    DateTime OrdrDate = Convert.ToDateTime(dsGetOrderDate.Tables[0].Rows[0]["OrderDate"].ToString());

        //                    DataSet dsAutocancel = new DataSet();
        //                    dsAutocancel = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=10) and MessageID=(select Id from mDropdownValues where Value='Reminder') and DepartmentID=" + DepartmentID + " ", conn);
        //                    if (dsAutocancel.Tables[0].Rows.Count > 0)
        //                    {
        //                        tCorrespond Cor = new tCorrespond();
        //                        Cor.OrderHeadId = paraReferenceID;
        //                        Cor.MessageTitle = dsAutocancel.Tables[0].Rows[0]["MailSubject"].ToString();
        //                        Cor.Message = dsAutocancel.Tables[0].Rows[0]["MailBody"].ToString();
        //                        Cor.date = DateTime.Now;
        //                        Cor.MessageSource = "Scheduler";
        //                        Cor.MessageType = "Reminder";
        //                        Cor.DepartmentID = DepartmentID;
        //                        // Cor.OrderDate = DateTime.Now;
        //                        Cor.OrderDate = OrdrDate;
        //                        Cor.CurrentOrderStatus = StatusID;
        //                        Cor.MailStatus = 0;
        //                        Cor.OrderCancelDays = OrderCancelDays;
        //                        Cor.AutoCancelReminderSchedule = AutoCancelReminderSchedule;
        //                        //Cor.ApprovalReminderSchedule = ApprovalReminderSchedule;
        //                        Cor.NxtAutoCancelReminderDate = DateTime.Now.AddDays(AutoCancelReminderSchedule);
        //                        Cor.Archive = false;

        //                        db.tCorresponds.AddObject(Cor);
        //                        db.SaveChanges();
        //                    }

        //                    DataSet dsApprovalReminder = new DataSet();
        //                    dsApprovalReminder = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2 and value='Order Submit') and MessageID=(select Id from mDropdownValues where Value='Reminder') and DepartmentID=" + DepartmentID + " ", conn);
        //                    if (dsApprovalReminder.Tables[0].Rows.Count > 0)
        //                    {
        //                        tCorrespond Cor = new tCorrespond();
        //                        Cor.OrderHeadId = paraReferenceID;
        //                        Cor.MessageTitle = dsApprovalReminder.Tables[0].Rows[0]["MailSubject"].ToString();
        //                        Cor.Message = dsApprovalReminder.Tables[0].Rows[0]["MailBody"].ToString();
        //                        Cor.date = DateTime.Now;
        //                        Cor.MessageSource = "Scheduler";
        //                        Cor.MessageType = "Reminder";
        //                        Cor.DepartmentID = DepartmentID;
        //                        Cor.OrderDate = OrdrDate; //DateTime.Now;
        //                        Cor.CurrentOrderStatus = StatusID;
        //                        Cor.MailStatus = 0;
        //                        // Cor.OrderCancelDays = OrderCancelDays;
        //                        // Cor.AutoCancelReminderSchedule = AutoCancelReminderSchedule;
        //                        Cor.ApprovalReminderSchedule = ApprovalReminderSchedule;
        //                        Cor.NxtApprovalReminderDate = DateTime.Now.AddDays(ApprovalReminderSchedule);
        //                        Cor.Archive = false;

        //                        db.tCorresponds.AddObject(Cor);
        //                        db.SaveChanges();
        //                    }


        //                    /* Update tProductstockDetails Reserve Quantity & Available Balance START>>>>>>>> */
        //                    UpdateTProductStockReserveQtyAvailBalance(paraReferenceID, conn);
        //                    /* <<<<<<<<Update tProductstockDetails Reserve Quantity & Available Balance END */
        //                    Suecc = 1;
        //                }
        //                else
        //                {
        //                    long OrdrID = paraReferenceID;
        //                    RollBack(OrdrID, conn);
        //                    Suecc = 0;
        //                }

        //            }
        //            else if (StatusID == 3)
        //            {
        //                //Add Message into mMessageTrans Table After Approve Order
        //                AddIntomMessageTrans(paraReferenceID, conn);
        //            }
        //            else
        //            {
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        if (ApprovalDetail == 0)
        //        {
        //            long OrdrID = paraReferenceID;
        //            RollBack(OrdrID, conn);
        //        }
        //    }
        //    finally { }

        //    //ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
        //    return Suecc;
        //}


        public void FinalSaveProductLeveltax(string CurrentObjectName, long paraReferenceID, string paraSessionID, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                ObjectParameter _CurrentObjectName = new ObjectParameter("CurrentObjectName", typeof(string));
                _CurrentObjectName.Value = CurrentObjectName;

                ObjectParameter _SessionID = new ObjectParameter("SessionID", typeof(string));
                _SessionID.Value = paraSessionID;

                ObjectParameter _ReferenceID = new ObjectParameter("ReferenceID", typeof(long));
                _ReferenceID.Value = paraReferenceID;

                ObjectParameter[] obj = new ObjectParameter[] { _CurrentObjectName, _SessionID, _ReferenceID };
                db.ExecuteFunction("WMS_SP_CartInsertIntoProductLevelTaxDetail", obj);
                db.SaveChanges();

                List<tAddToCartProductTaxDetail> pt=new List<tAddToCartProductTaxDetail> ();
                pt = (from p in db.tAddToCartProductTaxDetails
                      where p.ReferenceID == paraReferenceID
                      select p).ToList();

                if (pt.Count() == 0)
                {
                    DataSet ds = new DataSet();
                    ds = fillds("select * from tPurchaseOrderDetail where POOrderHeadId = " + paraReferenceID + "", conn);
                    int cnt = ds.Tables[0].Rows.Count;
                    if (cnt > 0)
                    {
                        for (int i = 0; i <= cnt - 1; i++)
                        {
                            long prdID = long.Parse(ds.Tables[0].Rows[i]["SkuId"].ToString());
                            decimal qty = decimal.Parse(ds.Tables[0].Rows[i]["OrderQty"].ToString());
                            decimal taxableamt = decimal.Parse(ds.Tables[0].Rows[i]["Total"].ToString());

                            DataSet dsPrd = new DataSet();
                            dsPrd = fillds("select P.ID,P.ProductTaxId ,T.ID TaxID,T.[Percent] TaxRate from mproduct P left outer join mTaxSetup T on P.ProductTaxId =T.ID where P.ID="+ prdID +"", conn);
                            if (dsPrd.Tables[0].Rows.Count > 0)
                            {
                                long taxID = long.Parse(dsPrd.Tables[0].Rows[0]["TaxID"].ToString());
                                decimal taxRate = decimal.Parse(dsPrd.Tables[0].Rows[0]["TaxRate"].ToString());

                                decimal taxAmt = (taxableamt * taxRate);
                                taxAmt = taxAmt / 100;

                                tAddToCartProductTaxDetail cpt = new tAddToCartProductTaxDetail();

                                cpt.ObjectName = "PurchaseOrderTax";
                                cpt.ReferenceID = paraReferenceID;
                                cpt.ProductDetail_Sequence = prdID;
                                cpt.TaxID = taxID;
                                cpt.TaxRate = taxRate;
                                cpt.TaxAmount = taxAmt;
                                cpt.TaxableAmount = taxableamt;

                                db.tAddToCartProductTaxDetails.AddObject(cpt);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch { }
        }
        public void FinalSaveTaxOnTotal(string CurrentObjectName, long paraReferenceID, string paraSessionID, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                ObjectParameter _CurrentObjectName = new ObjectParameter("CurrentObjectName", typeof(string));
                _CurrentObjectName.Value = CurrentObjectName;

                ObjectParameter _SessionID = new ObjectParameter("SessionID", typeof(string));
                _SessionID.Value = paraSessionID;

                ObjectParameter _ReferenceID = new ObjectParameter("ReferenceID", typeof(long));
                _ReferenceID.Value = paraReferenceID;

                ObjectParameter[] obj = new ObjectParameter[] { _CurrentObjectName, _SessionID, _ReferenceID };
                db.ExecuteFunction("WMS_SP_CartInsertIntoProductLevelTaxDetailTaxOnTotal", obj);
                db.SaveChanges();
            }
            catch { }
        }

        public tPurchaseOrderHead GetPoHeadByPOID(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tPurchaseOrderHead pohead = new tPurchaseOrderHead();
            pohead = db.tPurchaseOrderHeads.Where(p => p.Id == OrderID).FirstOrDefault();
            db.tPurchaseOrderHeads.Detach(pohead);
            return pohead;
        }

        public tTransferHead GetTransferHeadByTRID(long TransferID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tTransferHead trHead = new tTransferHead();
            trHead = db.tTransferHeads.Where(t => t.ID == TransferID).FirstOrDefault();
            db.tTransferHeads.Detach(trHead);
            return trHead;
        }

        public tOrderHead GetSoHeadBySOIDForSalesReturn(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead soh = new tOrderHead();
            soh = db.tOrderHeads.Where(s => s.Id == OrderID).FirstOrDefault();
            db.tOrderHeads.Detach(soh);
            return soh;
        }

        public List<mWarehouseMaster> GetWarehouseNameByUserID(long uid, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseMaster> WarehouseName = new List<mWarehouseMaster>();
            WarehouseName = (from w in ce.mWarehouseMasters
                             join u in ce.mUserWarehouses on w.ID equals u.WarehoueID
                             where u.UserID == uid
                             orderby w.WarehouseName
                             select w).ToList();
            return WarehouseName;
        }

        public List<mWarehouseMaster> GetAllWarehouseList(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseMaster> WarehouseName = new List<mWarehouseMaster>();
            WarehouseName = (from w in ce.mWarehouseMasters
                             orderby w.WarehouseName
                             select w).ToList();
            return WarehouseName;
        }

        public DataSet GetCorrespondance(long RequestID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from WMS_VW_CorrespondanceDetail where OrderHeadId=" + RequestID + " and MailStatus!=0 order by Id Desc ", conn);
            return ds;
        }

        public int GetApprovalInWorkFlow(long CmpID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select WH.ID,WH.WorkflowName ,WD.ObjectID,WD.Sequence from mWorkFlowHead WH left outer join mWorkFlowDetail WD on WH.ID=WD.WorkflowID left outer join mObject o on WD.ObjectID=o.OBJECTID where WH.CompanyID=" + CmpID + " and WH.ID=1 and o.OBJECTID=10 ", conn);
            int IsApproval = ds.Tables[0].Rows.Count;
            return IsApproval;
        }

        public long getCustomerofUser(long UserID,long CompanyID,string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mCustomer ct = new mCustomer();
            ct = (from c in db.mCustomers
                  where c.ParentID == CompanyID
                  select c).FirstOrDefault();
            long customerID = ct.ID;
            return customerID;
        }

        public decimal GetTaxofProduct(long ProdID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select P.ID,P.ProductTaxId ,t.Name,t.[Percent] from mProduct P left outer join mTaxSetup t on P.ProductTaxId = t.ID where P.id="+ ProdID +" ", conn);
            decimal per = decimal.Parse(ds.Tables[0].Rows[0]["Percent"].ToString());
            return per;
        }
        #endregion

        #region GRN

        public DataSet BindGRNGrid(long CompanyID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_GRNGridList where CompanyID=" + CompanyID + " order by ID desc";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet BindGRNGridUserWise(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_GRNGridList where ReceivedBy=" + UserID + " order by ID desc";
            ds = fillds(str, conn);
            return ds;
        }


        public DataSet BindGRNGridofSelectedPO(long POID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_SelectedPOGrid where OID=" + POID + " order by ID";
            ds = fillds(str, conn);
            return ds;
        }

        public List<WMS_SP_GetPartDetails_ForGRN_Result> GetGRNPartDetailsByPOID(string POID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForGRN_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
          //  PartDetail = db.WMS_SP_GetPartDetails_ForGRN(POID, "", "").ToList();
            SaveTempDataToDBGRN(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        public List<WMS_SP_GetPartDetails_ForGRN_Result> GetGRNPartDetailsBySOID(string SOID, string TRID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForGRN_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
       //     PartDetail = db.WMS_SP_GetPartDetails_ForGRN("", SOID, TRID).ToList();
            SaveTempDataToDBGRN(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        protected void SaveTempDataToDBGRN(List<WMS_SP_GetPartDetails_ForGRN_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBGRN(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(paraobjList);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObjectName.ToString();
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();
            /*End*/
        }

        public void ClearTempDataFromDBGRN(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraCurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<WMS_SP_GetPartDetails_ForGRN_Result> GetExistingTempDataBySessionIDObjectNameGRN(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForGRN_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetails_ForGRN_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public string UPdateGRNTempData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForGRN_Result GRN, string[] conn)
        {
            string RemainingQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_GetPartDetails_ForGRN_Result> getRec = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNameGRN(SessionID, UserID, CurrentObjectName, conn);

                WMS_SP_GetPartDetails_ForGRN_Result updateRec = new WMS_SP_GetPartDetails_ForGRN_Result();
                updateRec = getRec.Where(r => r.Sequence == GRN.Sequence).FirstOrDefault();

                updateRec.GRNQty = Convert.ToDecimal(GRN.GRNQty);
                RemainingQty = (updateRec.POQty - updateRec.GRNQty).ToString();
                if (Convert.ToDecimal(RemainingQty) > 0)
                {
                    updateRec.ShortQty = Convert.ToDecimal(RemainingQty);
                    updateRec.ExcessQty = 0;
                }
                else
                {
                    updateRec.ExcessQty = Math.Abs(Convert.ToDecimal(RemainingQty));
                    updateRec.ShortQty = 0;
                }
                SaveTempDataToDBGRN(getRec, SessionID, UserID, CurrentObjectName, conn);
            }
            catch { }
            finally { }
            return RemainingQty;
        }

        //public List<WMS_SP_GetPartDetails_ForGRN_Result> GetPendingGRNPartList(string SessionID, string UserID, string ObjectName, long POID, string[] conn)
        //{
        //    List<WMS_SP_GetPartDetails_ForGRN_Result> CurrentPartList = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
        //    CurrentPartList = GetExistingTempDataBySessionIDObjectNameGRN(SessionID, UserID, ObjectName, conn);
        //    var CrntPrdID=(from c in CurrentPartList


        //}

        public string GetUserNameByID(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select U.FirstName+' '+U.LastName CreatedBy  from  mUserProfileHead U  where U.Id=" + UserID + "";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows[0]["CreatedBy"].ToString();
        }

        public long SavetGRNHead(tGRNHead GRNHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (GRNHead.ID == 0)
            {
                db.tGRNHeads.AddObject(GRNHead);
            }
            else
            {
                db.tGRNHeads.Attach(GRNHead);
                db.ObjectStateManager.ChangeObjectState(GRNHead, EntityState.Modified);
            }
            db.SaveChanges();
            return GRNHead.ID;
        }

        public int FinalSaveGRNDetail(long Poid, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var tGRNDetails = 0;
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetails_ForGRN_Result> fnlSaveLst = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNameGRN(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                fnlSaveLst = fnlSaveLst.Where(p => p.POID == Poid).ToList();
                XElement xmlEle = new XElement("GRN", from rec in fnlSaveLst
                                                      select new XElement("PartList",
                                                          new XElement("GRN_ID", paraReferenceID),
                                                          new XElement("Prod_ID", Convert.ToInt64(rec.Prod_ID)),
                                                          new XElement("GRNQty", Convert.ToDecimal(rec.GRNQty)),
                                                          new XElement("POQty", Convert.ToDecimal(rec.POQty)),
                                                          new XElement("ShortQty", Convert.ToDecimal(rec.ShortQty)),
                                                          new XElement("ExcessQty", Convert.ToDecimal(rec.ExcessQty)),
                                                          new XElement("UOMID", Convert.ToInt64(rec.UOMID))));

                ObjectParameter _GRN_ID = new ObjectParameter("GRN_ID", typeof(long));
                _GRN_ID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _GRN_ID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotGRNDetail", obj);
                db.SaveChanges(); tGRNDetails = 1; Result = 1;

                /*Update RemainingQty in tPurchaseOrderDetail*/
                DataSet dsPONO = new DataSet();
                dsPONO.Reset();
                string str = "select OID,ObjectName from tGRNHead where ID=" + paraReferenceID + "";
                dsPONO = fillds(str, conn);
                long OrderNo = long.Parse(dsPONO.Tables[0].Rows[0]["OID"].ToString());
                string ObjectNM = dsPONO.Tables[0].Rows[0]["ObjectName"].ToString();

                if (ObjectNM == "Transfer")
                {
                    DataSet dsPrdGRGRN = new DataSet();
                    dsPrdGRGRN.Reset();
                    string stPGRN = "select * from tGRNDetail where GRNID=" + paraReferenceID + "";
                    dsPrdGRGRN = fillds(stPGRN, conn);
                    int dsCount = dsPrdGRGRN.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int g = 0; g <= dsCount - 1; g++)
                        {
                            long grnPrdID = long.Parse(dsPrdGRGRN.Tables[0].Rows[g]["ProdID"].ToString());
                            decimal grnQty = decimal.Parse(dsPrdGRGRN.Tables[0].Rows[g]["GRNQty"].ToString());

                            tTransferDetail GetSkuid = new tTransferDetail();
                            GetSkuid = (from ts in db.tTransferDetails
                                        where ts.TransferID == OrderNo && ts.RemainingGRNQty > 0 && ts.SkuId == grnPrdID
                                        select ts).FirstOrDefault();

                            decimal RemainingQty = decimal.Parse(GetSkuid.RemainingGRNQty.ToString());

                            RemainingQty = RemainingQty - grnQty;

                            GetSkuid.RemainingGRNQty = RemainingQty;
                            db.SaveChanges();
                        }
                    }
                    int CntTRQty = (from r in db.tTransferDetails
                                    where r.RemainingGRNQty > 0 && r.TransferID == OrderNo
                                    select r).Count();
                    if (CntTRQty == 0)
                    {
                        tTransferHead TRStatus = new tTransferHead();
                        TRStatus = (from tr in db.tTransferHeads
                                    where tr.ID == OrderNo
                                    select tr).FirstOrDefault();
                        TRStatus.Status = 60;
                        db.SaveChanges();
                    }
                }
                else
                {
                    DataSet dsPrdGRGRN = new DataSet();
                    dsPrdGRGRN.Reset();
                    string stPGRN = "select * from tGRNDetail where GRNID=" + paraReferenceID + "";
                    dsPrdGRGRN = fillds(stPGRN, conn);
                    int dsCount = dsPrdGRGRN.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int g = 0; g <= dsCount - 1; g++)
                        {
                            long grnPrdID = long.Parse(dsPrdGRGRN.Tables[0].Rows[g]["ProdID"].ToString());
                            decimal grnQty = decimal.Parse(dsPrdGRGRN.Tables[0].Rows[g]["GRNQty"].ToString());

                            if (ObjectNM == "PurchaseOrder")
                            {
                                tPurchaseOrderDetail GetSkuID = new tPurchaseOrderDetail();
                                GetSkuID = (from ps in db.tPurchaseOrderDetails
                                            where ps.POOrderHeadId == OrderNo && ps.RemaningQty > 0 && ps.SkuId == grnPrdID       //ps.Sequence == i + 1 
                                            select ps).FirstOrDefault();

                                decimal RemainingQty = decimal.Parse(GetSkuID.RemaningQty.ToString());
                                RemainingQty = RemainingQty - grnQty;
                                GetSkuID.RemaningQty = RemainingQty;
                                db.SaveChanges();

                                /*New Code For ASNDetail Update*/
                                List<tASNHead> AH = new List<tASNHead>();
                                AH = (from a in db.tASNHeads
                                      where a.POId == OrderNo && a.Status == 0
                                      select a).ToList();
                                if (AH.Count > 0)
                                {
                                    tASNHead asnH = new tASNHead();
                                    asnH = (from asn in db.tASNHeads
                                            where asn.POId == OrderNo && asn.Status == 0
                                            select asn).FirstOrDefault();
                                    long asnHeadID = asnH.ID;
                                    tAsnDetail AD = new tAsnDetail();
                                    AD = (from d in db.tAsnDetails
                                          where d.asnHeadId == asnHeadID && d.SkuId == grnPrdID && d.ShippedQty < d.AsnQty
                                          select d).FirstOrDefault();

                                    AD.ShippedQty = grnQty;
                                    db.SaveChanges();

                                    int asnDtcnt = (from ac in db.tAsnDetails
                                                    where ac.asnHeadId == asnHeadID
                                                    select ac).Count();

                                    int asnDtShipCnt = (from acs in db.tAsnDetails
                                                        where acs.asnHeadId == asnHeadID && acs.ShippedQty == acs.AsnQty
                                                        select acs).Count();
                                    if (asnDtcnt == asnDtShipCnt)
                                    {
                                        tASNHead chngStatus = new tASNHead();
                                        chngStatus = (from st in db.tASNHeads
                                                      where st.ID == asnHeadID
                                                      select st).FirstOrDefault();
                                        chngStatus.Status = 1;
                                        db.SaveChanges();
                                    }
                                }
                                /*New Code For ASNDetail Update*/
                            }
                            if (ObjectNM == "SalesReturn")
                            {
                                tOrderDetail GetSRSku = new tOrderDetail();
                                GetSRSku = (from sr in db.tOrderDetails
                                            where sr.OrderHeadId == OrderNo && sr.RemaningQty > 0 && sr.SkuId == grnPrdID
                                            select sr).FirstOrDefault();
                                decimal rtrnRemainingQty = decimal.Parse(GetSRSku.RemaningQty.ToString());
                                rtrnRemainingQty = rtrnRemainingQty - grnQty;
                                GetSRSku.RemaningQty = rtrnRemainingQty;
                                GetSRSku.ReturnQty = grnQty;
                                db.SaveChanges();
                            }

                        }
                    }

                    if (ObjectNM == "PurchaseOrder")
                    {
                        int CntRQty = (from r in db.tPurchaseOrderDetails
                                       where r.RemaningQty > 0 && r.POOrderHeadId == OrderNo
                                       select r).Count();

                        if (CntRQty == 0)
                        {
                            tPurchaseOrderHead POStatus = new tPurchaseOrderHead();
                            POStatus = (from st in db.tPurchaseOrderHeads
                                        where st.Id == OrderNo
                                        select st).FirstOrDefault();

                            POStatus.Status = 31; //Status GRN
                            db.SaveChanges();
                        }
                        else //Status For Partially Completed GRN
                        {
                            tPurchaseOrderHead POStatus = new tPurchaseOrderHead();
                            POStatus = (from st in db.tPurchaseOrderHeads
                                        where st.Id == OrderNo
                                        select st).FirstOrDefault();

                            POStatus.Status = 63; //Status Partially Completed
                            db.SaveChanges();
                        }
                    }
                    if (ObjectNM == "SalesReturn")
                    {
                        int CntRRQty = (from s in db.tOrderDetails
                                        where s.RemaningQty > 0 && s.OrderHeadId == OrderNo
                                        select s).Count();
                        if (CntRRQty == 0)
                        {
                            tOrderHead SOStatus = new tOrderHead();
                            SOStatus = (from oh in db.tOrderHeads
                                        where oh.Id == OrderNo
                                        select oh).FirstOrDefault();

                            SOStatus.Status = 52;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch { Result = 0; }
            finally { }
            return Result;
        }

        public int GetTotalGRNPOWise(long POID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            int TotGrn = (from g in db.tGRNHeads
                          where g.OID == POID
                          select g).Count();
            return TotGrn;
        }

        public WMS_VW_GetGRNDetails GetGRNDetailsByGRNID(long POID, string ObjectNM, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long GRNID = (from gid in db.tGRNHeads
                          where gid.OID == POID && gid.ObjectName == ObjectNM
                          select gid.ID).FirstOrDefault();

            WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
            GrnLst = (from g in db.WMS_VW_GetGRNDetails
                      where g.ID == GRNID
                      select g).FirstOrDefault();
            return GrnLst;
        }

        public WMS_VW_GetGRNDetails GetGRNDetailsByGRNIDGRNMenu(long GRNID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
            GrnLst = (from g in db.WMS_VW_GetGRNDetails
                      where g.ID == GRNID
                      select g).FirstOrDefault();
            return GrnLst;
        }

        public DataSet GetGrnSkuDetailsbyGRNID(long GRNID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select G.ProdID,G.GRNQty,G.POQty,G.ShortQty,G.ExcessQty,P.ProductCode Prod_Code,p.Name Prod_Name,P.[Description] Prod_Description,PD.UOMID,u.Name UOM, ROW_NUMBER() OVER(ORDER BY G.ID ASC) AS Sequence from tGRNDetail G left outer join mProduct P on  G.ProdID=P.ID left outer join tGRNHead GH on G.GRNID=GH.ID left outer join tPurchaseOrderDetail PD on GH.OID=PD.POOrderHeadId and G.ProdID=PD.SkuId left outer join mUOM u on PD.UOMID=u.ID where G.GRNID=" + GRNID + "";
            ds = fillds(str, conn);
            return ds;
        }

        public long GetPOIDFromGRNID(long GRNID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long POID = (from gid in db.tGRNHeads
                         where gid.ID == GRNID
                         select gid.OID).FirstOrDefault().Value;
            return POID;
        }

        public List<WMS_SP_GetPartDetails_ForGRN_Result> RemovePartFromGRN_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetails_ForGRN_Result> existingList = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameGRN(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Get Filter List [Filter By paraSequence]*/
            List<WMS_SP_GetPartDetails_ForGRN_Result> filterList = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/
            /*Save result to TempData*/
            SaveTempDataToDBGRN(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<WMS_SP_GetPartDetails_ForGRN_Result> NewList = new List<WMS_SP_GetPartDetails_ForGRN_Result>();
            NewList = GetExistingTempDataBySessionIDObjectNameGRN(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            WMS_SP_GetPartDetails_ForGRN_Result UpdRec = new WMS_SP_GetPartDetails_ForGRN_Result();

            if (cnt > 0)
            {
                for (int i = paraSequence; i <= cnt; i++)
                {
                    UpdRec = NewList.Where(u => u.Sequence == i + 1).FirstOrDefault();
                    UpdRec.Sequence = i;
                    SaveTempDataToDBGRN(NewList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
                }
            }
            /*End*/
            if (cnt > 0)
            { return NewList; }
            else { return filterList; }
        }

        public DataSet CheckSelectedPOJobCardNo(long PONumber, string ObjectName, string[] conn)
        {
            DataSet dsJCN = new DataSet(); dsJCN.Reset();
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long TaskID = (from t in db.tJobCardDetails
                           where t.OrderObjectName == ObjectName && t.OID == PONumber
                           select t.TaskID).FirstOrDefault().Value;
            if (TaskID > 0)
            {
                string OrderNo = "";
                DataSet ds = new DataSet();
                ds.Reset();
                string str = "select OID from tJobCardDetail where TaskID=" + TaskID + "";
                ds = fillds(str, conn);
                int Cnt = ds.Tables[0].Rows.Count;
                if (Cnt > 0)
                {
                    for (int i = 0; i <= Cnt - 1; i++)
                    {
                        if (i == 0)
                        {
                            OrderNo = ds.Tables[0].Rows[0]["OID"].ToString();
                        }
                        else
                        {
                            OrderNo = OrderNo + "," + ds.Tables[0].Rows[i]["OID"].ToString();
                        }
                    }
                }

                tTaskDetail tsk = new tTaskDetail();
                tsk = (from n in db.tTaskDetails
                       where n.TaskID == TaskID
                       select n).FirstOrDefault();

                string JobCardName = tsk.JobCardName;
                string CreationDate = tsk.CreationDate.ToShortDateString();
                long CreatedBy = long.Parse(tsk.CreatedBy.ToString());

                DataSet dsGetUserName = new DataSet();
                dsGetUserName.Reset();
                string Susr = "select FirstName+' '+LastName CreatedByUser from mUserProfileHead where id=" + CreatedBy + "";
                dsGetUserName = fillds(Susr, conn);
                string CreatedByUser = dsGetUserName.Tables[0].Rows[0]["CreatedByUser"].ToString();

                string Warehouse = "";
                if (ObjectName == "PurchaseOrder")
                {
                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(Warehouse) from tPurchaseOrderHead where ID in(" + OrderNo + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["Warehouse"].ToString();
                }
                else if (ObjectName == "GRN")
                {
                    DataSet dsG = new DataSet();
                    string stG = "select OID from tGRNHead where ID In(" + OrderNo + ")";
                    dsG = fillds(stG, conn);
                    long OID = long.Parse(dsG.Tables[0].Rows[0]["OID"].ToString());

                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(Warehouse) from tPurchaseOrderHead where ID in(" + OID + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["Warehouse"].ToString();
                }
                else if (ObjectName == "QC")
                {
                    DataSet dsQ = new DataSet();
                    string strQ = "select Warehouse from tPurchaseOrderHead where Id=(select OID from tGRNHead where ID=(select OID from tQualityControlHead where ID=" + PONumber + " and ObjectName='PurchaseOrder'))";
                    dsQ = fillds(strQ, conn);
                    Warehouse = dsQ.Tables[0].Rows[0]["Warehouse"].ToString();
                }
                else if (ObjectName == "PickUp")
                {
                    DataSet dsPKUP = new DataSet();
                    string strPKUP = "select OID from tPickUpHead where ID in (" + OrderNo + ")";
                    dsPKUP = fillds(strPKUP, conn);
                    long SOID = long.Parse(dsPKUP.Tables[0].Rows[0]["OID"].ToString());

                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(StoreId) from tOrderHead where ID in(" + SOID + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["StoreId"].ToString();
                }
                else if (ObjectName == "SalesReturn")
                {
                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(StoreId) from tOrderHead where ID in(" + PONumber + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["StoreId"].ToString();
                }
                else if (ObjectName == "Transfer")
                {
                    DataSet dsTR = new DataSet(); dsTR.Reset();
                    string sTR = "select distinct(FromPosition) from tTransferHead where ID in(" + PONumber + ")";
                    dsTR = fillds(sTR, conn);
                    Warehouse = dsTR.Tables[0].Rows[0]["FromPosition"].ToString();
                }
                string s = "select '" + OrderNo + "' as OrderNo, '" + JobCardName + "' as JobCardName, '" + CreationDate + "' as CreationDate, '" + CreatedByUser + "' as CreatedByUser, '" + Warehouse + "' as Warehouse";
                dsJCN = fillds(s, conn);

                return dsJCN;
            }
            else
            {
                string s = "select '' as OrderNo, '' as JobCardName, '' as CreationDate, '' as CreatedByUser, '' as Warehouse";
                dsJCN = fillds(s, conn);
                return dsJCN;
            }
        }

        public int CheckJobCard(long PONumber, string ObjectName, string[] conn)
        {
            DataSet dsW = new DataSet(); dsW.Reset();
            string sW = "select   TaskID  from  tJobCardDetail where OrderObjectName ='" + ObjectName + "' and OID =" + PONumber + "";
            dsW = fillds(sW, conn);
            return dsW.Tables[0].Rows.Count;
        }

        public int CheckSelectedGRNStatusIsSameOrNot(string SelectedGRN, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Count(*)Cnt,Status from tGRNHead where Id in(" + SelectedGRN + ") Group By Status";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows.Count;
        }
        #endregion

        #region QC
        public DataSet BindQCGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select QH.ID,GH.ID GRNID,P.Id PONO,case when T.JobCardName is null then 'Not Created' else T.JobCardName end as JobCardName ,QH.QCDate,QH.QCBy,U.FirstName+' '+U.LastName QCUser ,QH.Status,S.Status StatusName,P.VendorID,V.Name VendorName, Case When QH.Status in(32,33,35) then 'green' end as ImgQC, Case When QH.Status=32 then 'red' when QH.Status in(33,35) then 'green' when QH.Status=33 then 'gray' end as ImgLP, Case When QH.Status in(32) then 'gray' when QH.Status=33 then 'red' when QH.Status=35 then 'green' end as  ImgPutIn from tQualityControlHead QH  left outer join tGRNHead GH on QH.OID=GH.ID left outer join tPurchaseOrderHead P on GH.OID = P.Id  Left outer join mVendor V on P.VendorID=V.ID  left outer join mUserProfileHead U on QH.QCBy=U.ID  left outer join mStatus S on QH.Status=S.ID left outer join tJobCardDetail J on QH.Id=J.OID and J.OrderObjectName='QC' left outer join tTaskDetail T on J.TaskID=T.TaskID where QH.ObjectName='PurchaseOrder'  order by QH.ID desc ";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet BindQCGridofSO(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select QH.ID,PH.ID GRNID,OH.Id PONO,case when T.JobCardName is null then 'Not Created' else T.JobCardName end as JobCardName ,QH.QCDate,QH.QCBy,U.FirstName+' '+U.LastName QCUser ,QH.Status,S.Status StatusName,OH.ClientID,Cl.Name VendorName, Case When QH.Status in(32,33,40) then 'green' end as ImgQC, Case When QH.Status=32 then 'red' when QH.Status in(33,40) then 'green' when QH.Status=33 then 'gray' end as ImgDis  from tQualityControlHead QH  left outer join tPickUpHead PH on QH.OID=Ph.ID left outer join tOrderHead OH on PH.OID=OH.Id left outer join mClient Cl on OH.ClientID=Cl.ID left outer join mUserProfileHead U on QH.QCBy=U.ID   left outer join mStatus S on QH.Status=S.ID left outer join tJobCardDetail J on QH.Id=J.OID and J.OrderObjectName='QC' left outer join tTaskDetail T on J.TaskID=T.TaskID  where QH.ObjectName='SalesOrder' order by QH.ID desc ";
            ds = fillds(str, conn);
            return ds;
        }
        public DataSet BindQCGridofSelectedGRN(long GRNID, string[] conn)
        {
            DataSet dsTaskID = new DataSet();
            dsTaskID.Reset();
            string strTaskID = "select   TaskID  from  tJobCardDetail where OrderObjectName ='GRN' and OID =" + GRNID + "";
            dsTaskID = fillds(strTaskID, conn);

            DataSet ds = new DataSet();
            ds.Reset();
            string str = "";
            int cnt = dsTaskID.Tables[0].Rows.Count;
            if (cnt > 0)
            {
                str = "select QH.ID,GH.ID GRNID,P.Id PONO,case when T.JobCardName is null then 'Not Created' else T.JobCardName end as JobCardName ,QH.QCDate,QH.QCBy,U.FirstName+' '+U.LastName QCUser ,QH.Status,S.Status StatusName,P.VendorID,V.Name VendorName, Case When QH.Status in(32,33,35) then 'green' end as ImgQC, Case When QH.Status=32 then 'red' when QH.Status in(33,35) then 'green' when QH.Status=33 then 'gray' end as ImgLP, Case When QH.Status in(32) then 'gray' when QH.Status=33 then 'red' when QH.Status=35 then 'green' end as  ImgPutIn from tQualityControlHead QH  left outer join tGRNHead GH on QH.OID=GH.ID left outer join tPurchaseOrderHead P on GH.OID = P.Id  Left outer join mVendor V on P.VendorID=V.ID  left outer join mUserProfileHead U on QH.QCBy=U.ID  left outer join mStatus S on QH.Status=S.ID left outer join tJobCardDetail J on QH.Id=J.OID and J.OrderObjectName='QC' left outer join tTaskDetail T on J.TaskID=T.TaskID  where GH.ID in (select OID from tJobCardDetail where TaskID=(select   TaskID  from  tJobCardDetail where OrderObjectName ='GRN' and OID =" + GRNID + ")) order by QH.ID desc";
            }
            else
            {
                str = "select * from WMS_VW_QCGridSelectedGRN where GRNID in(" + GRNID + ") order by ID";
            }
            ds = fillds(str, conn);
            return ds;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByPOID(string POID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            PartDetail = db.WMS_SP_GetPartDetails_ForQC("0", POID, "0", "0", "0").ToList();
            SaveTempDataToDBQC(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByGRNID(string GRNID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            PartDetail = db.WMS_SP_GetPartDetails_ForQC(GRNID, "0", "0", "0", "0").ToList();
            SaveTempDataToDBQC(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByQCID(string GRNID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            PartDetail = db.WMS_SP_GetPartDetails_ForQC("0", "0", GRNID, "0", "0").ToList();
            SaveTempDataToDBQC(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByPickUPID(string PKUPID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            PartDetail = db.WMS_SP_GetPartDetails_ForQC("0", "0", "0", PKUPID, "0").ToList();
            SaveTempDataToDBQC(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetQCPartDetailsByTransferID(string trID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> PartDetail = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            PartDetail = db.WMS_SP_GetPartDetails_ForQC("0", "0", "0", "0", trID).ToList();
            SaveTempDataToDBQC(PartDetail, SessionID, UserID, CurrentObject, conn);
            return PartDetail;
        }

        protected void SaveTempDataToDBQC(List<WMS_SP_GetPartDetails_ForQC_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBQC(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(paraobjList);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObjectName.ToString();
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();
            /*End*/
        }

        public void ClearTempDataFromDBQC(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraCurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> GetExistingTempDataBySessionIDObjectNameQC(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetails_ForQC_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetails_ForQC_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public string UPdateQCTempData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForQC_Result QC, string[] conn)
        {
            string RemainingQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_GetPartDetails_ForQC_Result> getRec = new List<WMS_SP_GetPartDetails_ForQC_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNameQC(SessionID, UserID, CurrentObjectName, conn);

                WMS_SP_GetPartDetails_ForQC_Result updateRec = new WMS_SP_GetPartDetails_ForQC_Result();
                updateRec = getRec.Where(r => r.Sequence == QC.Sequence).FirstOrDefault();
                updateRec.SelectedQty = Convert.ToDecimal(QC.SelectedQty);
                updateRec.RejectedQty = Convert.ToDecimal(QC.RejectedQty);
                updateRec.Reason = QC.Reason.ToString();
                SaveTempDataToDBQC(getRec, SessionID, UserID, CurrentObjectName, conn);
            }
            catch { }
            finally { }
            return RemainingQty;
        }

        public string UpdateQCTempDataReason(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetails_ForQC_Result QC, string[] conn)
        {
            string RemainingQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_GetPartDetails_ForQC_Result> getRec = new List<WMS_SP_GetPartDetails_ForQC_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNameQC(SessionID, UserID, CurrentObjectName, conn);

                WMS_SP_GetPartDetails_ForQC_Result updateRec = new WMS_SP_GetPartDetails_ForQC_Result();
                updateRec = getRec.Where(r => r.Sequence == QC.Sequence).FirstOrDefault();
                updateRec.Reason = QC.Reason.ToString();
                SaveTempDataToDBQC(getRec, SessionID, UserID, CurrentObjectName, conn);
            }
            catch { }
            finally { }
            return RemainingQty;
        }

        public long SavetQualityControlHead(tQualityControlHead QCHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (QCHead.ID == 0)
            {
                db.tQualityControlHeads.AddObject(QCHead);
            }
            else
            {
                db.tQualityControlHeads.Attach(QCHead);
                db.ObjectStateManager.ChangeObjectState(QCHead, EntityState.Modified);
            }
            db.SaveChanges();
            return QCHead.ID;
        }

        public int FinalSaveQCDetail(long GrnId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string QCObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var tQCDetails = 0;
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetails_ForQC_Result> fnlSaveLst = new List<WMS_SP_GetPartDetails_ForQC_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNameQC(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                fnlSaveLst = fnlSaveLst.Where(g => g.GRNID == GrnId).ToList();

                XElement xmlEle = new XElement("QC", from rec in fnlSaveLst
                                                     select new XElement("PartList",
                                                         new XElement("QCID", paraReferenceID),
                                                         new XElement("ProdID", Convert.ToInt64(rec.Prod_ID)),
                                                         new XElement("OQty", Convert.ToDecimal(rec.POQty)),
                                                          new XElement("GRNQty", Convert.ToDecimal(rec.GRNQty)),
                                                         new XElement("QCQty", Convert.ToDecimal(rec.SelectedQty)),
                                                         new XElement("RejectedQty", Convert.ToDecimal(rec.RejectedQty)),
                                                         new XElement("Reason", Convert.ToString(rec.Reason)),
                                                         new XElement("UOMID", Convert.ToInt64(rec.UOMID))));

                ObjectParameter _QCID = new ObjectParameter("QCID", typeof(long));
                _QCID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _QCID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotQualityControlDetail", obj);
                db.SaveChanges(); tQCDetails = 1; Result = 1;

                /*Update GRN Status & PO Status */
                if (QCObject == "PurchaseOrder")
                {
                    tGRNHead grn = new tGRNHead();
                    grn = (from g in db.tGRNHeads
                           where g.ID == GrnId
                           select g).FirstOrDefault();

                    grn.Status = 32;   //Status QC    Change GRN Status To QC
                    db.SaveChanges();

                    tPurchaseOrderHead po = new tPurchaseOrderHead();
                    po = (from p in db.tPurchaseOrderHeads
                          where p.Id == grn.OID
                          select p).FirstOrDefault();
                    long GRNPO = po.Id;

                    if (po.Status == 63)
                    {
                    }
                    else
                    {
                        List<tGRNHead> lstGRN = new List<tGRNHead>();
                        lstGRN = (from lg in db.tGRNHeads
                                  where lg.OID == GRNPO
                                  select lg).ToList();

                        int CntAllGRN = lstGRN.Count();                 //Count of All GRN of One PO 
                        lstGRN = lstGRN.Where(n => n.Status == 32).ToList();
                        int QCGRN = lstGRN.Count();                     //Count of GRN with Status 32(QC)
                        if (CntAllGRN == QCGRN)
                        {
                            po.Status = 32;//Change status of PO to QC
                            db.SaveChanges();
                        }
                    }
                }
                if (QCObject == "SalesOrder")
                {
                    tPickUpHead pkup = new tPickUpHead();
                    pkup = (from p in db.tPickUpHeads
                            where p.ID == GrnId
                            select p).FirstOrDefault();
                    pkup.Status = 32;
                    db.SaveChanges();

                    tOrderHead so = new tOrderHead();
                    so = (from s in db.tOrderHeads
                          where s.Id == pkup.OID
                          select s).FirstOrDefault();
                    long pkupSO = so.Id;

                    List<tPickUpHead> lstpkup = new List<tPickUpHead>();
                    lstpkup = (from lp in db.tPickUpHeads
                               where lp.OID == pkupSO
                               select lp).ToList();

                    int CntAllPkUp = lstpkup.Count();
                    lstpkup = lstpkup.Where(n => n.Status == 32).ToList();
                    int QCPkUp = lstpkup.Count();
                    if (CntAllPkUp == QCPkUp)
                    {
                        so.Status = 32;
                        db.SaveChanges();
                    }
                }
                if (QCObject == "SalesReturn")
                {
                    tGRNHead grn = new tGRNHead();
                    grn = (from g in db.tGRNHeads
                           where g.ID == GrnId
                           select g).FirstOrDefault();

                    grn.Status = 55;   //Status QC    Change GRN Status To QC
                    db.SaveChanges();

                    tOrderHead OH = new tOrderHead();
                    OH = (from hd in db.tOrderHeads
                          where hd.Id == grn.OID
                          select hd).FirstOrDefault();
                    long GRNSO = OH.Id;
                    List<tGRNHead> lstGRN = new List<tGRNHead>();
                    lstGRN = (from lg in db.tGRNHeads
                              where lg.OID == GRNSO
                              select lg).ToList();
                    int CntAllSOGRN = lstGRN.Count();                 //Count of All GRN of One PO 
                    lstGRN = lstGRN.Where(n => n.Status == 55).ToList();
                    int QCGRN = lstGRN.Count();                     //Count of GRN with Status 32(QC)
                    if (CntAllSOGRN == QCGRN)
                    {
                        OH.Status = 55;//Change status of PO to QC
                        db.SaveChanges();
                    }
                }
                if (QCObject == "Transfer")
                {
                    tTransferHead th = new tTransferHead();
                    th = (from t in db.tTransferHeads
                          where t.ID == GrnId
                          select t).FirstOrDefault();
                    if (th.Status == 57)
                    {
                        th.Status = 58;
                    }
                    else
                    {
                        th.Status = 61;
                    }
                    db.SaveChanges();
                }

            }
            catch { Result = 0; }
            finally { }
            return Result;
        }

        public List<WMS_SP_GetPartDetails_ForQC_Result> RemovePartFromQC_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetails_ForQC_Result> existingList = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameQC(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Get Filter List [Filter By paraSequence]*/
            List<WMS_SP_GetPartDetails_ForQC_Result> filterList = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/
            /*Save result to TempData*/
            SaveTempDataToDBQC(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<WMS_SP_GetPartDetails_ForQC_Result> NewList = new List<WMS_SP_GetPartDetails_ForQC_Result>();
            NewList = GetExistingTempDataBySessionIDObjectNameQC(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            WMS_SP_GetPartDetails_ForQC_Result UpdRec = new WMS_SP_GetPartDetails_ForQC_Result();

            if (cnt > 0)
            {
                for (int i = paraSequence; i <= cnt; i++)
                {
                    UpdRec = NewList.Where(u => u.Sequence == i + 1).FirstOrDefault();
                    UpdRec.Sequence = i;
                    SaveTempDataToDBQC(NewList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
                }
            }
            /*End*/
            if (cnt > 0)
            { return NewList; }
            else { return filterList; }
        }

        public WMS_VW_GetQCDetails GetQCDetailsByGRNID(long GRNID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            WMS_VW_GetQCDetails LstGRN = new WMS_VW_GetQCDetails();
            LstGRN = (from g in db.WMS_VW_GetQCDetails
                      where g.OID == GRNID
                      select g).FirstOrDefault();
            return LstGRN;
        }

        public WMS_VW_GetQCDetails GetQCDetailsByQCID(long QCID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            WMS_VW_GetQCDetails LstGRN = new WMS_VW_GetQCDetails();
            LstGRN = (from g in db.WMS_VW_GetQCDetails
                      where g.ID == QCID
                      select g).FirstOrDefault();
            return LstGRN;
        }

        public int CheckSelectedQCStatusIsSameOrNot(string SelectedQC, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Count(*)Cnt,Status from tQualityControlHead where Id in(" + SelectedQC + ") Group By Status";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows.Count;
        }
        #endregion

        #region LP

        public DataSet BindLPGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet GetProductDetails(string SelectedQC, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from  WMS_FN_GetLabelPrinting('"+ SelectedQC +"')";
            ds = fillds(str, conn);
            return ds;
        }

        public void SetSelectedRecordToLabelPrintingStatus(string SelectedQC, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long qcid = long.Parse(SelectedQC.ToString());
            tQualityControlHead QH = new tQualityControlHead();
            QH = (from q in db.tQualityControlHeads
                  where q.ID == qcid
                  select q).FirstOrDefault();
            string qcObject = QH.ObjectName;

            DataSet ds = new DataSet();
            ds.Reset();
            string str = "";
            if (qcObject == "PurchaseOrder")
            {
                str = "update tQualityControlHead set Status=33 where ID in(" + SelectedQC + ")";
            }
            else if (qcObject == "SalesReturn")
            {
                str = "update tQualityControlHead set Status=53 where ID in(" + SelectedQC + ")";
            }
            ds = fillds(str, conn);

            DataSet dsGRN = new DataSet();
            dsGRN.Reset();
            string strGRN = "";
            if (qcObject == "PurchaseOrder")
            {
                strGRN = "update tGRNHead set Status =33 where ID in(select OID from tQualityControlHead where Status =33)";
            }
            else if (qcObject == "SalesReturn")
            {
                strGRN = "update tGRNHead set Status =53 where ID in(select OID from tQualityControlHead where Status =53)";
            }
            dsGRN = fillds(strGRN, conn);

            DataSet dsQCGRN = new DataSet();
            dsQCGRN.Reset();
            string QCGRN = "";
            if (qcObject == "PurchaseOrder")
            {
                QCGRN = "select OID from tGRNHead where Status=33";
            }
            else if (qcObject == "SalesReturn")
            {
                QCGRN = "select OID from tGRNHead where Status=53";
            }
            dsQCGRN = fillds(QCGRN, conn);
            int Cnt = dsQCGRN.Tables[0].Rows.Count;
            if (Cnt > 0)
            {
                for (int i = 0; i <= Cnt - 1; i++)
                {
                    long oid = long.Parse(dsQCGRN.Tables[0].Rows[i]["OID"].ToString());
                    int CntTotalGRN = (from g in db.tGRNHeads
                                       where g.OID == oid
                                       select g).Count();
                    long gStatus = 0;
                    if (qcObject == "PurchaseOrder")
                    {
                        gStatus = 33;
                    }
                    else if (qcObject == "SalesReturn")
                    {
                        gStatus = 53;
                    }

                    int CntGRNQC = (from gq in db.tGRNHeads
                                    where gq.OID == oid && gq.Status == gStatus
                                    select gq).Count();

                    if (CntTotalGRN == CntGRNQC)
                    {
                        if (qcObject == "PurchaseOrder")
                        {
                            tPurchaseOrderHead PH = new tPurchaseOrderHead();
                            PH = (from p in db.tPurchaseOrderHeads
                                  where p.Id == oid
                                  select p).FirstOrDefault();
                            if (PH.Status == 63) { }
                            else
                            {
                                PH.Status = gStatus;
                                db.SaveChanges();
                            }
                        }
                        else if (qcObject == "SalesReturn")
                        {
                            tOrderHead OH = new tOrderHead();
                            OH = (from ordr in db.tOrderHeads
                                  where OH.Id == oid
                                  select ordr).FirstOrDefault();
                            OH.Status = gStatus;
                            db.SaveChanges();
                        }
                    }
                }
            }

        }
        #endregion

        #region PutIn
        public DataSet BindPIGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select P.ID,PH.POOrderNo,P.OID,P.PutInDate,P.PutInBy,U.FirstName+' '+U.LastName PutInUser,P.Status,S.Status StatusName,V.Name VendorName, Case When P.Status=33 then 'red' when P.Status=35 then 'green' end as ImgPutIn from tPutInHead P left outer join tPurchaseOrderHead PH on P.OID =PH.Id Left outer join mVendor V on PH.VendorID=V.ID left outer join mUserProfileHead U on P.PutInBy=U.ID left outer join mStatus S on P.Status=S.ID ";
            ds = fillds(str, conn);
            return ds;
        }

        public List<WMS_SP_PutInList_Result> GetPutInList(string QCID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_PutInList_Result> PutInLst = new List<WMS_SP_PutInList_Result>();
            PutInLst = db.WMS_SP_PutInList(QCID).ToList();
            SaveTempDataToDBPutIn(PutInLst, SessionID, UserID, CurrentObject, conn);
            return PutInLst;

        }

        public DataSet GetSavedPutInListbyQCID(long qcID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string strGetW = "select PH.ID,PH.OID ,PD.ProdID, MP.ProductCode,PD.PutInQty LocQty,PD.LocationID,L.Code,L.Code LocationBarCode, L.Capacity,L.SortCode,L.AvailableBalance,PD.UOMID,PD.BatchNo,PD.Packing Pack, MP.OMSSKUCode,Row_Number() over(order by PH.OID) Sequence from tPutInHead PH  left outer join tPutInDetail PD on PH.ID=PD.PutInID left outer join mProduct MP on PD.ProdID=MP.ID left outer join mLocation L on PD.LocationID=L.ID where PH.OID=" + qcID + "";
            ds = fillds(strGetW, conn);
            return ds;
        }

        public DataSet GetSavedPutInListbyPutInID(string PutInNo, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string strGetW = "select PH.ID,PH.OID ,PD.ProdID, MP.ProductCode,PD.PutInQty LocQty,PD.LocationID,L.Code,L.Code LocationBarCode, L.Capacity,L.SortCode,L.AvailableBalance,PD.UOMID,PD.BatchNo,PD.Packing Pack, MP.OMSSKUCode,Row_Number() over(order by PH.OID) Sequence from tPutInHead PH  left outer join tPutInDetail PD on PH.ID=PD.PutInID left outer join mProduct MP on PD.ProdID=MP.ID left outer join mLocation L on PD.LocationID=L.ID where PH.ID=" + PutInNo + "";
            ds = fillds(strGetW, conn);
            return ds;
        }

        public List<WMS_SP_PutInList_Result> GetPutInListByTRID(long trID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tGRNHead GH = new tGRNHead();
            GH = (from g in db.tGRNHeads
                  where g.OID == trID && g.ObjectName == "Transfer"
                  select g).FirstOrDefault();
            long grnid = long.Parse(GH.ID.ToString());

            tQualityControlHead QH = new tQualityControlHead();
            QH = (from q in db.tQualityControlHeads
                  where q.OID == grnid
                  select q).FirstOrDefault();

            string qcId = QH.ID.ToString();
            List<WMS_SP_PutInList_Result> PutInLst = new List<WMS_SP_PutInList_Result>();
            PutInLst = db.WMS_SP_PutInList(qcId).ToList();
            SaveTempDataToDBPutIn(PutInLst, SessionID, UserID, CurrentObject, conn);
            return PutInLst;
        }

        protected void SaveTempDataToDBPutIn(List<WMS_SP_PutInList_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBPutIn(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(paraobjList);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObjectName.ToString();
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();
            /*End*/
        }
        public void ClearTempDataFromDBPutIn(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraCurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }
        public List<WMS_SP_PutInList_Result> GetExistingTempDataBySessionIDObjectNamePI(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_PutInList_Result> objtAddToCartProductDetailList = new List<WMS_SP_PutInList_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_PutInList_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public List<WMS_VW_GetLocationDetails> GetLocationForPutIn(int pageIndex, string filter, long qcId, string[] conn)
        {
            DataSet dsGetW = new DataSet();
            dsGetW.Reset();
            string strGetW = "select Warehouse from tPurchaseOrderHead where Id=(select OID from tGRNHead where ID=(select OID from tQualityControlHead where ID=" + qcId + "))";
            dsGetW = fillds(strGetW, conn);
            long WID = long.Parse(dsGetW.Tables[0].Rows[0]["Warehouse"].ToString());

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_VW_GetLocationDetails> result = new List<WMS_VW_GetLocationDetails>();
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.WMS_VW_GetLocationDetails
                          where lst.WarehouseID == WID
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<WMS_VW_GetLocationDetails> filterList = new List<WMS_VW_GetLocationDetails>();

                result = (from lst in db.WMS_VW_GetLocationDetails
                          where (lst.Code.Contains(filter))
                          && lst.WarehouseID == WID
                          select lst).Take(50 * pageIndex).ToList();    
            }
            result = result.Where(a => a.Active == "Yes").ToList();
            return result;
        }

        public List<WMS_VW_GetLocationDetails> GetLocationForPickUP(int pageIndex, string filter, long soId, string[] conn)
        {
            DataSet dsGetW = new DataSet();
            dsGetW.Reset();
            string strGetW = "select StoreId from torderhead where id in("+ soId +")";
            dsGetW = fillds(strGetW, conn);
            long WID = long.Parse(dsGetW.Tables[0].Rows[0]["StoreId"].ToString());

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_VW_GetLocationDetails> result = new List<WMS_VW_GetLocationDetails>();
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.WMS_VW_GetLocationDetails
                          where lst.WarehouseID == WID
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<WMS_VW_GetLocationDetails> filterList = new List<WMS_VW_GetLocationDetails>();

                result = (from lst in db.WMS_VW_GetLocationDetails
                          where (lst.Code.Contains(filter))
                          && lst.WarehouseID == WID
                          select lst).Take(50 * pageIndex).ToList();
            }
            result = result.Where(a => a.Active == "Yes").ToList();
            return result;
        }

        public string UpdatePutInLstQtyofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn)
        {
            string ActualQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_PutInList_Result> getRec = new List<WMS_SP_PutInList_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNamePI(SessionID, UserID, paraObjectName, conn);

                WMS_SP_PutInList_Result updRec = new WMS_SP_PutInList_Result();
                updRec = getRec.Where(r => r.Sequence == putIn.Sequence).FirstOrDefault();
                updRec.LocQty = Convert.ToDecimal(putIn.LocQty);

                SaveTempDataToDBPutIn(getRec, SessionID, UserID, paraObjectName, conn);
            }
            catch { }
            finally { }
            return ActualQty;
        }

        public string UpdatePutInLstLocofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn)
        {
            string ActualQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_PutInList_Result> getRec = new List<WMS_SP_PutInList_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNamePI(SessionID, UserID, paraObjectName, conn);

                WMS_SP_PutInList_Result updRec = new WMS_SP_PutInList_Result();
                updRec = getRec.Where(r => r.Sequence == putIn.Sequence).FirstOrDefault();
                updRec.LocationID = Convert.ToInt64(putIn.LocationID);
                updRec.Code = putIn.Code.ToString();
                updRec.SortCode = Convert.ToInt64(putIn.SortCode);
                updRec.Capacity = Convert.ToDecimal(putIn.Capacity);
                updRec.AvailableBalance = Convert.ToDecimal(putIn.AvailableBalance);

                SaveTempDataToDBPutIn(getRec, SessionID, UserID, paraObjectName, conn);
            }
            catch { }
            finally { }
            return ActualQty;
        }

        public string UpdatePutInLstPackofSelectedRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PutInList_Result putIn, string[] conn)
        {
            string ActualQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_PutInList_Result> getRec = new List<WMS_SP_PutInList_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNamePI(SessionID, UserID, paraObjectName, conn);

                WMS_SP_PutInList_Result updRec = new WMS_SP_PutInList_Result();
                updRec = getRec.Where(r => r.Sequence == putIn.Sequence).FirstOrDefault();
                updRec.Pack = putIn.Pack.ToString();

                SaveTempDataToDBPutIn(getRec, SessionID, UserID, paraObjectName, conn);
            }
            catch { }
            finally { }
            return ActualQty;
        }
       
        public long SavetPutInHead(tPutInHead PIHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (PIHead.ID == 0)
            {
                db.tPutInHeads.AddObject(PIHead);
            }
            else
            {
                db.tPutInHeads.Attach(PIHead);
                db.ObjectStateManager.ChangeObjectState(PIHead, EntityState.Modified);
            }
            db.SaveChanges();
            return PIHead.ID;
        }

        public int FinalSavePutInDetail(long QCId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string PutInObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var tQCDetails = 0;
            int Result = 0; long TransferQCID = 0;
            try
            {
                List<WMS_SP_PutInList_Result> fnlSaveLst = new List<WMS_SP_PutInList_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNamePI(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                if (PutInObject == "Transfer")
                {
                    tGRNHead GH = new tGRNHead();
                    GH = (from g in db.tGRNHeads
                          where g.OID == QCId && g.ObjectName == PutInObject
                          select g).FirstOrDefault();
                    long grnTrID = long.Parse(GH.ID.ToString());

                    tQualityControlHead QH = new tQualityControlHead();
                    QH = (from q in db.tQualityControlHeads
                          where q.OID == grnTrID && q.ObjectName == PutInObject
                          select q).FirstOrDefault();
                    TransferQCID = long.Parse(QH.ID.ToString());

                    fnlSaveLst = fnlSaveLst.Where(q => q.QCID == TransferQCID).ToList();
                }
                else
                {
                    fnlSaveLst = fnlSaveLst.Where(q => q.QCID == QCId).ToList();
                }

                XElement xmlEle = new XElement("PutIn", from rec in fnlSaveLst
                                                        select new XElement("PartList",
                                                            new XElement("PutInID", paraReferenceID),
                                                            new XElement("ProdID", Convert.ToInt64(rec.ProdID)),
                                                            new XElement("PutInQty", Convert.ToDecimal(rec.LocQty)),
                                                            new XElement("LocationID", Convert.ToInt64(rec.LocationID)),
                                                            new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                            new XElement("Packing",rec.Pack.ToString()),
                                                            new XElement("BatchNo",rec.BatchNo.ToString())
                                                            ));

                ObjectParameter _PutInID = new ObjectParameter("PutInID", typeof(long));
                _PutInID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _PutInID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotPutInDetail", obj);
                db.SaveChanges(); tQCDetails = 1; Result = 1;

                /*Update QC, GRN & PO Status */
                string qcObject = PutInObject;
                long GrnId = 0;
                long poID = 0;
                if (qcObject == "PurchaseOrder")
                {
                    tQualityControlHead qc = new tQualityControlHead();
                    qc = (from q in db.tQualityControlHeads
                          where q.ID == QCId
                          select q).FirstOrDefault();
                    qc.Status = 35;
                    db.SaveChanges();
                    GrnId = long.Parse(qc.OID.ToString());

                    tGRNHead grn = new tGRNHead();
                    grn = (from g in db.tGRNHeads
                           where g.ID == GrnId
                           select g).FirstOrDefault();
                    grn.Status = 35;
                    db.SaveChanges();

                    tPurchaseOrderHead po = new tPurchaseOrderHead();
                    po = (from p in db.tPurchaseOrderHeads
                          where p.Id == grn.OID
                          select p).FirstOrDefault();
                    long GRNPO = po.Id; poID = po.Id;

                    List<tGRNHead> lstGRN = new List<tGRNHead>();
                    lstGRN = (from lg in db.tGRNHeads
                              where lg.OID == GRNPO
                              select lg).ToList();

                    int CntAllGRN = lstGRN.Count();                 //Count of All GRN of One PO 
                    lstGRN = lstGRN.Where(n => n.Status == 35).ToList();
                    int QCGRN = lstGRN.Count();                     //Count of GRN with Status 35(PutIn)
                    if (CntAllGRN == QCGRN)
                    {
                        if (po.Status == 63) { }
                        else
                        {
                            po.Status = 35;//Change status of PO to Putin
                            db.SaveChanges();
                        }
                    }
                }
                if (qcObject == "SalesReturn")
                {
                    tQualityControlHead qc = new tQualityControlHead();
                    qc = (from q in db.tQualityControlHeads
                          where q.ID == QCId
                          select q).FirstOrDefault();
                    qc.Status = 54;
                    db.SaveChanges();
                    GrnId = long.Parse(qc.OID.ToString());

                    tGRNHead grn = new tGRNHead();
                    grn = (from g in db.tGRNHeads
                           where g.ID == GrnId
                           select g).FirstOrDefault();
                    grn.Status = 54;
                    db.SaveChanges();

                    tOrderHead oh = new tOrderHead();
                    oh = (from ordr in db.tOrderHeads
                          where ordr.Id == grn.OID
                          select ordr).FirstOrDefault();
                    long grnSO = oh.Id; poID = oh.Id;

                    List<tGRNHead> lstgrnSo = new List<tGRNHead>();
                    lstgrnSo = (from lgs in db.tGRNHeads
                                where lgs.OID == grnSO
                                select lgs).ToList();
                    int CntAllgrnSo = lstgrnSo.Count();
                    lstgrnSo = lstgrnSo.Where(s => s.Status == 54).ToList();
                    int qcgrnso = lstgrnSo.Count();
                    if (CntAllgrnSo == qcgrnso)
                    {
                        oh.Status = 54;
                        db.SaveChanges();
                    }
                }
                if (qcObject == "Transfer")
                {
                    tQualityControlHead qc = new tQualityControlHead();
                    qc = (from q in db.tQualityControlHeads
                          where q.ID == TransferQCID
                          select q).FirstOrDefault();
                    qc.Status = 62;
                    db.SaveChanges();
                    GrnId = long.Parse(qc.OID.ToString());

                    tGRNHead grn = new tGRNHead();
                    grn = (from g in db.tGRNHeads
                           where g.ID == GrnId
                           select g).FirstOrDefault();
                    grn.Status = 62;
                    db.SaveChanges();

                    tTransferHead th = new tTransferHead();
                    th = (from t in db.tTransferHeads
                          where t.ID == QCId
                          select t).FirstOrDefault();
                    th.Status = 62;
                    db.SaveChanges();
                }

                DataSet dsPutInDtl = new DataSet();
                dsPutInDtl.Reset();
                string strPutInDtl = "select * from tPutInDetail where PutInId = " + paraReferenceID + "";
                dsPutInDtl = fillds(strPutInDtl, conn);
                int PDCnt = dsPutInDtl.Tables[0].Rows.Count;
                if (PDCnt > 0)
                {
                    for (int i = 0; i <= PDCnt - 1; i++)
                    {
                        long PtDtPrdID = long.Parse(dsPutInDtl.Tables[0].Rows[i]["ProdID"].ToString());
                        decimal PtDtQty = decimal.Parse(dsPutInDtl.Tables[0].Rows[i]["PutInQty"].ToString());
                        long PtLocId = long.Parse(dsPutInDtl.Tables[0].Rows[i]["LocationID"].ToString());

                        DataSet dsPOD = new DataSet();
                        dsPOD.Reset();
                        string strPOD = "";
                        if (qcObject == "PurchaseOrder")
                        {
                            strPOD = "select OrderQty,ReceivedQty from tPurchaseOrderDetail where POOrderHeadId=" + poID + " and SkuId=" + PtDtPrdID + " ";
                            dsPOD = fillds(strPOD, conn);
                            decimal pOrderQty = decimal.Parse(dsPOD.Tables[0].Rows[0]["OrderQty"].ToString());
                            decimal ReceivedQty = decimal.Parse(dsPOD.Tables[0].Rows[0]["ReceivedQty"].ToString());
                            //if (ReceivedQty == null) ReceivedQty = 0;
                            if (ReceivedQty > 0) { ReceivedQty = ReceivedQty + PtDtQty; } else { ReceivedQty = PtDtQty; }

                            DataSet dsUpdPOD = new DataSet();
                            dsUpdPOD.Reset();
                            string strUpdPOD = "Update tPurchaseOrderDetail set ReceivedQty=" + ReceivedQty + " where POOrderHeadId=" + poID + " and SkuId=" + PtDtPrdID + "";
                            dsUpdPOD = fillds(strUpdPOD, conn);
                        }
                        else if (qcObject == "SalesReturn")
                        {
                            strPOD = "select OrderQty,RemaningQty ReceivedQty from tOrderDetail  where OrderHeadId=" + poID + " and SkuId=" + PtDtPrdID + " ";
                        }
                        else if (qcObject == "Transfer")
                        {
                            strPOD = "select Qty,ReceivedQty from tTransferDetail where TransferID=" + QCId + " and SkuId=" + PtDtPrdID + " ";
                            dsPOD = fillds(strPOD, conn);
                            decimal pOrderQty = decimal.Parse(dsPOD.Tables[0].Rows[0]["Qty"].ToString());
                            decimal ReceivedQty = decimal.Parse(dsPOD.Tables[0].Rows[0]["ReceivedQty"].ToString());
                            if (ReceivedQty > 0) { ReceivedQty = ReceivedQty + PtDtQty; } else { ReceivedQty = PtDtQty; }

                            DataSet dsUpdPOD = new DataSet();
                            dsUpdPOD.Reset();
                            string strUpdPOD = "Update tTransferDetail set ReceivedQty=" + ReceivedQty + " where TransferID=" + QCId + " and SkuId=" + PtDtPrdID + "";
                            dsUpdPOD = fillds(strUpdPOD, conn);
                        }

                        DataSet dsBatchID = new DataSet();
                        dsBatchID.Reset();
                        string strGetBatchID = "";
                        if (qcObject == "Transfer")
                        {
                            strGetBatchID = "select ID, OID,BatchNo from tGRNHead where OID in(" + QCId + ") and  ObjectName='Transfer'";
                        }
                        else
                        {
                            strGetBatchID = "select ID, OID,BatchNo from tGRNHead where ID in(select OID from tQualityControlHead where ID in(" + QCId + "))";
                        }
                        dsBatchID = fillds(strGetBatchID, conn);
                        string BatchID = dsBatchID.Tables[0].Rows[0]["BatchNo"].ToString();

                        tPutInHead PH = new tPutInHead();
                        PH = (from p in db.tPutInHeads
                              where p.ID == paraReferenceID
                              select p).FirstOrDefault();

                        tSKUTransaction skuTran = new tSKUTransaction();
                        skuTran.SKUId = PtDtPrdID;
                        skuTran.LocationID = PtLocId;
                        skuTran.BatchCode = BatchID;
                        skuTran.TransType = "Inbound";
                        skuTran.InQty = PtDtQty;
                        skuTran.OutQty = 0;
                        skuTran.ClosingBalance = PtDtQty;
                        skuTran.CompanyID = PH.Company;
                        // skuTran.CustomerID = PH.CustomerID;
                        skuTran.CreatedBy = PH.CreatedBy;
                        skuTran.CreationDate = PH.CreationDate;

                        db.tSKUTransactions.AddObject(skuTran);//Add record into tSKUTransaction
                        db.SaveChanges();
                        /*Update record of tProductStockDetails Trigger Update mLocation AvailableBalance */
                    }
                }
                //}
                //else
                //{
                //}

            }
            catch { Result = 0; }
            finally { }
            return Result;
        }

        public int CheckSelectedPutInStatusIsSameOrNot(string SelectedPutIn, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Count(*)Cnt,Status from tPutInHead where Id in(" + SelectedPutIn + ") Group By Status";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows.Count;
        }
        #endregion

        #region Transfer
        public DataSet BindTransferGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_TransferGrid order by ID desc";
            ds = fillds(str, conn);
            return ds;
        }

        public long GetGRNIDByTransferID(long trID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tGRNHead GH = new tGRNHead();
            GH = (from g in db.tGRNHeads
                  where g.OID == trID && g.ObjectName == "Transfer"
                  select g).FirstOrDefault();
            long grnid = long.Parse(GH.ID.ToString());
            return grnid;
        }
        #endregion

        #region Return
        public DataSet BindReturnGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "SELECT    R.ID,R.SONo, O.OrderNo,O.CustomerID,C.Name CustomerName,R.ReturnBy,U.FirstName+' '+U.LastName ReturnByUsr, R.Status,S.Status StatusName, R.ReturnDate,R.CompanyID, R.CustomerID, Case When R.Status=2 then 'red' when R.Status in(3,26) then 'green' end as  ImgApproved, Case When R.Status=2 then 'gray' when R.Status=3 then'red' when R.Status=26 then 'green' end as ImgReturn FROM  tReturnHead R  left outer join tOrderHead O on R.SONo=O.Id left outer join mCustomer C on O.CustomerID=C.ID left outer join mUserProfileHead U on R.ReturnBy=U.ID left outer join mStatus S on R.Status=S.ID";
            ds = fillds(str, conn);
            return ds;
        }
        #endregion

        #region ASN
        public DataSet GetASNHead(long poID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "SELECT  A.ID, A.ASNNumber, A.ASNDate,  A.Remark, A.CreatedBy, A.CreationDate, A.poId ,A.ASNEnteredBy, U.FirstName+' '+U.LastName ASNBy FROM tASNHead A left outer join mUserProfileHead U on A.ASNEnteredBy =U.id where poid=" + poID + "";
            ds = fillds(str, conn);
            return ds;
        }

        public tASNHead GetAsnByAsnID(long asnID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tASNHead asnhd = new tASNHead();
            asnhd = (from a in db.tASNHeads
                     where a.ID == asnID
                     select a).FirstOrDefault();
            return asnhd;
        }

        public DataSet GetASNDetailByID(long asnID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_ASNDetail where asnheadId=" + asnID + " ";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet GetLoaderDetailsOfGRN(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "SELECT L.ID, L.ObjectName, L.ReferenceID, L.LoaderID,V.Name LoaderName, L.InTime, L.OutTime, L.BoxHandels, L.RatePerBox, L.Total FROM  tLoaderDetail L left outer join mVendor V on L.LoaderID=V.ID";
            ds = fillds(str, conn);
            return ds;
        }

        public tLoaderDetail GetLoaderDetailOfGRN(long ldrID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tLoaderDetail ld = new tLoaderDetail();
            ld = (from l in db.tLoaderDetails
                  where l.ID == ldrID
                  select l).FirstOrDefault();
            return ld;
        }
        #endregion

        #region CreditNoteDebitNote
        public long GetExcessQtyByGRNID(long grnID, string[] conn)
        {
            long result = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tGRNDetail GD = new tGRNDetail();
            long cnt = (from g in db.tGRNDetails
                        where g.GRNID == grnID && g.ExcessQty > 0
                        select g).Count();
            if (cnt > 0) { result = 1; }
            else { result = 0; }

            return result;
        }

        public long GetShortQtyByGRNID(long grnID, string[] conn)
        {
            long result = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tGRNDetail GD = new tGRNDetail();
            long cnt = (from g in db.tGRNDetails
                        where g.GRNID == grnID && g.ShortQty > 0
                        select g).Count();
            if (cnt > 0) { result = 1; }
            else { result = 0; }

            return result;
        }

        public List<WMS_GetCreditNoteDetailByPOID_Result> GetCreditNotePartDetailByPOID(long poID, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_GetCreditNoteDetailByPOID_Result> PartDetail = new List<WMS_GetCreditNoteDetailByPOID_Result>();
            PartDetail = (from sp in db.WMS_GetCreditNoteDetailByPOID(poID, WarehouseID)
                          select sp).ToList();
            PartDetail = PartDetail.Where(q => q.RemaningQty > 0).ToList();
            return PartDetail;
        }

        public string GetVendorNameByID(long vendorID, string[] conn)
        {
            string vndrName = "";
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mVendor vn = new mVendor();
            vn = (from v in db.mVendors
                  where v.ID == vendorID
                  select v).FirstOrDefault();
            vndrName = vn.Name.ToString();
            return vndrName;
        }

        public decimal GetTotalofCreditNote(long poID, long warehouseID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select SUM(NewPrice) Tot from ( Select 	Convert(Decimal(18,2), ABS(ReqDetail.RemaningQty) * ReqDetail.Price) as NewPrice from dbo.mProduct mp Left Outer Join dbo.tProductStockDetails stk on mp.ID = stk.ProdID and stk.SiteID = " + warehouseID + " Inner join dbo.tPurchaseOrderDetail ReqDetail on mp.ID = ReqDetail.SkuId Left Outer Join dbo.mUOM mU On ReqDetail.UOMID = mU.ID left outer join tAddToCartProductTaxDetail PTx on ReqDetail.POOrderHeadId=PTx.ReferenceID and ReqDetail.SkuId=PTx.ProductDetail_Sequence Where ReqDetail.POOrderHeadId = " + poID + " and ReqDetail.RemaningQty!=0)aaa";
            ds = fillds(str, conn);
            decimal total = Convert.ToDecimal(ds.Tables[0].Rows[0]["Tot"].ToString());
            return total;
        }

        public long SaveIntotCreditNoteHead(tCreditNoteHead CNHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            db.tCreditNoteHeads.AddObject(CNHead);
            db.SaveChanges();
            return CNHead.ID;
        }

        public int SaveCreditNoteDetail(long cnhID, long poID, long warehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            int Result = 0;
            try
            {
                List<WMS_GetCreditNoteDetailByPOID_Result> fnlSaveLst = new List<WMS_GetCreditNoteDetailByPOID_Result>();
                fnlSaveLst = db.WMS_GetCreditNoteDetailByPOID(poID, warehouseID).ToList();

                XElement xmlEle = new XElement("CN", from rec in fnlSaveLst
                                                     select new XElement("PartList",
                                                         new XElement("CreditNoteID", cnhID),
                                                         new XElement("ProdID", Convert.ToInt64(rec.PRD_ID)),
                                                         new XElement("POQty", Convert.ToDecimal(rec.RequestQty)),
                                                         new XElement("CreditQty", Convert.ToDecimal(rec.RemaningQty)),
                                                         new XElement("Price", Convert.ToDecimal(rec.Price)),
                                                         new XElement("Total", Convert.ToDecimal(rec.NewPrice))
                                                         ));
                ObjectParameter _CreditNoteID = new ObjectParameter("CreditNoteID", typeof(long));
                _CreditNoteID.Value = cnhID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _CreditNoteID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotCreditNoteDetail", obj);
                db.SaveChanges();
                Result = 1;
            }
            catch (System.Exception ex)
            { Result = 0; }
            finally { }
            return Result;
        }

        public DataSet CheckCreditNote(long poID, string objectName, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "SELECT * FROM tCreditNoteHead  where ONO=" + poID + " and ObjectName='" + objectName + "'";
            ds = fillds(str, conn);
            return ds;
        }
        #endregion

        #region DebitNote
        public void MakePOStatusGRN(long poID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tPurchaseOrderHead PH = new tPurchaseOrderHead();
            PH = (from p in db.tPurchaseOrderHeads
                  where p.Id == poID
                  select p).FirstOrDefault();

            PH.Status = 32;//Make PO Status QC
            db.SaveChanges();
        }

        public List<WMS_GetDebitNoteDetailByPOID_Result> GetDebitNotePartDetailByPOID(long poID, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_GetDebitNoteDetailByPOID_Result> PartDetail = new List<WMS_GetDebitNoteDetailByPOID_Result>();
            PartDetail = (from sp in db.WMS_GetDebitNoteDetailByPOID(poID, WarehouseID)
                          select sp).ToList();
            PartDetail = PartDetail.Where(q => q.RemaningQty > 0).ToList();
            return PartDetail;
        }

        public DataSet CheckDebitNote(long poID, string objectName, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "SELECT * FROM tDebitNoteHead  where ONO=" + poID + " and ObjectName='" + objectName + "'";
            ds = fillds(str, conn);
            return ds;
        }

        public long SaveIntotDebitNoteHead(tDebitNoteHead DNHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            db.tDebitNoteHeads.AddObject(DNHead);
            db.SaveChanges();
            return DNHead.ID;
        }

        public int SaveDebitNoteDetail(long dnhID, long poID, long warehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            int Result = 0;
            try
            {
                List<WMS_GetDebitNoteDetailByPOID_Result> fnlSaveLst = new List<WMS_GetDebitNoteDetailByPOID_Result>();
                fnlSaveLst = db.WMS_GetDebitNoteDetailByPOID(poID, warehouseID).ToList();

                XElement xmlEle = new XElement("DN", from rec in fnlSaveLst
                                                     select new XElement("PartList",
                                                         new XElement("DebitNoteID", dnhID),
                                                         new XElement("ProdID", Convert.ToInt64(rec.PRD_ID)),
                                                         new XElement("OQty", Convert.ToDecimal(rec.RequestQty)),
                                                         new XElement("DebitQty", Convert.ToDecimal(rec.RemaningQty)),
                                                         new XElement("Price", Convert.ToDecimal(rec.Price)),
                                                         new XElement("Total", Convert.ToDecimal(rec.NewPrice))
                                                         ));
                ObjectParameter _DebitNoteID = new ObjectParameter("DebitNoteID", typeof(long));
                _DebitNoteID.Value = dnhID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _DebitNoteID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotDebitNoteDetail", obj);
                db.SaveChanges();
                Result = 1;
            }
            catch (System.Exception ex)
            { Result = 0; }
            finally { }
            return Result;
        }
        #endregion
    }
    //class Inbound
    //{
    //}
}
