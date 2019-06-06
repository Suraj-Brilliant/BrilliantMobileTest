using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.DocumentService;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.Document
{

    public partial class UC_AttachDocument : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        [WebMethod]
        public static void ClearDocument(string TargetObjectName, string SessionID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.DocumentService.iUC_AttachDocumentClient DocumentServiceClient = new iUC_AttachDocumentClient();
            DocumentServiceClient.ClearTempData(SessionID, profile.Personal.UserID.ToString(), TargetObjectName + "Document", profile.DBConnection._constr);
            DocumentServiceClient.Close();
        }

        public void ClearDocument(string TargetObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            hndDocumentTargetObjectName.Value = TargetObjectName;
            BrilliantWMS.DocumentService.iUC_AttachDocumentClient DocumentServiceClient = new iUC_AttachDocumentClient();
            DocumentServiceClient.ClearTempData(Session.SessionID, profile.Personal.UserID.ToString(), TargetObjectName + "Document", profile.DBConnection._constr);
            DocumentServiceClient.Close();
            GvDocument.DataSource = null;
            GvDocument.DataBind();
        }

        public void FinalSaveDocument(long ReferenceID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();
            DocumentSourceClient.FinalSaveToDBtDocument(Session.SessionID, ReferenceID, profile.Personal.UserID.ToString(), hndDocumentTargetObjectName.Value + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
        }

        [WebMethod]
        public static void FinalSaveDocument1(long ReferenceID, string SessionID, string TargetObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUC_AttachDocumentClient DocumentSourceClient = new iUC_AttachDocumentClient();
            DocumentSourceClient.FinalSaveToDBtDocument(SessionID, ReferenceID, profile.Personal.UserID.ToString(), TargetObjectName + "Document", HttpRuntime.AppDomainAppPath.ToString(), profile.DBConnection._constr);
        }

        public void FillDocumentByObjectNameReferenceID(long ReferenceID, string SourceObjectName, string TargetObjectName)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                hndDocumentTargetObjectName.Value = TargetObjectName;
                BrilliantWMS.DocumentService.iUC_AttachDocumentClient DocumentServiceClient = new iUC_AttachDocumentClient();
                GvDocument.DataSource = DocumentServiceClient.GetDocumentByReferenceId(SourceObjectName + "Document", TargetObjectName + "Document", ReferenceID, profile.Personal.UserID.ToString(), Session.SessionID.ToString(), profile.DBConnection._constr);
                GvDocument.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "UC_AttachDocuments.ascx.cs", "FillDocumentByObjectNameReferenceID");
            }
        }

        protected void GvDocument_OnRebind(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.DocumentService.iUC_AttachDocumentClient DocumentServiceClient = new iUC_AttachDocumentClient();
            if (Session["PORRequestID"].ToString() != "0" && Session["PORRequestID"].ToString() != "Company")
            {
                long RequestID = Convert.ToInt64(Session["PORRequestID"].ToString());
                FillDocumentByObjectNameReferenceID(RequestID, "RequestPartDetail", "RequestPartDetail");
            }
            else
            {
                GvDocument.DataSource = DocumentServiceClient.GetExistingTempDataBySessionIDObjectNameToRebind(Session.SessionID, profile.Personal.UserID.ToString(), hndDocumentTargetObjectName.Value + "Document", profile.DBConnection._constr);
                GvDocument.DataBind();
            }
            DocumentServiceClient.Close();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == null)
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
        }


        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lbldoclisst.Text = rm.GetString("DocumentList", ci);
            btnDocumentAdd.Value = rm.GetString("AddNew", ci);
            

        }
    }
}