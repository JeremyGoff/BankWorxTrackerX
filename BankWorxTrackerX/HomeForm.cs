using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankWorxTrackerX
{
    public partial class HomeForm : Form
    {
        LoginForm loginForm = new LoginForm();
        MainForm mainScreen = new MainForm();
        SettingsForm settingsScreen = new SettingsForm();
        String userDomain = "";
        String userName = "";
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public HomeForm()
        {
            InitializeComponent();
        }

        //************************************************************************************
        //Form Load Method
        //************************************************************************************
        private async void HomeForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            bool valid = await ValidateUserAsync();

            if (!valid)
            {
                MessageBox.Show("Invalid Domain '" + userDomain.Trim() + "'. Please contact customer support.", "BankWorx Tracker Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        //************************************************************************************
        //Other Methods
        //************************************************************************************
        private async Task<bool> ValidateUserAsync()
        {
            String[] nameAndDomainSplit = (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\');

            if (nameAndDomainSplit.Count() > 0)
            {
                userDomain = nameAndDomainSplit[0];
                userName = nameAndDomainSplit[1];

                if (userName.Equals("j_k_g") || userName.Equals("vsmiley"))
                {
                    userDomain = "BANKWORX";
                }

                bool b = await GetCompanyFromDomain();

                Cursor = Cursors.Default;
                return b;
            }
            else
            {
                userDomain = "";
                userName = "";

                MessageBox.Show("User not signed into a valid Windows Domain.", "BankWorx Tracker Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Cursor = Cursors.Default;
                return false;
            }
        }

        private async Task<bool> GetCompanyFromDomain()
        {
            Company.Domain = userDomain;
            dynamic jsonResult = null;
            tokenSource = new CancellationTokenSource();
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/company/getcompanybydomain/" + Company.Domain.Trim());
            var client = new HttpClient();
            bool returnValue = true;

            try
            {
                using (client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url, tokenSource.Token);

                    var jsonString = response.Content.ReadAsStringAsync();
                    //jsonString.Wait();

                    jsonResult = JsonConvert.DeserializeObject(jsonString.Result);

                    Company.ID = jsonResult.id;
                    Company.Name = jsonResult.name;
                    Company.Code = jsonResult.code;
                    Company.Logosmall = jsonResult.logosmall;
                    Company.Logolarge = jsonResult.logolarge;
                    Company.DayStartTime = jsonResult.dayStartTime;
                    Company.DayEndTime = jsonResult.dayEndTime;
                    Company.Domain = jsonResult.domain;
                    Company.Subscription = jsonResult.subscription;

                    if (Company.ID.Equals(""))
                    {
                        tokenSource.Cancel();
                        client.Dispose();

                        returnValue = false;
                        return returnValue;
                    }
                    else
                    {
                        LogUserIn();

                        returnValue = true;
                        return returnValue;
                    }
                }
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                client.Dispose();

                MessageBox.Show("We are having issues connecting to your services.", "BankWorx Tracker Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();

                return returnValue;
            }
        }

        private async void LogUserIn()
        {
            var client = new HttpClient();
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/GetByCompanyIDAndWindowsID/" + Company.ID.Trim() + "/" + userName.Trim());
            dynamic jsonResult = null;

            tokenSource = new CancellationTokenSource();

            try
            {
                BankUser.FullName = userName;
                BankUser.Domain = userDomain;
                BankUser.ID = "";

                HttpResponseMessage response = await client.GetAsync(url, tokenSource.Token);

                var jsonString = response.Content.ReadAsStringAsync();
                //jsonString.Wait();

                jsonResult = JsonConvert.DeserializeObject(jsonString.Result);

                BankUser.ID = jsonResult.id;
                BankUser.CompanyID = jsonResult.companyID;
                BankUser.WindowsID = jsonResult.windowsID;
                BankUser.Domain = jsonResult.domain;
                BankUser.FullName = jsonResult.fullName;
                BankUser.FirstName = jsonResult.firstName;
                BankUser.LastName = jsonResult.lastName;
                BankUser.ClassID = jsonResult.classID;
                BankUser.Email = jsonResult.email;
                BankUser.Active = true;
                BankUser.Online = true;

                if (BankUser.ID.Trim().Equals(""))
                {
                    MessageBox.Show("Sorry, your User Name was not found in the User table for BankWorx Tracker X.", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    tokenSource.Cancel();
                    client.Dispose();
                }
                else
                {
                    if (BankUser.ClassID == "1")
                    {
                        setupToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        setupToolStripMenuItem.Visible = false;
                    }

                    logUserOnline();

                    homeToolStripMenuItem_Click(null, null);

                    tokenSource.Cancel();
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                client.Dispose();

                MessageBox.Show("Sorry, found an error validating the User '" + userName.Trim() + "'.\n\n" + ex.Message.Trim(), "BankWorx TrackerX User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
            }
        }

        //private void processLogin()
        //{
        //    try
        //    {
        //        tokenSource = new CancellationTokenSource();

        //        BankUser.FullName = userName;
        //        BankUser.Domain = userDomain;
        //        BankUser.ID = "";
        //        BankUser.ClassID = "1";

        //        string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/GetByCompanyIDAndWindowsID/" + Company.ID.Trim() + "/" + BankUser.FullName.Trim());
        //        dynamic jsonResult = null;

        //        var client = new HttpClient();
        //        var task = client.GetAsync(url)
        //            .ContinueWith((taskwithresponse) =>
        //            {
        //                var response = taskwithresponse.Result;
        //                var jsonString = response.Content.ReadAsStringAsync();
        //                jsonString.Wait();

        //                jsonResult = JsonConvert.DeserializeObject(jsonString.Result);
        //                BankUser.ID = jsonResult.id;
        //                BankUser.CompanyID = jsonResult.companyID;
        //                BankUser.WindowsID = jsonResult.windowsID;
        //                BankUser.Domain = jsonResult.domain;
        //                BankUser.FullName = jsonResult.fullName;
        //                BankUser.FirstName = jsonResult.firstName;
        //                BankUser.LastName = jsonResult.lastName;
        //                BankUser.ClassID = jsonResult.classID;
        //                BankUser.Email = jsonResult.email;
        //                BankUser.Active = true;
        //                BankUser.Online = true;

        //                if (BankUser.ID.Trim().Equals(""))
        //                {
        //                    MessageBox.Show("Sorry, your User Name was not found in the User table for BankWorx Tracker X.", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //                    taskwithresponse.Dispose();
        //                    client.Dispose();

        //                    tokenSource.Cancel();
        //                    //showLoginScreen();
        //                }
        //                else
        //                {
        //                    logUserOnline();

        //                    taskwithresponse.Dispose();
        //                    client.Dispose();

        //                    homeToolStripMenuItem_Click(null, null);
        //                    tokenSource.Cancel();
        //                }
        //            }, tokenSource.Token);

        //        task.Wait();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Sorry, your User Name was not found in the User table for BankWorx Tracker X.", "Invalid User", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        Application.Exit();
        //    }
        //}

        private void showLoginScreen()
        {
            if (loginForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (BankUser.ClassID == "1")
                {
                    //homeMainMenuStrip.Visible = true;
                    //setupToolStripMenuItem.Visible = true;
                    //searchToolStripMenuItem.Visible = true;
                }
                else
                {
                    //homeMainMenuStrip.Visible = false;
                    //setupToolStripMenuItem.Visible = false;
                    //searchToolStripMenuItem.Visible = false;
                }

                homeToolStripMenuItem_Click(null, null);
            }
            else
            {
                Application.Exit();
            }
        }

        private void logUserOnline()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/logonoff/" + BankUser.ID.Trim() + "/1");
            dynamic jsonResult = null;

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    jsonResult = JsonConvert.DeserializeObject(jsonString.Result);
                });

            task.Wait();
        }

        private void logUserOffline()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/logonoff/" + BankUser.ID.Trim() + "/0");
            dynamic jsonResult = null;

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    jsonResult = JsonConvert.DeserializeObject(jsonString.Result);
                });

            task.Wait();
        }

        //************************************************************************************
        //Event Handlers
        //************************************************************************************    
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mainScreen.Hide();

            settingsScreen.MdiParent = this;
            settingsScreen.WindowState = FormWindowState.Maximized;
            settingsScreen.BringToFront();
            settingsScreen.Show();
        }

        public void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //settingsScreen.Hide();

            mainScreen.MdiParent = this;
            mainScreen.WindowState = FormWindowState.Maximized;
            mainScreen.BringToFront();
            mainScreen.LoadLoanPackages();
            mainScreen.Show();
        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsScreen.Hide();
            mainScreen.Hide();
            logUserOffline();

            Refresh();

            showLoginScreen();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!BankUser.ID.Equals(""))
            {
                logUserOffline();
            }

            Application.Exit();
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BankWorx Tracker X Help documentation coming soon...", "BankWorx TrackerX", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
