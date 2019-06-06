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
    public partial class WarehousePassage : System.Web.UI.Page
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

            if (Request.QueryString["FloarID"] != null) hdnfloarID.Value = Request.QueryString["FloarID"].ToString();
            if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value = Request.QueryString["CustomerID"].ToString();
            if (hdnfloarID.Value != "")
            {
                hdnCompanyID.Value = Session["CompanyID"].ToString();
                BindPassageList(long.Parse(hdnfloarID.Value));
                clear();
            }
            else
            {
                WebMsgBox.MsgBox.Show("Please Select Floar");
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
        }

        protected void BindPassageList(long FloarID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            grdpassage.DataSource = Warehouseclient.GetWarehousePassage(FloarID, profile.DBConnection._constr);
            grdpassage.DataBind();
        }

        protected void grdpassage_OnRebind(object sender, EventArgs e)
        {
            BindPassageList(long.Parse(hdnfloarID.Value));
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnpassageID.Value = hdnSelectedRec.Value.ToString();
            Session["PassageID"] = hdnpassageID.Value.ToString();
            GetPassageID();
            hdnstate.Value = "Edit";
        }

        protected void clear()
        {
            txtpassage.Text = "";
            txtsortcode.Text = "";
            txtdescription.Text = "";
           // hdnfloarID.Value = "";
            hdnpassageID.Value = "";
        }

        protected void GetPassageID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();

            try
            {
                mPathway path = new mPathway();
                path = Warehouseclient.GetWarehousePassageByID(long.Parse(hdnpassageID.Value), profile.DBConnection._constr);

                if (path.Name != null) txtpassage.Text = path.Name.ToString();
                if (path.SortCode != null) txtsortcode.Text = path.SortCode.ToString();
                if (path.Description != null) txtdescription.Text = path.Description.ToString();
                if (path.CustomerID != null) hdncustomerID.Value = path.CustomerID.ToString();
                hdnCompanyID.Value = path.CompanyID.ToString();
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

            mPathway path = new mPathway();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                path.Name = dictionary["Name"].ToString();
                path.SortCode = long.Parse(dictionary["SortCode"].ToString());
                path.Description = dictionary["description"].ToString();
                path.CompanyID = long.Parse(dictionary["CompanyId"].ToString());
                path.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                path.FloarID = long.Parse(dictionary["hdnfloarID"].ToString());

                if (State == "Edit")
                {
                    path.ID = Convert.ToInt64(HttpContext.Current.Session["PassageID"].ToString());
                    path.CreatedBy = profile.Personal.UserID;
                    path.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehousePassage(path, profile.DBConnection._constr);
                }
                else
                {
                    path.CreatedBy = profile.Personal.UserID;
                    path.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehousePassage(path, profile.DBConnection._constr);
                }
                result = "Passage saved successfully";
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