using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.Login;
using BrilliantWMS.ProductMasterService;
using System.Web.Services;

namespace BrilliantWMS.POR
{
    public partial class ToolTransfer : System.Web.UI.Page
    {
        static string sessionID;        
        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID.ToString();
            UCFormHeader1.FormHeaderText = "Asset Master";
            if (!IsPostBack)
            {
                Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                btnExport.Visible = false;
                Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                btnImport.Visible = false;
                Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                btmMail.Visible = false;
                Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                btnPrint.Visible = false;
                //Button btnConvertTo = (Button)UCToolbar1.FindControl("btnConvertTo");
                //btnConvertTo.Visible = false;
                               
                GetTransferList();
                setActiveTab(0);
                fillSite();
                //  ScriptManager.RegisterStartupScript(this.FlyoutChangeProdPrice, FlyoutChangeProdPrice.GetType(), "reg1", "SaveNewPrice();", false);
            }
            //this.UCToolbar1.ToolbarAccess("ProductMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
           // this.UCToolbar1.evClickImport += pageImport;           
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        public void fillSite()
        {
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlFrmSite.DataSource = UCCommonFilter.GetSiteNameByUserID_Transfer(profile.Personal.UserID, profile.DBConnection._constr);
            ddlFrmSite.DataBind();
            ListItem lstfrm = new ListItem();
            lstfrm.Text = "--Select--";
            lstfrm.Value = "0";
            ddlFrmSite.Items.Insert(0, lstfrm);

            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            UsersList = objService.GetUserListBySiteID(1, profile.DBConnection._constr).ToList();
            UsersList = UsersList.Distinct().ToList();
            vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
            UsersList.Insert(0, select);

            ddlTransferedBy.DataSource = null;
            ddlTransferedBy.DataBind();
            ddlTransferedBy.DataSource = UsersList;
            ddlTransferedBy.DataBind();
        }

        [WebMethod]
        public static List<mTerritory> WMGetFromSite(long FrmSiteID)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            SiteLst = UCCommonFilter.GetToSiteName_Transfer(FrmSiteID, profile.DBConnection._constr).ToList();

            return SiteLst;
        }

        protected void GVAssetList_OnRebind(object sender, EventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVAssetList.DataSource = null;
                GVAssetList.DataBind();
                var FrmSite = hdnSelectedFromSite.Value;
                if (FrmSite != "" && FrmSite != "0")
                {
                    if (hdnRemovePrd.Value == "0")
                    {
                        //GVAssetList.DataSource = productClient.GetSitewiseTool(Convert.ToInt64(FrmSite), profile.DBConnection._constr);
                        GVAssetList.DataSource = productClient.GetSitewiseTool(Convert.ToInt64(FrmSite), Session.SessionID,profile.Personal.UserID.ToString(),"AssetList",profile.DBConnection._constr);
                        GVAssetList.DataBind();
                    }
                    else
                    {
                        List<POR_SP_SiteWiseTools_Result> AssetLst = new List<POR_SP_SiteWiseTools_Result>();
                        AssetLst = productClient.GetExistingTempDataBySessionIDObjectNamePrd(Session.SessionID, profile.Personal.UserID.ToString(), "AssetList", profile.DBConnection._constr).ToList();

                        GVAssetList.DataSource = AssetLst;
                        GVAssetList.DataBind();
                    }
                }
            }
            catch (Exception ex) { Login.Profile.ErrorHandling(ex, "ToolTransfer", "GVAssetList"); }
            finally { productClient.Close(); }
        }

        [WebMethod]
        public static void WMRemovePartFromList(Int32 Sequence)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            //iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                CustomProfile profile = CustomProfile.GetProfile();
              //  objService.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), Sequence, profile.DBConnection._constr);   
                productClient.RemoveAssetFromCurrentAsset_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), "AssetList", Sequence, profile.DBConnection._constr);            
            }
            catch { }
            finally 
            { 
               productClient.Close(); 
            }
        }

        public void GetTransferList()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient astlst = new iProductMasterClient();
            List<POR_VW_ToolTransferDetails> transfrLst = new List<POR_VW_ToolTransferDetails>();
            transfrLst = astlst.GetTransferList(profile.DBConnection._constr).ToList();

            GvAssetTransfer.DataSource = transfrLst;
            GvAssetTransfer.DataBind();
        }

        protected void setActiveTab(int ActiveTab)
        {

            if (ActiveTab == 0)
            {
                TabPnlAssetTransferLst.Visible = true;
                tabTransferDetails.Visible = false;               
                TabConAssetTransfer.ActiveTabIndex = 0;
            }
            else
            {              
                tabTransferDetails.Visible = true;                
                TabConAssetTransfer.ActiveTabIndex = 1;
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            //clear();
            //GetProductSpecificationDetailByProductID();
            //GetProductTaxDetailByProductID();
            //GetProductImagesByProductID();
            //GVRateHistory();
            //FillInventoryGrid();
            setActiveTab(1);         
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                string state="";
                CustomProfile profile = CustomProfile.GetProfile();
                iProductMasterClient AssetClient = new iProductMasterClient();
                tToolTransferHead TransferHead = new tToolTransferHead();
                if (hdnprodID.Value != "0")
                {
                    state = "Edit";
                    TransferHead.Modifiedby = profile.Personal.UserID;
                    TransferHead.ModifiedDate = DateTime.Now;
                }
                else
                {
                    state = "AddNew";
                    TransferHead.CreatedBy = profile.Personal.UserID;
                    TransferHead.CreationDate = DateTime.Now;
                }
                                
                if (UCTransferDate.Date != null) { TransferHead.TransferDate = Convert.ToDateTime(UCTransferDate.Date); }
                                
                TransferHead.TransferedBy = null;
                if (ddlTransferedBy.SelectedIndex > 0) { TransferHead.TransferedBy = Convert.ToInt64(ddlTransferedBy.SelectedItem.Value); }

                TransferHead.Status = null;
                if (ddlStatus.SelectedIndex > 0) { TransferHead.Status = ddlStatus.SelectedItem.Value; }

                TransferHead.TransferFromSite = null;
                if (ddlFrmSite.SelectedIndex > 0) { TransferHead.TransferFromSite = Convert.ToInt64(ddlFrmSite.SelectedItem.Value); }

                TransferHead.TransferToSite = null;               
                if (hdnSelectedToSite.Value != "") { TransferHead.TransferToSite = Convert.ToInt64(hdnSelectedToSite.Value); }

                TransferHead.Airwaybill = null;
                if (txtAirwayBill.Text.ToString().Trim() != "") { TransferHead.Airwaybill = txtAirwayBill.Text.ToString(); }

                TransferHead.ShippingType = null;
                TransferHead.ShippingType = Convert.ToString(txtShippingType.Text);

                TransferHead.ShippingDate = null;
                if (UC_ShippingDate.Date != null) { TransferHead.ShippingDate = Convert.ToDateTime(UC_ShippingDate.Date); }

                TransferHead.ExpDeliveryDate = null;
                if (UC_ExpDeliveryDate.Date != null) { TransferHead.ExpDeliveryDate = Convert.ToDateTime(UC_ExpDeliveryDate.Date); }

                TransferHead.TransporterName = null;
                TransferHead.TransporterName = Convert.ToString(txtTransporterName.Text);

                TransferHead.Remark = null;
                TransferHead.Remark = Convert.ToString(txtRemark.Text);

                long TransferHeadID = AssetClient.SavetToolTransferHead(TransferHead, profile.DBConnection._constr);

                if (TransferHeadID > 0)
                {
                    DateTime tdate =Convert.ToDateTime(TransferHead.TransferDate);
                    AssetClient.FinalSaveToolTransferDetails(Session.SessionID, "AssetList", Convert.ToString(TransferHeadID), profile.Personal.UserID.ToString(), hdnSelectedToSite.Value, tdate, profile.DBConnection._constr);
                    WebMsgBox.MsgBox.Show("Record saved sussessfully");
                    Response.Redirect("../POR/ToolTransfer.aspx");
                }
                GetTransferList();
            }
            catch { WebMsgBox.MsgBox.Show("Some Error Occured"); }
            finally { }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { 
            //clear(); 
            setActiveTab(0); 
            //GetProductList(); 
        }

        //protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        //{
        //    ImageButton imgbtn = (ImageButton)sender;
        //    //clear();
        //    hdnprodID.Value = imgbtn.ToolTip.ToString();
        //    GetProductDetailByProductID();
        //    setActiveTab(1);
        //}

        //protected void GetProductDetailByProductID()
        //{
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iProductMasterClient productClient = new iProductMasterClient();

        //    tToolTransferHead obj = new tToolTransferHead();
        //    obj = productClient.GetToolTransferHead(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
        //    productClient.Close();

        //}
    }
}