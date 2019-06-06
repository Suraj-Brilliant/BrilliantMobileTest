using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;
using System.Data;

namespace Interface.Warehouse
{
    [ServiceContract]
    public partial interface iCycleCount
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetHeadCycleCount> GetCycleCountMain(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveCycleCounttemp(CycleCountTemp CycleTemp, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCycleCounttempdata(string Object, long CreatedBy, string SessionID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCycleCountTempDataByLoc(string Object, long CreatedBy, string SessionID, string[] conn);
        
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveCycleCountHead(tCycleCountHead CycleHead, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetCycleTempDataToInsert(string Object, long CreatedBy, string SessionID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         void SaveCyclePlanData(DataTable DetailInsertion, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         void DeleteCycletempData(string Object, long CreatedBy, string SessionID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         void DeleteCycleTempWithoutObj(long CreatedBy, string SessionID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         tCycleCountHead GetCyleCountHeadByID(long CycleHeadID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetLocationID(string Code, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long CheckLocInPlann(string Object, long CycleHeadID, long ReferenceID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveCycleCount(tCycleCountDetail cycledetail, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSKUID(string ProductCode, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<tCycleCountDetail> GetCycleCountDetail(long CycleHeadID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetRepeatedCycleCountData(long CountHeadID, long SKUID, long LocationID, string BatchCode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long GetSKUIDBySKUCode(string ProductCode, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetBatchCodeBySKU(long SKUId, long LocationID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal GetSystemQtyByBatch(long SKUId, long LocationID, string BatchCode, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateStockSkuTransForFromLoc(long SKUId, string BatchCode, long LocationID, decimal DiffQty, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateStocktransToLoc(long SKUId, string BatchCode, decimal DiffQty, long LocationID, long CreatedBy, long CycleHeadID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        decimal getLocationRemainingQty(long ID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet getCompanyCustomer(long ID, string[] conn);
   }
}
