using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWorxTrackerX
{
    class Company
    {
        private static string iD = "";
        public static string ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private static string name = "";
        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        private static string code = "";
        public static string Code
        {
            get { return code; }
            set { code = value; }
        }

        private static string logosmall = "";
        public static string Logosmall
        {
            get { return logosmall; }
            set { logosmall = value; }
        }

        private static string logolarge = "";
        public static string Logolarge
        {
            get { return logolarge; }
            set { logolarge = value; }
        }

        private static string dayStartTime = "";
        public static string DayStartTime
        {
            get { return dayStartTime; }
            set { dayStartTime = value; }
        }

        private static string dayEndTime = "";
        public static string DayEndTime
        {
            get { return dayEndTime; }
            set { dayEndTime = value; }
        }

        private static string domain = "";
        public static string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private static string subscription = "";
        public static string Subscription
        {
            get { return subscription; }
            set { subscription = value; }
        }

        private string timeZone = "";
        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
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
