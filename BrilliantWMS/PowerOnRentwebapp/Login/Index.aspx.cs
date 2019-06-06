using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace BrilliantWMS.Login
{
    public partial class Index : System.Web.UI.Page
    {
        string host;
        protected void Page_Load(object sender, EventArgs e)
        {
            host = HttpContext.Current.Request.Url.Host;
            setConfigurationSettings(host);
        }

        protected void setConfigurationSettings(string hostname)
        {
            
            Configuration myWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (hostname == "localhost")
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                string strDevConnection = @"Data Source=SERVER\BISPLSERVER; Initial Catalog=BISPL_CRMDB; User ID=sa; Password='Password123#'";
                myWebConfig.ConnectionStrings.ConnectionStrings["ApplicationServices"].ConnectionString = strDevConnection;  //DBConnectionString is the name of the current connectionstring in the web.config

            }
            else
            {
                
                string strLiveConnection = @"Data Source=elegantcrm.db.9297019.hostedresource.com; Initial Catalog=elegantcrm; User ID=elegantcrm; Password='Password123#'";
                myWebConfig.ConnectionStrings.ConnectionStrings["ApplicationServices"].ConnectionString = strLiveConnection;
            }
            myWebConfig.Save();
        }
    }
}