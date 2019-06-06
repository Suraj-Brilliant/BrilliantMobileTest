using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.DocumentService;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.PORServiceUCCommonFilter;

namespace BrilliantWMS.WMS
{
    public partial class AsnDetail : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string state = "";
        static string ObjectName = "ASN";
        #region PageEvents
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ASNID"] != null)
            {
                long asnID = long.Parse(Session["ASNID"].ToString());
                GetASNDetail(asnID);
            }
            else
            {

            }
            Toolbar1.SetUserRights("MaterialRequest", "Summary", "");

            Toolbar1.SetAddNewRight(true, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(true, "Not Allowed");
        }
        #endregion

        public void GetASNDetail(long asnID)
        {
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                tASNHead ah = new tASNHead();
                ah = Inbound.GetAsnByAsnID(asnID, profile.DBConnection._constr);

                txtASNNumber.Text = ah.ASNNumber.ToString();
                UCASNDate.Date = ah.ASNDate;

                UsersList = objService.GetUserListByWarehouseID(10013, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
                ddlASNBy.DataSource = UsersList;
                ddlASNBy.DataBind();
                ddlASNBy.SelectedIndex = ddlASNBy.Items.IndexOf(ddlASNBy.Items.FindByValue(ah.ASNEnteredBy.ToString()));

                List<mWarehouseMaster> WarehouseList = new List<mWarehouseMaster>();
                long UserID = profile.Personal.UserID;
                WarehouseList = Inbound.GetUserWarehouse(UserID, profile.DBConnection._constr).ToList();
                ddlWarehouse.DataSource = WarehouseList;
                ddlWarehouse.DataBind();
               // ddlWarehouse.SelectedIndex = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(ah.WarehouseNo.ToString()));

                List<mVendor> VendorLst = new List<mVendor>();
                VendorLst = Inbound.GetVendor(profile.Personal.CompanyID,profile.DBConnection._constr).ToList();
                ddlVendor.DataSource = VendorLst;
                ddlVendor.DataBind();
            //    ddlVendor.SelectedIndex = ddlVendor.Items.IndexOf(ddlVendor.Items.FindByValue(ah.VendorId.ToString()));

                txtRemark.Text = ah.Remark.ToString();

                Grid1.DataSource = Inbound.GetASNDetailByID(asnID, profile.DBConnection._constr);
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "AsnDetail.aspx", "GetASNDetail"); }
            finally { Inbound.Close(); }
        }
    }
}