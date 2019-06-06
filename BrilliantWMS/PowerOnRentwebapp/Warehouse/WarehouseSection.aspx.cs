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
    public partial class WarehouseSection : System.Web.UI.Page
    {
        long DeptID = 0;
        long CompanyID = 0;
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            // loadstring();

            if (Request.QueryString["PassageID"] != null) hdnpassageID.Value = Request.QueryString["PassageID"].ToString();
            if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value = Request.QueryString["CustomerID"].ToString();
            if (hdnpassageID.Value != "")
            {
                hdnCompanyID.Value = Session["CompanyID"].ToString();
                BindSectionList(long.Parse(hdnpassageID.Value));
                clear();
            }
            else
            {
                WebMsgBox.MsgBox.Show("Please Select Passage");
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
        }


        protected void BindSectionList(long PassageID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            grdsection.DataSource = Warehouseclient.GetWarehouseSection(PassageID, profile.DBConnection._constr);
            grdsection.DataBind();
        }


        protected void grdsection_OnRebind(object sender, EventArgs e)
        {
            BindSectionList(long.Parse(hdnpassageID.Value));
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnsectionID.Value = hdnSelectedRec.Value.ToString();
            Session["SectionID"] = hdnsectionID.Value.ToString();
            GetSectionByID();
            hdnstate.Value = "Edit";
        }

        protected void clear()
        {
            txtsection.Text = "";
            txtsortcode.Text = "";
            txtdescription.Text = "";
            // hdnfloarID.Value = "";
            hdnsectionID.Value = "";
        }

        protected void GetSectionByID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();

            try
            {
           
                mSection sec = new mSection();
                sec = Warehouseclient.GetWarehouseSectionByID(long.Parse(hdnsectionID.Value), profile.DBConnection._constr);

                if (sec.Name != null) txtsection.Text = sec.Name.ToString();
                if (sec.SortCode != null) txtsortcode.Text = sec.SortCode.ToString();
                if (sec.Description != null) txtdescription.Text = sec.Description.ToString();
                if (sec.CustomerID != null) hdncustomerID.Value = sec.CustomerID.ToString();
                hdnCompanyID.Value = sec.CompanyID.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "WarehousePassage", "GetPassageID");
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

            mSection sec = new mSection();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                sec.Name = dictionary["Name"].ToString();
                sec.SortCode = long.Parse(dictionary["SortCode"].ToString());
                sec.Description = dictionary["description"].ToString();
                sec.CompanyID = long.Parse(dictionary["CompanyId"].ToString());
                sec.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                sec.PathID = long.Parse(dictionary["hdnpassageID"].ToString());

                if (State == "Edit")
                {
                    sec.ID = Convert.ToInt64(HttpContext.Current.Session["PassageID"].ToString());
                    sec.CreatedBy = profile.Personal.UserID;
                    sec.CreationDate = DateTime.Now;
                    long Section = Warehouseclient.SaveWarehouseSection(sec, profile.DBConnection._constr);
                }
                else
                {
                    sec.CreatedBy = profile.Personal.UserID;
                    sec.CreationDate = DateTime.Now;
                    long SectionID = Warehouseclient.SaveWarehouseSection(sec, profile.DBConnection._constr);
                }
                result = "Section saved successfully";
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