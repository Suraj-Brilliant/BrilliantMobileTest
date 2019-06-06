using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
//using PowerOnRentwebapp.AddressInfoService;
using BrilliantWMS.ServiceContactPersonInfo;
using System.Web.Services;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.ContactPerson;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.PowerOnRent
{
    public partial class GetLocation : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long companyID = 0;
        static string sessionID;
        static string TargetObject;
        static string Sequence;
        static Page thispage;
        long DeptID = 0;
        long UserID = 0;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                UserID = profile.Personal.UserID;

                sessionID = Session.SessionID;
                thispage = this;

                DeptID = long.Parse(Session["DeptID"].ToString());

                BindAddressList(DeptID);
                clear();

                if (Session["Lang"] == "")
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();

                // Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry1" + sessionID, "setCountry('Select Country','Select State','0','0');", true);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "AddEditSearchAddress", "Page_Load");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('No Locations Available','Error','#')", true);
            }
            finally { }
        }

        protected void BindAddressList(long DeptID)
        {
            List<VW_GetUserLocation> LocLst = new List<VW_GetUserLocation>();
            List<tAddress> AdrsLst = new List<tAddress>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            //string AdrsType = UCCommonFilter.GetAdrsType(DeptID, profile.DBConnection._constr);
            //if (AdrsType == "Location")
            //{
            //    tblAddAdrs.Attributes.Add("style", "display:none");
            //    gvContactPerson.Columns[0].Visible = false;
            LocLst = UCCommonFilter.GetLocationOfUser(profile.Personal.UserID, profile.DBConnection._constr).ToList();
            //AdrsLst = UCCommonFilter.GetUserLocation(profile.Personal.UserID, profile.DBConnection._constr).ToList();
            //}
            //else
            //{
            tblAddAdrs.Attributes.Add("style", "display:''");
            long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr);
            // AdrsLst = UCCommonFilter.GetDeptAddressList(CompanyID, profile.DBConnection._constr).ToList();
            // AdrsLst = UCCommonFilter.GetDeptAddressListAdrsType(CompanyID, DeptID, profile.DBConnection._constr).ToList();
            //}
            gvContactPerson.DataSource = LocLst;
            gvContactPerson.DataBind();
        }
        protected void clear()
        {
            txtAddress.Text = "";
            txtLocationCode.Text = "";
            txtLocationName.Text = "";
            //txtcity.Text = "";
            //txtMobileNo.Text = "";
        }
        protected void gvContactPerson_OnRebind(object sender, EventArgs e)
        {
            BindAddressList(DeptID);
        }
        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnConID.Value = hdnSelectedRec.Value.ToString();
            Session["DeptID"] = DeptID.ToString();
            Session["ConID"] = hdnConID.Value.ToString();
            GetContactDetailByContactID();
            hdnstate.Value = "Edit";
        }
        protected void GetContactDetailByContactID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            try
            {
                tAddress conper = new tAddress();
                conper = UCCommonFilter.GetAddressDetailsByID(long.Parse(hdnConID.Value), profile.DBConnection._constr);
                VW_GetUserLocation LocDetail = UCCommonFilter.GetLocationDetailsByID(long.Parse(hdnConID.Value), profile.DBConnection._constr);
                btnSave.Visible = true;
                if (sessionID == null) sessionID = Session.SessionID;
                if (LocDetail.AddressLine1 != null) txtAddress.Text = LocDetail.AddressLine1.ToString();
                if (LocDetail.LocationCode != null) txtLocationCode.Text = LocDetail.LocationCode.ToString(); txtLocationCode.Enabled = false;
                if (LocDetail.LocationName != null) txtLocationName.Text = LocDetail.LocationName.ToString(); txtLocationName.Enabled = false;
                if (LocDetail.Name != null) txtContactName.Text = LocDetail.Name.ToString();
                if (LocDetail.MobileNo != null) txtContactNo.Text = LocDetail.MobileNo.ToString();
                if (LocDetail.EmailID != null) txtEmailID.Text = LocDetail.EmailID.ToString();


                // Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry1" + sessionID, "setCountry('India','Maharashtra','0','0');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry1" + sessionID, "setCountry('" + conper.County + "','" + conper.State + "','0','0');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + conper.County + "','" + conper.State + "','" + conper.Zone + "','" + conper.SubZone + "');", true);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "AddEditSearchAddress", "GetContactDetailByContactID");
            }
            finally
            {
                UCCommonFilter.Close();

            }
        }
        [WebMethod]
        public static string WMSaveRequestHead(object objAdr, string State)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            tAddress Adrs = new tAddress();
            tContactPersonDetail Con = new tContactPersonDetail();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objAdr;

                Adrs.AddressLine1 = dictionary["AddressLine1"].ToString();
                Adrs.ID = Convert.ToInt64(HttpContext.Current.Session["ConID"].ToString());
                Adrs.LastModifiedBy = profile.Personal.UserID.ToString();
                Adrs.LastModifiedDate = DateTime.Now;
                Adrs.City = "";
                UCCommonFilter.EditAddress(Adrs, profile.DBConnection._constr);

                Con.Name = dictionary["Name"].ToString();
                Con.EmailID = dictionary["EmilID"].ToString();
                Con.MobileNo = dictionary["MobileNo"].ToString();
                long AdrsID = Convert.ToInt64(HttpContext.Current.Session["ConID"].ToString());
                UCCommonFilter.EditLocation(Con, AdrsID, profile.DBConnection._constr);

                result = "Location saved successfully";
            }
            catch { result = "Some error occurred"; }
            finally { UCCommonFilter.Close(); }

            return result;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            // lblLocationAddress.Text = rm.GetString("AddressLine", ci);
            //  lblLocationList.Text = rm.GetString("AddressList", ci);
            btnSave.Value = rm.GetString("Save", ci);
            btnSubmit.Value = rm.GetString("Submit", ci);
        }
    }
}