using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;

namespace BrilliantWMS.Company
{
    public partial class TermsConditionSearch : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid(Request.QueryString["object"]);    
        }

        public void BindGrid(string groupName)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                /*TermsConditionMasterService.iTermConditionMasterClient TermsClient = new TermsConditionMasterService.iTermConditionMasterClient();
                GridTermConditionSearch.DataSource = TermsClient.GetTermRecordToBindGridUC(groupName, profile.DBConnection._constr);
                GridTermConditionSearch.DataBind();
                TermsClient.Close();*/

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "TermsConditionSearch", "BindGrid");
            }
            finally
            {
            }
        }

        protected void RebindGridTCS(object sender, EventArgs e)
        { }
    }
}