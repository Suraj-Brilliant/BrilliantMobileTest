using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for transfer_product_list
    /// </summary>
    public class transfer_product_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        long WarehouseID = 0, transferID = 0;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"resultlist\": [\n";   /*json Loop Start*/
            WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            transferID = long.Parse(context.Request.QueryString["orid"]);

            if (context.Request.QueryString["orid"] != null)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select TD.InternalTransferID,TD.SKUId,P.Name,P.ProductCode,P.OMSSKUCode,TD.FromLocation,FrmLc.Code FromLocationCode, TD.ToLocation,ToLc.Code ToLocationCode, TD.Qty,TD.UOMID,TD.Sequence ,ST.BatchCode,FrmLc.SortCode ,ROW_NUMBER() over(order By FrmLc.SortCode) as Seq from tInternalTransferDetail TD left outer join mProduct P on TD.SKUId = P.ID left outer join mLocation FrmLc on TD.FromLocation = FrmLc.ID left outer join mLocation ToLc on TD.ToLocation = ToLc.ID left outer join tSkuTransaction ST on TD.SKUId = ST.SKUId and TD.FromLocation = ST.LocationID where InternalTransferID = " + transferID + " order by  FrmLc.SortCode";
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;
                if (cntr > 0)
                {
                    for (int i = 0; i <= cntr - 1; i++)
                    {
                        string id = dt.Rows[i]["SKUId"].ToString();
                        string productName = dt.Rows[i]["Name"].ToString();
                        string productCode = dt.Rows[i]["ProductCode"].ToString();
                        string frmlocationCode = dt.Rows[i]["FromLocationCode"].ToString();
                        string qty = dt.Rows[i]["Qty"].ToString();
                        string sequence = dt.Rows[i]["Seq"].ToString();
                        string batchCode = dt.Rows[i]["BatchCode"].ToString();
                        string omsSkuCode = dt.Rows[i]["OMSSKUCode"].ToString();
                        string tolocationCode = dt.Rows[i]["ToLocationCode"].ToString();
                        string frmlocdbid = dt.Rows[i]["FromLocation"].ToString();
                        string tolocdbid = dt.Rows[i]["ToLocation"].ToString();

                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"id\": \"" + id.Trim() + "\",\n";
                        jsonString = jsonString + "\"productName\": \"" + CheckString(productName.Trim()) + "\",\n";
                        jsonString = jsonString + "\"skuCode\": \"" + CheckString(omsSkuCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCode\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"productCodeImg\": \"" + CheckString(productCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCode\": \"" + CheckString(frmlocationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationCodeImg\": \"" + CheckString(frmlocationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"tolocationCode\": \"" + CheckString(tolocationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"tolocationCodeImg\": \"" + CheckString(tolocationCode.Trim()) + "\",\n";
                        jsonString = jsonString + "\"locationDbId\": \"" + CheckString(frmlocdbid.Trim()) + "\",\n";
                        jsonString = jsonString + "\"tolocationDbId\": \"" + CheckString(tolocdbid.Trim()) + "\",\n";
                        jsonString = jsonString + "\"qty\": \"" + CheckString(qty.Trim()) + "\",\n";
                        jsonString = jsonString + "\"sequence\": \"" + CheckString(sequence.Trim()) + "\",\n";
                        jsonString = jsonString + "\"serialCode\": \"N/A\",\n";
                        jsonString = jsonString + "\"batchCode\": \"" + CheckString(batchCode.Trim()) + "\"\n";

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
            }
            else { }
            jsonString = jsonString + "]\n}";  /*json Loop End*/
            context.Response.Write(jsonString);
        }

        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
            value = value.Replace("\"", "&quot;");
            return value;
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