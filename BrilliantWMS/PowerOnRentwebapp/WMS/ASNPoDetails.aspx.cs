using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.WMSInbound;

namespace BrilliantWMS.WMS
{
    public partial class ASNPoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long asnID =long.Parse(Session["ASNID"].ToString());
            if (Session["ASNID"].ToString() != null)
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iInboundClient Inbound = new iInboundClient();
                gdASNSku.DataSource = Inbound.GetASNDetailByID(asnID, profile.DBConnection._constr);
                gdASNSku.DataBind();
            }
        }
    }
}