using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.POR
{
    public partial class ImportFSo : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            //UCFormHeader1.FormHeaderText = "Import Images";
           
                if (Session["Lang"] == "")
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();
           
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblHeading.Text = rm.GetString("ImportImages", ci);
            lblstep1.Text = rm.GetString("UploadFile", ci);
            lblstep2.Text = rm.GetString("validaton", ci);
            lblstep3.Text = rm.GetString("Finished", ci);
            btnCancle.Value = rm.GetString("Finish", ci);
            UCFormHeader1.FormHeaderText = rm.GetString("ImportImages", ci);
            lblSuccess.Text = rm.GetString("Imageimportsuccessful", ci);

        }
    }
}