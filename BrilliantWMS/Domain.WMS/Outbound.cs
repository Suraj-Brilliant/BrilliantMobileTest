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
    //[AspNetCompatibilityRequirements(RequirementsMode=AspNetCompatibilityRequirementsMode.Allowed)]
    //   [ServiceBehavior(IncludeExceptionDetailInFaults=true)]

    public partial class WMS : Interface.WMS.iOutbound
    {
        Domain.Server.Server sv = new Server.Server();
        DataHelper datahelperO = new DataHelper();

        #region Outbound

        protected DataSet filldsO(string strquery, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(sv.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            SqlDataAdapter da = new SqlDataAdapter(strquery, sqlConn);
            ds.Reset();
            da.Fill(ds);
            return ds;
        }

        public DataSet BindOutboundGrid(long userCompany,string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_OutboundList where CompanyID="+ userCompany +" order by Id desc ";
            ds = filldsO(str, conn);
            return ds;
        }

        public DataSet BindOutboundGridbyUser(long userID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from WMS_VW_OutboundList where CreatedBy=" + userID + " order by Id desc ";
            ds = filldsO(str, conn);
            return ds;
        }

        public int CheckSelectedSOStatusIsSameOrNot(string SelectedSO, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Count(*)Cnt,Status from tOrderHead where Id in(" + SelectedSO + ") Group By Status";
            ds = fillds(str, conn);
            return ds.Tables[0].Rows.Count;
        }

        public DataSet GetNextSOObject(string SelectedRecords, string ObjectName, long CompanyID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "";
            if (ObjectName == "SalesOrder")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tOrderHead where Id in(" + SelectedRecords + ") Group By Status))";
            }
            else if (ObjectName == "PickUp")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tPickupHead where Id in(" + SelectedRecords + ") Group By Status))";
            }
            else if (ObjectName == "QC")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tQualityControlHead where Id in(" + SelectedRecords + ") Group By Status))";
            }
            else if (ObjectName == "Dispatch")
            {
                str = "select OBJECTID from mObject where ObjectName in(select ObjectName from mStatus where Id in( select Status from tQualityControlHead where Id in(" + SelectedRecords + ") Group By Status))";
            }
            ds = fillds(str, conn);
            int ObjID = Convert.ToInt16(ds.Tables[0].Rows[0]["OBJECTID"].ToString());

            DataSet dsNewObject = new DataSet();
            dsNewObject.Reset();
            string st = "select Top(1)WH.WorkflowName,WD.ObjectID,WD.Sequence,o.ObjectName from mWorkFlowHead WH left outer join mWorkFlowDetail WD on WH.ID=WD.WorkflowID left outer join mObject o on WD.ObjectID=o.OBJECTID where WH.CompanyID=" + CompanyID + " and WH.WorkflowName='Inbound' and WD.ObjectID > " + ObjID + " and WD.Sequence > (select Sequence from mWorkFlowDetail where ObjectID=" + ObjID + " and CompanyID=" + CompanyID + " and WorkflowID=1)  order by WD.Sequence ";
            dsNewObject = fillds(st, conn);

            return dsNewObject;
        }
        #endregion
        #region SalesOrder
        public List<mWarehouseMaster> GetUserWarehouseSO(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseMaster> UsrW = new List<mWarehouseMaster>();
            UsrW = (from w in ce.mWarehouseMasters
                    join u in ce.mUserWarehouses on w.ID equals u.WarehoueID
                    where u.UserID == UserID
                    select w).ToList();
            return UsrW;
        }

        public List<mWarehouseMaster> GetWarehouseNameByUserIDSO(long uid, string[] conn)
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

        public List<mWarehouseMaster> GetAllWarehouseListSO(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseMaster> WarehouseName = new List<mWarehouseMaster>();
            WarehouseName = (from w in ce.mWarehouseMasters
                             orderby w.WarehouseName
                             select w).ToList();
            return WarehouseName;
        }

        public List<mClient> GetCompanyWiseClient(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mClient> Clnt = new List<mClient>();
            Clnt = (from c in ce.mClients
                    where c.CompanyID == CompanyID
                    select c).ToList();
            return Clnt;
        }

        public mClient GetClientNameByID(long ClientID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mClient cl = new mClient();
            cl = (from n in ce.mClients
                  where n.ID == ClientID
                  select n).FirstOrDefault();
            return cl;
        }

        public mClient GetClientDetailByClientName(string clientName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mClient cl = new mClient();
            cl = (from c in ce.mClients
                  where c.Name == clientName
                  select c).FirstOrDefault();
            if (cl != null)
                ce.Detach(cl);
            return cl;
        }

        public long SaveNewClientDetails(mClient objCl, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.mClients.AddObject(objCl);
            ce.SaveChanges();
            return objCl.ID;
        }

        public void ClearTempDataFromDBNEWSO(string paraSessionID, string paraUserID, string CurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.UserID == paraUserID
                        && rec.ObjectName == CurrentObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<mStatu> GetStatusListForOutbound(string ObjectName, string Remark, string state, long UserID, string[] conn)
        {
            List<mStatu> statusdetail = new List<mStatu>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                string[] RemarkArr = Remark.Split(',');
                if (Remark != "" && ObjectName != "")
                {
                    statusdetail = (from st in db.mStatus
                                    where (st.ObjectName == ObjectName && RemarkArr.Contains(st.Remark))
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

        public int GetWorkflowSequenceOfSO(string ObjectName, long CompanyID, string[] conn)
        {
            int Seq = 0;
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                mWorkFlowDetail WD = new mWorkFlowDetail();
                WD = (from h in db.mWorkFlowHeads
                      join d in db.mWorkFlowDetails on h.ID equals d.WorkflowID
                      join o in db.mObjects on d.ObjectID equals o.OBJECTID
                      where h.CompanyID == CompanyID && h.WorkflowName == "Outbound" && o.ObjectName == ObjectName
                      select d).FirstOrDefault();
                Seq = Int16.Parse(WD.Sequence.ToString());
            }
            catch { }
            finally { }
            return Seq;
        }

        public List<WMS_SP_GetPartDetail_ForSO_Result> GetExistingTempDataBySessionIDObjectNameSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetail_ForSO_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public List<WMS_SP_GetPartDetail_ForSO_Result> AddPartIntoRequest_TempDataSO(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForSO_Result> existingList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            long MaxSequenceNo = 0;
            if (existingList.Count > 0)
            {
                MaxSequenceNo = Convert.ToInt64((from lst in existingList
                                                 select lst.Sequence).Max().Value);
            }

            /*Get Product Details*/
            List<WMS_SP_GetPartDetail_ForSO_Result> getnewRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getnewRec = (from view in db.WMS_SP_GetPartDetail_ForSO(paraProductIDs, MaxSequenceNo, WarehouseID, 0)
                         orderby view.Sequence
                         select view).ToList();
            /*End*/

            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            List<WMS_SP_GetPartDetail_ForSO_Result> mergedList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            mergedList.AddRange(existingList);
            mergedList.AddRange(getnewRec);
            /*End*/

            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDBSO(mergedList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return mergedList;
        }

        protected void SaveTempDataToDBSO(List<WMS_SP_GetPartDetail_ForSO_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);
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

        public void ClearTempDataFromDBSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<WMS_SP_GetPartDetail_ForSO_Result> GetRequestPartDetailByRequestIDSO(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> PartDetail = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            PartDetail = (from sp in db.WMS_SP_GetPartDetail_ForSO("0", 0, WarehouseID, RequestID)
                          select sp).ToList();
            SaveTempDataToDBSO(PartDetail, sessionID, userID, CurrentObject, conn);
            return PartDetail;
        }

        public tOrderHead GetOrderHeadByOrderIDSO(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead SOH = new tOrderHead();
            SOH = db.tOrderHeads.Where(s => s.Id == OrderID).FirstOrDefault();
            db.tOrderHeads.Detach(SOH);
            return SOH;
        }

        public List<WMS_SP_GetPartDetail_ForSO_Result> RemovePartFromRequest_TempDataSO(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForSO_Result> existingList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Get Filter List [Filter By paraSequence]*/
            List<WMS_SP_GetPartDetail_ForSO_Result> filterList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/
            /*Save result to TempData*/
            SaveTempDataToDBSO(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<WMS_SP_GetPartDetail_ForSO_Result> NewList = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            NewList = GetExistingTempDataBySessionIDObjectNameSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result UpdRec = new WMS_SP_GetPartDetail_ForSO_Result();

            if (cnt > 0)
            {
                for (int i = paraSequence; i <= cnt; i++)
                {
                    UpdRec = NewList.Where(u => u.Sequence == i + 1).FirstOrDefault();
                    UpdRec.Sequence = i;
                    SaveTempDataToDBSO(NewList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
                }
            }
            /*End*/
            if (cnt > 0)
            { return NewList; }
            else { return filterList; }
        }

        public void UpdatePartRequest_TempData1SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDBSO(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public void UpdatePartRequest_TempDataRtrn(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.ReturnQty = Request.ReturnQty;
            SaveTempDataToDBSO(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public decimal GetTotalFromTempDataSO(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            //totPrice = getRec.Sum(s => s.Total);
            totPrice = getRec.Sum(s => s.AmountAfterTax);
            return totPrice;
        }

        public decimal GetTotalQTYFromTempDataSO(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            totPrice = getRec.Sum(s => s.RequestQty);
            return totPrice;
        }

        public void UpdatePartRequest_TempData12SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty; updateRec.UOM = Request.UOM;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            updateRec.Price = Request.Price;
            updateRec.IsPriceChange = Request.IsPriceChange;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDBSO(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public decimal GetTotalQTYofSequenceFromTempDataSO(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(s => s.Sequence == Sequence).FirstOrDefault();
            totPrice = updateRec.Total; long PrdID = updateRec.Prod_ID;
            return totPrice;
        }

        public long GetPrdIDofSequenceFromTempDataSO(int Sequence, string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            long PrdID = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);
            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(s => s.Sequence == Sequence).FirstOrDefault();
            PrdID = updateRec.Prod_ID;
            return PrdID;
        }

        public void UpdatePartRequest_TempData13SO(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForSO_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForSO_Result> getRec = new List<WMS_SP_GetPartDetail_ForSO_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameSO(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForSO_Result updateRec = new WMS_SP_GetPartDetail_ForSO_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();
            updateRec.TotalTaxAmount = Request.TotalTaxAmount;
            updateRec.AmountAfterTax = Request.AmountAfterTax;
            SaveTempDataToDBSO(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public long SetIntotSalesOrderHead(tOrderHead SOHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (SOHead.Id == 0)
            {
                db.tOrderHeads.AddObject(SOHead);
            }
            else
            {
                db.tOrderHeads.Attach(SOHead);
                db.ObjectStateManager.ChangeObjectState(SOHead, EntityState.Modified);
            }
            db.SaveChanges();
            return SOHead.Id;
        }

        public int FinalSavSODetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var tOrderDetails = 0;
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetail_ForSO_Result> fnlSaveLst = new List<WMS_SP_GetPartDetail_ForSO_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNameSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);

                XElement xmlEle = new XElement("SO", from rec in fnlSaveLst
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
                                                       new XElement("ShippedQty", 0),
                                                       new XElement("ReturnQty", Convert.ToDecimal(rec.ReturnQty))));

                ObjectParameter _PRH_ID = new ObjectParameter("PRH_ID", typeof(long));
                _PRH_ID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _PRH_ID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotOrderDetail", obj);
                db.SaveChanges(); tOrderDetails = 1; Result = 1;

                /*Add Record Of User into table tOrderWiseAccess*/
                if (StatusID == 49)
                {
                    tOrderHead Oh = new tOrderHead();
                    Oh = (from od in db.tOrderHeads
                          where od.Id == paraReferenceID
                          select od).FirstOrDefault();
                    Oh.Object = "SalesReturn";
                    Oh.Status = 48;
                    db.SaveChanges();
                }
                else
                {
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
                    /*Code For Add Product Wise Tax Details Start*/
                    FinalSaveProductLeveltaxSO("SalesOrderTax", paraReferenceID, paraSessionID, conn);
                    FinalSaveTaxOnTotalSO("SalesOrderTotalTax", paraReferenceID, paraSessionID, conn);
                    /*Code For Add Product Wise Tax Details End*/
                }
                ClearTempDataFromDBSO(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            }
            catch (System.Exception ex)
            {
                if (tOrderDetails == 0)
                {
                    long OrderID = paraReferenceID;
                    //RollBack(OrderID, conn);
                    Result = 0;
                }
            }
            finally { }
            return Result;
        }

        public void FinalSaveProductLeveltaxSO(string CurrentObjectName, long paraReferenceID, string paraSessionID, string[] conn)
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
            }
            catch { }
        }
        public void FinalSaveTaxOnTotalSO(string CurrentObjectName, long paraReferenceID, string paraSessionID, string[] conn)
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

        public tOrderHead GetSoHeadBySOID(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead sohead = new tOrderHead();
            sohead = db.tOrderHeads.Where(s => s.Id == OrderID).FirstOrDefault();
            db.tOrderHeads.Detach(sohead);
            return sohead;
        }

        public DataSet CheckSelectedSOJobCardNo(long SONumber, string ObjectName, string[] conn)
        {
            DataSet dsJCN = new DataSet(); dsJCN.Reset();
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long TaskID = (from t in db.tJobCardDetails
                           where t.OrderObjectName == ObjectName && t.OID == SONumber
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
                if (ObjectName == "SalesOrder")
                {
                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(StoreId) from tOrderHead where ID in(" + OrderNo + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["StoreId"].ToString();
                }
                else if (ObjectName == "PickUp")
                {
                    DataSet dsG = new DataSet();
                    string stG = "select OID from tPickUpHead where ID In(" + OrderNo + ")";
                    dsG = fillds(stG, conn);
                    long OID = long.Parse(dsG.Tables[0].Rows[0]["OID"].ToString());

                    DataSet dsW = new DataSet(); dsW.Reset();
                    string sW = "select distinct(StoreId) from tOrderHead where ID in(" + OID + ")";
                    dsW = fillds(sW, conn);
                    Warehouse = dsW.Tables[0].Rows[0]["StoreId"].ToString();
                }
                else if (ObjectName == "QC")
                {
                    DataSet dsQ = new DataSet();
                    string strQ = "select StoreId from tOrderHead where Id=(select OID from tPickUpHead where ID=(select OID from tQualityControlHead where ID=" + SONumber + " and ObjectName='SalesOrder'))";
                    dsQ = fillds(strQ, conn);
                    Warehouse = dsQ.Tables[0].Rows[0]["StoreId"].ToString();
                }
                else if (ObjectName == "Transfer")
                {
                    DataSet dsTr = new DataSet();
                    string strTr = "select FromPosition from tTransferHead where ID=" + SONumber + "";
                    dsTr = fillds(strTr, conn);
                    Warehouse = dsTr.Tables[0].Rows[0]["FromPosition"].ToString();
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

        #endregion
        #region PickList

        public DataSet BindPickUpGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select PH.ID,PH.OID,case when T.JobCardName is null then 'Not Created' else T.JobCardName end as JobCardName , PH.PickUpDate,PH.PickUpBy,U.FirstName+' '+U.LastName PickByUser ,PH.Status,S.Status StatusName,PH.CustomerID,Cl.Name CustomerName,  Case When PH.Status = 37 then 'red' when PH.Status In (38,40,32) then 'green'  end as ImgPickUp, Case When PH.Status =38 then 'red' when PH.Status=37 then 'gray' when PH.Status in(32,40) then 'green'  end as ImgQC, Case When PH.Status=32 then 'red' when PH.Status in(37,38) then 'gray' when PH.Status=40 then 'green' end as ImgDispatch from tPickUpHead PH left outer join tOrderHead O on PH.OID = O.Id left outer join mStatus S on PH.Status=S.ID left outer join mUserProfileHead U on PH.PickUpBy=U.ID left outer join mClient Cl on O.ClientID=Cl.ID left outer join tJobCardDetail J on PH.Id=J.OID and J.OrderObjectName='PickUp' left outer join tTaskDetail T on J.TaskID=T.TaskID order by PH.ID desc";
            ds = filldsO(str, conn);
            return ds;
        }

        public List<WMS_SP_PickUpList_Result> GetPickUpList(string ODID, string TRID,string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_PickUpList_Result> PickUpLst = new List<WMS_SP_PickUpList_Result>();
            PickUpLst = db.WMS_SP_PickUpList(ODID, TRID).ToList();
            SaveTempDataToDBPickUp(PickUpLst, SessionID, UserID, CurrentObject, conn);
            return PickUpLst;
        }

        public DataSet GetSavedPickUpListBySOID(string pkUPNo, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "exec WMS_SP_PickUpList '"+ pkUPNo +"',''";
            ds = filldsO(str, conn);
            return ds;
        }

        protected void SaveTempDataToDBPickUp(List<WMS_SP_PickUpList_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBPickUp(paraSessionID, paraUserID, paraCurrentObjectName, conn);
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

        public void ClearTempDataFromDBPickUp(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<WMS_SP_PickUpList_Result> GetExistingTempDataBySessionIDObjectNamePickUp(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_PickUpList_Result> objtAddToCartProductDetailList = new List<WMS_SP_PickUpList_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_PickUpList_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public string UpdatePickUPLstQtyofSelRow(string SessionID, string paraObjectName, string UserID, WMS_SP_PickUpList_Result pkuplst, string[] conn)
        {
            string ActualQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_PickUpList_Result> getrec = new List<WMS_SP_PickUpList_Result>();
                getrec = GetExistingTempDataBySessionIDObjectNamePickUp(SessionID, UserID, paraObjectName, conn);

                WMS_SP_PickUpList_Result updRec = new WMS_SP_PickUpList_Result();
                updRec = getrec.Where(s => s.Sequence == pkuplst.Sequence).FirstOrDefault();
                updRec.LocQty = Convert.ToDecimal(pkuplst.LocQty);

                SaveTempDataToDBPickUp(getrec, SessionID, UserID, paraObjectName, conn);
            }
            catch { }
            finally { }
            return ActualQty;
        }

        public string UpdatePkupLstLocofSelRow(string SessionID,string paraObjectName,string paraUserID,WMS_SP_PickUpList_Result pkuplst,string[] conn)
        {
            string ActualQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_PickUpList_Result> getrec = new List<WMS_SP_PickUpList_Result>();
                getrec = GetExistingTempDataBySessionIDObjectNamePickUp(SessionID, paraUserID, paraObjectName, conn);

                WMS_SP_PickUpList_Result updRec = new WMS_SP_PickUpList_Result();
                updRec = getrec.Where(s => s.Sequence == pkuplst.Sequence).FirstOrDefault();
                updRec.LocationID = Convert.ToInt64(pkuplst.LocationID);
                updRec.Code = pkuplst.Code.ToString();
                updRec.SortCode = Convert.ToInt64(pkuplst.SortCode);
                updRec.Capacity = Convert.ToDecimal(pkuplst.Capacity);
                updRec.AvailableBalance = Convert.ToDecimal(pkuplst.AvailableBalance);

                SaveTempDataToDBPickUp(getrec, SessionID, paraUserID, paraObjectName, conn);
            }
            catch { }
            finally { }
            return ActualQty;
        }

        public long SavetPickUpHead(tPickUpHead puHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (puHead.ID == 0)
            {
                db.tPickUpHeads.AddObject(puHead);
            }
            else
            {
                db.tPickUpHeads.Attach(puHead);
                db.ObjectStateManager.ChangeObjectState(puHead, EntityState.Modified);
            }
            db.SaveChanges();
            return puHead.ID;
        }

        public int FinalSavePickUpDetail(long ODId, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            int Result = 0;
            try
            {
                List<WMS_SP_PickUpList_Result> fnlSaveLst = new List<WMS_SP_PickUpList_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNamePickUp(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                fnlSaveLst = fnlSaveLst.Where(q => q.OrderHeadId == ODId).ToList();

                XElement xmlEle = new XElement("PickUp", from rec in fnlSaveLst
                                                         select new XElement("PartList",
                                                             new XElement("PickUpID", paraReferenceID),
                                                             new XElement("ProdID", Convert.ToInt64(rec.SkuId)),
                                                             new XElement("PickUpQty", Convert.ToDecimal(rec.LocQty)),
                                                              new XElement("LocationID", Convert.ToInt64(rec.LocationID)),
                                                             new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                             new XElement("BatchCode", rec.BatchNo)
                                                             ));

                ObjectParameter _PickUpID = new ObjectParameter("PickUpID", typeof(long));
                _PickUpID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _PickUpID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotPickUpDetail", obj);
                db.SaveChanges(); Result = 1;

                DataSet dspkNO = new DataSet();
                dspkNO.Reset();
                string str = "select OID,ObjectName from tPickUpHead where ID=" + paraReferenceID + "";
                dspkNO = fillds(str, conn);
                long OrderNo = long.Parse(dspkNO.Tables[0].Rows[0]["OID"].ToString());
                string ObjectNM = dspkNO.Tables[0].Rows[0]["ObjectName"].ToString();
                if (ObjectNM == "SalesOrder")
                {
                    DataSet dsPrdPkUp = new DataSet();
                    dsPrdPkUp.Reset();
                    string stPKUP = "select * from tPickUpDetail where PickUpID=" + paraReferenceID + "";
                    dsPrdPkUp = fillds(stPKUP, conn);
                    int dsCount = dsPrdPkUp.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int g = 0; g <= dsCount - 1; g++)
                        {
                            long PkUpPrdID = long.Parse(dsPrdPkUp.Tables[0].Rows[g]["ProdID"].ToString());
                            decimal grnQty = decimal.Parse(dsPrdPkUp.Tables[0].Rows[g]["PickUpQty"].ToString());

                            tOrderDetail GetSKUID = new tOrderDetail();
                            GetSKUID = (from pk in db.tOrderDetails
                                        where pk.OrderHeadId == OrderNo && pk.RemaningQty > 0 && pk.SkuId == PkUpPrdID
                                        select pk).FirstOrDefault();

                            decimal RemainingQty = decimal.Parse(GetSKUID.RemaningQty.ToString());
                            RemainingQty = RemainingQty - grnQty;
                            GetSKUID.RemaningQty = RemainingQty;
                            db.SaveChanges();
                        }
                    }
                    int CntSOQty = (from s in db.tOrderDetails
                                    where s.RemaningQty > 0 && s.OrderHeadId == OrderNo
                                    select s).Count();
                    if (CntSOQty == 0)
                    {
                        tOrderHead SOStatus = new tOrderHead();
                        SOStatus = (from so in db.tOrderHeads
                                    where so.Id == OrderNo
                                    select so).FirstOrDefault();
                        SOStatus.Status = 38;
                        db.SaveChanges();
                    }
                }
                else if (ObjectNM == "Transfer")
                {
                    DataSet dsPrdPkUp = new DataSet();
                    dsPrdPkUp.Reset();
                    string stPKUP = "select * from tPickUpDetail where PickUpID=" + paraReferenceID + "";
                    dsPrdPkUp = fillds(stPKUP, conn);
                    int dsCount = dsPrdPkUp.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int g = 0; g <= dsCount - 1; g++)
                        {
                            long PkUpPrdID = long.Parse(dsPrdPkUp.Tables[0].Rows[g]["ProdID"].ToString());
                            decimal grnQty = decimal.Parse(dsPrdPkUp.Tables[0].Rows[g]["PickUpQty"].ToString());

                            tTransferDetail GetTRSKUID = new tTransferDetail();
                            GetTRSKUID = (from tk in db.tTransferDetails
                                          where tk.TransferID == OrderNo && tk.RemaningQty > 0 && tk.SkuId == PkUpPrdID
                                          select tk).FirstOrDefault();

                            decimal RemainingQty = decimal.Parse(GetTRSKUID.RemaningQty.ToString());
                            RemainingQty = RemainingQty - grnQty;
                            GetTRSKUID.RemaningQty = RemainingQty;
                            db.SaveChanges();
                        }
                    }
                    int CntTRQty = (from t in db.tTransferDetails
                                    where t.RemaningQty > 0 && t.TransferID == OrderNo
                                    select t).Count();
                    if (CntTRQty == 0)
                    {
                        tTransferHead TRStatus = new tTransferHead();
                        TRStatus = (from th in db.tTransferHeads
                                    where th.ID == OrderNo
                                    select th).FirstOrDefault();
                        TRStatus.Status = 57;
                        db.SaveChanges();
                    }
                }
            }
            catch { Result = 0; }
            finally { }
            return Result;
        }

        public long GetSOIDfromPkUpID(long pkupId, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tPickUpHead Pkup = new tPickUpHead();
            Pkup = (from p in db.tPickUpHeads
                    where p.ID == pkupId
                    select p).FirstOrDefault();
            long SOID = long.Parse(Pkup.OID.ToString());
            return SOID;
        }
        #endregion
        #region QC
        #endregion
        #region Dispatch

        public DataSet BindDispatchGrid(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select D.ID,D.OID QCNo,D.DispatchDate,D.Status,S.Status StatusName, D.Title,D.CompanyID,D.CustomerID,C.Name CustomerName,D.DispatchBy,D.CreatedBy ,U.FirstName+' '+U.LastName DispatchByUser, Case When D.Status=32 then 'red' when D.Status in(37,38) then 'gray' when D.Status=40 then 'green' end as ImgDispatch from tDispatchHead D left outer join tQualityControlHead Q on D.OID=Q.ID left outer join mStatus S on D.Status=S.ID left outer join mUserProfileHead U on D.CreatedBy =U.ID  left outer join mCustomer C on D.CustomerID=C.ID order by D.ID desc ";
            ds = filldsO(str, conn);
            return ds;
        }

        public tOrderHead GetSoDetailByQCID(long qcID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tQualityControlHead qch = new tQualityControlHead();
            qch = (from q in db.tQualityControlHeads
                   where q.ID == qcID
                   select q).FirstOrDefault();
            long pkUpID = long.Parse(qch.OID.ToString());

            tPickUpHead pkh = new tPickUpHead();
            pkh = (from p in db.tPickUpHeads
                   where p.ID == pkUpID
                   select p).FirstOrDefault();
            long soID = long.Parse(pkh.OID.ToString());

            tOrderHead OH = new tOrderHead();
            OH = (from o in db.tOrderHeads
                  where o.Id == soID
                  select o).FirstOrDefault();
            return OH;
        }

        public List<WMS_SP_GetPartDetail_ForDispatch_Result> GetDispatchPartByQCID(string qcID, string trID,string sessionID, string userID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForDispatch_Result> Partlst = new List<WMS_SP_GetPartDetail_ForDispatch_Result>();
         //   Partlst = db.WMS_SP_GetPartDetail_ForDispatch(qcID, trID).ToList();
            SaveTempDataToDBDispatch(Partlst, sessionID, userID, CurrentObject, conn);
            return Partlst;
        }

        protected void SaveTempDataToDBDispatch(List<WMS_SP_GetPartDetail_ForDispatch_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBDispatch(paraSessionID, paraUserID, paraCurrentObjectName, conn);
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

        public void ClearTempDataFromDBDispatch(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<WMS_SP_GetPartDetail_ForDispatch_Result> GetExistingTempDataBySessionIDObjectNameDispatch(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForDispatch_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetail_ForDispatch_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetail_ForDispatch_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public string UpdateDispatchData(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForDispatch_Result Dispatch, string[] conn)
        {
            string RemainingQty = "0";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<WMS_SP_GetPartDetail_ForDispatch_Result> getRec = new List<WMS_SP_GetPartDetail_ForDispatch_Result>();
                getRec = GetExistingTempDataBySessionIDObjectNameDispatch(SessionID, UserID, CurrentObjectName, conn);

                WMS_SP_GetPartDetail_ForDispatch_Result updateRec = new WMS_SP_GetPartDetail_ForDispatch_Result();
                updateRec = getRec.Where(r => r.Sequence == Dispatch.Sequence).FirstOrDefault();

                //updateRec.GRNQty = Convert.ToDecimal(GRN.GRNQty);
                //RemainingQty = (updateRec.POQty - updateRec.GRNQty).ToString();

                SaveTempDataToDBDispatch(getRec, SessionID, UserID, CurrentObjectName, conn);
            }
            catch { }
            finally { }
            return RemainingQty;
        }

        public long SavetDispatchHead(tDispatchHead dpHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (dpHead.ID == 0)
            {
                db.tDispatchHeads.AddObject(dpHead);
            }
            else
            {
                db.tDispatchHeads.Attach(dpHead);
                db.ObjectStateManager.ChangeObjectState(dpHead, EntityState.Modified);
            }
            db.SaveChanges();
            return dpHead.ID;
        }

        public int FinalSaveDispatchDetail(long qcID, string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, string DObject ,string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetail_ForDispatch_Result> fnlSaveLst = new List<WMS_SP_GetPartDetail_ForDispatch_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNameDispatch(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                fnlSaveLst = fnlSaveLst.Where(p => p.QCID == qcID).ToList();
                XElement xmlEle = new XElement("Dispatch", from rec in fnlSaveLst
                                                           select new XElement("PartList",
                                                               new XElement("DispHeadId", paraReferenceID),
                                                               new XElement("ProdID", Convert.ToInt64(rec.Prod_ID)),
                                                               new XElement("DispatchQty", Convert.ToDecimal(rec.DispatchQty)),
                                                               new XElement("ShortQty", Convert.ToDecimal(rec.ShortQty)),
                                                               new XElement("ExcessQty", Convert.ToDecimal(rec.ExcessQty)),
                                                               new XElement("UOMID", Convert.ToInt64(rec.UOMID))));

                ObjectParameter _DispHeadId = new ObjectParameter("DispHeadId", typeof(long));
                _DispHeadId.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _DispHeadId, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotDispatchDetail", obj);
                db.SaveChanges(); Result = 1;

                /*Update Status of tQualityControlHead*/
                if (DObject == "SalesOrder")
                {
                    tQualityControlHead qch = new tQualityControlHead();
                    qch = (from q in db.tQualityControlHeads
                           where q.ID == qcID
                           select q).FirstOrDefault();
                    qch.Status = 40;    //Change Status to Dispatch
                    db.SaveChanges();

                    long pkUpId = long.Parse(qch.OID.ToString());
                    tPickUpHead pkh = new tPickUpHead();
                    pkh = (from pk in db.tPickUpHeads
                           where pk.ID == pkUpId
                           select pk).FirstOrDefault();
                    pkh.Status = 40;
                    db.SaveChanges();

                    long soID = long.Parse(pkh.OID.ToString());
                    DataSet dsDisPrd = new DataSet();
                    dsDisPrd.Reset();
                    string disPrd = "select * from tDispatchDetails where DispHeadId=" + paraReferenceID + "";
                    dsDisPrd = fillds(disPrd, conn);
                    int cnt = dsDisPrd.Tables[0].Rows.Count;
                    if (cnt > 0)
                    {
                        for (int i = 0; i <= cnt - 1; i++)
                        {
                            long prdId = long.Parse(dsDisPrd.Tables[0].Rows[i]["ProdID"].ToString());
                            decimal dispatchQty = decimal.Parse(dsDisPrd.Tables[0].Rows[i]["DispatchQty"].ToString());

                            tOrderDetail od = new tOrderDetail();
                            od = (from o in db.tOrderDetails
                                  where o.OrderHeadId == soID && o.SkuId == prdId
                                  select o).FirstOrDefault();

                            decimal calShipQty = decimal.Parse(od.ShippedQty.ToString()) + dispatchQty;
                            od.ShippedQty = calShipQty;
                            db.SaveChanges();
                        }
                    }

                    long cntShipQty = (from sh in db.tOrderDetails
                                       where sh.OrderHeadId == soID && sh.ShippedQty < sh.OrderQty
                                       select sh).Count();
                    if (cntShipQty == 0)
                    {
                        tOrderHead OH = new tOrderHead();
                        OH = (from or in db.tOrderHeads
                              where or.Id == soID
                              select or).FirstOrDefault();
                        OH.Status = 40;
                        db.SaveChanges();
                    }
                }
                if (DObject == "Transfer")
                {
                    //long soID = long.Parse(pkh.OID.ToString());
                    DataSet dsDisPrd = new DataSet();
                    dsDisPrd.Reset();
                    string disPrd = "select * from tDispatchDetails where DispHeadId=" + paraReferenceID + "";
                    dsDisPrd = fillds(disPrd, conn);
                    int cnt = dsDisPrd.Tables[0].Rows.Count;
                    if (cnt > 0)
                    {
                        for (int i = 0; i <= cnt - 1; i++)
                        {
                            long prdId = long.Parse(dsDisPrd.Tables[0].Rows[i]["ProdID"].ToString());
                            decimal dispatchQty = decimal.Parse(dsDisPrd.Tables[0].Rows[i]["DispatchQty"].ToString());

                            tTransferDetail TD = new tTransferDetail();
                            TD=(from d in db.tTransferDetails
                                where d.TransferID == qcID && d.SkuId == prdId
                                select d).FirstOrDefault();
                            
                            decimal calShipQty = decimal.Parse(TD.ShippedQty.ToString()) + dispatchQty;                            
                            //TD.ShippedQty = calShipQty;                           
                            //db.SaveChanges();

                            decimal calRGQty = decimal.Parse(TD.RemainingGRNQty.ToString()) + dispatchQty;
                            DataSet dsUpdPOD = new DataSet();
                            dsUpdPOD.Reset();
                            string strUpdPOD = "Update tTransferDetail set ShippedQty=" + calShipQty + ", RemainingGRNQty=" + calRGQty + ", ReceivedQty=0 where TransferID=" + qcID + " and SkuId=" + prdId + "";
                            dsUpdPOD = fillds(strUpdPOD, conn);
                        }
                    }
                    long cntShipQty = (from tr in db.tTransferDetails
                                       where tr.TransferID==qcID && tr.ShippedQty < tr.Qty
                                       select tr).Count();
                    if (cntShipQty == 0)
                    {
                        tTransferHead TH = new tTransferHead();
                        TH = (from trh in db.tTransferHeads
                              where trh.ID == qcID
                              select trh).FirstOrDefault();
                        TH.Status = 59;
                        db.SaveChanges();
                    }
                }
            }
            catch { Result = 0; }
            finally { }
            return Result;
        }

        public WMS_VW_GetDispatchDetails GetDispatchDetailsByQCID(long qcID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            WMS_VW_GetDispatchDetails dispatchLst = new WMS_VW_GetDispatchDetails();
            dispatchLst = (from d in db.WMS_VW_GetDispatchDetails
                           where d.OID == qcID
                           select d).FirstOrDefault();
            return dispatchLst;
        }

        public WMS_VW_GetDispatchDetails GetDispatchDetailsByDispatchID(long dispatchID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            WMS_VW_GetDispatchDetails dispatchLst = new WMS_VW_GetDispatchDetails();
            dispatchLst = (from d in db.WMS_VW_GetDispatchDetails
                           where d.ID == dispatchID
                           select d).FirstOrDefault();
            return dispatchLst;
        }

        public DataSet GetDispatchSkuDetailByDispatchID(long dispatchID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select D.ProdID as Prod_ID,ROW_NUMBER() OVER(ORDER BY D.ID ASC) AS Sequence ,D.DispatchQty as QCQty, D.DispatchQty,D.UOMID,D.ShortQty,D.ExcessQty,D.DispHeadId ,P.ProductCode as Prod_Code,p.Name as Prod_Name,p.[Description] as Prod_Description,u.Name UOM from tDispatchDetails D left outer join mProduct P on  D.ProdID=P.ID  left outer join mUOM u on D.UOMID=u.ID  where D.DispHeadId=" + dispatchID + "";
            ds = fillds(str, conn);
            return ds;
        }
        #endregion

        #region Return
        public DataSet BindOutboundGridForReturn(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select OH.Id,OH.Orderdate,'Sales Order' as Type,case when T.JobCardName is null then 'Not Created' else T.JobCardName end as JobCardName , OH.Status,S.Status StatusName,OH.Title,OH.CreatedBy,U.FirstName+' '+U.LastName SOBy,OH.CustomerID,Cl.Name CustomerName, OH.CompanyID,OH.OrderNo ,  Case when Oh.Status=46 then 'red' when OH.Status in (37,38,40,32) then 'green' end as ImgSO,Case When Oh.Status=46 then 'gray' when OH.Status = 37 then 'red' when OH.Status In (38,40,32) then 'green'  end as ImgPickList,Case When OH.Status =38 then 'red' when OH.Status in(37,46) then 'gray' when OH.Status in(32,40) then 'green'  end as ImgQC, Case When OH.Status=32 then 'red' when OH.Status in(37,38,46) then 'gray' when OH.Status=40 then 'green' end as ImgDispatch from tOrderHead OH left outer join mStatus S on OH.Status=S.ID left outer join mUserProfileHead U on OH.CreatedBy=U.ID left outer join mClient Cl on OH.ClientID=CL.ID left outer join mCustomer C on OH.CustomerID=C.ID  left outer join tJobCardDetail J on OH.Id=J.OID and J.OrderObjectName='SalesOrder' left outer join tTaskDetail T on J.TaskID=T.TaskID  where Oh.Status=40 order by OH.Id desc ";
            ds = filldsO(str, conn);
            return ds;
        }

        public int ChangeStatusToMarkForReturn(string SelectedRec, long UserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "update tOrderHead set Status = 49 where Id in(" + SelectedRec + ")";
            ds = filldsO(str, conn);
            return 1;
        }

        public long SaveReturnHead(tReturnHead rHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            db.tReturnHeads.AddObject(rHead);
            db.SaveChanges();
            return rHead.ID;
        }
        #endregion

        #region Transfer
        public List<WMS_SP_GetPartDetail_ForTransfer_Result> GetExistingTempDataBySessionIDObjectNameTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForTransfer_Result> objtAddToCartProductDetailList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<WMS_SP_GetPartDetail_ForTransfer_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public List<WMS_SP_GetPartDetail_ForTransfer_Result> AddPartIntoTransfer_TempDataTR(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForTransfer_Result> existingList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameTR(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            long MaxSequenceNo = 0;
            if (existingList.Count > 0)
            {
                MaxSequenceNo = Convert.ToInt64((from lst in existingList
                                                 select lst.Sequence).Max().Value);
            }
            /*Get Product Details*/
            List<WMS_SP_GetPartDetail_ForTransfer_Result> getnewRec = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            getnewRec = (from view in db.WMS_SP_GetPartDetail_ForTransfer(paraProductIDs, MaxSequenceNo, WarehouseID, 0)
                         orderby view.Sequence
                         select view).ToList();
            /*End*/
            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            List<WMS_SP_GetPartDetail_ForTransfer_Result> mergedList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            mergedList.AddRange(existingList);
            mergedList.AddRange(getnewRec);
            /*End*/
            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDBTR(mergedList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            return mergedList;
        }

        protected void SaveTempDataToDBTR(List<WMS_SP_GetPartDetail_ForTransfer_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBTR(paraSessionID, paraUserID, paraCurrentObjectName, conn);
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

        public void ClearTempDataFromDBTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<WMS_SP_GetPartDetail_ForTransfer_Result> RemovePartFromTransfer_TempDataTR(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<WMS_SP_GetPartDetail_ForTransfer_Result> existingList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNameTR(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Get Filter List [Filter By paraSequence]*/
            List<WMS_SP_GetPartDetail_ForTransfer_Result> filterList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/
            /*Save result to TempData*/
            SaveTempDataToDBTR(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/
            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<WMS_SP_GetPartDetail_ForTransfer_Result> NewList = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            NewList = GetExistingTempDataBySessionIDObjectNameTR(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForTransfer_Result UpdRec = new WMS_SP_GetPartDetail_ForTransfer_Result();

            if (cnt > 0)
            {
                for (int i = paraSequence; i <= cnt; i++)
                {
                    UpdRec = NewList.Where(u => u.Sequence == i + 1).FirstOrDefault();
                    UpdRec.Sequence = i;
                    SaveTempDataToDBTR(NewList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
                }
            }
            /*End*/
            if (cnt > 0)
            { return NewList; }
            else { return filterList; }
        }

        public List<WMS_SP_GetPartDetail_ForTransfer_Result> GetTransferPartDetailByTransferID(long RequestID, long WarehouseID, string sessionID, string userID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForTransfer_Result> PartDetail = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            PartDetail = (from sp in db.WMS_SP_GetPartDetail_ForTransfer("0", 0, WarehouseID, RequestID)
                          select sp).ToList();
            SaveTempDataToDBTR(PartDetail, sessionID, userID, CurrentObject, conn);
            return PartDetail;
        }

        public tTransferHead GetTransferHeadDetailByTransferID(long TRID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tTransferHead th = new tTransferHead();
            th = (from t in db.tTransferHeads
                  where t.ID == TRID
                  select t).FirstOrDefault();
            return th;
        }

        public void UpdateTransfer_TempDataTR(string SessionID, string CurrentObjectName, string UserID, WMS_SP_GetPartDetail_ForTransfer_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<WMS_SP_GetPartDetail_ForTransfer_Result> getRec = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
            getRec = GetExistingTempDataBySessionIDObjectNameTR(SessionID, UserID, CurrentObjectName, conn);

            WMS_SP_GetPartDetail_ForTransfer_Result updateRec = new WMS_SP_GetPartDetail_ForTransfer_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.Qty = Request.Qty;
            updateRec.UOMID = Request.UOMID;

            SaveTempDataToDBTR(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public long SaveIntotTransferHead(tTransferHead TRHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (TRHead.ID == 0)
            {
                db.tTransferHeads.AddObject(TRHead);
            }
            else
            {
                db.tTransferHeads.Attach(TRHead);
                db.ObjectStateManager.ChangeObjectState(TRHead, EntityState.Modified);
            }
            db.SaveChanges();
            return TRHead.ID;
        }

        public int FinalSaveTRDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            int Result = 0;
            try
            {
                List<WMS_SP_GetPartDetail_ForTransfer_Result> fnlSaveLst = new List<WMS_SP_GetPartDetail_ForTransfer_Result>();
                fnlSaveLst = GetExistingTempDataBySessionIDObjectNameTR(paraSessionID, paraUserID, paraCurrentObjectName, conn);

                XElement xmlEle = new XElement("TR", from rec in fnlSaveLst
                                                     select new XElement("PartList",
                                                         new XElement("TransferID", paraReferenceID),
                                                       new XElement("SkuId", Convert.ToInt64(rec.Prod_ID)),
                                                       new XElement("Qty", Convert.ToDecimal(rec.Qty)),
                                                       new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                       new XElement("Sequence", Convert.ToInt64(rec.Sequence)),
                                                       new XElement("RemaningQty", Convert.ToDecimal(rec.Qty)),
                                                       new XElement("ShippedQty", 0),
                                                       new XElement("RemainingGRNQty",0),
                                                       new XElement("ReceivedQty",0)
                                                       ));

                ObjectParameter _TransferID = new ObjectParameter("TransferID", typeof(long));
                _TransferID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter[] obj = new ObjectParameter[] { _TransferID, _xmlData };
                db.ExecuteFunction("WMS_SP_InsertIntotTransferDetail", obj);
                db.SaveChanges(); Result = 1;
            }
            catch (System.Exception ex)
            {               
                    long OrderID = paraReferenceID;
                    //RollBack(OrderID, conn);
                    Result = 0;                
            }
            finally { }
            return Result;
        }
        #endregion

        #region Report

        public List<mCustomer> GetCustomerByCompanyID(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCustomer> custLst = new List<mCustomer>();
            custLst = (from c in db.mCustomers
                       where c.ParentID == CompanyID
                       select c).ToList();
            return custLst;
        }

        #endregion
    }
}
