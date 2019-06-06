using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerOnRentwebapp.Login;
using BrilliantWMS.PORServicePartConsumption;
namespace PowerOnRentwebapp.PowerOnRent
{
    public partial class GridConsumptionSummary : System.Web.UI.Page
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
            iPartConsumptionClient objServie = new iPartConsumptionClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVConsumption.DataSource = null;
                GVConsumption.DataBind();
                if (FillBy == "UserID")
                {
                    GVConsumption.DataSource = objServie.GetConsumptionSummayByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                }
                else if (FillBy == "SiteID")
                {
                    //GVConsumption.DataSource = objServie.GetConsumptionSummayByUserID(Session["SiteIDs"].ToString(), profile.DBConnection._constr);
                }
                else if (FillBy == "RequestID")
                {
                    //GVConsumption.DataSource = objServie.GetConsumptionSummayByUserID(Convert.ToInt64(Session["PORRequestID"].ToString()), profile.DBConnection._constr);
                }
                else if (FillBy == "ReceiptID")
                {
                    GVConsumption.DataSource = objServie.GetConsumptionSummayByReceiptIDs(Session["PORReceiptID"].ToString(), profile.DBConnection._constr);
                }
                GVConsumption.DataBind();
            }
            catch { }
            finally { objServie.Close(); }
        }

    }
}