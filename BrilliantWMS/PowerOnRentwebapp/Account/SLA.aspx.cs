using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using BrilliantWMS.Login;
using BrilliantWMS.AccountSearchService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace BrilliantWMS.Account
{
    public partial class SLA : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            string DptID=Request.QueryString["DEPTID"].ToString();
            hdndeptid.Value = Request.QueryString["DEPTID"].ToString();
            if (!IsPostBack)
            {
                if (DptID != "0")
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    BrilliantWMS.AccountSearchService.iCustomerClient cust = new iCustomerClient();
                    mSLA sla = new mSLA();
                    sla = cust.GetSLADetailsDeptWise(long.Parse(DptID), profile.DBConnection._constr);
                    if (sla != null)
                    {
                        if (sla.PrimeDays == 0) { chkPrime.Checked = false; } else { chkPrime.Checked = true; txtPrime.Text = sla.PrimeDays.ToString(); txtPrime.Enabled = true; }
                        if (sla.ExpressDays == 0) { chkExpress.Checked = false; } else { chkExpress.Checked = true; txtExpress.Text = sla.ExpressDays.ToString(); txtExpress.Enabled = true; }
                        if (sla.RegularDays == 0) { chkRegular.Checked = false; } else { chkRegular.Checked = true; txtRegular.Text = sla.RegularDays.ToString(); txtRegular.Enabled = true; }
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string Prime ="", Express="",Regular="",DID="";
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.AccountSearchService.iCustomerClient cust = new iCustomerClient();
            mSLA sla = new mSLA();

            if (txtPrime.Text == "NA") { Prime = "0"; } else { Prime = txtPrime.Text; }
            if (txtExpress.Text == "NA") { Express = "0"; } else { Express = txtExpress.Text; }
            if (txtRegular.Text == "NA") { Regular = "0"; } else { Regular = txtRegular.Text; }
            if (hdndeptid.Value == "NA") { DID = "0"; } else { DID = hdndeptid.Value; }
            sla.PrimeDays = long.Parse(Prime);
            sla.ExpressDays = long.Parse(Express);
            sla.RegularDays = long.Parse(Regular);
            sla.DeptID = long.Parse(DID);
            sla.CreatedBy = profile.Personal.UserID;
            sla.CreatedDate = DateTime.Now;
            cust.AddIntomSLA(sla, profile.DBConnection._constr);
            Response.Write("<script>window.close();</" + "script>");
            Response.End();
        }

        [WebMethod]
        public static int WMAddIntomSLA(string Prime, string Express,string  Regular,string DID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.AccountSearchService.iCustomerClient cust = new iCustomerClient();
            mSLA sla = new mSLA();
            sla.DeptID = long.Parse(DID);
            long Pr = 0, Exp = 0, Reg = 0;
            if (Prime == "NA") { Pr = 0; } else { Pr = Convert.ToInt64(Prime); }
            if (Express == "NA") { Exp = 0; } else { Exp = Convert.ToInt64(Express); }
            if (Regular == "NA") { Reg = 0; } else { Reg = Convert.ToInt64(Regular); }

            sla.PrimeDays = Pr;
            sla.ExpressDays = Exp;
            sla.RegularDays = Reg;

            sla.CreatedBy = profile.Personal.UserID;
            sla.CreatedDate = DateTime.Now;

            cust.AddIntomSLA(sla, profile.DBConnection._constr);
            return 1;
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblheasertext.Text = rm.GetString("ServiceLevelAgreement", ci);
                lblDeliveryMode.Text = rm.GetString("DeliveryMode", ci);
                lblPrime.Text = rm.GetString("Prime", ci);
                lblExpress.Text = rm.GetString("Express", ci);
                lblRegular.Text = rm.GetString("Regular", ci);
                lblSLADays.Text = rm.GetString("SLAHours", ci);
                btnSubmit.Text = rm.GetString("Submit", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SLA", "loadstring");
            }
        }
    }
}