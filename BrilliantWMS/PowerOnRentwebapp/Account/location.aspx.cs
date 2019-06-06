using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using Obout.Grid;
using BrilliantWMS.PopupMessages;
using System.Web.Services;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.AddressInfoService;
using BrilliantWMS.PORServiceUCCommonFilter;
using System.IO;

namespace BrilliantWMS.Account
{
    public partial class location : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string sessionID = "";
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

            ActiveTab("Load");

            sessionID = Session.SessionID;
            UCToolbar1.ParentPage = this;

            if (!IsPostBack)
            {
                getLocationList();
                GetCompany();
                
            }

           this.UCToolbar1.ToolbarAccess("Accounts");
           this.UCToolbar1.evClickAddNew += pageAddNew;
           this.UCToolbar1.evClickSave += pageSave;
           this.UCToolbar1.evClickClear += pageClear;

          
                      
        }
        public void getLocationList()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            long LocID = 0;
            ds = CompanyClient.GetLocationList(LocID,profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                Gvlocation.DataSource = ds.Tables[0];
                Gvlocation.DataBind();
            }
            else
            {
                Gvlocation.DataSource = null;
                Gvlocation.DataBind();

            }
        }

        protected void Gvlocation_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)Gvlocation.SelectedRecords[0];
               
                hdnselectedLocation.Value = selectedrec["ID"].ToString();
                hdnmodestate.Value = "Edit";
                locationDetails(long.Parse(hdnselectedLocation.Value));
                ActiveTab("Edit");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "GvCustomer_Select");
            }
            finally
            {
            }
        }

        public void locationDetails(long locationID)
        {
            ddlCountry.SelectedIndex = -1;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                ds = CompanyClient.GetLocationList(locationID, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ddlcompany.SelectedValue = dt.Rows[0]["CompanyID"].ToString();
                    hdnSelectedCompany.Value = dt.Rows[0]["CompanyID"].ToString();
                    //getDepartment(long.Parse(dt.Rows[0]["CompanyID"].ToString()));
                   // ddldept.SelectedValue = dt.Rows[0]["deptid"].ToString();

                    txtlocationName.Text = dt.Rows[0]["LocationName"].ToString();

                   // hdnSelectedDepartment.Value = dt.Rows[0]["deptid"].ToString();
                    txtlocation.Text = dt.Rows[0]["LocationCode"].ToString();
                    txtCAddress1.Text = dt.Rows[0]["AddressLine1"].ToString();
                    txtAddress2.Text = dt.Rows[0]["AddressLine2"].ToString();
                    txtZipCode.Text = dt.Rows[0]["zipcode"].ToString();
                    txtCity.Text = dt.Rows[0]["City"].ToString();
                    txtLandMark.Text = dt.Rows[0]["landmark"].ToString();
                    txtFax.Text = dt.Rows[0]["FaxNo"].ToString();
                    txtname.Text = dt.Rows[0]["ContactName"].ToString();
                    txtemailid.Text = dt.Rows[0]["ContactEmail"].ToString();
                    txtphoneno.Text = dt.Rows[0]["MobileNo"].ToString();
                    string country = dt.Rows[0]["County"].ToString();
                    string State = dt.Rows[0]["State"].ToString();
                    hdnSearchContactName1.Value = dt.Rows[0]["ContactName"].ToString();
                    hdnSearchContactID1.Value = dt.Rows[0]["ShippingID"].ToString();
                    hdnSearchConEmail.Value = dt.Rows[0]["ContactEmail"].ToString();
                    hdnSearchConMobNo.Value = dt.Rows[0]["MobileNo"].ToString();
                    if(dt.Rows[0]["Active"].ToString() == "N")
                    {
                        rbtnActiveNo.Checked = true;
                        rbtnActiveYes.Checked = false;
                    }
                    hdnCountry.Value = country;
                    hdnState.Value = State;
                    Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + country + "','" + State + "');", true);
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Location Master", "locationDetails");
            }
            finally
            {
                CompanyClient.Close();
            }
        }

        public void GetCompany()
        {
            DataSet ds;
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            try
            {

                ds = productClient.GetCompanyname(profile.DBConnection._constr);
                List<mCompany> CompanyLst = new List<mCompany>();
                long UID = profile.Personal.UserID;
                string UserType = profile.Personal.UserType.ToString();
                if (UserType == "Admin")
                {
                    CompanyLst = objService.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
                }
                else
                {
                    CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                }
                //ddlcompany.DataSource = ds;
                ddlcompany.DataSource = CompanyLst;
                ddlcompany.DataTextField = "Name";
                ddlcompany.DataValueField = "ID";
                ddlcompany.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlcompany.Items.Insert(0, lst);
            }
            catch { }
            finally { productClient.Close(); }
        }

        //public void getDepartment(long Companyid)
        //{
        //    DataSet ds;
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iProductMasterClient productClient = new iProductMasterClient();
        //    ds = productClient.GetDepartment(Companyid, profile.DBConnection._constr);
        //    ddldept.DataSource = ds;
        //    ddldept.DataTextField = "Territory";
        //    ddldept.DataValueField = "ID";
        //    ddldept.DataBind();
        //    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
        //    ddldept.Items.Insert(0, lst);
 
        //}

        //[WebMethod]
        //public static List<contact> GetDepartment(object objReq)
        //{
        //    iProductMasterClient productClient = new iProductMasterClient();
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    List<contact> LocList = new List<contact>();
        //    try
        //    {
        //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //        dictionary = (Dictionary<string, object>)objReq;
        //        long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());
        //        iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
        //       if (profile.Personal.UserType == "Admin")
        //        {
        //            ds = UCCommonFilter.GetAddedDepartmentListDS(int.Parse(ddlcompanyId.ToString()), profile.Personal.UserID, profile.DBConnection._constr);
        //        }
        //        else
        //        {
        //            ds = productClient.GetDepartment(ddlcompanyId, profile.DBConnection._constr);
        //        }

        //        dt = ds.Tables[0];
        //        contact Loc = new contact();
        //        Loc.Name = "Select Department";
        //        Loc.Id = "0";
        //        LocList.Add(Loc);
        //        Loc = new contact();

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                Loc.Id = dt.Rows[i]["ID"].ToString();
        //                Loc.Name = dt.Rows[i]["Territory"].ToString();
        //                LocList.Add(Loc);
        //                Loc = new contact();
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        productClient.Close();
        //    }
        //    return LocList;
        //}

        public class contact
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                long locationid = 0;
                long companyid = long.Parse(hdnSelectedCompany.Value);
                long deptid = 0;
                locationid = long.Parse(hdnselectedLocation.Value);
                string hdnstate = hdnmodestate.Value;
                string locationCode = txtlocation.Text;
                string address1 = txtCAddress1.Text;
                string address2 = txtAddress2.Text;
                string city = txtCity.Text;
                string zipcode = txtZipCode.Text;
                string landmark = txtLandMark.Text;
                string faxno = txtFax.Text;
                //string contactName = txtname.Text;
                string contactName = hdnSearchContactName1.Value;
               // string email = txtemailid.Text;
                string email = hdnSearchConEmail.Value;
                string Locationname = txtlocationName.Text;
                long ContactID = long.Parse(hdnSearchContactID1.Value);
                long MobileNo = 0;
                if (hdnSearchConMobNo.Value != "")
                {
                    MobileNo = long.Parse(hdnSearchConMobNo.Value);
                }
                string Mobilecont = txtphoneno.Text;
                string Country = hdnCountry.Value;
                string State = hdnState.Value;
                string userid = profile.Personal.UserID.ToString();
                string active = "N";
                if (rbtnActiveYes.Checked == true)
                {
                    active = "Y";
                }

                if (hdnstate == "AddNew")
                {
                    
                    //long ContactID = CompanyClient.SaveLocContactInContact("Contact", companyid, 0, 1, contactName, email, Mobilecont, 4, "Y", userid, DateTime.Now, companyid, profile.DBConnection._constr);
                    CompanyClient.SaveEditLocation(locationid, companyid, deptid, locationCode, address1, address2, Country, State, city, zipcode, landmark, faxno, active, contactName, email,Locationname, MobileNo, userid, DateTime.Now, hdnstate,ContactID, profile.DBConnection._constr);
                    WebMsgBox.MsgBox.Show("Record Saved successfully");
                }
                else
                {
                    long contactIdUpdate = CompanyClient.GetContactIdasShippingId(locationid, profile.DBConnection._constr);

                    if (active=="N")
                    {
                        int totaluser = CompanyClient.CheckLocationIDForAssignedUser(locationid, profile.DBConnection._constr);
                        if (totaluser == 1)
                        {
                            WebMsgBox.MsgBox.Show("Location is assigned for user Please remove the location for that user");
                           // rbtnActiveNo.Text = "";
                            rbtnActiveNo.Checked = false;
                            rbtnActiveYes.Checked = true;
                        }
                        else
                        {
                           // CompanyClient.UpdateContacttableDetail(contactName, email, Mobilecont, contactIdUpdate, profile.DBConnection._constr);
                            CompanyClient.UpdateLocationDetails(locationid, companyid, deptid, locationCode, address1, address2, Country, State, city, zipcode, landmark, faxno, active, contactName, email, MobileNo, userid, DateTime.Now, hdnstate,Locationname,ContactID, profile.DBConnection._constr);
                            WebMsgBox.MsgBox.Show("Record Updated successfully");
                        }
                    }
                    else
                    {
                       // CompanyClient.UpdateContacttableDetail(contactName, email, Mobilecont, contactIdUpdate, profile.DBConnection._constr);
                        CompanyClient.UpdateLocationDetails(locationid, companyid, deptid, locationCode, address1, address2, Country, State, city, zipcode, landmark, faxno, active, contactName, email, MobileNo, userid, DateTime.Now, hdnstate,Locationname,ContactID, profile.DBConnection._constr);
                        WebMsgBox.MsgBox.Show("Record Updated successfully");
                    }
                   
                }

                clr();
                getLocationList();
                ActiveTab("Load");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Location Master", "pageSave");
            }
            finally 
            {
                CompanyClient.Close();
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
            hdnmodestate.Value = "AddNew";
            hdnselectedLocation.Value = "0";
            ActiveTab("Add");
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
            hdnmodestate.Value = "";
            ActiveTab("Add");
        }

        public void clr()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // ddlgroupcompany.SelectedIndex = -1;
               
                txtCAddress1.Text = "";
                txtAddress2.Text = "";
                txtLandMark.Text = "";
                ddlCountry.SelectedIndex = -1;
                ddlState.SelectedIndex = -1;
                ddlcompany.SelectedValue = "0";
                //ddldept.SelectedValue = "0";
                hndCompanyid.Value = "0";
                hdnSelectedDepartment.Value = "0";
                hdnSelectedCompany.Value = "0";
                txtCity.Text = "";
                txtZipCode.Text = "";
                txtphoneno.Text = "";
                txtFax.Text = "";
                hndState.Value = "";
                txtemailid.Text = "";
                hdnState.Value = "";
                hdnCountry.Value = "";
                txtlocation.Text = "";
                txtname.Text = "";
                hdnmodestate.Value = "";
                hdnselectedLocation.Value = "";
                rbtnActiveNo.Text = "";
                rbtnActiveYes.Text = "";
                txtlocationName.Text = "";
                hdnSearchContactID1.Value = "";
                hdnSearchContactName1.Value = "";
                hdnSearchConEmail.Value = "";
                hdnSearchConMobNo.Value = "";
              }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Location", "clr");
            }
            finally
            {
            }
        }

        protected void ActiveTab(string state)
        {

            if (state == "Edit")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                tabAccountInfo.Visible = true;
            }

            else if (state == "Add")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                tabAccountInfo.Visible = true;
            }
            else
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 2;
                tabAccountInfo.Visible = false;

            }
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                TabCustomerList.HeaderText = rm.GetString("LocationList", ci);
                lblheasertext.Text = rm.GetString("LocationList", ci);
                tabAccountInfo.HeaderText = rm.GetString("LocationInfo", ci);
                lblcustname.Text = rm.GetString("company", ci);
                lblwebsite.Text = rm.GetString("LocationName", ci);
                lblcode.Text = rm.GetString("LocationCode", ci);
                lbladdress1.Text = rm.GetString("AddressLine1", ci);
                lbladdress2.Text = rm.GetString("AddressLine2", ci);
                lblcountry.Text = rm.GetString("Country", ci);
                lblstate.Text = rm.GetString("State", ci);
                lblcity.Text = rm.GetString("City", ci);
                lblzip.Text = rm.GetString("ZIPCode", ci);
                lbllandmark.Text = rm.GetString("Landmark", ci);
                lblfax.Text = rm.GetString("FaxNo", ci);
                lblactive.Text = rm.GetString("Active", ci);
                rbtnActiveYes.Text = rm.GetString("Yes", ci);
                rbtnActiveNo.Text = rm.GetString("No", ci);
                lblname.Text = rm.GetString("ContactName", ci);
                lblphone.Text = rm.GetString("ContactPhoneNo", ci);
                lblemailid.Text = rm.GetString("ContactEmailID", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("LocationMaster", ci);
               
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "loadstring");
            }
        }

    }
}