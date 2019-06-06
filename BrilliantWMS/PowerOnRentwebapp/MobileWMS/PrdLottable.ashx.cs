using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using BrilliantWMS.Login;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for PrdLottable
    /// </summary>
    public class PrdLottable : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        string prdBarcode = "";
        long UserID = 0;
        string isGradeDropDown = "";
        long wrid = 0;
        string objectname = "";
        long oid = 0;
        string page = "";
        string isbatch = "No";
        long locid = 0;
        string loccode = "";
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);
            string Chklottable = "";
            prdBarcode = context.Request.QueryString["barcode"];
            UserID = long.Parse(context.Request.QueryString["user_id"]);
            if(context.Request.QueryString["gtype"]!=null)
            {
                isGradeDropDown = context.Request.QueryString["gtype"];  //dd or NULL
            }
            if (context.Request.QueryString["wrid"] != null)
            {
                wrid = Convert.ToInt64(context.Request.QueryString["wrid"]);
            }
            if (context.Request.QueryString["objname"] != null)
            {
                objectname = context.Request.QueryString["objname"].ToString();
            }
            if (context.Request.QueryString["oid"] != null)
            {
                oid = Convert.ToInt64(context.Request.QueryString["oid"]);
            }
            if (context.Request.QueryString["page"] != null)
            {
                page = context.Request.QueryString["page"].ToString();
            }

            if(context.Request.QueryString["scannedloc"] != null)
            {
                loccode = context.Request.QueryString["scannedloc"].ToString() ;
            }


            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;


            long CustomerID, CompanyID;
            DataSet dsUserDetail = new DataSet();
            dsUserDetail = GetUserDetails(UserID);
            CompanyID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CompanyID"].ToString());
            CustomerID = long.Parse(dsUserDetail.Tables[0].Rows[0]["CustomerID"].ToString());

            long PrdID = 0;
            PrdID = GetPrdID(prdBarcode, CompanyID, CustomerID);
            //barcode not match
            if (PrdID==0)
            {
                PrdID =GetPrdIDNew(prdBarcode, CompanyID, CustomerID);
                if(PrdID>0)
                {
                    if(page=="pickup")
                    {
                        isbatch = "Yes";
                    }
                }
                
            }
            if(PrdID==0)
            {
                PrdID = GetPrdIDNewCode(prdBarcode, CompanyID, CustomerID);
            }

            //Get Product all Details.
            DataSet dsProdDetails = new DataSet();
            string ProductName = "", ProductDess = "", ProductCode = "";          
           dsProdDetails = GetProductDetails(PrdID);
            if (dsProdDetails.Tables[0].Rows.Count > 0)
            {
                 ProductName = dsProdDetails.Tables[0].Rows[0]["Name"].ToString();
                 ProductDess = dsProdDetails.Tables[0].Rows[0]["Description"].ToString();
                 ProductCode = dsProdDetails.Tables[0].Rows[0]["ProductCode"].ToString();
            }
            string date = "";       
                
            
            //jsonString = "{\"product_id\":\"" + PrdID + "\",";
            jsonString = "{\n \"product_id\":\"" + PrdID + "\",\"product_name\":\"" + ProductName + "\",\"product_description\":\"" + ProductDess + "\",\"product_code\":\"" + ProductCode + "\", \n";          
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT ID, ProductID, LottableTitle, LottableDescription, Sequence, LottableFormat, Active,'' as LottableValue  FROM  mProductLottable where ProductID=" + PrdID + " order by Sequence";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;           
            if (cntr > 0)
            {
                if(page=="pickup")
                {
                   // string rslt = checkbatch(oid.ToString(), prdBarcode);
                    if (isbatch == "Yes")
                    {
                    jsonString = jsonString + "\n \"is_batchcode_required_to_scan\":\"" + isbatch + "\",\n";
                        string rslt = checkbatch(oid.ToString(), prdBarcode);
                        if(rslt=="Yes")
                        {
                            jsonString = jsonString + "\n \"is_batchcode_matched\":\"Yes\",\n";
                        }
                        else
                        {
                            jsonString = jsonString + "\n \"is_batchcode_matched\":\"No\",\n";
                        }
                    }
                    else
                    {
                        jsonString = jsonString + "\n \"is_batchcode_required_to_scan\":\"No\",\n";
                        jsonString = jsonString + "\n \"is_batchcode_matched\":\"\",\n";
                    }
                }
                

                jsonString = jsonString + "\n \"is_have_lottable\":\"Yes\",\n";
                string srno = ChkDuplicateLottable(prdBarcode, CompanyID, CustomerID);
                if(objectname=="SalesReturn")
                {
                    srno = ChkDuplicateLottableReturn(prdBarcode, CompanyID, CustomerID);
                }

                if(objectname=="SalesOrder" && page=="pickup")
                {
                    srno = CheckSrNoForPickUp(prdBarcode, CompanyID, CustomerID);
                }
                if (objectname == "Transfer" && page == "pickup")
                {
                    srno = CheckSrNoForPickUp(prdBarcode, CompanyID, CustomerID);
                }

                if (srno == "Yes")
                {
                    jsonString = jsonString + "\n \"is_serial_inuse\":\"Yes\",\n";
                }
                else
                {
                    jsonString = jsonString + "\n \"is_serial_inuse\":\"No\",\n";
                }
                if(page=="pickup")
                {
                    string IsSrNoAvail = GetSKUSrNoPickUp(PrdID, prdBarcode,loccode);
                    if (IsSrNoAvail == "Yes")
                    {
                        jsonString = jsonString + "\n \"is_exists_in_system\":\"Yes\",\n";
                    }
                    else
                    {
                        jsonString = jsonString + "\n \"is_exists_in_system\":\"No\",\n";
                    }
                }

                    jsonString = jsonString + "\"arr_lottable\":[\n";
                    for (int i = 0; i <= cntr - 1; i++)
                    {
                        if (prdBarcode.Length > 15)
                        {
                            date = prdBarcode.Substring(9, 4);
                        }
                        string LottableDescription = dt.Rows[i]["LottableDescription"].ToString();
                        string LottableValue = dt.Rows[i]["LottableValue"].ToString();
                        if (i == 0)
                        {
                            LottableValue = prdBarcode;
                        }
                        if (i == 1)
                        {
                            LottableValue = date;
                        }
                        if((i==2) && (page=="putin") )
                        {
                        string lott3 = GetSKUGrade(PrdID, prdBarcode, objectname);
                        // getLottableValue = "\"" + lott3.Trim() + "\"\n";
                        LottableValue = lott3;
                        }
                        if ((i == 2) && (page == "pickup"))
                        {
                            string lott3 = GetSKUGradePickUp(PrdID, prdBarcode);
                            // getLottableValue = "\"" + lott3.Trim() + "\"\n";
                            LottableValue = lott3;
                        }

                    jsonString = jsonString + "{\n";

                        // NEW CODE TO BIND GRADE DROPDOWN
                        string getLottableDescription = LottableDescription.Trim();

                        string getLottableValue = LottableValue.Trim();


                        if ((getLottableDescription == "GRADE") && (isGradeDropDown == "dd"))
                        {
                            // Temp static data... Bind to db later
                            string lottableDropDownOpt = "";
                            lottableDropDownOpt += "[\n";

                            DataSet dsgrade = new DataSet();
                            dsgrade = GetGrade();
                            int gradecount = dsgrade.Tables[0].Rows.Count;
                            if (dsgrade.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < gradecount; j++)
                                {
                                    string grade = dsgrade.Tables[0].Rows[j]["Grade"].ToString();
                                    lottableDropDownOpt += "{\n";
                                    lottableDropDownOpt += "\"ddOption\": \"" + grade.Trim() + "\",\n";
                                    lottableDropDownOpt += "\"ddvalue\": \"" + grade.Trim() + "\"\n";
                                    if (j == gradecount - 1)
                                    {
                                        lottableDropDownOpt += "}]\n";
                                    }
                                    else
                                    {
                                        lottableDropDownOpt += "},\n";
                                    }
                                }

                            }
                            // Template for loop
                            //lottableDropDownOpt += "{\n";
                            //lottableDropDownOpt += "\"ddOption\": \"A Grade\",\n";
                            //lottableDropDownOpt += "\"ddvalue\": \"A Grade\"\n";
                            //lottableDropDownOpt += "},\n"; // Don't put comma if record is last
                            //// Template for loop

                            //// Template for loop
                            //lottableDropDownOpt += "{\n";
                            //lottableDropDownOpt += "\"ddOption\": \"B Grade\",\n";
                            //lottableDropDownOpt += "\"ddvalue\": \"B Grade\"\n";
                            //lottableDropDownOpt += "}\n"; // Don't put comma if record is last
                            //// Template for loop
                            //lottableDropDownOpt += "]\n";

                            // Temp static data... Bind to db later
                            getLottableValue = lottableDropDownOpt + "\n";

                        }
                        else
                        {
                        getLottableValue = "\"" + LottableValue.Trim() + "\"\n";
                    }

                        jsonString = jsonString + "\"LottableName\": \"" + getLottableDescription + "\",\n";
                        jsonString = jsonString + "\"Lottablevalue\": " + getLottableValue + "\n";
                        // NEW CODE TO BIND GRADE DROPDOWN

                        // OLD CODE
                        //jsonString = jsonString + "\"LottableName\": \"" + LottableDescription.Trim() + "\",\n";
                        //jsonString = jsonString + "\"Lottablevalue\": \"" + LottableValue.Trim() + "\"\n";

                        if (i == cntr - 1)
                        {
                            jsonString = jsonString + "}\n";
                        }
                        else
                        {
                            jsonString = jsonString + "},\n";
                        }
                    }
            }
            else {
                //jsonString = jsonString + "\n \"is_have_lottable\":\"No\",\n";
                //jsonString = jsonString + "\"arr_lottable\":[\n";
                if (page == "pickup")
                {
                    // string rslt = checkbatch(oid.ToString(), prdBarcode);
                    if (isbatch == "Yes")
                    {
                        jsonString = jsonString + "\n \"is_batchcode_required_to_scan\":\"" + isbatch + "\",\n";
                        string rslt = checkbatch(oid.ToString(), prdBarcode);
                        if (rslt == "Yes")
                        {
                            jsonString = jsonString + "\n \"is_batchcode_matched\":\"Yes\",\n";
                        }
                        else
                        {
                            jsonString = jsonString + "\n \"is_batchcode_matched\":\"No\",\n";
                        }
                    }
                    else
                    {
                        jsonString = jsonString + "\n \"is_batchcode_required_to_scan\":\"No\",\n";
                        jsonString = jsonString + "\n \"is_batchcode_matched\":\"\",\n";
                    }
                }


                jsonString = jsonString + "\n \"is_have_lottable\":\"No\",\n";
                string srno = ChkDuplicateLottable(prdBarcode, CompanyID, CustomerID);
                if (objectname == "SalesReturn")
                {
                    srno = ChkDuplicateLottableReturn(prdBarcode, CompanyID, CustomerID);
                }

                if (objectname == "SalesOrder" && page == "pickup")
                {
                    srno = CheckSrNoForPickUp(prdBarcode, CompanyID, CustomerID);
                }
                if (objectname == "Transfer" && page == "pickup")
                {
                    srno = CheckSrNoForPickUp(prdBarcode, CompanyID, CustomerID);
                }

                if (srno == "Yes")
                {
                    jsonString = jsonString + "\n \"is_serial_inuse\":\"Yes\",\n";
                }
                else
                {
                    jsonString = jsonString + "\n \"is_serial_inuse\":\"No\",\n";
                }
                //if (page == "pickup")
                //{
                //    string IsSrNoAvail = GetSKUSrNoPickUp(PrdID, prdBarcode, loccode);
                //    if (IsSrNoAvail == "Yes")
                //    {
                //        jsonString = jsonString + "\n \"is_exists_in_system\":\"Yes\",\n";
                //    }
                //    else
                //    {
                //        jsonString = jsonString + "\n \"is_exists_in_system\":\"No\",\n";
                //    }
                //}
                jsonString = jsonString + "\n \"is_exists_in_system\":\"Yes\",\n";

                jsonString = jsonString + "\"arr_lottable\":[\n";
                for (int i = 0; i <= cntr - 1; i++)
                {
                    if (prdBarcode.Length > 15)
                    {
                        date = prdBarcode.Substring(9, 4);
                    }
                    string LottableDescription = dt.Rows[i]["LottableDescription"].ToString();
                    string LottableValue = dt.Rows[i]["LottableValue"].ToString();
                    if (i == 0)
                    {
                        LottableValue = prdBarcode;
                    }
                    if (i == 1)
                    {
                        LottableValue = date;
                    }
                    if ((i == 2) && (page == "putin"))
                    {
                        string lott3 = GetSKUGrade(PrdID, prdBarcode, objectname);
                        // getLottableValue = "\"" + lott3.Trim() + "\"\n";
                        LottableValue = lott3;
                    }
                    if ((i == 2) && (page == "pickup"))
                    {
                        string lott3 = GetSKUGradePickUp(PrdID, prdBarcode);
                        // getLottableValue = "\"" + lott3.Trim() + "\"\n";
                        LottableValue = lott3;
                    }

                    jsonString = jsonString + "{\n";

                    // NEW CODE TO BIND GRADE DROPDOWN
                    string getLottableDescription = LottableDescription.Trim();

                    string getLottableValue = LottableValue.Trim();


                    if ((getLottableDescription == "GRADE") && (isGradeDropDown == "dd"))
                    {
                        // Temp static data... Bind to db later
                        string lottableDropDownOpt = "";
                        lottableDropDownOpt += "[\n";

                        DataSet dsgrade = new DataSet();
                        dsgrade = GetGrade();
                        int gradecount = dsgrade.Tables[0].Rows.Count;
                        if (dsgrade.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < gradecount; j++)
                            {
                                string grade = dsgrade.Tables[0].Rows[j]["Grade"].ToString();
                                lottableDropDownOpt += "{\n";
                                lottableDropDownOpt += "\"ddOption\": \"" + grade.Trim() + "\",\n";
                                lottableDropDownOpt += "\"ddvalue\": \"" + grade.Trim() + "\"\n";
                                if (j == gradecount - 1)
                                {
                                    lottableDropDownOpt += "}]\n";
                                }
                                else
                                {
                                    lottableDropDownOpt += "},\n";
                                }
                            }

                        }
                        // Template for loop
                        //lottableDropDownOpt += "{\n";
                        //lottableDropDownOpt += "\"ddOption\": \"A Grade\",\n";
                        //lottableDropDownOpt += "\"ddvalue\": \"A Grade\"\n";
                        //lottableDropDownOpt += "},\n"; // Don't put comma if record is last
                        //// Template for loop

                        //// Template for loop
                        //lottableDropDownOpt += "{\n";
                        //lottableDropDownOpt += "\"ddOption\": \"B Grade\",\n";
                        //lottableDropDownOpt += "\"ddvalue\": \"B Grade\"\n";
                        //lottableDropDownOpt += "}\n"; // Don't put comma if record is last
                        //// Template for loop
                        //lottableDropDownOpt += "]\n";

                        // Temp static data... Bind to db later
                        getLottableValue = lottableDropDownOpt + "\n";

                    }
                    else
                    {
                        getLottableValue = "\"" + LottableValue.Trim() + "\"\n";
                    }

                    jsonString = jsonString + "\"LottableName\": \"" + getLottableDescription + "\",\n";
                    jsonString = jsonString + "\"Lottablevalue\": " + getLottableValue + "\n";
                    // NEW CODE TO BIND GRADE DROPDOWN

                    // OLD CODE
                    //jsonString = jsonString + "\"LottableName\": \"" + LottableDescription.Trim() + "\",\n";
                    //jsonString = jsonString + "\"Lottablevalue\": \"" + LottableValue.Trim() + "\"\n";

                    if (i == cntr - 1)
                    {
                        jsonString = jsonString + "}\n";
                    }
                    else
                    {
                        jsonString = jsonString + "},\n";
                    }
                }
            }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        private string checkbatch(string orderno,string prdBarcode)
        {
            string codeprd = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            string result = "No";
            string batch = "";
            cmd1.CommandType = CommandType.StoredProcedure;
           // cmd1.CommandText = "select ID from mProduct where ProductCode='" + prdBarcode + "' and CompanyID=" + companyID + " and CustomerID=" + customerID + "";
            cmd1.CommandText = "WMS_SP_PickUpList";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            if(objectname=="SalesOrder")
            {
                cmd1.Parameters.AddWithValue("@ODID", orderno);
                cmd1.Parameters.AddWithValue("@TRID", "");
            }
            else
            {
                cmd1.Parameters.AddWithValue("@ODID","");
                cmd1.Parameters.AddWithValue("@TRID", orderno);
            }
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for(int k=0;k<=dt1.Rows.Count-1;k++)
                {
                    codeprd = dt1.Rows[k]["ProductCode"].ToString();
                    batch = dt1.Rows[k]["BatchNo"].ToString();

                    string newprd = codeprd.Substring(codeprd.Length - 10);
                    string newbatch = batch.Remove(0, 6);
                    string newprodcode = newprd + "_" + newbatch;
                    if(newprodcode== prdBarcode)
                    {
                        result = "Yes";
                    }
                }
            }
            return result;
        }

        private string ChkDuplicateLottable(string lot1, long companyid, long customerid)
        {
            string result = "";
            DataTable dtnew = new DataTable();
            dtnew = GetDatatableDetails("select * from tSkuTransactionHistory where companyid=" + companyid + " and customerid=" + customerid + " and lottable1='" + lot1 + "'");
            if (dtnew.Rows.Count > 0)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

        private string ChkDuplicateLottableReturn(string lot1, long companyid, long customerid)
        {
            string result = "";
            DataTable dtnew = new DataTable();
            dtnew = GetDatatableDetails("select * from tSkuTransactionHistory where finalzone='GRN' and object='SalesReturn' and lottable1='" + lot1 + "'");
            if (dtnew.Rows.Count > 0)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

        private string CheckSrNoForPickUp(string lott1, long companyid, long customerid)
        {
            string result = "";
            DataTable dtnew = new DataTable();
            dtnew = GetDatatableDetails("select * from tSkuTransactionHistory where companyid=" + companyid + " and customerid=" + customerid + " and lottable1='" + lott1 + "' and finalzone='PickUp'");
            if (dtnew.Rows.Count > 0)
            {
                result = "Yes";
            }
            else
            {
                result = "No";
            }
            return result;
        }

        private DataTable GetDatatableDetails(string Query)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection conn = new SqlConnection(strcon);
                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.Fill(dt);
                    }
                }

            }
            catch
            { }
            finally { }
            return dt;
        }

        private long GetPrdIDNew(string prdBarcode, long companyID, long customerID)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.StoredProcedure;
            //  cmd1.CommandText = "select ID from mProduct where ProductCode='" + prdBarcode + "' and CompanyID=" + companyID + " and CustomerID=" + customerID + "";
            cmd1.CommandText = "SP_GetProductCode";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@code", prdBarcode);
            cmd1.Parameters.AddWithValue("@compid", companyID);
            cmd1.Parameters.AddWithValue("@custid", customerID);
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                id = Convert.ToInt64(ds1.Tables[0].Rows[0]["ID"].ToString());
            }
            return id;
        }

        private long GetPrdIDNewCode(string prdBarcode, long companyID, long customerID)
        {
            long id = 0;
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
              cmd1.CommandText = "select ID from mProduct where ProductCode='" + prdBarcode + "' and CompanyID=" + companyID + " and CustomerID=" + customerID + "";
           // cmd1.CommandText = "SP_GetProductCode";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (ds1.Tables[0].Rows.Count > 0)
            {
                id = Convert.ToInt64(ds1.Tables[0].Rows[0]["ID"].ToString());
            }
            return id;
        }


        private DataSet GetProductDetails(long prdID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from mproduct where id =" + prdID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        private DataSet GetGrade()
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from mGrade where WarehouseID =" + wrid + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        private string GetSKUGrade(long skuid,string lott1,string objname)
        {
            string lott3 = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from tskutransactionhistory where skuid='"+ skuid + "'  and Lottable1='"+ lott1 + "' and finalzone='QC' and object='"+ objname + "'";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            if(ds1.Tables[0].Rows.Count>0)
            {
                lott3 = ds1.Tables[0].Rows[0]["Lottable3"].ToString();
            }
            return lott3;
        }

        private string GetSKUGradePickUp(long skuid, string lott1)
        {
            string lott3 = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from tskutransactionhistory where skuid='" + skuid + "'  and Lottable1='" + lott1 + "' and finalzone='PutIn'";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lott3 = ds1.Tables[0].Rows[0]["Lottable3"].ToString();
            }
            return lott3;
        }

        private string GetSKUSrNoPickUp(long skuid, string lott1,string loccode)
        {
            string lott3 = "";
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataTable dt1 = new DataTable();
            long locid = 0;

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from mLocation where Code='"+loccode+"' and WarehouseID='"+wrid+"'";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da2.SelectCommand = cmd1;
            da2.Fill(ds2, "tbl1");
            if(ds2.Tables[0].Rows.Count>0)
            {
                locid = Convert.ToInt64(ds2.Tables[0].Rows[0]["ID"]);
            }

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select * from tskutransaction where skuid='" + skuid + "'  and Lottable1='" + lott1 + "'and LocationID='"+ locid + "'";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                lott3 = "Yes";
            }
            else
            {
                lott3 = "No";
            }
            return lott3;
        }



        public DataSet GetUserDetails(long UserID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select CompanyID,CustomerID from mUserProfileHEad where id =" + UserID + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            return ds1;
        }

        public long GetPrdID(string prdBarcode, long CompanyID, long CustomerID)
        {
            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd2 = new SqlCommand();
            SqlDataAdapter da2 = new SqlDataAdapter();
            DataSet ds2 = new DataSet();
            DataTable dt2 = new DataTable();
            long PrdID = 0;
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.CommandText = "SP_GetGRNBarcode";
            cmd2.Connection = conn;
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@Barcode", prdBarcode);
            cmd2.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd2.Parameters.AddWithValue("@CustomerID", CustomerID);
            da2.SelectCommand = cmd2;
            da2.Fill(ds2, "tbl1");
            dt2 = ds2.Tables[0];
            if (ds2.Tables[0].Rows.Count > 0)
            {
                 PrdID = long.Parse(ds2.Tables[0].Rows[0]["ID"].ToString());
            }
            return PrdID;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}