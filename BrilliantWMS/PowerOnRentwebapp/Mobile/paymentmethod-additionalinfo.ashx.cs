using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;

namespace BrilliantWMS.Mobile
{
    /// <summary>
    /// Summary description for paymentmethod_additionalinfo
    /// </summary>
    public class paymentmethod_additionalinfo : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string pmId;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public void ProcessRequest(HttpContext context)
        {
            string pmId = context.Request.QueryString["pmId"];
            SqlConnection conn = new SqlConnection(strcon);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.CommandText = "select * from mPaymentMethodDetail where PMethodID=" + pmId + "";
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cnt = dt.Rows.Count;

            context.Response.ContentType = "text/xml";
            string xmlString = string.Empty;
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
            xmlString = xmlString + "<addInfo>";
            if (cnt > 0)
            {
                for (int i = 0; i <= cnt - 1; i++)
                {
                    //string addinfostring = GetPMAdditionInfo(pmId);
                    string Id = dt.Rows[i]["ID"].ToString();
                    string FieldName = dt.Rows[i]["FieldName"].ToString();
                    string ControlType = dt.Rows[i]["ControlType"].ToString();
                    string Mandetory = dt.Rows[i]["Mandetory"].ToString();
                    string Query = dt.Rows[i]["Query"].ToString();

                    xmlString = xmlString + "<addId>" + Id + "</addId>";
                    xmlString = xmlString + "<fieldName>" + FieldName + "</fieldName>";
                    xmlString = xmlString + "<controlType>" + ControlType + "</controlType>";
                    xmlString = xmlString + "<mandetory>" + Mandetory + "</mandetory>";
                    xmlString = xmlString + "<query>" + Query + "</query>";
                }
            }
            xmlString = xmlString + "</addInfo>";
            xmlString = xmlString + "</gwcInfo>";
            context.Response.Write(xmlString);
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