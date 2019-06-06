using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.AccountSearchService;
using BrilliantWMS.CompanySetupService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.UserCreationService;
using System.Web.Services;

namespace WebClientElegantCRM.Account
{
    public partial class AccountSearch : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long CompanyID;
        long DeptId;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            loadstring();
            CompanyID = Convert.ToInt64(Request.QueryString["Com"].ToString());
            DeptId = Convert.ToInt64(Request.QueryString["Dept"].ToString());
            hdnNoOfApprover.Value = Request.QueryString["NoApprover"].ToString();

            if (!IsPostBack)
            {
                FillCustomerDeatilGrid();
            }

        }

        [WebMethod]
        public static string PMGetHiddenValue(string UserSelectedRec)
        {
          

            HttpContext.Current.Session["Userlist"] = UserSelectedRec;
            HttpContext.Current.Session["InsertData"] = "Insert";
            return UserSelectedRec;
        }

        private void FillCustomerDeatilGrid()
        {
            try
            {
                iUserCreationClient userClient = new iUserCreationClient();
                CustomProfile profile = CustomProfile.GetProfile();
                //GvAccount.DataSource = AccountClient.GetGetCustomerDetail(profile.Personal.UserID,profile.DBConnection._constr);//UserID added by vishal old
                GvAccount.DataSource = userClient.GWCSearchUserList(CompanyID, DeptId, profile.DBConnection._constr);

                //Change by Suresh For GCC Admin Invoiceing
                iCompanySetupClient Cmpny = new iCompanySetupClient();
               // GvAccount.DataSource = Cmpny.GetInstituteDetails(profile.DBConnection._constr);

                if (GvAccount.DataSource != null) GvAccount.DataBind();
                userClient.Close();
            }
            catch (System.Exception ex)
            {
               // Login.Profile.ErrorHandling(ex, this, "AccountSearch.aspx", "FillCustomerDeatilGrid");
            }
            finally
            {
            }
        }


        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lbluserlist.Text = rm.GetString("UserList", ci);
            btnSubmitAccountSearch.Value = rm.GetString("Submit", ci);
            Button1.Value = rm.GetString("Submit", ci);
        }

    }
}