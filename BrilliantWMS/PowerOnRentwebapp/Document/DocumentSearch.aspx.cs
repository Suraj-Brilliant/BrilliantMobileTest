using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using BrilliantWMS.DocumentService;
using BrilliantWMS.Login;

namespace BrilliantWMS.Document
{
    public partial class DocumentSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RebindGrid(sender, e);
        }

        protected void RebindGrid(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = GetPrdLst(GvDocument.CurrentPageIndex, hdnFilterText.Value);
                GvDocument.DataSource = ds;
                //GvDocument.GroupBy = hndgrupByGrid.Value;
                GvDocument.DataBind();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "DocumentSearch.aspx.cs", "RebindGrid");
            }
        }


        DataSet GetPrdLst(int pageIndex, string filter)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            DataSet ds1 = new DataSet();
            ds1.Reset();
            SqlConnection conn = new SqlConnection("");
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            long CustomerID = 10006;
            string str = "";
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                if (profile.Personal.UserType == "Super Admin")
                {
                    //str = "Select * from V_WMS_DocumentSearch";
                    str = "Select * from tDocument";
                }
                else
                {
                    str = "Select * from V_WMS_DocumentSearch where  CustomerHeadID = '" + CustomerID + "'";
                }
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }
            else
            {
                if (profile.Personal.UserType == "Super Admin")
                {
                    str = "Select * from V_WMS_DocumentSearch where ObjectName like '%" + filter + "%' or DocumentName like '%" + filter + "%' or DocType like '%" + filter + "%'";
                }
                else
                {
                    str = "Select * from V_WMS_DocumentSearch where CustomerHeadID = '" + CustomerID + "' and (ObjectName like '%" + filter + "%' or DocumentName like '%" + filter + "%' or DocType like '%" + filter + "%')";
                }
                SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(str, conn);
                ds1.Reset();
                da.Fill(ds1);
            }

            return ds1;
        }
    }
}