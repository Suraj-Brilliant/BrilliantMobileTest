using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartReceipts;
namespace BrilliantWMS.PowerOnRent
{
    public partial class GridReceiptSummary : System.Web.UI.Page
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
            iPartReceiptClient objServie = new iPartReceiptClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVReceipt.DataSource = null;
                GVReceipt.DataBind();
                if (FillBy == "UserID")
                {
                    GVReceipt.DataSource = objServie.GetReceiptSummaryByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                }
                else if (FillBy == "SiteID")
                {
                    GVReceipt.DataSource = objServie.GetReceiptSummaryBySiteIDs(Session["SiteIDs"].ToString(), profile.DBConnection._constr);
                }
                else if (FillBy == "RequestID")
                {
                    GVReceipt.DataSource = objServie.GetReceiptSummaryByRequestID(Convert.ToInt64(Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                }
                GVReceipt.DataBind();
            }
            catch { }
            finally { objServie.Close(); }
        }

    }
}