using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Threading;
using System.Reflection;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Web.Services;
using BrilliantWMS.Login;

namespace BrilliantWMS.Product
{
    public partial class VirtualQty : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            long skuid = long.Parse(Request.QueryString["skuid"].ToString());
            hdnskuid.Value = skuid.ToString();

        }

        [WebMethod]
        public static void SaveVirtualQty(object objReq)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataTable dt = new DataTable();
            decimal availbalance = 0m, virtualQty = 0m, AvailVirtyalQty = 0m;
            long storeID = 0;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
              
                long Quantity = long.Parse(dictionary["quantity"].ToString());
                long hdnskuid = long.Parse(dictionary["hdnskuid"].ToString());
                DataSet ds = productClient.GetAvailQuantity(hdnskuid, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    storeID = long.Parse(dt.Rows[0]["SiteID"].ToString());
                    availbalance = decimal.Parse(dt.Rows[0]["AvailableBalance"].ToString());
                    virtualQty = decimal.Parse(dt.Rows[0]["VirtualQty"].ToString());
                    AvailVirtyalQty = decimal.Parse(dt.Rows[0]["AvailVirtualQty"].ToString());
                }
                if (virtualQty == 0)
                {
                    AvailVirtyalQty = availbalance + Quantity;
                    virtualQty = Quantity; 
                }
                else
                {
                    AvailVirtyalQty = availbalance + virtualQty + Quantity;
                    virtualQty = virtualQty + Quantity;
                }
                productClient.UpdateVirtualBalance(hdnskuid,virtualQty, AvailVirtyalQty, profile.DBConnection._constr);
                productClient.InsertIntoInventry(hdnskuid, storeID, DateTime.Now, Quantity, profile.DBConnection._constr);

            }
            catch (System.Exception ex)
            {
                productClient.Close();
            }
            finally
            {
                productClient.Close();
            }

        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblContactPersonFormHeader.Text = rm.GetString("AddVirtualQuantity", ci);
                btnAddressSubmit.Value = rm.GetString("Submit", ci);
                btnAddressClear.Value = rm.GetString("Clear", ci);
                lblVirtualQuantity.Text = rm.GetString("EnterVirtualQuantity", ci);
                Button1.Value = rm.GetString("Submit", ci);
                Button2.Value = rm.GetString("Clear", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "VirtualQty", "loadstring");
            }
        }
    }
}