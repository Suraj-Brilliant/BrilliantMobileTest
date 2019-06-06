using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartRequest;
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServiceUCCommonFilter;


namespace BrilliantWMS.POR
{
    public partial class ImportDSo : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        DataTable dt;
        DataSet ds = new DataSet();
        SqlConnection con1 = new SqlConnection("");
        
        protected void Page_Load(object sender, EventArgs e)
        {
            con1.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            //UCFormHeader1.FormHeaderText = "Import Sales Order";
            Button12.Visible = false;
            Label1.Text = "";
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            if (!IsPostBack)
            {
                GetCompany();
            }
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
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string folderpath1 = "";
            try
            {
                long companyID = long.Parse(hdncompanyid.Value.ToString());
                long DepartmentID = long.Parse(hdndeptid.Value.ToString());
                Session.Add("DepartmentID", DepartmentID);
                long Success = 0, fail = 0;
                string userId = profile.Personal.UserID.ToString();

                string company = companyID.ToString();
                string dept = DepartmentID.ToString();

                string folderpath = company + dept + userId;
                folderpath1 = Server.MapPath("./" + folderpath.ToString());
                if (Directory.Exists(folderpath1))
                {
                    Directory.Delete(folderpath1);
                }
               // Directory.CreateDirectory(folderpath);
                Directory.CreateDirectory(folderpath1);

                //Dim flImages As HttpFileCollection = Request.Files   

                  // productClient.ClearImageUploadLog(profile.DBConnection._constr);
                    HttpFileCollection flImages = Request.Files;

                    

                    for (int i = 0; i < flImages.Count; i++)
                    {
                        HttpPostedFile userPostedFile = flImages[i];
                       // userPostedFile.SaveAs(Server.MapPath("./UploadedImages/") + userPostedFile.FileName);
                        
                       // userPostedFile.SaveAs(Server.MapPath("./" + folderpath.ToString()) + userPostedFile.FileName);
                        string Physicalpath = Server.MapPath("./" + folderpath.ToString()+"/" + userPostedFile.FileName);
                        userPostedFile.SaveAs(Physicalpath);
                       // userPostedFile.SaveAs(folderpath1 + userPostedFile.FileName);
                        //userPostedFile.SaveAs(Directory.CreateDirectory(folderpath1) + userPostedFile.FileName);
                    }
              
                // Multiple file upload

                 
                string path = "";
                
                char[] sep = new char[2];
               // string[] Directories = System.IO.Path.GetDirectoryName(FileuploadPO.PostedFile.FileName).Split(sep);
                //string DirectoryName = (Directories.Length == 2 && Directories[1].Length == 0) ? Directories[0] : Directories[Directories.Length - 1];

                path = FileuploadPO.PostedFile.FileName;
                if (path != "")
                {
                    //string fileName = "", Reason = "";
                   // string sourcePath = @"C:\Users\admin\Desktop\Logos";
                    string sourcePath = Server.MapPath("./" + folderpath.ToString());
                    string targetPath = Server.MapPath("./TempImg/");

                    string sourceFile = System.IO.Path.Combine(sourcePath, path);
                    string destFile = System.IO.Path.Combine(targetPath, path);

                    System.IO.File.Copy(sourceFile, destFile, true);

                    if (System.IO.Directory.Exists(sourcePath))
                    {
                        string[] files = System.IO.Directory.GetFiles(sourcePath);

                        string totalcount = files.Length.ToString();
                        Session.Add("TotalImages", totalcount);
                        // Copy the files and overwrite destination files if they already exist.
                        int currentFileCount = 1;
                        foreach (string s in files)
                        {
                            long availableProdId = 0;
                            string fileName = "", Reason = "";
                            // Use static Path methods to extract only the file name from the path.
                            fileName = System.IO.Path.GetFileName(s);
                            string FileNamewithoutext = System.IO.Path.GetFileNameWithoutExtension(s);
                            string OmsSkucode = FileNamewithoutext + "-" + companyID + "-" + DepartmentID;
                            string dbpath = "TempImg/";
                            string destFile1 = "";

                            availableProdId = productClient.GetSKUByFilename(OmsSkucode, profile.DBConnection._constr);
                            if (availableProdId != 0)
                            {
                                long getSKUIdfromtImage = productClient.getSKUIdfromtImage(availableProdId, profile.DBConnection._constr);
                                if (getSKUIdfromtImage == 0)
                                {
                                    fileName = System.IO.Path.GetFileName(s);
                                    string ext = Path.GetExtension(fileName);
                                    destFile = System.IO.Path.Combine(targetPath, fileName);
                                    destFile1 = System.IO.Path.Combine(dbpath, fileName);
                                    System.IO.File.Copy(s, destFile, true);
                                    string Imagename = OmsSkucode + "." + ext;
                                    /* Image File In Binary  */
                                    byte[] img = System.IO.File.ReadAllBytes(Server.MapPath(destFile1));
                                    /* Image File In Binary  */
                                    /*Change for Compress Image*/
                                    FileInfo fi = new FileInfo(destFile);
                                    var size=fi.Length;
                                    decimal newsize = Math.Round(((decimal)size / (decimal)1024), 2);
                                    if (newsize > 60)
                                    {
                                        productClient.InsertintotImage("Product", availableProdId, Imagename, destFile1, ext, userId, DateTime.Now, companyID, img, "", "", profile.DBConnection._constr);
                                    }
                                    else if (newsize > 45 && newsize <= 60)
                                    {
                                        System.Drawing.Image iimg = System.Drawing.Image.FromFile(destFile);
                                        int height = iimg.Height;
                                        int width = iimg.Width;

                                        Bitmap original_image = new Bitmap(destFile);
                                        Bitmap finalImg = null;
                                        Graphics graphic = null;
                                        decimal w = width / 2;
                                        int reqW = Convert.ToInt16(Math.Round(w));
                                        //if (reqW > 100) reqW = 100;
                                        decimal h = height / 2;
                                        int reqH = Convert.ToInt16(Math.Round(h));
                                        // if (reqH > 100) reqH = 100;
                                        finalImg = new Bitmap(reqW, reqH);
                                        graphic = Graphics.FromImage(finalImg);
                                        graphic.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, reqW, reqH));
                                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                        graphic.DrawImage(original_image, 0, 0, reqW, reqH);
                                        string type = ext;

                                        finalImg.Save(MapPath("~/Product/TempImg/" + Imagename + type));// MapPath("~/Product/TempImg/" + hdnprodID.Value + Path.GetExtension(FileUpload1.FileName)));
                                        byte[] newImgSize = System.IO.File.ReadAllBytes(MapPath("~/Product/TempImg/" + Imagename + type));

                                        productClient.InsertintotImage("Product", availableProdId, Imagename, destFile1, ext, userId, DateTime.Now, companyID, newImgSize, "", "", profile.DBConnection._constr);
                                    }
                                    else
                                    {
                                        productClient.InsertintotImage("Product", availableProdId, Imagename, destFile1, ext, userId, DateTime.Now, companyID, img, "", "", profile.DBConnection._constr);
                                    }
                                    /*Change for Compress Image*/
                                   // productClient.InsertintotImage("Product", availableProdId, Imagename, destFile1, ext, userId, DateTime.Now, companyID, img,"","", profile.DBConnection._constr);
                                    Success = Success + 1;
                                }
                                else
                                {
                                    Reason = "Image allready Present";
                                    destFile = System.IO.Path.Combine(targetPath, fileName);
                                    destFile1 = System.IO.Path.Combine(dbpath, fileName);
                                    productClient.InsertIntotImageImportLog(availableProdId, fileName, destFile1, userId, DateTime.Now, Reason, companyID, DepartmentID, OmsSkucode, profile.DBConnection._constr);
                                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + FileNamewithoutext.ToString() + "''Uploaded Image product Allready available!');", true);
                                    fail = fail + 1;
                                }
                            }
                            else
                            {
                                hdnnotavailableimageprod.Value = hdnnotavailableimageprod.Value + "," + fileName;
                                Reason = "SKU Not Available";
                                destFile = System.IO.Path.Combine(targetPath, fileName);
                                destFile1 = System.IO.Path.Combine(dbpath, fileName);
                                productClient.InsertIntotImageImportLog(availableProdId, fileName, destFile1, userId, DateTime.Now, Reason, companyID, DepartmentID, OmsSkucode, profile.DBConnection._constr);
                                fail = fail + 1;
                            }
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "checkProgress(" + currentFileCount + ");", true);
                            currentFileCount = currentFileCount + 1;
                            string Deleteimage = System.IO.Path.Combine(sourcePath, fileName);
                            FileInfo file = new FileInfo(Deleteimage);
                            file.Delete();
                           // productClient.Close();
                        }

                        

                      //  System.Threading.Thread.Sleep(7000);
                        Session.Add("SuccessImages", Success);
                        Session.Add("FailedImages", fail);
                        uploadMessage.Visible = true;
                        Button12.Visible = true;

                        if (Directory.Exists(folderpath1))
                        {
                            Directory.Delete(folderpath1);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Source path does not exist!");
                    }


                }
                else
                {
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select image to upload');", true);
                }

            }
            catch (Exception ex)
            {
                Array.ForEach(Directory.GetFiles(folderpath1), File.Delete);
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Image Import failed - Please select file with size less than 60kb.');", true);
            }
            finally 
            {
                productClient.Close();
            }










                    // string Fullpath = Server.MapPath("./ProductImage/" + path);
                    //System.IO.File.Exists(Fullpath);
                    //{
                    //    System.IO.File.Delete(Fullpath);
                    //}
                    //FileuploadPO.PostedFile.SaveAs(Server.MapPath("./ProductImage/" + path));
                    ////string strFileType = path.GetExtension(FileuploadPO.FileName).ToLower();
                    //string strFileType = System.IO.Path.GetExtension(path).ToString().ToLower();
                    // string Fullpath = Server.MapPath("ImportSO\\" + path);

                    //    //Connection String to Excel Workbook
                    //    if (strFileType.Trim() == ".xls")
                    //    {
                    //        connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath("./ImportSO/" + path) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    //    }
                    //    else if (strFileType.Trim() == ".xlsx")
                    //    {
                    //        connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Server.MapPath("./ImportSO/" + path) + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //        //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\Rahul\NGroup Project Start\NGROUPProduct\PowerOnRent\PowerOnRentwebapp\POR\ImportSO\" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //    }

                    //    OleDbConnection excelConnection = new OleDbConnection(connString);

                    //    //Create OleDbCommand to fetch data from Excel
                    //    OleDbCommand cmd1 = new OleDbCommand("Select * from [Template$]", excelConnection);
                    //    excelConnection.Open();
                    //    OleDbDataReader dReader;
                    //    dReader = cmd1.ExecuteReader();
                    //    SqlBulkCopy sqlBulk = new SqlBulkCopy(con1);
                    //    con1.Open();
                    //    sqlBulk.DestinationTableName = "tImportSO";
                    //    sqlBulk.WriteToServer(dReader);
                    //    excelConnection.Close();

                    //    iSalesOrderClient Sales = new iSalesOrderClient();
                    //    ds = Sales.GetimportSodata();
                    //    dt = ds.Tables[0];
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        System.Threading.Thread.Sleep(7000);
                    //        Label1.Text = "Upload Successful!";
                    //        btnNext.Visible = true;
                    //        Sales.UpdateSOtrim();
                    //    }
                    //    else
                    //    {
                    //        Label1.Text = "Upload Not Successful,Please upload Right template!";

                    //    }
              


       }


        protected void Btncancel_Click(object sender, EventArgs e)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            if (hdndeptid.Value != "")
            {
                productClient.deleteimportlogdata(long.Parse(hdndeptid.Value), profile.DBConnection._constr);
            }
            Response.Redirect("ImportDSo.aspx"); //CommonControls
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportVSo.aspx");
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            
            lblHeading.Text = rm.GetString("ImportImages", ci);
           // lblcompany.Text = rm.GetString("company", ci);
            //lblDept.Text = rm.GetString("Department", ci);
            lblSelecFile.Text = rm.GetString("SelectImportFile", ci);
            btnUploadPo.Text = rm.GetString("Upload", ci);
            Button12.Text = rm.GetString("Next", ci);
            Btncancel.Text = rm.GetString("Cancel", ci);
            lblstep1.Text = rm.GetString("UploadFile", ci);
            lblstep2.Text = rm.GetString("validaton", ci);
            //lblstep3.Text = rm.GetString("Finished", ci);
        }

      
    }
}