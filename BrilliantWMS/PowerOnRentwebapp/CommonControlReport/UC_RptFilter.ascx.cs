using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Collections;
//using System.Net.Json;
using System.IO;
using AjaxControlToolkit;
using Obout.Grid;
using Obout.Interface;
using BrilliantWMS.Login;

namespace BrilliantWMS.CommonControlReport
{
    public partial class UC_RptFilter : System.Web.UI.UserControl
    {       

        protected void Page_Load(object sender, EventArgs e)
        {
          
                string Rptid = (Request.QueryString["id"]).ToString();
                hdnRptId.Value = Rptid;
                CreateDynamicTable();
           
        }

        public void CreateDynamicTable()
        {
            DataSet ds = new DataSet();
            //ds.GetJSON();
            DataTable dt = new DataTable();

            DataSet Field_ds = new DataSet();
            DataTable Field_dt = new DataTable();
            ds = GetDataDB("SP_GetRptField", "");  //calling the function which describe the fieldname and fieldtype
            dt = ds.Tables[0];
            Table table = new Table();

            table.ID = "tbltrl";
            //table.Width = 200;


            const int colsCount = 2;
            if (dt.Rows.Count > 0)
            {

                for (Int32 i = 0; i < dt.Rows.Count - 1; )
                {
                    TableRow row = new TableRow();

                    String FieldName = Convert.ToString(dt.Rows[i]["Label"]);
                    String FieldType = Convert.ToString(dt.Rows[i]["ControlType"]);
                    String FieldValue = Convert.ToString(dt.Rows[i]["ValueField"]);
                    String FieldText = Convert.ToString(dt.Rows[i]["TextField"]);
                    String ControlId = Convert.ToString(dt.Rows[i]["ControlId"]);
                    String wid1 = Convert.ToString(dt.Rows[i]["width"]);
                    String clientMethod = Convert.ToString(dt.Rows[i]["clientMethod"]);
                    //td.Controls.Add(lbcustomename);
                    //tr.Controls.Add(td);


                    for (int j = 0; j < colsCount; )
                    {
                        TableCell cell = new TableCell();
                        TableCell cell1 = new TableCell();
                        cell.Style.Add("width", "170px");
                        cell1.Style.Add("width", "170px");

                        //cell.Width = 50;
                        FieldName = Convert.ToString(dt.Rows[i]["Label"]);
                        FieldType = Convert.ToString(dt.Rows[i]["ControlType"]);
                        FieldValue = Convert.ToString(dt.Rows[i]["ValueField"]);
                        FieldText = Convert.ToString(dt.Rows[i]["TextField"]);
                        ControlId = Convert.ToString(dt.Rows[i]["ControlId"]);
                        wid1 = Convert.ToString(dt.Rows[i]["width"]);
                        clientMethod = Convert.ToString(dt.Rows[i]["clientMethod"]);

                        string query = Convert.ToString(dt.Rows[i]["Condition"]);
                        if (query != null && query != "")
                        {
                            Field_ds = GetDataDB(query, Convert.ToString(dt.Rows[i]["FieldId"]));
                            Field_dt = Field_ds.Tables[0];
                        }

                        if (FieldType.ToLower().Trim() != "btn" && FieldType.ToLower().Trim() != "grid")
                        {
                            Label lbcustomename = new Label();
                            lbcustomename.ID = "lbl" + FieldName;
                            lbcustomename.Text = FieldName;
                           // lbcustomename.CssClass = "headerText";
                            cell.Controls.Add(lbcustomename);
                            row.Cells.Add(cell);

                        }
                        if (FieldType.ToLower().Trim() == "ddl")
                        {

                            DataRow newRow = Field_dt.NewRow();
                            newRow[0] = "0";
                            newRow[1] = "-Select-";
                            Field_dt.Rows.InsertAt(newRow, 0);

                            DropDownList lst = new DropDownList();
                            lst.ID = ControlId;

                            lst.ClientIDMode = ClientIDMode.Static;
                            lst.Width = Convert.ToInt32(wid1);
                            //ScriptManager.RegisterStartupScript(Page, GetType(), "disp_confirm", "<script>disp_confirm()</script>", false);
                            lst.DataSource = Field_dt;
                            lst.DataTextField = FieldText;
                            lst.DataValueField = FieldValue;
                            lst.DataBind();
                            // td1.Controls.Add(lst);
                            cell1.Controls.Add(lst);
                            row.Cells.Add(cell1);
                        }

                        else if (FieldType.ToLower().Trim() == "date")
                        {

                            TextBox txtdate = new TextBox();
                            txtdate.ID = ControlId;
                            txtdate.CssClass = "date";
                            txtdate.Width = Convert.ToInt32(wid1);
                            txtdate.ClientIDMode = ClientIDMode.Static;
                            txtdate.CssClass = "cal_Theme1";
                            cell1.Controls.Add(txtdate);
                            row.Controls.Add(cell1);

                        }
                        j++; i++;
                    }
                    table.Rows.Add(row);

                    //if (FieldType.ToLower().Trim() == "btn")
                    //{
                    //    TableCell cell = new TableCell();
                    //    Button btnSubmit = new Button();
                    //    btnSubmit.ID = ControlId;
                    //    btnSubmit.OnClientClick = clientMethod;
                    //    btnSubmit.Click += new EventHandler(ShowGrid_Click);
                    //    btnSubmit.Text = FieldName;
                    //    btnSubmit.Text = FieldName;
                    //    btnSubmit.ClientIDMode = ClientIDMode.Static;

                    //    cell.Controls.Add(btnSubmit);
                    //    row.Cells.Add(cell);

                    //}

                    //if (Convert.ToString(dt.Rows[i]["issubreport"]).ToString() == "Y")
                    //{
                    //    bnShowDtls.Visible = true;
                    //}


                }
                Rpt_placeH.Controls.Add(table);
                string str = hdnSelVal.Value;

                if (str == "")
                    Table1.Visible = false;
                if (ds.Tables[1].Rows[0][0].ToString() != "")
                    hdnIsSubRpt.Value = ds.Tables[1].Rows[0][0].ToString();

            }
        }

        public static DataSet GetReportDt(string[] values, string reportId)
        {
            DataSet dt = new DataSet();
            dt.Reset();

            if (values != null)
            {
                using (SqlConnection conn = new SqlConnection("Data Source=BWMSTest.db.11040877.c93.hostedresource.net; Initial Catalog=BWMSTest; User ID=BWMSTest; Password='Password123#'"))

                using (SqlCommand cmd = new SqlCommand("SP_GetRptObjetData", conn))
                {

                    if (values != null)
                    {
                        if (values[0] != "" && values[0] != null)
                        {
                            cmd.Parameters.Add("@Object", SqlDbType.VarChar).Value = values[0];
                        }
                        if (values[1] != "" && values[1] != null)
                        {
                            cmd.Parameters.Add("@CompIds", SqlDbType.VarChar).Value = values[1];
                        }
                        if (values[2] != "" && values[2] != null)
                        {
                            cmd.Parameters.Add("@CustomerIds", SqlDbType.VarChar).Value = values[2];
                        }
                        if (values[3] != "" && values[3] != null)
                        {
                            cmd.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = values[3];
                        }
                        if (values[4] != "" && values[4] != null)
                        {
                            cmd.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = values[4];
                        }
                        if (values[5] != "" && values[5] != null)
                        {
                            cmd.Parameters.Add("@VendorIDs", SqlDbType.VarChar).Value = values[5];
                        }
                        if (values[6] != "" && values[6] != null)
                        {
                            cmd.Parameters.Add("@userIds", SqlDbType.VarChar).Value = values[6];
                        }
                        if (values[7] != "" && values[7] != null)
                        {
                            cmd.Parameters.Add("@WareHouseIds", SqlDbType.VarChar).Value = values[7];
                        }
                        if (reportId != "" && reportId != null)
                        {
                            cmd.Parameters.Add("@ReportId", SqlDbType.VarChar).Value = reportId;
                        }
                    }


                    cmd.CommandType = CommandType.StoredProcedure;

                    // open connection, execute non query        
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // Fill the DataSet using default values for DataTable names, etc
                    da.Fill(dt);

                    // close connection
                    conn.Close();
                }

            }
            return dt;
        }


        public DataSet GetDataDB(string Sp_name, string paramId)
        {
            DataSet ds1 = new DataSet();
            ds1.Reset();


            using (SqlConnection conn = new SqlConnection("Data Source=BWMSTest.db.11040877.c93.hostedresource.net; Initial Catalog=BWMSTest; User ID=BWMSTest; Password='Password123#'"))

            using (SqlCommand cmd = new SqlCommand(Sp_name, conn))
            {
                if (hdnRptId.Value != null && hdnRptId.Value != "")
                {
                    cmd.Parameters.Add("@ReportId", SqlDbType.VarChar).Value = hdnRptId.Value;
                }

                if (paramId != null && paramId != "")
                {
                    cmd.Parameters.Add("@Object", SqlDbType.VarChar).Value = paramId;
                }


                cmd.CommandType = CommandType.StoredProcedure;

                // open connection, execute non query        
                conn.Open();

                cmd.ExecuteNonQuery();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds1);

                // close connection
                conn.Close();
            }
            return ds1;

        }

        protected void ShowGrid_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string str = hdnSelVal.Value;
            string[] values = str.Split(',');
            ds = GetReportDt(values, hdnRptId.Value);
            GvList.DataSource = ds.Tables[0];
            GvList.DataBind();
            Table1.Visible = true;
            btnShowList.Visible = true;
            if (hdnIsSubRpt.Value == "Y")
            { btnShowDtls.Visible = true; }

            string hdnSelAllVal = String.Join(",", ds.Tables[0].AsEnumerable().Select(x => x.Field<Int64>("id")).ToArray());
            hdnSelAll.Value = hdnSelAllVal;
        }

        public static DataSet GetFilterData(string CtlID, string FilterId, string ReportId, string fromDt, string ToDt)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection("Data Source=BWMSTest.db.11040877.c93.hostedresource.net; Initial Catalog=BWMSTest; User ID=BWMSTest; Password='Password123#'"))

            using (SqlCommand cmd = new SqlCommand("SP_GetRptObjetData", conn))
            {
                if (ReportId != null && ReportId != "")
                {
                    cmd.Parameters.Add("@ReportId", SqlDbType.VarChar).Value = ReportId;
                }

                if (CtlID != null && CtlID != "")
                {
                    cmd.Parameters.Add("@Object", SqlDbType.VarChar).Value = CtlID;
                }
                if (FilterId != null && FilterId != "")
                {
                    cmd.Parameters.Add("@SearchIds", SqlDbType.VarChar).Value = FilterId;
                }
                if (fromDt != null && fromDt != "")
                {
                    cmd.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = fromDt;
                }
                if (ToDt != null && ToDt != "")
                {
                    cmd.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = ToDt;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                conn.Close();
            }
            return ds;
        }

        public bool IsPostback { get; set; }
    }
}