using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using BrilliantWMS.WMSOutbound;
using BrilliantWMS.WMSInbound;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using System.Collections;

namespace BrilliantWMS.Warehouse
{
    public partial class PickUp : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();

        string Sortcode = "";
        string SelectedPO, MCN, PONum, ProdIds, Desc;
        string[] POs = { "0" };
        char[] ch = { ',' };

        static string ObjectName = "PickUp";

        protected void Page_Load(object sender, EventArgs e)
        {
           UCFormHeader1.FormHeaderText = "Shipping";

            if (!IsPostBack)
            {
                BindGrid();

                Toolbar1.SetUserRights("MaterialRequest", "EntryForm", "");
                Toolbar1.SetSaveRight(false, "Not Allowed");
                Toolbar1.SetAddNewRight(false, "Not Allowed");
                Toolbar1.SetImportRight(false, "Not Allowed");
                Toolbar1.SetClearRight(false, "Not Allowed");
            } 
        }

        public void BindGrid()
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                CustomProfile profile = CustomProfile.GetProfile();
                if (Session["SOID"] != null)
                {
                    hdnPickUpNo.Value = Session["SOID"].ToString();
                    GridReceipt.DataSource = Outbound.GetPickUpList(Session["SOID"].ToString(),"", Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    GridReceipt.DataBind();
                }
                else if (Session["PKUPID"] != null)
                {
                    long pkupId=long.Parse(Session["PKUPID"].ToString());
                    long soID = Outbound.GetSOIDfromPkUpID(pkupId, profile.DBConnection._constr); hdnPickUpNo.Value = soID.ToString();

                    GridReceipt.DataSource = Outbound.GetPickUpList(soID.ToString(), "",Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    GridReceipt.DataBind();
                }
                if (Session["SOstate"] != null)
                {
                    if (Session["SOstate"].ToString() == "View")
                    {
                        BtnSequence.Visible = false;
                    }
                }
                else if (Session["PKUPstate"] != null)
                {
                    if (Session["PKUPstate"].ToString() == "View")
                    {
                        BtnSequence.Visible = false;
                    }
                }
                else { BtnSequence.Visible = true; }

                if (Session["TRID"] != null)
                {
                    GridReceipt.DataSource = Outbound.GetPickUpList("",Session["TRID"].ToString(), Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                    GridReceipt.DataBind();
                    if (Session["TRstate"].ToString() == "View")
                    {
                        BtnSequence.Visible = false;
                    }
                }
            }
            catch { }
            finally { Outbound.Close(); }            
        }

        protected void GridReceipt_Select(object sender, EventArgs e)
        {
            try
            {
                Hashtable selectedrec = (Hashtable)GridReceipt.SelectedRecords[0];
                hdnSelectedPickUpRec.Value = selectedrec["Sequence"].ToString();
                TextBox1.Text = selectedrec["ProductCode"].ToString();
                txtReceiQty.Text = selectedrec["LocQty"].ToString();
                txtLocation.Text = selectedrec["Code"].ToString();
                txtLocationCapacity.Text = selectedrec["Capacity"].ToString();
                txtAvlBlc.Text = selectedrec["AvailableBalance"].ToString();

                Button1.Visible = true;
                BtnClearGrid.Visible = true;
                imgSearch.Visible = true;
            }
            catch (System.Exception ex)
            {
                Login.Profile.ErrorHandling(ex, this, "PickUp.aspx", "gvPrdPutIn_Select");
            }
            finally
            {
            }
        }

        protected void BtnSequence_Click(object sender, EventArgs e)
        {
            int RSLT = 0; long PkUpID = 0;
            string confirmValue = Request.Form["confirm_value"];
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            BrilliantWMS.WMSInbound.iInboundClient Inbound = new WMSInbound.iInboundClient();
            try
            {
                if (confirmValue == "Yes")
                {
                    CustomProfile profile = CustomProfile.GetProfile();
                    BrilliantWMS.WMSOutbound.tPickUpHead pkh = new WMSOutbound.tPickUpHead();
                    //BrilliantWMS.WMSOutbound.tTransferHead trh = new WMSOutbound.tTransferHead();
                    if (Session["SOID"] != null)
                    {
                        int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesOrder", profile.DBConnection._constr);
                        if (chkJObCart >= 1)
                        {
                            DataSet dsJCN = new DataSet();
                            dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["SOID"].ToString()), "SalesOrder", profile.DBConnection._constr);
                            if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                            {
                                string grpSOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                                string[] TotOD = grpSOID.Split(',');
                                int ODCnt = TotOD.Count();
                                for (int q = 0; q <= ODCnt - 1; q++)
                                {
                                    pkh.CreatedBy = profile.Personal.UserID;
                                    pkh.CreationDate = DateTime.Now;
                                    pkh.ObjectName = "SalesOrder";
                                    pkh.OID = long.Parse(TotOD[q].ToString());
                                    pkh.PickUpDate = DateTime.Now;
                                    pkh.Status = 38;
                                    pkh.CompanyID = profile.Personal.CompanyID;
                                    pkh.PickUpBy = profile.Personal.UserID;

                                    PkUpID = Outbound.SavetPickUpHead(pkh, profile.DBConnection._constr);

                                    if (PkUpID > 0)
                                    {
                                        RSLT = Outbound.FinalSavePickUpDetail(long.Parse(TotOD[q].ToString()), Session.SessionID, ObjectName, PkUpID, profile.Personal.UserID.ToString(), Convert.ToInt16(pkh.Status), profile.DBConnection._constr);
                                        if (RSLT == 1)
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Picked Up Successfully!','info','../WMS/PickUpList.aspx')", true);
                                        }
                                        else
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/PickUpList.aspx')", true);
                                        }
                                    }
                                }
                                Outbound.ClearTempDataFromDBPickUp(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                            }
                        }
                        else
                        {
                            pkh.CreatedBy = profile.Personal.UserID;
                            pkh.CreationDate = DateTime.Now;
                            pkh.ObjectName = "SalesOrder";
                            pkh.OID = long.Parse(Session["SOID"].ToString());
                            pkh.PickUpDate = DateTime.Now;
                            pkh.Status = 38;
                            pkh.CompanyID = profile.Personal.CompanyID;
                            pkh.PickUpBy = profile.Personal.UserID;

                            PkUpID = Outbound.SavetPickUpHead(pkh, profile.DBConnection._constr);
                            if (PkUpID > 0)
                            {
                                RSLT = Outbound.FinalSavePickUpDetail(long.Parse(Session["SOID"].ToString()), Session.SessionID, ObjectName, PkUpID, profile.Personal.UserID.ToString(), Convert.ToInt16(pkh.Status), profile.DBConnection._constr);
                                if (RSLT == 1)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Picked Up Successfully!','info','../WMS/PickUpList.aspx')", true);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/PickUpList.aspx')", true);
                                }
                            }
                            Outbound.ClearTempDataFromDBPickUp(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
                    }
                    else if (Session["TRID"] != null)
                    {
                        int chkJObCart = Inbound.CheckJobCard(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                        if (chkJObCart >= 1)
                        {
                            DataSet dsJCN = new DataSet();
                            dsJCN = Outbound.CheckSelectedSOJobCardNo(Convert.ToInt64(HttpContext.Current.Session["TRID"].ToString()), "Transfer", profile.DBConnection._constr);
                            if (dsJCN != null && dsJCN.Tables[0].Rows.Count > 0)
                            {
                                string grpSOID = dsJCN.Tables[0].Rows[0]["OrderNo"].ToString();
                                string[] TotOD = grpSOID.Split(',');
                                int ODCnt = TotOD.Count();
                                for (int q = 0; q <= ODCnt - 1; q++)
                                {
                                    pkh.CreatedBy = profile.Personal.UserID;
                                    pkh.CreationDate = DateTime.Now;
                                    pkh.ObjectName = "Transfer";
                                    pkh.OID = long.Parse(TotOD[q].ToString());
                                    pkh.PickUpDate = DateTime.Now;
                                    pkh.Status = 57;
                                    pkh.CompanyID = profile.Personal.CompanyID;
                                    pkh.PickUpBy = profile.Personal.UserID;

                                    PkUpID = Outbound.SavetPickUpHead(pkh, profile.DBConnection._constr);

                                    if (PkUpID > 0)
                                    {
                                        RSLT = Outbound.FinalSavePickUpDetail(long.Parse(TotOD[q].ToString()), Session.SessionID, ObjectName, PkUpID, profile.Personal.UserID.ToString(), Convert.ToInt16(pkh.Status), profile.DBConnection._constr);
                                        if (RSLT == 1)
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Picked Up Successfully!','info','../WMS/Transfer.aspx')", true);
                                        }
                                        else
                                        {
                                            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/Transfer.aspx')", true);
                                        }
                                    }
                                }
                                Outbound.ClearTempDataFromDBPickUp(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                            }
                        }
                        else
                        {
                            pkh.CreatedBy = profile.Personal.UserID;
                            pkh.CreationDate = DateTime.Now;
                            pkh.ObjectName = "Transfer";
                            pkh.OID = long.Parse(Session["TRID"].ToString());
                            pkh.PickUpDate = DateTime.Now;
                            pkh.Status = 57;
                            pkh.CompanyID = profile.Personal.CompanyID;
                            pkh.PickUpBy = profile.Personal.UserID;

                            PkUpID = Outbound.SavetPickUpHead(pkh, profile.DBConnection._constr);
                            if (PkUpID > 0)
                            {
                                RSLT = Outbound.FinalSavePickUpDetail(long.Parse(Session["TRID"].ToString()), Session.SessionID, ObjectName, PkUpID, profile.Personal.UserID.ToString(), Convert.ToInt16(pkh.Status), profile.DBConnection._constr);
                                if (RSLT == 1)
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Products Picked Up Successfully!','info','../WMS/Transfer.aspx')", true);
                                }
                                else
                                {
                                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "showAlert('Some Error Occured!','info','../WMS/Transfer.aspx')", true);
                                }
                            }
                            Outbound.ClearTempDataFromDBPickUp(HttpContext.Current.Session.SessionID, profile.Personal.UserID.ToString(), ObjectName, profile.DBConnection._constr);
                        }
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
                Outbound.Close();
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


        # region ShowSODetails
        public int ValidateStatus(string selPos)
        {
            int result = 0;
            //iSalesOrderClient Sales = new iSalesOrderClient();
            
            //try
            //{
            //    result = Sales.ValidateGreenSOStatus(selPos);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    Sales.Close();
            //}
            return result;
        }
        public void ShowSODetails(string selSos)
        {
           // iShippingClient ShippingClient = new iShippingClient();
            //try
            //{
            //    SOs = selSos.Split(ch);
            //    for (int i = 0; i < SOs.Length; i++)
            //    {

            //        DataSet dss = new DataSet();
            //        //dss = ShippingClient.GetSODetails(long.Parse(SOs[i].ToString()));
            //        if (dss.Tables[0].Rows.Count > 0)
            //        {
            //            for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
            //            {
            //                if (j == 0 && i == 0)
            //                {
            //                    if (dss.Tables[0].Rows[j][2].ToString() != "")
            //                    {
            //                        MCN = dss.Tables[0].Rows[j][2].ToString().Trim();
            //                    }
            //                    else { MCN = "Not Assigned"; }
            //                    SONum = dss.Tables[0].Rows[j][0].ToString().Trim();
            //                    if (dss.Tables[0].Rows[j][3].ToString() != "")
            //                    {
            //                        Desc = dss.Tables[0].Rows[j][3].ToString().Trim();
            //                    }
            //                    else
            //                    {
            //                        Desc = "Not Available";
            //                    }

            //                    ProdIds = dss.Tables[0].Rows[j][6].ToString().Trim();
            //                    Fill_Locations(ProdIds);
            //                }
            //                else
            //                {
            //                    if (dss.Tables[0].Rows[j][2].ToString() != "")
            //                    {
            //                        if (MCN != dss.Tables[0].Rows[j][2].ToString().Trim())
            //                        {
            //                            MCN = MCN + "" + " | " + "" + (dss.Tables[0].Rows[j][2].ToString()).Trim();
            //                        }
            //                    }

            //                    if (SONum != dss.Tables[0].Rows[j][0].ToString().Trim())
            //                    {
            //                        SONum = SONum + " | " + (dss.Tables[0].Rows[j][0].ToString()).Trim();
            //                    }
            //                    if (dss.Tables[0].Rows[j][3].ToString() != "")
            //                    {
            //                        if (Desc != dss.Tables[0].Rows[j][3].ToString().Trim())
            //                        {
            //                            Desc = Desc + "" + " | " + "" + dss.Tables[0].Rows[j][3].ToString();
            //                        }
            //                    }

            //                    ProdIds = ProdIds + "" + " | " + "" + (dss.Tables[0].Rows[j][6].ToString()).Trim();
            //                }
            //            }


            //        }
            //    }
            //    SONum = SONum + " ";
            //    ProdIds = ProdIds + " ";
            //    MCN = MCN + " ";
            //    Desc = Desc + " ";
            //    //lblSites.Text = GetUniqueVals(SONum);
            //    //lblRequestNo.Text = GetUniqueVals(MCN);
            //    //lblProdCode.Text = GetUniqueVals(ProdIds);
            //    //lblDesc.Text = GetUniqueVals(Desc);
            //    //Fill_ProductLst(lblProdCode.Text);

            //    TextBox1.Text = DDLProd.SelectedItem.Text;
            //   // int ans = ShippingClient.GetProdQty(DDLProd.SelectedItem.Text, selSos);
            //    //lblSoQty.Text = ans.ToString();
            //    lblBarcode.Text = DDLProd.SelectedItem.Text;
            //    Fill_Locations(DDLProd.SelectedItem.Text);
            //    ddlLocation.SelectedIndex = 1;
            //    HdnLoc.Value = ddlLocation.SelectedValue;
            //    HdnLocTxt.Value = ddlLocation.SelectedItem.Text;
            //    FillSO_List(SelectedSOs.Value, DDLProd.SelectedItem.Text);
            //    ddlSoList.SelectedIndex = 1;
            //    HdnSoId.Value = ddlSoList.SelectedValue;
            //}
            //catch
            //{
            //}
            //finally
            //{
            //   // ShippingClient.Close();
            //}
        }
        public void Fill_ProductLst(string Prodstr)
        {
            string[] ProdstrArr;
            char[] sp = { '|' };
            ProdstrArr = Prodstr.Split(sp);
            for (int i = 0; i < ProdstrArr.Length + 1; i++)
            {
                
            }

        }
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
        public static int GetProdQty(string ProdCode, string Sos)
        {
            int ans = 200;
            //iShippingClient ShippingClient = new iShippingClient();
          
            //try
            //{
            //    ans = ShippingClient.GetProdQty(ProdCode.Trim(), Sos);
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ShippingClient.Close();
            //}

            return ans;
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
        //    da = new SqlDataAdapter("select SUM(QtyOrder)sumation from tSalesOrderDetail where ProductID='" + Prodid + "'", conn);
        //    da.Fill(ds);
        //    dt = ds.Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        lblSoQty.Text = dt.Rows[0]["sumation"].ToString();
        //    }

        //    lblBarcode.Text = TextBox1.Text.ToString();

        //}
        #endregion

        #region AddDataToGrid
        protected void Button1_Click(object sender, EventArgs e)
        {
            //CustomProfile profile = CustomProfile.GetProfile();
            //iShippingClient ShippingClient = new iShippingClient();
            //try
            //{
            //    string SortCode = "";
            //    DataSet ds = (DataSet)Session["data"];
            //    DataRow dr = ds.Tables[0].NewRow();
            //    for (int i = 0; i < DDLSortcode.Items.Count; i++)
            //    {
            //        if (DDLSortcode.Items[i].Value == HdnLoc.Value)
            //        {
            //            SortCode = DDLSortcode.Items[i].Text;
            //            break;
            //        }
            //    }
            //    dr[0] = TextBox1.Text.Trim();
            //    dr[1] = txtReceiQty.Text.Trim();
            //    dr[2] = HdnLocTxt.Value;
            //    dr[3] = SortCode;
            //    dr[4] = TextBox1.Text.Trim();
            //    ds.Tables[0].Rows.Add(dr);
            //    GridReceipt.DataSource = ds;
            //    GridReceipt.DataBind();


            //    //save Temp Data
            //    int result = 0;
            //    result = ShippingClient.SaveTempData(HdnSoId.Value, TextBox1.Text.Trim(), HdnLoc.Value, decimal.Parse(txtReceiQty.Text.Trim()), profile.Personal.UserID.ToString());
            //    TextBox1.Text = "";
            //    txtReceiQty.Text = "";
            //    lblSoQty.Text = "";
            //    lblBarcode.Text = "";

            //    HdnLoc.Value = "";
            //    HdnLocTxt.Value = "";
            //    HdnSoId.Value = "";
            //    ddlLocation.SelectedIndex = 0;
            //    ddlSoList.SelectedIndex = 0;
            //    DDLProd.SelectedIndex = 0;
            //    BtnSequence.Enabled = true;
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ShippingClient.Close();
            //    profile = null;
            //}
        }
        #endregion

        # region ClearGridData
        protected void BtnClearGrid_Click(object sender, EventArgs e)
        {
            Session["data"] = "";
            CreateGrid();
            GridReceipt.DataSource = null;
            GridReceipt.DataBind();
        }
        # endregion

        # region SaveGridData
        [System.Web.Services.WebMethod]
        public static string WMSaveGridData(string s)
        {
           // iShippingClient ShippingClient = new iShippingClient();
            //iCycleCountClient Cycle = new iCycleCountClient();
            try
            {
                // int a = ShippingClient.SaveTransData();
            }
            catch
            {
            }

            finally
            {

                //ShippingClient.Close();
            }
            return "Success";
        }
        # endregion

        #region FillLocationList
        public void Fill_Locations(string ProdCode)
        {

            //iShippingClient ShippingClient = new iShippingClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ShippingClient.GetProdLocations(ProdCode);
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
            //finally { ShippingClient.Close(); }
        }

        [WebMethod]
        public static List<Locations> FillLocations(string ProdCode)
        {
            List<Locations> LocList = new List<Locations>();
            //iShippingClient ShippingClient = new iShippingClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
           
            //try
            //{
            //    ds = ShippingClient.GetProdLocations(ProdCode.Trim());
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
            //    ShippingClient.Close();
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

        # region FillSOList


        public void FillSO_List(string SelectedSO, string ProdCode)
        {

            //iShippingClient ShippingClient = new iShippingClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ShippingClient.GetSortedSOList(ProdCode, SelectedSO);
            //    dt = ds.Tables[0];
            //    // ddlLocation.Items.Add("Select Location");
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlSoList.DataSource = ds;
            //        ddlSoList.DataTextField = "SalesOrderNo";
            //        ddlSoList.DataValueField = "ID";
            //        ddlSoList.DataBind();
            //    }
            //    ddlSoList.Items.Insert(0, "Select Sales order");
            //}
            //catch (Exception ex) { }
            //finally { ShippingClient.Close(); }
        }

        [WebMethod]
        public static List<SOrders> FillSOList(string SelectedSO, string ProdCode)
        {
            List<SOrders> SOList = new List<SOrders>();
            //iShippingClient ShippingClient = new iShippingClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
           
            //try
            //{
            //    ds = ShippingClient.GetSortedSOList(ProdCode, SelectedSO);
            //    dt = ds.Tables[0];


            //    SOrders SO = new SOrders();
            //    SO.Name = "Select Sales order";
            //    SO.Id = "0";
            //    SOList.Add(SO);
            //    SO = new SOrders();

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            SO.Name = dt.Rows[i][1].ToString();
            //            SO.Id = dt.Rows[i][0].ToString();
            //            SOList.Add(SO);
            //            SO = new SOrders();

            //        }

            //    }
            //}
            //catch
            //{
            //}
            //finally
            //{
            //    ShippingClient.Close();
            //}
            return SOList;
        }


        public class SOrders
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

        # region SortCode
        public void Fill_SortCode()
        {

            //iShippingClient ShippingClient = new iShippingClient();
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //try
            //{
            //    ds = ShippingClient.FillSortCode();
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
            //finally { ShippingClient.Close(); }
        }

        # endregion

        # region CreateSequence
       
       
        //protected void GridReceipt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridReceipt.p = e.NewPageIndex;
        //    BindGrid();
        //}
        # endregion
        #endregion


        [WebMethod]
        public static void WMUpdatePickUpList(Object obj)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)obj;
                CustomProfile profile = CustomProfile.GetProfile();

                BrilliantWMS.WMSOutbound.WMS_SP_PickUpList_Result pkuplst = new WMSOutbound.WMS_SP_PickUpList_Result();
                pkuplst.Sequence=Convert.ToInt64(d["Sequence"]);
                pkuplst.LocQty = Convert.ToDecimal(d["LocQty"]);

                Outbound.UpdatePickUPLstQtyofSelRow(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), pkuplst, profile.DBConnection._constr);

            }
            catch(System.Exception ex) { Login.Profile.ErrorHandling(ex, "PickUp.aspx", "WMUpdatePickUpList"); }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static void WMUpdatePkupListLoc(Object obj)
        {
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d = (Dictionary<string, object>)obj;
                CustomProfile profile = CustomProfile.GetProfile();

                BrilliantWMS.WMSOutbound.WMS_SP_PickUpList_Result pkuplst = new WMSOutbound.WMS_SP_PickUpList_Result();
                pkuplst.Sequence = Convert.ToInt64(d["Sequence"]);
                pkuplst.LocationID = Convert.ToInt64(d["LocationID"]);
                pkuplst.Code = d["Code"].ToString();
                pkuplst.SortCode = Convert.ToInt64(d["SortCode"]);
                pkuplst.Capacity = Convert.ToDecimal(d["Capacity"]);
                pkuplst.AvailableBalance = Convert.ToDecimal(d["AvailableBalance"]);

                Outbound.UpdatePkupLstLocofSelRow(HttpContext.Current.Session.SessionID, ObjectName, profile.Personal.UserID.ToString(), pkuplst, profile.DBConnection._constr);
            }
            catch (System.Exception ex) { Login.Profile.ErrorHandling(ex, "PickUp.aspx", "WMUpdatePkupListLoc"); }
            finally { Outbound.Close(); }
        }

        [WebMethod]
        public static int WMShowReport(string PickUpNo)
        {
            int result = 0;
            DataSet ds = new DataSet();
            CustomProfile profile = CustomProfile.GetProfile();
            BrilliantWMS.WMSOutbound.iOutboundClient Outbound = new WMSOutbound.iOutboundClient();

            ds = Outbound.GetSavedPickUpListBySOID(PickUpNo, profile.DBConnection._constr);

            ds.Tables[0].TableName = "DataSet2";
            HttpContext.Current.Session["ReportDS"] = ds;
            HttpContext.Current.Session["SelObject"] = ds.Tables[0].Rows[0]["BatchNo"].ToString();
            HttpContext.Current.Session["BatchNo"] = ds.Tables[0].Rows[0]["BatchNo"].ToString();
            result = Convert.ToInt16(ds.Tables[0].Rows.Count);
            return result;
        }

    }
}