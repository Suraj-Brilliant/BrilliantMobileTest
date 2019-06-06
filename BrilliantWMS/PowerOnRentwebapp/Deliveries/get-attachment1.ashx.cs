using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>
    /// Summary description for get_attachment
    /// </summary>
    public class get_attachment : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlCommand cmd2 = new SqlCommand();
        SqlDataAdapter da2 = new SqlDataAdapter();
        DataSet ds2 = new DataSet();
        DataTable dt2 = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            String OdrID = context.Request.QueryString["odrId"];
            context.Response.ContentType = "text/xml";
            string xmlString = string.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
           
            //string[] filePaths = Directory.GetFiles(context.Server.MapPath("~/Deliveries/Attachment/" + OrderId));
            //    List<ListItem> files = new List<ListItem>();
            //    foreach (string filePath in filePaths)
            //    {

            //        files.Add(new ListItem(Path.GetFileName(filePath), filePath));
            //        string FileName = Path.GetFileName(filePath);
            //        string ext = Path.GetExtension(filePath);
            //        string Filepath = Path.GetFullPath(filePath);

            SqlCommand cmd3 = new SqlCommand();
            SqlDataAdapter da3 = new SqlDataAdapter();
            DataTable dt3 = new DataTable();
            DataSet ds3 = new DataSet();
            cmd3.CommandType = CommandType.Text;
            cmd3.CommandText = "select Id from tOrderHead where OrderNo='" + OdrID + "'";
            cmd3.Connection = conn;
            cmd3.Parameters.Clear();
            //cmd.Parameters.AddWithValue("param1", ResourceId);
            da3.SelectCommand = cmd3;
            da3.Fill(ds3, "tbl4");
            dt3 = ds3.Tables[0];

            string OrderId = dt3.Rows[0]["Id"].ToString();




            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from tDocument where ReferenceID=" + OrderId + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl");
            dt = ds.Tables[0];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                string filepath = dt.Rows[i]["DocumentSavePath"].ToString();
                if (filepath != "null")
                {
                    if (filepath != "Attachment/" + OrderId + "/signature.png")
                    {
                        xmlString = xmlString + "<pathDetails>";
                        xmlString = xmlString + "<imageName>" + filepath + "</imageName>";
                        xmlString = xmlString + "</pathDetails>";
                    }
                }
            }

           
            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}