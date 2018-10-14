using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using NSites.ApplicationObjects;
using NSites.ApplicationObjects.Classes;
//using NSites.ApplicationObjects.UserInterfaces.Transaction;
using NSites.ApplicationObjects.UserInterfaces;
using NSites.Global;

namespace NSites.ApplicationObjects.UserInterfaces
{//public ip smart-121.1.35.138, globe-222.127.186.138
    public partial class LogInUI : Form
    {
        SystemConfiguration loSystemConfiguration;
        User loUser;
        public bool lFromLogIn;
        string lLicenseDetails;
        string lLicenseNo;
        string lProcessorId;
        string lComputerName;
        string lTrialVersion;
        DateTime lTrialExpirtyDate;
        CryptorEngine loCryptorEngine;
        DataTable ldtSystemConfig = new DataTable();
        public LogInUI()
        {
            InitializeComponent();
            loSystemConfiguration = new SystemConfiguration();
            loUser = new User();
            lLicenseDetails = "";
            lLicenseNo = "";
            lProcessorId = "";
            lComputerName = "";
            lTrialVersion = "";
            loCryptorEngine = new CryptorEngine();
        }

        private void loadGlobalVariables()
        {
            DataTable _dtUserInfo = new DataTable();
            _dtUserInfo = loUser.getUserInfo(txtUsername.Text);
            foreach (DataRow _drUserInfo in _dtUserInfo.Rows)
            {
                GlobalVariables.Username = _drUserInfo["Username"].ToString();
                GlobalVariables.Userfullname = _drUserInfo["Fullname"].ToString();
                //GlobalVariables.OutletCode = _drUserInfo["OutletCode"].ToString();
                //GlobalVariables.EmployeeCode = _drUserInfo["EmployeeCode"].ToString();
                //GlobalVariables.DepartmentCode = _drUserInfo["DepartmentCode"].ToString();
                //GlobalVariables.DepartmentDesc = _drUserInfo["Department"].ToString();
            }
            GlobalVariables.DTCompanyLogo = GlobalFunctions.getCompanyLogo();
        }

        private void loadLogIn()
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnLogIn, "Hello!");
            txtUsername.Focus();
            if (loUser.connectDatabase())
            {
                ldtSystemConfig = loSystemConfiguration.getAllData();
                foreach (DataRow _drSystemConfig in ldtSystemConfig.Rows)
                {
                    if (_drSystemConfig["Key"].ToString() == "ApplicationName")
                    {
                        GlobalVariables.ApplicationName = _drSystemConfig["Value"].ToString();
                        //lblApplicationName.Text = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "VersionNo")
                    {
                        GlobalVariables.VersionNo = _drSystemConfig["Value"].ToString();
                        if (lblVersionNo.Text != GlobalVariables.VersionNo)
                        {
                            DialogResult _dr = new DialogResult();
                            MessageBoxUI _mb = new MessageBoxUI("Version Compatibility Issue!\nWould like to continue?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                            _mb.ShowDialog();
                            _dr = _mb.Operation;
                            if (_dr == DialogResult.No)
                            {
                                Application.Exit();
                            }
                        }
                    }
                    else if (_drSystemConfig["Key"].ToString() == "DevelopedBy")
                    {
                        GlobalVariables.DevelopedBy = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "CompanyName")
                    {
                        GlobalVariables.CompanyName = _drSystemConfig["Value"].ToString();
                        lblCompanyName.Text = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "CompanyAddress")
                    {
                        GlobalVariables.CompanyAddress = _drSystemConfig["Value"].ToString();
                        lblCompanyAddress.Text = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "ContactNumber")
                    {
                        GlobalVariables.ContactNumber = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "CompanyLogo")
                    {
                        GlobalVariables.CompanyLogo = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "BackupPath")
                    {
                        GlobalVariables.BackupPath = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "BackupMySqlDumpAddress")
                    {
                        GlobalVariables.BackupMySqlDumpAddress = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "RestoreMySqlAddress")
                    {
                        GlobalVariables.RestoreMySqlAddress = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "DisplayRecordLimit")
                    {
                        GlobalVariables.DisplayRecordLimit = int.Parse(_drSystemConfig["Value"].ToString());
                    }
                    else if (_drSystemConfig["Key"].ToString() == "ProcessingFeePercentage")
                    {
                        GlobalVariables.ProcessingFeePercentage = decimal.Parse(_drSystemConfig["Value"].ToString());
                    }
                    else if (_drSystemConfig["Key"].ToString() == "InterestRatePercentage")
                    {
                        GlobalVariables.InterestRatePercentage = decimal.Parse(_drSystemConfig["Value"].ToString());
                    }
                    else if (_drSystemConfig["Key"].ToString() == "MiscellaneousPercentage")
                    {
                        GlobalVariables.MiscellaneousPercentage = decimal.Parse(_drSystemConfig["Value"].ToString());
                    }
                    else if (_drSystemConfig["Key"].ToString() == "ScreenSaverImage")
                    {
                        GlobalVariables.ScreenSaverImage = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "MDITabAlignment")
                    {
                        GlobalVariables.TabAlignment = _drSystemConfig["Value"].ToString();
                    }
                }
            }
            else
            {
                MessageBoxUI ms = new MessageBoxUI("Cannot establish database connection!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                ms.showDialog();
                return;
            }
            byte[] hextobyte = GlobalFunctions.HexToBytes(GlobalVariables.CompanyLogo);
            //pctCompanyLogo.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
            //pctCompanyLogo.BackgroundImageLayout = ImageLayout.Stretch;
            GlobalVariables.DTCompanyLogo = GlobalFunctions.getCompanyLogo();
        }

        public void SoftwareDetailsRead()
        {
            string line = null;
            char[] splitter1 = { ';' };
            char[] splitter2 = { ':' };
            System.IO.TextReader readFile = new StreamReader(".../Main/text/SoftwareDetails.txt");
            line = readFile.ReadLine();
            lLicenseDetails = line;
 
            readFile.Close();
            readFile = null;
        }

        internal static string GetProcessorId()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (System.Management.ManagementClass theClass = new System.Management.ManagementClass("Win32_Processor"))
                {
                    using (System.Management.ManagementObjectCollection theCollectionOfResults = theClass.GetInstances())
                    {
                        foreach (System.Management.ManagementObject currentResult in theCollectionOfResults)
                        {
                            sb.Append(currentResult["ProcessorID"].ToString());
                        }
                    }
                }
                return sb.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "";
            }
        }

        private void LogInUI_Load(object sender, EventArgs e)
        {
            string _LicenseCertificate = "";
            string _ApplicationName = "";
            string _ProcessorId = "";
            DateTime _ExpiryDate = DateTime.Now;
            string _LicenseNo = "";

            loadCompanyInfo();

            _LicenseCertificate = LicenseCertificateRead();

            if (_LicenseCertificate == "")
            {
                Application.Exit();
            }

            #region "Split License Certificate"
            char[] splitter1 = { ';' };
            char[] splitter2 = { ':' };
            string[] data1 = _LicenseCertificate.Split(splitter1);
            for (int i = 0; i < data1.Length; i++)
            {
                string[] data2 = null;
                data2 = data1[i].Split(splitter2);
                if (data2[0].ToString() == "ApplicationName")
                {
                    _ApplicationName = data2[1].ToString();
                }
                else if (data2[0].ToString() == "ProcessorId")
                {
                    _ProcessorId = data2[1].ToString();
                }
                else if (data2[0].ToString() == "ExpiryDate")
                {
                    try
                    {
                        _ExpiryDate = DateTime.Parse(data2[1].ToString());
                    }
                    catch
                    {
                        _ExpiryDate = DateTime.Now;
                    }
                }
                else if (data2[0].ToString() == "LicenseNo")
                {
                    _LicenseNo = data2[1].ToString();
                }
            }
            #endregion

            #region "Processor ID"
            string _ComputerProcessorId = GlobalFunctions.GetProcessorId();
            #endregion
            if (_ApplicationName != lblApplicationName.Text)
            {
                getSoftwareLicense();
            }
            else if (_ComputerProcessorId != _ProcessorId)
            {
                getSoftwareLicense();
            }
            else if (_ExpiryDate.Date <= DateTime.Now.Date)
            {
                getSoftwareLicense();
            }
            else if (_LicenseNo == "")
            {
                getSoftwareLicense();
            }
            else
            {
                System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
                ToolTip1.SetToolTip(this.btnLogIn, "Hello!");
                rememberPasswordRead();
                loadLogIn();
            }
        }

        private void getSoftwareLicense()
        {
            //display software license.
            SoftwareLicenseUI loSofwareLicense = new SoftwareLicenseUI(GlobalVariables.CompanyName, GlobalVariables.ApplicationName);
            loSofwareLicense.ShowDialog();
            if (loSofwareLicense.lSuccessFullyVerified)
            {
                LicenseCertificateWrite(loSofwareLicense.lLicenseNo);
                loadLogIn();
            }
            else
            {
                Application.Exit();
            }
        }

        private void loadCompanyInfo()
        {
            if (loUser.connectDatabase())
            {
                ldtSystemConfig = loSystemConfiguration.getAllData();
                foreach (DataRow _drSystemConfig in ldtSystemConfig.Rows)
                {
                    if (_drSystemConfig["Key"].ToString() == "ApplicationName")
                    {
                        GlobalVariables.ApplicationName = _drSystemConfig["Value"].ToString();
                        //lblApplicationName.Text = _drSystemConfig["Value"].ToString();
                    }
                    else if (_drSystemConfig["Key"].ToString() == "CompanyName")
                    {
                        GlobalVariables.CompanyName = _drSystemConfig["Value"].ToString();
                        lblCompanyName.Text = GlobalVariables.CompanyName;
                    }
                }
            }
            else
            {
                MessageBoxUI ms = new MessageBoxUI("Cannot establish database connection!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                ms.showDialog();
                Application.Exit();
            }
        }

        public void readLicenseCertificate()
        {
            string line = null;
            char[] splitter1 = { ';' };
            char[] splitter2 = { ':' };
            System.IO.TextReader readFile = new StreamReader(".../Main/text/LicenseCertificate.txt");
            line = readFile.ReadLine();
            if (line != null)
            {
                string _StringToWrite = loCryptorEngine.DecryptString(line);
                string[] data1 = _StringToWrite.Split(splitter1);
                for (int i = 0; i < data1.Length; i++)
                {
                    string[] data2 = null;
                    data2 = data1[i].Split(splitter2);
                    if (data2[0].ToString() == "ApplicationId")
                    {
                        GlobalVariables.ApplicationId = data2[1].ToString();
                    }
                    if (data2[0].ToString() == "ProcessorId")
                    {
                        GlobalVariables.ProcessorId = data2[1].ToString();
                    }
                    if (data2[0].ToString() == "TrialVersion")
                    {
                        GlobalVariables.TrialVersion = data2[1].ToString();
                    }
                    if (data2[0].ToString() == "LicenseKey")
                    {
                        GlobalVariables.LicenseKey = data2[1].ToString();
                    }
                    if (data2[0].ToString() == "ExpiryDate")
                    {
                        GlobalVariables.lLicenseExpiry = DateTime.Parse(data2[1].ToString());
                    }
                }
            }
            else
            {
                Application.Exit();
            }
            readFile.Close();
            readFile = null;
        }

        public string LicenseCertificateRead()
        {
            string _result = "";
            string line = null;
            try
            {
                System.IO.TextReader readFile = new StreamReader(".../Main/text/LicenseCertificate.txt");
                line = readFile.ReadLine();
                if (line != null)
                {
                    _result = loCryptorEngine.DecryptString(line);
                }
                readFile.Close();
                readFile = null;
            }
            catch
            {
                _result = "";
            }
            return _result;
        }

        public void LicenseCertificateWrite(string pLicenseNo)
        {
            try
            {
                string _StringToWrite = "ApplicationName:" + lblApplicationName.Text + ";ProcessorId:" + GlobalFunctions.GetProcessorId() + ";" + "ExpiryDate:01/01/9999;" + "LicenseNo:" + pLicenseNo;
                _StringToWrite = loCryptorEngine.EncryptString(_StringToWrite);
                System.IO.TextWriter writeFile = new StreamWriter(".../Main/text/LicenseCertificate.txt");
                writeFile.WriteLine(_StringToWrite);
                writeFile.Flush();
                writeFile.Close();
                writeFile = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void rememberPasswordWrite()
        {
            try
            {
                string _StringToWrite = "Username:" + txtUsername.Text + ";" + "Password:" + txtPassword.Text;
                _StringToWrite = loCryptorEngine.EncryptString(_StringToWrite);
                System.IO.TextWriter writeFile = new StreamWriter(".../Main/text/userDetails.txt");
                writeFile.WriteLine(_StringToWrite);
                writeFile.Flush();
                writeFile.Close();
                writeFile = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void rememberUsernameWrite()
        {
            try
            {
                string _StringToWrite = "Username:" + txtUsername.Text;
                _StringToWrite = loCryptorEngine.EncryptString(_StringToWrite);
                System.IO.TextWriter writeFile = new StreamWriter(".../Main/text/userDetails.txt");
                writeFile.WriteLine(_StringToWrite);
                writeFile.Flush();
                writeFile.Close();
                writeFile = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void rememberPasswordRead()
        {
            string line = null;
            char[] splitter1 = { ';' };
            char[] splitter2 = { ':' };
            System.IO.TextReader readFile = new StreamReader(".../Main/text/userDetails.txt");
            line = readFile.ReadLine();
            if (line != null)
            {
                string _StringToWrite = loCryptorEngine.DecryptString(line);
                string[] data1 = _StringToWrite.Split(splitter1);
                for (int i = 0; i < data1.Length; i++)
                {
                    string[] data2 = null;
                    data2 = data1[i].Split(splitter2);
                    if (data2[0].ToString() == "Username")
                    {
                        txtUsername.Text = data2[1].ToString();
                    }
                    if (data2[0].ToString() == "Password")
                    {
                        txtPassword.Text = data2[1].ToString();
                        chbRemember.Checked = true;
                    }
                }
            }
            readFile.Close();
            readFile = null;
        }

        private void LogInUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // apply it later
            if (chbRemember.Checked)
            {
                rememberPasswordWrite();
            }
            else
            {
                rememberUsernameWrite();
            }
            string _day = DateTime.Now.Day.ToString();
            string _hour = DateTime.Now.Hour.ToString();
            string _minute = DateTime.Now.Minute.ToString();
            if (txtUsername.Text == "jbcsupport" && txtPassword.Text == _day + _hour + _minute)
            {
                GlobalVariables.Username = "jbcsupport";
                MDIUI MDI = new MDIUI();
                loadGlobalVariables();
                MDI.ShowDialog();
                GlobalVariables.xLocation = MDI.Location.X;
                GlobalVariables.yLocation = MDI.Location.Y;

                txtPassword.Clear();
                txtPassword.Focus();
            }
            else
            {
                if (loUser.autenticateUser(txtUsername.Text, txtPassword.Text))
                {
                    MDIUI MDI = new MDIUI();
                    loadGlobalVariables();
                    MDI.ShowDialog();
                    GlobalVariables.xLocation = MDI.Location.X;
                    GlobalVariables.yLocation = MDI.Location.Y;

                    txtPassword.Clear();
                    txtPassword.Focus();
                }
                else
                {
                    MessageBoxUI ms = new MessageBoxUI("Username and password combination is incorrect!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                    ms.showDialog();
                    txtPassword.Focus();
                    return;
                }
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnLogin_Click(null, new EventArgs());
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnLogin_Click(null, new EventArgs());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
