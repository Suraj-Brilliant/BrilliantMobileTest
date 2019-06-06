using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace BrilliantWMS.WMS
{
    public partial class ImportFPO : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            //loadstring();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportDPO.aspx");
        }
        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblHeading.Text = rm.GetString("ImportDirectOrder", ci);
            lblstep1.Text = rm.GetString("UploadFile", ci);
            lblstep2.Text = rm.GetString("DataValidationVerification", ci);
            lblstep3.Text = rm.GetString("Finished", ci);
            Button1.Text = rm.GetString("Finish", ci);

        }
    }
}