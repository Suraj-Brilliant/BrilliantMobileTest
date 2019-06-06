using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PowerOnRentwebapp.Login;
using System.IO;
using System.Web.Security;

namespace PowerOnRentwebapp.Deliveries
{
    /// <summary>
    /// Summary description for download_product_info_by_order_id
    /// </summary>
    public class download_product_info_by_order_id : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 0;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            jsonString = "{\n\"prodlist\": [\n";

            //string skey = context.Request.QueryString["skey"];
           // string driverid = context.Request.QueryString["drId"];
            string orid = context.Request.QueryString["orId"];

            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select OD.Id,OD.OrderheadId,OD.SkuId,OD.OrderQty,OD.UOMID,U.Description UOM,OD.Sequence,OD.Prod_Name,OD.Prod_description   from torderdetail OD left outer join mUOM U on OD.UOMID=U.ID  where OrderheadId IN("+ orid +")";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl2");
            dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    string orderId = dt1.Rows[i]["OrderheadId"].ToString();
                    string prodId = dt1.Rows[i]["SkuId"].ToString();
                    string prodTitle = dt1.Rows[i]["Prod_Name"].ToString();
                    string qty = dt1.Rows[i]["OrderQty"].ToString();
                    string unitId = dt1.Rows[i]["UOMID"].ToString();
                    string unitLabel = dt1.Rows[i]["UOM"].ToString();

                    jsonString = jsonString + "{\n";
                    jsonString = jsonString + "\"orderId\": \"" + orderId.Trim() + "\",\n";
                    jsonString = jsonString + "\"prodId\": \"" + prodId.Trim() + "\",\n";
                    jsonString = jsonString + "\"prodTitle\": \"" + prodTitle.Trim() + "\",\n";
                    jsonString = jsonString + "\"qty\": \"" + qty.Trim() + "\",\n";
                    jsonString = jsonString + "\"unitId\": \"" + unitId.Trim() + "\",\n";
                    jsonString = jsonString + "\"unitLabel\": \"" + unitLabel.Trim() + "\"\n";

                    if (i == dt1.Rows.Count - 1)
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}