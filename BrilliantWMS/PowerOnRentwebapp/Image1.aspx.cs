using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrilliantWMS
{
    public partial class Image1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ProfileImgMasterPg"] != null)
            {
                Response.BinaryWrite((byte[])Session["ProfileImgMasterPg"]);
            }
        }
    }
}