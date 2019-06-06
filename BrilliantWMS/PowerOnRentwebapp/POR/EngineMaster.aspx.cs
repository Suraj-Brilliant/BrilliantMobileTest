using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.PORServiceSiteMaster;


namespace BrilliantWMS.POR
{
    public partial class EngineMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UCFormHeader1.FormHeaderText = "Engine Master";
                if (!IsPostBack)
                {
                    clear();
                    BindGrid();
                    BindDropDown();
                    SetValueToUserControl();
                }
                this.UCToolbar1.ToolbarAccess("EngineMaster");
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


        public void BindGrid()
        {
            try
            {
                iEngineMasterClient EngineClient = new iEngineMasterClient();
                CustomProfile profile = CustomProfile.GetProfile();
                GvMEngine.DataSource = EngineClient.GetEngineMasterList(profile.DBConnection._constr);
                GvMEngine.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Engine Master", "BindGrid");
            }
            finally
            {
            }
        }

        public void BindDropDown()
        {
            try
            {
                iSiteMasterClient SiteMasterClient = new iSiteMasterClient();
                CustomProfile profile = CustomProfile.GetProfile();
                ddlSite.DataSource = SiteMasterClient.GetSiteDtls(profile.DBConnection._constr);
                ddlSite.DataBind();
                ListItem lst1 = new ListItem();
                lst1.Text = "-Select-";
                lst1.Value = "0";
                ddlSite.Items.Insert(0, lst1);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }

        public void clear()
        {
            try
            {
                txtContainer.Text = "";
                txtEngineModel.Text = "";
                txtEngineSerialNo.Text = "";
                txtGenratorModel.Text = "";
                txtGenratorSerialNo.Text = "";
                txtRemark.Text = ""; txtTransformerSerialNo.Text = "";
                ddlSite.SelectedIndex = -1; hdnEngineId.Value = null; UC_DateRecived.Date = null;
                UC_TrasformerDateRecv.Date = null;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }

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
                mEngine objEngine = new mEngine();
                iEngineMasterClient EngineClient = new iEngineMasterClient();

                if (hdnEngineId.Value != "")
                {
                    v_GetEngineDetails obj = new v_GetEngineDetails();
                    obj = EngineClient.GetmEngineListByID(Convert.ToInt32(hdnEngineId.Value), profile.DBConnection._constr);
                    objEngine.CreatedBy = obj.CreatedBy;
                    objEngine.CreationDate = obj.CreationDate;
                }
                else
                {
                    objEngine.LastModifiedBy = profile.Personal.UserID.ToString();
                    objEngine.LastModifiedDate = DateTime.Now;
                }
                if (ddlSite.SelectedIndex > 0) { objEngine.SiteID =Convert.ToInt32(ddlSite.SelectedItem.Value); }

                objEngine.Container = null;
                if (txtContainer.Text.ToString().Trim() != "") { objEngine.Container = txtContainer.Text.ToString(); }

                objEngine.GeneratorModel = null;
                if (txtGenratorModel.Text.ToString().Trim() != "") { objEngine.GeneratorModel = txtGenratorModel.Text.ToString(); }

                objEngine.GeneratorSerial = null;
                if (txtGenratorSerialNo.Text.ToString().Trim() != "") { objEngine.GeneratorSerial = txtGenratorSerialNo.Text.ToString(); }

                objEngine.EngineModel = null;
                if (txtEngineModel.Text.ToString().Trim() != "") { objEngine.EngineModel = txtEngineModel.Text.ToString(); }

                objEngine.EngineSerial = null;
                if (txtEngineSerialNo.Text.ToString().Trim() != "") { objEngine.EngineSerial = txtEngineSerialNo.Text.ToString(); }

                objEngine.EngineSerial = null;
                if (txtEngineSerialNo.Text.ToString().Trim() != "") { objEngine.EngineSerial = txtEngineSerialNo.Text.ToString(); }

                objEngine.TransformerSerial = null;
                if (txtTransformerSerialNo.Text.ToString().Trim() != "") { objEngine.TransformerSerial = txtTransformerSerialNo.Text.ToString(); }

                if (UC_DateRecived.Date != null) { objEngine.DateRecevied = Convert.ToDateTime(UC_TrasformerDateRecv.Date); }

                if (UC_TrasformerDateRecv.Date != null) { objEngine.TransformerDateRecevied = Convert.ToDateTime(UC_DateRecived.Date); }

                objEngine.Remark = null;
                if (txtRemark.Text.ToString().Trim() != "") { objEngine.Remark = txtRemark.Text.ToString(); }

                if (hdnEngineId.Value == string.Empty)
                {
                    objEngine.CreatedBy = profile.Personal.UserID.ToString();
                    objEngine.CreationDate = DateTime.Now;
                    int result = EngineClient.InsertmEngine(objEngine, profile.DBConnection._constr);
                    if (result == 1)
                    {
                        WebMsgBox.MsgBox.Show("Record saved successfully");
                    }
                }
                else
                {
                    objEngine.ID = Convert.ToInt64(hdnEngineId.Value);
                    objEngine.LastModifiedBy = profile.Personal.UserID.ToString();
                    objEngine.LastModifiedDate = DateTime.Now;
                    int result = EngineClient.updatemEngine(objEngine, profile.DBConnection._constr);
                    if (result == 1)
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


        protected void GvEngine_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)GvMEngine.SelectedRecords[0];
                hdnEngineId.Value = selectedrec["ID"].ToString();
                iEngineMasterClient EngineClient = new iEngineMasterClient();
                v_GetEngineDetails objv_GetEngineDetails = new v_GetEngineDetails();
                objv_GetEngineDetails = EngineClient.GetmEngineListByID(Convert.ToInt32(hdnEngineId.Value), profile.DBConnection._constr);
                if (objv_GetEngineDetails.Territory != null) { ddlSite.SelectedIndex = ddlSite.Items.IndexOf(ddlSite.Items.FindByValue(objv_GetEngineDetails.SiteID.ToString())); }
                if (objv_GetEngineDetails.EngineSerial != null) { txtEngineSerialNo.Text = objv_GetEngineDetails.EngineSerial; }
                if (objv_GetEngineDetails.Container != null) { txtContainer.Text = objv_GetEngineDetails.Container; }
                if (objv_GetEngineDetails.EngineModel != null) { txtEngineModel.Text = objv_GetEngineDetails.EngineModel; }
                if (objv_GetEngineDetails.GeneratorSerial != null) { txtGenratorSerialNo.Text = objv_GetEngineDetails.GeneratorSerial; }
                if (objv_GetEngineDetails.GeneratorModel != null) { txtGenratorModel.Text = objv_GetEngineDetails.GeneratorModel; }
                if (objv_GetEngineDetails.TransformerSerial != null) { txtTransformerSerialNo.Text = objv_GetEngineDetails.TransformerSerial; }
                if (objv_GetEngineDetails.Remark != null) { txtRemark.Text = objv_GetEngineDetails.Remark; }
                if (objv_GetEngineDetails.DateRecevied != null) { UC_DateRecived.Date = objv_GetEngineDetails.DateRecevied; }
                if (objv_GetEngineDetails.TransformerDateRecevied != null) { UC_TrasformerDateRecv.Date = objv_GetEngineDetails.TransformerDateRecevied; }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Site Master", "pageSave");
            }
            finally
            {
            }
        }

        protected void SetValueToUserControl()
        {
            UC_TrasformerDateRecv.DateIsRequired(true, "Save", "Select Trasformer Date");
            UC_DateRecived.DateIsRequired(true, "Save", "Select Recived Date");
        }


    }
}