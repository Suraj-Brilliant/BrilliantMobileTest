using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.AddressInfoService;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;

namespace BrilliantWMS.Address
{
    public partial class UCAddress : System.Web.UI.UserControl
    {
        ResourceManager rm;
        CultureInfo ci;
        public string BillingSeq;
        public string ShippingSeq;
        static string sessionID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Lang"] == "")
            {
                Session["Lang"] = Request.UserLanguages[0];
            }
            loadstring();
            sessionID = Session.SessionID;
            if (hdnbilling.Value == "") hdnbilling.Value = "1";
            BillingSeq = hdnbilling.Value;

            if (hdnshipping.Value == "") hdnshipping.Value = "1";
            ShippingSeq = hdnshipping.Value;
            if (Session["CompanyID"] != null)
            {
                CustomProfile profile = CustomProfile.GetProfile();
                iAddressInfoClient AddressClient = new iAddressInfoClient();
               DataSet ds = AddressClient.GridFillAddressByObjectNameReferenceID(long.Parse(Session["CompanyID"].ToString()), profile.DBConnection._constr);
               DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    GvAddressInfo.DataSource = ds.Tables[0];
                    GvAddressInfo.DataBind();
                   // btnsave.Visible = false;
                }
                else
                {
                    GvAddressInfo.DataSource = null;
                    GvAddressInfo.DataBind();

                }
            }
            if (IsPostBack) {
               
                //BindAddressFromTempData();
            }
           
        }
        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;
           // btnAddress.Value = rm.GetString("AddNew", ci);
            lbladdresslist.Text = rm.GetString("AddressList", ci);
           // btnsave.Text = rm.GetString("Save", ci);
            }


        public void FillAddressByObjectNameReferenceID(string SourceObjectName, long ReferenceID, string TargetObjectName)
        {
            try
            {
                if (hdnAddressTargetObject != null) hdnAddressTargetObject.Value = TargetObjectName;
                CustomProfile profile = CustomProfile.GetProfile();
                iAddressInfoClient AddressClient = new iAddressInfoClient();
                List<SP_GetAddressListToBindGrid_Result> Addresslst = new List<SP_GetAddressListToBindGrid_Result>();
                if (sessionID == null) sessionID = Session.SessionID;
                Addresslst = AddressClient.GetAddressByObjectNameReferenceID(SourceObjectName, ReferenceID, TargetObjectName + "_Address", sessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr).ToList();
                if (GvAddressInfo != null)
                {
                    GvAddressInfo.DataSource = Addresslst;
                    GvAddressInfo.DataBind();

                    if (hdnbilling != null) hdnbilling.Value = Addresslst.Where(add => add.BillIsChecked == "true").FirstOrDefault() == null ? "1" : Addresslst.Where(add => add.BillIsChecked == "true").FirstOrDefault().Sequence.ToString();
                    if (hdnshipping != null) hdnshipping.Value = Addresslst.Where(add => add.ShipIsChecked == "true").FirstOrDefault() == null ? "1" : hdnshipping.Value = Addresslst.Where(add => add.ShipIsChecked == "true").FirstOrDefault().Sequence.ToString();
                }

                AddressClient.Close();
               // btnsave.Visible = false;
            }
            catch (System.Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, this, "AddressInformation", "FillAddressByObjectNameReferenceID");
            }
        }

        protected void BindAddressFromTempData()
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            if (hdnbilling.Value == "") hdnbilling.Value = "1";
            if (hdnshipping.Value == "") hdnshipping.Value = "1";
            // GvAddressInfo.DataSource = AddressClient.GetAddressTempData(hdnAddressTargetObject.Value + "_Address", Convert.ToInt64(hdnbilling.Value), Convert.ToInt64(hdnshipping.Value), Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            GvAddressInfo.DataSource = AddressClient.GetAddressTempData(hdnAddressTargetObject.Value + "_Address", Convert.ToInt64(hdnbilling.Value), Convert.ToInt64(hdnshipping.Value), Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            GvAddressInfo.DataBind();
            AddressClient.Close();
        }

        protected void GvAddressInfo_RebindGrid(object sender, EventArgs e)
        {
            BindAddressFromTempData();
        }
        [WebMethod]
        public static void ClearAddress(string ObjectName, string sessionID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            AddressClient.ClearTempDataFromDB(sessionID, profile.Personal.UserID.ToString(), ObjectName + "_Address", profile.DBConnection._constr);
            AddressClient.Close();
        }

        public void ClearAddress(string ObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            hdnAddressTargetObject.Value = ObjectName;
            AddressClient.ClearTempDataFromDB(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName + "_Address", profile.DBConnection._constr);
            AddressClient.Close();
        }

        public void FinalSaveAddress(ReferenceObjectName _ReferenceObjectName, long ReferenceID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            AddressClient.FinalSaveAddress(Session.SessionID, _ReferenceObjectName.ToString(), ReferenceID, hdnAddressTargetObject.Value + "_Address", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            AddressClient.Close();
        }

        [WebMethod]
        public static void FinalSaveAddress1(ReferenceObjectName _ReferenceObjectName, long ReferenceID, string SessionID, string TargetObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            AddressClient.FinalSaveAddress(SessionID, _ReferenceObjectName.ToString(), ReferenceID, TargetObjectName + "_Address", profile.Personal.UserID.ToString(), profile.DBConnection._constr);
            AddressClient.Close();
        }

        public void MoveAddressToArchive(string Ids, string IsArchive, string CurrentObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iAddressInfoClient AddressClient = new iAddressInfoClient();
            AddressClient.SetAddressArchive(Ids, IsArchive, Convert.ToInt64(profile.Personal.UserID).ToString(), CurrentObjectName + "_Address", sessionID, profile.DBConnection._constr);
            AddressClient.Close();
        }

        public void DefaultAddressColumn(bool bill, bool ship, string BillTitle, string shipTitle)
        {
            /*Col index 9 = bill*/
            GvAddressInfo.Columns[9].Visible = bill;
            GvAddressInfo.Columns[9].HeaderText = BillTitle;

            /*Col index 10 = ship*/
            GvAddressInfo.Columns[10].Visible = ship;
            GvAddressInfo.Columns[10].HeaderText = shipTitle;
        }

        //protected void btnsave_Click(object sender, EventArgs e)
        //{
        //    DataSet ds;
        //    DataTable dt;
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    iAddressInfoClient AddressClient = new iAddressInfoClient();
        //   // FinalSaveAddress(Address.ReferenceObjectName.Account, long.Parse(Session["DepartmentID"].ToString()));
        //    FinalSaveAddress(Address.ReferenceObjectName.Account, long.Parse(Session["CompanyID"].ToString()));
        //    ClearAddress("_Address");
        //    long companyid = long.Parse(Session["CompanyID"].ToString());
        //    ds = AddressClient.GetDeptIDstoUpdateconatct(companyid, profile.DBConnection._constr);
        //    dt = ds.Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            long AddressID = long.Parse(dt.Rows[i]["ID"].ToString());
        //            long Cont1 = long.Parse(dt.Rows[i]["Zone"].ToString());
        //            long cont2 = long.Parse(dt.Rows[i]["Subzone"].ToString());
        //            if (Cont1 != 0)
        //            {
        //                AddressClient.Updatecontactref(Cont1, AddressID, profile.DBConnection._constr);
        //            }
        //            if (cont2 != 0)
        //            {
        //                AddressClient.Updatecontactref(cont2,AddressID, profile.DBConnection._constr);
        //            }
        //        }
                
        //    }

        //    Response.Redirect("AccountMaster.aspx");

        //}
    }

    public enum ReferenceObjectName
    {
        Account,
        Vendor,
        User,
        Warehouse,
        Client
    }

     
 
}