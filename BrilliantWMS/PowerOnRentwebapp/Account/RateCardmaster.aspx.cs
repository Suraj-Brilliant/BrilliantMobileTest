using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using Obout.Grid;
using System.Web.Services;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.CompanySetupService;
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
    public partial class RateCardmaster : System.Web.UI.Page
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
                MainRateCardGrid();
                GetCompany();
                FillCompany();
                GetRateType();
            }

            this.UCToolbar1.ToolbarAccess("Accounts");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }


        protected void FillCompany()
        {
            ddlgroupcompany.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlgroupcompany.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlgroupcompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlgroupcompany.Items.Insert(0, lst);
            CompanyClient.Close();
        }

        protected void GetRateType()
        {
            ddlratetype.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlratetype.DataSource = CompanyClient.GetRateTypeDropdown("RateCard", profile.DBConnection._constr);
            ddlratetype.DataTextField = "Value";
            ddlratetype.DataValueField = "Id";
            ddlratetype.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlratetype.Items.Insert(0, lst);
            CompanyClient.Close();
        }

        protected void MainRateCardGrid()
        {
            iCompanySetupClient Ratecard = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdratecard.DataSource = Ratecard.GetRateCardDetails(profile.DBConnection._constr);
                grdratecard.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "MainCustomerGridBind");
            }
            finally
            {
                Ratecard.Close();
            }

        }



        protected void grdratecard_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                clr();
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)grdratecard.SelectedRecords[0];
                hdnratecardId.Value = selectedrec["ID"].ToString();
                hndState.Value = "Edit";
                GetRateCardByID(long.Parse(hdnratecardId.Value));

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

        public void GetRateCardByID(long RateCardID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCompanySetupClient ratecardclient = new iCompanySetupClient();
            mRateCard RateCard = new mRateCard();
            RateCard = ratecardclient.GetRateCardByID(RateCardID, profile.DBConnection._constr);
            ddlgroupcompany.SelectedIndex = ddlgroupcompany.Items.IndexOf(ddlgroupcompany.Items.FindByValue(RateCard.CompanyID.ToString()));
            ddlaccounttypeer.SelectedIndex = ddlaccounttypeer.Items.IndexOf(ddlaccounttypeer.Items.FindByText(RateCard.Type.ToString()));
            ddlratetype.SelectedIndex = ddlratetype.Items.IndexOf(ddlratetype.Items.FindByValue(RateCard.RateType.ToString()));
            txtprice.Text = RateCard.Rate.ToString();
            txtratetitle.Text = RateCard.RateDetails.ToString();
            UC_Startdt.Date = RateCard.FromDate;
            UC_enddt.Date = RateCard.ToDate;
            UC_effectivedt.Date = RateCard.EffDate;
            rbtnActiveYes.Checked = true;
            if (RateCard.Active == "No") rbtnActiveNo.Checked = true;
            txtname.Text = RateCard.AccountName.ToString();
            hdnAccountID.Value = RateCard.AccountID.ToString();
            txtremark.Text = RateCard.Remark.ToString();
        }

        public void GetCompany()
        {
            //DataSet ds;
            //CustomProfile profile = CustomProfile.GetProfile();
            //iProductMasterClient productClient = new iProductMasterClient();
            //iUCCommonFilterClient objService = new iUCCommonFilterClient();
            //try
            //{

            //    ds = productClient.GetCompanyname(profile.DBConnection._constr);
            //    List<mCompany> CompanyLst = new List<mCompany>();
            //    long UID = profile.Personal.UserID;
            //    string UserType = profile.Personal.UserType.ToString();
            //    if (UserType == "Admin")
            //    {
            //        CompanyLst = objService.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
            //    }
            //    else
            //    {
            //        CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
            //    }
            //    ddlcompany.DataSource = ds;
            //    ddlcompany.DataSource = CompanyLst;
            //    ddlcompany.DataTextField = "Name";
            //    ddlcompany.DataValueField = "ID";
            //    ddlcompany.DataBind();
            //    ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            //    ddlcompany.Items.Insert(0, lst);
            //}
            //catch { }
            //finally { productClient.Close(); }
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
            iCompanySetupClient ratecardclient = new iCompanySetupClient();
            mRateCard RateCard = new mRateCard();
            if (hndState.Value == "Edit")
            {
                RateCard = ratecardclient.GetRateCardByID(long.Parse(hdnratecardId.Value), profile.DBConnection._constr);
            }
            RateCard.Type = ddlaccounttypeer.SelectedItem.Text;
            RateCard.RateDetails = txtratetitle.Text;
            RateCard.Rate = decimal.Parse(txtprice.Text);
            RateCard.FromDate = UC_Startdt.Date;
            RateCard.ToDate = UC_enddt.Date;
            RateCard.EffDate = UC_effectivedt.Date;
            RateCard.CompanyID = long.Parse(ddlgroupcompany.SelectedItem.Value);
            RateCard.RateType = long.Parse(ddlratetype.SelectedItem.Value);
            RateCard.AccountID = long.Parse(hdnAccountID.Value);
            RateCard.Remark = txtremark.Text;
            RateCard.AccountName = hdnAccountName.Value;
            RateCard.Active = "Yes";
            if (rbtnActiveNo.Checked == true) RateCard.Active = "No";
            if (hndState.Value == "Edit")
            {
                RateCard.ModifiedBy = profile.Personal.UserID;
                RateCard.ModifiedDate = DateTime.Now;
                long ratecardID = ratecardclient.SaveRateCardMaster(RateCard, profile.DBConnection._constr);
                WebMsgBox.MsgBox.Show("Record Updated Successfully");
            }
            else
            {
                RateCard.CreatedBy = profile.Personal.UserID;
                RateCard.CreationDate = DateTime.Now;
                long ratecardID = ratecardclient.SaveRateCardMaster(RateCard, profile.DBConnection._constr);
                WebMsgBox.MsgBox.Show("Record Saved Successfully");
            }
            clr();
            MainRateCardGrid();
            ActiveTab("Load");
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
            ActiveTab("Add");
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
            ActiveTab("Add");
        }

        public void clr()
        {
            try
            {
                ddlgroupcompany.SelectedIndex = 0;
                ddlaccounttypeer.SelectedIndex = 0;
                ddlratetype.SelectedIndex = 0;
                txtname.Text = "";
                txtratetitle.Text = "";
                txtprice.Text = "";
                UC_Startdt.Date = null;
                UC_enddt.Date = null;
                UC_effectivedt.Date = null;
                txtremark.Text = "";
                hdnratecardId.Value = "";
                hndState.Value = "";
                rbtnActiveYes.Checked = true;


                
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
            //try
            //{
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            //    rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            //    ci = Thread.CurrentThread.CurrentCulture;

            //    TabCustomerList.HeaderText = rm.GetString("LocationList", ci);
            //    lblheasertext.Text = rm.GetString("LocationList", ci);
            //    tabAccountInfo.HeaderText = rm.GetString("LocationInfo", ci);
            //    lblcustname.Text = rm.GetString("company", ci);
            //    lblwebsite.Text = rm.GetString("LocationName", ci);
            //    lblcode.Text = rm.GetString("LocationCode", ci);
            //    lbladdress1.Text = rm.GetString("AddressLine1", ci);
            //    lbladdress2.Text = rm.GetString("AddressLine2", ci);
            //    lblcountry.Text = rm.GetString("Country", ci);
            //    lblstate.Text = rm.GetString("State", ci);
            //    lblcity.Text = rm.GetString("City", ci);
            //    lblzip.Text = rm.GetString("ZIPCode", ci);
            //    lbllandmark.Text = rm.GetString("Landmark", ci);
            //    lblfax.Text = rm.GetString("FaxNo", ci);
            //    lblactive.Text = rm.GetString("Active", ci);
            //    rbtnActiveYes.Text = rm.GetString("Yes", ci);
            //    rbtnActiveNo.Text = rm.GetString("No", ci);
            //    lblname.Text = rm.GetString("ContactName", ci);
            //    lblphone.Text = rm.GetString("ContactPhoneNo", ci);
            //    lblemailid.Text = rm.GetString("ContactEmailID", ci);
            //    UCFormHeader1.FormHeaderText = rm.GetString("LocationMaster", ci);

            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, this, "Account Master", "loadstring");
            //}
        }
    }
}