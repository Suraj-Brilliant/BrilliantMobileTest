using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.Login;

namespace BrilliantWMS.PowerOnRent
{
    public partial class Correspondance : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        string RequestID = "";
        string CORID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PORRequestID"] != null)
            {
                RequestID = Session["PORRequestID"].ToString();
            }
            if (Session["POID"] != null)
            {
                RequestID = Session["POID"].ToString();
            }
            if (Session["GRNID"] != null)
            {
                RequestID = Session["GRNID"].ToString();
            }
            if (Session["SOID"] != null)
            {
                RequestID = Session["SOID"].ToString();
            }
            CORID = Request.QueryString["VW"].ToString();

            if (CORID != "" )
            {
                //if (Session["CORID"] != null)
                //{
                //    if (Session["CORID"].ToString() != null)
                //    {
                       // CORID = Session["CORID"].ToString();
                CORID = Request.QueryString["VW"].ToString();
                ShowData(CORID);
                //    }
                //}
            }

            //fillDropdown();
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];

            }
             loadstring();

        }

        public void ShowData(string CORID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            tCorrespond Cor = new tCorrespond();

            Cor = objService.GetCorrespondanceDetail(long.Parse(CORID.ToString()), profile.DBConnection._constr);

            txtSubject.Text = Cor.MessageTitle;
            hdnmsgbody.Value = Cor.Message.ToString();
            lblAddHTMLQuestionInRichBox.Visible = true;

            txtSubject.Enabled = false;
            // editEmail.Enabled = false;
            btnSubmit.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();

            tCorrespond Cor = new tCorrespond();

            string MessageBody = hdnmsgbody.Value;
            string MsgTitle = txtSubject.Text;
            if (MsgTitle == "")
            {
                Response.Write("<script>");
                Response.Write("showAlert('Enter Subject ', 'Error', '#');");
                txtSubject.Focus();
                Response.Write("</script>");                
            }
            else if (MessageBody == "" || MessageBody == "andBrSt;brandBrEn;\r\n")
            {
                Response.Write("<script>");
                Response.Write("showAlert('Enter Message Body ', 'Error', '#');");                
                Response.Write("</script>");  
            }
            else
            {
                long OrderHeadId = long.Parse(RequestID.ToString());

                Cor.date = DateTime.Now;
                Cor.Message = MessageBody;
                Cor.MessageFrom = profile.Personal.UserID;
                Cor.MessageTitle = MsgTitle;
                Cor.OrderHeadId = OrderHeadId;
                Cor.Archive = false;
                Cor.MailStatus = 1;

                objService.InsertIntotCorrespond(Cor, profile.DBConnection._constr);

                Response.Write("<script>");
                Response.Write("window.opener.GVInboxPOR.refresh();");
                Response.Write("self.close();");
                Response.Write("</script>");
            }
        }

        [WebMethod]
        public static string WMAddMessage(string hdnmsgbody, string Subject)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();

            tCorrespond Cor = new tCorrespond();

            string MessageBody = hdnmsgbody;
            long OrderHeadId = Convert.ToInt64(HttpContext.Current.Session["PORRequestID"].ToString());// long.Parse(RequestID.ToString());

            Cor.date = DateTime.Now;
            Cor.Message = MessageBody;
            Cor.MessageFrom = profile.Personal.UserID;
            Cor.MessageTitle = Subject;
            Cor.OrderHeadId = OrderHeadId;

            objService.InsertIntotCorrespond(Cor, profile.DBConnection._constr);

            return "true";
        }

        //public void fillDropdown()
        //{
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iPartRequestClient objService = new iPartRequestClient();

        //    lstCorrespondance.DataSource = null;
        //    lstCorrespondance.DataBind();
        //    objService.GetUserMailList(profile.Personal.UserID,profile.DBConnection._constr);
        //}

        //protected void Submit(object sender, EventArgs e)
        //{//http://www.aspsnippets.com/Articles/Multiple-Select-MultiSelect-DropDownList-with-CheckBoxes-in-ASPNet-using-jQuery.aspx
        //    string message = "";
        //    foreach (ListItem item in lstCorrespondance.Items)
        //    {
        //        if (item.Selected)
        //        {
        //            message += item.Text + " " + item.Value + "\\n";
        //        }
        //    }
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + message + "');", true);
        //}

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblSubject.Text = rm.GetString("Subject", ci);
            //lblTo.Text = rm.GetString("To", ci);
            //Button2.Text = rm.GetString("Submit", ci);
            btnSubmit.Text = rm.GetString("Submit", ci);
            lblBody.Text = rm.GetString("Body", ci);

        }

    }
}