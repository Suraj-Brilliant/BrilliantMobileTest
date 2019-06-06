using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UCProductSearchService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.Product
{
    public partial class ProductSearch : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_PreInit(Object sender, EventArgs e)
        { //CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                hndgrupByGrid.Value = GridProductSearch.GroupBy;
                RebindGrid(sender, e);
                if (Session["Lang"] == "")
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "Page_Load");
            }

        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                BrilliantWMS.UCProductSearchService.iUCProductSearchClient productSearchService = new BrilliantWMS.UCProductSearchService.iUCProductSearchClient();
                CustomProfile profile = CustomProfile.GetProfile();
                
                //List<GetProductDetail> ProductList = new List<GetProductDetail>();
                //ProductList = productSearchService.GetProductList1(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, profile.DBConnection._constr).ToList();

                List<VW_GetSKUDetailsWithPack> ProductList = new List<VW_GetSKUDetailsWithPack>();
                if (Session["DeptID"] != null)
                {
                    long DeptID = long.Parse(Session["DeptID"].ToString());
                    //if (profile.Personal.UserType == "Requester And Approver" || profile.Personal.UserType == "Requester" || profile.Personal.UserType == "User" || profile.Personal.UserType == "Admin")
                    //{
                    long UserID = profile.Personal.UserID;
                    ProductList = productSearchService.GetSKUListDeptWise(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, UserID, DeptID, profile.DBConnection._constr).ToList();
                }
                else if (Session["WarehouseID"] != null)
                {
                    long WarehouseID = long.Parse(Session["WarehouseID"].ToString());
                    ProductList = productSearchService.GetSKUListWarehouseWise(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, WarehouseID, profile.DBConnection._constr).ToList();
                }
                //}
                //else
                //{
                //    ProductList = productSearchService.GetSKUList(GridProductSearch.CurrentPageIndex, hdnFilterText.Value, DeptID, profile.DBConnection._constr).ToList();
                //}                
               
                GridProductSearch.DataSource = ProductList;
                GridProductSearch.GroupBy = hndgrupByGrid.Value;
               // if (!Page.IsPostBack) { GridProductSearch.GroupBy = "ProductType"; }
                GridProductSearch.DataBind();
                productSearchService.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
            }
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblheader.Text = rm.GetString("SKUList", ci);
         //   lblwithbom.Text = rm.GetString("WithBOM", ci);
            btnSubmitProductSearch1.Value = rm.GetString("Submit", ci);
            btnSubmitProductSearch2.Value = rm.GetString("Submit", ci);
          }

    }
}
