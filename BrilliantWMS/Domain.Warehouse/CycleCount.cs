using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.Warehouse;
using System.ServiceModel;
using System.Xml.Linq;
using System.Data.Objects;
using Domain.Server;
using System.Data.Linq;

namespace Domain.Warehouse
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]

    public partial class CycleCount : Interface.Warehouse.iCycleCount
    {
        Domain.Server.Server svr = new Server.Server();

        public List<V_WMS_GetHeadCycleCount> GetCycleCountMain(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_GetHeadCycleCount> count = new List<V_WMS_GetHeadCycleCount>();
            count = (from cm in ce.V_WMS_GetHeadCycleCount
                    orderby cm.ID descending
                    select cm).ToList();
            return count;
        }

        public long SaveCycleCounttemp(CycleCountTemp CycleTemp, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                db.CycleCountTemps.AddObject(CycleTemp);
                db.SaveChanges();
                return CycleTemp.ID;
            }
            catch
            {
                return 0;
            }
        }

        public DataSet GetCycleCounttempdata(string Object, long CreatedBy, string SessionID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetCycleCountTempData";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Object", Object);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("SessionID", SessionID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public DataSet GetCycleCountTempDataByLoc(string Object, long CreatedBy, string SessionID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetCycleTempforLocation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Object", Object);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("SessionID", SessionID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long SaveCycleCountHead(tCycleCountHead CycleHead, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {
               
                if (CycleHead.ID == 0)
                {
                    db.tCycleCountHeads.AddObject(CycleHead);
                    db.SaveChanges();
                }
                else
                {
                    db.tCycleCountHeads.Attach(CycleHead);
                    db.ObjectStateManager.ChangeObjectState(CycleHead, EntityState.Modified);
                    db.SaveChanges();
                }
                return CycleHead.ID;
            }
            catch
            {
                return 0;
            }
        }

        public DataSet GetCycleTempDataToInsert(string Object, long CreatedBy, string SessionID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetCycletempToInsert";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Object", Object);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("SessionID", SessionID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public void SaveCyclePlanData(DataTable DetailInsertion, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_InsertIntoCyclePlann";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("DetailInsertion", DetailInsertion);
            cmd.ExecuteNonQuery();
        }

        public void DeleteCycletempData(string Object, long CreatedBy, string SessionID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_DeleteCycleTempData";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Object", Object);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("SessionID", SessionID);
            cmd.ExecuteNonQuery();
        }

        public void DeleteCycleTempWithoutObj(long CreatedBy, string SessionID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DeleteCycleTempWithoutObj";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("SessionID", SessionID);
            cmd.ExecuteNonQuery();
        }

        public tCycleCountHead GetCyleCountHeadByID(long CycleHeadID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tCycleCountHead HeadCycle = new tCycleCountHead();
            HeadCycle = (from p in ce.tCycleCountHeads
                         where p.ID == CycleHeadID
                     select p).FirstOrDefault();
            return HeadCycle;
        }

        public long GetLocationID(string Code, long WarehouseID, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetLocIDByLocCode";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Code", Code);
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr["ID"].ToString());
            }
            dr.Close();
            return result;
        }

        public long CheckLocInPlann(string Object, long CycleHeadID, long ReferenceID, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetLoCProdInPlanCycle";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Object", Object);
            cmd.Parameters.AddWithValue("CycleHeadID", CycleHeadID);
            cmd.Parameters.AddWithValue("ReferenceID", ReferenceID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr["CountNo"].ToString());
            }
            dr.Close();
            return result;
        }


        public long SaveCycleCount(tCycleCountDetail cycledetail, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            try
            {

                if (cycledetail.ID == 0)
                {
                    db.tCycleCountDetails.AddObject(cycledetail);
                    db.SaveChanges();
                }
                else
                {
                    db.tCycleCountDetails.Attach(cycledetail);
                    db.ObjectStateManager.ChangeObjectState(cycledetail, EntityState.Modified);
                    db.SaveChanges();
                }
                return cycledetail.ID;
            }
            catch
            {
                return 0;
            }
        }

        public long GetSKUID(string ProductCode, long WarehouseID, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetSKUIDBySkuCode";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProductCode", ProductCode);
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr["ID"].ToString());
            }
            dr.Close();
            return result;
        }

        public List<tCycleCountDetail> GetCycleCountDetail(long CycleHeadID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<tCycleCountDetail> count = new List<tCycleCountDetail>();
            count = (from cm in ce.tCycleCountDetails
                     where cm.CountHeadID == CycleHeadID
                     orderby cm.ID descending
                     select cm).ToList();
            return count;
        }

        public DataSet GetRepeatedCycleCountData(long CountHeadID, long SKUID, long LocationID, string BatchCode, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetRepeatCycleCountRecord";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("CountHeadID", CountHeadID);
            cmd.Parameters.AddWithValue("SKUID", SKUID);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            cmd.Parameters.AddWithValue("BatchCode", BatchCode);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long GetSKUIDBySKUCode(string ProductCode, long WarehouseID, string[] conn)
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetSKUIdBySKUCode";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ProductCode", ProductCode);
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr["ID"].ToString());
            }
            dr.Close();
            return result;
        }

        public DataSet GetBatchCodeBySKU(long SKUId, long LocationID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetBatchCodeInfo";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId", SKUId);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public decimal GetSystemQtyByBatch(long SKUId, long LocationID, string BatchCode, string[] conn)
        {
            decimal result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetSystemQtyByBatch";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId", SKUId);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            cmd.Parameters.AddWithValue("BatchCode", BatchCode);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = decimal.Parse(dr["ClosingBalance"].ToString());
            }
            dr.Close();
            return result;
        }

        public void UpdateStockSkuTransForFromLoc(long SKUId, string BatchCode, long LocationID, decimal DiffQty, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_UpdateCycleFromStock";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId", SKUId);
            cmd.Parameters.AddWithValue("BatchCode", BatchCode);
            cmd.Parameters.AddWithValue("LocationID", LocationID);
            cmd.Parameters.AddWithValue("DiffQty", DiffQty);
            cmd.ExecuteNonQuery();
        }

        public void UpdateStocktransToLoc(long SKUId, string BatchCode, decimal DiffQty, long LocationID, long CreatedBy, long CycleHeadID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_UpdateCycleToStock";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SKUId", SKUId);
            cmd.Parameters.AddWithValue("BatchCode", BatchCode);
            cmd.Parameters.AddWithValue("DiffQty", DiffQty);
            cmd.Parameters.AddWithValue("ToLocationID", LocationID);
            cmd.Parameters.AddWithValue("CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("CycleHeadID", CycleHeadID);
            cmd.ExecuteNonQuery();
        }

        public decimal getLocationRemainingQty(long ID, string[] conn)
        {
            decimal result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetLocRemainCapacity";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = decimal.Parse(dr["RemainingBal"].ToString());
            }
            dr.Close();
            return result;
        }

        public DataSet getCompanyCustomer(long ID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_GetCompanyCustomerByWarehouse";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("ID", ID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        public long Chk()
        {
            long result = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ChkDuplicateCycleRecord";
            //cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            //cmd.Parameters.AddWithValue("ProductCode", ProductCode);
            //cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                result = long.Parse(dr["ID"].ToString());
            }
            dr.Close();
            return result;
        }
    }

}
