using System;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using PowerOnRentwebapp.Login;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for get_profile
    /// </summary>
    public class get_profile : IHttpHandler
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
            SqlConnection conn = new SqlConnection(strcon);

            String userId = context.Request.Form["usrId"];
            String userPass = context.Request.Form["usrPass"];


            CustomProfile profile = CustomProfile.GetProfile(userId);

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";


            xmlString = xmlString + "<profileInfo>";
            if (Membership.ValidateUser(userId, userPass))
            {
                UserCreationService.iUserCreationClient UserCreationClient = new UserCreationService.iUserCreationClient();
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
                    string cmpny = profile.Personal.CompanyID.ToString();
                    string cmpnyNm = profile.Personal.CName.ToString();
                    string deptID = profile.Personal.DepartmentID.ToString();


                    SqlCommand cmd1 = new SqlCommand();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();

                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select Territory from mTerritory where id=" + deptID + "";
                    cmd1.Connection = conn;
                    cmd1.Parameters.Clear();
                    da1.SelectCommand = cmd1;
                    da1.Fill(ds1, "tbl1");
                    dt1 = ds1.Tables[0];
                    string deptNm = ds1.Tables[0].Rows[0]["Territory"].ToString();

                    // string deptNm = profile.Personal.Department.ToString();
                    string deptNam = CheckString(deptNm);
                    string mobNo = profile.Personal.MobileNo;
                    string dbid = profile.Personal.UserID.ToString();
                    string type = profile.Personal.UserType.ToString();

                    string type1 = "";
                    if (type == "Requestor") { type1 = "Requester"; }
                    else if (type == "Requestor And Approver") { type1 = "Requester And Approver"; }
                    else if (type == "Admin") { type1 = "Requester And Approver"; }
                    else if (type == "Super Admin") { type1 = "Requester And Approver"; }
                    else { type1 = type; }

                    string deptlist = GetDeptList(dbid);

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " select Name from mCompany where ID=" + cmpny + "";
                    cmd.Connection = conn;
                    cmd.Parameters.Clear();
                    da.SelectCommand = cmd;
                    da.Fill(ds, "tbl1");
                    dt = ds.Tables[0];

                    string CompanyName = ds.Tables[0].Rows[0]["Name"].ToString();

                    xmlString = xmlString + "<username>" + CheckString(usrName) + "</username>";
                    xmlString = xmlString + "<companyid>" + cmpny + "</companyid>";
                    xmlString = xmlString + "<companyname>" + CheckString(CompanyName) + "</companyname>";
                    xmlString = xmlString + "<departmentid>" + deptID + "</departmentid>";
                    xmlString = xmlString + "<departmentname>" + CheckString(deptNam) + "</departmentname>";
                    xmlString = xmlString + "<departmentlist>" + CheckString(deptlist) + "</departmentlist>";

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

        protected string GetDeptList(string usrId)
        {
            string dept = "", deptid = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            String myQueryString1 = "select UTD.TerritoryID,MT.Territory from  mUserTerritoryDetail UTD left outer join mTerritory MT on UTD.TerritoryID=MT.ID where UTD.Userid=" + usrId + "";
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = myQueryString1;
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl5");
            dt1 = ds1.Tables[0];
            int cntr = dt1.Rows.Count;
            if (cntr > 0)
            {
                for (int i = 0; i <= cntr - 1; i++)
                {
                    string Dscr = dt1.Rows[i]["Territory"].ToString();
                    dept = dept + Dscr;
                    dept = dept + ":";
                    string uId = dt1.Rows[i]["TerritoryID"].ToString();
                    dept = dept + uId;
                    dept = dept + ":";
                }
                deptid = dept.Substring(0, (dept.Length - 1));
            }
            return deptid;
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            return value;
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