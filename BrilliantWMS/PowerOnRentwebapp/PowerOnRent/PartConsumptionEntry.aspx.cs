using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORServiceUCCommonFilter;
using PowerOnRentwebapp.Login;
using System.Web.Services;
using BrilliantWMS.PORServicePartConsumption;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.PORServicePartReceipts;
using BrilliantWMS.PORServicePartIssue;
using BrilliantWMS.PORServicePartRequest;

namespace PowerOnRentwebapp.PowerOnRent
{
    public partial class PartConsumptionEntry : System.Web.UI.Page
    {
        static string ObjectName = "ConsumptionPartDetail";

        #region Page Events
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(Object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillSites();
                FillStatus();
                Toolbar1.SetUserRights("Consumption", "EntryForm", "");

                if (Session["PORConsumptionID"] != null)
                {
                    if (Session["PORConsumptionID"].ToString() == "0")
                    {
                        WMpageAddNew();
                        SetDefaultValue();
                    }
                    else if (Session["PORConsumptionID"].ToString() != "0")
                    {
                        GetConsuptionHead();
                    }
                }
                else if (Session["PORReceiptID"] != null)
                {
                    if (Session["PORReceiptID"].ToString() != "0")
                    {
                        GetDefaultConsumptionfromRequestHead();
                    }
                }

            }
            UC_DateConsumption.DateIsRequired(true, "", "");
        }

        #endregion

        #region Toolbar Code
        [WebMethod]
        public static void WMpageAddNew()
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HttpContext.Current.Session["PORConsumptionID"] = "0";
                HttpContext.Current.Session["PORstate"] = "Add";
                objService.ClearTempDataFromDB(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Fill DropDown
        protected void FillSites()
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<mTerritory> SiteList = new List<mTerritory>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                SiteList = objService.GetSiteNameByUserID(profile.Personal.UserID, profile.DBConnection._constr).ToList();
                mTerritory select = new mTerritory() { ID = 0, Territory = "-Select-" };
                SiteList.Insert(0, select);
                ddlSites.DataSource = SiteList;
                ddlSites.DataBind();

                ddlSites.SelectedIndex = 1;
            }
            catch { }
            finally { objService.Close(); }
        }

        public List<PORServicePartConsumption.mStatu> FillStatus()
        {
            string state = HttpContext.Current.Session["PORstate"].ToString();
            iPartConsumptionClient objService = new iPartConsumptionClient();
            List<PORServicePartConsumption.mStatu> StatusList = new List<PORServicePartConsumption.mStatu>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                StatusList = objService.GetStatusListForConsumption("All,Consumption", state, profile.Personal.UserID, profile.DBConnection._constr).ToList();

                PORServicePartConsumption.mStatu select = new PORServicePartConsumption.mStatu() { ID = 0, Status = "-Select-" };
                StatusList.Insert(0, select);

                ddlStatus.DataSource = null;
                ddlStatus.DataBind();

                ddlStatus.DataSource = StatusList;
                ddlStatus.DataBind();

                ddlStatus.SelectedIndex = 1;
            }
            catch { }
            finally { objService.Close(); }
            return StatusList;
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
                UsersList = UsersList.GroupBy(x => x.userID).Select(x => x.FirstOrDefault()).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
            }
            catch { }
            finally { objService.Close(); }
            return UsersList;
        }

        [WebMethod]
        public static List<PORServiceUCCommonFilter.v_GetEngineDetails> WMFillEnginList(long SiteID)
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<PORServiceUCCommonFilter.v_GetEngineDetails> EngineList = new List<PORServiceUCCommonFilter.v_GetEngineDetails>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                EngineList = objService.GetEngineOfSite(SiteID.ToString(), profile.DBConnection._constr).ToList();
                PORServiceUCCommonFilter.v_GetEngineDetails select = new PORServiceUCCommonFilter.v_GetEngineDetails() { ID = 0, Container = "-Select-" };
                EngineList.Insert(0, select);

            }
            catch { }
            finally { objService.Close(); }
            return EngineList;
        }
        #endregion

        #region EquipmentDetails
        [WebMethod]
        public static PORServiceEngineMaster.v_GetEngineDetails WMGetEngineDetails(int EngineID)
        {
            iEngineMasterClient objService = new iEngineMasterClient();
            PORServiceEngineMaster.v_GetEngineDetails EngineRec = new PORServiceEngineMaster.v_GetEngineDetails();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                EngineRec = objService.GetmEngineListByID(EngineID, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return EngineRec;
        }

        #endregion

        #region Consumption Head
        protected void GetConsuptionHead()
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            PORtConsumptionHead ConsuptionHead = new PORtConsumptionHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ConsuptionHead = objService.GetConsumptionHeadByConsumptionID(Convert.ToInt64(HttpContext.Current.Session["PORConsumptionID"].ToString()), profile.DBConnection._constr);
                ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(ConsuptionHead.SiteID.ToString()));
                UC_DateConsumption.Date = ConsuptionHead.ConsumptionDate;
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(ConsuptionHead.StatusID.ToString()));

                ddlConsumedBy.DataSource = WMFillUserList(Convert.ToInt64(ConsuptionHead.SiteID));
                ddlConsumedBy.DataBind();
                ddlConsumedBy.SelectedIndex = ddlConsumedBy.Items.IndexOf(ddlConsumedBy.Items.FindByValue(ConsuptionHead.ConsumedByUserID.ToString()));
                txtRemark.Text = ConsuptionHead.Remark;

                ddlContainer.DataSource = WMFillEnginList(Convert.ToInt64(ConsuptionHead.SiteID));
                ddlContainer.DataBind();
                ddlContainer.SelectedIndex = ddlContainer.Items.IndexOf(ddlContainer.Items.FindByText(ConsuptionHead.Container.ToString()));

                lblEngineModel.Text = ConsuptionHead.EngineModel;
                lblEngineSerial.Text = ConsuptionHead.EngineSerial;
                txtFailureHours.Text = ConsuptionHead.FailureHours;
                txtFailureCause.Text = ConsuptionHead.FailureCause;
                txtFailureNature.Text = ConsuptionHead.FailureNature;
                FillPartDetailByConsumptionID(ConsuptionHead.ConH_ID);
                divDisabled();
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void SetDefaultValue()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ddlConsumedBy.DataSource = null;
                ddlConsumedBy.DataBind();

                ddlConsumedBy.DataSource = WMFillUserList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                ddlConsumedBy.DataBind();
                ddlConsumedBy.SelectedIndex = ddlConsumedBy.Items.IndexOf(ddlConsumedBy.Items.FindByValue(profile.Personal.UserID.ToString()));

                ddlContainer.DataSource = null;
                ddlContainer.DataBind();
                ddlContainer.DataSource = WMFillEnginList(Convert.ToInt64(ddlSites.SelectedItem.Value));
                ddlContainer.DataBind();

                UC_DateConsumption.Date = DateTime.Now;
            }
            catch { }
        }
        [WebMethod]
        public static string WMSaveConsumption(object objConsumption)
        {
            string result = "";
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                PORtConsumptionHead ConsumptionHead = new PORtConsumptionHead();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objConsumption;

                if (HttpContext.Current.Session["PORConsumptionID"] != null)
                {
                    if (HttpContext.Current.Session["PORConsumptionID"].ToString() == "0")
                    {
                        ConsumptionHead.CreatedBy = profile.Personal.UserID;
                        ConsumptionHead.CreationDt = DateTime.Now;
                    }
                    else
                    {
                        ConsumptionHead = objService.GetConsumptionHeadByConsumptionID(Convert.ToInt64(HttpContext.Current.Session["PORConsumptionID"]), profile.DBConnection._constr);
                        ConsumptionHead.LastModifiedBy = profile.Personal.UserID;
                        ConsumptionHead.LastModifiedDt = DateTime.Now;
                    }

                    ConsumptionHead.SiteID = Convert.ToInt64(dictionary["SiteID"]);
                    ConsumptionHead.ReferenceID = 0;
                    ConsumptionHead.ObjectName = "DirectConsumed";
                    if (HttpContext.Current.Session["PORReceiptID"] != null)
                    {
                        ConsumptionHead.ReferenceID = Convert.ToInt64(HttpContext.Current.Session["PORReceiptID"].ToString()); ;
                        ConsumptionHead.ObjectName = "MaterialReceipt";
                    }

                    ConsumptionHead.ConsumptionDate = Convert.ToDateTime(dictionary["ConsumptionDate"]);
                    ConsumptionHead.StatusID = Convert.ToInt64(dictionary["StatusID"]);
                    ConsumptionHead.ConsumedByUserID = Convert.ToInt64(dictionary["ConsumedByUserID"]);
                    ConsumptionHead.Remark = dictionary["Remark"].ToString();
                    ConsumptionHead.FailureCause = dictionary["FailureCause"].ToString();
                    ConsumptionHead.FailureHours = dictionary["FailureHours"].ToString();
                    ConsumptionHead.FailureNature = dictionary["FailureNature"].ToString();

                    ConsumptionHead.EngineSerial = dictionary["EngineSerial"].ToString();
                    ConsumptionHead.EngineModel = dictionary["EngineModel"].ToString();
                    ConsumptionHead.GeneratorModel = dictionary["GeneratorModel"].ToString();
                    ConsumptionHead.GeneratorSerial = dictionary["GeneratorSerial"].ToString();
                    ConsumptionHead.TransformerSerial = dictionary["TransformerSerial"].ToString();
                    ConsumptionHead.Container = dictionary["Container"].ToString();

                    long ConsumptionID = objService.SetIntoPartConsumptionHead(ConsumptionHead, profile.DBConnection._constr);

                    if (ConsumptionID > 0)
                    {
                        objService.FinalSavePartDetail(HttpContext.Current.Session.SessionID, ObjectName, ConsumptionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                        result = "Consumption record saved successfully";
                    }
                }
            }
            catch { result = "Some error occurred"; }
            finally { objService.Close(); }
            return result;
        }

        protected void divDisabled()
        {
            if (ddlStatus.Items.Count > 0)
            {
                if (Convert.ToInt32(ddlStatus.SelectedItem.Value) > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(true, 'divConsumptionDetail')", true);
                    Toolbar1.SetSaveRight(false, "Not Allowed");
                    Toolbar1.SetClearRight(false, "Not Allowed");
                }
                //else { ScriptManager.RegisterStartupScript(this, this.GetType(), "changemodeIssue" + Session.SessionID, "changemode(false, 'divConsumptionDetail')", true); }
            }
        }
        #endregion

        #region Consumption Part Details
        protected void FillPartDetailByConsumptionID(long ConsumptionID)
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = objService.GetConsumptionPartDetailByConsumptionID(ConsumptionID, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                Grid1.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void FillPartDetailByReceiptID(long ReceiptID)
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Grid1.DataSource = null;
                Grid1.DataBind();
                Grid1.DataSource = objService.GetConsumptionPartDetailByReceiptID(ReceiptID, Convert.ToInt64(ddlSites.SelectedItem.Value), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                Grid1.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                List<POR_SP_GetPartDetails_OfConsumption_Result> PartList = new List<POR_SP_GetPartDetails_OfConsumption_Result>();
                if (hdn.Value == "")
                {
                    PartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    PartList = objService.AddPartIntoConsumption_TempDataByPartIDs(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                }
                Grid1.DataSource = PartList;
                Grid1.DataBind();
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static string WMUpdateQty(object objConsumed)
        {
            string RemaningQty = "";
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objConsumed;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetails_OfConsumption_Result ConsumedPart = new POR_SP_GetPartDetails_OfConsumption_Result();
                ConsumedPart.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                ConsumedPart.ConsumedQty = Convert.ToDecimal(dictionary["ConsumedQty"]);

                objService.UpdatePartConsumedQty_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ConsumedPart, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return RemaningQty;
        }

        [WebMethod]
        public static void WMRemovePartFromList(Int32 Sequence)
        {
            iPartConsumptionClient objService = new iPartConsumptionClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                CustomProfile profile = CustomProfile.GetProfile();
                objService.RemovePartFrom_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
        }
        #endregion

        #region Set Consumption from Receipt Data
        public void GetDefaultConsumptionfromRequestHead()
        {
            iPartRequestClient objRequestService = new iPartRequestClient();
            iPartIssueClient objIssueService = new iPartIssueClient();
            iPartReceiptClient objReceiptService = new iPartReceiptClient();
            iPartConsumptionClient objConsumptionService = new iPartConsumptionClient();
            try
            {
                HttpContext.Current.Session["PORConsumptionID"] = "0";
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["PORReceiptID"] != null)
                {
                    if (Session["PORReceiptID"].ToString() != "0")
                    {
                        PORtGRNHead ReceiptHead = new PORtGRNHead();
                        PORtMINHead IssueHead = new PORtMINHead();
                        PORtPartRequestHead RequestHead = new PORtPartRequestHead();
                        ReceiptHead = objReceiptService.GetReceiptHeadByReceiptID(Convert.ToInt64(Session["PORReceiptID"].ToString()), profile.DBConnection._constr);
                        if (ReceiptHead != null)
                        {
                            IssueHead = objIssueService.GetIssueHeadByIssueID(Convert.ToInt64(ReceiptHead.ReferenceID), profile.DBConnection._constr);
                        }
                        if (IssueHead != null)
                        {
                            RequestHead = objRequestService.GetRequestHeadByRequestID(Convert.ToInt64(IssueHead.PRH_ID), profile.DBConnection._constr);
                        }

                        if (RequestHead != null)
                        {
                            ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(RequestHead.SiteID.ToString()));
                            UC_DateConsumption.Date = DateTime.Now;
                            ddlStatus.SelectedIndex = 1;

                            ddlConsumedBy.DataSource = null;
                            ddlConsumedBy.DataBind();
                            ddlConsumedBy.DataSource = WMFillUserList(Convert.ToInt64(RequestHead.SiteID));
                            ddlConsumedBy.DataBind();
                            ddlConsumedBy.SelectedIndex = ddlConsumedBy.Items.IndexOf(ddlConsumedBy.Items.FindByValue(profile.Personal.UserID.ToString()));

                            ddlContainer.DataSource = null;
                            ddlContainer.DataBind();

                            ddlContainer.DataSource = WMFillEnginList(Convert.ToInt64(RequestHead.SiteID));
                            ddlContainer.DataBind();
                            ddlContainer.SelectedIndex = ddlContainer.Items.IndexOf(ddlContainer.Items.FindByText(RequestHead.Container.ToString()));

                            lblEngineModel.Text = RequestHead.EngineModel.ToString();
                            lblEngineSerial.Text = RequestHead.EngineSerial.ToString();

                            txtFailureHours.Text = RequestHead.FailureHours.ToString();
                            txtFailureCause.Text = RequestHead.FailureCause.ToString();
                            txtFailureNature.Text = RequestHead.FailureNature.ToString();

                            FillPartDetailByReceiptID(Convert.ToInt64(Session["PORReceiptID"].ToString()));
                        }
                    }
                }
            }
            catch { }
            finally
            {
                objRequestService.Close();
                objIssueService.Close();
                objReceiptService.Close();
            }
        }
        #endregion
    }
}