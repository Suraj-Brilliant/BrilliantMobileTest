using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using BrilliantWMS.Login;

namespace BrilliantWMS.WMS
{
    public partial class ReportWMS : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            usrID.Value = profile.Personal.UserID.ToString();
        }
        [System.Web.Services.WebMethod]
        public static ListDetails[] GetFilterData(string CtlID, string FilterId, string ReportId)
        {
            DataSet dt = new DataSet();
            List<ListDetails> details = new List<ListDetails>();
            dt=BrilliantWMS.CommonControlReport.UC_RptFilter.GetFilterData(CtlID, FilterId, ReportId, "", "");
            foreach (DataRow dtrow in dt.Tables[0].Rows)
            {
                ListDetails ListDtls = new ListDetails();
                ListDtls.Id = Convert.ToInt32(dtrow["Id"].ToString());
                ListDtls.Name = dtrow["Name"].ToString();
                details.Add(ListDtls);
            }
            return details.ToArray();
                
        }



        public class ListDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }
    }
}