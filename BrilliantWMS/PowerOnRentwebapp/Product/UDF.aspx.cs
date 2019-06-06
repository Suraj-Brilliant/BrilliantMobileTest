using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Threading;
using System.Reflection;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Web.Services;
using BrilliantWMS.Login;

namespace BrilliantWMS.Product
{
    public partial class UDF : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;
        long specifId = 0;
        string Queryid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            if (Request.QueryString["Id"] != Queryid)
            {
                specifId = long.Parse(Request.QueryString["Id"].ToString());
                hdnspecifid.Value = specifId.ToString();
            }
            loadstring();
            GetEditdata();
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            lblContactPersonFormHeader.Text = rm.GetString("UpdateUDF", ci);
            btnAddressSubmit.Value = rm.GetString("Submit", ci);
            lblvaue.Text = rm.GetString("Description", ci);
            Button1.Value = rm.GetString("Submit", ci);
        }

        public void GetEditdata()
        {
            DataSet ds;
            DataTable dt;
            CustomProfile profile = CustomProfile.GetProfile();
            iProductMasterClient productClient = new iProductMasterClient();
            try
            {
                ds = productClient.GetEditSpecifcdetail(specifId, profile.DBConnection._constr);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {

                    long Id = long.Parse(dt.Rows[0]["ID"].ToString());
                    txtudf.Text = dt.Rows[0]["SpecificationTitle"].ToString();
                    txtvalue.Text = dt.Rows[0]["SpecificationDescription"].ToString();
                }
            }
            catch { }
            finally { }
        }

        [WebMethod]
        public static void PMSaveAddress(object objReq)
        {
            iProductMasterClient productClient = new iProductMasterClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                long SpecificID = long.Parse(dictionary["hdnspecifid"].ToString());
                string udf = dictionary["udf"].ToString();
                string value = dictionary["value"].ToString();
                productClient.Updatespecification(SpecificID,value, profile.DBConnection._constr);    
            }
            catch (System.Exception ex)
            {
                productClient.Close();
            }
            finally
            {
                productClient.Close();
            }

        }
    }
}