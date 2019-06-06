using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.Configuration;
using Interface.Product;
using System.ServiceModel;
using Domain.Server;

namespace Domain.Product
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public partial class UCProductSearch : Interface.Product.iUCProductSearch
    {
        Domain.Server.Server svr = new Server.Server();
        public List<GetProductDetail> GetProductList(string filter, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<GetProductDetail> result = new List<GetProductDetail>();
            if (filter == "")
            {
                result = (from lst in db.GetProductDetails
                          select lst).ToList();
            }
            else
            {
                List<GetProductDetail> filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductCode.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.Name.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.Description.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductCategory.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductSubCategory.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

            }
            if (result.Count == 0)
            {
                result = null;
            }
            return result;
        }

        public List<GetProductDetail> GetProductList1(int pageIndex, string filter, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<GetProductDetail> result = new List<GetProductDetail>();
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.GetProductDetails
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<GetProductDetail> filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductCode.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.Name.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.Description.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductCategory.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                filterList = new List<GetProductDetail>();
                filterList = db.GetProductDetails.Where(p => p.ProductSubCategory.Contains(filter)).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                result = result.Take(50 * pageIndex).ToList();

            }
            if (result.Count == 0)
            {
                result = null;
            }
            return result;
        }

        public List<VW_GetSKUDetailsWithPack> GetSKUListDeptWise(int pageIndex, string filter, long UserID, long DeptID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<VW_GetSKUDetailsWithPack> result = new List<VW_GetSKUDetailsWithPack>();

            //var DeptID = (from d in db.mUserTerritoryDetails
            //               where d.UserID == UserID
            //               select d.TerritoryID).First();

            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.VW_GetSKUDetailsWithPack
                          where lst.StoreId == DeptID
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<VW_GetSKUDetailsWithPack> filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => (p.ProductCode.Contains(filter) || p.Name.Contains(filter) || p.Packkey.Contains(filter) || p.GroupSet.Contains(filter)) && p.StoreId == DeptID).ToList();
                //  if (filterList.Count > 0) result.AddRange(filterList);

                result = (from lst in db.VW_GetSKUDetailsWithPack
                          where (lst.ProductCode.Contains(filter) || lst.Name.Contains(filter) || lst.Packkey.Contains(filter) || lst.GroupSet.Contains(filter))
                          && lst.StoreId == DeptID
                          select lst).Take(50 * pageIndex).ToList();



                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Name.Contains(filter) && p.StoreId == DeptID).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Description.Contains(filter) && p.StoreId == DeptID).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Packkey.Contains(filter) && p.StoreId == DeptID).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.GroupSet.Contains(filter) && p.StoreId == DeptID).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);


                //  result = result.Take(50 * pageIndex).ToList();
            }
            //if (result.Count == 0)
            //{
            //    result = null;                
            //}
            result = result.Where(a => a.Active == "Y").ToList();
            return result;
        }

        public List<VW_GetSKUDetailsWithPack> GetSKUList(int pageIndex, string filter, long DeptID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<VW_GetSKUDetailsWithPack> result = new List<VW_GetSKUDetailsWithPack>();
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.VW_GetSKUDetailsWithPack
                          where lst.StoreId == DeptID
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<VW_GetSKUDetailsWithPack> filterList = new List<VW_GetSKUDetailsWithPack>();
                filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.ProductCode.Contains(filter) || p.Name.Contains(filter) || p.Description.Contains(filter) || p.Packkey.Contains(filter) || p.GroupSet.Contains(filter) && p.StoreId == DeptID).ToList();
                if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();                
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Name.Contains(filter)).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();                
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Description.Contains(filter)).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.Packkey.Contains(filter)).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);

                //filterList = new List<VW_GetSKUDetailsWithPack>();
                //filterList = db.VW_GetSKUDetailsWithPack.Where(p => p.GroupSet.Contains(filter)).ToList();
                //if (filterList.Count > 0) result.AddRange(filterList);


                result = result.Take(50 * pageIndex).ToList();
            }
            if (result.Count == 0)
            {
                result = null;
            }
            return result;
        }

        public List<mProduct> SKUListForGrid(int CompanyID, int DeptID, int GroupSetID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<mProduct> SKUlst = new List<mProduct>();

            SKUlst = (from lst in db.mProducts
                      where lst.CompanyID == CompanyID && lst.StoreId == DeptID && lst.BOMHeaderId == GroupSetID
                      select lst).ToList();
            return SKUlst;
        }

        #region WarehouseProductSearch

        public List<VW_GetSKUDetailsWithPack> GetSKUListWarehouseWise(int pageIndex, string filter, long WarehouseID, string[] conn)
        {
            BISPL_CRMDBEntities db = new BISPL_CRMDBEntities(svr.GetEntityConnection(conn));
            List<VW_GetSKUDetailsWithPack> result = new List<VW_GetSKUDetailsWithPack>();
            pageIndex = pageIndex + 1;
            if (filter == "")
            {
                result = (from lst in db.VW_GetSKUDetailsWithPack
                          where lst.WarehouseID == WarehouseID
                          select lst).Take(50 * pageIndex).ToList();
            }
            else
            {
                List<VW_GetSKUDetailsWithPack> filterList = new List<VW_GetSKUDetailsWithPack>();

                result = (from lst in db.VW_GetSKUDetailsWithPack
                          where (lst.ProductCode.Contains(filter) || lst.Name.Contains(filter) || lst.Packkey.Contains(filter) || lst.GroupSet.Contains(filter))
                          && lst.WarehouseID == WarehouseID
                          select lst).Take(50 * pageIndex).ToList();
            }
            result = result.Where(a => a.Active == "Y").ToList();
            return result;
        }
        #endregion

        #region Main SKU Search Code
         

        #endregion

    }
}
