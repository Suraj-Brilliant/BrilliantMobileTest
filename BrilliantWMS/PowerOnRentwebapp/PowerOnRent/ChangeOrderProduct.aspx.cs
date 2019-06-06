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
using BrilliantWMS.PORServicePartRequest;
using System.Data;
using System.Data.SqlClient;

namespace BrilliantWMS.PowerOnRent
{
    public partial class ChangeOrderProduct : System.Web.UI.Page
    {
        static string ObjectName = "RequestPartDetail";
        ResourceManager rm;
        CultureInfo ci;
        static long OrderID,UserID;
        static int Sequence;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            CustomProfile profile = CustomProfile.GetProfile();
            OrderID = long.Parse(Session["PORRequestID"].ToString());
            Sequence = int.Parse(Session["SEQ"].ToString());
             UserID = profile.Personal.UserID;

             BindProductDetails(OrderID,Sequence,UserID);
        }

        public void BindProductDetails(long OrderID,int Sequence,long UserID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            int IsPriceEdit = objService.IsPriceEditYN(OrderID, UserID, profile.DBConnection._constr);
            int IsSkuChange = objService.IsSkuChangeYN(OrderID, UserID, profile.DBConnection._constr);
            if (IsPriceEdit == 1) { txtPrc.Disabled = false; } else { txtPrc.Disabled = true; }
            if (IsSkuChange == 1) { txtReqQty.Disabled = false;  } else { txtReqQty.Disabled = true; }

            DataSet ds = new DataSet();
            ds = objService.GetProductOfOrder(OrderID, Sequence,profile.DBConnection._constr);
            txtProductCode.Text = ds.Tables[0].Rows[0]["Prod_Code"].ToString();
            txtProductName.Text = ds.Tables[0].Rows[0]["Prod_Name"].ToString();
            txtProductDescription.Text = ds.Tables[0].Rows[0]["Prod_Description"].ToString();
            txtMOQ.Text = ds.Tables[0].Rows[0]["moq"].ToString();
            txtCurrentStock.Text = ds.Tables[0].Rows[0]["AvailableBalance"].ToString();
            txtReserveStock.Text = ds.Tables[0].Rows[0]["ResurveQty"].ToString();
         //   txtReqQty.Value = ds.Tables[0].Rows[0]["OrderQty"].ToString();
            txtOrderQty.Text = ds.Tables[0].Rows[0]["OrderQty"].ToString();
            txtPrc.Value = ds.Tables[0].Rows[0]["Price"].ToString();
            //txtPrice.Text = ds.Tables[0].Rows[0]["Price"].ToString();
            txtTotal.Text = ds.Tables[0].Rows[0]["Total"].ToString();
            int ProdID = int.Parse(ds.Tables[0].Rows[0]["SkuId"].ToString()); hdnSelectedProduct.Value = ProdID.ToString();
            DataSet dsUOM = new DataSet();
            dsUOM = objService.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);
            ddlUOM.DataSource = dsUOM;
            ddlUOM.DataBind();
            //ddlUOM.SelectedValue =ds.Tables[0].Rows[0]["UOMID"].ToString();
            ddlUOM.SelectedIndex = ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(ds.Tables[0].Rows[0]["UOMID"].ToString()));
            DataSet dsUOMSelPrd = new DataSet();
            dsUOMSelPrd = objService.GetUOMofSelectedProduct(ProdID, profile.DBConnection._constr);
            int SelInd = 0;
            SelInd = ddlUOM.SelectedIndex;
            decimal SelectedQty = decimal.Parse(dsUOMSelPrd.Tables[0].Rows[SelInd]["Quantity"].ToString());
            decimal UserQty = decimal.Parse(ds.Tables[0].Rows[0]["OrderQty"].ToString());
            txtReqQty.Value = Convert.ToString(UserQty / SelectedQty);

        }

        [WebMethod]
        public static long WMGetQty(long SelectedProduct, long SelectedUOM)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            DataSet dsUOM = new DataSet();
            dsUOM = objService.GetQtyofSelectedUOM(SelectedProduct, SelectedUOM,profile.DBConnection._constr);
            long Qty = long.Parse(dsUOM.Tables[0].Rows[0]["Quantity"].ToString());
            return Qty;
        }

        [WebMethod]
        public static int WMUpdateOrderProductDetails(decimal OrderQty, decimal Price, decimal Total)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            int Result = objService.UpdateOrderQtyTotal(OrderQty, Price, Total, OrderID, Sequence, profile.DBConnection._constr);
            return Result;
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblProductCode.Text = rm.GetString("ProductCode", ci);
                lblProductName.Text = rm.GetString("ProductName", ci);
                lblProdDescription.Text = rm.GetString("ProductDescription", ci);
                lblMOQ.Text = rm.GetString("MOQ", ci);
                lblCurrentStock.Text = rm.GetString("CurrentStock", ci);
                lblReserveQty.Text = rm.GetString("ReserveQty", ci);
                lblRequestQty.Text = rm.GetString("RequestQty", ci);
                lblUOM.Text = rm.GetString("uom", ci);
                lblOrderQty.Text = rm.GetString("OrderQty", ci);
                lblPrice.Text = rm.GetString("Price", ci);
                lblTotal.Text = rm.GetString("Total", ci);
                btnSubmit.Value = rm.GetString("Submit", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "loadstring");
            }
        }
    }
}