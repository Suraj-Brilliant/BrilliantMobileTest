using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServiceEngineMaster;
using System.Web.Services;
using WebMsgBox;
using BrilliantWMS.WarehouseService;
using System.Collections;

namespace PowerOnRentwebapp.Location
{
    public partial class WarehouseLocation : System.Web.UI.Page
    {
        static string sessionID;
        static string TargetObject;
        static string Sequence;

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            this.txtlocationCode.Attributes.Add("onblur", "javascript:validateLocCode();");
            this.txtsortCode.Attributes.Add("onblur", "javascript:Validatesortcode();");
            if (!IsPostBack)
            {
                if (Request.QueryString["warehouseID"] != null) hdnWarehouseID.Value = Request.QueryString["warehouseID"].ToString();
                if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value = Request.QueryString["CustomerID"].ToString();
                if (Request.QueryString["LocationID"] != null) hdnlocationID.Value = Request.QueryString["LocationID"].ToString();
                hdnCompanyID.Value = Session["CompanyID"].ToString();
                hdnWarehouseName.Value = Session["WarehouseName"].ToString();
                txtwarehouse.Text = hdnWarehouseName.Value;
                FillLocationType();
                GetCapacityIn();
                if (hdnlocationID.Value != "0")
                {
                    GetLocationDetailByID(long.Parse(hdnlocationID.Value),long.Parse(hdnWarehouseID.Value));
                }
            }
        }


        public void GetLocationDetailByID(long LocationID, long WarehouseID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            V_WMS_GetWareLocationByLocID Location = new V_WMS_GetWareLocationByLocID();
            Location = Warehouseclient.GetWarehouseLocByID(LocationID, WarehouseID, profile.DBConnection._constr);
            if (Location.Code != null) txtlocationCode.Text = Location.Code.ToString();
            if (Location.AliasCode != null) txtaliascode.Text = Location.AliasCode.ToString();
            if (Location.SortCode != null) txtsortCode.Text = Location.SortCode.ToString();
            if (Location.Capacity != null) txtcapacity.Text = Location.Capacity.ToString();
            if (Location.LocationType != null) ddllocationtype.SelectedIndex = ddllocationtype.Items.IndexOf(ddllocationtype.Items.FindByValue(Location.LocationType.ToString()));
            if (Location.CapacityIn != null) ddlcapacityin.SelectedIndex = ddlcapacityin.Items.IndexOf(ddlcapacityin.Items.FindByValue(Location.CapacityIn.ToString()));
            if (Location.VelocityType != null) ddlvelocityType.SelectedIndex = ddlvelocityType.Items.IndexOf(ddlvelocityType.Items.FindByValue(Location.VelocityType.ToString()));
            if (Location.shelfID != null) hdnShelfID.Value = Location.shelfID.ToString();
            if (Location.Building != null) txtbuilding.Text = Location.Building.ToString();
            if (Location.Floar != null) txtfloar.Text = Location.Floar.ToString();
            if (Location.Passage != null) txtpathway.Text = Location.Passage.ToString();
            if (Location.Section != null) txtsection.Text = Location.Section.ToString();
            if (Location.Shelf != null) txtshelf.Text = Location.Shelf.ToString();
            if (Location.Active.Trim() == "Yes")
            {
                radiochannelY.Checked = true;
            }
            else
            {
                radiochannelN.Checked = true;
            }
        }

       
        [WebMethod]
        public static string PMSaveWLocation(object WLocationInfo,string LocationID)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            mLocation loc = new mLocation();
            tSKUTransaction skutrans = new tSKUTransaction();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)WLocationInfo;

                loc.ShelfID = long.Parse(dictionary["ShelfID"].ToString());
                loc.Code = dictionary["LocationCode"].ToString();
                loc.AliasCode = dictionary["AliasCode"].ToString();
                loc.SortCode = long.Parse(dictionary["SortCode"].ToString());
                loc.LocationType = dictionary["LocationType"].ToString();
                loc.CapacityIn = long.Parse(dictionary["CapacityIn"].ToString());
                loc.Capacity = decimal.Parse(dictionary["Capacity"].ToString());
                loc.VelocityType = dictionary["VelocityType"].ToString();
                loc.CompanyID = long.Parse(dictionary["CompanyID"].ToString());
                loc.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                loc.Active = dictionary["Active"].ToString().Trim();
                loc.WarehouseID = long.Parse(dictionary["WarehouseID"].ToString());
                skutrans.ClosingBalance = 0;
                skutrans.InQty = 0;
                skutrans.OutQty = 0;
                skutrans.CreatedBy = profile.Personal.UserID;
                skutrans.CreationDate = DateTime.Now;
                skutrans.CompanyID = long.Parse(dictionary["CompanyID"].ToString());
                skutrans.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                if (LocationID != "0")
                {
                    loc.ID = long.Parse(LocationID);
                    loc.CreatedBy = profile.Personal.UserID;
                    loc.CreationDate = DateTime.Now;
                    long LocID = Warehouseclient.SaveWarehouseLocation(loc, profile.DBConnection._constr);
                }
                else
                {
                    loc.AvailableBalance = 0;
                    loc.CreatedBy = profile.Personal.UserID;
                    loc.CreationDate = DateTime.Now;
                    long LocID = Warehouseclient.SaveWarehouseLocation(loc, profile.DBConnection._constr);
                    skutrans.LocationID = LocID;
                    Warehouseclient.AddRecordInSkuTransaction(skutrans, profile.DBConnection._constr);

                }
                result = "Location saved successfully";
            }
            catch { result = "Some error occurred"; }
            finally { Warehouseclient.Close(); }

            return result;
        }


        [WebMethod]
        public static string ChkduplicateLoc(string LocationCode,string WarehouseID)
        {
            string result = "";
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long count = Warehouseclient.CheckDuplicateLocation(LocationCode, long.Parse(WarehouseID), profile.DBConnection._constr);
            if(count >= 1)
            {
              result="Duplicate Found";
            }
            return result;
        }

        [WebMethod]
        public static string ChkDuplicateSortCode(string SortCode, string WarehouseID)
        {
            string result = "";
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long count = Warehouseclient.CheckDuplicateSortCode(long.Parse(SortCode), long.Parse(WarehouseID), profile.DBConnection._constr);
            if (count >= 1)
            {
                result = "Duplicate Found";
            }
            return result;
        }

       
        [WebMethod]
        public static string PMCheckDuplicate(string LocationName, long sequence)
        {
            iEngineMasterClient Location = new iEngineMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string result = "";
           // result = Location.CheckDuplicateLocation(sessionID, LocationName, sequence, profile.Personal.UserID.ToString(), TargetObject, profile.DBConnection._constr);
            Location.Close();
            return result;
        }

        protected void FillLocationType()
        {
            ddllocationtype.Items.Clear();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddllocationtype.DataSource = Warehouseclient.GetLocationType(profile.DBConnection._constr);
            ddllocationtype.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddllocationtype.Items.Insert(0, lst);
        }

        protected void GetCapacityIn()
        {
            ddlcapacityin.Items.Clear();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            ddlcapacityin.DataSource = Warehouseclient.GetCapacityIn(profile.DBConnection._constr);
            ddlcapacityin.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlcapacityin.Items.Insert(0, lst);
        }

    }
}