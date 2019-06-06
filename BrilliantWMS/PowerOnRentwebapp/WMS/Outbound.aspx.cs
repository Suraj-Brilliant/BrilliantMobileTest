using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.WMS
{
    public partial class Outbound : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bindgrid();
            Toolbar1.SetUserRights("MaterialRequest", "Summary", "");

            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(true, "Not Allowed");
        }

        public void bindgrid()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                string role = profile.Personal.UserType.ToString();
                if (role == "Super Admin" || role == "Admin" || role == "Warehouse Admin")
                {
                    long userCompany = profile.Personal.CompanyID;
                    ds = Outbound.BindOutboundGrid(userCompany,profile.DBConnection._constr);
                }
                else
                {
                    ds = Outbound.BindOutboundGridbyUser(profile.Personal.UserID, profile.DBConnection._constr);
                }
                grdSalesOrder.DataSource = ds;
                grdSalesOrder.DataBind();

                grdSalesOrder.AllowMultiRecordSelection = true;
                grdSalesOrder.AllowRecordSelection = true;
            }
            catch { }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static string WMSetSessionAddNew(string state)
        {
            HttpContext.Current.Session["SOstate"] = state;
            //switch (ObjectName)
            //{
            //    case "Request":
            HttpContext.Current.Session["SOID"] = 0;
            HttpContext.Current.Session["ClientID"] = "0";
            // HttpContext.Current.Session["TemplateID"] = "0";
            //        break;
            //    case "Issue":
            //        HttpContext.Current.Session["PORIssueID"] = 0;
            //        break;
            //    case "Receipt":
            //        HttpContext.Current.Session["PORReceiptID"] = 0;
            //        break;
            //    case "Consumption":
            //        HttpContext.Current.Session["PORConsumptionID"] = 0;
            //        break;
            //    case "HQReceipt":
            //        HttpContext.Current.Session["PORHQReceiptID"] = 0;
            //        break;
            //}

            return "";
        }

        [WebMethod]
        public static string WMSetSessionRequest(string ObjectName, long RequestID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["SOID"] = RequestID;
            HttpContext.Current.Session["SOstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "SalesOrder":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("SalesOrder", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "PickUp":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PickUp", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "QC":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("QC", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "LabelPrinting":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("LabelPrinting", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "PutIn":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PutIn", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Dispatch":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Dispatch", profile.Personal.UserID, profile.DBConnection._constr);
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
            HttpContext.Current.Session["SOID"] = null;
            HttpContext.Current.Session["SOstate"] = null;
            HttpContext.Current.Session["PKUPID"] = null;
            HttpContext.Current.Session["PKUPstate"] = null;
            HttpContext.Current.Session["ClientID"] = null;
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;
        }

        [WebMethod]
        public static int WMCheckStatus(string SelectedSO)
        {
            int Result = 0;            
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            //iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bool exeresult = Inbound.CheckJobCardofSelectedRecord(SelectedSO, "SalesOrder", profile.DBConnection._constr);
                if (exeresult == true)
                {
                    BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
                    Result = Outbound.CheckSelectedSOStatusIsSameOrNot(SelectedSO, profile.DBConnection._constr);
                    Page objp = new Page();
                    objp.Session["SelectedRec"] = SelectedSO; objp.Session["ObjectName"] = "SalesOrder";
                }
                else
                {
                    Result = 2;
                }
            }
            catch { }
            finally { Inbound.Close(); }
            return Result;
        }
    }
}