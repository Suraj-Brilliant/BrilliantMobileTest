using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections;
using BrilliantWMS.ServiceTerritory;
using BrilliantWMS.Login;

namespace BrilliantWMS.Territory
{
    public partial class UC_Territory : System.Web.UI.UserControl
    {
        public long TerritoryID { get; set; }
        public long TerritoryUserID { get; set; }
        public Boolean VisiableUserList { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetTerritoryList(2, 1);
                BindListviewWithGroupTitle();
            }
            setValuesToProperty();
            if (VisiableUserList == false) { divUserList.Attributes.Add("style", "display:none"); }
        }

        public void setValuesToProperty()
        {
            if (hdnUserID_UT.Value == "") hdnUserID_UT.Value = "0";
            TerritoryUserID = Convert.ToInt64(hdnUserID_UT.Value);
        }

        public void BindListviewWithGroupTitle()
        {
            iTerritoryClient TerritoryClient = new iTerritoryClient();
            try
            {
                LstTerritory.DataSource = null;
                LstTerritory.DataBind();

                CustomProfile profile = CustomProfile.GetProfile();
                List<mTerritory> TerritoryList = new List<mTerritory>();
                TerritoryList = TerritoryClient.GetTerritoryGroupList(profile.DBConnection._constr).ToList();
                LstTerritory.DataSource = TerritoryList;
                LstTerritory.DataBind();
                hdnLastLevel_UT.Value = TerritoryList[TerritoryList.Count - 1].Level.ToString();

                //int id = 0;
                /*foreach (ListViewItem lstItem in LstTerritory.Items)
                {
                    DropDownList ddl = (DropDownList)lstItem.FindControl("ddlTerritory");
                    if (ddl != null)
                    {
                        ddl.ID = "ddlTerritory_" + TerritoryList[id].Level.ToString();
                        if (id < LstTerritory.Items.Count - 1)
                        {
                            DropDownList ddlChild = new DropDownList();
                            ddlChild.ID = "ddlTerritory_" + (TerritoryList[id].Level + 1).ToString();
                            ddl.Attributes.Add("onchange", "fillDDL1('" + ddl.ClientID + "','" + ddlChild.ClientID + "'," + (TerritoryList[id].Level + 1).ToString() + " )");
                        }
                        if (TerritoryList[id].IsMandatory.Value.ToString().ToLower() == "true")
                        {

                            RequiredFieldValidator req = new RequiredFieldValidator() { ID = "ReqddlTerritory_" + TerritoryList[id].Level.ToString(), ControlToValidate = ddl.ID, ErrorMessage = "Select " + TerritoryList[id].GroupTitle, ValidationGroup = "Save", InitialValue = "0", Display = ValidatorDisplay.None };
                            lstItem.Controls.Add(req);
                        }
                        id += 1;
                    }
                }*/
                for (int i = 0; i < LstTerritory.Items.Count; i++)
                {
                    DropDownList ddl = (DropDownList)LstTerritory.Items[i].FindControl("ddlTerritory");
                    if (ddl == null)
                    {
                        ddl = (DropDownList)LstTerritory.Items[i].FindControl("ddlTerritory_" + TerritoryList[i].Level.ToString());
                    }
                    if (ddl != null)
                    {
                        ddl.ID = "ddlTerritory_" + TerritoryList[i].Level.ToString();
                        HiddenField hdn = (HiddenField)LstTerritory.Items[i].FindControl("hdnTerritoryID");
                        if (i < LstTerritory.Items.Count - 1)
                        {
                            DropDownList ddlChild = (DropDownList)LstTerritory.Items[i + 1].FindControl("ddlTerritory");
                            ddlChild.ID = "ddlTerritory_" + (TerritoryList[i].Level + 1).ToString();
                            ddl.Attributes.Add("onchange", "fillDDL1('" + ddl.ClientID + "','" + ddlChild.ClientID + "'," + (TerritoryList[i].Level + 1).ToString() + " ); setValueToHiddenField(this,'" + hdn.ClientID + "');");
                        }
                        else
                        {
                            ddl.Attributes.Add("onchange", "setValueToHiddenField(this,'" + hdn.ClientID + "'); FillUserListByTerritory(" + (TerritoryList[i].Level).ToString() + ", this);");
                        }
                        if (TerritoryList[i].IsMandatory.Value.ToString().ToLower() == "true")
                        {
                            RequiredFieldValidator req = new RequiredFieldValidator() { ID = "ReqddlTerritory_" + TerritoryList[i].Level.ToString(), ControlToValidate = ddl.ID, ErrorMessage = "Select " + TerritoryList[i].GroupTitle, ValidationGroup = "Save", InitialValue = "0", Display = ValidatorDisplay.None };
                            LstTerritory.Items[i].Controls.Add(req);
                        }
                    }

                }
                //foreach (ListViewItem lstItem in LstTerritory.Items)
                //{
                //    DropDownList ddl = (DropDownList)lstItem.FindControl("ddlTerritory");
                //    if (ddl != null)
                //    {
                //        ddl.ID = "ddlTerritory_" + TerritoryList[id].Level.ToString();
                //        if (id < LstTerritory.Items.Count - 1)
                //        {
                //            DropDownList ddlChild = new DropDownList();
                //            ddlChild.ID = "ddlTerritory_" + (TerritoryList[id].Level + 1).ToString();
                //            ddl.Attributes.Add("onchange", "fillDDL1('" + ddl.ClientID + "','" + ddlChild.ClientID + "'," + (TerritoryList[id].Level + 1).ToString() + " )");
                //        }
                //        if (TerritoryList[id].IsMandatory.Value.ToString().ToLower() == "true")
                //        {

                //            RequiredFieldValidator req = new RequiredFieldValidator() { ID = "ReqddlTerritory_" + TerritoryList[id].Level.ToString(), ControlToValidate = ddl.ID, ErrorMessage = "Select " + TerritoryList[id].GroupTitle, ValidationGroup = "Save", InitialValue = "0", Display = ValidatorDisplay.None };
                //            lstItem.Controls.Add(req);
                //        }
                //        id += 1;
                //    }
                //}
                GetTerritoryList(2, 1);
                //Page.ClientScript.RegisterStartupScript(GetType(), "fillFirstddl", "fillDDL('1')");

            }
            catch { }
            finally { TerritoryClient.Close(); }
        }
        public List<mTerritory> GetTerritoryList(long Level, long ParentID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            iTerritoryClient TerritoryClient = new iTerritoryClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                TerritoryList = TerritoryClient.GetTerritoryList(Level, ParentID, profile.DBConnection._constr).ToList();

                if (Level == 2)
                {
                    DropDownList ddl = (DropDownList)LstTerritory.Items[0].FindControl("ddlTerritory_2");
                    if (ddl != null)
                    {
                        ddl.DataSource = TerritoryList;
                        ddl.DataBind();
                        ListItem lst = new ListItem() { Text = "Select " + TerritoryList[0].GroupTitle, Value = "0" };
                        ddl.Items.Insert(0, lst);
                    }
                }
            }
            catch { }
            finally { TerritoryClient.Close(); }
            return TerritoryList;
        }

        public List<vGetUserProfileList> GetUserListByTerritory(long Level, long ParentID)
        {
            List<vGetUserProfileList> UserList = new List<vGetUserProfileList>();
            iTerritoryClient TerritoryClient = new iTerritoryClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UserList = TerritoryClient.GetUserListByTerritory(Level, ParentID, profile.DBConnection._constr).ToList();

                if (UserList == null)
                {
                    ListItem lst = new ListItem() { Text = "Not available", Value = "0" };
                    ddlUserList.Items.Insert(0, lst);
                }
            }
            catch { }
            finally { TerritoryClient.Close(); }
            return UserList;

        }


        public DateTime childID { get; set; }


         public List<mTerritory> GetDepartmentList(long CompanyID)
         {
             List<mTerritory> TerritoryList = new List<mTerritory>();
             iTerritoryClient TerritoryClient = new iTerritoryClient();
             try
             {
                 CustomProfile profile = CustomProfile.GetProfile();
                 TerritoryList = TerritoryClient.GetDepartmentList(CompanyID, profile.DBConnection._constr).ToList();
             }
             catch { }
             finally { TerritoryClient.Close(); }
             return TerritoryList;
         }

         
        
        
    }
}