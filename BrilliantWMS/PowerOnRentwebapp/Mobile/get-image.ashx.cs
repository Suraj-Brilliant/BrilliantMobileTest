using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for get_image
    /// </summary>
    public class get_image : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            byte[] buffer = null;
            string querySqlStr = "";
            if (context.Request.QueryString["imgID"] != null)
            {
                querySqlStr = "select * from timage where ReferenceId=" + context.Request.QueryString["imgID"] + " AND SkuImage is not null";
            }
            else
            {
                querySqlStr = "select * from timage";
            }
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand command = new SqlCommand(querySqlStr, connection);
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    //get the extension name of image
                    while (reader.Read())
                    {
                        string name = reader["Path"].ToString();
                        int endIndex = name.LastIndexOf('.');
                        string extensionName = name.Remove(0, endIndex + 1);
                        buffer = (byte[])reader["SkuImage"];
                        context.Response.Clear();
                        context.Response.ContentType = "image/" + extensionName;
                        context.Response.BinaryWrite(buffer);
                        context.Response.Flush();
                        context.Response.Close();

                    }
                }
                else
                {

                    context.Response.ContentType = "image/jpg";
                    context.Response.WriteFile(System.Web.HttpContext.Current.Server.MapPath("images/no-preview.jpg"));
                }
                reader.Close();

            }
            finally
            {
                connection.Close();
            }
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