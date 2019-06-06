using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartRequest;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Web.Services;

namespace BrilliantWMS.PowerOnRent
{
    public partial class AllocateDriver : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RebindGrid(sender, e);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "AllocateDriver.aspx.cs", "Page_Load"); }
            finally { }

            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
        }
        protected void RebindGrid(object sender, EventArgs e)
        {
            iPartRequestClient assignDriver = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            var SearchedValue = hdnFilterText.Value;
            DataSet ds = new DataSet();
            if (SearchedValue == "")
            {
                ds = assignDriver.GetDriverDetails(profile.DBConnection._constr);
            }
            else
            {
                ds = assignDriver.GetFilteredDriverList(SearchedValue,profile.DBConnection._constr);
            }
            GVDriver.DataSource = ds;
            GVDriver.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static int WMAssignDriver(long hndSelectedRec, string OrderID, string TruckDetails)
        {
            iPartRequestClient assignDriver = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();            
            string[] eorder = OrderID.Split(',');
            int cnt = eorder.Count();
            for (int i = 0; i <= cnt - 1; i++)
            {
                assignDriver.AssignSelectedDriver(long.Parse(eorder[i].ToString()), hndSelectedRec,TruckDetails, profile.Personal.UserID ,profile.DBConnection._constr);
            }
           return 1;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblTruckDetail.Text = rm.GetString("TruckDetails", ci);
            lblheader.Text = rm.GetString("DriverList", ci);
          
        }
    }
}