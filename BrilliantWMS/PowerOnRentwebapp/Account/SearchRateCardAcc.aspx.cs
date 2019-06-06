using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using BrilliantWMS.Login;
using BrilliantWMS.ProductMasterService;
using System.Web.Services;
using WebMsgBox;
using System.Configuration;

namespace BrilliantWMS.Account
{
    public partial class SearchRateCardAcc : System.Web.UI.Page
    {
        static string sessionID;
        static string TargetObject;
        static string Sequence;
        static Page thispage;
        long CustomerID = 0;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            sessionID = Session.SessionID;
            thispage = this;
            if (Request.QueryString["CompanyID"] != null) hdncompanyId.Value = Request.QueryString["CompanyID"].ToString();
            if (Request.QueryString["AccountType"] != null) hdnacounttype.Value = Request.QueryString["AccountType"].ToString();
            RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst(GVVendor.CurrentPageIndex, hdnFilterText.Value);
                GVVendor.DataSource = ds;
                GVVendor.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SearchrateCardAcc.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst(int pageIndex, string filter)
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
               
                if (hdnacounttype.Value == "Vendor")
                {
                    str = "select * from V_WMS_GetVendorDetails where CompanyID = '" + long.Parse(hdncompanyId.Value) + "' and Active = 'Yes'";
                }
                else
                {
                    str = "select * from V_WMS_GetClientDetails where CompanyID = '" + long.Parse(hdncompanyId.Value) + "' and Active = 'Yes'";
                }
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                if (hdnacounttype.Value == "Vendor")
                {
                    str = "select * from V_WMS_GetVendorDetails where CompanyID = '" + long.Parse(hdncompanyId.Value) + "' and Active = 'Yes' and  (Name like '%" + filter + "%' or  Customer like '%" + filter + "%')";
                }
                else
                {
                    str = "select * from V_WMS_GetClientDetails where CompanyID = '" + long.Parse(hdncompanyId.Value) + "' and Active = 'Yes' and (Name like '%" + filter + "%' or  Customer like '%" + filter + "%')";
                }
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        //[WebMethod]
        //public static void PMSaveLocation(string selectedIds, long SkuId)
        //{
        //    iProductMasterClient productVen = new iProductMasterClient();
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    mProductVendor prodvend = new mProductVendor();

        //    string ids = selectedIds.ToString();
        //    string[] words = ids.Split(',');
        //    for (int i = 1; i < words.Length; i++)
        //    {
        //        long vendorID = long.Parse(words[i]);
        //        prodvend.SKUID = SkuId;
        //        prodvend.VendorID = long.Parse(words[i]);
        //        long count = productVen.GetProdVendCount(SkuId, vendorID, profile.DBConnection._constr);
        //        if (count <= 0)
        //        {
        //            productVen.InsertProductVendor(prodvend, profile.DBConnection._constr);
        //        }
        //    }

        //}
    }
}