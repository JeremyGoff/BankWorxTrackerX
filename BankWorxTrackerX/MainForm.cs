using BankWorxTrackerX.Models;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.ServiceBus;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Threading;

namespace BankWorxTrackerX
{
    public partial class MainForm : Form
    {
        private static BindingList<LoanPackage> loanPackageList = new BindingList<LoanPackage>();
        private List<User> arrivalUserList = new List<User>();
        private List<User> processedUserList = new List<User>();
        private List<User> reviewedUserList = new List<User>();
        private List<Officer> officerList = new List<Officer>();
        private List<Models.Type> typeList = new List<Models.Type>();
        private List<Models.Type> filterTypeList = new List<Models.Type>();
        private LoanPackage selectedLoanPackage = new LoanPackage();
        private ContextMenu loanPackageListPopupMenu = new ContextMenu();
        private Models.Type selectedType = new Models.Type();
        int closingHour = 0;
        int closingMinute = 0;
        int openingHour = 0;
        int openingMinute = 0;
        int addTimeHour = 0;
        int addTimeMinute = 0;
        int cautionTimeHour = 0;
        int cautionTimeMinute = 0;
        bool validNumericKey = false;
        bool pageLoading = true;
        DateTime systemDateTime;

        public MainForm()
        {
            InitializeComponent();
        }

        //************************************************************************************
        //Form Load Method
        //************************************************************************************
        private void MainForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            SetBankTimeFields();

            txtAmount.DataBindings.Add("Text", selectedLoanPackage, "Amount", true, DataSourceUpdateMode.OnValidation, 0, "C2");

            lblUserID.Text = BankUser.FullName;
            this.Text = Company.Name;

            LoadUsers();
            LoadOfficers();
            LoadTypes();
            LoadPopupMenu();

            ListenForMessages();

            Cursor = Cursors.Default;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            LoadLoanPackages();

            ClearLoanPackageFields();

            pageLoading = false;

            Cursor = Cursors.Default;
        }

        //************************************************************************************
        //Other Methods
        //************************************************************************************
        private async void ListenForMessages()
        {
            SubscriptionClient Client = null;
            bool userSubscription = true;

            OnMessageOptions options;
            //string connectionString = "Endpoint=sb://bankworxtrackerx.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=nlL7NCGsguXyfsfZWlR/FKW3g24QFew3scJQOFvz3c0=";
            string connectionString = ConfigurationManager.AppSettings["AzureServiceBusConnectionString"];

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            userSubscription = await namespaceManager.SubscriptionExistsAsync(Company.Subscription.Trim(), BankUser.WindowsID.Trim());

            if (!userSubscription)
            {
                TopicDescription dataCollectionTopic = namespaceManager.GetTopic(Company.Subscription.Trim());
                namespaceManager.CreateSubscription(dataCollectionTopic.Path, BankUser.WindowsID.Trim(), new SqlFilter("UserID = '" + BankUser.ID.Trim() + "'"));
            }

            Client = SubscriptionClient.CreateFromConnectionString(connectionString, Company.Subscription.Trim(), BankUser.WindowsID.Trim());

            options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);
            options.ExceptionReceived += OnMessageError;
            Client.OnMessageAsync(OnMessageReceived, options);
        }

        private static void OnMessageError(object sender, ExceptionReceivedEventArgs e)
        {
            if (e != null && e.Exception != null)
            {
               MessageBox.Show("Message Error:" + e.Exception.Message + "\r\n\r\n", "Message Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task OnMessageReceived(BrokeredMessage message)
        {
            await message.CompleteAsync();

            LoanPackage loan = new LoanPackage();

            var data = message.GetBody<Stream>();
            byte[] buff = new byte[data.Length];
            data.Read(buff, 0, (int)data.Length);
            var anySerializableObject = Encoding.UTF8.GetString(buff);
            loan = JsonConvert.DeserializeObject<LoanPackage>(anySerializableObject);

            (Application.OpenForms[2] as MainForm).RefreshLoanPackageListFromMessage();

            //if (message.Properties["Type"].ToString().Equals("Post"))
            //{
            //    //(Application.OpenForms[1] as MainForm).lblMessgeUpdate.Text = "A New Loan Package has been Created.\nList needs to be Refreshed.";
            //    (Application.OpenForms[1] as MainForm).RefreshLoanPackageListFromMessage();
            //}
            //else if (message.Properties["Type"].ToString().Equals("Put"))
            //{
            //    (Application.OpenForms[1] as MainForm).RefreshLoanPackageListFromMessage();
            //    ////Update Loan Package List
            //    //for (int x = 0; x < loanPackageList.Count(); x++)
            //    //{
            //    //    if (loanPackageList[x].ID.Equals(loan.ID))
            //    //    {
            //    //        loanPackageList[x].OfficerInitials = loan.OfficerInitials;
            //    //        loanPackageList[x].Customer = loan.Customer;
            //    //        loanPackageList[x].ID = loan.ID;
            //    //        loanPackageList[x].CompanyID = loan.CompanyID;
            //    //        loanPackageList[x].Customer = loan.Customer;
            //    //        loanPackageList[x].OfficerID = loan.OfficerID;
            //    //        loanPackageList[x].TypeID = loan.TypeID;
            //    //        loanPackageList[x].TypeDescription = loan.TypeDescription;
            //    //        loanPackageList[x].Amount = loan.Amount;
            //    //        loanPackageList[x].StatusID = loan.StatusID;
            //    //        loanPackageList[x].AddedByUserID = loan.AddedByUserID;
            //    //        loanPackageList[x].ProcessedByUserID = loan.ProcessedByUserID;
            //    //        loanPackageList[x].ReviewedByUserID = loan.ReviewedByUserID;
            //    //        loanPackageList[x].CheckedOutByUserID = loan.CheckedOutByUserID;
            //    //        loanPackageList[x].CheckedOutByUser = loan.CheckedOutByUser;
            //    //        loanPackageList[x].QC = loan.QC;
            //    //        loanPackageList[x].Printer = loan.Printer;
            //    //        loanPackageList[x].AddedDateTime = loan.AddedDateTime;
            //    //        loanPackageList[x].ProcessedDateTime = loan.ProcessedDateTime;
            //    //        loanPackageList[x].ReviewedDateTime = loan.ReviewedDateTime;
            //    //        loanPackageList[x].PrintedDateTime = loan.PrintedDateTime;
            //    //        loanPackageList[x].DueDateTime = loan.DueDateTime;
            //    //        loanPackageList[x].Comments = loan.Comments;
            //    //        loanPackageList[x].Status = loan.Status;
            //    //        loanPackageList[x].Closed = loan.Closed;

            //    //        x = loanPackageList.Count() + 1;
            //    //    }
            //    //}

            //    //(Application.OpenForms[1] as MainForm).dgLoanPackages.DataSource = loanPackageList;
            //    //(Application.OpenForms[1] as MainForm).SetLoanPackageListColor();
            //}
            //else if (message.Properties["Type"].ToString().Equals("Delete"))
            //{
            //    //(Application.OpenForms[1] as MainForm).lblMessgeUpdate.Text = "A Loan Package has been Deleted.\nList needs to be Refreshed.";
            //    (Application.OpenForms[1] as MainForm).RefreshLoanPackageListFromMessage();
            //}
            //else
            //{ }
        }

        private void RefreshLoanPackageListFromMessage()
        {
            Invoke(new Action(() =>
            {
                LoadLoanPackages();
            }));
        }

        private async void SetBankTimeFields()
        {
            String[] dayEndTime = Company.DayEndTime.Split(':');
            String[] dayStartTime = Company.DayStartTime.Split(':');

            closingHour = Int32.Parse(dayEndTime[0]);
            closingMinute = Int32.Parse(dayEndTime[1]);
            openingHour = Int32.Parse(dayStartTime[0]);
            openingMinute = Int32.Parse(dayStartTime[1]);

            systemDateTime = await GetCurrentDateTime();
            systemTimer.Start();
        }

        private void LoadPopupMenu()
        {
            loanPackageListPopupMenu.MenuItems.Add("Check Out for Processing", new EventHandler(CheckOutForProcessing_OnClick));
            loanPackageListPopupMenu.MenuItems.Add("Check In – Processed", new EventHandler(CheckInAsProcessed_OnClick));
            loanPackageListPopupMenu.MenuItems.Add("-");
            loanPackageListPopupMenu.MenuItems.Add("Check Out for Review", new EventHandler(CheckOutForReview_OnClick));
            loanPackageListPopupMenu.MenuItems.Add("Check In – Reviewed", new EventHandler(CheckInAsReviewed_OnClick));
            loanPackageListPopupMenu.MenuItems.Add("-");
            loanPackageListPopupMenu.MenuItems.Add("Check Out for QC", new EventHandler(CheckOutForQC_OnClick));
            loanPackageListPopupMenu.MenuItems.Add("Check In – QC", new EventHandler(CheckInAsQC_OnClick));
        }

        protected void CheckOutForProcessing_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            selectedLoanPackage.Status = "New";

            if (selectedLoanPackage.ProcessedByUserID.Equals("0"))
            {
                selectedLoanPackage.ProcessedByUserID = "0";
                selectedLoanPackage.ProcessedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);//.ToString();
            }

            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        protected void CheckInAsProcessed_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //TimeZoneInfo bankTimeZone;
            //bankTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            selectedLoanPackage.Status = "In Processing";
            selectedLoanPackage.ProcessedByUserID = BankUser.ID;
            selectedLoanPackage.ProcessedDateTime = systemDateTime;//await GetCurrentDateTime();//DateTime.Now;

            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        protected void CheckOutForReview_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            selectedLoanPackage.Status = "Processed";
            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        protected void CheckInAsReviewed_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            selectedLoanPackage.Status = "In Review";
            selectedLoanPackage.ReviewedByUser = BankUser.ID;
            selectedLoanPackage.ReviewedDateTime = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;

            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        protected void CheckOutForQC_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            selectedLoanPackage.Status = "Reviewed";
            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        protected void CheckInAsQC_OnClick(System.Object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            selectedLoanPackage.Status = "In QC";
            ChangeLoanPackageStatus();

            Cursor = Cursors.Default;
        }

        private void mainTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabCtrl = (TabControl)sender;
            Brush fontBrush = Brushes.Black;
            string title = tabCtrl.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            int indent = 3;
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y + indent, e.Bounds.Width, e.Bounds.Height - indent);

            if (e.Index == mainTabControl.SelectedIndex)
            {
                e.Graphics.DrawString(mainTabControl.TabPages[e.Index].Text,
                    new System.Drawing.Font(mainTabControl.Font, FontStyle.Bold),
                    Brushes.Black,
                    new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
            }
            else
            {
                e.Graphics.DrawString(mainTabControl.TabPages[e.Index].Text,
                    mainTabControl.Font,
                    Brushes.Black,
                    new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
            }

            if (tabCtrl.TabPages[e.Index].ImageIndex >= 0)
            {
                Image img = tabCtrl.ImageList.Images[tabCtrl.TabPages[e.Index].ImageIndex];
                float _x = (rect.X + rect.Width) - img.Width - indent;
                float _y = ((rect.Height - img.Height) / 2.0f) + rect.Y;
                e.Graphics.DrawImage(img, _x, _y);
            }

        }

        public void LoadLoanPackages()
        {
            string url = "";
            string filter = "LoanPackage.CompanyID=" + Company.ID.Trim();

            if (chkShowAll.Checked == false)
            {
                filter = filter + " AND LoanPackage.Closed = 0";
            }

            if (cboFilterStatus.SelectedIndex > -1)
            {
                filter = filter + " AND LoanPackage.Status = '" + cboFilterStatus.SelectedItem.ToString().Trim() + "'";
            }

            if (cboFilterType.SelectedIndex > -1)
            {
                filter = filter + " AND LoanPackage.TypeID = " + (cboFilterType.SelectedItem as Models.Type).ID;
            }

            url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/loanpackage/getloanpackagebyfilter/" + filter.Trim());

            loanPackageList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    loanPackageList = JsonConvert.DeserializeObject<BindingList<LoanPackage>>(jsonString.Result);
                });

            task.Wait();

            dgLoanPackages.DataSource = loanPackageList;

            foreach (DataGridViewColumn column in dgLoanPackages.Columns)
            {
                switch (column.HeaderText)
                {
                    case "ID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "CompanyID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Customer":
                        {
                            column.Width = 200;
                            break;
                        }
                    case "OfficerID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "OfficerInitials":
                        {
                            column.Width = 100;
                            column.HeaderText = "Officer";
                            break;
                        }
                    case "OfficerName":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "TypeID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "TypeDescription":
                        {
                            column.Width = 125;
                            column.HeaderText = "Type";
                            break;
                        }
                    case "TypeAddTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "TypeCautionTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Amount":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "StatusID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "AddedByUserID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "AddedByUser":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ProcessedByUserID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ProcessedByUser":
                        {
                            column.Width = 200;
                            column.HeaderText = "Processor";
                            break;
                        }
                    case "ReviewedByUserID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ReviewedByUser":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "CheckedOutByUserID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "CheckedOutByUser":
                        {
                            column.Width = 200;
                            column.HeaderText = "Checked-Out By";
                            break;
                        }
                    case "QC":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Printer":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "AddedDateTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ProcessedDateTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ReviewedDateTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "PrintedDateTime":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "DueDateTime":
                        {
                            column.HeaderText = "Due Date";
                            column.Width = 175;
                            break;
                        }
                    case "Comments":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Status":
                        {
                            column.Width = 150;
                            break;
                        }
                    case "Closed":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "LastUpdatedBy":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Error":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ErrorMessage":
                        {
                            column.Visible = false;
                            break;
                        }
                }
            }

            SetLoanPackageListColor();
        }

        private void LoadUsers()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/getactivebycompanyid/" + Company.ID.Trim());

            arrivalUserList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    arrivalUserList = JsonConvert.DeserializeObject<List<User>>(jsonString.Result);
                });

            task.Wait();

            foreach (User usr in arrivalUserList)
            {
                processedUserList.Add(usr);
                reviewedUserList.Add(usr);
            }

            cboArrivalUser.DataSource = arrivalUserList;
            cboArrivalUser.DisplayMember = "FullName";
            cboArrivalUser.ValueMember = "ID";
            cboArrivalUser.SelectedIndex = -1;

            cboProcessedUser.DataSource = processedUserList;
            cboProcessedUser.DisplayMember = "FullName";
            cboProcessedUser.ValueMember = "ID";
            cboProcessedUser.SelectedIndex = -1;

            cboReviewedUser.DataSource = reviewedUserList;
            cboReviewedUser.DisplayMember = "FullName";
            cboReviewedUser.ValueMember = "ID";
            cboReviewedUser.SelectedIndex = -1;
        }

        private void LoadOfficers()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/officer/getofficersbycompanyID/" + Company.ID.Trim());
            officerList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    officerList = JsonConvert.DeserializeObject<List<Officer>>(jsonString.Result);
                });

            task.Wait();

            cboOfficer.DataSource = officerList;
            cboOfficer.DisplayMember = "Name";
            cboOfficer.ValueMember = "ID";
            cboOfficer.SelectedIndex = -1;
        }

        private void LoadTypes()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/type/gettypesbycompanyid/" + Company.ID.Trim());

            typeList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    typeList = JsonConvert.DeserializeObject<List<Models.Type>>(jsonString.Result);
                });

            task.Wait();

            foreach (Models.Type type in typeList)
            {
                filterTypeList.Add(type);
            }

            cboType.DataSource = typeList;
            cboType.DisplayMember = "Description";
            cboType.ValueMember = "ID";
            cboType.SelectedIndex = -1;

            cboFilterType.DataSource = filterTypeList;
            cboFilterType.DisplayMember = "Description";
            cboFilterType.ValueMember = "ID";
            cboFilterType.SelectedIndex = -1;
        }

        private bool ValidLoanPackageFields()
        {
            bool valid = true;
            String errorMessage = "";

            if (txtCustomer.Text.Equals(""))
            {
                errorMessage = errorMessage + "Customer cannot be blank.\n";
                valid = false;
            }
            if (cboArrivalUser.SelectedIndex == -1)
            {
                errorMessage = errorMessage + "Please select an Arrival User.\n";
                valid = false;
            }
            if (cboOfficer.SelectedIndex == -1)
            {
                errorMessage = errorMessage + "Please select an Officer.\n";
                valid = false;
            }
            if (cboType.SelectedIndex == -1)
            {
                errorMessage = errorMessage + "Please select an Type.\n";
                valid = false;
            }

            if (valid == false)
            {
                MessageBox.Show(errorMessage, "Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return valid;
        }

        private void ClearLoanPackageFields()
        {
            selectedLoanPackage = new LoanPackage();
            selectedLoanPackage.ID = "0";
            selectedLoanPackage.LastUpdatedBy = BankUser.ID;

            btnProcessingSave.Enabled = true;
            btnProcessingSave.Image = Properties.Resources.Save_32x32;

            cboArrivalUser.SelectedIndex = -1;
            cboArrivalUser.SelectedText = "";
            cboArrivalUser.SelectedValue = "";
            cboArrivalUser.SelectedItem = null;

            cboProcessedUser.SelectedIndex = -1;
            cboProcessedUser.SelectedText = "";
            cboProcessedUser.SelectedValue = "";
            cboProcessedUser.SelectedItem = null;

            cboReviewedUser.SelectedIndex = -1;
            cboReviewedUser.SelectedText = "";
            cboReviewedUser.SelectedValue = "";
            cboReviewedUser.SelectedItem = null;

            cboOfficer.SelectedIndex = -1;
            cboOfficer.SelectedText = "";
            cboOfficer.SelectedValue = "";
            cboOfficer.SelectedItem = null;

            cboType.SelectedIndex = -1;
            cboType.SelectedText = "";
            cboType.SelectedValue = "";
            cboType.SelectedItem = null;

            cboStatus.SelectedItem = 0;

            txtCustomer.Text = "";
            txtPrinter.Text = "";
            txtAmount.Text = "$0.00";
            txtComment.Text = "";

            lblDueDate.Visible = false;
            dtDueDate.Visible = false;
            dtDueTime.Visible = false;
            txtDueDateHighlight.Visible = false;

            dtArrivalDate.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
            dtArrivalTime.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
            dtProcessedDate.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtProcessedTime.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtReviewedDate.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtReviewedTime.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtDueDate.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtDueTime.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtPrintedDate.Value = new DateTime(1900, 1, 1, 0, 0, 0);
            dtPrintedTime.Value = new DateTime(1900, 1, 1, 0, 0, 0);

            dtProcessedDate.Visible = false;
            dtProcessedTime.Visible = false;
            dtReviewedDate.Visible = false;
            dtReviewedTime.Visible = false;
            dtDueDate.Visible = false;
            dtDueTime.Visible = false;
            dtPrintedDate.Visible = false;
            dtPrintedTime.Visible = false;
            txtPrinter.Visible = false;
            lblPrinter.Visible = false;

            imgProcessedClear.Visible = false;
            imgReviewedClear.Visible = false;
            imgPrintedClear.Visible = false;

            imgLoanPackageEdit.Image = Properties.Resources.Edit_Disabled;
            imgLoanPackageEdit.Enabled = false;
            imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
            imgLoanPackageDelete.Enabled = false;
            btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
            btnProcessingCheckIn.Enabled = false;
            btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
            btnProcessingCheckOut.Enabled = false;
            btnProcessingComplete.Text = "Complete";
            toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");
            btnProcessingComplete.Image = Properties.Resources.LockDisabled_32x32;
            btnProcessingComplete.Enabled = false;

            txtCustomer.Focus();
        }

        private bool CreateNewLoanPackage(LoanPackage newLoanPackage)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/loanpackage");
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(newLoanPackage);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var task = client.PostAsync(url, contentPost)
                .ContinueWith((taskwithresponse) =>
                {
                    try
                    {
                        taskwithresponse.Result.EnsureSuccessStatusCode();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        MessageBox.Show("Error Found Adding New Loan Package:\n\n" + ex.Message.Trim(), "New Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool UpdateLoanPackage(LoanPackage updateLoanPackage)
        {
            //string url = String.Format("http://bankworxtrackerxservice.azurewebsites.net/api/loanpackage/" + updateLoanPackage.ID.Trim());
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/loanpackage/" + updateLoanPackage.ID.Trim());
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(updateLoanPackage);
            HttpContent contentPut = new StringContent(param, Encoding.UTF8, "application/json");

            var task = client.PutAsync(url, contentPut)
                .ContinueWith((taskwithresponse) =>
                {
                    try
                    {
                        taskwithresponse.Result.EnsureSuccessStatusCode();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        MessageBox.Show("Error Found Updating Loan Package:\n\n" + ex.Message.Trim(), "Update Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private void SetLoanPackageListColor()
        {
            foreach (DataGridViewRow row in dgLoanPackages.Rows)
            {
                LoanPackage loan = new LoanPackage();
                loan = row.DataBoundItem as LoanPackage;
               
                String[] dateTimeTOD = loan.DueDateTime.ToString().Split(' ');
                String[] lmmddyyyy = dateTimeTOD[0].Split('/');
                String[] lhhmmss = dateTimeTOD[1].Split(':'); 

                DateTime dueDate;
                if (dateTimeTOD[2].Equals("AM"))
                {
                    dueDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
                }
                else
                {
                    if (lhhmmss[0].Equals("12"))
                    {
                        dueDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 12, Int32.Parse(lhhmmss[1]), 0);
                    }
                    else
                    {
                        dueDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
                    }
                }

                TimeSpan dueDateTime = dueDate.TimeOfDay;
                String[] typeCautionTime = loan.TypeCautionTime.Split(':');
                cautionTimeHour = Int32.Parse(typeCautionTime[0]);
                cautionTimeMinute = Int32.Parse(typeCautionTime[1]);

                TimeSpan cautionTime = new TimeSpan(cautionTimeHour, cautionTimeMinute, 0);
                TimeSpan calculatedTime = dueDateTime.Subtract(cautionTime);
                DateTime cautionTimeDate = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, calculatedTime.Hours, calculatedTime.Minutes, 0);

                row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
                row.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

                if (loan.Closed)
                {
                    row.DefaultCellStyle.Font = new System.Drawing.Font(new System.Drawing.FontFamily("Calibri"), 11.25F, FontStyle.Strikeout);

                    row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
                else if (dueDate.CompareTo(systemDateTime) == -1) 
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Red; //System.Drawing.Color.FromArgb(255, 223, 223);  //System.Drawing.Color.Red;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
                else if (systemDateTime.CompareTo(cautionTimeDate) > 0)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange; //System.Drawing.Color.FromArgb(255, 230, 204); //System.Drawing.Color.Orange;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
                }
            }
        }

        private void SortLoanPackage(string column, SortOrder sortOrder)
        {
            switch (column)
            {
                case "OfficerInitials":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.OfficerInitials).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.OfficerInitials).ToList();
                        }
                        break;
                    }
                case "Customer":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.Customer).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.Customer).ToList();
                        }
                        break;
                    }
                case "TypeDescription":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.TypeDescription).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.TypeDescription).ToList();
                        }
                        break;
                    }
                case "DueDateTime":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.DueDateTime).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.DueDateTime).ToList();
                        }
                        break;
                    }
                case "ProcessedByUser":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.ProcessedByUser).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.ProcessedByUser).ToList();
                        }
                        break;
                    }
                case "Status":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.Status).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.Status).ToList();
                        }
                        break;
                    }
                case "CheckedOutByUser":
                    {
                        if (sortOrder == SortOrder.Ascending)
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => x.CheckedOutByUser).ToList();
                        }
                        else
                        {
                            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => x.CheckedOutByUser).ToList();
                        }
                        break;
                    }

                    //case "Customer":
                    //    {
                    //        if (sortOrder == SortOrder.Ascending)
                    //        {
                    //            dgLoanPackages.DataSource = loanPackageList.OrderBy(x => Decimal.Parse(x.BudgetAmount.Trim().Replace("$", "").Replace(",", "").Replace("(", "-").Replace(")", ""))).ToList();
                    //        }
                    //        else
                    //        {
                    //            dgLoanPackages.DataSource = loanPackageList.OrderByDescending(x => Decimal.Parse(x.BudgetAmount.Trim().Replace("$", "").Replace(",", "").Replace("(", "-").Replace(")", ""))).ToList();
                    //        }
                    //        break;
                    //    }
            }

            SetLoanPackageListColor();
        }

        private DateTime convertToDate(string dateString)
        {
            DateTime returnDate;

            String[] requestMMDDYY = new String[3];
            requestMMDDYY = dateString.Split(new Char[] { '/' });
            String[] requestHHMMSSTemp = new String[3];
            requestHHMMSSTemp = dateString.Split(new Char[] { ' ' });

            if (dateString.Contains(':'))
            {
                String[] requestHHMMSS = new String[3];
                requestHHMMSS = requestHHMMSSTemp[1].Split(new Char[] { ':' });

                returnDate = new DateTime(Int32.Parse(requestMMDDYY[2].Substring(0, 4)), Int32.Parse(requestMMDDYY[0]), Int32.Parse(requestMMDDYY[1]), Int32.Parse(requestHHMMSS[0]), Int32.Parse(requestHHMMSS[1]), Int32.Parse(requestHHMMSS[2]));
            }
            else
            {
                returnDate = new DateTime(Int32.Parse(requestMMDDYY[2].Substring(0, 4)), Int32.Parse(requestMMDDYY[0]), Int32.Parse(requestMMDDYY[1]));
            }

            return returnDate;
        }

        private void SetLoanPackageFields()
        {
            pageLoading = true;

            switch (selectedLoanPackage.Status.Trim())
            {
                case ("New"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 0;
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
                        btnProcessingCheckIn.Enabled = false;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.LockDisabled_32x32;
                        btnProcessingComplete.Enabled = false;
                        break;
                    }
                case ("In Processing"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 1;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreen_32x32;
                        btnProcessingCheckIn.Enabled = true;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("Processed"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 2;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
                        btnProcessingCheckIn.Enabled = false;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRed_32x32;
                        btnProcessingCheckOut.Enabled = true;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("In Review"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 3;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreen_32x32;
                        btnProcessingCheckIn.Enabled = true;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("Reviewed"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 4;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
                        btnProcessingCheckIn.Enabled = false;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRed_32x32;
                        btnProcessingCheckOut.Enabled = true;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("In QC"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 5;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreen_32x32;
                        btnProcessingCheckIn.Enabled = true;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("QC’d"):
                    {
                        cboStatus.Enabled = true;
                        cboStatus.SelectedIndex = 6;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.Save_32x32;
                        btnProcessingSave.Enabled = true;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
                        btnProcessingCheckIn.Enabled = false;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.Lock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
                case ("Complete"):
                    {
                        cboStatus.SelectedIndex = -1;
                        cboStatus.Text = "Complete";
                        cboStatus.Enabled = false;
                        imgLoanPackageDelete.Image = Properties.Resources.Close_Disabled;
                        imgLoanPackageDelete.Enabled = false;
                        btnProcessingSave.Image = Properties.Resources.SaveDisabled_32x32;
                        btnProcessingSave.Enabled = false;
                        btnProcessingCheckIn.Image = Properties.Resources.CheckInGreenDisabled_32x32;
                        btnProcessingCheckIn.Enabled = false;
                        btnProcessingCheckOut.Image = Properties.Resources.CheckOutRedDisabled_32x32;
                        btnProcessingCheckOut.Enabled = false;
                        btnProcessingComplete.Image = Properties.Resources.UnLock_32x32;
                        btnProcessingComplete.Enabled = true;
                        break;
                    }
            }

            if (selectedLoanPackage.AddedByUserID.Equals(""))
            {
                cboArrivalUser.SelectedIndex = -1;
            }
            else
            {
                for (int x=0; x<arrivalUserList.Count; x++)
                {
                    if (arrivalUserList[x].ID.Equals(selectedLoanPackage.AddedByUserID))
                    {
                        cboArrivalUser.SelectedIndex = x;
                        x = arrivalUserList.Count + 1;
                    }
                }
            }

            if (selectedLoanPackage.ProcessedByUserID.Equals(""))
            {
                cboProcessedUser.SelectedIndex = -1;
            }
            else
            {
                for (int x = 0; x < processedUserList.Count; x++)
                {
                    if (processedUserList[x].ID.Equals(selectedLoanPackage.ProcessedByUserID))
                    {
                        cboProcessedUser.SelectedIndex = x;
                        x = processedUserList.Count + 1;
                    }
                }
            }

            if (selectedLoanPackage.ReviewedByUserID.Equals(""))
            {
                cboReviewedUser.SelectedIndex = -1;
            }
            else
            {
                for (int x = 0; x < reviewedUserList.Count; x++)
                {
                    if (reviewedUserList[x].ID.Equals(selectedLoanPackage.ReviewedByUserID))
                    {
                        cboReviewedUser.SelectedIndex = x;
                        x = reviewedUserList.Count + 1;
                    }
                }
            }

            if (selectedLoanPackage.OfficerID.Equals(""))
            {
                cboOfficer.SelectedIndex = -1;
            }
            else
            {
                for (int x = 0; x < officerList.Count; x++)
                {
                    if (officerList[x].ID.Equals(selectedLoanPackage.OfficerID))
                    {
                        cboOfficer.SelectedIndex = x;
                        x = officerList.Count + 1;
                    }
                }
            }

            if (selectedLoanPackage.TypeID.Equals(""))
            {
                cboType.SelectedIndex = -1;
            }
            else
            {
                for (int x = 0; x < typeList.Count; x++)
                {
                    if (typeList[x].ID.Equals(selectedLoanPackage.TypeID))
                    {
                        cboType.SelectedIndex = x;
                        x = typeList.Count + 1;
                    }
                }
            }

            dtArrivalDate.Value = selectedLoanPackage.AddedDateTime;// arriveDate;
            dtArrivalTime.Value = selectedLoanPackage.AddedDateTime;// arriveDate;
            dtProcessedDate.Value = selectedLoanPackage.ProcessedDateTime;
            dtProcessedTime.Value = selectedLoanPackage.ProcessedDateTime;
            dtReviewedDate.Value = selectedLoanPackage.ReviewedDateTime;
            dtReviewedTime.Value = selectedLoanPackage.ReviewedDateTime;
            dtDueDate.Value = selectedLoanPackage.DueDateTime;
            dtDueTime.Value = selectedLoanPackage.DueDateTime;
            dtPrintedDate.Value = selectedLoanPackage.PrintedDateTime;
            dtPrintedTime.Value = selectedLoanPackage.PrintedDateTime;
            lblDueDate.Visible = true;
            dtDueDate.Visible = true;
            dtDueTime.Visible = true;

            if (selectedLoanPackage.ProcessedDateTime.Year.Equals(1900))
            {
                dtProcessedDate.Visible = false;
                dtProcessedTime.Visible = false;
                imgProcessedClear.Visible = false;
            }
            else
            {
                dtProcessedDate.Visible = true;
                dtProcessedTime.Visible = true;
                imgProcessedClear.Visible = true;
            }

            if (selectedLoanPackage.ReviewedDateTime.Year.Equals(1900))
            {
                dtReviewedDate.Visible = false;
                dtReviewedTime.Visible = false;
                imgReviewedClear.Visible = false;
            }
            else
            {
                dtReviewedDate.Visible = true;
                dtReviewedTime.Visible = true;
                imgReviewedClear.Visible = true;
            }

            if (selectedLoanPackage.PrintedDateTime.Year.Equals(1900))
            {
                dtPrintedDate.Visible = false;
                dtPrintedTime.Visible = false;
                imgPrintedClear.Visible = false;
                txtPrinter.Visible = false;
                lblPrinter.Visible = false;
            }
            else
            {
                dtPrintedDate.Visible = true;
                dtPrintedTime.Visible = true;
                imgPrintedClear.Visible = true;
                txtPrinter.Visible = true;
                lblPrinter.Visible = true;
            }

            //Set Arrive Date Fields
            //String[] dateTimeTOD = selectedLoanPackage.AddedDateTime.Split(' ');
            //String[] lmmddyyyy = dateTimeTOD[0].Split('/');
            //String[] lhhmmss = dateTimeTOD[1].Split(':');

            //DateTime arriveDate;
            //if (dateTimeTOD[2].Equals("AM"))
            //{
            //    arriveDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
            //}
            //else
            //{
            //    if (lhhmmss[0].Equals("12"))
            //    {
            //        arriveDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 0, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //    else
            //    {
            //        arriveDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //}

            //Set Processed Date Fields
            //String[] dateTimeTOD = selectedLoanPackage.ProcessedDateTime.ToString().Split(' ');
            //String[] lmmddyyyy = dateTimeTOD[0].Split('/');
            //String[] lhhmmss = dateTimeTOD[1].Split(':');

            //DateTime processedDate;

            //if (dateTimeTOD[2].Equals("AM"))
            //{
            //    processedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
            //}
            //else
            //{
            //    if (lhhmmss[0].Equals("12"))
            //    {
            //        processedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 0, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //    else
            //    {
            //        processedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //}

            //if (lmmddyyyy[2].Equals("1900"))
            //{
            //    dtProcessedDate.Value = DateTime.Now;
            //    dtProcessedTime.Value = DateTime.Now;
            //    dtProcessedDate.Visible = false;
            //    dtProcessedTime.Visible = false;
            //    imgProcessedClear.Visible = false;
            //}
            //else
            //{
            //    dtProcessedDate.Value = processedDate;
            //    dtProcessedTime.Value = processedDate;
            //    dtProcessedDate.Visible = true;
            //    dtProcessedTime.Visible = true;
            //    imgProcessedClear.Visible = true;
            //}

            ////Set Reviewed Date Fields
            //dateTimeTOD = selectedLoanPackage.ReviewedDateTime.ToString().Split(' ');
            //lmmddyyyy = dateTimeTOD[0].Split('/');
            //lhhmmss = dateTimeTOD[1].Split(':');

            //DateTime reviewedDate;

            //if (dateTimeTOD[2].Equals("AM"))
            //{
            //    reviewedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
            //}
            //else
            //{
            //    if (lhhmmss[0].Equals("12"))
            //    {
            //        reviewedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 0, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //    else
            //    {
            //        reviewedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //}

            //if (lmmddyyyy[2].Equals("1900"))
            //{
            //    dtReviewedDate.Value = DateTime.Now;
            //    dtReviewedTime.Value = DateTime.Now;
            //    dtReviewedDate.Visible = false;
            //    dtReviewedTime.Visible = false;
            //    imgReviewedClear.Visible = false;
            //}
            //else
            //{
            //    dtReviewedDate.Value = reviewedDate;
            //    dtReviewedTime.Value = reviewedDate;
            //    dtReviewedDate.Visible = true;
            //    dtReviewedTime.Visible = true;
            //    imgReviewedClear.Visible = true;
            //}

            ////Set Due Date Fields
            //dateTimeTOD = selectedLoanPackage.DueDateTime.ToString().Split(' ');
            //lmmddyyyy = dateTimeTOD[0].Split('/');
            //lhhmmss = dateTimeTOD[1].Split(':');

            //DateTime duedDate;

            //if (dateTimeTOD[2].Equals("AM"))
            //{
            //    duedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
            //}
            //else
            //{
            //    if (lhhmmss[0].Equals("12"))
            //    {
            //        duedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 0, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //    else
            //    {
            //        duedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
            //    }
            //}

            //if (lmmddyyyy[2].Equals("1900"))
            //{
            //    dtDueDate.Value = DateTime.Now;
            //    dtDueTime.Value = DateTime.Now;
            //    dtDueDate.Visible = false;
            //    dtDueTime.Visible = false;
            //}
            //else
            //{
            //    dtDueDate.Value = duedDate;
            //    dtDueTime.Value = duedDate;
            //    dtDueDate.Visible = true;
            //    dtDueTime.Visible = true;
            //}

            ////Set Sent/Completed Date Fields
            //if (selectedLoanPackage.PrintedDateTime.Equals(""))
            //{
            //    dateTimeTOD = selectedLoanPackage.PrintedDateTime.ToString().Split(' ');

            //    dtPrintedDate.Value = DateTime.Now;
            //    dtPrintedTime.Value = DateTime.Now;
            //    dtPrintedDate.Visible = false;
            //    dtPrintedTime.Visible = false;
            //    imgPrintedClear.Visible = false;
            //}
            //else
            //{
            //    lmmddyyyy = dateTimeTOD[0].Split('/');
            //    lhhmmss = dateTimeTOD[1].Split(':');

            //    DateTime printedDate;

            //    if (dateTimeTOD[2].Equals("AM"))
            //    {
            //        printedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]), Int32.Parse(lhhmmss[1]), 0);
            //    }
            //    else
            //    {
            //        if (lhhmmss[0].Equals("12"))
            //        {
            //            printedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), 0, Int32.Parse(lhhmmss[1]), 0);
            //        }
            //        else
            //        {
            //            printedDate = new DateTime(Int32.Parse(lmmddyyyy[2]), Int32.Parse(lmmddyyyy[0]), Int32.Parse(lmmddyyyy[1]), Int32.Parse(lhhmmss[0]) + 12, Int32.Parse(lhhmmss[1]), 0);
            //        }
            //    }

            //    if (lmmddyyyy[2].Equals("1900"))
            //    {
            //        dtPrintedDate.Value = DateTime.Now;
            //        dtPrintedTime.Value = DateTime.Now;
            //        dtPrintedDate.Visible = false;
            //        dtPrintedTime.Visible = false;
            //        imgPrintedClear.Visible = false;
            //    }
            //    else
            //    {
            //        dtPrintedDate.Value = printedDate;
            //        dtPrintedTime.Value = printedDate;
            //        dtPrintedDate.Visible = true;
            //        dtPrintedTime.Visible = true;
            //        imgPrintedClear.Visible = true;
            //    }
            //}

            txtCustomer.Text = selectedLoanPackage.Customer;
            txtPrinter.Text = selectedLoanPackage.Printer;
            txtAmount.Text = selectedLoanPackage.Amount.ToString("C2");
            txtComment.Text = selectedLoanPackage.Comments;

            pageLoading = false;
        }

        private void SetGridButtons()
        {
            switch (selectedLoanPackage.Status.Trim())
            {
                case ("New"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("In Processing"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("Processed"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("In Review"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("Reviewed"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("In QC"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("QC’d"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit;
                        imgLoanPackageEdit.Enabled = true;
                        break;
                    }
                case ("Complete"):
                    {
                        imgLoanPackageDelete.Image = Properties.Resources.Close;
                        imgLoanPackageDelete.Enabled = true;
                        imgLoanPackageEdit.Image = Properties.Resources.Edit_Disabled;
                        imgLoanPackageEdit.Enabled = false;
                        break;
                    }
            }
        }

        private void SetDueDate()
        {
            if (cboType.SelectedIndex > -1)
            {
                DateTime arrivalDateTime = dtArrivalTime.Value;
                TimeSpan selectedTime = new TimeSpan(arrivalDateTime.TimeOfDay.Hours, arrivalDateTime.TimeOfDay.Minutes, 0);
                TimeSpan closingTime = new TimeSpan(closingHour, closingMinute, 00);
                TimeSpan openingTime = new TimeSpan(openingHour, openingMinute, 00);
                TimeSpan addTime = new TimeSpan(addTimeHour, addTimeMinute, 00);
                TimeSpan dayCuttoffTime = closingTime.Subtract(addTime);
                TimeSpan newExtraTime = selectedTime.Add(new TimeSpan(addTime.Hours, addTime.Minutes, 0));

                if (newExtraTime.CompareTo(closingTime) <= 0)
                {
                    //There are enough hours left to set Due Date
                    DateTime newDueDate = new DateTime(arrivalDateTime.Year, arrivalDateTime.Month, arrivalDateTime.Day, newExtraTime.Hours, newExtraTime.Minutes, 00);
                    dtDueDate.Value = newDueDate;
                    dtDueTime.Value = newDueDate;
                }
                else
                {
                    //Not enough hours, will need to roll over to next day
                    TimeSpan nextDayTime = closingTime.Subtract(newExtraTime);

                    int extraTimeHour = nextDayTime.Hours < 0 ? nextDayTime.Hours * -1 : nextDayTime.Hours;
                    int extraTimeMinute = nextDayTime.Minutes < 0 ? nextDayTime.Minutes * -1 : nextDayTime.Minutes;

                    DateTime newDueDate = new DateTime(arrivalDateTime.Year, arrivalDateTime.Month, arrivalDateTime.Day + 1, openingHour + extraTimeHour, openingMinute + extraTimeMinute, 0);
                    newDueDate.TimeOfDay.Add(nextDayTime);

                    dtDueDate.Value = newDueDate;
                    dtDueTime.Value = newDueDate;
                }

                lblDueDate.Visible = true;
                dtDueDate.Visible = true;
                dtDueTime.Visible = true;
            }

            //if (selectedTime.CompareTo(closingTime) > 0)
            //{
            //    //Selected Time is Greater than 5:00, so Move to 8:00 the Next Day
            //    DateTime newDate = new DateTime(arrivalDateTime.Year, arrivalDateTime.Month, arrivalDateTime.Day + 1, 8, 00, 00);
            //    dtArrivalTime.Value = newDate;
            //    dtArrivalDate.Value = newDate;
            //}
            //else if (selectedTime.CompareTo(openingTime) < 0)
            //{
            //    //Selected Time is Less Than 8:00, so Move To 5:00 the Prior Day
            //    DateTime newDate = new DateTime(arrivalDateTime.Year, arrivalDateTime.Month, arrivalDateTime.Day - 1, 17, 00, 00);
            //    dtArrivalTime.Value = newDate;
            //    dtArrivalDate.Value = newDate;
            //}
            //else
            //{

            //}
        }

        private Models.Type GetTypeByID(string id)
        {
            Models.Type returnType = new Models.Type();

            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/type/" + id.Trim());

            typeList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    returnType = JsonConvert.DeserializeObject<Models.Type>(jsonString.Result);
                });

            task.Wait();

            return returnType;
        }

        private bool DeleteLoanPackage(LoanPackage deletedLoanPackage)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/loanpackage/DeleteLoanPackage");
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(deletedLoanPackage);
            HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

            var task = client.PostAsync(url, contentPost)
                .ContinueWith((taskwithresponse) =>
                {
                    try
                    {
                        taskwithresponse.Result.EnsureSuccessStatusCode();
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        MessageBox.Show("Error Found Deleting Loan Package:\n\n" + ex.Message.Trim(), "Delete Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;

            //string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/loanpackage/DeleteLoanPackage" + id.Trim());
            //bool success = false;

            //var client = new HttpClient();

            //var task = client.DeleteAsync(url)
            //    .ContinueWith((taskwithresponse) =>
            //    {
            //        try
            //        {
            //            taskwithresponse.Result.EnsureSuccessStatusCode();
            //            success = true;
            //        }
            //        catch (Exception ex)
            //        {
            //            success = false;
            //            MessageBox.Show("Error Found Deleting Loan Package:\n\n" + ex.Message.Trim(), "Delete Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    });

            //task.Wait();

            //return success;
        }

        private void ChangeLoanPackageStatus()
        {
            switch (selectedLoanPackage.Status.Trim())
            {
                case ("New"):
                    {
                        selectedLoanPackage.Status = "In Processing";
                        selectedLoanPackage.CheckedOutByUserID = BankUser.ID;
                        selectedLoanPackage.QC = false;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("In Processing"):
                    {
                        selectedLoanPackage.Status = "Processed";
                        selectedLoanPackage.CheckedOutByUserID = "0";
                        selectedLoanPackage.QC = false;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("Processed"):
                    {
                        selectedLoanPackage.Status = "In Review";
                        selectedLoanPackage.CheckedOutByUserID = BankUser.ID;
                        selectedLoanPackage.QC = false;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("In Review"):
                    {
                        selectedLoanPackage.Status = "Reviewed";
                        selectedLoanPackage.CheckedOutByUserID = "0";
                        selectedLoanPackage.QC = false;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;
                        selectedLoanPackage.ReviewedByUserID = BankUser.ID;
                        selectedLoanPackage.ReviewedDateTime = systemDateTime; //await GetCurrentDateTime(); //DateTime.Now;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("Reviewed"):
                    {
                        selectedLoanPackage.Status = "In QC";
                        selectedLoanPackage.CheckedOutByUserID = BankUser.ID;
                        selectedLoanPackage.QC = false;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("In QC"):
                    {
                        selectedLoanPackage.Status = "QC’d";
                        selectedLoanPackage.CheckedOutByUserID = "0";
                        selectedLoanPackage.QC = true;
                        selectedLoanPackage.LastUpdatedBy = BankUser.ID;

                        UpdateLoanPackage(selectedLoanPackage);
                        ClearLoanPackageFields();
                        LoadLoanPackages();
                        mainSplitContainer.Panel1Collapsed = true;

                        break;
                    }
                case ("QC’d"):
                    {
                        //Do you want to mark as Complete?

                        break;
                    }
                case ("Complete"):
                    {
                        break;
                    }
            }
        }

        private void SetCheckInCheckOutStatusLabel()
        {
            switch (selectedLoanPackage.Status.Trim())
            {
                case ("New"):
                    {
                        btnProcessingCheckIn.Text = " Check-In";
                        btnProcessingCheckOut.Text = " Check-Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("In Processing"):
                    {
                        btnProcessingCheckIn.Text = " Processed";
                        btnProcessingCheckOut.Text = " Check-Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In as 'Processed'");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("Processed"):
                    {
                        btnProcessingCheckIn.Text = " Check-In";
                        btnProcessingCheckOut.Text = " In Review";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out as 'In Review'");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("In Review"):
                    {
                        btnProcessingCheckIn.Text = " Reviewed";
                        btnProcessingCheckOut.Text = " Check-Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In as 'Reviewed'");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("Reviewed"):
                    {
                        btnProcessingCheckIn.Text = " Check In";
                        btnProcessingCheckOut.Text = " In-QC";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out as 'In-QC'");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("In QC"):
                    {
                        btnProcessingCheckIn.Text = " QC'd";
                        btnProcessingCheckOut.Text = " Check-Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In as 'QC'd'");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("QC’d"):
                    {
                        btnProcessingCheckIn.Text = " Check-In";
                        btnProcessingCheckOut.Text = " Check Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Complete";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Complete");

                        break;
                    }
                case ("Complete"):
                    {
                        btnProcessingCheckIn.Text = " Check-In";
                        btnProcessingCheckOut.Text = " Check-Out";
                        toolTip1.SetToolTip(btnProcessingCheckIn, "Click to Check-In");
                        toolTip1.SetToolTip(btnProcessingCheckOut, "Click to Check-Out");
                        btnProcessingComplete.Text = "Re-Open";
                        toolTip1.SetToolTip(btnProcessingComplete, "Click to Re-Open");

                        break;
                    }
            }
        }


        private void EditLoanPackage()
        {
            SetCheckInCheckOutStatusLabel();

            mainSplitContainer.Panel1Collapsed = false;

            lblProcessed.Visible = true;
            lblReviewed.Visible = true;
            lblSentCompleted.Visible = true;

            dtProcessedDate.Visible = true;
            dtProcessedTime.Visible = true;
            dtReviewedDate.Visible = true;
            dtReviewedTime.Visible = true;
            dtPrintedDate.Visible = true;
            dtPrintedTime.Visible = true;

            cboReviewedUser.Visible = true;
            cboProcessedUser.Visible = true;
            cboStatus.SelectedIndex = 0;
            cboStatus.Enabled = true;

            SetLoanPackageFields();
        }

        private void CompleteLoanPackage()
        {
            selectedLoanPackage.QC = true;
            selectedLoanPackage.Status = "Complete";
            selectedLoanPackage.Closed = true;
            selectedLoanPackage.CheckedOutByUserID = "0";
            selectedLoanPackage.LastUpdatedBy = BankUser.ID;
            selectedLoanPackage.PrintedDateTime = new DateTime(dtPrintedDate.Value.Year, dtPrintedDate.Value.Month, dtPrintedDate.Value.Day, dtPrintedTime.Value.Hour, dtPrintedTime.Value.Minute, 00); 
            selectedLoanPackage.Printer = txtPrinter.Text;

            UpdateLoanPackage(selectedLoanPackage);
            ClearLoanPackageFields();
            LoadLoanPackages();

            mainSplitContainer.Panel1Collapsed = true;
        }

        private void ReOpenLoanPackage()
        {
            mainSplitContainer.Enabled = false;
            cboReOpenStatus.SelectedIndex = -1;
            winReOpen.Visible = true;
        }

        private bool IsNumeric(int Val)
        {
            return ((Val >= 48 && Val <= 57) || (Val == 8) || (Val == 46) || (Val >= 96 && Val <= 105));
        }

        private void SetLoanPackageClass()
        {
            selectedLoanPackage.CompanyID = Company.ID;
            selectedLoanPackage.Customer = txtCustomer.Text;
            selectedLoanPackage.OfficerID = (cboOfficer.SelectedItem as Officer).ID;
            selectedLoanPackage.TypeID = (cboType.SelectedItem as Models.Type).ID;
            selectedLoanPackage.Amount = Decimal.Parse(txtAmount.Text.Replace("$", "").Replace(",", ""));
            selectedLoanPackage.StatusID = "";
            selectedLoanPackage.AddedByUserID = cboArrivalUser.SelectedIndex > -1 ? (cboArrivalUser.SelectedItem as User).ID : "0";
            selectedLoanPackage.ProcessedByUserID = cboProcessedUser.SelectedIndex > -1 ? (cboProcessedUser.SelectedItem as User).ID : "0";
            selectedLoanPackage.ReviewedByUserID = cboReviewedUser.SelectedIndex > -1 ? (cboReviewedUser.SelectedItem as User).ID : "0";
            selectedLoanPackage.Printer = txtPrinter.Text;
            selectedLoanPackage.AddedDateTime = new DateTime(dtArrivalDate.Value.Year, dtArrivalDate.Value.Month, dtArrivalDate.Value.Day, dtArrivalTime.Value.Hour, dtArrivalTime.Value.Minute, 00); //dtArrivalDate.Value.ToShortDateString() + " " + dtArrivalTime.Value.ToShortTimeString();
            selectedLoanPackage.DueDateTime = new DateTime(dtDueDate.Value.Year, dtDueDate.Value.Month, dtDueDate.Value.Day, dtDueTime.Value.Hour, dtDueTime.Value.Minute, 00); //dtDueDate.Value.ToShortDateString() + " " + dtDueTime.Value.ToShortTimeString();
            selectedLoanPackage.Comments = txtComment.Text;
            selectedLoanPackage.LastUpdatedBy = BankUser.ID;
            selectedLoanPackage.CheckedOutByUserID = (cboStatus.SelectedItem.ToString().Contains("In")) ? BankUser.ID : "0";
            selectedLoanPackage.Status = cboStatus.SelectedItem.ToString();
            selectedLoanPackage.ProcessedDateTime = new DateTime(dtProcessedDate.Value.Year, dtProcessedDate.Value.Month, dtProcessedDate.Value.Day, dtProcessedTime.Value.Hour, dtProcessedTime.Value.Minute, 00); //cboProcessedUser.SelectedIndex > -1 ? dtProcessedDate.Value.ToShortDateString() + " " + dtProcessedTime.Value.ToShortTimeString() : "";
            selectedLoanPackage.ReviewedDateTime = new DateTime(dtReviewedDate.Value.Year, dtReviewedDate.Value.Month, dtReviewedDate.Value.Day, dtReviewedTime.Value.Hour, dtReviewedTime.Value.Minute, 00); //cboReviewedUser.SelectedIndex > -1 ? dtReviewedDate.Value.ToShortDateString() + " " + dtReviewedTime.Value.ToShortTimeString() : "";
            selectedLoanPackage.PrintedDateTime = new DateTime(dtPrintedDate.Value.Year, dtPrintedDate.Value.Month, dtPrintedDate.Value.Day, dtPrintedTime.Value.Hour, dtPrintedTime.Value.Minute, 00); //txtPrinter.Text.Equals("") ? "" : dtProcessedDate.Value.ToShortDateString() + " " + dtProcessedTime.Value.ToShortTimeString();
            selectedLoanPackage.Closed = cboStatus.SelectedItem.ToString().Equals("Complete") ? true : false;

            if (selectedLoanPackage.Status.Equals("QC’d"))
            {
                selectedLoanPackage.CheckedOutByUserID = "0";
                selectedLoanPackage.QC = true;
                selectedLoanPackage.LastUpdatedBy = BankUser.ID;
            }
        }

        private async Task<DateTime> GetCurrentDateTime()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            DateTime currentDateTime;
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/company/GetCompanyTimeByID/" + Company.ID.Trim());
            var client = new HttpClient();

            tokenSource = new CancellationTokenSource();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url, tokenSource.Token);

                var jsonString = response.Content.ReadAsStringAsync();

                currentDateTime = JsonConvert.DeserializeObject<DateTime>(jsonString.Result);
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                client.Dispose();
                currentDateTime = DateTime.Now;
            }

            return currentDateTime;
        }
        //************************************************************************************
        //Event Handlers
        //************************************************************************************    
        private void imgMinMax_Click(object sender, EventArgs e)
        {
            if (mainSplitContainer.Panel1Collapsed)
            {
                mainSplitContainer.Panel1Collapsed = false;
                imgMinMax.Image = Properties.Resources.ArrowUp;
            }
            else
            {
                mainSplitContainer.Panel1Collapsed = true;
                imgMinMax.Image = Properties.Resources.ArrowDown;
            }
        }

        private void btnProcessingSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (ValidLoanPackageFields())
            {
                SetLoanPackageClass();

                if (selectedLoanPackage.ID.Equals("0"))
                {
                    //Create New Loan Package
                    selectedLoanPackage.CheckedOutByUserID = "0";
                    selectedLoanPackage.Status = "New";
                    selectedLoanPackage.ProcessedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    selectedLoanPackage.ReviewedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
                    selectedLoanPackage.PrintedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);

                    if (CreateNewLoanPackage(selectedLoanPackage))
                    {
                        mainSplitContainer.Panel1Collapsed = true;
                        LoadLoanPackages();
                        ClearLoanPackageFields();
                    }
                }
                else
                {
                    //Update Loan Package
                    if (selectedLoanPackage.Status.Equals("Processed") && selectedLoanPackage.ProcessedByUserID.Equals("0"))
                    {
                        selectedLoanPackage.ProcessedByUserID = BankUser.ID;
                        selectedLoanPackage.ProcessedDateTime = systemDateTime; //await GetCurrentDateTime(); //DateTime.Now;
                    }

                    if (selectedLoanPackage.Status.Equals("Reviewed") && selectedLoanPackage.ReviewedByUserID.Equals("0"))
                    {
                        selectedLoanPackage.ReviewedByUserID = BankUser.ID;
                        selectedLoanPackage.ReviewedDateTime = systemDateTime; //await GetCurrentDateTime(); //DateTime.Now;
                    }

                    if (UpdateLoanPackage(selectedLoanPackage))
                    {
                        mainSplitContainer.Panel1Collapsed = true;
                        LoadLoanPackages();
                        ClearLoanPackageFields();
                    }
                }
            }

            Cursor = Cursors.Default;
        }

        private void btnProcessingClear_Click(object sender, EventArgs e)
        {
            ClearLoanPackageFields();

            dgLoanPackages.ClearSelection();

            mainSplitContainer.Panel1Collapsed = true;
        }

        private void dgLoanPackages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                DataGridView grid = (DataGridView)sender;
                SortOrder so = SortOrder.None;

                if (grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.None ||
                    grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    so = SortOrder.Descending;
                }
                else
                {
                    so = SortOrder.Ascending;
                }

                SortLoanPackage(grid.Columns[e.ColumnIndex].DataPropertyName, so);
                grid.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = so;
            }
            else
            {
                if (dgLoanPackages.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgLoanPackages.SelectedRows)
                    {
                        selectedLoanPackage = row.DataBoundItem as LoanPackage;
                        SetGridButtons();

                        row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;

                        if (row.DefaultCellStyle.BackColor.Name == "0")
                        {
                            row.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                        }
                        else if (row.DefaultCellStyle.BackColor.Name == "White")
                        {
                            row.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            row.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                        }
                    }
                }
            }
        }

        private void dgLoanPackages_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgLoanPackages.ClearSelection();
        }

        private void dgLoanPackages_MouseUp(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitTestInfo;

            if (e.Button == MouseButtons.Right)
            {
                hitTestInfo = dgLoanPackages.HitTest(e.X, e.Y);

                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    dgLoanPackages.Rows[hitTestInfo.RowIndex].Selected = true;

                    foreach (DataGridViewRow row in dgLoanPackages.SelectedRows)
                    {
                        selectedLoanPackage = row.DataBoundItem as LoanPackage;
                    }

                    bool continueWithEdit = true;

                    if (selectedLoanPackage.Closed)
                    {
                        continueWithEdit = false;
                    }
                    else
                    {
                        if (!selectedLoanPackage.CheckedOutByUserID.Equals("0") && !selectedLoanPackage.CheckedOutByUserID.Equals(BankUser.ID))
                        {
                            if (MessageBox.Show("You are changing an item that is currently checked out by '" + selectedLoanPackage.CheckedOutByUser.Trim() + "'.  Would you like to continue?  This could cause problems if they are still working.", "Loan Package In Use", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                continueWithEdit = true;
                            }
                            else
                            {
                                continueWithEdit = false;
                            }
                        }
                        else
                        {
                            continueWithEdit = true;
                        }
                    }

                    if (continueWithEdit)
                    {
                        SetGridButtons();
                        loanPackageListPopupMenu.Show(dgLoanPackages, new Point(e.X, e.Y));
                    }
                }
            }
        }

        private void dgLoanPackages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgLoanPackages.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgLoanPackages.SelectedRows)
                {
                    selectedLoanPackage = row.DataBoundItem as LoanPackage;

                    bool continueWithEdit = true;

                    if (!selectedLoanPackage.CheckedOutByUserID.Equals("0") && !selectedLoanPackage.CheckedOutByUserID.Equals(BankUser.ID))
                    {
                        if (MessageBox.Show("You are opening an item that is currently checked out by '" + selectedLoanPackage.CheckedOutByUser.Trim() + "'.  Would you like to take it instead?  This could cause problems if they are still working.", "Loan Package In Use", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            continueWithEdit = true;
                        }
                        else
                        {
                            continueWithEdit = false;
                        }
                    }
                    else
                    {
                        continueWithEdit = true;
                    }

                    if (continueWithEdit)
                    {
                        EditLoanPackage();
                    }
                }
            }
        }

        private void imgLoanPackageAdd_Click(object sender, EventArgs e)
        {
            ClearLoanPackageFields();

            SetCheckInCheckOutStatusLabel();

            mainSplitContainer.Panel1Collapsed = false;

            lblProcessed.Visible = false;
            lblReviewed.Visible = false;
            lblSentCompleted.Visible = false;
            lblPrinter.Visible = false;

            dtProcessedDate.Visible = false;
            dtProcessedTime.Visible = false;
            dtReviewedDate.Visible = false;
            dtReviewedTime.Visible = false;
            dtPrintedDate.Visible = false;
            dtPrintedTime.Visible = false;

            imgProcessedClear.Visible = false;
            imgReviewedClear.Visible = false;
            imgPrintedClear.Visible = false;

            cboReviewedUser.Visible = false;
            cboProcessedUser.Visible = false;
            cboStatus.SelectedIndex = 0;
            cboStatus.Enabled = false;
            txtPrinter.Visible = false;
            lblPrinter.Visible = false;

            for (int x = 0; x < arrivalUserList.Count; x++)
            {
                if (arrivalUserList[x].ID.Equals(BankUser.ID))
                {
                    cboArrivalUser.SelectedIndex = x;
                    x = arrivalUserList.Count + 1;
                }
            }

            txtCustomer.Focus();
        }

        private void imgLoanPackageDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete the selected Loan Package?\n\n" + selectedLoanPackage.Customer.Trim() + "\n" + selectedLoanPackage.TypeDescription.Trim() + "\n" + selectedLoanPackage.Amount.ToString("C2") + "\n" + selectedLoanPackage.Comments.Trim() + "", "Delete Loan Package", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DeleteLoanPackage(selectedLoanPackage))
                {
                    LoadLoanPackages();
                    ClearLoanPackageFields();
                }
            }
        }

        private void imgLoanPackageEdit_Click(object sender, EventArgs e)
        {
            EditLoanPackage();
        }

        private void cboType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!pageLoading)
            {
                if (cboType.SelectedIndex > -1)
                {
                    selectedType = (cboType.SelectedItem as Models.Type);

                    String[] typeAddTime = selectedType.AddTime.Split(':');
                    String[] typeCautionTime = selectedType.AddTime.Split(':');

                    addTimeHour = Int32.Parse(typeAddTime[0]);
                    addTimeMinute = Int32.Parse(typeAddTime[1]);
                    cautionTimeHour = Int32.Parse(typeCautionTime[0]);
                    cautionTimeMinute = Int32.Parse(typeCautionTime[1]);

                    dtDueTime.CalendarForeColor = System.Drawing.Color.Red;
                    dtDueDate.Refresh();

                    txtDueDateHighlight.Visible = true;

                    SetDueDate();
                }
            }
        }

        private void dtArrivalTime_ValueChanged(object sender, EventArgs e)
        {
            DateTime arrivalDateTime = dtArrivalTime.Value;
            TimeSpan selectedTime = new TimeSpan(arrivalDateTime.TimeOfDay.Hours, arrivalDateTime.TimeOfDay.Minutes, 0);
            TimeSpan currentTime = (systemDateTime).TimeOfDay;//new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (selectedTime.CompareTo(currentTime) > 0)
            {
                //Time Cannot be Greater than Current Time
                DateTime currentDateTime = new DateTime(arrivalDateTime.Year, arrivalDateTime.Month, arrivalDateTime.Day, currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
                dtArrivalTime.Value = currentDateTime;
                dtArrivalTime.Value = currentDateTime;
            }
        }

        private void btnProcessingCheckIn_Click(object sender, EventArgs e)
        {
            SetLoanPackageClass();
            ChangeLoanPackageStatus();
        }

        private void btnProcessingCheckOut_Click(object sender, EventArgs e)
        {
            SetLoanPackageClass();
            ChangeLoanPackageStatus();
        }

        private void imgLoanPackageRefreshList_Click(object sender, EventArgs e)
        {
            LoadLoanPackages();
        }

        private void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadLoanPackages();
        }

        private void cboFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanPackages();
        }

        private void cboFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanPackages();
        }

        private void lblFilterStatus_Click(object sender, EventArgs e)
        {
            cboFilterStatus.SelectedIndex = -1;
            LoadLoanPackages();
        }

        private void lblFilterType_Click(object sender, EventArgs e)
        {
            cboFilterType.SelectedIndex = -1;
            LoadLoanPackages();
        }

        private void btnProcessingClose_Click(object sender, EventArgs e)
        {
            if (selectedLoanPackage.Amount >= selectedType.QCAmount && selectedLoanPackage.QC == false)
            {
                MessageBox.Show("Sorry, this loan amount exceeds the QC Amount of " + selectedType.QCAmount.ToString("C2") + " for this type. Loan must be QC'd before it can be placed in a 'Complete' status.", "Complete Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtPrinter.Text.Equals(""))
            {
                MessageBox.Show("'Sent Via' cannot be blank on Complete.", "Complete Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (!dtPrintedDate.Visible)
                {
                    dtPrintedDate.Visible = true;
                    dtPrintedTime.Visible = true;
                    dtPrintedDate.Value = systemDateTime; //await GetCurrentDateTime();// DateTime.Now;
                    dtPrintedTime.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
                    imgPrintedClear.Visible = true;
                    txtPrinter.Visible = true;
                    lblPrinter.Visible = true;

                    txtPrinter.Focus();
                }
            }
            else
            {
                if (!dtPrintedDate.Visible)
                {
                    dtPrintedDate.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
                    dtPrintedTime.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
                }

                if (btnProcessingComplete.Text.Equals("Complete"))
                {
                    CompleteLoanPackage();
                }
                else
                {
                    ReOpenLoanPackage();
                }
            }
        }

        private void tsExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BankWorxTrackerX.xlsx", SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add Sheets to the Workbook.
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "LoanPackage" };
                sheets.Append(sheet);

                // Get the sheetData cell table.
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // storing header part in Excel
                int i = 0;
                int j = 0;

                //Create the Header Rows
                Row headerRow = new Row();

                List<String> columns = new List<string>();

                for (i = 1; i < dgLoanPackages.Columns.Count + 1; i++)
                {
                    //if (dgLoanPackages.Columns[i - 1].Visible)
                    //{
                    if (!dgLoanPackages.Columns[i - 1].HeaderText.Contains("Error"))
                    {
                        columns.Add(dgLoanPackages.Columns[i - 1].HeaderText);

                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dgLoanPackages.Columns[i - 1].HeaderText);
                        headerRow.AppendChild(cell);
                    }
                    //}
                }

                sheetData.AppendChild(headerRow);

                for (i = 0; i <= dgLoanPackages.RowCount - 1; i++)
                {
                    Row newRow = new Row();

                    for (j = 0; j < dgLoanPackages.ColumnCount; j++)
                    {
                        DataGridViewCell dgCell = dgLoanPackages[j, i];

                        //if (dgCell.Visible)
                        //{
                        if (!dgCell.OwningColumn.HeaderText.Contains("Error"))
                        {
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(dgCell.Value.ToString());

                            newRow.AppendChild(cell);
                        }
                        //}
                    }

                    sheetData.AppendChild(newRow);
                }

                spreadsheetDocument.Close();

                Cursor = Cursors.Default;

                MessageBox.Show("Loan Package Data Successfully Exported to Excel:\n\nFile Name: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BankWorxTrackerX.xlsx", "Export Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error found Exported to Excel:\n\n" + ex.Message.Trim(), "Export Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsExportToExcel_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void tsExportToExcel_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void cboProcessedUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtProcessedDate.Value.Year.Equals(1900))
            {
                dtProcessedDate.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
            }

            dtProcessedDate.Visible = true;
            dtProcessedTime.Visible = true;
            imgProcessedClear.Visible = true;
        }

        private void cboReviewedUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtReviewedDate.Value.Year.Equals(1900))
            {
                dtReviewedDate.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
            }

            dtReviewedDate.Visible = true;
            dtReviewedTime.Visible = true;
            imgReviewedClear.Visible = true;
        }

        private void lblSentCompleted_Click(object sender, EventArgs e)
        {
            if (selectedLoanPackage.Amount >= selectedType.QCAmount && selectedLoanPackage.QC == false)
            {
                dtPrintedDate.Visible = false;
                dtPrintedTime.Visible = false;
                dtPrintedDate.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                dtPrintedTime.Value = new DateTime(1900, 1, 1, 0, 0, 0);
                imgPrintedClear.Visible = false;
                txtPrinter.Visible = false;
                lblPrinter.Visible = false;

                MessageBox.Show("Sorry, this loan amount exceeds the QC Amount of " + selectedType.QCAmount.ToString("C2") + " for this type. Loan must be QC'd before it can be placed in a 'Complete' status.", "Complete Loan Package Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                dtPrintedDate.Visible = true;
                dtPrintedTime.Visible = true;
                dtPrintedDate.Value = systemDateTime; //await GetCurrentDateTime();// DateTime.Now;
                dtPrintedTime.Value = systemDateTime; //await GetCurrentDateTime();//DateTime.Now;
                imgPrintedClear.Visible = true;
                txtPrinter.Visible = true;
                lblPrinter.Visible = true;

                txtPrinter.Focus();
            }
        }

        private void txtAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) || e.KeyCode == Keys.Back || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal)
            {
                validNumericKey = true;
            }
            else
            {
                validNumericKey = false;
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!validNumericKey)
            {
                e.Handled = true;
                return;
            }
            else
            {
                if (e.KeyChar == Convert.ToChar(Keys.Back))
                {
                    txtAmount.Text = "";
                    //formatAmount = "";

                    //if (formatAmount.Length > 0)
                    //{
                    //    formatAmount = formatAmount.Substring(0, formatAmount.Length - 1);
                    //}
                }
                else
                {
                    if (txtAmount.SelectionLength > 0)
                    {
                        txtAmount.Text = txtAmount.Text.Remove(txtAmount.SelectionStart, txtAmount.SelectionLength);
                    }

                    txtAmount.Text = txtAmount.Text + e.KeyChar;

                    if (txtAmount.Text.Contains("."))
                    {
                        String[] decimalValues = txtAmount.Text.Split('.');

                        if (decimalValues[1].Length == 0)
                        {
                            //txtAmount.Text = txtAmount.Text.Trim() + ".";
                        }
                        else if (decimalValues[1].Length == 1)
                        {
                            txtAmount.Text = Decimal.Parse(txtAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0.0");
                        }
                        else if (decimalValues[1].Length == 2)
                        {
                            txtAmount.Text = Decimal.Parse(txtAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0.00");
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        txtAmount.Text = Decimal.Parse(txtAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0");
                    }

                    //if (formatAmount.Length > 2)
                    //{
                    //    formatAmount = formatAmount + e.KeyChar;
                    //}
                    //else
                    //{
                    //    formatAmount = formatAmount + e.KeyChar;
                    //}
                }

                //if (formatAmount.Length == 0)
                //{
                //    txtAmount.Text = "0";
                //}
                //else if (formatAmount.Length == 1)
                //{
                //    txtAmount.Text = "0.0" + formatAmount;
                //}
                //else if (formatAmount.Length == 2)
                //{
                //    txtAmount.Text = "0." + formatAmount;
                //}
                //else if (formatAmount.Length > 2)
                //{
                //    txtAmount.Text = Decimal.Parse(formatAmount.Replace("$", "").Replace(",", "")).ToString("C2");
                //    //formatAmount.Substring(0, formatAmount.Length - 2) + "." + formatAmount.Substring(formatAmount.Length - 2);
                //}

                txtAmount.Select(txtAmount.Text.Length, 0);
            }

            e.Handled = true;
        }

        private void txtAmount_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void dgLoanPackages_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgv = (DataGridView)sender;

            if (dgv.Rows[e.RowIndex].Selected)
            {
                int width = dgLoanPackages.Width;

                Rectangle r = dgv.GetRowDisplayRectangle(e.RowIndex, false);
                Rectangle rect = e.RowBounds;

                ControlPaint.DrawBorder(e.Graphics, rect,
                                        System.Drawing.Color.FromArgb(0, 125, 0), 2, ButtonBorderStyle.Solid,
                                        System.Drawing.Color.FromArgb(0, 125, 0), 2, ButtonBorderStyle.Solid,
                                        System.Drawing.Color.FromArgb(0, 125, 0), 2, ButtonBorderStyle.Solid,
                                        System.Drawing.Color.FromArgb(0, 125, 0), 2, ButtonBorderStyle.Solid);
            }
        }

        private void imgProcessedClear_Click(object sender, EventArgs e)
        {
            DateTime clearDate = new DateTime(1900, 1, 1, 0, 0, 0);
            cboProcessedUser.SelectedIndex = -1;
            dtProcessedDate.Value = clearDate;
            dtProcessedTime.Value = clearDate;
            dtProcessedDate.Visible = false;
            dtProcessedTime.Visible = false;
            imgProcessedClear.Visible = false;

            selectedLoanPackage.ProcessedByUser = "";
            selectedLoanPackage.ProcessedByUserID = "0";
            selectedLoanPackage.ProcessedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        }

        private void imgReviewedClear_Click(object sender, EventArgs e)
        {
            DateTime clearDate = new DateTime(1900, 1, 1, 0, 0, 0);
            cboReviewedUser.SelectedIndex = -1;
            dtReviewedDate.Value = clearDate;
            dtReviewedTime.Value = clearDate;
            dtReviewedDate.Visible = false;
            dtReviewedTime.Visible = false;
            imgReviewedClear.Visible = false;

            selectedLoanPackage.ReviewedByUser = "";
            selectedLoanPackage.ReviewedByUserID = "0";
            selectedLoanPackage.ReviewedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        }

        private void imgPrintedClear_Click(object sender, EventArgs e)
        {
            DateTime clearDate = new DateTime(1900, 1, 1, 0, 0, 0);
            cboProcessedUser.SelectedIndex = -1;
            dtPrintedDate.Value = clearDate;
            dtPrintedTime.Value = clearDate;
            dtPrintedDate.Visible = false;
            dtPrintedTime.Visible = false;
            imgPrintedClear.Visible = false;
            txtPrinter.Text = "";

            selectedLoanPackage.PrintedDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
            selectedLoanPackage.Printer = "";
        }

        private void btnReOpenSave_Click(object sender, EventArgs e)
        {
            selectedLoanPackage.Status = cboReOpenStatus.SelectedItem.ToString();
            selectedLoanPackage.CheckedOutByUserID = (cboReOpenStatus.SelectedItem.ToString().Contains("In")) ? BankUser.ID : "0";
            selectedLoanPackage.LastUpdatedBy = BankUser.ID;
            selectedLoanPackage.Closed = false;

            UpdateLoanPackage(selectedLoanPackage);
            SetLoanPackageFields();
            SetCheckInCheckOutStatusLabel();

            mainSplitContainer.Enabled = true;
            winReOpen.Visible = false;
        }

        private void btnReOpenCancel_Click(object sender, EventArgs e)
        {
            winReOpen.Visible = false;
            cboReOpenStatus.SelectedIndex = -1;
            mainSplitContainer.Enabled = true;
        }

        private void dgLoanPackages_Paint(object sender, PaintEventArgs e)
        {
            SetLoanPackageListColor();
        }

        private void systemTimer_Tick(object sender, EventArgs e)
        {
            systemDateTime = systemDateTime.AddSeconds(1);
            //txtSystemTime.Text = systemDateTime.ToShortDateString() + " " + systemDateTime.ToShortTimeString();
        }
    }
}
