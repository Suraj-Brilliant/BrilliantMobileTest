using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BrilliantWMS.Login;
using BrilliantWMS.DocumentService;
using System.IO;
using System.Web.UI;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;



namespace BrilliantWMS.Document
{

    public partial class Document : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string sessionID, TargetObjectName;
        static long Sequence;
        static FileUpload DocFileUpload;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

             if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            DocFileUpload = FileUploadDocument;
            sessionID = Session.SessionID;
            if (Request.QueryString["Sequence"] != null) { Sequence = Convert.ToInt32(Request.QueryString["Sequence"]); }
            if (Request.QueryString["TargetObjectName"] != null) { TargetObjectName = Request.QueryString["TargetObjectName"].ToString() + "Document"; }
        }

        [WebMethod]
        public static string CheckDocumentTitle(string DocumentTitle)
        {
            BrilliantWMS.DocumentService.iUC_AttachDocumentClient DocumentClient = new BrilliantWMS.DocumentService.iUC_AttachDocumentClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string Result;
            Result = DocumentClient.CheckDuplicateDocumentTitle(sessionID, DocumentTitle, profile.Personal.UserID.ToString(), TargetObjectName, profile.DBConnection._constr);
            DocumentClient.Close();
            return Result;
        }


        protected void upload_LinkBtn_Click(object sender, EventArgs e)
        {
            string DocumentSaveAsPath = "";
            string DocumentDownLoadPath = "";
            string HttpAppPath = HttpRuntime.AppDomainAppPath;
            iUC_AttachDocumentClient documentClient = new iUC_AttachDocumentClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (FileUploadDocument.PostedFile != null)
                {
                    if (profile.Personal.CompanyID.ToString() != "")
                    {
                        if (!(Directory.Exists(HttpAppPath + "Document\\TempAttach_Document\\" + profile.Personal.CompanyID.ToString())))
                        {
                            Directory.CreateDirectory(HttpAppPath + "Document\\TempAttach_Document\\" + profile.Personal.CompanyID.ToString());
                        }
                    }
                    //string FileType = FileUploadDocument.PostedFile.ContentType.Split('/').LastOrDefault();
                    string[] strArr = FileUploadDocument.PostedFile.FileName.Split('.');
                    string FileType = strArr[strArr.Length - 1];
                    string FileName = Session.SessionID.ToString() + "_" + DateTime.Now.Ticks.ToString() + "." + FileType;
                    DocumentDownLoadPath = "../Document/TempAttach_Document/" + profile.Personal.CompanyID.ToString() + "/" + FileName;
                    DocumentSaveAsPath = HttpAppPath + "Document\\TempAttach_Document\\" + profile.Personal.CompanyID.ToString() + "\\" + FileName;
                    FileUploadDocument.SaveAs(DocumentSaveAsPath);

                    /*Insert into TempData*/
                    SP_GetDocumentList_Result newDocument = new SP_GetDocumentList_Result();

                    newDocument.ObjectName = TargetObjectName; ;
                    newDocument.ReferenceID = Convert.ToInt64(Sequence);
                    newDocument.DocumentName = null;
                    if (txtDocTitle.Text.ToString().Trim() != "") newDocument.DocumentName = txtDocTitle.Text.ToString().Trim();
                    newDocument.Sequence = Convert.ToInt32(Sequence);
                    newDocument.Description = null;
                    if (txtDocDesc.Text.ToString().Trim() != "") newDocument.Description = txtDocDesc.Text.ToString().Trim();

                    newDocument.DocumentDownloadPath = DocumentDownLoadPath;
                    newDocument.DocumentSavePath = DocumentSaveAsPath;
                    newDocument.FileType = FileType;

                    newDocument.Keywords = null;
                    if (txtKeyword.Text.ToString().Trim() != "") newDocument.Keywords = txtKeyword.Text.ToString().Trim();

                    if (rbtnPrivate.Checked == true)
                    {
                        newDocument.ViewAccess_Value = "";
                        newDocument.DeleteAccess_Value = hdnDeleteAccessIDs.Value;
                        newDocument.DowloadAccess_Value = hdDownLoadAccessIDs.Value;
                    }
                    else if (rbtnPublic.Checked == true)
                    {
                        newDocument.ViewAccess_Value = "Public";
                        newDocument.DeleteAccess_Value = "Public";
                        newDocument.DowloadAccess_Value = "Public";
                    }
                    else if (rbtnSelf.Checked == true)
                    {
                        newDocument.ViewAccess_Value = newDocument.DeleteAccess_Value = newDocument.DowloadAccess_Value = profile.Personal.UserID.ToString();
                    }

                    newDocument.Active = "Y";
                    newDocument.CreatedBy = profile.Personal.UserID.ToString();
                    newDocument.CreationDate = DateTime.Now;
                    newDocument.CustomerHeadID = 0;
                    newDocument.CompanyID = profile.Personal.CompanyID;
                    newDocument.ViewAccess = "true";
                    newDocument.DeleteAccess = "true";
                    newDocument.DowloadAccess = "true";

                    newDocument.DocumentType = ddlDocumentType.SelectedValue.ToString();

                    documentClient.InsertIntoTemp(newDocument, Session.SessionID.ToString(), profile.Personal.UserID.ToString(), TargetObjectName, profile.DBConnection._constr);
                    if (Session["PORRequestID"].ToString() != "0")
                    {
                        long RequestID = Convert.ToInt64(Session["PORRequestID"].ToString());
                        documentClient.FinalSaveToDBtDocument(Session.SessionID.ToString(), RequestID, profile.Personal.UserID.ToString(),"RequestPartDetailDocument", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
                    }
                    ClientScript.RegisterStartupScript(GetType(), "hwa", "onSuccessTempSaveDocument('true');", true);

                }
            }
            catch (System.Exception ex)
            {
                if (DocumentSaveAsPath != "") if (File.Exists(DocumentSaveAsPath)) File.Delete(DocumentSaveAsPath);
                Login.Profile.ErrorHandling(ex, this, "UC Document", "upload_LinkBtn_Click");
            }
            finally { documentClient.Close(); }
        }


        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblDocumentFormHeader.Text = rm.GetString("AddNewDocument", ci);
                btnUploadDocuemnt.Text = rm.GetString("UploadFiles", ci);
                btnDocumentClear.Value = rm.GetString("Clear", ci);
                lblDocumentTitle.Text = rm.GetString("DocumentTitle", ci);
                lblDocumentType.Text = rm.GetString("DocumentType", ci);
                lblDescription.Text = rm.GetString("Description", ci);
                lblKeyWords.Text = rm.GetString("KeyWords", ci);
                btnUploadDocuemnt2.Text = rm.GetString("UploadFiles", ci);
                btnDocumentClear2.Value = rm.GetString("Clear", ci);
                lblSelectDocument.Text = rm.GetString("SelectDocument", ci);
               
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Document", "loadstring");
            }
        }
    }
}
