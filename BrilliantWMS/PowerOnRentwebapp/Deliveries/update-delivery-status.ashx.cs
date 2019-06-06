using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace BrilliantWMS.Deliveries
{
    /// <summary>
    /// Summary description for update_delivery_status
    /// </summary>
    public class update_delivery_status : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd2 = new SqlCommand();
        SqlDataAdapter da2 = new SqlDataAdapter();
        DataSet ds2 = new DataSet();
        DataTable dt2 = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        String attachment1 = "";
        String attachment2 = "";
        String attachment3 = "";
        String attachment4 = "";
        String signature = "";
        SqlConnection conn;
        long OrderId = 0;
        String Deliverymode = "";
        String Remark = "";
        String PhotoIdType = "";
        long CompanyID = 0;
        long CreatedBy = 0;
        long Sequence = 0;
        string ObjectName = "RequestPartDetailDocument";
        string OdrID = "";

        string extension1 = "";
        string doc1 = "";
        string filename1 = "";
        string documentDownloadPath1 = "";
        string docSave1 = "";
        string documentSavePath1 = "";

        string extension2 = "";
        string doc2 = "";
        string filename2 = "";
        string documentDownloadPath2 = "";
        string docSave2 = "";
        string documentSavePath2 = "";

        string extension3 = "";
        string doc3 = "";
        string filename3 = "";
        string documentDownloadPath3 = "";
        string docSave3 = "";
        string documentSavePath3 = "";

        string extension4 = "";
        string doc4 = "";
        string filename4 = "";
        string documentDownloadPath4 = "";
        string docSave4 = "";
        string documentSavePath4 = "";

        string extension5 = "";
        string doc5 = "";
        string filename5 = "";
        string documentDownloadPath5 = "";
        string docSave5 = "";
        string documentSavePath5 = "";

        long DeptId = 0;
        public void ProcessRequest(HttpContext context)
        {
            conn = new SqlConnection(strcon);

            OdrID = context.Request.Form["txtOrderId"];
            Deliverymode = context.Request.Form["txtDeliveryMode"];
            Remark = context.Request.Form["txtReturnRemark"];
            PhotoIdType = context.Request.Form["txtPhotoIdType"];


            SqlCommand cmd9 = new SqlCommand();
            SqlDataAdapter da9 = new SqlDataAdapter();
            DataTable dt9 = new DataTable();
            DataSet ds9 = new DataSet();
            cmd9.CommandType = CommandType.Text;
            cmd9.CommandText = "select Id from tOrderHead where OrderNo='" + OdrID + "'";
            cmd9.Connection = conn;
            cmd9.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);
            da9.SelectCommand = cmd9;
            da9.Fill(ds9, "tbl10");
            dt9 = ds9.Tables[0];

            OrderId = Convert.ToInt64(dt9.Rows[0]["Id"].ToString());
            if (context.Request.Form["txtAttachment1"] != "" && context.Request.Form["txtAttachment1"] != null && context.Request.Form["txtAttachment1"] != string.Empty && context.Request.Form["txtAttachment1"] != "null")
            {
                string[] aa = context.Request.Form["txtAttachment1"].Split('/').ToArray();
                aa[1] = Convert.ToString(OrderId);
                attachment1 = aa[0] + "/" + aa[1] + "/" + aa[2];
                if (attachment1 != String.Empty && attachment1 != null && attachment1 != "" && attachment1 != "null")
                {
                    extension1 = attachment1.Split('.').Last();
                    doc1 = attachment1.Split('.').First();
                    filename1 = Path.GetFileNameWithoutExtension(attachment1);
                    documentDownloadPath1 = "http://testoms.gwclogistics.com:8080/GWCMobile/Deliveries/" + doc1 + "";
                    docSave1 = Path.GetDirectoryName(attachment1);
                    documentSavePath1 = "C:\\GWCMobile\\Deliveries\\" + docSave1 + "\\";

                }
            }
            if (context.Request.Form["txtAttachment2"] != "" && context.Request.Form["txtAttachment2"] != null && context.Request.Form["txtAttachment2"] != string.Empty && context.Request.Form["txtAttachment2"] != "null")
            {
                string[] bb = context.Request.Form["txtAttachment2"].Split('/').ToArray();
                bb[1] = Convert.ToString(OrderId);
                attachment2 = bb[0] + "/" + bb[1] + "/" + bb[2];
                if (attachment2 != String.Empty && attachment2 != null && attachment2 != "" && attachment2 != "null")
                {
                    extension2 = attachment2.Split('.').Last();
                    doc2 = attachment2.Split('.').First();
                    filename2 = Path.GetFileNameWithoutExtension(attachment2);
                    documentDownloadPath2 = "http://testoms.gwclogistics.com:8080/GWCMobile/Deliveries/" + doc2 + "";
                    docSave2 = Path.GetDirectoryName(attachment2);
                    documentSavePath2 = "C:\\GWCMobile\\Deliveries\\" + docSave2 + "\\";
                }
            }
            if (context.Request.Form["txtAttachment3"] != "" && context.Request.Form["txtAttachment3"] != null && context.Request.Form["txtAttachment3"] != string.Empty && context.Request.Form["txtAttachment3"] != "null")
            {
                string[] cc = context.Request.Form["txtAttachment3"].Split('/').ToArray();
                cc[1] = Convert.ToString(OrderId);
                attachment3 = cc[0] + "/" + cc[1] + "/" + cc[2];

                if (attachment3 != String.Empty && attachment3 != null && attachment3 != "" && attachment3 != "null")
                {
                    extension3 = attachment3.Split('.').Last();
                    doc3 = attachment3.Split('.').First();
                    filename3 = Path.GetFileNameWithoutExtension(attachment3);
                    documentDownloadPath3 = "http://testoms.gwclogistics.com:8080/GWCMobile/Deliveries/" + doc3 + "";
                    docSave3 = Path.GetDirectoryName(attachment3);
                    documentSavePath3 = "C:\\GWCMobile\\Deliveries\\" + docSave3 + "\\";
                }

            }
            if (context.Request.Form["txtAttachment4"] != "" && context.Request.Form["txtAttachment4"] != null && context.Request.Form["txtAttachment4"] != string.Empty && context.Request.Form["txtAttachment4"] != "null")
            {
                string[] dd = context.Request.Form["txtAttachment4"].Split('/').ToArray();
                dd[1] = Convert.ToString(OrderId);
                attachment4 = dd[0] + "/" + dd[1] + "/" + dd[2];

                if (attachment4 != String.Empty && attachment4 != null && attachment4 != "" && attachment4 != "null")
                {
                    extension4 = attachment4.Split('.').Last();
                    doc4 = attachment4.Split('.').First();
                    filename4 = Path.GetFileNameWithoutExtension(attachment4);
                    documentDownloadPath4 = "http://testoms.gwclogistics.com:8080/GWCMobile/Deliveries/" + doc4 + "";
                    docSave4 = Path.GetDirectoryName(attachment4);
                    documentSavePath4 = "C:\\GWCMobile\\Deliveries\\" + docSave4 + "\\";
                }
            }
            if (context.Request.Form["txtSignature"] != " " && context.Request.Form["txtSignature"] != null && context.Request.Form["txtSignature"] != string.Empty && context.Request.Form["txtSignature"] != "null")
            {
                string[] ee = context.Request.Form["txtSignature"].Split('/').ToArray();
                ee[1] = Convert.ToString(OrderId);
                signature = ee[0] + "/" + ee[1] + "/" + ee[2];
                if (signature != String.Empty && signature != null && signature != "" && signature != "null")
                {
                    extension5 = signature.Split('.').Last();
                    doc5 = signature.Split('.').First();
                    filename5 = Path.GetFileNameWithoutExtension(signature);
                    documentDownloadPath5 = "http://testoms.gwclogistics.com:8080/GWCMobile/Deliveries/" + doc5 + "";
                    docSave5 = Path.GetDirectoryName(signature);
                    documentSavePath5 = "C:\\GWCMobile\\Deliveries\\" + docSave5 + "\\";

                }
            }

            context.Response.ContentType = "text/xml";
            string xmlString = string.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
            xmlString = xmlString + "<updatedeliverystatus>";

            if (context.Request.Form["txtOrderId"] != null)
            {
                if (Deliverymode == "Delivered" || Deliverymode == "Dispatch")
                {

                    SqlCommand cmd6 = new SqlCommand();
                    SqlDataAdapter da6 = new SqlDataAdapter();
                    DataSet ds6 = new DataSet();
                    DataTable dt6 = new DataTable();
                    cmd6.CommandType = CommandType.Text;
                    cmd6.CommandText = "select * from tDriverassignDetail where OrderId=" + OrderId + "";
                    cmd6.Connection = conn;
                    cmd6.Parameters.Clear();
                    da6.SelectCommand = cmd6;
                    da6.Fill(ds6, "tbl6");
                    dt6 = ds6.Tables[0];
                    if (dt6.Rows.Count > 0)
                    {
                        CreatedBy = Convert.ToInt64(dt6.Rows[0]["DriverID"].ToString());
                    }


                    SqlCommand cmd4 = new SqlCommand();
                    SqlDataAdapter da4 = new SqlDataAdapter();
                    DataSet ds4 = new DataSet();
                    DataTable dt4 = new DataTable();

                    cmd4.CommandType = CommandType.Text;
                    cmd4.CommandText = "select * from tOrderHead where Id=" + OrderId + "";
                    cmd4.Connection = conn;
                    cmd4.Parameters.Clear();
                    da4.SelectCommand = cmd4;
                    da4.Fill(ds4, "tbl4");
                    dt4 = ds4.Tables[0];

                    if (dt4.Rows.Count > 0)
                    {
                        DeptId = Convert.ToInt64(dt4.Rows[0]["StoreId"].ToString());
                        SqlCommand cmd5 = new SqlCommand();
                        SqlDataAdapter da5 = new SqlDataAdapter();
                        DataSet ds5 = new DataSet();
                        DataTable dt5 = new DataTable();


                        cmd5.CommandType = CommandType.Text;
                        cmd5.CommandText = "Select ParentID from mTerritory  where ID=" + DeptId + "";
                        cmd5.Connection = conn;
                        cmd5.Parameters.Clear();
                        da5.SelectCommand = cmd5;
                        da5.Fill(ds5, "tbl5");
                        dt5 = ds5.Tables[0];
                        if (dt5.Rows.Count > 0)
                        {
                            CompanyID = Convert.ToInt64(dt5.Rows[0]["ParentID"].ToString());
                        }


                    }

                    // long MySequence = 1;
                    //string[] filePaths = Directory.GetFiles(context.Server.MapPath("~/Deliveries/Attachment/" + OrderId));
                    //List<ListItem> files = new List<ListItem>();
                    //foreach (string filePath in filePaths)
                    //{

                    //    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    //    string FileName = Path.GetFileName(filePath);
                    //    string ext = Path.GetExtension(filePath);
                    //    string Filepath = Path.GetFullPath(filePath);
                    //    string Active = "1";

                    if (attachment1 != String.Empty && attachment1 != null && attachment1 != "" && attachment1 != "null")
                    {

                        insertAttachnment(ObjectName, OrderId, filename1, documentDownloadPath1, documentSavePath1, extension1, PhotoIdType);
                    }
                    if (attachment2 != String.Empty && attachment2 != null && attachment2 != "" && attachment2 != "null")
                    {
                        insertAttachnment(ObjectName, OrderId, filename2, documentDownloadPath2, documentSavePath2, extension2, PhotoIdType);
                    }

                    if (attachment3 != String.Empty && attachment3 != null && attachment3 != "" && attachment3 != "null")
                    {
                        insertAttachnment(ObjectName, OrderId, filename3, documentDownloadPath3, documentSavePath3, extension3, PhotoIdType);
                    }

                    if (attachment4 != String.Empty && attachment4 != null && attachment4 != "" && attachment4 != "null")
                    {
                        insertAttachnment(ObjectName, OrderId, filename4, documentDownloadPath4, documentSavePath4, extension4, PhotoIdType);
                    }

                    if (signature != String.Empty && signature != null && signature != "" && signature != "null")
                    {
                        string sig = "Signature";
                        insertAttachnment(ObjectName, OrderId, filename5, documentDownloadPath5, documentSavePath5, extension5, sig);
                    }

                    //cmd.CommandType = CommandType.Text;
                    //cmd.Connection = conn;
                    //cmd.CommandText = "update mCustomerDetail set PhotoIdtype='" + PhotoIdType + "' where OrderID=" + OrderId + "";
                    //da.SelectCommand = cmd;
                    //da.Fill(ds, "tbl");
                    ////dt = ds.Tables[0];
                    ////string MsgDescription=OrderId +"|" + DateTime.Now+"| Dispatch | NA";
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;
                    cmd2.CommandText = "update tOrderHead set Remark='" + Remark + "',Status=8,CompletedDate='" + DateTime.Now.ToLongTimeString() + "' where Id=" + OrderId + "";
                    //insert into tCorrespond(OrderHeadId,MessageTitle,Message,date,MessageSource,MessageType,DepartmentID,CurrentOrderStatus,MailStatus,Archive)values(" + OrderId + ",'Your Order is Dispatched','Your Order is Dispatched'," + DateTime.Now.ToString() + ",'Scheduler','Information',"+ DeptId +",8,0,0)";

                    da2.SelectCommand = cmd2;
                    da2.Fill(ds2, "tbl2");
                    //dt2 = ds2.Tables[0];

                    insertintoCorrespondTbl(OrderId, DeptId);

                    xmlString = xmlString + "<orderid>" + OrderId + "</orderid>";
                    xmlString = xmlString + "<status>updated</status>";

                    /*New Code For Dispatch Mail Sent Start*/
                    DispatchMailSent(DeptId, OrderId);
                    /*New Code For Dispatch Mail Sent Start*/

                }
                else if (Deliverymode == "Return")
                {
                    //cmd.CommandType = CommandType.Text;
                    //cmd.Connection = conn;
                    //cmd.CommandText = "update mCustomerDetail set PhotoIdtype='" + PhotoIdType + "' where OrderID=" + OrderId + "";
                    //da.SelectCommand = cmd;
                    //da.Fill(ds, "tbl");

                    cmd2.CommandType = CommandType.Text;
                    cmd2.Connection = conn;
                    cmd2.CommandText = "update tOrderHead set Remark='" + Remark + "',Status=26,CompletedDate='" + DateTime.Now.ToLongTimeString() + "' where Id=" + OrderId + "";
                    da2.SelectCommand = cmd2;
                    da2.Fill(ds2, "tbl2");
                    //dt2 = ds2.Tables[0];



                    //SqlCommand cmd8 = new SqlCommand();
                    //SqlDataAdapter da8 = new SqlDataAdapter();
                    //DataSet ds8 = new DataSet();
                    //DataTable dt8 = new DataTable();
                    //cmd8.CommandType = CommandType.Text;
                    //cmd8.Connection = conn;
                    //cmd8.CommandText = "update mPaymentDetail set Remark='" + Remark + "' where OrderID=" + OrderId + "";
                    //da8.SelectCommand = cmd8;
                    //da8.Fill(ds8, "tbl8");

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.CommandText = "select * from tOrderDetail where OrderHeadId=" + OrderId + " ";
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tbl");
                    dt = ds.Tables[0];
                    int cnt = dt.Rows.Count;
                    if (cnt > 0)
                    {
                        for (int i = 0; i <= cnt - 1; i++)
                        {
                            decimal AvailableBalanceQty = 0;
                            decimal TotalDispatchQty = 0;
                            decimal returnQty = Convert.ToDecimal(dt.Rows[i]["OrderQty"].ToString());
                            long skuid = Convert.ToInt64(dt.Rows[i]["SkuId"].ToString());

                            SqlCommand cmd3 = new SqlCommand();
                            SqlDataAdapter da3 = new SqlDataAdapter();
                            DataTable dt3 = new DataTable();
                            DataSet ds3 = new DataSet();
                            cmd3.CommandType = CommandType.Text;
                            cmd3.Connection = conn;
                            cmd3.CommandText = "select * from tProductStockDetails where ProdID=" + skuid + " ";
                            da3.SelectCommand = cmd3;
                            da3.Fill(ds3, "tbl3");
                            dt3 = ds3.Tables[0];
                            if (dt3.Rows.Count > 0)
                            {
                                AvailableBalanceQty = Convert.ToDecimal(dt3.Rows[0]["AvailableBalance"].ToString());
                                TotalDispatchQty = Convert.ToDecimal(dt3.Rows[0]["TotalDispatchQty"].ToString());
                            }

                            decimal TotalAvailableQty = returnQty + AvailableBalanceQty;
                            decimal DispatchQty = TotalDispatchQty - returnQty;

                            SqlCommand cmd7 = new SqlCommand();
                            SqlDataAdapter da7 = new SqlDataAdapter();
                            DataTable dt7 = new DataTable();
                            DataSet ds7 = new DataSet();
                            cmd7.CommandType = CommandType.Text;
                            cmd7.Connection = conn;
                            cmd7.CommandText = "update tProductStockDetails set AvailableBalance=" + TotalAvailableQty + ",TotalDispatchQty=" + DispatchQty + " where ProdID=" + skuid + "";
                            da7.SelectCommand = cmd7;
                            da7.Fill(ds7, "tbl7");
                            //dt4 = ds4.Tables[0];
                        }


                        xmlString = xmlString + "<orderid>" + OrderId + "</orderid>";
                        xmlString = xmlString + "<status>updated</status>";

                        /*New Code For Dispatch Mail Sent Start*/
                        DispatchMailSent(DeptId, OrderId);
                        /*New Code For Dispatch Mail Sent Start*/
                    }
                }
            }
            xmlString = xmlString + "</updatedeliverystatus>";
            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
        }

        public void insertintoCorrespondTbl(long orderID, long deptID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_ChkeckDispatchMessage";
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderID", orderID);
                cmd.Parameters.AddWithValue("@deptId", deptID);
                da.SelectCommand = cmd;
                conn.Open();
                conn.Close();
            }
            catch { }
            finally { conn.Close(); }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void insertAttachnment(string objname, long OrderId, string filename, string DocumentDownloadPath, string DocumentSavePath, string filetype, string phototype)
        {

            SqlCommand cmd4 = new SqlCommand();
            SqlDataAdapter da4 = new SqlDataAdapter();
            DataSet ds4 = new DataSet();
            DataTable dt4 = new DataTable();

            cmd4.CommandType = CommandType.Text;
            cmd4.CommandText = "select * from tDocument where ReferenceId=" + OrderId + " Order By Sequence desc";
            cmd4.Connection = conn;
            cmd4.Parameters.Clear();
            da4.SelectCommand = cmd4;
            da4.Fill(ds4, "dt5");
            dt4 = ds4.Tables[0];
            if (dt4.Rows.Count > 0)
            {
                long sequence = Convert.ToInt64(dt4.Rows[0]["Sequence"].ToString());
                sequence = sequence + 1;

                SqlCommand cmd3 = new SqlCommand();
                SqlDataAdapter da3 = new SqlDataAdapter();
                DataSet ds3 = new DataSet();
                DataTable dt3 = new DataTable();
                conn.Open();
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "SP_InsertDocument";
                //cmd3.CommandType = CommandType.Text;
                //cmd3.CommandText = "insert into tDocument(ReferenceID,DocumentSavePath,CreatedBy,CreationDate,CompanyID) values(@p_ReferenceID,p_DocumentSavePath,p_Createdby,p_CreationDate,p_CompanyID)";
                cmd3.Parameters.AddWithValue("@p_ObjectName", objname);
                cmd3.Parameters.AddWithValue("@p_ReferenceID", OrderId);
                cmd3.Parameters.AddWithValue("@p_DocumentName", filename);
                cmd3.Parameters.AddWithValue("@p_DocumentDownloadPath", DocumentDownloadPath);
                cmd3.Parameters.AddWithValue("@p_DocumentSavePath", DocumentSavePath);
                cmd3.Parameters.AddWithValue("@p_FileType", filetype);
                cmd3.Parameters.AddWithValue("@p_Sequence", sequence);
                cmd3.Parameters.AddWithValue("@p_CreatedBy", CreatedBy);
                cmd3.Parameters.AddWithValue("@p_CreationDate", DateTime.Now);
                cmd3.Parameters.AddWithValue("@p_CompanyID", CompanyID);
                cmd3.Parameters.AddWithValue("@p_DocumentType", phototype);
                cmd3.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd3.Connection = conn;
                cmd3.ExecuteNonQuery();
                long docID = Convert.ToInt64(cmd3.Parameters["@id"].Value.ToString());

                SqlCommand cmd7 = new SqlCommand();
                SqlDataAdapter da7 = new SqlDataAdapter();
                DataSet ds7 = new DataSet();
                DataTable dt7 = new DataTable();


                cmd7.CommandType = CommandType.Text;
                cmd7.CommandText = "insert into tDocumentDetail(ObjectName,ReferenceID,DocumentID) values('" + ObjectName + "'," + OrderId + "," + docID + ")";
                cmd7.Connection = conn;
                cmd7.ExecuteNonQuery();
                conn.Close();

                //cmd3.Parameters.Clear();
                //da3.SelectCommand = cmd3;
                //da3.Fill(ds3, "tbl3");

            }
            else
            {
                conn.Open();
                SqlCommand cmd3 = new SqlCommand();
                SqlDataAdapter da3 = new SqlDataAdapter();
                DataSet ds3 = new DataSet();
                DataTable dt3 = new DataTable();
                long sequence = 1;
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.CommandText = "SP_InsertDocument";
                //cmd3.CommandType = CommandType.Text;
                //cmd3.CommandText = "insert into tDocument(ReferenceID,DocumentSavePath,CreatedBy,CreationDate,CompanyID) values(@p_ReferenceID,p_DocumentSavePath,p_Createdby,p_CreationDate,p_CompanyID)";
                cmd3.Parameters.AddWithValue("@p_ObjectName", objname);
                cmd3.Parameters.AddWithValue("@p_ReferenceID", OrderId);
                cmd3.Parameters.AddWithValue("@p_DocumentName", filename);
                cmd3.Parameters.AddWithValue("@p_DocumentDownloadPath", DocumentDownloadPath);
                cmd3.Parameters.AddWithValue("@p_DocumentSavePath", DocumentSavePath);
                cmd3.Parameters.AddWithValue("@p_FileType", filetype);
                cmd3.Parameters.AddWithValue("@p_Sequence", sequence);
                cmd3.Parameters.AddWithValue("@p_CreatedBy", CreatedBy);
                cmd3.Parameters.AddWithValue("@p_CreationDate", DateTime.Now);
                cmd3.Parameters.AddWithValue("@p_CompanyID", CompanyID);
                cmd3.Parameters.AddWithValue("@p_DocumentType", phototype);
                cmd3.Parameters.AddWithValue("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd3.Connection = conn;
                cmd3.ExecuteNonQuery();
                long docID = Convert.ToInt64(cmd3.Parameters["@id"].Value.ToString());
                SqlCommand cmd7 = new SqlCommand();
                SqlDataAdapter da7 = new SqlDataAdapter();
                DataSet ds7 = new DataSet();
                DataTable dt7 = new DataTable();


                cmd7.CommandType = CommandType.Text;
                cmd7.CommandText = "insert into tDocumentDetail(ObjectName,ReferenceID,DocumentID) values('" + ObjectName + "'," + OrderId + "," + docID + ")";
                cmd7.Connection = conn;
                cmd7.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DispatchMailSent(long DeptId, long orderID)
        {
            try
            {
                long UserID = 0;
                string UserEmail = "", UserName = "";

                SqlDataAdapter adpDN1 = new SqlDataAdapter();
                DataSet dsDN1 = new DataSet();
                DataTable dtDN1 = new DataTable();
                adpDN1 = new SqlDataAdapter("select * from torderhead where ID=" + orderID + "", conn);
                adpDN1.Fill(dsDN1);

                UserID = Convert.ToInt64(dsDN1.Tables[0].Rows[0]["RequestBy"].ToString());
                UserEmail = GetEmailOfUser(UserID);

                /*Contact Person Emails*/
                long Con1ID = Convert.ToInt64(dsDN1.Tables[0].Rows[0]["ContactId1"].ToString());
                SqlDataAdapter adpDN2 = new SqlDataAdapter();
                DataSet dsDN2 = new DataSet();
                DataTable dtDN2 = new DataTable();
                adpDN2 = new SqlDataAdapter("select EmailID from tcontactpersondetail where ID= " + Con1ID + "", conn);
                adpDN2.Fill(dsDN2);
                if (dsDN2.Tables[0].Rows.Count > 0)
                {
                    string Con1Email = dsDN2.Tables[0].Rows[0]["EmailID"].ToString();
                    if (Con1Email != "" || Con1Email != null)
                    {
                        UserEmail = UserEmail + "," + Con1Email;
                    }

                    string Con2ID = dsDN1.Tables[0].Rows[0]["Con2"].ToString();
                    if (Con2ID == "0" || Con2ID == "") { }
                    else
                    {
                        SqlDataAdapter adpDN3 = new SqlDataAdapter();
                        DataSet dsDN3 = new DataSet();
                        DataTable dtDN3 = new DataTable();
                        adpDN3 = new SqlDataAdapter("select EmailID from tcontactpersondetail where ID in( " + Con2ID + ")", conn);
                        adpDN3.Fill(dsDN3);
                        string Con2Email = "";
                        if (dsDN3.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsDN3.Tables[0].Rows.Count; i++)
                            {
                                if (i == 0) { Con2Email = dsDN3.Tables[0].Rows[i]["EmailID"].ToString(); }
                                else
                                {
                                    Con2Email = Con2Email + "," + dsDN3.Tables[0].Rows[i]["EmailID"].ToString();
                                }
                            }
                            UserEmail = UserEmail + "," + Con2Email;
                        }
                    }
                }


                long Templateid = 0;
                string Message = "", MessageTitle = "";
                DataTable dtCOS8 = new DataTable();
                SqlCommand cmdCOS8 = new SqlCommand("select * from mMessageEMailTemplates where DepartmentID=" + DeptId + " and ActivityID=10 and MessageID=12  and Active='Yes'", conn);
                SqlDataAdapter daCOS8 = new SqlDataAdapter();
                daCOS8.SelectCommand = cmdCOS8;
                daCOS8.Fill(dtCOS8);
                if (dtCOS8.Rows.Count > 0)
                {
                    Templateid = Convert.ToInt64(dtCOS8.Rows[0]["ID"].ToString());
                    Message = dtCOS8.Rows[0]["MailBody"].ToString();
                    MessageTitle = dtCOS8.Rows[0]["MailSubject"].ToString();
                }
                MessageTitle = MessageTitle + " ";
                int k = MailCompose(UserEmail, "All", Message, MessageTitle, orderID);
            }
            catch { }
            finally { }
        }

        public string GetEmailOfUser(long UserID)
        {
            string EmailID = "";
            SqlDataAdapter adp = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            adp = new SqlDataAdapter("select EmailID from mUserProfileHead where ID=" + UserID + "", conn);
            adp.Fill(ds);
            int cnt = ds.Tables[0].Rows.Count;
            dt = ds.Tables[0];
            EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
            return EmailID;
        }
        public string GetNameUser(long UserID)
        {
            string UserName = "";
            SqlDataAdapter adp = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            adp = new SqlDataAdapter("select * from mUserProfileHead where ID=" + UserID + "", conn);
            adp.Fill(ds);
            int cnt = ds.Tables[0].Rows.Count;
            dt = ds.Tables[0];
            UserName = ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString();
            return UserName;
        }

        protected string MailGetFooter()
        {
            string MailFooter = "<br/><br/>" +
                                //"<a href='http://elegantcrm.com/gwc/Login/Login.aspx' target='_blank' style='font-size: 18px; color: #3BB9FF; font-family: Comic Sans MS; text-decoration: none;'>Go to GWC</a>" +
                                "Please <a href='http://elegantcrm.com/gwc/Login/Login.aspx' target='_blank' style='color: #3BB9FF;  text-decoration: none;'>click here </a>  to view the order details." +
                                "<br/><br/>" +
                                "Thank you, <br/>" +
                                "OMS Notification Team<br/>" +
                                "<br/><br/><hr/>" +
                                "<b>SELF EXPRESSION. BY GWC </b>" +
                                "<br/>This email including its attachments are confidential and intended solely for the use of the individual or entity to whom they are addressed. If you have received this email in error, please delete it from your system and notify the sender immediately. If you are not the intended recipient you are notified that disclosing, copying or distributing the content of this information is strictly prohibited. " +
                                //"<br/>This e-mail, and any attachments hereto, is intended for use only by the addressee(s) named herein, and may contain legally privileged and/or confidential information. If you are not an intended recipient of this e-mail, you are notified that any dissemination, distribution or copying of this e-mail, and any attachments hereto, is strictly prohibited. If you have received this e-mail in error, please notify the sender by reply e-mail, and permanently delete this e-mail, and any copies or printouts. " +
                                "<br/><br/>PLEASE CONSIDER THE ENVIRONMENT BEFORE PRINTING THIS EMAIL.";

            return MailFooter;
        }

        protected string EMailGetRequestDetail(long RequestID)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GWC_SP_GetRequestHeadByRequestIDs";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@RequestIDs", RequestID.ToString());
            da.SelectCommand = cmd;
            conn.Open();
            da.Fill(ds, "t");
            conn.Close();

            SqlDataAdapter adp6 = new SqlDataAdapter();
            DataSet ds6 = new DataSet();
            DataTable dt6 = new DataTable();
            adp6 = new SqlDataAdapter("select cancelDays from mterritory where id=" + ds.Tables[0].Rows[0]["SiteID"].ToString() + "", conn);
            adp6.Fill(ds6);
            int cnt6 = ds6.Tables[0].Rows.Count;
            dt6 = ds6.Tables[0];

            long CancelDays = Convert.ToInt64(ds6.Tables[0].Rows[0]["cancelDays"].ToString());
            DateTime OrDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["OrderDate"].ToString());
            string OrderCancelDate = OrDate.AddDays(CancelDays).ToShortDateString();

            string messageBody = "<font><b>Order Summary :  </b> </font><br/><br/>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:#555555; text-align: center;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px; text-align: center;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;

            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Order Id" + htmlTdEnd;
            messageBody += htmlTdStart + "Customer Order Reference No." + htmlTdEnd;
            messageBody += htmlTdStart + "Order Date" + htmlTdEnd;
            messageBody += htmlTdStart + "Exp. Delivery Date" + htmlTdEnd;
            messageBody += htmlTdStart + "Status" + htmlTdEnd;
            messageBody += htmlTdStart + "Department" + htmlTdEnd;
            messageBody += htmlTdStart + "Request Type" + htmlTdEnd;
            messageBody += htmlTdStart + "Requested By" + htmlTdEnd;
            messageBody += htmlTdStart + "Remark" + htmlTdEnd;
            messageBody += htmlTdStart + "Auto Cancellation Date" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;

            string OrderDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["OrderDate"].ToString()).ToShortDateString();
            string Deliverydate = Convert.ToDateTime(ds.Tables[0].Rows[0]["Deliverydate"].ToString()).ToShortDateString();

            messageBody += htmlTrStart;
            //messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["ID"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["OrderNo"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["OrderNumber"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + OrderDate + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + Deliverydate + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["RequestStatus"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["SiteName"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["Priority"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["RequestByUserName"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + ds.Tables[0].Rows[0]["Remark"].ToString() + " " + htmlTdEnd;
            messageBody += htmlTdStart + " " + OrderCancelDate + " " + htmlTdEnd;
            messageBody += htmlTrEnd;

            messageBody += htmlTableEnd;

            return messageBody;
        }

        protected string EMailGetRequestPartDetail(long RequestID)
        {
            DataTable dtdtP = new DataTable();
            SqlCommand cmdd2P = new SqlCommand();
            SqlDataAdapter dadaP = new SqlDataAdapter();
            cmdd2P.Connection = conn;
            cmdd2P.CommandText = String.Format("select OD.Sequence,OD.Prod_Code,OD.Prod_Description,OD.OrderQty ,PRD.GroupSet from torderdetail OD left outer join mProduct PRD on OD.SkuId =PRD.ID where OD.Orderheadid=" + RequestID + "");
            dadaP.SelectCommand = cmdd2P;
            dadaP.Fill(dtdtP);

            //DataSet dsOrdrDetail = new DataSet();
            //dsOrdrDetail = fillds("select OD.Sequence,OD.Prod_Code,OD.Prod_Description,OD.OrderQty ,PRD.GroupSet from torderdetail OD left outer join mProduct PRD on OD.SkuId =PRD.ID where OD.Orderheadid=" + RequestID + "", conn);

            string messageBody = "<font><b>Order Details :  </b> </font><br/><br/>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:#555555; text-align: center;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px; text-align: center;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;

            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "Sr. No." + htmlTdEnd;
            messageBody += htmlTdStart + "Item Code" + htmlTdEnd;
            messageBody += htmlTdStart + "Description" + htmlTdEnd;
            messageBody += htmlTdStart + "Qty" + htmlTdEnd;
            messageBody += htmlTdStart + "Group Set" + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;
            if (dtdtP.Rows.Count > 0)
            {
                for (int r = 0; r <= dtdtP.Rows.Count - 1; r++)
                {
                    messageBody += htmlTrStart;
                    messageBody += htmlTdStart + " " + dtdtP.Rows[r]["Sequence"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dtdtP.Rows[r]["Prod_Code"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dtdtP.Rows[r]["Prod_Description"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dtdtP.Rows[r]["OrderQty"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTdStart + " " + dtdtP.Rows[r]["GroupSet"].ToString() + " " + htmlTdEnd;
                    messageBody += htmlTrEnd;
                }
            }
            messageBody += htmlTableEnd;
            return messageBody;

        }

        public int MailCompose(string UserEmail, string UserName, string Msg, string MessageTitle, long RequestID)
        {
            string OrderNo = "";
            DataTable dtdt = new DataTable();
            SqlCommand cmdd2 = new SqlCommand();
            SqlDataAdapter dada = new SqlDataAdapter();
            cmdd2.Connection = conn;
            cmdd2.CommandText = String.Format("select Status,OrderNo from tOrderHead where Id=" + RequestID + "");
            dada.SelectCommand = cmdd2;
            dada.Fill(dtdt);
            string message = string.Empty;
            if (dtdt.Rows.Count > 0)
            {
                OrderNo = dtdt.Rows[0]["OrderNo"].ToString();
                long status = Convert.ToInt64(dtdt.Rows[0]["Status"].ToString());
                if (status == 7) { message = "This is an automatically generated message in reference to Order Ready For Dispatch <br/>"; }
                else if (status == 8) { message = "This is an automatically generated message in reference to Order Dispatch. <br/>"; }
                else if (status == 10) { message = "This is an automatically generated message in reference to a order cancel. <br/>"; }
            }


            MailProerties mp = new MailProerties();
            mp.MM_From = "admin@brilliantinfosys.com";
            mp.MM_FromMailPassword = "6march1986";
            mp.MM_SrNo = 1;
            mp.MM_Port = 80;
            mp.MM_IsBodyHtml = Convert.ToBoolean("True");
            mp.MM_smtpHost = "smtpout.asia.secureserver.net";
            mp.MM_smtpUseDefaultCredentials = false;
            mp.MM_EnableSsl = false;
            mp.MM_IsActive = true;
            mp.MM_SrNo = 1;
            mp.mailTo = UserEmail;
            //mp.Subject = MessageTitle + ", Order # "+ RequestID +" ";
            mp.Subject = MessageTitle + ", Order # " + OrderNo + " ";
            mp.mailBody = "Dear " + UserName + ", <br/><br/>";
            mp.mailBody = mp.mailBody + message;
            mp.mailBody = mp.mailBody + Msg;
            mp.mailBody = mp.mailBody + EMailGetRequestDetail(RequestID);
            mp.mailBody = mp.mailBody + "<br/><br/>" + EMailGetRequestPartDetail(RequestID);
            mp.mailBody = mp.mailBody + MailGetFooter();

            mp = sendmail(mp);

            return 1;
        }

        public MailProerties sendmail(MailProerties mp)
        {
            mp.isMailSend = false;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                mail.To.Add(mp.mailTo);
                mail.From = new MailAddress(mp.MM_From);
                mail.Body = mp.mailBody;
                mail.Subject = mp.Subject;
                mail.IsBodyHtml = true;
                smtp.Host = mp.MM_smtpHost;
                smtp.Port = mp.MM_Port;
                smtp.UseDefaultCredentials = mp.MM_smtpUseDefaultCredentials;
                smtp.Credentials = new System.Net.NetworkCredential(mp.MM_From, mp.MM_FromMailPassword);
                smtp.EnableSsl = mp.MM_EnableSsl;

                smtp.Send(mail);
                mp.isMailSend = true;
                mp.ErrorMsg = "Send Succcessfully";
            }
            catch (SmtpException ex)
            {
                mp.isMailSend = false;
                mp.ErrorMsg = ex.Message.ToString();
            }

            return mp;
        }

        public class MailProerties
        {
            private Int32 _int_MM_SrNo;
            private string _str_MM_From;
            private Int32 _int_MM_Port;
            private Boolean _bt_MM_IsBodyHtml;
            private string _str_MM_smtpHost;
            private Boolean _bt_MM_smtpUseDefaultCredentials;
            private string _str_MM_FromMailPassword;
            private Boolean _bt_MM_EnableSsl;
            private Boolean _bt_MM_IsActive;

            private String _str_mailTo;
            private String _str_mailBody;
            private Int32 _str_mailCount;
            private String _str_mailCC;
            private Boolean _bt_isMailSend;
            private String _str_ErrorMsg;
            private String _str_Subject;

            public Int32 MM_SrNo
            {
                get { return _int_MM_SrNo; }
                set { _int_MM_SrNo = value; }
            }
            public string MM_From
            {
                get { return _str_MM_From; }
                set { _str_MM_From = value; }
            }
            public string ErrorMsg
            {
                get { return _str_ErrorMsg; }
                set { _str_ErrorMsg = value; }
            }
            public Boolean isMailSend
            {
                get { return _bt_isMailSend; }
                set { _bt_isMailSend = value; }
            }
            public Int32 MM_Port
            {
                get { return _int_MM_Port; }
                set { _int_MM_Port = value; }
            }
            public Boolean MM_IsBodyHtml
            {
                get { return _bt_MM_IsBodyHtml; }
                set { _bt_MM_IsBodyHtml = value; }
            }
            public string MM_smtpHost
            {
                get { return _str_MM_smtpHost; }
                set { _str_MM_smtpHost = value; }
            }
            public Boolean MM_smtpUseDefaultCredentials
            {
                get { return _bt_MM_smtpUseDefaultCredentials; }
                set { _bt_MM_smtpUseDefaultCredentials = value; }
            }
            public string MM_FromMailPassword
            {
                get { return _str_MM_FromMailPassword; }
                set { _str_MM_FromMailPassword = value; }
            }
            public Boolean MM_EnableSsl
            {
                get { return _bt_MM_EnableSsl; }
                set { _bt_MM_EnableSsl = value; }
            }
            public Boolean MM_IsActive
            {
                get { return _bt_MM_IsActive; }
                set { _bt_MM_IsActive = value; }
            }
            public String mailTo
            {
                get { return _str_mailTo; }
                set { _str_mailTo = value; }
            }
            public String mailBody
            {
                get { return _str_mailBody; }
                set { _str_mailBody = value; }
            }
            public Int32 mailCount
            {
                get { return _str_mailCount; }
                set { _str_mailCount = value; }
            }
            public String mailCC
            {
                get { return _str_mailCC; }
                set { _str_mailCC = value; }
            }

            public string Subject
            {
                get { return _str_Subject; }
                set { _str_Subject = value; }
            }
        }
    }
}