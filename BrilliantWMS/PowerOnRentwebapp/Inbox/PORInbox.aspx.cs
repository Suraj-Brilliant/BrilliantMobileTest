using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.InboxService;
using BrilliantWMS.Login;
namespace BrilliantWMS.Inbox
{
    public partial class PORInbox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            iInboxClient objService = new iInboxClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVInbox.DataSource = objService.GetInboxDataByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                GVInbox.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }
    }
}