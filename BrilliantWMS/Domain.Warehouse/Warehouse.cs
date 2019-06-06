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

    public partial class Warehouse : Interface.Warehouse.iWarehouse
    {
        Domain.Server.Server svr = new Server.Server();

        public List<V_WMS_GetWarehouseDetails> GetWarehouseList(long CompanyID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_GetWarehouseDetails> ware = new List<V_WMS_GetWarehouseDetails>();
            ware = (from cm in ce.V_WMS_GetWarehouseDetails
                    orderby cm.ID descending
                    select cm).ToList();
            return ware;
        }

        public V_WMS_GetWarehouseDetails GetWarehouseDetailByID(long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            V_WMS_GetWarehouseDetails Warehouse = new V_WMS_GetWarehouseDetails();
            Warehouse = db.V_WMS_GetWarehouseDetails.Where(p => p.ID == WarehouseID).FirstOrDefault();
            return Warehouse;
        }

        public long SaveWarehouseMaster(mWarehouseMaster Warehouse, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (Warehouse.ID == 0)
                {
                    db.mWarehouseMasters.AddObject(Warehouse);
                    db.SaveChanges();
                }
                else
                {
                    db.mWarehouseMasters.Attach(Warehouse);
                    db.ObjectStateManager.ChangeObjectState(Warehouse, EntityState.Modified);
                    db.SaveChanges();
                }

                return Warehouse.ID;
            }
            catch
            {
                return 0;
            }
        }

        public long SaveWarehouseAddress(tAddress WMaddress, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (WMaddress.ID == 0)
                {
                    db.tAddresses.AddObject(WMaddress);
                    db.SaveChanges();
                }
                else
                {
                    db.tAddresses.Attach(WMaddress);
                    db.ObjectStateManager.ChangeObjectState(WMaddress, EntityState.Modified);
                    db.SaveChanges();
                }

                return WMaddress.ID;
            }
            catch
            {
                return 0;
            }
        }

        public tAddress GetWarehouseAddress(long ReferenceID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            tAddress wmaddress = new tAddress();
            wmaddress = db.tAddresses.Where(p => p.ReferenceID == ReferenceID).FirstOrDefault();
            return wmaddress;
        }

        public mWarehouseMaster GetWarehouseMasterByID(Int64 WarehID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mWarehouseMaster warehouse = new mWarehouseMaster();
            warehouse = (from p in ce.mWarehouseMasters
                         where p.ID == WarehID
                       select p).FirstOrDefault();
            return warehouse;
        }

        public List<V_WMS_WarehouseLocation> GetWarehouseLocation(long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<V_WMS_WarehouseLocation> location = new List<V_WMS_WarehouseLocation>();
            location = (from cm in ce.V_WMS_WarehouseLocation
                    where cm.WarehouseID == WarehouseID
                    select cm).ToList();
            return location;
        }


        public DataSet GetWarehouseLocationByID(long WarehouseID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetWarehouseLocation";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }

        // Warehouse Building Code

        public List<mWarehouseBuilding> GetWarehouseBuilding(long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mWarehouseBuilding> Building = new List<mWarehouseBuilding>();
            Building = (from cm in ce.mWarehouseBuildings
                        where cm.WarehouseID == WarehouseID
                        select cm).ToList();
            return Building;
        }

        public mWarehouseBuilding GetWareBuildingByID(long buildingID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mWarehouseBuilding building = new mWarehouseBuilding();
            building = (from p in ce.mWarehouseBuildings
                         where p.ID == buildingID
                         select p).FirstOrDefault();
            return building;
        }

        public long SaveWareBuilding(mWarehouseBuilding WBuilding, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (WBuilding.ID == 0)
                {
                    db.mWarehouseBuildings.AddObject(WBuilding);
                    db.SaveChanges();
                }
                else
                {
                    db.mWarehouseBuildings.Attach(WBuilding);
                    db.ObjectStateManager.ChangeObjectState(WBuilding, EntityState.Modified);
                    db.SaveChanges();
                }
                return WBuilding.ID;
            }
            catch
            {
                return 0;
            }
        }

        // Warehouse floar Code

        public List<mFloar> GetWarehouseFloar(long BuildingID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mFloar> floar = new List<mFloar>();
            floar = (from cm in ce.mFloars
                     where cm.BuildingID == BuildingID
                        select cm).ToList();
            return floar;
        }

        public mFloar GetWarehouseFloarbyID(long FloarID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mFloar floar = new mFloar();
            floar = (from p in ce.mFloars
                        where p.ID == FloarID
                        select p).FirstOrDefault();
            return floar;
        }

        public long SaveWarehouseFloar(mFloar wfloar, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (wfloar.ID == 0)
                {
                    db.mFloars.AddObject(wfloar);
                    db.SaveChanges();
                }
                else
                {
                    db.mFloars.Attach(wfloar);
                    db.ObjectStateManager.ChangeObjectState(wfloar, EntityState.Modified);
                    db.SaveChanges();
                }
                return wfloar.ID;
            }
            catch
            {
                return 0;
            }
        }

        // Warehouse Passage Code

        public List<mPathway> GetWarehousePassage(long floarID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mPathway> path = new List<mPathway>();
            path = (from cm in ce.mPathways
                    where cm.FloarID == floarID
                     select cm).ToList();
            return path;
        }

        public mPathway GetWarehousePassageByID(long PassageID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mPathway floar = new mPathway();
            floar = (from p in ce.mPathways
                     where p.ID == PassageID
                     select p).FirstOrDefault();
            return floar;
        }

        public long SaveWarehousePassage(mPathway wpassage, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (wpassage.ID == 0)
                {
                    db.mPathways.AddObject(wpassage);
                    db.SaveChanges();
                }
                else
                {
                    db.mPathways.Attach(wpassage);
                    db.ObjectStateManager.ChangeObjectState(wpassage, EntityState.Modified);
                    db.SaveChanges();
                }
                return wpassage.ID;
            }
            catch
            {
                return 0;
            }
        }

        // Warehouse Section Code
        public List<mSection> GetWarehouseSection(long PassageID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mSection> Sec = new List<mSection>();
            Sec = (from cm in ce.mSections
                    where cm.PathID == PassageID
                    select cm).ToList();
            return Sec;
        }

        public mSection GetWarehouseSectionByID(long SectionID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mSection sec = new mSection();
            sec = (from p in ce.mSections
                     where p.ID == SectionID
                     select p).FirstOrDefault();
            return sec;
        }

        public long SaveWarehouseSection(mSection wsection, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (wsection.ID == 0)
                {
                    db.mSections.AddObject(wsection);
                    db.SaveChanges();
                }
                else
                {
                    db.mSections.Attach(wsection);
                    db.ObjectStateManager.ChangeObjectState(wsection, EntityState.Modified);
                    db.SaveChanges();
                }
                return wsection.ID;
            }
            catch
            {
                return 0;
            }
        }

        // Warehouse Shelf Code

        public List<mShelf> GetWarehouseShelf(long SectionID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mShelf> shelf = new List<mShelf>();
            shelf = (from cm in ce.mShelves
                   where cm.SectionID == SectionID
                   select cm).ToList();
            return shelf;
        }

        public mShelf GetWarehouseShelfByID(long ShelfID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            mShelf shelf = new mShelf();
            shelf = (from p in ce.mShelves
                   where p.ID == ShelfID
                   select p).FirstOrDefault();
            return shelf;
        }

        public long SaveWarehouseShelf(mShelf wshelf, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (wshelf.ID == 0)
                {
                    db.mShelves.AddObject(wshelf);
                    db.SaveChanges();
                }
                else
                {
                    db.mShelves.Attach(wshelf);
                    db.ObjectStateManager.ChangeObjectState(wshelf, EntityState.Modified);
                    db.SaveChanges();
                }
                return wshelf.ID;
            }
            catch
            {
                return 0;
            }
        }

        // Warehouse Location Code

        public List<mDropdownValue> GetLocationType(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mDropdownValue> Subsription = new List<mDropdownValue>();
            Subsription = (from p in ce.mDropdownValues
                           where p.Parameter == "LocType"
                           select p).ToList();
            return Subsription;

        }

        public List<mDropdownValue> GetCapacityIn(string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mDropdownValue> Subsription = new List<mDropdownValue>();
            Subsription = (from p in ce.mDropdownValues
                           where p.Parameter == "Capacity"
                           select p).ToList();
            return Subsription;

        }

        public long SaveWarehouseLocation(mLocation wlocation, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
                if (wlocation.ID == 0)
                {
                    db.mLocations.AddObject(wlocation);
                    db.SaveChanges();
                }
                else
                {
                    db.mLocations.Attach(wlocation);
                    db.ObjectStateManager.ChangeObjectState(wlocation, EntityState.Modified);
                    db.SaveChanges();
                }
                return wlocation.ID;
            }
            catch
            {
                return 0;
            }
        }

        public long CheckDuplicateLocation(string Code, long WarehouseID, string[] conn)
        {
            long locationcount = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_CHKDupliLocationCode";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("Code", Code);
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                locationcount = long.Parse(dr["duplicate"].ToString());
            }
            dr.Close();
            return locationcount;
        }

        public long CheckDuplicateSortCode(long SortCode, long WarehouseID, string[] conn)
        {
            long SortCount = 0;
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_CHKDupliSortCode";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("SortCode", SortCode);
            cmd.Parameters.AddWithValue("WarehouseID", WarehouseID);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                SortCount = long.Parse(dr["duplicate"].ToString());
            }
            dr.Close();
            return SortCount;
        }

        public V_WMS_GetWareLocationByLocID GetWarehouseLocByID(long LocationID, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities ce = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            V_WMS_GetWareLocationByLocID Location = new V_WMS_GetWareLocationByLocID();
            Location = (from p in ce.V_WMS_GetWareLocationByLocID
                     where p.ID == LocationID && p.WarehouseID == WarehouseID 
                     select p).FirstOrDefault();
            return Location;
        }

        public DataSet GetWarehousebyUserID(long UserID, string[] conn)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_WMS_GetWarehouseByUserID";
            cmd.Connection = svr.GetSqlConn(conn);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("UserID", UserID);
            da.SelectCommand = cmd;
            da.Fill(ds);
            return ds;
        }
        public void AddRecordInSkuTransaction(tSKUTransaction skutrans, string[] conn)
        {
            try
            {
                BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
               db.tSKUTransactions.AddObject(skutrans);
               db.SaveChanges();             
            }
            catch
            {
               
            }
        }
    }
}
