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
    public partial class Loader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ldrID"] != null)
            {
                long ldrID = long.Parse(Session["ldrID"].ToString());
                GetLoaderDetail(ldrID);
            }
            else
            {
                GetLoaderDetail(1);
            }
        }

        public void GetLoaderDetail(long ldrID)
        {
            iInboundClient Inbound = new iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                tLoaderDetail LD = new tLoaderDetail();
                LD = Inbound.GetLoaderDetailOfGRN(ldrID, profile.DBConnection._constr);

                List<mVendor> VendorLst = new List<mVendor>();
                VendorLst = Inbound.GetVendor(profile.Personal.CompanyID,profile.DBConnection._constr).ToList();
                ddlLoaderName.DataSource = VendorLst;
                ddlLoaderName.DataBind();
                ddlLoaderName.SelectedIndex = ddlLoaderName.Items.IndexOf(ddlLoaderName.Items.FindByValue(LD.LoaderID.ToString()));

                txtInTime.Text = LD.InTime.ToString();
                txtOutTime.Text = LD.OutTime.ToString();
                txtBoxhandled.Text = LD.BoxHandels.ToString();
                txtRate.Text = LD.RatePerBox.ToString();
                txtTotal.Text = LD.Total.ToString();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "Loader.aspx", "GetLoaderDetail"); }
            finally { Inbound.Close(); }
        }
    }
}