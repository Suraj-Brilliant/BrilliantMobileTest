using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using System.Data.SqlClient;
using System.Web.Services;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;



namespace BrilliantWMS.MasterPage
{
    public partial class CRM : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BrilliantWMS.UserCreationService.iUserCreationClient UserCreationClient = new BrilliantWMS.UserCreationService.iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session.Timeout > 0)
                {

                    userName.Text = profile.Personal.UserName;
                    
                    /*Company Logo New Code*/
                    long CmpnyID = profile.Personal.CompanyID;

                    ClientLogo.ImageUrl = UserCreationClient.GetComppanyLogo(CmpnyID, profile.DBConnection._constr);
                    /*Company Logo New Code*/
                    //ClientLogo.ImageUrl = profile.Personal.CLogoURL;

                    
                    btnLogout.HRef = "../Login/Login.aspx?ID=" + profile.Personal.CompanyID.ToString();


                    //ListItem list = new ListItem();

                    //ListItem[] items = new ListItem[2];
                    //items[0] = new ListItem("English", "0");
                    //items[1] = new ListItem("Arabic", "1");
                    
  
                    //ddlLangauge.Items.AddRange(items);
                    //ddlLangauge.DataBind();

                    //ddlLangauge
                    //BindMenuService.iBindMenuClient objBindMenuClient = new BindMenuService.iBindMenuClient();
                    //BindMenuService.ProBindMenu objProBindMenu = new BindMenuService.ProBindMenu();
                    //List<BindMenuService.ProBindMenu> ListobjProBindMenu = new List<BindMenuService.ProBindMenu>();
                    //objProBindMenu._constP = profile.DBConnection._constr;
                    //objProBindMenu.CompanyCode = profile.Personal.CompanyID;
                    //objProBindMenu.UserCode = profile.Personal.UserID;
                    //ListobjProBindMenu = objBindMenuClient.BindUserMenu(objProBindMenu).ToList();

                    string language = Session["Lang"].ToString(); 
                    if (Session["htmlMenu"] == null)
                    {
                        Session["htmlMenu"] = UserCreationClient.GetHTMLMenuByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                    }
                    
                    if (language == "")
                    {
                        Session["Lang"] = Request.UserLanguages[0];
                        Session["htmlMenu"] = UserCreationClient.GetHTMLMenuByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                    }
                    else if (language == "ar-QA")
                    {                        
                        Session["htmlMenu"] = UserCreationClient.GetHTMLMenuArabicByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                        
                    }
                    else if (language == "en-US")
                    {
                        Session["htmlMenu"] = UserCreationClient.GetHTMLMenuByUserID(profile.Personal.UserID, profile.DBConnection._constr);
                       
                    }

                   

                    //dvMenu.InnerHtml = Session["htmlMenu"].ToString();  Original
                    //dvMenu.InnerHtml = "<ul id='css3menu1' class='topmunu1'><li><a href='#'>Setup &raquo;</a><ul><li><a href='#'>Company Management &raquo;</a><ul><li><a href='#'>Company Master</a></li><li><a href='#'>Customer Master</a></li><li><a href='#'>Statutory Master</a></li><li><a href='../Company/TermsAndConditionMaster.aspx'>Terms And Condition Master</a></li><li><a href='#'>Customer Location</a></li></ul></li><li><a href='#'>User Management &raquo;</a><ul><li><a href='../UserManagement/DepartmentMaster.aspx'>Department Master</a></li><li><a href='../UserManagement/UserCreation.aspx'>User Master</a></li><li><a href='../UserManagement/RoleMaster.aspx'>Role Master</a></li></ul></li><li><a href='#'>Product Management &raquo;</a><ul><li> <a href='../Product/ProductCategoryMaster.aspx'>Category Master</a></li><li><a href='../Product/ProductSubCategoryMaster.aspx'>Sub Category Master</a></li><li><a href='../Product/ProductMaster.aspx'>Product Master</a></li><li><a href='../Product/DiscountMaster.aspx'>Discount Master</a></li></ul></li><li><a href='#'>Warehouse Management &raquo;</a><ul><li><a href='#'>Warehouse Master</a></li></ul></li><li><a href='#'>Account Management &raquo;</a><ul><li><a href='#'>Client Master</a></li><li><a href='#'>Vendor Master</a></li><li><a href='#'>Rate Card</a></li></ul></li><li><a href='#'>Document Management &raquo;</a><ul><li><a href='#'>Document Type Master</a></li><li><a href='#'>Document Master</a></li></ul></li><li><a href='#'>Tax Management &raquo;</a><ul><li><a href='#'>Tax Master</a></li></ul></li><li><a href='#'>Approval Management &raquo;</a><ul><li><a href='#'>Approval Master</a></li></ul></li><li><a href='#'>Workflow Management</a><ul></ul></li><li><a href='#'>Cycle Count</a><ul></ul></li><li><a href='#'>ECommerce Management &raquo;</a><ul><li><a href='#'>Channel Setup</a></li><li><a href='#'>Aggregator</a></li><li><a href='#'>ECommercce Setup</a></li></ul></li><li><a href='#'>Tools And Utility    &raquo;</a><ul><li><a href='#'>SKU Search</a></li><li><a href='#'>Bin Search</a></li><li><a href='#'>Image Import</a></li><li><a href='../Product/EmailTemplate.aspx'>Email And SMS Configuration</a></li></ul></li></ul></li><li><a href='../Inbox/InboxPOR.aspx'>Inbox </a><ul></ul></li><li><a href='#'>Dashboard &raquo;</a><ul><li><a href='#'>Warehouse &raquo;</a><ul><li><a href='#'>Utilization</a></li><li><a href='#'>Task Performance</a></li></ul></li><li><a href='#'>Order Management &raquo;</a><ul><li><a href='#'>Sales Order Status</a></li><li><a href='#'>Purchase Order Status</a></li><li><a href='#'>Sales Order</a></li><li><a href='#'>Purchase Order</a></li></ul></li><li><a href='#'>3PL Billing &raquo;</a><ul><li><a href='#'>Customer Billing</a></li></ul></li><li><a href='#'>Delivery Management &raquo;</a><ul><li><a href='#'>Delivery Dashboard</a></li></ul></li></ul></li><li> <a href='../WMS/Inbound.aspx'>Inbound &raquo;</a><ul><li><a href='../WMS/Inbound.aspx'>Purchase Order</a><ul></ul></li><li><a href='../WMS/GridGRN.aspx'>Goods Receipt Note</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx'>Quality Control</a><ul></ul></li><li><a href='../WMS/LabelPrinting.aspx'>Label Printing</a><ul></ul></li><li><a href='../WMS/PutInList.aspx'>Put In</a><ul></ul></li></ul></li><li> <a href='../WMS/Outbound.aspx'>Outbound &raquo;</a><ul><li><a href='../WMS/Outbound.aspx'>Sales Order</a><ul></ul></li><li><a href='../WMS/PickUpList.aspx'>Pick List</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx'>Quality Control</a><ul></ul></li><li><a href='../WMS/Dispatch.aspx'>Dispatch</a><ul></ul></li></ul></li><li> <a href='../WMS/Transfer.aspx'>Transfer &raquo;</a><ul><li><a href='#'>Internal Transfer</a><ul></ul></li><li><a href='#'>Warehouse To Warehouse Transfer</a><ul></ul></li></ul></li><li> <a href='../WMS/Return.aspx'>Return </a><ul></ul></li><li> <a href='#'>3 PL Billing &raquo;</a><ul><li><a href='#'>Invoice</a><ul></ul></li></ul></li><li><a href='#'>Delivery</a><ul></ul></li><li> <a href='#'>Report &raquo;</a><ul><li><a href='#'>Image Audit Trails</a><ul></ul></li><li><a href='#'>SKU Report</a><ul></ul></li><li><a href='#'>SKU Detail Report</a><ul></ul></li><li><a href='#'>BOM Detail Report</a><ul></ul></li><li><a href='#'>Order Report</a><ul></ul></li><li><a href='#'>Order Detail Report</a><ul></ul></li><li><a href='#'>Order Lead Time Report</a><ul></ul></li><li><a href='#'>User Report</a><ul></ul></li><li><a href='#'>Order Delivery Report</a><ul></ul></li><li><a href='#'>SLA Report</a><ul></ul></li><li><a href='#'>Total Delivery Vs Total Request</a><ul></ul></li><li><a href='#'>Velocity Report</a><ul></ul></li><li><a href='#'>Purchase Order Report</a><ul></ul></li><li><a href='#'>Sales Order Report</a><ul></ul></li><li><a href='#'>Receivable Report</a><ul></ul></li><li><a href='#'>QC Report</a><ul></ul></li><li><a href='#'>Dispatch Report</a><ul></ul></li><li><a href='#'>Transfer Report</a><ul></ul></li><li><a href='#'>Return Report</a><ul></ul></li><li><a href='#'>Warehouse Utilisation Report</a><ul></ul></li><li><a href='#'>Invoice Register</a><ul></ul></li><li><a href='#'>Cycle Count</a><ul></ul></li></ul></li></ul>";
                    if(profile.Personal.CompanyID==10237)
                    {
                        //dvMenu.InnerHtml="<ul id='css3menu1' class='topmunu1'><li><a href='#'>Setup &raquo;</a><ul><li><a href='#'>Company Management &raquo;</a><ul><li><a href='../Account/AccountMaster.aspx'>Company Master</a></li><li><a href='../Account/Customer.aspx'>Customer Master</a></li><li><a href='../Tax/StatutoryMaster.aspx'>Statutory Master</a></li><li><a href='../Company/TermsAndConditionMaster.aspx'>Terms And Condition Master</a></li><li><a href='#'>Customer Location</a></li></ul></li><li><a href='#'>User Management &raquo;</a><ul><li><a href='../UserManagement/DepartmentMaster.aspx'>Department Master</a></li><li><a href='../UserManagement/UserCreation.aspx'>User Master</a></li><li><a href='../UserManagement/RoleMaster.aspx'>Role Master</a></li><li><a href='../UserManagement/DesignationMaster.aspx'>Designation Master</a></li></ul></li><li><a href='#'>Product Management &raquo;</a><ul><li> <a href='../Product/ProductCategoryMaster.aspx'>Category Master</a></li><li><a href='../Product/ProductSubCategoryMaster.aspx'>Sub Category Master</a></li><li><a href='../Product/ProductMaster.aspx'>Product Master</a></li><li><a href='../Product/DiscountMaster.aspx'>Discount Master</a></li></ul></li><li><a href='#'>Warehouse Management &raquo;</a><ul><li><a href='../Warehouse/WarehouseMaster.aspx'>Warehouse Master</a></li></ul></li><li><a href='#'>Account Management &raquo;</a><ul><li><a href='../Account/ClientMaster.aspx'>Client Master</a></li><li><a href='../Account/VendorMaster.aspx'>Vendor Master</a></li><li><a href='../Account/RateCardmaster.aspx'>Rate Card</a></li></ul></li><li><a href='#'>Document Management &raquo;</a><ul><li><a href='../Document/DocumentTypeMaster.aspx'>Document Type Master</a></li><li><a href='../Document/DocumentMaster.aspx'>Document Master</a></li></ul></li><li><a href='#'>Tax Management &raquo;</a><ul><li><a href='../Tax/TaxMaster.aspx'>Tax Master</a></li></ul></li><li><a href='#'>Approval Management &raquo;</a><ul><li><a href='../Approval/ApprovalMaster.aspx'>Approval Master</a></li></ul></li><li><a href='#'>Workflow Management</a><ul></ul></li><li><a href='../Warehouse/CycleCount.aspx'>Cycle Count</a><ul></ul></li><li><a href='#'>ECommerce Management &raquo;</a><ul><li><a href='../Account/ChannelMaster.aspx'>Channel Setup</a></li><li><a href='../Account/AggregatorMaster.aspx'>Aggregator</a></li><li><a href='#'>ECommercce Setup</a></li></ul></li><li><a href='#'>Tools And Utility    &raquo;</a><ul><li><a href='#'>SKU Search</a></li><li><a href='#'>Bin Search</a></li><li><a href='../Product/ImportDSo.aspx'>Image Import</a></li><li><a href='../Product/EmailTemplate.aspx'>Email And SMS Configuration</a></li></ul></li></ul></li><li><a href='../Inbox/InboxPOR.aspx'>Inbox </a><ul></ul></li><li><a href='#'>Dashboard &raquo;</a><ul><li><a href='#'>Warehouse &raquo;</a><ul><li><a href='../POR/DashboardPOR.aspx?invoker=utilization'>Utilization</a></li><li><a href='#'>Task Performance</a></li></ul></li><li><a href='#'>Order Management &raquo;</a><ul><li><a href='../POR/DashboardPOR.aspx?invoker=sostatus'>Sales Order Status</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=postatus'>Purchase Order Status</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=salesorder'>Sales Order</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=purchaseorder'>Purchase Order</a></li></ul></li><li><a href='#'>3PL Billing &raquo;</a><ul><li><a href='#'>Customer Billing</a></li></ul></li><li><a href='#'>Delivery Management &raquo;</a><ul><li><a href='#'>Delivery Dashboard</a></li></ul></li></ul></li><li> <a href='../WMS/Inbound.aspx'>Inbound &raquo;</a><ul><li><a href='../WMS/Inbound.aspx'>Purchase Order</a><ul></ul></li><li><a href='../WMS/GridGRN.aspx?POID=0'>Goods Receipt Note</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx?inv=PO'>Quality Control</a><ul></ul></li><li><a href='../WMS/LabelPrinting.aspx?ID=0'>Label Printing</a><ul></ul></li><li><a href='../WMS/PutInList.aspx'>Put In</a><ul></ul></li></ul></li><li> <a href='../WMS/Outbound.aspx'>Outbound &raquo;</a><ul><li><a href='../WMS/Outbound.aspx'>Sales Order</a><ul></ul></li><li><a href='../WMS/PickUpList.aspx'>Pick List</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx?inv=SO'>Quality Control</a><ul></ul></li><li><a href='../WMS/Dispatch.aspx'>Dispatch</a><ul></ul></li></ul></li><li> <a href='../WMS/Transfer.aspx'>Transfer &raquo;</a><ul><li><a href='#'>Internal Transfer</a><ul></ul></li><li><a href='../WMS/Transfer.aspx'>Warehouse To Warehouse Transfer</a><ul></ul></li></ul></li><li> <a href='../WMS/Return.aspx'>Return </a><ul></ul></li><li> <a href='#'>3 PL Billing &raquo;</a><ul><li><a href='#'>Invoice</a><ul></ul></li></ul></li><li><a href='#'>Delivery</a><ul></ul></li><li> <a href='#'>Report &raquo;</a><ul><li><a href='#'>Warehouse &raquo;</a><ul><li><a href='#'>Velocity Report</a></li><li><a href='#'>Purchase Order List</a></li><li><a href='#'>Purchase Order Detail</a></li><li><a href='#'>Sales Order List</a></li><li><a href='#'>Sales Order Detail</a></li><li><a href='#'>GRN List</a></li><li><a href='#'>GRN Detail</a></li><li><a href='#'>QC List</a></li><li><a href='#'>SLA Report</a></li><li><a href='#'>Put In List</a></li><li><a href='#'>Put In Detail</a></li><li><a href='#'>Label Printing List</a></li><li><a href='#'>Label Printing Detail</a></li><li><a href='#'>Pick Up List</a></li><li><a href='#'>Pick Up Detail</a></li><li><a href='#'>Transfer List</a></li><li><a href='#'>Transfer Detail</a></li><li><a href='#'>Return List</a></li><li><a href='#'>Return Detail</a></li><li><a href='#'>Dispatch List</a></li><li><a href='#'>Dispatch Detail</a></li><li><a href='#'>Warehouse Utilization Report</a></li><li><a href='#'>ABC Analysis</a></li><li><a href='#'>FSN Analysis</a></li><li><a href='#'>Aging Report</a></li><li><a href='#'>Warehouse Location  List </a></li><li><a href='#'>Lead Time Report - Inbound</a></li><li><a href='#'>Lead Time Report - Outbound </a></li><li><a href='#'>Cycle Count Report </a></li><li><a href='#'>User list Report</a></li></ul></li><li><a href='#'>General &raquo;</a><ul><li><a href='#'>Image Audit Trails</a></li><li><a href='#'>SKU Report</a></li><li><a href='#'>SKU Detail Report</a></li><li><a href='#'>BOM Detail Report</a></li><li><a href='#'>User Report</a></li></ul></li><li><a href='#'>Order Management &raquo;</a><ul><li><a href='#'>Order Report</a></li><li><a href='#'>Order Detail Report</a></li><li><a href='#'>Order Lead Time Report</a></li><li><a href='#'>SLA Report</a></li><li><a href='#'>Total Delivery Vs. Total Request Report</a></li></ul></li><li><a href='#'>Delivery Management &raquo;</a><ul><li><a href='#'>Order Delivery Report</a></li><li><a href='#'>SLA Report</a></li><li><a href='#'>Total Delivery Vs. Total Request Report</a></li></ul></li></ul></li></ul>";
                        dvMenu.InnerHtml = "<ul id='css3menu1' class='topmunu1'>   <li><a href='#'>Setup &raquo;</a><ul>   <li><a href='#'>Company Management &raquo;</a>   <ul><li><a href='../Account/AccountMaster.aspx'>Company Master</a></li><li><a href='../Account/Customer.aspx'>Customer Master</a></li><li><a href='../Tax/StatutoryMaster.aspx'>Statutory Master</a></li><li><a href='../Company/TermsAndConditionMaster.aspx'>Terms And Condition Master</a></li></ul></li><li><a href='#'>User Management &raquo;</a><ul><li><a href='../UserManagement/DepartmentMaster.aspx'>Department Master</a></li><li><a href='../UserManagement/UserCreation.aspx'>User Master</a></li><li><a href='../UserManagement/RoleMaster.aspx'>Role Master</a></li><li><a href='../UserManagement/DesignationMaster.aspx'>Designation Master</a></li></ul></li><li><a href='#'>Product Management &raquo;</a><ul><li> <a href='../Product/ProductCategoryMaster.aspx'>Category Master</a></li><li><a href='../Product/ProductSubCategoryMaster.aspx'>Sub Category Master</a></li><li><a href='../Product/ProductMaster.aspx'>Product Master</a></li><li><a href='../Product/DiscountMaster.aspx'>Discount Master</a></li></ul></li><li><a href='#'>Warehouse Management &raquo;</a><ul><li><a href='../Warehouse/WarehouseMaster.aspx'>Warehouse Master</a></li></ul></li><li><a href='#'>Account Management &raquo;</a><ul><li><a href='../Account/ClientMaster.aspx'>Client Master</a></li><li><a href='../Account/VendorMaster.aspx'>Vendor Master</a></li><li><a href='../Account/RateCardmaster.aspx'>Rate Card</a></li></ul></li><li><a href='#'>Document Management &raquo;</a><ul><li><a href='../Document/DocumentTypeMaster.aspx'>Document Type Master</a></li><li><a href='../Document/DocumentMaster.aspx'>Document Master</a></li></ul></li><li><a href='#'>Tax Management &raquo;</a><ul><li><a href='../Tax/TaxMaster.aspx'>Tax Master</a></li></ul></li><li><a href='#'>Approval Management &raquo;</a><ul><li><a href='../Approval/ApprovalMaster.aspx'>Approval Master</a></li></ul></li>      <li><a href='../Warehouse/CycleCount.aspx'>Cycle Count</a><ul></ul></li>   <li><a href='#'>Tools And Utility    &raquo;</a><ul><li><a href='../Product/ImportDSo.aspx'>Image Import</a></li><li><a href='../Product/EmailTemplate.aspx'>Email And SMS Configuration</a></li></ul></li></ul></li><li><a href='../Inbox/InboxPOR.aspx'>Inbox </a><ul></ul></li>   <li><a href='#'>Dashboard &raquo;</a> <ul> <li><a href='#'>Warehouse &raquo;</a>  <ul> <li><a href='../POR/DashboardPOR.aspx?invoker=utilization'>Utilization</a></li>  <li><a href='#'>Task Performance</a></li> </ul>  </li>  <li><a href='#'>Order Management &raquo;</a><ul><li><a href='../POR/DashboardPOR.aspx?invoker=sostatus'>Sales Order Status</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=postatus'>Purchase Order Status</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=salesorder'>Sales Order</a></li><li><a href='../POR/DashboardPOR.aspx?invoker=purchaseorder'>Purchase Order</a></li> </ul></li></ul></li><li> <a href='../WMS/Inbound.aspx'>Inbound &raquo;</a><ul><li><a href='../WMS/GridGRN.aspx?POID=0'>Goods Receipt Note</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx?inv=PO'>Quality Control</a><ul></ul></li><li><a href='../WMS/PutInList.aspx'>Put In</a><ul></ul></li></ul></li><li> <a href='../WMS/Outbound.aspx'>Outbound &raquo;</a> <ul> <li><a href='../WMS/Outbound.aspx'>Sales Order</a><ul></ul></li> <li><a href='../WMS/PickUpList.aspx'>Pick List</a><ul></ul></li><li><a href='../WMS/QualityControl.aspx?inv=SO'>Quality Control</a><ul></ul></li> <li><a href='../WMS/Dispatch.aspx'>Dispatch</a><ul></ul></li></ul></li><li> <a href='../WMS/Transfer.aspx'>Transfer &raquo;</a>  <ul>   <li><a href='#'>Internal Transfer</a><ul></ul></li> <li><a href='../WMS/Transfer.aspx'>Warehouse To Warehouse Transfer</a><ul></ul></li> </ul></li> <li> <a href='../WMS/Return.aspx'>Return </a><ul></ul></li><li> <a href='#'>Report &raquo;</a><ul><li><a href='#'>Warehouse  &raquo;</a><ul><li><a href='../PowerOnRent/CommonReport.aspx?invoker=grn'>GRN Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=qc'>QC Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=putin'>PutIn Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=order'>Sales Order Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=pickup'>PickUp Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=dispatch'>Dispatch Report</a></li><li><a href='#'>Transfer Report</a></li><li><a href='#'>Return Report</a></li><li><a href='#'>Warehouse Utilization Report</a></li><li><a href='#'>ABC Analysis</a></li><li><a href='#'>FSN Analysis</a></li><li><a href='#'>Aging Report</a></li><li><a href='#'>Warehouse Location  List </a></li><li><a href='#'>Lead Time Report - Inbound</a></li><li><a href='#'>Lead Time Report - Outbound </a></li><li><a href='#'>Cycle Count Report </a></li></ul></li><li><a href='#'>General &raquo;</a><ul><li><a href='#'>Image Audit Trails</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=sku'>SKU Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=SkuDetails'>SKU Detail Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=BomDetail'>BOM Detail Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=user'>User Report</a></li></ul></li><li><a href='#'>Order Management &raquo;</a><ul><li><a href='#'>Order Report</a></li><li><a href='#'>Order Detail Report</a></li><li><a href='../PowerOnRent/CommonReport.aspx?invoker=sla'>Order Lead Time Report</a></li><li><a href='#'>SLA Report</a></li><li><a href='#'>Total Delivery Vs. Total Request Report</a></li></ul></li><li><a href='#'>Delivery Management &raquo;</a><ul><li><a href='#'>Order Delivery Report</a></li><li><a href='#'>SLA Report</a></li><li><a href='#'>Total Delivery Vs. Total Request Report</a></li></ul></li></ul></li></ul>";
                    }
                    else{
                    dvMenu.InnerHtml = UserCreationClient.GetAllHTMLMenu(profile.Personal.UserID,profile.DBConnection._constr);
                    }
                    
                    if (profile.Personal.ProfileImg != null)
                    {
                        Session["ProfileImgMasterPg"] = profile.Personal.ProfileImg;
                        ImgProfileMasterPg.Src = "../Image1.aspx";
                        ImgProfileMasterPg.Src = "../App_Themes/Blue/img/User1.png";
                    }
                    else
                    {
                        ImgProfileMasterPg.Src = "../App_Themes/Blue/img/Male.png";
                        if (profile.Personal.Gender != null)
                        {
                            if (profile.Personal.Gender != "M") { ImgProfileMasterPg.Src = "../App_Themes/Blue/img/Female.png"; }
                        }
                    }
                }
                else
                {
                    BrilliantWMS.LoginService.iLoginClient loginclient = new BrilliantWMS.LoginService.iLoginClient();
                    loginclient.ClearTempDataBySessionID(Session.SessionID, profile.Personal.UserID.ToString(), profile.DBConnection._constr);
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("../Login/Login.aspx?TimeOut=true&ID=" + profile.Personal.CompanyID.ToString(), false);
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, "Master Page", "Bind Menu");
            }
            finally { UserCreationClient.Close(); }
        }

        [WebMethod]
        public static void WMClearSession()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
        }

        
    }
}