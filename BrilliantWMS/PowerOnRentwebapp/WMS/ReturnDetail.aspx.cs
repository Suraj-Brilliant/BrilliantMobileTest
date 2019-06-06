using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using System.Web.Services;
using BrilliantWMS.ToolbarService;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.WMS
{
    public partial class ReturnDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bindgrid();
            Toolbar1.SetUserRights("MaterialRequest", "Summary", "");

            Toolbar1.SetSaveRight(false, "Not Allowed");
            Toolbar1.SetClearRight(false, "Not Allowed");
            Toolbar1.SetImportRight(false, "Not Allowed");
        }

        public void bindgrid()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                DataSet ds = new DataSet();
                ds = Outbound.BindOutboundGridForReturn(profile.DBConnection._constr);
                grdSalesOrder.DataSource = ds;
                grdSalesOrder.DataBind();

                grdSalesOrder.AllowMultiRecordSelection = true;
                grdSalesOrder.AllowRecordSelection = true;
            }
            catch { }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static int WMChangeStatus(string SelectedRec)
        {
            int Result = 0;
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Result = Outbound.ChangeStatusToMarkForReturn(SelectedRec, profile.Personal.UserID,profile.DBConnection._constr);

                BrilliantWMS.WMSOutbound.tReturnHead rtrn = new WMSOutbound.tReturnHead();
                string[] OrdrCnt = SelectedRec.Split(',');
                int TotOrdres = OrdrCnt.Count();
                for (int i = 0; i <= TotOrdres - 1; i++)
                {                    
                    rtrn.SONo = long.Parse(OrdrCnt[i].ToString());
                    rtrn.ReturnBy = profile.Personal.UserID;
                    rtrn.ReturnDate = DateTime.Now;
                    rtrn.CreatedBy = profile.Personal.UserID;
                    rtrn.CreationDate = DateTime.Now;
                    rtrn.Status = 49;

                    long rtrnID = Outbound.SaveReturnHead(rtrn, profile.DBConnection._constr);
                }                
            }
            catch { }
            finally { Outbound.Close(); }

            return Result;
        }
    }
}