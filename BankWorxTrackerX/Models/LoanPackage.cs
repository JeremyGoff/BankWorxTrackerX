using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWorxTrackerX.Models
{
    class LoanPackage : INotifyPropertyChanged
    {
        private string iD = "";
        public string ID
        {
            get { return iD; }
            set { iD = value;  onPropertyChanged(this, "ID"); }
        }

        private string companyID = "";
        public string CompanyID
        {
            get { return companyID; }
            set { companyID = value;  onPropertyChanged(this, "CompanyID"); }
        }

        private string officerInitials = "";
        public string OfficerInitials
        {
            get { return officerInitials; }
            set { officerInitials = value; onPropertyChanged(this, "OfficerInitials"); }
        }

        private DateTime addedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime AddedDateTime
        {
            get { return addedDateTime; }
            set { addedDateTime = value; }
        }

        //private string addedDateTime = "";
        //public string AddedDateTime
        //{
        //    get { return addedDateTime; }
        //    set { addedDateTime = value;  onPropertyChanged(this, "AddedDateTime"); }
        //}

        private string customer = "";
        public string Customer
        {
            get { return customer; }
            set { customer = value;  onPropertyChanged(this, "Customer"); }
        }

        private string typeDescription = "";
        public string TypeDescription
        {
            get { return typeDescription; }
            set { typeDescription = value;  onPropertyChanged(this, "TypeDescription"); }
        }

        private string typeAddTime = "";
        public string TypeAddTime
        {
            get { return typeAddTime; }
            set { typeAddTime = value;  onPropertyChanged(this, "TypeAddTime"); }
        }

        private string typeCautionTime = "";
        public string TypeCautionTime
        {
            get { return typeCautionTime; }
            set { typeCautionTime = value;  onPropertyChanged(this, "TypeCautionTime"); }
        }

        private DateTime dueDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime DueDateTime
        {
            get { return dueDateTime; }
            set { dueDateTime = value; }
        }

        private string processedByUser = "";
        public string ProcessedByUser
        {
            get { return processedByUser; }
            set { processedByUser = value;  onPropertyChanged(this, "ProcessedByUser"); }
        }

        private string status = "";
        public string Status
        {
            get { return status; }
            set { status = value; onPropertyChanged(this, "Status"); }
        }

        private Boolean closed = false;
        public Boolean Closed
        {
            get { return closed; }
            set { closed = value;  onPropertyChanged(this, "Closed"); }
        }

        private string officerID = "";
        public string OfficerID
        {
            get { return officerID; }
            set { officerID = value;  onPropertyChanged(this, "OfficerID"); }
        }

        private string officerName = "";
        public string OfficerName
        {
            get { return officerName; }
            set { officerName = value;  onPropertyChanged(this, "OfficerName"); }
        }

        private string typeID = "";
        public string TypeID
        {
            get { return typeID; }
            set { typeID = value;  onPropertyChanged(this, "TypeID"); }
        }

        private Decimal amount = 0;
        public Decimal Amount
        {
            get { return amount; }
            set { amount = value;  onPropertyChanged(this, "Amount"); }
        }

        private string statusID = "";
        public string StatusID
        {
            get { return statusID; }
            set { statusID = value;  onPropertyChanged(this, "StatusID"); }
        }

        private string addedByUserID = "";
        public string AddedByUserID
        {
            get { return addedByUserID; }
            set { addedByUserID = value;  onPropertyChanged(this, "AddedByUserID"); }
        }

        private string addedByUser = "";
        public string AddedByUser
        {
            get { return addedByUser; }
            set { addedByUser = value;  onPropertyChanged(this, "AddedByUser"); }
        }

        private string processedByUserID = "";
        public string ProcessedByUserID
        {
            get { return processedByUserID; }
            set { processedByUserID = value;  onPropertyChanged(this, "ProcessedByUserID"); }
        }

        private string reviewedByUserID = "";
        public string ReviewedByUserID
        {
            get { return reviewedByUserID; }
            set { reviewedByUserID = value;  onPropertyChanged(this, "ReviewedByUserID"); }
        }

        private string reviewedByUser = "";
        public string ReviewedByUser
        {
            get { return reviewedByUser; }
            set { reviewedByUser = value;  onPropertyChanged(this, "ReviewedByUser"); }
        }

        private string checkedOutByUserID = "";
        public string CheckedOutByUserID
        {
            get { return checkedOutByUserID; }
            set { checkedOutByUserID = value;  onPropertyChanged(this, "CheckedOutByUserID"); }
        }

        private string checkedOutByUser = "";
        public string CheckedOutByUser
        {
            get { return checkedOutByUser; }
            set { checkedOutByUser = value;  onPropertyChanged(this, "CheckedOutByUser"); }
        }

        private Boolean qC = false;
        public Boolean QC
        {
            get { return qC; }
            set { qC = value;  onPropertyChanged(this, "QC"); }
        }

        private string printer = "";
        public string Printer
        {
            get { return printer; }
            set { printer = value;  onPropertyChanged(this, "Printer"); }
        }


        private DateTime processedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime ProcessedDateTime
        {
            get { return processedDateTime; }
            set { processedDateTime = value; }
        }

        private DateTime reviewedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime ReviewedDateTime
        {
            get { return reviewedDateTime; }
            set { reviewedDateTime = value; }
        }

        private DateTime printedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        public DateTime PrintedDateTime
        {
            get { return printedDateTime; }
            set { printedDateTime = value; }
        }

        private string comments = "";
        public string Comments
        {
            get { return comments; }
            set { comments = value;  onPropertyChanged(this, "Comments"); }
        }

        private string lastUpdatedBy = "";
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; onPropertyChanged(this, "LastUpdatedBy"); }
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

        // Declare the PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // OnPropertyChanged will raise the PropertyChanged event passing the
        // source property that is being updated.
        private void onPropertyChanged(object sender, string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
