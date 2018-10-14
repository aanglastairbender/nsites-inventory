using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using System.Diagnostics;

using NSites.Global;
using NSites.ApplicationObjects.Classes;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class BackupRestoreDataUI : Form
    {
        #region "VARIABLES"
        CryptorEngine loCryptoEngine;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTOR"
        public BackupRestoreDataUI()
        {
            InitializeComponent();
            loCryptoEngine = new CryptorEngine();
        }
        #endregion "END OF CONSTRUCTOR"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
       
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;

                //Save file to C:\ with the current date as a filename
                string path;
                path = txtSaveToURL.Text + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + ".sql";
                StreamWriter file = new StreamWriter(path);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = GlobalVariables.BackupMySqlDumpAddress;
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    txtBackupUserId.Text, txtBackupPassword.Text, txtBackupServer.Text, txtBackupDatabase.Text);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
                MessageBoxUI mb = new MessageBoxUI("Database has been backup successfully!", GlobalVariables.Icons.Ok, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            catch (IOException ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.Close);
                mb.ShowDialog();
                return;
            }
        }
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = txtSqlFileAddress.Text;//.../Main/MySqlBackUp/POSBackup2012-1-6-18-44-57-353.sql
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = GlobalVariables.RestoreMySqlAddress;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                     txtBackupUserId.Text, txtBackupPassword.Text, txtBackupServer.Text, txtBackupDatabase.Text);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
                MessageBoxUI mb = new MessageBoxUI("Database has been successfully restored!", GlobalVariables.Icons.Ok, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            catch (IOException ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.Close);
                mb.ShowDialog();
            }
        }

        #endregion "END OF METHODS"

        #region "EVENTS"

        private void ImportDataUI_Load(object sender, EventArgs e)
        {
            string line = null;
            System.IO.TextReader readFile = new StreamReader("...\\databaseconnection.txt");
            line = readFile.ReadLine();
            if (line != null)
            {
                string _StringToRead = loCryptoEngine.DecryptString(line);
                line = _StringToRead;
            }
            readFile.Close();
            readFile = null;

            if (line != null)
            {
                string[] backuplocation = line.Split(';');
                for (int i = 0; i < backuplocation.Length; i++)
                {
                    string[] item;
                    switch (i)
                    {
                        case 0:
                            item = backuplocation[i].Split('=');
                            txtBackupServer.Text = item[1].ToString();
                            txtRestoreServer.Text = item[1].ToString();
                            break;
                        case 1:
                            item = backuplocation[i].Split('=');
                            txtBackupDatabase.Text = item[1].ToString();
                            txtRestoreDatabase.Text = item[1].ToString();
                            break;
                        case 2:
                            item = backuplocation[i].Split('=');
                            txtBackupUserId.Text = item[1].ToString();
                            txtRestoreUserId.Text = item[1].ToString();
                            break;
                        case 3:
                            item = backuplocation[i].Split('=');
                            txtBackupPassword.Text = item[1].ToString();
                            txtRestorePassword.Text = item[1].ToString();
                            break;
                        case 4:
                            item = backuplocation[i].Split('=');
                            txtBackupPort.Text = item[1].ToString();
                            txtRestorePort.Text = item[1].ToString();
                            break;
                    }
                }
            }

            txtSaveToURL.Text = GlobalVariables.BackupPath;
            string pages = tbcBackupRestore.SelectedTab.Text;
            switch (pages)
            {
                case "Backup":
                    btnBackupDatabase.Visible = true;
                    btnRestoreDatabase.Visible = false;
                    break;
                case "Restore":
                    btnRestoreDatabase.Visible = true;
                    btnBackupDatabase.Visible = false;
                    break;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
            }
            catch
            {
                this.Close();
            }
        }
        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmBackupRestore", "Backup"))
            {
                return;
            }
            
            pctBackup1.Visible = false;
            pctBackup2.Visible = false;
            pctBackup3.Visible = false;
            pctBackup4.Visible = false;
            if (txtSaveToURL.Text == "")
            {
                pctBackup1.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("URL to save the file must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtSaveToURL.Focus();
                return;
            }
            if (txtBackupServer.Text == "")
            {
                pctBackup2.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Server IP or Hostname must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtBackupServer.Focus();
                return;
            }
            if (txtBackupDatabase.Text == "")
            {
                pctBackup3.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Database name must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtBackupDatabase.Focus();
                return;
            }
            if (txtBackupUserId.Text == "")
            {
                pctBackup4.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Database user Id must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtBackupUserId.Focus();
                return;
            }
            Backup();
        }
        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmBackupRestore", "Restore"))
            {
                return;
            }
            
            pctRestore1.Visible = false;
            pctRestore2.Visible = false;
            pctRestore3.Visible = false;
            pctRestore4.Visible = false;
            if (txtSqlFileAddress.Text == "")
            {
                pctRestore1.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Sql file address must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtSqlFileAddress.Focus();
                return;
            }
            if (txtRestoreServer.Text == "")
            {
                pctRestore2.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Server IP or Hostname must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtRestoreServer.Focus();
                return;
            }
            if (txtRestoreDatabase.Text == "")
            {
                pctRestore3.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Database name must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtRestoreDatabase.Focus();
                return;
            }
            if (txtRestoreUserId.Text == "")
            {
                pctRestore4.Visible = true;
                MessageBoxUI mb = new MessageBoxUI("Database user Id must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                txtRestoreUserId.Focus();
                return;
            }
            Restore();
        }
        private void tbcBackupRestore_Selected(object sender, TabControlEventArgs e)
        {
            string pages = tbcBackupRestore.SelectedTab.Text;
            switch (pages)
            {
                case "Backup":
                    btnBackupDatabase.Visible = true;
                    btnRestoreDatabase.Visible = false;
                    break;
                case "Restore":
                    btnRestoreDatabase.Visible = true;
                    btnBackupDatabase.Visible = false;
                    break;
            }
        }

        private void btnOpenRestoreFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog();
            openFD.InitialDirectory = ".../Main/MySqlBackUp/";
            openFD.Title = "Insert an SQL file";
            openFD.Filter = "SQL Files|*.sql";
            if (openFD.ShowDialog() == DialogResult.Cancel)
            {
                MessageBoxUI mb = new MessageBoxUI("Operation Cancelled", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            else
            {
                txtSqlFileAddress.Text = openFD.FileName;
            }
        }
        #endregion "END OF EVENTS"

        private void btnCheckTableUpdates_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmBackupRestore", "Check Table Updates"))
            {
                return;
            }
            TechnicalUpdateUI loCheckTableUpdate = new TechnicalUpdateUI();
            loCheckTableUpdate.ShowDialog();
        }
    }
}
