using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWorxTrackerX.Models
{
    class User
    {
        private string iD = "";
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string companyID = "";
        public string CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        private string windowsID = "";
        public string WindowsID
        {
            get { return windowsID; }
            set { windowsID = value; }
        }

        private string domain = "";
        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        private string fullName = "";
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private string firstName = "";
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName = "";
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string classID = "";
        public string ClassID
        {
            get { return classID; }
            set { classID = value; }
        }

        private string email = "";
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private Boolean active = false;
        public Boolean Active
        {
            get { return active; }
            set { active = value; }
        }

        private Boolean online = false;
        public Boolean Online
        {
            get { return online; }
            set { online = value; }
        }

        private bool error = false;
        public bool Error
        {
            get { return error; }
            set { error = value; }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
    }
}
