namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    partial class SystemConfigurationUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemConfigurationUI));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpApplication = new System.Windows.Forms.TabPage();
            this.lblNoOfConcurrentUsers = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.lblNoOfLicenses = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.lblDevelopedBy = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblVersionNo = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblApplicationName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbpCompany = new System.Windows.Forms.TabPage();
            this.btnFind = new System.Windows.Forms.Button();
            this.pctCompanyLogo = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtContactNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCompanyAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbpBackup = new System.Windows.Forms.TabPage();
            this.txtRestoreMySqlAddress = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBackupMySqlDumpAddress = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbpDisplaySetting = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMiscellaneousPercentage = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtInterestRatePercentage = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProcessingFeePercentage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnFindScreenSaver = new System.Windows.Forms.Button();
            this.pctScreenSaver = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRecordLimit = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbLeft = new System.Windows.Forms.RadioButton();
            this.rdbRight = new System.Windows.Forms.RadioButton();
            this.rdbBottom = new System.Windows.Forms.RadioButton();
            this.rdbTop = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tbpApplication.SuspendLayout();
            this.tbpCompany.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctCompanyLogo)).BeginInit();
            this.tbpBackup.SuspendLayout();
            this.tbpDisplaySetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctScreenSaver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecordLimit)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnCancel.Location = new System.Drawing.Point(322, 427);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 40);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(216, 427);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 40);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Controls.Add(this.tbpApplication);
            this.tabControl1.Controls.Add(this.tbpCompany);
            this.tabControl1.Controls.Add(this.tbpBackup);
            this.tabControl1.Controls.Add(this.tbpDisplaySetting);
            this.tabControl1.Location = new System.Drawing.Point(12, 16);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(581, 402);
            this.tabControl1.TabIndex = 18;
            // 
            // tbpApplication
            // 
            this.tbpApplication.Controls.Add(this.lblNoOfConcurrentUsers);
            this.tbpApplication.Controls.Add(this.label40);
            this.tbpApplication.Controls.Add(this.lblNoOfLicenses);
            this.tbpApplication.Controls.Add(this.label39);
            this.tbpApplication.Controls.Add(this.lblDevelopedBy);
            this.tbpApplication.Controls.Add(this.label17);
            this.tbpApplication.Controls.Add(this.lblVersionNo);
            this.tbpApplication.Controls.Add(this.label15);
            this.tbpApplication.Controls.Add(this.lblApplicationName);
            this.tbpApplication.Controls.Add(this.label12);
            this.tbpApplication.Location = new System.Drawing.Point(4, 26);
            this.tbpApplication.Name = "tbpApplication";
            this.tbpApplication.Size = new System.Drawing.Size(573, 372);
            this.tbpApplication.TabIndex = 4;
            this.tbpApplication.Text = "Application";
            this.tbpApplication.UseVisualStyleBackColor = true;
            // 
            // lblNoOfConcurrentUsers
            // 
            this.lblNoOfConcurrentUsers.BackColor = System.Drawing.Color.Transparent;
            this.lblNoOfConcurrentUsers.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOfConcurrentUsers.ForeColor = System.Drawing.Color.Navy;
            this.lblNoOfConcurrentUsers.Location = new System.Drawing.Point(216, 207);
            this.lblNoOfConcurrentUsers.Name = "lblNoOfConcurrentUsers";
            this.lblNoOfConcurrentUsers.Size = new System.Drawing.Size(202, 23);
            this.lblNoOfConcurrentUsers.TabIndex = 23;
            this.lblNoOfConcurrentUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(48, 209);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(157, 20);
            this.label40.TabIndex = 22;
            this.label40.Text = "No. of Concurrent Users";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNoOfLicenses
            // 
            this.lblNoOfLicenses.BackColor = System.Drawing.Color.Transparent;
            this.lblNoOfLicenses.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoOfLicenses.ForeColor = System.Drawing.Color.Navy;
            this.lblNoOfLicenses.Location = new System.Drawing.Point(216, 175);
            this.lblNoOfLicenses.Name = "lblNoOfLicenses";
            this.lblNoOfLicenses.Size = new System.Drawing.Size(202, 23);
            this.lblNoOfLicenses.TabIndex = 21;
            this.lblNoOfLicenses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(93, 177);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(112, 20);
            this.label39.TabIndex = 20;
            this.label39.Text = "No. of Licenses";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDevelopedBy
            // 
            this.lblDevelopedBy.BackColor = System.Drawing.Color.Transparent;
            this.lblDevelopedBy.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevelopedBy.ForeColor = System.Drawing.Color.Navy;
            this.lblDevelopedBy.Location = new System.Drawing.Point(216, 239);
            this.lblDevelopedBy.Name = "lblDevelopedBy";
            this.lblDevelopedBy.Size = new System.Drawing.Size(202, 23);
            this.lblDevelopedBy.TabIndex = 19;
            this.lblDevelopedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(93, 241);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(112, 20);
            this.label17.TabIndex = 18;
            this.label17.Text = "Developed By";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVersionNo
            // 
            this.lblVersionNo.BackColor = System.Drawing.Color.Transparent;
            this.lblVersionNo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersionNo.ForeColor = System.Drawing.Color.Navy;
            this.lblVersionNo.Location = new System.Drawing.Point(216, 143);
            this.lblVersionNo.Name = "lblVersionNo";
            this.lblVersionNo.Size = new System.Drawing.Size(202, 23);
            this.lblVersionNo.TabIndex = 17;
            this.lblVersionNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(93, 146);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 20);
            this.label15.TabIndex = 16;
            this.label15.Text = "Version No.";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblApplicationName
            // 
            this.lblApplicationName.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationName.ForeColor = System.Drawing.Color.Navy;
            this.lblApplicationName.Location = new System.Drawing.Point(216, 111);
            this.lblApplicationName.Name = "lblApplicationName";
            this.lblApplicationName.Size = new System.Drawing.Size(309, 23);
            this.lblApplicationName.TabIndex = 15;
            this.lblApplicationName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(93, 115);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 17);
            this.label12.TabIndex = 14;
            this.label12.Text = "Application Name";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbpCompany
            // 
            this.tbpCompany.Controls.Add(this.btnFind);
            this.tbpCompany.Controls.Add(this.pctCompanyLogo);
            this.tbpCompany.Controls.Add(this.label4);
            this.tbpCompany.Controls.Add(this.txtContactNumber);
            this.tbpCompany.Controls.Add(this.label3);
            this.tbpCompany.Controls.Add(this.txtCompanyAddress);
            this.tbpCompany.Controls.Add(this.label2);
            this.tbpCompany.Controls.Add(this.txtCompanyName);
            this.tbpCompany.Controls.Add(this.label1);
            this.tbpCompany.Location = new System.Drawing.Point(4, 26);
            this.tbpCompany.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpCompany.Name = "tbpCompany";
            this.tbpCompany.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpCompany.Size = new System.Drawing.Size(573, 372);
            this.tbpCompany.TabIndex = 0;
            this.tbpCompany.Text = "Company";
            this.tbpCompany.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFind.Location = new System.Drawing.Point(397, 172);
            this.btnFind.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(70, 34);
            this.btnFind.TabIndex = 9;
            this.btnFind.Text = "Find";
            this.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // pctCompanyLogo
            // 
            this.pctCompanyLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctCompanyLogo.Location = new System.Drawing.Point(242, 172);
            this.pctCompanyLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pctCompanyLogo.Name = "pctCompanyLogo";
            this.pctCompanyLogo.Size = new System.Drawing.Size(149, 134);
            this.pctCompanyLogo.TabIndex = 7;
            this.pctCompanyLogo.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(95, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Company Logo";
            // 
            // txtContactNumber
            // 
            this.txtContactNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContactNumber.Location = new System.Drawing.Point(242, 139);
            this.txtContactNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtContactNumber.Name = "txtContactNumber";
            this.txtContactNumber.Size = new System.Drawing.Size(225, 25);
            this.txtContactNumber.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(94, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Contact Number";
            // 
            // txtCompanyAddress
            // 
            this.txtCompanyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyAddress.Location = new System.Drawing.Point(242, 78);
            this.txtCompanyAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCompanyAddress.Multiline = true;
            this.txtCompanyAddress.Name = "txtCompanyAddress";
            this.txtCompanyAddress.Size = new System.Drawing.Size(225, 53);
            this.txtCompanyAddress.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Company Address";
            // 
            // txtCompanyName
            // 
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyName.Location = new System.Drawing.Point(242, 45);
            this.txtCompanyName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(225, 25);
            this.txtCompanyName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Company Name";
            // 
            // tbpBackup
            // 
            this.tbpBackup.Controls.Add(this.txtRestoreMySqlAddress);
            this.tbpBackup.Controls.Add(this.label25);
            this.tbpBackup.Controls.Add(this.txtBackupPath);
            this.tbpBackup.Controls.Add(this.label14);
            this.tbpBackup.Controls.Add(this.txtBackupMySqlDumpAddress);
            this.tbpBackup.Controls.Add(this.label13);
            this.tbpBackup.Location = new System.Drawing.Point(4, 26);
            this.tbpBackup.Name = "tbpBackup";
            this.tbpBackup.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBackup.Size = new System.Drawing.Size(573, 372);
            this.tbpBackup.TabIndex = 5;
            this.tbpBackup.Text = "Back up";
            this.tbpBackup.UseVisualStyleBackColor = true;
            // 
            // txtRestoreMySqlAddress
            // 
            this.txtRestoreMySqlAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRestoreMySqlAddress.Location = new System.Drawing.Point(264, 91);
            this.txtRestoreMySqlAddress.Name = "txtRestoreMySqlAddress";
            this.txtRestoreMySqlAddress.Size = new System.Drawing.Size(217, 25);
            this.txtRestoreMySqlAddress.TabIndex = 86;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(82, 93);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(149, 17);
            this.label25.TabIndex = 85;
            this.label25.Text = "Restore My Sql Address";
            // 
            // txtBackupPath
            // 
            this.txtBackupPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBackupPath.Location = new System.Drawing.Point(264, 29);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.Size = new System.Drawing.Size(217, 25);
            this.txtBackupPath.TabIndex = 84;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(82, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 17);
            this.label14.TabIndex = 83;
            this.label14.Text = "Backup Path";
            // 
            // txtBackupMySqlDumpAddress
            // 
            this.txtBackupMySqlDumpAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBackupMySqlDumpAddress.Location = new System.Drawing.Point(264, 60);
            this.txtBackupMySqlDumpAddress.Name = "txtBackupMySqlDumpAddress";
            this.txtBackupMySqlDumpAddress.Size = new System.Drawing.Size(217, 25);
            this.txtBackupMySqlDumpAddress.TabIndex = 82;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(82, 62);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(180, 17);
            this.label13.TabIndex = 81;
            this.label13.Text = "Backup MySql Dump Address";
            // 
            // tbpDisplaySetting
            // 
            this.tbpDisplaySetting.Controls.Add(this.label10);
            this.tbpDisplaySetting.Controls.Add(this.txtMiscellaneousPercentage);
            this.tbpDisplaySetting.Controls.Add(this.label16);
            this.tbpDisplaySetting.Controls.Add(this.label8);
            this.tbpDisplaySetting.Controls.Add(this.txtInterestRatePercentage);
            this.tbpDisplaySetting.Controls.Add(this.label9);
            this.tbpDisplaySetting.Controls.Add(this.label7);
            this.tbpDisplaySetting.Controls.Add(this.txtProcessingFeePercentage);
            this.tbpDisplaySetting.Controls.Add(this.label6);
            this.tbpDisplaySetting.Controls.Add(this.btnFindScreenSaver);
            this.tbpDisplaySetting.Controls.Add(this.pctScreenSaver);
            this.tbpDisplaySetting.Controls.Add(this.label5);
            this.tbpDisplaySetting.Controls.Add(this.nudRecordLimit);
            this.tbpDisplaySetting.Controls.Add(this.label11);
            this.tbpDisplaySetting.Controls.Add(this.groupBox1);
            this.tbpDisplaySetting.Location = new System.Drawing.Point(4, 26);
            this.tbpDisplaySetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpDisplaySetting.Name = "tbpDisplaySetting";
            this.tbpDisplaySetting.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbpDisplaySetting.Size = new System.Drawing.Size(573, 372);
            this.tbpDisplaySetting.TabIndex = 1;
            this.tbpDisplaySetting.Text = "Display Settings";
            this.tbpDisplaySetting.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(282, 292);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 17);
            this.label10.TabIndex = 67;
            this.label10.Text = "%";
            // 
            // txtMiscellaneousPercentage
            // 
            this.txtMiscellaneousPercentage.Location = new System.Drawing.Point(211, 289);
            this.txtMiscellaneousPercentage.Name = "txtMiscellaneousPercentage";
            this.txtMiscellaneousPercentage.Size = new System.Drawing.Size(69, 25);
            this.txtMiscellaneousPercentage.TabIndex = 66;
            this.txtMiscellaneousPercentage.Text = "0.00";
            this.txtMiscellaneousPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(25, 292);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(159, 17);
            this.label16.TabIndex = 65;
            this.label16.Text = "Miscellaneous Percentage";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(282, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 17);
            this.label8.TabIndex = 64;
            this.label8.Text = "%";
            // 
            // txtInterestRatePercentage
            // 
            this.txtInterestRatePercentage.Location = new System.Drawing.Point(211, 258);
            this.txtInterestRatePercentage.Name = "txtInterestRatePercentage";
            this.txtInterestRatePercentage.Size = new System.Drawing.Size(69, 25);
            this.txtInterestRatePercentage.TabIndex = 63;
            this.txtInterestRatePercentage.Text = "0.00";
            this.txtInterestRatePercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 17);
            this.label9.TabIndex = 62;
            this.label9.Text = "Interest Rate Percentage";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(282, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 17);
            this.label7.TabIndex = 61;
            this.label7.Text = "%";
            // 
            // txtProcessingFeePercentage
            // 
            this.txtProcessingFeePercentage.Location = new System.Drawing.Point(211, 227);
            this.txtProcessingFeePercentage.Name = "txtProcessingFeePercentage";
            this.txtProcessingFeePercentage.Size = new System.Drawing.Size(69, 25);
            this.txtProcessingFeePercentage.TabIndex = 60;
            this.txtProcessingFeePercentage.Text = "0.00";
            this.txtProcessingFeePercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 230);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 17);
            this.label6.TabIndex = 59;
            this.label6.Text = "Processing Fee Percentage";
            // 
            // btnFindScreenSaver
            // 
            this.btnFindScreenSaver.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnFindScreenSaver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindScreenSaver.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFindScreenSaver.Image = ((System.Drawing.Image)(resources.GetObject("btnFindScreenSaver.Image")));
            this.btnFindScreenSaver.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFindScreenSaver.Location = new System.Drawing.Point(472, 36);
            this.btnFindScreenSaver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFindScreenSaver.Name = "btnFindScreenSaver";
            this.btnFindScreenSaver.Size = new System.Drawing.Size(70, 34);
            this.btnFindScreenSaver.TabIndex = 58;
            this.btnFindScreenSaver.Text = "Find";
            this.btnFindScreenSaver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFindScreenSaver.UseVisualStyleBackColor = false;
            this.btnFindScreenSaver.Click += new System.EventHandler(this.btnFindScreenSaver_Click);
            // 
            // pctScreenSaver
            // 
            this.pctScreenSaver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctScreenSaver.Location = new System.Drawing.Point(317, 36);
            this.pctScreenSaver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pctScreenSaver.Name = "pctScreenSaver";
            this.pctScreenSaver.Size = new System.Drawing.Size(149, 134);
            this.pctScreenSaver.TabIndex = 57;
            this.pctScreenSaver.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(314, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 56;
            this.label5.Text = "ScreenSaver";
            // 
            // nudRecordLimit
            // 
            this.nudRecordLimit.Location = new System.Drawing.Point(211, 184);
            this.nudRecordLimit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRecordLimit.Name = "nudRecordLimit";
            this.nudRecordLimit.Size = new System.Drawing.Size(69, 25);
            this.nudRecordLimit.TabIndex = 55;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 186);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(127, 17);
            this.label11.TabIndex = 54;
            this.label11.Text = "Display Record Limit";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbLeft);
            this.groupBox1.Controls.Add(this.rdbRight);
            this.groupBox1.Controls.Add(this.rdbBottom);
            this.groupBox1.Controls.Add(this.rdbTop);
            this.groupBox1.Location = new System.Drawing.Point(24, 23);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(229, 147);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " MDI Tab Alignment ";
            // 
            // rdbLeft
            // 
            this.rdbLeft.Location = new System.Drawing.Point(16, 58);
            this.rdbLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbLeft.Name = "rdbLeft";
            this.rdbLeft.Size = new System.Drawing.Size(55, 27);
            this.rdbLeft.TabIndex = 3;
            this.rdbLeft.TabStop = true;
            this.rdbLeft.Text = "Left";
            this.rdbLeft.UseVisualStyleBackColor = true;
            // 
            // rdbRight
            // 
            this.rdbRight.Location = new System.Drawing.Point(157, 59);
            this.rdbRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbRight.Name = "rdbRight";
            this.rdbRight.Size = new System.Drawing.Size(65, 27);
            this.rdbRight.TabIndex = 2;
            this.rdbRight.TabStop = true;
            this.rdbRight.Text = "Right";
            this.rdbRight.UseVisualStyleBackColor = true;
            // 
            // rdbBottom
            // 
            this.rdbBottom.Location = new System.Drawing.Point(83, 102);
            this.rdbBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbBottom.Name = "rdbBottom";
            this.rdbBottom.Size = new System.Drawing.Size(79, 27);
            this.rdbBottom.TabIndex = 1;
            this.rdbBottom.TabStop = true;
            this.rdbBottom.Text = "Bottom";
            this.rdbBottom.UseVisualStyleBackColor = true;
            // 
            // rdbTop
            // 
            this.rdbTop.Location = new System.Drawing.Point(83, 21);
            this.rdbTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbTop.Name = "rdbTop";
            this.rdbTop.Size = new System.Drawing.Size(57, 27);
            this.rdbTop.TabIndex = 0;
            this.rdbTop.TabStop = true;
            this.rdbTop.Text = "Top";
            this.rdbTop.UseVisualStyleBackColor = true;
            // 
            // SystemConfigurationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(605, 480);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SystemConfigurationUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Configuration";
            this.Load += new System.EventHandler(this.SystemConfigurationUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbpApplication.ResumeLayout(false);
            this.tbpCompany.ResumeLayout(false);
            this.tbpCompany.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctCompanyLogo)).EndInit();
            this.tbpBackup.ResumeLayout(false);
            this.tbpBackup.PerformLayout();
            this.tbpDisplaySetting.ResumeLayout(false);
            this.tbpDisplaySetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctScreenSaver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecordLimit)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpApplication;
        private System.Windows.Forms.TabPage tbpCompany;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.PictureBox pctCompanyLogo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtContactNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCompanyAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tbpDisplaySetting;
        private System.Windows.Forms.Button btnFindScreenSaver;
        private System.Windows.Forms.PictureBox pctScreenSaver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudRecordLimit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbLeft;
        private System.Windows.Forms.RadioButton rdbRight;
        private System.Windows.Forms.RadioButton rdbBottom;
        private System.Windows.Forms.RadioButton rdbTop;
        private System.Windows.Forms.TabPage tbpBackup;
        private System.Windows.Forms.TextBox txtRestoreMySqlAddress;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBackupMySqlDumpAddress;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblNoOfConcurrentUsers;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label lblNoOfLicenses;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label lblDevelopedBy;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblVersionNo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblApplicationName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMiscellaneousPercentage;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtInterestRatePercentage;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProcessingFeePercentage;
        private System.Windows.Forms.Label label6;
    }
}