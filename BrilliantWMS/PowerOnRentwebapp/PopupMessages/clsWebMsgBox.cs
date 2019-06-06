using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;

/// <summary>
/// Summary description for clsWebMsgBox
/// </summary>
///  

namespace WebMsgBox
{
    public class MsgBox
    {
        protected static Hashtable handlerPages = new Hashtable();
        public MsgBox()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static void Show(string Message)
        {
            if (!(handlerPages.Contains(HttpContext.Current.Handler)))
            {
                Page currentPage = (Page)HttpContext.Current.Handler;
                if (!((currentPage == null)))
                {
                    Queue messageQueue = new Queue();
                    messageQueue.Enqueue(Message);
                    handlerPages.Add(HttpContext.Current.Handler, messageQueue);
                    currentPage.Unload += new EventHandler(CurrentPageUnload);
               }
            }
            else
            {
                Queue queue = ((Queue)(handlerPages[HttpContext.Current.Handler]));
                queue.Enqueue(Message);
            }

        }
        private static void CurrentPageUnload(object sender, EventArgs e)
        {
            Queue queue = ((Queue)(handlerPages[HttpContext.Current.Handler]));
            if (queue != null)
            {
                StringBuilder builder = new StringBuilder();
                int iMsgCount = queue.Count;
                builder.Append("<script language='javascript'>");
                string sMsg;
                while ((iMsgCount > 0))
                {

                    iMsgCount = iMsgCount - 1;
                    sMsg = System.Convert.ToString(queue.Dequeue());
                    sMsg = sMsg.Replace("\"", "'");
                    builder.Append("alert( \"" + sMsg + "\" );");
                }
                builder.Append("</script>");
                handlerPages.Remove(HttpContext.Current.Handler);
                HttpContext.Current.Response.Write(builder.ToString());
            }
       }
    }
}
