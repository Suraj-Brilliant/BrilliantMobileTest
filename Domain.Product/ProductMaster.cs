using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.Product;
using System.ServiceModel;
using Domain.Server;
using Domain.Tempdata;
using System.Xml.Linq;
using System.Data.Objects;

namespace Domain.Product
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class ProductMaster : Interface.Product.iProductMaster
    {
        Domain.Server.Server svr = new Server.Server();
        DataHelper datahelper = new DataHelper();

        #region Bind Dropdown
        public List<mProductType> GetProductTypeList(string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductType> ProductTypeList = new List<mProductType>();
            ProductTypeList = (from p in db.mProductTypes
                               where p.Active == "Y"
                               select p).ToList();

            return ProductTypeList;
        }

        public List<mUOM> GetProductUOMList(string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mUOM> UOMList = new List<mUOM>();
            UOMList = (from p in db.mUOMs
                       where p.Active == "Y"
                       orderby p.Sequence
                       select p).ToList();

            return UOMList;
        }
        #endregion

        #region Product Info
        public mProduct GetmProductToUpdate(long productID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mProduct product = new mProduct();
            product = db.mProducts.Where(p => p.ID == productID).FirstOrDefault();
            db.mProducts.Detach(product);
            return product;
        }
        public GetProductDetail GetProductDetailByProductID(long productID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            GetProductDetail product = new GetProductDetail();
            product = db.GetProductDetails.Where(p => p.ID == productID).FirstOrDefault();
            return product;
        }

        public tImage GetImageDetailByImageId(long ImgID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tImage IMG = new tImage();
            IMG = db.tImages.Where(i => i.ID == ImgID).FirstOrDefault();
            return IMG;
        }

        public mProduct GetProductDetailByProductIDForUpdate(long productID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mProduct product = new mProduct();
            product = db.mProducts.Where(p => p.ID == productID).FirstOrDefault();
            if (product != null) db.mProducts.Detach(product);
            return product;
        }

        public long FinalSaveProductDetailByProductID(mProduct product, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (product.ID == 0) { db.AddTomProducts(product); db.SaveChanges(); }
                else
                {
                    db.mProducts.Attach(product);
                    db.ObjectStateManager.ChangeObjectState(product, EntityState.Modified);
                    db.SaveChanges();
                }

                return product.ID;
            }
            catch
            {
                return 0;
            }
        }

        public List<GetProductDetail> GetProductList(string[] conn) /*Bind GridView*/
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<GetProductDetail> productlist = new List<GetProductDetail>();
            productlist = (from db in ce.GetProductDetails
                           orderby db.ProductCode, db.Name
                           select db).ToList();

            return productlist;
        }

        public List<GetProductDetail> GetProductListDeptWise(long UID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<GetProductDetail> productlist = new List<GetProductDetail>();
            productlist = (from g in ce.GetProductDetails
                           join t in ce.mUserTerritoryDetails
                           on g.StoreId equals t.TerritoryID
                           where t.UserID == UID
                           select g).ToList();
            return productlist;
        }

        public List<GetProductDetail> GetAssetList(string[] conn) /*Bind GridView*/
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<GetProductDetail> productlist = new List<GetProductDetail>();
            productlist = (from db in ce.GetProductDetails
                           where db.ProductTypeID >= 3
                           orderby db.ProductCode, db.Name
                           select db).ToList();

            return productlist;
        }
        #endregion

        #region Prodcut Tax Setup
        public void UpdateTempTaxSetup(string TaxID, string IsChecked, string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetProductMasterTaxSetupByProductID_Result> taxlist = new List<SP_GetProductMasterTaxSetupByProductID_Result>();
            taxlist = GetTempSaveProductTaxDetailBySessionID(sessionID, userID, conn);

            List<SP_GetProductMasterTaxSetupByProductID_Result> updatetaxsetuplist = new List<SP_GetProductMasterTaxSetupByProductID_Result>();
            updatetaxsetuplist = taxlist.Where(lst => lst.ParentID == Convert.ToInt64(TaxID)).ToList();
            foreach (SP_GetProductMasterTaxSetupByProductID_Result obj in updatetaxsetuplist)
            { obj.Checked = IsChecked; }

            TempSaveProductTaxDetailBySessionID(taxlist, sessionID, userID, conn);
        }

        public List<SP_GetProductMasterTaxSetupByProductID_Result> GetProductTaxDetailByProductID(long productID, string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetProductMasterTaxSetupByProductID_Result> taxlist = new List<SP_GetProductMasterTaxSetupByProductID_Result>();
            taxlist = (from sp in db.SP_GetProductMasterTaxSetupByProductID(productID)
                       select sp).ToList();
            TempSaveProductTaxDetailBySessionID(taxlist, sessionID, userID, conn);
            return taxlist;
        }

        public List<SP_GetProductMasterTaxSetupByProductID_Result> GetTempSaveProductTaxDetailBySessionID(string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetProductMasterTaxSetupByProductID_Result> producttaxsetup = new List<SP_GetProductMasterTaxSetupByProductID_Result>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == sessionID
                        && temp.ObjectName == "ProductMasterTaxSetup"
                        && temp.UserID == userID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                producttaxsetup = datahelper.DeserializeEntity1<SP_GetProductMasterTaxSetupByProductID_Result>(tempdata.Data);
            }

            return producttaxsetup;
        }

        public void FinalSaveProductTaxDetailByProductID(string sessionID, string userID, long productID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetProductMasterTaxSetupByProductID_Result> finalSaveLst = new List<SP_GetProductMasterTaxSetupByProductID_Result>();
            finalSaveLst = GetTempSaveProductTaxDetailBySessionID(sessionID, userID, conn);

            List<SP_GetProductMasterTaxSetupByProductID_Result> SelectedList = new List<SP_GetProductMasterTaxSetupByProductID_Result>();
            SelectedList = finalSaveLst.Where(f => f.Checked == "true").ToList();

            XElement xmlEle = new XElement("ProductMasterTaxSetupList", from rec in SelectedList
                                                                        select new XElement("TaxSetup",
                                                                        new XElement("ProductID", productID),
                                                                        new XElement("TaxID", rec.ID),
                                                                        new XElement("TaxPercent", rec.Percent),
                                                                        new XElement("IsEdit", false),
                                                                        new XElement("Active", true),
                                                                        new XElement("CreatedBy", userID),
                                                                        new XElement("CreationDate", DateTime.Now)));

            ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
            _xmlData.Value = xmlEle.ToString();

            ObjectParameter _ProductID = new ObjectParameter("ProductID", typeof(long));
            _ProductID.Value = productID;


            ObjectParameter[] obj = new ObjectParameter[] { _xmlData, _ProductID };
            db.ExecuteFunction("SP_InsertInto_mProductTaxDetail", obj);
            db.SaveChanges();

            ClearTempSaveProductTaxDetailBySessionID(sessionID, userID, conn);
        }

        protected void TempSaveProductTaxDetailBySessionID(List<SP_GetProductMasterTaxSetupByProductID_Result> lst, string sessionID, string userID, string[] conn)
        {

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempSaveProductTaxDetailBySessionID(sessionID, userID, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(lst);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = sessionID.ToString();
            tempdata.UserID = userID.ToString();
            tempdata.ObjectName = "ProductMasterTaxSetup";
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();

        }

        public void ClearTempSaveProductTaxDetailBySessionID(string sessionID, string userid, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == sessionID
                        && rec.UserID == userid
                        && rec.ObjectName == "ProductMasterTaxSetup"
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }
        #endregion

        #region Prodcut Specification

        public List<mProductSpecificationDetail> GetProductSpecificationDetailByProductID(long productID, string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductSpecificationDetail> productspecificationlst = new List<mProductSpecificationDetail>();
            productspecificationlst = (from sp in db.mProductSpecificationDetails
                                       where sp.ProductID == productID
                                       select sp).ToList();
            TempSaveProductSpecificationDetailBySessionID(productspecificationlst, sessionID, userID, conn);
            return productspecificationlst;
        }

        protected void TempSaveProductSpecificationDetailBySessionID(List<mProductSpecificationDetail> lst, string sessionID, string userID, string[] conn)
        {

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempSaveProductSpecificationDetailBySessionID(sessionID, userID, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(lst);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = sessionID.ToString();
            tempdata.UserID = userID.ToString();
            tempdata.ObjectName = "ProductMasterSpecification";
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();

        }

        public void ClearTempSaveProductSpecificationDetailBySessionID(string sessionID, string userid, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == sessionID
                        && rec.UserID == userid
                        && rec.ObjectName == "ProductMasterSpecification"
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        protected List<mProductSpecificationDetail> GetTempSaveProductSpecificationDetailBySessionID(string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductSpecificationDetail> ProductMasterSpecification = new List<mProductSpecificationDetail>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == sessionID
                        && temp.ObjectName == "ProductMasterSpecification"
                        && temp.UserID == userID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                ProductMasterSpecification = datahelper.DeserializeEntity1<mProductSpecificationDetail>(tempdata.Data);
            }

            return ProductMasterSpecification;
        }

        public void FinalSaveProductSpecificationDetailByProductID(string sessionID, string userID, long productID, long companyID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductSpecificationDetail> finalSaveLst = new List<mProductSpecificationDetail>();
            finalSaveLst = GetTempSaveProductSpecificationDetailBySessionID(sessionID, userID, conn);
            bool result = false;
            try
            {
                XElement xmlEle = new XElement("SpecificationList", from rec in finalSaveLst
                                                                    select new XElement("Specification",
                                                                    new XElement("ProductID", productID),
                                                                    new XElement("SpecificationTitle", rec.SpecificationTitle),
                                                                    new XElement("SpecificationDescription", rec.SpecificationDescription),
                                                                    new XElement("Active", rec.Active),
                                                                    new XElement("CreatedBy", userID),
                                                                    new XElement("CreationDate", DateTime.Now),
                                                                    new XElement("CompanyID", companyID),
                                                                    new XElement("Sequence", rec.Sequence)));

                ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
                _xmlData.Value = xmlEle.ToString();

                ObjectParameter _ProductID = new ObjectParameter("ProductID", typeof(long));
                _ProductID.Value = productID;


                ObjectParameter[] obj = new ObjectParameter[] { _xmlData, _ProductID };
                db.ExecuteFunction("SP_InsertInto_mProductSpecificationDetail", obj);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                /*uc.ErrorTracking(ex, UserID, ConnectionStr);*/
            }
            ClearTempSaveProductSpecificationDetailBySessionID(sessionID, userID, conn);
        }

        public List<mProductSpecificationDetail> AddProductSpecificationToTempData(mProductSpecificationDetail ProductSpecification, string sessionID, string userID, string[] conn)
        {
            List<mProductSpecificationDetail> existinglist = new List<mProductSpecificationDetail>();
            existinglist = GetTempSaveProductSpecificationDetailBySessionID(sessionID, userID, conn);
            ProductSpecification.Sequence = existinglist.Count + 1;
            existinglist.Add(ProductSpecification);
            TempSaveProductSpecificationDetailBySessionID(existinglist, sessionID, userID, conn);
            return existinglist;
        }

        #region  SetValuesToTempData
        public List<mProductSpecificationDetail> SetValuesToTempData_onChange(long productID, string sessionID, string userID, string[] conn, int paraSequence, mProductSpecificationDetail paraInput)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductSpecificationDetail> existingList = new List<mProductSpecificationDetail>();
            existingList = GetTempSaveProductSpecificationDetailBySessionID(sessionID, userID, conn);

            mProductSpecificationDetail editRow = new mProductSpecificationDetail();
            editRow = (from exist in existingList
                       where exist.Sequence == paraSequence
                       select exist).FirstOrDefault();
            editRow = paraInput;
            existingList = existingList.Where(e => e.Sequence != paraSequence).ToList();
            existingList.Add(editRow);
            existingList = (from e in existingList
                            orderby e.Sequence
                            select e).ToList();

            TempSaveProductSpecificationDetailBySessionID(existingList, sessionID, userID, conn);

            return existingList;

        }

        public mProductSpecificationDetail GetSpecificationDetailFromTempTableBySequence(string paraSessionID, string paraUserID, long productID, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<mProductSpecificationDetail> existingAddressList = new List<mProductSpecificationDetail>();
            existingAddressList = GetTempSaveProductSpecificationDetailBySessionID(paraSessionID, paraUserID, conn);
            /*End*/

            /*Get Filter List [Filter By paraSequence]*/
            mProductSpecificationDetail filterList = new mProductSpecificationDetail();
            filterList = (from exist in existingAddressList
                          where exist.Sequence == paraSequence
                          select exist).FirstOrDefault();
            return filterList;
        }
        #endregion
        #endregion

        #region ProductMaster Images

        public void AddTempProductImages(tImage AddImage, string sessionID, string userID, string[] conn)
        {
            List<tImage> imageList = new List<tImage>();
            imageList = GetTempSaveProductImagesListBySessionID(sessionID, userID, conn);
            imageList.Add(AddImage);

            TempSaveProductImagesBySessionID(imageList, sessionID, userID, conn);
        }

        public void EditTempProductImages(tImage AddImage, string sessionID, string userID, string state, string[] conn)
        {
            List<tImage> imageList = new List<tImage>();
            ClearTempSaveProductImagesBySessionID(sessionID, userID, conn);

            imageList.Add(AddImage);

            TempSaveProductImagesBySessionID(imageList, sessionID, userID, conn);
        }

        public DataSet GetProductImagesByProductID(long productID, string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tImage> ImgageList = new List<tImage>();
            ImgageList = (from t in db.tImages
                          where t.ObjectName == "Product" && t.ReferenceID == productID
                          select t).ToList();

            TempSaveProductImagesBySessionID(ImgageList, sessionID, userID, conn);
            return ImageListConvertToDataSet(ImgageList);
        }

        protected DataSet ImageListConvertToDataSet(List<tImage> ImgageList)
        {
            XElement xmlEle = new XElement("ProductImageList", from rec in ImgageList
                                                               select new XElement("Images",
                                                               new XElement("ID", rec.ID),
                                                               new XElement("ImageName", rec.ImageName),
                                                               new XElement("ImgeTitle", rec.ImgeTitle),
                                                               new XElement("ImageDesc", rec.ImageDesc),
                                                                   //new XElement("Path", rec.ID != 0 ? rec.Path + rec.CompanyID.ToString() + "_" + rec.ID.ToString() + "." + rec.Extension : rec.Path),
                                                                new XElement("Path", rec.Path),
                                                                new XElement("SkuImage",rec.SkuImage),
                                                               new XElement("Active", rec.Active == "Y" ? "Yes" : "No")));
            DataSet ds = new DataSet();
            ds.ReadXml(xmlEle.CreateReader());
            if (ds.Tables.Count <= 0) ds.Tables.Add();

            return ds;
        }

        public DataSet GetTempSaveProductImagesBySessionID(string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tImage> producttaxsetup = new List<tImage>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == sessionID
                        && temp.ObjectName == "ProductMasterImages"
                        && temp.UserID == userID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                producttaxsetup = datahelper.DeserializeEntity1<tImage>(tempdata.Data);
            }

            return ImageListConvertToDataSet(producttaxsetup);
        }

        protected List<tImage> GetTempSaveProductImagesListBySessionID(string sessionID, string userID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tImage> ImageList = new List<tImage>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == sessionID
                        && temp.ObjectName == "ProductMasterImages"
                        && temp.UserID == userID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                ImageList = datahelper.DeserializeEntity1<tImage>(tempdata.Data);
            }

            return ImageList;
        }

        public void FinalSaveProductImagesByProductIDEdit(string sessionID, string userID, long productID, string FilePath, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tImage> finalSaveLst = new List<tImage>();
            finalSaveLst = GetTempSaveProductImagesListBySessionID(sessionID, userID, conn);
            List<tImage> NewImageLst = new List<tImage>();
            NewImageLst = finalSaveLst.Where(f => f.ID == 0 || f.ID == null).ToList();
            foreach (tImage obj in NewImageLst)
            {
                obj.ReferenceID = productID;
                //db.tImages.AddObject(obj);
                //db.SaveChanges();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_GWC_UpdateTImage";
                cmd.Connection = svr.GetSqlConn(conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("ImageName", obj.ImageName);
                cmd.Parameters.AddWithValue("Path", obj.Path);
                cmd.Parameters.AddWithValue("Extension", obj.Extension);
                cmd.Parameters.AddWithValue("LastModifiedBy", obj.LastModifiedBy);
                cmd.Parameters.AddWithValue("LastModifiedDate", obj.LastModifiedDate);
                cmd.Parameters.AddWithValue("ImgeTitle", obj.ImgeTitle);
                cmd.Parameters.AddWithValue("ImageDesc", obj.ImageDesc);
                cmd.Parameters.AddWithValue("SkuImage", obj.SkuImage);
                cmd.Parameters.AddWithValue("ReferenceID", obj.ReferenceID);
                cmd.ExecuteNonQuery();

            }
            ClearTempSaveProductImagesBySessionID(sessionID, userID, conn);
        }

        public void FinalSaveProductImagesByProductID(string sessionID, string userID, long productID, string FilePath, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tImage> finalSaveLst = new List<tImage>();
            finalSaveLst = GetTempSaveProductImagesListBySessionID(sessionID, userID, conn);
            List<tImage> NewImageLst = new List<tImage>();
            NewImageLst = finalSaveLst.Where(f => f.ID == 0 || f.ID == null).ToList();
            foreach (tImage obj in NewImageLst)
            {
                obj.ReferenceID = productID;
                db.tImages.AddObject(obj);
                db.SaveChanges();
                //System.IO.File.Move(FilePath + "/" + obj.Path, FilePath + "/ProductImage/" + obj.CompanyID.ToString() + "_" + obj.ID.ToString() + "." + obj.Extension);

                //tImage updatepath = new tImage();
                //updatepath = db.tImages.Where(i => i.ID == obj.ID).FirstOrDefault();
                //db.tImages.Detach(updatepath);
                //updatepath.Path = "ProductImage/";
                //db.tImages.Attach(updatepath);
                //db.ObjectStateManager.ChangeObjectState(updatepath, EntityState.Modified);
                //db.SaveChanges();
            }

            ClearTempSaveProductImagesBySessionID(sessionID, userID, conn);


            //BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            //List<tImage> finalSaveLst = new List<tImage>();
            //finalSaveLst = GetTempSaveProductImagesListBySessionID(sessionID, userID, conn);
            //List<tImage> NewImageLst = new List<tImage>();
            //NewImageLst = finalSaveLst.Where(f => f.ID == 0 || f.ID == null).ToList();
            //foreach (tImage obj in NewImageLst)
            //{
            //    obj.ReferenceID = productID;
            //    db.tImages.AddObject(obj);
            //    db.SaveChanges();
            //    System.IO.File.Move(FilePath + "/" + obj.Path, FilePath + "/ProductImage/" + obj.CompanyID.ToString() + "_" + obj.ID.ToString() + "." + obj.Extension);

            //    tImage updatepath = new tImage();
            //    updatepath = db.tImages.Where(i => i.ID == obj.ID).FirstOrDefault();
            //    db.tImages.Detach(updatepath);
            //    updatepath.Path = "ProductImage/";
            //    db.tImages.Attach(updatepath);
            //    db.ObjectStateManager.ChangeObjectState(updatepath, EntityState.Modified);
            //    db.SaveChanges();
            //}

            //ClearTempSaveProductImagesBySessionID(sessionID, userID, conn);
        }

        protected void TempSaveProductImagesBySessionID(List<tImage> lst, string sessionID, string userID, string[] conn)
        {

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempSaveProductImagesBySessionID(sessionID, userID, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(lst);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = sessionID.ToString();
            tempdata.UserID = userID.ToString();
            tempdata.ObjectName = "ProductMasterImages";
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();

        }

        public void ClearTempSaveProductImagesBySessionID(string sessionID, string userid, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == sessionID
                        && rec.UserID == userid
                        && rec.ObjectName == "ProductMasterImages"
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        #endregion

        #region checkDuplicateRecord
        /// <summary>
        /// checkDuplicateRecord is providing List of LeadSource by Name 
        /// </summary>
        /// <returns></returns>
        /// 
        public string checkDuplicateRecord(string ProdName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.GetProductDetails
                          where p.Name == ProdName
                          select new { p.Name }).FirstOrDefault();
            if (output != null)
            {
                result = "[ " + ProdName + " ] SKU name already exist";
            }
            return result;
        }

        public string checkDuplicateRecordSKUCODE(string omsskucode, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.GetProductDetails
                          where p.OMSSKUCode == omsskucode
                          select new { p.Name }).FirstOrDefault();
            if (output != null)
            {
                result = "[ " + omsskucode + " ] SKU Code already exist";
            }
            return result;
        }
        #endregion

        #region checkDuplicateRecordEdit
        /// <summary>
        /// checkDuplicateRecord is providing List of LeadSource by Name and ID for Edit mode
        /// </summary>
        /// <returns></returns>
        /// 
        public string checkDuplicateRecordEdit(int ProdID, string ProdName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.GetProductDetails
                          where p.Name == ProdName && p.ID != ProdID
                          select new { p.Name }).FirstOrDefault();
            if (output != null)
            {
                result = "[ " + ProdName + " ] Product name already exist";
            }
            return result;
        }

        public string checkDuplicateRecordEditSKUCODE(int ProdID, string omsskucode, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.GetProductDetails
                          where p.OMSSKUCode == omsskucode && p.ID != ProdID
                          select new { p.OMSSKUCode }).FirstOrDefault();
            if (output != null)
            {
                result = "[ " + omsskucode + " ] SKU Code already exist";
            }
            return result;

        }
        #endregion

        #region Change Rate
        public long SaveNewRates(mProductRateDetail NewRate, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            long result = 0;
            try
            {
                db.AddTomProductRateDetails(NewRate);
                db.SaveChanges();
                result = NewRate.ID;
            }
            catch (Exception ex)
            {
            }
            finally { }
            return result;
        }

        public List<mProductRateDetail> GetProductRateHistory(long ProductID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProductRateDetail> RateHistoryList = new List<mProductRateDetail>();
            try
            {
                RateHistoryList = db.mProductRateDetails.Where(pr => pr.ID == ProductID).OrderByDescending(pr => pr.ID).ToList();
            }
            catch (Exception ex)
            { }
            finally { }
            return RateHistoryList;
        }

        #endregion

        #region GetProductofEngine
        ///GetProductofEngine is providing List of Products of Selected Site & Engine

        public DataSet GetProductofEngine(string frmdt, string todt, string SID, string EID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            ds.Reset();
            // string str = "declare @s varchar(5000);set @s=('" + SID + "') select Name from vprsdashboard where EngineSerial in ( " + EID + ") AND SiteName in (select part from SplitString(@s,',')) and ConsumptionDate between '" + frmdt + "' and '" + todt + "' ";
            string str = "declare @s varchar(5000);set @s=('" + SID + "') select distinct Name from vprsdashboard where EngineSerial in ( " + EID + ") AND SiteName in (select part from SplitString(@s,',')) and ConsumptionDate between '" + frmdt + "' and '" + todt + "' and Name is not null ";
            ds = fillds(str, conn);
            return ds;
        }

        #endregion

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

        #region Inventory Code
        public List<SP_GetSiteWiseInventoryByProductIDs_Result> GetInventoryDataByProductIDs(string ProductIDs, string sessionID, string userID, string CurrentObject, string[] conn)
        {

            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetSiteWiseInventoryByProductIDs_Result> PartDetail = new List<SP_GetSiteWiseInventoryByProductIDs_Result>();
            PartDetail = (from sp in db.SP_GetSiteWiseInventoryByProductIDs(ProductIDs)
                          select sp).ToList();
            SaveTempDataToDB(PartDetail, sessionID, userID, CurrentObject, conn);
            return PartDetail;
        }

        protected void SaveTempDataToDB(List<SP_GetSiteWiseInventoryByProductIDs_Result> paraobjList, string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
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

        public List<SP_GetSiteWiseInventoryByProductIDs_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetSiteWiseInventoryByProductIDs_Result> objtAddToCartProductDetailList = new List<SP_GetSiteWiseInventoryByProductIDs_Result>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objtAddToCartProductDetailList = datahelper.DeserializeEntity1<SP_GetSiteWiseInventoryByProductIDs_Result>(tempdata.Data);
            }
            return objtAddToCartProductDetailList;
        }

        public long UpdateProductInvetory_TempData(string SessionID, string CurrentObjectName, string UserID, SP_GetSiteWiseInventoryByProductIDs_Result InventoryRec, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetSiteWiseInventoryByProductIDs_Result> getRec = new List<SP_GetSiteWiseInventoryByProductIDs_Result>();
            getRec = GetExistingTempDataBySessionIDObjectName(SessionID, UserID, CurrentObjectName, conn);

            SP_GetSiteWiseInventoryByProductIDs_Result updateRec = new SP_GetSiteWiseInventoryByProductIDs_Result();
            updateRec = getRec.Where(g => g.SiteID == InventoryRec.SiteID).FirstOrDefault();

            if (updateRec.OpeningStock <= InventoryRec.OpeningStock)
            {
                updateRec.AvailableBalance = updateRec.AvailableBalance + (InventoryRec.OpeningStock - updateRec.OpeningStock);
            }
            else if (updateRec.OpeningStock > InventoryRec.OpeningStock)
            {
                updateRec.AvailableBalance = updateRec.AvailableBalance - (updateRec.OpeningStock - InventoryRec.OpeningStock);
            }

            updateRec.OpeningStock = InventoryRec.OpeningStock;
            updateRec.MaxStockLimit = InventoryRec.MaxStockLimit;
            updateRec.ReorderQty = InventoryRec.ReorderQty;

            SaveTempDataToDB(getRec, SessionID, UserID, CurrentObjectName, conn);
            return updateRec.AvailableBalance;
        }

        public void FinalSaveProductInventory(string paraSessionID, string paraCurrentObjectName, long ProductID, DateTime EffectiveDate, string paraUserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetSiteWiseInventoryByProductIDs_Result> finalSaveLst = new List<SP_GetSiteWiseInventoryByProductIDs_Result>();
            finalSaveLst = GetExistingTempDataBySessionIDObjectName(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            XElement xmlEle = new XElement("Product", from rec in finalSaveLst
                                                      select new XElement("StockList",
                                                      new XElement("SiteID", Convert.ToInt64(rec.SiteID)),
                                                      new XElement("ProdID", ProductID),
                                                      new XElement("OpeningStock", Convert.ToInt64(rec.OpeningStock)),
                                                      new XElement("MaxStockLimit", Convert.ToInt64(rec.MaxStockLimit)),
                                                      new XElement("AvailableBalance", Convert.ToInt64(rec.AvailableBalance)),
                                                      new XElement("ReorderQty", Convert.ToInt64(rec.ReorderQty)),
                                                      new XElement("EffectiveDate", EffectiveDate)));

            ObjectParameter _ModifiedByUserID = new ObjectParameter("ModifiedByUserID", typeof(long));
            _ModifiedByUserID.Value = Convert.ToInt64(paraUserID);

            ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
            _xmlData.Value = xmlEle.ToString();


            ObjectParameter[] obj = new ObjectParameter[] { _ModifiedByUserID, _xmlData };
            db.ExecuteFunction("SP_InsertInto_tProductStockDetails", obj);

            db.SaveChanges();
            ClearTempDataFromDB(paraSessionID, paraUserID, paraCurrentObjectName, conn);
        }
        #endregion

        #region ToolSite
        public long SaveToolSiteHistory(mToolSiteHistory mTool, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

                db.AddTomToolSiteHistories(mTool);
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public List<VW_ToolSiteHistoryDetail> GetToolSiteHistory(long ProductID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<VW_ToolSiteHistoryDetail> SiteHistoryList = new List<VW_ToolSiteHistoryDetail>();
            try
            {
                SiteHistoryList = db.VW_ToolSiteHistoryDetail.Where(pr => pr.ProdID == ProductID).ToList();
            }
            catch (Exception ex)
            { }
            finally { }
            return SiteHistoryList;
        }

        //public List<VW_ToolSiteHistoryDetail> GetSitewiseTool(long SiteID, string[] conn)
        //{
        //    BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
        //    List<VW_ToolSiteHistoryDetail> SiteWiseToolList = new List<VW_ToolSiteHistoryDetail>();
        //    try
        //    {
        //        SiteWiseToolList = db.VW_ToolSiteHistoryDetail.Where(pr => pr.SiteID == SiteID).ToList();
        //    }
        //    catch (Exception ex)
        //    { }
        //    finally { }
        //    return SiteWiseToolList;
        //}

        public List<POR_SP_SiteWiseTools_Result> GetSitewiseTool(long SiteID, string SessionID, string UserID, string CurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_SiteWiseTools_Result> SiteWiseToolList = new List<POR_SP_SiteWiseTools_Result>();
            try
            {
                SiteWiseToolList = (from sp in db.POR_SP_SiteWiseTools(SiteID)
                                    where sp.Active == true
                                    select sp).ToList();
                SaveTempDataToDBPrd(SiteWiseToolList, SessionID, UserID, CurrentObject, conn);
            }
            catch (Exception ex)
            { }
            finally { }
            return SiteWiseToolList;
        }

        protected void SaveTempDataToDBPrd(List<POR_SP_SiteWiseTools_Result> SiteWiseToolList, string paraSessionID, string paraUserID, string paraCurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempDataFromDBPrd(paraSessionID, paraUserID, paraCurrentObject, conn);
            /*End*/

            /*Begin : Serialize MergedAddToCartList*/
            string xml = "";
            xml = datahelper.SerializeEntity(SiteWiseToolList);
            /*End*/

            /*Begin : Save Serialized List into TempData */
            TempData tempdata = new TempData();
            tempdata.Data = xml;
            tempdata.XmlData = "";
            tempdata.LastUpdated = DateTime.Now;
            tempdata.SessionID = paraSessionID.ToString();
            tempdata.UserID = paraUserID.ToString();
            tempdata.ObjectName = paraCurrentObject.ToString();
            tempdata.TableName = "table";
            db.AddToTempDatas(tempdata);
            db.SaveChanges();
            /*End*/
        }

        public void ClearTempDataFromDBPrd(string paraSessionID, string paraUserID, string paraCurrentObject, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in db.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraCurrentObject
                        select rec).FirstOrDefault();
            if (tempdata != null) { db.DeleteObject(tempdata); db.SaveChanges(); }
        }

        public List<POR_SP_SiteWiseTools_Result> RemoveAssetFromCurrentAsset_TempData(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            List<POR_SP_SiteWiseTools_Result> existingList = new List<POR_SP_SiteWiseTools_Result>();
            existingList = GetExistingTempDataBySessionIDObjectNamePrd(paraSessionID, paraUserID, paraCurrentObjectName, conn);

            List<POR_SP_SiteWiseTools_Result> filterList = new List<POR_SP_SiteWiseTools_Result>();
            filterList = (from lst in existingList
                          where lst.Sequence != paraSequence
                          select lst).ToList();

            SaveTempDataToDBPrd(filterList, paraSessionID, paraUserID, paraCurrentObjectName, conn);

            return filterList;
        }

        public List<POR_SP_SiteWiseTools_Result> GetExistingTempDataBySessionIDObjectNamePrd(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_SiteWiseTools_Result> objGetLstofAsset = new List<POR_SP_SiteWiseTools_Result>();

            TempData tempdata = new TempData();
            tempdata = (from temp in db.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraCurrentObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                objGetLstofAsset = datahelper.DeserializeEntity1<POR_SP_SiteWiseTools_Result>(tempdata.Data);
            }
            return objGetLstofAsset;
        }

        public long SavetToolTransferHead(tToolTransferHead tToolHead, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (tToolHead.ID == 0)
                {
                    db.AddTotToolTransferHeads(tToolHead);
                }
                else
                {
                    db.tToolTransferHeads.Attach(tToolHead);
                    db.ObjectStateManager.ChangeObjectState(tToolHead, EntityState.Modified);
                }
                db.SaveChanges();
                return tToolHead.ID;
            }
            catch
            {
                return 0;
            }
        }

        public void FinalSaveToolTransferDetails(string paraSessionID, string paraObjectName, string paraReferenceID, string UserID, string ToSite, DateTime TransferDate, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_SP_SiteWiseTools_Result> FinalSaveList = new List<POR_SP_SiteWiseTools_Result>();

            FinalSaveList = GetExistingTempDataBySessionIDObjectNamePrd(paraSessionID, UserID, paraObjectName, conn);

            XElement xmlEle = new XElement("TransferDetail", from rec in FinalSaveList
                                                             select new XElement("PartList",
                                                                 new XElement("TransferID", paraReferenceID),
                                                                 new XElement("ProductCode", Convert.ToInt64(rec.ProdID))
                                                                 ));

            ObjectParameter _TransferID = new ObjectParameter("TransferID", typeof(long));
            _TransferID.Value = paraReferenceID;

            ObjectParameter _xmlData = new ObjectParameter("xmlData", typeof(string));
            _xmlData.Value = xmlEle.ToString();

            ObjectParameter[] obj = new ObjectParameter[] { _TransferID, _xmlData };
            db.ExecuteFunction("POR_SP_SaveTransferDetails", obj);
            db.SaveChanges();

            //For Insert in Table mToolSiteHistory
            XElement xmlEle1 = new XElement("TransferDetailPrd", from rec1 in FinalSaveList
                                                                 select new XElement("PrdList",
                                                                     new XElement("ProdID", Convert.ToInt64(rec1.ProdID)),
                                                                     new XElement("SiteID", Convert.ToInt64(ToSite)),
                                                                     new XElement("EffectiveDate", TransferDate),
                                                                     new XElement("StartDate", TransferDate),
                                                                     new XElement("CreatedBy", UserID),
                                                                     new XElement("CreationDate", DateTime.Now),
                                                                     new XElement("Active", true)
                                                                     ));

            ObjectParameter _xmlData1 = new ObjectParameter("xmlData1", typeof(string));
            _xmlData1.Value = xmlEle1.ToString();

            ObjectParameter[] obj1 = new ObjectParameter[] { _xmlData1 };
            db.ExecuteFunction("POR_SP_InsertInto_mToolSiteHistory", obj1);
            db.SaveChanges();

            //update Table mToolSiteHistory

            long transID = long.Parse(paraReferenceID);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "POR_SP_Update_mToolSiteHistory";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SiteID", ToSite);
            cmd.Parameters.AddWithValue("TransferID", transID);

            cmd.ExecuteNonQuery();
        }

        public List<POR_VW_ToolTransferDetails> GetTransferList(string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<POR_VW_ToolTransferDetails> TransferLst = new List<POR_VW_ToolTransferDetails>();

            TransferLst = (from l in db.POR_VW_ToolTransferDetails
                           select l).ToList();

            return TransferLst;
        }

        public tToolTransferHead GetToolTransferHead(long TransferHeadID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tToolTransferHead Ttool = new tToolTransferHead();
            Ttool = db.tToolTransferHeads.Where(p => p.ID == TransferHeadID).FirstOrDefault();
            return Ttool;
        }


        #endregion

        # region new product related code for WMS

        public DataSet GetDepartment(long ParentID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDepartmentContact";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetCompanyname(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetCompanyforProduct";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetUOMList(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUOMList";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public string GetUomShort(long ID, string[] conn)
        {
            SqlDataReader dr;
            string UOMShortId = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUOMShortDescri";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                UOMShortId = dr[0].ToString();
            }
            dr.Close();
            return UOMShortId;
        }

        public void InsertIntomPackUom(long SkuId, string ShortDescri, string Description, long Quantity, long Sequence, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertntomPackUOM";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SkuId", SkuId);
            cmd.Parameters.AddWithValue("ShortDescri", ShortDescri);
            cmd.Parameters.AddWithValue("Description", Description);
            cmd.Parameters.AddWithValue("Quantity", Quantity);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetUOMPackDetails(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUOMPackDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetEditmpackUOmdetail(long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetMpackUOMbyId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void UpdatemPackUom(long Id, string ShortDescri, string Description, long Quantity, long Sequence, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdatemPackUOM";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.Parameters.AddWithValue("ShortDescri", ShortDescri);
            cmd.Parameters.AddWithValue("Description", Description);
            cmd.Parameters.AddWithValue("Quantity", Quantity);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetBomProductDetail(string Edit, long BOMheaderId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetBOMDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Edit", Edit);
            cmd.Parameters.AddWithValue("BOMheaderId", BOMheaderId);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void saveBomDetail(long BOMgeaderId, long SKUId, long Quantity, long Sequence, string Remark, string state, long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveUpdateBomDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("BOMheaderId", BOMgeaderId);
            cmd.Parameters.AddWithValue("SKUId", SKUId);
            cmd.Parameters.AddWithValue("Quantity", Quantity);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.Parameters.AddWithValue("Remark", Remark);
            cmd.Parameters.AddWithValue("state", state);
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.ExecuteNonQuery();
        }

        public void RemoveBOMDetailSKu(long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_DeletefromBOMDetailtable";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.ExecuteNonQuery();
        }
        public DataSet GetBOMDetailById(long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetBOMDetailById";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetUOMPackDetailsByProdId(long SkuId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUOMPackDetailsById";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SkuId", SkuId);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetBOmDeyailbyIdforedit(long BOMheaderId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetBOmDetailtoeditGrid";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("BOMheaderId", BOMheaderId);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet getProductSpecification(long ProductID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetProductSpecification";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProductID", ProductID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet FillInventoryGrid(long SiteID, long ProdID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetInventryDetailbyDeptProdId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SiteID", SiteID);
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }
        public void UpdateInventryOpeningBal(decimal OpeningStock, long ProdID, long SiteID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateinventryOpeningstck";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("OpeningStock", OpeningStock);
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            cmd.Parameters.AddWithValue("SiteID", SiteID);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePackUOMforSkuId(long SkuId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdatePackUOMforSkuId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SkuId", SkuId);
            cmd.ExecuteNonQuery();
            ChkUOM(SkuId, conn);
        }

        protected void ChkUOM(long SkuId, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from mPackUOM where Skuid=" + SkuId + "";
            ds = fillds(str, conn);

            int cnt = ds.Tables[0].Rows.Count;
            if (cnt >= 3)
            { }
            else
            {
                if (cnt == 0)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (i <= 2)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_InsertIntomPackUOM";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("SkuId", SkuId);
                            cmd.Parameters.AddWithValue("ShortDescri", "NA");
                            cmd.Parameters.AddWithValue("Description", "NULL");
                            cmd.Parameters.AddWithValue("Quantity", 0);
                            cmd.Parameters.AddWithValue("Sequence", i);
                            cmd.ExecuteNonQuery();
                        }
                        else if (i == 3)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_InsertIntomPackUOM";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("SkuId", SkuId);
                            cmd.Parameters.AddWithValue("ShortDescri", "EA");
                            cmd.Parameters.AddWithValue("Description", "Each");
                            cmd.Parameters.AddWithValue("Quantity", 1);
                            cmd.Parameters.AddWithValue("Sequence", i);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else if (cnt == 1)
                {
                    long seq = Convert.ToInt64(ds.Tables[0].Rows[0]["Sequence"].ToString());
                    if (seq == 3)
                    {
                        for (int i = 1; i <= 2; i++)
                        {

                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "SP_InsertIntomPackUOM";
                            cmd.Connection = svr.GetSqlConn(conn);
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("SkuId", SkuId);
                            cmd.Parameters.AddWithValue("ShortDescri", "NA");
                            cmd.Parameters.AddWithValue("Description", "NULL");
                            cmd.Parameters.AddWithValue("Quantity", 0);
                            cmd.Parameters.AddWithValue("Sequence", i);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (seq == 2)
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            if (i == 1)
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_InsertIntomPackUOM";
                                cmd.Connection = svr.GetSqlConn(conn);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("SkuId", SkuId);
                                cmd.Parameters.AddWithValue("ShortDescri", "NA");
                                cmd.Parameters.AddWithValue("Description", "NULL");
                                cmd.Parameters.AddWithValue("Quantity", 0);
                                cmd.Parameters.AddWithValue("Sequence", i);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_InsertIntomPackUOM";
                                cmd.Connection = svr.GetSqlConn(conn);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("SkuId", SkuId);
                                cmd.Parameters.AddWithValue("ShortDescri", "EA");
                                cmd.Parameters.AddWithValue("Description", "Each");
                                cmd.Parameters.AddWithValue("Quantity", 1);
                                cmd.Parameters.AddWithValue("Sequence", 3);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (seq == 1)
                    {
                        for (int i = 2; i <= 3; i++)
                        {
                            if (i == 2)
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_InsertIntomPackUOM";
                                cmd.Connection = svr.GetSqlConn(conn);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("SkuId", SkuId);
                                cmd.Parameters.AddWithValue("ShortDescri", "NA");
                                cmd.Parameters.AddWithValue("Description", "NULL");
                                cmd.Parameters.AddWithValue("Quantity", 0);
                                cmd.Parameters.AddWithValue("Sequence", i);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "SP_InsertIntomPackUOM";
                                cmd.Connection = svr.GetSqlConn(conn);
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("SkuId", SkuId);
                                cmd.Parameters.AddWithValue("ShortDescri", "EA");
                                cmd.Parameters.AddWithValue("Description", "Each");
                                cmd.Parameters.AddWithValue("Quantity", 1);
                                cmd.Parameters.AddWithValue("Sequence", 3);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else if (cnt == 2)
                {
                    long seq1 = Convert.ToInt64(ds.Tables[0].Rows[0]["Sequence"].ToString());
                    long seq2 = Convert.ToInt64(ds.Tables[0].Rows[1]["Sequence"].ToString());
                    if (seq1 == 1 && seq2 == 2)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_InsertIntomPackUOM";
                        cmd.Connection = svr.GetSqlConn(conn);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("SkuId", SkuId);
                        cmd.Parameters.AddWithValue("ShortDescri", "EA");
                        cmd.Parameters.AddWithValue("Description", "Each");
                        cmd.Parameters.AddWithValue("Quantity", 1);
                        cmd.Parameters.AddWithValue("Sequence", 3);
                        cmd.ExecuteNonQuery();
                    }
                    else if (seq1 == 1 && seq2 == 3)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_InsertIntomPackUOM";
                        cmd.Connection = svr.GetSqlConn(conn);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("SkuId", SkuId);
                        cmd.Parameters.AddWithValue("ShortDescri", "NA");
                        cmd.Parameters.AddWithValue("Description", "NULL");
                        cmd.Parameters.AddWithValue("Quantity", 0);
                        cmd.Parameters.AddWithValue("Sequence", 2);
                        cmd.ExecuteNonQuery();
                    }
                    else if (seq1 == 2 && seq2 == 3)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_InsertIntomPackUOM";
                        cmd.Connection = svr.GetSqlConn(conn);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("SkuId", SkuId);
                        cmd.Parameters.AddWithValue("ShortDescri", "NA");
                        cmd.Parameters.AddWithValue("Description", "NULL");
                        cmd.Parameters.AddWithValue("Quantity", 0);
                        cmd.Parameters.AddWithValue("Sequence", 1);
                        cmd.ExecuteNonQuery();
                    }
                }

            }

        }

        public void UpdateBOMforsSkuId(long BOMheaderId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateBOMforSkuId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("BOMheaderId", BOMheaderId);
            cmd.ExecuteNonQuery();
        }

        public void DeleteMpackUOMBOMClear(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteMpackUOmBOMClear";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }

        public long GetSKUByFilename(string OMSSKUCode, string[] conn)
        {
            SqlDataReader dr;
            long SKUID = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_GetSKUIDByImageFilename";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("OMSSKUCode", OMSSKUCode);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                SKUID = long.Parse(dr[0].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return SKUID;
        }
        public long getSKUIdfromtImage(long ReferenceID, string[] conn)
        {
            SqlDataReader dr;
            long ReferenceId = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_getSKUIdfromtImage";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ReferenceId = long.Parse(dr[0].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return ReferenceId;

        }

        public void InsertintotImage(string ObjectName, long ReferenceID, string ImageName, string Path, string Extension, string CreatedBy, DateTime CreationDate, long CompanyID, byte[] SkuImage,string ImageTitle,string ImageDescr, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertintotImage";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ObjectName", ObjectName);
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("ImageName", ImageName);
            cmd.Parameters.AddWithValue("Path", Path);
            cmd.Parameters.AddWithValue("Extension", Extension);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("SkuImage", SkuImage);
            cmd.Parameters.AddWithValue("ImgeTitle", ImageTitle);
            cmd.Parameters.AddWithValue("ImageDesc", ImageDescr);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void InsertIntotImageImportLog(long ReferenceID, string ImageName, string Path, string CreatedBy, DateTime CreationDate, string Reason, long CompanyID, long DeptID, string OMSSkuCode, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertintoImageLog";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("ImageName", ImageName);
            cmd.Parameters.AddWithValue("Path", Path);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("Reason", Reason);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.Parameters.AddWithValue("OMSSkuCode", OMSSkuCode);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public DataSet GetFailedImageDetail(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetImportfailedImages";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void deleteimportlogdata(long DeptID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_deleteimportlogdata";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.ExecuteNonQuery();
        }

        public void ClearImageUploadLog(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_ClearImageUploadLog";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }

        public void SaveProductStockDetail(long SiteID, long ProdID, decimal AvailableBalance, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertIntotproductStockDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SiteID", SiteID);
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            cmd.Parameters.AddWithValue("AvailableBalance", AvailableBalance);
            cmd.ExecuteNonQuery();
        }

        public decimal UpdateProductStockQty(long ProdID, string[] conn)
        {
            SqlDataReader dr;
            decimal ReferenceId = 3.00M;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetStockDetailBOMReturn";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ReferenceId = decimal.Parse(dr["stock"].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return ReferenceId;
        }

        public void InsertIntoDistribution(long TemplateID, long ContactID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertIntoDistribution";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("TemplateID", TemplateID);
            cmd.Parameters.AddWithValue("ContactID", ContactID);
            cmd.ExecuteNonQuery();
        }

        public void UpdateDistribution(long TemplateID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateDisrtibution";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("TemplateID", TemplateID);
            cmd.ExecuteNonQuery();
        }

        public void DeleteDistribution(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteDistribution";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }
        public void RemoveDistribution(long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "RemoveDistribution";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            cmd.ExecuteNonQuery();
        }

        public decimal GetAvailStockById(long ProdID, string[] conn)
        {
            SqlDataReader dr;
            decimal AvailBalance = 3.00M;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetAvailStockById";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                AvailBalance = decimal.Parse(dr["AvailableBalance"].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return AvailBalance;
        }

        public DataSet GetEditSpecifcdetail(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetEditSpecification";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }
        public void Updatespecification(long ID, string SpecificationDescription, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateSpecification";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("SpecificationDescription", SpecificationDescription);
            cmd.ExecuteNonQuery();
        }
        #endregion

        public DataSet GetCreatedByDate(string SelImageID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select CreatedBy,CreationDate from timage where ID= " + SelImageID + "";
            ds = fillds(str, conn);
            return ds;
        }

        public long GetAVLBalance(string MinQty, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select MIN(Convert(bigint,part)) FnlQTY  from  SplitString('" + MinQty + "','|')";
            ds = fillds(str, conn);

            long FnlQty = Convert.ToInt64(ds.Tables[0].Rows[0]["FnlQTY"].ToString());
            return FnlQty;
        }

        public void UpdateImageInTImage(long ReferenceID, string ImageName, string Path, string Extension, string LastModifiedBy, long CompanyID, byte[] SkuImage, string ImageTitle, string ImageDescr, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdatetImage";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();            
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("ImageName", ImageName);
            cmd.Parameters.AddWithValue("Path", Path);
            cmd.Parameters.AddWithValue("Extension", Extension);
            cmd.Parameters.AddWithValue("LastModifiedBy", LastModifiedBy);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("SkuImage", SkuImage);
            cmd.Parameters.AddWithValue("ImageTitle", ImageTitle);
            cmd.Parameters.AddWithValue("ImageDescr", ImageDescr);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        
        public DataSet GetAvailQuantity(long ProdID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select SiteID,AvailableBalance,VirtualQty,AvailVirtualQty from tProductStockDetails where ProdID= " + ProdID + "";
            ds = fillds(str, conn);
            return ds;
        }

        public void UpdateVirtualBalance(long ProdID, decimal VirtualQty, decimal AvailVirtualQty, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateVirtualBalance";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProdID",ProdID);
            cmd.Parameters.AddWithValue("VirtualQty",VirtualQty);
            cmd.Parameters.AddWithValue("AvailVirtualQty", AvailVirtualQty);
            cmd.ExecuteNonQuery();
        }

        public void InsertIntoInventry(long SKUId, long StoreId, DateTime Transactiondate, decimal Quantity, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertInventryVirtual";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId",SKUId);
             cmd.Parameters.AddWithValue("StoreId",StoreId);
             cmd.Parameters.AddWithValue("Transactiondate", Transactiondate);
            cmd.Parameters.AddWithValue("Quantity", Quantity);
            cmd.ExecuteNonQuery();
        }

        #region SKU Price Import
        public void DeleteSKUPricetemp(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteTempPrice";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }

        public DataSet GetSkuPriceTemp(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetTempPrice";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetPriceImportData(long StoreId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetImportPriceData";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("StoreId", StoreId);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void UpdateImportSkuPrice(string ProductCode, decimal PrincipalPrice, long StoreId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateImportSKUPrice";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProductCode", ProductCode);
            cmd.Parameters.AddWithValue("PrincipalPrice", PrincipalPrice);
            cmd.Parameters.AddWithValue("StoreId", StoreId);
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        #endregion

        #region   Code for Direct Order request Import

        public DataSet GetLocation(long CompanyID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetLocationforImport";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void DeleteOrderImport(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteFromTempOrderImport";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }

        public DataSet GetTempDirectOrder(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from TempOrderImport ";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet GetDirectOrderData(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDirectImportData";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetDistinctPLCodes(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDistinctDLCodeImport";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetImportDatabyDisDeptLocId(long ID, long locationId, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetImportDataByLocDeptId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("locationId", locationId);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public string getOrderFormatNumber(long StoreId, string[] conn)
        {
            SqlDataReader dr;
            string result = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_getOrderFormatNo";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("StoreId", StoreId);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr["NewOrderNo"].ToString();
            }
            dr.Close();
            cmd.Connection.Close();
            return result;
        }

        public long GetmaxDeliverydays(long ID, string[] conn)
        {
            SqlDataReader dr;
            long days = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetMaxDeliveryDays";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                days = long.Parse(dr["MaxDeliveryDays"].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return days;
        }

        public long SaveOrderHeaderImport(long StoreId, DateTime Orderdate, DateTime Deliverydate, long AddressId, long Status, long CreatedBy, DateTime Creationdate, string Title, DateTime ApprovalDate, decimal TotalQty, decimal GrandTotal, string OrderNo, long LocationID, string[] conn)
        {
            long orderheadid = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveImportIntoTorderHead";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("StoreId", StoreId);
            cmd.Parameters.AddWithValue("Orderdate", Orderdate);
            cmd.Parameters.AddWithValue("Deliverydate", Deliverydate);
            cmd.Parameters.AddWithValue("AddressId", AddressId);
            cmd.Parameters.AddWithValue("Status", Status);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("Creationdate", Creationdate);
            cmd.Parameters.AddWithValue("Title", Title);
            cmd.Parameters.AddWithValue("ApprovalDate", ApprovalDate);
            cmd.Parameters.AddWithValue("TotalQty", TotalQty);
            cmd.Parameters.AddWithValue("GrandTotal", GrandTotal);
            cmd.Parameters.AddWithValue("OrderNo", OrderNo);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            //cmd.ExecuteNonQuery();
            orderheadid = long.Parse(cmd.ExecuteScalar().ToString());

            return orderheadid;

        }

        public void SaveOrderDetailImport(long OrderHeadId,long SkuId,decimal OrderQty, long UOMID,long Sequence,string Prod_Name,string Prod_Description, string Prod_Code, decimal Price,decimal Total, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveImportOrderDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("OrderHeadId", OrderHeadId);
            cmd.Parameters.AddWithValue("SkuId", SkuId);
            cmd.Parameters.AddWithValue("OrderQty", OrderQty);
            cmd.Parameters.AddWithValue("UOMID", UOMID);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.Parameters.AddWithValue("Prod_Name", Prod_Name);
            cmd.Parameters.AddWithValue("Prod_Description", Prod_Description);
            cmd.Parameters.AddWithValue("Prod_Code", Prod_Code);
            cmd.Parameters.AddWithValue("Price", Price);
            cmd.Parameters.AddWithValue("Total", Total);
            cmd.ExecuteNonQuery();
        }

        public void updateproductstockdetailimport(long SiteID, long ProdID, decimal AvailableBalance, decimal TotalDispatchQty, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateProductStockDetailImport";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SiteID", SiteID);
            cmd.Parameters.AddWithValue("ProdID", ProdID);
            cmd.Parameters.AddWithValue("AvailableBalance", AvailableBalance);
            cmd.Parameters.AddWithValue("TotalDispatchQty", TotalDispatchQty);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetTotalForOrderHead(long ID, long locationId, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select Sum(OrderQty) TotalOrderQty,sum(Total) GrandTotalPrice from View_GetDirectImportData where ID = " + ID + " and locationId = " + locationId + "";
            ds = fillds(str, conn);
            return ds;
        }


        public void ImportMsgTransHeader(long RequestID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mMessageTran Msg = new mMessageTran();

            GWC_VW_MessOrderHeadImport MsgHD = new GWC_VW_MessOrderHeadImport();
            MsgHD = db.GWC_VW_MessOrderHeadImport.Where(h => h.Id == RequestID).FirstOrDefault();

            string ContactPerson2 = MsgHD.ContactP2;

            string ContactPerson2Names = "";
            if (ContactPerson2 == "NA" || ContactPerson2 == "" || ContactPerson2 == null) { ContactPerson2Names = "NA"; }
            else
            { 
                //ContactPerson2Names = GetContactPersonNames(ContactPerson2, conn); 
            }


            Msg.MsgDescription = MsgHD.Id + " | " + MsgHD.OrderNumber + " | " + MsgHD.Orderdate.Value.ToShortDateString() + " | " + MsgHD.Deliverydate.Value.ToShortDateString() + " | " + MsgHD.StoreCode + " | " + MsgHD.Con1 + " | " + ContactPerson2Names + " | " + MsgHD.addressline12 + " | " + MsgHD.Remark + " | " + MsgHD.OrderNo + " | " + MsgHD.InvoiceNo + " | " + MsgHD.LocationCode + " | " + MsgHD.RequestorName + " | " + MsgHD.RequestorMobileNo + " | " + MsgHD.ConsigneeName + " | " + MsgHD.ConsigneeAddress + " | " + MsgHD.ConsigneePhone;

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
        #endregion

        #region Code for Brilliant WMS according to new change

        // code for Inventry tab

        //public mProductLocation GetProductLocation(Int64 ID, string[] conn)
        //{
        //    BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

        //    mProductLocation Location = new mProductLocation();
        //    Location = (from p in ce.mProductLocations
        //               where p.Id == ID
        //               select p).FirstOrDefault();
        //    return Location;
        //}

        public List<V_WMS_GetProductInventryLocation> GetProductLocationbyID(long Id, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_GetProductInventryLocation> rate = new List<V_WMS_GetProductInventryLocation>();
            rate = (from cm in ce.V_WMS_GetProductInventryLocation
                    where cm.Id == Id
                   select cm).ToList();
            return rate;
        }

        public DataSet GetProdLocByPLID(long Id, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetProductLocByID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Id", Id);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long InsertProductLocation(mProductLocation prdloc, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.mProductLocations.AddObject(prdloc);
            ce.SaveChanges();
            return 1;
        }

        public int UpdateProdLocation(mProductLocation prdloc, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.mProductLocations.Attach(prdloc);
            ce.ObjectStateManager.ChangeObjectState(prdloc, EntityState.Modified);
            ce.SaveChanges();
            return 1;
        }

        public long InsertOpBlInProductStock(tProductStockDetail Prdstock, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.tProductStockDetails.AddObject(Prdstock);
            ce.SaveChanges();
            return 1;
        }

        public int UpdateOpBlnProductStock(tProductStockDetail Prdstock, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.tProductStockDetails.Attach(Prdstock);
            ce.ObjectStateManager.ChangeObjectState(Prdstock, EntityState.Modified);
            ce.SaveChanges();
            return 1;
        }
        public List<V_WMS_GetProductInventryLocation> GetProductLocation(long ProdId, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_GetProductInventryLocation> rate = new List<V_WMS_GetProductInventryLocation>();
            rate = (from cm in ce.V_WMS_GetProductInventryLocation
                    where cm.ProdId == ProdId
                    select cm).ToList();
            return rate;
        }

        //  Code for Channel tab

        public long insertProductChannel(mProductChannel prdch, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.mProductChannels.AddObject(prdch);
            ce.SaveChanges();
            return 1;
        }

        public int updateproductChannel(mProductChannel prdch, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.mProductChannels.Attach(prdch);
            ce.ObjectStateManager.ChangeObjectState(prdch, EntityState.Modified);
            ce.SaveChanges();
            return 1;
        }

        public List<V_WMS_GetProductChannel> GetProductChannel(long ProdId, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_GetProductChannel> rate = new List<V_WMS_GetProductChannel>();
            rate = (from cm in ce.V_WMS_GetProductChannel
                    where cm.ProdID == ProdId
                    select cm).ToList();
            return rate;
        }

        public DataSet GetProdChannelByID(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetProdChanByID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        // Code for Product Vendor Tab

        public void InsertProductVendor(mProductVendor prdvend, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.mProductVendors.AddObject(prdvend);
            ce.SaveChanges();
       }

        public DataSet GetProductVendor(long SKUID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetProductVendor";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUID", SKUID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long GetProdVendCount(long SKUID, long VendorID, string[] conn)
        {
            SqlDataReader dr;
            long Count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetProdVendCount";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUID", SKUID);
            cmd.Parameters.AddWithValue("VendorID", VendorID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Count = long.Parse(dr["countNo"].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return Count;
        }

        // end Product Vendor

        public DataSet GetTaxByCustomer(long CustomerID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetTaxByCustomer";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CustomerID", CustomerID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }
        
        
        #endregion

    }

}


