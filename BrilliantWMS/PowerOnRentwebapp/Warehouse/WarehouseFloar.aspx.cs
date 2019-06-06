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
    public partial class WarehouseFloar : System.Web.UI.Page
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

 
               if (Request.QueryString["BuildingID"] != null) hdnbuilding.Value = Request.QueryString["BuildingID"].ToString();
               if (Request.QueryString["CustomerID"] != null) hdncustomerID.Value = Request.QueryString["CustomerID"].ToString();
                if (hdnbuilding.Value != "")
                {
                    hdnCompanyID.Value = Session["CompanyID"].ToString();
                    BindFloarList(long.Parse(hdnbuilding.Value));
                    clear();
                }
                else
                {
                    WebMsgBox.MsgBox.Show("Please Select Building");
                    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                }
          
            
            
        }

        protected void BindFloarList(long Building)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();
            grdWareFloar.DataSource = Warehouseclient.GetWarehouseFloar(Building, profile.DBConnection._constr);
            grdWareFloar.DataBind();
        }
       

        protected void grdWareFloar_OnRebind(object sender, EventArgs e)
        {
            BindFloarList(long.Parse(hdnbuilding.Value));
        }

        protected void imgBtnEdit_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;
            clear();
            string prdSelValue = hdnSelectedRec.Value.ToString();
            hdnfloarID.Value = hdnSelectedRec.Value.ToString();
            Session["FloarID"] = hdnfloarID.Value.ToString();
            GetFloarByID();
            hdnstate.Value = "Edit";
        }

        protected void clear()
        {
            txtfloarname.Text = "";
            txtsortcode.Text = "";
            txtcapacity.Text = "";
            txtdescription.Text = "";
            hdnfloarID.Value = "";
        }

        protected void GetFloarByID()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iWarehouseClient Warehouseclient = new iWarehouseClient();

            try
            {
                mFloar Floar = new mFloar();
                Floar = Warehouseclient.GetWarehouseFloarbyID(long.Parse(hdnfloarID.Value), profile.DBConnection._constr);

                if (Floar.Name != null) txtfloarname.Text = Floar.Name.ToString();
                if (Floar.SortCode != null) txtsortcode.Text = Floar.SortCode.ToString();
                if (Floar.Capacity != null) txtcapacity.Text = Floar.Capacity.ToString();
                if (Floar.Description != null) txtdescription.Text = Floar.Description.ToString();
                if (Floar.CustomerID != null) hdncustomerID.Value = Floar.CustomerID.ToString();
                hdnCompanyID.Value = Floar.CompanyID.ToString();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "WarehouseFloar", "GetFloarByID");
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
            mFloar floar = new mFloar();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objCon;

                floar.Name = dictionary["Name"].ToString();
                floar.SortCode = long.Parse(dictionary["SortCode"].ToString());
                floar.Capacity = decimal.Parse(dictionary["Capacity"].ToString());
                floar.Description = dictionary["description"].ToString();
                floar.CompanyID = long.Parse(dictionary["CompanyId"].ToString());
                floar.CustomerID = long.Parse(dictionary["CustomerID"].ToString());
                floar.BuildingID = long.Parse(dictionary["BuildingID"].ToString());     

                if (State == "Edit")
                {
                    floar.ID = Convert.ToInt64(HttpContext.Current.Session["FloarID"].ToString());
                    floar.CreatedBy = profile.Personal.UserID;
                    floar.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehouseFloar(floar, profile.DBConnection._constr);
                }
                else
                {
                    floar.CreatedBy = profile.Personal.UserID;
                    floar.CreationDate = DateTime.Now;
                    long FloarID = Warehouseclient.SaveWarehouseFloar(floar, profile.DBConnection._constr);
                }
                result = "Floar saved successfully";
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