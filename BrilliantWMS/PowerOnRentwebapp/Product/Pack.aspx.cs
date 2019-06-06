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

namespace BrilliantWMS.Product
{
    public partial class Pack : System.Web.UI.Page
    {
        static string sessionID;
        static string CurrentObject = "ProductInventoryDetail";
        protected void Page_Load(object sender, EventArgs e)
        {
            //sessionID = Session.SessionID.ToString();
            sessionID = Session.SessionID.ToString();
            UCFormHeader1.FormHeaderText = "Pack Master";
            if (!IsPostBack)
            {
                Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                btnExport.Visible = false;
                Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                btnImport.Visible = false;
                Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                btmMail.Visible = false;
                Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                btnPrint.Visible = false;
                //Button btnConvertTo = (Button)UCToolbar1.FindControl("btnConvertTo");
                //btnConvertTo.Visible = false;

                ResetUserControl();
                GetProductList();
                setActiveTab(0);
                ScriptManager.RegisterStartupScript(this.FlyoutChangeProdPrice, FlyoutChangeProdPrice.GetType(), "reg1", "SaveNewPrice();", false);
            }
            //this.UCToolbar1.ToolbarAccess("ProductMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
            //this.UCToolbar1.evClickImport += pageImport;
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


        /*End*/
        

        #region Product Info

        protected void GetProductDetailByProductID()
        {
           // CustomProfile profile = CustomProfile.GetProfile();
           // iProductMasterClient productClient = new iProductMasterClient();
           // GetProductDetail obj = new GetProductDetail();
           // obj = productClient.GetProductDetailByProductID(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
           // productClient.Close();

           //// if (obj.ProductTypeID != null) ddlProductType.SelectedIndex = ddlProductType.Items.IndexOf(ddlProductType.Items.FindByValue(obj.ProductTypeID.Value.ToString()));

           // //if (obj.ProductCategoryID != null) ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(obj.ProductCategoryID.Value.ToString()));
           // BindProductSubCategory();
           // //if (obj.ProductSubCategoryID != null) ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByValue(obj.ProductSubCategoryID.Value.ToString()));
           // if (obj.ProductCode != null) txtProductCode.Text = obj.ProductCode.ToString();
           // if (obj.Name != null) txtProductName.Text = obj.Name.ToString();
           // if (obj.UOMID != null) ddlUOM.SelectedIndex = ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(obj.UOMID.Value.ToString()));
           // if (obj.PrincipalPrice != null) txtPrincipalPrice.Text = obj.PrincipalPrice.ToString();
           // //if (obj.FixedDiscount != null) txtFixedDisc.Text = obj.FixedDiscount.ToString();
           // //if (obj.FixedDiscountPercent != null) chkboxFixedDiscIsPercent.Checked = Convert.ToBoolean(obj.FixedDiscountPercent);
           // //if (obj.Installable != null) chkProductSpe.Items[0].Selected = Convert.ToBoolean(obj.Installable);
           // //if (obj.AMC != null) chkProductSpe.Items[1].Selected = Convert.ToBoolean(obj.AMC);
           // //if (obj.WarrantyDays != null) txtWarrenyInDays.Text = obj.WarrantyDays.ToString();
           // //if (obj.GuaranteeDays != null) txtGuaranteeInDays.Text = obj.GuaranteeDays.ToString();
           // rbtNo.Checked = false;
           // rbtYes.Checked = false;
           // if (obj.Active != null)
           // {
           //     if (obj.Active == "N") rbtNo.Checked = true;
           //     if (obj.Active == "Y") rbtYes.Checked = true;
           // }
           // GetProductDocumentByProductID();
           // GetProductSpecificationDetailByProductID();
           // GetProductTaxDetailByProductID();
           // GetProductImagesByProductID();
           // GVRateHistory();
           // FillInventoryGrid();
           // changePrice1.Attributes.Add("style", "visibility:visible");
        }

        protected void FinalSaveProductDetailByProductID()
        {
            try
            {
                //string state;
                //CustomProfile profile = CustomProfile.GetProfile();
                //if (checkDuplicate() == "")
                //{

                //    iProductMasterClient productClient = new iProductMasterClient();
                //    mProduct obj = new mProduct();
                //    if (hdnprodID.Value != "0")
                //    {
                //        state = "Edit";
                //        obj = productClient.GetmProductToUpdate(Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
                //        obj.LastModifiedBy = profile.Personal.UserID.ToString();
                //        obj.LastModifiedDate = DateTime.Now;
                //    }
                //    else
                //    {
                //        state = "AddNew";
                //        obj.CreatedBy = profile.Personal.UserID.ToString();
                //        obj.CreationDate = DateTime.Now;
                //    }

                //    //obj.ProductTypeID = Convert.ToInt64(ddlProductType.SelectedItem.Value);
                //    //obj.ProductCategoryID = Convert.ToInt64(ddlCategory.SelectedItem.Value);
                //   // if (ddlSubCategory.SelectedIndex > 0) obj.ProductSubCategoryID = Convert.ToInt64(ddlSubCategory.SelectedItem.Value);
                //    obj.ProductCode = txtProductCode.Text.ToString().Trim();
                //    obj.Name = txtProductName.Text.ToString().Trim();
                //    obj.UOMID = Convert.ToInt64(ddlUOM.SelectedItem.Value);
                //    if (txtPrincipalPrice.Text == "") txtPrincipalPrice.Text = "0";
                //    obj.PrincipalPrice = Convert.ToDecimal(txtPrincipalPrice.Text);

                //   // if (txtFixedDisc.Text == "") txtFixedDisc.Text = "0";
                //    //obj.FixedDiscount = Convert.ToDecimal(txtFixedDisc.Text);

                //    //obj.FixedDiscountPercent = chkboxFixedDiscIsPercent.Checked;
                //   // obj.Installable = chkProductSpe.Items[0].Selected;
                //   // obj.AMC = chkProductSpe.Items[1].Selected;
                //   // if (txtWarrenyInDays.Text == "") txtWarrenyInDays.Text = "0";
                //   // obj.WarrantyDays = Convert.ToInt32(txtWarrenyInDays.Text);
                //   // if (txtGuaranteeInDays.Text == "") txtGuaranteeInDays.Text = "0";
                //   // obj.GuaranteeDays = Convert.ToInt32(txtGuaranteeInDays.Text);
                //    obj.Active = "N";
                //    if (rbtYes.Checked == true) obj.Active = "Y";

                //    hdnprodID.Value = productClient.FinalSaveProductDetailByProductID(obj, profile.DBConnection._constr).ToString();
                //    productClient.FinalSaveProductTaxDetailByProductID(Session.SessionID, profile.Personal.UserID.ToString(), Convert.ToInt64(hdnprodID.Value), profile.DBConnection._constr);
                //    productClient.FinalSaveProductImagesByProductID(Session.SessionID.ToString(), profile.Personal.UserID.ToString(), Convert.ToInt64(hdnprodID.Value), Server.MapPath(""), profile.DBConnection._constr);
                //    productClient.FinalSaveProductSpecificationDetailByProductID(sessionID, profile.Personal.UserID.ToString(), Convert.ToInt64(hdnprodID.Value), Convert.ToInt64(profile.Personal.CompanyID), profile.DBConnection._constr);
                //    productClient.FinalSaveProductInventory(sessionID, CurrentObject, Convert.ToInt64(hdnprodID.Value), DateTime.Now, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                //    UC_AttachDocument1.FinalSaveDocument(Convert.ToInt64(hdnprodID.Value));
                //    productClient.Close();
                //    if (hdnprodID.Value != "0")
                //    {
                //        WebMsgBox.MsgBox.Show("Record saved sussessfully");
                //    }
                //    // clear();
                //    GetProductDocumentByProductID();
                //    GetProductSpecificationDetailByProductID();
                //    GetProductTaxDetailByProductID();
                //    GetProductImagesByProductID();
                //    GetProductList();
                //    GVRateHistory();
                //    FillInventoryGrid();
                //    setActiveTab(1);
                //    changePrice1.Attributes.Add("style", "visibility:visible");
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Product master", "pageSave");
            }
            finally
            {
            }
        }

        protected void GetProductList() /*Bind GridView*/
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            //grvProduct.DataSource = productClient.GetProductList(profile.DBConnection._constr);
            //grvProduct.DataBind();
            productClient.Close();
        }
        #endregion

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
            try
            {
                //iProductMasterClient productClient = new iProductMasterClient();
                iProductMasterClient productClient = new iProductMasterClient();
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
                Login.Profile.ErrorHandling(ex, this, "Productmaster", "BtnSubmit_Click");
            }
            finally
            {
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
                Login.Profile.ErrorHandling(ex, this, "Productmaster", "imgBtnEdit_Click");
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
                Login.Profile.ErrorHandling(ex, this, "UC_AddressInformation", "GetRecordFromTempTableForUpdate");
            }
            finally
            {
            }
        }

        #region Toolbar Code
        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
            GetProductSpecificationDetailByProductID();
            GetProductTaxDetailByProductID();
            GetProductImagesByProductID();
            GVRateHistory();
            FillInventoryGrid();
            setActiveTab(1);
           // changePrice1.Attributes.Add("style", "visibility:hidden");
        }

        protected void pageImport(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { Response.Redirect("../Import/Import.aspx?Objectname=" + "Product"); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            FinalSaveProductDetailByProductID();
            txtImageDescription.Text = "";
            txtImageTitle.Text = "";
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); setActiveTab(0); GetProductList(); }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            hdnprodID.Value = imgbtn.ToolTip.ToString();
            GetProductDetailByProductID();
            setActiveTab(1);

        }

        protected void clear()
        {
            CustomProfile profile = CustomProfile.GetProfile();
           // ddlProductType.SelectedIndex = -1;
            //ddlCategory.SelectedIndex = -1;
           // ddlSubCategory.SelectedIndex = -1;
            txtProductCode.Text = "";
            txtProductName.Text = "";
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
                TabPanelProductList.Visible = false;
                tabProductDetails.Visible = true;
                tabSpecification.Visible = false;
                tabTaxSetup.Visible = false;
                tabImages.Visible = false;
                Bindropdown();
                tabDocuments.Visible = false;
                tabInventory.Visible = false;
                tabVendor.Visible = false;
                TabContainerProductMaster.ActiveTabIndex = 0;
            }
            else
            {
                tabProductDetails.Visible = true;
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
                CustomProfile profile = CustomProfile.GetProfile();
                iProductMasterClient productClient = new iProductMasterClient();
                string result = "";
                if (hdnprodID.Value == string.Empty)
                {
                    result = productClient.checkDuplicateRecord(txtProductName.Text.Trim(), profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        //txtProductName.Text = "";
                    }
                    txtProductName.Focus();
                }
                else
                {
                    result = productClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnprodID.Value), txtProductName.Text.Trim(), profile.DBConnection._constr);
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
    }
}