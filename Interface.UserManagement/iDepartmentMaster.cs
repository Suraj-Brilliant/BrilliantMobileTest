using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.Entity;

using System.Data;

namespace Interface.UserManagement
{
    [ServiceContract]
    public partial interface iDepartmentMaster
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<mDepartment> GetDeparmentList(long CustomerID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int InsertmDepartment(mDepartment Department, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        int updatemDepartment(mDepartment updateDepartment, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        mDepartment GetDepartmentListByID(int DepartmentId, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecord(string DepartmentName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string checkDuplicateRecordEdit(int DepartmentID, string DepartmentName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDepartmentRecordToBind(string[] conn);

         # region Brilliant WMS code for department

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetDepartmentList>GetDepartmentListtoBind(long UserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<V_WMS_GetDepartmentGrid> GetDepartmentToBindGrid(long UserID, string[] conn);

        // get Document Type List

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDocumentTypeList(string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        DataSet GetDeptByCustomerID(long CustomerID, string[] conn);

        #endregion

    }
}
