using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.PORDashboardService;
using BrilliantWMS.Login;

namespace BrilliantWMS.DashBoard
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetDashboardByUserID();
        }

        protected void GetDashboardByUserID()
        {
            iDashboardClient objService = new iDashboardClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                List<POR_Dashboard_UserWise> DashboardList = new List<POR_Dashboard_UserWise>();
                DashboardList = objService.GetDashboardsByUserID(profile.Personal.UserID, profile.DBConnection._constr).ToList();

                string str = "";
                int column = 1;
                foreach (POR_Dashboard_UserWise dashB in DashboardList)
                {
                    switch (column)
                    {
                        case 1:
                            str = str + "<tr><td>" +
                                        "<iframe src='../Dashboard/BindDashboard.aspx?ID=" + dashB.ID.ToString() + "' style='border: none; height: 100%; width: 100%'></iframe>" +
                                        "</td>";
                            column += 1;
                            break;
                        case 2:
                            str = str + "<td>" +
                                        "<iframe src='../Dashboard/BindDashboard.aspx?ID=" + dashB.ID.ToString() + "' style='border: none; height: 100%; width: 100%'></iframe>" +
                                        "</td>";
                            column += 1;
                            break;
                        case 3:
                            str = str + "<td>" +
                                        "<iframe src='../Dashboard/BindDashboard.aspx?ID=" + dashB.ID.ToString() + "' style='border: none; height: 100%; width: 100%'></iframe>" +
                                        "</td></tr>";
                            column = 1;
                            break;
                    }
                }

                if (str.EndsWith("</tr>") == false)
                {
                    str = str + "</tr>";
                }

                tableLeft.InnerHtml = str;

            }
            catch { }
            finally { objService.Close(); }
        }
    }
}