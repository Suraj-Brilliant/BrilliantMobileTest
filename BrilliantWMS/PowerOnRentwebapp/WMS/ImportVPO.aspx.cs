﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using BrilliantWMS.Login;
using BrilliantWMS.ToolbarService;
using System.Web.Services;
using System.Configuration;
using System.IO;
using System.Data;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Drawing;
using BrilliantWMS.ProductMasterService;
using BrilliantWMS.PORServiceUCCommonFilter;
using BrilliantWMS.PORServicePartRequest;

namespace BrilliantWMS.WMS
{
    public partial class ImportVPO : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Add("Object", "SalesOrder");
        }        
    }
}