using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.ProductUOMService;
//using PowerOnRentwebapp.ProductUOMService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;

namespace BrilliantWMS.Product
{
    public partial class UOM : System.Web.UI.Page
    {
        BrilliantWMS.ProductUOMService.iProductUOMClient  ProductUOMClient = new BrilliantWMS.ProductUOMService.iProductUOMClient();
        
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Product UOM Master";
            if (!IsPostBack)
            {
                BindGrid();
                hdnPrdUOMID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("ProductUOMMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvPrdUOM.DataSource = ProductUOMClient.GetProductUOMList(profile.DBConnection._constr);
                gvPrdUOM.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product UOM Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void clear()
        {
            TxtUOM.Text = "";
            TxtSequence.Text = "";
            hdnPrdUOMID.Value = null;
            TxtUOM.Focus();
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { 
            clear();
            TxtUOM.Focus();
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (checkDuplicate() == "")
                {
                    mUOM ObjPrdUOM = new mUOM();
                    if (hdnPrdUOMID.Value == string.Empty)
                    {
                        ObjPrdUOM.Name = TxtUOM.Text.Trim();
                        if (TxtSequence.Text != string.Empty)
                        { ObjPrdUOM.Sequence = Convert.ToInt64(TxtSequence.Text); }
                        else
                        { ObjPrdUOM.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjPrdUOM.Active = "Y"; }
                        else
                        { ObjPrdUOM.Active = "N"; }
                        ObjPrdUOM.CreatedBy = profile.Personal.UserID.ToString();
                        ObjPrdUOM.CreationDate = DateTime.Now;

                        ObjPrdUOM.CompanyID = profile.Personal.CompanyID;
                        int result = ProductUOMClient.InsertmProductUOM(ObjPrdUOM, profile.DBConnection._constr);
                        
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");
                        }
                        BindGrid();
                        clear();
                    }

                    else
                    {
                        ObjPrdUOM = ProductUOMClient.GetProductUOMListByID (Convert.ToInt32(hdnPrdUOMID.Value), profile.DBConnection._constr);
                        ObjPrdUOM.Name = TxtUOM.Text.Trim();
                        if (TxtSequence.Text != string.Empty)
                        { ObjPrdUOM.Sequence = Convert.ToInt64(TxtSequence.Text); }
                        else
                        { ObjPrdUOM.Sequence = 0; }
                        if (rbtnYes.Checked == true)
                        { ObjPrdUOM.Active = "Y"; }
                        else
                        { ObjPrdUOM.Active = "N"; }
                        ObjPrdUOM.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjPrdUOM.LastModifiedDate = DateTime.Now;
                        int result = ProductUOMClient.updatemProductUOM(ObjPrdUOM, profile.DBConnection._constr);
                        
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record updated successfully");
                        }
                        BindGrid();
                        clear();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product UOM Master", "pageSave");
            }
            finally
            {
            }
        }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnPrdUOMID.Value == string.Empty)
                {
                    result = ProductUOMClient.checkDuplicateRecord(TxtUOM.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        TxtUOM.Text = "";
                    }
                    TxtUOM.Focus();
                }
                else
                {
                    result = ProductUOMClient.checkDuplicateRecordEdit(TxtUOM.Text.Trim(), Convert.ToInt32(hdnPrdUOMID.Value), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        TxtUOM.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product UOM Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { 
            clear();
        }

        protected void gvPrdUOM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvPrdUOM.SelectedRecords[0];
                hdnPrdUOMID.Value = selectedrec["ID"].ToString();
                TxtSequence.Text = selectedrec["Sequence"].ToString();
                TxtUOM.Text = selectedrec["Name"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product UOM Master", "gvPrdUOM_Select");
            }
            finally
            {
            }
        }
    }
}