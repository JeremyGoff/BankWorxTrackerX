namespace BankWorxTrackerX
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.tabOfficers = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.chkOfficerActive = new System.Windows.Forms.CheckBox();
            this.txtOfficerInitials = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOfficerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOfficerClear = new System.Windows.Forms.Button();
            this.btnOfficerDelete = new System.Windows.Forms.Button();
            this.btnOfficerSave = new System.Windows.Forms.Button();
            this.dgOfficers = new System.Windows.Forms.DataGridView();
            this.tabType = new System.Windows.Forms.TabPage();
            this.dtTypeCautionTime = new System.Windows.Forms.DateTimePicker();
            this.dtTypeAddTime = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkTypeActive = new System.Windows.Forms.CheckBox();
            this.txtTypeQCAmount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgTypes = new System.Windows.Forms.DataGridView();
            this.btnTypeClear = new System.Windows.Forms.Button();
            this.btnTypeDelete = new System.Windows.Forms.Button();
            this.btnTypeSave = new System.Windows.Forms.Button();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.chkUserOnline = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.txtUserLastName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtUserFirstName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtUserEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUserDomain = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkUserActive = new System.Windows.Forms.CheckBox();
            this.txtUserWindowsID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dgUsers = new System.Windows.Forms.DataGridView();
            this.btnUserClear = new System.Windows.Forms.Button();
            this.btnUserDelete = new System.Windows.Forms.Button();
            this.btnUserSave = new System.Windows.Forms.Button();
            this.settingsImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel10 = new System.Windows.Forms.Panel();
            this.imgSettingsHome = new System.Windows.Forms.PictureBox();
            this.label20 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label11 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cboUserClassID = new System.Windows.Forms.ComboBox();
            this.settingsTabControl.SuspendLayout();
            this.tabOfficers.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOfficers)).BeginInit();
            this.tabType.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTypes)).BeginInit();
            this.tabUsers.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).BeginInit();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSettingsHome)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.settingsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsTabControl.Controls.Add(this.tabOfficers);
            this.settingsTabControl.Controls.Add(this.tabType);
            this.settingsTabControl.Controls.Add(this.tabUsers);
            this.settingsTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.settingsTabControl.ImageList = this.settingsImageList;
            this.settingsTabControl.ItemSize = new System.Drawing.Size(35, 105);
            this.settingsTabControl.Location = new System.Drawing.Point(0, 34);
            this.settingsTabControl.Multiline = true;
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(1124, 682);
            this.settingsTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.settingsTabControl.TabIndex = 0;
            this.settingsTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.settingsTabControl_DrawItem);
            this.settingsTabControl.SelectedIndexChanged += new System.EventHandler(this.settingsTabControl_SelectedIndexChanged);
            // 
            // tabOfficers
            // 
            this.tabOfficers.BackColor = System.Drawing.SystemColors.Control;
            this.tabOfficers.Controls.Add(this.label11);
            this.tabOfficers.Controls.Add(this.panel1);
            this.tabOfficers.Controls.Add(this.chkOfficerActive);
            this.tabOfficers.Controls.Add(this.txtOfficerInitials);
            this.tabOfficers.Controls.Add(this.label2);
            this.tabOfficers.Controls.Add(this.txtOfficerName);
            this.tabOfficers.Controls.Add(this.label1);
            this.tabOfficers.Controls.Add(this.btnOfficerClear);
            this.tabOfficers.Controls.Add(this.btnOfficerDelete);
            this.tabOfficers.Controls.Add(this.btnOfficerSave);
            this.tabOfficers.Controls.Add(this.dgOfficers);
            this.tabOfficers.ImageIndex = 0;
            this.tabOfficers.Location = new System.Drawing.Point(109, 4);
            this.tabOfficers.Name = "tabOfficers";
            this.tabOfficers.Padding = new System.Windows.Forms.Padding(3);
            this.tabOfficers.Size = new System.Drawing.Size(1011, 674);
            this.tabOfficers.TabIndex = 0;
            this.tabOfficers.Text = " Officer";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.label15);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1005, 32);
            this.panel1.TabIndex = 31;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(461, 4);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(70, 23);
            this.label15.TabIndex = 0;
            this.label15.Text = "Officers";
            // 
            // chkOfficerActive
            // 
            this.chkOfficerActive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkOfficerActive.AutoSize = true;
            this.chkOfficerActive.Location = new System.Drawing.Point(614, 209);
            this.chkOfficerActive.Name = "chkOfficerActive";
            this.chkOfficerActive.Size = new System.Drawing.Size(65, 22);
            this.chkOfficerActive.TabIndex = 2;
            this.chkOfficerActive.Text = "Active";
            this.toolTip1.SetToolTip(this.chkOfficerActive, "Check this box if you wish users to see this Officer in their drop-downs.");
            this.chkOfficerActive.UseVisualStyleBackColor = true;
            // 
            // txtOfficerInitials
            // 
            this.txtOfficerInitials.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtOfficerInitials.Location = new System.Drawing.Point(614, 145);
            this.txtOfficerInitials.Name = "txtOfficerInitials";
            this.txtOfficerInitials.Size = new System.Drawing.Size(73, 26);
            this.txtOfficerInitials.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(548, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 18);
            this.label2.TabIndex = 28;
            this.label2.Text = "Initials";
            // 
            // txtOfficerName
            // 
            this.txtOfficerName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtOfficerName.Location = new System.Drawing.Point(614, 98);
            this.txtOfficerName.Name = "txtOfficerName";
            this.txtOfficerName.Size = new System.Drawing.Size(215, 26);
            this.txtOfficerName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(548, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 18);
            this.label1.TabIndex = 26;
            this.label1.Text = "Name";
            // 
            // btnOfficerClear
            // 
            this.btnOfficerClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOfficerClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOfficerClear.Image = global::BankWorxTrackerX.Properties.Resources.Refresh_32x32;
            this.btnOfficerClear.Location = new System.Drawing.Point(645, 582);
            this.btnOfficerClear.Name = "btnOfficerClear";
            this.btnOfficerClear.Size = new System.Drawing.Size(98, 46);
            this.btnOfficerClear.TabIndex = 5;
            this.btnOfficerClear.Text = " Clear";
            this.btnOfficerClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOfficerClear.UseVisualStyleBackColor = true;
            this.btnOfficerClear.Click += new System.EventHandler(this.btnOfficerClear_Click);
            // 
            // btnOfficerDelete
            // 
            this.btnOfficerDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOfficerDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOfficerDelete.Enabled = false;
            this.btnOfficerDelete.Image = global::BankWorxTrackerX.Properties.Resources.Denials_Disabled;
            this.btnOfficerDelete.Location = new System.Drawing.Point(439, 582);
            this.btnOfficerDelete.Name = "btnOfficerDelete";
            this.btnOfficerDelete.Size = new System.Drawing.Size(98, 46);
            this.btnOfficerDelete.TabIndex = 4;
            this.btnOfficerDelete.Text = " Delete";
            this.btnOfficerDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOfficerDelete.UseVisualStyleBackColor = true;
            this.btnOfficerDelete.Click += new System.EventHandler(this.btnOfficerDelete_Click);
            // 
            // btnOfficerSave
            // 
            this.btnOfficerSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOfficerSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOfficerSave.Image = global::BankWorxTrackerX.Properties.Resources.Save_32x32;
            this.btnOfficerSave.Location = new System.Drawing.Point(219, 582);
            this.btnOfficerSave.Name = "btnOfficerSave";
            this.btnOfficerSave.Size = new System.Drawing.Size(98, 46);
            this.btnOfficerSave.TabIndex = 3;
            this.btnOfficerSave.Text = " Save";
            this.btnOfficerSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOfficerSave.UseVisualStyleBackColor = true;
            this.btnOfficerSave.Click += new System.EventHandler(this.btnOfficerSave_Click);
            // 
            // dgOfficers
            // 
            this.dgOfficers.AllowUserToAddRows = false;
            this.dgOfficers.AllowUserToDeleteRows = false;
            this.dgOfficers.AllowUserToResizeRows = false;
            this.dgOfficers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgOfficers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOfficers.Location = new System.Drawing.Point(51, 59);
            this.dgOfficers.MultiSelect = false;
            this.dgOfficers.Name = "dgOfficers";
            this.dgOfficers.ReadOnly = true;
            this.dgOfficers.RowHeadersVisible = false;
            this.dgOfficers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOfficers.Size = new System.Drawing.Size(420, 497);
            this.dgOfficers.TabIndex = 6;
            this.dgOfficers.TabStop = false;
            this.dgOfficers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgOfficers_CellClick);
            this.dgOfficers.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgOfficers_DataBindingComplete);
            // 
            // tabType
            // 
            this.tabType.BackColor = System.Drawing.SystemColors.Control;
            this.tabType.Controls.Add(this.label17);
            this.tabType.Controls.Add(this.dtTypeCautionTime);
            this.tabType.Controls.Add(this.dtTypeAddTime);
            this.tabType.Controls.Add(this.panel2);
            this.tabType.Controls.Add(this.label7);
            this.tabType.Controls.Add(this.label6);
            this.tabType.Controls.Add(this.chkTypeActive);
            this.tabType.Controls.Add(this.txtTypeQCAmount);
            this.tabType.Controls.Add(this.label3);
            this.tabType.Controls.Add(this.txtTypeName);
            this.tabType.Controls.Add(this.label5);
            this.tabType.Controls.Add(this.dgTypes);
            this.tabType.Controls.Add(this.btnTypeClear);
            this.tabType.Controls.Add(this.btnTypeDelete);
            this.tabType.Controls.Add(this.btnTypeSave);
            this.tabType.ImageIndex = 2;
            this.tabType.Location = new System.Drawing.Point(109, 4);
            this.tabType.Name = "tabType";
            this.tabType.Padding = new System.Windows.Forms.Padding(3);
            this.tabType.Size = new System.Drawing.Size(1011, 674);
            this.tabType.TabIndex = 2;
            this.tabType.Text = " Types";
            // 
            // dtTypeCautionTime
            // 
            this.dtTypeCautionTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtTypeCautionTime.CustomFormat = "hh:mm";
            this.dtTypeCautionTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTypeCautionTime.Location = new System.Drawing.Point(768, 232);
            this.dtTypeCautionTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtTypeCautionTime.Name = "dtTypeCautionTime";
            this.dtTypeCautionTime.ShowUpDown = true;
            this.dtTypeCautionTime.Size = new System.Drawing.Size(65, 26);
            this.dtTypeCautionTime.TabIndex = 3;
            this.toolTip1.SetToolTip(this.dtTypeCautionTime, "Number of hours and minutes before due to turn the loan package row Caution Orang" +
        "e.");
            // 
            // dtTypeAddTime
            // 
            this.dtTypeAddTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtTypeAddTime.CustomFormat = "hh:mm";
            this.dtTypeAddTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtTypeAddTime.Location = new System.Drawing.Point(768, 188);
            this.dtTypeAddTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtTypeAddTime.Name = "dtTypeAddTime";
            this.dtTypeAddTime.ShowUpDown = true;
            this.dtTypeAddTime.Size = new System.Drawing.Size(65, 26);
            this.dtTypeAddTime.TabIndex = 2;
            this.toolTip1.SetToolTip(this.dtTypeAddTime, "Hours and minutes Added to the Arrival Time to determine Due Date/Time.");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1005, 32);
            this.panel2.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(461, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Types";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(665, 236);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 18);
            this.label7.TabIndex = 42;
            this.label7.Text = "Caution Time";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(665, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 18);
            this.label6.TabIndex = 40;
            this.label6.Text = "Add Time";
            // 
            // chkTypeActive
            // 
            this.chkTypeActive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkTypeActive.AutoSize = true;
            this.chkTypeActive.Location = new System.Drawing.Point(768, 280);
            this.chkTypeActive.Name = "chkTypeActive";
            this.chkTypeActive.Size = new System.Drawing.Size(65, 22);
            this.chkTypeActive.TabIndex = 4;
            this.chkTypeActive.Text = "Active";
            this.toolTip1.SetToolTip(this.chkTypeActive, "Check this box if you wish users to see this Type in their drop-downs.");
            this.chkTypeActive.UseVisualStyleBackColor = true;
            // 
            // txtTypeQCAmount
            // 
            this.txtTypeQCAmount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTypeQCAmount.Location = new System.Drawing.Point(768, 141);
            this.txtTypeQCAmount.Name = "txtTypeQCAmount";
            this.txtTypeQCAmount.Size = new System.Drawing.Size(136, 26);
            this.txtTypeQCAmount.TabIndex = 1;
            this.txtTypeQCAmount.Text = "$0.00";
            this.txtTypeQCAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.txtTypeQCAmount, "Amount above which Quality Control will be Required.");
            this.txtTypeQCAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTypeQCAmount_KeyDown);
            this.txtTypeQCAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTypeQCAmount_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(665, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 18);
            this.label3.TabIndex = 37;
            this.label3.Text = "QC Amount";
            // 
            // txtTypeName
            // 
            this.txtTypeName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtTypeName.Location = new System.Drawing.Point(768, 94);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.Size = new System.Drawing.Size(162, 26);
            this.txtTypeName.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(665, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 18);
            this.label5.TabIndex = 35;
            this.label5.Text = "Type";
            // 
            // dgTypes
            // 
            this.dgTypes.AllowUserToAddRows = false;
            this.dgTypes.AllowUserToDeleteRows = false;
            this.dgTypes.AllowUserToResizeRows = false;
            this.dgTypes.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTypes.Location = new System.Drawing.Point(28, 51);
            this.dgTypes.MultiSelect = false;
            this.dgTypes.Name = "dgTypes";
            this.dgTypes.ReadOnly = true;
            this.dgTypes.RowHeadersVisible = false;
            this.dgTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTypes.Size = new System.Drawing.Size(583, 523);
            this.dgTypes.TabIndex = 31;
            this.dgTypes.TabStop = false;
            this.dgTypes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTypes_CellClick);
            this.dgTypes.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgTypes_DataBindingComplete);
            // 
            // btnTypeClear
            // 
            this.btnTypeClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTypeClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeClear.Image = global::BankWorxTrackerX.Properties.Resources.Refresh_32x32;
            this.btnTypeClear.Location = new System.Drawing.Point(612, 590);
            this.btnTypeClear.Name = "btnTypeClear";
            this.btnTypeClear.Size = new System.Drawing.Size(98, 46);
            this.btnTypeClear.TabIndex = 7;
            this.btnTypeClear.Text = " Clear";
            this.btnTypeClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTypeClear.UseVisualStyleBackColor = true;
            this.btnTypeClear.Click += new System.EventHandler(this.btnTypeClear_Click);
            // 
            // btnTypeDelete
            // 
            this.btnTypeDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTypeDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeDelete.Enabled = false;
            this.btnTypeDelete.Image = global::BankWorxTrackerX.Properties.Resources.Denials_Disabled;
            this.btnTypeDelete.Location = new System.Drawing.Point(404, 590);
            this.btnTypeDelete.Name = "btnTypeDelete";
            this.btnTypeDelete.Size = new System.Drawing.Size(98, 46);
            this.btnTypeDelete.TabIndex = 6;
            this.btnTypeDelete.Text = " Delete";
            this.btnTypeDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTypeDelete.UseVisualStyleBackColor = true;
            this.btnTypeDelete.Click += new System.EventHandler(this.btnTypeDelete_Click);
            // 
            // btnTypeSave
            // 
            this.btnTypeSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTypeSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTypeSave.Image = global::BankWorxTrackerX.Properties.Resources.Save_32x32;
            this.btnTypeSave.Location = new System.Drawing.Point(188, 590);
            this.btnTypeSave.Name = "btnTypeSave";
            this.btnTypeSave.Size = new System.Drawing.Size(98, 46);
            this.btnTypeSave.TabIndex = 5;
            this.btnTypeSave.Text = " Save";
            this.btnTypeSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTypeSave.UseVisualStyleBackColor = true;
            this.btnTypeSave.Click += new System.EventHandler(this.btnTypeSave_Click);
            // 
            // tabUsers
            // 
            this.tabUsers.BackColor = System.Drawing.SystemColors.Control;
            this.tabUsers.Controls.Add(this.cboUserClassID);
            this.tabUsers.Controls.Add(this.chkUserOnline);
            this.tabUsers.Controls.Add(this.panel3);
            this.tabUsers.Controls.Add(this.txtUserLastName);
            this.tabUsers.Controls.Add(this.label14);
            this.tabUsers.Controls.Add(this.txtUserFirstName);
            this.tabUsers.Controls.Add(this.label13);
            this.tabUsers.Controls.Add(this.txtUserEmail);
            this.tabUsers.Controls.Add(this.label8);
            this.tabUsers.Controls.Add(this.label12);
            this.tabUsers.Controls.Add(this.txtUserDomain);
            this.tabUsers.Controls.Add(this.label10);
            this.tabUsers.Controls.Add(this.chkUserActive);
            this.tabUsers.Controls.Add(this.txtUserWindowsID);
            this.tabUsers.Controls.Add(this.label9);
            this.tabUsers.Controls.Add(this.dgUsers);
            this.tabUsers.Controls.Add(this.btnUserClear);
            this.tabUsers.Controls.Add(this.btnUserDelete);
            this.tabUsers.Controls.Add(this.btnUserSave);
            this.tabUsers.ImageIndex = 4;
            this.tabUsers.Location = new System.Drawing.Point(109, 4);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(1011, 674);
            this.tabUsers.TabIndex = 3;
            this.tabUsers.Text = " Users";
            // 
            // chkUserOnline
            // 
            this.chkUserOnline.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkUserOnline.AutoSize = true;
            this.chkUserOnline.Location = new System.Drawing.Point(817, 479);
            this.chkUserOnline.Name = "chkUserOnline";
            this.chkUserOnline.Size = new System.Drawing.Size(69, 22);
            this.chkUserOnline.TabIndex = 7;
            this.chkUserOnline.Text = "Online";
            this.toolTip1.SetToolTip(this.chkUserOnline, "The Online checkbox will be managed by the system.  No need to set it.");
            this.chkUserOnline.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(125)))), ((int)(((byte)(0)))));
            this.panel3.Controls.Add(this.label16);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1005, 32);
            this.panel3.TabIndex = 52;
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(461, 3);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(54, 23);
            this.label16.TabIndex = 0;
            this.label16.Text = "Users";
            // 
            // txtUserLastName
            // 
            this.txtUserLastName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUserLastName.Location = new System.Drawing.Point(740, 263);
            this.txtUserLastName.Name = "txtUserLastName";
            this.txtUserLastName.Size = new System.Drawing.Size(193, 26);
            this.txtUserLastName.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(737, 241);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 18);
            this.label14.TabIndex = 50;
            this.label14.Text = "Last Name";
            // 
            // txtUserFirstName
            // 
            this.txtUserFirstName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUserFirstName.Location = new System.Drawing.Point(740, 204);
            this.txtUserFirstName.Name = "txtUserFirstName";
            this.txtUserFirstName.Size = new System.Drawing.Size(193, 26);
            this.txtUserFirstName.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(737, 181);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 18);
            this.label13.TabIndex = 48;
            this.label13.Text = "First Name";
            // 
            // txtUserEmail
            // 
            this.txtUserEmail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUserEmail.Location = new System.Drawing.Point(740, 384);
            this.txtUserEmail.Name = "txtUserEmail";
            this.txtUserEmail.Size = new System.Drawing.Size(222, 26);
            this.txtUserEmail.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(737, 361);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 18);
            this.label8.TabIndex = 46;
            this.label8.Text = "Email";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(737, 302);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 18);
            this.label12.TabIndex = 44;
            this.label12.Text = "Class";
            // 
            // txtUserDomain
            // 
            this.txtUserDomain.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUserDomain.Location = new System.Drawing.Point(740, 144);
            this.txtUserDomain.Name = "txtUserDomain";
            this.txtUserDomain.Size = new System.Drawing.Size(193, 26);
            this.txtUserDomain.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtUserDomain, "The domain that issues the Windows ID.  If unsure, see your network administrator" +
        ".");
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(737, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 18);
            this.label10.TabIndex = 40;
            this.label10.Text = "Domain";
            // 
            // chkUserActive
            // 
            this.chkUserActive.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkUserActive.AutoSize = true;
            this.chkUserActive.Location = new System.Drawing.Point(817, 435);
            this.chkUserActive.Name = "chkUserActive";
            this.chkUserActive.Size = new System.Drawing.Size(65, 22);
            this.chkUserActive.TabIndex = 6;
            this.chkUserActive.Text = "Active";
            this.toolTip1.SetToolTip(this.chkUserActive, "Check this box if you wish Users to be able to launch the App.");
            this.chkUserActive.UseVisualStyleBackColor = true;
            // 
            // txtUserWindowsID
            // 
            this.txtUserWindowsID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUserWindowsID.Location = new System.Drawing.Point(740, 84);
            this.txtUserWindowsID.Name = "txtUserWindowsID";
            this.txtUserWindowsID.Size = new System.Drawing.Size(152, 26);
            this.txtUserWindowsID.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtUserWindowsID, "The ID with which the user logs into their computer.");
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(737, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 18);
            this.label9.TabIndex = 35;
            this.label9.Text = "Windows ID";
            // 
            // dgUsers
            // 
            this.dgUsers.AllowUserToAddRows = false;
            this.dgUsers.AllowUserToDeleteRows = false;
            this.dgUsers.AllowUserToResizeRows = false;
            this.dgUsers.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dgUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgUsers.Location = new System.Drawing.Point(11, 51);
            this.dgUsers.MultiSelect = false;
            this.dgUsers.Name = "dgUsers";
            this.dgUsers.ReadOnly = true;
            this.dgUsers.RowHeadersVisible = false;
            this.dgUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgUsers.Size = new System.Drawing.Size(696, 525);
            this.dgUsers.TabIndex = 31;
            this.dgUsers.TabStop = false;
            this.dgUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgUsers_CellClick);
            this.dgUsers.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgUsers_DataBindingComplete);
            // 
            // btnUserClear
            // 
            this.btnUserClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUserClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserClear.Image = global::BankWorxTrackerX.Properties.Resources.Refresh_32x32;
            this.btnUserClear.Location = new System.Drawing.Point(587, 602);
            this.btnUserClear.Name = "btnUserClear";
            this.btnUserClear.Size = new System.Drawing.Size(98, 46);
            this.btnUserClear.TabIndex = 10;
            this.btnUserClear.Text = " Clear";
            this.btnUserClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUserClear.UseVisualStyleBackColor = true;
            this.btnUserClear.Click += new System.EventHandler(this.btnUserClear_Click);
            // 
            // btnUserDelete
            // 
            this.btnUserDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUserDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserDelete.Enabled = false;
            this.btnUserDelete.Image = global::BankWorxTrackerX.Properties.Resources.Denials_Disabled;
            this.btnUserDelete.Location = new System.Drawing.Point(394, 602);
            this.btnUserDelete.Name = "btnUserDelete";
            this.btnUserDelete.Size = new System.Drawing.Size(98, 46);
            this.btnUserDelete.TabIndex = 9;
            this.btnUserDelete.Text = " Delete";
            this.btnUserDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUserDelete.UseVisualStyleBackColor = true;
            this.btnUserDelete.Click += new System.EventHandler(this.btnUserDelete_Click);
            // 
            // btnUserSave
            // 
            this.btnUserSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUserSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUserSave.Image = global::BankWorxTrackerX.Properties.Resources.Save_32x32;
            this.btnUserSave.Location = new System.Drawing.Point(201, 602);
            this.btnUserSave.Name = "btnUserSave";
            this.btnUserSave.Size = new System.Drawing.Size(98, 46);
            this.btnUserSave.TabIndex = 8;
            this.btnUserSave.Text = " Save";
            this.btnUserSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUserSave.UseVisualStyleBackColor = true;
            this.btnUserSave.Click += new System.EventHandler(this.btnUserSave_Click);
            // 
            // settingsImageList
            // 
            this.settingsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("settingsImageList.ImageStream")));
            this.settingsImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.settingsImageList.Images.SetKeyName(0, "Officer.png");
            this.settingsImageList.Images.SetKeyName(1, "Status.png");
            this.settingsImageList.Images.SetKeyName(2, "Type.png");
            this.settingsImageList.Images.SetKeyName(3, "Users.png");
            this.settingsImageList.Images.SetKeyName(4, "User.png");
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.SystemColors.Control;
            this.panel10.Controls.Add(this.imgSettingsHome);
            this.panel10.Controls.Add(this.label20);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Margin = new System.Windows.Forms.Padding(5);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1129, 34);
            this.panel10.TabIndex = 4;
            // 
            // imgSettingsHome
            // 
            this.imgSettingsHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgSettingsHome.Image = global::BankWorxTrackerX.Properties.Resources.Home;
            this.imgSettingsHome.Location = new System.Drawing.Point(10, 1);
            this.imgSettingsHome.Name = "imgSettingsHome";
            this.imgSettingsHome.Size = new System.Drawing.Size(32, 32);
            this.imgSettingsHome.TabIndex = 18;
            this.imgSettingsHome.TabStop = false;
            this.imgSettingsHome.Click += new System.EventHandler(this.imgSettingsHome_Click);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(1051, 6);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 23);
            this.label20.TabIndex = 17;
            this.label20.Text = "Settings";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(87, 644);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(758, 18);
            this.label11.TabIndex = 32;
            this.label11.Text = "To enter a new Officer, simply clear the form by pressing the Clear button, enter" +
    " the Information in the form, then press Save";
            // 
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(63, 649);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(780, 18);
            this.label17.TabIndex = 45;
            this.label17.Text = "To enter a new Loan Type, simply clear the form by pressing the Clear button, ent" +
    "er the Information in the form, then press Save";
            // 
            // cboUserClassID
            // 
            this.cboUserClassID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cboUserClassID.FormattingEnabled = true;
            this.cboUserClassID.Items.AddRange(new object[] {
            "Administrator",
            "User"});
            this.cboUserClassID.Location = new System.Drawing.Point(740, 325);
            this.cboUserClassID.Name = "cboUserClassID";
            this.cboUserClassID.Size = new System.Drawing.Size(142, 26);
            this.cboUserClassID.TabIndex = 53;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 717);
            this.ControlBox = false;
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.settingsTabControl);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BankWorx Tracker X Settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SettingsForm_Paint);
            this.settingsTabControl.ResumeLayout(false);
            this.tabOfficers.ResumeLayout(false);
            this.tabOfficers.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOfficers)).EndInit();
            this.tabType.ResumeLayout(false);
            this.tabType.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTypes)).EndInit();
            this.tabUsers.ResumeLayout(false);
            this.tabUsers.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgUsers)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgSettingsHome)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl settingsTabControl;
        private System.Windows.Forms.TabPage tabOfficers;
        private System.Windows.Forms.TabPage tabType;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.ImageList settingsImageList;
        private System.Windows.Forms.DataGridView dgOfficers;
        private System.Windows.Forms.Button btnOfficerClear;
        private System.Windows.Forms.Button btnOfficerDelete;
        private System.Windows.Forms.Button btnOfficerSave;
        private System.Windows.Forms.CheckBox chkOfficerActive;
        private System.Windows.Forms.TextBox txtOfficerInitials;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOfficerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkTypeActive;
        private System.Windows.Forms.TextBox txtTypeQCAmount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTypeClear;
        private System.Windows.Forms.Button btnTypeDelete;
        private System.Windows.Forms.Button btnTypeSave;
        private System.Windows.Forms.DataGridView dgTypes;
        private System.Windows.Forms.TextBox txtUserEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUserDomain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkUserActive;
        private System.Windows.Forms.TextBox txtUserWindowsID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUserClear;
        private System.Windows.Forms.Button btnUserDelete;
        private System.Windows.Forms.Button btnUserSave;
        private System.Windows.Forms.DataGridView dgUsers;
        private System.Windows.Forms.TextBox txtUserLastName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtUserFirstName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DateTimePicker dtTypeCautionTime;
        private System.Windows.Forms.DateTimePicker dtTypeAddTime;
        private System.Windows.Forms.CheckBox chkUserOnline;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox imgSettingsHome;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cboUserClassID;
    }
}