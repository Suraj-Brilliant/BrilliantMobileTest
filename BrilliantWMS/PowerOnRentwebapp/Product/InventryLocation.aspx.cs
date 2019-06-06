using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Collections;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using Obout.Grid;
using Obout.Ajax.UI.FileUpload;
using System.Data.OleDb;
using BrilliantWMS.Login;
using BrilliantWMS.ProductMasterService;
using System.Web.Services;
using WebMsgBox;

namespace BrilliantWMS.Product
{
    public partial class InventryLocation : System.Web.UI.Page
    {
        static string sessionID;
        static string TargetObject;
        long SkuId = 0;
        static string Sequence;
        static Page thispage;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            thispage = this;
            if (Request.QueryString["skuid"] != null) hdnskuid.Value = Request.QueryString["skuid"].ToString();
            if (Request.QueryString["CustomerID"] != null) hdncustomerId.Value = Request.QueryString["CustomerID"].ToString();
            if (Request.QueryString["prodLoc"] != null) hdnprodlocID.Value = Request.QueryString["prodLoc"].ToString();

            if (!IsPostBack)
            {
                if (hdnprodlocID.Value != "0")
                {
                    GetProductLocation();
                    txtOPeningBalance.Enabled = false;
                }
            }
        }

        private void GetProductLocation()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds;
            DataTable dt;
            iProductMasterClient productclient = new iProductMasterClient();
            ds = productclient.GetProdLocByPLID(long.Parse(hdnprodlocID.Value), profile.DBConnection._constr);
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtlocation.Text = dt.Rows[0]["Location"].ToString();
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(dt.Rows[0]["LocType"].ToString()));
                txtminReordrQty.Text = dt.Rows[0]["MinOrderQty"].ToString();
                txtMaxQty.Text = dt.Rows[0]["MaxOrderQty"].ToString();
                txtOPeningBalance.Text = dt.Rows[0]["OpeningStock"].ToString();
                hdnskuid.Value = dt.Rows[0]["ProdId"].ToString();
                hdnLocationSearchID.Value = dt.Rows[0]["LocationId"].ToString();
            }
        }


        public void BindDropdown()
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //iProductMasterClient productClient = new iProductMasterClient();
            //ddlLocation.DataSource = productClient.GetProductLocation(profile.DBConnection._constr);
            //ddlLocation.DataBind();

            //ListItem lst = new ListItem();
            //lst.Text = "--Select--";
            //lst.Value = "0";
            //ddlLocation.Items.Insert(0, lst);

            //productClient.Close();
        }

        [WebMethod]
        public static void PMSaveLocation(object LocationInfo)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Dictionary<string, object> rec = new Dictionary<string, object>();
                rec = (Dictionary<string, object>)LocationInfo;
               
                mProductLocation Location = new mProductLocation();
                tProductStockDetail ProdStock = new tProductStockDetail();

                long ProdLocID = Convert.ToInt64(rec["ProdLocID"].ToString());
                Location.ProdId = Convert.ToInt64(rec["SKUID"].ToString());
                Location.LocationId = Convert.ToInt64(rec["LocationID"].ToString());
                Location.LocType = rec["LocationType"].ToString();
                Location.MinOrderQty = decimal.Parse(rec["MinQty"].ToString());
                Location.MaxOrderQty = decimal.Parse(rec["MaxQty"].ToString());
                ProdStock.SiteID = Convert.ToInt64(rec["LocationID"].ToString());
                ProdStock.ProdID = Convert.ToInt64(rec["SKUID"].ToString());
                ProdStock.OpeningStock = decimal.Parse(rec["OpeningBalance"].ToString());
                ProdStock.AvailableBalance = decimal.Parse(rec["OpeningBalance"].ToString());
                if (ProdLocID != 0)
                {
                    Location.Id = Convert.ToInt64(rec["ProdLocID"].ToString());
                    int result = productClient.UpdateProdLocation(Location, profile.DBConnection._constr);
                }
                else
                {
                    productClient.InsertProductLocation(Location, profile.DBConnection._constr);
                    long Res = productClient.InsertOpBlInProductStock(ProdStock, profile.DBConnection._constr);
                }
                productClient.Close();
            }
            catch (System.Exception ex)
            {
                productClient.Close();
                Login.Profile.ErrorHandling(ex, thispage, "LocationInfo", "PMSaveLocation");
            }
        }
    }
}