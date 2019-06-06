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

namespace BrilliantWMS.WMS
{
    public partial class Inbound : System.Web.UI.Page
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
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                string role = profile.Personal.UserType.ToString();
                if (role == "Super Admin" || role == "Admin" || role == "Warehouse Admin")
                {
                    long userCompany = profile.Personal.CompanyID;
                    ds = Inbound.BindInboundGrid(userCompany,profile.DBConnection._constr);
                }
                else
                {
                    ds = Inbound.BindInboundGridbyUser(profile.Personal.UserID,profile.DBConnection._constr);
                }
                /*Temparary Change*/
                if (profile.Personal.CompanyID == 10237)
                {
                    hdnUsrCompany.Value = "1";
                    grdPurchaseOrder.Columns[8].Visible = false;
                    grdPurchaseOrder.Columns[11].Visible = false;
                }
                /*Temparary Change*/
                grdPurchaseOrder.DataSource = ds;
                grdPurchaseOrder.DataBind();

                grdPurchaseOrder.AllowMultiRecordSelection = true;
                grdPurchaseOrder.AllowRecordSelection = true;
            }
            catch { }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static string WMSetSessionAddNew( string state)
        {
            HttpContext.Current.Session["POstate"] = state;
            //switch (ObjectName)
            //{
            //    case "Request":
                    HttpContext.Current.Session["POID"] = 0;
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
        public static string WMSetSessionRequest(string ObjectName, long RequestID, string state,string Type)
        {
            ClearSession();
            
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "PurchaseOrder":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PurchaseOrder", profile.Personal.UserID, profile.DBConnection._constr); 
                    HttpContext.Current.Session["POID"] = RequestID;
                        HttpContext.Current.Session["POstate"] = state;
                    break;
                case "GRN":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GRN", profile.Personal.UserID, profile.DBConnection._constr);
                    if (Type == "PurchaseOrder")
                    {
                        HttpContext.Current.Session["POID"] = RequestID;
                        HttpContext.Current.Session["POstate"] = state;
                    }
                    else if (Type == "SalesReturn")
                    {
                        HttpContext.Current.Session["SOID"] = RequestID;
                        HttpContext.Current.Session["SOstate"] = state;
                    }
                    break;
                case "QC":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("QC", profile.Personal.UserID, profile.DBConnection._constr);
                    if (Type == "PurchaseOrder")
                    {
                        HttpContext.Current.Session["POID"] = RequestID;
                        HttpContext.Current.Session["POstate"] = state;
                    }
                    else if (Type == "SalesReturn")
                    {
                        HttpContext.Current.Session["SOID"] = RequestID;
                        HttpContext.Current.Session["SOstate"] = state;
                    }
                    break;
                case "LabelPrinting":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("LabelPrinting", profile.Personal.UserID, profile.DBConnection._constr);
                    if (Type == "PurchaseOrder")
                    {
                        HttpContext.Current.Session["POID"] = RequestID;
                        HttpContext.Current.Session["POstate"] = state;
                    }
                    else if (Type == "SalesReturn")
                    {
                        HttpContext.Current.Session["SOID"] = RequestID;
                        HttpContext.Current.Session["SOstate"] = state;
                    }
                    break;
                case "PutIn":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("PutIn", profile.Personal.UserID, profile.DBConnection._constr);
                    if (Type == "PurchaseOrder")
                    {
                        HttpContext.Current.Session["POID"] = RequestID;
                        HttpContext.Current.Session["POstate"] = state;
                    }
                    else if (Type == "SalesReturn")
                    {
                        HttpContext.Current.Session["SOID"] = RequestID;
                        HttpContext.Current.Session["SOstate"] = state;
                    }
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
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;
            
        }

        [WebMethod]
        public static int WMCheckStatus(string SelectedPO)
        {
            int Result = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bool exeresult = Inbound.CheckJobCardofSelectedRecord(SelectedPO, "PurchaseOrder",profile.DBConnection._constr);
                if (exeresult == true)
                {
                    Result = Inbound.CheckSelectedPOStatusIsSameOrNot(SelectedPO, profile.DBConnection._constr);
                    Page objp = new Page();
                    objp.Session["SelectedRec"] = SelectedPO; objp.Session["ObjectName"] = "PurchaseOrder";
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

        [WebMethod]
        public static int WMCancelOrder(long SelectedOrder)
        {
            iInboundClient Inbound = new iInboundClient();           
            CustomProfile profile = CustomProfile.GetProfile();
            long UserID = profile.Personal.UserID;
            int result = Inbound.CancelSelectedOrder(SelectedOrder, UserID, profile.DBConnection._constr);
            return result;
        }
    }
}