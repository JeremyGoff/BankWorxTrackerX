using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWorxTrackerX.Models
{
    class Type
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

        private string description = "";
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private Decimal qCAmount = 0;
        public Decimal QCAmount
        {
            get { return qCAmount; }
            set { qCAmount = value; qCAmountFormatted = qCAmount.ToString("C2"); }
        }

        private string qCAmountFormatted = "";
        public string QCAmountFormatted
        {
            get { return qCAmountFormatted; }
            set { qCAmountFormatted = value; }
        }

        private string addTime = "";
        public string AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

        private string cautionTime = "";
        public string CautionTime
        {
            get { return cautionTime; }
            set { cautionTime = value; }
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
