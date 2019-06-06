using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.UserManagement;
using System.ServiceModel;
using System.Xml.Linq;
using System.Data.Objects;
using Domain.Server;
using System.Data.Linq;

namespace Domain.UserManagement
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public partial class UserCreation : Interface.UserManagement.iUserCreation
    {
        Domain.Server.Server svr = new Server.Server();
        #region GetUserCreationList
        /// <summary>
        /// GetUserCreationList is providing List of User
        /// </summary>
        /// <returns></returns>
        /// 
        public DataSet GetUserCreationList(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mUserProfileHead> Status = new List<mUserProfileHead>();
            XElement xmlUserCreation = new XElement("UserCreation", from m in ce.mUserProfileHeads.ToList()

                                                                    select new XElement("User",
                                                                    new XElement("ID", m.ID),
                                                                    new XElement("Name", m.FirstName + " " + m.LastName),
                                                                    new XElement("DateOfJoining", string.Format("{0:dd-MMM-yyyy}", m.DateOfJoining)),
                                                                    new XElement("DateOfBirth", string.Format("{0:dd-MMM-yyyy}", m.DateOfBirth)),
                                                                    new XElement("EmailID", m.EmailID),
                                                                    new XElement("MobileNo", m.MobileNo)
                                                                    ));


            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.ReadXml(xmlUserCreation.CreateReader());
            dt = ds.Tables.Add("Dt");
            return ds;
        }


        #endregion

        #region InsertUserCreation

        /// <summary>
        ///  mUserProfileHead is the Method To Insert Record In Database
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public long InsertUserCreation(mUserProfileHead user, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
                ce.mUserProfileHeads.AddObject(user);
                ce.SaveChanges();
                return user.ID;
            }
            catch { return 0; }
        }
        #endregion

        public List<SP_GetUserRoleDetail_Result> UpdateRoleIntoSessionList(List<SP_GetUserRoleDetail_Result> sessionList, SP_GetUserRoleDetail_Result updateRole, int rowindex)
        {
            SP_GetUserRoleDetail_Result findRow = new SP_GetUserRoleDetail_Result();
            findRow = sessionList.Where(s => s.mSequence == updateRole.mSequence && s.pSequence == updateRole.pSequence && s.oSequence == updateRole.oSequence).FirstOrDefault();
            if (findRow != null)
            {
                sessionList = sessionList.Where(s => s != findRow).ToList();
                findRow.Add = updateRole.Add;
                findRow.Edit = updateRole.Edit;
                findRow.View = updateRole.View;
                findRow.Delete = updateRole.Delete;
                findRow.Approval = updateRole.Approval;
                findRow.AssignTask = updateRole.AssignTask;
            }
            sessionList.Add(findRow);
            return sessionList;
        }

        public List<SP_GetUserRoleDetail_Result> GetDataToBindRoleMasterDetailsByRoleID(long RoleID, long UserIdForRole, long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GetUserRoleDetail_Result> lst = new List<SP_GetUserRoleDetail_Result>();
            lst = (from dbtable in ce.SP_GetUserRoleDetail(RoleID, UserIdForRole, CompanyID)
                   orderby (long)(dbtable.mSequence), (long)(dbtable.pSequence), (long)(dbtable.oSequence)
                   select dbtable).ToList();
            return lst;
        }

        public List<SP_GWCGetUserRoleDetail_Result> GetRoleDetails(long RoleID, long UserIdForRole, long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<SP_GWCGetUserRoleDetail_Result> lst = new List<SP_GWCGetUserRoleDetail_Result>();
            lst = (from dbtable in ce.SP_GWCGetUserRoleDetail(RoleID, UserIdForRole, CompanyID)
                   orderby (long)(dbtable.mSequence), (long)(dbtable.pSequence), (long)(dbtable.oSequence)
                   select dbtable).ToList();
            return lst;
        }

        #region UpdateUserProfile

        /// <summary>
        /// Updatestatus Is Method To Update mUserProfileHead Table
        /// </summary>
        /// <param name="Updatestatus"></param>
        /// <returns></returns>
        /// 
        public int UpdateUserProfile(mUserProfileHead User, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ce.mUserProfileHeads.Attach(User);
            ce.ObjectStateManager.ChangeObjectState(User, EntityState.Modified);
            ce.SaveChanges();
            return Convert.ToInt32(User.ID);
        }
        #endregion

        #region GetUserByID
        /// <summary>
        /// GetUserCreationList is providing List of User
        /// </summary>
        /// <returns></returns>
        /// 
        public mUserProfileHead GetUserByID(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mUserProfileHead User = new mUserProfileHead();
            User = (from p in ce.mUserProfileHeads
                    where p.ID == UserID
                    select p).FirstOrDefault();
            ce.Detach(User);
            return User;
        }
        #endregion

        #region Dupliacte

        public string checkDuplicateRecord(string EmpCode, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.mUserProfileHeads
                          where p.EmployeeID == EmpCode
                          select new { p.EmployeeID }).FirstOrDefault();

            if (output != null)
            {
                result = "Same Employee Code Already Exist";
            }
            return result;

        }
        #endregion

        #region chechDuplicateEdit
        public string checkDuplicateRecordEdit(string EmpCode, int UserID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            var output = (from p in ce.mUserProfileHeads
                          where p.EmployeeID == EmpCode && p.ID != UserID
                          select new { p.EmployeeID }).FirstOrDefault();

            if (output != null)
            {
                result = "Same Employee Code Already Exist";

            }
            return result;
        }
        #endregion

        public List<mUserProfileHead> SelectEmployeeDepartmentwise(mDesignation objmDesignation, string[] conn)
        {


            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mUserProfileHead> lst = new List<mUserProfileHead>();

            if (objmDesignation.Name == "0")
            {
                var result = from dbtable in ce.mUserProfileHeads
                             where ((dbtable.DesignationID == objmDesignation.ID) && (dbtable.DepartmentID == objmDesignation.DepartmentID))
                             select new { dbtable.FirstName, dbtable.ID, dbtable.MiddelName, dbtable.LastName, dbtable.EmployeeID };
                lst = result.AsEnumerable().Select(o => new mUserProfileHead { FirstName = o.FirstName, ID = o.ID, MiddelName = o.MiddelName, LastName = o.LastName, EmployeeID = o.EmployeeID }).ToList();
            }
            else
            {
                var result = from dbtable in ce.mUserProfileHeads
                             where ((dbtable.DesignationID == objmDesignation.ID) && (dbtable.DepartmentID == objmDesignation.DepartmentID) && (dbtable.ReportingTo == objmDesignation.Name))
                             select new { dbtable.FirstName, dbtable.ID, dbtable.MiddelName, dbtable.LastName, dbtable.EmployeeID };
                lst = result.AsEnumerable().Select(o => new mUserProfileHead { FirstName = o.FirstName, ID = o.ID, MiddelName = o.MiddelName, LastName = o.LastName, EmployeeID = o.EmployeeID }).ToList();
            }



            return lst;
        }

        public bool FinalSaveUserRoles(List<SP_GetUserRoleDetail_Result> sessionList, string userID, long userIDForRole, long CompanyID, long RoleID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
                if (sessionList.Count > 0)
                {
                    XElement xmlEle = new XElement("RoleMasterList", from rec in sessionList.AsEnumerable()
                                                                     select new XElement("RoleMaster",
                                                                    new XElement("ObjectName", rec.ObjectName),
                                                                    new XElement("Add", rec.Add),
                                                                    new XElement("Edit", rec.Edit),
                                                                    new XElement("View", rec.View),
                                                                    new XElement("Delete", rec.Delete),
                                                                    new XElement("Approval", rec.Approval),
                                                                    new XElement("AssignTask", rec.AssignTask)
                                                                    ));

                    ObjectParameter _paraxmlData = new ObjectParameter("xmlData", typeof(string));
                    _paraxmlData.Value = xmlEle.ToString();

                    ObjectParameter _paraCompanyID = new ObjectParameter("paraCompanyID", typeof(long));
                    _paraCompanyID.Value = CompanyID;

                    ObjectParameter _paraUserIDForRole = new ObjectParameter("paraUserIDForRole", typeof(long));
                    _paraUserIDForRole.Value = userIDForRole;

                    ObjectParameter _paraUserID = new ObjectParameter("paraUserID", typeof(long));
                    _paraUserID.Value = userID;

                    ObjectParameter _paraRoleID = new ObjectParameter("paraRoleID", typeof(long));
                    _paraRoleID.Value = RoleID;

                    ObjectParameter[] obj = new ObjectParameter[] { _paraxmlData, _paraCompanyID, _paraUserIDForRole, _paraUserID, _paraRoleID };
                    ce.ExecuteFunction("SP_InsertIntoUserRoleDetail", obj);
                    ce.SaveChanges();
                }

                return true;
            }
            catch { return false; }
        }

        public List<vGetUserProfileList> GetUserList(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<vGetUserProfileList> userProfileList = new List<vGetUserProfileList>();
            userProfileList = (from c in ce.vGetUserProfileLists
                               orderby c.deptname, c.desiName, c.userName
                               select c).ToList();
            return userProfileList;
        }

        public List<vGWCGetUserProfileList> GetGWCUserList(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<vGWCGetUserProfileList> userProfileList = new List<vGWCGetUserProfileList>();
            userProfileList = (from c in ce.vGWCGetUserProfileLists
                               orderby c.userID descending
                               select c).ToList();
            return userProfileList;
        }

        public List<vGWCGetUserProfileList> GetGWCUserListCompanyWise(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<vGWCGetUserProfileList> userProfileList = new List<vGWCGetUserProfileList>();
            userProfileList = (from c in ce.vGWCGetUserProfileLists
                               where c.CompanyID==CompanyID
                               orderby c.userID descending
                               select c).ToList();
            return userProfileList;
        }

        public vGetUserProfileByUserID GetUserProfileByUserID(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            vGetUserProfileByUserID UserProfile = new vGetUserProfileByUserID();
            UserProfile = db.vGetUserProfileByUserIDs.Where(u => u.userID == UserID).FirstOrDefault();
            return UserProfile;
        }

        public void SaveUsersLocationDetails(long ToUserID, long Level, string LocationIDs, string CreatedBy, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
                ObjectParameter SPUserID = new ObjectParameter("UserID", typeof(long));
                SPUserID.Value = ToUserID;

                ObjectParameter SPLevel = new ObjectParameter("Level", typeof(long));
                SPLevel.Value = Level;

                ObjectParameter SPTerritoryIDs = new ObjectParameter("TerritoryIDs", typeof(string));
                SPTerritoryIDs.Value = LocationIDs;

                ObjectParameter SPCreatedBy = new ObjectParameter("CreatedBy", typeof(string));
                SPCreatedBy.Value = CreatedBy;

                ObjectParameter[] obj = new ObjectParameter[] { SPUserID, SPLevel, SPTerritoryIDs, SPCreatedBy };
                db.ExecuteFunction("SP_InsertIntomUserProfieTerritoryDetail", obj);
                db.SaveChanges();
            }
            catch { }
            finally { }

        }

        public string GetLocationListByUserID(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mTerritory> lst = new List<mTerritory>();
            string result = "";
            try
            {
                lst = (from mt in ce.mTerritories
                       join mut in ce.mUserTerritoryDetails on mt.ID equals mut.TerritoryID
                       where mut.UserID == UserID
                       select mt).ToList();

                foreach (mTerritory mt in lst)
                {
                    if (result != "")
                    {
                        result += " | " + mt.Territory;
                    }
                    else
                    {
                        result = mt.Territory;
                    }
                }

                if (result == "") result = "N/A";
            }
            catch { }
            return result;
        }

        public string GetHTMLMenuByUserID(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            try
            {
                //result = (from s in db.SP_GetMenuHTML(UserID)
                  //        select s.HTMLMenu).FirstOrDefault();

                DataSet ds = new DataSet();
                //ds = fillds("Declare @result varchar(max); Set @result =''; Select @result = @result+ ' '+ Cast(mo.FormUrl as varchar(max)) from sysmObjects mo Left outer join mUserRolesDetail rd on Cast(mo.ObjectName as varchar(100)) = Cast(rd.ObjectName as varchar(100))  Where mo.HeaderMenu = 'YS' and rd.UserID = " + UserID + " And (rd.[Add] = 'true' OR rd.Edit = 'true' OR rd.[View] = 'true' or rd.[Delete]='True' or rd.Approval = 'true') order by mo.Sequence Set @result =  '<ul id=''css3menu1'' class=''topmenu1''>' + @result + '</ul>';  Select @result as 'HTMLMenu' ", conn);
                ds = fillds("Declare @rs varchar(max); set @rs=''; declare @cnt bigint; set @cnt=0; select @cnt=count(mo.FormUrl) from  sysmObjects mo Left outer join mUserRolesDetail rd on Cast(mo.ObjectName as varchar(100)) = Cast(rd.ObjectName as varchar(100))  Where mo.HeaderMenu = 'YS' and rd.UserID = " + UserID + " And (rd.[Add] = 'true' OR rd.Edit = 'true' OR rd.[View] = 'true' or rd.[Delete]='True' or rd.Approval = 'true') Declare @counter bigint; set @counter =1;  while(@counter<=@cnt) begin    select @rs=@rs+FormUrl from ( select ROW_NUMBER() OVER(ORDER BY mo.Sequence ) AS Row,mo.FormUrl from  sysmObjects mo Left outer join mUserRolesDetail rd on Cast(mo.ObjectName as varchar(100)) = Cast(rd.ObjectName as varchar(100))  Where mo.HeaderMenu = 'YS' and rd.UserID = " + UserID + " And (rd.[Add] = 'true' OR rd.Edit = 'true' OR rd.[View] = 'true' or rd.[Delete]='True' or rd.Approval = 'true'))aaa  where Row=@counter  set @counter=@counter+1; end; set @rs='<ul id=''css3menu1'' class=''topmunu1''>' + @rs + '</ul>';   select @rs as 'HTMLMenu'", conn); 
                result = ds.Tables[0].Rows[0]["HTMLMenu"].ToString();

            }
            catch (Exception ex) { result = ex.InnerException.Message.ToString(); }
            finally { }
            return result;
        }

        /*Add By Suresh For Arabic Menu*/
        public string GetHTMLMenuArabicByUserID(long UserID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            try
            {
              //  result = (from s in db.SP_GetMenuHTMLArabic(UserID)
               //           select s.HTMLMenu).FirstOrDefault();

                DataSet ds = new DataSet();
                ds = fillds("Declare @result nvarchar(max); Set @result =''; Select @result = @result+ ' '+ Cast(mo.FormUrlArabic as nvarchar(max)) from sysmObjects mo Left outer join mUserRolesDetail rd on Cast(mo.ObjectName as varchar(100)) = Cast(rd.ObjectName as varchar(100))   Where mo.HeaderMenu = 'YS' and rd.UserID = " + UserID + "  And (rd.[Add] = 'true' OR rd.Edit = 'true' OR rd.[View] = 'true'  or rd.[Delete]='True' or rd.Approval = 'true') order by mo.Sequence Set @result =  '<ul id=''css3menu1'' class=''topmenu1''>' + @result + '</ul>';  Select @result as 'HTMLMenu' ", conn);
                result = ds.Tables[0].Rows[0]["HTMLMenu"].ToString();
                    
            }
            catch (Exception ex) { result = ex.InnerException.Message.ToString(); }
            finally { }
            return result;
        }
        /*Add By Suresh For Arabic Menu*/


        public string GetTerritoryID_FromUserId(long userId, string[] conn)
        {

            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mUserTerritoryDetail> SiteList = new List<mUserTerritoryDetail>();
            string TerritoryID = "";
            SiteList = (from p in ce.mUserTerritoryDetails
                        where p.UserID == userId
                        select p).ToList();

            foreach (var v in SiteList)
            {
                if (TerritoryID.ToString() != "")
                {

                    TerritoryID = TerritoryID + "," + v.TerritoryID;
                }
                else
                {
                    TerritoryID = v.TerritoryID.ToString();
                }
            }

            return TerritoryID;
        }


        public DataSet GetSiteNameFromId(string sid, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select id,Territory from mTerritory where id in(" + sid + ")";
            ds = fillds(str, conn);
            return ds;
        }
        protected DataSet fillds(string strquery, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();
            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            SqlDataAdapter da = new SqlDataAdapter(strquery, sqlConn);
            ds.Reset();
            da.Fill(ds);
            return ds;
        }

        public List<mCompany> GetCompanyName(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCompany> companylist = new List<mCompany>();
            companylist = (from cl in ce.mCompanies
                           orderby cl.Name
                           select cl).ToList();
            return companylist;
        }

        public List<mDropdownValue> GetUserType(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mDropdownValue> UserTypeList = new List<mDropdownValue>();
            UserTypeList = (from U in ce.mDropdownValues
                            where U.Parameter == "User"
                            select U).ToList();
            return UserTypeList;
        }


        public bool FinalSaveGWCUserRoles(List<SP_GWCGetUserRoleDetail_Result> sessionList, string userID, long userIDForRole, long CompanyID, long RoleID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
                if (sessionList.Count > 0)
                {
                    XElement xmlEle = new XElement("RoleMasterList", from rec in sessionList.AsEnumerable()
                                                                     select new XElement("RoleMaster",
                                                                    new XElement("ObjectName", rec.ObjectName),
                                                                    new XElement("Add", rec.Add),
                                                                    new XElement("Edit", rec.Edit),
                                                                    new XElement("View", rec.View),
                                                                    new XElement("Delete", rec.Delete),
                                                                    new XElement("Approval", rec.Approval),
                                                                    new XElement("AssignTask", rec.AssignTask)
                                                                    ));

                    ObjectParameter _paraxmlData = new ObjectParameter("xmlData", typeof(string));
                    _paraxmlData.Value = xmlEle.ToString();

                    ObjectParameter _paraCompanyID = new ObjectParameter("paraCompanyID", typeof(long));
                    _paraCompanyID.Value = CompanyID;

                    ObjectParameter _paraUserIDForRole = new ObjectParameter("paraUserIDForRole", typeof(long));
                    _paraUserIDForRole.Value = userIDForRole;

                    ObjectParameter _paraUserID = new ObjectParameter("paraUserID", typeof(long));
                    _paraUserID.Value = userID;

                    ObjectParameter _paraRoleID = new ObjectParameter("paraRoleID", typeof(long));
                    _paraRoleID.Value = RoleID;

                    ObjectParameter[] obj = new ObjectParameter[] { _paraxmlData, _paraCompanyID, _paraUserIDForRole, _paraUserID, _paraRoleID };
                    ce.ExecuteFunction("SP_InsertIntoUserRoleDetail", obj);
                    ce.SaveChanges();
                }

                return true;
            }
            catch { return false; }
        }


        public List<vGWCGetUserList> GWCSearchUserList(long ComanyID, long DeptID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<vGWCGetUserList> userProfileList = new List<vGWCGetUserList>();
            userProfileList = (from c in ce.vGWCGetUserLists
                               where c.CompanyID == ComanyID && c.DepartmentID == DeptID
                               select c).ToList();
            return userProfileList;
        }

        public DataSet GetUserDelegationDetail(string state, long DeligateFrom, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_BindAcccessDelegation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("state", state);
            cmd.Parameters.AddWithValue("DeligateFrom", DeligateFrom);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void SaveEditUserDelegation(long ID, long DeligateFrom, DateTime FromDate, DateTime ToDate, long DeligateTo, string Remark, string state, long CreatedBy, DateTime CreatedDate, long DeptID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveEditUserDelegate";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("DeligateFrom", DeligateFrom);
            cmd.Parameters.AddWithValue("FromDate", FromDate);
            cmd.Parameters.AddWithValue("ToDate", ToDate);
            cmd.Parameters.AddWithValue("DeligateTo", DeligateTo);
            cmd.Parameters.AddWithValue("Remark", Remark);
            cmd.Parameters.AddWithValue("state", state);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreatedDate", CreatedDate);
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.ExecuteNonQuery();
        }

        public DataSet getUserDelegateDetail(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUserDelegateDetail";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet getDelegateToList(long DepartmentID, long UserID,string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDelegatetoUserList";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DepartmentID", DepartmentID);
            cmd.Parameters.AddWithValue("UserID", UserID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void UpdateDelegateFrom(long DeligateFrom, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateDelegateFrom";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DeligateFrom", DeligateFrom);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetDepartmentforUsersave(long ParentID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDepartmentContact";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetRollNameById(long TerritoryID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetRollNameByDeptId";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("TerritoryID", TerritoryID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public string GetUserTypeByRoll(long ID, string[] conn)
        {
            SqlDataReader dr;
            string UserType = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetUserTypeByRoll";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                UserType = dr[0].ToString();
            }
            dr.Close();
            return UserType;
        }

        public DataSet CheckPasswordHistory(long UserProfileID, string Password, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetPasswordHistory";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserProfileID", UserProfileID);
            cmd.Parameters.AddWithValue("Password", Password);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void SavePasswordDetails(long UserProfileID, string Email, string UserName, string Password, string transactedby, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_SavePasswordDetails";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserProfileID", UserProfileID);
            cmd.Parameters.AddWithValue("Email", Email);
            cmd.Parameters.AddWithValue("UserName", UserName);
            cmd.Parameters.AddWithValue("Password", Password);
            cmd.Parameters.AddWithValue("CreatedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("transactedby", transactedby);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetUserDepartment(long userID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select UTD.TerritoryID ,MT.Territory from  mUserTerritoryDetail UTD left outer join mTerritory MT on  UTD.TerritoryID=MT.ID where UTD.UserID="+ userID +"";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet getDelegateToListMultipleDept(string  SelectedLocation, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select ID,FirstName +' ' + MiddelName + ' '+ LastName as Name from mUserProfileHead where DepartmentID in("+ SelectedLocation +") and UserType in ('Super Admin','Admin','Approver','Requester And Approver')";
            ds = fillds(str, conn);
            return ds;
        }

        public void updatelockunlock(string UserName, byte IsLockedOut, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateLocking";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserName", UserName);
            cmd.Parameters.AddWithValue("IsLockedOut", IsLockedOut);
            cmd.ExecuteNonQuery();
        }

        public string GetLogoPath(long ID, string[] conn)
        {
            SqlDataReader dr;
            string UserLogo = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetLogoPath";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                UserLogo = dr[0].ToString();
            }
            dr.Close();
            return UserLogo;
        }


        public int GetApprovalDetailsOfUser(long userID, string[] conn)
        {
            int result = 0;
            DataSet ds=new DataSet ();
            ds.Reset();
            string str = "select * from  mApprovalLevelDetail where Userid="+ userID +"";
            ds = fillds(str, conn);
            result = ds.Tables[0].Rows.Count;
            return result;
        }

        public int GetAdditionalDistributationOfUser(long userID, string[] conn)
        {
            int result = 0;
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select * from mAddDistribution where ContactID=" + userID + "";
            ds = fillds(str, conn);
            result = ds.Tables[0].Rows.Count;
            return result;
        }

        public DataSet GetDepartmentDelegate(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select distinct ID, Territory from mTerritory where ID in (Select TerritoryID from mUserTerritoryDetail where UserID = '" + UserID + "') order by Territory asc";
            ds = fillds(str, conn);
            return ds;
        }

        public DataSet GetDepartmentSSuperAdmin(string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select ID, Territory from  mTerritory order by Territory asc";
            ds = fillds(str, conn);
            return ds;
        }

        public void Deletedelegate(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteDelegation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.ExecuteNonQuery();
        }

        public long GetDuplicateDelegate(DateTime FromDate, DateTime ToDate, long DeligateTo, long DeptID, long DeligateFrom, string[] conn)
        {

            SqlDataReader dr;
            long Count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CheckDuplicateDelegate";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("FromDate", FromDate);
            cmd.Parameters.AddWithValue("ToDate", ToDate);
            cmd.Parameters.AddWithValue("DeligateTo", DeligateTo);
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.Parameters.AddWithValue("DeligateFrom", DeligateFrom);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Count = long.Parse(dr[0].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return Count;
        }

        public string CheckOneDayValidation(long UserID, string[] conn)
        {
            string result = "";
            DataSet dsLastCreateddate = new DataSet();
            string str = "SELECT top (1)  Convert(varchar(10), CreatedDate,101) CreatedDate FROM  mPasswordDetails where UserProfileID=" + UserID + " order by CreatedDate desc";
            dsLastCreateddate = fillds(str, conn);
            DateTime LstChngDt =Convert.ToDateTime(dsLastCreateddate.Tables[0].Rows[0]["CreatedDate"].ToString());

            DataSet dsCurrentdate = new DataSet();
            string str1 = "select convert(varchar(10),getdate(),101) CurrentDate";
            dsCurrentdate = fillds(str1, conn);
            DateTime CrntDt =Convert.ToDateTime(dsCurrentdate.Tables[0].Rows[0]["CurrentDate"].ToString());

            if (LstChngDt == CrntDt) result = "SameDay";
            return result;
        }

        public string GetUserNameByID(long UserID, string[] conn)
        {
            DataSet ds = new DataSet();
            string str = "SELECT DISTINCT UserName  FROM   mPasswordDetails  WHERE  (UserProfileID IN ("+UserID+")) ";
            ds = fillds(str, conn);
            string UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
            return UserName;
        }


        public string GetComppanyLogo(long CmpnyID, string[] conn)
        {
            DataSet ds = new DataSet();
            string str = "select LogoPath from mCompany where id=" + CmpnyID + " ";
            ds = fillds(str, conn);
            string LogoPath = ds.Tables[0].Rows[0]["LogoPath"].ToString();
            return LogoPath;
        }

        public DataSet GetUserLoginDetails(long UserProfileID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds.Reset();
            string str = "select top(1) * from mPasswordDetails where UserProfileID = '" + UserProfileID + "' order by ID desc ";
            ds = fillds(str, conn);
            return ds;
        }

        public void InsertIntoUserLocation(long UserID,long LocationID,string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertIntoUserLocation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserID", UserID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            cmd.ExecuteNonQuery();
        }

        public long GetDuplicatlocationUser(long UserID, long LocationID, string[] conn)
        {
            SqlDataReader dr;
            long Count = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDuplicateLocUser";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserID", UserID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Count = long.Parse(dr[0].ToString());
            }
            dr.Close();
            cmd.Connection.Close();
            return Count;
        }

        public void RemoveUserLoc(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_RemoveUserLoc";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.ExecuteNonQuery();
        }

        #region NewAllMenu
        public string GetAllHTMLMenu(string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            string result = "";
            try
            {
                DataSet ds = new DataSet();
                ds = fillds("select XMLMenu from  FN_AllMenu()", conn);
                result = ds.Tables[0].Rows[0]["XMLMenu"].ToString();
            }
            catch (Exception ex) { result = ex.InnerException.Message.ToString(); }
            finally { }
            return result;
        }
        #endregion

        #region UserWarehouse
        public DataSet GetUserWarehouseDetails(string userID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from mWarehouseMaster where ID in(select WarehoueID from mUserWarehouse where UserID="+ userID +")", conn);
            return ds;
        }
        #endregion

    }
}
