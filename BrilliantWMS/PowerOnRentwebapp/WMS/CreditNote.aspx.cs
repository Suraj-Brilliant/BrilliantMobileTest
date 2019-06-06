using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ProductMasterService;
using System.Data;
using System.Configuration;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.WMSInbound;

namespace BrilliantWMS.WMS
{
    public partial class CreditNote : System.Web.UI.Page
    {
        static string ObjectName = "";

        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile(); if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["GRNID"] != null)
                {
                    lblVendorName.Text = "Vendor Name ";
                    lblPurchaseOrderNo.Text = "Purchase Order No ";                    
                    lblPODate.Text = "Purchase Order Date ";
                    long grnID = long.Parse(Session["GRNID"].ToString());
                    UCCreditDate.Date = DateTime.Now;
                    GetPODetailsByGRNID(grnID);
                }
                else if (Session["QCID"] != null)
                {
                    lblVendorName.Text = "Vendor Name ";
                    lblPurchaseOrderNo.Text = "Purchase Order No ";
                    lblPODate.Text = "Purchase Order Date ";
                    UCCreditDate.Date = DateTime.Now;
                    long qcID = long.Parse(Session["QCID"].ToString());
                    GetOrderDetailsByQCID(qcID);
                }
            }
        }

        public void GetPODetailsByGRNID(long grnID)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                long poID = 0,warehouseID=0;
                poID = Inbound.GetPOIDFromGRNID(grnID, profile.DBConnection._constr); hdnSessionPONo.Value = poID.ToString();
                lblPONO.Text = poID.ToString();

                tPurchaseOrderHead PH = new tPurchaseOrderHead();
                PH=Inbound.GetPoHeadByPOID(poID, profile.DBConnection._constr);
                warehouseID = long.Parse(PH.Warehouse.ToString()); hdnWarehouseID.Value = PH.Warehouse.ToString();
                long CompanyID = long.Parse(PH.CompanyID.ToString()); hdnCompanyID.Value = PH.CompanyID.ToString();
                long vendorID =long.Parse(PH.VendorID.ToString());
                string vndrName = Inbound.GetVendorNameByID(vendorID, profile.DBConnection._constr);
                lblVndrName.Text = vndrName.ToString();

                lblPODt.Text = PH.Creationdate.ToString();

                List<WMS_GetCreditNoteDetailByPOID_Result> partLst = new List<WMS_GetCreditNoteDetailByPOID_Result>();
                partLst = Inbound.GetCreditNotePartDetailByPOID(poID,warehouseID, profile.DBConnection._constr).ToList();
                grdSkuList.DataSource = partLst;
                grdSkuList.DataBind();

                decimal totAmt = Inbound.GetTotalofCreditNote(poID, warehouseID, profile.DBConnection._constr);
                lblTotal.Text = totAmt.ToString();

                DataSet ds=new DataSet ();
                ds = Inbound.CheckCreditNote(poID, "PurchaseOrder",profile.DBConnection._constr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnSaveCN.Visible = false;
                    txtRemark.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                    UCCreditDate.Date =Convert.ToDateTime(ds.Tables[0].Rows[0]["CreditNoteDate"].ToString());
                    txtRemark.Enabled = false;
                }                
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "CreditNote.aspx", "GetPODetailsByGRNID"); }
            finally { Inbound.Close(); }
        }

        public void GetOrderDetailsByQCID(long qcID)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                WMS_VW_GetQCDetails qcD = new WMS_VW_GetQCDetails();
                qcD = Inbound.GetQCDetailsByQCID(qcID, profile.DBConnection._constr);
                string objectName = qcD.ObjectName.ToString();
                if (objectName == "PurchaseOrder")
                {
                    long grnid=long.Parse(qcD.OID.ToString());
                    GetPODetailsByGRNID(grnid);
                }
                else if (objectName == "SalesOrder")
                {
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "CreditNote.aspx", "GetOrderDetailsByQCID"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static string WMSaveCreditNote(object objCNHead,long poID, long warehouseID)
        {
            long cnhID = 0;
            int rslt = 0;
            string result = "";
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                tCreditNoteHead ch = new tCreditNoteHead();
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)objCNHead;
                if (HttpContext.Current.Session["GRNID"] != null)
                {
                    ch.ObjectName = "PurchaseOrder";
                    ch.ONO = Convert.ToInt64(d["ONO"]);
                    ch.Remark = d["Remark"].ToString();
                    ch.CreditNoteDate = Convert.ToDateTime(d["CreditNoteDate"]);
                    ch.Total = Convert.ToDecimal(d["Total"]);
                    ch.Company = Convert.ToInt64(d["Company"]);
                    ch.CreatedBy = profile.Personal.UserID;
                    ch.CreationDate = DateTime.Now;

                    cnhID = Inbound.SaveIntotCreditNoteHead(ch, profile.DBConnection._constr);

                    if (cnhID > 0)
                    {
                        rslt = Inbound.SaveCreditNoteDetail(cnhID, poID, warehouseID, profile.DBConnection._constr);
                        if (rslt == 1) { result = "Credit Note saved successfully"; }
                        else { result = "Some error occurred"; }
                    }
                }
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "CreditNote.aspx", "WMSaveCreditNote"); }
            finally { Inbound.Close(); }
            return result;
        }
    }
}