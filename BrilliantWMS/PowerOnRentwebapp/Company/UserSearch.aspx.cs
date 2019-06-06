using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;

namespace BrilliantWMS.Student
{
    public partial class UserSearch : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
        }
        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.UserCreationService.iUserCreationClient usercreationclient = new BrilliantWMS.UserCreationService.iUserCreationClient();
                GridUserSearch.DataSource = usercreationclient.GetUserList(profile.DBConnection._constr);
                GridUserSearch.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "UserSearch.aspx.cs", "RebindGrid");
            }
        }
    }
}