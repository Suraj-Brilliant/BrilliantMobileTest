using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using System.Collections;

namespace BrilliantWMS.Warehouse
{
    public partial class PutIn : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        string Sortcode = "";
        string SelectedPO, MCN, PONum, ProdIds, Desc;
        string[] POs = { "0" };
        char[] ch = { ',' };

        static string ObjectName = "PutIn";

        protected void Page_Load(object sender, EventArgs e)
        {

            UCFormHeader1.FormHeaderText = "Receiving";

            if (!IsPostBack)
            {
                BindGrid();

                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");
                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetAddNewRight(false, "Not Allowed");
                Toolbar1.SetImportRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
                //try
                //{
                //    if ((Request.QueryString["PO"] != "") || (Request.QueryString["PO"] != null))
                //    {
                //       // iPurchaseOrderClient ShipClient = new iPurchaseOrderClient();
                //        //ShipClient.ClearTempData();
                //        SelectedPO = Request.QueryString["PO"].ToString();
                //        SelectedPOs.Value = SelectedPO;
                //        Fill_SortCode();
                //        CreateGrid();
                //        int a = ValidateStatus(SelectedPO);
                //        if (a == 0)
                //        {
                //            Button1.Enabled = false;
                //            BtnClearGrid.Enabled = false;
                //            BtnSequence.Enabled = false;
                //            ddlLocation.Enabled = false;
                //            ddlPoList.Enabled = false;
                //            txtReceiQty.Enabled = false;
                //            //iReceivableClient ReceivableClient = new iReceivableClient();
                //            //ds = ReceivableClient.DisplayGrid(SelectedPO);
                //            GridReceipt.DataSource = ds;
                //            GridReceipt.DataBind();
                //           // ReceivableClient.Close();
                //        }
                //        ShowPODetails(SelectedPOs.Value);
                //    }
                //}
                //catch
                //{
                //}
            }
        }

        public void BindGrid()
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["QCID"] != null)
                {
                    //DataSet ds = new DataSet();
                    //ds = 
                    if (Session["QCstate"].ToString() == "View")
                    {                        
                        GridReceipt.Columns[0].Visible = false;
                        long qcID=long.Parse(Session["QCID"].ToString());
                        DataSet ds = new DataSet();
                        ds = Inbound.GetSavedPutInListbyQCID(long.Parse(Session["QCID"].ToString()), profile.DBConnection._constr);
                        GridReceipt.DataSource = ds; // Inbound.GetSavedPutInListbyQCID(long.Parse(Session["QCID"].ToString()), profile.DBConnection._constr);
                        GridReceipt.DataBind();
                        hdnPutInNo.Value=ds.Tables[0].Rows[0]["ID"].ToString();
                        BtnSequence.Visible = false; btnUnpacked.Visible = false;
                    }
                    else 
                    {
                        GridReceipt.DataSource = Inbound.GetPutInList(Session["QCID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        GridReceipt.DataBind(); 
                        BtnSequence.Visible = true; btnUnpacked.Visible = true; 
                    }                    
                }
                else if (Session["TRID"] != null)
                {
                    
                    GridReceipt.DataSource = Inbound.GetPutInListByTRID(long.Parse(Session["TRID"].ToString()), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    GridReceipt.DataBind();
                    if (Session["TRstate"].ToString() == "View")
                    {
                        BtnSequence.Visible = false; imgBtnEdit1.Visible = false; btnUnpacked.Visible = false;
                    }
                    else { BtnSequence.Visible = true; imgBtnEdit1.Visible = true; btnUnpacked.Visible = true; }
                }
               
            }
            catch { }
            finally { Inbound.Close(); }
            //iReceivableClient ReceivableClient = new iReceivableClient();

            //DataSet ds = ReceivableClient.GetTempData();
            //GridReceipt.DataSource = ds;
            //GridReceipt.DataBind();
        }

       // protected void gvPrdPutIn_Select(object sender, Obout.Grid.GridRecordEventArgs e)
        protected void gvPrdPutIn_Select(object sender,EventArgs e)
        {
            try
            {
                Hashtable selectedrec = (Hashtable)GridReceipt.SelectedRecords[0];
                hdnSelectedPutInRec.Value = selectedrec["Sequence"].ToString();
                TextBox1.Text = selectedrec["ProductCode"].ToString();
                txtReceiQty.Text = selectedrec["LocQty"].ToString();
                txtLocation.Text = selectedrec["Code"].ToString();
                txtLocationCapacity.Text = selectedrec["Capacity"].ToString();
                txtAvlBlc.Text = selectedrec["AvailableBalance"].ToString();

                Button1.Visible = true;
                BtnClearGrid.Visible= true;
                imgSearch.Visible = true;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "PutIn.aspx", "gvPrdPutIn_Select");
            }
            finally
            {
            }
        }

        # region Code by Pallavi
        #region CreateGrid
        public void CreateGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("Prod_Code");
            DataColumn dc1 = new DataColumn("ReceivedQty");
            DataColumn dc2 = new DataColumn("Location");
            DataColumn dc3 = new DataColumn("SortCode");
            DataColumn dc4 = new DataColumn("BarCode");
            dt.Columns.Add(dc);
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            ds.Tables.Add(dt);
            Session["data"] = ds;

        }
        #endregion

        # region ShowPODetails

        public int ValidateStatus(string selPos)
        {
            int result = 0;
            //iPurchaseOrderClient Purchase = new iPurchaseOrderClient();
            //try
            //{
            //    result = Purchase.ValidateGreenPOStatus(selPos);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Purchase.Close();
            //}
            return result;
        }
        public void ShowPODetails(string selPos)
        {
            // iReceivableClient ReceivableClient = new iReceivableClient();
            try
            {
                POs = selPos.Split(ch);
                for (int i = 0; i < POs.Length; i++)
                {

                    DataSet dss = new DataSet();
                    // dss = ReceivableClient.GetPODetails(long.Parse(POs[i].ToString()));
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                        {
                            if (j == 0 && i == 0)
                            {
                                if (dss.Tables[0].Rows[j][2].ToString() != "")
                                {
                                    MCN = dss.Tables[0].Rows[j][2].ToString().Trim();
                                }
                                else { MCN = "Not Assigned"; }
                                PONum = dss.Tables[0].Rows[j][0].ToString().Trim();
                                if (dss.Tables[0].Rows[j][3].ToString() != "")
                                {
                                    Desc = dss.Tables[0].Rows[j][3].ToString().Trim();
                                }
                                else
                                {
                                    Desc = "Not Available";
                                }

                                ProdIds = " " + dss.Tables[0].Rows[j][6].ToString().Trim();
                                Fill_Locations(ProdIds.Trim());
                            }
                            else
                            {
                                if (dss.Tables[0].Rows[j][2].ToString() != "")
                                {
                                    if (MCN != dss.Tables[0].Rows[j][2].ToString().Trim())
                                    {
                                        MCN = MCN + "" + " | " + "" + (dss.Tables[0].Rows[j][2].ToString()).Trim();
                                    }
                                }

                                if (PONum != dss.Tables[0].Rows[j][0].ToString().Trim())
                                {
                                    PONum = PONum + " | " + (dss.Tables[0].Rows[j][0].ToString()).Trim();
                                }
                                if (dss.Tables[0].Rows[j][3].ToString() != "")
                                {
                                    if (Desc != dss.Tables[0].Rows[j][3].ToString().Trim())
                                    {
                                        Desc = Desc + "" + " | " + "" + dss.Tables[0].Rows[j][3].ToString();
                                    }
                                }

                                ProdIds = ProdIds + "" + " | " + "" + (dss.Tables[0].Rows[j][6].ToString()).Trim();
                            }
                        }


                    }
                }
                PONum = PONum + " ";
                ProdIds = ProdIds + " ";
                char[] chh = { '|' };
                string[] ProdIdss = { "0" };
                ProdIdss = ProdIds.Split(chh);
                MCN = MCN + " ";
                Desc = Desc + " ";
                //lblSites.Text = GetUniqueVals(PONum);
                //lblRequestNo.Text = GetUniqueVals(MCN);
                //lblProdCode.Text = GetUniqueVals(ProdIds);
                //lblDesc.Text = GetUniqueVals(Desc);

                TextBox1.Text = ProdIdss[0].ToString();
                // btn.Value = TextBox1.Text;
                // int ans = ReceivableClient.GetProdQty(TextBox1.Text.Trim(), selPos);
                //lblPoQty.Text = ans.ToString();
                // txtReceiQty.Text = ans.ToString();
                // lblBarcode.Text = TextBox1.Text;
                Fill_Locations(TextBox1.Text.Trim());
                // ddlLocation.SelectedIndex = 1;
                //HdnLoc.Value = ddlLocation.SelectedValue;
                //HdnLocTxt.Value = ddlLocation.SelectedItem.Text;
                FillPO_List(SelectedPOs.Value, TextBox1.Text.Trim());
                //  ddlPoList.SelectedIndex = 1;
                // HdnPoId.Value = ddlPoList.SelectedValue;
            }
            catch
            {
            }
            finally
            {
                // ReceivableClient.Close();
            }
        }
        //public void Fill_ProductLst(string Prodstr)
        //{
        //    string[] ProdstrArr;
        //    char[] sp = { '|' };
        //    ProdstrArr = Prodstr.Split(sp);
        //    for (int i = 0; i < ProdstrArr.Length + 1; i++)
        //    {
        //        if (i == 0)
        //        {
        //            DDLProd.Items.Add("Select Product");
        //        }
        //        else{
        //             DDLProd.Items.Add(ProdstrArr[i-1].ToString());
        //        }
        //            //DDLProd.Items.Insert(i, ProdstrArr[i].ToString());
        //        DDLProd.SelectedIndex = 1;

        //    }

        //}
        public string GetUniqueVals(string OldStr)
        {
            string[] SOstr;
            char[] sp = { '|' };
            string NewSO = "";
            SOstr = OldStr.Split(sp);
            IEnumerable<String> disctinctName = SOstr.Distinct();
            int k = 0;
            foreach (String theString in disctinctName)
            {
                if (k == 0)
                {
                    NewSO = theString;
                    k = 1;
                }
                else
                {
                    NewSO = NewSO + "" + " | " + "" + theString;
                }
            }

            return NewSO;
        }
        #endregion

        # region GetProdQty

        [System.Web.Services.WebMethod]
        public static int GetProdQty(string ProdCode, string SelPos)
        {
            int ans = 200;
            //try
            //{
            //    iReceivableClient ReceivableClient = new iReceivableClient();

            //    ans = ReceivableClient.GetProdQty(ProdCode.Trim(), SelPos);

            //}
            //catch
            //{
            //}
            return ans;
        }

        [WebMethod]
        public static string ValidateProduct(string Prod, string ProdLst)
        {

            string[] ProdArr = { "0" };
            char[] ch = { '|' };
            int ctr = 0;
            ProdArr = ProdLst.Split(ch);
            for (int i = 0; i < ProdArr.Length; i++)
            {
                if (Prod.Trim() == ProdArr[i].Trim())
                {
                    ctr = 2;
                    break;
                }

            }
            return ctr.ToString();

        }
        #endregion

        # region Removed code for performance
        //protected void TextBox1_TextChanged(object sender, EventArgs e)
        //{
        //    long Prodid = 0;
        //    string qry = "select ID from mProduct where ProductCode='" + TextBox1.Text + "'";
        //    SqlCommand cmd = new SqlCommand(qry, conn);
        //    cmd.CommandType = CommandType.Text;
        //    conn.Open();
        //    dr = cmd.ExecuteReader();
        //    if (dr.Read())
        //    {
        //        Prodid = long.Parse(dr[0].ToString());
        //    }
        //    dr.Close();
        //    da = new SqlDataAdapter("select SUM(QtyOrder)sumation from tPurchaseOrderDetail where ProductID='" + Prodid + "'", conn);
        //    da.Fill(ds);
        //    dt = ds.Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        lblPoQty.Text = dt.Rows[0]["sumation"].ToString();
        //    }

        //    lblBarcode.Text = TextBox1.Text.ToString();

        //}
        #endregion

        #region AddDataToGrid

        [WebMethod]
        public static void WMUpdatePutInList(Object obj)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)obj;
                CustomProfile profile = CustomProfile.GetProfile();

                WMS_SP_PutInList_Result ptinLst = new WMS_SP_PutInList_Result();
                ptinLst.Sequence = Convert.ToInt64(d["Sequence"]);
                ptinLst.LocQty = Convert.ToDecimal(d["LocQty"]);

                Inbound.UpdatePutInLstQtyofSelectedRow(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ptinLst,profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PutIn.aspx", "WMUpdatePutInList"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdatePutInListLoc(Object obj)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)obj;
                CustomProfile profile = CustomProfile.GetProfile();

                WMS_SP_PutInList_Result ptinLst = new WMS_SP_PutInList_Result();
                ptinLst.Sequence = Convert.ToInt64(d["Sequence"]);
                ptinLst.LocationID = Convert.ToInt64(d["LocationID"]);
                ptinLst.Code = d["Code"].ToString();
                ptinLst.SortCode = Convert.ToInt64(d["SortCode"]);
                ptinLst.Capacity = Convert.ToDecimal(d["Capacity"]);
                ptinLst.AvailableBalance = Convert.ToDecimal(d["AvailableBalance"]);

                Inbound.UpdatePutInLstLocofSelectedRow(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ptinLst, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PutIn.aspx", "WMUpdatePutInList"); }
            finally { Inbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdatePutInListPack(Object obj)
        {
             iInboundClient Inbound = new iInboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)obj;
                CustomProfile profile = CustomProfile.GetProfile();

                string seq = d["Sequence"].ToString();
                string[] chk = seq.Split(',');
                int cnt = chk.Count();
                for (int i = 0; i <= cnt - 1; i++)
                {
                    WMS_SP_PutInList_Result ptinLst = new WMS_SP_PutInList_Result();
                    ptinLst.Sequence = Convert.ToInt64(chk[i]);
                    ptinLst.Pack = d["Pack"].ToString();

                    Inbound.UpdatePutInLstPackofSelectedRow(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), ptinLst, profile.DBConnection._constr);
                }

                
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PutIn.aspx", "WMUpdatePutInListPack"); }
            finally { Inbound.Close(); }
        }

        protected void GridReceipt_OnRebind(object sender, EventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                
                GridReceipt.DataSource = Inbound.GetExistingTempDataBySessionIDObjectNamePI(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr); //.GetPutInList(Session["QCID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                GridReceipt.DataBind();   
            }
            catch { }
            finally { Inbound.Close(); }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            iInboundClient Inbound = new iInboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                TextBox1.Text = "";
                txtReceiQty.Text = "";
                txtLocationCapacity.Text = "";
                txtAvlBlc.Text = "";
                txtLocation.Text = "";

                GridReceipt.DataSource = Inbound.GetExistingTempDataBySessionIDObjectNamePI(Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr); //.GetPutInList(Session["QCID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                GridReceipt.DataBind();   
            }
            catch { }
            finally { Inbound.Close(); }
        }
        #endregion

        # region ClearGridData
        protected void BtnClearGrid_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            txtReceiQty.Text = "";
            txtLocation.Text = "";
            txtLocationCapacity.Text = "";
            txtAvlBlc.Text = "";
            hdnSelectedPutInRec.Value = "0";
        }
        # endregion

        # region SaveGridData
        [System.Web.Services.WebMethod]
        public static string WMSaveGridData(string s)
        {
            //iReceivableClient ReceivableClient = new iReceivableClient();

            //try
            //{
            //    int a = ReceivableClient.SaveTransData();
            //}
            //catch
            //{
            //}

            //finally
            //{

            //    ReceivableClient.Close();
            //}
            return "Success";
        }
        # endregion

        #region FillLocationList
        public void Fill_Locations(string ProdCode)
        {

            //iReceivableClient ReceivableClient = new iReceivableClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
            //    dt = ds.Tables[0];
            //    // ddlLocation.Items.Add("Select Location");
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlLocation.DataSource = ds;
            //        ddlLocation.DataTextField = "LocationCode";
            //        ddlLocation.DataValueField = "ID";
            //        ddlLocation.DataBind();
            //    }
            //    ddlLocation.Items.Insert(0, "Select Location");
            //}
            //catch (Exception ex) { }
            //finally { ReceivableClient.Close(); }
        }

        [WebMethod]
        public static List<Locations> FillLocations(string ProdCode)
        {
            List<Locations> LocList = new List<Locations>();
            //iReceivableClient ReceivableClient = new iReceivableClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();

            //try
            //{
            //    ds = ReceivableClient.GetProdLocations(ProdCode.Trim());
            //    dt = ds.Tables[0];


            //    Locations Loc = new Locations();
            //    Loc.Name = "Select Location";
            //    Loc.Id = "0";
            //    LocList.Add(Loc);
            //    Loc = new Locations();

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            Loc.Name = dt.Rows[i][1].ToString();
            //            Loc.Id = dt.Rows[i][0].ToString();
            //            LocList.Add(Loc);
            //            Loc = new Locations();

            //        }

            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ReceivableClient.Close();
            //}
            return LocList;
        }
        public class Locations
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }
        #endregion

        # region FillPOList
        public void FillPO_List(string SelectedPO, string ProdCode)
        {

            //iReceivableClient ReceivableClient = new iReceivableClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ReceivableClient.GetSortedPOList(ProdCode.Trim(), SelectedPO);
            //    dt = ds.Tables[0];
            //    // ddlLocation.Items.Add("Select Location");
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlPoList.DataSource = ds;
            //        ddlPoList.DataTextField = "PoNumber";
            //        ddlPoList.DataValueField = "ID";
            //        ddlPoList.DataBind();
            //    }
            //    ddlPoList.Items.Insert(0, "Select Purchase order");
            //}
            //catch (Exception ex) { }
            //finally { ReceivableClient.Close(); }
        }
        [WebMethod]
        public static List<POrders> FillPOList(string SelectedPO, string ProdCode)
        {
            List<POrders> POList = new List<POrders>();
            //iReceivableClient ReceivableClient = new iReceivableClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();

            //try
            //{
            //    ds = ReceivableClient.GetSortedPOList(ProdCode.Trim(), SelectedPO);
            //    dt = ds.Tables[0];


            //    POrders PO = new POrders();
            //    PO.Name = "Select Purchase order";
            //    PO.Id = "0";
            //    POList.Add(PO);
            //    PO = new POrders();

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            PO.Name = dt.Rows[i][1].ToString();
            //            PO.Id = dt.Rows[i][0].ToString();
            //            POList.Add(PO);
            //            PO = new POrders();

            //        }

            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ReceivableClient.Close();
            //}
            return POList;
        }


        public class POrders
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            private string _id;
            public string Id
            {
                get { return _id; }
                set { _id = value; }
            }
        }
        # endregion


        # region CreateSequence
        protected void BtnSequence_Click(object sender, EventArgs e)
        {
            //long result = 0;
            int RSLT = 0; long PIID = 0;
            string confirmValue = Request.Form["confirm_value"];
            iInboundClient Inbound = new iInboundClient();

            try
            {
                if (confirmValue == "Yes")
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    tPutInHead pth = new tPutInHead();
                    if (Session["QCID"] != null)
                    {
                        int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "QC", profile.DBConnection._constr);
                        if (chkJObCart >= 1)
                        {
                            DataSet dsJCN = new DataSet();
                            dsJCN = Inbound.CheckSelectedPOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["QCID"].ToString()), "QC", profile.DBConnection._constr);
                            if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                            {
                                string grpQCID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                                string[] TotQC = grpQCID.Split(',');
                                int QCCnt = TotQC.Count();
                                for (int q = 0; q <= QCCnt - 1; q++)
                                {
                                    WMS_VW_GetQCDetails qcd = new WMS_VW_GetQCDetails();
                                    qcd = Inbound.GetQCDetailsByQCID(long.Parse(TotQC[q].ToString()), profile.DBConnection._constr);
                                    string putInObj = qcd.ObjectName.ToString();

                                    pth.CreatedBy = profile.Personal.UserID;
                                    pth.CreationDate = DateTime.Now;
                                    pth.ObjectName = putInObj;
                                    pth.OID = long.Parse(TotQC[q].ToString());
                                    pth.PutInDate = DateTime.Now;
                                    if (putInObj == "PurchaseOrder") { pth.Status = 35; } else if (putInObj == "SalesReturn") { pth.Status = 54; }
                                    pth.Company = profile.Personal.CompanyID;
                                    pth.PutInBy = profile.Personal.UserID;

                                    PIID = Inbound.SavetPutInHead(pth, profile.DBConnection._constr); hdnPutInNo.Value = PIID.ToString();

                                    if (PIID > 0)
                                    {
                                        RSLT = Inbound.FinalSavePutInDetail(long.Parse(TotQC[q].ToString()), Session.SessionID, ObjectName, PIID, profile.Personal.UserID.ToString(), Convert.ToInt16(pth.Status), putInObj, profile.DBConnection._constr);
                                        if (RSLT == 1)
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Received Successfully!','info','../WMS/Inbound.aspx')", true);
                                        }
                                        else
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/Inbound.aspx')", true);
                                        }
                                    }
                                }
                                Inbound.ClearTempDataFromDBPutIn(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                            }
                        }
                        else
                        {
                            WMS_VW_GetQCDetails qcd = new WMS_VW_GetQCDetails();
                            qcd = Inbound.GetQCDetailsByQCID(long.Parse(Session["QCID"].ToString()), profile.DBConnection._constr);
                            string putInObj = qcd.ObjectName.ToString();

                            pth.CreatedBy = profile.Personal.UserID;
                            pth.CreationDate = DateTime.Now;
                            pth.ObjectName = putInObj;
                            pth.OID = long.Parse(Session["QCID"].ToString());
                            pth.PutInDate = DateTime.Now;
                            if (putInObj == "PurchaseOrder") { pth.Status = 35; } else if (putInObj == "SalesReturn") { pth.Status = 54; }
                            pth.Company = profile.Personal.CompanyID;
                            pth.PutInBy = profile.Personal.UserID;

                            PIID = Inbound.SavetPutInHead(pth, profile.DBConnection._constr); hdnPutInNo.Value = PIID.ToString();

                            if (PIID > 0)
                            {
                                RSLT = Inbound.FinalSavePutInDetail(long.Parse(Session["QCID"].ToString()), Session.SessionID, ObjectName, PIID, profile.Personal.UserID.ToString(), Convert.ToInt16(pth.Status), putInObj, profile.DBConnection._constr);
                                if (RSLT == 1)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Received Successfully!','info','../WMS/Inbound.aspx')", true);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/Inbound.aspx')", true);
                                }
                            }
                            Inbound.ClearTempDataFromDBPutIn(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else if (Session["TRID"] != null)
                    {
                        WMS_VW_GetQCDetails qcd = new WMS_VW_GetQCDetails();
                        //qcd = Inbound.GetQCDetailsByQCID(long.Parse(Session["TRID"].ToString()), profile.DBConnection._constr);
                        //string putInObj = qcd.ObjectName.ToString();

                        pth.CreatedBy = profile.Personal.UserID;
                        pth.CreationDate = DateTime.Now;
                        pth.ObjectName = "Transfer";
                        pth.OID = long.Parse(Session["TRID"].ToString());
                        pth.PutInDate = DateTime.Now;
                        pth.Status = 62;
                        pth.Company = profile.Personal.CompanyID;
                        pth.PutInBy = profile.Personal.UserID;

                        PIID = Inbound.SavetPutInHead(pth, profile.DBConnection._constr); hdnPutInNo.Value = PIID.ToString();

                        if (PIID > 0)
                        {
                            RSLT = Inbound.FinalSavePutInDetail(long.Parse(Session["TRID"].ToString()), Session.SessionID, ObjectName, PIID, profile.Personal.UserID.ToString(), Convert.ToInt16(pth.Status), "Transfer", profile.DBConnection._constr);
                            if (RSLT == 1)
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Received Successfully!','info','../WMS/Transfer.aspx')", true);
                            }
                            else
                            {
                                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/Transfer.aspx')", true);
                            }
                        }
                        Inbound.ClearTempDataFromDBPutIn(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    }
                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('One or More Product Dones Not Assigned The Location. Please Assign the Location For Product... ','erroe','#')", true);
                }
            
            }
            catch (Exception ex)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('" + ex.Message.ToString() + "','Error','#')", true);
            }
            finally
            {
                Inbound.Close();
            }
        }


        //protected void GridReceipt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridReceipt.p = e.NewPageIndex;
        //    BindGrid();
        //}
        # endregion

        # region SortCode
        public void Fill_SortCode()
        {

            //iReceivableClient ReceivableClient = new iReceivableClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ReceivableClient.FillSortCode();
            //    dt = ds.Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        DDLSortcode.DataSource = ds;
            //        DDLSortcode.DataTextField = "SortCode";
            //        DDLSortcode.DataValueField = "ID";
            //        DDLSortcode.DataBind();
            //    }
            //    DDLSortcode.Items.Insert(0, "Select SortCode");
            //}
            //catch (Exception ex) { }
            //finally { ReceivableClient.Close(); }
        }

        # endregion


        #endregion

        # region Report Coading

        [WebMethod]
        //public static int WMShowReport(string PurchaseOrderNo, string MCNNO, string PoNo, string PrdNo)
        public static int WMShowReport(string PutInNo)
        {
            int result = 0;
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            iInboundClient Inbound = new iInboundClient();

            ds = Inbound.GetSavedPutInListbyPutInID(PutInNo, profile.DBConnection._constr);
            ds.Tables[0].TableName = "DataSet1";
            HttpContext.Current.Session["ReportDS"] = ds;
            HttpContext.Current.Session["SelObject"] = ds.Tables[0].Rows[0]["BatchNo"].ToString(); ;
            HttpContext.Current.Session["BatchNo"] = ds.Tables[0].Rows[0]["BatchNo"].ToString();
            result = Convert.ToInt16(ds.Tables[0].Rows.Count);
            return result;
        }

        #endregion

    }
}