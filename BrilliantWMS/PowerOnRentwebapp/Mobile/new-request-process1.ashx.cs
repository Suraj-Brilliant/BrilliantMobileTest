using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using PowerOnRentwebapp.Login;
using PowerOnRentwebapp.PORServicePartRequest;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for new_request_process
    /// </summary>
    public class new_request_process : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            String userId = context.Request.Form["txtUserName"];

            long StoreId = Convert.ToInt64(context.Request.Form["txtDepartmentId"]);
            // String userPass = context.Request.Form["usrPass"];

            CustomProfile profile = CustomProfile.GetProfile(userId);

            long uId = profile.Personal.UserID;
            DateTime creationdate = DateTime.Now;
            string template = context.Request.Form["txtTemplate"];
            DateTime OrderDate = Convert.ToDateTime(context.Request.Form["txtReqDate"].ToString());
            string Title = context.Request.Form["txtTitle"];
            DateTime Deliverydate = Convert.ToDateTime(context.Request.Form["txtDisDate"].ToString());
            string OrderNumber = context.Request.Form["txtOrderNo"];
            string Remark = context.Request.Form["txtRemark"];
            long Contact1 = Convert.ToInt64(context.Request.Form["txtContact1"]);
            long Contact2 = Convert.ToInt64(context.Request.Form["txtContact2"]);
            long AddressId = Convert.ToInt64(context.Request.Form["txtAddress"]);
            long PaymentMethodID = Convert.ToInt64(context.Request.Form["txtPaymentMethod"]);
            string PMField1 = context.Request.Form["txtPaymentMethodField1"];
            string PMField2 = context.Request.Form["txtPaymentMethodField2"];

            decimal totalqty = Convert.ToDecimal(context.Request.Form["txttotalqty"]);
            decimal grandtotal = Convert.ToDecimal(context.Request.Form["txtgrandtotal"]);





            string ConID2 = context.Request.Form["txtContact2"];
            string Priority = "High";

            string OrderNo = "";
            SqlCommand cmd6 = new SqlCommand();
            SqlDataAdapter da6 = new SqlDataAdapter();
            DataSet ds6 = new DataSet();
            DataTable dt6 = new DataTable();
            string ordertype = "";
            cmd6.CommandType = CommandType.Text;
            cmd6.CommandText = "select * from FN_GetOrderNo(" + StoreId + ")";
            cmd6.Connection = conn;
            cmd6.Parameters.Clear();
            da6.SelectCommand = cmd6;
            da6.Fill(ds6, "dtl6");
            dt6 = ds6.Tables[0];
            if (dt6.Rows.Count > 0)
            {
                OrderNo = dt6.Rows[0]["NewOrderNo"].ToString();
                long PreviousStatusID = 0;
                iPartRequestClient objService = new iPartRequestClient();
                tOrderHead RequestHead = new tOrderHead();

                RequestHead.CreatedBy = profile.Personal.UserID;
                RequestHead.Creationdate = DateTime.Now;

                RequestHead.OrderNumber = OrderNumber;
                RequestHead.StoreId = StoreId;
                RequestHead.Priority = Priority;
                RequestHead.Status = 2;
                RequestHead.Title = Title.ToString();
                RequestHead.Orderdate = OrderDate;
                RequestHead.RequestBy = profile.Personal.UserID;
                RequestHead.Remark = Remark;
                RequestHead.Deliverydate = Deliverydate;
                RequestHead.ContactId1 = Contact1;
                RequestHead.ContactId2 = Contact2;
                RequestHead.Con2 = ConID2;
                RequestHead.AddressId = AddressId;
                RequestHead.OrderNo = OrderNo.ToString();
                RequestHead.TotalQty = totalqty;
                RequestHead.GrandTotal = grandtotal;
                RequestHead.PaymentID = PaymentMethodID;
                RequestHead.orderType = "Mobile";
                RequestHead.LocationID = 0;
                long RequestID = objService.SetIntotOrderHead(RequestHead, profile.DBConnection._constr);


                string prdDetails = context.Request.Form["txtProductDetails"];
                string myQueryString = " select * from  SplitString('" + prdDetails + "','|')";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = myQueryString;
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;
                if (cntr > 0)
                {
                    long MySequence = 1;
                    for (int i = 0; i <= (cntr - 1); i++)
                    {
                        long PrdID = Convert.ToInt64(ds.Tables[0].Rows[i]["part"].ToString());
                        i = i + 1;
                        long uomId = Convert.ToInt64(ds.Tables[0].Rows[i]["part"].ToString());
                        i = i + 1;
                        decimal Qty = Convert.ToDecimal(ds.Tables[0].Rows[i]["part"].ToString());
                        i = i + 1;
                        decimal price = Convert.ToDecimal(ds.Tables[0].Rows[i]["part"].ToString());
                        i = i + 1;
                        long ischanged = Convert.ToInt64(ds.Tables[0].Rows[i]["part"].ToString());
                        i = i + 1;
                        decimal totalprice = Convert.ToDecimal(ds.Tables[0].Rows[i]["part"].ToString());

                        SqlCommand cmd1 = new SqlCommand();
                        SqlDataAdapter da1 = new SqlDataAdapter();
                        DataSet ds1 = new DataSet();
                        DataTable dt1 = new DataTable();
                        string dsprdDetails = "select ProductCode,Name , Description  from mProduct where ID=" + PrdID + "";
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = dsprdDetails;
                        cmd1.Connection = conn;
                        cmd1.Parameters.Clear();
                        da1.SelectCommand = cmd1;
                        da1.Fill(ds1, "tbl2");
                        dt1 = ds1.Tables[0];

                        string name = ds1.Tables[0].Rows[0]["Name"].ToString();
                        string descr = ds1.Tables[0].Rows[0]["Description"].ToString();
                        string code = ds1.Tables[0].Rows[0]["ProductCode"].ToString();

                        SqlCommand cmd3 = new SqlCommand();
                        SqlDataAdapter da3 = new SqlDataAdapter();
                        DataSet ds3 = new DataSet();
                        DataTable dt3 = new DataTable();

                        string insPrdDetails = "insert into torderdetail(OrderHeadId,SkuId,OrderQty,UOMID,Sequence,Prod_Name,Prod_Description,Prod_Code,RemaningQty,Price,Total,IsPriceChange) values(" + RequestID + "," + PrdID + "," + Qty + "," + uomId + "," + MySequence + ",'" + name + "','" + descr + "','" + code + "'," + Qty + "," + price + "," + totalprice + "," + ischanged + ")";
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = insPrdDetails;
                        cmd3.Connection = conn;
                        cmd3.Parameters.Clear();
                        da3.SelectCommand = cmd3;
                        da3.Fill(ds3, "tbl3");
                        //dt3 = ds3.Tables[0];

                        //after Save
                        MySequence = MySequence + 1;
                    }
                }

               

                SqlCommand cmd7 = new SqlCommand();
                SqlDataAdapter da7 = new SqlDataAdapter();
                DataSet ds7 = new DataSet();
                DataTable dt7 = new DataTable();
                cmd7.CommandType = CommandType.Text;
                cmd7.CommandText = "select * from mPaymentMethodDetail where PMethodID=" + PaymentMethodID + "";
                cmd7.Connection = conn;
                cmd7.Parameters.Clear();
                da7.SelectCommand = cmd7;
                da7.Fill(ds7, "dtl7");
                dt7 = ds7.Tables[0];
                if (dt7.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt7.Rows.Count - 1; i++)
                    {
                        int StatutoryId = Convert.ToInt32(dt7.Rows[i]["ID"].ToString());
                        if (StatutoryId == 5)
                        {
                            SqlCommand cmd12 = new SqlCommand();
                            SqlDataAdapter da12 = new SqlDataAdapter();
                            DataSet ds12 = new DataSet();
                            DataTable dt12 = new DataTable();
                            cmd12.CommandType = CommandType.Text;
                            cmd12.CommandText = "select ID from mcostcentermain where CenterName='" + PMField1 + "'";
                            cmd12.Connection=conn;
                            cmd12.Parameters.Clear();
                            da12.SelectCommand = cmd12;
                            da12.Fill(ds12, "dtl12");
                            dt12 = ds12.Tables[0];
                            if (dt12.Rows.Count > 0)
                            {
                                PMField1 = dt12.Rows[0]["ID"].ToString();
                            }

                        }
                        int Sequence = Convert.ToInt32(dt7.Rows[i]["Sequence"].ToString());
                        SqlCommand cmd8 = new SqlCommand();
                        SqlDataAdapter da8 = new SqlDataAdapter();
                        DataSet ds8 = new DataSet();
                        DataTable dt8 = new DataTable();
                        string Object = "RequestPartDetail";
                        int active = 1;
                        int companyid = 0;
                        cmd8.CommandType = CommandType.Text;
                        if (Sequence == 1)
                        {
                            cmd8.CommandText = "Insert into tStatutorydetail(ObjectName,ReferenceID,StatutoryID,StatutoryValue,Active,CreatedBy,CreatedDate,CompanyID,Sequence)values('" + Object + "'," + RequestID + "," + StatutoryId + ",'" + PMField1 + "'," + active + "," + uId + ",'" + creationdate + "'," + companyid + "," + Sequence + ")";

                        }
                        else if (Sequence == 2)
                        {
                            cmd8.CommandText = "Insert into tStatutorydetail(ObjectName,ReferenceID,StatutoryID,StatutoryValue,Active,CreatedBy,CreatedDate,CompanyID,Sequence)values('" + Object + "'," + RequestID + "," + StatutoryId + ",'" + PMField2 + "'," + active + "," + uId + ",'" + creationdate + "'," + companyid + "," + Sequence + ")";
                        }
                        cmd8.Connection = conn;
                        cmd8.Parameters.Clear();
                        da8.SelectCommand = cmd8;
                        da8.Fill(ds8, "dtl8");

                    }
                }


                string Doctype1 = context.Request.Form["txtDocType1"];
                string Docname1 = context.Request.Form["txtDocName1"];
                string Docpath1 = context.Request.Form["txtDocPath1"];

                string extension1 = "";
                string doc1 = "";
                string filename1 = "";
                string documentDownloadPath1 = "";
                string docSave1 = "";
                string documentSavePath1 = "";
                if (Docpath1 != String.Empty && Docpath1 != null && Docpath1 != "" && Docpath1 != "null")
                {
                    extension1 = Docpath1.Split('.').Last();                  
                    filename1 = Path.GetFileNameWithoutExtension(Docpath1);                 
                    docSave1 = Path.GetDirectoryName(Docpath1);
                    documentDownloadPath1 = "http://testoms.gwclogistics.com:8080/GWCMobile/Mobile/" + docSave1 + "/" + RequestID + "/" + filename1 + "";
                    documentSavePath1 = "C:\\GWCMobile\\Mobile\\" + docSave1 + "\\" + RequestID + "\\";

                }




                string Doctype2 = context.Request.Form["txtDocType2"];
                string Docname2 = context.Request.Form["txtDocName2"];
                string Docpath2 = context.Request.Form["txtDocPath2"];

                string extension2 = "";
                string doc2 = "";
                string filename2 = "";
                string documentDownloadPath2 = "";
                string docSave2 = "";
                string documentSavePath2 = "";
                if (Docpath2 != String.Empty && Docpath2 != null && Docpath2 != "" && Docpath2 != "null")
                {
                    extension2 = Docpath2.Split('.').Last();                   
                    filename2 = Path.GetFileNameWithoutExtension(Docpath2);                    
                    docSave2 = Path.GetDirectoryName(Docpath2);
                    documentDownloadPath2 = "http://testoms.gwclogistics.com:8080/GWCMobile/Mobile/" + docSave2 + "/" + RequestID + "/" + filename2 + "";
                    documentSavePath2 = "C:\\GWCMobile\\Mobile\\" + docSave2 + "\\" + RequestID + "\\";
                }


                string Doctype3 = context.Request.Form["txtDocType3"];
                string Docname3 = context.Request.Form["txtDocName3"];
                string Docpath3 = context.Request.Form["txtDocPath3"];

                string extension3 = "";
                string doc3 = "";
                string filename3 = "";
                string documentDownloadPath3 = "";
                string docSave3 = "";
                string documentSavePath3 = "";
                if (Docpath3 != String.Empty && Docpath3 != null && Docpath3 != "" && Docpath3 != "null")
                {
                    extension3 = Docpath3.Split('.').Last();                    
                    filename3 = Path.GetFileNameWithoutExtension(Docpath3);                   
                    docSave3 = Path.GetDirectoryName(Docpath3);
                    documentDownloadPath3 = "http://testoms.gwclogistics.com:8080/GWCMobile/Mobile/" + docSave3 + "/" + RequestID + "/" + filename3 + "";
                    documentSavePath3 = "C:\\GWCMobile\\Mobile\\" + docSave3 + "\\" + RequestID + "\\";
                }


                string Doctype4 = context.Request.Form["txtDocType4"];
                string Docname4 = context.Request.Form["txtDocName4"];
                string Docpath4 = context.Request.Form["txtDocPath4"];

                string extension4 = "";
                string doc4 = "";
                string filename4 = "";
                string documentDownloadPath4 = "";
                string docSave4 = "";
                string documentSavePath4 = "";
                if (Docpath4 != String.Empty && Docpath4 != null && Docpath4 != "" && Docpath4 != "null")
                {
                    extension4 = Docpath4.Split('.').Last();                  
                    filename4 = Path.GetFileNameWithoutExtension(Docpath4);                
                    docSave4 = Path.GetDirectoryName(Docpath4);
                    documentDownloadPath4 = "http://testoms.gwclogistics.com:8080/GWCMobile/Mobile/" + docSave4 + "/" + RequestID + "/" + filename4 + "";
                    documentSavePath4 = "C:\\GWCMobile\\Mobile\\" + docSave4 + "\\" + RequestID + "\\";
                }


                SqlCommand cmd10 = new SqlCommand();
                SqlDataAdapter da10 = new SqlDataAdapter();
                DataSet ds10 = new DataSet();
                DataTable dt10 = new DataTable();
                cmd10.CommandType = CommandType.Text;
                cmd10.CommandText = "Select ParentID from mTerritory  where ID=" + StoreId + "";
                cmd10.Connection = conn;
                cmd10.Parameters.Clear();
                da10.SelectCommand = cmd10;
                da10.Fill(ds10, "tbl10");
                dt10 = ds10.Tables[0];
                if (dt10.Rows.Count > 0)
                {
                    long CompanyID = Convert.ToInt64(dt10.Rows[0]["ParentID"].ToString());
                    if (Docname1 != "" && Docpath1 != "" && Doctype1 != "")
                    {
                        InsertDocument(RequestID, Docname1, documentDownloadPath1, documentSavePath1, extension1, uId, creationdate, CompanyID, Doctype1);
                        string dirattachment = context.Server.MapPath("~/Mobile/Attachment/" + RequestID);
                        if (!Directory.Exists(dirattachment))
                        {
                            Directory.CreateDirectory(dirattachment);
                        }
                        string sourcePath = context.Server.MapPath("~/Mobile/Attachment/" + Path.GetFileName(Docpath1));
                        string destinationPath = context.Server.MapPath("~/Mobile/Attachment/" + RequestID + "/" + Path.GetFileName(Docpath1));
                        if (File.Exists(sourcePath))
                        {
                            File.Move(sourcePath, destinationPath);
                        }

                    }
                    if (Docname2 != "" && Docpath2 != "" && Doctype2 != "")
                    {
                        InsertDocument(RequestID, Docname2, documentDownloadPath2, documentSavePath2, extension2, uId, creationdate, CompanyID, Doctype2);
                        string dirattachment = context.Server.MapPath("~/Mobile/Attachment/" + RequestID);
                        if (!Directory.Exists(dirattachment))
                        {
                            Directory.CreateDirectory(dirattachment);
                        }
                        string sourcePath = context.Server.MapPath("~/Mobile/Attachment/" + Path.GetFileName(Docpath2));
                        string destinationPath = context.Server.MapPath("~/Mobile/Attachment/" + RequestID + "/" + Path.GetFileName(Docpath2));
                        if (File.Exists(sourcePath))
                        {
                            File.Move(sourcePath, destinationPath);
                        }
                    }
                    if (Docname3 != "" && Docpath3 != "" && Doctype3 != "")
                    {
                        InsertDocument(RequestID, Docname3, documentDownloadPath3, documentSavePath3, extension3, uId, creationdate, CompanyID, Doctype3);
                        string dirattachment = context.Server.MapPath("~/Mobile/Attachment/" + RequestID);
                        if (!Directory.Exists(dirattachment))
                        {
                            Directory.CreateDirectory(dirattachment);
                        }
                        string sourcePath = context.Server.MapPath("~/Mobile/Attachment/" + Path.GetFileName(Docpath3));
                        string destinationPath = context.Server.MapPath("~/Mobile/Attachment/" + RequestID + "/" + Path.GetFileName(Docpath3));
                        if (File.Exists(sourcePath))
                        {
                            File.Move(sourcePath, destinationPath);
                        }
                    }
                    if (Docname4 != "" && Docpath4 != "" && Doctype4 != "")
                    {
                        InsertDocument(RequestID, Docname4, documentDownloadPath4, documentSavePath4, extension4, uId, creationdate, CompanyID, Doctype4);
                        string dirattachment = context.Server.MapPath("~/Mobile/Attachment/" + RequestID);
                        if (!Directory.Exists(dirattachment))
                        {
                            Directory.CreateDirectory(dirattachment);
                        }
                        string sourcePath = context.Server.MapPath("~/Mobile/Attachment/" + Path.GetFileName(Docpath4));
                        string destinationPath = context.Server.MapPath("~/Mobile/Attachment/" + RequestID + "/" + Path.GetFileName(Docpath4));
                        if (File.Exists(sourcePath))
                        {
                            File.Move(sourcePath, destinationPath);
                        }
                    }
                }




                objService.SetApproverDataafterSave("RequestPartDetail", RequestID, uId.ToString(), 2, StoreId, 0, profile.DBConnection._constr);
                String xmlString = String.Empty;
                // String orderNo = String.Empty;

                context.Response.ContentType = "text/xml";
                xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xmlString = xmlString + "<gwcInfo>";
                xmlString = xmlString + "<newRequestInfo>";
                if (RequestID > 0)
                {
                    xmlString = xmlString + "<requestmsg>success</requestmsg>";
                    //xmlString = xmlString + "<orderno> Order ID : " + RequestID + "</orderno>";
                   // xmlString = xmlString + "<gwcOrderLabel> " + OrderNo + "</gwcOrderLabel>";
                    xmlString = xmlString + "<orderno> Order ID : " + OrderNo + "</orderno>";

                }
                else
                {
                    xmlString = xmlString + "<requestmsg>failed</requestmsg>";
                }

                xmlString = xmlString + "</newRequestInfo>";
                xmlString = xmlString + "</gwcInfo>";

                context.Response.Write(xmlString);
            }
        }





        public void InsertDocument(long orderid, string title, string documentDownloadPath, string documentSavePath, string extension, long createdby, DateTime creationdate, long companyid, string doctype)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd11 = new SqlCommand();
            SqlDataAdapter da11 = new SqlDataAdapter();
            DataSet ds11 = new DataSet();
            DataTable dt11 = new DataTable();

            cmd11.CommandType = CommandType.Text;
            cmd11.CommandText = "select * from tDocument where ReferenceId=" + orderid + " Order By Sequence desc";
            cmd11.Connection = conn;
            cmd11.Parameters.Clear();
            da11.SelectCommand = cmd11;
            da11.Fill(ds11, "dt11");
            dt11 = ds11.Tables[0];
            string Downloadaccess = "Public";
            string Active = "Y";
            string objectName = "RequestPartDetailDocument";
            if (dt11.Rows.Count > 0)
            {
              
                long sequence = Convert.ToInt64(dt11.Rows[0]["Sequence"].ToString());
                sequence = sequence + 1;

                SqlCommand cmd9 = new SqlCommand();
                SqlDataAdapter da9 = new SqlDataAdapter();
                DataSet ds9 = new DataSet();
                DataTable dt9 = new DataTable();
                //cmd9.CommandType = CommandType.Text;
                //cmd9.CommandText = "Insert into tDocument(ObjectName,ReferenceID,DocumentName,Sequence,DocumentDownloadPath,DocumentSavePath,FileType,DowloadAccess,Active,CreatedBy,CreationDate,CompanyID,DocumentType)values('" + objectName + "'," + orderid + ",'" + filename + "'," + sequence + ",'" + documentDownloadPath + "','" + documentSavePath + "','" + extension + "','"+ Downloadaccess +"','"+ Active +"'," + createdby + ",'" + creationdate + "'," + companyid + ",'" + doctype + "')";
                //cmd9.Connection = conn;
                //cmd9.Parameters.Clear();
                //da9.SelectCommand = cmd9;
                //da9.Fill(ds9, "dtl9");
                 conn.Open();
                cmd9.CommandType = CommandType.StoredProcedure;
                cmd9.CommandText = "SP_InsertDocument";
                cmd9.Parameters.AddWithValue("@p_ObjectName", objectName);
                cmd9.Parameters.AddWithValue("@p_ReferenceID", orderid);
                cmd9.Parameters.AddWithValue("@p_DocumentName", CheckString(title));
                cmd9.Parameters.AddWithValue("@p_DocumentDownloadPath", CheckString(documentDownloadPath));
                cmd9.Parameters.AddWithValue("@p_DocumentSavePath", CheckString(documentSavePath));
                cmd9.Parameters.AddWithValue("@p_FileType", extension);
                cmd9.Parameters.AddWithValue("@p_Sequence", sequence);
                cmd9.Parameters.AddWithValue("@p_CreatedBy", createdby);
                cmd9.Parameters.AddWithValue("@p_CreationDate", DateTime.Now);
                cmd9.Parameters.AddWithValue("@p_CompanyID", companyid);
                cmd9.Parameters.AddWithValue("@p_DocumentType", doctype);
                cmd9.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd9.Connection = conn;
                cmd9.ExecuteNonQuery();
                long docID = Convert.ToInt64(cmd9.Parameters["@id"].Value.ToString());

                SqlCommand cmd10 = new SqlCommand();
                SqlDataAdapter da10 = new SqlDataAdapter();
                DataSet ds10 = new DataSet();
                DataTable dt10 = new DataTable();


                cmd10.CommandType = CommandType.Text;
                cmd10.CommandText = "insert into tDocumentDetail(ObjectName,ReferenceID,DocumentID) values('" + objectName + "'," + orderid + "," + docID + ")";
                cmd10.Connection = conn;
                cmd10.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                //SqlCommand cmd9 = new SqlCommand();
                //SqlDataAdapter da9 = new SqlDataAdapter();
                //DataSet ds9 = new DataSet();
                //DataTable dt9 = new DataTable();
                //long sequence = 1;
                //cmd9.CommandType = CommandType.Text;
                //cmd9.CommandText = "Insert into tDocument(ObjectName,ReferenceID,DocumentName,Sequence,DocumentDownloadPath,DocumentSavePath,FileType,Active,CreatedBy,CreationDate,CompanyID,DocumentType)values('" + objectName + "'," + orderid + ",'" + filename + "'," + sequence + ",'" + documentDownloadPath + "','" + documentSavePath + "','" + extension + "','" + Downloadaccess + "','" + Active + "'," + createdby + ",'" + creationdate + "'," + companyid + ",'" + doctype + "')";
                //cmd9.Connection = conn;
                //cmd9.Parameters.Clear();
                //da9.SelectCommand = cmd9;
                //da9.Fill(ds9, "dtl9");
                SqlCommand cmd9 = new SqlCommand();
                SqlDataAdapter da9 = new SqlDataAdapter();
                DataSet ds9 = new DataSet();
                DataTable dt9 = new DataTable();
                long sequence = 1;
                conn.Open();
                cmd9.CommandType = CommandType.StoredProcedure;
                cmd9.CommandText = "SP_InsertDocument";
                cmd9.Parameters.AddWithValue("@p_ObjectName", objectName);
                cmd9.Parameters.AddWithValue("@p_ReferenceID", orderid);
                cmd9.Parameters.AddWithValue("@p_DocumentName", CheckString(title));
                cmd9.Parameters.AddWithValue("@p_DocumentDownloadPath", documentDownloadPath);
                cmd9.Parameters.AddWithValue("@p_DocumentSavePath", CheckString(documentSavePath));
                cmd9.Parameters.AddWithValue("@p_FileType", extension);
                cmd9.Parameters.AddWithValue("@p_Sequence", sequence);
                cmd9.Parameters.AddWithValue("@p_CreatedBy", createdby);
                cmd9.Parameters.AddWithValue("@p_CreationDate", DateTime.Now);
                cmd9.Parameters.AddWithValue("@p_CompanyID", companyid);
                cmd9.Parameters.AddWithValue("@p_DocumentType", doctype);
                cmd9.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd9.Connection = conn;
                cmd9.ExecuteNonQuery();
                long docID = Convert.ToInt64(cmd9.Parameters["@id"].Value.ToString());

                SqlCommand cmd10 = new SqlCommand();
                SqlDataAdapter da10 = new SqlDataAdapter();
                DataSet ds10 = new DataSet();
                DataTable dt10 = new DataTable();


                cmd10.CommandType = CommandType.Text;
                cmd10.CommandText = "insert into tDocumentDetail(ObjectName,ReferenceID,DocumentID) values('" + objectName + "'," + orderid + "," + docID + ")";
                cmd10.Connection = conn;
                cmd10.ExecuteNonQuery();
                conn.Close();
            }


        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            return value;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}