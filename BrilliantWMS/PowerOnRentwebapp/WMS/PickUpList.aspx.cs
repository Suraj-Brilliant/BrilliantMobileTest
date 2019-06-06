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
using BrilliantWMS.ToolbarService;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.Warehouse
{
    public partial class PickUpList : System.Web.UI.Page
    {
        int dsvalue;
        DataTable dt;
        DataSet ds = new DataSet();
        static string ObjectName = "RequestPartDetail";

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Sales Orders";
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

        protected void BindSOGrid(object sender, EventArgs e)
        {
            iOutboundClient Outbound = new iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                long CompanyID = profile.Personal.CompanyID;
                ds = Outbound.BindPickUpGrid(CompanyID,profile.DBConnection._constr);
                grdPickUp.DataSource = ds;
                grdPickUp.DataBind();
            }
            catch { }
            finally { Outbound.Close(); }

            //iSalesOrderClient Sales = new iSalesOrderClient();
            //try
            //{
            //    ds.Reset();
            //    ds = Sales.ShowSalesOrder();
            //    dt = ds.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        Grid1.DataSource = ds;
            //        Grid1.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    Sales.Close();
            //}
        }

        [WebMethod]
        public static string WMSetSessionRequest(string ObjectName, long RequestID, string state)
        {
            ClearSession();
            HttpContext.Current.Session["PKUPID"] = RequestID;
            HttpContext.Current.Session["PKUPstate"] = state;
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
            //HttpContext.Current.Session["PORIssueID"] = null;
            //HttpContext.Current.Session["PORReceiptID"] = null;
            //HttpContext.Current.Session["PORConsumptionID"] = null;
            //HttpContext.Current.Session["PORHQReceiptID"] = null;
        }

        #region Toolbar Code
        [WebMethod]
        public static void WMpageAddNew()
        {
            //iPartRequestClient objService = new iPartRequestClient();
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    HttpContext.Current.Session["PORRequestID"] = "0";
            //    HttpContext.Current.Session["PORstate"] = "Add";
            //    objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            //}
            //catch { }
            //finally { objService.Close(); }
        }
        #endregion

        #region Toolbar Code
        //[WebMethod]
        //public static void WMpageAddNew()
        //{
        //    iPartRequestClient objService = new iPartRequestClient();
        //    try
        //    {
        //        CustomProfile profile = CustomProfile.GetProfile();
        //        HttpContext.Current.Session["PORRequestID"] = "0";
        //        HttpContext.Current.Session["PORstate"] = "Add";
        //        objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
        //    }
        //    catch { }
        //    finally { objService.Close(); }
        //}
        #endregion

        [WebMethod]
        public static string WMGetSOData(string SelectedSo)
        {
            //iSalesOrderClient Sales = new iSalesOrderClient();
            DataSet ds1 = new DataSet();

            //ds1 = Sales.ShowSOCountEnteredStatus(SelectedSo);
            if (Convert.ToInt32(ds1.Tables[0].Rows[0]["NotEntered"].ToString()) > 0)
            {

                return "1";
            }
            else
            {
                return SelectedSo;
            }
        }

        # region Code By Pallavi
        [WebMethod]
        public static string GetAllSOsOfGroup(string SelectedSOs)
        {
            string AllSos = "";
           // iSalesOrderClient Sales = new iSalesOrderClient();
            try
            {
               // AllSos = Sales.GetAllSos(SelectedSOs);
            }
            catch
            {
            }
            finally
            {
                //Sales.Close();
            }
            return AllSos;
        }

        [System.Web.Services.WebMethod]
        public static string WMChangeSOStatus(string SelectedSOs)
        {
            //iSalesOrderClient Sales = new iSalesOrderClient();
            int result = 0;
            try
            {
                //result = Sales.ValidateSOStatus(SelectedSOs);
            }
            catch
            {
            }
            finally
            {
                //Sales.Close();
            }
            return result.ToString();
        }
        //ValidateGreenPOStatus

        [System.Web.Services.WebMethod]
        public static string ValidateGreenSOStatus(string SelectedSOs)
        {
            int result = 1;
            //iSalesOrderClient Sales = new iSalesOrderClient();
            
            //try
            //{
            //    //  result = Sales.ValidateGreenSOStatus(SelectedSOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Sales.Close();
            //}
            return result.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string WMChangePOStatusNow(string SelectedSOs)
        {
            int result = 0;
            //iSalesOrderClient Sales = new iSalesOrderClient();
            
            //try
            //{
            //    result = Sales.ChangeSOStatus(SelectedSOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Sales.Close();
            //}
            return result.ToString();
        }
        #endregion
    }
}