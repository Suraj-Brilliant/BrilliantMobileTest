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
using BrilliantWMS.WMSInbound;
//using BrilliantWMS.NServicePurchaseOrder;


namespace BrilliantWMS.Warehouse
{
    public partial class PutInList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Purchase Orders";
            BindPOGrid(sender, e);
            if (!IsPostBack)
            {
                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");

                BindPOGrid(sender, e);
            }

            //Add By Suresh
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        protected void BindPOGrid(object sender, EventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();            
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds = Inbound.BindPIGrid(profile.DBConnection._constr);
                grdPutIn.DataSource = ds;
                grdPutIn.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }

            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            //try
            //{

            //    DataSet ds = new DataSet();
            //    ds.Reset();
            //    ds = Purchase.ShowPurchaseOrder();
            //    Grid1.DataSource = ds;
            //    Grid1.DataBind();
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
        }

        [WebMethod]
        public static int WMCheckStatus(string SelectedGRN)
        {
            int Result = 0;
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                bool exeresult = Inbound.CheckJobCardofSelectedRecord(SelectedGRN, "PutIn", profile.DBConnection._constr);
                if (exeresult == true)
                {
                    Result = Inbound.CheckSelectedPutInStatusIsSameOrNot(SelectedGRN, profile.DBConnection._constr);
                    Page objp = new Page();
                    objp.Session["SelectedRec"] = SelectedGRN; objp.Session["ObjectName"] = "PutIn";
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
        //protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        //{
        //    Response.Redirect("../POR/PurchaseOrderEntry.aspx");
        //}

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

        [WebMethod]
        public static string WMGetPOData(string SelectedPo)
        {

           // iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            DataSet ds = new DataSet();
            try
            {
               // ds = Purchase.ShowCountEnteredStatus(SelectedPo);           
            }
            catch { }
            finally
            { 
                //Purchase.Close(); 
            }

            if (Convert.ToInt32(ds.Tables[0].Rows[0]["NotEntered"].ToString()) > 0)
            {
                return "1";
            }
            else
            {
                return SelectedPo;
            }

        }

        # region Code By Pallavi
        [WebMethod]
        public static string GetAllPOsOfGroup(string SelectedPOs)
        {
            string AllPos = "";
            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            //try
            //{
            //    AllPos = Purchase.GetAllPos(SelectedPOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
            return AllPos;
        }

        [System.Web.Services.WebMethod]
        public static string WMChangePOStatus(string SelectedPOs)
        {
            int result = 0;
            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            
            //try
            //{
            //    result = Purchase.ValidatePOStatus(SelectedPOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
            return result.ToString();
        }
       

        [System.Web.Services.WebMethod]
        public static string ValidateGreenPOStatus(string SelectedPOs)
        {
            int result = 1;
            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            //try
            //{
            //    // result = Purchase.ValidateGreenPOStatus(SelectedPOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
            return result.ToString();
        }

        [System.Web.Services.WebMethod]
        public static string WMChangePOStatusNow(string SelectedPOs)
        {
            int result = 0;
            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            //try
            //{
            //    result = Purchase.ChangePOStatus(SelectedPOs);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
            return result.ToString();
        }
        #endregion
    }
}