using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Domain.UserManagement;
namespace Service.UserManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserCreationService" in code, svc and config file together.
    public class UserCreationService : Domain.UserManagement.UserCreation
    {
        public void DoWork()
        {
        }
    }
}
