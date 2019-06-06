using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using Obout.Interface;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServiceSiteMaster;
using BrilliantWMS.PORServiceSiteMaster;
namespace BrilliantWMS.POR
{
    public partial class SiteMaster : System.Web.UI.Page
    {
        static string sessionID { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sessionID = Session.SessionID.ToString();
                UCFormHeader1.FormHeaderText = "Site Master"; BindGrid();
                if (!IsPostBack)
                {
                    BindGrid(); clear();
                }
                this.UCToolbar1.ToolbarAccess("SiteMaster");
                this.UCToolbar1.evClickAddNew += pageAddNew;
                this.UCToolbar1.evClickSave += pageSave;
                this.UCToolbar1.evClickClear += pageClear;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }
        protected void BindGrid()
        {
            try
            {
                iSiteMasterClient SiteMasterClient = new iSiteMasterClient();
                CustomProfile profile = CustomProfile.GetProfile();
                GvSite.DataSource = SiteMasterClient.GetmTerritoryList(profile.DBConnection._constr);
                GvSite.DataBind();
                SiteMasterClient.GetmTerritoryList(profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void clear()
        {
            txtSiteName.Text = ""; txtCAddress1.Text = ""; txtCity.Text = ""; txtemailid.Text = ""; txtFax.Text = ""; HdnSiteId.Value = null; hdnCountry.Value = null; hdnState.Value = null;
            txtLandMark.Text = ""; txtphoneno.Text = ""; txtZipCode.Text = ""; ddlCountry.SelectedIndex = -1; ddlState.SelectedIndex = -1;
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mTerritory objmTerritory = new mTerritory();
                tAddress objtAddress = new tAddress();
                iSiteMasterClient SiteMasterClient = new iSiteMasterClient();
                if (HdnSiteId.Value != "")
                {
                    v_GetSiteDetails SiteDtls = new v_GetSiteDetails();
                    SiteDtls = SiteMasterClient.GetTerritoryListByID(Convert.ToInt64(HdnSiteId.Value), profile.DBConnection._constr);
                    objtAddress.CreatedBy = SiteDtls.Address_CreatedBy;
                    objtAddress.CreationDate = SiteDtls.Address_CreationDate;
                    objtAddress.ReferenceID = SiteDtls.ReferenceID;
                    objtAddress.ID = SiteDtls.AddressID;
                    objmTerritory.CreatedBy = SiteDtls.CreatedBy;
                    objmTerritory.CreationDate = SiteDtls.CreationDate;

                }
                else
                {
                    objtAddress.LastModifiedBy = profile.Personal.UserID.ToString();
                    objtAddress.LastModifiedDate = DateTime.Now;
                    objmTerritory.LastModifiedBy = profile.Personal.UserID.ToString();
                    objmTerritory.LastModifiedDate = DateTime.Now;
                }

                objtAddress.AddressLine1 = null;
                if (txtCAddress1.Text.ToString().Trim() != "") { objtAddress.AddressLine1 = txtCAddress1.Text; }

                objtAddress.County = hdnCountry.Value;
                objtAddress.State = hdnState.Value;

                objtAddress.City = null;
                if (txtCity.Text.ToString().Trim() != "") { objtAddress.City = txtCity.Text; }

                objtAddress.Zipcode = null;
                if (txtZipCode.Text.ToString().Trim() != "") { objtAddress.Zipcode = txtZipCode.Text; }

                objtAddress.Landmark = null;
                if (txtLandMark.Text.ToString().Trim() != "") { objtAddress.Landmark = txtLandMark.Text; }

                objtAddress.EmailID = null;
                if (txtemailid.Text.ToString().Trim() != "") { objtAddress.EmailID = txtemailid.Text; }

                objtAddress.PhoneNo = null;
                if (txtphoneno.Text.ToString().Trim() != "") { objtAddress.PhoneNo = txtphoneno.Text; }

                objtAddress.FaxNo = null;
                if (txtFax.Text.ToString().Trim() != "") { objtAddress.FaxNo = txtFax.Text; }

                objtAddress.Active = "Y";

                objtAddress.AddressType = "none";

                objtAddress.ObjectName = "Site";

                objtAddress.CompanyID = profile.Personal.CompanyID;

                objmTerritory.Territory = null;
                if (txtSiteName.Text.ToString().Trim() != "") { objmTerritory.Territory = txtSiteName.Text; }

                objmTerritory.ParentID = null;
                objmTerritory.ParentID = 1; //For HQ

                objmTerritory.Level = null;
                objmTerritory.Level = 2;

                objmTerritory.GroupTitle = null;
                objmTerritory.GroupTitle = "Site";

                if (HdnSiteId.Value == string.Empty)
                {
                    objmTerritory.CreatedBy = profile.Personal.UserID.ToString();
                    objmTerritory.CreationDate = DateTime.Now;
                    objtAddress.CreatedBy = profile.Personal.UserID.ToString();
                    objtAddress.CreationDate = DateTime.Now;
                    long result = SiteMasterClient.InsertSiteMaster(objmTerritory, profile.DBConnection._constr);
                    objtAddress.ReferenceID = result;
                    long result1 = SiteMasterClient.InsertSiteAddress(objtAddress, profile.DBConnection._constr);
                    if (result != 0 && result1 != 0)
                    {
                        WebMsgBox.MsgBox.Show("Record saved successfully");
                    }
                }
                else
                {
                    objmTerritory.ID = Convert.ToInt64(HdnSiteId.Value);
                    objmTerritory.LastModifiedBy = profile.Personal.UserID.ToString();
                    objmTerritory.LastModifiedDate = DateTime.Now;
                    objtAddress.LastModifiedBy = profile.Personal.UserID.ToString();
                    objtAddress.LastModifiedDate = DateTime.Now;
                    long result = SiteMasterClient.updatemTerritory(objmTerritory, profile.DBConnection._constr);

                    long result1 = SiteMasterClient.updateSiteAddress(objtAddress, profile.DBConnection._constr);
                    if (result != 0 && result1 != 0)
                    {
                        WebMsgBox.MsgBox.Show("Record Update successfully");
                    }
                }
                BindGrid();
                clear();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void GvSite_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)GvSite.SelectedRecords[0];
                HdnSiteId.Value = selectedrec["ID"].ToString();
                iSiteMasterClient SiteMasterClient = new iSiteMasterClient();
                v_GetSiteDetails objv_GetSiteDetails = new v_GetSiteDetails();
                objv_GetSiteDetails = SiteMasterClient.GetTerritoryListByID(Convert.ToInt32(HdnSiteId.Value), profile.DBConnection._constr);

                if (objv_GetSiteDetails.AddressLine1 != null) { txtCAddress1.Text = objv_GetSiteDetails.AddressLine1; }

                if (objv_GetSiteDetails.County != null) { hdnCountry.Value = objv_GetSiteDetails.County; }
                if (objv_GetSiteDetails.State != null) { hdnState.Value = objv_GetSiteDetails.State; }
                Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + objv_GetSiteDetails.County + "','" + objv_GetSiteDetails.State + "');", true);
                if (objv_GetSiteDetails.City != null) { txtCity.Text = objv_GetSiteDetails.City; }
                if (objv_GetSiteDetails.Zipcode != null) { txtZipCode.Text = objv_GetSiteDetails.Zipcode; }
                if (objv_GetSiteDetails.Landmark != null) { txtLandMark.Text = objv_GetSiteDetails.Landmark; }
                if (objv_GetSiteDetails.EmailID != null) { txtemailid.Text = objv_GetSiteDetails.EmailID; }
                if (objv_GetSiteDetails.PhoneNo != null) { txtphoneno.Text = objv_GetSiteDetails.PhoneNo; }
                if (objv_GetSiteDetails.FaxNo != null) { txtFax.Text = objv_GetSiteDetails.FaxNo; }
                if (objv_GetSiteDetails.Territory != null) { txtSiteName.Text = objv_GetSiteDetails.Territory; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }


    }
}