using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;


namespace Interface.Product
{
    [ServiceContract]
    public partial interface iUCProductSearch
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetProductDetail> GetProductList(string filter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<GetProductDetail> GetProductList1(int pageIndex, string filter, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetSKUDetailsWithPack> GetSKUList(int pageIndex, string filter, long DeptID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mProduct> SKUListForGrid(int CompanyID, int DeptID, int GroupSetID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetSKUDetailsWithPack> GetSKUListDeptWise(int pageIndex, string filter, long UserID, long DeptID, string[] conn);

        #region WarehouseProductSearch

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<VW_GetSKUDetailsWithPack> GetSKUListWarehouseWise(int pageIndex, string filter, long WarehouseID, string[] conn);

        #endregion
    }
}
