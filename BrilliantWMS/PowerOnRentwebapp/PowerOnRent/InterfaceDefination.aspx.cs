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
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.PORServiceEngineMaster;
using BrilliantWMS.PORServiceSiteMaster;
using BrilliantWMS.ProductCategoryService;
using BrilliantWMS.ProductSubCategoryService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Collections;

namespace BrilliantWMS.PowerOnRent
{
    public partial class InterfaceDefination : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string ObjectName = "InterfaceDefinationDetail";

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //UCFormHeader1.FormHeaderText = "Interface Defination";
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

                GetInterfaceList();
                BindTable();
                BindDatatype();
            }

            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                iSiteMasterClient InterfaceClient = new iSiteMasterClient();

                if (checkDuplicate() == "")
                {


                        string Tablename = ddlTable.SelectedItem.Text;
                        string DataType = ddlDataype.SelectedItem.Text;
                        string FieldName = txtFieldName.Text;
                        string IsNull = ddlIsNull.SelectedItem.Text;

                        if (hdnInterfaceID.Value != "0" && hdnInterfaceID.Value != "")
                        {
                            DataSet ds = new DataSet();
                            long ModifyBy = Convert.ToInt64(profile.Personal.UserID.ToString());
                            hdnSate.Value = "Edit";
                            InterfaceClient.UpdateInterface(Convert.ToInt64(hdnInterfaceID.Value), Tablename, DataType, FieldName, IsNull, ModifyBy, profile.DBConnection._constr);

                        }
                        else
                        {
                            hdnSate.Value = "AddNew";
                            long CreatedBy = Convert.ToInt64(profile.Personal.UserID.ToString());
                            InterfaceClient.InsertInterface(Tablename, DataType, FieldName, IsNull,CreatedBy, profile.DBConnection._constr);
                        }
                       
                        if (hdnSate.Value == "Edit")
                        {
                            WebMsgBox.MsgBox.Show("Record Update sussessfully");
                            ClearAll();
                        }
                        if( hdnSate.Value == "AddNew")
                        {
                            WebMsgBox.MsgBox.Show("Record saved sussessfully");
                            ClearAll();
                        }

                    }
                

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product master", "pageSave");
            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            tblInterfaceDefLst.Visible = false;
            tbTemplateDetail.Visible = true;
            tabContainerReqTemplate.ActiveTabIndex = 1;
            //clear();
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
            //clear();
            hdnprodID.Value = imgbtn.ToolTip.ToString();
            //GetProductDetailByProductID();
            tblInterfaceDefLst.Visible = false;
            tbTemplateDetail.Visible = true;
            tabContainerReqTemplate.ActiveTabIndex = 1;

        }

        protected void GetInterfaceList()
        {           
            iPartRequestClient objService = new iPartRequestClient();

            
            CustomProfile profile = CustomProfile.GetProfile();
            GVInterface.DataSource = null;
            GVInterface.DataBind();

            DataSet dsInterface = new DataSet();
            dsInterface = objService.GetGetInterfaceDetails(profile.DBConnection._constr);

            GVInterface.DataSource = dsInterface;
            GVInterface.DataBind();
        }

        protected void BindTable()
        {
            iSiteMasterClient objService = new iSiteMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet Table = new DataSet();
            Table = objService.GetTableList(profile.DBConnection._constr);
            ddlTable.DataSource = Table;
            ddlTable.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlTable.Items.Insert(0, lst);
           
        }

        protected void BindDatatype()
        {
            iSiteMasterClient objService = new iSiteMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet DatatType = new DataSet();
            DatatType = objService.GetDataTypeList(profile.DBConnection._constr);
            ddlDataype.DataSource = DatatType;
            ddlDataype.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlDataype.Items.Insert(0, lst);
        }

        protected void GVInterface_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                hdnSate.Value = "Edit";
                Hashtable selectedrec = (Hashtable)GVInterface.SelectedRecords[0];
                hdnInterfaceID.Value = selectedrec["ID"].ToString();
               
                BindTable();
                ddlTable.SelectedIndex = ddlTable.Items.IndexOf(ddlTable.Items.FindByText(selectedrec["TableName"].ToString()));

                BindDatatype();
                ddlDataype.SelectedIndex = ddlDataype.Items.IndexOf(ddlDataype.Items.FindByText(selectedrec["FieldDataType"].ToString()));

                ddlIsNull.SelectedIndex = ddlIsNull.Items.IndexOf(ddlIsNull.Items.FindByValue(selectedrec["IsNull"].ToString()));
                txtFieldName.Text = selectedrec["Fieldname"].ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Interface Defination", "GVInterface_Select");

            }
            finally
            {
            }
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            tblInterfaceDefLst.HeaderText = rm.GetString("InterfaceDefinationList", ci);
            lblTemplate.Text = rm.GetString("InterfaceDefinationList", ci);
            tbTemplateDetail.HeaderText = rm.GetString("InterfaceDefinationDetail", ci);
            lblSelectTable.Text = rm.GetString("SelectTable", ci);
            lblFieldName.Text = rm.GetString("FieldName", ci);
            lblDataType.Text = rm.GetString("DataType", ci);
            lblTemplate.Text = rm.GetString("TemplateTitle", ci);
            lblIsNull.Text = rm.GetString("IsNull", ci);
            UCFormHeader1.FormHeaderText = rm.GetString("InterfaceDefination", ci);
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            //if (hndstate.Value == "Edit")
            //{
            //    GetUserByID();
            //}
            //else
            //{
               ClearAll();
            //    FillRoleDropDown();
            //    ResetUserControl(0);
            //}


        }

        private void ClearAll()
        {
            txtFieldName.Text = "";
            ddlDataype.SelectedIndex = -1;
            ddlIsNull.SelectedIndex = -1;
            ddlTable.SelectedIndex = -1;
            //ActiveTab("0");
        }

        public string checkDuplicate()
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient InterfaceClient1 = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();

                if (hdnInterfaceID.Value == string.Empty)
                {
                    result = InterfaceClient1.checkDuplicateInterface(ddlTable.SelectedItem.Text, txtFieldName.Text, ddlDataype.SelectedItem.Text, ddlIsNull.SelectedItem.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        
                    }
                    
                }
                else
                {

                    result = InterfaceClient1.checkDuplicateInterfaceEdit(Convert.ToInt32(hdnInterfaceID.Value), ddlTable.SelectedItem.Text, txtFieldName.Text, ddlDataype.SelectedItem.Text, ddlIsNull.SelectedItem.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        //txtProductName.Text = "";
                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Leads Source master", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }

    }
}