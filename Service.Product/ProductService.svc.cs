﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Domain.Product;

namespace Service.Product
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductService" in code, svc and config file together.
    public class ProductService : Domain.Product.ProductMaster
    {
        public void DoWork()
        {
        }
    }
}
