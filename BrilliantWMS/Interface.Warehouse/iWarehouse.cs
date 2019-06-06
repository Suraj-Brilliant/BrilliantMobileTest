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
    public partial interface iWarehouse
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetWarehouseDetails> GetWarehouseList(long CompanyID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        V_WMS_GetWarehouseDetails GetWarehouseDetailByID(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehouseMaster(mWarehouseMaster Warehouse, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehouseAddress(tAddress WMaddress, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        tAddress GetWarehouseAddress(long ReferenceID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mWarehouseMaster GetWarehouseMasterByID(Int64 WarehID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_WarehouseLocation> GetWarehouseLocation(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetWarehouseLocationByID(long WarehouseID, string[] conn);


        // Warehouse building Code

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mWarehouseBuilding> GetWarehouseBuilding(long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mWarehouseBuilding GetWareBuildingByID(long buildingID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWareBuilding(mWarehouseBuilding WBuilding, string[] conn);

        // Warehouse Floar Code

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mFloar> GetWarehouseFloar(long BuildingID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mFloar GetWarehouseFloarbyID(long FloarID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehouseFloar(mFloar wfloar, string[] conn);

        // Warehouse Passage code

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mPathway> GetWarehousePassage(long floarID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mPathway GetWarehousePassageByID(long PassageID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehousePassage(mPathway wpassage, string[] conn);

        // Warehouse Section Code

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         List<mSection> GetWarehouseSection(long PassageID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         mSection GetWarehouseSectionByID(long SectionID, string[] conn);

         [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         long SaveWarehouseSection(mSection wsection, string[] conn);

        // Warehouse Shelf Code

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mShelf> GetWarehouseShelf(long SectionID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
         mShelf GetWarehouseShelfByID(long ShelfID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehouseShelf(mShelf wshelf, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mDropdownValue> GetLocationType(string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mDropdownValue> GetCapacityIn(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long SaveWarehouseLocation(mLocation wlocation, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long CheckDuplicateLocation(string Code, long WarehouseID, string[] conn);


        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        long CheckDuplicateSortCode(long SortCode, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        V_WMS_GetWareLocationByLocID GetWarehouseLocByID(long LocationID, long WarehouseID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetWarehousebyUserID(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void AddRecordInSkuTransaction(tSKUTransaction skutrans, string[] conn);

    }
   }
