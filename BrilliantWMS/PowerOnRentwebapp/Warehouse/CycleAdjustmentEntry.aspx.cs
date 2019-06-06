using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;
using BrilliantWMS.Login;
using BrilliantWMS.CycleCountService;

namespace BrilliantWMS.POR
{
    public partial class CycleAdjustmentEntry : System.Web.UI.Page
    {
        long CycleDetailID = 0, CountHeadID = 0;
        SqlConnection con1 = new SqlConnection("");
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        protected void Page_Load(object sender, EventArgs e)
        {
            con1.ConnectionString = ConfigurationManager.ConnectionStrings["elegantcrm7ConnectionString"].ConnectionString;
            if (Request.QueryString["Sequence"] != null) CycleDetailID = long.Parse(Request.QueryString["Sequence"].ToString());
            if (Request.QueryString["CountHeadID"] != null) CountHeadID = long.Parse(Request.QueryString["CountHeadID"].ToString());
            hdnCycleHeadID.Value = CountHeadID.ToString();
            hdncycleDetailID.Value = CycleDetailID.ToString();
            GetCycleCountRecord();
        }


        public void GetCycleCountRecord()
        {
            iCycleCountClient Cycle = new iCycleCountClient();
            try
            {
                if (Request.QueryString["Sequence"] != "" || Request.QueryString["Sequence"] != null || Request.QueryString["Sequence"] != "")
                {
                    da = new SqlDataAdapter("select ProductCode,LocationCode,QtyBalance,ActualQty,AdjustmentQty,Remark,AdjustLocation,DiffQty,BatchCode,SKUID,LocationID from tCycleCountDetail where ID='" + CycleDetailID + "' and CountHeadID=" + CountHeadID + "", con1);
                    da.Fill(ds);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtprodCode.Text = dt.Rows[0]["ProductCode"].ToString();
                        txtlocCode.Text = dt.Rows[0]["LocationCode"].ToString();
                        txtsysQty.Text = dt.Rows[0]["QtyBalance"].ToString();
                        txtActualQty.Text = dt.Rows[0]["ActualQty"].ToString();
                        txtAdjQty.Text = dt.Rows[0]["DiffQty"].ToString();
                        txtbatch.Text = dt.Rows[0]["BatchCode"].ToString();
                        hdnproductId.Value = dt.Rows[0]["SKUID"].ToString();
                        hdndiffQty.Value = dt.Rows[0]["DiffQty"].ToString();
                        hdnfromlocID.Value = dt.Rows[0]["LocationID"].ToString(); 
                    }
                }
            }
            catch { }
            finally
            {
                con1.Close();
                Cycle.Close();
            }
        }

        [WebMethod]
        public static string GetlocationQty(object objReq)
        {
            iCycleCountClient Cycle = new iCycleCountClient();
            CustomProfile profile = CustomProfile.GetProfile();
            tCycleCountDetail Dcycle = new tCycleCountDetail();
            string result = "";
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)objReq;
                 
                string ProductCode = dictionary["ProductCode"].ToString();
                string locationcode = dictionary["Location"].ToString();
                long ProductID = long.Parse(dictionary["ProductID"].ToString());
                long LocationID = long.Parse(dictionary["FromLocID"].ToString());
                string BatchCode = dictionary["BatchCode"].ToString();
                decimal SystemQty = decimal.Parse(dictionary["SystemQty"].ToString());
                decimal ActualQty = decimal.Parse(dictionary["ActualQty"].ToString());
                decimal AjustmentQty = decimal.Parse(dictionary["AjustmentQty"].ToString());
                string Remark = dictionary["Remark"].ToString();
                string AdjustLoc = dictionary["AdjustmentLocIds"].ToString();
                long CycleHeadID = long.Parse(dictionary["CycleHeadID"].ToString());
                long CycleDetailID = long.Parse(dictionary["CycleDetailID"].ToString());

                string[] ToLocIds = AdjustLoc.Split(',');
                decimal calculateQty = AjustmentQty;

                

                Cycle.UpdateStockSkuTransForFromLoc(ProductID, BatchCode, LocationID, AjustmentQty,profile.DBConnection._constr); 

                for (int i = 0; i < ToLocIds.Length; i++)
                {
                     long ToLocID = long.Parse(ToLocIds[i].ToString());
                     decimal availQty = Cycle.getLocationRemainingQty(ToLocID, profile.DBConnection._constr);      // need to craete sp add in domain ninterface and build
                     if (calculateQty > availQty)
                     {
                         calculateQty = calculateQty - availQty;
                         Cycle.UpdateStocktransToLoc(ProductID, BatchCode, calculateQty, ToLocID, profile.Personal.UserID, CycleHeadID, profile.DBConnection._constr);   // need to Add CompanyID,CustomerID and UserID
                     }
                     else
                     {
                         Cycle.UpdateStocktransToLoc(ProductID, BatchCode, calculateQty, ToLocID, profile.Personal.UserID,CycleHeadID, profile.DBConnection._constr);     // need to Add CompanyID,CustomerID and UserID
                         calculateQty = 0;
                     }
                   
                }

                Dcycle.ID = CycleDetailID;
                Dcycle.CountHeadID = CycleHeadID;
                Dcycle.QtyBalance = SystemQty;
                Dcycle.ActualQty = ActualQty;
                Dcycle.DiffQty = AjustmentQty;
                Dcycle.AdjustmentQty = AjustmentQty;
                Dcycle.Remark = Remark;
                Dcycle.AdjustLocation = AdjustLoc;
                Dcycle.ProductCode = ProductCode;
                Dcycle.LocationCode = locationcode;
                Dcycle.SKUID = ProductID;
                Dcycle.LocationID = LocationID;
                Dcycle.BatchCode = BatchCode;
                Dcycle.CreatedBy = profile.Personal.UserID.ToString();
                Dcycle.CreationDate = DateTime.Now;
                long CycleDtailID = Cycle.SaveCycleCount(Dcycle, profile.DBConnection._constr);





                    /*decimal ClosingBalOutProd = Cycle.GetClosingBalance(Parameter, AjustmentQty, ProductID, LocationID);
                    decimal ClosingBalInProd = Cycle.GetClosingBalance(param, AjustmentQty, ProductID, AdjustLoc);
                    decimal AdjustmentSUMOut = Cycle.GetAdjustmentSUM(AjustmentQty, ProductID, LocationID);
                    decimal AdjustmentSUMIN = Cycle.GetAdjustmentSUM(AjustmentQty, ProductID, AdjustLoc);*/

                    // Cycle.UpdateCycleCountDetail(AjustmentQty, Remark, CreatedBy, DateTime.Now.Date, ProductCode, CycleHeadID, AdjstmentLoc);
                    // // write method for transaction Entries One for out product and one for In product
                    // Cycle.InsertTransactionAdjustment("Cycle Count", CycleHeadID, ProductID, LocationID, AdjustLoc, Remark, CreatedBy, DateTime.Now.Date, DateTime.Now.Date, AjustmentQty);

                    // // update tinventry AdjustmentQty & QtyBalance for Out transaction
                    // Cycle.UpdateInventryForOut(AdjustmentSUMOut, ClosingBalOutProd, ProductID, LocationID);


                    // if records of ClosingBalIn product Is not inserted then we have to insert that and if present then update that
                    /*   long CheckEntry = Cycle.GetCountTOCheckEntryInInventry(AdjustLoc, ProductID);
                       if (CheckEntry > 0)
                       {
                           Cycle.UpdateInventryForOut(AdjustmentSUMIN, ClosingBalInProd, ProductID, AdjustLoc);
                       }
                       else
                       {
                           Cycle.InsertInventryCycleCount(ProductID, AdjustLoc, "Secondary", AjustmentQty, AjustmentQty, AjustmentQty, CreatedBy, DateTime.Now.Date, "Inventory_Inventory");
                       }*/


                    result = "Adjustment successful";


            }
            catch { result = "Some error occurred"; }
            finally
            {

                Cycle.Close();
            }
            return result;

        }

        [WebMethod]
        public static string EditLocation(object objReq)
        {
            string result = "";
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary = (Dictionary<string, object>)objReq;
            result = "Edit successful";
            return result;
        }


    }
}