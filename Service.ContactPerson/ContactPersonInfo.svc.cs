﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Domain.ContactPerson;

namespace Service.ContactPerson
{    
    public class ContactPersonInfo:Domain.ContactPerson.ContactPersonInfo
    {
        public void DoWork()
        {
        }
       
    }
}
