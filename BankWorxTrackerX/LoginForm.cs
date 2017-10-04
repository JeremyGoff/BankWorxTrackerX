using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankWorxTrackerX
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            splashTimer.Start();
        }

        //************************************************************************************
        //Other Methods
        //************************************************************************************

        //************************************************************************************
        //Event Handlers
        //************************************************************************************    
        private void splashTimer_Tick(object sender, EventArgs e)
        {
            splashTimer.Stop();

            HomeForm homeScreen = new HomeForm();
            homeScreen.Show();

            this.Hide();

            Cursor = Cursors.Default;
        }
    }
}
