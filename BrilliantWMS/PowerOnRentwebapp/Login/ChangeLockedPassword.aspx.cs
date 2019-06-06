using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Security;
using BrilliantWMS.UserCreationService;
using System.Data.SqlClient;
using System.Data;

namespace BrilliantWMS.Login
{
    public partial class ChangeLockedPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtConfirmNewPassword.Attributes.Add("onblur", "CheckPassword();");
            if (!IsPostBack)
            {
                CustomProfile profile = CustomProfile.GetProfile();
                lblLoginName.Text = profile.UserName;
            }
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }

            //var onBlurScript = Page.ClientScript.GetPostBackEventReference(txtConfirmNewPassword, "OnBlur");
            //txtConfirmNewPassword.Attributes.Add("onblur", onBlurScript);
        }

        [WebMethod]
        public static string PMSaveChangePassword(string loginname, string currentpassword, string newpassword)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iUserCreationClient userClient = new iUserCreationClient();
                MembershipUser u = Membership.GetUser(loginname);
                if (u.IsLockedOut == true) u.UnlockUser();
                if (u.ChangePassword(currentpassword, newpassword) == true)
                {
                    string Email = profile.Personal.EmailID;
                    userClient.SavePasswordDetails(profile.Personal.UserID, Email, loginname, newpassword, profile.Personal.UserName, profile.DBConnection._constr);
                    return "Saved";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex) { return ""; }
            finally { }
        }

        [WebMethod]
        public static string PMCheckPassword(string ConfirmPassword)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            DataSet ds = new DataSet();
            string str = "";
            try
            {
                ds = userClient.CheckPasswordHistory(profile.Personal.UserID, ConfirmPassword, profile.DBConnection._constr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Password"].ToString() == ConfirmPassword)
                    {
                        str = "PasswordFound";
                    }
                }

                if (str == "")
                {
                    string OneDayValidation = userClient.CheckOneDayValidation(profile.Personal.UserID, profile.DBConnection._constr);
                    str = OneDayValidation;
                }
            }
            catch (Exception ex) { }
            finally { }
            return str;
        }

    }
}