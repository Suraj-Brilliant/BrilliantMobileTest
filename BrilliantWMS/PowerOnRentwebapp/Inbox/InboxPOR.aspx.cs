using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.InboxService;
using System.Web.Services;
using BrilliantWMS.Approval;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Data;
using BrilliantWMS.UserCreationService;

namespace BrilliantWMS.Inbox
{
    public partial class InboxPOR : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        static Page staticThispage;
        static int ShowMsg = 0;
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; } }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iInboxClient InboxService = new iInboxClient();
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();



            if (hndLinkValue.Value == "") hndLinkValue.Value = "All";

            if (profile.Personal.UserType == "Driver")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('You are not allow to access web system...!!!','Error','../Login/Login.aspx')", true);
            }
            else
            {
                //GVInbox.DataSource = "";
                //GVInbox.DataBind();

                DataSet dsInbox = new DataSet();
                if (profile.Personal.UserType == "User")
                {
                    dsInbox = InboxService.GetUserInbox(profile.Personal.UserID, profile.DBConnection._constr);
                    if (dsInbox.Tables[0].Rows.Count > 0)
                    {
                        GVInbox.DataSource = dsInbox;
                    }
                }
                else
                {
                    dsInbox = InboxService.GetInbox(profile.Personal.UserID, profile.DBConnection._constr);
                    if (dsInbox.Tables[0].Rows.Count > 0)
                    {
                        GVInbox.DataSource = dsInbox;
                    }
                }

                //GVInboxPOR.DataSource = InboxService.GetInboxDetailBySiteUserID(profile.Personal.UserID, hndLinkValue.Value, profile.DBConnection._constr).ToList();
                GVInbox.DataBind();

                //if (ShowMsg == 0)
                //{
                long Days = CheckPasswordAge();
                if (Days > 14) { }
                else if (Days < 14 && Days > 0)
                {
                    string msg = "Your Password Will Be Expired Within " + Days + " Days. Please Change Your Password.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('" + msg + "','Error','#')", true);
                }
                else if (Days <= 0)
                {
                    //iUserCreationClient userClient = new iUserCreationClient();
                    //long UserID = profile.Personal.UserID;
                    //string UserName = userClient.GetUserNameByID(UserID, profile.DBConnection._constr);
                    //DataSet ds = new DataSet();
                    //byte lockunlock = 1;
                    //userClient.updatelockunlock(UserName, lockunlock, profile.DBConnection._constr);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showAlert('Your Password Is Expired. Please Change The Password.','Error','../Login/ChangeLockedPassword.aspx')", true);
                }
                //    ShowMsg = 1;
                //}
            }
        }

        public long CheckPasswordAge()
        {
            long DtRemaining = 0;
            CustomProfile profile = CustomProfile.GetProfile();
            iInboxClient InboxService = new iInboxClient();
            try
            {
                long UserID = profile.Personal.UserID;

                DateTime PasswordChngDays = InboxService.GetLastPasswordChangeDate(UserID, profile.DBConnection._constr);

                DateTime CrntDt = DateTime.Now;

                System.TimeSpan datediff = CrntDt - PasswordChngDays;

                long DtDiff =Convert.ToInt64(datediff.Days);

                 DtRemaining = 60 - DtDiff;               

            }
            catch { }
            finally { }
            return DtRemaining;
        }

        protected void GVInbox_OnRebind(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iInboxClient InboxService = new iInboxClient();
            try
            {
                var SelectedValue = hndLinkValue.Value;
                if (profile.Personal.UserType == "User")
                {
                    if (SelectedValue == "")
                    {
                        GVInbox.DataSource = InboxService.GetUserInbox(profile.Personal.UserID, profile.DBConnection._constr);
                    }
                    else
                    {
                        GVInbox.DataSource = InboxService.GetUserInboxWhere(profile.Personal.UserID, SelectedValue, profile.DBConnection._constr);
                    }
                }
                else
                {
                    if (SelectedValue == "")
                    {
                        GVInbox.DataSource = InboxService.GetInbox(profile.Personal.UserID, profile.DBConnection._constr);
                    }
                    else
                    {
                        GVInbox.DataSource = InboxService.GetInboxWhere(profile.Personal.UserID, SelectedValue, profile.DBConnection._constr);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "inbox", "GVInbox_OnRebind");

            }
            finally
            {
                InboxService.Close();
            }
            GVInbox.DataBind();
        }

        protected void imgBtnView_OnClick(object sender, ImageClickEventArgs e)
        {
            ImageButton imgbtn = (ImageButton)sender;

            var CorID = imgbtn.ToolTip.ToString();

            Session["CORID"] = CorID.ToString();
        }

        [WebMethod]
        public static string WMSetArchive(string SelectedRec)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iInboxClient InboxService = new iInboxClient();
            try
            {
                InboxService.SetArchive(SelectedRec, profile.DBConnection._constr);

            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Inbox", "WMSetArchive");
            }
            finally { InboxService.Close(); }

            return "true";
        }

        [WebMethod]
        public static string wmUpdateApproval(string Status, string Remark, string tApprovalIDs)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UC_Approval uc = new UC_Approval();
                return uc.FinalUpdateApproval(Status, Remark, tApprovalIDs, profile.Personal.UserID);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, staticThispage, "Inbox", "wmUpdateApproval");
                return "";
            }

        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                //  lblTodaysMsg.Text = rm.GetString("TodayMsg", ci);
                lblSystemGeneratedMessage.Text = rm.GetString("SystemGeneratedMessage", ci);

                // lblYestardayMsg.Text = rm.GetString("YestMsg", ci);
                lblCorrespondanceMessage.Text = rm.GetString("CorrespondanceMessage", ci);

                lblAllMsg.Text = rm.GetString("AllMessages", ci);
                lblArchiveMsg.Text = rm.GetString("ArchiveMessages", ci);
                lblInbox.Text = rm.GetString("Inbox", ci);
                lblInbox1.Text = rm.GetString("lblInbox1", ci);
                btnArchive.Value = rm.GetString("Archive", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, staticThispage, "Inbox", "loadstring");
            }
        }
    }
}