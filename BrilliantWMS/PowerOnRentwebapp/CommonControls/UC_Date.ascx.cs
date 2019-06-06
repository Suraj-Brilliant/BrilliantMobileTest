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
                if (txtDate.Text != "")
                {
                    DateTime date = DateTime.ParseExact(txtDate.Text, "dd-MMM-yyyy", null);
                    return date;
                }
                return null;
            }
            set
            {
                if (value == null) { txtDate.Text = ""; }
                else { txtDate.Text = value.Value.ToString("dd-MMM-yyyy"); }
            }

        }

        public String strDate
        {
            get
            {
                if (txtDate.Text != "")
                {
                    return txtDate.Text;
                }
                return null;
            }
            set
            {
                if (value == null) { txtDate.Text = ""; }
                else { txtDate.Text = Convert.ToDateTime(value).ToString("dd-MMM-yyyy"); }
            }

        }

        public void startdate(DateTime sdate)
        {
            txtDate_CalendarExtender.StartDate = sdate;

        }

        public void enddate(DateTime edate)
        {
            txtDate_CalendarExtender.EndDate = edate;
        }

        public void DateIsRequired(Boolean IsRequired, string validationGroup = "Save", string validationMessage = "Select Date")
        {
            try
            {
                RFVDate.Visible = IsRequired;
                if (IsRequired == true)
                {
                    txtDate.ValidationGroup = validationGroup;
                    RFVDate.ValidationGroup = validationGroup;
                    RFVDate.ErrorMessage = validationMessage;
                    txtDate.AccessKey = "1";
                }
                else
                {
                    txtDate.AccessKey = "";
                }

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC_Date", "DateIsRequired");
            }
            finally
            {
            }
        }


    }
}
