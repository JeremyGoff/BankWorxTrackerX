using BankWorxTrackerX.Models;
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
    public partial class SettingsForm : Form
    {
        private List<Officer> officerList = new List<Officer>();
        private Officer selectedOfficer = new Officer();
        private List<Models.Type> typeList = new List<Models.Type>();
        private Models.Type selectedType = new Models.Type();
        private List<User> userList = new List<User>();
        private User selectedUser = new User();
        bool validNumericKey = false;

        public SettingsForm()
        {
            InitializeComponent();
        }

        //************************************************************************************
        //Form Load Method
        //************************************************************************************
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            txtTypeQCAmount.DataBindings.Add("Text", selectedType, "QCAmount", true, DataSourceUpdateMode.OnValidation, 0, "C2");

            LoadOfficers();

            selectedOfficer = new Officer();
            selectedOfficer.ID = "0";
        }

        //************************************************************************************
        //Other Methods
        //************************************************************************************
        private void settingsTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabCtrl = (TabControl)sender;
            Brush fontBrush = Brushes.Black;
            string title = tabCtrl.TabPages[e.Index].Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            int indent = 3;
            Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y + indent, e.Bounds.Width, e.Bounds.Height - indent);

            if (e.Index == settingsTabControl.SelectedIndex)
            {
                e.Graphics.DrawString(settingsTabControl.TabPages[e.Index].Text,
                    new Font(settingsTabControl.Font, FontStyle.Bold),
                    Brushes.Black,
                    new PointF(e.Bounds.X + 3, e.Bounds.Y + 3));
            }
            else
            {
                e.Graphics.DrawString(settingsTabControl.TabPages[e.Index].Text,
                    settingsTabControl.Font,
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

        private void SettingsForm_Paint(object sender, PaintEventArgs e)
        {
            //settingsTabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            //settingsTabControl.Alignment = TabAlignment.Left;
            //settingsTabControl.SizeMode = TabSizeMode.Fixed;
        }

        private void ClearOfficerFields()
        {
            selectedOfficer = new Officer();
            selectedOfficer.ID = "0";

            txtOfficerName.Text = "";
            txtOfficerInitials.Text = "";
            chkOfficerActive.Checked = false;

            btnOfficerDelete.Image = Properties.Resources.Denials_Disabled;
            btnOfficerDelete.Enabled = false;

            txtOfficerName.Focus();
        }

        private void SetOfficerFields()
        {
            txtOfficerName.Text = selectedOfficer.Name;
            txtOfficerInitials.Text = selectedOfficer.Initials;
            chkOfficerActive.Checked = selectedOfficer.Active;

            btnOfficerDelete.Image = Properties.Resources.Denials;
            btnOfficerDelete.Enabled = true;

            txtOfficerName.SelectAll();
            txtOfficerName.Focus();
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

            dgOfficers.DataSource = officerList;

            foreach (DataGridViewColumn column in dgOfficers.Columns)
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
                    case "Initials":
                        {
                            column.Width = 100;
                            column.ValueType = typeof(DataGridViewTextBoxCell);
                            break;
                        }
                    case "Name":
                        {
                            column.Width = 200;
                            column.HeaderText = "Officer";
                            column.ValueType = typeof(DataGridViewTextBoxCell);
                            break;
                        }
                    case "Active":
                        {
                            column.Width = 75;
                            column.ValueType = typeof(DataGridViewTextBoxCell);
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
        }

        private bool CreateNewOfficer(Officer newOfficer)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/officer");
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(newOfficer);
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
                        MessageBox.Show("Error Found Adding New Officer:\n\n" + ex.Message.Trim(), "New Officer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool UpdateOfficer(Officer updateOfficer)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/officer/" + updateOfficer.ID.Trim());
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(updateOfficer);
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
                        MessageBox.Show("Error Found Updating Officer:\n\n" + ex.Message.Trim(), "Update Officer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool DeleteOfficer(String id)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/officer/" + id.Trim());
            bool success = false;

            var client = new HttpClient();

            var task = client.DeleteAsync(url)
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
                        MessageBox.Show("Error Found Deleting Officer:\n\n" + ex.Message.Trim(), "Delete Officer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private async void ClearTypeFields()
        {
            selectedType = new Models.Type();
            selectedType.ID = "0";

            txtTypeName.Text = "";
            txtTypeQCAmount.Text = "";
            chkTypeActive.Checked = false;

            DateTime currentDate = await GetCurrentDateTime();
            DateTime addTimeDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 4, 0, 00);
            dtTypeAddTime.Value = addTimeDate;

            DateTime cautionTimeDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 1, 00, 00);
            dtTypeCautionTime.Value = cautionTimeDate;

            btnTypeDelete.Image = Properties.Resources.Denials_Disabled;
            btnTypeDelete.Enabled = false;

            txtTypeQCAmount.Text = "$0.00";

            txtTypeName.Focus();
        }

        private async void SetTypeFields()
        {
            DateTime currentDate = await GetCurrentDateTime();

            txtTypeName.Text = selectedType.Description;
            txtTypeQCAmount.Text = selectedType.QCAmount.ToString("C2");
            chkTypeActive.Checked = selectedType.Active;

            String[] addhhmmss = selectedType.AddTime.Split(':');
            dtTypeAddTime.Value = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, Int32.Parse(addhhmmss[0]), Int32.Parse(addhhmmss[1]), 00);

            String[] cautionhhmmss = selectedType.CautionTime.Split(':');
            dtTypeCautionTime.Value = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, Int32.Parse(cautionhhmmss[0]), Int32.Parse(cautionhhmmss[1]), 00);

            btnTypeDelete.Image = Properties.Resources.Denials;
            btnTypeDelete.Enabled = true;

            txtTypeName.SelectAll();
            txtTypeName.Focus();
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

            dgTypes.DataSource = typeList;

            foreach (DataGridViewColumn column in dgTypes.Columns)
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
                    case "Description":
                        {
                            column.Width = 125;
                            column.HeaderText = "Type";
                            break;
                        }
                    case "QCAmount":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "QCAmountFormatted":
                        {
                            column.Width = 150;
                            column.HeaderText = "QC Amount";
                            break;
                        }
                    case "AddTime":
                        {
                            column.Width = 125;
                            column.HeaderText = "Add Time";
                            break;
                        }
                    case "CautionTime":
                        {
                            column.Width = 125;
                            column.HeaderText = "Caution Time";
                            break;
                        }
                    case "Active":
                        {
                            column.Width = 75;
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
        }

        private bool CreateNewType(Models.Type newType)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/type");
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(newType);
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
                        MessageBox.Show("Error Found Adding New Type:\n\n" + ex.Message.Trim(), "New Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool UpdateType(Models.Type updateType)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/type/" + updateType.ID.Trim());
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(updateType);
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
                        MessageBox.Show("Error Found Updating Type:\n\n" + ex.Message.Trim(), "Update Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool DeleteType(String id)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/type/" + id.Trim());
            bool success = false;

            var client = new HttpClient();

            var task = client.DeleteAsync(url)
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
                        MessageBox.Show("Error Found Deleting Type:\n\n" + ex.Message.Trim(), "Delete Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private void ClearUserFields()
        {
            selectedUser = new User();
            selectedUser.ID = "0";

            txtUserWindowsID.Text = "";
            txtUserDomain.Text = "";
            txtUserFirstName.Text = "";
            txtUserLastName.Text = "";
            cboUserClassID.SelectedIndex = -1;
            txtUserEmail.Text = "";
            chkUserActive.Checked = false;
            chkUserOnline.Checked = false;

            btnUserDelete.Image = Properties.Resources.Denials_Disabled;
            btnUserDelete.Enabled = false;

            txtUserWindowsID.Focus();
        }

        private void SetUserFields()
        {
            txtUserWindowsID.Text = selectedUser.WindowsID;
            txtUserDomain.Text = selectedUser.Domain;
            txtUserFirstName.Text = selectedUser.FirstName;
            txtUserLastName.Text = selectedUser.LastName;
            cboUserClassID.SelectedIndex = selectedUser.ClassID.Equals("1") == true ? 0 : 1;
            txtUserEmail.Text = selectedUser.Email;
            chkUserActive.Checked = selectedUser.Active;
            chkUserOnline.Checked = selectedUser.Online;

            btnUserDelete.Image = Properties.Resources.Denials;
            btnUserDelete.Enabled = true;

            txtUserWindowsID.Focus();
            txtUserWindowsID.SelectAll();
        }

        private void LoadUsers()
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/getactivebycompanyid/" + Company.ID.Trim());

            userList.Clear();

            var client = new HttpClient();
            var task = client.GetAsync(url)
                .ContinueWith((taskwithresponse) =>
                {
                    var response = taskwithresponse.Result;
                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    userList = JsonConvert.DeserializeObject<List<User>>(jsonString.Result);
                });

            task.Wait();

            dgUsers.DataSource = userList;

            foreach (DataGridViewColumn column in dgUsers.Columns)
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
                    case "WindowsID":
                        {
                            column.Width = 125;
                            column.HeaderText = "Windows ID";
                            break;
                        }
                    case "Domain":
                        {
                            column.Width = 125;
                            break;
                        }
                    case "FullName":
                        {
                            column.Width = 200;
                            column.HeaderText = "Name";
                            break;
                        }
                    case "FirstName":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "LastName":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "ClassID":
                        {
                            column.Visible = false;
                            break;
                        }
                    case "Email":
                        {
                            column.Width = 100;
                            break;
                        }
                    case "Active":
                        {
                            column.Width = 75;
                            break;
                        }
                    case "Online":
                        {
                            column.Width = 75;
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
        }

        private bool CreateNewUser(User newUser)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user");
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(newUser);
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
                        MessageBox.Show("Error Found Adding New User:\n\n" + ex.Message.Trim(), "New User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool UpdateUser(User updateUser)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/" + updateUser.ID.Trim());
            bool success = false;

            var client = new HttpClient();
            var param = Newtonsoft.Json.JsonConvert.SerializeObject(updateUser);
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
                        MessageBox.Show("Error Found Updating User:\n\n" + ex.Message.Trim(), "Update User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
        }

        private bool DeleteUser(String id)
        {
            string url = String.Format(ConfigurationManager.AppSettings["TrackerXServiceURL"] + "/user/" + id.Trim());
            bool success = false;

            var client = new HttpClient();

            var task = client.DeleteAsync(url)
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
                        MessageBox.Show("Error Found Deleting User:\n\n" + ex.Message.Trim(), "Delete User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            task.Wait();

            return success;
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
        private void settingsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (settingsTabControl.SelectedIndex)
            {
                case 0: //Officer
                    {
                        LoadOfficers();
                        ClearOfficerFields();
                        break;
                    }
                case 1: //Type
                    {
                        LoadTypes();
                        ClearTypeFields();
                        break;
                    }
                case 2: //Users
                    {
                        LoadUsers();
                        ClearUserFields();
                        break;
                    }
            }
        }

        //************************************************************************************
        //Officer Methods
        //************************************************************************************    
        private void btnOfficerSave_Click(object sender, EventArgs e)
        {
            if (txtOfficerName.Text.Equals(""))
            {
                MessageBox.Show("Officer Name cannot be blank.", "Officer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtOfficerInitials.Text.Equals(""))
            {
                MessageBox.Show("Officer Initials cannot be blank.", "Officer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (selectedOfficer.ID.Trim().Equals("0"))
                {
                    selectedOfficer = new Officer();

                    selectedOfficer.ID = "0";
                    selectedOfficer.CompanyID = Company.ID;
                    selectedOfficer.Initials = txtOfficerInitials.Text.Trim();
                    selectedOfficer.Name = txtOfficerName.Text.Trim();
                    selectedOfficer.Active = chkOfficerActive.Checked;

                    if (CreateNewOfficer(selectedOfficer))
                    {
                        LoadOfficers();
                        ClearOfficerFields();
                    }
                }
                else
                {
                    selectedOfficer.CompanyID = Company.ID;
                    selectedOfficer.Initials = txtOfficerInitials.Text.Trim();
                    selectedOfficer.Name = txtOfficerName.Text.Trim();
                    selectedOfficer.Active = chkOfficerActive.Checked;

                    if (UpdateOfficer(selectedOfficer))
                    {
                        LoadOfficers();
                        ClearOfficerFields();
                    }
                }
            }
        }

        private void btnOfficerDelete_Click(object sender, EventArgs e)
        {
            if (selectedOfficer.ID.Trim().Equals("0"))
            {
                MessageBox.Show("Please select an Officer from the List.", "Officer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete the selected Officer Record?\n\n" + selectedOfficer.Name.Trim() + "", "Delete Officer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DeleteOfficer(selectedOfficer.ID.Trim()))
                    {
                        LoadOfficers();
                        ClearOfficerFields();
                    }
                }
            }
        }

        private void btnOfficerClear_Click(object sender, EventArgs e)
        {
            ClearOfficerFields();
        }

        private void dgOfficers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgOfficers.SelectedRows)
            {
                //Clear All Fields
                ClearOfficerFields();

                selectedOfficer = row.DataBoundItem as Officer;

                SetOfficerFields();
            }
        }

        private void dgOfficers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgOfficers.ClearSelection();
        }

        //************************************************************************************
        //Type Methods
        //************************************************************************************    
        private void btnTypeSave_Click(object sender, EventArgs e)
        {
            if (txtTypeName.Text.Equals(""))
            {
                MessageBox.Show("Type Name cannot be blank.", "Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtTypeQCAmount.Text.Equals(""))
            {
                MessageBox.Show("QC Amount cannot be blank.", "Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dtTypeCautionTime.Value.TimeOfDay.CompareTo(dtTypeAddTime.Value.TimeOfDay) > 0)
            {
                MessageBox.Show("Caution Time cannot be greater than Add Time.", "Type Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                selectedType.CompanyID = Company.ID;
                selectedType.Description = txtTypeName.Text.Trim();
                selectedType.QCAmount = Decimal.Parse(txtTypeQCAmount.Text.Trim().Replace("$", "").Replace(",", ""));
                selectedType.AddTime = dtTypeAddTime.Value.TimeOfDay.ToString();
                selectedType.CautionTime = dtTypeCautionTime.Value.TimeOfDay.ToString();
                selectedType.Active = chkTypeActive.Checked;

                if (selectedType.ID.Trim().Equals("0"))
                {
                    selectedType.ID = "0";

                    if (CreateNewType(selectedType))
                    {
                        LoadTypes();
                        ClearTypeFields();
                    }
                }
                else
                {
                    if (UpdateType(selectedType))
                    {
                        LoadTypes();
                        ClearTypeFields();
                    }
                }
            }
        }

        private void btnTypeDelete_Click(object sender, EventArgs e)
        {
            if (selectedType.ID.Trim().Equals("0"))
            {
                MessageBox.Show("Please select an Type from the List.", "Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete the selected Type Record?\n\n" + selectedType.Description.Trim() + "", "Delete Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DeleteType(selectedType.ID.Trim()))
                    {
                        LoadTypes();
                        ClearTypeFields();
                    }
                }
            }
        }

        private void btnTypeClear_Click(object sender, EventArgs e)
        {
            ClearTypeFields();
        }

        private void dgTypes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgTypes.SelectedRows)
            {
                //Clear All Fields
                ClearTypeFields();

                selectedType = row.DataBoundItem as Models.Type;

                SetTypeFields();
            }
        }

        private void dgTypes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgTypes.ClearSelection();
        }

        private void txtTypeQCAmount_KeyDown(object sender, KeyEventArgs e)
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

        private void txtTypeQCAmount_KeyPress(object sender, KeyPressEventArgs e)
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
                    txtTypeQCAmount.Text = "";
                }
                else
                {
                    if (txtTypeQCAmount.SelectionLength > 0)
                    {
                        txtTypeQCAmount.Text = txtTypeQCAmount.Text.Remove(txtTypeQCAmount.SelectionStart, txtTypeQCAmount.SelectionLength);
                    }

                    txtTypeQCAmount.Text = txtTypeQCAmount.Text + e.KeyChar;

                    if (txtTypeQCAmount.Text.Contains("."))
                    {
                        String[] decimalValues = txtTypeQCAmount.Text.Split('.');

                        if (decimalValues[1].Length == 0)
                        {
                            //txtAmount.Text = txtAmount.Text.Trim() + ".";
                        }
                        else if (decimalValues[1].Length == 1)
                        {
                            txtTypeQCAmount.Text = Decimal.Parse(txtTypeQCAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0.0");
                        }
                        else if (decimalValues[1].Length == 2)
                        {
                            txtTypeQCAmount.Text = Decimal.Parse(txtTypeQCAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0.00");
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        txtTypeQCAmount.Text = Decimal.Parse(txtTypeQCAmount.Text.Replace("$", "").Replace(",", "")).ToString("#,###,##0");
                    }
                }

                txtTypeQCAmount.Select(txtTypeQCAmount.Text.Length, 0);
            }

            e.Handled = true;
        }
        //************************************************************************************
        //User Methods
        //************************************************************************************    
        private void btnUserSave_Click(object sender, EventArgs e)
        {
            if (txtUserWindowsID.Text.Equals(""))
            {
                MessageBox.Show("Windows ID cannot be blank.", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUserFirstName.Text.Equals(""))
            {
                MessageBox.Show("First Name cannot be blank.", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUserLastName.Text.Equals(""))
            {
                MessageBox.Show("Last Name cannot be blank.", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (cboUserClassID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a User Class.", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                selectedUser.CompanyID = Company.ID;
                selectedUser.WindowsID = txtUserWindowsID.Text.Trim();
                selectedUser.Domain = txtUserDomain.Text.Trim();
                selectedUser.FirstName = txtUserFirstName.Text.Trim();
                selectedUser.LastName = txtUserLastName.Text.Trim();
                selectedUser.FullName = txtUserFirstName.Text.Trim() + " " + txtUserLastName.Text.Trim();
                selectedUser.ClassID = cboUserClassID.SelectedIndex == 0 ? "1" : "2";
                selectedUser.Email = txtUserEmail.Text.Trim();
                selectedUser.Active = chkUserActive.Checked;
                selectedUser.Online = false;

                if (selectedUser.ID.Trim().Equals("0"))
                {
                    if (CreateNewUser(selectedUser))
                    {
                        LoadUsers();
                        ClearUserFields();
                    }
                }
                else
                {
                    if (UpdateUser(selectedUser))
                    {
                        LoadUsers();
                        ClearUserFields();
                    }
                }
            }
        }

        private void btnUserDelete_Click(object sender, EventArgs e)
        {
            if (selectedUser.ID.Trim().Equals("0"))
            {
                MessageBox.Show("Please select an User from the List.", "User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to Delete the selected User Record?\n\n" + selectedUser.FullName.Trim() + "", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (DeleteUser(selectedUser.ID.Trim()))
                    {
                        LoadUsers();
                        ClearUserFields();
                    }
                }
            }
        }

        private void btnUserClear_Click(object sender, EventArgs e)
        {
            ClearUserFields();
        }

        private void dgUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgUsers.SelectedRows)
            {
                //Clear All Fields
                ClearUserFields();

                selectedUser = row.DataBoundItem as User;

                SetUserFields();
            }
        }

        private void dgUsers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgUsers.ClearSelection();
        }

        private void imgSettingsHome_Click(object sender, EventArgs e)
        {
            (MdiParent as HomeForm).homeToolStripMenuItem_Click(null, null);
        }

    }
}
