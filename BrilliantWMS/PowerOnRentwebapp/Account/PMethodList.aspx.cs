using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebMsgBox;
using System.Web.Services;
using BrilliantWMS.CompanySetupService;

namespace BrilliantWMS.Account
{
    public partial class PMethodList : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        long StoreId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();

             CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
           if (Request.QueryString["deptid"] != null)
            {
                StoreId = long.Parse(Request.QueryString["deptid"].ToString());
                hdndeptID.Value = Request.QueryString["deptid"].ToString();
                Session.Add("PDeptID", hdndeptID.Value);
            }
           if (!IsPostBack)
           {
               CompanyClient.DeleteRecordWithZeroQty(profile.DBConnection._constr);
           }
           RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst();
                GridpayMethod.DataSource = ds;
                GridpayMethod.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProductSearch.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst()
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            str = "select DM.DeptID, DM.ID,PM.MethodName,DM.Sequence from mDeptPaymentMethod DM left outer join mPaymentMethodMain PM on DM.PMethodID = PM.ID where DeptID = 0 or DeptID = '" + StoreId + "'";
                // str = "select ProductCode,OMSSKUCode,Name,Description,from mProduct mp where StoreId = '" + StoreId + "' and mp.ProductCode like '%" + filter + "%' or mp.Name like '%" + filter + "%' or mp.Description like '%" + filter + "%' ";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
                return ds1;
        }

        [WebMethod]
        public static string RemoveSku(object objReq)
        {
            string result = "";
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;
            long PMethodID = long.Parse(dictionary["Methodid"].ToString());
            CompanyClient.RemoveDeptPMethod(PMethodID, profile.DBConnection._constr);
            result = "success";
            return result;
        }

        [WebMethod]
        public static void SaveDeptMethod(string Dept)
        {
            long DeptID = long.Parse(Dept);
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.CompanySetupService.iCompanySetupClient CompanyClient = new BrilliantWMS.CompanySetupService.iCompanySetupClient();
            // update DeptPMethodTable Code
            CompanyClient.UpdateDeptPaymentMethod(DeptID, profile.DBConnection._constr);
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblheader.Text = rm.GetString("PaymentMethodList", ci);
               btnSubmitProductSearch1.Value=rm.GetString("Submit",ci);
               btnadd.Value = rm.GetString("Add", ci);
               btnSubmitProductSearch2.Value = rm.GetString("Submit", ci);
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Account Master", "loadstring");
            }
        }
    }
}