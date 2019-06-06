using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.WMS
{
    public partial class ClientList : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) FillClientDeatilGrid();
        }

        public void FillClientDeatilGrid()
        {
            iOutboundClient Outbound = new iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdClient.DataSource = Outbound.GetCompanyWiseClient(profile.Personal.CompanyID, profile.DBConnection._constr);
                grdClient.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ClientList.aspx", "FillClientDeatilGrid");
            }
            finally
            { Outbound.Close(); }
        }
    }
}