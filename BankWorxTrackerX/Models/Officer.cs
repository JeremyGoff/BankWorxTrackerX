using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BankWorxTrackerX.Models
{
    [JsonObject]
    [Serializable]

    class Officer
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

        private string initials = "";
        public string Initials
        {
            get { return initials; }
            set { initials = value; }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private Boolean active = false;
        public Boolean Active
        {
            get { return active; }
            set { active = value; }
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
