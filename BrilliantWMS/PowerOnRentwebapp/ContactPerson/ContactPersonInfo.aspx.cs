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
using System.Data;

namespace BrilliantWMS.ContactPerson
{
    public partial class ContactPersonInfo : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long companyID = 0;
        static string sessionID;
        static string TargetObject;
        static string Sequence;
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
           // loadstring();
            sessionID = Session.SessionID;

            //companyID = long.Parse(Session["CompanyID"].ToString());
            //hdncompanyid.Value = companyID.ToString();

            if (Request.QueryString["TargetObject"] != null) TargetObject = Request.QueryString["TargetObject"].ToString(); 
            if (Request.QueryString["Sequence"] != null) Sequence = Request.QueryString["Sequence"].ToString();
            if (!IsPostBack)
            {
              //  GetDepartment();
                FillDropDown();
                GetRecordFromTempDataBySequenceID(Sequence);
            }
        }

        protected void GetRecordFromTempDataBySequenceID(string SequenceNo)
        {
            SP_GetContactPersonListToBindGrid_Result ContactPerson = new SP_GetContactPersonListToBindGrid_Result();
            iContactPersonInfoClient ContactPersonClient = new iContactPersonInfoClient();
            try
            {
                //lblContactPersonFormHeader.Text = "Add New Conatct Person Info";
                if (SequenceNo != "0")
                {
                    if (sessionID == null) sessionID = Session.SessionID;
                    lblContactPersonFormHeader.Text = "Edit Conatct Person Info";
                    CustomProfile profile = CustomProfile.GetProfile();
                    ContactPerson = ContactPersonClient.GetContactPersonTempDataBySequence(Convert.ToInt64(SequenceNo), sessionID, TargetObject + "_ContactPerson", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    //ContactPerson = ContactPersonClient.GetContactPersonTempDataBySequence(Convert.ToInt64(SequenceNo), sessionID, "_ContactPerson", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    txtName.Text = ContactPerson.Name;
                   //if (ContactPerson.Department != null) { txtDeparment.Text = ContactPerson.Department; }
                    if (ContactPerson.EmailID != null) { txtEmailID.Text = ContactPerson.EmailID; }
                    if (ContactPerson.OfficeNo != null) { txtOfficeNo.Text = ContactPerson.OfficeNo; }
                    if (ContactPerson.MobileNo != null) { txtMobileNo.Text = ContactPerson.MobileNo; }
                    //ddldepartment.SelectedIndex = ddldepartment.Items.IndexOf(ddldepartment.Items.FindByValue(ContactPerson.Department.ToString()));

                    ddlcontacttype.SelectedIndex = ddlcontacttype.Items.IndexOf(ddlcontacttype.Items.FindByValue(ContactPerson.ContactTypeID.ToString()));
                    //if (ContactPerson.InterestedIn != null) { txtIntrestedIn.Text = ContactPerson.InterestedIn; }
                    //if (ContactPerson.Hobbies != null) { txtHobbies.Text = ContactPerson.Hobbies; }
                    //if (ContactPerson.FacebookID != null) { txtFacebookID.Text = ContactPerson.FacebookID; }
                    //if (ContactPerson.TwitterID != null) { txtTwitterID.Text = ContactPerson.TwitterID; }
                    //if (ContactPerson.OtherID != null) { txtOtherID.Text = ContactPerson.OtherID; }
                    //if (ContactPerson.HighestQualification != null) { txtHighestQualification.Text = ContactPerson.HighestQualification; }
                    //if (ContactPerson.CollegeOrUniversity != null) { txtCollege.Text = ContactPerson.CollegeOrUniversity; }
                    //if (ContactPerson.HighSchool != null) { txtHighScholl.Text = ContactPerson.HighSchool; }
                    if (ContactPerson.Remark != null) { txtremark.Text = ContactPerson.Remark; }
                    rbtnActiveYes.Checked = true;
                    rbtnActiveNo.Checked = false;
                    if (ContactPerson.Active == "N") { rbtnActiveYes.Checked = false; rbtnActiveNo.Checked = true; }
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ConatactPerson.aspx.cs", "GetRecordFromTempDataBySequenceID");
            }
            finally { ContactPersonClient.Close(); }
        }

        [WebMethod]
        public static void PMSaveConatctPerson(object ContactPersonInfo)
        {
            iContactPersonInfoClient ContactPersonClient = new iContactPersonInfoClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                SP_GetContactPersonListToBindGrid_Result ContactPersonList = new SP_GetContactPersonListToBindGrid_Result();

                if (Sequence != "" && Sequence != "0")
                {
                    ContactPersonList = ContactPersonClient.GetContactPersonTempDataBySequence(Convert.ToInt64(Sequence), sessionID, TargetObject + "_ContactPerson", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    ContactPersonList.Sequence = Convert.ToInt64(Sequence);
                }
                else { ContactPersonList.Sequence = 0; }
                Dictionary<string, object> rec = new Dictionary<string, object>();
                rec = (Dictionary<string, object>)ContactPersonInfo;

                ContactPersonList.ObjectName = TargetObject;
                ContactPersonList.ReferenceID = Convert.ToInt64(Sequence);
                ContactPersonList.CustomerHeadID = 0;
                ContactPersonList.Sequence = Convert.ToInt64(Sequence);
                ContactPersonList.Name = rec["Name"].ToString();
                ContactPersonList.Department = Convert.ToInt64(rec["Department"].ToString());
                ContactPersonList.Designation = rec["Designation"].ToString();
               // ContactPersonList.Designation = rec["Designation"].ToString();
                ContactPersonList.EmailID = rec["EmailID"].ToString();
                ContactPersonList.OfficeNo = rec["OfficeNo"].ToString();
                ContactPersonList.MobileNo = rec["MobileNo"].ToString();
                ContactPersonList.ContactTypeID = Convert.ToInt64(rec["ContactTypeID"].ToString());
                ContactPersonList.Remark = rec["Remark"].ToString();
                ContactPersonList.Active = "N";             //rec["Active"].ToString(); //Active "N" = Is Archive
                ContactPersonList.CreatedBy = profile.Personal.UserID.ToString();
                ContactPersonList.CreationDate = DateTime.Now;
                ContactPersonList.LastModifiedBy = profile.Personal.UserID.ToString();
                ContactPersonList.LastModifiedDate = DateTime.Now;
                ContactPersonList.CompanyID = profile.Personal.CompanyID;     
                //ContactPersonList.CompanyID = Convert.ToInt64(rec["hdncompanyid"].ToString()); ;
                ContactPersonList.ContactType = rec["ContactType"].ToString();
                ContactPersonList.selected = "";
                if (Sequence != "0" && Sequence != "")
                { ContactPersonClient.SetValuesToTempData_onChange(sessionID, profile.Personal.UserID.ToString(), TargetObject + "_ContactPerson", Convert.ToInt32(Sequence), ContactPersonList, profile.DBConnection._constr); }
                else
                { ContactPersonClient.InsertIntoTemp(ContactPersonList, sessionID, profile.Personal.UserID.ToString(), TargetObject + "_ContactPerson", profile.DBConnection._constr); }
                ContactPersonClient.Close();

            }

            catch (System.Exception ex)
            {
                ContactPersonClient.Close();
                Login.Profile.ErrorHandling(ex, "ContactPersonInfo.aspx", "PMSaveAddress");
            }
        }

        [WebMethod]
        public static string PMCheckDuplicate(string ContactName, long Sequence)
        {
            iContactPersonInfoClient ContactPersonClient = new iContactPersonInfoClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string Result;
            Result = ContactPersonClient.CheckDuplicateContactPersonName(sessionID, ContactName, Sequence, profile.Personal.UserID.ToString(), TargetObject, profile.DBConnection._constr);
            ContactPersonClient.Close();
            return Result;
        }

        protected void FillDropDown()
        {

            ddlcontacttype.Items.Clear();
            iContactPersonInfoClient ObjContactPerson = new iContactPersonInfoClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcontacttype.DataSource = ObjContactPerson.GetContactTypeList(profile.DBConnection._constr);
            ddlcontacttype.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcontacttype.Items.Insert(0, lst);
        }

        //public void GetDepartment()
        //{
        //    DataSet ds;
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iContactPersonInfoClient ObjContactPerson = new iContactPersonInfoClient();
        //    try
        //    {

        //        ds = ObjContactPerson.GetDepartmentcontact(companyID, profile.DBConnection._constr);
        //        ddldepartment.DataSource = ds;
        //        ddldepartment.DataTextField = "Territory";
        //        ddldepartment.DataValueField = "ID";
        //        ddldepartment.DataBind();
        //        ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
        //        ddldepartment.Items.Insert(0, lst);
        //    }
        //    catch { }
        //    finally { ObjContactPerson.Close(); }
        //}

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            btnAddressSubmit.Value = rm.GetString("Submit", ci);
            btnAddressClear.Value = rm.GetString("Clear", ci);
            lblconname.Text = rm.GetString("ContactName", ci);
           // lbldeptc.Text = rm.GetString("Department", ci);
            lblemailid.Text = rm.GetString("EmailID", ci);
            lblofficeno.Text = rm.GetString("officeno", ci);
            lblmobno.Text = rm.GetString("MobileNo", ci);
            lblcontvalidn.Text = rm.GetString("conatctvalidn", ci);
            lblremark.Text = rm.GetString("Remark", ci);
            lblconttype.Text = rm.GetString("conacttype", ci);
            lblactive.Text = rm.GetString("Active", ci);
            rbtnActiveYes.Text = rm.GetString("Yes", ci);
            rbtnActiveNo.Text = rm.GetString("No", ci);
            Button1.Value = rm.GetString("Submit", ci);
            Button2.Value = rm.GetString("Clear", ci);
            lblContactPersonFormHeader.Text = rm.GetString("addnewcontact", ci);

        }
    }
}