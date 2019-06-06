using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace PowerOnRentwebapp.Mobile
{
    /// <summary>
    /// Summary description for get_item_detials_by_template
    /// </summary>
    public class get_item_details_by_template : IHttpHandler
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlDataReader dr;
        string strcon = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;        

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection conn = new SqlConnection(strcon);

            long tmplId = Convert.ToInt64(context.Request.QueryString["tmplId"]);

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select TH.ID,TH.TemplateTitle,TH.Accesstype,Th.Department,TH.Remark,TH.Address,Th.Contact1,TH.Contact2, CPD1.Name Con1,CPD2.Name Con2,Adr.AddressLine1 from mRequestTemplateHead TH  left outer join tContactPersonDetail CPD1 on Th.Contact1=CPD1.ID  left outer join tContactPersonDetail CPD2 on TH.Contact2=CPD2.ID  left outer join tAddress Adr on TH.Address=Adr.ID  where TH.ID=" + tmplId + "";
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            /*cmd.Parameters.AddWithValue("SiteIDs", 0);
            cmd.Parameters.AddWithValue("UserID", uId);*/
            da.SelectCommand = cmd;
            da.Fill(ds, "tbl1");
            dt = ds.Tables[0];
            int cntr = dt.Rows.Count;

            String xmlString = String.Empty;
            context.Response.ContentType = "text/xml";
            xmlString = xmlString + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            xmlString = xmlString + "<gwcInfo>";
                        
            //string Con1 = ds.Tables[0].Rows[0]["Con1"].ToString();
            //string Con2 = ds.Tables[0].Rows[0]["Con2"].ToString();
            //string AddressLine1 = ds.Tables[0].Rows[0]["AddressLine1"].ToString();
            //long Con1Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Contact1"].ToString());
            //long Con2Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Contact2"].ToString());
            //long AddressLine1Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Address"].ToString());
            

            xmlString = xmlString + "<templateDetails>";
            
            //xmlString = xmlString + "<contact1>" + Con1 + "</contact1>";
            xmlString = xmlString + "<contact1></contact1>";
            //xmlString = xmlString + "<contact1Id>" + Con1Id + "</contact1Id>";
            xmlString = xmlString + "<contact1Id></contact1Id>";
            //xmlString = xmlString + "<contact2>" + Con2 + "</contact2>";
            xmlString = xmlString + "<contact2></contact2>";
            //xmlString = xmlString + "<contact2Id>" + Con2Id + "</contact2Id>";
            xmlString = xmlString + "<contact2Id></contact2Id>";
            //xmlString = xmlString + "<addressLine1>" + AddressLine1 + "</addressLine1>";
            xmlString = xmlString + "<addressLine1></addressLine1>";
            //xmlString = xmlString + "<addressLine1Id>" + AddressLine1Id + "</addressLine1Id>";
            xmlString = xmlString + "<addressLine1Id></addressLine1Id>";

            xmlString = xmlString + "</templateDetails>";

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