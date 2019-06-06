using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Obout.Grid;
using System.IO;
using WebMsgBox;
using System.Web.Security;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;


//namespace PowerOnRentwebapp.Login
namespace BrilliantWMS.Login
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientLogo.ImageUrl = "#";
            lblSessionMsg.Visible = false;
            
            if (Request.QueryString["ID"] != null)
            {
                //ClientLogo.ImageUrl = "~/Company/Logo/" + Request.QueryString["ID"].ToString() + ".png";
                //loginuser.HyperLink1.NavigateUrl = "~/login/ForgotPassword.aspx?ID=" + Request.QueryString["ID"].ToString();
            }

            if (Request.QueryString["TimeOut"] != null)
            {
                if (Request.QueryString["TimeOut"] == "true") { lblSessionMsg.Visible = true; }
            }
            if (Request.QueryString["act"] == "0") { WebMsgBox.MsgBox.Show("Your Account Has Been Blocked. Please Contact GWC System Administrator."); }

            BrilliantWMS.UserCreationService.iUserCreationClient UserCreationClient = new BrilliantWMS.UserCreationService.iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();

            Session["Lang"] = ddllanguage.SelectedValue;
            string culturname = ddllanguage.SelectedValue.ToString();
            Page.Culture = culturname;
            Page.UICulture = culturname;
        }

        protected void loginuser_OnLoggedIn(object sender, EventArgs e)
        {
            CustomProfile profile1 = CustomProfile.GetProfile(loginuser.UserName);
            if (profile1 != null)
            {
                //Session.Abandon();
               // Session.Clear();
                profile1.Personal.Theme = "Blue";

                //profile.DBConnection._constr[0] = "elegantcrm.db.11040877.hostedresource.com";
                //profile.DBConnection._constr[1] = "elegantcrm";
                //profile.DBConnection._constr[2] = "Password123#";
                //profile.DBConnection._constr[3] = "elegantcrm";

                //profile1.DBConnection._constr[0] = "BWMSTest.db.11040877.c93.hostedresource.net";
                //profile1.DBConnection._constr[1] = "BWMSTest";
                //profile1.DBConnection._constr[2] = "Password123#";
                //profile1.DBConnection._constr[3] = "BWMSTest";

                profile1.DBConnection._constr[0] = "166.62.35.21";
                profile1.DBConnection._constr[1] = "BWMSTest";
                profile1.DBConnection._constr[2] = "Password123#";
                profile1.DBConnection._constr[3] = "sa";

                //profile1.DBConnection._constr[0] = "DEVLOPMENT-PC";
                //profile1.DBConnection._constr[1] = "BrilliantWMS";
                //profile1.DBConnection._constr[2] = "Password123#";
                //profile1.DBConnection._constr[3] = "sa";

                profile1.Save();

                BrilliantWMS.UserCreationService.iUserCreationClient UserCreationClient = new BrilliantWMS.UserCreationService.iUserCreationClient();
                CustomProfile profile = CustomProfile.GetProfile();


                
                BrilliantWMS.UserCreationService.vGetUserProfileByUserID objuser = new BrilliantWMS.UserCreationService.vGetUserProfileByUserID();
                objuser = UserCreationClient.GetUserProfileByUserID(profile1.Personal.UserID, profile1.DBConnection._constr);

                string Actv = objuser.Active;
                if (Actv == "No")
                {
                    //WebMsgBox.MsgBox.Show("This User Is Not Active User");
                    Response.Redirect("../Login/Login.aspx?act=0");                    
                }
                else
                {
                    Session["Lang"] = ddllanguage.SelectedValue;
                    string culturname = ddllanguage.SelectedValue.ToString();
                    Page.Culture = culturname;
                    Page.UICulture = culturname;

                    Response.Redirect("../Inbox/InboxPOR.aspx");//For POR
                }
                //Response.Redirect("../PowerOnRent/Default.aspx?invoker=Request");
            }
        }

        protected void loginuser_OnLoginError(object sender, EventArgs e)
        {
            MembershipUser userInfo = Membership.GetUser(loginuser.UserName);
        }

        //protected void ddllanguage_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    UserCreationService.iUserCreationClient UserCreationClient = new UserCreationService.iUserCreationClient();
        //    CustomProfile profile = CustomProfile.GetProfile();

        //    Session["Lang"] = ddllanguage.SelectedValue;
        //    string culturname = ddllanguage.SelectedValue.ToString();
        //    Page.Culture = culturname;
        //    Page.UICulture = culturname;
        //}
    }
}
