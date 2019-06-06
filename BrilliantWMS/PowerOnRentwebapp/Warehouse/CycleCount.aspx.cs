using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BrilliantWMS.Login;
using System.Data;
using WebMsgBox;
using System.Data.SqlClient;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.WarehouseService;
using BrilliantWMS.CycleCountService;
using System.Collections;

namespace BrilliantWMS.POR
{
    public partial class CycleCount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.txtlocation.Attributes.Add("onblur", "javascript:JSBarCode();");
            this.txtproduct.Attributes.Add("onblur", "javascript:CheckSKUinPlan();");

            if (!IsPostBack)
            {
                setActiveTab(0);
                BindCycleMainGrid();
                FillWarehouse();

            }
            this.UCToolbar1.ToolbarAccess("DesignationMaster");
            this.UCToolbar1.evClickAddNew += pageAddNew;
            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;

        }

       
        protected void grdcyclecount_Select(object sender, EventArgs e)
        {
            clear();
             CustomProfile profile = CustomProfile.GetProfile();
            this.UCToolbar1.ToolbarAccess("Edit");
            Hashtable selectedrec = (Hashtable)grdcyclecount.SelectedRecords[0];
            hdnCycleheadID.Value = selectedrec["ID"].ToString();
            long reuslt = long.Parse(hdnCycleheadID.Value);
            GetCyleCountHeadByID(reuslt);
            BindCycleDetailGrid(reuslt);
            ddlWarehouse.Enabled = false;
            txtexicutive.Text = profile.Personal.UserName.ToString();
           // UC_FromDate.EnableTheming = false;
            ddlcountbasis.Enabled = false;
            hdnstate.Value = "Edit";
            txtexicutive.Enabled = false;
            setActiveTab(1);
        }



        protected void GetCyleCountHeadByID(long CycleheadID)
        {
            iCycleCountClient cycle = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            tCycleCountHead Cyclehead = new tCycleCountHead();
            Cyclehead = cycle.GetCyleCountHeadByID(CycleheadID, profile.DBConnection._constr);
            if (Cyclehead.Title != null) txtTitle.Text = Cyclehead.Title.ToString();
            FillWarehouse();
            if (Cyclehead.WarehouseID != null) ddlWarehouse.SelectedIndex = ddlWarehouse.Items.IndexOf(ddlWarehouse.Items.FindByValue(Cyclehead.WarehouseID.ToString()));
            hdnwarehouseId.Value = Cyclehead.WarehouseID.ToString();
            if (Cyclehead.CycleCountDate != null) UC_FromDate.Date = Cyclehead.CycleCountDate;
            if (Cyclehead.Exicutive != null) txtexicutive.Text = Cyclehead.Exicutive.ToString();
            if (Cyclehead.Status != null) ddlstatus.SelectedIndex = ddlstatus.Items.IndexOf(ddlstatus.Items.FindByText(Cyclehead.Status.ToString()));
            if (Cyclehead.CountBasis != null) ddlcountbasis.SelectedIndex = ddlcountbasis.Items.IndexOf(ddlcountbasis.Items.FindByText(Cyclehead.CountBasis.ToString()));
            if (Cyclehead.Frequency != null) hdnfrequency.Value = Cyclehead.Frequency.ToString();
            if (Cyclehead.Active == "Yes")
            {
                rbtnActiveYes.Checked = true;
                rbtnActiveNo.Checked = false;
            }
            else
            {
                rbtnActiveYes.Checked = false;
                rbtnActiveNo.Checked = true;
            }
        }


        protected void grdImportView_RebindGrid(object sender, EventArgs e)
        {
            BindCycleDetailGrid(long.Parse(hdnCycleheadID.Value));
        }

        public void BindCycleDetailGrid(long CycleHeadID)
        {
            iCycleCountClient cycle = new iCycleCountClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdImportView.DataSource = cycle.GetCycleCountDetail(CycleHeadID, profile.DBConnection._constr);                                             //GetCycleCountMain(profile.DBConnection._constr);
                grdImportView.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "MainCustomerGridBind");
            }
            finally
            {
                cycle.Close();
            }
        }

        protected void FillWarehouse()
        {
            ddlWarehouse.Items.Clear();
            iWarehouseClient Warehouse = new iWarehouseClient();
            CustomProfile profile = CustomProfile.GetProfile();
            long UserID = profile.Personal.UserID;
            ddlWarehouse.DataSource = Warehouse.GetWarehousebyUserID(UserID, profile.DBConnection._constr);
            ddlWarehouse.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlWarehouse.Items.Insert(0, lst);
            Warehouse.Close();
        }

        protected void setActiveTab(int ActiveTab)
        {
            Button btnSave = (Button)UCToolbar1.FindControl("btnSave");
            if (btnSave != null)
                if (ActiveTab == 0)
                {
                    TabChannelList.Visible = true;
                    tabChannelInfo.Visible = false;
                    tabChannelMaster.ActiveTabIndex = 0;
                }
                else
                {
                    TabChannelList.Visible = true;
                    tabChannelInfo.Visible = true;
                    tabChannelMaster.ActiveTabIndex = 1;
                }
        }

        public void BindCycleMainGrid()
        {
            iCycleCountClient cycle = new iCycleCountClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                grdcyclecount.DataSource = cycle.GetCycleCountMain(profile.DBConnection._constr);                                             //GetCycleCountMain(profile.DBConnection._constr);
                grdcyclecount.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Warehouse Master", "MainCustomerGridBind");
            }
            finally
            {
                cycle.Close();
            }
            
        }

        protected void pageAddNew(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
            setActiveTab(1);
            ddlWarehouse.Enabled = true;
            UC_FromDate.EnableTheming = true;
            ddlcountbasis.Enabled = true;
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iCycleCountClient cycleclient = new iCycleCountClient();
            tCycleCountHead cycleH = new tCycleCountHead();
            DataSet ds = new DataSet();
            if (hdnCycleheadID.Value != "")
            {
                cycleH.ID = long.Parse(hdnCycleheadID.Value);
                cycleH.Title = txtTitle.Text.ToString();
                cycleH.Status = ddlstatus.SelectedItem.Text;
                cycleH.CountBasis = ddlcountbasis.SelectedItem.Text;
                cycleH.CycleCountDate = UC_FromDate.Date;
                cycleH.WarehouseID = long.Parse(hdnwarehouseId.Value);
                cycleH.Frequency = hdnfrequency.Value;
                cycleH.CreatedBy = profile.Personal.UserID;
                cycleH.CreationDate = DateTime.Now;
                cycleH.Active = "Yes";
                if (rbtnActiveNo.Checked == true) cycleH.Active = "No";

                ds = cycleclient.getCompanyCustomer(long.Parse(hdnwarehouseId.Value), profile.DBConnection._constr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   cycleH.CompanyID  = long.Parse(ds.Tables[0].Rows[0]["CompanyID"].ToString());
                   cycleH.CustomerID = long.Parse(ds.Tables[0].Rows[0]["CustomerID"].ToString());
                }

                long CycleHeadID = cycleclient.SaveCycleCountHead(cycleH, profile.DBConnection._constr);
                WebMsgBox.MsgBox.Show("Record saved successfully");
                clear();
                BindCycleMainGrid();
                Response.Redirect("CycleCount.aspx");
            }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            clear();
        }

        public void clear()
        {
            txtTitle.Text = "";
            ddlWarehouse.SelectedIndex = 0;
            txtexicutive.Text = "";
            ddlstatus.SelectedIndex = 0;
            ddlcountbasis.SelectedIndex = 0;
            hdnCycleheadID.Value = "";
            hdnstate.Value = "";
            hdnfrequency.Value = "";
            hdnwarehouseId.Value = "";
            rbtnActiveYes.Checked = true;

        }

        protected void grdcyclecount_RebindGrid(object sender, EventArgs e)
        {
            //BindCycleCountProduct();
            BindCycleMainGrid();
        }


        [WebMethod]
        public static string CheckLocInPlan(string LocationCode, string CycleHeadID, string WarehouseID, string Object)
        {
            string result = "";
            string Location = "";
            long Count;
            long LocationID = 0;
            iCycleCountClient cycle = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                LocationID = cycle.GetLocationID(LocationCode.Trim(), long.Parse(WarehouseID), profile.DBConnection._constr);
                Location = LocationID.ToString();
                if (LocationID != 0)
                {
                    Count = cycle.CheckLocInPlann(Object, long.Parse(CycleHeadID), LocationID, profile.DBConnection._constr);

                    if (Count <= 0)
                    {
                        result = "Not In Plan";
                    }
                }

                else
                {
                    result = "Location Not Found";
                }

                result = result + "-" + Location; 
            }
            catch { }
            finally
            {
                cycle.Close();
            }

            return result;

        }


        [WebMethod]
        public static string CheckSKUInPlan(string SKUCode, string CycleHeadID, string WarehouseID, string Object,string locationID)
        {
            string result = "";
            string SKU = "";
            long Count;
            long SKUId = 0;
            iCycleCountClient cycle = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                SKUId = cycle.GetSKUIDBySKUCode(SKUCode.Trim(), long.Parse(WarehouseID), profile.DBConnection._constr);
                SKU = SKUId.ToString();
                if (SKUId != 0)
                {
                    Count = cycle.CheckLocInPlann(Object, long.Parse(CycleHeadID), SKUId, profile.DBConnection._constr);

                    if (Count <= 0)
                    {
                        result = "Not In Plan";
                    }
                }

                else
                {
                    result = "SKU Not Found";
                }

                result = result + "-" + SKU;
            }
            catch { }
            finally
            {
                cycle.Close();
            }

            return result;

        }




        [WebMethod]
        public static string PMSaveCycleCount(object cyclecount)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iCycleCountClient cycleclient = new iCycleCountClient();
            tCycleCountDetail cycledetail = new tCycleCountDetail();
            DataSet ds = new DataSet();
            string ContBasis = "", SessionID = "";
            decimal DiffQuantity = 0;
            long LocationID =0,SKUID=0;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)cyclecount;

                cycledetail.CountHeadID = long.Parse(dictionary["CycleheadID"].ToString());
                long CycleHeadId = long.Parse(dictionary["CycleheadID"].ToString());
                SKUID = long.Parse(dictionary["hdnProductID"].ToString());
                LocationID = long.Parse(dictionary["locationID"].ToString());
                string SKUCode = dictionary["txtproduct"].ToString();
                string LocationCode = dictionary["txtlocationCode"].ToString();
                long WarehouseID = long.Parse(dictionary["WarehouseID"].ToString());
                decimal Quantity = decimal.Parse(dictionary["Quantity"].ToString());
                cycledetail.BatchCode = dictionary["BatchCode"].ToString();
                string BatchCode = dictionary["BatchCode"].ToString();
                if (LocationID == 0 || LocationID == null)
                {
                    LocationID = cycleclient.GetLocationID(LocationCode.Trim(), WarehouseID, profile.DBConnection._constr);
                }
                if (SKUID == 0 || SKUID == null) 
                {
                    SKUID = cycleclient.GetSKUID(SKUCode.Trim(), WarehouseID, profile.DBConnection._constr);
                }
                cycledetail.ProductCode = SKUCode;
                cycledetail.LocationCode = LocationCode;
                cycledetail.SKUID = SKUID;
                cycledetail.LocationID = LocationID;
                cycledetail.CreatedBy = profile.Personal.UserID.ToString();
                cycledetail.CreationDate = DateTime.Now;

                
                    decimal SystemQty = cycleclient.GetSystemQtyByBatch(SKUID, LocationID, BatchCode, profile.DBConnection._constr);
                    cycledetail.QtyBalance = SystemQty;

                    ds = cycleclient.GetRepeatedCycleCountData(CycleHeadId, SKUID, LocationID, BatchCode, profile.DBConnection._constr);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        DiffQuantity = SystemQty - Quantity;
                        cycledetail.ActualQty = Quantity;
                        cycledetail.DiffQty = DiffQuantity;
                        long CycleDetailID = cycleclient.SaveCycleCount(cycledetail, profile.DBConnection._constr);
                    }
                    else
                    {
                        long DetailID = long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                        decimal ActualQty = decimal.Parse(ds.Tables[0].Rows[0]["ActualQty"].ToString());
                        decimal DiffQty = decimal.Parse(ds.Tables[0].Rows[0]["DiffQty"].ToString());
                        cycledetail.ActualQty = Quantity + ActualQty;
                        cycledetail.ID = DetailID;
                        DiffQuantity = DiffQty - Quantity;
                        cycledetail.DiffQty = DiffQuantity;
                        long CycleDetailID = cycleclient.SaveCycleCount(cycledetail, profile.DBConnection._constr);
                    }

            }
            catch
            {
                result = "Some error occurred";
            }
            finally
            {
 
            }

            return result;
        }


        [WebMethod]
        public static List<contact> GetBatchCodeBySKU(object objReq)
        {
            iCycleCountClient cycle = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;

                long WarehouseID = long.Parse(dictionary["WarehouseID"].ToString());
                long SKUID = long.Parse(dictionary["SKUID"].ToString());
                long locationID = long.Parse(dictionary["locationID"].ToString());

                //ds = StatutoryClient.GetCustomerList(ddlcompanyId, profile.DBConnection._constr);
                ds = cycle.GetBatchCodeBySKU(SKUID, locationID, profile.DBConnection._constr);
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
                        Loc.Name = dt.Rows[i]["BatchCode"].ToString();
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
                cycle.Close();
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



        [WebMethod]
        public static string CheckProduct(string productcode, long CountHeadID, string ProdLoc)
        {
            string result = "";

            return result;
        }

        [WebMethod]
        public static string WMSaveRequestHead(long CycleID)
        {
            string result = "Request saved successfully";

            return result;
        }

        [WebMethod]
        public static decimal GetlocationQty(object objReq)
        {
            decimal result=0.00M;

            return result;
        }

        [WebMethod]
        public static List<Locations> CheckSONumber(string ProdCode)
        {
            List<Locations> LocList = new List<Locations>();

            return LocList;
        }

        public class Locations
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