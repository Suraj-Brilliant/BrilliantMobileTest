using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PopupMessages;
using System.Web.DynamicData;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
/*using BrilliantWMS.DiscountMasterService;*/
using System.Web.Services;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;

namespace BrilliantWMS.Product
{
    public partial class DiscountMaster : System.Web.UI.Page
    {
       /* DiscountMasterService.iDiscountMasterClient DiscountClient = new DiscountMasterService.iDiscountMasterClient();*/
        public Page ParentForm { get; set; }
        static string sessionID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCProductSearch1.ParentPage = this;

            UC_FromDate.DateIsRequired(true, "Save", "Please select Start Date");
            UC_FromDate.startdate(DateTime.Now);
            UC_ToDate.DateIsRequired(false, "x", "");
            UC_ToDate.startdate(DateTime.Now);
            sessionID = Session.SessionID;
            UCFormHeader1.FormHeaderText = "Discount Master";
            TextBox txtstartdate = (TextBox)UC_FromDate.FindControl("txtDate");
            TextBox txtenddate = (TextBox)UC_ToDate.FindControl("txtDate");

            txtstartdate.Attributes.Add("onchange", "validateDate('" + txtstartdate.ClientID + "','" + txtenddate.ClientID + "','Start'),'From Date Should Not Be Less Than End Date'");
            txtenddate.Attributes.Add("onchange", "validateDate('" + txtstartdate.ClientID + "','" + txtenddate.ClientID + "','End','To Date Should Be Greater Than Start Date')");
            if (!IsPostBack)
            {
                BindMainGrid();
                hdnDiscountID.Value = "";
            }
            this.UCToolbar1.ToolbarAccess("DiscountMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindMainGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               /* gvDiscountM.DataSource = DiscountClient.GetDiscountRecordToBindGrid(profile.DBConnection._constr);
                gvDiscountM.DataBind();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "BindMainGrid");
            }
            finally
            {
            }
        }

        protected void clear()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            txtDiscount.Text = "";
            //txtSequence.Text = "";
            UC_FromDate.Date = null;
            UC_ToDate.Date = null;
            hdnDiscountID.Value = null;
            UC_FromDate.Date = null;
            UC_ToDate.Date = null;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
            Grid1.DataSource = null;
            Grid1.DataBind();
            HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
            hdn.Value = "";
           /* DiscountClient.ClearTempDataFromDB(sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);*/
        }

        protected void eventSelectedProductIDs()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnProductSearchSelectedRec");
                Grid1.DataSource = null;
                Grid1.DataBind();
                if (hdn.Value != string.Empty)
                {
                    string[] strings = new string[] { };
                    strings = hdn.Value.Split(',');
                    long[] arrayIDs = strings.Select(x => long.Parse(x)).ToArray();
                    if (hdnDiscountID.Value == string.Empty)
                    {
                       /* Grid1.DataSource = DiscountClient.CreateProductTempDataList(arrayIDs, Session.SessionID.ToString(), 0, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                        Grid1.DataBind();*/
                    }
                    else
                    {
                        /*Grid1.DataSource = DiscountClient.CreateProductTempDataList(arrayIDs, Session.SessionID.ToString(), Convert.ToInt64(hdnDiscountID.Value), profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                        Grid1.DataBind();*/
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "eventSelectedProductIDs");
            }
            finally
            {
            }
        }

        protected void RebindGrid(object sender, EventArgs e)
        { BindGrid(); }

        protected void BindGrid()
        { eventSelectedProductIDs(); }

        protected void imgbtnRemove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ImageButton imgbtn = (ImageButton)sender;
                Grid1.DataSource = null;
                Grid1.DataBind();
                /*Grid1.DataSource = DiscountClient.RemoveProductFromTempDataList(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), Convert.ToInt32(imgbtn.ToolTip), profile.DBConnection._constr);
                Grid1.DataBind();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "imgbtnRemove_Click");
            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, ToolbarService.iUCToolbarClient e)
        {
            clear();
            tabDiscountM.ActiveTabIndex = 2;
        }

        protected void pageSave(Object sender, ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (checkDuplicate() == "")
                {
                   /* tDiscountHead ObjDis = new tDiscountHead();
                    if (hdnDiscountID.Value == string.Empty)
                    {
                        ObjDis.Name = txtDiscount.Text;
                        //if (txtSequence.Text != string.Empty)
                        //{ ObjDis.Sequence = Convert.ToInt64(txtSequence.Text); }
                        //else
                        //{ ObjDis.Sequence = 0; }
                        ObjDis.Sequence = 0;
                        if (rbtnYes.Checked == true)
                        { ObjDis.Active = "Y"; }
                        else
                        { ObjDis.Active = "N"; }
                        ObjDis.FromDate = UC_FromDate.Date.Value;
                        if (UC_ToDate.Date != null)
                        { ObjDis.ToDate = UC_ToDate.Date.Value; }
                        else { ObjDis.ToDate = null; }
                        ObjDis.CreatedBy = profile.Personal.UserID.ToString();
                        ObjDis.CreationDate = DateTime.Now;
                        ObjDis.CompanyID = profile.Personal.CompanyID;
                        int result = DiscountClient.InserttDiscountHead(ObjDis, profile.DBConnection._constr);
                        if (Grid1.TotalRowCount != 0)
                        { DiscountClient.FinalSaveToDBtDiscountMappingDetails(Session.SessionID.ToString(), result, profile.Personal.UserID.ToString(), profile.DBConnection._constr); }
                        if (result != null)
                        { WebMsgBox.MsgBox.Show("Record saved successfully"); }
                        BindMainGrid();
                        clear();
                        tabDiscountM.ActiveTabIndex = 0;
                    }
                    else
                    {
                        ObjDis = DiscountClient.GetDiscountListByID(Convert.ToInt32(hdnDiscountID.Value), profile.DBConnection._constr);
                        ObjDis.Name = txtDiscount.Text;
                        //if (txtSequence.Text != string.Empty)
                        //{ ObjDis.Sequence = Convert.ToInt64(txtSequence.Text); }
                        //else
                        //{ ObjDis.Sequence = 0; }
                        ObjDis.Sequence = 0;
                        if (rbtnYes.Checked == true)
                        { ObjDis.Active = "Y"; }
                        else
                        { ObjDis.Active = "N"; }
                        ObjDis.FromDate = UC_FromDate.Date.Value;
                        if (UC_ToDate.Date != null)
                        { ObjDis.ToDate = UC_ToDate.Date.Value; }
                        else { ObjDis.ToDate = null; }
                        ObjDis.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjDis.LastModifiedDate = DateTime.Now;
                        int result = DiscountClient.UpdatetDiscountHead(ObjDis, profile.DBConnection._constr);
                        DiscountClient.FinalSaveToDBtDiscountMappingDetails(Session.SessionID.ToString(), Convert.ToInt64(hdnDiscountID.Value), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");
                        }
                        BindMainGrid();
                        clear();
                        tabDiscountM.ActiveTabIndex = 0;
                    }*/
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnDiscountID.Value == string.Empty)
                {
                   /* result = DiscountClient.checkDuplicateRecord(txtDiscount.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDiscount.Text = "";
                    }*/
                    // txtSequence.Focus();
                }
                else
                {
                   /* result = DiscountClient.checkDuplicateRecordEdit(txtDiscount.Text.Trim(), Convert.ToInt32(hdnDiscountID.Value), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtDiscount.Text = "";
                    }*/
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }

        }

        protected void UpdateRecord(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                HiddenField hdn = (HiddenField)UCProductSearch1.FindControl("hdnSelectedRec");
                hdn.Value = "";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "UpdateRecord");

            }
            finally
            {
            }
        }



        [WebMethod]
        public static void UpdateOrder(object order)
        {
            CustomProfile profile = CustomProfile.GetProfile();
           /* DiscountMasterService.iDiscountMasterClient DiscountClient = new DiscountMasterService.iDiscountMasterClient();*/
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)order;

           /* SP_GetProductListForDiscountMaster_Result updateRec = new SP_GetProductListForDiscountMaster_Result();
            updateRec.Sequence = Convert.ToInt64(dictionary["Sequence"]);
            updateRec.ProductCode = dictionary["ProductCode"].ToString();
            updateRec.ProductName = dictionary["ProductName"].ToString();
            updateRec.UOM = dictionary["UOM"].ToString();
            updateRec.DiscountRate = Convert.ToDecimal(dictionary["DiscountRate"]);
            updateRec.IsDiscountPercent = Convert.ToBoolean(dictionary["IsDiscountPercent"].ToString());
            updateRec.MinOrderQuantity = Convert.ToInt32(dictionary["MinOrderQuantity"]);
            updateRec.PrincipalPrice = Convert.ToDecimal(dictionary["PrincipalPrice"]);
            updateRec.AmountAfterDiscount = Convert.ToDecimal(dictionary["AmountAfterDiscount"]);
            updateRec.Active = (dictionary["Active"]).ToString();
            if (updateRec.Active == "Y")
            { updateRec.Active = "Y"; }
            else { updateRec.Active = "N"; }

            DiscountClient.UpdateRecord(sessionID, profile.Personal.UserID.ToString(), updateRec, profile.DBConnection._constr);*/
        }

        protected void gvDiscountM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvDiscountM.SelectedRecords[0];
                hdnDiscountID.Value = selectedrec["ID"].ToString();
                //txtSequence.Text = selectedrec["Sequence"].ToString();
                txtDiscount.Text = selectedrec["Name"].ToString();
                UC_FromDate.Date = Convert.ToDateTime(selectedrec["FromDate"].ToString());
                if (selectedrec["ToDate"].ToString() == "")
                { UC_ToDate.Date = null; }
                else
                { UC_ToDate.Date = Convert.ToDateTime(selectedrec["ToDate"].ToString()); }
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
               /* Grid1.DataSource = DiscountClient.GetDiscountListByDiscountID(sessionID, Convert.ToInt64(hdnDiscountID.Value), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                Grid1.DataBind();*/
                tabDiscountM.ActiveTabIndex = 2;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Discount Master", "gvDiscountM_Select");

            }
            finally
            {
            }
        }
    }
}