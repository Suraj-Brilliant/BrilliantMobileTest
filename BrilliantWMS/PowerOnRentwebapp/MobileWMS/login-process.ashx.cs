using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for login_process
    /// </summary>
    public class login_process : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        string userid = "", pass = "";

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            userid = context.Request.Form["usrId"];
            pass = context.Request.Form["usrPass"];
            //userid = context.Request.QueryString["userid"];
            //pass = context.Request.QueryString["pass"];

            CustomProfile profile = CustomProfile.GetProfile(userid);

            if (Membership.ValidateUser(userid, pass))
            {
                BrilliantWMS.UserCreationService.iUserCreationClient UserCreationClient = new BrilliantWMS.UserCreationService.iUserCreationClient();

                string usrName = profile.Personal.UserName.ToString();
                string cmpny = profile.Personal.CompanyID.ToString();
                string cmpnyNm = profile.Personal.CName.ToString();
                string deptID = profile.Personal.DepartmentID.ToString();
                string deptNm = profile.Personal.Department.ToString();
                string mobNo = profile.Personal.MobileNo;
                string dbid = profile.Personal.UserID.ToString();
                string type = profile.Personal.UserType.ToString();
                string warehouseList = GetWarehouseList(dbid);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = " select Name from mCompany where ID=" + cmpny + "";
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                string CompanyName = ds.Tables[0].Rows[0]["Name"].ToString();

                SqlCommand cmd3 = new SqlCommand();
                SqlDataAdapter da3 = new SqlDataAdapter();
                DataSet ds3 = new DataSet();
                DataTable dt3 = new DataTable();
                cmd3.CommandType = CommandType.Text;
                cmd3.CommandText = "select MobileInterface from mUserProfileHead where id=" + dbid + "";
                cmd3.Connection = conn;
                cmd3.Parameters.Clear();
                da3.SelectCommand = cmd3;
                da3.Fill(ds3, "tbl4");
                dt3 = ds3.Tables[0];
                int mobileinterface = Convert.ToInt32(dt3.Rows[0]["MobileInterface"].ToString());
                if (mobileinterface == 1)
                {
                    SqlCommand cmd1 = new SqlCommand();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    DataSet ds1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select top(1) U.WarehoueID ,W.WarehouseName from mUserWarehouse U left outer join mWarehouseMaster W on U.WarehoueID=W.ID where U.UserID=" + dbid + "";
                    cmd1.Connection = conn;
                    cmd1.Parameters.Clear();
                    da1.SelectCommand = cmd1;
                    da1.Fill(ds1, "tbl2");
                    dt1 = ds1.Tables[0];
                    string warehouseID="",  wName = "";
                    if (dt1.Rows.Count > 0)
                    {
                        warehouseID = ds1.Tables[0].Rows[0]["WarehoueID"].ToString();
                        wName = ds1.Tables[0].Rows[0]["WarehouseName"].ToString();                      
                    }
                    jsonString = jsonString + "{\n";
                    jsonString = jsonString + "\"username\": \"" + usrName.Trim() + "\",\n";
                    jsonString = jsonString + "\"companyid\": \"" + cmpny.Trim() + "\",\n";
                    jsonString = jsonString + "\"companyname\": \"" + CompanyName.Trim() + "\",\n";

                    jsonString = jsonString + "\"warehouseid\": \"" + warehouseID.Trim() + "\",\n";
                    jsonString = jsonString + "\"warehouse\": \"" + wName.Trim() + "\",\n";

                    jsonString = jsonString + "\"warehouselist\": \"" + warehouseList.Trim() + "\",\n";
                    jsonString = jsonString + "\"userid\": \"" + userid.Trim() + "\",\n";
                    jsonString = jsonString + "\"dbid\": \"" + dbid.Trim() + "\",\n";
                    jsonString = jsonString + "\"mobile\": \"" + mobNo.Trim() + "\",\n";
                    jsonString = jsonString + "\"usertype\": \"" + type.Trim() + "\",\n";
                    jsonString = jsonString + "\"authmsg\": \"success\"\n";
                }
                else
                {
                    jsonString = jsonString + "{\"authmsg\": \"failed\"";
                }

            }
            else
            {
                jsonString = jsonString + "{\"authmsg\": \"failed\"";
            }
            jsonString = jsonString + "}]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        protected string GetWarehouseList(string userid)
        {
            string dept = "", deptid = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            String myQueryString1 = "select U.WarehoueID ,W.WarehouseName from mUserWarehouse U left outer join mWarehouseMaster W on U.WarehoueID=W.ID where U.UserID=" + userid + "";
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
                    string Dscr = dt1.Rows[i]["WarehouseName"].ToString();
                    dept = dept + Dscr;
                    dept = dept + ":";
                    string uId = dt1.Rows[i]["WarehoueID"].ToString();
                    dept = dept + uId;
                    dept = dept + ":";
                }
                deptid = dept.Substring(0, (dept.Length - 1));
            }
            return deptid;
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