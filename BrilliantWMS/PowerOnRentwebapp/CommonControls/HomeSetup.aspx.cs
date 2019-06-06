using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;

namespace BrilliantWMS.CommonControls
{
    public partial class HomeSetup : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Setup";

        }

        //protected void LinkButton34_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/UserManagement/DesignationMaster.aspx");
        //}
    }
}