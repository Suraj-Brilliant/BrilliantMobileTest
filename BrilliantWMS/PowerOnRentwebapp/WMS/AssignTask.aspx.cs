using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
//using WebClientElegantCRM.UCAssignTaskService;
using System.Data;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.PORServiceUCCommonFilter;


namespace BrilliantWMS.WMS
{
    public partial class AssignTask : System.Web.UI.Page
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            if (profile.Personal.Theme == null || profile.Personal.Theme == string.Empty) { Page.Theme = "Blue"; } else { Page.Theme = profile.Personal.Theme; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack != true)
            {
                string SelectedRecords = Session["SelectedRec"].ToString();
                string ObjectName = Session["ObjectName"].ToString();

                FillObjectList(SelectedRecords, ObjectName);
                ListItem lst = new ListItem { Text = "-Select-", Value = "0" };
               // ddlActivityType.Items.Insert(0, lst);
               // ddlAssignTo.Items.Insert(0, lst);
                ddlObjectName.Items.Insert(0, lst);

                UCReminderDate.DateIsRequired(false, "x", "");
                UCReminderDate.startdate(DateTime.Now);

                UC_ECDate.DateIsRequired(true, "Save", "Select ECD");
                UC_ECDate.startdate(DateTime.Now);                

            }
        }

        public void FillObjectList(string SelectedRecords, string ObjectName)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            //iInboundClient Inbound = new iInboundClient();
            iUCCommonFilterClient objService = new iUCCommonFilterClient();
            List<vGetUserProfileByUserID> UsersList = new List<vGetUserProfileByUserID>();
            try
            {
                DataSet ds = new DataSet();
                ds = Inbound.GetNextObject(SelectedRecords, ObjectName, profile.Personal.CompanyID, profile.DBConnection._constr);
                ddlObjectName.DataSource = ds;
                ddlObjectName.DataBind();
                long WarehouseID = 0;
                string[] onePo = SelectedRecords.Split(','); long POID = 0;
                if(ObjectName=="PurchaseOrder")
                {
                    POID = long.Parse(onePo[0].ToString());
                    BrilliantWMS.WMSInbound.tPurchaseOrderHead POHead = new WMSInbound.tPurchaseOrderHead();
                    //tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                    POHead = Inbound.GetPoHeadByPOID(POID, profile.DBConnection._constr);
                    WarehouseID = long.Parse(POHead.Warehouse.ToString());
                }
                else if (ObjectName == "GRN")
                {
                    long GRNID=long.Parse(onePo[0].ToString());
                    BrilliantWMS.WMSInbound.WMS_VW_GetGRNDetails GrnLst = new WMSInbound.WMS_VW_GetGRNDetails();
                    //WMS_VW_GetGRNDetails GrnLst = new WMS_VW_GetGRNDetails();
                    GrnLst = Inbound.GetGRNDetailsByGRNIDGRNMenu(GRNID, profile.DBConnection._constr);
                    POID = long.Parse(GrnLst.OID.ToString());

                    BrilliantWMS.WMSInbound.tPurchaseOrderHead POHead = new WMSInbound.tPurchaseOrderHead();
                    //tPurchaseOrderHead POHead = new tPurchaseOrderHead();
                    POHead = Inbound.GetPoHeadByPOID(POID, profile.DBConnection._constr);
                    WarehouseID = long.Parse(POHead.Warehouse.ToString());
                }
                else if (ObjectName == "QC")
                {
                    long QCID = long.Parse(onePo[0].ToString());
                    BrilliantWMS.WMSInbound.WMS_VW_GetQCDetails QClist = new WMSInbound.WMS_VW_GetQCDetails();
                    //WMS_VW_GetQCDetails QClist = new WMS_VW_GetQCDetails();
                    QClist = Inbound.GetQCDetailsByQCID(QCID, profile.DBConnection._constr);
                    WarehouseID =long.Parse(QClist.WarehouseID.ToString());
                }
                else if (ObjectName == "SalesOrder")
                {
                    long SOID = long.Parse(onePo[0].ToString());
                    BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
                    BrilliantWMS.WMSOutbound.tOrderHead soHead= new WMSOutbound.tOrderHead ();
                    //tOrderHead soHead = new tOrderHead();
                    soHead=Outbound.GetSoHeadBySOID(SOID, profile.DBConnection._constr);
                    WarehouseID = long.Parse(soHead.StoreId.ToString());
                }
                UsersList = objService.GetUserListByWarehouseID(WarehouseID, profile.DBConnection._constr).ToList();
                UsersList = UsersList.Where(x => x.userID == profile.Personal.UserID).ToList();
                vGetUserProfileByUserID select = new vGetUserProfileByUserID() { userID = 0, userName = "-Select-" };
                UsersList.Insert(0, select);
                ddlAssignTo.DataSource = UsersList;
                ddlAssignTo.DataBind();
            }
            catch { }
            finally { Inbound.Close(); }
            //if (Request.QueryString["invoker"] != null) CurrentObject = Request.QueryString["invoker"].ToString();
            //UCAssignTaskService.iUCAssignTaskClient ucAssignTaskService = new UCAssignTaskService.iUCAssignTaskClient();
            //ddlObjectName.DataSource = ucAssignTaskService.GetObjectToBind(CurrentObject, profile.DBConnection._constr);
            //ddlObjectName.DataBind();
            //ucAssignTaskService.Close();
        }

        //[WebMethod]
        //public static List<SP_AssignToList_Result> FillAssignTo(string ObjectName)
        //{
        //    UCAssignTaskService.iUCAssignTaskClient ucAssignTaskService = new UCAssignTaskService.iUCAssignTaskClient();

        //    CustomProfile profile = CustomProfile.GetProfile();
        //    List<SP_AssignToList_Result> objAssignto = new List<SP_AssignToList_Result>();
        //    string CheckAccessLevel = "";
        //    CheckAccessLevel = ucAssignTaskService.CheckAccessLeve(ObjectName, profile.Personal.UserID, profile.DBConnection._constr);

        //    if (CheckAccessLevel == "True")
        //    {
        //        objAssignto = ucAssignTaskService.GetAssignToList(ObjectName, profile.Personal.UserID, profile.DBConnection._constr).ToList();
        //    }
        //    else
        //    {
        //        SP_AssignToList_Result ObjFillAssignTo = new SP_AssignToList_Result();
        //        ObjFillAssignTo.Assignto = "Report To Supervisor";
        //        ObjFillAssignTo.ID = 0;
        //        objAssignto.Add(ObjFillAssignTo);
        //    }
        //    return objAssignto;
        //}

        //[WebMethod]
        //public static List<SP_GetActivityList_Result> FillActivity(string ObjectName)
        //{
        //    UCAssignTaskService.iUCAssignTaskClient ucAssignTaskService = new UCAssignTaskService.iUCAssignTaskClient();
        //    CustomProfile profile = CustomProfile.GetProfile();
        //    List<SP_GetActivityList_Result> ObjFillActivity = new List<SP_GetActivityList_Result>();
        //    ObjFillActivity = ucAssignTaskService.GetActivityList(ObjectName, profile.DBConnection._constr).ToList();
        //    return ObjFillActivity;
        //}

        [WebMethod]
        public static string PMSaveAssignTask(string ObjectName, string JobCardName, string AssignTo, DateTime UC_ECDate, string Remarks, string chkbxEmail, DateTime UCReminderDate, string priority)
        {
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            //iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                BrilliantWMS.WMSInbound.tTaskDetail objtTaskDetail = new WMSInbound.tTaskDetail();
              //  tTaskDetail objtTaskDetail = new tTaskDetail();
                bool exeresult = false;

                objtTaskDetail.ParentTaskID = 0;
                objtTaskDetail.ObjectName = ObjectName;
                objtTaskDetail.ReferenceID = 0;
        //        objtTaskDetail.ActivityID = Convert.ToInt64(ActivityID);
        //        objtTaskDetail.ActivitySubID = Convert.ToInt64(SubActivityID);

                objtTaskDetail.AssignTo = AssignTo == "0.1" ? null : AssignTo;
                objtTaskDetail.TaskDate = DateTime.Now;
                objtTaskDetail.Priority = priority;

                if (UC_ECDate != Convert.ToDateTime("01/01/0001"))
                {
                    objtTaskDetail.TaskECD = UC_ECDate;
                }

                objtTaskDetail.TaskRemark = Remarks;
                objtTaskDetail.AlertType = "Null";
                if (UCReminderDate != Convert.ToDateTime("01/01/0001"))
                  {
                    objtTaskDetail.AlertDateTime = UCReminderDate;
                }
                else
                {
                    objtTaskDetail.AlertDateTime = null;
                }
                objtTaskDetail.ActionTakenBy = null;
                objtTaskDetail.ActionTakenDate = null;
                objtTaskDetail.StatusID = null;
                objtTaskDetail.ReasonID = null;
                objtTaskDetail.ActionTakenRemark = "";
                objtTaskDetail.CreatedBy = profile.Personal.UserID.ToString();
                objtTaskDetail.CreationDate = DateTime.Now;
                objtTaskDetail.LastModifiedBy = "";
                objtTaskDetail.LastModifiedDate = null;
                objtTaskDetail.Active = "Y";

                objtTaskDetail.JobCardName = JobCardName;
                exeresult = Inbound.SaveAssignedTask(objtTaskDetail, HttpContext.Current.Session["ObjectName"].ToString(), HttpContext.Current.Session["SelectedRec"].ToString(), profile.DBConnection._constr);        
                return exeresult.ToString();
            }

            catch (System.Exception ex)
            {
                //Login.Profile.ErrorHandling(ex, this, "UCAssignTask", "btnSave_Click");
                return "false";
            }
            finally { Inbound.Close(); }

        }

    }
}