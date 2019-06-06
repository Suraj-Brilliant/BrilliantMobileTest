using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;

using System.Data;

namespace Interface.Tax
{
    [ServiceContract]
    public partial interface iUC_StatutoryInfo
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetStatutoryDetails_Result> GetStatutoryListToBind(long paraReferenceID, string paraUserID, string ParaObjectName, long ParaCompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveToTStatutoryDetails(List<tStatutoryDetail> ObjStatutory, string paraObjectName, long paraReferenceID, string paraUserID, long paraCompanyID, string[] conn);
    }
}
