using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.UC_StatutoryServices;
using BrilliantWMS.Login;

namespace BrilliantWMS.Tax
{
    public partial class UC_StatutoryDetails : System.Web.UI.UserControl
    {
        public Page ParentPage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindGridStatutoryDetails(long ReferenceID, string ObjectName, long CompanyID)
        {
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                UC_StatutoryServices.iUC_StatutoryInfoClient StatutoryClient = new iUC_StatutoryInfoClient();
                LstStatutoryInfo.DataSource = StatutoryClient.GetStatutoryListToBind(ReferenceID, profile.Personal.UserID.ToString(), ObjectName, CompanyID, profile.DBConnection._constr);
                LstStatutoryInfo.DataBind();
                StatutoryClient.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC_StatutoryDetails", "BindGridStatutoryDetails");
            }
            finally
            {
            }
        }


        public void FinalSaveToStatutoryDetails(long paraReferenceID, string ObjectName, long CompanyID)
        {
            List<tStatutoryDetail> Statutory = new List<tStatutoryDetail>();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                for (int i = 0; i <= LstStatutoryInfo.Items.Count - 1; i++)
                {
                    tStatutoryDetail objstatutory = new tStatutoryDetail();
                    Label lblName = new Label();
                    lblName = (Label)LstStatutoryInfo.Items[i].FindControl("lblname");
                    TextBox txtStatvalue = new TextBox();
                    txtStatvalue = (TextBox)LstStatutoryInfo.Items[i].FindControl("textbox");
                    objstatutory.ObjectName = ObjectName;
                    objstatutory.ReferenceID = paraReferenceID;
                    objstatutory.StatutoryID = Convert.ToInt64(lblName.ToolTip);
                    objstatutory.StatutoryValue = txtStatvalue.Text;
                    objstatutory.Active = "Y";
                    objstatutory.CreatedDate = DateTime.Now;
                    objstatutory.CreatedBy = profile.Personal.UserID.ToString();
                    objstatutory.LastEditBy = profile.Personal.UserID.ToString();
                    objstatutory.LastEditDate = DateTime.Now;
                    Statutory.Add(objstatutory);
                }

                UC_StatutoryServices.iUC_StatutoryInfoClient StatutoryClient = new iUC_StatutoryInfoClient();
                StatutoryClient.FinalSaveToTStatutoryDetails(Statutory.ToArray(), ObjectName, paraReferenceID, profile.Personal.UserID.ToString(), profile.Personal.CompanyID, profile.DBConnection._constr);
                StatutoryClient.Close();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, ParentPage, "UC_StatutoryDetails", "FinalSaveToStatutoryDetails");
            }
            finally
            {
            }
        }

      
    }
}