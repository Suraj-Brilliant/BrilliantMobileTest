using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BrilliantWMS.UCProductSearchService;
using BrilliantWMS.Login;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;


namespace BrilliantWMS.Product
{
    public partial class UCProductSearch : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        public Page ParentPage { get; set; }
        BrilliantWMS.UCProductSearchService.iUCProductSearchClient productSearchService = new BrilliantWMS.UCProductSearchService.iUCProductSearchClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Lang"] == null)
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
             loadstring();
            }
            //try
            //{
            //    CustomProfile profile = CustomProfile.GetProfile();
            //    GridProductSearch.DataSource = productSearchService.GetProductList(profile.DBConnection._constr);
            //    GridProductSearch.DataBind();
            //}
            //catch (System.Exception ex)
            //{
            //    Login.Profile.ErrorHandling(ex, ParentPage, "UCProductSearch", "Page_Load");
            //}

        }

        //protected void RebindGrid(object sender, EventArgs e)
        //{
        //}

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
            btnProductSearch.Value = rm.GetString("AddItemsToList", ci);
        }
    }
}