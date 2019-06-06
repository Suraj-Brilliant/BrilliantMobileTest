using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BrilliantWMS.Login;
using BrilliantWMS.UC_ApprovalService;
namespace BrilliantWMS.Approval
{
    public partial class UC_Approval : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void chekcLoginUserApprovalLevel(string ObjectName, long ReferenceID)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            tApprovalDetail approvalDetail = new tApprovalDetail();
            iUC_ApprovalClient UC_ApprovalService = new iUC_ApprovalClient();
            approvalDetail = UC_ApprovalService.chekcApprovalPermission(ObjectName, ReferenceID, profile.Personal.UserID, profile.DBConnection._constr);
            btnApproval.Attributes.Add("class", "Off FixWidth");
            if (approvalDetail != null)
            {
                btnApproval.Attributes.Add("class", "FixWidth");
                hdnApprovalDetailIDs.Value = approvalDetail.ID.ToString();
            }
        }

        public string FinalUpdateApproval(string status, string remark, string tapprovalIDs, long StatusChangedBy)
        {
            CustomProfile profile = CustomProfile.GetProfile();
            iUC_ApprovalClient UC_ApprovalService = new iUC_ApprovalClient();
            UC_ApprovalService.FinalUpdateUCApproval(status, remark, tapprovalIDs, Convert.ToInt64(StatusChangedBy), profile.DBConnection._constr);
            return "";
        }
    }
}