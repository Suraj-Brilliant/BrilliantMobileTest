using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.ToolbarService;
using System.Web.Services;

namespace BrilliantWMS.PowerOnRent
{
    public partial class GridRequestSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["FillBy"] != null)
            {
                //FillGVRequest(Request.QueryString["FillBy"].ToString());
                FillGVRequest(Request.QueryString["FillBy"].ToString(),Request.QueryString["Invoker"].ToString());
            }
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void GVRequest_OnRebind(object sender, EventArgs e)
        {
            FillGVRequest(Request.QueryString["FillBy"].ToString(), Request.QueryString["Invoker"].ToString());
        }

        protected void FillGVRequest(string FillBy,string Invoker)
        {
            iPartRequestClient objServie = new iPartRequestClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                GVRequest.DataSource = null;
                GVRequest.DataBind();
                //New Added By Suresh for GWC

                string UserType = profile.Personal.UserType.ToString();

                //New Added By Suresh for GWC
                if (FillBy == "UserID")
                {
                    if (Invoker == "Request")
                    {
                        if (UserType == "User" || UserType == "Requester" || UserType == "Requestor")
                        {
                            GVRequest.DataSource = objServie.GetRequestSummayOfUser(profile.Personal.UserID, profile.DBConnection._constr);
                        }
                        else
                        {
                            GVRequest.DataSource = objServie.GetRequestSummayByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                        }
                        GVRequest.Columns[12].Visible = false; GVRequest.AllowMultiRecordSelection = true; GVRequest.AllowRecordSelection = true;
                    }
                    else if (Invoker == "Issue")
                    {
                        if (UserType == "User" || UserType == "Requester" || UserType == "Requestor")
                        {
                            GVRequest.DataSource = objServie.GetRequestSummayOfUserIssue(profile.Personal.UserID, profile.DBConnection._constr);
                        }
                        else
                        {
                            GVRequest.DataSource = objServie.GetRequestSummayByUserIDIssue(profile.Personal.UserID, profile.DBConnection._constr);                   
                        }
                        GVRequest.Columns[12].Visible = true; GVRequest.AllowMultiRecordSelection = true; GVRequest.AllowRecordSelection = true;
                    }
                }
                else if (FillBy == "SiteIDs")
                {
                    GVRequest.DataSource = objServie.GetRequestSummayBySiteIDs(Session["SiteIDs"].ToString(), profile.DBConnection._constr);
                }
                GVRequest.DataBind();
            }
            catch { }
            finally { objServie.Close(); }
        }

    }
}