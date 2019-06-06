using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BrilliantWMS.CommonControls
{
    public partial class UCFormHeader : System.Web.UI.UserControl
    {
        public string FormHeaderText;
        public string ObjectName;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblFormHeader.Text = FormHeaderText;
            //ImgFormHeader.ImageUrl = "" ;
        }
    }
}