using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Obout.Interface;
using System.Collections;
using Obout.Grid;
using BrilliantWMS.PopupMessages;
using System.Web.Services;
using BrilliantWMS.AccountSearchService;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using WebMsgBox;
using BrilliantWMS.DocumentService;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Net.Mail;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.CompanySetupService;
using Obout.Ajax.UI.FileUpload;
using BrilliantWMS.AddressInfoService;
using System.IO;
using System.Text.RegularExpressions;

namespace BrilliantWMS.Account
{
    public partial class Customer : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        Byte[] bytes;
        string filePath = string.Empty;
        string ObjectName = "Account";
        long CustHeadID = 0;
        Int64 ID = 0;
        static string sessionID = "";// { get; set; }
        string Emedium = "";
        string Mmedium = "";
        string Hmedium = "";
        string MediumList = "";

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void ResetUserControl()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                /*Set Values to UCFormHeader1*/
                //UCFormHeader1.FormHeaderText = "Customer Master";

                /*Set Values to UC_AddressInformation1*/
                UCAddress1.ClearAddress("_Address");


                /*Set Values to UC_ContactPerson1*/
                UCContactPerson1.ClearContactPerson("Customer");

                /*Set Values to UC_Document*/
                UC_AttachDocument1.ClearDocument("Customer");

                /*Set Values to UC_StatutoryDetails1*/
                UC_StatutoryDetails1.BindGridStatutoryDetails(0, "Customer", profile.Personal.CompanyID);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "ResetUserControl");
            }
            finally
            {
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            txtapproval.Attributes.Add("onblur", "CheckLevel();");
            imgSearch.Visible = false;
            sessionID = Session.SessionID;
            UCToolbar1.ParentPage = this;

            //  UCContactPerson1.ParentPage = this;

            if (!IsPostBack)
            {
                //bindDeptgrid(ID);
                TabCustomerList.Focus();
                MainCustomerGridBind();
                HdnAccountId.Value = HdnOpeningBalId.Value = null;
                if (Request.QueryString.ToString().Length <= 0)
                {
                    ActiveTab("NewChange");
                }
                else
                {
                    ActiveTab("Address");
                }
                SubscriptStartDate.startdate(DateTime.Now);
                SubscriptEndDate.startdate(DateTime.Now);
            }
            this.UCToolbar1.ToolbarAccess("Accounts");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
            if (hdnCountry.Value != "")
            {
                ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(hdnCountry.Value.ToString()));
                ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(hdnState.Value.ToString()));
            }
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                TabCustomerList.HeaderText = rm.GetString("CustomerList", ci);
                tabAccountInfo.HeaderText = rm.GetString("CustomerInfo", ci);
                lblcustname.Text = rm.GetString("CompanyName", ci);
                lblwebsite.Text = rm.GetString("Website", ci);
                lblemailid.Text = rm.GetString("EmailID", ci);
                lbladdress1.Text = rm.GetString("AddressLine1", ci);
                lbladdress2.Text = rm.GetString("AddressLine2", ci);
                lbladdress3.Text = rm.GetString("AddressLine3", ci);
                lblcountry.Text = rm.GetString("Country", ci);
                lblstate.Text = rm.GetString("State", ci);
                lblcity.Text = rm.GetString("City", ci);
                lblzip.Text = rm.GetString("ZIPCode", ci);
                lbllandmark.Text = rm.GetString("Landmark", ci);
                lblphone.Text = rm.GetString("PhoneNo", ci);
                lblfax.Text = rm.GetString("FaxNo", ci);
                lbllogo.Text = rm.GetString("UploadLogo", ci);
                btncustomernext.Text = rm.GetString("Next", ci);
                lblheasertext.Text = rm.GetString("CustomerList", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("CustomerList", ci);

                lblcharremain.Text = rm.GetString("charremain", ci);

                lblactive.Text = rm.GetString("Active", ci);
                rbtnActiveYes.Text = rm.GetString("Yes", ci);
                rbtnActiveNo.Text = rm.GetString("No", ci);
                lnkUpdateProfileImg.Text = rm.GetString("Upload", ci);
                tableDeptInfo.HeaderText = rm.GetString("DepartmentInfo", ci);
                lblcompany.Text = rm.GetString("Customer", ci);
                lbldepartment.Text = rm.GetString("Department", ci);
                lbldeptcode.Text = rm.GetString("DepartmentCode", ci);
                lblapproval.Text = rm.GetString("Approvallevel", ci);
                lblautocancel.Text = rm.GetString("AutoCancel", ci);
                lblcanceldays.Text = rm.GetString("CancelDays", ci);
                OboutRadioButton1.Text = rm.GetString("Yes", ci);
                OboutRadioButton2.Text = rm.GetString("No", ci);
                lblacti.Text = rm.GetString("Active", ci);
                btnadd.Text = rm.GetString("Submit", ci);
                // btnnextdept.Text = rm.GetString("Next", ci);
                lbldesilist.Text = rm.GetString("DeptList", ci);
                // tabContactInfo.HeaderText = rm.GetString("ContactPersonInfo", ci);
                // tabSatutoryInfo.HeaderText = rm.GetString("DesignationInfo", ci);
                tabAddressInfo.HeaderText = rm.GetString("AddressInfo", ci);
                //lbldepartmnt.Text = rm.GetString("Department", ci);
                //lblsequence.Text = rm.GetString("Sequence", ci);
                //lbldesignation.Text = rm.GetString("Designation", ci);
                //lblactiv2.Text = rm.GetString("Active", ci);
                //rbtnYes.Text = rm.GetString("Yes", ci);
                //rbtnNo.Text = rm.GetString("No", ci);
                //Button1.Text = rm.GetString("Next", ci);
                //Button2.Text = rm.GetString("Clear", ci);
                lblcharremain2.Text = rm.GetString("charremain", ci);
                lblcharremain3.Text = rm.GetString("charremain", ci);
                UCFormHeader1.FormHeaderText = rm.GetString("CustomerMaster", ci);
                lblappreminder.Text = rm.GetString("ApprovalReminder", ci);
                lblremaprrosche.Text = rm.GetString("ReminderSchedule", ci);
                lblremdays.Text = rm.GetString("Days", ci);
                lblautodays.Text = rm.GetString("Days", ci);

                lblaautoeminder.Text = rm.GetString("AutoCancelReminder", ci);
                lblautoremschedule.Text = rm.GetString("ReminderSchedule", ci);
                // lbldeptList.Text = rm.GetString("DeptList", ci);

                lblGWCDeliveries.Text = rm.GetString("GWCDeliveries", ci);
                lblecommerce.Text = rm.GetString("ECommerce", ci);
                lblorderformat.Text = rm.GetString("OrderNoFormat", ci);
                lblecommerce.Text = rm.GetString("ECommerce", ci);
                lbldeliverydays.Text = rm.GetString("MaxDeliveryDays", ci);
                // lbladdresstype.Text = rm.GetString("AddressType", ci);
                lblfinapprover.Text = rm.GetString("FinancialApprover", ci);
                lblpricechange.Text = rm.GetString("PriceChange", ci);


                lbldeptList.Text = rm.GetString("CostCenterList", ci);
                // tabSatutoryInfo.HeaderText = rm.GetString("CostCenter", ci);
                lblcostcenter.Text = rm.GetString("CostCenterName", ci);
                lblcode.Text = rm.GetString("Code", ci);
                lblcostapprover.Text = rm.GetString("FinancialApprover", ci);
                lblRemark.Text = rm.GetString("Remark", ci);
                btnaddapprover.Value = rm.GetString("Add", ci);



            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "loadstring");
            }
        }


        #region Customer form code for GWC
        protected void btncustomernext_Click(object sender, System.EventArgs e)
        {
        }

        public void saveimage()
        {
            string fileName = string.Empty;

            FileStream fs;
            BinaryReader br;
            if (fileUploadCompanyLogo.HasFile)
            {
                string filePath1 = fileUploadCompanyLogo.PostedFile.FileName;
                string filename = Path.GetFileName(filePath1);
                string ext = Path.GetExtension(filePath1);
                fileName = fileUploadCompanyLogo.FileName;
                string fpath = Server.MapPath("./Logo/" + fileName);
                filePath = "../Account/Logo/" + fileName;
                fileUploadCompanyLogo.SaveAs(fpath);
            }
        }


        public string checkDuplicate()
        {
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string result = "";
                if (hndCompanyid.Value == string.Empty)
                {
                    result = CompanyClient.checkDuplicateRecord(txtcompname.Text, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtcompname.Text = "";
                    }

                }
                else
                {
                    int id = Convert.ToInt32(hndCompanyid.Value);
                    result = CompanyClient.checkDuplicateRecordEdit(txtcompname.Text, id, profile.DBConnection._constr);
                    if (result != string.Empty)
                    {
                        WebMsgBox.MsgBox.Show(result);
                        txtcompname.Text = "";

                    }
                }
                return result;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Company Setup", "checkDuplicate");
                string result = "";
                return result;
            }
            finally
            {
            }
        }


        // code for department info save

        public void fillCompany(long ID)
        {
            DataTable dt = new DataTable();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = company.GetCompanyName(ID, profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtcompanynamedept.Text = dt.Rows[0]["Name"].ToString();
            }

        }


        protected void btnadd_Click(object sender, System.EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            string active = "", editvar = "", Approreminder = "", autocancerem = ""; ;
            bool gwcdelivery = false, ecommerce = false, pricechange = false;
            long maxdeliveydays = 0;
            try
            {
                long canceldays = 0;
                long companyid = long.Parse(hndCompanyid.Value);
                string deptname = txtdepartment.Text;
                string deptcode = txtdeptcode.Text;
                string autocancel = ddlautocancel.SelectedItem.Text;
                long approval = long.Parse(txtapproval.Text);
                //string OrderFormat = TextBox1.Text +"-"+ TextBox2.Text;
                string OrderFormat = "NULL";
                if (txtdeliverydays.Text != "")
                {
                    maxdeliveydays = long.Parse(txtdeliverydays.Text);
                }
                //long addressType = long.Parse(ddladdressType.SelectedValue);
                long addressType = 64;
                long Finapprover = 0;
                if (hdnapproversearchId.Value != "")
                {
                    Finapprover = long.Parse(hdnapproversearchId.Value);
                }

                if (autocancel != "No")
                {
                    canceldays = long.Parse(txtcanceldays.Text);
                }
                else
                {
                    canceldays = 0;
                }
                bool active1 = OboutRadioButton1.Checked;



                if (active1 == true)
                {
                    active = "Yes";
                }
                else
                {
                    active = "No";
                }
                bool Approreminder1 = false;
                Approreminder1 = chkapproremyes.Checked;
                if (Approreminder1 == true)
                {
                    Approreminder = "Yes";
                }
                else
                {
                    Approreminder = "No";
                }
                if (chkDeliver.Checked == true)
                {
                    gwcdelivery = true;
                }
                if (chkecommerce.Checked == true)
                {
                    ecommerce = true;
                }
                if (chkpricechange.Checked == true)
                {
                    pricechange = true;
                }
                else
                {
                    Finapprover = 0;
                }
                string remschedule = txtremschedule.Text.ToString();
                long approschedays = 0;
                if (remschedule != "")
                {
                    approschedays = long.Parse(remschedule.ToString());
                }


                bool autocancerem1 = chkautocancelreminder.Checked;
                if (autocancerem1 == true)
                {
                    autocancerem = "Yes";
                }
                else
                {
                    autocancerem = "No";
                }
                string autoremsche = txtautoremsche.Text.ToString();
                long autocanceschedays = 0;
                if (autoremsche != "")
                {
                    autocanceschedays = long.Parse(txtautoremsche.Text.ToString());
                }
                string userid = profile.Personal.UserID.ToString();

                if (Session["editstate"] != null)
                {
                    editvar = Session["editstate"].ToString();
                }

                if (editvar == "Edit")
                {
                    long DeptId = long.Parse(hdnSelectedDepartment.Value);
                    if (chkecommerce.Checked == true)
                    {
                        long dupliecommerce = company.chkecommerceduplicate(companyid, DeptId, profile.DBConnection._constr);
                        //if (dupliecommerce <= 0)
                        //{
                        company.UpdateDeptInfo(DeptId, deptname, deptcode, approval, autocancel, canceldays, userid, DateTime.Now, active, Approreminder, approschedays, autocancerem, autocanceschedays, gwcdelivery, ecommerce, OrderFormat, maxdeliveydays, Finapprover, addressType, pricechange, profile.DBConnection._constr);
                        //}
                        //else
                        //{
                        //    WebMsgBox.MsgBox.Show("ECommerce allready assign to department");
                        //}
                    }
                    else
                    {
                        company.UpdateDeptInfo(DeptId, deptname, deptcode, approval, autocancel, canceldays, userid, DateTime.Now, active, Approreminder, approschedays, autocancerem, autocanceschedays, gwcdelivery, ecommerce, OrderFormat, maxdeliveydays, Finapprover, addressType, pricechange, profile.DBConnection._constr);
                    }
                }
                else
                {

                    long resultval = company.chkDeptDuplicate(txtdepartment.Text, txtdeptcode.Text, profile.DBConnection._constr);
                    if (resultval <= 0)
                    {
                        if (chkecommerce.Checked == true)
                        {
                            long deptid = 0;
                            long dupliecommerce = company.chkecommerceduplicate(companyid, deptid, profile.DBConnection._constr);
                            //if (dupliecommerce <= 0)
                            //{
                            company.Savecustdeptinfo(companyid, deptname, 1, deptcode, approval, autocancel, canceldays, userid, DateTime.Now, active, Approreminder, approschedays, autocancerem, autocanceschedays, gwcdelivery, ecommerce, OrderFormat, maxdeliveydays, addressType, pricechange, profile.DBConnection._constr);
                            //}
                            //else
                            //{
                            //    WebMsgBox.MsgBox.Show("ECommerece allready assign to department");
                            //}
                        }
                        else
                        {
                            company.Savecustdeptinfo(companyid, deptname, 1, deptcode, approval, autocancel, canceldays, userid, DateTime.Now, active, Approreminder, approschedays, autocancerem, autocanceschedays, gwcdelivery, ecommerce, OrderFormat, maxdeliveydays, addressType, pricechange, profile.DBConnection._constr);
                        }
                    }
                    else
                    {
                        WebMsgBox.MsgBox.Show("Same Department or Department Code is already present");
                        txtdepartment.Text = "";
                        txtdeptcode.Text = "";
                    }
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "OnPostBack" + sessionID, "setCountry('" + hdnCountry.Value + "','" + hdnState.Value + "');", true);
                long reuslt = long.Parse(hndCompanyid.Value);
                if (hdnmodestate.Value == "Edit")
                {
                    FillUserControl(reuslt);
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Company Setup", "btnadd_Click");
            }
            finally { company.Close(); }


        }


        protected void Grid1_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                ImageButton imgbtn = (ImageButton)sender;
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)Grid1.SelectedRecords[0];
                hdnSelectedDepartment.Value = selectedrec["ID"].ToString();
                hdnSelectedDepartment.Value = imgbtn.ToolTip.ToString();
                Session.Add("editstate", Edit);
                // hndState.Value = "Edit";
                Getpettycash();
                long reuslt = long.Parse(hndCompanyid.Value);
                if (hdnmodestate.Value == "Edit")
                {
                    FillUserControl(reuslt);
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

        protected void imgBtnEditbom_OnClick(object sender, ImageClickEventArgs e)
        {
            try
            {
                chkDeliver.Checked = false;
                ImageButton imgbtn = (ImageButton)sender;
                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)Grid1.SelectedRecords[0];
                hdnSelectedDepartment.Value = selectedrec["ID"].ToString();
                hdnSelectedDepartment.Value = imgbtn.ToolTip.ToString();
                hdnSelectedDepartment.Value = hdnPartSelectedRec.Value;
                imgSearch.Visible = true;
                chkpricechange.Enabled = true;
                Session.Add("editstate", Edit);
                // hndState.Value = "Edit";
                Getpettycash();
                long reuslt = long.Parse(hndCompanyid.Value);
                if (hdnmodestate.Value == "Edit")
                {
                    FillUserControl(reuslt);
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

        public void Getpettycash()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            DataSet ds;
            DataTable dt;
            long ApproverID = 0;
            try
            {
                long deptId = long.Parse(hdnSelectedDepartment.Value);
                Session.Add("DepartmentId", deptId);

                ds = company.GetDepartmentToEdit(deptId, profile.DBConnection._constr);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtdepartment.Text = dt.Rows[0]["Territory"].ToString();
                    txtdeptcode.Text = dt.Rows[0]["StoreCode"].ToString();
                    txtapproval.Text = dt.Rows[0]["ApprovalLevel"].ToString();
                    txtdeliverydays.Text = dt.Rows[0]["MaxDeliveryDays"].ToString();
                    //string OrderFormat = dt.Rows[0]["OrderFormat"].ToString();
                    //string[] format = Regex.Split(OrderFormat, "-");
                    //for (int i = 0; i < format.Length ; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        TextBox1.Text = format[i].ToString();
                    //    }
                    //    else
                    //    {
                    //        TextBox2.Text = format[i].ToString();
                    //    }
                    //}
                    if (dt.Rows[0]["FinApproverID"].ToString() != "")
                    {
                        ApproverID = long.Parse(dt.Rows[0]["FinApproverID"].ToString());
                        hdnapproversearchId.Value = dt.Rows[0]["FinApproverID"].ToString();
                    }
                    txtbomsku.Text = company.getFinApprovername(ApproverID, profile.DBConnection._constr);

                    if (dt.Rows[0]["AutoCancel"].ToString() == "Yes")
                    {
                        ddlautocancel.SelectedValue = "2";
                    }
                    else
                    {
                        // ddlautocancel.SelectedIndex = 1;
                        ddlautocancel.SelectedValue = "3";
                        txtcanceldays.Enabled = false;
                    }

                    txtcanceldays.Text = dt.Rows[0]["cancelDays"].ToString();
                    if (dt.Rows[0]["Active"].ToString() == "Yes")
                    {
                        OboutRadioButton1.Checked = true;
                        OboutRadioButton2.Checked = false;
                    }
                    else
                    {
                        OboutRadioButton1.Checked = false;
                        OboutRadioButton2.Checked = true;
                    }
                    if (dt.Rows[0]["GwcDeliveries"].ToString() == "True")
                    {
                        chkDeliver.Checked = true;
                    }
                    if (dt.Rows[0]["ECommerce"].ToString() == "True")
                    {
                        chkecommerce.Checked = true;
                    }
                    if (dt.Rows[0]["PriceChange"].ToString() == "True")
                    {
                        chkpricechange.Checked = true;
                    }

                    if (dt.Rows[0]["ApprovalRem"].ToString() == "Yes")
                    {
                        chkapproremyes.Checked = true;
                        txtremschedule.Enabled = true;
                    }
                    if (dt.Rows[0]["AutoCancRen"].ToString() == "Yes")
                    {
                        chkautocancelreminder.Checked = true;
                        txtautoremsche.Enabled = true;
                    }
                    txtremschedule.Text = dt.Rows[0]["ApproRemSchedul"].ToString();
                    txtautoremsche.Text = dt.Rows[0]["AutoRemSchedule"].ToString();
                    //AddressType();
                    //ddladdressType.SelectedIndex = ddladdressType.Items.IndexOf(ddladdressType.Items.FindByValue(dt.Rows[0]["AddressType"].ToString()));
                    //ddladdressType.SelectedValue = dt.Rows[0]["AddressType"].ToString();

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "Grid1_Select");
            }
            finally { company.Close(); }
        }


        [WebMethod]
        public static string saveissuedata(object objReq)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            string result = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long companyid = long.Parse(dictionary["companyid"].ToString());
                string deptname = dictionary["deptname"].ToString();
                string deptcode = dictionary["deptcode"].ToString();
                string autocancel = dictionary["autocancel"].ToString();
                long approval = long.Parse(dictionary["approval"].ToString());
                long canceldays = long.Parse(dictionary["canceldays"].ToString());
                string active = dictionary["active"].ToString();
                //  company.Savecustdeptinfo(companyid, deptname, 1, deptcode, approval, autocancel, canceldays, profile.Personal.UserID, DateTime.Now,active, profile.DBConnection._constr);
                result = "success";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Account Master", "saveissuedata");
            }
            finally
            {
                company.Close();
            }
            return result;
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

        #endregion

        #region Code for Cost Center

        protected void gvCostCenter_RebindGrid(object sender, EventArgs e)
        {
            BindCostCenterGrid();
        }

        public void BindCostCenterGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                ds = company.GetCostCenterList(long.Parse(hndCompanyid.Value), profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    gvCostCenter.DataSource = ds.Tables[0];
                    gvCostCenter.DataBind();
                }
                else
                {
                    gvCostCenter.DataSource = null;
                    gvCostCenter.DataBind();

                }
            }
            catch { }
            finally { company.Close(); }
        }

        public void DeleteCostCenterWithZeroQty()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            company.DeleteZeroCompanyIDCostCenter(profile.DBConnection._constr);
        }

        [WebMethod]
        public static string SaveCostCenterData(object objReq)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient company = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            string result = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;

                string CenterName = dictionary["CenterName"].ToString();
                string code = dictionary["code"].ToString();
                long approver = long.Parse(dictionary["approver"].ToString());
                string Remark = dictionary["Remark"].ToString();
                long companyID = 0;
                long Count = company.Duplicatecostcenter(CenterName, code, profile.DBConnection._constr);
                if (Count <= 0)
                {
                    company.SaveCostCenter(CenterName, code, approver, companyID, Remark, DateTime.Now, profile.Personal.UserID, profile.DBConnection._constr);
                    result = "success";
                }
                else
                {
                    result = "fail";
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Account Master", "SaveCostCenter");
            }
            finally
            {
                company.Close();
            }
            return result;
        }

        [WebMethod]
        public static string RemoveSku(object objReq)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;
            long CenterID = long.Parse(dictionary["CenterId"].ToString());
            CompanyClient.RemoveCostCenter(CenterID, profile.DBConnection._constr);
            result = "success";
            return result;
        }

        #endregion

        protected void ActiveTab(string state)
        {

            if (state == "Edit")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                tabAccountInfo.Visible = true;
                tableDeptInfo.Visible = false;
                tabContactInfo.Visible = true;
                tabAddressInfo.Visible = false;
                tabSatutoryInfo.Visible = true;
                tabAttachedDocumentInfo.Visible = true;
                tabAccountHistory.Visible = true;
                tabRateCard.Visible = true;

            }

            else if (state == "Add")
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 1;
                tabAccountInfo.Visible = true;
                tableDeptInfo.Visible = false;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = true;
                tabSatutoryInfo.Visible = true;
                tabSatutoryInfo12.Visible = false;
                tabAttachedDocumentInfo.Visible = true;
                tabAccountHistory.Visible = true;
                tabRateCard.Visible = true;
            }
            else
            {
                TabCustomerList.Visible = true;
                tabAccountMaster.ActiveTabIndex = 2;
                tabAccountInfo.Visible = false;
                tableDeptInfo.Visible = false;
                tabAddressInfo.Visible = false;
                tabContactInfo.Visible = false;
                tabSatutoryInfo.Visible = false;
                tabSatutoryInfo12.Visible = false;
                tabAttachedDocumentInfo.Visible = false;
                tabAccountHistory.Visible = false;
                tabRateCard.Visible = false;
            }
        }
        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                //if (SubscriptStartDate.Date != null || SubscriptEndDate.Date != null)
                //{
                    if (checkDuplicate() == "")
                    {
                        //mCompany objcompany = new mCompany();
                        tCompanyRegistrationDetail objcomRegis = new tCompanyRegistrationDetail();
                        mConfiguration objconfig = new mConfiguration();
                        mCustomer objcompany = new mCustomer(); 

                        if (hdnmodestate.Value == "Edit")
                        {
                            objcompany = CompanyClient.GetCustomerbyID(Convert.ToInt32(hndCompanyid.Value), profile.DBConnection._constr);
                            objcomRegis = CompanyClient.GetCompanyRegisById(Convert.ToInt32(hndCompanyid.Value), profile.DBConnection._constr);
                            objconfig = CompanyClient.GetCompanyConfiguration(Convert.ToInt64(hndCompanyid.Value), profile.DBConnection._constr);
                            hdnlogovalue.Value = objcompany.Logo.ToString();
                        }
                        // if (ddlgroupcompany.SelectedIndex > 0) objcompany.ParentID = Convert.ToInt64(ddlgroupcompany.SelectedValue);
                        objcompany.ParentID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        objcompany.Name = txtcompname.Text;
                        objcompany.Website = txtwebsite.Text;
                        objcompany.AddressLine1 = txtCAddress1.Text;
                        objcompany.AddressLine2 = txtAddress2.Text;
                        objcompany.AddressLine3 = txtAddress3.Text;
                        objcompany.Landmark = txtLandMark.Text;
                        objcompany.County = hdnCountry.Value;
                        objcompany.State = hdnState.Value;
                        objcompany.City = txtCity.Text;
                        objcompany.Zipcode = txtZipCode.Text;
                        objcompany.PhoneNo = txtphoneno.Text;
                        objcompany.FaxNo = txtFax.Text;
                        objcompany.EmailID = txtemailid.Text;
                        objcompany.Active = "Y";
                        objcompany.OrderFormat = TextBox1.Text + "-" + TextBox2.Text;
                        if (rbtnActiveNo.Checked == true) objcompany.Active = "N";
                        saveimage();
                        LogoInBytes();
                        objcompany.LogoPath = filePath;
                        if (hdnmodestate.Value == "Edit")
                        {
                            objcompany.Logo = objcompany.Logo;
                        }
                        else
                        {
                            objcompany.Logo = (byte[])Session["ProfileImg"];
                        }
                        // objcompany.Logo = bytes;
                        objcompany.StampImg = (byte[])Session["ProfileImgStamp"];

                        // Parameteres for configuration
                        char WMS = 'N', OMS = 'N', Delivery = 'N', pl3 = 'N';
                        objconfig.Object = "Customer";
                        objconfig.SubscriType = long.Parse(hdnsubscription.Value);
                        objconfig.StartDate = SubscriptStartDate.Date;
                        objconfig.EndDate = SubscriptEndDate.Date;
                        if (txtnowarehouse.Text == "")
                        {
                            txtnowarehouse.Text = "1";
                        }
                        objconfig.NoOfWarehouse = long.Parse(txtnowarehouse.Text);

                        foreach (ListItem listitem in chkbusiness.Items)
                        {
                            if (listitem.Text == "WMS" && listitem.Selected)
                            {
                                WMS = 'Y';
                            }
                            else if (listitem.Text == "OMS" && listitem.Selected)
                            {
                                OMS = 'Y';
                            }
                            else if (listitem.Text == "Delivery" && listitem.Selected)
                            {
                                Delivery = 'Y';
                            }
                            else if (listitem.Text == "3PL" && listitem.Selected)
                            {
                                pl3 = 'Y';
                            }
                        }
                        objconfig.WMS = WMS.ToString();
                        objconfig.OMS = OMS.ToString();
                        objconfig.Delivery = Delivery.ToString();
                        objconfig.pl3 = pl3.ToString();
                        objconfig.CompanyID = long.Parse(ddlcompanymain.SelectedItem.Value);
                        objconfig.Createdby = profile.Personal.UserID;


                        if (hdnmodestate.Value == "Edit")
                        {
                            objcompany.LastModifiedBy = profile.Personal.UserID.ToString();
                            objcompany.LastModifiedDate = DateTime.Today;
                            int result = CompanyClient.UpdateCustomer(objcompany, profile.DBConnection._constr);
                            int config = CompanyClient.UpdateCompanyCongig(objconfig, profile.DBConnection._constr);
                            //  int resultregis = CompanyClient.UpdateCompanyRegistration(objcomRegis, profile.DBConnection._constr);
                            if (result == 1)
                            {
                                UC_StatutoryDetails1.FinalSaveToStatutoryDetails(objcompany.ID, "Customer", profile.Personal.CompanyID);
                                UC_AttachDocument1.FinalSaveDocument(objcompany.ID);
                                UCContactPerson1.FinalSaveContactPerson("Customer", objcompany.ID);
                                // WebMsgBox.MsgBox.Show("Record Updated successfully");
                            }
                        }
                        else
                        {
                            objcompany.CreatedBy = profile.Personal.UserID.ToString();
                            objcompany.CreationDate = DateTime.Today;
                            objcomRegis.CreatedBy = profile.Personal.UserID.ToString();
                            objcomRegis.CreationDate = DateTime.Now;
                             bool chkDuplicate_aspnetmember = false;

                            if (hdnmodestate.Value != "Edit")
                            {
                                // if (Membership.GetUser(txtAdminUserName.Text.Trim()) != null) { chkDuplicate_aspnetmember = true; }
                            }

                            if (chkDuplicate_aspnetmember == true)
                            {
                                // txtAdminUserName.Text = "";
                                MsgBox.Show("Login name already exist");
                            }

                            else if (chkDuplicate_aspnetmember == false)
                            {
                                /* Insert Comapny record into elegant db */
                               // ID = CompanyClient.InsertmCompany(objcompany, profile.DBConnection._constr);
                                ID = CompanyClient.InsertCustomer(objcompany, profile.DBConnection._constr);
                                objcomRegis.CompanyID = ID;
                                hndCompanyid.Value = ID.ToString();

                                if (ID != 0)
                                {
                                    UC_StatutoryDetails1.FinalSaveToStatutoryDetails(ID, "Customer", profile.Personal.CompanyID);
                                    UC_AttachDocument1.FinalSaveDocument(ID);
                                    UCContactPerson1.FinalSaveContactPerson("Customer", ID);
                                    UCContactPerson1.ClearContactPerson("Customer");
                                    UC_AttachDocument1.ClearDocument("Customer");
                                    //fileupload(ID);
                                }

                                /* Insert Comapny record into new company record */
                                string[] _constr = new string[4];
                                /*Save into New Company DB*/
                                objcompany.ID = ID;
                                // objcompany.LogoPath = FileName;
                               // CompanyClient.InsertmCompany(objcompany, _constr);
                                objconfig.ReferenceID = ID;
                                CompanyClient.InsertConfiguration(objconfig, profile.DBConnection._constr);

                                //WebMsgBox.MsgBox.Show("Record saved successfully");
                            }

                        }
                        //FillCompany();
                        Page.ClientScript.RegisterStartupScript(GetType(), "OnPostBack" + sessionID, "setCountry('" + hdnCountry.Value + "','" + hdnState.Value + "');", true);
                        hndState.Value = "Edit";
                        hdnmodestate.Value = "Edit";
                        hndCompanyid.Value = ID.ToString();
                        Session.Add("CompanyID", hndCompanyid.Value);
                        // UC_StatutoryDetails1.BindGridStatutoryDetails(Convert.ToInt64(hndCompanyid.Value), "Company", profile.Personal.CompanyID);
                        //AddressType();
                        clr();
                        MainCustomerGridBind();
                        ActiveTab("List");
                        Response.Redirect("Customer.aspx");

                    }
                    else
                    {
                        WebMsgBox.MsgBox.Show("Same Customer Name Already Exist");
                    }
                //}
                //else
                //{
                //    WebMsgBox.MsgBox.Show("Please Select Subscription Start Date & End Date");
                //}
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "Save");
            }
            finally
            {
                CompanyClient.Close();
            }
        }
        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
            FillCompany();
            this.UCToolbar1.ToolbarAccess("AddNew");
            HdnAccountId.Value = null;
            btncustomernext.Visible = false;
            hdnmodestate.Value = "AddNew";
            ActiveTab("Add"); //UC_AttachDocument1.FillDocumentByObjectNameReferenceID(0, "Account", "Account");
            ResetUserControl();
            FillDropDownSubscription();
            Session.Add("PORRequestID", "Company");

        }
        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clr();
        }
        public void clr()
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                // ddlgroupcompany.SelectedIndex = -1;
                txtcompname.Text = "";
                txtwebsite.Text = "";
                txtCAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtLandMark.Text = "";
                ddlCountry.SelectedIndex = -1;
                ddlState.SelectedIndex = -1;
                hndCompanyid.Value = "0";
                hndCompanyid.Value = "0";
                txtCity.Text = "";
                txtZipCode.Text = "";
                txtphoneno.Text = "";
                txtFax.Text = "";
                hndState.Value = "";
                txtemailid.Text = "";
                ResetUserControl();
                hdnState.Value = "";
                hdnCountry.Value = "";
                Session["ProfileImg"] = null;
                Session["ProfileImgStamp"] = null;
                HdnAccountId.Value = null;
                chkapproremyes.Checked = false;
                chkautocancelreminder.Checked = false;
                txtautoremsche.Text = "";
                txtremschedule.Text = "";
                Grid1.DataSource = null;
                Grid1.DataBind();
                txtcompanynamedept.Text = "";
                txtdepartment.Text = "";
                txtdeptcode.Text = "";
                chkecommerce.Checked = false;
                chkDeliver.Checked = false;
                chkpricechange.Checked = false;
                TextBox1.Text = "";
                TextBox2.Text = "";
                hdnlogovalue.Value = "";
                hdnsubscription.Value = "";
                txtnowarehouse.Text = "";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "clr");
            }
            finally
            {
            }
        }

        public void LogoInBytes()
        {
            try
            {

                if (fileUploadCompanyLogo.PostedFile != null)
                {
                    string[] filetype;
                    filetype = new string[2];
                    filetype = fileUploadCompanyLogo.PostedFile.ContentType.ToString().Split('/');
                    string path = Server.MapPath("Logo/" + Session.SessionID.ToString() + "." + filetype[1]);
                    if (filetype[1] == "jpeg" || filetype[1] == "jpg" || filetype[1] == "png" || filetype[1] == "bmp" || filetype[1] == "gif")
                    {
                        fileUploadCompanyLogo.PostedFile.SaveAs(path);
                        if (File.Exists(path)) File.Delete(path);
                        fileUploadCompanyLogo.PostedFile.SaveAs(path);
                        // imgComapnyLogo.Src = "~/Company/TempLogo/" + Session.SessionID.ToString() + "." + filetype[1];
                        //imgComapnyLogo.Visible = true;
                        hdnFilePath.Value = path;
                        hdnFileTye.Value = filetype[1];
                    }
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "OnPostBack" + sessionID, "setCountry('" + hdnCountry.Value + "','" + hdnState.Value + "');", true);
                Session["ProfileImg"] = fileUploadCompanyLogo.FileBytes;
                // imgComapnyLogo.Src = "../Image.aspx";
            }
            catch (Exception ex) { }
        }

        protected void FillDropDownSubscription()
        {

            ddlsubscription.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlsubscription.DataSource = CompanyClient.GetContactTypeList(profile.DBConnection._constr);
            ddlsubscription.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlsubscription.Items.Insert(0, lst);
        }

        protected void FillCompany()
        {
            ddlcompanymain.Items.Clear();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcompanymain.DataSource = CompanyClient.GetCompanyDropDown(profile.Personal.CompanyID,profile.DBConnection._constr);
            ddlcompanymain.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcompanymain.Items.Insert(0, lst);
        }


        private void GetCompanyConfiguration()
        {
            CustomProfile profile = CustomProfile.GetProfile();

            mConfiguration AddList = new mConfiguration();
            iCompanySetupClient CompanyClient = new iCompanySetupClient();
            AddList = CompanyClient.GetCompanyConfiguration(Convert.ToInt64(hndCompanyid.Value), profile.DBConnection._constr);
            FillDropDownSubscription();
            ddlsubscription.SelectedIndex = ddlsubscription.Items.IndexOf(ddlsubscription.Items.FindByValue(AddList.SubscriType.ToString()));
            hdnsubscription.Value = AddList.SubscriType.ToString();
            SubscriptStartDate.Date = AddList.StartDate;
            SubscriptEndDate.Date = AddList.EndDate;
            txtnowarehouse.Text = AddList.NoOfWarehouse.ToString();
            foreach (ListItem listitem in chkbusiness.Items)
            {
                if (AddList.WMS.ToString().Trim() == "Y" && listitem.Text == "WMS")
                {
                    listitem.Selected = true;
                }
                else if (AddList.OMS.ToString().Trim() == "Y" && listitem.Text == "OMS")
                {
                    listitem.Selected = true;
                }
                else if (AddList.Delivery.ToString().Trim() == "Y" && listitem.Text == "Delivery")
                {
                    listitem.Selected = true;
                }
                else if (AddList.pl3.ToString().Trim() == "Y" && listitem.Text == "3PL")
                {
                    listitem.Selected = true;
                }
            }

        }

        protected void GvCustomer_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {
                btncustomernext.Visible = false;
                // btnnextdept.Visible = false;
                this.UCToolbar1.ToolbarAccess("Edit");

                CustomProfile profile = CustomProfile.GetProfile();
                Hashtable selectedrec = (Hashtable)GvCustomer.SelectedRecords[0];
                hndCompanyid.Value = selectedrec["ID"].ToString();
                long reuslt = long.Parse(hndCompanyid.Value);
                hndState.Value = "Edit";
                hdnmodestate.Value = "Edit";
                GetCustomerByID();
                fillCompany(reuslt);
                GetCompanyConfiguration();
                FillUserControl(reuslt);
                RateCardGridBind(reuslt);
                Session.Add("CompanyID", hndCompanyid.Value);
                Session.Add("PORRequestID", "Company");
                //Session.Add("DepartmentID", hdnSelectedDepartment.Value);
               // UC_StatutoryDetails1.BindGridStatutoryDetails(Convert.ToInt64(hndCompanyid.Value), "Company", profile.Personal.CompanyID);
                ActiveTab("Edit");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "GvCustomer_Select");
            }
            finally
            {
            }
        }
        private void GetCustomerByID()
        {
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                Session["ProfileImg"] = null;
                Session["ProfileImgStamp"] = null;
                CustomProfile profile = CustomProfile.GetProfile();
                mCustomer AddList = new mCustomer();
                tCompanyRegistrationDetail company = new tCompanyRegistrationDetail();
                AddList = CompanyClient.GetCustomerbyID(Convert.ToInt64(hndCompanyid.Value), profile.DBConnection._constr);
                // ddlgroupcompany.SelectedValue = Convert.ToInt64(AddList.ParentID).ToString();
                FillCompany();
                ddlcompanymain.SelectedIndex = ddlcompanymain.Items.IndexOf(ddlcompanymain.Items.FindByValue(AddList.ParentID.ToString()));
                txtcompname.Text = AddList.Name;
                txtwebsite.Text = AddList.Website;
                txtCAddress1.Text = AddList.AddressLine1;
                txtAddress2.Text = AddList.AddressLine2;
                txtAddress3.Text = AddList.AddressLine3;
                hdnCountry.Value = AddList.County;
                // ddlCountry.SelectedItem.Text = hdnCountry.Value.ToString();
                //ddlCountry.SelectedItem.Text = AddList.County;
                hdnState.Value = AddList.State;
                //ddlState.SelectedItem.Text = hdnState.Value.ToString();
                //ddlState.SelectedItem.Text = AddList.State;
                Page.ClientScript.RegisterStartupScript(GetType(), "fillCountry" + sessionID, "setCountry('" + AddList.County + "','" + AddList.State + "');", true);
                txtCity.Text = AddList.City;
                txtLandMark.Text = AddList.Landmark;
                txtemailid.Text = AddList.EmailID;
                txtZipCode.Text = AddList.Zipcode;
                txtphoneno.Text = AddList.PhoneNo;
                txtFax.Text = AddList.FaxNo;
                if (AddList.Active == "Y")
                {
                    rbtnActiveYes.Checked = true;
                    rbtnActiveNo.Checked = false;
                }
                else
                {
                    rbtnActiveYes.Checked = false;
                    rbtnActiveNo.Checked = true;
                }
                hdnLogo.Value = null;

                //  for order format To show after open company in edit mode/////////////

                //string OrderFormat = dt.Rows[0]["OrderFormat"].ToString();
                string OrderFormat = AddList.OrderFormat;
                string[] format = Regex.Split(OrderFormat, "-");
                for (int i = 0; i < format.Length; i++)
                {
                    if (i == 0)
                    {
                        TextBox1.Text = format[i].ToString();
                    }
                    else
                    {
                        TextBox2.Text = format[i].ToString();
                    }
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Customer Master", "GetCompanyByID");
            }
            finally
            {
                CompanyClient.Close();
            }
        }

        protected void FillUserControl(long resultId)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               // UCAddress1.FillAddressByObjectNameReferenceID("Account", resultId, "");
               // UCContactPerson1.FillContactPersonByObjectNameReferenceID("Contact", resultId, "");
                UCContactPerson1.FillContactPersonByObjectNameReferenceID("Customer", resultId, "Customer");
                UC_StatutoryDetails1.BindGridStatutoryDetails(resultId, "Customer", profile.Personal.CompanyID);
                UC_AttachDocument1.FillDocumentByObjectNameReferenceID(resultId, "Customer", "Customer");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Customer Master", "FillUserControl");
            }
            finally { }
        }

        protected void RateCardGridBind( long CustomerID)
        {
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //GvCustomer.DataSource = ServiceAccountMaster.GetGetCustomerDetail(profile.Personal.UserID, profile.Personal.UserType.ToString(), profile.DBConnection._constr);
                grdratecard.DataSource = CompanyClient.GetCustomerRateCard(CustomerID, "Customer", profile.DBConnection._constr);
                grdratecard.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Customer Master", "RateCardGridBind");
            }
            finally
            {
                CompanyClient.Close();
            }
        }


        protected void MainCustomerGridBind()
        {
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                //GvCustomer.DataSource = ServiceAccountMaster.GetGetCustomerDetail(profile.Personal.UserID, profile.Personal.UserType.ToString(), profile.DBConnection._constr);
                if (profile.Personal.UserType == "SuperAdmin")
                {
                    GvCustomer.DataSource = CompanyClient.GetSuperAdminCustomerList(profile.DBConnection._constr);
                }
                else
                {
                    GvCustomer.DataSource = CompanyClient.GetCustomerList(profile.Personal.CompanyID, profile.DBConnection._constr);
                }
                GvCustomer.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Customer Master", "MainCustomerGridBind");
            }
            finally
            {
                CompanyClient.Close();
            }
        }

        protected void GvCustomer_OnRebind(Object sender, EventArgs e)
        {
            MainCustomerGridBind();
        }

        protected void pageImport(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                // Response.Redirect("../Import/Import.aspx?Objectname=" + "Customer");
                Response.Redirect("../Account/StudImport.aspx", false);
            }
            catch (System.Exception ex)
            {
                // Login.Profile.ErrorHandling(ex, this, "Account Master", "pageImport");
            }
            finally
            {
            }
        }
        [WebMethod]
        public static string PMDeleteDocument(string Sequence)
        {
            iUC_AttachDocumentClient DocumentClient = new iUC_AttachDocumentClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DocumentClient.DeleteDocumentFormTemp(Convert.ToInt64(Sequence), sessionID, profile.Personal.UserID.ToString(), "Account", profile.DBConnection._constr);
                return "true";
            }
            catch (Exception ex) { return "false"; }
            finally { DocumentClient.Close(); }
        }
       
         
        [WebMethod]
        public static void DeleteFA(long deptid)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.AccountSearchService.iCustomerClient Cclient = new BrilliantWMS.AccountSearchService.iCustomerClient();
            Cclient.DeleteFanancialApprover(deptid, profile.DBConnection._constr);

        }
    }
}