using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.ServiceContactPersonInfo;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.ContactPerson
{
    public partial class UCContactPerson : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        public string ContactPersonIDs;
        static string sessionID;
        public Page ParentPage { get; set; }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            if (hnddefaultchk.Value == "") hnddefaultchk.Value = "1";
            ContactPersonIDs = hnddefaultchk.Value;

            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            
            loadstring();
            if (!IsPostBack)
            {
                //btnsave.Visible = true;
            }
        }

        public void FillContactPersonByObjectNameReferenceID(string SourceObjectName, long ReferenceID, string TargetObjectName)
        {
            try
            {
                if (hdnConPersonTargetObject != null) hdnConPersonTargetObject.Value = TargetObjectName;
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
                List<SP_GetContactPersonListToBindGrid_Result> ContactPersonList = new List<SP_GetContactPersonListToBindGrid_Result>();

                if (sessionID == null) sessionID = Session.SessionID;

                ContactPersonList = ServiceContactPerson.GetContactPersonByObjectNameReferenceID(SourceObjectName, ReferenceID, TargetObjectName + "_ContactPerson", sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                if (GVContactPerson != null)
                {
                    GVContactPerson.DataSource = ContactPersonList;
                    GVContactPerson.DataBind();
                }
                ServiceContactPerson.Close();
               // btnsave.Visible = false;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC ContactPerson", "FillContactPersonByObjectNameReferenceID");
            }

        }

        public void FillContactPersonByObjectNameReferenceIdLead(string SourceObjectName, long ReferenceID, string TargetObjectName)
        {
            try
            {
                if (hdnConPersonTargetObject != null) hdnConPersonTargetObject.Value = TargetObjectName;
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
                List<SP_GetContactPersonListToBindGridLead_Result> ContactPersonList = new List<SP_GetContactPersonListToBindGridLead_Result>();

                if (sessionID == null) sessionID = Session.SessionID;

                ContactPersonList = ServiceContactPerson.GetContactPersonByObjectNameReferenceIDLead(SourceObjectName, ReferenceID, TargetObjectName + "_ContactPerson", sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                if (GVContactPerson != null)
                {
                    GVContactPerson.DataSource = ContactPersonList;
                    GVContactPerson.DataBind();
                }
                ServiceContactPerson.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC ContactPerson", "FillContactPersonByObjectNameReferenceID");
            }
        }

        protected void BindContactPersonFromTempData()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
                if (hnddefaultchk.Value == "") hnddefaultchk.Value = "1";
                GVContactPerson.DataSource = ServiceContactPerson.GetContactPersonTempData(hdnConPersonTargetObject.Value + "_ContactPerson", Convert.ToInt64(hnddefaultchk.Value), Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
               // GVContactPerson.DataSource = ServiceContactPerson.GetContactPersonTempData("_ContactPerson", Convert.ToInt64(hnddefaultchk.Value), Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                GVContactPerson.DataBind();
                ServiceContactPerson.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC ContactPerson", "BindContactPersonFromTempData");
            }
        }

        protected void GVContactPerson_OnRebind(object sender, EventArgs e)
        {
            BindContactPersonFromTempData();
        }

        [WebMethod]
        public static void ClearContactPerson(string ObjectName, string SessionID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
            ServiceContactPerson.ClearTempDataFromDB(SessionID, profile.Personal.UserID.ToString(), ObjectName + "_ContactPerson", profile.DBConnection._constr);
            ServiceContactPerson.Close();
        }

        public void ClearContactPerson(string ObjectName)
        {
            hdnConPersonTargetObject.Value = ObjectName;
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
            ServiceContactPerson.ClearTempDataFromDB(Session.SessionID, profile.Personal.UserID.ToString(), hdnConPersonTargetObject.Value + "_ContactPerson", profile.DBConnection._constr);
            ServiceContactPerson.Close();
        }

        public void FinalSaveContactPerson(string ObjectName, long ReferenceID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
            ServiceContactPerson.FinalSaveToDBtAddToContactPerson(Session.SessionID.ToString(), hdnConPersonTargetObject.Value.ToString() + "_ContactPerson", ReferenceID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
           // ServiceContactPerson.FinalSaveToDBtAddToContactPerson(Session.SessionID.ToString(), "_ContactPerson", ReferenceID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            ServiceContactPerson.Close();
        }

        [WebMethod]
        public static void FinalSaveContactPerson1(string ObjectName, long ReferenceID, string SessionID, string TargetObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient ServiceContactPerson = new BrilliantWMS.ServiceContactPersonInfo.iContactPersonInfoClient();
            ServiceContactPerson.FinalSaveToDBtAddToContactPerson(SessionID, TargetObjectName + "_ContactPerson", ReferenceID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            ServiceContactPerson.Close();
        }

        public void MoveToArchiveContactPerson(string Ids, string IsArchive, string CurrentObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iContactPersonInfoClient ContactPersonClient = new iContactPersonInfoClient();
            ContactPersonClient.SetContactPersonArchive(Ids, IsArchive, Convert.ToInt64(profile.Personal.UserID).ToString(), CurrentObjectName + "_ContactPerson", sessionID, profile.DBConnection._constr);
            ContactPersonClient.Close();
        }

        public enum ReferenceObjectName
        {
            Account,
            Vendor
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            lblcontlist.Text = rm.GetString("ContactList", ci);
           // btnContactPerson.Value = rm.GetString("AddNew", ci);
           // btnsave.Text = rm.GetString("Next", ci);
        }

        //protected void btnsave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        FinalSaveContactPerson("Contact", long.Parse(Session["CompanyID"].ToString()));
        //        ClearContactPerson("_ContactPerson");
        //        FillContactPersonByObjectNameReferenceID("Contact", long.Parse(Session["CompanyID"].ToString()), "");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Login.Profile.ErrorHandling(ex, ParentPage, "UC ContactPerson", "btnsave_Click");
        //    }
            
        //}
    }
}