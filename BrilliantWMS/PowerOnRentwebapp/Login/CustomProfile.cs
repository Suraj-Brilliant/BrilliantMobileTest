using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
//using PowerOnRentwebapp.Login;
using BrilliantWMS.Login;
//namespace PowerOnRentwebapp.Login
namespace BrilliantWMS.Login
{
    public class CustomProfile : ProfileBase
    {

        public Personal Personal
        {
            get { return (Personal)GetPropertyValue("Personal"); }
        }

        public DBConnection DBConnection
        {
            get { return (DBConnection)GetPropertyValue("DBConnection"); }
        }

        /// <summary>
        /// Get the profile of the currently logged-on user.
        /// </summary>      
        public static CustomProfile GetProfile()
        {
            return (CustomProfile)HttpContext.Current.Profile;
        }

        /// <summary>
        /// Gets the profile of a specific user.
        /// </summary>
        /// <param name="userName">The user name of the user whose profile you want to retrieve.</param>
        public static CustomProfile GetProfile(string userName)
        {
            return (CustomProfile)Create(userName);
        }
    }
}
