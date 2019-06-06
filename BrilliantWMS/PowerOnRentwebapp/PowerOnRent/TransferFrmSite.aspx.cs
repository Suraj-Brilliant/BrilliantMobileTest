using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.PORServiceUCCommonFilter;
//using BrilliantWMS.PORServiceHQGoodsReceipt;
using BrilliantWMS.PORServicePartIssue;

namespace BrilliantWMS.PowerOnRent
{
    public partial class TransferFrmSite : System.Web.UI.Page
    {
        static string ObjectName = "HQReceiptPartDetail";
        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillStatus();
                FillSites();
                Toolbar1.SetUserRights("GoodsReceipt", "EntryForm", "");

                if (Session["PORIssueID"] != null)
                {
                    //WMpageAddNew();
                    FillReceiptPartGridByIssueID(Convert.ToInt64(Session["PORIssueID"].ToString()));
                    // GetReceiptHeadByReceiptID();
                }
                else if (Session["PORHQReceiptID"] != null)
                {
                    if (Session["PORHQReceiptID"].ToString() == "0")
                    {
                     //   WMpageAddNew();
                    }
                    else if (Session["PORHQReceiptID"].ToString() != "0")
                    {
                        GetReceiptHeadByReceiptID();
                    }
                }
            }
            UCFormHeader1.FormHeaderText = "Transfer From Site";
            // UC_ReceiptDate.DateIsRequired(true, "", "");

        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        #endregion

        #region Toolbar Code
        //[WebMethod]
        //public static void WMpageAddNew()
        //{
            ////iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    HttpContext.Current.Session["PORHQReceiptID"] = "0";
            //    HttpContext.Current.Session["PORstate"] = "Add";
            //  //  objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            //    //objService.GetIssuePartDetailByRequestID(Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString()), HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "true", profile.DBConnection._constr).ToList();
            //}
            //catch { }
            //finally
            //{
            //    objService.Close();
            //}
        //}

        #endregion

        #region Fill Dropdown
        protected void FillSites()
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<mTerritory> SiteList = new List<mTerritory>();
                // SiteList = objService.GetSiteNameByUserID(profile.Personal.UserID, profile.DBConnection._constr).ToList();

                SiteList = objService.GetSiteNameByUserID_IssueID(Convert.ToInt64(Session["PORIssueID"].ToString()), profile.Personal.UserID, profile.DBConnection._constr).ToList();

                ddlSites.DataSource = SiteList;
                ddlSites.DataBind();

                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlSites.Items.Insert(0, lst);

                if (Session["PORstate"].ToString() == "Add")
                {
                    ddlSites.SelectedIndex = 1;
                    ddlReceivedBy.DataSource = null;
                    ddlReceivedBy.DataBind();
                    ddlReceivedBy.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                    ddlReceivedBy.DataBind();
                    ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(profile.Personal.UserID.ToString()));
                    UC_PODate.Date = DateTime.Now;
                    // UC_ReceiptDate.Date = DateTime.Now;
                    // lblReceiptNo.Text = "Generate when save";
                    if (ddlStatus.Items.Count > 0) ddlStatus.SelectedIndex = 1;
                }

            }
            catch { }
            finally { objService.Close(); }
        }


        protected void FillStatus()
        {
            //string state = HttpContext.Current.Session["PORstate"].ToString();
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //List<BrilliantWMS.PORServiceHQGoodsReceipt.mStatu> StatusList = new List<BrilliantWMS.PORServiceHQGoodsReceipt.mStatu>();
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    // StatusList = objService.GetStatusListForGRN("All,Receipt", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
            //    StatusList = objService.GetStatusListForGRN("All,Transfer", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();
            //    BrilliantWMS.PORServiceHQGoodsReceipt.mStatu select = new BrilliantWMS.PORServiceHQGoodsReceipt.mStatu() { ID = 0, Status = "-Select-" };
            //    StatusList.Insert(0, select);

            //    ddlStatus.DataSource = null;
            //    ddlStatus.DataBind();
            //    ddlStatus.DataSource = StatusList;
            //    ddlStatus.DataBind();
            //}
            //catch { }
            //finally { objService.Close(); }
        }

        [WebMethod]
        public static List<vGetUserProfileByUserID> WMFillUserList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UsersList = objService.GetUserListBySiteID(SiteID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Distinct().ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }
        #endregion

        #region Receipt Part Detail
        protected void FillReceiptPartGridByIssueID(long IssueID)
        {
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();

            //    // lblReceiptNo.Text = "Generate when save";

            //    UC_PODate.Date = DateTime.Now;
            //    //ddlSites.SelectedIndex = 1;

            //    UC_ShippingDate.Date = DateTime.Now;
            //    UC_ExpDeliveryDate.Date = DateTime.Now.AddDays(7);

            //    ddlStatus.SelectedIndex = 3;

            //    ddlReceivedBy.DataSource = null;
            //    ddlReceivedBy.DataBind();

            //    ddlReceivedBy.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
            //    ddlReceivedBy.DataBind();
            //    ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(profile.Personal.UserID.ToString()));

            //    Grid1.DataSource = null;
            //    Grid1.DataBind();
            //    // Grid1.DataSource = objService.GetReceiptPartDetailByIssueID(IssueID, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "", profile.DBConnection._constr);
            //    Grid1.DataSource = objService.GetIssuePartDetailByIssueID_Transfer(IssueID, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "", Convert.ToInt64(ddlSites.SelectedItem.Value), profile.DBConnection._constr);    // (IssueID, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "", profile.DBConnection._constr);
            //    Grid1.DataBind();
            //}
            //catch { }
            //finally { objService.Close(); }
        }

        protected void FillReceiptPartGridByReceiptID(long ReceiptID)
        {
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //try
            //{
            //    Grid1.DataSource = null;
            //    Grid1.DataBind();
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    Grid1.DataSource = objService.GetReceiptPartDetailByReceiptID(ReceiptID, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, "", profile.DBConnection._constr);
            //    Grid1.DataBind();
            //}
            //catch { }
            //finally { objService.Close(); }
        }

        protected void GridReceipt_OnRebind(object sender, EventArgs e)
        {
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //try
            //{
            //    Grid1.DataSource = null;
            //    Grid1.DataBind();
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
            //    List<POR_SP_GetPartDetails_OfMIN_Transfer_Result> PartList = new List<POR_SP_GetPartDetails_OfMIN_Transfer_Result>();
            //    if (hdn.Value == "")
            //    {
            //        // PartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
            //        PartList = objService.GetExistingTempDataBySessionIDObjectName_Transfer(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
            //    }
            //    //else if (hdn.Value != "")
            //    //{
            //    //    PartList = objService.AddPartIntoReceipt_TempData(hdn.Value, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
            //    //}
            //    Grid1.DataSource = PartList;
            //    Grid1.DataBind();
            //}
            //catch { }
            //finally { objService.Close(); }
        }

        //[WebMethod]
        //public static string[] WMUpdateReceiptQty(object objReceipt)
        //{
            //string[] QtyResult = new string[] { };
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //try
            //{
            //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //    dictionary = (Dictionary<string, object>)objReceipt;
            //    CustomProfile profile = CustomProfile.GetProfile();

            //    POR_SP_GetPartDetails_OfGRN_HQ_Result ReceiptRec = new POR_SP_GetPartDetails_OfGRN_HQ_Result();
            //    ReceiptRec.Sequence = Convert.ToInt64(dictionary["Sequence"]);
            //    ReceiptRec.ReceivedQty = Convert.ToDecimal(dictionary["ReceivedQty"]);
            //    QtyResult = objService.UpdatePartReceipt_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ReceiptRec, profile.DBConnection._constr);
            //}
            //catch { }
            //finally { objService.Close(); }
            //return QtyResult;
        //}

        //[WebMethod]
        //public static void WMRemovePartFromRequest(Int32 Sequence)
        //{
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            ////iPartIssueClient objService = new iPartIssueClient();
            //try
            //{
            //    Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //    CustomProfile profile = CustomProfile.GetProfile();
            //  //  objService.RemovePartFromTransfer(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
            //}
            //catch { }
            //finally { objService.Close(); }
        //}
        #endregion

        #region Receipt Head
        protected void GetReceiptHeadByReceiptID()
        {
            //iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
            //PORtGRNHead ReceiptHead = new PORtGRNHead();
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    ReceiptHead = objService.GetReceiptHeadByReceiptID(Convert.ToInt64(HttpContext.Current.Session["PORHQReceiptID"].ToString()), profile.DBConnection._constr);
            //    if (ReceiptHead != null)
            //    {
            //        //  txtPONo.Text = ReceiptHead.ChallanNo;
            //        if (ReceiptHead.ChallanDate != null) UC_PODate.Date = ReceiptHead.ChallanDate;
            //        //    txtVendor.Text = ReceiptHead.Vendor;
            //        ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(ReceiptHead.SiteID.ToString()));
            //        //    lblReceiptNo.Text = ReceiptHead.GRNH_ID.ToString();
            //        //    UC_ReceiptDate.Date = ReceiptHead.GRN_Date;
            //        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(ReceiptHead.StatusID.Value.ToString()));
            //        ddlReceivedBy.DataSource = null;
            //        ddlReceivedBy.DataBind();
            //        ddlReceivedBy.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
            //        ddlReceivedBy.DataBind();
            //        ddlReceivedBy.SelectedIndex = ddlReceivedBy.Items.IndexOf(ddlReceivedBy.Items.FindByValue(ReceiptHead.ReceivedByUserID.Value.ToString()));
            //        //  txtReceiptRemark.Text = ReceiptHead.Remark;
            //        FillReceiptPartGridByReceiptID(ReceiptHead.GRNH_ID);
            //        divDisabled();
            //    }
            //}
            //catch { }
            //finally { objService.Close(); }
        }

        //[WebMethod]
        //public static string WMSaveReceiptHead(object objReceipt)
        //{
        //    string result = "";
        //    iHQGoodsReceiptClient objService = new iHQGoodsReceiptClient();
        //    try
        //    {
        //        CustomProfile profile = CustomProfile.GetProfile();

        //        PORtGRNHead ReceiptHead = new PORtGRNHead();
        //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //        dictionary = (Dictionary<string, object>)objReceipt;

        //        if (HttpContext.Current.Session["PORHQReceiptID"] != null)
        //        {
        //            if (HttpContext.Current.Session["PORHQReceiptID"] == "0")
        //            {
        //                ReceiptHead.CreatedBy = profile.Personal.UserID;
        //                ReceiptHead.CreationDt = DateTime.Now;
        //            }
        //            else
        //            {
        //                ReceiptHead = objService.GetReceiptHeadByReceiptID(Convert.ToInt64(HttpContext.Current.Session["PORHQReceiptID"]), profile.DBConnection._constr);
        //                ReceiptHead.LastModifiedBy = profile.Personal.UserID;
        //                ReceiptHead.LastModifiedDt = DateTime.Now;
        //            }

        //            ReceiptHead.SiteID = Convert.ToInt64(dictionary["SiteID"]);
        //            ReceiptHead.ObjectName = dictionary["ObjectName"].ToString();
        //            ReceiptHead.ReferenceID = Convert.ToInt64(dictionary["ReferenceID"].ToString());
        //            ReceiptHead.GRN_No = "N/A";
        //            ReceiptHead.GRN_Date = Convert.ToDateTime(dictionary["GRN_Date"].ToString());
        //            ReceiptHead.ReceivedByUserID = Convert.ToInt64(dictionary["ReceivedByUserID"]);
        //            ReceiptHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);
        //            ReceiptHead.Remark = dictionary["Remark"].ToString();

        //            /**/
        //            ReceiptHead.ChallanNo = dictionary["ChallanNo"].ToString();
        //            if (dictionary["ChallanDate"].ToString() != "") ReceiptHead.ChallanDate = Convert.ToDateTime(dictionary["ChallanDate"].ToString());
        //            ReceiptHead.Vendor = dictionary["Vendor"].ToString();

        //            ReceiptHead.IsSubmit = Convert.ToBoolean(dictionary["IsSubmit"]);

        //            long ReceiptID = objService.SetIntoGRNHead(ReceiptHead, profile.DBConnection._constr);

        //            if (ReceiptID > 0)
        //            {
        //                objService.FinalSaveReceiptPartDetail(HttpContext.Current.Session.SessionID, ObjectName, ReceiptID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
        //                result = "HQ's Material Receipt record saved successfully";
        //                HttpContext.Current.Session.Remove("PORIssueID");
        //            }
        //        }
        //    }
        //    catch { result = "Some error occurred"; }
        //    finally { objService.Close(); }
        //    return result;
        //}

        protected void divDisabled()
        {
            if (ddlStatus.Items.Count > 0)
            {
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(true, 'divReceiptDetail');changemode(true, 'divPODetail')", true);
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                }
                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(false, 'divReceiptDetail');changemode(false, 'divPODetail')", true); }
            }
        }
        #endregion

        protected void ddlStatus_SelectedIndexChange(object semder, EventArgs e)
        {
            FillReceiptPartGridByIssueID(Convert.ToInt64(Session["PORIssueID"].ToString()));
        }

        [WebMethod]
        public static string WMUpdateHQStock(object objHQ)
        {
            string HQQty = "";
            iPartIssueClient objService = new iPartIssueClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objHQ;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetails_OfMIN_Result IssuePart = new POR_SP_GetPartDetails_OfMIN_Result();
                IssuePart.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                IssuePart.IssuedQty = Convert.ToDecimal(dictionary["IssuedQty"]);

                HQQty = objService.UpdateHQStock_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), IssuePart, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return HQQty;
        }


        [WebMethod]
        public static string GetToSiteID(long IssueID)
        {
            string result;
            iUCCommonFilterClient SiteID = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long ID = SiteID.GetSiteID(IssueID, profile.DBConnection._constr);
            result = Convert.ToString(ID);
            return result;
        }

        [WebMethod]
        public static string WMSaveTransferHead(object objTrans)
        {
            string result = "";
            long TransferedID = 0;
            iPartIssueClient objService = new iPartIssueClient();

            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                PORtTransHead TransHead = new PORtTransHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objTrans;

                TransHead.CreatedBy = profile.Personal.UserID;
                TransHead.CreationDt = DateTime.Now;

                TransHead.ToSiteID = Convert.ToInt64(dictionary["ToSiteID"]);
                TransHead.MIN_No = "N/A";
                TransHead.TransferDate = Convert.ToDateTime(dictionary["TransferDate"]);
                TransHead.TransferedByUserID = Convert.ToInt64(dictionary["TransferedByUserID"]);
                TransHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);

                TransHead.AirwayBill = dictionary["AirwayBill"].ToString();
                TransHead.ShippingType = dictionary["ShippingType"].ToString();
                if (dictionary["ShippingDate"].ToString() != "") TransHead.ShippingDate = Convert.ToDateTime(dictionary["ShippingDate"].ToString());
                if (dictionary["ExpectedDelDate"].ToString() != "") TransHead.ExpectedDelDate = Convert.ToDateTime(dictionary["ExpectedDelDate"].ToString());
                TransHead.TransporterName = dictionary["TransporterName"].ToString();
                TransHead.Remark = dictionary["Remark"].ToString();
                TransHead.IsSubmit = Convert.ToBoolean(dictionary["IsSubmit"]);
                TransHead.FromSiteID = Convert.ToInt64(dictionary["FromSiteID"]);
                TransHead.PRH_ID = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());

                TransferedID = objService.SetIntoTransHead(TransHead, profile.DBConnection._constr);

                if (TransferedID > 0)
                {
                    string TransferStatus = "NA";
                    if (TransHead.StatusID != 1 && TransHead.StatusID != 10) TransferStatus = "Transfer";
                    objService.FinalSaveTransferPartDetail(HttpContext.Current.Session.SessionID, ObjectName, TransferedID, Convert.ToInt64(TransHead.PRH_ID), profile.Personal.UserID.ToString(), TransferStatus, Convert.ToInt64(TransHead.FromSiteID), profile.DBConnection._constr);
                    result = "Material Transfer record saved successfully";
                }
            }
            catch { result = "Some error occurred"; }
            finally { objService.Close(); }

            result = Convert.ToString(TransferedID);
            return result;
        }

        [WebMethod]
        public static string WMSaveIssueHead(object objIssue)
        {
            string result="";
            iPartIssueClient objService = new iPartIssueClient();

            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                PORtMINHead IssueHead = new PORtMINHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objIssue;

                IssueHead.CreatedBy = profile.Personal.UserID;
                IssueHead.CreationDt = DateTime.Now;

                IssueHead.MINH_ID = Convert.ToInt64(dictionary["MINH_ID"]);
                IssueHead.SiteID = Convert.ToInt64(dictionary["SiteID"]);
                IssueHead.PRH_ID = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());
                IssueHead.MIN_No = "N/A";
                IssueHead.IssueDate = Convert.ToDateTime(dictionary["IssueDate"]);
                IssueHead.IssuedByUserID = Convert.ToInt64(dictionary["IssuedByUserID"]);
                IssueHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);

                IssueHead.AirwayBill = dictionary["AirwayBill"].ToString();
                IssueHead.ShippingType = dictionary["ShippingType"].ToString();
                if (dictionary["ShippingDate"].ToString() != "") IssueHead.ShippingDate = Convert.ToDateTime(dictionary["ShippingDate"].ToString());
                if (dictionary["ExpectedDelDate"].ToString() != "") IssueHead.ExpectedDelDate = Convert.ToDateTime(dictionary["ExpectedDelDate"].ToString());
                IssueHead.TransporterName = dictionary["TransporterName"].ToString();
                IssueHead.Remark = dictionary["Remark"].ToString();
                IssueHead.IsSubmit = Convert.ToBoolean(dictionary["IsSubmit"]);
                IssueHead.TransH_ID = Convert.ToInt64(dictionary["TransH_ID"]);

                if (IssueHead.StatusID != 1 && IssueHead.StatusID != 10)
                {
                    IssueHead.StatusID = objService.GetStatusOfIssueHead_Transfer(HttpContext.Current.Session.SessionID.ToString(), profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt64(IssueHead.PRH_ID), profile.DBConnection._constr);
                }

                long IssueID = objService.SetIntoMINHead_Transfer(IssueHead, profile.DBConnection._constr);
                
                if (IssueID > 0)
                {
                    string IssueStatus = "NA";
                    if (IssueHead.StatusID != 1 && IssueHead.StatusID != 10) IssueStatus = "Issue";
                    objService.FinalSaveIssuePartDetail_Transfer(HttpContext.Current.Session.SessionID, ObjectName, IssueID, Convert.ToInt64(IssueHead.PRH_ID), profile.Personal.UserID.ToString(), IssueStatus, profile.DBConnection._constr);
                    result = "Material Issue record saved successfully";
                }
            }
            catch { result = "Some error occurred"; }
            finally { objService.Close(); }

            return result;
        }
    }
}