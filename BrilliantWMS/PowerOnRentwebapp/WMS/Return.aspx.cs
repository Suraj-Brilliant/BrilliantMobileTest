using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;
using BrilliantWMS.WMSInbound;

namespace BrilliantWMS.WMS
{
    public partial class Return : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindReturnGrid();
            Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");

            Toolbar1.SetAddNewRight(true, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(true, "Not Allowed");
        }

        public void BindReturnGrid()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds = Inbound.BindReturnGrid(profile.DBConnection._constr);
                grdReturn.DataSource = ds;
                grdReturn.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }
        }
    }
}