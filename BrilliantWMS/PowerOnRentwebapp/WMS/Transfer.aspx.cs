using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;
using BrilliantWMS.WMSInbound;
using System.Web.Services;
using BrilliantWMS.ToolbarService;

namespace BrilliantWMS.WMS
{
    public partial class Transfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
            Toolbar1.SetUserRights("MaterialRequest", "Summary", "");

            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(true, "Not Allowed");
        }

        public void BindGrid()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                long CompanyID = profile.Personal.CompanyID;
                ds = Inbound.BindTransferGrid(CompanyID,profile.DBConnection._constr);
                grdTransfer.DataSource = ds;
                grdTransfer.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }   
        }

        [WebMethod]
        public static string WMSetSessionAddNew(string state)
        {
            HttpContext.Current.Session["TRstate"] = state;
            HttpContext.Current.Session["TRID"] = 0;
            return "";
        }

        [WebMethod]
        public static string WMSetSessionRequest(string ObjectName, long RequestID, string state)
        {
            ClearSession();

            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            HttpContext.Current.Session["TRID"] = RequestID;
            HttpContext.Current.Session["TRstate"] = state;

            switch (ObjectName)
            {
                case "PickUp":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PickUp", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "GRN":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GRN", profile.Personal.UserID, profile.DBConnection._constr);                    
                    break;
                case "QCOut":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("QC", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "QCIn":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("QC", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Dispatch":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Dispatch", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "PutIn":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PutIn", profile.Personal.UserID, profile.DBConnection._constr);
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
            HttpContext.Current.Session["POID"] = null;
            HttpContext.Current.Session["POstate"] = null;
            HttpContext.Current.Session["GRNID"] = null;
            HttpContext.Current.Session["GRNstate"] = null;
            HttpContext.Current.Session["TRID"] = null;
            HttpContext.Current.Session["TRstate"] = null;
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;

        }
    }
}