using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
//using BrilliantWMS.NServicePurchaseOrder;
//using BrilliantWMS.NServiceSalesOrder;

namespace BrilliantWMS.Warehouse
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static int WMGetGroupData(string PoNo, string GName, string GDesc)
        {
           // iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                string Objtype = "MCNPO";
                string CreatedBy = Convert.ToString(profile.Personal.UserID);
               // Purchase.InsertTGroupingPO(Objtype, GName, GDesc, CreatedBy, DateTime.Now);
                //Purchase.UpdateTgroupingPOHead(PoNo);
             }
            catch { }
            finally 
            {
               // Purchase.Close();
            }
            return 1;
        }

        [WebMethod]
        public static int WMGetSOGroupData(string SoNo, string GName, string GDesc)
        {
           // iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
           // iSalesOrderClient Sales = new iSalesOrderClient();
            CustomProfile profile = CustomProfile.GetProfile();
            try
            {
                string Objtype = "MCNSO";
                string createdby = Convert.ToString(profile.Personal.UserID);
                //Purchase.InsertTGroupingPO(Objtype, GName, GDesc, createdby, DateTime.Now);              
                //Sales.UpdateTgroupingSOHead(SoNo);               
            }
            catch { }
            finally
            {
                //Purchase.Close();
                //Sales.Close();
            }
            return 1;

        }
    }
}