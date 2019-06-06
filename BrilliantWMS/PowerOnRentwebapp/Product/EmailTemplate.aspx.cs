using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Data;
using Obout.Grid;
using Obout.Ajax.UI.FileUpload;
using System.Data.OleDb;
using BrilliantWMS.Login;
using BrilliantWMS.ProductMasterService;
using System.Web.Services;
using WebMsgBox;
using BrilliantWMS.DocumentService;
using BrilliantWMS.ProductSubCategoryService;
using BrilliantWMS.ProductCategoryService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.ServiceTerritory;
using BrilliantWMS.Territory;
using BrilliantWMS.UserCreationService;
using System.Configuration;


namespace BrilliantWMS.Product
{
    public partial class EmailTemplate : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static string sessionID;
        static string CurrentObject = "ProductInventoryDetail";
        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            sessionID = Session.SessionID.ToString();
           // UCFormHeader1.FormHeaderText = "Email Template";
            if (!IsPostBack)
            {
               
                BindCustomer();
                BindActivity();
                BindMessageType();
                ResetUserControl();
                GetTemplateList();
                setActiveTab(0);
                ScriptManager.RegisterStartupScript(this.FlyoutChangeProdPrice, FlyoutChangeProdPrice.GetType(), "reg1", "SaveNewPrice();", false);
            }
            UCToolbar1.SetImgbtnAddNewRight(false, "Not Allowed");
            //this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }
        protected void Page_PreInit(Object sender, EventArgs e)
        { CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }

        protected void ResetUserControl()
        {
            iProductMasterClient objService = new iProductMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UC_AttachDocument1.ClearDocument("Product");
                objService.ClearTempDataFromDB(Session.SessionID, profile.Personal.UserID.ToString(), CurrentObject, profile.DBConnection._constr);
                hdnprodID.Value = "0";
            }
            catch { }
            finally { objService.Close(); }
        }

        protected void GetTemplateList()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient TemplateClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
            try
            {
                if (profile.Personal.UserType == "Admin")
                {
                    gvUserCreationM.DataSource = TemplateClient.GetTemplateListForGridAdmin(Convert.ToInt64(profile.Personal.UserID),profile.DBConnection._constr);
                }
                else
                {
                    List<vGetTemplateList1> templateLst = new List<vGetTemplateList1>();
                    templateLst = TemplateClient.GetTemplateListForGrid(profile.DBConnection._constr).ToList();
                    templateLst = templateLst.Where(t => t.CompanyID == profile.Personal.CompanyID).ToList();
                    gvUserCreationM.DataSource = templateLst;
                    //gvUserCreationM.DataSource = TemplateClient.GetTemplateListForGrid(profile.DBConnection._constr);
                    // gvUserCreationM.GroupBy = "CompanyName,Territory";
                }
                gvUserCreationM.DataBind();
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email Template", "GetTemplateList");
            }
            finally
            {
                TemplateClient.Close();
            }
        }

        #region Prodcut Tax Setup
        /*Prodcut Tax Setup*/
        protected void GetProductTaxDetailByProductID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdnprodID.Value == "") hdnprodID.Value = "0";
            iProductMasterClient productClient = new iProductMasterClient();
            GVTaxSetup.DataSource = productClient.GetProductTaxDetailByProductID(Convert.ToInt64(hdnprodID.Value), Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            GVTaxSetup.DataBind();
            productClient.Close();
        }

        [WebMethod]
        public static void TempSaveTaxSetup(string IsChecked, string TaxID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            //connectiondetails conn = new connectiondetails() { DataBaseName = Profile.DataBase, DataSource = Profile.DataSource, DBPassword = Profile.DBPassword };
            productClient.UpdateTempTaxSetup(TaxID, IsChecked.ToLower(), sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            productClient.Close();
        }

        protected void GVTaxSetup_OnRebind(object sender, EventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iProductMasterClient productClient = new iProductMasterClient();
                GVTaxSetup.DataSource = productClient.GetTempSaveProductTaxDetailBySessionID(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                GVTaxSetup.DataBind();
                productClient.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductMaster", "GVTaxSetup_OnRebind");
            }
            finally
            {

            }
        }
        #endregion

        #region Prodcut Specification
        /*Prodcut Specification*/
        protected void GetProductSpecificationDetailByProductID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdnprodID.Value == "") hdnprodID.Value = "0";
            iProductMasterClient productClient = new iProductMasterClient();
            GVProductSpecification.DataSource = productClient.GetProductSpecificationDetailByProductID(Convert.ToInt64(hdnprodID.Value), Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            GVProductSpecification.DataBind();
            productClient.Close();
        }

        protected void GVProductSpecification_OnRebind(object sender, EventArgs e)
        {
            GetProductSpecificationDetailByProductID();
        }

        protected void GVProductSpecification_InsertRecord(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            mProductSpecificationDetail oprodspec = new mProductSpecificationDetail();
            oprodspec.SpecificationTitle = e.Record["SpecificationTitle"].ToString();
            oprodspec.SpecificationDescription = e.Record["SpecificationDescription"].ToString();
            oprodspec.Active = "Y";
            oprodspec.ProductID = Convert.ToInt32(hdnprodID.Value);
            oprodspec.CreatedBy = profile.Personal.UserID.ToString(); //need to change
            oprodspec.CreationDate = DateTime.Now;
            oprodspec.CompanyID = profile.Personal.CompanyID; // need to change
            //int upprodsperesult = productClient.InserttProductSpecificationDetail(oprodspec, profile.DBConnection._constr);
            iProductMasterClient productClient = new iProductMasterClient();
            productClient.AddProductSpecificationToTempData(oprodspec, Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            productClient.Close();

        }

        protected void GVProductSpecification_OnUpdateCommand(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            /*
            if (hdnprodID.Value != null)
            {
                int Spcid = Convert.ToInt32(e.Record["ID"].ToString());
                var prodspec = (from c in productClient.GetProductSpecificationDetailsByProdSpecID(Spcid, profile.DBConnection._constr)
                                select c).ToList();
                foreach (var v in prodspec)
                {
                    mProductSpecificationDetail oprodspec = new mProductSpecificationDetail();
                    oprodspec.ID = v.ID;
                    oprodspec.SpecificationTitle = e.Record["SpecificationTitle"].ToString();
                    oprodspec.SpecificationDescription = e.Record["SpecificationDescription"].ToString();
                    oprodspec.Active = v.Active;
                    oprodspec.ProductID = v.ProductID;
                    oprodspec.CreatedBy = v.CreatedBy;
                    oprodspec.CreationDate = v.CreationDate;
                    oprodspec.LastModifiedBy = profile.Personal.UserID.ToString(); //need to change
                    oprodspec.LastModifiedDate = DateTime.Now;
                    oprodspec.CompanyID = v.CompanyID;
                    int upprodsperesult = productClient.UpdatemProductSpecificationDetail(oprodspec, profile.DBConnection._constr);
                    if (upprodsperesult > 0)
                    {
                        pop.DisplayPopupMessage(this, "Record Updated sucessfully.", PopupMessages.PopupMessage.AlertType.Error);
                        //bindGVProductSpecification();
                    }
                    else
                    {
                        pop.DisplayPopupMessage(this, "Error while updating record! Please try latter.", PopupMessages.PopupMessage.AlertType.Error);
                    }
                }
            }*/
        }

        protected void GVProductSpecification_OnDeleteCommand(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            /*
            if (hdnprodID.Value != null)
            {
                int Spcid = Convert.ToInt32(e.Record["ID"].ToString());
                var prodspec = (from c in productClient.GetProductSpecificationDetailsByProdSpecID(Spcid, profile.DBConnection._constr)
                                select c).ToList();
                foreach (var v in prodspec)
                {
                    PowerOnRentwebapp.ProductService.mProductSpecificationDetail oprodspec = new PowerOnRentwebapp.ProductService.mProductSpecificationDetail();
                    oprodspec.ID = v.ID;
                    oprodspec.SpecificationTitle = e.Record["SpecificationTitle"].ToString();
                    oprodspec.SpecificationDescription = e.Record["SpecificationDescription"].ToString();
                    oprodspec.Active = v.Active;
                    oprodspec.ProductID = v.ProductID;
                    oprodspec.CreatedBy = v.CreatedBy;
                    oprodspec.CreationDate = v.CreationDate;
                    oprodspec.LastModifiedBy = profile.Personal.UserID.ToString(); //need to change
                    oprodspec.LastModifiedDate = DateTime.Now;
                    oprodspec.CompanyID = v.CompanyID;
                    int upprodsperesult = productClient.DeletemProductSpecificationDetail(oprodspec, profile.DBConnection._constr);
                }
            }*/
        }

        #endregion

        #region Prodcut Imgs
        protected void GetProductImagesByProductID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdnprodID.Value == "") hdnprodID.Value = "0";
            iProductMasterClient productClient = new iProductMasterClient();
            GVImages.DataSource = productClient.GetProductImagesByProductID(Convert.ToInt64(hdnprodID.Value), Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            GVImages.DataBind();
            productClient.Close();
        }

        protected void btnProductMasterUploadImg_OnClick(object sender, EventArgs e)
        {
            if (FileUploadProductMasterImg.PostedFiles != null)
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iProductMasterClient productClient = new iProductMasterClient();

                string uploadfilepath = HttpRuntime.AppDomainAppPath;

                foreach (PostedFileInfo info in FileUploadProductMasterImg.PostedFiles)
                {
                    string type = info.ContentType.Replace("image/", "").Replace("application/", "");
                    //type = type.Replace("application/", "");
                    string gridDisplayPath = "TempImg\\" + Session.SessionID.ToString() + DateTime.Now.Ticks.ToString() + "." + type;
                    string SaveAsPath = uploadfilepath + "\\" + gridDisplayPath;
                    //SaveAsPath = SaveAsPath.Replace('\\', '/');

                    if (!(Directory.Exists(uploadfilepath + "/TempImg")))
                    {
                        Directory.CreateDirectory(uploadfilepath + "/TempImg");
                    }
                    info.SaveAs(SaveAsPath);

                    tImage UploadedImage = new tImage();
                    UploadedImage.ObjectName = "Product";
                    UploadedImage.ReferenceID = Convert.ToInt32(hdnprodID.Value);
                    UploadedImage.ImageName = info.FileName;
                    UploadedImage.ImgeTitle = txtImageTitle.Text;
                    UploadedImage.ImageDesc = txtImageDescription.Text;
                    UploadedImage.Path = gridDisplayPath;
                    UploadedImage.Extension = type;
                    //UploadedImage.Active = "Y";
                    if (rbtnYes.Checked == true)
                    { UploadedImage.Active = "Y"; }
                    else
                    { UploadedImage.Active = "N"; }
                    UploadedImage.CreatedBy = profile.Personal.UserID.ToString();
                    UploadedImage.CreationDate = DateTime.Now;
                    UploadedImage.CompanyID = profile.Personal.CompanyID;
                    productClient.AddTempProductImages(UploadedImage, Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                }

                GVImages.DataSource = productClient.GetTempSaveProductImagesBySessionID(Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                GVImages.DataBind();
                txtImageDescription.Text = "";
                txtImageTitle.Text = "";
            }
        }
        #endregion

        #region Prodcut Vendor Details
        protected void GetProductVendorsByProductID()
        {

        }
        protected void FinalSaveProductVendorsByProductID()
        {

        }
        #endregion

        #region Prodcut Documents
        protected void GetProductDocumentByProductID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdnprodID.Value == "") hdnprodID.Value = "0";
            UC_AttachDocument1.FillDocumentByObjectNameReferenceID(Convert.ToInt64(hdnprodID.Value), "Product", "Product");
        }
        protected void FinalSaveProductDocumentByProductID()
        {

        }
        #endregion

        #region Bind Dropdown

        public void BindActivity()
        {
            BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient ActivityList = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
            
            CustomProfile profile = CustomProfile.GetProfile();
            ddlactivity.DataSource = ActivityList.GetActivityList(profile.DBConnection._constr);
            ddlactivity.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlactivity.Items.Insert(0, lst);
        }

        public void BindMessageType()
        {
            BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient MessageList = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
           
            CustomProfile profile = CustomProfile.GetProfile();
            ddlMessageType.DataSource = MessageList.GetMessageList(profile.DBConnection._constr);
            ddlMessageType.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlMessageType.Items.Insert(0, lst);
        }


        public void BindCustomer()
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompany.DataSource = userClient.GetCompanyName(profile.DBConnection._constr);
            ddlcompany.DataBind();
            ListItem lst = new ListItem();
            lst.Text = "--Select--";
            lst.Value = "0";
            ddlcompany.Items.Insert(0, lst);
        }
        public void Bindropdown()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            // ddlProductType.DataSource = productClient.GetProductTypeList(profile.DBConnection._constr);
            // ddlProductType.DataBind();

            ListItem lst1 = new ListItem();
            lst1.Text = "-Select-";
            lst1.Value = "0";
            //ddlProductType.Items.Insert(0, lst1);

            //ddlUOM.SelectedIndex = -1;
            //ddlUOM.DataSource = productClient.GetProductUOMList(profile.DBConnection._constr);
            //ddlUOM.DataBind();
            //ListItem lst3 = new ListItem();
            //lst3.Text = "-Select-";
            //lst3.Value = "0";
            //ddlUOM.Items.Insert(0, lst3);
            productClient.Close();

            BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient productcategoryClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
            //ProductCategoryService.connectiondetails conn = new ProductCategoryService.connectiondetails() { DataBaseName = Profile.DataBase, DataSource = Profile.DataSource, DBPassword = Profile.DBPassword };

            //ddlCategory.SelectedIndex = -1;
            // ddlCategory.DataSource = productcategoryClient.GetProductCategoryList(profile.DBConnection._constr);
            //ddlCategory.DataBind();
            productcategoryClient.Close();
            ListItem lst2 = new ListItem();
            lst2.Text = "-Select-";
            lst2.Value = "0";
            // ddlCategory.Items.Insert(0, lst2);
        }

        public void ddl_Category_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductSubCategory();
        }

        protected void BindProductSubCategory()
        {
            //ddlSubCategory.Items.Clear();

            //if (ddlCategory.SelectedIndex > 0)
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    ProductSubCategoryService.iProductSubCategoryMasterClient productsubcategoryClient = new ProductSubCategoryService.iProductSubCategoryMasterClient();
            //    //ProductSubCategoryService.connectiondetails conn = new ProductSubCategoryService.connectiondetails() { DataBaseName = Profile.DataBase, DataSource = Profile.DataSource, DBPassword = Profile.DBPassword };
            //    ddlSubCategory.DataSource = productsubcategoryClient.GetProductSubCategoryByProductCategoryID(Convert.ToInt32(ddlCategory.SelectedItem.Value), profile.DBConnection._constr);
            //    ddlSubCategory.DataBind();
            //    productsubcategoryClient.Close();
            //}
            //if (ddlSubCategory.Items.Count > 0)
            //{
            //    if (ddlSubCategory.Items[0].Text != "Not available")
            //    {
            //        ListItem lst = new ListItem();
            //        lst.Text = "-Select-";
            //        lst.Value = "0";
            //        ddlSubCategory.Items.Insert(0, lst);
            //    }
            //}
        }
        #endregion

        protected void BtnSubMitproductSp_Click(object sender, EventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                //iProductMasterClient productClient = new iProductMasterClient();
               
                List<mProductSpecificationDetail> ProductSpecificationDetail = new List<mProductSpecificationDetail>();
                CustomProfile profile = CustomProfile.GetProfile();
                mProductSpecificationDetail oprodspec = new mProductSpecificationDetail();
                if (Hndstate.Value == "Edit")
                {
                    oprodspec = productClient.GetSpecificationDetailFromTempTableBySequence(Session.SessionID, profile.Personal.UserID.ToString(), 0, Convert.ToInt16(hndsequence.Value), profile.DBConnection._constr);
                    oprodspec.Sequence = Convert.ToInt64(hndsequence.Value);
                }
                else
                {
                    oprodspec.Sequence = 0;
                }
                oprodspec.SpecificationTitle = txtspecificationtitle.Text; ;
                oprodspec.SpecificationDescription = txtSpecificationDesc.Text;
                oprodspec.Active = "Y";
                oprodspec.ProductID = Convert.ToInt32(hdnprodID.Value);
                oprodspec.CreatedBy = profile.Personal.UserID.ToString(); //need to change
                oprodspec.CreationDate = DateTime.Now;
                oprodspec.CompanyID = profile.Personal.CompanyID; // need to change
                //int upprodsperesult = productClient.InserttProductSpecificationDetail(oprodspec, profile.DBConnection._constr);
                if (Hndstate.Value == "Edit")
                {
                    ProductSpecificationDetail = productClient.SetValuesToTempData_onChange(0, Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr, Convert.ToInt16(hndsequence.Value), oprodspec).ToList();
                }
                else
                {
                    ProductSpecificationDetail = productClient.AddProductSpecificationToTempData(oprodspec, Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();

                }
                GVProductSpecification.DataSource = ProductSpecificationDetail;
                GVProductSpecification.DataBind();
                productClient.Close();
                Hndstate.Value = "";
                clr();

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email template", "BtnSubMitproductSp_Click");
            }
            finally
            {
                productClient.Close();
            }
        }

        private void clr()
        {
            txtSpecificationDesc.Text = "";
            txtspecificationtitle.Text = "";
        }

        protected void imgBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Hndstate.Value = "Edit";
                Hashtable selectedrec = (Hashtable)GVProductSpecification.SelectedRecords[0];
                hndsequence.Value = selectedrec["Sequence"].ToString();

                GetRecordFromTempTableForUpdate();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email template", "imgBtnEdit_Click");
            }
            finally
            {
            }
        }

        private void GetRecordFromTempTableForUpdate()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mProductSpecificationDetail FillList = new mProductSpecificationDetail();

                iProductMasterClient productClient = new iProductMasterClient();

                FillList = productClient.GetSpecificationDetailFromTempTableBySequence(Session.SessionID, profile.Personal.UserID.ToString(), 0, Convert.ToInt16(hndsequence.Value), profile.DBConnection._constr);

                txtspecificationtitle.Text = FillList.SpecificationTitle;
                txtSpecificationDesc.Text = FillList.SpecificationDescription;
                //AddressClient.Close();
                productClient.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email template", "GetRecordFromTempTableForUpdate");
            }
            finally
            {
            }
        }

        #region Toolbar Code
        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            clear();
            GetProductSpecificationDetailByProductID();
            GetProductTaxDetailByProductID();
            GetProductImagesByProductID();
            GVRateHistory();
           // FillInventoryGrid();
            productClient.DeleteDistribution(profile.DBConnection._constr);
            setActiveTab(1);
            hdnTemplateID.Value = "";
            productClient.Close();
            // changePrice1.Attributes.Add("style", "visibility:hidden");
        }

        protected void pageImport(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { Response.Redirect("../Import/Import.aspx?Objectname=" + "Product"); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            //FinalSaveProductDetailByProductID();
            //FinalSaveProductDetailByProductID();
            iProductMasterClient productClient = new iProductMasterClient();

            try
            {
                  //string state;
                string CancelStatus = "";
                  string result = "";

                CustomProfile profile = CustomProfile.GetProfile();
               // int val = ddlDepartment.SelectedIndex;
                if (ddlcompany.SelectedItem.Text == "--Select--" || hdnSelectedDepartment.Value == "" || ddlactivity.SelectedItem.Text == "--Select--" || ddlMessageType.SelectedItem.Text == "--Select--")
                {
                    WebMsgBox.MsgBox.Show("Please select All the Information");
                }
                else
                {
                    if (checkDuplicate() == "")
                    {

                        BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient TemplateClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
                        mMessageEMailTemplate obj = new mMessageEMailTemplate();
                        if (hdnTemplateID.Value != "0" && hdnTemplateID.Value != "")
                        {
                            CancelStatus = TemplateClient.GetAutoCancellationStatus(Convert.ToInt64(ddlDepartment.SelectedValue), profile.DBConnection._constr);
                        }
                        else
                        {
                            CancelStatus = TemplateClient.GetAutoCancellationStatus(Convert.ToInt64(hdnSelectedDepartment.Value), profile.DBConnection._constr);
                        }

                        if (CancelStatus == "No")
                        {
                            WebMsgBox.MsgBox.Show("You Can Not Create Email Template For Auto Cancellation Status = No");
                        }
                        else if (ddlactivity.SelectedItem.Text == "Auto Cancellation" && CancelStatus == "Yes")
                        {
                            if (ddlMessageType.SelectedItem.Text == "Action")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                                //WebMsgBox.MsgBox.Show("You can't select more than " + txtnoapprovar.Text + " users");
                            }
                            if (ddlMessageType.SelectedItem.Text == "Information")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                            }
                        }
                        //else if (ddlactivity.SelectedItem.Text == "Order Submit" && (ddlMessageType.SelectedItem.Text == "Action" || ddlMessageType.SelectedItem.Text == "Reminder"))
                        //{
                        //    if (ddlMessageType.SelectedItem.Text == "Action")
                        //    {
                        //        WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                        //        //WebMsgBox.MsgBox.Show("You can't select more than " + txtnoapprovar.Text + " users");
                        //    }
                        //    if (ddlMessageType.SelectedItem.Text == "Reminder")
                        //    {
                        //        WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                        //    }
                        //}
                        else if (ddlactivity.SelectedItem.Text == "Order Approve" && ddlMessageType.SelectedItem.Text == "Information")
                        {
                            WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                        }

                        else if (ddlactivity.SelectedItem.Text == "Order Reject" && (ddlMessageType.SelectedItem.Text == "Action" || ddlMessageType.SelectedItem.Text == "Reminder"))
                        {
                            if (ddlMessageType.SelectedItem.Text == "Action")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                                //WebMsgBox.MsgBox.Show("You can't select more than " + txtnoapprovar.Text + " users");
                            }
                            if (ddlMessageType.SelectedItem.Text == "Reminder")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                            }
                        }

                        else if (ddlactivity.SelectedItem.Text == "Order Dispatch" && (ddlMessageType.SelectedItem.Text == "Action" || ddlMessageType.SelectedItem.Text == "Reminder"))
                        {
                            if (ddlMessageType.SelectedItem.Text == "Action")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                                //WebMsgBox.MsgBox.Show("You can't select more than " + txtnoapprovar.Text + " users");
                            }
                            if (ddlMessageType.SelectedItem.Text == "Reminder")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                            }
                        }

                        else if (ddlactivity.SelectedItem.Text == "Order Completion" && (ddlMessageType.SelectedItem.Text == "Action" || ddlMessageType.SelectedItem.Text == "Reminder"))
                        {
                            if (ddlMessageType.SelectedItem.Text == "Action")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                                //WebMsgBox.MsgBox.Show("You can't select more than " + txtnoapprovar.Text + " users");
                            }
                            if (ddlMessageType.SelectedItem.Text == "Reminder")
                            {
                                WebMsgBox.MsgBox.Show("You Can Not Select Message Type " + ddlMessageType.SelectedItem.Text + " for " + ddlactivity.SelectedItem.Text);
                            }
                        }

                        else
                        {
                            obj.MailSubject = txtSubject.Text;
                            obj.MailBody = editEmail.Content;
                           // obj.Active = "Yes";
                            if (hdnTemplateID.Value != "0" && hdnTemplateID.Value != "")
                            {
                                obj.CompanyID = Convert.ToInt64(ddlcompany.SelectedValue);
                                obj.DepartmentID = Convert.ToInt64(ddlDepartment.SelectedValue);

                            }
                            else
                            {
                                obj.CompanyID = Convert.ToInt64(hdnSelectedCompany.Value);
                                obj.DepartmentID = Convert.ToInt64(hdnSelectedDepartment.Value);
                            }


                            obj.ActivityID = Convert.ToInt64(ddlactivity.SelectedValue);
                            obj.MessageID = Convert.ToInt64(ddlMessageType.SelectedValue);
                            obj.TemplateTitle = txttitle.Text;
                            // obj.ObjectName = null;
                            // obj.mStatusID = 0;
                            //obj.MailType = "";

                            if (rbtYes.Checked == true) { obj.Active = "Yes"; } else { obj.Active = "No"; } //New Added

                            if (hdnTemplateID.Value != "0" && hdnTemplateID.Value != "")
                            {
                                DataSet ds = new DataSet();
                                Hndstate.Value = "Edit";
                                obj.ModifiedBy = Convert.ToInt64(profile.Personal.UserID.ToString());
                                obj.ModifiedDate = DateTime.Now;
                                TemplateClient.UpdateEmailTemplate(hdnTemplateID.Value, txtSubject.Text, editEmail.Content, Convert.ToInt64(profile.Personal.UserID.ToString()), Convert.ToInt64(ddlcompany.SelectedValue), Convert.ToInt64(ddlDepartment.SelectedValue), Convert.ToInt64(ddlactivity.SelectedValue), Convert.ToInt64(ddlMessageType.SelectedValue), txttitle.Text,obj.Active,profile.DBConnection._constr);
                                productClient.UpdateDistribution(long.Parse(hdnTemplateID.Value), profile.DBConnection._constr);
                            }
                            else
                            {
                                Hndstate.Value = "AddNew";
                                obj.CreatedBy = Convert.ToInt64(profile.Personal.UserID.ToString());
                                obj.CreationDate = DateTime.Now;
                                result = TemplateClient.InsertEmailTemplate(obj, profile.DBConnection._constr).ToString();
                                productClient.UpdateDistribution(long.Parse(result), profile.DBConnection._constr);
                            }

                            if (result != "")
                            {
                                WebMsgBox.MsgBox.Show("Record saved successfully");
                                clearALL();
                            }
                            if (Hndstate.Value == "Edit")
                            {
                                WebMsgBox.MsgBox.Show("Record Update successfully");
                                clearALL();
                            }

                            setActiveTab(0);

                        }
                    }
                }
                      
            }
            catch (System.Exception ex)
            {
               // Login.Profile.ErrorHandling(ex, this, "Product master", "pageSave");
            }
            finally
            {
            }
          
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            //clear(); 
            //setActiveTab(0); 
            //GetProductList(); 
            ddlcompany.SelectedIndex = 0;
            ddlDepartment.SelectedIndex = 0;
            ddlactivity.SelectedIndex = 0;
            ddlMessageType.SelectedIndex = 0;
            txttitle.Text = "";
            txtSubject.Text = "";
            editEmail.Content = "";
            clearALL();
            setActiveTab(1);
            productClient.DeleteDistribution(profile.DBConnection._constr);
            Grid1.DataSource = null;
            Grid1.DataBind();
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            //ImageButton imgbtn = (ImageButton)sender;
            //clear();
            //hdnprodID.Value = imgbtn.ToolTip.ToString();
            //GetProductDetailByProductID();

            Hashtable selectedrec = (Hashtable)gvUserCreationM.SelectedRecords[0];
            hdnTemplateID.Value = selectedrec["userID"].ToString();
            setActiveTab(1);

        }

        protected void gvUserCreationM_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Hndstate.Value = "Edit";
                productClient.DeleteDistribution(profile.DBConnection._constr);
                Hashtable selectedrec = (Hashtable)gvUserCreationM.SelectedRecords[0];
                hdnTemplateID.Value = selectedrec["ID"].ToString();
                BindCustomer();
                ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(selectedrec["CompanyID"].ToString()));

                if (ddlcompany.SelectedValue != "0" || ddlcompany.SelectedValue != "")
                {

                    FillDept(Convert.ToInt64(ddlcompany.SelectedValue));
                    ddlDepartment.SelectedValue = selectedrec["DepartmentID"].ToString();
                    hdnSelectedDepartment.Value = selectedrec["DepartmentID"].ToString();
                }

                ddlactivity.SelectedIndex = ddlactivity.Items.IndexOf(ddlactivity.Items.FindByValue(selectedrec["ActivityID"].ToString()));
                ddlMessageType.SelectedIndex = ddlMessageType.Items.IndexOf(ddlMessageType.Items.FindByValue(selectedrec["MessageID"].ToString()));
                txttitle.Text= selectedrec["TemplateTitle"].ToString();
                txtSubject.Text = selectedrec["MailSubject"].ToString();
                editEmail.Content = selectedrec["MailBody"].ToString();

                if(selectedrec["Active"].ToString()=="Yes"){ rbtYes.Checked=true; } else { rbtNo.Checked=true; }

                if (selectedrec["ActivityID"].ToString() == "9" || selectedrec["ActivityID"].ToString() == "10" || selectedrec["ActivityID"].ToString() == "11")
                {
                    rbtYes.Enabled = true; rbtNo.Enabled = true;
                }
                else { rbtYes.Enabled = false; rbtNo.Enabled = false; }

                RebindGrid(sender, e);
                setActiveTab(1);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email Template", "gvUserCreationM_Select");

            }
            finally
            {
            }
        }



        protected void clear()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            // ddlProductType.SelectedIndex = -1;
            //ddlCategory.SelectedIndex = -1;
            // ddlSubCategory.SelectedIndex = -1;
            //txtProductCode.Text = "";
           // txtProductName.Text = "";
            // ddlUOM.SelectedIndex = -1;
            //txtPrincipalPrice.Text = "";
            //txtFixedDisc.Text = "";
            //chkboxFixedDiscIsPercent.Checked = false;
            //chkProductSpe.Items[0].Selected = false;
            //chkProductSpe.Items[1].Selected = false;
            //txtWarrenyInDays.Text = "0";
            //txtGuaranteeInDays.Text = "0";
            //txtOpeningBalance.Text = "";
            //txtCurrentBalance.Text = "";
            //txtReorderLevel.Text = "";
            //txtMaxBalanceQuantity.Text = "";
            //txtMinOrderQuantity.Text = "";
            //txtLeadTimeInDays.Text = "";
            hdnprodID.Value = null;

            GVImages.DataSource = null;
            GVImages.DataBind();

            GVProductSpecification.DataSource = null;
            GVProductSpecification.DataBind();

            GVTaxSetup.DataSource = null;
            GVTaxSetup.DataBind();

            iProductMasterClient productClient = new iProductMasterClient();
            productClient.ClearTempSaveProductSpecificationDetailBySessionID(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);

            productClient.ClearTempSaveProductTaxDetailBySessionID(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);

            productClient.ClearTempSaveProductImagesBySessionID(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), profile.DBConnection._constr);

            productClient.Close();
            ResetUserControl();

        }

        protected void setActiveTab(int ActiveTab)
        {

            if (ActiveTab == 0)
            {
                TabPanelProductList.Visible = true;
                tabProductDetails.Visible = false;
                pnldistribution.Visible = false;
                tabSpecification.Visible = false;
                tabTaxSetup.Visible = false;
                tabImages.Visible = false;
                Bindropdown();
                GetTemplateList();
                tabDocuments.Visible = false;
                tabInventory.Visible = false;
                tabVendor.Visible = false;
                TabContainerProductMaster.ActiveTabIndex = 0;
            }
            //else if (ActiveTab == 1)
            //{
            //    tabProductDetails.Visible = false;
            //    pnldistribution.Visible = true;
            //    tabSpecification.Visible = false;
            //    //tabTaxSetup.Visible = true;
            //    tabTaxSetup.Visible = false;
            //    tabImages.Visible = false;
            //    tabDocuments.Visible = false;
            //    tabInventory.Visible = false;
            //    tabVendor.Visible = false;
            //    TabContainerProductMaster.ActiveTabIndex = 1;
            //}
            else
            {
                tabProductDetails.Visible = true;
                pnldistribution.Visible = true;
                tabSpecification.Visible = false;
                //tabTaxSetup.Visible = true;
                tabTaxSetup.Visible = false;
                tabImages.Visible = false;
                tabDocuments.Visible = false;
                tabInventory.Visible = false;
                tabVendor.Visible = false;
                TabContainerProductMaster.ActiveTabIndex = 1;
            }
        }
        #endregion

        #region  checkDuplicate
        public string checkDuplicate()
        {
            try
            {
                string result = "";
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient TemplateClient = new BrilliantWMS.ProductCategoryService.iProductCategoryMasterClient();
              
                if (hdnTemplateID.Value == string.Empty)
                {
                    result = TemplateClient.checkDuplicateTemplate(Convert.ToInt64(ddlcompany.SelectedValue), Convert.ToInt64(hdnSelectedDepartment.Value), Convert.ToInt64(ddlactivity.SelectedValue), Convert.ToInt64(ddlMessageType.SelectedValue), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        //txtProductName.Text = "";
                    }
                    //txtProductName.Focus();
                }
                else
                {
                    //result = productClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnprodID.Value), txtProductName.Text.Trim(), profile.DBConnection._constr);
                    result = TemplateClient.checkDuplicateTemplateEdit(Convert.ToInt32(hdnTemplateID.Value), Convert.ToInt64(ddlcompany.SelectedValue), Convert.ToInt64(ddlDepartment.SelectedValue), Convert.ToInt64(ddlactivity.SelectedValue), Convert.ToInt64(ddlMessageType.SelectedValue), profile.DBConnection._constr);
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

        #endregion

        [WebMethod]
        public static List<vGetProductSubCagetoryList> PMprint_ProductSubCategory(long ProductSubCategoryID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient productsubcategoryClient = new BrilliantWMS.ProductSubCategoryService.iProductSubCategoryMasterClient();
            List<vGetProductSubCagetoryList> SubCategoryList = new List<vGetProductSubCagetoryList>();
            SubCategoryList = productsubcategoryClient.GetProductSubCategoryByProductCategoryID(ProductSubCategoryID, profile.DBConnection._constr).ToList();
            return SubCategoryList;
        }

        #region Rate History Code
        [WebMethod]
        public static string PMSaveNewRates(object newPrice0)
        {
            string result = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)newPrice0;

                mProductRateDetail newPrice = new mProductRateDetail();
                if (dictionary["ProdID"].ToString() != "0" && dictionary["ProdID"].ToString() != "")
                {
                    newPrice.ProdID = Convert.ToInt64(dictionary["ProdID"].ToString());
                    newPrice.Rate = Convert.ToDecimal(dictionary["Rate"].ToString());
                    if (dictionary["EffectiveDate"] != null) newPrice.EffectiveDate = Convert.ToDateTime(dictionary["EffectiveDate"]);
                    if (dictionary["StartDate"] != null) newPrice.StartDate = Convert.ToDateTime(dictionary["StartDate"]);
                    if (dictionary["EndDate "] != null) newPrice.EndDate = Convert.ToDateTime(dictionary["EndDate"]);
                    result = "";
                }
                else
                {
                    result = "Please save the product / part first";
                }
            }
            catch (Exception ex) { }
            finally { }
            return result;
        }

        protected void GVRateHistory()
        {
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // GVRateHistory1.DataSource = null;
                // GVRateHistory1.DataBind();

                if (hdnprodID.Value != "" && hdnprodID.Value != "0")
                {
                    //GVRateHistory1.DataSource = productClient.GetProductRateHistory(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
                    //GVRateHistory1.DataBind();
                }
            }
            catch (Exception ex) { Login.Profile.ErrorHandling(ex, "ProductMaster", "GVRateHistory"); }
            finally { productClient.Close(); }
        }

        protected void GVRateHistory_OnRebind(object sender, EventArgs e)
        {
            GVRateHistory();
        }
        #endregion

        #region Inventory code

        protected void FillInventoryGrid()
        {
            iProductMasterClient objService = new iProductMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<SP_GetSiteWiseInventoryByProductIDs_Result> InventoryList = new List<SP_GetSiteWiseInventoryByProductIDs_Result>();
                InventoryList = objService.GetInventoryDataByProductIDs(hdnprodID.Value, Session.SessionID, profile.Personal.UserID.ToString(), CurrentObject, profile.DBConnection._constr).ToList();
                GVInventory.DataSource = null;
                GVInventory.DataBind();
                GVInventory.DataSource = InventoryList;
                GVInventory.DataBind();

                if (InventoryList.Count > 0)
                {
                    if (InventoryList[0].EffectiveDate != null)
                    {
                        // UC_EffectiveDateInventory.Date = InventoryList[0].EffectiveDate;
                    }
                }
            }
            catch { }
            finally { objService.Close(); }
        }

        [WebMethod]
        public static long WMUpdateInventoryQty(long SiteID, long OpeningStock, long MaxStockLimit, long ReorderQty)
        {
            iProductMasterClient objService = new iProductMasterClient();
            long AvailableBalance = 0;
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                SP_GetSiteWiseInventoryByProductIDs_Result rec = new SP_GetSiteWiseInventoryByProductIDs_Result();
                rec.SiteID = SiteID;
                rec.OpeningStock = OpeningStock;
                rec.ReorderQty = ReorderQty;
                rec.MaxStockLimit = MaxStockLimit;
                AvailableBalance = objService.UpdateProductInvetory_TempData(HttpContext.Current.Session.SessionID.ToString(), CurrentObject, profile.Personal.UserID.ToString(), rec, profile.DBConnection._constr);
            }
            catch { }
            finally { objService.Close(); }
            return AvailableBalance;
        }
        #endregion

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

               // TabPanelProductList.HeaderText = rm.GetString("EmailTemplateList", ci);
              //  lblrolllist.Text = rm.GetString("EmailTemplateList", ci);
                tabProductDetails.HeaderText = rm.GetString("TemplateDetails", ci);
               // lblcompany.Text = rm.GetString("company", ci);
               // lbldepartment.Text = rm.GetString("Department", ci);
                lblactivity.Text = rm.GetString("Activity", ci);
                lblTemplate.Text = rm.GetString("TemplateTitle", ci);
                lblsubject.Text = rm.GetString("Subject", ci);
                lblmessage.Text = rm.GetString("Body", ci);
                lblMessageType.Text = rm.GetString("MsgType", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("EmailTemplate", ci);
                btnContactPerson.Value = rm.GetString("AddContact", ci);
                pnldistribution.HeaderText = rm.GetString("AdditionalDistribution", ci);
                lbldisrtibution.Text = rm.GetString("AdditionalDistribution", ci);
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Email template", "loadstring");
            }

        }

        protected void clearALL()
        {
            ddlcompany.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlactivity.SelectedIndex = -1;
            ddlMessageType.SelectedIndex = -1;
            txttitle.Text = "";
            txtSubject.Text = "";
            hdnprodID.Value = null;
            editEmail.Content = "";
            ResetUserControl();
        }


        [WebMethod]
        public static List<mTerritory> PMGetDepartmentList(long CompanyID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
            }
            catch { }
            finally { }
            return TerritoryList;
        }

        public void FillDept(long CompanyID)
        {
            List<mTerritory> TerritoryList = new List<mTerritory>();
            try
            {
                UC_Territory uc_territory = new UC_Territory();
                TerritoryList = uc_territory.GetDepartmentList(CompanyID).ToList();
                ddlDepartment.DataSource = TerritoryList;
                ddlDepartment.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlDepartment.Items.Insert(0, lst);

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Role Master", "FillDept");

            }
            finally
            {
            }
        }


        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst();

                Grid1.DataSource = ds;
                Grid1.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst()
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            if (Hndstate.Value != "Edit")
            {
                str = "select ad.Id,cp.ID as contactid ,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID inner join mAddDistribution ad on cp.ID = ad.ContactID where cp.ID in (Select ContactID from mAddDistribution where ad.TemplateID = 0)";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "select ad.Id,cp.ID as contactid ,c.Name company, t.Territory, cp.Name, cp.EmailID  from tContactPersonDetail cp inner join mCompany c on cp.CompanyID = c.ID inner join mTerritory t on cp.Department = t.ID inner join mAddDistribution ad on cp.ID = ad.ContactID where cp.ID in (Select ContactID from mAddDistribution where ad.TemplateID = 0 or ad.TemplateID = "+ long.Parse(hdnTemplateID.Value) +")";
                // str = "select ProductCode,OMSSKUCode,Name,Description,from mProduct mp where StoreId = '" + StoreId + "' and mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%' ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        [WebMethod]
        public static string RemoveSku(object objReq)
        {
            string result = "";
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;

            long DistributionID = long.Parse(dictionary["distribId"].ToString());
            //productClient.RemoveBOMDetailSKu(BOMDetailsku, profile.DBConnection._constr);
            productClient.RemoveDistribution(DistributionID, profile.DBConnection._constr);
            result = "success";
            return result;
        }

        [WebMethod]
        public static List<contact> GetDepartment(object objReq)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
                long ddlcompanyId = long.Parse(dictionary["ddlcompanyId"].ToString());

                ds = productClient.GetDepartment(ddlcompanyId, profile.DBConnection._constr);

                dt = ds.Tables[0];


                contact Loc = new contact();
                Loc.Name = "Select Department";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Territory"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();

                    }

                }
            }
            catch(Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "ProductSearch.aspx.cs", "RebindGrid");
            }
            finally
            {
                productClient.Close();
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