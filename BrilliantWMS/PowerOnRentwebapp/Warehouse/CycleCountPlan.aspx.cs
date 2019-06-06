using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.WarehouseService;
using BrilliantWMS.CycleCountService;
using System.Data;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Configuration;

namespace BrilliantWMS.Warehouse
{
    public partial class CycleCountPlan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillWarehouse();
            UC_FromDate.Date = DateTime.Now;
            hdnsessionID.Value = Session.SessionID;
           // BulkInsertToDataBase("Product", 9, hdnsessionID.Value);
            if (!IsPostBack)
            {
                txtgridvalues.Text = "";
                DeleteCycleTempData(hdnsessionID.Value);
                UC_FromDate.startdate(DateTime.Now);
                UC_ToDate.startdate(DateTime.Now);
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

        protected void grdcyclecount_Select(object sender, EventArgs e)
        {

        }

        protected void grdcyclecount_RebindGrid(object sender, EventArgs e)
        {
            ddlWarehouse.Enabled = false;
            ddlcountbasis.Enabled = false;
            BindCycleCountProduct();
           
        }

        public void BindCycleCountProduct()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            iCycleCountClient cycleclient = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            string Object = "Product";
            
            string SessionID = Session.SessionID;
            try
            {
                //ds = company.GetCostCenterList(long.Parse(hndCompanyid.Value), profile.DBConnection._constr);
                ds = cycleclient.GetCycleCounttempdata(Object, profile.Personal.UserID, SessionID, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtgridvalues.Text = "Product";
                    grdcyclecount.DataSource = ds.Tables[0];
                    grdcyclecount.DataBind();
                  
                }
                else
                {
                    grdcyclecount.DataSource = null;
                    grdcyclecount.DataBind();
                }
            }
            catch { }
            finally { cycleclient.Close(); }
        }

        protected void grdlocation_RebindGrid(object sender, EventArgs e)
        {
            ddlWarehouse.Enabled = false;
            ddlcountbasis.Enabled = false;
            BindCycleCountLocation();
           
        }

        public void BindCycleCountLocation()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            iCycleCountClient cycleclient = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            //string Object = "Location";
            string Object = hdnobject.Value;

            string SessionID = Session.SessionID;
            try
            {
                ds = cycleclient.GetCycleCountTempDataByLoc(Object, profile.Personal.UserID, SessionID, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtgridvalues.Text = "Location";
                    grdlocation.DataSource = ds.Tables[0];
                    grdlocation.DataBind();

                }
                else
                {
                    grdlocation.DataSource = null;
                    grdlocation.DataBind();
                }
            }
            catch { }
            finally { cycleclient.Close(); }
        }





        [WebMethod]
        public static string PMSaveWLocation(object Plans)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            iCycleCountClient cycleclient = new iCycleCountClient();
            tCycleCountHead cycleH = new tCycleCountHead();
            string ContBasis = "", SessionID = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)Plans;

                cycleH.Title = dictionary["Title"].ToString();
                cycleH.WarehouseID = long.Parse(dictionary["WarehouseID"].ToString());
                cycleH.Status = dictionary["Status"].ToString();
                cycleH.Frequency = dictionary["Frequency"].ToString();
                string Frequency = dictionary["Frequency"].ToString();
                cycleH.CountBasis = dictionary["CountBasis"].ToString();
                cycleH.Active = "Yes";
                ContBasis = dictionary["CountBasis"].ToString();
                SessionID = dictionary["session"].ToString();

                DateTime FromDate = DateTime.Parse(dictionary["FromDate"].ToString());
                DateTime ToDate = DateTime.Parse(dictionary["ToDate"].ToString());
                TimeSpan days = ToDate - FromDate;
                long DaysNo = long.Parse(days.TotalDays.ToString());
                long totaldays=0;
                int i = 0;
                int j = 0;
                int ival;
                if (Frequency == "Daily")
                {
                    if (DaysNo >= 12)
                    {
                        result = "More Than Daily";
                       
                    }
                    else
                    {
                        totaldays = DaysNo;
                    }
                }
                else if (Frequency == "Weekly")
                {
                    if (DaysNo >= 84)
                    {
                        result = "More Than Weekly";
                    }
                    else
                    {
                      double dayst = DaysNo / 7;
                      totaldays = long.Parse( Math.Round(dayst).ToString());
                    }
                }
                else if (Frequency == "Monthly")
                {
                    if (DaysNo >= 365)
                    {
                        result = "More Than Monthly";
                    }
                    else
                    {
                        double dayst = DaysNo / 30;
                    }
                }
                else if (Frequency == "Quarterly")
                {
                    if (DaysNo >= 1080)
                    {
                        result = "More Than Quarterly";
                    }
                    else
                    {
                        double dayst = DaysNo / 90;
                        totaldays = long.Parse(Math.Round(dayst).ToString());
                    }
                }

                if (result == "")
                {
                    for (i = 0; i <= totaldays; i++)
                    {
                        cycleH.CycleCountDate = FromDate.AddDays(j);
                        long CycleHeadID = cycleclient.SaveCycleCountHead(cycleH, profile.DBConnection._constr);
                        BulkInsertToDataBase(ContBasis, CycleHeadID, SessionID);

                        if (Frequency == "Daily") j = j + 1;
                        else if (Frequency == "Weekly") j = j + 7;
                        else if (Frequency == "Monthly") j = j + 30;
                        else if (Frequency == "Quarterly") j = j + 90;
                    }
                    result = "Cycle saved successfully";
                    DeleteFromCycleTempData(ContBasis, SessionID);
                }
            }
            catch
            { 
                result = "Some error occurred";
                DeleteFromCycleTempData(ContBasis, SessionID);
            }
            finally
            {
                cycleclient.Close();
            }
          
            return result;
        }

        private static void BulkInsertToDataBase(string Object1, long HeadID,string SessionID)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection("");
            iCycleCountClient cycleclient = new iCycleCountClient();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            conn.Open();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataTable dt = new DataTable();
                ds = cycleclient.GetCycleTempDataToInsert(Object1, profile.Personal.UserID, SessionID, profile.DBConnection._constr);
                dt = ds.Tables[0];
                DataRow row = dt.NewRow();
                dt.Columns.Add("CycleHeadID", typeof(System.Int64));
                dt.Columns["CycleHeadID"].Expression = HeadID.ToString();
                cycleclient.SaveCyclePlanData(dt, profile.DBConnection._constr);
            }
            catch { }
            finally
            {
                cycleclient.Close();
                conn.Close();
            }
        }

        public static void DeleteFromCycleTempData(string Object, string SessionID)
        {
            iCycleCountClient cycleclient = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            cycleclient.DeleteCycletempData(Object, profile.Personal.UserID, SessionID, profile.DBConnection._constr);
            cycleclient.Close();
        }

        public void DeleteCycleTempData(string SessionID)
        {
            iCycleCountClient cycleclient = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            cycleclient.DeleteCycleTempWithoutObj(profile.Personal.UserID, SessionID, profile.DBConnection._constr);
            cycleclient.Close();
        }
    }
}