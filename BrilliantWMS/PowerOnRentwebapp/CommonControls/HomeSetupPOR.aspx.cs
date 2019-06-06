using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.CommonControls
{
    public partial class HomeSetupPOR : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //UCFormHeader1.FormHeaderText = "Setup";
            
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            showhidesetup();
        }

        private void showhidesetup()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            string type = profile.Personal.UserType.ToString();
            if (type == "Admin")
            {
                row1.Visible = false;
                row2.Visible = false;
                ctd4.Visible = false;
                ctd5.Visible = false;
                ctd6.Visible = false;
                ctd7.Visible = true;
            }
            else
            {
                ctd7.Visible = false;
            }
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lnkBtnCompanyMang.Text = rm.GetString("CustomerManagement", ci);
            lnkBtnUserMang0.Text = rm.GetString("UserManagement", ci);
            lblcustomermaster.Text = rm.GetString("CustomerMaster", ci);
           // lblrollmaster.Text = rm.GetString("RoleMaster", ci);
            lblusermaster.Text = rm.GetString("UserMaster", ci);
            lblapproval.Text = rm.GetString("ApprovalMaster", ci);
            lnkBtnProductMang.Text = rm.GetString("SKUManagement", ci);
            lnkbtninterface.Text = rm.GetString("InterfaceManagement", ci);
            lblsku.Text = rm.GetString("sku", ci);
            lblinterdef.Text = rm.GetString("InterfaceDefination", ci);
            lblmsgdef.Text = rm.GetString("MessageDefination", ci);
            lnkBtnActivityMang.Text = rm.GetString("Utility", ci);
            lblemailconfig.Text = rm.GetString("EmailConfiguration", ci);
            lblrequsttemp.Text = rm.GetString("RequestTemplate", ci);
            lblimageimport.Text = rm.GetString("ImageImport", ci);
            UCFormHeader1.FormHeaderText = rm.GetString("Setup", ci);

            lnkHelp.Text = rm.GetString("Help", ci);
            lblHelp.Text = rm.GetString("Help", ci);

            Label1.Text = rm.GetString("LocationMaster", ci);
            lblimportprice.Text = rm.GetString("ImportPrice", ci);
            Label2.Text = rm.GetString("DirectOrder", ci);
           
        }
    }
}