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
    public partial interface iTaxMaster
    {
         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mTaxSetup> GetTaxList(string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         int InsertmTaxSetup(mTaxSetup tax, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         int updatemTaxSetup(mTaxSetup updateTax, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         mTaxSetup GetTaxListByID(int taxId, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         string checkDuplicateRecord(string taxName, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         string checkDuplicateRecordEdit(int taxID, string taxName, string[] conn);

         //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         //List<vTaxSetupDetail> GetTaxListForGrid();

         //[OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         //List<mTaxSetup> GetTaxListForTaxMappingGrid();

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         DataSet GetTaxRecordToBindGrid(string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         DataSet GetTaxRecordToBindTaxMappingGrid(string[] conn);


    }
}
