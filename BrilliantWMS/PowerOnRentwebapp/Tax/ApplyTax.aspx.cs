using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UCApplyTaxService;
using BrilliantWMS.AddToCartService;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.Tax
{
    public partial class ApplyTax : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /*New Change For WMS Start*/
                CartBind();
                /*New Change For WMS End*/
                BindGrid();
                lblTaxableAmount.Text = Request.QueryString["TaxableAmt"].ToString();
            }
        }
        protected void CartBind()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            AddToCartService.iAddToCartClient AddToCartService = new iAddToCartClient();     
            long Seq=long.Parse(Request.QueryString["CartSeq"].ToString());
            string ProductID = Request.QueryString["PrdID"].ToString();
            if (ProductID == "undefined") ProductID = "0";
            List<SP_GetCartProductDetail_Result> lst = new List<SP_GetCartProductDetail_Result>();
            lst = AddToCartService.GetAddToCartListBySequence(ProductID,Seq, Session.SessionID, profile.Personal.UserID.ToString(), Request.QueryString["Object"], Request.QueryString["Object"], 0, profile.DBConnection._constr).ToList();
            AddToCartService.Close();
        }
        protected void BindGrid()
        {
            UCApplyTaxService.iUCApplyTaxClient ApplyTaxServie = new UCApplyTaxService.iUCApplyTaxClient();
            try
            {
                //if (Request.QueryString["CartSeq"] != null && Request.QueryString["Object"] != null && (hdnTaxSelectedRec.Value == "0" || hdnTaxSelectedRec.Value == ""))
                if (Request.QueryString["CartSeq"] != null && Request.QueryString["Object"] != null && hdnTaxIsUpdate.Value == "")
                {
                    //if (hdnTaxCartSequence.Value != "" && (hdnTaxSelectedRec.Value == "0" || hdnTaxSelectedRec.Value == "") && hdnTaxIsUpdate.Value == "false")
                    CustomProfile profile = CustomProfile.GetProfile();

                    List<TempCartProductLevelTaxDetail> lst = new List<TempCartProductLevelTaxDetail>();
                   // lst = ApplyTaxServie.GetTaxListBySequence(Request.QueryString["Object"].ToString(), Session.SessionID, Convert.ToInt64(Request.QueryString["CartSeq"].ToString()), profile.DBConnection._constr).ToList();
                    string ProductID = Request.QueryString["PrdID"].ToString();
                    if (ProductID == "undefined") ProductID = "0";
                    long PrdID = Convert.ToInt64(ProductID);
                    lst = ApplyTaxServie.GetTaxListBySequence(Request.QueryString["Object"].ToString(), Session.SessionID, PrdID, profile.DBConnection._constr).ToList();
                    ApplyTaxServie.Close();
                    GridTaxList.DataSource = lst;
                    GridTaxList.DataBind();
                    hdnTaxSelectedRec.Value = "";
                    lblTaxAmount.Text = (from result in lst
                                         select result.TaxAmount).Sum().ToString();

                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ApplyTax.aspx.cs", "BindGrid");
            }
            finally { ApplyTaxServie.Close(); }
        }

        protected void RebindGrid(object sender, EventArgs e)
        { BindGrid(); }

        protected void UpdateRecord(object sender, Obout.Grid.GridRecordEventArgs e)
        {
            UCApplyTaxService.iUCApplyTaxClient ApplyTaxServie = new UCApplyTaxService.iUCApplyTaxClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();   
                object key = "Save";
                if (e.Record[key].ToString() == "N")
                {
                    string ProductID = Request.QueryString["PrdID"].ToString();
                    if (ProductID == "undefined") ProductID = "0";
                    long PrdID = Convert.ToInt64(ProductID);
                    List<SP_CartBeforeUpdateProductLevelTaxDetail_Result> lst = new List<SP_CartBeforeUpdateProductLevelTaxDetail_Result>();
                   // lst = ApplyTaxServie.GetCalculatedTaxListBeforeUpdate(Request.QueryString["Object"].ToString(), Session.SessionID, Convert.ToInt64(Request.QueryString["CartSeq"].ToString()), Convert.ToDecimal(lblTaxableAmount.Text), hdnTaxSelectedRec.Value.ToString(), profile.DBConnection._constr).ToList();
                    lst = ApplyTaxServie.GetCalculatedTaxListBeforeUpdate(Request.QueryString["Object"].ToString(), Session.SessionID, PrdID, Convert.ToDecimal(lblTaxableAmount.Text), hdnTaxSelectedRec.Value.ToString(), profile.DBConnection._constr).ToList();
                    ApplyTaxServie.Close();
                    GridTaxList.DataSource = lst;
                    GridTaxList.DataBind();
                }
                else if (e.Record[key].ToString() == "Save") {
                    string TotalTaxAmount=UpdateTaxAmount();
                    ApplyTaxServie.Close();
                     
                    string obj=Request.QueryString["Object"].ToString();
                    if (obj == "PurchaseOrderTax")
                    {
                        BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
                        //iInboundClient Inbound = new iInboundClient();
                        BrilliantWMS.WMSInbound.WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMSInbound.WMS_SP_GetPartDetail_ForPO_Result();
                        //WMS_SP_GetPartDetail_ForPO_Result PartRequest = new WMS_SP_GetPartDetail_ForPO_Result();
                        PartRequest.Sequence = Convert.ToInt64(Request.QueryString["CartSeq"].ToString());
                        PartRequest.TotalTaxAmount = Convert.ToDecimal(TotalTaxAmount);
                        decimal TtlAftrtax = Convert.ToDecimal(TotalTaxAmount) + Convert.ToDecimal(Request.QueryString["TaxableAmt"].ToString());
                        PartRequest.AmountAfterTax = TtlAftrtax;
                        //string obj = Request.QueryString["Object"].ToString();
                       // if (obj == "PurchaseOrderTax")
                            obj = "PurchaseOrder";
                       // else obj = "SalesOrderTax";
                        Inbound.UpdatePartRequest_TempData13(Session.SessionID, obj, profile.Personal.UserID.ToString(), PartRequest, profile.DBConnection._constr);
                        Inbound.Close();
                    }
                    else if (obj == "SalesOrderTax")
                    {
                        BrilliantWMS.WMSOutbound.iOutboundClient Outbound =new WMSOutbound.iOutboundClient ();
                        BrilliantWMS.WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result PartRSO =new WMSOutbound.WMS_SP_GetPartDetail_ForSO_Result ();
                        PartRSO.Sequence = Convert.ToInt64(Request.QueryString["CartSeq"].ToString());
                        PartRSO.TotalTaxAmount = Convert.ToDecimal(TotalTaxAmount);
                        decimal TtlAftrtax = Convert.ToDecimal(TotalTaxAmount) + Convert.ToDecimal(Request.QueryString["TaxableAmt"].ToString());
                        PartRSO.AmountAfterTax = TtlAftrtax;
                        obj = "SalesOrder";
                        Outbound.UpdatePartRequest_TempData13SO(Session.SessionID, obj, profile.Personal.UserID.ToString(), PartRSO, profile.DBConnection._constr);
                        Outbound.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "UCApplyTax", "UpdateRecord");
            }
            finally
            {   }
        }

        public void ResetUCApplyTax(long CartSequence, string CurrentObjectName, string SessionID, decimal TaxableAmount, string UserID)
        {
            UCApplyTaxService.iUCApplyTaxClient ApplyTaxServie = new UCApplyTaxService.iUCApplyTaxClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ApplyTaxServie.ClearTempDataByCurrentObjectSessionID(CurrentObjectName + "ProductDetail", Session.SessionID, profile.DBConnection._constr);
            }
            catch (System.Exception ex)
            { Login.Profile.ErrorHandling(ex, this, "UCApplyTax", "ResetUCApplyTax"); }

            finally
            { ApplyTaxServie.Close(); }
        }

        public string UpdateTaxAmount()
        {
            UCApplyTaxService.iUCApplyTaxClient ApplyTaxServie = new UCApplyTaxService.iUCApplyTaxClient();
            string TotalTaxAmount = "0";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                string ProductID = Request.QueryString["PrdID"].ToString();
                if (ProductID == "undefined") ProductID = "0";
                long PrdID = Convert.ToInt64(ProductID);
                
               // TotalTaxAmount = ApplyTaxServie.UpdateCalculatedTaxList(Request.QueryString["Object"].ToString(), Session.SessionID, Convert.ToInt64(Request.QueryString["CartSeq"].ToString()), Convert.ToDecimal(lblTaxableAmount.Text), hdnTaxSelectedRec.Value.ToString(), profile.DBConnection._constr);
                TotalTaxAmount = ApplyTaxServie.UpdateCalculatedTaxList(Request.QueryString["Object"].ToString(), Session.SessionID, PrdID, Convert.ToDecimal(lblTaxableAmount.Text), hdnTaxSelectedRec.Value.ToString(), profile.DBConnection._constr);
                ApplyTaxServie.Close();

                AddToCartService.iAddToCartClient addtocartclient = new AddToCartService.iAddToCartClient();
                SP_GetCartProductDetail_Result updateRec = new SP_GetCartProductDetail_Result();
               // updateRec = addtocartclient.GetCartProductDetailBySequence(Session.SessionID, Request.QueryString["Object"].ToString(), Convert.ToInt64(Request.QueryString["CartSeq"].ToString()), profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                updateRec = addtocartclient.GetCartProductDetailBySequence(Session.SessionID, Request.QueryString["Object"].ToString(), PrdID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                if (updateRec != null)
                {
                    updateRec.TotalTaxAmount = Convert.ToDecimal(TotalTaxAmount);
                    updateRec.AmountAfterTax = updateRec.AmountAfterDiscount + Convert.ToDecimal(TotalTaxAmount);
                    addtocartclient.UpdateRecord(Session.SessionID, Request.QueryString["Object"].ToString(), profile.Personal.UserID.ToString(), updateRec, profile.DBConnection._constr);
                    addtocartclient.Close();
                    GridTaxList.DataSource = null;
                    GridTaxList.DataBind();
                    hdnTaxSelectedRec.Value = "";
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "UCApplyTax", "UpdateTaxAmount");
            }
            finally
            { ApplyTaxServie.Close(); }
            return TotalTaxAmount;
        }
    }
}