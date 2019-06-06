using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UCProductSearchService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.WMSInbound;

namespace BrilliantWMS.WMS
{
    public partial class LocationSearch : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_PreInit(Object sender, EventArgs e)
        { //CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } 
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iInboundClient Inbound = new iInboundClient();
                if (Session["QCID"] != null)
                {
                    long qcId = long.Parse(Session["QCID"].ToString());
                    grdLocationSearch.DataSource = Inbound.GetLocationForPutIn(grdLocationSearch.CurrentPageIndex,hdnFilterText.Value, qcId, profile.DBConnection._constr);
                    grdLocationSearch.DataBind();
                }
                else if (Session["SOID"] != null)
                {
                    long soId = long.Parse(Session["SOID"].ToString());
                    //grdLocationSearch.DataSource = Inbound.GetLocationForPutIn(grdLocationSearch.CurrentPageIndex, hdnFilterText.Value, soId, profile.DBConnection._constr);
                    grdLocationSearch.DataSource = Inbound.GetLocationForPickUP(grdLocationSearch.CurrentPageIndex, hdnFilterText.Value, soId, profile.DBConnection._constr);
                    grdLocationSearch.DataBind();
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "LocationSearch.aspx.cs", "RebindGrid");
            }
        }
    }
}