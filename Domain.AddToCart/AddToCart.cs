using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Interface.AddToCart;
using Domain.Tempdata;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using Domain.Server;

namespace Domain.AddToCart
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class AddToCart : iAddToCart
    {
        Domain.Server.Server svr = new Server.Server();
        DataHelper datahelper = new DataHelper();

        /// <summary>
        /// 1. CreateAddToCartTempDataList
        ///    a. Call : GetExistingAddToCartListBySessionIDObjectName
        ///    b. Getproduct Details for paraProductIDs
        ///    c. Merge with existing records. 
        ///    d. Merged Data Serialize
        ///    e. Call : SaveTempDataToDB
        ///    f. Return Merged List
        /// </summary>
        /// <param name="paraProductIDs"></param>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <returns></returns>
        public List<SP_GetCartProductDetail_Result> CreateAddToCartTempDataList(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<SP_GetCartProductDetail_Result> existingAddToCartList = new List<SP_GetCartProductDetail_Result>();
            existingAddToCartList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            long MaxSequenceNo = 0;
            if (existingAddToCartList.Count > 0)
            {
                MaxSequenceNo = Convert.ToInt64((from lst in existingAddToCartList
                                                 select lst.Sequence).Max().Value);
            }

            /*Get Product Details*/
            List<SP_GetCartProductDetail_Result> getnewRec = new List<SP_GetCartProductDetail_Result>();
            getnewRec = (from view in db.SP_GetCartProductDetail(paraProductIDs, MaxSequenceNo, paraSessionID, paraCurrentObjectName, 0, paraObjectConvertFrom)
                         orderby view.Sequence
                         select view).ToList();
            /*End*/

            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            List<SP_GetCartProductDetail_Result> mergedAddToCartList = new List<SP_GetCartProductDetail_Result>();
            mergedAddToCartList.AddRange(existingAddToCartList);
            mergedAddToCartList.AddRange(getnewRec);
            /*End*/

            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDB(mergedAddToCartList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return mergedAddToCartList;
        }

        /// <summary>
        /// 2. GetExistingTempDataBySessionIDObjectName
        ///     a. Fetch TempData by Session ID & Object Name  from DB [TempData]  
        ///     b. DeSerialize fetched data
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <returns></returns>
        public List<SP_GetCartProductDetail_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> objtAddToCartProductDetailList = new List<SP_GetCartProductDetail_Result>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<SP_GetCartProductDetail_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        /// <summary>
        /// 2. SaveTempDataToDB            
        ///     a.  paraobjList Save to DB [ TempData ]
        /// </summary>
        /// <param name="paraobjList"></param>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
        protected void SaveTempDataToDB(List<SP_GetCartProductDetail_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        /// <summary>
        /// 4. GetAddToCartListByReferenceIDObjectName
        ///     a. Fetch data from DB by ReferenceID & ObjectName order by Sequence [ tAddToCartProductDetail ]
        ///     b. Fetched data serialize
        ///     c. Serialized data save to DB [ TempData ]
        ///     d. Return fetched list
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <param name="paraReferenceID"></param>
        /// <returns></returns>
        public List<SP_GetCartProductDetail_Result> GetAddToCartListByReferenceIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, long paraReferenceID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> CartProductDetails = new List<SP_GetCartProductDetail_Result>();
            /*Begin : Fetch AddToCartList from tAddToCartProductDetail by ReferenceID & ObjectName*/
            CartProductDetails = (from cart in db.SP_GetCartProductDetail("0", 0, paraSessionID, paraCurrentObjectName, paraReferenceID, paraObjectConvertFrom)
                                  orderby cart.Sequence
                                  select cart).ToList();

            /*End*/
            ///*Begin : Serialize & Save AddToCartList*/
            SaveTempDataToDB(CartProductDetails, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return CartProductDetails;
        }

        /// <summary>
        /// 5. ClearTempDataFromDB
        ///     a. Fetch TempData by SessionID & ObjectName  from DB [TempData] By ObjectName & ReferenceID   
        ///     b. Delete from DB [ TempData ]where Data = fetcheddata
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
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

        /// <summary>
        /// 7. Remove Prodcut From AddToCartList
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraUserID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <param name="paraSequence"></param>
        /// <returns></returns>
        public List<SP_GetCartProductDetail_Result> RemoveProductFromTempDataList(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<SP_GetCartProductDetail_Result> existingAddToCartList = new List<SP_GetCartProductDetail_Result>();
            existingAddToCartList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            /*Get Filter List [Filter By paraSequence]*/
            List<SP_GetCartProductDetail_Result> filterList = new List<SP_GetCartProductDetail_Result>();
            filterList = (from exist in existingAddToCartList
                          where exist.Sequence != paraSequence
                          select exist).ToList();
            /*End*/

            /*Save result to TempData*/
            SaveTempDataToDB(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return filterList;
        }


        /// <summary>
        /// 9. FinalSaveToDBtAddToCartProductDetail
        ///     a. Define ObjectParameter to Passvalues for SQL StoreProcedure
        ///     b. Call : SP_InsertIntoAddToCartProductDetail [ SQL StoreProcedure for BulkInsert ]
        ///     c. Call : ClearTempDataFromDB 
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <param name="paraReferenceID"></param>
        /// <param name="paraUserID"></param>
        public void FinalSaveToDBtAddToCartProductDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> finalSaveLst = new List<SP_GetCartProductDetail_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            XElement xmlEle = new XElement("AddToCartList", from rec in finalSaveLst
                                                            select new XElement("AddToCartProduct",
                                                            new XElement("ID", rec.ID),
                                                            new XElement("ObjectName", paraCurrentObjectName),
                                                            new XElement("ReferenceID", paraReferenceID),
                                                            new XElement("Sequence", rec.Sequence),
                                                            new XElement("ProductID", rec.ProductID),
                                                            new XElement("ProductCode", rec.ProductCode),
                                                            new XElement("ProductName", rec.ProductName),
                                                            new XElement("ProductDescription", rec.ProductDescription),
                                                            new XElement("UOMID", rec.UOMID),
                                                            new XElement("UOM", rec.UOM),
                                                            new XElement("ProductPrice", rec.ProductPrice),
                                                            new XElement("PerUnitDiscount", rec.PerUnitDiscount),
                                                            new XElement("IsDiscountPercent", rec.IsDiscountPercent),
                                                            new XElement("DiscountID", rec.DiscountID),
                                                            new XElement("RateAfterDiscount", rec.RateAfterDiscount),
                                                            new XElement("Quantity", rec.Quantity),
                                                            new XElement("AmountAfterDiscount", rec.AmountAfterDiscount),
                                                            new XElement("TotalTaxAmount", rec.TotalTaxAmount),
                                                            new XElement("AmountAfterTax", rec.AmountAfterTax),
                                                            new XElement("Remark", rec.Remark)
                                                            ));


            TempData tempdata = new TempData();
            tempdata = (from t in db.TempDatas
                        where t.ObjectName == paraCurrentObjectName && t.SessionID == paraSessionID && t.UserID == paraUserID
                        select t).FirstOrDefault();
            tempdata.XmlData = xmlEle.ToString();
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObjectName.ToString();
            tempdata.TableName = "table";
            db.SaveChanges();

            ObjectParameter _SessionID = new ObjectParameter("SessionID", typeof(string));
            _SessionID.Value = paraSessionID;

            ObjectParameter _ReferenceID = new ObjectParameter("ReferenceID", typeof(long));
            _ReferenceID.Value = paraReferenceID;

            ObjectParameter _UserID = new ObjectParameter("UserID", typeof(string));
            _UserID.Value = paraUserID;

            ObjectParameter _CurrentObjectName = new ObjectParameter("CurrentObjectName", typeof(string));
            _CurrentObjectName.Value = paraCurrentObjectName;

            ObjectParameter[] obj = new ObjectParameter[] { _SessionID, _ReferenceID, _UserID, _CurrentObjectName };
            db.ExecuteFunction("SP_InsertIntoCartProductDetail", obj);
            db.SaveChanges();

            ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
        }

        public void UpdateRecord(string paraSessionID, string paraCurrentObjectName, string paraUserID, SP_GetCartProductDetail_Result order, string[] conn)
        {

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> getRec = new List<SP_GetCartProductDetail_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            SP_GetCartProductDetail_Result updateRec = new SP_GetCartProductDetail_Result();
            updateRec = getRec.Where(g => g.Sequence == order.Sequence).FirstOrDefault();

            updateRec.ProductPrice = order.ProductPrice;
            updateRec.PerUnitDiscount = order.PerUnitDiscount;
            updateRec.IsDiscountPercent = order.IsDiscountPercent;
            //updateRec.DiscountID = updateRec.DiscountID"];
            updateRec.RateAfterDiscount = order.RateAfterDiscount;
            updateRec.Quantity = order.Quantity;
            /*If New Amount AfterDiscount <> Old Amount After Discount then Calculate Tax*/
            //if (updateRec.AmountAfterDiscount != order.AmountAfterDiscount)
            //{ CallCalcuatedTax = true; }
            /*End*/
            updateRec.AmountAfterDiscount = order.AmountAfterDiscount;
            updateRec.TotalTaxAmount = order.TotalTaxAmount;
            updateRec.AmountAfterTax = order.AmountAfterTax;
            //updateRec.Remark = updateRec.Remark"];
            SaveTempDataToDB(getRec, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            //return CallCalcuatedTax;

            /*Delete Record in Table TempCartProductLevelTaxDetail*/
            DataSet ds = new DataSet();
            ds = fillds("insert into TempCartProductLevelTaxDetailNEW select * from TempCartProductLevelTaxDetail where TempCartProductLevelTaxDetail.IsChecked=1  delete from TempCartProductLevelTaxDetail where SessionID = '" + paraSessionID + "'", conn);

        }

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <param name="paraCartSequence"></param>
        /// <param name="paraUserID"></param>
        /// <returns></returns>
        public SP_GetCartProductDetail_Result GetCartProductDetailBySequence(string paraSessionID, string paraCurrentObjectName, long paraCartSequence, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            SP_GetCartProductDetail_Result result = new SP_GetCartProductDetail_Result();
            List<SP_GetCartProductDetail_Result> productDetailList = new List<SP_GetCartProductDetail_Result>();
            productDetailList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

           // result = productDetailList.Where(p => p.Sequence == Convert.ToInt64(paraCartSequence)).FirstOrDefault();
            result = productDetailList.Where(p => p.ProductID  == Convert.ToInt64(paraCartSequence)).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paraSessionID"></param>
        /// <param name="paraCurrentObjectName"></param>
        /// <param name="paraCartSequence"></param>
        /// <param name="paraUserID"></param>
        /// <returns></returns>
        public string GetFooterTotal(string paraSessionID, string paraCurrentObjectName, long paraCartSequence, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> CartList = new List<SP_GetCartProductDetail_Result>();
            CartList = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            string Total = "";
            Total = (from lst in CartList
                     select lst.AmountAfterTax).Sum().ToString();

            return Total;
        }


        #region NewWMS

        public List<SP_GetCartProductDetail_Result> GetAddToCartListBySequence(string  PrdID,long Sequence,string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, long paraReferenceID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetCartProductDetail_Result> CartProductDetails = new List<SP_GetCartProductDetail_Result>();
            /*Begin : Fetch AddToCartList from tAddToCartProductDetail by ReferenceID & ObjectName*/
            CartProductDetails = (from cart in db.SP_GetCartProductDetail(PrdID, Sequence, paraSessionID, paraCurrentObjectName, paraReferenceID, paraObjectConvertFrom)
                                  orderby cart.Sequence
                                  select cart).ToList();

            /*End*/
            ///*Begin : Serialize & Save AddToCartList*/
            SaveTempDataToDB(CartProductDetails, paraSessionID, paraUserID, paraCurrentObjectName, conn);
            /*End*/

            return CartProductDetails;
        }

        #endregion
    }

}
