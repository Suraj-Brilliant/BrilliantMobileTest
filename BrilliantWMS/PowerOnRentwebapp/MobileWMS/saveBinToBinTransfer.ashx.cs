using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;
using BrilliantWMS.Login;
using BrilliantWMS.WMSOutbound;

namespace BrilliantWMS.MobileWMS
{
    /// <summary>
    /// Summary description for saveBinToBinTransfer
    /// </summary>
    public class saveBinToBinTransfer : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;

        long WarehouseID = 0, UserID = 0;
        long txtProductId = 0, txtOldLocationId = 0, txtNewLocationId = 0;
        string txtOldLocationCode = "", txtNewLocationCode = "", txtSearialCode = "";
        decimal txtQty = 0;

        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/plain";
            String jsonString = String.Empty;
            //WarehouseID = long.Parse(context.Request.QueryString["wrid"]);
            //UserID = long.Parse(context.Request.QueryString["user_id"]);
            long IntID = 0;

            jsonString = "{\n\"result\": [\n";
            try
            {
                if (context.Request.Form["uid"] != null)
                {
                    UserID = Convert.ToInt64(context.Request.Form["uid"]);
                }
                if (context.Request.Form["wid"] != null)
                {
                    WarehouseID = Convert.ToInt64(context.Request.Form["wid"]);
                }
                if (context.Request.Form["txtProductId"] != null)
                {
                    txtProductId = Convert.ToInt64(context.Request.Form["txtProductId"]);
                }
                if (context.Request.Form["txtOldLocationId"] != null)
                {
                    txtOldLocationId = Convert.ToInt64(context.Request.Form["txtOldLocationId"]);
                }
                if (context.Request.Form["txtNewLocationId"] != null)
                {
                    txtNewLocationId = Convert.ToInt64(context.Request.Form["txtNewLocationId"]);
                }
                if (context.Request.Form["txtOldLocationCode"] != null)
                {
                    txtOldLocationCode = context.Request.Form["txtOldLocationCode"].ToString();
                }
                if (context.Request.Form["txtNewLocationCode"] != null)
                {
                    txtNewLocationCode = context.Request.Form["txtNewLocationCode"].ToString();
                }
                if (context.Request.Form["txtSearialCode"] != null)
                {
                    txtSearialCode = context.Request.Form["txtSearialCode"].ToString();
                }
                if (context.Request.Form["txtQty"] != null)
                {
                    txtQty = Convert.ToDecimal(context.Request.Form["txtQty"]);
                }

                string userName = GetUserID(UserID);
                CustomProfile profile = CustomProfile.GetProfile(userName);
                iOutboundClient outbound = new iOutboundClient();
                tInternalTransferHead IntHead = new tInternalTransferHead();

                IntHead.TransferDate = DateTime.Now;
                IntHead.TransferBy = UserID;
                IntHead.Status = 64;
                IntHead.WarehouseID = WarehouseID;
                IntHead.Remark = "";
                IntHead.CreatedBy = UserID;
                IntHead.CreationDate = DateTime.Now;
                IntHead.CompanyID = profile.Personal.CompanyID;
                IntHead.CustomerID = profile.Personal.CustomerId;
                if (IntID == 0)
                {
                    IntID = outbound.SaveIntotInternalTransferHead(IntHead, profile.DBConnection._constr);
                }
                if (IntID > 0)
                {
                    SqlCommand cmdDetail = new SqlCommand();
                    SqlDataAdapter daDetail = new SqlDataAdapter();
                    cmdDetail.CommandType = CommandType.StoredProcedure;
                    cmdDetail.CommandText = "SP_SaveIntTransferDetail";
                    cmdDetail.Connection = conn;
                    cmdDetail.Parameters.Clear();
                    cmdDetail.Parameters.AddWithValue("intid", IntID);
                    cmdDetail.Parameters.AddWithValue("prdID", txtProductId);
                    cmdDetail.Parameters.AddWithValue("oldloc", txtOldLocationId);
                    cmdDetail.Parameters.AddWithValue("newloc", txtNewLocationId);
                    cmdDetail.Parameters.AddWithValue("qty", txtQty);
                    cmdDetail.Parameters.AddWithValue("UOMID", 30);
                    cmdDetail.Parameters.AddWithValue("seq", 1);
                    cmdDetail.Parameters.AddWithValue("srno", txtSearialCode);
                    cmdDetail.Connection.Open();
                    int result = cmdDetail.ExecuteNonQuery();
                    cmdDetail.Connection.Close();

                    if (result > 0)
                    {
                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"status\": \"success\",\n";
                        jsonString = jsonString + "\"reason\": \"\"\n";
                        jsonString = jsonString + "}\n";
                        jsonString = jsonString + "]\n}";
                        context.Response.Write(jsonString);
                    }
                    else
                    {
                        jsonString = jsonString + "{\n";
                        jsonString = jsonString + "\"status\": \"failed\",\n";
                        jsonString = jsonString + "\"reason\": \"servererror\"\n";
                        jsonString = jsonString + "}\n";
                        jsonString = jsonString + "]\n}";
                        context.Response.Write(jsonString);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonString = jsonString + "{\n";
                jsonString = jsonString + "\"status\": \"failed\",\n";
                jsonString = jsonString + "\"reason\": \"servererror\"\n";
                jsonString = jsonString + "}\n";
                jsonString = jsonString + "]\n}";
                context.Response.Write(jsonString);
            }
            finally
            {

            }
        }

        public string GetUserID(long uid)
        {
            SqlCommand cmd1 = new SqlCommand();
            SqlDataAdapter da1 = new SqlDataAdapter();
            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            string username = "";
            SqlConnection conn = new SqlConnection(strcon);
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = "select top (1) UserName from mPassWordDetails where UserProfileID=" + uid + "";
            cmd1.Connection = conn;
            cmd1.Parameters.Clear();
            da1.SelectCommand = cmd1;
            da1.Fill(ds1, "tbl1");
            dt1 = ds1.Tables[0];
            if (dt1.Rows.Count > 0)
            {
                username = dt1.Rows[0]["UserName"].ToString();
            }
            return username;
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