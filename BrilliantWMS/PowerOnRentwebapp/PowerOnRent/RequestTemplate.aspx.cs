using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.PowerOnRent
{
    public partial class RequestTemplate : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string ObjectName = "TemplatePartDetail";
        static long adrs, c1, c2, dpt, cmp;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // h4DivHead.InnerText = "List of Request Template";
            UCFormHeader1.FormHeaderText = "Request Template";
            //Toolbar1.SetUserRights("MaterialRequest", "Summary", "");
            //  FillSites();
            if (!IsPostBack)
            {
                if (Session["Lang"] == null)
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();

                FillSites();

                iPartRequestClient objService = new iPartRequestClient();
                CustomProfile profile = CustomProfile.GetProfile();

                tbTemplateLst.Visible = true;
                tbTemplateDetail.Visible = false;
                tabContainerReqTemplate.ActiveTabIndex = 0;
                objService.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);

            }
            GetTemplateList();
            //Toolbar1.SetSaveRight(false, "Not Allowed");
            //Toolbar1.SetClearRight(false, "Not Allowed");

            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void FillSites()
        {
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();

                List<mCompany> CompanyLst = new List<mCompany>();
                string UserType = profile.Personal.UserType.ToString();
                long UID = profile.Personal.UserID;

                if (UserType == "Admin")
                {
                    //  CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                    // CompanyLst = objService.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();

                    CompanyLst = objService.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
                }
                else if (UserType == "User" || UserType == "Requester And Approver" || UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
                {
                    CompanyLst = objService.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();
                }
                else
                {
                    CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                }
                ddlCompany.DataSource = CompanyLst;
                ddlCompany.DataBind();

                if (UserType == "Admin")
                {
                    // ListItem lstCmpny = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlCompany.Items.Insert(0, lstCmpny);
                    if (ddlCompany.Items.Count > 0)
                        ddlCompany.SelectedIndex = 0; //1;
                    hdnselectedCompany.Value = ddlCompany.SelectedValue.ToString();

                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    // SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

                    int Cmpny = int.Parse(hdnselectedCompany.Value);
                    SiteLst = WMGetSelDept(Cmpny, profile.Personal.UserID);

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0)
                        ddlSites.SelectedIndex = 0; // 1;

                    hdnselectedDept.Value = ddlSites.SelectedValue.ToString(); Session["DeptID"] = ddlSites.SelectedValue.ToString();

                    //long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString();
                    //long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    //ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    //ddlContact1.DataBind();
                    //ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlContact1.Items.Insert(0, lstContact);

                    //ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    //ddlAddress.DataBind();
                    //ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlAddress.Items.Insert(0, lstAdrs);
                }
                else if (UserType == "User" || UserType == "Requester And Approver" || UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
                {
                    ddlCompany.Enabled = false;

                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    //SiteLst = UCCommonFilter.GetDepartmentListUserWise(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();
                    SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 1;

                    //  ddlSites.Enabled = false;
                    long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString();

                    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    //ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    //ddlContact1.DataBind();
                    //ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlContact1.Items.Insert(0, lstContact);

                    //ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    //ddlAddress.DataBind();
                    //ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlAddress.Items.Insert(0, lstAdrs);
                }
                else
                {
                    ListItem lstCmpny = new ListItem { Text = "-Select-", Value = "0" };
                    ddlCompany.Items.Insert(0, lstCmpny);
                    if (ddlCompany.Items.Count > 0) ddlCompany.SelectedIndex = 1;
                    List<mTerritory> SiteLst = new List<mTerritory>();
                    iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                    SiteLst = UCCommonFilter.GetSiteNameByUserID(Convert.ToInt16(UID), profile.DBConnection._constr).ToList();

                    ddlSites.DataSource = SiteLst;
                    ddlSites.DataBind();
                    if (ddlSites.Items.Count > 0) ddlSites.SelectedIndex = 1;

                    long DeptID = UCCommonFilter.GetSiteIdOfUser(UID, profile.DBConnection._constr); hdnselectedDept.Value = DeptID.ToString(); Session["DeptID"] = DeptID.ToString();
                    long CompanyID = UCCommonFilter.GetCompanyIDFromSiteID(DeptID, profile.DBConnection._constr); hdnselectedCompany.Value = CompanyID.ToString();

                    //ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(DeptID);
                    //ddlContact1.DataBind();
                    //ListItem lstContact = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlContact1.Items.Insert(0, lstContact);

                    //ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(DeptID);
                    //ddlAddress.DataBind();
                    //ListItem lstAdrs = new ListItem { Text = "-Select-", Value = "0" };
                    //ddlAddress.Items.Insert(0, lstAdrs);
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "FillSites");
            }
            finally { objService.Close(); }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            clearfields();
            txtTitle.Focus();
            tbTemplateLst.Visible = false;
            tbTemplateDetail.Visible = true;
            tabContainerReqTemplate.ActiveTabIndex = 1;
            hdnTemplateID.Value = "";
            objService.ClearTempDataFromDBNEW(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
        }

        public void clearfields()
        {
            txtTitle.Text = "";
            ddlAccessType.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;
            txtRemark.Text = "";
            Grid1.DataSource = null;
            Grid1.DataBind();
            ddlSites.SelectedIndex = -1;
            //ddlAddress.SelectedIndex = -1;
            //ddlContact1.SelectedIndex = -1;
            //ddlContact2.SelectedIndex = -1;

        }

        protected void pageSave(object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mRequestTemplateHead ReqTempHead = new mRequestTemplateHead();

                int cnt;
                cnt = objService.GridRowCount(Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);

                if (cnt == 0)
                {
                    WebMsgBox.MsgBox.Show("Add atleast one part into the Request Part List");
                }
                else
                {
                    List<POR_SP_GetPartDetail_ForRequest_Result> GetGridRowsTemplate = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                    GetGridRowsTemplate = objService.GridRowsTemplate(Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                    //var ReqQty = GetGridRowsTemplate.Where(r => r.RequestQty == 0).ToList();

                    //long RQty = Convert.ToInt64(ReqQty.Count);
                    //if (RQty >=1)
                    //{
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('One or more request quantity is zero','Error','#')", true);

                    //}
                    //else
                    //{

                    if (hdnTemplateID.Value == "")
                    {
                        ReqTempHead.CreatedBy = profile.Personal.UserID;
                        ReqTempHead.CreatedDate = DateTime.Now;
                    }
                    else
                    {
                        ReqTempHead = objService.GetTemplateOrderHead(Convert.ToInt64(hdnTemplateID.Value), profile.DBConnection._constr);
                        ReqTempHead.CreatedBy = ReqTempHead.CreatedBy;
                        ReqTempHead.CreatedDate = ReqTempHead.CreatedDate;

                        ReqTempHead.ModifiedBy = profile.Personal.UserID;
                        ReqTempHead.ModifiedDate = DateTime.Now;
                        ReqTempHead.ID = Convert.ToInt64(hdnTemplateID.Value);
                    }

                    ReqTempHead.TemplateTitle = txtTitle.Text;
                    ReqTempHead.Accesstype = ddlAccessType.SelectedValue.ToString();

                    if (hdnselectedDept.Value == "")
                    { ReqTempHead.Department = dpt; }
                    else { ReqTempHead.Department = Convert.ToInt64(hdnselectedDept.Value); }

                    if (hdnselectedCompany.Value == "")
                    { ReqTempHead.Customer = cmp; }
                    else { ReqTempHead.Customer = Convert.ToInt64(hdnselectedCompany.Value); }
                    ReqTempHead.Active = "Yes";

                    ReqTempHead.Remark = txtRemark.Text;
                    if (hdnSelAddress.Value == "")
                    { ReqTempHead.Address = adrs; }
                    else { ReqTempHead.Address = Convert.ToInt64(hdnSelAddress.Value); }
                    if (hdnselectedCont1.Value == "")
                    { ReqTempHead.Contact1 = c1; }
                    else { ReqTempHead.Contact1 = Convert.ToInt64(hdnselectedCont1.Value); }
                    if (hdnselectedCont2.Value == "")
                    { ReqTempHead.Contact2 = c2; }
                    else { ReqTempHead.Contact2 = Convert.ToInt64(hdnselectedCont2.Value); }

                    long RequestTemplateID = objService.InsertIntomRequestTemplateHead(ReqTempHead, profile.DBConnection._constr);
                    if (RequestTemplateID > 0)
                    {
                        objService.FinalSavemRequestTemplateDetailTemplate(HttpContext.Current.Session.SessionID, ObjectName, RequestTemplateID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                        WebMsgBox.MsgBox.Show("Template Saved Successfully");
                        clearfields();
                        //tbTemplateLst.Visible = true;
                        //tbTemplateDetail.Visible = false;
                        //tabContainerReqTemplate.ActiveTabIndex = 0;
                        //GetTemplateList();
                        //Response.Redirect("../PowerOnRent/RequestTemplate.aspx");
                    }
                    //}

                }
                //}

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "pageSave");
            }
            finally { objService.Close(); }
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            //clear();
            try
            {
                string TmplSelValue = hdnTmplSelectedRec.Value.ToString();
                // hdnTemplateID.Value = imgbtn.ToolTip.ToString();
                hdnTemplateID.Value = hdnTmplSelectedRec.Value.ToString();
                GetTemplateDetails();
                tbTemplateLst.Visible = false;
                tbTemplateDetail.Visible = true;
                tabContainerReqTemplate.ActiveTabIndex = 1;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "pageSave");
            }
        }

        protected void GetTemplateDetails()
        {
            iPartRequestClient objService = new iPartRequestClient();
            mRequestTemplateHead ReqTmpHead = new mRequestTemplateHead();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ReqTmpHead = objService.GetTemplateOrderHead(Convert.ToInt64(hdnTemplateID.Value), profile.DBConnection._constr);

                long SiteID = Convert.ToInt64(ReqTmpHead.Department); hdnselectedDept.Value = SiteID.ToString(); Session["DeptID"] = SiteID.ToString();

                iUCCommonFilterClient objCommon = new iUCCommonFilterClient(); 
                long CompanyID = objCommon.GetCompanyIDFromSiteID(SiteID, profile.DBConnection._constr);

                txtTitle.Text = ReqTmpHead.TemplateTitle.ToString();
                ddlAccessType.SelectedValue = ReqTmpHead.Accesstype.ToString();

                //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(ReqTmpHead.Customer.ToString()));
                //cmp = long.Parse(ReqTmpHead.Customer.ToString());
                List<mCompany> CompanyLst = new List<mCompany>();
                string UserType = profile.Personal.UserType.ToString();
                long UID = profile.Personal.UserID;

                if (UserType == "Admin")
                {
                    //  CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                    // CompanyLst = objService.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();

                    CompanyLst = objCommon.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
                }
                else if (UserType == "User" || UserType == "Requester And Approver" || UserType == "Requester" || profile.Personal.UserType == "Requestor" || profile.Personal.UserType == "Requestor And Approver")
                {
                    CompanyLst = objCommon.GetUserCompanyName(UID, profile.DBConnection._constr).ToList();
                }
                else
                {
                    CompanyLst = objCommon.GetCompanyName(profile.DBConnection._constr).ToList();
                }
                ddlCompany.DataSource = CompanyLst;
                ddlCompany.DataBind();

                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyID.ToString())); //ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(ReqTmpHead.Customer.ToString()));
                cmp = CompanyID;

                int Cmpny = int.Parse(CompanyID.ToString());  //Convert.ToInt16(ReqTmpHead.Customer.ToString());
                ddlSites.DataSource = WMGetDept(Cmpny);
                ddlSites.DataBind();
                ddlSites.SelectedIndex = ddlSites.Items.IndexOf(ddlSites.Items.FindByValue(ReqTmpHead.Department.ToString()));
                dpt = long.Parse(ReqTmpHead.Department.ToString());

                txtRemark.Text = ReqTmpHead.Remark.ToString();

                //ddlAddress.DataSource = WMGetDeptAddress(CompanyID); //WMGetDeptAddress(SiteID);
                //ddlAddress.DataBind();
                //ddlAddress.SelectedIndex = ddlAddress.Items.IndexOf(ddlAddress.Items.FindByValue(ReqTmpHead.Address.ToString()));
                //adrs = long.Parse(ReqTmpHead.Address.ToString());

                //ddlContact1.DataSource = WMGetContactPersonLst(CompanyID); //WMGetContactPersonLst(SiteID);
                //ddlContact1.DataBind();
                //ddlContact1.SelectedIndex = ddlContact1.Items.IndexOf(ddlContact1.Items.FindByValue(ReqTmpHead.Contact1.ToString()));
                //c1 = long.Parse(ReqTmpHead.Contact1.ToString());
                ////ddlContact2.DataSource = WMGetContactPerson2Lst(Convert.ToInt64(ReqTmpHead.Department), Convert.ToInt64(ddlContact1.SelectedIndex));
                //ddlContact2.DataSource = WMGetContactPerson2Lst(CompanyID, Convert.ToInt64(ddlContact1.SelectedIndex));
                //ddlContact2.DataBind();
                //ddlContact2.SelectedIndex = ddlContact2.Items.IndexOf(ddlContact2.Items.FindByValue(ReqTmpHead.Contact2.ToString()));
                //c2 = long.Parse(ReqTmpHead.Contact2.ToString());

                //lblAddress.Text = ddlAddress.SelectedItem.ToString();

                DataSet dsTemplatePartLst = new DataSet();
                dsTemplatePartLst = objService.GetTemplatePartLstByTemplateID(Convert.ToInt64(hdnTemplateID.Value), profile.DBConnection._constr);
                List<POR_SP_GetPartDetail_ForRequest_Result> TemplatePartList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                if (dsTemplatePartLst.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= dsTemplatePartLst.Tables[0].Rows.Count - 1; i++)
                    {
                        TemplatePartList = objService.AddPartIntoRequest_TempData(dsTemplatePartLst.Tables[0].Rows[i]["PrdID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, SiteID, profile.DBConnection._constr).ToList();
                        string uom = objService.GetUOMName(Convert.ToInt64(dsTemplatePartLst.Tables[0].Rows[i]["UOMID"].ToString()), profile.DBConnection._constr);
                        POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                        PartRequest.Sequence = i + 1;
                        PartRequest.RequestQty = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Qty"].ToString()); // Convert.ToDecimal(dictionary["RequestQty"]);
                        PartRequest.Price = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Price"].ToString());
                        PartRequest.Total = Convert.ToDecimal(dsTemplatePartLst.Tables[0].Rows[i]["Total"].ToString());
                        PartRequest.IsPriceChange = Convert.ToInt16(dsTemplatePartLst.Tables[0].Rows[i]["IsPriceChange"].ToString());
                        PartRequest.UOMID = Convert.ToInt64(dsTemplatePartLst.Tables[0].Rows[i]["UOMID"].ToString());
                        PartRequest.UOM = uom;

                        objService.UpdatePartRequest_TempData12(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
                       // objService.UpdatePartRequest_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);

                        TemplatePartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                    }
                }

                Grid1.DataSource = TemplatePartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "GetTemplateDetails");
            }
            finally { objService.Close(); }
        }


        protected void Grid1_OnRebind(object sender, EventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Grid1.DataSource = null;
                Grid1.DataBind();
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                List<POR_SP_GetPartDetail_ForRequest_Result> RequestPartList = new List<POR_SP_GetPartDetail_ForRequest_Result>();
                if (hdn.Value == "")
                {
                    RequestPartList = objService.GetExistingTempDataBySessionIDObjectName(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr).ToList();
                }
                else if (hdn.Value != "")
                {
                    RequestPartList = objService.AddPartIntoRequest_TempData(hdn.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, 0, profile.DBConnection._constr).ToList();
                }

                ////Add by Suresh
                //if (hdnprodID.Value != "")
                //{
                //    RequestPartList = objService.AddPartIntoRequest_TempData(hdnprodID.Value, Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Convert.ToInt32(ddlSites.SelectedItem.Value), profile.DBConnection._constr).ToList();
                //    hdnprodID.Value = "";
                //}

                if (hdnChngDept.Value == "0x00x0")
                {
                    objService.ClearTempDataFromDBNEW(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    RequestPartList = null;
                }
                hdnChngDept.Value = "";
                var chngdpt = "1x1";
                hdnChngDept.Value = chngdpt;

                Grid1.DataSource = RequestPartList;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "Grid1_OnRebind");
            }
            finally { objService.Close(); }
        }

        protected void Grid1_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();

            try
            {
                if (e.Row.RowType == Obout.Grid.GridRowType.DataRow)
                {
                    //Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[6] as Obout.Grid.GridDataControlFieldCell;
                    //Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[7] as Obout.Grid.GridDataControlFieldCell;
                    Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[8] as Obout.Grid.GridDataControlFieldCell;

                    DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;
                    HiddenField hdnUOM = cell.FindControl("hdnMyUOM") as HiddenField;
                    //Label rowQtySpn = e.Row.Cells[9].FindControl("rowQtyTotal") as Label;
                    Label rowQtySpn = e.Row.Cells[10].FindControl("rowQtyTotal") as Label;

                    //TextBox txtUsrQty = e.Row.Cells[6].FindControl("txtUsrQty") as TextBox;
                    TextBox txtUsrQty = e.Row.Cells[7].FindControl("txtUsrQty") as TextBox;

                    int ProdID = Convert.ToInt32(e.Row.Cells[0].Text);
                    decimal moq = Convert.ToDecimal(e.Row.Cells[6].Text);

                    TextBox txtUsrPrice = e.Row.Cells[11].FindControl("txtUsrPrice") as TextBox;
                    Label rowPriceTotal = e.Row.Cells[12].FindControl("rowPriceTotal") as Label;

                    DataSet dsUOM = new DataSet();
                    dsUOM = objService.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);

                    ddl.DataSource = dsUOM;
                    ddl.DataTextField = "Description";
                    ddl.DataValueField = "UMOGroup";
                    ddl.DataBind();

                    decimal SelectedQty = 0, SelectedUOM = 0;
                    decimal Price = decimal.Parse(txtUsrPrice.Text.ToString());

                    //ddl.SelectedValue = e.Row.Cells[6].Text;
                    if (hdnTemplateID.Value != "")
                    {
                        long TemplID = Convert.ToInt64(hdnTemplateID.Value);
                        string selectedUom = objService.GetSelectedUomTemplate(TemplID, ProdID, profile.DBConnection._constr);
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(selectedUom.ToString()));
                        rowQtySpn.Text = txtUsrQty.Text;
                        rowPriceTotal.Text = e.Row.Cells[12].Text;
                    }
                    else
                    {
                        ddl.SelectedIndex = 2;

                        SelectedQty = decimal.Parse(dsUOM.Tables[0].Rows[2]["Quantity"].ToString());
                        SelectedUOM = decimal.Parse(dsUOM.Tables[0].Rows[2]["UOMID"].ToString());
                        decimal rowQty = decimal.Parse(txtUsrQty.Text.ToString());
                        decimal UsrQty = SelectedQty * rowQty;                        

                        hdnSelectedQty.Value = SelectedQty.ToString();
                        rowQtySpn.Text = UsrQty.ToString();
                    }

                    ddl.Attributes.Add("onchange", "javascript:GetIndex(this,'" + hdnUOM.ClientID.ToString() + "','" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");

                    txtUsrQty.Attributes.Add("onblur", "javascript:GetIndexQty(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + "," + Price + ",'" + rowPriceTotal.ClientID.ToString() + "'," + moq + ")");

                    txtUsrPrice.Attributes.Add("onblur", "javascript:GetChangedPrice(this," + SelectedQty + "," + SelectedUOM + ",'" + rowQtySpn.ClientID.ToString() + "','" + txtUsrQty.ClientID.ToString() + "'," + e.Row.RowIndex + ",'" + rowPriceTotal.ClientID.ToString() + "'," + ProdID + ")");
                    //}
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "Grid1_OnRowDataBound");
            }
            finally { objService.Close(); }
        }

        protected void Grid1_RowCommand(object sender, Obout.Grid.GridRowEventArgs e)
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();

            try
            {
                Obout.Grid.GridDataControlFieldCell cell = e.Row.Cells[6] as Obout.Grid.GridDataControlFieldCell;
                DropDownList ddl = cell.FindControl("ddlUOM") as DropDownList;

                ddl.Attributes.Add("onchange", "javascript:GetIndex('" + ddl.SelectedIndex + "'," + e.Row.RowIndex + ")");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "Grid1_RowCommand");
            }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WMUpdateRequestQty(object objRequest)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]);

                objService.UpdatePartRequest_TempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Request Template", "WMUpdateRequestQty");
            }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static void WMRemovePartFromRequest(Int32 Sequence)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                CustomProfile profile = CustomProfile.GetProfile();
                objService.RemovePartFromRequest_TempData(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, Sequence, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Request Template", "WMRemovePartFromRequest");
            }
            finally { objService.Close(); }
        }

        protected void GetTemplateList()
        {
            //select * from VW_GetTemplateDetails
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVRequest.DataSource = null;
            GVRequest.DataBind();

            DataSet dsTemplate = new DataSet();
            string UserType = profile.Personal.UserType.ToString();

            if (UserType == "Super Admin")
            {
                dsTemplate = objService.GetTemplateDetailsSuperAdmin(profile.DBConnection._constr);
            }
            else if (UserType == "Admin")
            {
                dsTemplate = objService.GetTemplateDetailsAdmin(profile.Personal.UserID, profile.DBConnection._constr);
            }
            //else
            //{
            //   // dsTemplate = objService.GetTemplateDetails(profile.Personal.UserID, profile.DBConnection._constr);
            //}
            //dsTemplate = objService.GetTemplateDetails(profile.Personal.UserID, profile.DBConnection._constr);

            GVRequest.DataSource = dsTemplate;
            GVRequest.DataBind();

        }

        protected void GVRequest_OnRebind(Object sender, EventArgs e)
        {
            GetTemplateList();
        }

        [WebMethod]
        public static List<mTerritory> WMGetDept(int Cmpny)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.UserType == "Admin")
            {
                SiteLst = UCCommonFilter.GetAddedDepartmentList(Cmpny, profile.Personal.UserID, profile.DBConnection._constr).ToList();
            }
            else
            {
                SiteLst = UCCommonFilter.GetDepartmentList(Cmpny, profile.DBConnection._constr).ToList();
            }
            return SiteLst;
        }

        [WebMethod]
        public static List<tContactPersonDetail> WMGetContactPersonLst(long Dept)
        {
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            ConPerLst = UCCommonFilter.GetContactPersonList(Dept, profile.DBConnection._constr).ToList();

            return ConPerLst;
        }

        [WebMethod]
        public static List<tContactPersonDetail> WMGetContactPerson2Lst(long Dept, long Cont1)
        {
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            ConPerLst = UCCommonFilter.GetContactPerson2List(Dept, Cont1, profile.DBConnection._constr).ToList();

            return ConPerLst;
        }

        [WebMethod]
        public static List<tAddress> WMGetDeptAddress(long Dept)
        {
            List<tAddress> AdrsLst = new List<tAddress>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            AdrsLst = UCCommonFilter.GetDeptAddressList(Dept, profile.DBConnection._constr).ToList();

            return AdrsLst;
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            txtTitle.Text = "";
            ddlAccessType.SelectedIndex = -1;
            ddlCompany.SelectedIndex = -1;
            ddlSites.SelectedIndex = -1;
            //ddlAddress.SelectedIndex = -1;
            //ddlContact1.SelectedIndex = -1;
            //ddlContact2.SelectedIndex = -1;
            txtRemark.Text = "";
            txtTitle.Focus();
            Grid1.DataSource = null;
            Grid1.DataBind();
        }

        [WebMethod]
        public static string WMGetDepartmentSession(string Dept)
        {
            Page objp = new Page();
            objp.Session["DeptID"] = Dept;
            // hdnselectedDept.Value = Dept;
            return Dept;
        }
        [WebMethod]
        public static void WMUpdRequestPart(object objRequest)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();

                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]);
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);

                objService.UpdatePartRequest_TempData1(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Request Template", "WMUpdRequestPart");
            }
            finally { objService.Close(); }
        }

        public List<mTerritory> WMGetSelDept(int Cmpny, long UserID)
        {
            List<mTerritory> SiteLst = new List<mTerritory>();
            iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            SiteLst = UCCommonFilter.GetAddedDepartmentList(Cmpny, UserID, profile.DBConnection._constr).ToList();

            return SiteLst;
        }

        [WebMethod]
        public static decimal WMGetTotal()
        {
            iPartRequestClient objService = new iPartRequestClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                tot = objService.GetTotalFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetTotal"); }
            finally { objService.Close(); }
            return tot;
        }

        [WebMethod]
        public static decimal WMGetTotalQty()
        {
            iPartRequestClient objService = new iPartRequestClient();
            decimal tot = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                tot = objService.GetTotalQTYFromTempData(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMGetTotalQty"); }
            finally { objService.Close(); }
            return tot;
        }
        [WebMethod]
        public static void WMUpdRequestPartPrice(object objRequest, int ProdID)
        {
            iPartRequestClient objService = new iPartRequestClient();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objRequest;
                CustomProfile profile = CustomProfile.GetProfile();
                string uom = objService.GetUOMName(Convert.ToInt64(dictionary["UOMID"]), profile.DBConnection._constr);
                POR_SP_GetPartDetail_ForRequest_Result PartRequest = new POR_SP_GetPartDetail_ForRequest_Result();
                PartRequest.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                PartRequest.RequestQty = Convert.ToDecimal(dictionary["RequestQty"]); PartRequest.UOM = uom;
                PartRequest.UOMID = Convert.ToInt64(dictionary["UOMID"]);
                PartRequest.Total = Convert.ToDecimal(dictionary["Total"]);
                PartRequest.Price = Convert.ToDecimal(dictionary["Price"]);
                // PartRequest.IsPriceChange = Convert.ToInt16(dictionary["IsPriceChange"]);
                decimal price = Convert.ToDecimal(dictionary["Price"]);
                int ISPriceChangedYN = objService.IsPriceChanged(ProdID, price, profile.DBConnection._constr);
                if (ISPriceChangedYN == 0) { PartRequest.IsPriceChange = 0; }
                else { PartRequest.IsPriceChange = 1; }
                objService.UpdatePartRequest_TempData12(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PartRequestEntry.aspx", "WMUpdRequestPart"); }
            finally { objService.Close(); }
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblreqtemplist.Text = rm.GetString("RequestTemplateList", ci);
                tbTemplateLst.HeaderText = rm.GetString("RequestTemplateList", ci);
                tbTemplateDetail.HeaderText = rm.GetString("TemplateDetail", ci);
                lblTitle.Text = rm.GetString("Title", ci);
                //   lblDate.Text = rm.GetString("Date", ci);
                //  lblTemplateCreatedby.Text = rm.GetString("TemplateCreatedBy", ci);
                lblAccessType.Text = rm.GetString("AccessType", ci);
                lblDepartment.Text = rm.GetString("Department", ci);
                //   lblAddress.Text = rm.GetString("Address", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("RequestTemplate", ci);
                //  lblContact.Text = rm.GetString("Contact", ci);
                lblRemark.Text = rm.GetString("Remark", ci);
                lblskulist.Text = rm.GetString("SKUList", ci);
                lblCustomerName.Text = rm.GetString("CustomerName", ci);
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Request Template", "loadstring");
            }

        }

    }
}