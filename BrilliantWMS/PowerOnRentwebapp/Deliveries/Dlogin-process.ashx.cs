using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using BrilliantWMS.Login;
using System.IO;
using System.Web.Security;

namespace BrilliantWMS.Deliveries
{
    /// <summary>
    /// Summary description for Dlogin_process
    /// </summary>
    public class Dlogin_process : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            string userId = context.Request.Form["usrId"];
            string userPass = context.Request.Form["usrPass"];

            CustomProfile profile = CustomProfile.GetProfile(userId);

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";


            xmlString = xmlString + "<profileInfo>";

            if (Membership.ValidateUser(userId, userPass))
            {
                UserCreationService.iUserCreationClient UserCreationclient = new UserCreationService.iUserCreationClient();
                long useridd = Convert.ToInt64(profile.Personal.UserID.ToString());


                SqlCommand cmd3 = new SqlCommand();
                SqlDataAdapter da3 = new SqlDataAdapter();
                DataSet ds3 = new DataSet();
                DataTable dt3 = new DataTable();
                cmd3.CommandType = CommandType.Text;
                cmd3.CommandText = "select MobileInterface from mUserProfileHead where id=" + useridd + "";
                cmd3.Connection = conn;
                cmd3.Parameters.Clear();
                da3.SelectCommand = cmd3;
                da3.Fill(ds3, "tbl4");
                dt3 = ds3.Tables[0];

                int mobileinterface = Convert.ToInt32(dt3.Rows[0]["MobileInterface"].ToString());
                if (mobileinterface == 1)
                {
                    string usrName = profile.Personal.UserName.ToString();

                    string mobNo = profile.Personal.MobileNo;
                    string dbid = profile.Personal.UserID.ToString();
                    string type = profile.Personal.UserType.ToString();

                    string type1 = "";
                    if (type == "Driver") { type1 = "Driver"; }
                    else { type1 = type; }

                    xmlString = xmlString + "<username>" + usrName + "</username>";
                    xmlString = xmlString + "<userid>" + userId + "</userid>";
                    xmlString = xmlString + "<dbid>" + dbid + "</dbid>";
                    xmlString = xmlString + "<mobile>" + mobNo + "</mobile>";
                    xmlString = xmlString + "<type>" + type1 + "</type>";
                    xmlString = xmlString + "<authmsg>success</authmsg>";
                }
                else
                {
                    xmlString = xmlString + "<authmsg>You don't have access for Mobile Application</authmsg>";
                }
            }
            else
            {
                xmlString = xmlString + "<authmsg>failed</authmsg>";
            }


            xmlString = xmlString + "</profileInfo>";

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