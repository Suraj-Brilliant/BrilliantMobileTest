using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.DocumentService;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.ProductMasterService;

namespace BrilliantWMS.WMS
{
    public partial class WToWTransferDetail : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string state = "";
        static string ObjectName = "Transfer";

        #region PageEvents

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fillWarehouse();
                if (Session["TRID"] != null)
                {
                    if (Session["TRID"].ToString() != "0" && Session["TRstate"].ToString() == "View")
                    {
                        state = "View";
                        GetTransferDetails(Session["TRID"].ToString());
                    }
                    else if (Session["TRID"].ToString() == "0" && Session["TRstate"].ToString() == "Add")
                    {
                        state = "Add";
                        fillUser();
                        WMpageAddNew();
                        ddlWTWStatus.DataSource = WMFillStatus();
                        ddlWTWStatus.DataBind();
                        UCWTWTransferDate.Date = DateTime.Now;
                        UC_ShippingDate.Date = DateTime.Now;
                        UCExpDeliveryDate.Date = DateTime.Now;
                    }
                }
            }
            Toolbar1.SetAddNewRight(false, "Not Allowed");
            Toolbar1.SetEditRight(false, "Not Allowed");
            Toolbar1.SetSaveRight(true, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        [WebMethod]
        public static long WMSaveTransferHead(object objTRHead)
        {
            long result = 0;
            int RSLT = 0; long TRID = 0;
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSOutbound.tTransferHead trH = new WMSOutbound.tTransferHead();  
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objTRHead;
                if (HttpContext.Current.Session["TRID"] != null)
                {
                    if (HttpContext.Current.Session["TRID"].ToString() == "0")
                    {
                        trH.CreatedBy= profile.Personal.UserID;
                        trH.CreationDate = DateTime.Now;
                    }
                    else
                    {
                        trH.ID = Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString());
                        trH.Modifiedby = profile.Personal.UserID;
                        trH.ModiedDate= DateTime.Now;
                    }
                    trH.Type = 2;
                    trH.TransferDate = Convert.ToDateTime(d["TransferDate"]);
                    trH.FromPosition = Convert.ToInt64(d["FromPosition"]);
                    trH.ToPosition = Convert.ToInt64(d["ToPosition"]);
                    trH.TransferBy = Convert.ToInt64(d["TransferBy"]);
                    trH.Status = Convert.ToInt64(d["Status"]);
                    trH.Remark = d["Remark"].ToString();
                    trH.CompanyID = profile.Personal.CompanyID;

                    trH.AirwayBill = d["AirwayBill"].ToString();
                    trH.ShippingType = d["ShippingType"].ToString();
                    trH.TransporterName = d["TransporterName"].ToString();
                    trH.ShippingDate = Convert.ToDateTime(d["ShippingDate"]);
                    trH.ExpDeliveryDate = Convert.ToDateTime(d["ExpDeliveryDate"]);
                    trH.ObjectName = "Transfer";

                    TRID = Outbound.SaveIntotTransferHead(trH, profile.DBConnection._constr);
                    if (TRID > 0)
                    {
                        RSLT = Outbound.FinalSaveTRDetail(HttpContext.Current.Session.SessionID, ObjectName, TRID, profile.Personal.UserID.ToString(), Convert.ToInt16(trH.Status), 0, profile.DBConnection._constr);
                        if (RSLT == 1 || RSLT == 2) { result = TRID; }  //"Request saved successfully";
                        else if (RSLT == 3) { result = -3; }
                    }

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "WMSaveRequestHead");
                result = 0; // "Some error occurred";
            }
            finally
            {
                Outbound.Close();               
            }
            return result;
        }

        public void GetTransferDetails(string TransferID)
        {
            long TRID =long.Parse(TransferID);
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                BrilliantWMS.WMSOutbound.tTransferHead trh = new WMSOutbound.tTransferHead();  //= new tTransferHead();
                trh = Outbound.GetTransferHeadDetailByTransferID(TRID, profile.DBConnection._constr);

                lblWTWTransferNo.Text = trh.ID.ToString();
                UCWTWTransferDate.Date = trh.TransferDate;

                fillUser();
                ddlWTWTransferBy.SelectedIndex = ddlWTWTransferBy.Items.IndexOf(ddlWTWTransferBy.Items.FindByValue(trh.TransferBy.ToString()));

                ddlWTWStatus.DataSource = WMFillStatus();
                ddlWTWStatus.DataBind();
                ddlWTWStatus.SelectedIndex = ddlWTWStatus.Items.IndexOf(ddlWTWStatus.Items.FindByValue(trh.Status.ToString()));

                fillWarehouse();
                ddlFWarehouse.SelectedIndex = ddlFWarehouse.Items.IndexOf(ddlFWarehouse.Items.FindByValue(trh.FromPosition.ToString()));

                List<BrilliantWMS.WMSInbound.mWarehouseMaster> ToWarehouseList = new List<WMSInbound.mWarehouseMaster>();
                long UserID = profile.Personal.UserID;
                ToWarehouseList = Inbound.GetUserWarehouse(UserID, profile.DBConnection._constr).ToList();
                ddlTWarehouse.DataSource = ToWarehouseList;
                ddlTWarehouse.DataBind();
                ddlTWarehouse.SelectedIndex = ddlTWarehouse.Items.IndexOf(ddlTWarehouse.Items.FindByValue(trh.ToPosition.ToString()));

                WTWTRemark.Text = trh.Remark.ToString();
                txtAirwayBill.Text = trh.AirwayBill.ToString();
                txtShippingType.Text = trh.ShippingType.ToString();
                UC_ShippingDate.Date = trh.ShippingDate;
                UCExpDeliveryDate.Date = trh.ExpDeliveryDate;
                txtTransporterName.Text = trh.TransporterName.ToString();

                Grid1.DataSource = Outbound.GetTransferPartDetailByTransferID(TRID, long.Parse(trh.FromPosition.ToString()), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                Grid1.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeRequest" + Session.SessionID, "changemode(true, 'divWTWTransferDetail');LoadingOff();", true);
            }
            catch { }
            finally { Outbound.Close(); objService.Close(); UsersList.Clear(); }
        }
        #endregion

        #region BindDropdown
        protected void fillWarehouse()
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<BrilliantWMS.WMSInbound.mWarehouseMaster> WarehouseList = new List<WMSInbound.mWarehouseMaster>();
                long UserID = profile.Personal.UserID;
                WarehouseList = Inbound.GetUserWarehouse(UserID, profile.DBConnection._constr).ToList();
                ddlFWarehouse.DataSource = WarehouseList;
                ddlFWarehouse.DataBind();
                ListItem lstW = new ListItem { Text = "-Select-", Value = "0" };
                ddlFWarehouse.Items.Insert(0, lstW);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "fillWarehouse");
            }
            finally { Inbound.Close(); }
        }

        public void fillUser()
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<SP_GWC_GetUserInfo_Result> Usrlst = new List<SP_GWC_GetUserInfo_Result>();
                Usrlst = objService.GetUsrLst1("", "", profile.DBConnection._constr).ToList();
                Usrlst = Usrlst.Where(u => u.ID == profile.Personal.UserID).ToList();
                ddlWTWTransferBy.DataSource = Usrlst;
                ddlWTWTransferBy.DataBind();
                ListItem lstUsr = new ListItem { Text = "-Select-", Value = "0" };
                ddlWTWTransferBy.Items.Insert(0, lstUsr);
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static List<BrilliantWMS.WMSInbound.mStatu> WMFillStatus()
        {
            string state = HttpContext.Current.Session["TRstate"].ToString();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            List<BrilliantWMS.WMSInbound.mStatu> StatusList = new List<BrilliantWMS.WMSInbound.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (HttpContext.Current.Session["TRID"].ToString() == "0" && state == "Add")
                {
                    StatusList = Inbound.GetStatusListForInbound(ObjectName, "", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                }
                else if (HttpContext.Current.Session["TRID"].ToString() != "0" && state == "View")
                {
                    StatusList = Inbound.GetStatusListForInbound("", "Transfer", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
                }
                BrilliantWMS.WMSInbound.mStatu select = new BrilliantWMS.WMSInbound.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "WMFillStatus");
            }
            finally { Inbound.Close(); }
            return StatusList;
        }

        [WebMethod]
        public static List<BrilliantWMS.WMSInbound.mWarehouseMaster> WMGetToWarehouse(long FrmWarehouse)
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();

            CustomProfile profile = CustomProfile.GetProfile();
            List<BrilliantWMS.WMSInbound.mWarehouseMaster> WarehouseLst = new List<WMSInbound.mWarehouseMaster>();
            long UserID = profile.Personal.UserID;
            WarehouseLst = Inbound.GetUserWarehouse(UserID, profile.DBConnection._constr).ToList();
            WarehouseLst = WarehouseLst.Where(w => w.ID != FrmWarehouse).ToList();

            Page objp = new Page();
            objp.Session["WarehouseID"] = FrmWarehouse; objp.Session["DeptID"] = null;

            return WarehouseLst;
        }
        #endregion

        #region ToolbarCode
        [WebMethod]
        public static void WMpageAddNew()
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["TRID"] = 0;
                HttpContext.Current.Session["TRstate"] = "Add";
                Inbound.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "WMpageAddNew");
            }
            finally { Inbound.Close(); }
        }
        #endregion

        public static List<WMSInbound.mWarehouseMaster> WarehouseLst { get; set; }

        #region TransferProduct
        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearchWTW.FindControl("hdnProductSearchSelectedRec");
                List<BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForTransfer_Result> TRPartList = new List<WMSOutbound.WMS_SP_GetPartDetail_ForTransfer_Result>();
                if (hdn.Value == "")
                {
                    TRPartList = Outbound.GetExistingTempDataBySessionIDObjectNameTR(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    TRPartList = Outbound.AddPartIntoTransfer_TempDataTR(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(hdnFrmWarehouse.Value), profile.DBConnection._constr).ToList();
                }
                Grid1.DataSource = TRPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "WToWTransferDetail.aspx", "Grid1_OnRebind"); }
            finally { Outbound.Close(); }
        }

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                    Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[7] as Obout.Grid.GridDataControlFieldCell;
                    DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;
                    HiddenField hdnUOM = cell.FindControl("hdnMyUOM") as HiddenField;
                    Label rowQtySpn = e.Row.Cells[9].FindControl("rowQtyTotal") as Label;
                    TextBox txtUsrQty = e.Row.Cells[6].FindControl("txtUsrQty") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);
                    decimal CrntStock = Convert.ToDecimal(e.Row.Cells[5].Text);

                    //TextBox txtReturnQty = e.Row.Cells[10].FindControl("txtReturnQty") as TextBox;
                    DataSet dsUOM = new DataSet();
                    dsUOM = Inbound.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);

                    ddl.DataSource = dsUOM;
                    ddl.DataTextField = "Description";
                    ddl.DataValueField = "UMOGroup";
                    ddl.DataBind();
                    if (Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()) > 0)
                    {
                        //txtUsrQty.Text = Convert.ToString(UserQty / SelectedQty);
                        UCProductSearchWTW.Visible = false;
                        txtUsrQty.Enabled = false;
                        ddl.Enabled = false;
                    }
                    else
                    {
                        ddl.SelectedIndex = 0;

                        decimal SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[0]["Quantity"].ToString());
                        decimal SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[0]["UOMID"].ToString());

                        decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                        decimal UsrQty = SelectedQty * rowQty;

                        hdnSelectedQty.Value = SelectedQty.ToString();
                        rowQtySpn.Text = UsrQty.ToString();

                        if (UsrQty > CrntStock)
                        { rowQtySpn.Text = "0"; }
                        else
                        {
                            rowQtySpn.Text = UsrQty.ToString();
                            //Price = decimal.Parse(rowQtySpn.Text.ToString()) * decimal.Parse(txtUsrPrice.Text.ToString());
                            //rowPriceTotal.Text = Price.ToString();

                        }
                        ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ")");
                        txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + CrntStock + "," + e.Row.RowIndex + ")");
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, this, "WToWTransferDetail.aspx", "Grid1_OnRowDataBound"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static int WMRemovePartFromRequest(Int32 Sequence)
        {
            int editOrder = 0;
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();            
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                Outbound.RemovePartFromTransfer_TempDataTR(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
                editOrder = 1;

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "WMRemovePartFromRequest");
            }
            finally { Outbound.Close(); }
            return editOrder;
        }

        [WebMethod]
        public static void WMUpdTransferPart(object objTransfer)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objTransfer;
                CustomProfile profile = CustomProfile.GetProfile();

                string uom = Inbound.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);

                BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForTransfer_Result PartTransfer = new WMSOutbound.WMS_SP_GetPartDetail_ForTransfer_Result();

                PartTransfer.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartTransfer.Qty = Convert.ToDecimal(dictionary["Qty"]); //PartRequest.UOM = uom;
                PartTransfer.UOMID = Convert.ToInt64(dictionary["UOMID"]);

                Outbound.UpdateTransfer_TempDataTR(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartTransfer, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "WToWTransferDetail.aspx", "WMUpdRequestPart"); }
            finally { Outbound.Close(); }
        }
        #endregion
    }
}