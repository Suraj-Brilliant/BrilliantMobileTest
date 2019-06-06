using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UCProductSearchService;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebMsgBox;

namespace BrilliantWMS.Account
{
    public partial class CApprovalSearch : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long companyID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            if (Session["CompanyID"].ToString() != null)
            {
                companyID = long.Parse(Session["CompanyID"].ToString());
            }

            RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst(GridProductSearch.CurrentPageIndex, hdnFilterText.Value);
                GridProductSearch.DataSource = ds;
                GridProductSearch.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "SearchProduct.aspx.cs", "RebindGrid");
            }
        }

        DataSet GetPrdLst(int pageIndex, string filter)
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                // str = "select Id,ProductCode,OMSSKUCode,Name,Description from mProduct where StoreId = '" + StoreId + "'";
               
                str = "select uh.ID, uh.FirstName+ ' '+ uh.LastName AName,uh.EmailID,t.Territory,c.Name from mUserProfileHead uh left outer join mTerritory t on uh.DepartmentID = t.ID left outer join mCompany c on uh.CompanyID = c.ID where uh.CompanyID ='" + companyID + "' and uh.UserType in ('Admin','Requestor And Approver','Approver')";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                str = "select uh.ID, uh.FirstName+ ' '+ uh.LastName AName,uh.EmailID,t.Territory,c.Name from mUserProfileHead uh left outer join mTerritory t on uh.DepartmentID = t.ID left outer join mCompany c on uh.CompanyID = c.ID where uh.CompanyID ='" + companyID + "' and uh.UserType in ('Admin','Requestor And Approver','Approver') and (uh.FirstName like '%" + filter + "%' or t.Territory like '%" + filter + "%' or c.Name like '%" + filter + "%')";
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }

        private void loadstring()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
                rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
                ci = Thread.CurrentThread.CurrentCulture;

                lblheader.Text = rm.GetString("ApproverList", ci);
                btnSubmitProductSearch1.Value = rm.GetString("Submit", ci);
                btnSubmitProductSearch2.Value = rm.GetString("Submit", ci);
             
               

            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "Document", "loadstring");
            }
        }
    }
}