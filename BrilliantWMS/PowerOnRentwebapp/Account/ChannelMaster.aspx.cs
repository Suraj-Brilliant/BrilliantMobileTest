using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PopupMessages;
using System.Web.Services;
using System.Collections;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using System.Resources;
using System.Globalization;
using System.Data;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;


namespace BrilliantWMS.Account
{
    public partial class ChannelMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setActiveTab(0);
                FillCompany();
                WarehouseGridBind();
                //GetChannelByType();
            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void WarehouseGridBind()
        {
            iCompanySetupClient Channel = new iCompanySetupClient();         
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdchannel.DataSource = Channel.GetChannelList(profile.DBConnection._constr);
                grdchannel.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "MainCustomerGridBind");
            }
            finally
            {
                Channel.Close();
            }
        }


        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { 
            clear();
            setActiveTab(1);
           
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mChannel Channel = new mChannel();
                if (hdnState.Value == "Edit")
                {
                    Channel = CompanyClient.GetChannelByChanID(long.Parse(hdnselectedchanID.Value), profile.DBConnection._constr);
                }
                Channel.ChannelName = txtchannelname.Text;
                Channel.ChannelURL = txtchanlUrl.Text;
                Channel.UserName = txtusername.Text;
                Channel.Password = txtpassword.Text;
                Channel.TypeID = long.Parse(ddlchantype.SelectedItem.Value);
                if (hdnchannelid.Value != "")
                {
                    Channel.StandardChannelID = long.Parse(hdnchannelid.Value);
                }
                if (hndCompanyid.Value != "")
                {
                    Channel.CompanyID = long.Parse(hndCompanyid.Value);
                }
                Channel.CustomerID = long.Parse(hdncustomerid.Value);
                Channel.Active = "Yes";
                if (rbtnActiveYes.Checked != true) Channel.Active = "No";

                if (hdnState.Value == "Edit")
                {
                    Channel.ModifiedBy = profile.Personal.UserID;
                    Channel.ModifiedDate = DateTime.Today;
                    long channelid = CompanyClient.SaveChannelDetail(Channel, profile.DBConnection._constr);
                }
                else
                {
                    Channel.CreatedBy = profile.Personal.UserID;
                    Channel.CreationDate = DateTime.Now;
                    long channelid = CompanyClient.SaveChannelDetail(Channel, profile.DBConnection._constr);
                }
                WebMsgBox.MsgBox.Show("Record saved successfully");
                WarehouseGridBind();
                clear();
                setActiveTab(0);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Channel Master", "Save");
            }
            finally
            {
                CompanyClient.Close();
            }
       }

        protected void grdchannel_Select(object sender, EventArgs e)
        {
            clear();
            this.UCToolbar1.ToolbarAccess("Edit");
            Hashtable selectedrec = (Hashtable)grdchannel.SelectedRecords[0];
            hdnselectedchanID.Value = selectedrec["ID"].ToString();
            long reuslt = long.Parse(hdnselectedchanID.Value);
            GetChannelForEdit(reuslt);
            hdnState.Value = "Edit";
            setActiveTab(1);
        }

        public void GetChannelForEdit(long channelID)
        {
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                mChannel chan = new mChannel();
                chan = CompanyClient.GetChannelByChanID(channelID, profile.DBConnection._constr);
                if (chan.ChannelName != null) txtchannelname.Text = chan.ChannelName.ToString();
                if (chan.ChannelURL != null) txtchanlUrl.Text = chan.ChannelURL.ToString();
                if (chan.UserName != null) txtusername.Text = chan.UserName.ToString();
                if (chan.Password != null) txtpassword.Text = chan.Password.ToString();
                txtpassword.Attributes["value"] = chan.Password.ToString();
                if (chan.Active == "No")
                {
                    rbtnActiveNo.Checked = true;
                }
                FillCompany();
                if (chan.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(chan.CompanyID.ToString()));
                hndCompanyid.Value = chan.CompanyID.ToString();
                getCustomer(long.Parse(chan.CompanyID.ToString()));
                if (chan.CustomerID != null) ddlcutomer.SelectedIndex = ddlcutomer.Items.IndexOf(ddlcutomer.Items.FindByValue(chan.CustomerID.ToString()));
                hdncustomerid.Value = chan.CustomerID.ToString();
               // if (chan.TypeID != null) ddlchantype.SelectedIndex = ddlcutomer.Items.IndexOf(ddlchantype.Items.FindByValue(chan.TypeID.ToString()));
                if (chan.TypeID != null) ddlchantype.SelectedItem.Value = chan.TypeID.ToString();
                ddlchantype.SelectedValue = chan.TypeID.ToString();
                if (chan.TypeID.ToString() == "1")
                {
                    ddlchantype.SelectedIndex = 1;
                }
                else if (chan.TypeID.ToString() == "2")
                {
                    ddlchantype.SelectedIndex = 2;
                }
                
                if (chan.TypeID.ToString() == "1")
                {
                    GetChannelByType();
                    if (chan.StandardChannelID != null) ddlchannel.SelectedIndex = ddlchannel.Items.IndexOf(ddlchannel.Items.FindByValue(chan.StandardChannelID.ToString()));
                   
                }
                else
                {
                    ddlchannel.Enabled = false;
                }
                
                
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Channel Master", "GetChannelForEdit");
            }
            finally
            {
                CompanyClient.Close();
            }
        }


        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { 
            clear();
        }

        public void clear()
        {
           // ddlcompany.Items.Clear();
            ddlcutomer.SelectedIndex = -1;
            //ddlchantype.Items.Clear();
            ddlchannel.SelectedIndex = -1;
            ddlchantype.SelectedIndex = 0;
            txtchannelname.Text = "";
            txtchanlUrl.Text = "";
            txtusername.Text = "";
            txtpassword.Text = "";
            rbtnActiveYes.Checked = true;
            hdnchannelid.Value = "";
        }

       
        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
            if (btnSave != null)
                if (ActiveTab == 0)
                {
                    TabChannelList.Visible = true;
                    tabChannelInfo.Visible = false;
                    tabChannelMaster.ActiveTabIndex = 0;
                }
                else
                {
                    TabChannelList.Visible = true;
                    tabChannelInfo.Visible = true;
                    tabChannelMaster.ActiveTabIndex = 1;
                }
        }

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

        // Get Customer Code
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

        // Get Channel Name
        public void GetChannelByType()
        {
            ddlchannel.Items.Clear();
            iCompanySetupClient Channel = new iCompanySetupClient(); 
            CustomProfile profile = CustomProfile.GetProfile();
            ddlchannel.DataSource = Channel.GetChannelName("Channel", profile.DBConnection._constr);
            ddlchannel.DataTextField = "Value";
            ddlchannel.DataValueField = "Id";
            ddlchannel.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlchannel.Items.Insert(0, lst);
            Channel.Close();
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

         [WebMethod]
        public static List<contact> GetChannel(object objReq)
        {
            iCompanySetupClient Channel = new iCompanySetupClient(); 
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());
                string ChannelType = "Channel";
                ds = Channel.GetChannelName(ChannelType, profile.DBConnection._constr);
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
                        Loc.Id = dt.Rows[i]["Id"].ToString();
                        Loc.Name = dt.Rows[i]["Value"].ToString();
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
                Channel.Close();
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
      
    }
}