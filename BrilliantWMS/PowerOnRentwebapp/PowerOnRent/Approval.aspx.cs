using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using BrilliantWMS.PORServicePartRequest;
using BrilliantWMS.Login;

namespace BrilliantWMS.PowerOnRent
{
    public partial class Approval : System.Web.UI.Page
    {
        ResourceManager rm;
        CultureInfo ci;

        static string ObjectName = "RequestPartDetail";
        string OrderID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            OrderID=Request.QueryString["REQID"];
            GetInvoiceNo(OrderID);
           
                if (Session["Lang"] == "")
                {
                    Session["Lang"] = Request.UserLanguages[0];
                }
                loadstring();
            
        }

        public void GetInvoiceNo(string OrderID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iPartRequestClient objService = new iPartRequestClient();
            string InvNo = objService.GetInvoiceNoofOrder(long.Parse(OrderID), profile.DBConnection._constr);
            if (InvNo == "0") { txtInvoiceNo.Text = ""; } else { txtInvoiceNo.Text = InvNo; }
        }

        [WebMethod]
        public static string WMSaveApproval(long RequestID, string ApprovalStatus, long APL, string ApprovalRemark,string InvoiceNo)
        {
            iPartRequestClient objService = new iPartRequestClient();
            string result = "";
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();                

                if (ApprovalStatus == "3")
                {
                    objService.UpdatetApprovalTransAfterApproval(APL,RequestID,3,ApprovalRemark,profile.Personal.UserID,InvoiceNo,profile.DBConnection._constr);
                    result = "true";
                }
                else if (ApprovalStatus == "4")
                {
                    objService.UpdatetApprovalTransAfterReject(APL, RequestID, 4, ApprovalRemark, profile.Personal.UserID, profile.DBConnection._constr);
                    result = "true";
                }
                else if (ApprovalStatus == "24")
                {
                    objService.UpdatetApprovalTransAfterApproval(0, RequestID, 24, ApprovalRemark, profile.Personal.UserID, "0",profile.DBConnection._constr);
                    result = "truerev";
                }            
            }
            catch { }
            finally
            { objService.Close(); }            
            return result;
        }

        private void loadstring()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Session["Lang"].ToString());
            rm = new ResourceManager("BrilliantWMS.App_GlobalResources.Lang", Assembly.GetExecutingAssembly());
            ci = Thread.CurrentThread.CurrentCulture;

            //lblapprovalop.Text = rm.GetString("OperationApproval", ci);
            //lblDate.Text = rm.GetString("Date", ci);
            lblApprovRemark.Text = rm.GetString("ApproverComments", ci);
            btnSaveApproval.Value = rm.GetString("Submit", ci);
            //lblApproved.Text = rm.GetString("Approve", ci);
            //lblRejected.Text = rm.GetString("Reject", ci);
            //lblRevison.Text = rm.GetString("Revision", ci);
        }
    }
}