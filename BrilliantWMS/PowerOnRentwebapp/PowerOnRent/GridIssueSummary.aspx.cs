using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartIssue;
namespace BrilliantWMS.PowerOnRent
{
    public partial class GridIssueSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["FillBy"] != null)
            {
                FillGVRequest(Request.QueryString["FillBy"].ToString());
            }
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void FillGVRequest(string FillBy)
        {
            iPartIssueClient objServie = new iPartIssueClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVIssue.DataSource = null;
                GVIssue.DataBind();
                if (FillBy == "UserID")
                {
                    GVIssue.DataSource = objServie.GetIssueSummayByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                }
                else if (FillBy == "SiteID")
                {
                    GVIssue.DataSource = objServie.GetIssueSummayBySiteIDs(Session["SiteIDs"].ToString(), profile.DBConnection._constr);
                }
                else if (FillBy == "RequestID")
                {
                    GVIssue.DataSource = objServie.GetIssueSummayByRequestID(Convert.ToInt64(Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                }
                GVIssue.DataBind();
            }
            catch { }
            finally { objServie.Close(); }
        }

    }
}