using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrilliantWMS.Login;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace BrilliantWMS.Product
{
    /// <summary>
    /// Summary description for ShowImage
    /// </summary>
    public class ShowImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            string imageid = context.Request.QueryString["ID"];
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            conn.Open();
            SqlCommand command = new SqlCommand("select SkuImage from tImage where ReferenceID=" + imageid + "", conn);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                context.Response.BinaryWrite((Byte[])dr[0]);
            }
            conn.Close();
            context.Response.End();       
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