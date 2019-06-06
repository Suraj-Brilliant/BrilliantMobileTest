using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;


namespace BrilliantWMS.WMS
{
    public partial class GridGRN : System.Web.UI.Page
    {
        long PreviousPOID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               PreviousPOID=long.Parse(Request.QueryString["POID"]);
                BindGrid();
                Toolbar1.SetUserRights("MaterialRequest", "Summary", "");
            }

            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        public void BindGrid()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                if (PreviousPOID!=0)
                {
                    if (Session["POID"] != null)
                    {
                        ds = Inbound.BindGRNGridofSelectedPO(long.Parse(Session["POID"].ToString()), profile.DBConnection._constr);
                    }
                    else if (Session["SOID"] != null)
                    {
                        ds = Inbound.BindGRNGridofSelectedPO(long.Parse(Session["SOID"].ToString()), profile.DBConnection._constr);
                    }
                }
                else
                {
                    string role = profile.Personal.UserType.ToString();
                    if (role == "Super Admin" || role == "Admin" || role == "Warehouse Admin")
                    {
                        long userCompany = profile.Personal.CompanyID;
                        ds = Inbound.BindGRNGrid(userCompany, profile.DBConnection._constr);
                    }
                    else
                    {
                        ds = Inbound.BindGRNGridUserWise(profile.Personal.UserID,profile.DBConnection._constr);
                    }
                }
                
                grdGRN.DataSource = ds;
                grdGRN.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static string WMSetSessionGRN(string ObjectName, long GRNID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["GRNID"] = GRNID;
            HttpContext.Current.Session["GRNstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "GRN":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GRN", profile.Personal.UserID, profile.DBConnection._constr);
                    break;
                case "QC":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("QC", profile.Personal.UserID, profile.DBConnection._constr);
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
            HttpContext.Current.Session["GRNID"] = null;
            HttpContext.Current.Session["GRNstate"] = null;
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;
            HttpContext.Current.Session["POstate"] = null;
        }

        [WebMethod]
        public static int WMCheckStatus(string SelectedGRN)
        {
            int Result = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bool exeresult = Inbound.CheckJobCardofSelectedRecord(SelectedGRN, "GRN", profile.DBConnection._constr);
                if (exeresult == true)
                {
                    Result = Inbound.CheckSelectedGRNStatusIsSameOrNot(SelectedGRN, profile.DBConnection._constr);
                    Page objp = new Page();
                    objp.Session["SelectedRec"] = SelectedGRN; objp.Session["ObjectName"] = "GRN";
                }
                else
                {
                    Result = 0;
                }
            }
            catch { }
            finally { Inbound.Close(); }
            return Result;
        }
    }
}