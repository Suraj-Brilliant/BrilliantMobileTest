using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.PORServicePartRequest;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.PowerOnRent
{
    public partial class bomDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string PrdID = Request.QueryString["id"].ToString();
            fillGrid(PrdID);
        }

        protected void fillGrid(string PrdID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVBOMDetail.DataSource = null;
            GVBOMDetail.DataBind();

            DataSet dsBomDetails = new DataSet();
            dsBomDetails = objService.GetBomDetails(PrdID, profile.DBConnection._constr);

            GVBOMDetail.DataSource = dsBomDetails;
            GVBOMDetail.DataBind();
        }
    }
}