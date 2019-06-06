using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.Login;
using BrilliantWMS.ContactPerson;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.PowerOnRent
{
    public partial class AddEditSearchContact : System.Web.UI.Page
    {
        long DeptID = 0;
        long VendorID = 0;
        long ClntID = 0;
        ResourceManager rm;
        CultureInfo ci;

        protected void Page_Load(object sender, EventArgs e)
        {
            string w = Request.QueryString["inv"].ToString();
            if (w == "Warehouse")
            {
                if (Session["VendorID"] != null)
                {
                    VendorID = long.Parse(Session["VendorID"].ToString());
                    BindVndrContactList(VendorID);
                }
                else if (Session["ClientID"] != null)
                {
                    ClntID = long.Parse(Session["ClientID"].ToString());
                    BindClientContactList(ClntID);
                }
            }
            else
            {
                DeptID = long.Parse(Session["DeptID"].ToString());
                BindContactList(DeptID);
            }
            
            clear();
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
        }

        protected void BindContactList(long DeptID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

            ConPerLst = UCCommonFilter.GetContactPersonListDeptWise(DeptID, profile.DBConnection._constr).ToList();

            gvContactPerson.DataSource = ConPerLst;
            gvContactPerson.DataBind();
        }

        protected void BindVndrContactList(long VendorID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

            ConPerLst = UCCommonFilter.GetContactPersonListVendorWise(VendorID, profile.DBConnection._constr).ToList();

            gvContactPerson.DataSource = ConPerLst;
            gvContactPerson.DataBind();
        }

        protected void BindClientContactList(long ClntID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            ConPerLst = UCCommonFilter.GetContactPersonListClientWise(ClntID, profile.DBConnection._constr).ToList();

            gvContactPerson.DataSource = ConPerLst;
            gvContactPerson.DataBind();

        }

        protected void gvContactPerson_OnRebind(object sender, EventArgs e)
        {
            BindContactList(DeptID);
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

        protected void clear()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtMobileNo.Text = "";
        }

        protected void GetContactDetailByContactID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            try
            {
                tContactPersonDetail conper = new tContactPersonDetail();
                conper = UCCommonFilter.GetContactPersonDetailsByID(long.Parse(hdnConID.Value), profile.DBConnection._constr);

                if (conper.Name != null) txtName.Text = conper.Name.ToString();
                if (conper.EmailID != null) txtEmail.Text = conper.EmailID.ToString();
                if (conper.MobileNo != null) txtMobileNo.Text = conper.MobileNo.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "AddEditSearchContact", "GetContactDetailByContactID");
            }
            finally
            {
                UCCommonFilter.Close();

            }
        }

        [WebMethod]
        public static string WMSaveRequestHead(object objCon,string State)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            tContactPersonDetail ConPerD=new tContactPersonDetail();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                ConPerD.Name = dictionary["Name"].ToString();
                ConPerD.MobileNo = dictionary["MobileNo"].ToString();
                ConPerD.EmailID = dictionary["EmailID"].ToString();

                if (State == "Edit")
                {
                    ConPerD.ID = Convert.ToInt64(HttpContext.Current.Session["ConID"].ToString());
                    ConPerD.LastModifiedBy = profile.Personal.UserID.ToString();
                    ConPerD.LastModifiedDate = DateTime.Now;
                    UCCommonFilter.EditContactPerson(ConPerD,profile.DBConnection._constr);                    
                }
                else
                {
                    long DID=Convert.ToInt64(HttpContext.Current.Session["DeptID"].ToString());
                    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DID, profile.DBConnection._constr);

                    ConPerD.ReferenceID = CompanyID;
                    ConPerD.Department = DID;
                    ConPerD.CompanyID = CompanyID;
                    ConPerD.Sequence = 1;
                    ConPerD.ObjectName = "Contact";
                    ConPerD.CustomerHeadID = 0;
                    ConPerD.ContactTypeID = 4;
                    ConPerD.Active = "N";
                    ConPerD.CreatedBy = profile.Personal.UserID.ToString();
                    ConPerD.CreationDate = DateTime.Now;
                    UCCommonFilter.AddIntotContactpersonDetail(ConPerD,profile.DBConnection._constr);
                }
                result = "Contact saved successfully";
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

            lblContactName.Text = rm.GetString("ContactName", ci);
            lblEmailID.Text = rm.GetString("EmailID", ci);
            lblMobileNo.Text = rm.GetString("MobileNo", ci);
            lblContactPersonList.Text = rm.GetString("ContactPersonList",ci);
            btnSubmit.Value = rm.GetString("Submit", ci);
            btnSave.Value = rm.GetString("Save", ci);
        }
    }

}