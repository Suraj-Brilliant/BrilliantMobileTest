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
    public partial class AddPacks : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long UomPackId = 0;
        string state = "";
        string Queryid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            string val = Request.QueryString["Id"];
            if (Request.QueryString["skuid"] != null) hdnskuid.Value = Request.QueryString["skuid"].ToString();
            if (Request.QueryString["Id"] != Queryid)
            {
                UomPackId = long.Parse(Request.QueryString["Id"].ToString());
                hdnpackuomid.Value = UomPackId.ToString();
                state = "Edit";
                hdnstate.Value = state;
                GetEditdata();
            }
            else
            {
                hdnpackuomid.Value = "0";
                hdnstate.Value = "add";
                GetUOMList();
            }
            loadstring();
           
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblContactPersonFormHeader.Text = rm.GetString("AddNewPack", ci);
            btnAddressSubmit.Value = rm.GetString("Submit", ci);
            btnAddressClear.Value = rm.GetString("Clear", ci);
            lbluom1.Text = rm.GetString("uom", ci);
            lblQuantity.Text = rm.GetString("Quantity", ci);
            Button1.Value = rm.GetString("Submit", ci);
            Button2.Value = rm.GetString("Clear", ci);
            lblsequence.Text = rm.GetString("Sequence", ci);
        }

        private void GetUOMList()
        {
            DataSet ds;
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                ds = productClient.GetUOMList(profile.DBConnection._constr);
                ddluom1.DataSource = ds;
                ddluom1.DataTextField = "Description";
                ddluom1.DataValueField = "ID";
                ddluom1.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddluom1.Items.Insert(0, lst);
            }
            catch { }
            finally { productClient.Close(); }
        }

        [WebMethod]
        public static void PMSaveAddress(object objReq)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long UOMId = long.Parse(dictionary["UOMDescri"].ToString());
                long Quantity = long.Parse(dictionary["quantity"].ToString());
                long sequence = long.Parse(dictionary["sequence"].ToString());
                string Description = dictionary["Description"].ToString();
                string hdnstate = dictionary["hdnstate"].ToString();
                long hdnpackuomid = long.Parse(dictionary["hdnpackuomid"].ToString());
                long SKuId = long.Parse(dictionary["hdnskuid"].ToString());
                string UomShort = productClient.GetUomShort(UOMId,profile.DBConnection._constr);

                if (hdnstate != "Edit")
                {
                    productClient.InsertIntomPackUom(SKuId, UomShort, Description, Quantity, sequence, profile.DBConnection._constr);
                }
                else
                {
                    productClient.UpdatemPackUom(hdnpackuomid,UomShort, Description, Quantity, sequence, profile.DBConnection._constr);
                }

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

        public void GetEditdata()
        {
            DataSet ds;
            DataTable dt;
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                ds = productClient.GetEditmpackUOmdetail(UomPackId, profile.DBConnection._constr);
                 dt = ds.Tables[0];
                 if (dt.Rows.Count > 0)
                 {

                     long Id = long.Parse(dt.Rows[0]["ID"].ToString());
                     GetUOMList();
                    
                     if (ddluom1.Items.Count >= 1) ddluom1.SelectedIndex = 0;
                     ddluom1.SelectedIndex = ddluom1.Items.IndexOf(ddluom1.Items.FindByValue(dt.Rows[0]["ID"].ToString()));
                     ddluom1.SelectedItem.Value = Id.ToString();
                     ddluom1.SelectedItem.Text = dt.Rows[0]["Description"].ToString();
                     
                     //ddluom1.SelectedItem.Value = dt.Rows[0]["ID"].ToString();
                     //ddluom1.SelectedItem.Text = dt.Rows[0]["Description"].ToString();
                     txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                     txtsequence.Text = dt.Rows[0]["Sequence"].ToString();
                 }
            }
            catch{}
            finally{}
        }
          
    }
}