using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.Company;
using System.ServiceModel;
using System.Xml.Linq;
using Domain.Server;
using System.Data.Objects;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using System.Data;
using System.Collections;

namespace Domain.Company
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public partial class CompanySetup : Interface.Company.iCompanySetup
    {
        Domain.Server.Server svr = new Server.Server();

        #region GetGroupCompany
        /// <summary>
        /// GetGroupCompany is providing List of Group Company
        /// </summary>
        /// <returns></returns>
        /// 
        public List<mCompany> GetGroupCompany(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            List<mCompany> GroupCompany = new List<mCompany>();
            GroupCompany = (from p in ce.mCompanies
                            select p).ToList();
            if (GroupCompany.Count == 0)
            {
                GroupCompany = null;
            }
            return GroupCompany;
        }
        #endregion

        #region InsertmCompany

        /// <summary>
        ///  InsertmCompany is the Method To Insert Record In mCompany Table
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public long InsertmCompany(mCompany Company, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

                if (Company.ID == null || Company.ID == 0) { ce.mCompanies.AddObject(Company); }
                else
                {
                    SqlConnection sqlconn = new SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
                    SqlCommand cmd = new SqlCommand("dbcc checkident (mCompany, reseed, " + (Company.ID - 1) + ")", sqlconn);
                    sqlconn.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    sqlconn.Close();
                    ce.mCompanies.AddObject(Company);

                }

                ce.SaveChanges();
                return Company.ID;
            }
            catch (Exception ex) { return 0; }
        }
        #endregion

        #region UpdateCompany
        /// <summary>
        /// Updatestatus Is Method To Update mCompany Table
        /// </summary>
        /// <param name="Updatestatus"></param>
        /// <returns></returns>
        /// 
        public int UpdateCompany(mCompany UpdateCompany, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.mCompanies.Attach(UpdateCompany);
            ce.ObjectStateManager.ChangeObjectState(UpdateCompany, EntityState.Modified);
            ce.SaveChanges();
            return 1;
        }
        #endregion

        #region Dupliacte

        public string checkDuplicateRecord(string CompanyName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            string result = "";
            var output = (from p in ce.mCompanies
                          where p.Name == CompanyName
                          select new { p.Name }).FirstOrDefault();

            if (output != null)
            {
                result = "Same Company Name Already Exist";
            }
            return result;

        }
        #endregion

        #region chechDuplicateEdit
        public string checkDuplicateRecordEdit(string CompanyName, int CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            string result = "";
            var output = (from p in ce.mCompanies
                          where p.Name == CompanyName && p.ID != CompanyID
                          select new { p.Name }).FirstOrDefault();

            if (output != null)
            {
                result = "Same Company Name Already Exist";
            }
            return result;
        }
        #endregion

        #region GetCompanyById
        /// <summary>
        /// GetGroupCompany is providing List of Group Company
        /// </summary>
        /// <returns></returns>
        /// 
        public mCompany GetCompanyById(Int64 CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            mCompany Company = new mCompany();
            Company = (from p in ce.mCompanies
                       where p.ID == CompanyID
                       select p).FirstOrDefault();

            return Company;
        }
        #endregion

        #region InsertmCompanyRegistration

        /// <summary>
        ///  InsertmCompany is the Method To Insert Record In mCompany Table
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int InsertmCompanyRegistration(tCompanyRegistrationDetail Company, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.tCompanyRegistrationDetails.AddObject(Company);
            ce.SaveChanges();
            return 1;
        }
        #endregion

        #region UpdateCompanyRegistration
        /// <summary>
        /// Updatestatus Is Method To Update mCompany Table
        /// </summary>
        /// <param name="Updatestatus"></param>
        /// <returns></returns>
        /// 
        public int UpdateCompanyRegistration(tCompanyRegistrationDetail UpdateCompany, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            ce.tCompanyRegistrationDetails.Attach(UpdateCompany);
            ce.ObjectStateManager.ChangeObjectState(UpdateCompany, EntityState.Modified);
            ce.SaveChanges();
            return 1;
        }
        #endregion

        public List<vGetCompanyDetail> GetCompanyList(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<vGetCompanyDetail> mcomp = new List<vGetCompanyDetail>();
            mcomp = (from cm in ce.vGetCompanyDetails
                     orderby  cm.ID descending
                     select cm).ToList();
            return mcomp;
        }

        #region GetCompanyById
        /// <summary>
        /// GetGroupCompany is providing List of Group Company
        /// </summary>
        /// <returns></returns>
        /// 
        public tCompanyRegistrationDetail GetCompanyRegisById(Int64 CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

            tCompanyRegistrationDetail Company = new tCompanyRegistrationDetail();
            Company = (from p in ce.tCompanyRegistrationDetails
                       where p.CompanyID == CompanyID
                       select p).FirstOrDefault();

            return Company;
        }
        #endregion

        #region ChechkCompanyServerNameDuplicate
        public string ChechkCompanyServerNameDuplicate(string ServerName, string[] conn)
        {
            string Result = "";
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCompany> Company = new List<mCompany>();
            Company = (from p in ce.mCompanies
                       where p.DataSource == ServerName
                       select p).ToList();
            if (Company.Count == 0)
            {
                Result = "";
            }
            else
            {
                Result = "Server Name already exist";
            }
            return Result;
        }

        #endregion

        #region ChechkCompanyDatabaseNameDuplicate
        public string ChechkCompanyDatabaseNameDuplicate(string DataBaseName, string[] conn)
        {
            string Result = "";
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCompany> Company = new List<mCompany>();
            Company = (from p in ce.mCompanies
                       where p.DataBaseName == DataBaseName
                       select p).ToList();
            if (Company.Count == 0)
            {
                Result = "";
            }
            else
            {
                Result = "Same DataBase Name Already Exist";
            }
            return Result;
        }

        #endregion

        #region ChechkCompanyNameDuplicate
        public string ChechkCompanyNameDuplicate(string CompanyName, string[] conn)
        {
            string Result = "";
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCompany> Company = new List<mCompany>();
            Company = (from p in ce.mCompanies
                       where p.DataBaseName == CompanyName
                       select p).ToList();
            if (Company.Count == 0)
            {
                Result = "";
            }
            else
            {
                Result = "Same Company Name Already Exist";
            }
            return Result;
        }



        #region ChechkCompanyServerNameDuplicate
        public string ChechkCompanyServer_DataBaseNameDuplicate(string ServerName, string Database, string[] conn)
        {
            string Result = "";
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mCompany> Company = new List<mCompany>();
            Company = (from p in ce.mCompanies
                       where p.DataSource == ServerName && p.DataBaseName == Database
                       select p).ToList();
            if (Company.Count == 0)
            {
                Result = "";
            }
            else
            {
                Result = "Same Server  Name Already Exist";
            }
            return Result;
        }

        #endregion

        #endregion

        #region RestoreDatabase
        /// <summary>
        /// GetGroupCompany is providing List of Group Company
        /// </summary>
        /// <returns></returns>
        /// 
        public void RestoreDatabase(string DataBaseName, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            ObjectParameter _paraDataBaseName = new ObjectParameter("DataBaseName", typeof(string));
            _paraDataBaseName.Value = DataBaseName;

            //ObjectParameter _paraObjectName = new ObjectParameter("paraObjectName", typeof(string));
            //_paraObjectName.Value = "VendorProduct";

            //ObjectParameter _paraReferenceID = new ObjectParameter("paraReferenceID", typeof(long));
            //_paraReferenceID.Value = paraReferenceID;

            //ObjectParameter _paraUserID = new ObjectParameter("paraUserID", typeof(string));
            //_paraUserID.Value = paraUserID;

            ObjectParameter[] obj = new ObjectParameter[] { _paraDataBaseName };

            ce.ExecuteFunction("SP_RestoreDataBase", obj);
            ce.SaveChanges();

        }
        #endregion

        # region New Customer or company code for GWC project
        public DataSet GetCompanyName(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_GetCompanyNameID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }


        public DataSet GetDepartmentListforgrid(long ParentID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDepartmentforGrid";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void Savecustdeptinfo(long ParentID, string Territory, long Sequence, string StoreCode, long ApprovalLevel, string AutoCancel, long cancelDays, string CreatedBy, DateTime CreationDate, string Active, string ApprovalRem, long ApproRemSchedul, string AutoCancRen, long AutoRemSchedule, bool GwcDeliveries, bool ECommerce, string OrderFormat, long MaxDeliveryDays, long AddressType, bool PriceChange, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertCustDeptInfo";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            cmd.Parameters.AddWithValue("Territory", Territory);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.Parameters.AddWithValue("StoreCode", StoreCode);
            cmd.Parameters.AddWithValue("ApprovalLevel", ApprovalLevel);
            cmd.Parameters.AddWithValue("AutoCancel", AutoCancel);
            cmd.Parameters.AddWithValue("cancelDays", cancelDays);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("Active", Active);
            cmd.Parameters.AddWithValue("ApprovalRem", ApprovalRem);
            cmd.Parameters.AddWithValue("ApproRemSchedul", ApproRemSchedul);
            cmd.Parameters.AddWithValue("AutoCancRen", AutoCancRen);
            cmd.Parameters.AddWithValue("AutoRemSchedule", AutoRemSchedule);
            cmd.Parameters.AddWithValue("GwcDeliveries", GwcDeliveries);
            cmd.Parameters.AddWithValue("ECommerce", ECommerce);
            cmd.Parameters.AddWithValue("OrderFormat", OrderFormat);
            cmd.Parameters.AddWithValue("MaxDeliveryDays", MaxDeliveryDays);
            cmd.Parameters.AddWithValue("AddressType", AddressType);
            cmd.Parameters.AddWithValue("PriceChange", PriceChange);
            cmd.ExecuteNonQuery();
            InsertEmailMessages(CreatedBy, conn);
        }

        public void InsertEmailMessages(string CreatedBy, string[] conn)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = fillds("select top (1) ID , ParentID from mTerritory order by id desc", conn);
                long DeptID = long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                long CompanyID = long.Parse(ds.Tables[0].Rows[0]["ParentID"].ToString());

                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

               // mMessageEMailTemplate mEmail = new mMessageEMailTemplate();

                mMessageEMailTemplate mEmail1 = new mMessageEMailTemplate();
                mEmail1.MailSubject = "OMS Order Submitted Successfully waiting for Approval ";
                mEmail1.MailBody = "Below order has been created successfully and is waiting for Approval,";
                mEmail1.Active = "Yes";
                mEmail1.CreatedBy = long.Parse(CreatedBy);
                mEmail1.CreationDate = DateTime.Now;
                mEmail1.CompanyID = CompanyID;
                mEmail1.DepartmentID = DeptID;
                mEmail1.ActivityID = 6;
                mEmail1.MessageID = 12;
                mEmail1.TemplateTitle = "Order Submit Information";

                db.mMessageEMailTemplates.AddObject(mEmail1);
                db.SaveChanges();

                mMessageEMailTemplate mEmail2 = new mMessageEMailTemplate();
                mEmail2.MailSubject = "OMS Order Submitted Successfully waiting for Approval ";
                mEmail2.MailBody = "Below order has been created successfully and is waiting for Approval,";
                mEmail2.Active = "Yes";
                mEmail2.CreatedBy = long.Parse(CreatedBy);
                mEmail2.CreationDate = DateTime.Now;
                mEmail2.CompanyID = CompanyID;
                mEmail2.DepartmentID = DeptID;
                mEmail2.ActivityID = 6;
                mEmail2.MessageID = 13;
                mEmail2.TemplateTitle = "Order Submitted Action";

                db.mMessageEMailTemplates.AddObject(mEmail2);
                db.SaveChanges();

                mMessageEMailTemplate mEmail3 = new mMessageEMailTemplate();
                mEmail3.MailSubject = "Order Approve Reminder";
                mEmail3.MailBody = "Please Approve Order Before Cancellation Days";
                mEmail3.Active = "Yes";
                mEmail3.CreatedBy = long.Parse(CreatedBy);
                mEmail3.CreationDate = DateTime.Now;
                mEmail3.CompanyID = CompanyID;
                mEmail3.DepartmentID = DeptID;
                mEmail3.ActivityID = 6;
                mEmail3.MessageID = 14;
                mEmail3.TemplateTitle = "Order Approve Reminder";

                db.mMessageEMailTemplates.AddObject(mEmail3);
                db.SaveChanges();

                mMessageEMailTemplate mEmail4 = new mMessageEMailTemplate();
                mEmail4.MailSubject = "OMS Order Approved Successfully";
                mEmail4.MailBody = "Below Order has been approved by approver for warehouse processing,";
                mEmail4.Active = "Yes";
                mEmail4.CreatedBy = long.Parse(CreatedBy);
                mEmail4.CreationDate = DateTime.Now;
                mEmail4.CompanyID = CompanyID;
                mEmail4.DepartmentID = DeptID;
                mEmail4.ActivityID = 7;
                mEmail4.MessageID = 13;
                mEmail4.TemplateTitle = "OMS Order Approved Successfully";

                db.mMessageEMailTemplates.AddObject(mEmail4);
                db.SaveChanges();

                mMessageEMailTemplate mEmail5 = new mMessageEMailTemplate(); //Need To Change
                mEmail5.MailSubject = "OMS Order Cancellation By Warehouse ";
                mEmail5.MailBody = "Below Order has been Cancelled by the warehouse,";
                mEmail5.Active = "Yes";
                mEmail5.CreatedBy = long.Parse(CreatedBy);
                mEmail5.CreationDate = DateTime.Now;
                mEmail5.CompanyID = CompanyID;
                mEmail5.DepartmentID = DeptID;
                mEmail5.ActivityID = 11;// 7; 
                mEmail5.MessageID = 12; // 14; 
                mEmail5.TemplateTitle = "OMS Order Cancellation By Warehouse";

                db.mMessageEMailTemplates.AddObject(mEmail5);
                db.SaveChanges();

                mMessageEMailTemplate mEmail6 = new mMessageEMailTemplate();
                mEmail6.MailSubject = "OMS Order Rejected by Approver";
                mEmail6.MailBody = "Below Order has been Rejected By the Approver,";
                mEmail6.Active = "Yes";
                mEmail6.CreatedBy = long.Parse(CreatedBy);
                mEmail6.CreationDate = DateTime.Now;
                mEmail6.CompanyID = CompanyID;
                mEmail6.DepartmentID = DeptID;
                mEmail6.ActivityID = 8;
                mEmail6.MessageID = 12;
                mEmail6.TemplateTitle = "OMS Order Rejected by Approver";

                db.mMessageEMailTemplates.AddObject(mEmail6);
                db.SaveChanges();

                mMessageEMailTemplate mEmail7 = new mMessageEMailTemplate();
                mEmail7.MailSubject = "OMS Order Ready for Pickup";
                mEmail7.MailBody = "Below Order is picked and ready for pickup,";
                mEmail7.Active = "Yes";
                mEmail7.CreatedBy = long.Parse(CreatedBy);
                mEmail7.CreationDate = DateTime.Now;
                mEmail7.CompanyID = CompanyID;
                mEmail7.DepartmentID = DeptID;
                mEmail7.ActivityID = 9;
                mEmail7.MessageID = 12;
                mEmail7.TemplateTitle = "OMS Order Ready for Pickup ";

                db.mMessageEMailTemplates.AddObject(mEmail7);
                db.SaveChanges();

                mMessageEMailTemplate mEmail8 = new mMessageEMailTemplate();
                mEmail8.MailSubject = "OMS Order Delivered";
                mEmail8.MailBody = "Below Order has been delivered";
                mEmail8.Active = "Yes";
                mEmail8.CreatedBy = long.Parse(CreatedBy);
                mEmail8.CreationDate = DateTime.Now;
                mEmail8.CompanyID = CompanyID;
                mEmail8.DepartmentID = DeptID;
                mEmail8.ActivityID = 10;
                mEmail8.MessageID = 12;
                mEmail8.TemplateTitle = "OMS Order Delivered ";

                db.mMessageEMailTemplates.AddObject(mEmail8);
                db.SaveChanges();

                mMessageEMailTemplate mEmail9 = new mMessageEMailTemplate();
                mEmail9.MailSubject = "OMS Order Auto Cancelled ";
                mEmail9.MailBody = "Below Order is Cancelled due to Auto Cancellation Policy,";
                mEmail9.Active = "Yes";
                mEmail9.CreatedBy = long.Parse(CreatedBy);
                mEmail9.CreationDate = DateTime.Now;
                mEmail9.CompanyID = CompanyID;
                mEmail9.DepartmentID = DeptID;
                mEmail9.ActivityID = 11;
                mEmail9.MessageID = 14;
                mEmail9.TemplateTitle = "OMS Order Auto Cancelled ";

                db.mMessageEMailTemplates.AddObject(mEmail9);
                db.SaveChanges();

                /*Order Out For Delivery */
                mMessageEMailTemplate mEmailOD = new mMessageEMailTemplate();
                mEmailOD.MailSubject = "Order Out For Delivery ";
                mEmailOD.MailBody = "Below Order has been Out For Delivery,";
                mEmailOD.Active = "Yes";
                mEmailOD.CreatedBy = long.Parse(CreatedBy);
                mEmailOD.CreationDate = DateTime.Now;
                mEmailOD.CompanyID = CompanyID;
                mEmailOD.DepartmentID = DeptID;
                mEmailOD.ActivityID = 61;
                mEmailOD.MessageID = 12;
                mEmailOD.TemplateTitle = "Order Out For Delivery";

                db.mMessageEMailTemplates.AddObject(mEmailOD);
                db.SaveChanges();

                /*Order Return */
                mMessageEMailTemplate mEmailRT = new mMessageEMailTemplate();
                mEmailRT.MailSubject = "Order Returned ";
                mEmailRT.MailBody = "Below Order is Returned,";
                mEmailRT.Active = "Yes";
                mEmailRT.CreatedBy = long.Parse(CreatedBy);
                mEmailRT.CreationDate = DateTime.Now;
                mEmailRT.CompanyID = CompanyID;
                mEmailRT.DepartmentID = DeptID;
                mEmailRT.ActivityID = 62;
                mEmailRT.MessageID = 12;
                mEmailRT.TemplateTitle = "Order Returned";

                db.mMessageEMailTemplates.AddObject(mEmailRT);
                db.SaveChanges();

                /*order cancel by requester*/
                mMessageEMailTemplate mEmailCR = new mMessageEMailTemplate();
                mEmailCR.MailSubject = "Cancel by Requester";
                mEmailCR.MailBody = "Order Cancel by Requester,";
                mEmailCR.Active = "Yes";
                mEmailCR.CreatedBy = long.Parse(CreatedBy);
                mEmailCR.CreationDate = DateTime.Now;
                mEmailCR.CompanyID = CompanyID;
                mEmailCR.DepartmentID = DeptID;
                mEmailCR.ActivityID = 66;
                mEmailCR.MessageID = 12;
                mEmailCR.TemplateTitle = "Cancel by Requester";

                db.mMessageEMailTemplates.AddObject(mEmailCR);
                db.SaveChanges();

                /*Approved With Revision*/

                mMessageEMailTemplate mEmailAR = new mMessageEMailTemplate();
                mEmailAR.MailSubject = "Approved With Revision";
                mEmailAR.MailBody = "Order Approved With Revision,";
                mEmailAR.Active = "Yes";
                mEmailAR.CreatedBy = long.Parse(CreatedBy);
                mEmailAR.CreationDate = DateTime.Now;
                mEmailAR.CompanyID = CompanyID;
                mEmailAR.DepartmentID = DeptID;
                mEmailAR.ActivityID = 67;
                mEmailAR.MessageID = 12;
                mEmailAR.TemplateTitle = "Approved With Revision";

                db.mMessageEMailTemplates.AddObject(mEmailAR);
                db.SaveChanges();

                /*Direct Order Approval  */
                mMessageEMailTemplate mEmailDo = new mMessageEMailTemplate();
                mEmailDo.MailSubject = "Direct Order Approved";
                mEmailDo.MailBody = "Below order has been submitted with auto approved and sent to warehouse for further processing,";
                mEmailDo.Active = "Yes";
                mEmailDo.CreatedBy = long.Parse(CreatedBy);
                mEmailDo.CreationDate = DateTime.Now;
                mEmailDo.CompanyID = CompanyID;
                mEmailDo.DepartmentID = DeptID;
                mEmailDo.ActivityID = 68;
                mEmailDo.MessageID = 13;
                mEmailDo.TemplateTitle = "Direct Order Approved";

                db.mMessageEMailTemplates.AddObject(mEmailDo);
                db.SaveChanges();

                /* Price Change Email For Approver*/
                mMessageEMailTemplate mEmailPC = new mMessageEMailTemplate();
                mEmailPC.MailSubject = "OMS Order Submitted Successfully waiting for Approval ";
                mEmailPC.MailBody = "This order is awaiting your approval due to a change of price. ";
                mEmailPC.Active = "Yes";
                mEmailPC.CreatedBy = long.Parse(CreatedBy);
                mEmailPC.CreationDate = DateTime.Now;
                mEmailPC.CompanyID = CompanyID;
                mEmailPC.DepartmentID = DeptID;
                mEmailPC.ActivityID = 76;
                mEmailPC.MessageID = 13;
                mEmailPC.TemplateTitle = "Order Submitted Action";

                db.mMessageEMailTemplates.AddObject(mEmailPC);
                db.SaveChanges();

                /*Save SLA Details OF Department*/
                SaveSLA(DeptID,conn);
                InsertPMethod(DeptID, conn);
            }
            catch { }
            finally { }
        }

        public void InsertPMethod(long deptid, string[] conn)
        {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Insert into mDeptPaymentMethod(DeptID,PMethodID,Sequence)values(" + deptid + ",1,1)";
                cmd.Connection = svr.GetSqlConn(conn);
                cmd.Parameters.Clear();
                
                cmd.ExecuteNonQuery();
        }

        public void SaveSLA(long DeptID,string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select ID from mSLA where DeptID=0", conn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                long SLAID=long.Parse(ds.Tables[0].Rows[0]["ID"].ToString());

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update mSLA set DeptID=" + DeptID + " where ID=" + SLAID + "";
                cmd.Connection = svr.GetSqlConn(conn);
                cmd.Parameters.Clear();
                
                cmd.ExecuteNonQuery();
            }
        }

        protected DataSet fillds(String strquery, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            DataSet ds = new DataSet();

            System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection("Data Source=" + conn[0] + ";Initial Catalog=" + conn[1] + "; User ID=" + conn[3] + "; Password=" + conn[2] + ";");
            SqlDataAdapter da = new SqlDataAdapter(strquery, sqlConn);
            ds.Reset();
            da.Fill(ds);
            return ds;
        }
        public DataSet GetDepartmentToEdit(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDepartmentToEdit";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }


        public void UpdateDeptInfo(long ID, string Territory, string StoreCode, long ApprovalLevel, string AutoCancel, long cancelDays, string CreatedBy, DateTime CreationDate, string Active, string ApprovalRem, long ApproRemSchedul, string AutoCancRen, long AutoRemSchedule, bool GwcDeliveries, bool ECommerce, string OrderFormat, long MaxDeliveryDays, long FinApproverID, long AddressType, bool PriceChange, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_UpdateDeptInfoEdit";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("Territory", Territory);
            cmd.Parameters.AddWithValue("StoreCode", StoreCode);
            cmd.Parameters.AddWithValue("ApprovalLevel", ApprovalLevel);
            cmd.Parameters.AddWithValue("AutoCancel", AutoCancel);
            cmd.Parameters.AddWithValue("cancelDays", cancelDays);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("Active", Active);
            cmd.Parameters.AddWithValue("ApprovalRem", ApprovalRem);
            cmd.Parameters.AddWithValue("ApproRemSchedul", ApproRemSchedul);
            cmd.Parameters.AddWithValue("AutoCancRen", AutoCancRen);
            cmd.Parameters.AddWithValue("AutoRemSchedule", AutoRemSchedule);

            cmd.Parameters.AddWithValue("GwcDeliveries", GwcDeliveries);
            cmd.Parameters.AddWithValue("ECommerce", ECommerce);
            cmd.Parameters.AddWithValue("OrderFormat", OrderFormat);
            cmd.Parameters.AddWithValue("MaxDeliveryDays", MaxDeliveryDays);
            cmd.Parameters.AddWithValue("FinApproverID", FinApproverID);
            cmd.Parameters.AddWithValue("AddressType", AddressType);
            cmd.Parameters.AddWithValue("PriceChange", PriceChange);
            cmd.ExecuteNonQuery();
        }

        public long chkDeptDuplicate(string Territory, string StoreCode, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Sp_CheckDeptDuplicate";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Territory", Territory);
            cmd.Parameters.AddWithValue("StoreCode", StoreCode);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr[0].ToString());
            }
            dr.Close();
            return result;
        }

        public DataSet GetDeptListWithSLA(long ParentID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetDeptListForGrid";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetLocationList(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetLocationDetailList";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long SaveLocContactInContact(string ObjectName, long ReferenceID, long CustomerHeadID, long Sequence, string Name, string EmailID, string MobileNo, long ContactTypeID, string Active, string CreatedBy, DateTime CreationDate, long CompanyID, string[] conn)
        {
            long ContactID = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveLocationContact";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ObjectName", ObjectName);
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("CustomerHeadID", CustomerHeadID);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.Parameters.AddWithValue("Name", Name);
            cmd.Parameters.AddWithValue("EmailID", EmailID);
            cmd.Parameters.AddWithValue("MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("ContactTypeID", ContactTypeID);
            cmd.Parameters.AddWithValue("Active", Active);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);

            //cmd.ExecuteNonQuery();
            ContactID = long.Parse(cmd.ExecuteScalar().ToString());

            return ContactID;
        }

        public void SaveEditLocation(long ID, long CompanyID, long ReferenceID, string LocationCode, string AddressLine1, string AddressLine2, string County, string State, string City, string zipcode, string landmark, string FaxNo, string Active, string ContactName, string ContactEmail, string LocationName, long MobileNo, string CreatedBy, DateTime CreationDate, string hdnstate, long ShippingID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_SaveEditLocation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("AddressLine1", AddressLine1);
            cmd.Parameters.AddWithValue("AddressLine2", AddressLine2);
            cmd.Parameters.AddWithValue("County", County);
            cmd.Parameters.AddWithValue("State", State);
            cmd.Parameters.AddWithValue("City", City);
            cmd.Parameters.AddWithValue("zipcode", zipcode);
            cmd.Parameters.AddWithValue("landmark", landmark);
            cmd.Parameters.AddWithValue("FaxNo", FaxNo);
            cmd.Parameters.AddWithValue("Active", Active);
            cmd.Parameters.AddWithValue("ContactName", ContactName);
            cmd.Parameters.AddWithValue("ContactEmail", ContactEmail);
            cmd.Parameters.AddWithValue("LocationName", LocationName);
            cmd.Parameters.AddWithValue("MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("hdnstate", hdnstate);
            cmd.Parameters.AddWithValue("ShippingID", ShippingID);
            cmd.ExecuteNonQuery();
        }

        public void UpdateLocationDetails(long ID, long CompanyID, long ReferenceID, string LocationCode, string AddressLine1, string AddressLine2, string County, string State, string City, string zipcode, string landmark, string FaxNo, string Active, string ContactName, string ContactEmail, long MobileNo, string CreatedBy, DateTime CreationDate, string hdnstate, string LocationName, long ShippingID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateLocationDetails";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            cmd.Parameters.AddWithValue("LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("AddressLine1", AddressLine1);
            cmd.Parameters.AddWithValue("AddressLine2", AddressLine2);
            cmd.Parameters.AddWithValue("County", County);
            cmd.Parameters.AddWithValue("State", State);
            cmd.Parameters.AddWithValue("City", City);
            cmd.Parameters.AddWithValue("zipcode", zipcode);
            cmd.Parameters.AddWithValue("landmark", landmark);
            cmd.Parameters.AddWithValue("FaxNo", FaxNo);
            cmd.Parameters.AddWithValue("Active", Active);
            cmd.Parameters.AddWithValue("ContactName", ContactName);
            cmd.Parameters.AddWithValue("ContactEmail", ContactEmail);
            cmd.Parameters.AddWithValue("MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("hdnstate", hdnstate);
            cmd.Parameters.AddWithValue("LocationName", LocationName);
            cmd.Parameters.AddWithValue("ShippingID", ShippingID);
            cmd.ExecuteNonQuery();
        }

        public string getFinApprovername(long ID, string[] conn)
        {
            string result = "";
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetFiananceApprover";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = dr[0].ToString();
            }
            dr.Close();
            return result;
        }

        public DataSet GetAddressType(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetAddresstypeValues";
            cmd.Connection = svr.GetSqlConn(conn);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long chkecommerceduplicate(long ParentID, long ID, string[] conn) 
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CheckEcommerce";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ParentID", ParentID);
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr[0].ToString());
            }
            dr.Close();
            return result;
        }

        public void InsertIntoDeptPayment(long DeptID, long PMethodID, int Sequence, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertDeptPMethod";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.Parameters.AddWithValue("PMethodID", PMethodID);
            cmd.Parameters.AddWithValue("Sequence", Sequence);
            cmd.ExecuteNonQuery();
        }

        public void DeleteRecordWithZeroQty(string[] conn)
        {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_DeleteRecordWithZeroQty";
                cmd.Connection = svr.GetSqlConn(conn);
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();
        }

        public void RemoveDeptPMethod(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_RemoveDeptMethod";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.ExecuteNonQuery();
        }

        public void UpdateDeptPaymentMethod(long DeptID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateDeptPMethod";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            cmd.ExecuteNonQuery();
        }

        public DataSet GetCostCenterList(long CompanyID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetCompanyCostCenter";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void DeleteZeroCompanyIDCostCenter(string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteCostCenterZeroQty";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.ExecuteNonQuery();
        }

        public void SaveCostCenter(string CenterName, string Code, long ApproverID, long CompanyID, string Remark, DateTime CreationDate, long CreatedBy, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_InsertIntoCostCenter";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CenterName", CenterName);
            cmd.Parameters.AddWithValue("Code", Code);
            cmd.Parameters.AddWithValue("ApproverID", ApproverID);
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("Remark", Remark);
            cmd.Parameters.AddWithValue("CreationDate", CreationDate);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
             cmd.ExecuteNonQuery();
        }

        public void RemoveCostCenter(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_RemoveCostCenter";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.ExecuteNonQuery();
        }

        public void UpdateCostCenterCmpanyID(long CompanyID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateCostCenterCompanyID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CompanyID", CompanyID);
            cmd.ExecuteNonQuery();
        }

        public long Duplicatecostcenter(string CenterName, string Code, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CheckCostCenterDuplicate";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CenterName", CenterName);
            cmd.Parameters.AddWithValue("Code", Code);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr[0].ToString());
            }
            dr.Close();
            return result;
        }

        public long CheckDuplicatePMethod(long PMethodID, long DeptID, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CheckDuplicatePmethod";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("PMethodID", PMethodID);
            cmd.Parameters.AddWithValue("DeptID", DeptID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr[0].ToString());
            }
            dr.Close();
            return result;
        }


        public int CheckLocationIDForAssignedUser(long Location, string[] conn)
        {
            int id = 0; ;
            DataTable dt =new DataTable();
            DataSet ds = new System.Data.DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection=svr.GetSqlConn(conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from mUserLocation where LocationID=" + Location + "";
            da.SelectCommand = cmd;
            da.Fill(ds,"tbl");
            dt = ds.Tables[0];
            if(dt.Rows.Count>0)           
            {
                id = 1;
            }
            return id;
        }

        public long GetContactIdasShippingId(long ID, string[] conn)
        {
            long ContactTableid = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetLocationContactID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ContactTableid = long.Parse(dr["ShippingID"].ToString());
            }
            dr.Close();
            return ContactTableid;
        }

        public void UpdateContacttableDetail(string Name, string EmailID, string MobileNo, long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_UpdateLocationContactTable";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Name", Name);
            cmd.Parameters.AddWithValue("EmailID", EmailID);
            cmd.Parameters.AddWithValue("MobileNo", MobileNo);
            cmd.Parameters.AddWithValue("ID", ID);
            cmd.ExecuteNonQuery();
        }

        public List<tContactPersonDetail> GetContactPersonListDeptWise(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tContactPersonDetail> ConPerLst = new List<tContactPersonDetail>();
            ConPerLst = (from dpt in ce.tContactPersonDetails
                         where dpt.CompanyID == CompanyID
                         select dpt).ToList();
            return ConPerLst;
        }

        public DataSet GetContactPersonLocList(long CompanyID, string[] conn)
        {
            DataSet ds = new DataSet();
            ds = fillds("select * from  tContactPersonDetail where CompanyID = " + CompanyID + " order by ID desc", conn);
            return ds;
        }
    

        #endregion


        # region new code for BrilliantWMS Project

        // Customer Grid Code
        
         public List<V_GetCustomerDetails>GetCustomerList(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_GetCustomerDetails> cust = new List<V_GetCustomerDetails>();
            cust = (from cm in ce.V_GetCustomerDetails
                  where cm.ParentID == CompanyID
                  orderby cm.ID descending
                    select cm).ToList();
            return cust;
        }

         public List<V_GetCustomerDetails> GetSuperAdminCustomerList(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_GetCustomerDetails> cust = new List<V_GetCustomerDetails>();
             cust = (from cm in ce.V_GetCustomerDetails
                     orderby cm.ID descending
                     select cm).ToList();
             return cust;
         }
        // Channel Grid Code

         public List<V_WMS_GetChannelDetails> GetChannelList(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetChannelDetails> chann = new List<V_WMS_GetChannelDetails>();
             chann = (from cm in ce.V_WMS_GetChannelDetails
                     orderby cm.ID descending
                     select cm).ToList();
             return chann;
         }

         public List<V_WMS_GetVendorDetails> GetVendorList(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetVendorDetails> vend = new List<V_WMS_GetVendorDetails>();
             vend = (from cm in ce.V_WMS_GetVendorDetails
                      orderby cm.ID descending
                      select cm).ToList();
             return vend;
         }

         public List<V_WMS_GetClientDetails> GetClientList(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetClientDetails> Client = new List<V_WMS_GetClientDetails>();
             Client = (from cm in ce.V_WMS_GetClientDetails
                     orderby cm.ID descending
                     select cm).ToList();
             return Client;
         }

         public List<V_WMS_GetRateCardDetails> GetRateCardDetails(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetRateCardDetails> rate = new List<V_WMS_GetRateCardDetails>();
             rate = (from cm in ce.V_WMS_GetRateCardDetails
                       orderby cm.ID descending
                       select cm).ToList();
             return rate;
         }

         public DataSet GetAggregatorList(string[] conn)
         {
             SqlCommand cmd = new SqlCommand();
             SqlDataAdapter da = new SqlDataAdapter();
             DataSet ds = new DataSet();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_WMS_GetAggregatorList";
             cmd.Connection = svr.GetSqlConn(conn);
             cmd.Parameters.Clear();
             da.SelectCommand = cmd;
             da.Fill(ds);
             return ds;
         }

         public DataSet GetTermsnConditionList(string[] conn)
         {
             SqlCommand cmd = new SqlCommand();
             SqlDataAdapter da = new SqlDataAdapter();
             DataSet ds = new DataSet();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_WMS_GetTermsnConditionList";
             cmd.Connection = svr.GetSqlConn(conn);
             cmd.Parameters.Clear();
             da.SelectCommand = cmd;
             da.Fill(ds);
             return ds;
         }


        #region Code for Copany Master
         public long InsertConfiguration(mConfiguration company, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             ce.mConfigurations.AddObject(company);
             ce.SaveChanges();
             return 1;
         }

         public List<mDropdownValue> GetContactTypeList(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<mDropdownValue> Subsription = new List<mDropdownValue>();
             Subsription = (from p in ce.mDropdownValues
                            where p.Parameter == "Subscription"
                            select p).ToList();
             return Subsription;

         }

         public mConfiguration GetCompanyConfiguration(Int64 CompanyID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

             mConfiguration Company = new mConfiguration();
             Company = (from p in ce.mConfigurations
                        where p.ReferenceID == CompanyID
                        select p).FirstOrDefault();

             return Company;
         }

         public int UpdateCompanyCongig(mConfiguration UpdateCompany, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

             ce.mConfigurations.Attach(UpdateCompany);
             ce.ObjectStateManager.ChangeObjectState(UpdateCompany, EntityState.Modified);
             ce.SaveChanges();
             return 1;
         }



        #endregion

        #region Code For Customer Master
         public List<mCompany> GetCompanyDropDown(string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<mCompany> Company = new List<mCompany>();
             Company = (from p in ce.mCompanies
                            where p.Active=="Y"
                            select p).ToList();
             return Company;

         }

         public long InsertCustomer(mCustomer Customer, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             ce.mCustomers.AddObject(Customer);
             ce.SaveChanges();
             return Customer.ID;
         }

         public mCustomer GetCustomerbyID(Int64 CustomerID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

             mCustomer Customer = new mCustomer();
             Customer = (from p in ce.mCustomers
                        where p.ID == CustomerID
                        select p).FirstOrDefault();

             return Customer;
         }

         public int UpdateCustomer(mCustomer UpdateCustomer, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));

             ce.mCustomers.Attach(UpdateCustomer);
             ce.ObjectStateManager.ChangeObjectState(UpdateCustomer, EntityState.Modified);
             ce.SaveChanges();
             return 1;
         }

        
         public List<V_WMS_GetRateCardDetails> GetCustomerRateCard(Int64 CustomerID, string Type, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetRateCardDetails> rate = new List<V_WMS_GetRateCardDetails>();
             rate = (from cm in ce.V_WMS_GetRateCardDetails
                     where cm.AccountID == CustomerID && cm.AccountType == Type  
                     select cm).ToList();
             return rate;
         }


        #endregion

        #region Code for Channel Master

         public DataSet GetChannelName(string Parameter, string[] conn)
         {
             SqlCommand cmd = new SqlCommand();
             SqlDataAdapter da = new SqlDataAdapter();
             DataSet ds = new DataSet();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_WMS_GetChannelName";
             cmd.Connection = svr.GetSqlConn(conn);
             cmd.Parameters.Clear();
             cmd.Parameters.AddWithValue("Parameter", Parameter);
             da.SelectCommand = cmd;
             da.Fill(ds);
             return ds;
         }


         public long SaveChannelDetail(mChannel Channel, string[] conn)
         {
             try
             {
                 BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                 if (Channel.ID == 0)
                 {
                     db.mChannels.AddObject(Channel);
                     db.SaveChanges();
                 }
                 else
                 {
                     db.mChannels.Attach(Channel);
                     db.ObjectStateManager.ChangeObjectState(Channel, EntityState.Modified);
                     db.SaveChanges();
                 }
                 return Channel.ID;
             }
             catch
             {
                 return 0;
             }
         }


         public mChannel GetChannelByChanID(long ChannelID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             mChannel channel = new mChannel();
             channel = (from p in ce.mChannels
                      where p.ID == ChannelID
                      select p).FirstOrDefault();
             return channel;
         }

        #endregion

        #region Code for Aggregator Master

        // Aggreegator Master Code

         public long SaveAggregatorMaster(mAgreegator Aggrm, string[] conn)
         {
             try
             {
                 BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                 if (Aggrm.ID == 0)
                 {
                     db.mAgreegators.AddObject(Aggrm);
                     db.SaveChanges();
                 }
                 else
                 {
                     db.mAgreegators.Attach(Aggrm);
                     db.ObjectStateManager.ChangeObjectState(Aggrm, EntityState.Modified);
                     db.SaveChanges();
                 }
                 return Aggrm.ID;
             }
             catch
             {
                 return 0;
             }
         }

         public mAgreegator GetAggregatorMasterbyID(long AggregatID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             mAgreegator aggri = new mAgreegator();
             aggri = (from p in ce.mAgreegators
                        where p.ID == AggregatID
                        select p).FirstOrDefault();
             return aggri;
         }


        // Aggreegator API Code

         public long SaveAggreegatorAPI(mAgreegatorAPI AggriAPI, string[] conn)
         {
             try
             {
                 BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                 if (AggriAPI.ID == 0)
                 {
                     db.mAgreegatorAPIs.AddObject(AggriAPI);
                     db.SaveChanges();
                 }
                 else
                 {
                     db.mAgreegatorAPIs.Attach(AggriAPI);
                     db.ObjectStateManager.ChangeObjectState(AggriAPI, EntityState.Modified);
                     db.SaveChanges();
                 }
                 return AggriAPI.ID;
             }
             catch
             {
                 return 0;
             }
         }

         public mAgreegatorAPI GetAggreegatorAPIbyID(long AggregatAPIID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             mAgreegatorAPI AggriAPI = new mAgreegatorAPI();
             AggriAPI = (from p in ce.mAgreegatorAPIs
                      where p.ID == AggregatAPIID
                      select p).FirstOrDefault();
             return AggriAPI;
         }

         public List<mAgreegatorAPI> GetAPIListByID(long AggreID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<mAgreegatorAPI> API = new List<mAgreegatorAPI>();
             API = (from cm in ce.mAgreegatorAPIs
                    where cm.AgreegatorID == AggreID
                     orderby cm.ID descending
                     select cm).ToList();
             return API;
         }

        #endregion


        #region Code For Rate Card Master
         public DataSet GetRateTypeDropdown(string Parameter, string[] conn)
         {
             SqlCommand cmd = new SqlCommand();
             SqlDataAdapter da = new SqlDataAdapter();
             DataSet ds = new DataSet();
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.CommandText = "SP_WMS_GetDropDownByParameter";
             cmd.Connection = svr.GetSqlConn(conn);
             cmd.Parameters.Clear();
             cmd.Parameters.AddWithValue("Parameter", Parameter);
             da.SelectCommand = cmd;
             da.Fill(ds);
             return ds;
         }

         public long SaveRateCardMaster(mRateCard ratecard, string[] conn)
         {
             try
             {
                 BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                 if (ratecard.ID == 0)
                 {
                     db.mRateCards.AddObject(ratecard);
                     db.SaveChanges();
                 }
                 else
                 {
                     db.mRateCards.Attach(ratecard);
                     db.ObjectStateManager.ChangeObjectState(ratecard, EntityState.Modified);
                     db.SaveChanges();
                 }
                 return ratecard.ID;
             }
             catch
             {
                 return 0;
             }
         }

         public mRateCard GetRateCardByID(long RateCardID, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             mRateCard RateCard = new mRateCard();
             RateCard = (from p in ce.mRateCards
                         where p.ID == RateCardID
                         select p).FirstOrDefault();
             return RateCard;
         }

         public List<V_WMS_GetRateCardDetails> GetVendorRateByVendorID(long VendorID, string type, string[] conn)
         {
             BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
             List<V_WMS_GetRateCardDetails> RateCard = new List<V_WMS_GetRateCardDetails>();
             RateCard = (from p in ce.V_WMS_GetRateCardDetails
                         where p.AccountID == VendorID && p.AccountType == type
                         select p).ToList();
             return RateCard;
         }



        #endregion

        # endregion


    }



}
