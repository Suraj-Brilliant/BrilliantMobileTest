using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.ProductCategoryService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Collections;

namespace BrilliantWMS.PowerOnRent
{
    public partial class MessageDefination : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string ObjectName = "MessageDefinationDetail";

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // UCFormHeader1.FormHeaderText = "Message Defination";
            //Toolbar1.SetUserRights("MaterialRequest", "Summary", "");
            if (Session["Lang"] == null)
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            if (!IsPostBack)
            {
                //Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                //btnExport.Visible = false;
                //Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                //btnImport.Visible = false;
                //Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                //btmMail.Visible = false;
                //Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                //btnPrint.Visible = false;
                tblInterfaceDefLst.Visible = true;
                tbTemplateDetail.Visible = false;
                tabContainerReqTemplate.ActiveTabIndex = 0;

                GetMessageList();
                BindDestination();
                BindActionType();
                BindObject();
            }

            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }
        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {

            iProductCategoryMasterClient Message = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (hdnState.Value == "Add")
            {
                DataSet ds = new DataSet();
                long HeaderId = 0;
                try
                {

                    ds = Message.GetMessageTempData(profile.DBConnection._constr);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i < 1)
                        {
                            string Destination = ds.Tables[i].Rows[i]["Destination"].ToString();
                            string ActionType = ds.Tables[i].Rows[i]["ActionType"].ToString();
                            string TableName = ds.Tables[i].Rows[i]["TableName"].ToString();
                            string description = ds.Tables[i].Rows[i]["description"].ToString();
                            string Remark = ds.Tables[i].Rows[i]["Remark"].ToString();
                            string sequence = ds.Tables[i].Rows[i]["sequence"].ToString();
                            string FieldID = ds.Tables[i].Rows[i]["FieldID"].ToString();
                            long CreatedBy = profile.Personal.UserID;
                            Message.InsertMessageHeader(Destination, ActionType, TableName, description, CreatedBy, Remark, profile.DBConnection._constr);
                            DataSet ds1 = new DataSet();
                            ds1 = Message.GetMessHeaderID(profile.DBConnection._constr);
                            HeaderId = Convert.ToInt64(ds1.Tables[0].Rows[0][0].ToString());
                            Message.InsrtIntoMessageHeader(HeaderId, Convert.ToInt64(sequence), Convert.ToInt64(FieldID), CreatedBy, profile.DBConnection._constr);
                        }
                        else
                        {
                            long Sequence = Convert.ToInt64(ds.Tables[0].Rows[i]["sequence"].ToString());
                            long FieldID = Convert.ToInt64(ds.Tables[0].Rows[i]["FieldID"].ToString());
                            long CreatedBy = profile.Personal.UserID;
                            Message.InsrtIntoMessageHeader(HeaderId, Sequence, FieldID, CreatedBy, profile.DBConnection._constr);
                        }

                    }
                    Message.DeleteMessageTemptable(profile.DBConnection._constr);
                    WebMsgBox.MsgBox.Show("Record saved successfully");
                }
                catch
                { }
                finally
                { }
            }

            else if (hdnState.Value == "Edit")
            {
                long MessageID = Convert.ToInt64(hdnMessageID.Value);
                long ModifyBy = profile.Personal.UserID;
                string Destination = ddlDestination.SelectedItem.Text;
                string ActionType = ddlType.SelectedItem.Text;
                string TableName = ddlObject.SelectedItem.Text;
                string description = txtTitle.Text;
                string Remark = txtPurpose.Text;
                try
                {

                    Message.UpdateMessageHeader(MessageID, Destination, ActionType, TableName, description, ModifyBy, Remark, profile.DBConnection._constr);
                    WebMsgBox.MsgBox.Show("Record Update successfully");
                    clear();
                }

                catch { }
                finally { }
            }


        }
        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {

            iProductCategoryMasterClient Message = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            tblInterfaceDefLst.Visible = false;
            tbTemplateDetail.Visible = true;
            tabContainerReqTemplate.ActiveTabIndex = 1;
            hdnState.Value = "Add";
            if (hdnState.Value == "Add")
            {
                Message.DeleteMessageTemptable(profile.DBConnection._constr);
            }
            clear();
            //GetProductSpecificationDetailByProductID();
            //GetProductTaxDetailByProductID();
            //GetProductImagesByProductID();
            //GVRateHistory();
            //FillInventoryGrid();
            //setActiveTab(1);
            //changePrice1.Attributes.Add("style", "visibility:hidden");
        }
        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            hdnState.Value = "Edit";
            //clear();
            //GetProductDetailByProductID();
            Hashtable selectedrec = (Hashtable)GVMessage.SelectedRecords[0];
            hdnMessageID.Value = selectedrec["Id"].ToString();
            txtTitle.Text = selectedrec["description"].ToString();
            BindDestination();
            ddlDestination.SelectedIndex = ddlDestination.Items.IndexOf(ddlDestination.Items.FindByText(selectedrec["Destination"].ToString()));
            //ddlDestination.SelectedValue = selectedrec["Destination"].ToString();
            BindActionType();
            ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(selectedrec["ActionType"].ToString()));
            //ddlType.SelectedValue = selectedrec["ActionType"].ToString();
            txtPurpose.Text = selectedrec["remark"].ToString();
            BindObject();
            ddlObject.SelectedIndex = ddlObject.Items.IndexOf(ddlObject.Items.FindByText(selectedrec["TableName"].ToString()));
            //ddlObject.SelectedValue = selectedrec["TableName"].ToString();

           // ddlField.SelectedIndex = -1;
            GetFieldList();
            tblInterfaceDefLst.Visible = false;
            tbTemplateDetail.Visible = true;
            tabContainerReqTemplate.ActiveTabIndex = 1;

        }
        public void clear()
        {
            txtTitle.Text = "";
            ddlDestination.SelectedIndex = -1;
            ddlType.SelectedIndex = -1;
            txtPurpose.Text = "";
            ddlObject.SelectedIndex = -1;
            ddlField.SelectedIndex = -1;
            txtSequence.Text = "";
        }

        public void BindDestination()
        {
            iProductCategoryMasterClient DestinationClient = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlDestination.DataSource = DestinationClient.GetDestination(profile.DBConnection._constr);
            ddlDestination.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlDestination.Items.Insert(0, lst);
        }
        public void BindActionType()
        {
            iProductCategoryMasterClient ActionClient = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlType.DataSource = ActionClient.GetActionType(profile.DBConnection._constr);
            ddlType.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlType.Items.Insert(0, lst);
        }
        public void BindObject()
        {
            iProductCategoryMasterClient ObjectClient = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlObject.DataSource = ObjectClient.GetObject(profile.DBConnection._constr);
            ddlObject.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlObject.Items.Insert(0, lst);
        }
        protected void GetMessageList()
        {
            iPartRequestClient objService = new iPartRequestClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVMessage.DataSource = null;
            GVMessage.DataBind();

            DataSet dsMessage = new DataSet();
            dsMessage = objService.GetGetMessageDetails(profile.DBConnection._constr);

            GVMessage.DataSource = dsMessage;
            GVMessage.DataBind();
        }
        [WebMethod]
        public static List<mInterfaceMap> PMGetFieldList(string Selectedbject)
        {

            iProductCategoryMasterClient ObjectClient = new iProductCategoryMasterClient();
            List<mInterfaceMap> FieldList = new List<mInterfaceMap>();
            try
            {

                CustomProfile profile = CustomProfile.GetProfile();
                FieldList = ObjectClient.FieldList(Selectedbject, profile.DBConnection._constr).ToList();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "User Creation", "PMfillDesignation");
            }
            finally
            {

            }
            return FieldList;
        }

        [WebMethod]
        public static void WMRemoveFieldFromUserList(string ID)
        {
            iProductCategoryMasterClient Message = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Message.DeleteFieldFromList(Convert.ToInt64(ID), profile.DBConnection._constr);
            }
            catch { }
            finally { }
        }


        [WebMethod]
        public static void WMRemoveFieldFromUserListAtEditTime(string ID)
        {
            iProductCategoryMasterClient Message = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Message.DeleteFieldFromListWhenEdit(Convert.ToInt64(ID), profile.DBConnection._constr);
                
            }
            catch { }
            finally { }
        }


        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            tblInterfaceDefLst.HeaderText = rm.GetString("MessageDefinationList", ci);
            lmlmessagedef.Text = rm.GetString("MessageDefinationList", ci);
            tbTemplateDetail.HeaderText = rm.GetString("MessageDefinationDetail", ci);
            lblTitle.Text = rm.GetString("Title", ci);
            lblPurpose.Text = rm.GetString("Purpose", ci);
            lblDestination.Text = rm.GetString("Destination", ci);
            lblType.Text = rm.GetString("Type", ci);
            lblObject.Text = rm.GetString("Object", ci);
            lblField.Text = rm.GetString("Field", ci);
            lblfieldlist.Text = rm.GetString("FieldList", ci);
            lblSequence.Text = rm.GetString("Sequence", ci);
            UCFormHeader1.FormHeaderText = rm.GetString("MessageDefination", ci);
            ADDButton.Text = rm.GetString("Add", ci);
            //btnAdd.Value = rm.GetString("Add", ci); Comment by vishal
        }
        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {

        }

        protected void AddBtn_Click(object sender, System.EventArgs e)
        {
            string Title, Destination, Type, Purpose, Object1;
            long Sequence, Field1;

            Title = txtTitle.Text;
            Destination = ddlDestination.SelectedItem.Text;
            Type = ddlType.SelectedItem.Text;
            Purpose = txtPurpose.Text;
            Object1 = ddlObject.SelectedItem.Text;
            Field1 = Convert.ToInt64(hdnSelectedField.Value);
            Sequence = Convert.ToInt64(txtSequence.Text);
            iProductCategoryMasterClient Message = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();

            if (hdnState.Value == "Add")
            {
                try
                {
                    //Message.DeleteMessageTemptable(profile.DBConnection._constr);

                    Message.InsertMessageIntoTemptable(Title, Destination, Type, Purpose, Object1, Field1, Sequence, profile.DBConnection._constr);
                    GetFieldList();
                    txtSequence.Text = "";
                    ddlObject.SelectedIndex = -1;
                    ddlField.SelectedIndex = -1;

                }
                catch
                { }
                finally
                { }
            }
            if (hdnState.Value == "Edit")
            {
                long MsgHeadID = Convert.ToInt64(hdnMessageID.Value);
                long Sequence1 = Convert.ToInt64(txtSequence.Text);
                long Field = Convert.ToInt64(hdnSelectedField.Value);
                long CreateBy = profile.Personal.UserID;

                try
                {
                    Message.InsertMessageDetails(MsgHeadID, Field, Sequence1, CreateBy,profile.DBConnection._constr);
                    GetFieldList();
                }
                catch
                { }
                finally
                { }
            }

        }

        protected void GetFieldList()
        {
            iProductCategoryMasterClient objService = new iProductCategoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            GVFields.DataSource = null;
            GVFields.DataBind();
            DataSet dsFieldList = new DataSet();
            if (hdnState.Value == "Add")
            {
                dsFieldList = objService.GetFieldDetails(profile.DBConnection._constr);
            }

            if (hdnState.Value == "Edit")
            {
                dsFieldList = objService.GetFieldDetailsFromMessageTable(Convert.ToInt64(hdnMessageID.Value), profile.DBConnection._constr);
            }

            GVFields.DataSource = dsFieldList;
            GVFields.DataBind();
        }




        protected void GVFields_OnRebind(object sender, EventArgs e)
        {
            GetFieldList();
        }
    }
}