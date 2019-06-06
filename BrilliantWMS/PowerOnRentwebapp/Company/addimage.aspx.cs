using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace BrilliantWMS.Company
{
    public partial class addimage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            
            string filePath = FileUpload1.PostedFile.FileName;

            string filename = Path.GetFileName(filePath);

            string ext = Path.GetExtension(filename);

            string contenttype = String.Empty;
            Stream fs = FileUpload1.PostedFile.InputStream;

            BinaryReader br = new BinaryReader(fs);

            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            SqlConnection conn=new SqlConnection(@"Data Source=SERVER\SQLEXPRESS;Initial Catalog=BWMS;User ID=sa;Password='Password123#'");
            SqlCommand cmd=new SqlCommand("Update mCompany set Logo='"+bytes+"' where ID=" + txtid.Text + " " ,conn);
            SqlDataAdapter da=new SqlDataAdapter();
            DataSet ds=new DataSet();
            da.SelectCommand=cmd;
            da.Fill(ds,"tbl");
            Response.Write("updated sucessfully");
            
        }
    }
}