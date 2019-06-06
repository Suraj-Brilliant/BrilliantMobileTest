using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.Document;
using System.ServiceModel;
using System.Data;
using Domain.Tempdata;
using System.Xml.Linq;
using System.Data.Objects;
using Interface.Document;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Domain.Server;

namespace Domain.Document
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public partial class UC_AttachDocument : Interface.Document.iUC_AttachDocument
    {
        Domain.Server.Server svr = new Server.Server();

        DataHelper datahelper = new DataHelper();

        #region GetDocumentByReferenceId()
        /// <summary>
        /// GetDocumentByReferenceId Is The Method To Get Document By Reference ID
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="ReferenceID"></param>
        /// <returns></returns>
        public List<SP_GetDocumentList_Result> GetDocumentByReferenceId(string BaseObjectName, string TargetObjectName, long ReferenceID, string LoginID, string paraSessionID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetDocumentList_Result> lst = new List<SP_GetDocumentList_Result>();
            lst = (from db in ce.SP_GetDocumentList(ReferenceID, BaseObjectName, LoginID)
                   select db).ToList();
            SaveTempDataToDB(lst, paraSessionID, LoginID, TargetObjectName, conn);
            return lst.Where(d => d.Active == "Y").OrderByDescending(d => d.Sequence).ToList();
        }
        #endregion

        #region InsertIntoTemp
        /// <summary>
        /// This Is Method ToInsert Record In to TempData table
        /// </summary>
        /// <param name="ContactPerson"></param>
        /// <param name="SessionID"></param>
        /// <param name="UserID"></param>
        /// <param name="paraObjectName"></param>
        /// <returns></returns>

        public List<SP_GetDocumentList_Result> InsertIntoTemp(SP_GetDocumentList_Result newDocument, string SessionID, string UserID, string TargetObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<SP_GetDocumentList_Result> existingDocumentList = new List<SP_GetDocumentList_Result>();
            existingDocumentList = GetTempDataByObjectNameSessionID(SessionID, UserID, TargetObjectName, conn);
            newDocument.Sequence = existingDocumentList.Count + 1;
            newDocument.Active = "Y";
            /*End*/

            /*Begin : Merge (Existing + Newly Added) Products to Create TempData of AddToCart*/
            existingDocumentList.Add(newDocument);
            /*End*/

            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDB(existingDocumentList, SessionID, UserID, TargetObjectName, conn);
            /*End*/
            return existingDocumentList.Where(d => d.Active == "Y").OrderByDescending(d => d.Sequence).ToList();
        }
        #endregion

        public List<SP_GetDocumentList_Result> DeleteDocumentFormTemp(long DeletedSeq, string paraSessionID, string paraUserID, string paraTargetObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Get Existing Records from TempData*/
            List<SP_GetDocumentList_Result> existingDocumentList = new List<SP_GetDocumentList_Result>();
            existingDocumentList = GetTempDataByObjectNameSessionID(paraSessionID, paraUserID, paraTargetObjectName, conn).Where(d => d.Sequence != DeletedSeq).ToList();
            /*End*/

            SP_GetDocumentList_Result DocumentRec = new SP_GetDocumentList_Result();
            DocumentRec = GetTempDataByObjectNameSessionID(paraSessionID, paraUserID, paraTargetObjectName, conn).Where(d => d.Sequence == DeletedSeq).FirstOrDefault();
            if (DocumentRec != null)
            {
                DocumentRec.Active = "N";
                existingDocumentList.Add(DocumentRec);

                if (DocumentRec.ID == null)
                {
                    if (File.Exists(DocumentRec.DocumentSavePath))
                    { File.Delete(DocumentRec.DocumentSavePath); }
                }
            }
            /*Begin : Serialize & Save MergedAddToCartList*/
            SaveTempDataToDB(existingDocumentList, paraSessionID, paraUserID, paraTargetObjectName, conn);
            /*End*/
            return existingDocumentList.Where(d => d.Active == "Y").OrderByDescending(d => d.Sequence).ToList();
        }

        public List<SP_GetDocumentList_Result> GetExistingTempDataBySessionIDObjectNameToRebind(string paraSessionID, string paraUserID, string paraObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetDocumentList_Result> ObjDocList = new List<SP_GetDocumentList_Result>();
            TempData tempdata = new TempData();

            tempdata = (from temp in ce.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == paraObjectName
                        && temp.UserID == paraUserID

                        select temp).FirstOrDefault();

            if (tempdata != null)
            {
                ObjDocList = datahelper.DeserializeEntity1<SP_GetDocumentList_Result>(tempdata.Data).Where(d => d.Active == "Y").OrderByDescending(d => d.Sequence).ToList();
            }
            return ObjDocList;
        }

        protected void SaveTempDataToDB(List<SP_GetDocumentList_Result> paraobjList, string paraSessionID, string paraUserID, string paraObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            /*Begin : Remove Existing Records*/
            ClearTempData(paraSessionID, paraUserID, paraObjectName, conn);
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
            tempdata.ObjectName = paraObjectName.ToString();
            tempdata.TableName = "-";
            ce.AddToTempDatas(tempdata);
            ce.SaveChanges();
            /*End*/

        }

        public void ClearTempData(string paraSessionID, string paraUserID, string paraObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            TempData tempdata = new TempData();
            tempdata = (from rec in ce.TempDatas
                        where rec.SessionID == paraSessionID
                        && rec.UserID == paraUserID
                        && rec.ObjectName == paraObjectName
                        select rec).FirstOrDefault();
            if (tempdata != null) { ce.DeleteObject(tempdata); ce.SaveChanges(); }

        }

        protected List<SP_GetDocumentList_Result> GetTempDataByObjectNameSessionID(string paraSessionID, string paraUserID, string ObjectName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetDocumentList_Result> lst = new List<SP_GetDocumentList_Result>();
            TempData tempdata = new TempData();
            tempdata = (from temp in ce.TempDatas
                        where temp.SessionID == paraSessionID
                        && temp.ObjectName == ObjectName
                        && temp.UserID == paraUserID
                        select temp).FirstOrDefault();
            if (tempdata != null)
            {
                lst = datahelper.DeserializeEntity1<SP_GetDocumentList_Result>(tempdata.Data);
            }
            return lst;
        }

        public void FinalSaveToDBtDocument(string SessionID, long ReferenceID, string UserID, string TargetObjectName, string HttpAppPath, string[] conn)
        {

            try
            {
                BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                List<SP_GetDocumentList_Result> paraobjList = new List<SP_GetDocumentList_Result>();
                paraobjList = GetTempDataByObjectNameSessionID(SessionID, UserID, TargetObjectName, conn).Where(d => d.Active == "Y" && d.ID == null).ToList();

                foreach (var v in paraobjList)
                {
                    string TempSaveAsPath;
                    tDocument objDocument = new tDocument();
                    objDocument.ObjectName = TargetObjectName.ToString();
                    objDocument.ReferenceID = Convert.ToInt64(ReferenceID);
                    objDocument.DocumentName = v.DocumentName;
                    objDocument.Sequence = Convert.ToInt64(v.Sequence);
                    objDocument.Description = v.Description;
                   // objDocument.DocumentDownloadPath = "../Document/Attach_Document/" + v.CompanyID.ToString() + "/";//v.DocumentPath;
                    objDocument.DocumentDownloadPath = "../Document/Attach_Document/" + ReferenceID.ToString() + "/"+ v.DocumentName.ToString();
                    TempSaveAsPath = v.DocumentSavePath;
                   // objDocument.DocumentSavePath = HttpAppPath + "Document\\Attach_Document\\" + v.CompanyID.ToString() + "\\";
                    objDocument.DocumentSavePath = HttpAppPath + "Document\\Attach_Document\\" + ReferenceID.ToString() + "\\";
                    objDocument.Keywords = v.Keywords;
                    objDocument.FileType = v.FileType;
                    objDocument.AccessTo = v.AccessTo;/*document type or extension*/
                    objDocument.ViewAccess = v.ViewAccess_Value;
                    objDocument.DowloadAccess = v.DowloadAccess_Value;
                    objDocument.DeleteAccess = v.DeleteAccess_Value;
                    objDocument.Active = v.Active;
                    objDocument.CreatedBy = v.CreatedBy;
                    objDocument.CreationDate = Convert.ToDateTime(v.CreationDate);
                    objDocument.CustomerHeadID = Convert.ToInt64(v.CustomerHeadID);
                    objDocument.CompanyID = Convert.ToInt64(v.CompanyID);
                    objDocument.DocumentType = v.DocumentType;

                    ce.tDocuments.AddObject(objDocument);
                    ce.SaveChanges();
                    /* For File Move form Temp To Main*/
                    //if (!(Directory.Exists(HttpAppPath + "Document\\Attach_Document\\" + v.CompanyID.ToString())))
                    //{
                    //    Directory.CreateDirectory(HttpAppPath + "Document\\Attach_Document\\" + v.CompanyID.ToString());
                    //}

                    if (!(Directory.Exists(objDocument.DocumentSavePath)))
                    {
                        Directory.CreateDirectory(objDocument.DocumentSavePath);
                    }

                   // File.Move(TempSaveAsPath, objDocument.DocumentSavePath + objDocument.ID.ToString() + "." + v.FileType);
                    File.Move(TempSaveAsPath, objDocument.DocumentSavePath + objDocument.DocumentName.ToString() + "." + v.FileType);

                    tDocumentDetail objDocumentDtls = new tDocumentDetail();
                    objDocumentDtls.ObjectName = TargetObjectName.ToString();
                    objDocumentDtls.ReferenceID = Convert.ToInt64(ReferenceID);
                    objDocumentDtls.DocumentID = Convert.ToInt64(objDocument.ID);
                    ce.tDocumentDetails.AddObject(objDocumentDtls);
                    ce.SaveChanges();
                }

                paraobjList = GetTempDataByObjectNameSessionID(SessionID, UserID, TargetObjectName, conn).Where(d => d.Active == "Y" && d.ID != null).ToList();
                foreach (var v in paraobjList)
                {
                    if (ce.tDocumentDetails.Where(d => d.ObjectName == TargetObjectName && d.ReferenceID == ReferenceID && d.DocumentID == v.ID).Count() == 0)
                    {
                        tDocumentDetail objDocumentDtls = new tDocumentDetail();
                        objDocumentDtls.ObjectName = TargetObjectName.ToString();
                        objDocumentDtls.ReferenceID = Convert.ToInt64(ReferenceID);
                        objDocumentDtls.DocumentID = Convert.ToInt64(v.ID);
                        ce.tDocumentDetails.AddObject(objDocumentDtls);
                        ce.SaveChanges();
                    }
                }

                paraobjList = GetTempDataByObjectNameSessionID(SessionID, UserID, TargetObjectName, conn).Where(d => d.Active == "N" && d.ID != null).ToList();
                foreach (var v in paraobjList)
                {
                    tDocumentDetail docdetail = ce.tDocumentDetails.Where(d => d.ObjectName == TargetObjectName && d.ReferenceID == ReferenceID && d.DocumentID == v.ID).FirstOrDefault();
                    ce.tDocumentDetails.DeleteObject(docdetail);
                    ce.SaveChanges();
                }

                //ClearTempData(SessionID, UserID, TargetObjectName, conn);
            }
            catch (Exception ex) { }
        }

        public string CheckDuplicateDocumentTitle(string paraSessionID, string DocumentTitle, string paraUserID, string currentFormID, string[] conn)
        {
            string Result = "";
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            List<SP_GetDocumentList_Result> existingAddressList = new List<SP_GetDocumentList_Result>();
            existingAddressList = GetTempDataByObjectNameSessionID(paraSessionID, paraUserID, currentFormID, conn);

            /*Get Filter List [Filter By paraSequence]*/
            SP_GetDocumentList_Result filterList = new SP_GetDocumentList_Result();
            filterList = (from exist in existingAddressList
                          where exist.DocumentName == DocumentTitle
                          select exist).FirstOrDefault();
            if (filterList == null)
            {
                Result = "";
            }
            else
            {
                Result = "Same Document Name Already Exist";
            }
            return Result;
        }

        /*public void FinalSaveToDBtDocument(string paraSessionID, long paraReferenceID, string paraUserID, string BaseObjectName, string TargetObjectName, string State, string Location, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetDocumentList_Result> paraobjList = new List<SP_GetDocumentList_Result>();
            paraobjList = GetTempDataByObjectNameSessionID(paraSessionID, paraUserID, TargetObjectName, conn);

            foreach (var v in paraobjList)
            {
                if (State == "AddNew")
                {
                    if (v.ID == null)
                    {
                        ElegantCRM.Model.tDocument objDocument = new tDocument();
                        objDocument.ObjectName = TargetObjectName.ToString();
                        objDocument.ReferenceID = Convert.ToInt64(paraReferenceID);
                        objDocument.DocumentName = v.DocumentName;
                        objDocument.Sequence = Convert.ToInt64(v.Sequence);
                        objDocument.Description = v.Description;
                        objDocument.DocumentPath = "../Document/Attach_Document/" + v.CompanyID.ToString() + "/";//v.DocumentPath;
                        objDocument.Keywords = v.Keywords;
                        objDocument.AccessTo = v.AccessTo;
                        objDocument.ViewAccess = v.ViewAccess_Value;
                        objDocument.DowloadAccess = v.DowloadAccess_Value;
                        objDocument.DeleteAccess = v.DeleteAccess_Value;
                        objDocument.Active = v.Active;
                        objDocument.CreatedBy = v.CreatedBy;
                        objDocument.CreationDate = Convert.ToDateTime(v.CreationDate);
                        objDocument.LastModifiedBy = v.LastModifiedBy;
                        objDocument.LastModifiedDate = Convert.ToDateTime(v.LastModifiedDate);
                        objDocument.CustomerHeadID = Convert.ToInt64(v.CustomerHeadID);
                        objDocument.CompanyID = Convert.ToInt64(v.CompanyID);

                        ce.tDocuments.AddObject(objDocument);
                        ce.SaveChanges();
                        long DocID = Convert.ToInt64(objDocument.ID);
                        
                        string NewLocation, OldLocation, location;
                        location = Location + "Document\\Attach_Document\\";
                        OldLocation = Location + "Document/TempAttach_Document/" + v.CompanyID.ToString() + "/" + paraSessionID + '_' + v.Sequence + v.AccessTo; ;
                        OldLocation = OldLocation.Replace('\\', '/');
                        if (!(Directory.Exists(location + v.CompanyID)))
                        {
                            Directory.CreateDirectory(location + v.CompanyID);
                        }

                        NewLocation = Location + "Document/Attach_Document/" + objDocument.CompanyID + "/" + DocID + v.AccessTo;
                        NewLocation = NewLocation.Replace('\\', '/');
                        File.Move(OldLocation, NewLocation);

                        ElegantCRM.Model.tDocumentDetail objDocumentDtls = new tDocumentDetail();
                        objDocumentDtls.ObjectName = TargetObjectName.ToString();
                        objDocumentDtls.ReferenceID = Convert.ToInt64(paraReferenceID);
                        objDocumentDtls.DocumentID = Convert.ToInt64(DocID);
                        ce.tDocumentDetails.AddObject(objDocumentDtls);
                        ce.SaveChanges();
                    }
                    else
                    {
                        ElegantCRM.Model.tDocumentDetail objDocumentDtls = new tDocumentDetail();
                        objDocumentDtls.ObjectName = TargetObjectName.ToString();
                        objDocumentDtls.ReferenceID = Convert.ToInt64(paraReferenceID);
                        objDocumentDtls.DocumentID = Convert.ToInt64(v.ID);
                        ce.tDocumentDetails.AddObject(objDocumentDtls);
                        ce.SaveChanges();
                    }
                }
                else
                {
                    if (v.ID == null)
                    {
                        ElegantCRM.Model.tDocument objDocument = new tDocument();
                        objDocument.ObjectName = TargetObjectName.ToString();
                        objDocument.ReferenceID = Convert.ToInt64(paraReferenceID);
                        objDocument.DocumentName = v.DocumentName;
                        objDocument.Sequence = Convert.ToInt64(v.Sequence);
                        objDocument.Description = v.Description;
                        objDocument.DocumentPath = "../Document/Attach_Document/" + v.CompanyID.ToString() + "/";//v.DocumentPath;
                        objDocument.Keywords = v.Keywords;
                        objDocument.AccessTo = v.AccessTo;
                        objDocument.ViewAccess = v.ViewAccess_Value;
                        objDocument.DowloadAccess = v.DowloadAccess_Value;
                        objDocument.DeleteAccess = v.DeleteAccess_Value;
                        objDocument.Active = v.Active;
                        objDocument.CreatedBy = v.CreatedBy;
                        objDocument.CreationDate = Convert.ToDateTime(v.CreationDate);
                        objDocument.LastModifiedBy = v.LastModifiedBy;
                        objDocument.LastModifiedDate = Convert.ToDateTime(v.LastModifiedDate);
                        objDocument.CustomerHeadID = Convert.ToInt64(v.CustomerHeadID);
                        objDocument.CompanyID = Convert.ToInt64(v.CompanyID);

                        ce.tDocuments.AddObject(objDocument);
                        ce.SaveChanges();
                        long DocID = Convert.ToInt64(objDocument.ID);
                        
                        string NewLocation, OldLocation, location;
                        location = Location + "Document\\Attach_Document\\";
                        OldLocation = Location + "Document/TempAttach_Document/" + v.CompanyID.ToString() + "/" + paraSessionID + '_' + v.Sequence + v.AccessTo; ;
                        OldLocation = OldLocation.Replace('\\', '/');
                        if (!(Directory.Exists(location + v.CompanyID)))
                        {
                            Directory.CreateDirectory(location + v.CompanyID);
                        }

                        NewLocation = Location + "Document/Attach_Document/" + objDocument.CompanyID + "/" + DocID + v.AccessTo;
                        NewLocation = NewLocation.Replace('\\', '/');
                        File.Move(OldLocation, NewLocation);

                        ElegantCRM.Model.tDocumentDetail objDocumentDtls = new tDocumentDetail();
                        objDocumentDtls.ObjectName = TargetObjectName.ToString();
                        objDocumentDtls.ReferenceID = Convert.ToInt64(paraReferenceID);
                        objDocumentDtls.DocumentID = Convert.ToInt64(DocID);
                        ce.tDocumentDetails.AddObject(objDocumentDtls);
                        ce.SaveChanges();
                    }
                }
            }
            ClearTempData(paraSessionID, paraUserID, TargetObjectName, conn);
        }*/

        # region New Code For Brilliant WMS
        // Code to get Document List

        public DataSet GetDocumentList(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetDocumentList";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long SaveDocumentType(mDocumentType DocType, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (DocType.ID == 0)
                {
                    db.mDocumentTypes.AddObject(DocType);
                    db.SaveChanges();
                }
                else
                {
                    db.mDocumentTypes.Attach(DocType);
                    db.ObjectStateManager.ChangeObjectState(DocType, EntityState.Modified);
                    db.SaveChanges();
                }
                return DocType.ID;
            }
            catch
            {
                return 0;
            }
        }

        public mDocumentType GetDocumentTypebyID(long DocTypeID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mDocumentType DocType = new mDocumentType();
            DocType = (from p in ce.mDocumentTypes
                        where p.ID == DocTypeID
                        select p).FirstOrDefault();
            return DocType;
        }

        #endregion
    }
}
