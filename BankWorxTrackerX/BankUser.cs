using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWorxTrackerX
{
    class BankUser
    {
        private static string iD = "";
        public static string ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private static string companyID = "";
        public static string CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private static string windowsID = "";
        public static string WindowsID
        {
            get { return windowsID; }
            set { windowsID = value; }
        }

        private static string domain = "";
        public static string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private static string fullName = "";
        public static string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private static string firstName = "";
        public static string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private static string lastName = "";
        public static string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private static string classID = "";
        public static string ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        private static string email = "";
        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

        private static Boolean active = false;
        public static Boolean Active
        {
            get { return active; }
            set { active = value; }
        }

        private static Boolean online = false;
        public static Boolean Online
        {
            get { return online; }
            set { online = value; }
        }

        private static bool error = false;
        public static bool Error
        {
            get { return error; }
            set { error = value; }
        }

        private static string errorMessage = "";
        public static string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }
}
