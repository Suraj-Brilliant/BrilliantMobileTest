﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Domain.Inventory;

namespace Service.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceMaterialIssue" in code, svc and config file together.
    public class ServiceMaterialIssue : Domain.Inventory.MaterialIssue 
    {
        public void DoWork()
        {
        }
    }
}
