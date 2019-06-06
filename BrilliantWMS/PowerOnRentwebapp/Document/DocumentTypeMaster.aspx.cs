using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.DocumentService;
using Obout.Interface;
using System.Collections;
using BrilliantWMS.Login;
using WebMsgBox;
using BrilliantWMS.CompanySetupService;
using BrilliantWMS.StatutoryService;
using BrilliantWMS.DepartmentService;
using System.Data;
using System.Resources;
using System.Web.Services;

namespace BrilliantWMS.Document
{
    public partial class DocumentTypeMaster : System.Web.UI.Page
    {
        BrilliantWMS.DepartmentService.iDepartmentMasterClient DepartmentClient = new BrilliantWMS.DepartmentService.iDepartmentMasterClient();
        BrilliantWMS.DesignationService.iDesignationMasterClient DesignationClient = new BrilliantWMS.DesignationService.iDesignationMasterClient();
        PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            UCFormHeader1.FormHeaderText = "Document Master";
            if (!IsPostBack)
            {
                FillCompany();
                BindGrid();
                hdnDesignationID.Value = null;
                if (profile.Personal.CompanyID == 14)
                {
                    Button btnExport = (Button)UCToolbar1.FindControl("btnExport");
                    btnExport.Visible = false;
                    Button btnImport = (Button)UCToolbar1.FindControl("btnImport");
                    btnImport.Visible = false;
                    Button btmMail = (Button)UCToolbar1.FindControl("btmMail");
                    btmMail.Visible = false;
                    Button btnPrint = (Button)UCToolbar1.FindControl("btnPrint");
                    btnPrint.Visible = false;
                }
            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        public void BindGrid()
        {
            iDepartmentMasterClient document = new iDepartmentMasterClient();
            try
            {
                long UserID = 540;
                CustomProfile profile = CustomProfile.GetProfile();
                grddocument.DataSource = document.GetDocumentTypeList(profile.DBConnection._constr);
                grddocument.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "BindGrid");
            }
            finally
            {
            }
        }

      

        public void clear()
        {
            try
            {
                txtdoctype.Text = "";
                txtDescription.Text = "";
                txtSequence.Text = "";
                ddlcompany.SelectedIndex = 0;
                ddlcustomer.SelectedIndex = -1;
                hdndocumenttypeID.Value = "";
                rbtnYes.Checked = true;
              }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Designation Master", "BinddlDepartment");
            }
            finally
            {
            }
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUC_AttachDocumentClient Document = new iUC_AttachDocumentClient();
            mDocumentType DocType = new mDocumentType();
            if (hdnstate.Value == "Edit")
            {
                DocType = Document.GetDocumentTypebyID(long.Parse(hdndocumenttypeID.Value), profile.DBConnection._constr);
            }
            DocType.DocumentType = txtdoctype.Text;
            DocType.Sequence = int.Parse(txtSequence.Text);
            DocType.Description = txtDescription.Text;
            DocType.CompanyID = long.Parse(ddlcompany.SelectedItem.Value);
            DocType.CustomerID = long.Parse(hdncustomerid.Value);
            DocType.CreatedBy = profile.Personal.UserID;
            DocType.CreationDate = DateTime.Now;
            DocType.Active = "Yes";
            if (rbtnNo.Checked == true) DocType.Active = "No";
            long DocTypeID = Document.SaveDocumentType(DocType, profile.DBConnection._constr);
            if (hdnstate.Value == "Edit")
            {
                WebMsgBox.MsgBox.Show("Record Updated Succefully!");
            }
            else
            {
                 WebMsgBox.MsgBox.Show("Record Save Succefully!");
            }
            clear();
            BindGrid();

        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        { clear(); }

        public string checkDuplicate()
        {
            string result = "";
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    

            //    if (hdnDesignationID.Value == string.Empty)
            //    {
            //        result = DesignationClient.checkDuplicateRecord(txtDesignation.Text.Trim(), Convert.ToInt32(ddlDepartment.SelectedValue), profile.DBConnection._constr);
            //        if (result != string.Empty)
            //        {
            //            WebMsgBox.MsgBox.Show(result);
            //            txtDesignation.Text = "";
            //        }
            //        txtSequence.Focus();
            //    }
            //    else
            //    {
            //        result = DesignationClient.checkDuplicateRecordEdit(Convert.ToInt32(hdnDesignationID.Value), txtDesignation.Text.Trim(), Convert.ToInt32(ddlDepartment.SelectedValue), profile.DBConnection._constr);
            //        if (result != string.Empty)
            //        {
            //            WebMsgBox.MsgBox.Show(result);
            //            txtDesignation.Text = "";
            //        }
            //    }
            ////    return result;
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, this, "Designation Master", "checkDuplicate");
            //    string result = "";
            //    return result;
            //}
            //finally
            //{
            //}
            return result;
        }

        protected void grddocument_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            try
            {

                rbtnNo.Checked = false;
                rbtnYes.Checked = false;
                Hashtable selectedrec = (Hashtable)grddocument.SelectedRecords[0];
                hdndocumenttypeID.Value = selectedrec["ID"].ToString();
                long result = long.Parse(hdndocumenttypeID.Value);
                GetDocumentTypeByID(result);
                hdnstate.Value = "Edit";
                 
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Document Master", "grddocument_Select");
            }
            finally
            {
            }
        }

        protected void GetDocumentTypeByID(long DocTypeID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUC_AttachDocumentClient Document = new iUC_AttachDocumentClient();
            mDocumentType DocType = new mDocumentType();
            DocType = Document.GetDocumentTypebyID(long.Parse(hdndocumenttypeID.Value), profile.DBConnection._constr);
            FillCompany();
            if (DocType.CompanyID != null) ddlcompany.SelectedIndex = ddlcompany.Items.IndexOf(ddlcompany.Items.FindByValue(DocType.CompanyID.ToString()));
            hdnCompanyid.Value = DocType.CompanyID.ToString();
            getCustomer(long.Parse(DocType.CompanyID.ToString()));
            if (DocType.CustomerID != null) ddlcustomer.SelectedIndex = ddlcustomer.Items.IndexOf(ddlcustomer.Items.FindByValue(DocType.CustomerID.ToString()));
            hdncustomerid.Value = DocType.CustomerID.ToString();
            if (DocType.DocumentType != null) txtdoctype.Text = DocType.DocumentType.ToString();
            if (DocType.Description != null) txtDescription.Text = DocType.Description.ToString();
            if (DocType.Sequence != null) txtSequence.Text = DocType.Sequence.ToString();
            if (DocType.Active == "No")
            {
                rbtnNo.Checked = true;
            }
            else
            {
                rbtnYes.Checked = true;
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
            ddlcustomer.Items.Clear();
            iStatutoryMasterClient StatutoryClient = new iStatutoryMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcustomer.DataSource = StatutoryClient.GetCustomerList(CompanyID, profile.DBConnection._constr);
            ddlcustomer.DataTextField = "Name";
            ddlcustomer.DataValueField = "ID";
            ddlcustomer.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcustomer.Items.Insert(0, lst);
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
    }
}