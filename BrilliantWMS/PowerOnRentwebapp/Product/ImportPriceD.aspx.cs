using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using System.Web.Services;
using System.Configuration;
using System.IO;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Drawing;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServiceUCCommonFilter;


namespace BrilliantWMS.Product
{
    public partial class ImportPriceD : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        SqlConnection con1 = new SqlConnection("");
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            if (!IsPostBack)
            {
                GetCompany();
            }
            lblmessagesuccess.Visible = false;
            Button12.Enabled = false;
            Button12.CssClass = "class1";   
        }

        public void GetCompany()
        {
            DataSet ds;
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            try
            {

                //ds = productClient.GetCompanyname(profile.DBConnection._constr);
                List<mCompany> CompanyLst = new List<mCompany>();
                long UID = profile.Personal.UserID;
                string UserType = profile.Personal.UserType.ToString();
                if (UserType == "Admin")
                {
                    CompanyLst = objService.GetUserCompanyNameNEW(UID, profile.DBConnection._constr).ToList();
                }
                else
                {
                    CompanyLst = objService.GetCompanyName(profile.DBConnection._constr).ToList();
                }
                //ddlcompany.DataSource = ds;
                ddlcompany.DataSource = CompanyLst;
                ddlcompany.DataTextField = "Name";
                ddlcompany.DataValueField = "ID";
                ddlcompany.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlcompany.Items.Insert(0, lst);
            }
            catch { }
            finally { productClient.Close(); }
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

                /*Add By Suresh For Selected Department List Show */

                iUCCommonFilterClient UCCommonFilter = new iUCCommonFilterClient();

                //SiteLst = UCCommonFilter.GetAddedDepartmentList(int.Parse(ddlcompanyId.ToString()), profile.Personal.UserID, profile.DBConnection._constr).ToList();
                /* Add By Suresh For Selected Department List Show */

                if (profile.Personal.UserType == "Admin")
                {
                    ds = UCCommonFilter.GetAddedDepartmentListDS(int.Parse(ddlcompanyId.ToString()), profile.Personal.UserID, profile.DBConnection._constr);
                }
                else
                {
                    ds = productClient.GetDepartment(ddlcompanyId, profile.DBConnection._constr);
                }

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
            catch
            {
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

        protected void btnUploadPo_Click(object sender, EventArgs e)
        {
            con1.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            Label1.Text = "";
            try
            {
                if (ddlcompany.SelectedValue != "0" && ddldepartment.SelectedValue != "0")
                {
                    productClient.DeleteSKUPricetemp(profile.DBConnection._constr);

                    string connString = "";
                    string path = FileuploadPO.PostedFile.FileName;
                    string strFileType = System.IO.Path.GetExtension(path).ToString().ToLower();
                    
                    string Fullpath = Server.MapPath("~/Product/ImportPrice/" + path);
                   
                    //if (strFileType.Trim() != ".xls")
                    //{
                    //    Label1.Text = "Please upload only Excel files with .xls extention!";
                    //}
                    //else
                    //{
                    Label1.Text = "";
                    FileuploadPO.PostedFile.SaveAs(Server.MapPath("~/Product/ImportPrice/" + path));
                   
                    if (strFileType.Trim() == ".xls")
                    {
                        connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Fullpath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Fullpath + ";Extended Properties='Excel 12.0;HDR=Yes'";
                    }
                    OleDbConnection excelConnection = new OleDbConnection(connString);
                    OleDbCommand cmd1 = new OleDbCommand("Select * from [Template$]", excelConnection);
                   
                    excelConnection.Open();
                    OleDbDataReader dReader;
                    dReader = cmd1.ExecuteReader();
                    SqlBulkCopy sqlBulk = new SqlBulkCopy(con1);
                    con1.Open();
                    sqlBulk.DestinationTableName = "TempPrice";
                    sqlBulk.WriteToServer(dReader);
                    excelConnection.Close();
                    
                    ds = productClient.GetSkuPriceTemp(profile.DBConnection._constr);
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        //System.Threading.Thread.Sleep(8000);
                        Label1.Text = "Upload Successful !!!";
                        lblmessagesuccess.Visible = true;
                        Button12.Enabled = true;
                        Button12.CssClass = "class2";
                        // adm.UpdateStudenttrim();
                    }
                    else
                    {
                        WebMsgBox.MsgBox.Show("Upload Not Successful, Please Upload Right Template");
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Upload Not Successful,Please upload Right template!');", true);
                    }
                    //adm.UpdateStudenttrim();
                    //}
                }
                else
                {
                    WebMsgBox.MsgBox.Show("Please Select Company and Department");
                 
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "showAlert('" + ex.ToString() + "','Error','#');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Exception in Catch!');", true);
            }
            finally { con1.Close(); productClient.Close(); }
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            if (ddlcompany.SelectedValue != "0" && ddldepartment.SelectedValue != "0")
            {
                Session.Add("CompanyIdPI", long.Parse(hdncompanyid.Value));
                Session.Add("DepartmentIDPI", long.Parse(hdndeptid.Value));
                Response.Redirect("ImportPriceV.aspx");
            }
            else
            {
                WebMsgBox.MsgBox.Show("Please Select Company and Department");
                //Label1.Text = "Upload Not Successful,Please upload Right template!";
            }
        }

        protected void Btncancel_Click(object sender, EventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            productClient.DeleteSKUPricetemp(profile.DBConnection._constr);
            productClient.Close();
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblstep1.Text = rm.GetString("UploadFile", ci);
                lblstep2.Text = rm.GetString("DataValidationVerification", ci);
                lblstep3.Text = rm.GetString("Finished", ci);
                lblHeading.Text = rm.GetString("ImportPrice", ci);
                lbltext1.Text = rm.GetString("ImportOrder", ci);
                lblcompany.Text = rm.GetString("company", ci);
                lblDept.Text = rm.GetString("Department", ci);
                lblSelecFile.Text = rm.GetString("SelectImportFile", ci);
                Button12.Text = rm.GetString("Next", ci);
                Btncancel.Text = rm.GetString("Cancel", ci);
                btnUploadPo.Text = rm.GetString("Upload", ci);
                Lbl.Text = rm.GetString("DownloadImportOrder",ci);
                
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SLA", "loadstring");
            }
        }
    }
}