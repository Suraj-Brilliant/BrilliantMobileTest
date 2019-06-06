using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.Login;
using BrilliantWMS.ContactPerson;
using BrilliantWMS.PORServiceUCCommonFilter;
using System.Data;
using BrilliantWMS.WarehouseService;

namespace BrilliantWMS.Warehouse
{
    public partial class WarehouseShelf : System.Web.UI.Page
    {
       
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            // loadstring();

            if (Request.QueryString["SectionID"] != null) hdnsectionID.Value = Request.QueryString["SectionID"].ToString();
            if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value = Request.QueryString["CustomerID"].ToString();
            if (hdnsectionID.Value != "")
            {
                hdnCompanyID.Value = Session["CompanyID"].ToString();
                BindShelfList(long.Parse(hdnsectionID.Value));
                clear();
            }
            else
            {
                WebMsgBox.MsgBox.Show("Please Select Section");
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
        }

        protected void BindShelfList(long SectionID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            grdshelf.DataSource = Warehouseclient.GetWarehouseShelf(SectionID, profile.DBConnection._constr);
            grdshelf.DataBind();
        }

        protected void grdshelf_OnRebind(object sender, EventArgs e)
        {
            BindShelfList(long.Parse(hdnsectionID.Value));
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnshelfID.Value = hdnSelectedRec.Value.ToString();
            Session["Shelf"] = hdnshelfID.Value.ToString();
            GetWarehouseShelfByID();
            hdnstate.Value = "Edit";
        }

        protected void clear()
        {
            txtshelf.Text = "";
            txtsortcode.Text = "";
            txtdescription.Text = "";
            // hdnfloarID.Value = "";
            hdnshelfID.Value = "";
            
        }

        protected void GetWarehouseShelfByID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();

            try
            {
                mShelf shelf = new mShelf();
                shelf = Warehouseclient.GetWarehouseShelfByID(long.Parse(hdnshelfID.Value), profile.DBConnection._constr);

                if (shelf.Name != null) txtshelf.Text = shelf.Name.ToString();
                if (shelf.SortCode != null) txtsortcode.Text = shelf.SortCode.ToString();
                if (shelf.Description != null) txtdescription.Text = shelf.Description.ToString();
                if (shelf.CustomerID != null) hdncustomerID.Value = shelf.CustomerID.ToString();
                hdnCompanyID.Value = shelf.CompanyID.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "WarehousePassage", "GetWarehouseShelfByID");
            }
            finally
            {
                Warehouseclient.Close();

            }
        }

        

        [WebMethod]
        public static string WMSaveRequestHead(object objCon, string State)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();

            mShelf shelf = new mShelf();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                shelf.Name = dictionary["Name"].ToString();
                shelf.SortCode = long.Parse(dictionary["SortCode"].ToString());
                shelf.Description = dictionary["description"].ToString();
                shelf.CompanyID = long.Parse(dictionary["CompanyId"].ToString());
                shelf.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                shelf.SectionID = long.Parse(dictionary["hdnsectionID"].ToString());

                if (State == "Edit")
                {
                    shelf.ID = Convert.ToInt64(HttpContext.Current.Session["Shelf"].ToString());
                    shelf.CreatedBy = profile.Personal.UserID;
                    shelf.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehouseShelf(shelf, profile.DBConnection._constr);
                }
                else
                {
                    shelf.CreatedBy = profile.Personal.UserID;
                    shelf.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehouseShelf(shelf, profile.DBConnection._constr);
                }
                result = "Shelf saved successfully";
            }
            catch { result = "Some error occurred"; }
            finally { Warehouseclient.Close(); }

            return result;
        }

        private void loadstring()
        {
            try
            {
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                //rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                //ci = Thread.CurrentThread.CurrentCulture;

                //lblContactName.Text = rm.GetString("ContactName", ci);
                //lblEmailID.Text = rm.GetString("EmailID", ci);
                //lblMobileNo.Text = rm.GetString("MobileNo", ci);
                //lblContactPersonList.Text = rm.GetString("ContactPersonList", ci);
                //btnSubmit.Value = rm.GetString("Submit", ci);
                //btnSave.Value = rm.GetString("Save", ci);
            }
            catch { }
        }
    }
}