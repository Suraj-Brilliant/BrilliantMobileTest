using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerOnRentwebapp.Login;
using System.Data.SqlClient;
using System.Web.Services;



namespace PowerOnRentwebapp.MasterPage
{
    public partial class CRM : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserCreationService.iUserCreationClient UserCreationClient = new UserCreationService.iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session.Timeout > 0)
                {

                    userName.Text = profile.Personal.UserName;
                    ClientLogo.ImageUrl = profile.Personal.CLogoURL;
                    //btnLogout.HRef = "../Login/Login.aspx?ID=" + profile.Personal.CompanyID.ToString();

                    //BindMenuService.iBindMenuClient objBindMenuClient = new BindMenuService.iBindMenuClient();
                    //BindMenuService.ProBindMenu objProBindMenu = new BindMenuService.ProBindMenu();
                    //List<BindMenuService.ProBindMenu> ListobjProBindMenu = new List<BindMenuService.ProBindMenu>();
                    //objProBindMenu._constP = profile.DBConnection._constr;
                    //objProBindMenu.CompanyCode = profile.Personal.CompanyID;
                    //objProBindMenu.UserCode = profile.Personal.UserID;
                    //ListobjProBindMenu = objBindMenuClient.BindUserMenu(objProBindMenu).ToList();

                    if (Session["htmlMenu"] == null)
                    {
                        Session["htmlMenu"] = UserCreationClient.GetHTMLMenuByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                    }

                    dvMenu.InnerHtml = Session["htmlMenu"].ToString();
                    if (profile.Personal.ProfileImg != null)
                    {
                        Session["ProfileImgMasterPg"] = profile.Personal.ProfileImg;
                        ImgProfileMasterPg.Src = "../Image1.aspx";
                    }
                    else
                    {
                        ImgProfileMasterPg.Src = "../App_Themes/Blue/img/Male.png";
                        if (profile.Personal.Gender != null)
                        {
                            if (profile.Personal.Gender != "M") { ImgProfileMasterPg.Src = "../App_Themes/Blue/img/Female.png"; }
                        }
                    }
                }
                else
                {
                    LoginService.iLoginClient loginclient = new LoginService.iLoginClient();
                    loginclient.ClearTempDataBySessionID(Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("../Login/Login.aspx?TimeOut=true&ID=" + profile.Personal.CompanyID.ToString(), false);
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Master Page", "Bind Menu");
            }
            finally { UserCreationClient.Close(); }
        }
              

        [WebMethod]          
        public static void WMClearSession()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
        }
    }
}