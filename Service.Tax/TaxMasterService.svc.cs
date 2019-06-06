using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Domain.Tax;

namespace Service.Tax
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TaxMasterService" in code, svc and config file together.
    public class TaxMasterService : Domain.Tax.TaxMaster
    {
        public void DoWork()
        {
        }
    }
}
