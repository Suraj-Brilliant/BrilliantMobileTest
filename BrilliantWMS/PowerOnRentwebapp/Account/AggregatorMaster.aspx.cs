using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Web.Services;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using System.Collections;

namespace BrilliantWMS.Account
{
    public partial class AggregatorMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setActiveTab(0);
                BindAgregatorGridMain();
                FillCompany();
            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindAgregatorGridMain()
        {
            iCompanySetupClient aggregator = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdaggregator.DataSource = aggregator.GetAggregatorList(profile.DBConnection._constr);
                grdaggregator.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Aggrgator Master", "BindAgregatorGridMain");
            }
            finally
            {
                aggregator.Close();
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
            setActiveTab(1);
            btncustomernext.Visible = true;
        }

        protected void btncustomernext_Click(object sender, System.EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCompanySetupClient aggregator = new iCompanySetupClient();
            try
            {
                mAgreegator aggree = new mAgreegator();
                aggree.AgreegatorName = txtaggregator.Text;
                aggree.ContactPersonName = txtcontactperson.Text;
                aggree.EmailID = txtemail.Text;
                aggree.MobileNo = long.Parse(txtmobno.Text);
                aggree.Active = "Yes";
                if (rbtnActiveNo.Checked == true) aggree.Active = "No";
                aggree.CompanyID = long.Parse(hdnCompanyid.Value);
                aggree.CustomerID = long.Parse(hdncustomerid.Value);
                aggree.CreatedBy = profile.Personal.UserID;
                aggree.CreationDate = DateTime.Now;
                long AggreeID = aggregator.SaveAggregatorMaster(aggree,profile.DBConnection._constr);
                hdnselectedAggID.Value = AggreeID.ToString();
                setActiveTab(2);
                btncustomernext.Visible = false;

            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Aggregator Master", "Buttonnextsave");
            }
            finally
            {
                aggregator.Close();
            }

        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCompanySetupClient aggregator = new iCompanySetupClient();
            mAgreegator aggree = new mAgreegator();
            if (hdnState.Value == "Edit")
            {
                aggree = aggregator.GetAggregatorMasterbyID(long.Parse(hdnselectedAggID.Value), profile.DBConnection._constr);

            }
            aggree.AgreegatorName = txtaggregator.Text;
            aggree.ContactPersonName = txtcontactperson.Text;
            aggree.EmailID = txtemail.Text;
            aggree.MobileNo = long.Parse(txtmobno.Text);
            aggree.Active = "Yes";
            if (rbtnActiveNo.Checked == true) aggree.Active = "No";
            aggree.CompanyID = long.Parse(hdnCompanyid.Value);
            aggree.CustomerID = long.Parse(hdncustomerid.Value);
            if (hdnState.Value == "Edit")
            {
                aggree.ModifiedBy = profile.Personal.UserID;
                aggree.ModifiedDate = DateTime.Now;
                long AggreeID = aggregator.SaveAggregatorMaster(aggree, profile.DBConnection._constr);             
            }
            BindAgregatorGridMain();
            setActiveTab(0);
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
        }

        public void clear()
        {
            ddlcompany.SelectedItem.Value = "0";
            ddlcompany.SelectedIndex = 0;
            ddlcutomer.SelectedIndex = -1;
            txtaggregator.Text = "";
            txtcontactperson.Text = "";
            txtemail.Text = "";
            txtmobno.Text = "";
            rbtnActiveYes.Checked = true;
            txtapi.Text = "";
            txtpurpose.Text = "";
            txtinputpara.Text = "";
            txtoutputpara.Text = "";
            txturl.Text = "";
            txtremark.Text = "";
            hdnState.Value = "";
            hdnAPIState.Value = "";
            hdnselectedAggID.Value = "";
        }
        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
            if (btnSave != null)
                if (ActiveTab == 0)
                {
                    tabaggregatorlist.Visible = true;
                    tabAggrgator.Visible = false;
                    tabApiDetail.Visible = false;
                    tabAggregatorMaster.ActiveTabIndex = 0;
                }
                else if (ActiveTab == 2)
                {
                    tabaggregatorlist.Visible = true;
                    tabAggrgator.Visible = true;
                    tabApiDetail.Visible = true;
                    tabAggregatorMaster.ActiveTabIndex = 2;
                }
                else
                {
                    tabaggregatorlist.Visible = true;
                    tabAggrgator.Visible = true;
                    tabApiDetail.Visible = true;
                    tabAggregatorMaster.ActiveTabIndex = 1;
                }
        }
        protected void grdaggregator_Select(object sender, EventArgs e)
        {
            clear();
            btncustomernext.Visible = false;
            this.UCToolbar1.ToolbarAccess("Edit");
            Hashtable selectedrec = (Hashtable)grdaggregator.SelectedRecords[0];

            hdnselectedAggID.Value = selectedrec["ID"].ToString();
            long reuslt = long.Parse(hdnselectedAggID.Value);
            GetAggregatmasterDetail(reuslt);
            BindAggreAPIGrid(reuslt);
            hdnState.Value = "Edit";
            setActiveTab(1);
        }


        public void GetAggregatmasterDetail(long AggregatID)
        {
            iCompanySetupClient aggregator = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            mAgreegator aggree = new mAgreegator();
            aggree = aggregator.GetAggregatorMasterbyID(AggregatID, profile.DBConnection._constr);
            FillCompany();
            if (aggree.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(aggree.CompanyID.ToString()));
            hdnCompanyid.Value = aggree.CompanyID.ToString();
            getCustomer(long.Parse(aggree.CompanyID.ToString()));
            if (aggree.CustomerID != null) ddlcutomer.SelectedIndex = ddlcutomer.Items.IndexOf(ddlcutomer.Items.FindByValue(aggree.CustomerID.ToString()));
            hdncustomerid.Value = aggree.CustomerID.ToString();
            if (aggree.AgreegatorName != null) txtaggregator.Text = aggree.AgreegatorName.ToString();
            if (aggree.ContactPersonName != null) txtcontactperson.Text = aggree.ContactPersonName.ToString();
            if (aggree.EmailID != null) txtemail.Text = aggree.EmailID.ToString();
            if (aggree.MobileNo != null) txtmobno.Text = aggree.MobileNo.ToString();
            if (aggree.Active == "No")
            {
                rbtnActiveNo.Checked = true;
            }

        }


        // Company Customer DropDown Code

        protected void FillCompany()
        {
            ddlcompany.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompany.Items.Insert(0, lst);
            CompanyClient.Close();
        }

        public void getCustomer(long CompanyID)
        {
            ddlcutomer.Items.Clear();
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcutomer.DataSource = StatutoryClient.GetCustomerList(CompanyID, profile.DBConnection._constr);
            ddlcutomer.DataTextField = "Name";
            ddlcutomer.DataValueField = "ID";
            ddlcutomer.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcutomer.Items.Insert(0, lst);
            StatutoryClient.Close();
        }

        [WebMethod]
        public static List<contact> GetCustomerByComp(object objReq)
        {
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());
                ds = StatutoryClient.GetCustomerList(ddlcompanyId, profile.DBConnection._constr);
                dt = ds.Tables[0];
                contact Loc = new contact();
                Loc.Name = "--Select--";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Name"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                StatutoryClient.Close();
            }
            return LocList;
        }

        public class contact
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        // Code For Aggregator API


        protected void BindAggreAPIGrid(long AggreID)
        {
            iCompanySetupClient aggregator = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdapidetail.DataSource = aggregator.GetAPIListByID(AggreID, profile.DBConnection._constr);
                grdapidetail.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Aggrgator Master", "BindAgregatorGridMain");
            }
            finally
            {
                aggregator.Close();
            }
        }

        protected void grdapidetail_OnRebind(object sender, EventArgs e)
        {
               BindAggreAPIGrid(long.Parse(hdnselectedAggID.Value));
        }

        [WebMethod]
        public static string WMSaveAPIDetail(object objCon, string State)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iCompanySetupClient aggregator = new iCompanySetupClient();
            mAgreegatorAPI AgreAPI = new mAgreegatorAPI();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                AgreAPI.AgreegatorID = long.Parse(dictionary["AggregatorID"].ToString());
                AgreAPI.APIName = dictionary["APIName"].ToString();
                AgreAPI.Purpose = dictionary["Purpose"].ToString();
                AgreAPI.InputParameter = dictionary["InputPara"].ToString();
                AgreAPI.OutputParameter = dictionary["OutPutPara"].ToString();
                AgreAPI.APIURL = dictionary["ApiURL"].ToString();
                AgreAPI.Remark = dictionary["Remark"].ToString();
                AgreAPI.Active = dictionary["Active"].ToString();
                AgreAPI.CompanyID = long.Parse(dictionary["CompanyID"].ToString());
                AgreAPI.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                if (State == "Edit")
                {
                    AgreAPI.ID = Convert.ToInt64(HttpContext.Current.Session["AggreAPIID"].ToString());

                    long AggiAPIID = aggregator.SaveAggreegatorAPI(AgreAPI, profile.DBConnection._constr);
                }
                else
                {

                    long AggiAPIID = aggregator.SaveAggreegatorAPI(AgreAPI, profile.DBConnection._constr);
                }
                result = "API saved successfully";
            }
            catch { result = "Some error occurred"; }
            finally { aggregator.Close(); }

            return result;
        }


        protected void ClearAPIDetail()
        {
            txtapi.Text = "";
            txtpurpose.Text = "";
            txtinputpara.Text = "";
            txtoutputpara.Text = "";
            txturl.Text = "";
            txtremark.Text = "";
            hdnAPIState.Value = "";
        }

        protected void grdapidetail_Select(object sender, EventArgs e)
        {
            ClearAPIDetail();
          //  btncustomernext.Visible = false;
            this.UCToolbar1.ToolbarAccess("Edit");
            Hashtable selectedrec = (Hashtable)grdapidetail.SelectedRecords[0];
            hdnAggreAPIID.Value = selectedrec["ID"].ToString();
            long reuslt = long.Parse(hdnAggreAPIID.Value);
            GetAggreAPIDetailByID(reuslt);
            Session["AggreAPIID"] = hdnAggreAPIID.Value.ToString();
            hdnAPIState.Value = "Edit";
            //setActiveTab(1);
        }

        public void GetAggreAPIDetailByID(long APIID)
        {
            iCompanySetupClient aggregator = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            mAgreegatorAPI aggreAPI = new mAgreegatorAPI();
            aggreAPI = aggregator.GetAggreegatorAPIbyID(APIID, profile.DBConnection._constr);
            if (aggreAPI.AgreegatorID != null) hdnselectedAggID.Value = aggreAPI.AgreegatorID.ToString();
            if (aggreAPI.APIName != null) txtapi.Text = aggreAPI.APIName.ToString();
            if (aggreAPI.Purpose != null) txtpurpose.Text = aggreAPI.Purpose.ToString();
            if (aggreAPI.InputParameter != null) txtinputpara.Text = aggreAPI.InputParameter.ToString();
            if (aggreAPI.OutputParameter != null) txtoutputpara.Text = aggreAPI.OutputParameter.ToString();
            if (aggreAPI.APIURL != null) txturl.Text = aggreAPI.APIURL.ToString();
            if (aggreAPI.Remark != null) txtremark.Text = aggreAPI.Remark.ToString();
            if (aggreAPI.CompanyID != null) hdnCompanyid.Value = aggreAPI.CompanyID.ToString();
            if (aggreAPI.CustomerID != null) hdncustomerid.Value = aggreAPI.CustomerID.ToString();
            if (aggreAPI.Active == "No")
            {
                rbtnActiveNo.Checked = true;
            }
        }

    }
}