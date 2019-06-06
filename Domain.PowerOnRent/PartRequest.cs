using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.Data;
using Domain.Tempdata;
using System.Xml.Linq;
using System.Data.Objects;
//using System.Web.Mail;
using System.Net;
using Interface.PowerOnRent;
using System.Net.Mail;
using System.Data.SqlClient;
namespace Domain.PowerOnRent
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class PartRequest : iPartRequest
    {
        Domain.Server.Server svr = new Server.Server();
        DataHelper datahelper = new DataHelper();

        #region Part Request Head
        public PORtPartRequestHead GetRequestHeadByRequestID(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            PORtPartRequestHead PartReq = new PORtPartRequestHead();
            PartReq = db.PORtPartRequestHeads.Where(r => r.PRH_ID == RequestID).FirstOrDefault();
            db.PORtPartRequestHeads.Detach(PartReq);
            return PartReq;
        }

        public tOrderHead GetOrderHeadByOrderID(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead PartReq = new tOrderHead();
            PartReq = db.tOrderHeads.Where(r => r.Id == OrderID).FirstOrDefault();
            db.tOrderHeads.Detach(PartReq);
            return PartReq;
        }

        public long SetIntoPartRequestHead(PORtPartRequestHead PartRequest, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (PartRequest.PRH_ID == 0)
            {
                db.PORtPartRequestHeads.AddObject(PartRequest);
            }
            else
            {
                db.PORtPartRequestHeads.Attach(PartRequest);
                db.ObjectStateManager.ChangeObjectState(PartRequest, EntityState.Modified);
            }
            db.SaveChanges();
            return PartRequest.PRH_ID;
        }

        public List<mStatu> GetStatusListForRequest(string Remark, string state, long UserID, string[] conn)
        {
            List<mStatu> statusdetail = new List<mStatu>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                string[] RemarkArr = Remark.Split(',');
                if (Remark != "")
                {
                    statusdetail = (from st in db.mStatus
                                    where (st.ObjectName == "MaterialRequest" && RemarkArr.Contains(st.Remark))
                                    select st).OrderBy(st => st.Sequence).ToList();
                }
                else
                {
                    statusdetail = (from st in db.mStatus
                                    where (st.ObjectName == "MaterialRequest")
                                    select st).OrderBy(st => st.Sequence).ToList();
                }

                //if (state == "Add" || state == "Edit")
                //{
                //    mUserRolesDetail RoleDetail = new mUserRolesDetail();
                //    RoleDetail = db.mUserRolesDetails.Where(r => r.UserID == UserID && r.ObjectName == "MaterialRequest").FirstOrDefault();
                //    if (RoleDetail != null)
                //    {
                //        if (RoleDetail.Approval == false)
                //        {
                //            statusdetail = statusdetail.Where(s => s.ID != 3 && s.ID != 4 && s.ID != 21 && s.ID != 22 && s.ID != 24).ToList();

                //        }
                //    }
                //}
            }
            catch { }
            finally { }
            return statusdetail;
        }
        #endregion

        #region Request Part Detail
        public List<POR_SP_GetPartDetail_ForRequest_Result> GetRequestPartDetailByRequestID(long RequestID, long siteID, string sessionID, string userID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> PartDetail = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            PartDetail = (from sp in db.POR_SP_GetPartDetail_ForRequest("0", 0, siteID, RequestID)
                          select sp).ToList();
            SaveTempDataToDB(PartDetail, sessionID, userID, CurrentObject, conn);
            return PartDetail;
        }

        protected void SaveTempDataToDB(List<POR_SP_GetPartDetail_ForRequest_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<POR_SP_GetPartDetail_ForRequest_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> objtAddToCartProductDetailList = new List<POR_SP_GetPartDetail_ForRequest_Result>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<POR_SP_GetPartDetail_ForRequest_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public List<POR_SP_GetPartDetail_ForRequest_Result> AddPartIntoRequest_TempData(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, long SiteID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<POR_SP_GetPartDetail_ForRequest_Result> existingList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            existingList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            long MaxSequenceNo = 0;
            if (existingList.Count > 0)
            {
                MaxSequenceNo = Convert.ToInt64((from lst in existingList
                                                 select lst.Sequence).Max().Value);
            }

            /*Get Product Details*/
            List<POR_SP_GetPartDetail_ForRequest_Result> getnewRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getnewRec = (from view in db.POR_SP_GetPartDetail_ForRequest(paraProductIDs, MaxSequenceNo, SiteID, 0)
                         orderby view.Sequence
                         select view).ToList();
            /*End*/

            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            List<POR_SP_GetPartDetail_ForRequest_Result> mergedList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            mergedList.AddRange(existingList);
            mergedList.AddRange(getnewRec);
            /*End*/

            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDB(mergedList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return mergedList;
        }

        public List<POR_SP_GetPartDetail_ForRequest_Result> RemovePartFromRequest_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<POR_SP_GetPartDetail_ForRequest_Result> existingList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            existingList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Get Filter List [Filter By paraSequence]*/
            List<POR_SP_GetPartDetail_ForRequest_Result> filterList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            filterList = (from exist in existingList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/

            /*Save result to TempData*/
            SaveTempDataToDB(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Newly Added Code By Suresh For Update Sequene No After Remove Paart From List*/
            int cnt = filterList.Count;
            List<POR_SP_GetPartDetail_ForRequest_Result> NewList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            NewList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result UpdRec = new POR_SP_GetPartDetail_ForRequest_Result();

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
            else
            { return filterList; }
        }

        public void UpdatePartRequest_TempData(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> getRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result updateRec = new POR_SP_GetPartDetail_ForRequest_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }
        public void UpdatePartRequest_TempData1(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> getRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result updateRec = new POR_SP_GetPartDetail_ForRequest_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty; //updateRec.UOM = Request.UOM;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }

        public void UpdatePartRequest_TempData12(string SessionID, string CurrentObjectName, string UserID, POR_SP_GetPartDetail_ForRequest_Result Request, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> getRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result updateRec = new POR_SP_GetPartDetail_ForRequest_Result();
            updateRec = getRec.Where(g => g.Sequence == Request.Sequence).FirstOrDefault();

            updateRec.RequestQty = Request.RequestQty; updateRec.UOM = Request.UOM;
            updateRec.UOMID = Request.UOMID;
            updateRec.Total = Request.Total;
            updateRec.Price = Request.Price;
            updateRec.IsPriceChange = Request.IsPriceChange;
            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
        }
        public int FinalSaveRequestPartDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, long PreviousStatusID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var torderdetails = 0;
            int Result = 0;
            try
            {
                if (PreviousStatusID == 2)
                {
                    tOrderDetailHistory OD = new tOrderDetailHistory();
                    DataSet dsOldProducts = new DataSet();
                    dsOldProducts = fillds(" Select * from tOrderDetail where OrderHeadId=" + paraReferenceID + "", conn);
                    int CntPrds = dsOldProducts.Tables[0].Rows.Count;
                    if (CntPrds > 0)
                    {
                        for (int i = 0; i < CntPrds - 1; i++)
                        {
                            OD.OrderHeadId = paraReferenceID;
                            OD.SkuId = Convert.ToInt64(dsOldProducts.Tables[0].Rows[i]["SkuId"].ToString());
                            OD.OrderQty = Convert.ToDecimal(dsOldProducts.Tables[0].Rows[i]["OrderQty"].ToString());
                            OD.UOMID = Convert.ToInt64(dsOldProducts.Tables[0].Rows[i]["UOMID"].ToString());
                            OD.Sequence = Convert.ToInt64(dsOldProducts.Tables[0].Rows[i]["Sequence"].ToString());
                            OD.Prod_Name = dsOldProducts.Tables[0].Rows[i]["Prod_Name"].ToString();
                            OD.Prod_Description = dsOldProducts.Tables[0].Rows[i]["Prod_Description"].ToString();
                            OD.Prod_Code = dsOldProducts.Tables[0].Rows[i]["Prod_Code"].ToString();
                            OD.RemaningQty = Convert.ToDecimal(dsOldProducts.Tables[0].Rows[i]["RemainingQty"].ToString());

                            db.tOrderDetailHistories.Attach(OD);
                            db.SaveChanges();
                        }
                    }
                }

                List<POR_SP_GetPartDetail_ForRequest_Result> finalSaveLst = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

                XElement xmlEle = new XElement("Request", from rec in finalSaveLst
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
                                                          new XElement("Prod_Code", rec.Prod_Code)));

                ObjectParameter _PRH_ID = new ObjectParameter("PRH_ID", typeof(long));
                _PRH_ID.Value = paraReferenceID;

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();


                ObjectParameter[] obj = new ObjectParameter[] { _PRH_ID, _xmlData };
                //db.ExecuteFunction("POR_SP_InsertIntoPORtPartRequestDetail", obj);
                db.ExecuteFunction("POR_SP_InsertIntotOrderDetail", obj);

                db.SaveChanges(); torderdetails = 1; Result = 1;

                /*Add Record Of User into table tOrderWiseAccess*/
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

                    tOrderWiseAccess ODA = new tOrderWiseAccess();
                    ODA.UserApproverID = long.Parse(paraUserID);
                    ODA.ApprovalLevel = 0;
                    if (IsPriceChenged == 1)
                    {
                        ODA.PriceEdit = false;
                        ODA.SkuQtyEdit = false;
                    }
                    else
                    {
                        ODA.PriceEdit = false;
                        ODA.SkuQtyEdit = true;
                    }
                    ODA.UserType = "User";
                    ODA.OrderID = paraReferenceID;
                    ODA.Date = DateTime.Now;
                    db.AddTotOrderWiseAccesses(ODA);
                    db.SaveChanges();
                }
                /*Add Record Of User into table tOrderWiseAccess*/
                if (StatusID == 1) { }
                else
                {
                    tOrderDetail ODT = new tOrderDetail();
                    ODT = db.tOrderDetails.Where(r => r.OrderHeadId == paraReferenceID).FirstOrDefault();
                    if (ODT != null)
                    {
                        Result = SetApproverDataafterSave(paraCurrentObjectName, paraReferenceID, paraUserID, StatusID, DepartmentID, PreviousStatusID, conn);
                    }
                    else
                    {
                        long OrderID = paraReferenceID;
                        RollBack(OrderID, conn);
                        Result = 0;
                    }
                }
                ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
                /*if (result == finalSaveLst.Count)
                {
               
                }*/
            }
            catch (System.Exception ex)
            {
                if (torderdetails == 0)
                {
                    long OrderID = paraReferenceID;
                    RollBack(OrderID, conn);
                    Result = 0;
                }
            }
            finally { }
            return Result;
        }

        public int SetApproverDataafterSave(string paraCurrentObjectName, long paraReferenceID, string paraUserID, int StatusID, long DepartmentID, long PreviousStatusID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            var ApprovalDetail = 0;
            int Rslt = 0, Suecc = 0;
            try
            {
                if (PreviousStatusID == 2) { }
                else
                {
                    if (StatusID == 2)
                    {
                        /* Insert record of Approval Level 1 in tApprovalTrans Table===>>>> */

                        /* If Price is change of product then add Financial Approver at Approval Level 1  START*/
                        /*Check if Price is Changed or not*/
                        int IsPriceChenged = 0;
                        DataSet dsIsPriceChange = new DataSet();
                        dsIsPriceChange = fillds("select * from torderdetail where orderheadid=" + paraReferenceID + " and IsPriceChange=1", conn);
                        int rowcount = dsIsPriceChange.Tables[0].Rows.Count;
                        if (rowcount > 0)
                        {
                            IsPriceChenged = 1; long FinanAppId = 0;
                            DataSet dsFinanAppID = new DataSet();
                            dsFinanAppID = fillds("select FinApproverID from mterritory where id=" + DepartmentID + "", conn);
                            FinanAppId = Convert.ToInt64(dsFinanAppID.Tables[0].Rows[0]["FinApproverID"].ToString());

                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_Insert_tapprovaltrans";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("OrderId", paraReferenceID);
                            cmd.Parameters.AddWithValue("StoreId", DepartmentID);
                            cmd.Parameters.AddWithValue("UserId", Convert.ToInt64(paraUserID));
                            cmd.Parameters.AddWithValue("ApprovalId", 1);
                            cmd.Parameters.AddWithValue("ApproverID", FinanAppId);
                            cmd.Parameters.AddWithValue("Status", StatusID);
                            cmd.ExecuteNonQuery();

                            Rslt = EmailSendToApprover(FinanAppId, paraReferenceID, conn);

                            /*Add Record Of User into table tOrderWiseAccess*/
                            tOrderWiseAccess ODA = new tOrderWiseAccess();
                            ODA.UserApproverID = FinanAppId;
                            ODA.ApprovalLevel = 1;
                            ODA.PriceEdit = true;
                            ODA.SkuQtyEdit = false;
                            ODA.UserType = "Financial Approver";
                            ODA.OrderID = paraReferenceID;
                            ODA.Date = DateTime.Now;
                            ODA.ApproverLogic = "AND";
                            db.AddTotOrderWiseAccesses(ODA);
                            db.SaveChanges();
                            /*Add Record Of User into table tOrderWiseAccess*/
                            AddAllApprovalLevel(IsPriceChenged, paraReferenceID, DepartmentID, conn);
                        }
                        else
                        {
                            IsPriceChenged = 0;
                            /* If Price is change of product then add Financial Approver at Approval Level 1  END*/
                            /*Add Record Of User into table tOrderWiseAccess*/
                            AddAllApprovalLevel(IsPriceChenged, paraReferenceID, DepartmentID, conn);

                            /*New Code After tOrderWiseAccess able added for order wise approval level start*/
                            DataSet dsFirstApprover = new DataSet();
                            dsFirstApprover = fillds("select OW.ID,OW.UserApproverID,OW.ApprovalLevel,OW.PriceEdit,OW.SkuQtyEdit,OW.UserType,OW.OrderID,OW.ApproverLogic ,Dl.DeligateTo from tOrderWiseAccess  OW left outer join tOrderHead OH on OW.OrderID=OH.ID left outer join mDeligate AS Dl ON OW.UserApproverID = Dl.DeligateFrom and CONVERT(VARCHAR(10), GETDATE(), 111) <=Convert(VARCHAR(10), Dl.ToDate,111) and CONVERT(VARCHAR(10), GETDATE(), 111) >=Convert(VARCHAR(10), Dl.FromDate,111)  and OH.StoreId=Dl.DeptID where OW.OrderID=" + paraReferenceID + " and OW.UserType != 'User' and OW.ApprovalLevel=1", conn);
                            int CntFirstApprover = dsFirstApprover.Tables[0].Rows.Count;
                            if (CntFirstApprover > 0)
                            {
                                for (int i = 0; i <= CntFirstApprover - 1; i++)
                                {
                                    SqlCommand cmd1 = new SqlCommand();
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.CommandText = "SP_Insert_tapprovaltrans";
                                    cmd1.Connection = svr.GetSqlConn(conn);
                                    cmd1.Parameters.Clear();
                                    cmd1.Parameters.AddWithValue("OrderId", paraReferenceID);
                                    cmd1.Parameters.AddWithValue("StoreId", DepartmentID);
                                    cmd1.Parameters.AddWithValue("UserId", Convert.ToInt64(paraUserID));
                                    cmd1.Parameters.AddWithValue("ApprovalId", 1);
                                    cmd1.Parameters.AddWithValue("ApproverID", Convert.ToInt64(dsFirstApprover.Tables[0].Rows[i]["UserApproverID"].ToString()));
                                    cmd1.Parameters.AddWithValue("Status", StatusID);
                                    cmd1.ExecuteNonQuery();

                                    ApprovalDetail = 1;

                                    /*Send Email to Approvers*/
                                    Rslt = EmailSendToApprover(Convert.ToInt64(dsFirstApprover.Tables[0].Rows[i]["UserApproverID"].ToString()), paraReferenceID, conn);
                                }
                            }
                        }
                        /*New Code After tOrderWiseAccess able added for order wise approval level end*/

                        tApprovalTran APRT = new tApprovalTran();
                        APRT = db.tApprovalTrans.Where(rec => rec.OrderId == paraReferenceID).FirstOrDefault();
                        if (APRT != null)
                        {

                            Rslt = EmailSendWhenRequestSubmit(paraReferenceID, conn); //if Rslt =2 then mail sent to requestor else if Rslt=3 then mail not sent to requestoe
                            /*Insert record of Auto Cancellation Reminder & Approval reminder in tCorrespond table START*/
                            mTerritory Dept = new mTerritory();
                            Dept = db.mTerritories.Where(r => r.ID == DepartmentID).FirstOrDefault();
                            long OrderCancelDays = 0, ApprovalReminderSchedule = 0, AutoCancelReminderSchedule = 0;
                            if (Dept != null)
                            {
                                OrderCancelDays = long.Parse(Dept.cancelDays.ToString());
                                ApprovalReminderSchedule = long.Parse(Dept.ApproRemSchedul.ToString());
                                AutoCancelReminderSchedule = long.Parse(Dept.AutoRemSchedule.ToString());
                            }

                            DataSet dsGetOrderDate = new DataSet();
                            dsGetOrderDate = fillds("select OrderDate from torderhead where id=" + paraReferenceID + "", conn);
                            DateTime OrdrDate = Convert.ToDateTime(dsGetOrderDate.Tables[0].Rows[0]["OrderDate"].ToString());

                            DataSet dsAutocancel = new DataSet();
                            dsAutocancel = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=10) and MessageID=(select Id from mDropdownValues where Value='Reminder') and DepartmentID=" + DepartmentID + " ", conn);
                            if (dsAutocancel.Tables[0].Rows.Count > 0)
                            {
                                tCorrespond Cor = new tCorrespond();
                                Cor.OrderHeadId = paraReferenceID;
                                Cor.MessageTitle = dsAutocancel.Tables[0].Rows[0]["MailSubject"].ToString();
                                Cor.Message = dsAutocancel.Tables[0].Rows[0]["MailBody"].ToString();
                                Cor.date = DateTime.Now;
                                Cor.MessageSource = "Scheduler";
                                Cor.MessageType = "Reminder";
                                Cor.DepartmentID = DepartmentID;
                                // Cor.OrderDate = DateTime.Now;
                                Cor.OrderDate = OrdrDate;
                                Cor.CurrentOrderStatus = StatusID;
                                Cor.MailStatus = 0;
                                Cor.OrderCancelDays = OrderCancelDays;
                                Cor.AutoCancelReminderSchedule = AutoCancelReminderSchedule;
                                //Cor.ApprovalReminderSchedule = ApprovalReminderSchedule;
                                Cor.NxtAutoCancelReminderDate = DateTime.Now.AddDays(AutoCancelReminderSchedule);
                                Cor.Archive = false;

                                db.tCorresponds.AddObject(Cor);
                                db.SaveChanges();
                            }

                            DataSet dsApprovalReminder = new DataSet();
                            dsApprovalReminder = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2 and value='Order Submit') and MessageID=(select Id from mDropdownValues where Value='Reminder') and DepartmentID=" + DepartmentID + " ", conn);
                            if (dsApprovalReminder.Tables[0].Rows.Count > 0)
                            {
                                tCorrespond Cor = new tCorrespond();
                                Cor.OrderHeadId = paraReferenceID;
                                Cor.MessageTitle = dsApprovalReminder.Tables[0].Rows[0]["MailSubject"].ToString();
                                Cor.Message = dsApprovalReminder.Tables[0].Rows[0]["MailBody"].ToString();
                                Cor.date = DateTime.Now;
                                Cor.MessageSource = "Scheduler";
                                Cor.MessageType = "Reminder";
                                Cor.DepartmentID = DepartmentID;
                                Cor.OrderDate = OrdrDate; //DateTime.Now;
                                Cor.CurrentOrderStatus = StatusID;
                                Cor.MailStatus = 0;
                                // Cor.OrderCancelDays = OrderCancelDays;
                                // Cor.AutoCancelReminderSchedule = AutoCancelReminderSchedule;
                                Cor.ApprovalReminderSchedule = ApprovalReminderSchedule;
                                Cor.NxtApprovalReminderDate = DateTime.Now.AddDays(ApprovalReminderSchedule);
                                Cor.Archive = false;

                                db.tCorresponds.AddObject(Cor);
                                db.SaveChanges();
                            }


                            /* Update tProductstockDetails Reserve Quantity & Available Balance START>>>>>>>> */
                            UpdateTProductStockReserveQtyAvailBalance(paraReferenceID, conn);
                            /* <<<<<<<<Update tProductstockDetails Reserve Quantity & Available Balance END */
                            Suecc = 1;
                        }
                        else
                        {
                            long OrdrID = paraReferenceID;
                            RollBack(OrdrID, conn);
                            Suecc = 0;
                        }

                    }
                    else if (StatusID == 3)
                    {
                        //Add Message into mMessageTrans Table After Approve Order
                        AddIntomMessageTrans(paraReferenceID, conn);
                    }
                    else
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (ApprovalDetail == 0)
                {
                    long OrdrID = paraReferenceID;
                    RollBack(OrdrID, conn);
                }
            }
            finally { }

            //ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            return Suecc;
        }

        protected void AddAllApprovalLevel(int IsPriceChenged, long paraReferenceID, long DepartmentID, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                DataSet dsApproverUserID = new DataSet();
                dsApproverUserID = fillds("select VWA.ID,VWA.ApprovalLevelID,VWA.UserID,VWA.ApproverName,VWA.EmailID,VWA.CompanyID,VWA.DepartmentID,VWA.ApproverLogic,VWA.ApprovalLevel,dl.DeligateTo from VW_ApprovalLevelDetail VWA left outer join mDeligate AS Dl ON VWA.UserID = Dl.DeligateFrom AND CONVERT(VARCHAR(10), GETDATE(), 111) <=Convert(VARCHAR(10), Dl.ToDate,111) and  CONVERT(VARCHAR(10), GETDATE(), 111) >=Convert(VARCHAR(10), Dl.FromDate,111) AND VWA.DepartmentID = Dl.DeptID where VWA.DepartmentID=" + DepartmentID + " order by VWA.ApprovalLevel", conn);
                int cnt = dsApproverUserID.Tables[0].Rows.Count;
                if (cnt > 0)
                {
                    for (int i = 0; i <= cnt - 1; i++)
                    {
                        int AppLvl = Convert.ToInt16(dsApproverUserID.Tables[0].Rows[i]["ApprovalLevel"].ToString());
                        if (IsPriceChenged == 1) { AppLvl = AppLvl + 1; }

                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandText = "SP_Insert_tOrderWiseAccess";
                        cmd2.Connection = svr.GetSqlConn(conn);
                        cmd2.Parameters.Clear();
                        cmd2.Parameters.AddWithValue("UserApproverID", Convert.ToInt64(dsApproverUserID.Tables[0].Rows[i]["UserID"].ToString()));
                        cmd2.Parameters.AddWithValue("ApprovalLevel", AppLvl);
                        cmd2.Parameters.AddWithValue("PriceEdit", false);
                        cmd2.Parameters.AddWithValue("SkuQtyEdit", true);
                        cmd2.Parameters.AddWithValue("UserType", "General Approver");
                        cmd2.Parameters.AddWithValue("OrderID", paraReferenceID);
                        cmd2.Parameters.AddWithValue("ApproverLogic", dsApproverUserID.Tables[0].Rows[i]["ApproverLogic"].ToString());
                        cmd2.ExecuteNonQuery();
                    }
                }

                //SqlCommand cmd3 = new SqlCommand();
                //cmd3.CommandType = CommandType.StoredProcedure;
                //cmd3.CommandText = "SP_PaymentMethodFOC";
                //cmd3.Connection = svr.GetSqlConn(conn);
                //cmd3.Parameters.Clear();
                //cmd3.Parameters.AddWithValue("OrderID", paraReferenceID);
                //cmd3.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Sp_EnterErrorTracking";
                cmd.Connection = svr.GetSqlConn(conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("Data", ex.Data.ToString());
                cmd.Parameters.AddWithValue("GetType", ex.Source.ToString());
                cmd.Parameters.AddWithValue("InnerException", "Error");
                cmd.Parameters.AddWithValue("Message", ex.Message.ToString());
                cmd.Parameters.AddWithValue("Source", "AddAllApprovalLevel");
                cmd.Parameters.AddWithValue("DateTime", DateTime.Now);
                cmd.Parameters.AddWithValue("UserID", 123);
                cmd.ExecuteNonQuery();
            }
            finally { }

        }

        protected void RollBack(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
                /*Update tProductStockDetails Start*/
                DataSet dsPrdDetails = new DataSet();
                dsPrdDetails = fillds("Select * from tOrderDetail where OrderHeadId=" + OrderID + "", conn);
                int PrdCnt = dsPrdDetails.Tables[0].Rows.Count;
                //if (PrdCnt > 0)
                //{
                //    for (int i = 0; i <= PrdCnt - 1; i++)
                //    {
                //        long SkuID = long.Parse(dsPrdDetails.Tables[0].Rows[i]["SkuId"].ToString());
                //        decimal Qty = decimal.Parse(dsPrdDetails.Tables[0].Rows[i]["OrderQty"].ToString());

                //        mProduct prd = new mProduct();
                //        prd = db.mProducts.Where(p => p.ID == SkuID).FirstOrDefault();
                //        if (prd.GroupSet == "Yes")
                //        {
                //            DataSet dsBomProds = new DataSet();
                //            dsBomProds = fillds("select SKUId,Quantity from mBOMDetail BD where BD.BOMheaderId=" + SkuID + "", conn);
                //            if (dsBomProds.Tables[0].Rows.Count > 0)
                //            {
                //                for (int b = 0; b <= dsBomProds.Tables[0].Rows.Count - 1; b++)
                //                {
                //                    long bomPrd = long.Parse(dsBomProds.Tables[0].Rows[b]["SKUId"].ToString());
                //                    decimal bomQty = decimal.Parse(dsBomProds.Tables[0].Rows[b]["Quantity"].ToString());

                //                    decimal FinalQty = Qty * bomQty;

                //                    SqlCommand cmd = new SqlCommand();
                //                    cmd.CommandType = CommandType.StoredProcedure;
                //                    cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQtyRej";
                //                    cmd.Connection = svr.GetSqlConn(conn);
                //                    cmd.Parameters.Clear();
                //                    cmd.Parameters.AddWithValue("SkuID", bomPrd);
                //                    cmd.Parameters.AddWithValue("Qty", FinalQty);
                //                    cmd.ExecuteNonQuery();
                //                }
                //            }
                //        }
                //        else if (prd.GroupSet == "No")
                //        {
                //            SqlCommand cmd = new SqlCommand();
                //            cmd.CommandType = CommandType.StoredProcedure;
                //            cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQtyRej";
                //            cmd.Connection = svr.GetSqlConn(conn);
                //            cmd.Parameters.Clear();
                //            cmd.Parameters.AddWithValue("SkuID", SkuID);
                //            cmd.Parameters.AddWithValue("Qty", Qty);
                //            cmd.ExecuteNonQuery();
                //        }
                //    }
                //}
                /*Update tProductStockDetails End*/

                /* Delete Record From tCorrespond,tApprovalTrans,tOrderDetail,tOrderHead Start*/
                SqlCommand cmdR = new SqlCommand();
                cmdR.CommandType = CommandType.StoredProcedure;
                cmdR.CommandText = "GWC_SP_RollBack";
                cmdR.Connection = svr.GetSqlConn(conn);
                cmdR.Parameters.Clear();
                cmdR.Parameters.AddWithValue("OrderID", OrderID);
                cmdR.ExecuteNonQuery();
                /* Delete Record From tCorrespond,tApprovalTrans,tOrderDetail,tOrderHead End*/
            }
            catch (System.Exception ex) { }
            finally { }
        }

        protected void UpdateTProductStockReserveQtyAvailBalance(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet dsHstry = new DataSet();
            dsHstry = fillds(" select * from tOrderDetailHistory where OrderHeadId=" + RequestID + "", conn);
            int CntHstry = dsHstry.Tables[0].Rows.Count;
            if (CntHstry > 0)
            {
                for (int j = 0; j < CntHstry - 1; j++)
                {
                    long HstrySkuID = long.Parse(dsHstry.Tables[0].Rows[j]["SkuId"].ToString());
                    decimal HstryQty = decimal.Parse(dsHstry.Tables[0].Rows[j]["OrderQty"].ToString());

                    decimal FnlQty = 0;
                    DataSet dsNewPrdLst = new DataSet();
                    dsNewPrdLst = fillds("Select * from tOrderDetail where OrderHeadId=" + RequestID + " and SkuId=" + HstrySkuID + "", conn);
                    if (dsNewPrdLst.Tables[0].Rows.Count > 0)
                    {
                        decimal NewAdSkuQty = decimal.Parse(dsNewPrdLst.Tables[0].Rows[0]["OrderQty"].ToString());

                        FnlQty = HstryQty - NewAdSkuQty;
                    }

                    tProductStockDetail psd1 = new tProductStockDetail();
                    psd1 = db.tProductStockDetails.Where(a => a.ProdID == HstrySkuID).FirstOrDefault();
                    if (psd1 != null)
                    {
                        db.tProductStockDetails.Detach(psd1);
                        psd1.ResurveQty = psd1.ResurveQty + FnlQty;
                        psd1.AvailableBalance = psd1.AvailableBalance - FnlQty;
                        db.tProductStockDetails.Attach(psd1);
                        db.ObjectStateManager.ChangeObjectState(psd1, EntityState.Modified);
                        db.SaveChanges();
                    }

                }
            }
            else
            {
                DataSet dsPrdDetails = new DataSet();
                dsPrdDetails = fillds("Select * from tOrderDetail where OrderHeadId=" + RequestID + "", conn);
                int PrdCnt = dsPrdDetails.Tables[0].Rows.Count;
                if (PrdCnt > 0)
                {
                    for (int i = 0; i <= PrdCnt - 1; i++)
                    {
                        //update tProductstockDetails set ResurveQty=ResurveQty+@Qty,AvailableBalance=AvailableBalance-@Qty where ProdID=@Prd ;

                        //tProductStockDetail psd = new tProductStockDetail();
                        long SkuID = long.Parse(dsPrdDetails.Tables[0].Rows[i]["SkuId"].ToString());
                        decimal Qty = decimal.Parse(dsPrdDetails.Tables[0].Rows[i]["OrderQty"].ToString());
                        //psd = db.tProductStockDetails.Where(a => a.ProdID == SkuID).FirstOrDefault();
                        //if (psd != null)
                        //{
                        //    db.tProductStockDetails.Detach(psd);
                        //    psd.ResurveQty = psd.ResurveQty + Qty;
                        //    psd.AvailableBalance = psd.AvailableBalance - Qty;
                        //    db.tProductStockDetails.Attach(psd);
                        //    db.ObjectStateManager.ChangeObjectState(psd, EntityState.Modified);
                        //    db.SaveChanges();
                        //}

                        //SqlCommand cmd = new SqlCommand();
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQty";
                        //cmd.Connection = svr.GetSqlConn(conn);
                        //cmd.Parameters.Clear();
                        //cmd.Parameters.AddWithValue("SkuID", SkuID);
                        //cmd.Parameters.AddWithValue("Qty", Qty);                        
                        //cmd.ExecuteNonQuery();
                        mProduct prd = new mProduct();
                        prd = db.mProducts.Where(p => p.ID == SkuID).FirstOrDefault();
                        if (prd.GroupSet == "Yes")
                        {
                            DataSet dsBomProds = new DataSet();
                            dsBomProds = fillds("select SKUId,Quantity from mBOMDetail BD where BD.BOMheaderId=" + SkuID + "", conn);
                            if (dsBomProds.Tables[0].Rows.Count > 0)
                            {
                                for (int b = 0; b <= dsBomProds.Tables[0].Rows.Count - 1; b++)
                                {
                                    long bomPrd = long.Parse(dsBomProds.Tables[0].Rows[b]["SKUId"].ToString());
                                    decimal bomQty = decimal.Parse(dsBomProds.Tables[0].Rows[b]["Quantity"].ToString());

                                    decimal FinalQty = Qty * bomQty;

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQty";
                                    cmd.Connection = svr.GetSqlConn(conn);
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("SkuID", bomPrd);
                                    cmd.Parameters.AddWithValue("Qty", FinalQty);
                                    cmd.ExecuteNonQuery();

                                    // InsertIntotInventory(bomPrd, RequestID, FinalQty, "Dispatch", conn);
                                }
                            }
                        }
                        else if (prd.GroupSet == "No")
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQty";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("SkuID", SkuID);
                            cmd.Parameters.AddWithValue("Qty", Qty);
                            cmd.ExecuteNonQuery();

                            //InsertIntotInventory(SkuID, RequestID, Qty, "Dispatch", conn);
                        }
                    }
                }
            }
        }

        #endregion

        #region GridRequestList Summary
        public List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayByUserID(long UserID, string[] conn)
        {
            List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestBySiteIDsOrUserID("0", UserID).OrderByDescending(o => o.ID).ToList();

                RequestList = RequestList.Where(s => s.Status != 1).Union(RequestList.Where(k => k.Status == 1 && k.RequestBy == UserID)).ToList();

                RequestList = RequestList.OrderByDescending(s => s.ID).ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }

        public List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayByUserIDIssue(long UserID, string[] conn)
        {
            List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestBySiteIDsOrUserID("0", UserID).OrderByDescending(o => o.ID).ToList();

                RequestList = RequestList.Where(r => r.ImgIssue == "green").ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }

        public List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayOfUser(long UserID, string[] conn)
        {
            List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestBySiteIDsOrUserID("0", UserID).OrderByDescending(o => o.ID).ToList();

                RequestList = RequestList.Where(r => r.RequestBy == UserID).ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }

        public List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayOfUserIssue(long UserID, string[] conn)
        {
            List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestBySiteIDsOrUserID("0", UserID).OrderByDescending(o => o.ID).ToList();

                RequestList = RequestList.Where(r => r.ImgIssue == "green").ToList();
                RequestList = RequestList.Where(r => r.RequestBy == UserID).ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }


        public List<POR_SP_GetRequestBySiteIDsOrUserID_Result> GetRequestSummayBySiteIDs(string SiteIDs, string[] conn)
        {
            List<POR_SP_GetRequestBySiteIDsOrUserID_Result> RequestList = new List<POR_SP_GetRequestBySiteIDsOrUserID_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestBySiteIDsOrUserID(SiteIDs, 0).OrderByDescending(o => o.ID).ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }

        public List<POR_SP_GetRequestByRequestIDs_Result> GetRequestSummayByRequestIDs(string RequestIDs, string[] conn)
        {
            List<POR_SP_GetRequestByRequestIDs_Result> RequestList = new List<POR_SP_GetRequestByRequestIDs_Result>();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                RequestList = db.POR_SP_GetRequestByRequestIDs(RequestIDs).OrderByDescending(o => o.PRH_ID).ToList();
            }
            catch { }
            finally { }
            return RequestList;
        }
        #endregion

        #region Approval Code
        public string SaveApprovalStatus(long RequestID, string ApprovalStatus, string ApprovalRemark, long UserID, string[] conn)
        {
            string result = "";
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                tApprovalDetail rec = new tApprovalDetail();
                rec = db.tApprovalDetails.Where(a => a.ObjectName == "MaterialRequest" && a.ReferenceID == RequestID).FirstOrDefault();
                if (rec != null)
                {
                    PORtPartRequestHead RequestHead = new PORtPartRequestHead();
                    RequestHead = GetRequestHeadByRequestID(RequestID, conn);

                    db.tApprovalDetails.Detach(rec);
                    rec.Remark = ApprovalRemark;
                    if (ApprovalStatus.ToLower() == "true" || ApprovalStatus.ToLower() == "false")
                    {
                        if (ApprovalStatus.ToLower() == "true")
                        {
                            rec.Status = "Approved";
                            RequestHead.StatusID = db.mStatus.Where(s => s.Status == "Approved").FirstOrDefault().ID;
                        }
                        else if (ApprovalStatus.ToLower() == "false")
                        {
                            rec.Status = "Rejected";
                            RequestHead.StatusID = db.mStatus.Where(s => s.Status == "Rejected").FirstOrDefault().ID;
                        }

                        rec.StatusChangedBy = UserID;
                        rec.ApprovedDate = DateTime.Now;

                        db.tApprovalDetails.Attach(rec);
                        db.ObjectStateManager.ChangeObjectState(rec, EntityState.Modified);
                        db.SaveChanges();

                        SetIntoPartRequestHead(RequestHead, conn);

                        if (ApprovalStatus.ToLower() == "true")
                        {
                            /*Insert into IssueHead & IssuePartDetails*/
                            PORtMINHead IssueHead = new PORtMINHead();
                            IssueHead.PRH_ID = RequestHead.PRH_ID;
                            IssueHead.SiteID = RequestHead.SiteID;
                            IssueHead.IssuedByUserID = 0;
                            IssueHead.StatusID = 1;
                            IssueHead.IsSubmit = false;
                            IssueHead.CreatedBy = UserID;
                            IssueHead.CreationDt = DateTime.Now;
                            db.PORtMINHeads.AddObject(IssueHead);
                            db.SaveChanges();

                            List<PORtPartRequestDetail> PartList = new List<PORtPartRequestDetail>();
                            PartList = db.PORtPartRequestDetails.Where(r => r.PRH_ID == RequestHead.PRH_ID).ToList();

                            foreach (PORtPartRequestDetail part in PartList)
                            {
                                PORtMINDetail IssuePart = new PORtMINDetail();
                                IssuePart.MINH_ID = IssueHead.MINH_ID;
                                IssuePart.PRD_ID = part.PRD_ID;
                                IssuePart.Prod_ID = part.Prod_ID;
                                IssuePart.Prod_Name = part.Prod_Name;
                                IssuePart.Prod_Description = part.Prod_Description;
                                IssuePart.IssuedQty = part.RemaningQty;
                                IssuePart.Sequence = part.Sequence;
                                IssuePart.UOMID = part.UOMID;
                                IssuePart.Prod_Code = part.Prod_Code;

                                db.PORtMINDetails.AddObject(IssuePart);
                                db.SaveChanges();
                            }

                            /*End*/

                            EmailSendofApproved(RequestHead.PRH_ID, conn);
                        }
                        else
                        {
                            EMailSendWhenRequestRejected(RequestHead.PRH_ID, conn);
                        }
                        result = "true";
                    }
                }

            }
            catch { result = "false"; }
            finally { }
            return result;
        }

        public tApprovalDetail GetApprovalDetailsByReqestID(long RequestID, string[] conn)
        {
            tApprovalDetail rec = new tApprovalDetail();
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                rec = db.tApprovalDetails.Where(a => a.ObjectName == "MaterialRequest" && a.ReferenceID == RequestID).FirstOrDefault();
            }
            catch { }
            finally { }
            return rec;
        }
        #endregion

        #region RequestMail

        public void SendMail(string MailBody, string MailSubject, string ToEmailIDs)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtpout.asia.secureserver.net", 25);
                //SmtpClient smtpClient = new SmtpClient("10.228.134.54", 25);
                MailMessage message = new MailMessage();


                MailAddress fromAddress = new MailAddress("admin@brilliantinfosys.com", "GWC");
                //  MailAddress fromAddress = new MailAddress("OMSNotification@gulfwarehousing.com", "GWC");

                //From address will be given as a MailAddress Object
                message.From = fromAddress;

                //To address collection of MailAddress
                message.To.Add(ToEmailIDs);
                message.Subject = MailSubject;

                //Body can be Html or text format
                //Specify true if it  is html message
                message.IsBodyHtml = true;

                //Message body content
                message.Body = MailBody;

                smtpClient.EnableSsl = false;
                //Send SMTP mail
                smtpClient.UseDefaultCredentials = false;
                NetworkCredential basicCredential = new NetworkCredential("admin@brilliantinfosys.com", "6march1986");
                // NetworkCredential basicCredential = new NetworkCredential("OMSNotification@gulfwarehousing.com", "");
                smtpClient.Credentials = basicCredential;

                smtpClient.Send(message);
            }
            catch { }
        }

        protected string EMailGetRequestDetail(POR_SP_GetRequestByRequestIDs_Result Request)
        {
            string result = "";

            result = "Request No. : <b>" + Request.PRH_ID.ToString() + "</b>" +
                     "<br/>" +
                     "Request Date : <b>" + Request.RequestDate.Value.ToString("dd-MMM-yyyy") + "</b>" +
                     "<br/>" +
                     "Status : <b>" + Request.RequestStatus + "</b>" +
                     "<br/>" +
                     "Site / Warehouse : <b>" + Request.SiteName + "</b>" +
                     "<br/>" +
                     "Request Type : <b>" + Request.RequestType + "</b>" +
                     "<br/>" +
                     "Requested By : <b>" + Request.RequestByUserName + "</b>";
            return result;
        }

        protected string EMailGetRequestPartDetail(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet dsOrdrDetail = new DataSet();
            dsOrdrDetail = fillds("select OD.Sequence,OD.Prod_Code,OD.Prod_Description,OD.OrderQty ,PRD.GroupSet from torderdetail OD left outer join mProduct PRD on OD.SkuId =PRD.ID where OD.Orderheadid=" + RequestID + "", conn);

            string messageBody = "<font><b>Order Details :  </b> </font><br/><br/>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:black; text-align: center;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:black; border-style:solid; border-width:thin; padding: 5px; text-align: center;font-size: 10pt;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;

            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Sr. No." + htmlTdEnd;
            messageBody += htmlTdStart + "Item Code" + htmlTdEnd;
            messageBody += htmlTdStart + "Description" + htmlTdEnd;
            messageBody += htmlTdStart + "Qty" + htmlTdEnd;
            messageBody += htmlTdStart + "Group Set" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;
            if (dsOrdrDetail.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r <= dsOrdrDetail.Tables[0].Rows.Count - 1; r++)
                {
                    messageBody += htmlTrStart;
                    messageBody += htmlTdStart + " " + dsOrdrDetail.Tables[0].Rows[r]["Sequence"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dsOrdrDetail.Tables[0].Rows[r]["Prod_Code"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dsOrdrDetail.Tables[0].Rows[r]["Prod_Description"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dsOrdrDetail.Tables[0].Rows[r]["OrderQty"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dsOrdrDetail.Tables[0].Rows[r]["GroupSet"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTrEnd;
                }
            }
            messageBody += htmlTableEnd;
            return messageBody;

        }

        protected string EMailGetRequestDetail(GWC_SP_GetRequestHeadByRequestIDs_Result Request, string[] conn)
        {
            //string result = "";
            string locationcode = "";
            string LocationName = "";
            string messageBody = "<font><b>Order Summary :  </b> </font><br/><br/>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:black; text-align: center;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:black; border-style:solid; border-width:thin; padding: 5px; text-align: center;font-size: 10pt;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;

            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Order Date" + htmlTdEnd;
            messageBody += htmlTdStart + "Order Id" + htmlTdEnd;
            //messageBody += htmlTdStart + "Customer Order Reference No." + htmlTdEnd;          
            messageBody += htmlTdStart + "Exp. Delivery Date" + htmlTdEnd;
            messageBody += htmlTdStart + "Status" + htmlTdEnd;
            messageBody += htmlTdStart + "Department" + htmlTdEnd;
            messageBody += htmlTdStart + "Location Code" + htmlTdEnd;
            messageBody += htmlTdStart + "Location Name" + htmlTdEnd;
            messageBody += htmlTdStart + "Request Type" + htmlTdEnd;
            messageBody += htmlTdStart + "Requested By" + htmlTdEnd;
            messageBody += htmlTdStart + "Remark" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;

            messageBody += htmlTrStart;
            messageBody += htmlTdStart + " " + Request.OrderDate.Value.ToString("dd-MMM-yyyy") + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.OrderNo.ToString() + " " + htmlTdEnd;
            // messageBody += htmlTdStart + " " + Request.OrderNumber + " " + htmlTdEnd;            
            messageBody += htmlTdStart + " " + Request.Deliverydate.Value.ToString("dd-MMM-yyyy") + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.RequestStatus + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.SiteName + " " + htmlTdEnd;
            if (Request.LocationID == 0)
            {
                locationcode = "N/A";
                LocationName = "N/A";
            }
            else
            {
                DataSet dsGetLocation = new DataSet();
                dsGetLocation = fillds("select LocationCode,LocationName from tAddress where ID=" + Request.LocationID + "", conn);

                locationcode = dsGetLocation.Tables[0].Rows[0]["LocationCode"].ToString();
                LocationName = dsGetLocation.Tables[0].Rows[0]["LocationName"].ToString();
            }
            messageBody += htmlTdStart + " " + locationcode + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + LocationName + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.Priority + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.RequestByUserName + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Request.Remark + " " + htmlTdEnd;
            messageBody += htmlTrEnd;

            messageBody += htmlTableEnd;

            //result = "Order Id. : <b>" + Request.ID.ToString() + "</b>" +
            //         "<br/>" +
            //         "Customer Order Reference No.: <b>" + Request.OrderNumber+ "</b>"+
            //         "<br/>" +
            //         "Order Date : <b>" + Request.OrderDate.Value.ToString("dd-MMM-yyyy") + "</b>" +
            //         "<br/>" +
            //         "Exp. Delivery Date : <b>" + Request.Deliverydate.Value.ToString("dd-MMM-yyyy") + "</b>" +
            //         "<br/>" +
            //         "Status : <b>" + Request.RequestStatus + "</b>" +
            //         "<br/>" +
            //         "Department : <b>" + Request.SiteName + "</b>" +
            //         "<br/>" +
            //         "Request Type : <b>" + Request.Priority + "</b>" +
            //         "<br/>" +
            //         "Requested By : <b>" + Request.RequestByUserName + "</b>" +
            //         "<br/>" +
            //         "Remark : <b>" + Request.Remark + "</b>";                    

            return messageBody;
        }

        protected string EmailGetAddressDetails(GWC_SP_GetRequestHeadByRequestIDs_Result Request, string[] conn)
        {
            string location = "";
            string Address = "";
            string messageBody = "<font><b>Address Details:  </b> </font><br/><br/>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:black; text-align: center;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:black; border-style:solid; border-width:thin; padding: 5px; text-align: center;font-size: 10pt;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Customer Address" + htmlTdEnd;
            messageBody += htmlTdStart + "Location" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;
            if (Request.LocationID == 0)
            {
                location = "N/A";
            }
            else
            {
                DataSet dsGetLocations = new DataSet();
                dsGetLocations = fillds("select AddressLine1 from tAddress where ID=" + Request.LocationID + "", conn);
                location = dsGetLocations.Tables[0].Rows[0]["AddressLine1"].ToString();
            }
            DataSet dsGetAddressid = new DataSet();
            dsGetAddressid = fillds("select AddressId from torderhead where Id=" + Request.ID + "", conn);
            long AddressId = Convert.ToInt64(dsGetAddressid.Tables[0].Rows[0]["AddressId"].ToString());
            if (AddressId == 0)
            {
                Address = "N/A";
            }
            else
            {
                DataSet dsGetAddress = new DataSet();
                dsGetAddress = fillds("select AddressLine1 from tAddress where ID=" + AddressId + "", conn);
                Address = dsGetAddress.Tables[0].Rows[0]["AddressLine1"].ToString();
            }
            messageBody += htmlTrStart;
            messageBody += htmlTdStart + " " + Address + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + location + " " + htmlTdEnd;
            messageBody += htmlTrEnd;
            messageBody += htmlTableEnd;
            return messageBody;
        }


        protected string EMailGetRequestPratDetail(long RequestID, long SiteID, string[] conn, List<POR_SP_GetPartDetail_ForRequest_Result> PartList = null)
        {
            string result = "";
            try
            {
                List<POR_SP_GetPartDetail_ForRequest_Result> htmlList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                if (RequestID != 0)
                {
                    BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                    htmlList = db.POR_SP_GetPartDetail_ForRequest("0", 0, SiteID, RequestID).ToList();
                }
                else { htmlList = PartList; }
                int srno = 0;
                XElement xmlEle = new XElement("table", from rec in htmlList
                                                        select new XElement("tr",
                                                        new XElement("td", (srno = srno + 1) + "."),
                                                        new XElement("td", rec.Prod_Code),
                                                        new XElement("td", rec.Prod_Name),
                                                        new XElement("td", rec.Prod_Description),
                                                        new XElement("td1", rec.RequestQty),
                                                        new XElement("td", rec.UOM)));


                string tblHeader = "<br /><table cellpadding='2' cellspacing='5' style='text-align: left; font-family: Tahoma; font-size: 10px;'>";
                tblHeader = tblHeader + "<tr><td colspan='6'><b>Part Details : </b></td></tr>" +
                                        "<tr>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>Sr.No.</td>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>Part Code</td>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>Part Name</td>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>Part Description</td>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>Request Qty</td>" +
                                        "<td style='font-weight: bold; text-align: center; border-bottom: solid 1px gray; border-top: solid 1px gray;'>UOM</td></tr>";

                //result = result + xmlEle.ToString().Replace("<table>", tblHeader);
                result = result + xmlEle.ToString().Replace("<table>", tblHeader).Replace("<td1>", "<td style='text-align: right;'>").Replace("</td1>", "</td>");
            }
            catch { }
            return result;
        }

        protected string EmailGetEmailIDsByUserID(long UserID, string[] conn)
        {
            string result;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mUserProfileHead user = new mUserProfileHead();
            user = db.mUserProfileHeads.Where(u => u.ID == UserID).FirstOrDefault();
            result = user.EmailID;
            return result;
        }

        protected string GetUserNameByUserID(long UserID, string[] conn)
        {
            string result;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mUserProfileHead user = new mUserProfileHead();
            user = db.mUserProfileHeads.Where(u => u.ID == UserID).FirstOrDefault();
            result = user.FirstName + " " + user.LastName;
            return result;
        }

        protected string[] EmailGetEmailIDsBySiteIDApprovalLevel(long SiteID, long ApprovalLevel, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string[] result = new string[] { "", "" };

            if (ApprovalLevel > 0)
            {
                var obj = from u in db.mUserProfileHeads
                          join ald in db.mApprovalLevelDetails on u.ID equals ald.UserID
                          join al in db.mApprovalLevels on ald.ApprovalLevelID equals al.ID
                          where al.TerritoryID == SiteID && al.ApprovalLevel == ApprovalLevel
                          select u;

                foreach (mUserProfileHead o in obj)
                {
                    if (result[0] != "")
                    {
                        result[0] = result[0] + " | " + (o.FirstName + " " + o.MiddelName + " " + o.LastName);
                        result[1] = result[1] + "," + o.EmailID;
                    }
                    else
                    {
                        result[0] = o.FirstName + " " + o.MiddelName + " " + o.LastName;
                        result[1] = o.EmailID;
                    }
                }
            }
            else if (ApprovalLevel == 0)
            {
                var obj = from u in db.mUserProfileHeads
                          join mtd in db.mUserTerritoryDetails on u.ID equals mtd.UserID
                          where mtd.TerritoryID == SiteID && mtd.Level == 1
                          select u;

                foreach (mUserProfileHead o in obj)
                {
                    if (result[0] != "")
                    {
                        result[0] = result[0] + " | " + (o.FirstName + " " + o.MiddelName + " " + o.LastName);
                        result[1] = result[1] + "," + o.EmailID;
                    }
                    else
                    {
                        result[0] = o.FirstName + " " + o.MiddelName + " " + o.LastName;
                        result[1] = o.EmailID;
                    }
                }
            }
            return result;
        }

        protected string MailGetFooter()
        {
            string MailFooter = "<br/><br/>" +
                //"<a href='http://elegantcrm.com/gwc/Login/Login.aspx' target='_blank' style='font-size: 18px; color: #3BB9FF; font-family: Comic Sans MS; text-decoration: none;'>Go to GWC</a>" +
                                "Please <a href='http://elegantcrm.com/gwc/Login/Login.aspx' target='_blank' style='color: #3BB9FF;  text-decoration: none;'>click here </a>  to view the order details." +
                                "<br/><br/>" +
                                "Thank you, <br/>" +
                                "OMS Notification Team<br/>" +
                                "<br/><br/><hr/>" +
                                "<b>SELF EXPRESSION. BY GWC </b>" +
                                "<br/>This email including its attachments are confidential and intended solely for the use of the individual or entity to whom they are addressed. If you have received this email in error, please delete it from your system and notify the sender immediately. If you are not the intended recipient you are notified that disclosing, copying or distributing the content of this information is strictly prohibited. " +
                //"<br/>This e-mail, and any attachments hereto, is intended for use only by the addressee(s) named herein, and may contain legally privileged and/or confidential information. If you are not an intended recipient of this e-mail, you are notified that any dissemination, distribution or copying of this e-mail, and any attachments hereto, is strictly prohibited. If you have received this e-mail in error, please notify the sender by reply e-mail, and permanently delete this e-mail, and any copies or printouts. " +
                                "<br/><br/>PLEASE CONSIDER THE ENVIRONMENT BEFORE PRINTING THIS EMAIL.";

            return MailFooter;
        }

        protected void EmailSendofRequestReject(long RequestBy, long RequestID, string[] conn)
        {
            string MailSubject = "";
            string MailBody = "";
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
            Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

            long Orderstatus = long.Parse(Request.Status.ToString());
            long DepartmentID = long.Parse(Request.SiteID.ToString());
            long Status = long.Parse(Request.Status.ToString());

            DataSet dsMailSubBody = new DataSet();
            dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=4) and MessageID=(select Id from mDropdownValues where Value='Information') and DepartmentID=" + DepartmentID + "", conn);

            DataSet dsOrderApprovers = new DataSet();
            dsOrderApprovers = fillds("select UserApproverID from torderwiseaccess where OrderID=" + RequestID + " and UserType!='User'", conn);
            int dscnt = dsOrderApprovers.Tables[0].Rows.Count;
            if (dscnt > 0)
            {
                for (int i = 0; i <= dscnt - 1; i++)
                {
                    MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

                    //MailBody = "Dear " + GetUserNameByUserID(RequestBy, conn) + ",";
                    MailBody = "Dear " + GetUserNameByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn) + ",";
                    MailBody = dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                    //SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(RequestBy, conn));
                    SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn));
                }
            }


            long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
            AdditionalDistribution(RequestID, TemplateID, conn);

            SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
        }

        public void EmailSendofApproved(long UserID, long RequestID, string[] conn)
        {
            string MailSubject;
            string MailBody;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
            Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

            long Orderstatus = long.Parse(Request.Status.ToString());
            long DepartmentID = long.Parse(Request.SiteID.ToString());
            long Status = long.Parse(Request.Status.ToString());

            DataSet dsMailSubBody = new DataSet();
           // dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=3) and MessageID=(select Id from mDropdownValues where Value='Action') and DepartmentID=" + DepartmentID + "", conn);
            dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=30) and MessageID=(select Id from mDropdownValues where Value='Action') and DepartmentID=" + DepartmentID + "", conn);
            MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

            MailBody = "Dear " + GetUserNameByUserID(UserID, conn) + ", <br/>";
            MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

            MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
            MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
            MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

            SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(UserID, conn));

            long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
            AdditionalDistribution(RequestID, TemplateID, conn);

            SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
        }
        protected int EmailSendToApprover(long ApproverID, long RequestID, string[] conn)
        {
            int Result = 0;
            try
            {
                string MailSubject;
                string MailBody;
                // string partdetail = EMailGetRequestPratDetail(0, 0, conn, ReqPartDetils);
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

                GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
                Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

                long Orderstatus = long.Parse(Request.Status.ToString());
                long DepartmentID = long.Parse(Request.SiteID.ToString());
                long Status = long.Parse(Request.Status.ToString());

                DataSet dsMailSubBody = new DataSet();
                dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2 and value='Order Submit') and MessageID=(select Id from mDropdownValues where Value='Action') and DepartmentID=" + DepartmentID + "", conn);
                if (dsMailSubBody.Tables[0].Rows.Count > 0)
                {
                    MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

                    MailBody = "Dear " + GetUserNameByUserID(ApproverID, conn) + ", <br/>";

                    DataSet dsFA = new DataSet();
                    dsFA = fillds("select UserType from tOrderWiseAccess  where OrderID=" + RequestID + " and UserApproverID=" + ApproverID + "", conn);
                    string UsrType = dsFA.Tables[0].Rows[0]["UserType"].ToString();
                    if (UsrType == "General Approver")
                    { MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString(); }
                    else
                    {
                        DataSet dsFAMailBody = new DataSet();
                        dsFAMailBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2 and value='Order Submit Price Change') and MessageID=(select Id from mDropdownValues where Value='Action') and DepartmentID=" + DepartmentID + "", conn);
                        MailBody = MailBody + dsFAMailBody.Tables[0].Rows[0]["MailBody"].ToString();
                    }

                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                    SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(ApproverID, conn));
                    Result = 2;//Mail Send Successfully to approver
                    long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
                    AdditionalDistribution(RequestID, TemplateID, conn);

                    SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
                }
            }
            catch (System.Exception ex)
            {
                Result = 3;//Mail Not Send Successfully to approver
            }
            finally { }
            return Result;
        }

        // protected void EmailSendWhenRequestSubmit(long RequestID, List<POR_SP_GetPartDetail_ForRequest_Result> ReqPartDetils, string[] conn)
        public int EmailSendWhenRequestSubmit(long RequestID, string[] conn)
        {
            int Result = 0;
            try
            {
                string MailSubject;
                string MailBody;

                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

                GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
                Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

                long Orderstatus = long.Parse(Request.Status.ToString());
                long DepartmentID = long.Parse(Request.SiteID.ToString());
                long Status = long.Parse(Request.Status.ToString());

                DataSet dsMailSubBody = new DataSet();
                dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2 and value='Order Submit') and MessageID=(select Id from mDropdownValues where Value='Information') and DepartmentID=" + DepartmentID + "", conn);
                if (dsMailSubBody.Tables[0].Rows.Count > 0)
                {
                    DataSet ds = new DataSet();
                    ds = fillds("select orderType from tOrderHead where Id=" + RequestID + "", conn);
                    string ordertype = ds.Tables[0].Rows[0]["Ordertype"].ToString();
                    if (Status == 2)
                    {
                        MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";
                    }
                    else if (ordertype == "Direct Import" && Status == 3)
                    {
                        MailSubject = "Order has been submitted sucessfully with Direct Order Import , Order No # " + Request.OrderNo + "";
                    }
                    else
                    {
                        string CrntStatus = Request.RequestStatus.ToString();
                        MailSubject = "Order " + CrntStatus + ", Order No # " + Request.OrderNo + "";
                    }

                    MailBody = "Dear " + GetUserNameByUserID(Convert.ToInt64(Request.RequestBy), conn) + ", <br/>";
                    MailBody = MailBody + "This is an automatically generated message in reference to a order request. An approval action is required before OMS can proceed. <br/> Thank you for your request. Before we can proceed, we need approver's formal approval to proceed. <br/>";
                    if (ordertype == "Direct Import" && Status == 3)
                    {
                        MailBody = MailBody + "<br/><br/>" + "Order has been submitted sucessfully with Direct Order Import";
                    }
                    MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                    if (ordertype == "Direct Import" && Status == 3)
                    {
                        MailBody = MailBody + "<br/><br/>" + "Note : The order processes through direct import doesn't required any approver. These Order send to WMS for further processing ";
                    }

                    SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(Request.RequestBy), conn));
                    Result = 2;// Mail sent to requestor
                    long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
                    if (Status == 2)
                    {
                        AdditionalDistribution(RequestID, TemplateID, conn);
                    }
                    SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
                    if (Request.ContactId1 > 0)
                    {
                        string Con1Name = Request.Contact1Name; string Con1Email = Request.Con1Email;
                        MailBody = "";
                        MailBody = "Dear " + Con1Name + ", <br/>";
                        MailBody = MailBody + "This is an automatically generated message in reference to a order request. An approval action is required before OMS can proceed. <br/> Thank you for your request. Before we can proceed, we need approver's formal approval to proceed. <br/>";
                        if (ordertype == "Direct Import" && Status == 3)
                        {
                            MailBody = MailBody + "<br/><br/>" + "Order has been submitted sucessfully with Direct Order Import";
                        }

                        MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                        MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                        MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                        MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);
                        if (ordertype == "Direct Import" && Status == 3)
                        {
                            MailBody = MailBody + "<br/><br/>" + "Note : The order processes through direct import doesn't required any approver. These Order send to WMS for further processing ";
                        }
                        SendMail(MailBody + MailGetFooter(), MailSubject, Con1Email);

                        string Contact2 = Request.Con2.ToString();
                        //long Con2ID = long.Parse(Request.ContactId2.ToString());
                        //if (Con2ID > 0)
                        if (Contact2 != "0")
                        {
                            string Contact2Emails = GetContact2Email(Contact2, conn);
                            // string Con2Name = Request.Contact2Name; string Con2Email = Request.Con2Email;
                            MailBody = "";
                            MailBody = "Hi, <br/>";
                            MailBody = MailBody + "This is an automatically generated message in reference to a order request. An approval action is required before OMS can proceed. <br/> Thank you for your request. Before we can proceed, we need approver's formal approval to proceed. <br/>";
                            if (ordertype == "Direct Import" && Status == 3)
                            {
                                MailBody = MailBody + "<br/><br/>" + "Order has been submitted sucessfully with Direct Order Import";
                            }
                            MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();
                            MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                            MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                            MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);
                            if (ordertype == "Direct Import" && Status == 3)
                            {
                                MailBody = MailBody + "<br/><br/>" + "Note : The order processes through direct import doesn't required any approver. These Order send to WMS for further processing ";
                            }
                            SendMail(MailBody + MailGetFooter(), MailSubject, Contact2Emails);
                        }
                    }
                }
            }
            catch
            {
                Result = 3;//mail not sent to requestor.
            }
            finally { }
            return Result;
        }

        public string GetContact2Email(string Contact2, string[] conn)
        {
            string Contact1Name = "";
            DataSet ds = new DataSet();
            ds = fillds("select EmailID from tcontactpersondetail where ID IN( " + Contact2 + " )", conn);
            int cnt = ds.Tables[0].Rows.Count;
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    if (i == 0) { Contact1Name = ds.Tables[0].Rows[i]["EmailID"].ToString(); }
                    else
                    {
                        Contact1Name = Contact1Name + "," + ds.Tables[0].Rows[i]["EmailID"].ToString();
                    }
                }
            }
            return Contact1Name;
        }

        protected void SaveCorrespondsData(long RequestID, string MailSubject, string MailBody, long DepartmentID, long StatusID, string[] conn)
        {
            tCorrespond Cor = new tCorrespond();
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            Cor.OrderHeadId = RequestID;
            Cor.MessageTitle = MailSubject;
            Cor.Message = MailBody;
            Cor.date = DateTime.Now;
            Cor.MessageSource = "Correspondance";
            Cor.MessageType = "Information";
            Cor.DepartmentID = DepartmentID;
            Cor.CurrentOrderStatus = StatusID;
            Cor.MailStatus = 1;
            Cor.Archive = false;
            db.tCorresponds.AddObject(Cor);
            db.SaveChanges();
        }

        protected void AdditionalDistribution(long RequestID, long TemplateID, string[] conn)
        {
            DataSet dsAddDist = new DataSet();
            dsAddDist = fillds("select * from GWV_VW_DistributionList where TemplateID=" + TemplateID + "", conn);
            int cnt = dsAddDist.Tables[0].Rows.Count;
            if (cnt > 0)
            {
                for (int i = 0; i <= cnt - 1; i++)
                {
                    string MailSubject;
                    string MailBody;
                    // string partdetail = EMailGetRequestPratDetail(0, 0, conn, ReqPartDetils);
                    BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

                    GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
                    Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

                    long Orderstatus = long.Parse(Request.Status.ToString());
                    long DepartmentID = long.Parse(Request.SiteID.ToString());
                    long Status = long.Parse(Request.Status.ToString());

                    string AdDistName = dsAddDist.Tables[0].Rows[i]["Name"].ToString();
                    string AdDistEmail = dsAddDist.Tables[0].Rows[i]["EmailID"].ToString();

                    DataSet dsMailSubBody = new DataSet();
                    // dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=2) and MessageID=(select Id from mDropdownValues where Value='Action') and DepartmentID=" + DepartmentID + "", conn);

                    dsMailSubBody = fillds("select * from mMessageEMailTemplates where ID=" + TemplateID + " ", conn);

                    if (Status == 2)
                    {
                        MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";
                    }
                    else
                    {
                        string CrntStatus = Request.RequestStatus.ToString();
                        MailSubject = "Order " + CrntStatus + ", Order No # " + Request.OrderNo + "";
                    }


                    MailBody = "Dear " + AdDistName + ", <br/>";
                    MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                    SendMail(MailBody + MailGetFooter(), MailSubject, AdDistEmail);
                }
            }
        }


        protected void EmailSendofApproved(long RequestID, string[] conn)
        {
            try
            {
                string MailSubject;
                string MailBody;
                //int E_ID;

                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                POR_SP_GetRequestByRequestIDs_Result Request = new POR_SP_GetRequestByRequestIDs_Result();
                Request = db.POR_SP_GetRequestByRequestIDs(RequestID.ToString()).FirstOrDefault();
                string partdetail = EMailGetRequestPratDetail(Request.PRH_ID, Request.SiteID, conn);


                /*Notification Email to Requestor*/
                //MailSubject = "Approved : Part Request No. " + Request.RequestNo + "[ " + Request.RequestType + " ] Site : " + Request.SiteName + "";
                MailSubject = "Approval Notification : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Approved";
                MailBody = " Hello, <br/><b> " + Request.RequestByUserName + " </b> <br/><br/>" +
                           " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                           " Approval has been received from Operation Manager." +
                           " <br/>" +
                           " Cummins Team will now begin work on this request.  If you have questions or comments, please contact the Operation Manager directly." +
                           " Your request details are provided below : ";
                //  MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail((Request,conn);
               
                MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);
                MailBody = MailBody + partdetail;
                SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(Request.RequestBy), conn));


                //  SaveInboxData(Convert.ToInt64(Request.RequestBy), Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                /*End*/

                string[] MailTo = new string[] { };
                MailTo = new string[] { };
                MailTo = EmailGetEmailIDsBySiteIDApprovalLevel(1, 0, conn);
                string[] MailToName = MailTo[0].Split('|');
                string[] MailToEmailID = MailTo[1].Split(',');
                for (int i = 0; i < MailToName.Count(); i++)
                {
                    /*Information Email to ProjectLead */
                    MailSubject = "Pending for Issue : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Approved";
                    MailBody = " Hello,<br/> <b> " + MailToName[i] + " </b> <br/><br/>" +
                               " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                               " Approval has been received from Operation Manager." +
                               " <br/>" +
                               " Now it's pending for issue." +
                               " Part Request details are provided below : ";
                    //MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);
                    MailBody = MailBody + partdetail;
                    /*End*/
                    SendMail(MailBody + MailGetFooter(), MailSubject, MailToEmailID[i]);


                    //E_ID = Convert.ToInt32(GetIDFromEmailName(MailTo[0], MailTo[1], conn));
                    //SaveInboxData(E_ID, Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                }


                MailTo = new string[] { };
                MailTo = EmailGetEmailIDsBySiteIDApprovalLevel(Request.SiteID, 1, conn);
                MailToName = new string[] { };
                MailToEmailID = new string[] { };
                MailToName = MailTo[0].Split('|');
                MailToEmailID = MailTo[1].Split(',');
                for (int i = 0; i < MailToName.Count(); i++)
                {
                    /*Acknowledgement Email to OperationManager when approved */
                    MailSubject = "Approval Acknowledgement : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Approved";
                    MailBody = " Hello,<br/> <b> " + MailToName[i] + " </b> <br/><br/>" +
                               " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                               " Thank you for giving approval." +
                               " <br/>" +
                               " Cummins Team will now begin work on this request." +
                               " Part Request details are provided below : ";
                    // MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + partdetail;
                    /*End*/
                    SendMail(MailBody + MailGetFooter(), MailSubject, MailToEmailID[i]);


                    // E_ID = Convert.ToInt32(GetIDFromEmailName(MailTo[0], MailTo[1], conn));
                    //SaveInboxData(E_ID, Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                }


            }
            catch { }
            finally { }
        }

        protected void EMailSendWhenRequestRejected(long RequestID, string[] conn)
        {
            try
            {
                string MailSubject;
                string MailBody;
                int E_ID;

                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                POR_SP_GetRequestByRequestIDs_Result Request = new POR_SP_GetRequestByRequestIDs_Result();
                Request = db.POR_SP_GetRequestByRequestIDs(RequestID.ToString()).FirstOrDefault();
                string partdetail = EMailGetRequestPratDetail(Request.PRH_ID, Request.SiteID, conn);

                /*Notification Email to Requestor*/
                MailSubject = "Rejection Notification : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Rejected";
                MailBody = " Hello, <br/><b> " + Request.RequestByUserName + " </b> <br/><br/>" +
                           " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                           " Material Request has been rejected by Operation Manager" +
                           " <br/>" +
                           " If you have questions or comments, please contact the Operation Manager directly." +
                           " Your request details are provided below : ";
                //MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                MailBody = MailBody + partdetail;
                SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(Request.RequestBy), conn));


                //   SaveInboxData(Convert.ToInt64(Request.RequestBy), Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                /*End*/

                string[] MailTo = new string[] { };
                MailTo = new string[] { };
                MailTo = EmailGetEmailIDsBySiteIDApprovalLevel(1, 0, conn);
                string[] MailToName = MailTo[0].Split('|');
                string[] MailToEmailID = MailTo[1].Split(',');
                for (int i = 0; i < MailToName.Count(); i++)
                {
                    /*Information Email to ProjectLead */
                    MailSubject = "Rejection Notification : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Rejected";
                    MailBody = " Hello,<br/> <b> " + MailToName[i] + " </b> <br/><br/>" +
                               " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                               " Material Request has been rejected by Operation Manager" +
                               " <br/>" +
                               " If you have questions or comments, please contact the Operation Manager directly." +
                               " Part Request details are provided below : ";
                    // MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + partdetail;
                    /*End*/
                    SendMail(MailBody + MailGetFooter(), MailSubject, MailToEmailID[i]);


                    //   E_ID = Convert.ToInt32(GetIDFromEmailName(MailTo[0], MailTo[1], conn));
                    //   SaveInboxData(E_ID, Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                }



                MailTo = new string[] { };
                MailTo = EmailGetEmailIDsBySiteIDApprovalLevel(Request.SiteID, 1, conn);
                MailToName = new string[] { };
                MailToEmailID = new string[] { };
                MailToName = MailTo[0].Split('|');
                MailToEmailID = MailTo[1].Split(',');
                for (int i = 0; i < MailToName.Count(); i++)
                {
                    /*Acknowledgement Email to OperationManager when Rejected */
                    MailSubject = "Rejection Acknowledgement : Material Request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() + " is Rejected";
                    MailBody = " Hello,<br/> <b> " + MailToName[i] + " </b> <br/><br/>" +
                               " This is an automatically generated message in reference to a Material request for " + Request.SiteName + " - ID " + Request.PRH_ID.ToString() +
                               " The request has been rejected by you." +
                               " <br/>" +
                               " Cummins Team will now begin work on this request.";
                    // MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + partdetail;
                    /*End*/
                    SendMail(MailBody + MailGetFooter(), MailSubject, MailToEmailID[i]);


                    //  E_ID = Convert.ToInt32(GetIDFromEmailName(MailTo[0], MailTo[1], conn));
                    //  SaveInboxData(E_ID, Request.SiteID, "Material Request", MailSubject, MailBody, Convert.ToInt64(Request.StatusID), conn);
                }
            }
            catch { }
            finally { }
        }
        #endregion

        #region System Inbox
        protected void SaveInboxData(long ToUserID, long SiteID, string ObjectName, string Subject, string Details, long StatusID, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                PORtInboxData Data = new PORtInboxData();
                Data.ToUserID = ToUserID;
                Data.SiteID = SiteID;
                Data.ObjectName = ObjectName;
                Data.Subject = Subject;
                Data.Details = Details;
                Data.StatusID = StatusID;
                Data.IsRead = false;
                Data.IsArchive = false;
                Data.FolderName = "Inbox";
                Data.CreationDate = DateTime.Now.ToLocalTime();
                db.AddToPORtInboxDatas(Data);
                db.SaveChanges();
            }
            catch
            {
            }
        }
        #endregion

        protected string GetIDFromEmailName(string name, string emailid, string[] conn)
        {
            string ide = "";
            string[] m = new string[] { };
            m = new string[] { };
            string[] n = new string[] { };
            n = new string[] { };
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                mUserProfileHead user = new mUserProfileHead();
                m = emailid.Split(',');
                n = name.Split('|');

                for (int i = 0; i < emailid.Count(); i++)
                {
                    user = db.mUserProfileHeads.Where(u => u.EmailID == emailid[i].ToString()).FirstOrDefault();
                    ide = Convert.ToString(user.ID);
                }
            }
            catch { }
            finally { }
            return ide;
        }

        #region GWC
        protected DataSet fillds(String strquery, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();

            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            SqlDataAdapter da = new SqlDataAdapter(strquery, sqlConn);
            ds.Reset();
            da.Fill(ds);
            return ds;
        }

        public DataSet GetTemplateDetails(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from VW_GetTemplateDetails where createdBy=" + UserID + "   union select * from VW_GetTemplateDetails where createdBy!=" + UserID + "  and Accesstype='Public' ", conn);
            return ds;
        }

        public DataSet GetTemplateDetailsSuperAdmin(string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from VW_GetTemplateDetails", conn);
            return ds;
        }

        public DataSet GetTemplateDetailsAdmin(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from VW_GetTemplateDetails where Department in (select TerritoryID from mUserTerritoryDetail where UserID=" + UserID + ")", conn);
            return ds;
        }

        public DataSet GetTemplateDetailsBind(long UserID, long DeptID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from VW_GetTemplateDetails where createdBy=" + UserID + " and Department=" + DeptID + "  union select * from VW_GetTemplateDetails where createdBy!=" + UserID + "  and Accesstype='Public' and Department=" + DeptID + "", conn);
            return ds;
        }

        //public DataSet GetTemplateDetailsBind(long UserID, long DeptID, string[] conn)
        //{
        //    DataSet ds = new DataSet();
        //    ds = fillds("select * from VW_GetTemplateDetails where createdBy=" + UserID + " and Department=" + DeptID + "  union select * from VW_GetTemplateDetails where createdBy!=" + UserID + "  and Accesstype='Public' and Department=" + DeptID + " ", conn);
        //    return ds;
        //}

        public DataSet GetGetInterfaceDetails(string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from mInterfaceMap", conn);
            return ds;
        }

        public DataSet GetGetMessageDetails(string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from mMessageHeader", conn);
            return ds;
        }

        public DataSet GetApprovalDetailsNew(long OrderID, long ApproverID, string[] conn)
        {
            DataSet ds = new DataSet();
            // ds = fillds("select * from VW_ApprovalTransDetails where (DeligateTo=" + ApproverID + " or ApproverID=" + ApproverID + ") and OrderID=" + OrderID + "", conn);
            //ds = fillds("select * from VW_ApprovalTransDetails where (DeligateTo=" + ApproverID + " or ApproverID=" + ApproverID + ")  and OrderID=" + OrderID + "  union all select * from VW_ApprovalTransDetails  where   OrderID=" + OrderID + " and  Status=3 ", conn);
            // ds = fillds("select distinct id,OrderId,StoreId,UserId,UserName,EmailID,ApprovalId,ApproverID,ApproverName,ApproverEmailID,Status,StatusName,Date,Remark,DeligateFrom,DeligateUser,DeligateTo,ImgApproval,ImgReject,ImgApprovewithRevision from (select * from VW_ApprovalTransDetails where (DeligateTo=" + ApproverID + " or ApproverID=" + ApproverID + ")  and OrderID=" + OrderID + "  union all select * from VW_ApprovalTransDetails  where   OrderID=" + OrderID + " and  Status in(3,24,4))aaa ", conn);

            ds = fillds("select distinct id,OrderId,OrderCurrentStatus,StoreId,UserId,UserName,EmailID,ApprovalId,ApproverID,ApproverName,ApproverEmailID,Status,StatusName,Date,Remark,DeligateFrom,DeligateUser,DeligateTo,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgApproval end ImgApproval,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgReject end ImgReject,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgApprovewithRevision end ImgApprovewithRevision from (select * from VW_ApprovalTransDetails where (DeligateTo=" + ApproverID + " or ApproverID=" + ApproverID + ")  and OrderID=" + OrderID + "  union all select * from VW_ApprovalTransDetails  where   OrderID=" + OrderID + " and  Status in(3,24,4))aaa ", conn);
            return ds;
        }

        public DataSet GetApprovalDetailsNewAdmin(long OrderID, string[] conn)
        {
            DataSet ds = new DataSet();
            //ds = fillds("select distinct id,OrderId,StoreId,UserId,UserName,EmailID,ApprovalId,ApproverID,ApproverName,ApproverEmailID,Status,StatusName,Date,Remark,DeligateFrom,DeligateUser,DeligateTo,ImgApproval,ImgReject,ImgApprovewithRevision from (select * from VW_ApprovalTransDetails  where   OrderID=" + OrderID + " )aaa ", conn);
            ds = fillds("select distinct id,OrderId,OrderCurrentStatus,StoreId,UserId,UserName,EmailID,ApprovalId,ApproverID,ApproverName,ApproverEmailID,Status,StatusName,Date,Remark,DeligateFrom,DeligateUser,DeligateTo,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgApproval end ImgApproval,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgReject end ImgReject,case when OrderCurrentStatus IN(4,10) then 'gray' else ImgApprovewithRevision end ImgApprovewithRevision from (select * from VW_ApprovalTransDetails  where   OrderID=" + OrderID + " )aaa ", conn);
            return ds;
        }

        public DataSet GetApprovalDetailsAllApproved(string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select 'Approver Level1' as ApproverLevel, 'Bispl Admin1' as ApproverName, '26/08/2016' as  ApprovedDate ,'Approved' as Remark,'Approved' as Status,'green' as ImgApproval union select 'Approver Level2' as ApproverLevel, 'Bispl Admin2' as ApproverName, '27/08/2016' as  ApprovedDate ,'Approved' as Remark,'Approved' as Status,'green' as ImgApproval union select 'Approver Level3' as ApproverLevel, 'Bispl Admin3' as ApproverName, '28/08/2016' as  ApprovedDate ,'Approved' as Remark,'Approved' as Status,'green' as ImgApproval ", conn);
            return ds;
        }

        public DataSet GetUOMofSelectedProduct(int ProdID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select PackUomid,SkuId,ShortDescri,Description,Quantity,UOMID,Sequence, (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " order by Sequence ", conn);
            return ds;
        }

        public long SetIntotOrderHead(tOrderHead PartRequest, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (PartRequest.Id == 0)
            {
                db.tOrderHeads.AddObject(PartRequest);
            }
            else
            {
                //string ONO = "";
                //DataSet dsONO = new DataSet();
                //dsONO = fillds("select OrderNo from torderhead where id="+ PartRequest.Id +"", conn);
                //ONO = dsONO.Tables[0].Rows[0]["OrderNo"].ToString();
                //PartRequest.OrderNo = ONO;
                db.tOrderHeads.Attach(PartRequest);
                db.ObjectStateManager.ChangeObjectState(PartRequest, EntityState.Modified);

                tOrderHeadHistory OH = new tOrderHeadHistory();
                OH.Id = PartRequest.Id;
                OH.OrderNumber = PartRequest.OrderNumber;
                OH.StoreId = PartRequest.StoreId;
                OH.Orderdate = PartRequest.Orderdate;
                OH.Deliverydate = PartRequest.Deliverydate;
                OH.Priority = PartRequest.Priority;
                OH.ContactId1 = PartRequest.ContactId1;
                OH.ContactId2 = PartRequest.ContactId2;
                OH.AddressId = PartRequest.AddressId;
                OH.Remark = PartRequest.Remark;
                OH.Status = PartRequest.Status;
                OH.CreatedBy = PartRequest.CreatedBy;
                OH.Creationdate = PartRequest.Creationdate;
                OH.Title = PartRequest.Title;
                OH.RequestBy = PartRequest.RequestBy;

                db.tOrderHeadHistories.Attach(OH);
            }
            db.SaveChanges();
            return PartRequest.Id;
        }

        public long InsertIntomRequestTemplateHead(mRequestTemplateHead ReqTemplHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            if (ReqTemplHead.ID == 0)
            {
                db.mRequestTemplateHeads.AddObject(ReqTemplHead);
            }
            else
            {
                db.mRequestTemplateHeads.Attach(ReqTemplHead);
                db.ObjectStateManager.ChangeObjectState(ReqTemplHead, EntityState.Modified);
            }
            db.SaveChanges();
            return ReqTemplHead.ID;
        }


        public void FinalSavemRequestTemplateDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> finalSaveLst = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            XElement xmlEle = new XElement("Request", from rec in finalSaveLst
                                                      select new XElement("PartList",
                                                      new XElement("PRH_ID", paraReferenceID),
                                                      new XElement("Prod_ID", Convert.ToInt64(rec.Prod_ID)),
                                                      new XElement("RequestQty", Convert.ToDecimal(rec.RequestQty)),
                                                      new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                      new XElement("Price", Convert.ToDecimal(rec.Price)),
                                                      new XElement("Total", Convert.ToDecimal(rec.Total)),
                                                      new XElement("IsPriceChange", Convert.ToInt16(rec.IsPriceChange))
                                                      ));

            ObjectParameter _PRH_ID = new ObjectParameter("PRH_ID", typeof(long));
            _PRH_ID.Value = paraReferenceID;

            ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
            _xmlData.Value = xmlEle.ToString();


            ObjectParameter[] obj = new ObjectParameter[] { _PRH_ID, _xmlData };
            db.ExecuteFunction("POR_SP_mRequestTemplateDetail", obj);

            //ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
        }

        public void FinalSavemRequestTemplateDetailTemplate(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> finalSaveLst = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            XElement xmlEle = new XElement("Request", from rec in finalSaveLst
                                                      select new XElement("PartList",
                                                      new XElement("PRH_ID", paraReferenceID),
                                                      new XElement("Prod_ID", Convert.ToInt64(rec.Prod_ID)),
                                                      new XElement("RequestQty", Convert.ToDecimal(rec.RequestQty)),
                                                      new XElement("UOMID", Convert.ToInt64(rec.UOMID)),
                                                      new XElement("Price", Convert.ToDecimal(rec.Price)),
                                                      new XElement("Total", Convert.ToDecimal(rec.Total)),
                                                      new XElement("IsPriceChange", Convert.ToInt16(rec.IsPriceChange))
                                                      ));

            ObjectParameter _PRH_ID = new ObjectParameter("PRH_ID", typeof(long));
            _PRH_ID.Value = paraReferenceID;

            ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
            _xmlData.Value = xmlEle.ToString();


            ObjectParameter[] obj = new ObjectParameter[] { _PRH_ID, _xmlData };
            db.ExecuteFunction("POR_SP_mRequestTemplateDetail", obj);

            ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
        }

        public mRequestTemplateHead GetTemplateOrderHead(long TemplateID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mRequestTemplateHead TempHead = new mRequestTemplateHead();
            TempHead = db.mRequestTemplateHeads.Where(t => t.ID == TemplateID).FirstOrDefault();
            if (TempHead != null)
            {
                db.mRequestTemplateHeads.Detach(TempHead);
            }
            return TempHead;
        }

        public DataSet GetTemplatePartLstByTemplateID(long TemplateID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from mRequestTemplateDetail where Templateheadid=" + TemplateID + "", conn);
            return ds;
        }

        public void UpdatetApprovalTransAfterApproval(long ApprovalID, long RequestID, long statusID, string Remark, long ApproverID, string InvoiceNo, string[] conn)
        {
            //int Result = 0;
            //update tApprovalTrans set Remark='"+ Remark +"',Date="+ DateTime.Now  +",Status="+ statusID +" where orderid=" + RequestID  + " and ApproverID="+ ApproverID +" and Id="+ ApprovalID +" 
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            /*Update InvoiceNo in tOrderHead*/
            if (InvoiceNo != "")
            {
                DataSet dsEditAccessRemove = new DataSet();
                dsEditAccessRemove = fillds("update torderhead set InvoiceNo='" + InvoiceNo + "' where id=" + RequestID + "", conn);
            }
            /*Update InvoiceNo in tOrderHead*/

            //change tOrderwiseAccess to 0 after approve order
            tOrderWiseAccess OrdrAccess = new tOrderWiseAccess();
            OrdrAccess = db.tOrderWiseAccesses.Where(a => a.OrderID == RequestID && a.UserApproverID == ApproverID && a.ApprovalLevel == ApprovalID).FirstOrDefault();
            if (OrdrAccess != null)
            {
                db.tOrderWiseAccesses.Detach(OrdrAccess);
                OrdrAccess.PriceEdit = false;
                OrdrAccess.SkuQtyEdit = false;

                db.tOrderWiseAccesses.Attach(OrdrAccess);
                db.ObjectStateManager.ChangeObjectState(OrdrAccess, EntityState.Modified);
                db.SaveChanges();
            }

            tApprovalTran rec = new tApprovalTran();
            //rec = db.tApprovalTrans.Where(a => a.Id == ApprovalID && a.OrderId == RequestID && a.ApproverID == ApproverID).FirstOrDefault();

            DataSet dsDeligateApprover = new DataSet();
            long DeligateFrom = 0;

            dsDeligateApprover = fillds("select * from VW_ApprovalTransDetails where   OrderID=" + RequestID + " and DeligateTo=" + ApproverID + " and Status!=3", conn);
            //  dsDeligateApprover = fillds("select * from VW_ApprovalTransDetails where   OrderID=" + RequestID + " and (DeligateTo=" + ApproverID + " OR ApproverID=" + ApproverID + ") and Status!=3", conn);
            if (dsDeligateApprover.Tables[0].Rows.Count > 0)
            {
                DeligateFrom = long.Parse(dsDeligateApprover.Tables[0].Rows[0]["DeligateFrom"].ToString());
                ApproverID = DeligateFrom;

                rec = db.tApprovalTrans.Where(a => a.OrderId == RequestID && a.ApproverID == ApproverID).FirstOrDefault();   //Change Add ApprovalId
                if (rec != null)
                {
                    db.tApprovalTrans.Detach(rec);
                    rec.Remark = Remark;
                    rec.Date = DateTime.Now;
                    rec.Status = statusID;

                    db.tApprovalTrans.Attach(rec);
                    db.ObjectStateManager.ChangeObjectState(rec, EntityState.Modified);
                    db.SaveChanges();
                }
            }
            else
            {
                rec = db.tApprovalTrans.Where(a => a.OrderId == RequestID && a.ApproverID == ApproverID && a.ApprovalId == ApprovalID).FirstOrDefault();    //Change Add ApprovalId
                if (rec != null)
                {
                    db.tApprovalTrans.Detach(rec);
                    rec.Remark = Remark;
                    rec.Date = DateTime.Now;
                    rec.Status = statusID;

                    db.tApprovalTrans.Attach(rec);
                    db.ObjectStateManager.ChangeObjectState(rec, EntityState.Modified);
                    db.SaveChanges();
                }
            }

            //Send mail to approver, second approver if any, second level approver(s) if any & requester

            long DeptID = Convert.ToInt64(rec.StoreId);
            DataSet ds1 = new DataSet();
            // ds1 = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and UserID=" + ApproverID + "", conn);//Change  select * from tOrderWiseAccess where OrderId=1062 and UserApproverID=128  and ApprovalLevel=(select ApprovalId from tapprovalTrans where OrderId=1062 and ApproverID=128)
            ds1 = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + " and UserApproverID=" + ApproverID + "  and ApprovalLevel=" + ApprovalID + "", conn);

            string ApproverLogic = ds1.Tables[0].Rows[0]["ApproverLogic"].ToString();
            int ApprovalLevel = Convert.ToInt16(ds1.Tables[0].Rows[0]["ApprovalLevel"].ToString());

            DataSet ds2 = new DataSet();
            long SecondApproverID = 0;
            long status = 0;
            //ds2 = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and UserID!=" + ApproverID + " and Approvallevel=" + ApprovalLevel + "", conn);//Change  select * from tOrderWiseAccess where OrderId=1062 and UserApproverID!=128 and ApprovalLevel=1
            ds2 = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + " and UserApproverID!=" + ApproverID + " and ApprovalLevel=" + ApprovalLevel + " ", conn);
            DataSet ds3 = new DataSet();
            if (ds2.Tables[0].Rows.Count > 0)
            {  //Require For Loop Here
                //SecondApproverID = Convert.ToInt64(ds2.Tables[0].Rows[0]["UserID"].ToString());
                SecondApproverID = Convert.ToInt64(ds2.Tables[0].Rows[0]["UserApproverID"].ToString());
                ds3 = fillds("select * from VW_ApprovalTransDetails where (DeligateTo=" + SecondApproverID + " or ApproverID=" + SecondApproverID + ") and OrderID=" + RequestID + " and ApprovalId=" + ApprovalLevel + "", conn);
                status = Convert.ToInt64(ds3.Tables[0].Rows[0]["Status"].ToString());
            }
            else
            {
                ds3 = fillds("select * from VW_ApprovalTransDetails where  OrderID=" + RequestID + "", conn);
                status = Convert.ToInt64(ds3.Tables[0].Rows[0]["Status"].ToString());
            }

            //if (status > 2)
            if ((status == 3) || (status == 24))
            {
                //if next approval level is available then get o.w. change Request status to Approved
                DataSet ds4 = new DataSet();
                ApprovalLevel++;
                // ds4 = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and ApprovalLevel=" + ApprovalLevel + "", conn); //Change  select * from tOrderWiseAccess where OrderId=1062 and ApprovalLevel=1
                ds4 = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + " and ApprovalLevel=" + ApprovalLevel + " ", conn);
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= ds4.Tables[0].Rows.Count - 1; i++)
                    {
                        tApprovalTran AppTran = new tApprovalTran();
                        AppTran.OrderId = RequestID;
                        AppTran.UserId = Convert.ToInt64(ds3.Tables[0].Rows[0]["UserId"].ToString());
                        AppTran.StoreId = DeptID;
                        AppTran.ApprovalId = ApprovalLevel;
                        AppTran.ApproverID = Convert.ToInt64(ds4.Tables[0].Rows[i]["UserApproverID"].ToString());   //Old UserID   New UserApproverID
                        if (ApprovalLevel == 2) { AppTran.Status = 21; }
                        else if (ApprovalLevel == 3) { AppTran.Status = 22; }
                        else { AppTran.Status = 22; }
                        db.tApprovalTrans.AddObject(AppTran);
                        db.SaveChanges();

                        //Send mail to Approvers 
                        EmailSendToApprover(Convert.ToInt64(ds4.Tables[0].Rows[i]["UserApproverID"].ToString()), RequestID, conn);   //Old UserID   New UserApproverID
                    }

                    tOrderHead Order = new tOrderHead();
                    Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
                    if (Order != null)
                    {
                        db.tOrderHeads.Detach(Order);
                        if (ApprovalLevel == 2) { Order.Status = 21; }
                        else if (ApprovalLevel == 3) { Order.Status = 22; }
                        // Order.Status = status;

                        db.tOrderHeads.Attach(Order);
                        db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                        db.SaveChanges();
                    }

                    tCorrespond Cor = new tCorrespond();
                    Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
                    if (Cor != null)
                    {
                        db.tCorresponds.Detach(Cor);
                        if (ApprovalLevel == 2) { Cor.CurrentOrderStatus = 21; }
                        else if (ApprovalLevel == 3) { Cor.CurrentOrderStatus = 22; }
                        db.tCorresponds.Attach(Cor);
                        db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                        db.SaveChanges();
                    }

                    EmailSendWhenRequestSubmit(RequestID, conn);

                }
                else
                {
                    /*Add Cost Center Approver*/

                    //change request status to approved.
                    tOrderHead Order = new tOrderHead();
                    Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
                    if (Order != null)
                    {
                        db.tOrderHeads.Detach(Order);
                        Order.Status = status;
                        Order.ApprovalDate = DateTime.Now;

                        db.tOrderHeads.Attach(Order);
                        db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                        db.SaveChanges();
                    }

                    tCorrespond Cor = new tCorrespond();
                    Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
                    if (Cor != null)
                    {
                        db.tCorresponds.Detach(Cor);
                        Cor.CurrentOrderStatus = status;

                        db.tCorresponds.Attach(Cor);
                        db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                        db.SaveChanges();
                    }

                    //Send Email of Status Approved      New Change Send Email To all Approvers 

                    DataSet dsAllApplLevel = new DataSet();
                    dsAllApplLevel = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + "   and ApprovalLevel>0", conn);
                    int ApplCnt = dsAllApplLevel.Tables[0].Rows.Count;
                    if (ApplCnt > 0)
                    {
                        for (int a = 0; a <= ApplCnt - 1; a++)
                        {
                            EmailSendofApproved(Convert.ToInt64(dsAllApplLevel.Tables[0].Rows[a]["UserApproverID"].ToString()), RequestID, conn);
                            //EmailSendofApproved(Convert.ToInt64(ds1.Tables[0].Rows[a]["UserApproverID"].ToString()), RequestID, conn);  ///Old UserID   New UserApproverID  
                        }
                    }
                    EmailSendWhenRequestSubmit(RequestID, conn);

                    //Add Message into mMessageTrans Table After Approve Order
                    AddIntomMessageTrans(RequestID, conn);

                    //Update tProductStockDetails TotalDispatchQty
                    UpdateTProductStockReserveQtyTotalDispatchQty(RequestID, conn);

                }
            }
            else //if (status == 2)
            {
                DataSet dsSAI = new DataSet();
                long SecondApproverID2 = 0;
                // dsSAI = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and UserID!=" + ApproverID + " and Approvallevel=" + ApprovalLevel + "", conn);  //Change  select * from tOrderWiseAccess where OrderId=1062 and UserApproverID!=128 and ApprovalLevel=1
                dsSAI = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + " and UserApproverID!=" + ApproverID + " and ApprovalLevel=" + ApprovalLevel + " ", conn);
                int CntdsSAI = dsSAI.Tables[0].Rows.Count;

                if (ApproverLogic == "AND")
                {
                    //Send Mail of pending approval
                    // EmailSendToApprover(SecondApproverID, RequestID, conn);

                    if (CntdsSAI > 0)
                    {
                        for (int d = 0; d <= CntdsSAI - 1; d++)
                        {
                            SecondApproverID2 = Convert.ToInt64(dsSAI.Tables[0].Rows[d]["UserApproverID"].ToString()); ////Old UserID   New UserApproverID
                            EmailSendToApprover(SecondApproverID2, RequestID, conn);
                        }
                    }
                }
                else if (ApproverLogic == "OR")
                {
                    //SecondApproverID
                    if (CntdsSAI > 0)
                    {
                        for (int d = 0; d <= CntdsSAI - 1; d++)
                        {
                            SecondApproverID2 = Convert.ToInt64(dsSAI.Tables[0].Rows[d]["UserApproverID"].ToString());  ////Old UserID   New UserApproverID
                            rec = db.tApprovalTrans.Where(a => a.OrderId == RequestID && a.ApproverID == SecondApproverID2).FirstOrDefault();
                            // rec = db.tApprovalTrans.Where(a => a.OrderId == RequestID && a.ApproverID == SecondApproverID).FirstOrDefault();
                            if (rec != null)
                            {
                                db.tApprovalTrans.Detach(rec);
                                rec.Remark = Remark;
                                rec.Date = DateTime.Now;
                                rec.Status = statusID;

                                db.tApprovalTrans.Attach(rec);
                                db.ObjectStateManager.ChangeObjectState(rec, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }
                    }

                    //Update Request Status
                    DataSet ds4 = new DataSet();
                    ApprovalLevel++;
                    // ds4 = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and ApprovalLevel=" + ApprovalLevel + "", conn); //Change select * from tOrderWiseAccess where OrderId=1062  and ApprovalLevel=1
                    ds4 = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + "  and ApprovalLevel=" + ApprovalLevel + " ", conn);
                    if (ds4.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= ds4.Tables[0].Rows.Count - 1; i++)
                        {
                            tApprovalTran AppTran = new tApprovalTran();
                            AppTran.OrderId = RequestID;
                            AppTran.UserId = Convert.ToInt64(ds3.Tables[0].Rows[0]["UserId"].ToString());
                            AppTran.StoreId = DeptID;
                            AppTran.ApprovalId = ApprovalLevel;
                            AppTran.ApproverID = Convert.ToInt64(ds4.Tables[0].Rows[i]["UserApproverID"].ToString());  ////Old UserID   New UserApproverID
                            if (ApprovalLevel == 2) { AppTran.Status = 21; }
                            else if (ApprovalLevel == 3) { AppTran.Status = 22; }
                            else { AppTran.Status = 22; }
                            db.tApprovalTrans.AddObject(AppTran);
                            db.SaveChanges();

                            //Send mail to Approvers 
                            EmailSendToApprover(Convert.ToInt64(ds4.Tables[0].Rows[i]["UserApproverID"].ToString()), RequestID, conn);  ////Old UserID   New UserApproverID
                        }

                        if (ApprovalLevel >= 3)
                        {
                            DataSet ds6 = new DataSet();

                            // ds6 = fillds("select * from VW_ApprovalLevelDetail where DepartmentID=" + DeptID + " and ApprovalLevel=" + ApprovalLevel + "", conn); //Change select * from tOrderWiseAccess where OrderId=1062  and ApprovalLevel=1
                            ds6 = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + "  and ApprovalLevel=" + ApprovalLevel + "", conn);
                            if (ds6.Tables[0].Rows.Count > 0)
                            {

                            }
                            else
                            {

                                //Change request status to Approved
                                tOrderHead Order = new tOrderHead();
                                Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
                                if (Order != null)
                                {
                                    db.tOrderHeads.Detach(Order);
                                    Order.Status = status;
                                    Order.ApprovalDate = DateTime.Now;

                                    db.tOrderHeads.Attach(Order);
                                    db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                                    db.SaveChanges();
                                }

                                tCorrespond Cor = new tCorrespond();
                                Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
                                if (Cor != null)
                                {
                                    db.tCorresponds.Detach(Cor);
                                    Cor.CurrentOrderStatus = status;

                                    db.tCorresponds.Attach(Cor);
                                    db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                                    db.SaveChanges();
                                }

                                //select * from tOrderWiseAccess where OrderId=1073   and ApprovalLevel>0
                                DataSet dsAllApplLevel = new DataSet();
                                dsAllApplLevel = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + "   and ApprovalLevel>0", conn);
                                int ApplCnt = dsAllApplLevel.Tables[0].Rows.Count;
                                if (ApplCnt > 0)
                                {
                                    for (int a = 0; a <= ApplCnt-1; a++)
                                    {
                                        EmailSendofApproved(Convert.ToInt64(dsAllApplLevel.Tables[0].Rows[a]["UserApproverID"].ToString()), RequestID, conn);
                                        // EmailSendofApproved(Convert.ToInt64(ds1.Tables[0].Rows[0]["UserApproverID"].ToString()), RequestID, conn);  ////Old UserID   New UserApproverID
                                    }
                                }

                                EmailSendWhenRequestSubmit(RequestID, conn);

                                /* After request approved delete records of Auto cancellation & Approval Reminder START */
                                tCorrespond COR = new tCorrespond();
                                COR = db.tCorresponds.Where(i => i.OrderHeadId == RequestID && i.MessageType == "Reminder").FirstOrDefault();
                                db.tCorresponds.DeleteObject(COR);
                                /* After request approved delete records of Auto cancellation & Approval Reminder END */

                                //Add Message into mMessageTrans Table After Approve Order
                                AddIntomMessageTrans(RequestID, conn);

                                //Update tProductStockDetails TotalDispatchQty
                                UpdateTProductStockReserveQtyTotalDispatchQty(RequestID, conn);
                            }
                        }
                        else
                        {
                            tOrderHead Order = new tOrderHead();
                            Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
                            if (Order != null)
                            {
                                db.tOrderHeads.Detach(Order);
                                if (ApprovalLevel == 2) { Order.Status = 21; }
                                //else if (ApprovalLevel == 3) { Order.Status = 22; }
                                Order.Status = status;

                                db.tOrderHeads.Attach(Order);
                                db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                                db.SaveChanges();
                            }

                            tCorrespond Cor = new tCorrespond();
                            Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
                            if (Cor != null)
                            {
                                db.tCorresponds.Detach(Cor);
                                Cor.CurrentOrderStatus = status;

                                db.tCorresponds.Attach(Cor);
                                db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        //Change request status to Approved
                        tOrderHead Order = new tOrderHead();
                        Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
                        if (Order != null)
                        {
                            db.tOrderHeads.Detach(Order);
                            //Order.Status = status;
                            Order.Status = 3;
                            Order.ApprovalDate = DateTime.Now;

                            db.tOrderHeads.Attach(Order);
                            db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                            db.SaveChanges();
                        }

                        tCorrespond Cor = new tCorrespond();
                        Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
                        if (Cor != null)
                        {
                            db.tCorresponds.Detach(Cor);
                            Cor.CurrentOrderStatus = 3;

                            db.tCorresponds.Attach(Cor);
                            db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                            db.SaveChanges();
                        }


                        DataSet dsAllApplLevel = new DataSet();
                        dsAllApplLevel = fillds("select * from tOrderWiseAccess where OrderId=" + RequestID + "   and ApprovalLevel>0", conn);
                        int ApplCnt = dsAllApplLevel.Tables[0].Rows.Count;
                        if (ApplCnt > 0)
                        {
                            for (int a = 0; a <= ApplCnt-1; a++)
                            {
                                EmailSendofApproved(Convert.ToInt64(dsAllApplLevel.Tables[0].Rows[a]["UserApproverID"].ToString()), RequestID, conn);
                                // EmailSendofApproved(Convert.ToInt64(ds1.Tables[0].Rows[0]["UserApproverID"].ToString()), RequestID, conn);   ////Old UserID   New UserApproverID
                            }
                        }


                        EmailSendWhenRequestSubmit(RequestID, conn);

                        /* After request approved delete records of Auto cancellation & Approval Reminder START */
                        tCorrespond COR = new tCorrespond();
                        COR = db.tCorresponds.Where(i => i.OrderHeadId == RequestID && i.MessageType == "Reminder").FirstOrDefault();
                        db.tCorresponds.DeleteObject(COR);
                        /* After request approved delete records of Auto cancellation & Approval Reminder END */

                        //Add Message into mMessageTrans Table After Approve Order
                        AddIntomMessageTrans(RequestID, conn);

                        //Update tProductStockDetails TotalDispatchQty
                        UpdateTProductStockReserveQtyTotalDispatchQty(RequestID, conn);
                    }
                }
            }

        }

        public void UpdatetApprovalTransAfterReject(long ApprovalID, long RequestID, long statusID, string Remark, long ApproverID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            //change tOrderwiseAccess to 0 after approve order
            DataSet dsEditAccessRemove = new DataSet();
            dsEditAccessRemove = fillds("update torderwiseaccess set PriceEdit=0,SkuQtyEdit=0 where orderid=" + RequestID + "", conn);

            tApprovalTran rec = new tApprovalTran();
            // rec = db.tApprovalTrans.Where(a => a.Id == ApprovalID && a.OrderId == RequestID && a.ApproverID == ApproverID).FirstOrDefault();
            rec = db.tApprovalTrans.Where(a => a.OrderId == RequestID && a.ApproverID == ApproverID).FirstOrDefault();
            if (rec != null)
            {
                db.tApprovalTrans.Detach(rec);
                rec.Remark = Remark;
                rec.Date = DateTime.Now;
                rec.Status = statusID;

                db.tApprovalTrans.Attach(rec);
                db.ObjectStateManager.ChangeObjectState(rec, EntityState.Modified);
                db.SaveChanges();
            }

            tOrderHead Order = new tOrderHead();
            Order = db.tOrderHeads.Where(r => r.Id == RequestID).FirstOrDefault();
            if (Order != null)
            {
                db.tOrderHeads.Detach(Order);
                Order.Status = statusID;

                db.tOrderHeads.Attach(Order);
                db.ObjectStateManager.ChangeObjectState(Order, EntityState.Modified);
                db.SaveChanges();
            }

            tCorrespond Cor = new tCorrespond();
            Cor = db.tCorresponds.Where(c => c.OrderHeadId == RequestID).FirstOrDefault();
            if (Cor != null)
            {
                db.tCorresponds.Detach(Cor);
                Cor.CurrentOrderStatus = statusID;

                db.tCorresponds.Attach(Cor);
                db.ObjectStateManager.ChangeObjectState(Cor, EntityState.Modified);
                db.SaveChanges();
            }

            //Send mail to Approvers & Requester 
            EmailSendofRequestReject(long.Parse(Order.RequestBy.ToString()), RequestID, conn);
            EmailSendWhenRequestSubmit(RequestID, conn);

            //Update Available balance of Ordered Product 
            UpdateAvailableBalanceAfterRequestReject(RequestID, conn);
        }

        protected void UpdateAvailableBalanceAfterRequestReject(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet dsPrdDetails = new DataSet();
            dsPrdDetails = fillds("Select * from tOrderDetail where OrderHeadId=" + RequestID + "", conn);
            int PrdCnt = dsPrdDetails.Tables[0].Rows.Count;
            if (PrdCnt > 0)
            {
                for (int i = 0; i <= PrdCnt - 1; i++)
                {
                    //update tProductstockDetails set ResurveQty=ResurveQty-@Qty1,TotalDispatchQty=TotalDispatchQty+@Qty1  where ProdID=@SkuId1                     
                    tProductStockDetail psd = new tProductStockDetail();
                    long SkuID = long.Parse(dsPrdDetails.Tables[0].Rows[i]["SkuId"].ToString());
                    decimal Qty = decimal.Parse(dsPrdDetails.Tables[0].Rows[i]["OrderQty"].ToString());

                    mProduct prd = new mProduct();
                    prd = db.mProducts.Where(p => p.ID == SkuID).FirstOrDefault();
                    if (prd.GroupSet == "Yes")
                    {
                        DataSet dsBomProds = new DataSet();
                        dsBomProds = fillds("select SKUId,Quantity from mBOMDetail BD where BD.BOMheaderId=" + SkuID + "", conn);
                        if (dsBomProds.Tables[0].Rows.Count > 0)
                        {
                            for (int b = 0; b <= dsBomProds.Tables[0].Rows.Count - 1; b++)
                            {
                                long bomPrd = long.Parse(dsBomProds.Tables[0].Rows[b]["SKUId"].ToString());
                                decimal bomQty = decimal.Parse(dsBomProds.Tables[0].Rows[b]["Quantity"].ToString());

                                decimal FinalQty = Qty * bomQty;

                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQtyRej";
                                cmd.Connection = svr.GetSqlConn(conn);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("SkuID", bomPrd);
                                cmd.Parameters.AddWithValue("Qty", FinalQty);
                                cmd.ExecuteNonQuery();

                                //psd = db.tProductStockDetails.Where(a => a.ProdID == bomPrd).FirstOrDefault();
                                //if (psd != null)
                                //{
                                //    db.tProductStockDetails.Detach(psd);
                                //    psd.ResurveQty = psd.ResurveQty - FinalQty;
                                //    psd.AvailableBalance = psd.AvailableBalance + FinalQty;
                                //    db.tProductStockDetails.Attach(psd);
                                //    db.ObjectStateManager.ChangeObjectState(psd, EntityState.Modified);
                                //    db.SaveChanges();
                                //}
                                InsertIntotInventory(bomPrd, RequestID, FinalQty, "Reject", conn);
                            }
                        }
                    }
                    else if (prd.GroupSet == "No")
                    {
                        //psd = db.tProductStockDetails.Where(a => a.ProdID == SkuID).FirstOrDefault();
                        //if (psd != null)
                        //{
                        //    db.tProductStockDetails.Detach(psd);
                        //    psd.ResurveQty = psd.ResurveQty - Qty;
                        //    psd.AvailableBalance = psd.AvailableBalance + Qty;
                        //    db.tProductStockDetails.Attach(psd);
                        //    db.ObjectStateManager.ChangeObjectState(psd, EntityState.Modified);
                        //    db.SaveChanges();
                        //}

                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsAvailableResurveQtyRej";
                        cmd.Connection = svr.GetSqlConn(conn);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("SkuID", SkuID);
                        cmd.Parameters.AddWithValue("Qty", Qty);
                        cmd.ExecuteNonQuery();

                        InsertIntotInventory(SkuID, RequestID, Qty, "Reject", conn);
                    }
                }
            }
        }

        protected void UpdateTProductStockReserveQtyTotalDispatchQty(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tProductStockDetail psd = new tProductStockDetail();
            try
            {
                DataSet dsPrdDetails = new DataSet();
                dsPrdDetails = fillds("Select * from tOrderDetail where OrderHeadId=" + RequestID + "", conn);
                int PrdCnt = dsPrdDetails.Tables[0].Rows.Count;
                if (PrdCnt > 0)
                {
                    for (int i = 0; i <= PrdCnt - 1; i++)
                    {
                        //update tProductstockDetails set ResurveQty=ResurveQty-@Qty1,TotalDispatchQty=TotalDispatchQty+@Qty1  where ProdID=@SkuId1
                        long SkuID = long.Parse(dsPrdDetails.Tables[0].Rows[i]["SkuId"].ToString());
                        decimal Qty = decimal.Parse(dsPrdDetails.Tables[0].Rows[i]["OrderQty"].ToString());

                        mProduct prd = new mProduct();
                        prd = db.mProducts.Where(p => p.ID == SkuID).FirstOrDefault();
                        if (prd.GroupSet == "Yes")
                        {
                            DataSet dsBomProds = new DataSet();
                            dsBomProds = fillds("select SKUId,Quantity from mBOMDetail BD where BD.BOMheaderId=" + SkuID + "", conn);
                            if (dsBomProds.Tables[0].Rows.Count > 0)
                            {
                                for (int b = 0; b <= dsBomProds.Tables[0].Rows.Count - 1; b++)
                                {
                                    long bomPrd = long.Parse(dsBomProds.Tables[0].Rows[b]["SKUId"].ToString());
                                    decimal bomQty = decimal.Parse(dsBomProds.Tables[0].Rows[b]["Quantity"].ToString());

                                    decimal FinalQty = Qty * bomQty;

                                    SqlCommand cmd = new SqlCommand();
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsDispatchResurveQty";
                                    cmd.Connection = svr.GetSqlConn(conn);
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("SkuID", bomPrd);
                                    cmd.Parameters.AddWithValue("Qty", FinalQty);
                                    cmd.ExecuteNonQuery();
                                    cmd.Connection.Close();

                                    InsertIntotInventory(bomPrd, RequestID, FinalQty, "Dispatch", conn);
                                }
                            }
                        }
                        else if (prd.GroupSet == "No")
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "GWC_SP_UpdatetProductstockDetailsDispatchResurveQty";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("SkuID", SkuID);
                            cmd.Parameters.AddWithValue("Qty", Qty);
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();

                            InsertIntotInventory(SkuID, RequestID, Qty, "Dispatch", conn);
                        }
                    }
                }
            }
            catch { }
            finally { }
        }

        public void InsertIntotInventory(long SkuID, long RequestID, decimal Qty, string TransactionType, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GWC_SP_InsertIntotInventry";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId", SkuID);
            cmd.Parameters.AddWithValue("TransactionId", RequestID);
            cmd.Parameters.AddWithValue("TransactionType", TransactionType);
            cmd.Parameters.AddWithValue("Quantity", Qty);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public long GetPreviousStatusID(long RequestId, string[] conn)
        {
            long PrvStatusId = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead Order = new tOrderHead();
            Order = db.tOrderHeads.Where(r => r.Id == RequestId).FirstOrDefault();

            PrvStatusId = long.Parse(Order.Status.ToString());
            return PrvStatusId;
        }

        public void InsertIntotCorrespond(tCorrespond Msg, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            db.tCorresponds.AddObject(Msg);
            db.SaveChanges();
        }

        public DataSet GetCorrespondance(long RequestID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from VW_CorrespondanceDetail where OrderHeadId=" + RequestID + " and MailStatus!=0 order by Id Desc ", conn);
            return ds;
        }

        public tCorrespond GetCorrespondanceDetail(long CORID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tCorrespond tCor = new tCorrespond();
            tCor = db.tCorresponds.Where(c => c.Id == CORID).FirstOrDefault();
            db.tCorresponds.Detach(tCor);
            return tCor;
        }

        public void AddIntomMessageTrans(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mMessageTran Msg = new mMessageTran();

            GWC_VW_MsgOrderHead MsgHD = new GWC_VW_MsgOrderHead();
            MsgHD = db.GWC_VW_MsgOrderHead.Where(h => h.Id == RequestID).FirstOrDefault();

            string ContactPerson2 = MsgHD.ContactP2;

            string ContactPerson2Names = "";
            if (ContactPerson2 == "NA" || ContactPerson2 == "" || ContactPerson2 == null) { ContactPerson2Names = "NA"; }
            else
            { ContactPerson2Names = GetContactPersonNames(ContactPerson2, conn); }


            Msg.MsgDescription = MsgHD.Id + " | " + MsgHD.OrderNumber + " | " + MsgHD.Orderdate.Value.ToShortDateString() + " | " + MsgHD.Deliverydate.Value.ToShortDateString() + " | " + MsgHD.StoreCode + " | " + MsgHD.Con1 + " | " + ContactPerson2Names + " | " + MsgHD.AddressLine1 + " | " + MsgHD.Remark + " | " + MsgHD.OrderNo + " | " + MsgHD.InvoiceNo + " | " + MsgHD.LocationCode + " | " + MsgHD.RequestorName + " | " + MsgHD.RequestorMobileNo + " | " + MsgHD.ConsigneeName + " | " + MsgHD.ConsigneeAddress + " | " + MsgHD.ConsigneePhone;

            DataSet ds = new DataSet();
            ds = fillds("select * from GWC_VW_MsgOrderDetail where OrderHeadId=" + RequestID + " ", conn);

            int Cnt = ds.Tables[0].Rows.Count;
            if (Cnt > 0)
            {
                for (int i = 0; i <= Cnt - 1; i++)
                {
                    long PrdID = long.Parse(ds.Tables[0].Rows[i]["SkuId"].ToString());
                    mProduct prd = new mProduct();
                    prd = db.mProducts.Where(p => p.ID == PrdID).FirstOrDefault();
                    if (prd.GroupSet == "Yes")
                    {
                        DataSet dsBomProds = new DataSet();
                        dsBomProds = fillds("select SKUId,Quantity from mBOMDetail BD where BD.BOMheaderId=" + PrdID + "", conn);
                        if (dsBomProds.Tables[0].Rows.Count > 0)
                        {
                            for (int b = 0; b <= dsBomProds.Tables[0].Rows.Count - 1; b++)
                            {
                                decimal bomQty = decimal.Parse(dsBomProds.Tables[0].Rows[b]["Quantity"].ToString());
                                decimal Qty = decimal.Parse(ds.Tables[0].Rows[i]["OrderQty"].ToString());
                                decimal FinalQty = Qty * bomQty;

                                long bomPrd = long.Parse(dsBomProds.Tables[0].Rows[b]["SKUId"].ToString());
                                DataSet dsPrdName = new DataSet();
                                dsPrdName = fillds("select ProductCode from mproduct where ID=" + bomPrd + "", conn);
                                string ProductCode = dsPrdName.Tables[0].Rows[0]["ProductCode"].ToString();

                                Msg.MsgDescription = Msg.MsgDescription + " | " + ProductCode + " | " + ds.Tables[0].Rows[i]["UOM"].ToString() + " | " + FinalQty;
                            }
                        }
                    }
                    else if (prd.GroupSet == "No")
                    {
                        Msg.MsgDescription = Msg.MsgDescription + " | " + ds.Tables[0].Rows[i]["Prod_Code"].ToString() + " | " + ds.Tables[0].Rows[i]["UOM"].ToString() + " | " + ds.Tables[0].Rows[i]["OrderQty"].ToString();
                    }
                }
            }


            Msg.MessageHdrId = 1;
            Msg.Object = "Order";
            // Msg.Destination = "WMS";
            Msg.Destination = MsgHD.StoreCode;
            Msg.Status = 0;
            Msg.CreationDate = DateTime.Now;
            Msg.Createdby = "OMS";

            db.mMessageTrans.AddObject(Msg);
            db.SaveChanges();

        }

        public string GetContactPersonNames(string ContactPerson2, string[] conn)
        {
            string Contact1Name = "";
            DataSet ds = new DataSet();
            ds = fillds("select Name from tcontactpersondetail where ID IN( " + ContactPerson2 + " )", conn);
            int cnt = ds.Tables[0].Rows.Count;
            if (cnt > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    if (i == 0) { Contact1Name = ds.Tables[0].Rows[i]["Name"].ToString(); }
                    else
                    {
                        Contact1Name = Contact1Name + "," + ds.Tables[0].Rows[i]["Name"].ToString();
                    }
                }
            }
            return Contact1Name;
        }


        public DataSet GetBomDetails(string PrdID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select BD.Id,BD.BOMheaderId,BD.SKUId,MP.ProductCode,MP.Name,MP.Description,BD.Quantity,BD.Sequence,BD.Remark from mBOMDetail BD left outer join mProduct MP on BD.SKUId=MP.ID where BD.BOMheaderId=" + PrdID + " ", conn);
            return ds;
        }

        public string GetSelectedUom(long OrderId, long ProdID, string[] conn)
        {
            string uomid = "";
            DataSet ds = new DataSet();
            //ds = fillds("select UOMID from tOrderDetail where Orderheadid="+ OrderId +" and SkuId="+ ProdID +"", conn);
            ds = fillds("select (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " and UOMID=(select UOMID from tOrderDetail where Orderheadid=" + OrderId + " and SkuId=" + ProdID + ")", conn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                uomid = ds.Tables[0].Rows[0]["UMOGroup"].ToString();
            }
            else { uomid = "0"; }
            return uomid;
        }

        public string GetSelectedUomTemplate(long OrderId, long ProdID, string[] conn)
        {
            string uomid = "0";
            try
            {
                DataSet ds = new DataSet();
                ds = fillds("select (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " and UOMID=(select UOMID from mRequestTemplateDetail where TemplateHeadID=" + OrderId + " and PrdID=" + ProdID + ")", conn);
                uomid = ds.Tables[0].Rows[0]["UMOGroup"].ToString();
            }
            catch
            {
                DataSet ds1 = new DataSet();
                ds1 = fillds("select (CONVERT(VARCHAR(15),UOMID) + ':' + CONVERT(VARCHAR(15),Quantity)) as UMOGroup from VW_SkuUOMDetails where SkuId=" + ProdID + " ", conn);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    uomid = ds1.Tables[0].Rows[2]["UMOGroup"].ToString();
                }
                else { uomid = "0"; }

            }
            finally { }
            return uomid;
        }


        public int GridRowCount(string paraSessionID, string paraCurrentObjectName, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> finalSaveLst = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            int rCount;
            rCount = finalSaveLst.Count;
            return rCount;
        }

        public List<POR_SP_GetPartDetail_ForRequest_Result> GridRowsTemplate(string paraSessionID, string paraCurrentObjectName, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> finalSaveLst = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            return finalSaveLst;
        }


        public long ChkTemplateTitle(string TemplateTitle, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from mRequestTemplateHead where TemplateTitle='" + TemplateTitle + "'	", conn);
            long result = ds.Tables[0].Rows.Count;
            return result;
        }
        #endregion

        #region GWC_Deliveries
        public VW_OrderDeliveryDetails GetOrderDeliveryDetails(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            VW_OrderDeliveryDetails ODD = new VW_OrderDeliveryDetails();
            ODD = db.VW_OrderDeliveryDetails.Where(d => d.OrderID == OrderID).FirstOrDefault();
            db.VW_OrderDeliveryDetails.Detach(ODD);
            return ODD;
        }

        public int GetDispatchedOrders(string SelectedOrder, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from torderhead where id in(" + SelectedOrder + ") and status=8 ", conn);
            int cnt = ds.Tables[0].Rows.Count;
            return cnt;
        }

        public DataSet GetDriverDetails(string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select ID,FirstName+' '+LastName Name,EmailID,MobileNo from muserprofilehead where Usertype='Driver' ", conn);
            return ds;
        }

        public DataSet GetFilteredDriverList(string SearchValue, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select ID,FirstName+' '+LastName Name,EmailID,MobileNo from muserprofilehead where Usertype='Driver' and (Firstname like '%" + SearchValue + "%' or LastName like '%" + SearchValue + "%' or EmailID like '%" + SearchValue + "%') ", conn);
            return ds;
        }

        public int AssignSelectedDriver(long orderNo, long DriverID, string TruckDetails, long AssignBy, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_AssignDriverToOrder";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DriverId", DriverID);
            cmd.Parameters.AddWithValue("TruckDetail", TruckDetails);
            cmd.Parameters.AddWithValue("AssignBy", AssignBy);
            cmd.Parameters.AddWithValue("OrderId", orderNo);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            EmailSendofRequestOutForDelivery(orderNo, conn);

            return 1;
        }
        #endregion

        #region GWCVer2
        public DataSet GetDeptWisePaymentMethod(long selectedDept, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("SELECT  DPM.ID, DPM.DeptID, DPM.PMethodID, DPM.Sequence,PM.MethodName FROM mDeptPaymentMethod DPM left outer join mPaymentMethodMain PM on DPM.PMethodID=PM.ID where DPM.DeptID=" + selectedDept + "", conn);
            return ds;
        }

        public DataSet GetPaymentMethodFields(long SelpaymentMethod, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("SELECT  ID, PMethodID, FieldName, ControlType, Mandetory, Query, Sequence, '' as StatutoryValue FROM mPaymentMethodDetail where PMethodID=" + SelpaymentMethod + "", conn);
            return ds;
        }

        public List<mPaymentMethodDetail> GetPMFields(long SelpaymentMethod, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mPaymentMethodDetail> PMD = new List<mPaymentMethodDetail>();
            PMD = db.mPaymentMethodDetails.Where(p => p.PMethodID == SelpaymentMethod).ToList();
            return PMD;
        }

        public List<VW_DeptWisePaymentMethod> GEtDeptPaymentmethod(long selectedDept, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<VW_DeptWisePaymentMethod> PMD = new List<VW_DeptWisePaymentMethod>();
            PMD = db.VW_DeptWisePaymentMethod.Where(d => d.DeptID == selectedDept).ToList();
            return PMD;
        }

        public List<mCostCenterMain> GetCostCenter(long DeptId, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mTerritory cmpny = new mTerritory();
            cmpny = (from c in db.mTerritories
                     where c.ID == DeptId
                     select c).FirstOrDefault();

            long cmpnyID = long.Parse(cmpny.ParentID.ToString());

            List<mCostCenterMain> cost = new List<mCostCenterMain>();
            cost = db.mCostCenterMains.Where(t => t.CompanyID == cmpnyID).ToList();
            return cost;
        }

        public decimal GetTotalFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> getRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result updateRec = new POR_SP_GetPartDetail_ForRequest_Result();
            totPrice = getRec.Sum(s => s.Total);
            return totPrice;
        }

        public decimal GetTotalQTYFromTempData(string SessionID, string CurrentObjectName, string UserID, string[] conn)
        {
            decimal totPrice = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_GetPartDetail_ForRequest_Result> getRec = new List<POR_SP_GetPartDetail_ForRequest_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            POR_SP_GetPartDetail_ForRequest_Result updateRec = new POR_SP_GetPartDetail_ForRequest_Result();
            totPrice = getRec.Sum(s => s.RequestQty);
            return totPrice;
        }

        public long GetMaxDeliveryDaysofDept(long Dept, string[] conn)
        {
            long mdd = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mTerritory d = new mTerritory();
            d = (from c in db.mTerritories
                 where c.ID == Dept
                 select c).FirstOrDefault();
            mdd = long.Parse(d.MaxDeliveryDays.ToString());
            return mdd;
        }

        public string GetMandatoryFields(long pm, string[] conn)
        {
            string seq = "";
            DataSet ds = new DataSet();
            ds = fillds("select Sequence from mPaymentMethodDetail where PMethodID=" + pm + " and Mandetory>0", conn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    if (i == 0) { seq = ds.Tables[0].Rows[i]["Sequence"].ToString(); }
                    else
                    {
                        seq = seq + ',' + ds.Tables[0].Rows[i]["Sequence"].ToString();
                    }
                }
            }
            return seq;
        }

        public long GetStatutoryID(string PMLabel, long pmID, string[] conn)
        {
            long PMDID = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mPaymentMethodDetail pmd = new mPaymentMethodDetail();
            pmd = (from d in db.mPaymentMethodDetails
                   where d.FieldName == PMLabel && d.PMethodID == pmID
                   select d).FirstOrDefault();
            PMDID = long.Parse(pmd.ID.ToString());
            return PMDID;
        }

        public void AddIntotStatutory(tStatutoryDetail pmd, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long ReqID = pmd.ReferenceID;
            //db.tStatutoryDetails.AddObject(pmd);
            //db.SaveChanges();

            SqlCommand c = new SqlCommand();
            c.CommandType = CommandType.StoredProcedure;
            c.CommandText = "SP_InsertUpdate_update tStatutoryDetail";
            c.Connection = svr.GetSqlConn(conn);
            c.Parameters.Clear();
            c.Parameters.AddWithValue("ObjectName", pmd.ObjectName);
            c.Parameters.AddWithValue("ReferenceID", pmd.ReferenceID);
            c.Parameters.AddWithValue("StatutoryID", pmd.StatutoryID);
            c.Parameters.AddWithValue("StatutoryValue", pmd.StatutoryValue);
            c.Parameters.AddWithValue("CreatedBy", pmd.CreatedBy);
            c.Parameters.AddWithValue("Sequence", pmd.Sequence);
            c.Parameters.AddWithValue("ApproverID", pmd.ApproverID);
            c.ExecuteNonQuery();

            tOrderHead oh = new tOrderHead();
            oh = db.tOrderHeads.Where(i => i.Id == ReqID).FirstOrDefault();
            long StatusID = Convert.ToInt64(oh.Status);
            if (StatusID == 1) { }
            else
            {
                SqlCommand cmd3 = new SqlCommand();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "SP_PaymentMethodFOC";
                cmd3.Connection = svr.GetSqlConn(conn);
                cmd3.Parameters.Clear();
                cmd3.Parameters.AddWithValue("OrderID", ReqID);
                cmd3.ExecuteNonQuery();
            }
        }

        public int GetDeptPriceChange(long deptID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mTerritory rec = new mTerritory();
            rec = db.mTerritories.Where(t => t.ID == deptID).FirstOrDefault();
            bool prch = Convert.ToBoolean(rec.PriceChange);

            int PC = 0;
            //DataSet ds = new DataSet();
            //ds = fillds("select PriceChange from mterritory where ID="+ deptID +"", conn);
            //string pricechng =ds.Tables[0].Rows[0]["PriceChange"].ToString();
            //if (pricechng == "" || pricechng == null) { PC = 0; }
            //else 
            if (prch == false) { PC = 0; }
            else if (prch == true) { PC = 1; }
            return PC;
        }

        public string GetNewOrderNo(long StoreId, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from FN_GetOrderNo(" + StoreId + ")", conn);
            string NewONO = ds.Tables[0].Rows[0]["NewOrderNo"].ToString();
            return NewONO;
        }

        public void UpdateStatutoryDetails(long RequestID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("update tStatutoryDetail set ReferenceID=" + RequestID + " where ReferenceID=0", conn);
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

        public DataSet GetSelectedCostCenter(long RequestId, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select StatutoryValue from tStatutoryDetail	where ReferenceID=" + RequestId + "", conn);
            //long SelCostCenterID = long.Parse(ds.Tables[0].Rows[0]["StatutoryValue"].ToString());
            return ds;
        }

        public DataSet GetAddedAdditionalFields(long RequestID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select SD.ID,SD.ObjectName,SD.ReferenceID,SD.StatutoryID,ST.Name FieldName,SD.StatutoryValue,SD.Active,SD.CreatedBy,SD.CreatedDate,SD.CompanyID from tStatutoryDetail	SD left outer join mStatutory	ST on SD.StatutoryID=ST.ID	where SD.ReferenceID=" + RequestID + "", conn);
            return ds;
        }

        public int GetPartAccessofUser(long requestID, long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from torderwiseaccess where OrderID=" + requestID + " and  UserApproverID=" + UserID + " and (PriceEdit=1 or SkuQtyEdit=1)", conn);
            int AccessYN = ds.Tables[0].Rows.Count;
            return AccessYN;
        }

        public DataSet GetProductOfOrder(long OrderID, int Sequence, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select OD.Id, OD.OrderHeadId,OD.SkuId,OD.OrderQty,OD.UOMID,OD.Sequence,OD.Prod_Name,OD.Prod_Description,OD.Prod_Code,OD.Price,OD.Total,OD.IsPriceChange,P.moq,PSD.AvailableBalance,PSD.ResurveQty from torderdetail OD left outer join mProduct P on OD.SkuId=P.Id left outer join tProductStockDetails PSD on OD.SkuId=PSD.ProdID  where OD.OrderHeadId=" + OrderID + " and OD.Sequence=" + Sequence + "", conn);
            return ds;
        }

        public DataSet GetOrderProductAccess(long requestID, long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from torderwiseaccess where OrderID=" + requestID + " and  UserApproverID=" + UserID + " and (PriceEdit=1 or SkuQtyEdit=1)", conn);
            return ds;
        }

        public int IsPriceEditYN(long OrderID, long UserID, string[] conn)
        {
            int ChangePrice = 0;
            DataSet ds = new DataSet();
            ds = fillds("select * from torderwiseaccess where OrderID=" + OrderID + " and  UserApproverID=" + UserID + " and PriceEdit=1 ", conn);
            int cnt = ds.Tables[0].Rows.Count;
            if (cnt > 0) { ChangePrice = 1; } else { ChangePrice = 0; }
            return ChangePrice;
        }
        public int IsSkuChangeYN(long OrderID, long UserID, string[] conn)
        {
            int ChangeSku = 0;
            DataSet ds = new DataSet();
            ds = fillds("select * from torderwiseaccess where OrderID=" + OrderID + " and  UserApproverID=" + UserID + " and SkuQtyEdit=1 ", conn);
            int cnt = ds.Tables[0].Rows.Count;
            if (cnt > 0) { ChangeSku = 1; } else { ChangeSku = 0; }
            return ChangeSku;
        }

        public DataSet GetQtyofSelectedUOM(long SelectedProduct, long SelectedUOM, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select Quantity from VW_SkuUOMDetails where SkuId=" + SelectedProduct + " and UOMID=" + SelectedUOM + "", conn);
            return ds;
        }

        public int UpdateOrderQtyTotal(decimal OrderQty, decimal Price, decimal Total, long OrderID, int Sequence, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_Update_OrderQtyTotal";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("OrderQty", OrderQty);
            cmd.Parameters.AddWithValue("Price", Price);
            cmd.Parameters.AddWithValue("Total", Total);
            cmd.Parameters.AddWithValue("orderid", OrderID);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            return 1;
        }

        protected void EmailSendofRequestOutForDelivery(long RequestID, string[] conn)
        {
            string MailSubject = "";
            string MailBody = "";
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
            Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

            long Orderstatus = long.Parse(Request.Status.ToString());
            long DepartmentID = long.Parse(Request.SiteID.ToString());
            long Status = long.Parse(Request.Status.ToString());
            int SMail = 0;

            DataSet dsMailSubBody = new DataSet();
            dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=25) and MessageID=(select Id from mDropdownValues where Value='Information') and DepartmentID=" + DepartmentID + "", conn);
            string MsgTTL = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString();
            DataSet dsChkSendMail = new DataSet();
            dsChkSendMail = fillds("select MailStatus from tCorrespond where OrderHeadID=" + RequestID + " and MessageTitle='" + MsgTTL + "'", conn);
            if (dsChkSendMail.Tables[0].Rows.Count > 0)
            {
                SMail = Convert.ToInt16(dsChkSendMail.Tables[0].Rows[0]["MailStatus"].ToString());
                if (SMail == 1) { }
                else
                {
                    DataSet dsOrderApprovers = new DataSet();
                    dsOrderApprovers = fillds("select UserApproverID from torderwiseaccess where OrderID=" + RequestID + "", conn);
                    int dscnt = dsOrderApprovers.Tables[0].Rows.Count;
                    if (dscnt > 0)
                    {
                        for (int i = 0; i <= dscnt - 1; i++)
                        {
                            MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

                            MailBody = "Dear " + GetUserNameByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn) + ",";
                            MailBody = dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                            MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                            MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                            MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                            SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn));
                        }
                    }
                    long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
                    AdditionalDistribution(RequestID, TemplateID, conn);
                    SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
                }

            }
            else
            {
                DataSet dsOrderApprovers = new DataSet();
                dsOrderApprovers = fillds("select UserApproverID from torderwiseaccess where OrderID=" + RequestID + "", conn);
                int dscnt = dsOrderApprovers.Tables[0].Rows.Count;
                if (dscnt > 0)
                {
                    for (int i = 0; i <= dscnt - 1; i++)
                    {
                        MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

                        MailBody = "Dear " + GetUserNameByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn) + ",";
                        MailBody = dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                        MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                        MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                        MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                        SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(Convert.ToInt64(dsOrderApprovers.Tables[0].Rows[i]["UserApproverID"].ToString()), conn));
                    }
                }
                long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
                AdditionalDistribution(RequestID, TemplateID, conn);
                SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
            }
        }

        public List<tOrderHead> GetOrderHeadByOrderIDQTYTotal(long OrderID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tOrderHead> PartReq = new List<tOrderHead>();
            PartReq = db.tOrderHeads.Where(r => r.Id == OrderID).ToList();
            //db.tOrderHeads.Detach(PartReq);
            return PartReq;
        }

        public int CancelSelectedOrder(long SelectedOrder, long UserID, string[] conn)
        {
            int result = 0;
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tOrderHead PartReq = new tOrderHead();
            PartReq = db.tOrderHeads.Where(c => c.Id == SelectedOrder && c.RequestBy == UserID).FirstOrDefault();
            if (PartReq != null)
            {
                long OrderStatus = long.Parse(PartReq.Status.ToString());
                if (OrderStatus == 2 || OrderStatus == 21 || OrderStatus == 22)
                {
                    DataSet ds = new DataSet();
                    ds = fillds("update torderhead set Status=28 where id=" + SelectedOrder + "", conn); // Update Order Status
                    UpdateAvailableBalanceAfterRequestReject(SelectedOrder, conn); // Update Stock
                    SendEmailWhenOrderCancelByRequestor(SelectedOrder, conn);                   //Email 
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

        public void SendEmailWhenOrderCancelByRequestor(long RequestID, string[] conn)
        {
            string MailSubject;
            string MailBody = "";
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            GWC_SP_GetRequestHeadByRequestIDs_Result Request = new GWC_SP_GetRequestHeadByRequestIDs_Result();
            Request = db.GWC_SP_GetRequestHeadByRequestIDs(RequestID.ToString()).FirstOrDefault();

            long Orderstatus = long.Parse(Request.Status.ToString());
            long DepartmentID = long.Parse(Request.SiteID.ToString());
            long Status = long.Parse(Request.Status.ToString());
            long UserID = 0;

            DataSet dsMailSubBody = new DataSet();
            dsMailSubBody = fillds("select * from mMessageEMailTemplates where ActivityID=(select Id from mDropdownValues where StatusID=28) and MessageID=(select Id from mDropdownValues where Value='Information') and DepartmentID=" + DepartmentID + "", conn);

            MailSubject = dsMailSubBody.Tables[0].Rows[0]["MailSubject"].ToString() + ", Order No # " + Request.OrderNo + "";

            DataSet dsAllUsers = new DataSet();
            dsAllUsers = fillds(" select UserApproverID from tOrderWiseAccess where OrderId=" + RequestID + "", conn);
            int cnt = dsAllUsers.Tables[0].Rows.Count;
            if (cnt > 0)
            {
                for (int i = 0; i <= cnt - 1; i++)
                {
                    UserID = long.Parse(dsAllUsers.Tables[0].Rows[i]["UserApproverID"].ToString());
                    MailBody = "Dear " + GetUserNameByUserID(UserID, conn) + ", <br/>";
                    MailBody = MailBody + dsMailSubBody.Tables[0].Rows[0]["MailBody"].ToString();

                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestDetail(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EmailGetAddressDetails(Request, conn);
                    MailBody = MailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID, conn);

                    SendMail(MailBody + MailGetFooter(), MailSubject, EmailGetEmailIDsByUserID(UserID, conn));
                }
            }
            long TemplateID = long.Parse(dsMailSubBody.Tables[0].Rows[0]["ID"].ToString());
            AdditionalDistribution(RequestID, TemplateID, conn);

            SaveCorrespondsData(RequestID, MailSubject, MailBody, DepartmentID, Status, conn);
        }

        public string GetUOMName(long UOMID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select Description from muom where ID=" + UOMID + "", conn);
            string UOM = ds.Tables[0].Rows[0]["Description"].ToString();
            return UOM;
        }

        public long GetCostCenterApproverID(long StatutoryValue, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select ApproverID  FROM mCostCenterMain where ID=" + StatutoryValue + "", conn);
            long ApproverID = Convert.ToInt64(ds.Tables[0].Rows[0]["ApproverID"].ToString());
            return ApproverID;
        }

        public string GetInvoiceNoofOrder(long OrderID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select InvoiceNo from torderhead where id=" + OrderID + "", conn);
            string InvNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
            if (InvNo == "" || InvNo == null) InvNo = "0";
            return InvNo;
        }

        public void RemoveFromTStatutory(long OrderID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("delete from tstatutorydetail where ReferenceID=" + OrderID + "", conn);
        }

        public DataSet GetApproverDepartmentWise(long Deptid, string[] conn)
        {
             DataSet ds = new DataSet();
             ds = fillds("select UserId from mApprovalLevelDetail where DepartmentID=" + Deptid + "", conn);
             return ds;
        }
        #endregion
    }
}
