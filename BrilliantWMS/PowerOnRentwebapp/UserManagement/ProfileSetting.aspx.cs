using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BrilliantWMS.DesignationService;
using BrilliantWMS.UserCreationService;
using System.Web.Services;
using BrilliantWMS.Login;
using System.IO;

namespace BrilliantWMS.UserManagement
{
    public partial class ProfileSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UCAddress1.DefaultAddressColumn(true, false, "Default", "Shipping");
            if (!IsPostBack)
            {
                GetUserByID();
            }

            this.UCToolbar1.evClickSave += pageSave;
            this.UCToolbar1.evClickClear += pageClear;
        }

        protected void Page_PreInit(Object sender, EventArgs e)
        { Page.Theme = "Blue"; }

        private void GetUserByID()
        {
            BrilliantWMS.UserCreationService.iUserCreationClient userClient = new BrilliantWMS.UserCreationService.iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                
                BrilliantWMS.UserCreationService.vGetUserProfileByUserID objuser = new BrilliantWMS.UserCreationService.vGetUserProfileByUserID();
                objuser = userClient.GetUserProfileByUserID(profile.Personal.UserID, profile.DBConnection._constr);

                lblEmpNo.Text = objuser.EmployeeID;
                lblUserName.Text = objuser.Title != null ? objuser.Title + " " + objuser.userName : objuser.userName;
                lblDepartment.Text = objuser.deptname;
                lblDesignation.Text = objuser.desiName;
                lblDOB.Text = objuser.DateOfBirth == null ? "" : Convert.ToDateTime(objuser.DateOfBirth).ToString("dd-MMM-yyyy");
                lblDOJ.Text = objuser.DateOfJoining == null ? "" : Convert.ToDateTime(objuser.DateOfJoining).ToString("dd-MMM-yyyy");
                txtPhone.Text = objuser.PhoneNo;
                txtMobile.Text = objuser.MobileNo;
                txtEmailID.Text = objuser.EmailID;
                txtOtherEmailID.Text = objuser.OtherID;
                txtHighestQualification.Text = objuser.HighestQualification;
                txtInstratedIn.Text = objuser.InterestedIn;
                lblReportingTo.Text = objuser.ReportingTo;

                if (objuser.ProfileImg != null)
                {
                    Session["ProfileImg"] = objuser.ProfileImg;
                    ImgProfile.Src = "../Image.aspx";
                }
                else
                {
                    ImgProfile.Src = "../App_Themes/Blue/img/Male.png";
                    if (profile.Personal.Gender != null)
                    {
                        if (profile.Personal.Gender != "M") { ImgProfile.Src = "../App_Themes/Blue/img/Female.png"; }
                    }
                }   
                
                UCAddress1.ClearAddress("User");
                UCAddress1.FillAddressByObjectNameReferenceID("User", profile.Personal.UserID, "User");
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProfileSetting", "GetUserByID");
            }
            finally
            {
                userClient.Close();
            }
        }

        protected void lnkUpdateProfileImg_Click(object sender, EventArgs e)
        {
            Session["ProfileImg"] = FileUploadProfileImg.FileBytes;
            ImgProfile.Src = "../Image.aspx";
        }

        protected void pageSave(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            BrilliantWMS.UserCreationService.iUserCreationClient userClient = new BrilliantWMS.UserCreationService.iUserCreationClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                mUserProfileHead mUserProfile = new mUserProfileHead();
                mUserProfile = userClient.GetUserByID(profile.Personal.UserID, profile.DBConnection._constr);
                if (mUserProfile != null)
                {
                    mUserProfile.MobileNo = txtMobile.Text.Trim();
                    mUserProfile.PhoneNo = txtPhone.Text.Trim();
                    mUserProfile.EmailID = txtEmailID.Text.Trim();
                    mUserProfile.OtherID = txtOtherEmailID.Text.Trim();
                    mUserProfile.HighestQualification = txtHighestQualification.Text.Trim();
                    mUserProfile.InterestedIn = txtInstratedIn.Text.Trim();
                    mUserProfile.ProfileImg = (byte[])Session["ProfileImg"];
                    mUserProfile.DefaultAddress = UCAddress1.BillingSeq.Trim();

                    mUserProfile.LastModifiedBy = profile.Personal.UserID.ToString();
                    mUserProfile.LastModifiedDate = DateTime.Now;
                    userClient.UpdateUserProfile(mUserProfile, profile.DBConnection._constr);
                    UCAddress1.FinalSaveAddress(Address.ReferenceObjectName.User, profile.Personal.UserID);

                    profile.Personal.ProfileImg = mUserProfile.ProfileImg;
                    profile.Save();

                    WebMsgBox.MsgBox.Show("Profile updated successfully");
                }


            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProfileSetting", "pageSave");
            }
            finally { userClient.Close(); }
        }

        protected void pageClear(Object sender, BrilliantWMS.ToolbarService.iUCToolbarClient e)
        {
            try
            {
                GetUserByID();
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "ProfileSetting", "pageClear");
            }
        }

    }
}