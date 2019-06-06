using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;

namespace BrilliantWMS.WMS
{
    public partial class LabelPrinting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
            Toolbar1.SetUserRights("MaterialRequest", "Summary", "");

            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
        }

        public void BindGrid()
        {
            iInboundClient Inbound = new iInboundClient();            
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                long CompanyID = profile.Personal.CompanyID;
                long GRNID=long.Parse(Request.QueryString["ID"].ToString());
                if (GRNID == 0)
                {
                    ds = Inbound.BindQCGrid(CompanyID, profile.DBConnection._constr);
                }
                else
                {
                    ds = Inbound.BindQCGridofSelectedGRN(GRNID, profile.DBConnection._constr);
                }
                grdLabelPrint.DataSource = ds;
                grdLabelPrint.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static int WMShowReport(string SelectedQC)
        {
            int result = 0;
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Inbound.SetSelectedRecordToLabelPrintingStatus(SelectedQC, profile.DBConnection._constr);

                ds = Inbound.GetProductDetails(SelectedQC,profile.DBConnection._constr);
                ds.Tables[0].TableName = "DataSet1";
                HttpContext.Current.Session["ReportDS"] = ds;
                HttpContext.Current.Session["SelObject"] = "LabelPrinting";
                result = Convert.ToInt16(ds.Tables[0].Rows.Count);
            }
            catch { }
            finally { Inbound.Close(); }
            return result;
        }

        [WebMethod]
        public static int WMCheckStatus(string SelectedQC)
        {
            int Result = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bool exeresult = Inbound.CheckJobCardofSelectedRecord(SelectedQC, "QC", profile.DBConnection._constr);
                if (exeresult == true)
                {
                    Result = Inbound.CheckSelectedQCStatusIsSameOrNot(SelectedQC, profile.DBConnection._constr);
                    Page objp = new Page();
                    objp.Session["SelectedRec"] = SelectedQC; objp.Session["ObjectName"] = "QC";
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

        [WebMethod]
        public static string WMSetSessionQC(string ObjectName, long QCID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["QCID"] = QCID;
            HttpContext.Current.Session["QCstate"] = state;
            iUCToolbarClient objService = new iUCToolbarClient();
            mUserRolesDetail checkRole = new mUserRolesDetail();
            CustomProfile profile = CustomProfile.GetProfile();
            switch (ObjectName)
            {
                case "GRN":
                    checkRole = objService.GetUserRightsBy_ObjectNameUserID("GRN", profile.Personal.UserID, profile.DBConnection._constr);
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
            HttpContext.Current.Session["POID"] = null;
            HttpContext.Current.Session["GRNID"] = null;
            HttpContext.Current.Session["GRNstate"] = null;
            HttpContext.Current.Session["QCID"] = null;
            HttpContext.Current.Session["QCstate"] = null;            
            HttpContext.Current.Session["POstate"] = null;
        }
    }
}