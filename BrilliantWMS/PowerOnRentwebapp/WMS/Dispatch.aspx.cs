using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.ToolbarService;

namespace BrilliantWMS.WMS
{
    public partial class Dispatch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Dispatch";
            BindSOGrid(sender, e);
            if (!IsPostBack)
            {
                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");

                BindSOGrid(sender, e);
            }
            //dsvalue = int.Parse(hdndsvalue.Value.ToString());
            //Add By Suresh
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        public void BindSOGrid(object sender, EventArgs e)
        {
            iOutboundClient Outbound = new iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                long companyID = profile.Personal.CompanyID;
                ds = Outbound.BindDispatchGrid(companyID, profile.DBConnection._constr);
                grdDispatch.DataSource = ds;
                grdDispatch.DataBind();
            }
            catch { }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static string WMSetSessionDispatch(string ObjectName, long DispatchID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["DispID"] = DispatchID;
            HttpContext.Current.Session["Dispstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Dispatch":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Dispatch", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "QC":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GRN", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
            }
            if (checkRole.Add == false && checkRole.View == false)
            {
                ObjectName = "AccessDenied";
            }
            else if (ObjectName == "Approval" && checkRole.Approval == false)
            {
                ObjectName = "AccessDenied";
            }
            return ObjectName;
        }

        static void ClearSession()
        {
            HttpContext.Current.Session["DispID"] = null;
            HttpContext.Current.Session["Dispstate"] = null;
            HttpContext.Current.Session["QCID"] = null;
            HttpContext.Current.Session["QCstate"] = null;
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;
            HttpContext.Current.Session["POstate"] = null;
        }
    }
}