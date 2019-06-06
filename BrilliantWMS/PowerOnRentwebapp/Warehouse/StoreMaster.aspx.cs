using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;

namespace BrilliantWMS.Warehouse
{
    public partial class StoreMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setActiveTab(0);
                bindDeptgrid(10095);
            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }



        public void bindDeptgrid(long ID)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                // ds = company.GetDepartmentListforgrid(ID, profile.DBConnection._constr);
                ds = company.GetDeptListWithSLA(ID, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    Grid1.DataSource = ds.Tables[0];
                    Grid1.DataBind();
                }
                else
                {
                    Grid1.DataSource = null;
                    Grid1.DataBind();

                }
            }
            catch { }
            finally
            {
                company.Close();
            }
        }

        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
            if (btnSave != null)
                if (ActiveTab == 0)
                {
                    TabCustomerList.Visible = true;
                    tabStoreInfo.Visible = false;
                    tabStoreMaster.ActiveTabIndex = 0;
                }
                else
                {
                    TabCustomerList.Visible = true;
                    tabStoreInfo.Visible = true;
                    tabStoreMaster.ActiveTabIndex = 1;
                }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
            setActiveTab(1);
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {

        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
        }

        public void clear()
        {

        }


        protected void imgBtnEditbom_OnClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                chkDeliver.Checked = false;
                ImageButton imgbtn = (ImageButton)sender;
                CustomProfile profile = CustomProfile.GetProfile();
               // Hashtable selectedrec = (Hashtable)Grid1.SelectedRecords[0];
               // hdnSelectedDepartment.Value = selectedrec["ID"].ToString();
                hdnSelectedDepartment.Value = imgbtn.ToolTip.ToString();
                hdnSelectedDepartment.Value = hdnPartSelectedRec.Value;
                imgSearch.Visible = true;
                chkpricechange.Enabled = true;
                //Session.Add("editstate", Edit);
               // Getpettycash();
                long reuslt = long.Parse(hndCompanyid.Value);
                if (hdnmodestate.Value == "Edit")
                {
                    //FillUserControl(reuslt);
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "Grid1_Select");
            }
            finally
            {
            }
        }
    }
}