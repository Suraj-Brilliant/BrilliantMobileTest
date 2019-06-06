using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.PORServicePartRequest;

namespace BrilliantWMS.PowerOnRent
{
    public partial class Default : System.Web.UI.Page
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
           

            if (Request.QueryString["invoker"].ToString() == "Request")
            {

                h4DivHead.InnerText = " Request";
                UCFormHeader1.FormHeaderText = "Request";
              //  iframePOR.Attributes.Add("src", "../PowerOnRent/GridRequestSummary.aspx?FillBy=UserID");
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridRequestSummary.aspx?FillBy=UserID&Invoker=Request");
                Toolbar1.SetUserRights("MaterialRequest", "Summary", "");
                btnDriver.Visible = false;
                btnCancelOrder.Visible = true;
            }
            else if (Request.QueryString["invoker"].ToString() == "Approval")
            {
                h4DivHead.InnerText = " Approvals ";
                UCFormHeader1.FormHeaderText = "Approvals";
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridRequestSummary.aspx?FillBy=UserID&Invoker=Request");
                Toolbar1.SetUserRights("MaterialRequest", "Summary", "");
                Toolbar1.SetAddNewRight(false, "Click on pending Approved record [Red box] to Add New / Edit Issue");
                btnDriver.Visible = false;
                btnCancelOrder.Visible = false;
            }
            else if (Request.QueryString["invoker"].ToString() == "Issue")
            {
                h4DivHead.InnerText = " Dispatch";
                UCFormHeader1.FormHeaderText = "Dispatch";
                //iframePOR.Attributes.Add("src", "../PowerOnRent/GridIssueSummary.aspx?FillBy=UserID");
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridRequestSummary.aspx?FillBy=UserID&Invoker=Issue");
                Toolbar1.SetUserRights("MaterialIssue", "Summary", "");
                Toolbar1.SetAddNewRight(false, "Click on pending Issue record [Red box] to Add New / Edit Issue");
                btnDriver.Visible = true;
                btnCancelOrder.Visible = false;
            }
            else if (Request.QueryString["invoker"].ToString() == "Receipt")
            {
                h4DivHead.InnerText = "List of Material Receipts";
                UCFormHeader1.FormHeaderText = "Material Receipts";
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridReceiptSummary.aspx?FillBy=UserID");
                Toolbar1.SetUserRights("MaterialReceipt", "Summary", "");
                Toolbar1.SetAddNewRight(false, "Click on pending Receipt record [Red box] to Add New / Edit Receipt");
            }
            else if (Request.QueryString["invoker"].ToString() == "Consumption")
            {
                h4DivHead.InnerText = "List of Consumption";
                UCFormHeader1.FormHeaderText = "Consumption";
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridConsumptionSummary.aspx?FillBy=UserID");
                Toolbar1.SetUserRights("Consumption", "Summary", "");
            }
            else if (Request.QueryString["invoker"].ToString() == "HQReceipt")
            {
                h4DivHead.InnerText = "List of Goods Receipts [HQ]";
                UCFormHeader1.FormHeaderText = "Goods Receipts [HQ]";
                iframePOR.Attributes.Add("src", "../PowerOnRent/GridHQReceiptSummary.aspx?FillBy=UserID");
                Toolbar1.SetUserRights("GoodsReceipt", "Summary", "");
            }

            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            if (!IsPostBack)
            {
               
                //Button btnExport = (Button)Toolbar1.FindControl("btnExport");
                //btnExport.Visible = false;
                //Button btnImport = (Button)Toolbar1.FindControl("btnImport");
                //btnImport.Visible = false;
                //Button btmMail = (Button)Toolbar1.FindControl("btmMail");
                //btmMail.Visible = false;
                //Button btnPrint = (Button)Toolbar1.FindControl("btnPrint");
                //btnPrint.Visible = false;
            }
        }

        [WebMethod]
        public static string WMSetSessionAddNew(string ObjectName, string state)
        {
            HttpContext.Current.Session["PORstate"] = state;
            switch (ObjectName)
            {
                case "Request":
                    HttpContext.Current.Session["PORRequestID"] = 0;
                    HttpContext.Current.Session["TemplateID"] = "0"; 
                    break;
                case "Issue":
                    HttpContext.Current.Session["PORIssueID"] = 0;
                    break;
                case "Receipt":
                    HttpContext.Current.Session["PORReceiptID"] = 0;
                    break;
                case "Consumption":
                    HttpContext.Current.Session["PORConsumptionID"] = 0;
                    break;
                case "HQReceipt":
                    HttpContext.Current.Session["PORHQReceiptID"] = 0;
                    break;
            }

            return ObjectName;
        }

        [WebMethod]
        public static string WMSetSessionRequest(string ObjectName, long RequestID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PORRequestID"] = RequestID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Request":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialRequest", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Approval":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialRequest", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Issue":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialIssue", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Receipt":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialReceipt", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Consumption":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Consumption", profile.Personal.UserID, profile.DBConnection._constr);
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

        [WebMethod]
        public static string WMSetSessionIssue(string ObjectName, long IssueID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PORIssueID"] = IssueID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Issue":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialIssue", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Receipt":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialReceipt", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Consumption":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Consumption", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
            }
            if (checkRole.Add == false && checkRole.View == false)
            {
                ObjectName = "AccessDenied";
            }
            return ObjectName;
        }

        [WebMethod]
        public static string WMSetSessionReceipt(string ObjectName, long ReceiptID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PORReceiptID"] = ReceiptID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Receipt":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("MaterialReceipt", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "Consumption":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Consumption", profile.Personal.UserID, profile.DBConnection._constr);
                    HttpContext.Current.Session["PORConsumptionID"] = null;
                    break;
            }
            if (checkRole.Add == false && checkRole.View == false)
            {
                ObjectName = "AccessDenied";
            }
            return ObjectName;
        }

        [WebMethod]
        public static string WMSetSessionConsumption(string ObjectName, long ConsumptionID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PORConsumptionID"] = ConsumptionID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "Consumption":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("Consumption", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
            }
            if (checkRole.Add == false && checkRole.View == false)
            {
                ObjectName = "AccessDenied";
            }
            return ObjectName;
        }

        [WebMethod]
        public static string WMSetSessionHQReceipt(string ObjectName, long ReceiptID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PORHQReceiptID"] = ReceiptID;
            HttpContext.Current.Session["PORstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "HQReceipt":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GoodsReceipt", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
            }
            if (checkRole.Add == false && checkRole.View == false)
            {
                ObjectName = "AccessDenied";
            }
            return ObjectName;
        }

        static void ClearSession()
        {
            HttpContext.Current.Session["PORRequestID"] = null;
            HttpContext.Current.Session["PORIssueID"] = null;
            HttpContext.Current.Session["PORReceiptID"] = null;
            HttpContext.Current.Session["PORConsumptionID"] = null;
            HttpContext.Current.Session["PORHQReceiptID"] = null;
            HttpContext.Current.Session["PORstate"] = null;
        }

        [WebMethod]
        public static int WMChkDispatchedOrder(string SelectedOrder)
        {
            iPartRequestClient objServie = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            int result = objServie.GetDispatchedOrders(SelectedOrder, profile.DBConnection._constr);
            return result;
        }
        [WebMethod]
        public static int WMCancelOrder(long SelectedOrder)
        {
            iPartRequestClient objServie = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long UserID = profile.Personal.UserID;
            int result = objServie.CancelSelectedOrder(SelectedOrder, UserID, profile.DBConnection._constr);
            return result;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            //rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblHeading.Text = rm.GetString("NotApplicable", ci);
            lblCompleted.Text = rm.GetString("Completed", ci);
            if (Request.QueryString["invoker"].ToString() == "Request")
            {
                UCFormHeader1.FormHeaderText = rm.GetString("Request", ci);
                h4DivHead.InnerText = rm.GetString("Request", ci);
                btnCancelOrder.Value = rm.GetString("CancelOrder", ci);

            }
            else if (Request.QueryString["invoker"].ToString() == "Approval")
            {
                UCFormHeader1.FormHeaderText = rm.GetString("Approval", ci);
                h4DivHead.InnerText = rm.GetString("Approval", ci);
            }
            else if (Request.QueryString["invoker"].ToString() == "Issue")
            {
                UCFormHeader1.FormHeaderText = rm.GetString("Dispatch", ci);
                h4DivHead.InnerText = rm.GetString("Dispatch", ci);
                btnDriver.Value = rm.GetString("AllocateDriver", ci);
            }

            lblPending.Text = rm.GetString("Pending", ci);
            lblCancelled.Text = rm.GetString("Cancelled", ci);
            

        }
    }
}
