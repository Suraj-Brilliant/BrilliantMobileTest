using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using BrilliantWMS.UserCreationService;
using System.Web.Services;
using BrilliantWMS.Login;
using System.Web.Security;
using WebMsgBox;
using Obout.Grid;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;


namespace BrilliantWMS.UserManagement
{
    public partial class Delegation : System.Web.UI.Page
    {
        long UserID = 0;
        string UserType = "";
        ResourceManager rm;
        CultureInfo ci;
        protected void Page_Load(object sender, EventArgs e)
        {
             CustomProfile profile = CustomProfile.GetProfile();
             UserID = profile.Personal.UserID;
             UserType = profile.Personal.UserType;
             hnduserID.Value = UserID.ToString();
             UC_Date1.startdate(DateTime.Now);
             UC_Date2.startdate(DateTime.Now);
             if (Session["Lang"] == "")
             {
                 Session["Lang"] = Request.UserLanguages[0];
             }
             loadstring();

             if (UserType == "Requestor" || UserType == "Requester")
             {
                 ddlDepartment.Visible = false;
                 ddlUOM.Visible = false;
                 btnsumit.Visible = false;
                 UC_Date1.Visible = false;
                 UC_Date2.Visible = false;
                 ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Access delegation not allowed to Requestor');", true);
             }

             if (IsPostBack != true)
             {
                 BindDepartment(UserID, UserType);
                 BindAccessDelgrid("AddNew", UserID);
                 Session.Add("hdnedit", "");
             }

        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblheader.Text = rm.GetString("EmailTemplateList", ci);
                lblheader.Text = rm.GetString("AccessDelegation", ci);
                lblfrmdate.Text = rm.GetString("FromDate", ci);
                lbltodate.Text = rm.GetString("ToDate", ci);
                lblrightsto.Text = rm.GetString("Delegation", ci);
                lblremark.Text = rm.GetString("Remark", ci);
                lblaccdele.Text = rm.GetString("AccessDelegation", ci);
                btnsumit.Value = rm.GetString("Submit", ci);
                lbldept.Text = rm.GetString("Department", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Access Delegation", "loadstring");
            }
        }

        public void BindDepartment(long UserID, string usertype)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds;
            try
            {
                if (usertype != "Super Admin")
                {
                    ds = userClient.GetDepartmentDelegate(UserID, profile.DBConnection._constr);
                }
                else
                {
                    ds = userClient.GetDepartmentSSuperAdmin(profile.DBConnection._constr);
                }
                ddlDepartment.DataSource = ds;
                ddlDepartment.DataTextField = "Territory";
                ddlDepartment.DataValueField = "ID";
                ddlDepartment.DataBind();
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
                ddlDepartment.Items.Insert(0, lst);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Access Delegation", "BindDepartment");
            }
            finally { userClient.Close(); }
           
        }

        [WebMethod]
        public static string SaveAccessDelegation(object objReq)
        {
            string result = "";
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {
                string hiddneedit = "";
               
                CustomProfile profile = CustomProfile.GetProfile();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long department = long.Parse(dictionary["department"].ToString());
                long Delegateto = long.Parse(dictionary["Delegateto"].ToString());
                string Remark = dictionary["Remark"].ToString();
                string hndstate = dictionary["hndstate"].ToString();
                long UserId = long.Parse(dictionary["UserId"].ToString());
                DateTime FromDate = Convert.ToDateTime(dictionary["FromDate"].ToString());
                DateTime ToDate = Convert.ToDateTime(dictionary["ToDate"].ToString());
                // long newdelegate = long.Parse(dictionary["newDelegate"].ToString());
                long DelegateId = 0;
                long delegatefrom = UserId;



                if (HttpContext.Current.Session["hdnedit"].ToString() != "" || HttpContext.Current.Session["hdnedit"].ToString() != null)
                {
                    hndstate = HttpContext.Current.Session["hdnedit"].ToString();
                }

                string hdndelegatestateedit = dictionary["hdndeligateeditstate"].ToString();
                if (hndstate == "Edit")
                {
                        string hdndeligateeditstate = hiddneedit.ToString();
                        hndstate = hdndeligateeditstate.ToString();
                        string delid = HttpContext.Current.Session["hdnnewDelegateid"].ToString();
                        DelegateId = long.Parse(delid.ToString());
                        HttpContext.Current.Session["hdnnewDelegateid"] = "";
                        HttpContext.Current.Session["hdnedit"] = "";
                        userClient.SaveEditUserDelegation(DelegateId, delegatefrom, FromDate, ToDate, Delegateto, Remark, hndstate, profile.Personal.UserID, DateTime.Now, department, profile.DBConnection._constr);
                }
                else
                {
                      long valuecount = 0;
                      valuecount = userClient.GetDuplicateDelegate(FromDate, ToDate, Delegateto, department, delegatefrom, profile.DBConnection._constr);
                       if (valuecount == 0)
                       {
                           string hdndeligateeditstate = hiddneedit.ToString();
                           hndstate = hdndeligateeditstate.ToString();
                           hndstate = "Add";
                           //string delid = HttpContext.Current.Session["hdnnewDelegateid"].ToString();
                           // DelegateId = long.Parse(delid.ToString());
                           HttpContext.Current.Session["hdnnewDelegateid"] = "";
                           HttpContext.Current.Session["hdnedit"] = "";
                           userClient.SaveEditUserDelegation(DelegateId, delegatefrom, FromDate, ToDate, Delegateto, Remark, hndstate, profile.Personal.UserID, DateTime.Now, department, profile.DBConnection._constr);
                       }
                       else
                       {
                           result = "Exist";
                       }
                }

               
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Access Delegation", "SaveAccessDelegation");

            }
            finally
            {
                userClient.Close();
            }

            return result;
        }

        public void GetDelegationDetail(long Delegateid)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                DataSet ds;
                DataTable dt;
                //ds = productClient.GetBOMDetailById(BOMDetailId, profile.DBConnection._constr);
                ds = userClient.getUserDelegateDetail(Delegateid, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    long deptId = long.Parse(dt.Rows[0]["DeptID"].ToString());
                    BindDepartment(UserID, UserType);
                    ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByValue(dt.Rows[0]["DeptID"].ToString()));
                    getDelegateToList(deptId);
                    if (ddlUOM.Items.Count >= 1) ddlUOM.SelectedIndex = 0;
                    ddlUOM.SelectedIndex = ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(dt.Rows[0]["DeligateTo"].ToString()));
                    ddlUOM.SelectedItem.Value = dt.Rows[0]["DeligateTo"].ToString();
                    //ddlUOM.SelectedItem.Text = dt.Rows[0]["Name"].ToString();

                    hdndegateId.Value = Delegateid.ToString();

                    txtPrincipalPrice.Text = dt.Rows[0]["Remark"].ToString();
                    UC_Date1.Date = Convert.ToDateTime(dt.Rows[0]["FromDate"].ToString());
                    UC_Date2.Date = Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString());
                    hdndeligateeditstate.Value = "Edit";
                    hdnnewDelegateid.Value = dt.Rows[0]["ID"].ToString();
                    
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Access Delegation", "GetDelegationDetail");
            }
            finally
            {
                userClient.Close();
            }


        }

        //protected void grdaccessdele_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        //{
        //    try
        //    {
        //        Hashtable selectedrec = (Hashtable)grdaccessdele.SelectedRecords[0];
        //        hdndegateId.Value = selectedrec["ID"].ToString();

        //        hdnnewDelegateid.Value = selectedrec["ID"].ToString();
        //        hdndeligateeditstate.Value = "Edit";
        //        GetDelegationDetail(long.Parse(hdndegateId.Value));
        //        //string editsession = "Edit";

        //        Session["hdnedit"] = "Edit";
        //        Session.Add("hdnnewDelegateid", hdnnewDelegateid.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        Login.Profile.ErrorHandling(ex, this, "Access Delegation", "BindAccessDelgrid");
        //    }
        //    finally {}
        //}

        //protected void imgBtnEditbom_OnClick(object sender, ImageClickEventArgs e)
        //{
        //    ImageButton imgbtn = (ImageButton)sender;
        //    hdnnewDelegateid.Value = imgbtn.ToolTip.ToString();
        //    hdndegateId.Value = imgbtn.ToolTip.ToString();
        //    hdndeligateeditstate.Value = "Edit";
        //    GetDelegationDetail(long.Parse(hdndegateId.Value));
        //    //string editsession = "Edit";

        //    Session["hdnedit"] = "Edit";
        //    Session.Add("hdnnewDelegateid", hdnnewDelegateid.Value);
        //}

        public void getDelegateToList(long DeptId)
        {

            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            long UserID = long.Parse(profile.Personal.UserID.ToString());
            DataSet ds;
            ds = userClient.getDelegateToList(DeptId,UserID, profile.DBConnection._constr);
           // ds = userClient.getDelegateToListMultipleDept(hdnSelectedLocation.Value, profile.DBConnection._constr);
            ddlUOM.DataSource = ds;
            ddlUOM.DataTextField = "Name";
            ddlUOM.DataValueField = "ID";
            ddlUOM.DataBind();
            ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
            ddlUOM.Items.Insert(0, lst);

        }

        protected void grdaccessdele_RebindGrid(object sender, EventArgs e)
        {
            if (hndstate.Value == "Edit")
            {
                BindAccessDelgrid(hndstate.Value, Convert.ToInt64(hnduserID.Value));
            }
            else
            {
                BindAccessDelgrid(hndstate.Value, Convert.ToInt64(hnduserID.Value));
            }
        }

        public void BindAccessDelgrid(string state, long UserId)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            iUserCreationClient userClient = new iUserCreationClient();
            try
            {
                ds = userClient.GetUserDelegationDetail(state, UserId, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    grdaccessdele.DataSource = ds.Tables[0];
                    grdaccessdele.DataBind();
                }
                else
                {
                    grdaccessdele.DataSource = null;
                    grdaccessdele.DataBind();

                }
            }
            catch (Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Access Delegation", "BindAccessDelgrid");
            }
            finally { userClient.Close(); }
        }

        [WebMethod]
        public static List<contact> Getdelegate(object objReq)
        {
            iUserCreationClient userClient = new iUserCreationClient();
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<contact> LocList = new List<contact>();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                //ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
                long ddlDepartment = long.Parse(dictionary["ddlDepartment"].ToString());
                long UsrId = long.Parse(profile.Personal.UserID.ToString());
                ds = userClient.getDelegateToList(ddlDepartment, UsrId, profile.DBConnection._constr);

                dt = ds.Tables[0];


                contact Loc = new contact();
                Loc.Name = "--Select--";
                Loc.Id = "0";
                LocList.Add(Loc);
                Loc = new contact();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Loc.Id = dt.Rows[i]["ID"].ToString();
                        Loc.Name = dt.Rows[i]["Name"].ToString();
                        LocList.Add(Loc);
                        Loc = new contact();

                    }

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Access Delegation", "Getdelegate");
            }
            finally
            {
                userClient.Close();

            }
            return LocList;
        }

        public class contact
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }

        [WebMethod]
        public static string RemoveSku(object objReq)
        {
            string result = "";
            iUserCreationClient userClient = new iUserCreationClient();
            try{
            CustomProfile profile = CustomProfile.GetProfile();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;
            long Delegateid = long.Parse(dictionary["Delegateid"].ToString());
            userClient.Deletedelegate(Delegateid, profile.DBConnection._constr);
            result = "success";
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Access Delegation", "RemoveSku");
            }
            finally
            {
                userClient.Close();

            }
            return result;

        }
    }
}