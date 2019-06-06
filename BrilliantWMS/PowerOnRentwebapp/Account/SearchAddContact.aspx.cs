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
using System.Data;

namespace BrilliantWMS.Account
{
    public partial class SearchAddContact : System.Web.UI.Page
    {
        long DeptID = 0;
        long CompanyID = 0;
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
           // DeptID = long.Parse(Session["DeptID"].ToString());
            if (Request.QueryString.ToString().Length >= 0)
            {
                CompanyID = Convert.ToInt64(Request.QueryString["CompanyID"].ToString());
                hdncompanyid.Value = Request.QueryString["CompanyID"].ToString();
            }
            BindContactList(CompanyID);
            clear();
            
        }

        protected void BindContactList(long CompanyID)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();

            ds = CompanyClient.GetContactPersonLocList(CompanyID,profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                gvContactPerson.DataSource = ds.Tables[0];
                gvContactPerson.DataBind();
            }
            else
            {
                gvContactPerson.DataSource = null;
                gvContactPerson.DataBind();

            }

            //CustomProfile profile = CustomProfile.GetProfile();
            //List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            //iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

            //ConPerLst = UCCommonFilter.GetContactPersonListDeptWise(CompanyID, profile.DBConnection._constr).ToList();

            //gvContactPerson.DataSource = ConPerLst;
            //gvContactPerson.DataBind();
        }

        protected void gvContactPerson_OnRebind(object sender, EventArgs e)
        {
            BindContactList(CompanyID);
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
        public static string WMSaveRequestHead(object objCon, string State)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            tContactPersonDetail ConPerD = new tContactPersonDetail();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                ConPerD.Name = dictionary["Name"].ToString();
                ConPerD.MobileNo = dictionary["MobileNo"].ToString();
                ConPerD.EmailID = dictionary["EmailID"].ToString();
                long CompanyID = long.Parse(dictionary["CompanyId"].ToString());

                if (State == "Edit")
                {
                    ConPerD.ID = Convert.ToInt64(HttpContext.Current.Session["ConID"].ToString());
                    ConPerD.LastModifiedBy = profile.Personal.UserID.ToString();
                    ConPerD.LastModifiedDate = DateTime.Now;
                    UCCommonFilter.EditContactPerson(ConPerD, profile.DBConnection._constr);
                }
                else
                {
                    //long DID = Convert.ToInt64(HttpContext.Current.Session["DeptID"].ToString());
                   // long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DID, profile.DBConnection._constr);
                    long DID = 0;
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
                    UCCommonFilter.AddIntotContactpersonDetail(ConPerD, profile.DBConnection._constr);
                }
                result = "Contact saved successfully";
            }
            catch { result = "Some error occurred"; }
            finally { UCCommonFilter.Close(); }

            return result;
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblContactName.Text = rm.GetString("ContactName", ci);
                lblEmailID.Text = rm.GetString("EmailID", ci);
                lblMobileNo.Text = rm.GetString("MobileNo", ci);
                lblContactPersonList.Text = rm.GetString("ContactPersonList", ci);
                btnSubmit.Value = rm.GetString("Submit", ci);
                btnSave.Value = rm.GetString("Save", ci);
            }
            catch { }
        }
    }
}