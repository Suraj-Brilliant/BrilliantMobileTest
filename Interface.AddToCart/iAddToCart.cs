using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;


namespace Interface.AddToCart
{
    [ServiceContract]
    public partial interface iAddToCart
    {
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetCartProductDetail_Result> CreateAddToCartTempDataList(string paraProductIDs, string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetCartProductDetail_Result> GetExistingTempDataBySessionIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetCartProductDetail_Result> GetAddToCartListByReferenceIDObjectName(string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, long paraReferenceID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void ClearTempDataFromDB(string paraSessionID, string paraUserID, string paraCurrentObjectName, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetCartProductDetail_Result> RemoveProductFromTempDataList(string paraSessionID, string paraUserID, string paraCurrentObjectName, int paraSequence, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void FinalSaveToDBtAddToCartProductDetail(string paraSessionID, string paraCurrentObjectName, long paraReferenceID, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        void UpdateRecord(string paraSessionID, string paraCurrentObjectName, string paraUserID, SP_GetCartProductDetail_Result order, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        SP_GetCartProductDetail_Result GetCartProductDetailBySequence(string paraSessionID, string paraCurrentObjectName, long paraCartSequence, string paraUserID, string[] conn);

        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        string GetFooterTotal(string paraSessionID, string paraCurrentObjectName, long paraCartSequence, string paraUserID, string[] conn);


        #region NewWMS
        [OperationContract, XmlSerializerFormat(Style = OperationFormatStyle.Document)]
        List<SP_GetCartProductDetail_Result> GetAddToCartListBySequence(string  PrdID,long Sequence, string paraSessionID, string paraUserID, string paraCurrentObjectName, string paraObjectConvertFrom, long paraReferenceID, string[] conn);
        #endregion

    }
}
