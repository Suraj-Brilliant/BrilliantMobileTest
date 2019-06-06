using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrilliantWMS.Login;
using System.Data.SqlClient;
using System.Data;


namespace BrilliantWMS.Product
{
    /// <summary>
    /// Summary description for DisplaySkuImage
    /// </summary>
    public class DisplaySkuImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            CustomProfile profile = CustomProfile.GetProfile();

            string imageid = context.Request.QueryString["ID"];

            string[] conn = profile.DBConnection._constr;
            SqlConnection connection = new SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            connection.Open();
            SqlCommand command = new SqlCommand("select SkuImage from tImage where ReferenceID=" + imageid + "",connection);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                context.Response.BinaryWrite((Byte[])dr[0]);
            }
            connection.Close();
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