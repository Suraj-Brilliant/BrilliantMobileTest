using System;
using System.Collections.Generic;

//namespace PowerOnRentwebapp.Login
namespace BrilliantWMS.Login
{
    [Serializable]
    public class Personal
    {
        /*User details*/
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string UserType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string ProfileImageURL { get; set; }
        public string HeaderMenu { get; set; }
        public string ReportingTo { get; set; }
        public byte[] ProfileImg { get; set; }
        public string Title { get; set; }

        public long DepartmentID { get; set; }
        public string Department { get; set; }

        public long DesignationID { get; set; }
        public string Designation { get; set; }
        public string IPAddress { get; set; }
        public string MachineID { get; set; }

        /*Company Details*/
        public long CompanyID { get; set; }
        public string CName { get; set; }
        public string CLogoURL { get; set; }
        public string CRMUrl { get; set; }

        public long CustomerId { get; set; }

        /*Preferences*/
        public string Theme { get; set; }
        public string TimeZone { get; set; }
        public string DateTime { get; set; }

    }

    [Serializable]
    public class DBConnection
    {
        public string[] _constr = new string[4];
        public string this[int index]
        {
            get
            {
                return _constr[index];
            }
            set
            {
                _constr[index] = value;
            }
        }
    }
}