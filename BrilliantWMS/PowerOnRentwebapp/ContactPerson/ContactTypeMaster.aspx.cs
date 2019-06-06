using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.ContactTypeService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;

namespace BrilliantWMS.ContactPerson
{
    public partial class ContactTypeMaster : System.Web.UI.Page
    {
        iContactTypeMasterClient ContactTypeClient = new iContactTypeMasterClient();

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            UCFormHeader1.FormHeaderText = "Contact Type Master";
            if (!IsPostBack)
            {
                BindGrid();
                hdnContactTypeID.Value = null;
            }
            this.UCToolbar1.ToolbarAccess("Lead");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvContact.DataSource = ContactTypeClient.GetContactTypeToBind(profile.DBConnection._constr);
                gvContact.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, Page, "Contact Type Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void clear()
        {
            txtContactType.Text = "";
            txtRemark.Text = "";
            txtSequence.Text = "";
            hdnContactTypeID.Value = null;
            rbtnYes.Checked = true;
            rbtnNo.Checked = false;
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";

                if (hdnContactTypeID.Value == string.Empty)
                {
                    result = ContactTypeClient.checkDuplicateRecord(txtContactType.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtContactType.Text = "";
                    }
                    txtSequence.Focus();
                }
                else
                {
                    result = ContactTypeClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnContactTypeID.Value), txtContactType.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtContactType.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Contact Type Master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (checkDuplicate() == "")
                {
                    mContactType ObjContactType = new mContactType();
                    if (hdnContactTypeID.Value == string.Empty)
                    {
                        ObjContactType.ContactType = txtContactType.Text;
                        if (txtSequence.Text == string.Empty)
                        { ObjContactType.Sequence = 0; }
                        else
                        { ObjContactType.Sequence = Convert.ToInt64(txtSequence.Text); }
                        if (rbtnYes.Checked == true)
                        { ObjContactType.Active = "Y"; }
                        else
                        { ObjContactType.Active = "N"; }
                        ObjContactType.CreatedBy = profile.Personal.UserID.ToString();
                        ObjContactType.CreationDate = DateTime.Now;

                        ObjContactType.Remark = txtRemark.Text;
                        ObjContactType.CompanyID = profile.Personal.CompanyID;
                        int result = ContactTypeClient.InsertmContactType(ObjContactType, profile.DBConnection._constr);
                        if (result == 1)
                        {
                            WebMsgBox.MsgBox.Show("Record saved successfully");
                        }
                        BindGrid();
                        clear();
                    }
                    else
                    {
                        ObjContactType = ContactTypeClient.GetContactTypeListByID(Convert.ToInt32(hdnContactTypeID.Value), profile.DBConnection._constr);
                        ObjContactType.ContactType = txtContactType.Text;
                        ObjContactType.Remark = txtRemark.Text;
                        if (txtSequence.Text == string.Empty)
                        { ObjContactType.Sequence = 0; }
                        else
                        { ObjContactType.Sequence = Convert.ToInt64(txtSequence.Text); }
                        if (rbtnYes.Checked == true)
                        { ObjContactType.Active = "Y"; }
                        else
                        { ObjContactType.Active = "N"; }
                        ObjContactType.LastModifiedBy = profile.Personal.UserID.ToString();
                        ObjContactType.LastModifiedDate = DateTime.Now;
                        int result = ContactTypeClient.UpdatemContactType(ObjContactType, profile.DBConnection._constr);
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
                Login.Profile.ErrorHandling(ex, Page, "Contact Type Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }


        protected void gvContact_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)gvContact.SelectedRecords[0];
                hdnContactTypeID.Value = selectedrec["ID"].ToString();
                txtSequence.Text = selectedrec["Sequence"].ToString();
                txtContactType.Text = selectedrec["ContactType"].ToString();
                txtRemark.Text = selectedrec["Remark"].ToString();
                if (selectedrec["Active"].ToString() == "No")
                { rbtnNo.Checked = true; }
                else
                { rbtnYes.Checked = true; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, Page, "Contact Type Master", "gvContact_Select");
            }
            finally
            {
            }
        }
    }
}