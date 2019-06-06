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
    public partial interface iUCApplyTax
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<TempCartProductLevelTaxDetail> GetTaxListBySequence(string CurrentObjectName, string SessionID, long CartSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string UpdateCalculatedTaxList(string CurrentObjectName, string SessionID, long CartSequence, decimal TaxableAmount, string SelectedTaxIDs, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_CartBeforeUpdateProductLevelTaxDetail_Result> GetCalculatedTaxListBeforeUpdate(string CurrentObjectName, string SessionID, long CartSequence, decimal TaxableAmount, string SelectedTaxIDs, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        Decimal GetCalculatedTaxAmount(List<TempCartProductLevelTaxDetail> TaxDetailList);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        bool FinalSaveProductLevelTax(string CurrentObjectName, long ReferenceID, string SessionID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        bool RemoveTaxDetailBySequence(string CurrentObjectName, string SessionID, long CartSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataByCurrentObjectSessionID(string CurrentObjectName, string SessionID, string[] conn);
    }
}
