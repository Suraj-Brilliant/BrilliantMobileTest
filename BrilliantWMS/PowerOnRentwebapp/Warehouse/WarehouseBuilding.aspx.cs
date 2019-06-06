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
    public partial class WarehouseBuilding : System.Web.UI.Page
    {
        long WarehouseID = 0;
        long CustomerID = 0;
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
           // loadstring();

            if (Request.QueryString["warehouseID"] != null) hdnwarehouseID.Value = Request.QueryString["warehouseID"].ToString();
            if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value =Request.QueryString["CustomerID"].ToString();
            hdnCompanyID.Value = Session["CompanyID"].ToString();
            BindBuildingList(long.Parse(hdnwarehouseID.Value));
            clear();
        }


        protected void BindBuildingList(long WarehouseID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            gvBuilding.DataSource = Warehouseclient.GetWarehouseBuilding(WarehouseID, profile.DBConnection._constr);
            gvBuilding.DataBind();
        }

        protected void gvBuilding_OnRebind(object sender, EventArgs e)
        {
            BindBuildingList(long.Parse(hdnwarehouseID.Value));
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnbuilding.Value = hdnSelectedRec.Value.ToString();
           // Session["DeptID"] = DeptID.ToString();
            Session["BuildingID"] = hdnbuilding.Value.ToString();
            GetBuildingByID();
            hdnstate.Value = "Edit";
        }

        protected void clear()
        {
            txtbuildname.Text = "";
            txtsortcode.Text = "";
            txtcapacity.Text = "";
            txtdescription.Text = "";
            hdnbuilding.Value = "";
       }

        protected void GetBuildingByID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            try
            {
                mWarehouseBuilding warebuilding = new mWarehouseBuilding();
                warebuilding = Warehouseclient.GetWareBuildingByID(long.Parse(hdnbuilding.Value), profile.DBConnection._constr);

                if (warebuilding.Name != null) txtbuildname.Text = warebuilding.Name.ToString();
                if (warebuilding.SortCode != null) txtsortcode.Text = warebuilding.SortCode.ToString();
                if (warebuilding.Capacity != null) txtcapacity.Text = warebuilding.Capacity.ToString();
                if (warebuilding.Description != null) txtdescription.Text = warebuilding.Description.ToString();
                if (warebuilding.CustomerID != null) hdncustomerID.Value = warebuilding.CustomerID.ToString();
                hdnCompanyID.Value = warebuilding.CompanyID.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "AddEditSearchContact", "GetContactDetailByContactID");
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
            mWarehouseBuilding building = new mWarehouseBuilding();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                building.Name = dictionary["Name"].ToString();
                building.SortCode = dictionary["SortCode"].ToString();
                building.Capacity = decimal.Parse(dictionary["Capacity"].ToString());
                building.Description = dictionary["description"].ToString();
                building.CompanyID = long.Parse(dictionary["CompanyId"].ToString());
                building.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                building.WarehouseID = long.Parse(dictionary["WarehouseID"].ToString());                

                if (State == "Edit")
                {
                    building.ID = Convert.ToInt64(HttpContext.Current.Session["BuildingID"].ToString());
                    long BuildID = Warehouseclient.SaveWareBuilding(building, profile.DBConnection._constr);
                   
                }
                else
                {
                    building.CreatedBy = profile.Personal.UserID;
                    building.CreationDate = DateTime.Now;
                    long BuildID = Warehouseclient.SaveWareBuilding(building, profile.DBConnection._constr);
                }
                result = "Building saved successfully";
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