using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrilliantWMS
{
    public partial class Image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ProfileImg"] != null)
            {
                Response.BinaryWrite((byte[])Session["ProfileImg"]);
            }
            else if (Session["PrdImg"] != null)
            {
                Response.BinaryWrite((byte[])Session["PrdImg"]);
            }
        }
    }
}