using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrilliantWMS
{
    public partial class Image2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ProfileImgStamp"] != null)
            {
                Response.BinaryWrite((byte[])Session["ProfileImgStamp"]);
            }
        }
    }
}