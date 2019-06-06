using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServiceEngineMaster;
using System.Web.Services;
using WebMsgBox;


namespace PowerOnRentwebapp.Location
{
    public partial class UCLocation : System.Web.UI.UserControl
    {
        public string LocationIDs;
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
            LocationIDs = hnddefaultchk.Value;
        }

        public void FillLocationByObjectNameReferenceID(string SourceObjectName, long ReferenceID, string TargetObjectName)
        {
            //try
            //{
            //    if (hdnConPersonTargetObject != null) hdnConPersonTargetObject.Value = TargetObjectName;
            //    CustomProfile profile = CustomProfile.GetProfile();

            //    PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //    List<NG_SP_GetWarehouseLocationDetails_Result> LocationList = new List<NG_SP_GetWarehouseLocationDetails_Result>();
            //    sessionID = Session.SessionID;

            //    LocationList = ServiceLocation.GetLocationByObjectNameReferenceID(SourceObjectName, ReferenceID, TargetObjectName + "_Location", sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
            //    if (GVLocation != null)
            //    {
            //        GVLocation.DataSource = LocationList;
            //        GVLocation.DataBind();
            //    }
            //    ServiceLocation.Close();

            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, ParentPage, "UCLocation", "FillLocationByObjectNameReferenceID");
            //}
            //finally { }
        }

        protected void GVLocation_OnRebind(object sender, EventArgs e)
        {
           
            MsgBox.Show("Location Saved Successfully");
            FillLocationByObjectNameReferenceID("Warehouse", 20, "Warehouse");
        }

        protected void BindLocationFromTempData()
        {
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //    if (hnddefaultchk.Value == "") hnddefaultchk.Value = "1";

            //    List<mEngine> LstLoc = new List<mEngine>();
            //    LstLoc = ServiceLocation.getAllLocation(profile.DBConnection._constr).ToList();
            //    GVLocation.DataSource = LstLoc;
            //    GVLocation.DataBind();
            //    ServiceLocation.Close();
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, ParentPage, "UCLocation", "BindLocationFromTempData");
            //}
        }

        [WebMethod]
        public static void ClearLocation(string ObjectName, string SessionID)
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //ServiceLocation.ClearTempDataFromDB(SessionID, profile.Personal.UserID.ToString(), ObjectName + "_Location", profile.DBConnection._constr);
            //ServiceLocation.Close();
        }

        public void ClearLocation(string ObjectName)
        {
            //hdnConPersonTargetObject.Value = ObjectName;
            //CustomProfile profile = CustomProfile.GetProfile();
            //PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //ServiceLocation.ClearTempDataFromDB(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName + "_Location", profile.DBConnection._constr);
            //ServiceLocation.Close();
        }

        public void FinalSaveLocation(string ObjectName, long ReferenceID)
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //ServiceLocation.FinalSaveToDBAddToLocation(Session.SessionID.ToString(), hdnConPersonTargetObject.Value.ToString() + "_Location", ReferenceID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            //ServiceLocation.Close();
        }

        [WebMethod]
        public static void FinalSaveLocation1(string ObjectName, long ReferenceID,string SessionID,string TargetObjectName)
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //ServiceLocation.FinalSaveToDBAddToLocation(SessionID, TargetObjectName + "_Location", ReferenceID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            //ServiceLocation.Close();
        }

        public void MoveToArchiveContactPerson(string Ids, string IsArchive, string CurrentObjectName)
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //PORServiceEngineMaster.iEngineMasterClient ServiceLocation = new PORServiceEngineMaster.iEngineMasterClient();
            //ServiceLocation.SetLocationArchive(Ids, IsArchive, Convert.ToInt64(profile.Personal.UserID).ToString(), CurrentObjectName + "_Location", sessionID, profile.DBConnection._constr);
            //ServiceLocation.Close();
        }

        public enum ReferenceObjectName
        {
            Account,
            Vendor
        }        

    }
}