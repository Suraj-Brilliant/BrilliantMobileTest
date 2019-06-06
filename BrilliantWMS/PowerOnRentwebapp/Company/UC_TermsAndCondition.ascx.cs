using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.Services;
/*using BrilliantWMS.UCTermConditionService;*/
using BrilliantWMS.Login;

namespace BrilliantWMS.Company
{
    public partial class UC_TermsAndCondition : System.Web.UI.UserControl
    {
        public Page ParentPage { get; set; }

        public string groupName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddTermsConditiont.Attributes.Add("onclick", "openTermConnditionSearchWindow('" + groupName + "')");
        }

        public void ResetUCTermC(string paraObjectName_Old, long paraReferenceID, string paraUserID, string sessionID, string paraObjectName_New)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                gvTermConditionDetail.DataSource = null;
                gvTermConditionDetail.DataBind();
                hdnObjectName_OldTC.Value = paraObjectName_Old + "TermCondDetail";
                hdnReferenceIDTC.Value = paraReferenceID.ToString();
                hdnUserIDTC.Value = paraUserID;
                hdnSessionIDTC.Value = sessionID;
                hdnObjectName_NewTC.Value = paraObjectName_New + "TermCondDetail";
               /* iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                ucTermClient.ClearTempDataFromDB(hdnSessionIDTC.Value.ToString(), hdnUserIDTC.Value.ToString(), hdnObjectName_OldTC.Value.ToString(), profile.DBConnection._constr);
                ucTermClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "ResetUCTermC");
            }
            finally
            {
            }
        }

        protected void RebindGridTCD(object sender, EventArgs e)
        {
            try { BindGrid(); }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "RebindGrid");
            }
            finally
            {
            }

        }

        protected void BindGrid()
        {
            try { eventSelectedProductIDs(); }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "BindGrid");
            }
            finally
            {
            }
        }

        protected void eventSelectedProductIDs()
        {
            try
            {
                HiddenField hdn = (HiddenField)this.FindControl("hdnSelectedRecTC");
                gvTermConditionDetail.DataSource = null;
                gvTermConditionDetail.DataBind();
                if (hdn.Value != string.Empty)
                {
                    string[] strings = new string[] { };
                    strings = hdn.Value.Split(',');
                    long[] arrayIDs = strings.Select(x => long.Parse(x)).ToArray();
                    if (hdnTermsCondTC.Value == string.Empty)
                    {
                        CustomProfile profile = CustomProfile.GetProfile();
                        /*iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                        gvTermConditionDetail.DataSource = ucTermClient.CreateTermCTempDataList(arrayIDs, hdnSessionIDTC.Value.ToString(), Convert.ToInt32(hdnReferenceIDTC.Value), hdnUserIDTC.Value.ToString(), hdnObjectName_OldTC.Value.ToString(), profile.DBConnection._constr).ToList();
                        gvTermConditionDetail.DataBind();
                        ucTermClient.Close();*/
                    }
                }
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "eventSelectedProductIDs");
            }
            finally
            {
            }
        }

        protected void imgbtnRemove_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                ImageButton imgbtn = (ImageButton)sender;
                gvTermConditionDetail.DataSource = null;
                gvTermConditionDetail.DataBind();
               /* iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                gvTermConditionDetail.DataSource = ucTermClient.RemoveProductFromTempDataList(hdnSessionIDTC.Value.ToString(), hdnUserIDTC.Value.ToString(), Convert.ToInt32(imgbtn.ToolTip), hdnObjectName_OldTC.Value.ToString(), profile.DBConnection._constr);
                gvTermConditionDetail.DataBind();
                ucTermClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "imgbtnRemove_Click");
            }
            finally
            {
            }
        }

        public void FinalSaveTermConditionDetail(long paraReferenceID)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
               /* iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                ucTermClient.FinalSaveToDBtDiscountMappingDetails(hdnSessionIDTC.Value.ToString(), hdnObjectName_OldTC.Value.ToString(), Convert.ToInt32(paraReferenceID), hdnUserIDTC.Value.ToString(), hdnObjectName_NewTC.Value.ToString(), profile.DBConnection._constr);
                ucTermClient.Close();
                gvTermConditionDetail.DataSource = null;
                gvTermConditionDetail.DataBind();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "FinalSaveTermConditionDetail");
            }
            finally
            {
            }
        }

        public void GetTermsConditionListByReferenceIDObjectName(long paraReferenceID)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                /*iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                gvTermConditionDetail.DataSource = ucTermClient.GetTermCListByparaReferenceID(hdnSessionIDTC.Value.ToString(), Convert.ToInt64(paraReferenceID), hdnUserIDTC.Value.ToString(), hdnObjectName_OldTC.Value.ToString(), profile.DBConnection._constr);
                gvTermConditionDetail.DataBind();
                ucTermClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "GetTermsConditionListByReferenceIDObjectName");
            }
            finally
            {
            }
        }

        public void UpdateOrder(string paraSessionID, string paraObjectName_Old, string paraUserID, object order)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary = (Dictionary<string, object>)order;

               /* SP_GetTermConditionListForUCTermCondition_Result updateRec = new SP_GetTermConditionListForUCTermCondition_Result();

                updateRec.Sequence = Convert.ToInt64(dictionary["Sequence"]);
                updateRec.Term = dictionary["Term"].ToString();
                updateRec.Condition = dictionary["Condition"].ToString();
                updateRec.Active = dictionary["Active"].ToString();
                if (updateRec.Active == "Y")
                { updateRec.Active = "Y"; }
                else { updateRec.Active = "N"; }*/
                /*iUCTermConditionClient ucTermClient = new iUCTermConditionClient();
                ucTermClient.UpdateRecord(paraSessionID, paraUserID, updateRec, paraObjectName_Old + "TermCondDetail", profile.DBConnection._constr);
                ucTermClient.Close();*/
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC Terms & Condition ", "UpdateOrder");
            }
            finally
            {
            }
        }
    }
}