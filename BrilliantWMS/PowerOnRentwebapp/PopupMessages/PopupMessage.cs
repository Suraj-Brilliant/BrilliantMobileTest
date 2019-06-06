using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BrilliantWMS.PopupMessages
{
    public class PopupMessage
    {
        //string header = "ElegantCRM";
        public enum AlertType
        {
            Error,
            confirm,
            modalbox
        }

        public void DisplayPopupMessage(Page CurrentPage, string message, AlertType obj, string header = "GWC")
        {
            if (CurrentPage != null)
            {
                if (obj == AlertType.Error)
                {
                    ScriptManager.RegisterClientScriptBlock(CurrentPage, typeof(Page), "ToggleScript", "dhtmlx.alert({ title:'" + header + "', text:'" + message + "'})", true);
                }
                if (obj == AlertType.confirm)
                {
                    ScriptManager.RegisterClientScriptBlock(CurrentPage, typeof(Page), "ToggleScript", "dhtmlx.confirm({ title:'" + header + "', text:'" + message + "'})", true);
                }
                if (obj == AlertType.modalbox)
                {
                    ScriptManager.RegisterClientScriptBlock(CurrentPage, typeof(Page), "ToggleScript", "dhtmlx.modalbox({ title:'" + header + "', text:'" + message + "'})", true);
                }
            }
        }
    }
}