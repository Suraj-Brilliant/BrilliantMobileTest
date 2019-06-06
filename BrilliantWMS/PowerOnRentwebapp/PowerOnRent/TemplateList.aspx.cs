using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.PORServicePartRequest;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.PowerOnRent
{
    public partial class TemplateList : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == null)
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            GetTemplateList();
            loadstring();
        }

        protected void GetTemplateList()
        {
            //select * from VW_GetTemplateDetails    HttpContext.Current.Session["ReportDS"] = dsCmnRpt;
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVRequest.DataSource = null;
            GVRequest.DataBind();
            long DeptID = long.Parse(Session["DeptID"].ToString());
            DataSet dsTemplate = new DataSet();
            dsTemplate = objService.GetTemplateDetailsBind(profile.Personal.UserID, DeptID,profile.DBConnection._constr);
           // dsTemplate = objService.GetTemplateDetails(profile.Personal.UserID,  profile.DBConnection._constr);

            GVRequest.DataSource = dsTemplate;
            GVRequest.DataBind();

        }

        [WebMethod]
        public static string WMGetSelectedTemplateID(string hdnSelTemplateID)
        {
            HttpContext.Current.Session["TemplateID"] = hdnSelTemplateID;
            return hdnSelTemplateID;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            //rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblTemplateList.Text = rm.GetString("TemplateList", ci);
            btnSubmitProductSearch1.Value = rm.GetString("Submit", ci);
        }
    }
}