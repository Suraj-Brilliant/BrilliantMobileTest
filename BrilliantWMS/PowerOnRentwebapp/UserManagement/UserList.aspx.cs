using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;

namespace BrilliantWMS.UserManagement
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.UserCreationService.iUserCreationClient usercreationclient = new BrilliantWMS.UserCreationService.iUserCreationClient();
                GvAccessTo.DataSource = usercreationclient.GetUserList(profile.DBConnection._constr);
                GvAccessTo.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "UserList.aspx.cs", "Page_Load");
            }
        }
    }
}