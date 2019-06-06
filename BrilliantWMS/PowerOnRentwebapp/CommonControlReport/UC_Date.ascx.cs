using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrilliantWMS.CommonControls
{
    public partial class UC_Date : System.Web.UI.UserControl
    {
        public Page ParentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentdate = DateTime.Today.ToString("dd-MMM-yyyy");
            //txtDate.Text = currentdate.ToString();
        }
        public DateTime? Date
        {
            get
            {
                if (txtRDate.Text != "")
                {
                    DateTime date = DateTime.ParseExact(txtRDate.Text, "dd-MMM-yyyy", null);
                    return date;
                }
                return null;
            }
            set
            {
                if (value == null) { txtRDate.Text = ""; }
                else { txtDate.Text = value.Value.ToString("dd-MMM-yyyy"); }
            }

        }

        public String strDate
        {
            get
            {
                if (txtRDate.Text != "")
                {
                    return txtDate.Text;
                }
                return null;
            }
            set
            {
                if (value == null) { txtRDate.Text = ""; }
                else { txtDate.Text = Convert.ToDateTime(value).ToString("dd-MMM-yyyy"); }
            }

        }

        public void startdate(DateTime sdate)
        {
            txtRDate_CalendarExtender.StartDate = sdate;

        }

        public void enddate(DateTime edate)
        {
            txtRDate_CalendarExtender.EndDate = edate;
        }

        public void DateIsRequired(Boolean IsRequired, string validationGroup = "Save", string validationMessage = "Select Date")
        {
            try
            {
                RFVRDate.Visible = IsRequired;
                if (IsRequired == true)
                {
                    txtRDate.ValidationGroup = validationGroup;
                    RFVRDate.ValidationGroup = validationGroup;
                    RFVRDate.ErrorMessage = validationMessage;
                    txtRDate.AccessKey = "1";
                }
                else
                {
                    txtRDate.AccessKey = "";
                }

            }
            catch (System.Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, ParentPage, "UC_Date", "DateIsRequired");
            }
            finally
            {
            }
        }


    }
}
