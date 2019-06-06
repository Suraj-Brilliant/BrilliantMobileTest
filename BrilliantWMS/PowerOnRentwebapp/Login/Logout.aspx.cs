using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using Obout.Grid;
using System.IO;
using WebMsgBox;
using System.Web.Security;
using System.Web.UI.WebControls;
using BrilliantWMS.LoginService;

namespace BrilliantWMS.Login
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            iLoginClient objService = new iLoginClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                objService.ClearTempDataBySessionID(Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                Session.RemoveAll();
                Session.Abandon();
            }
            catch { }
            finally { objService.Close(); }
        }
    }
}
