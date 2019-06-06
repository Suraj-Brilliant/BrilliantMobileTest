using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BrilliantWMS.LoginService;
//using PowerOnRentwebapp.LoginService;
//using PowerOnRentwebapp.localLoginService;
using WebMsgBox;
using System.Data;
//namespace PowerOnRentwebapp.Login
namespace BrilliantWMS.Login
{
    public class Profile
    {
        // static PopupMessages.PopupMessage pop = new PopupMessages.PopupMessage();
        /*string _TestField;
        public string TestField { get { return _TestField; } set { _TestField = value; } }

        public static long CompanyID { get; set; }
        public static string CompanyName { get; set; }
        public static string CompanyLogoURL { get; set; }

        public static long UserID { get; set; }
        public static string UserTitle { get; set; }
        public static string UserName { get; set; }
        public static string UserType { get; set; }
        public static string Department { get; set; }
        public static string Designation { get; set; }
        public static string ProfileImgURL { get; set; }
        public static string HeaderMenu { get; set; }

        public static string Theme { get; set; }
        public static string ConnectionString { get; set; }

        public static string DataSource { get; set; }
        public static string DataBase { get; set; }
        public static string DBPassword { get; set; }

        public static string MainDataSource { get; set; }
        public static string MainDataBase { get; set; }
        public static string MainDBPassword { get; set; }

        public static string IPAddress { get; set; }
        public static string MachineID { get; set; }
        public static string TimeZone { get; set; }

        public static DateTime LogingDateTime { get; set; }
        public static DateTime LogOutDateTime { get; set; }

        static string message = "";
        static string result = "";*/

        public static void ErrorHandling(Exception ex, System.Web.UI.Page cpage, string header, string errorCause)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.LoginService.iLoginClient log = new BrilliantWMS.LoginService.iLoginClient();
            ErrorLog err = new ErrorLog();
            err.Data = ex.Data.ToString();
            err.Source = ex.Source.ToString();
            err.Message = ex.Message.ToString() + "/" + cpage + "/" + header + "/" + errorCause;
            err.GetType = ex.GetType().ToString();
            err.UserID = profile.Personal.UserID.ToString();
            if (profile.DBConnection._constr != null)
            {
                log.ErrorTracking1(err.Data, err.Source, "Error", err.Message, errorCause, DateTime.Now, err.UserID, profile.DBConnection._constr);
                //log.ErrorTracking(err, profile.DBConnection._constr);
                //switch (ex.Message)
                //{

                //    case "Input string was not in a correct format.":
                //        {
                //            WebMsgBox.MsgBox.Show(ex.Message + "Please fill correct Informations");
                //            break;

                //        }
                //    case "An error occurred while updating the entries. See the inner exception for details.":
                //        {
                //            WebMsgBox.MsgBox.Show(ex.Message);
                //            break;
                //        }
                //    default:
                //        {
                //            WebMsgBox.MsgBox.Show("An unexpected error has occured");
                //            break;
                //        }
                //}
            }
            else
            {
                WebMsgBox.MsgBox.Show("An unexpected error has occured");
            }
        }


        public static void ErrorHandling(Exception ex, string header, string errorCause)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.LoginService.iLoginClient log = new BrilliantWMS.LoginService.iLoginClient();
            ErrorLog err = new ErrorLog();
            err.Data = ex.Data.ToString();
            err.Source = ex.Source.ToString();
            err.Message = ex.Message.ToString() + "/" + header + "/" + errorCause;
            err.GetType = ex.GetType().ToString();
            err.UserID = profile.Personal.UserID.ToString();
            if (profile.DBConnection._constr != null)
            {
                log.ErrorTracking1(err.Data, err.Source, "Error", err.Message, errorCause, DateTime.Now, err.UserID, profile.DBConnection._constr);
                //log.ErrorTracking(err, profile.DBConnection._constr);
                //switch (ex.Message)
                //{

                //    case "Input string was not in a correct format.":
                //        {
                //            WebMsgBox.MsgBox.Show(ex.Message + "Please fill currect Informations");
                //            break;

                //        }
                //    case "An error occurred while updating the entries. See the inner exception for details.":
                //        {
                //            WebMsgBox.MsgBox.Show(ex.Message);
                //            break;
                //        }
                //    default:
                //        {
                //            WebMsgBox.MsgBox.Show("An unexpected error has occured");
                //            break;
                //        }
                //}
            }
            else
            {
                WebMsgBox.MsgBox.Show("An unexpected error has occured");
            }
        }


        //public static void LableAlice()
        //{
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    DataSet ds = new DataSet();

        //    ds.Tables.Add("colName");
        //    ds.Tables["colName"].Columns.Add("CompanyID");
        //    ds.Tables["colName"].Columns.Add("Value");
        //    ds.Tables["colName"].Columns.Add("Alice");

        //    ds.Tables["colName"].Rows.Add();
        //    ds.Tables["colName"].Rows[0]["CompanyID"] = "5";
        //    ds.Tables["colName"].Rows[0]["Value"] = "Zone";
        //    ds.Tables["colName"].Rows[0]["Alice"] = "District :";

        //    ds.Tables["colName"].Rows.Add();
        //    ds.Tables["colName"].Rows[1]["CompanyID"] = "5";
        //    ds.Tables["colName"].Rows[1]["Value"] = "SubZone";
        //    ds.Tables["colName"].Rows[1]["Alice"] = "Taluka :";

        //    ds.WriteXml(MapPath("LabelAlice.xml"));
        //    ds.Reset();
        //    ds.ReadXml(MapPath("LabelAlice.xml"));

        //    DataRow[] dr;
        //    dr = ds.Tables[0].Select(" CompanyID = " + profile.Personal.CompanyID + " and Value = '" + lblZone.Text.Replace(":", "") + "'");
        //    if (dr.Length != 0)
        //    {
        //        lblZone.Text = dr[0]["Alice"].ToString();
        //    }
        //    DataRow[] dr1;
        //    dr1 = ds.Tables[0].Select(" CompanyID = " + profile.Personal.CompanyID + " and Value = '" + lblSubZone.Text.Replace(":", "") + "'");
        //    if (dr1.Length != 0)
        //    {
        //        lblSubZone.Text = dr1[0]["Alice"].ToString(); 
        //    }
           
        //}





    }
}
