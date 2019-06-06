using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for my_request_list
    /// </summary>
    public class my_request_list : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        long ResourceId = 1;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        long usrid = 0;
        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            context.Response.ContentType = "text/xml";
            String xmlString = String.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";

            string skey = context.Request.QueryString["skey"];
            string st = context.Request.QueryString["st"];
            string dptId = context.Request.QueryString["dptId"];

            if (context.Request.QueryString["uId"] != null)
            {
                usrid = Convert.ToInt32(context.Request.QueryString["uId"]);

                cmd.CommandType = CommandType.Text;
                if ((context.Request.QueryString["skey"] != null) && (context.Request.QueryString["skey"] != ""))
                {
                    if (st == null || st == "")
                    {
                        cmd.CommandText = "select OH.Id,Replace(OH.OrderNumber,'&','&amp;')OrderNumber,Replace(OH.Title,'&','&amp;')Title,Replace(OH.Remark,'&','&amp;')Remark,ST.Status StatusName,ST.ID StatusID ,OH.StoreId,OH.RequestBy,OH.OrderNo  from torderhead OH left outer join mStatus ST on OH.Status =ST.ID where OH.RequestBy=" + usrid + " and (OH.OrderNumber like '%" + skey + "%' OR OH.Title like '%" + skey + "%' OR OH.Remark like '%" + skey + "%' OR OH.OrderNo like '%" + skey + "%') and OH.StoreId=" + dptId + "  order by OH.Id desc";
                    }
                    else
                    {
                        cmd.CommandText = "select OH.Id,Replace(OH.OrderNumber,'&','&amp;')OrderNumber,Replace(OH.Title,'&','&amp;')Title,Replace(OH.Remark,'&','&amp;')Remark,ST.Status StatusName,ST.ID StatusID ,OH.StoreId,OH.RequestBy,OH.OrderNo from torderhead OH left outer join mStatus ST on OH.Status =ST.ID where OH.RequestBy=" + usrid + " and (OH.OrderNumber like '%" + skey + "%' OR OH.Title like '%" + skey + "%' OR OH.Remark like '%" + skey + "%' OR OH.OrderNo like '%" + skey + "%') and ST.ID=" + st + " and OH.StoreId=" + dptId + " order by OH.Id desc";
                    }
                }
                else //if ((context.Request.QueryString["skey"] == null) && (context.Request.QueryString["skey"] == ""))
                {
                    if (st == null || st == "")
                    {
                        cmd.CommandText = "select OH.Id,Replace(OH.OrderNumber,'&','&amp;')OrderNumber,Replace(OH.Title,'&','&amp;')Title,Replace(OH.Remark,'&','&amp;')Remark,ST.Status StatusName,ST.ID StatusID ,OH.StoreId,OH.RequestBy,OH.OrderNo from torderhead OH left outer join mStatus ST on OH.Status =ST.ID where OH.RequestBy=" + usrid + " and OH.StoreId=" + dptId + "  order by OH.Id desc";
                    }
                    else
                    {
                        cmd.CommandText = "select OH.Id,Replace(OH.OrderNumber,'&','&amp;')OrderNumber,Replace(OH.Title,'&','&amp;')Title,Replace(OH.Remark,'&','&amp;')Remark,ST.Status StatusName,ST.ID StatusID ,OH.StoreId,OH.RequestBy,OH.OrderNo from torderhead OH left outer join mStatus ST on OH.Status =ST.ID where OH.RequestBy=" + usrid + " and ST.ID=" + st + " and StoreId=" + dptId + "  order by OH.Id desc";
                    }
                }
                cmd.Connection = conn;
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("param1", ResourceId);

                da.SelectCommand = cmd;
                da.Fill(ds, "tbl1");
                dt = ds.Tables[0];
                int cntr = dt.Rows.Count;

                if (cntr > 0)
                {
                    for (int i = 0; i < cntr; i++)
                    {
                        String reqId = dt.Rows[i]["Id"].ToString();
                        String reqNo = dt.Rows[i]["OrderNumber"].ToString();
                        String reqTitle = dt.Rows[i]["Title"].ToString();
                        String reqNote = dt.Rows[i]["Remark"].ToString();
                        String Status = dt.Rows[i]["StatusName"].ToString();
                        String StatusId = dt.Rows[i]["StatusID"].ToString();
                        String OrderNo = dt.Rows[i]["OrderNo"].ToString();


                        // if (skuCode == "") skuCode = "N/A";

                        // RECORD LOOP
                        xmlString = xmlString + "<requestItem>";
                        xmlString = xmlString + "<reId>" + OrderNo + "</reId>";
                        xmlString = xmlString + "<reNo>Order No. " + CheckString(reqNo) + "</reNo>";
                        xmlString = xmlString + "<itemTitle>" + CheckString(reqTitle) + "</itemTitle>";
                        xmlString = xmlString + "<itemNote>" + reqNote + "</itemNote>";
                        xmlString = xmlString + "<itemStatus>" + Status + "</itemStatus>";
                        xmlString = xmlString + "<itemStatusId>" + StatusId + "</itemStatusId>";
                        //xmlString = xmlString + "<gwcOrderLabel>" + OrderNo + "</gwcOrderLabel>";
                        xmlString = xmlString + "</requestItem>";
                        // RECORD LOOP
                    }
                }
            }
            else
            {
                // throw new ArgumentException("No Parameter Specified");
            }
            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
        }


        public string CheckString(string value)
        {
            value = value.Replace("&", "and");
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