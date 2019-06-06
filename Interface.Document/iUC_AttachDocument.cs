using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;
using System.Data;

namespace Interface.Document
{
    [ServiceContract]

    public partial interface iUC_AttachDocument
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetDocumentList_Result> GetDocumentByReferenceId(string BaseObjectName, string TargetObjectName, long ReferenceID, string LoginID, string paraSessionID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempData(string paraSessionID, string paraUserID, string paraObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveToDBtDocument(string SessionID, long ReferenceID, string UserID, string TargetObjectName, string HttpAppPath, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetDocumentList_Result> InsertIntoTemp(SP_GetDocumentList_Result newDocument, string SessionID, string UserID, string TargetObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string CheckDuplicateDocumentTitle(string paraSessionID, string DocumentTitle, string paraUserID, string currentFormID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetDocumentList_Result> DeleteDocumentFormTemp(long DeletedSeq, string paraSessionID, string paraUserID, string paraTargetObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetDocumentList_Result> GetExistingTempDataBySessionIDObjectNameToRebind(string paraSessionID, string paraUserID, string paraObjectName, string[] conn);

        # region New Code For Brilliant WMS
        // Code to get Document List
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDocumentList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveDocumentType(mDocumentType DocType, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mDocumentType GetDocumentTypebyID(long DocTypeID, string[] conn);

        #endregion

    }
}
