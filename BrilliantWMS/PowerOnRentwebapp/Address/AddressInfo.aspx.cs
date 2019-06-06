using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.AddressInfoService;
using System.Web.Services;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.Address
{
    public partial class AddressInfo : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long companyID = 0;
        static string sessionID;
        static string TargetObject;
        static string Sequence;
        static Page thispage;

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

            sessionID = Session.SessionID;
            thispage = this;
            if (Request.QueryString["TargetObject"] != null) TargetObject = Request.QueryString["TargetObject"].ToString() + "_Address";
            if (Request.QueryString["Sequence"] != null) Sequence = Request.QueryString["Sequence"].ToString();
            if (!IsPostBack)
            {
                FillDropDown(); GetRecordFromTempDataBySequenceID(Sequence);

                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds.ReadXml(MapPath("LabelAlice.xml"));
                if (ds.Tables.Count > 0)
                {
                    DataRow[] dr;
                    dr = ds.Tables[0].Select("CompanyID = " + profile.Personal.CompanyID + " and Value = '" + lblZone.Text + "'");
                    if (dr.Length > 0) lblZone.Text = dr[0]["Alice"].ToString();

                    dr = new DataRow[] { };
                    dr = ds.Tables[0].Select("CompanyID = " + profile.Personal.CompanyID + " and Value = '" + lblSubZone.Text + "'");
                    if (dr.Length > 0) lblSubZone.Text = dr[0]["Alice"].ToString();
                }

               // if (Sequence == "0") Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry1" + sessionID, "setCountry('Select Country','Select State','0','0');", true);
            }
        }

        [WebMethod]
        public static void PMSaveAddress(object AddressInfo)
        {
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                SP_GetAddressListToBindGrid_Result Address = new SP_GetAddressListToBindGrid_Result();

                if (Sequence != "" && Sequence != "0")
                {
                    Address = AddressClient.GetAddressTempDataBySequence(Convert.ToInt64(Sequence), sessionID, TargetObject, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    Address.Sequence = Convert.ToInt64(Sequence);
                }
                else { Address.Sequence = 0; }
                Dictionary<string, object> rec = new Dictionary<string, object>();
                rec = (Dictionary<string, object>)AddressInfo;

                Address.ObjectName = TargetObject;
                Address.ReferenceID = Convert.ToInt64(Sequence);
                Address.AddressType = "none";

                Address.AddressLine1 = rec["AddressLine1"].ToString();
                Address.AddressLine2 = rec["AddressLine2"].ToString();
                Address.AddressLine3 = rec["AddressLine3"].ToString();
                Address.RouteID = Convert.ToInt64(rec["RouteID"].ToString());
                Address.Landmark = rec["Landmark"].ToString();
                Address.County = rec["County"].ToString();
                Address.State = rec["State"].ToString();
                Address.Zone = rec["Zone"].ToString();//new add
                Address.SubZone = rec["SubZone"].ToString();//new add
                Address.City = rec["City"].ToString();
                Address.Zipcode = rec["Zipcode"].ToString();
                Address.PhoneNo = rec["PhoneNo"].ToString();
                Address.FaxNo = rec["FaxNo"].ToString();
                Address.EmailID = rec["EmailID"].ToString();
                Address.Active = "N";// Active Use for Is Archive
                Address.CompanyID = profile.Personal.CompanyID;
                Address.ShipIsChecked = "false";
                Address.BillIsChecked = "false";
                Address.IsDefault = "N";
                if (Sequence != "0" && Sequence != "")
                { AddressClient.SetValuesToTempData_onChange(sessionID, profile.Personal.UserID.ToString(), TargetObject, Convert.ToInt32(Sequence), Address, profile.DBConnection._constr); }
                else
                { AddressClient.InsertIntoTemp(Address, sessionID, profile.Personal.UserID.ToString(), TargetObject, profile.DBConnection._constr); }
                AddressClient.Close();

            }
            catch (System.Exception ex)
            {
                AddressClient.Close();
                Login.Profile.ErrorHandling(ex, thispage, "AddressInfo", "PMSaveAddress");
            }

        }

        [WebMethod]
        public static string PMCheckDuplicateAddress(string Address, string Country, string State, string City)
        {
            iAddressInfoClient addressinfoclient = new iAddressInfoClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string Result;
            Result = addressinfoclient.CheckDuplicateAddress(Address, Country, State, City, sessionID, TargetObject, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            addressinfoclient.Close();
            return Result;
        }

        protected void GetRecordFromTempDataBySequenceID(string SequenceNo)
        {
            SP_GetAddressListToBindGrid_Result Address = new SP_GetAddressListToBindGrid_Result();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            try
            {
                lblAddressFormHeader.Text = "Add New Address Info";
                if (SequenceNo != "0")
                {
                    if (sessionID == null) sessionID = Session.SessionID;
                    lblAddressFormHeader.Text = "Edit Address Info";
                    CustomProfile profile = CustomProfile.GetProfile();
                    Address = AddressClient.GetAddressTempDataBySequence(Convert.ToInt64(SequenceNo), sessionID, TargetObject, profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                    txtAddress1.Text = Address.AddressLine1;
                    if (Address.AddressLine2 != null) txtAddress2.Text = Address.AddressLine2;
                    if (Address.AddressLine3 != null) txtAddress3.Text = Address.AddressLine3;
                    if (ddlRoute.Items.Count >= 1) ddlRoute.SelectedIndex = 0;
                    if (Address.RouteID != null) ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindByValue(Address.RouteID.ToString()));
                    if (Address.Landmark != null) txtLandMark.Text = Address.Landmark;
                    Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + Address.County + "','" + Address.State + "','" + Address.Zone + "','" + Address.SubZone + "');", true);
                    txtcity.Text = Address.City;
                    if (Address.Zipcode != null) txtPostalCode.Text = Address.Zipcode;
                    if (Address.Zipcode != null) txtPostalCode.Text = Address.Zipcode;
                    if (Address.PhoneNo != null) txtPhone.Text = Address.PhoneNo;
                    if (Address.PhoneNo != null) txtPhone.Text = Address.PhoneNo;
                    if (Address.FaxNo != null) txtFax.Text = Address.FaxNo;
                    if (Address.FaxNo != null) txtFax.Text = Address.FaxNo;
                    if (Address.EmailID != null) txtEmailID.Text = Address.EmailID;
                    //rbtnActiveYes.Checked = true;
                    //rbtnActiveNo.Checked = false;
                    //if (Address.Active == "N") { rbtnActiveYes.Checked = false; rbtnActiveNo.Checked = true; }
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "AddressInfo.aspx.cs", "GetRecordFromTempDataBySequenceID");
            }
            finally { AddressClient.Close(); }
        }

        protected void FillDropDown()
        {
            ddlRoute.Items.Clear();
            iAddressInfoClient objAddress = new iAddressInfoClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlRoute.DataSource = objAddress.GetRouteList(profile.DBConnection._constr);
            ddlRoute.DataBind();

            ListItem lst = new ListItem() { Text = "-Select-", Value = "0" };
            ddlRoute.Items.Insert(0, lst);

        }

        [WebMethod]
        public static List<mZone> PMprintZone(string Country, string State)
        {
            List<mZone> ZoneList = new List<mZone>();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ZoneList = AddressClient.GetZoneList(Country, State, profile.DBConnection._constr).ToList();
                return ZoneList;
            }
            catch (Exception ex) { return ZoneList; }
            finally { AddressClient.Close(); }
        }

        [WebMethod]
        public static List<mSubZone> PMprintSubZone(long ZoneID)
        {
            List<mSubZone> SubZoneList = new List<mSubZone>();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                SubZoneList = AddressClient.GetSubZoneList(ZoneID, profile.DBConnection._constr).ToList();
                return SubZoneList;
            }
            catch (Exception ex) { return SubZoneList; }
            finally { AddressClient.Close(); }
        }

        private void loadstring()
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            //rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            //ci = Thread.CurrentThread.CurrentCulture;
            //lblAddressFormHeader.Text = rm.GetString("newaddress", ci);
            //btnAddressSubmit.Value = rm.GetString("Submit", ci);
            //btnAddressClear.Value = rm.GetString("Clear", ci);
            //lbladdress1.Text = rm.GetString("AddressLine1", ci);
            //lbladdress2.Text = rm.GetString("AddressLine2", ci);
            //lbladdress3.Text = rm.GetString("AddressLine3", ci);
            //lblcharremain.Text = rm.GetString("charremain", ci);
            //lblcharremain2.Text = rm.GetString("charremain", ci);
            //lblcharremain3.Text = rm.GetString("charremain", ci);
            //lblemail.Text = rm.GetString("EmailID", ci);
            //lbllandmark.Text = rm.GetString("Landmark", ci);
            //lblpostal.Text = rm.GetString("ZIPCode", ci);
            //lblcountry.Text = rm.GetString("Country", ci);
            //lblstate.Text = rm.GetString("State", ci);
            //lblcity.Text = rm.GetString("City", ci);
            //lbldept.Text = rm.GetString("Department", ci);
            //lblZone.Text = rm.GetString("conatact1", ci);
            //lblSubZone.Text = rm.GetString("conatact2", ci);
            //lblphone1.Text = rm.GetString("phone1", ci);
            //lblphone2.Text = rm.GetString("phone2", ci);
            //Button1.Value = rm.GetString("Submit", ci);
            //Button2.Value = rm.GetString("Clear", ci);
        }

       

    }
}