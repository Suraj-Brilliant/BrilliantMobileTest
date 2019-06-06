using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using BrilliantWMS.Login;
using BrilliantWMS.ImportService;

namespace BrilliantWMS.CommonControls
{
    public partial class UCImport : System.Web.UI.UserControl
    {
        string Validate = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlImport.Visible = true;
            //Up_PnlGirdProduct.Visible = true;
            btnimpnext.Enabled = false;
            btnimpnext.CssClass = "class1";
           // btnimportNext.Enabled = false;
           // btnimportNext.CssClass = "class1";
            uploadMessage.Visible = false;
            lblmessagesuccess.Visible = false;
            if (!IsPostBack)
            {
                string Object = "";
                Object = Session["Object"].ToString();
                hdnobject.Value = Object;
                GetImportheadData(Object);
            }
        }

        public void GetImportheadData(string Object)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            long CompanyID = 0;
            long UserID = 0;
            CompanyID = profile.Personal.CompanyID;
            UserID = profile.Personal.UserID;
            DataSet ds = Import.GetTemplateDataByObject(Object, UserID);
            DataTable dt = ds.Tables[0];
            if(dt.Rows.Count > 0)
            {
                lbltext1.Text = ds.Tables[0].Rows[0]["Instruction"].ToString();
                downloadlink.HRef = ds.Tables[0].Rows[0]["Template"].ToString();
            }
            ds.Clear();
        }

        protected void btnUploadPo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ImportData = new DataSet();
            string connString = "";
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            Import.DeleteFromtempTable(hdnobject.Value, profile.Personal.UserID);
            Import.Close();
            string path = FileuploadPO.PostedFile.FileName;
            string strFileType = System.IO.Path.GetExtension(path).ToString().ToLower();

            string Fullpath = Server.MapPath("../CommonControls/ImportFiles/" + path);
            // string Fullpath = "https://testoms.gwclogistics.com/GWCTestVersion2/PowerOnRent/OrderImport/1DirectOrderImportTest.xlsx";

            FileuploadPO.PostedFile.SaveAs(Server.MapPath("../CommonControls/ImportFiles/" + path));

            if (strFileType.Trim() == ".xls")
            {

                connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Fullpath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (strFileType.Trim() == ".xlsx")
            {
                connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Fullpath + ";Extended Properties='Excel 8.0;HDR=Yes'";
            }
            OleDbConnection excelConnection = new OleDbConnection(connString);
            OleDbCommand cmd1 = new OleDbCommand("Select * from [Sheet1$]", excelConnection);
            OleDbDataAdapter oda = new OleDbDataAdapter();
            excelConnection.Open();
            oda.SelectCommand = cmd1;
            oda.Fill(ImportData);
            Session.Add("ImportValues", ImportData);
            excelConnection.Close();
            btnimpnext.Enabled = true;
            btnimpnext.CssClass = "class2";
            //btnimportNext.Enabled = true;
            //btnimportNext.CssClass = "class2";
            uploadMessage.Visible = true;
            lblmessagesuccess.Visible = true;
        }
        protected void btnimportNext_Click(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            DataSet ds = new DataSet();
            DataSet Importvalue = new DataSet();
            Importvalue = (DataSet)Session["ImportValues"];
             
            ds = Import.ValidateImportData(hdnobject.Value, profile.Personal.UserID, profile.Personal.CompanyID, Importvalue);
            GVImportView.DataSource = ds;
            GVImportView.DataBind();
            if (Validate != "")
            {
                lblbackMessage.Text = "Colored Row Data is not valid, Please Check Remark and Click on Back button.";
                btnnext.Enabled = false;
                btnnext.CssClass = "class1";
            }
            else
            {
                lblOkMessage.Text = "All data are verified.Please click on Next Button ";
                btnnext.Enabled = true;
                lblbackMessage.Visible = false;
                btnnext.CssClass = "class2";
            }
           
           // Up_PnlGirdProduct.Visible = false;
           // UpdateGirdProductProcess.Visible = false;
            Import.Close();
            pnlImport.Visible = false;
            pnlvalidate.Visible = true;
        }
        protected void btnimportcancel_Click(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            Import.DeleteFromtempTable(hdnobject.Value, profile.Personal.UserID);
            Import.Close();
            
        }
        protected void btnnext_Click(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            DataSet DSInsert = new DataSet();
            pnlImport.Visible = false;
            pnlvalidate.Visible = false;
            pnlfinish.Visible = true;
            DSInsert = (DataSet)Session["ImportValues"];
            long Result = Import.InsertImportLocation(hdnobject.Value, profile.Personal.UserID, profile.Personal.CompanyID, DSInsert);
            Import.DeleteFromtempTable(hdnobject.Value, profile.Personal.UserID);
            Import.Close();
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            ImportServiceClient Import = new ImportServiceClient();
            Import.DeleteFromtempTable(hdnobject.Value, profile.Personal.UserID);
            pnlvalidate.Visible = false;
            pnlImport.Visible = true;
            btnnext.Enabled = true;
            Import.Close();

        }
        protected void GVImportView_OnRowDataBound(object sender, Obout.Grid.GridRowEventArgs e)
        {
          //  if (DataBinder.Eval(e.Row.DataItem, "Remark") != null)
            string Remark = e.Row.Cells[GVImportView.Columns["Remark"].Index].Text;

            if (Remark != "")
            {
                e.Row.BackColor = System.Drawing.Color.Cyan;
                Validate = "False";
            }
        }

        protected void btnfinish_Click(object sender, EventArgs e)
        {
            pnlfinish.Visible = false;
            pnlvalidate.Visible = false;
            pnlImport.Visible = true;
        }

    }
}