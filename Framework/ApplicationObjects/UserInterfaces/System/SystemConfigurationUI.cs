using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;

using NSites.Global;
using NSites.ApplicationObjects.Classes;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class SystemConfigurationUI : Form
    {
        #region "VARIABLES"
        public GlobalVariables.Operation lOperation;
        SystemConfiguration loSystemConfiguration;
        Hashtable lSystemConfigHash;
        string lCompanyLogo = "";
        string lScreenSaverImage = "";
        string lOutletCode;
        string lAddress;
        LookUpValueUI loLookupValue;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public SystemConfigurationUI()
        {
            InitializeComponent();
            lSystemConfigHash = new Hashtable();
            loSystemConfiguration = new SystemConfiguration();
            loLookupValue = new LookUpValueUI();
        }
        #endregion "END OF CONSTRUCTORS"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        private void loadDataToHash()
        {
            lSystemConfigHash.Clear();
            lSystemConfigHash.Add("ApplicationName", lblApplicationName.Text);
            lSystemConfigHash.Add("VersionNo", lblVersionNo.Text);
            lSystemConfigHash.Add("DevelopedBy", lblDevelopedBy.Text);
            lSystemConfigHash.Add("CompanyName", txtCompanyName.Text);
            lSystemConfigHash.Add("CompanyAddress", txtCompanyAddress.Text);
            lSystemConfigHash.Add("ContactNumber", txtContactNumber.Text);
            lSystemConfigHash.Add("CompanyLogo", lCompanyLogo);
            lSystemConfigHash.Add("BackupPath", txtBackupPath.Text);
            lSystemConfigHash.Add("BackupMySqlDumpAddress", txtBackupMySqlDumpAddress.Text);
            lSystemConfigHash.Add("RestoreMySqlAddress", txtRestoreMySqlAddress.Text);
            lSystemConfigHash.Add("DisplayRecordLimit", nudRecordLimit.Value.ToString());
            lSystemConfigHash.Add("ProcessingFeePercentage", txtProcessingFeePercentage.Text);
            lSystemConfigHash.Add("InterestRatePercentage", txtInterestRatePercentage.Text);
            lSystemConfigHash.Add("MiscellaneousPercentage", txtMiscellaneousPercentage.Text);
            lSystemConfigHash.Add("ScreenSaverImage", lScreenSaverImage);
            if (rdbBottom.Checked)
            {
                lSystemConfigHash.Add("MDITabAlignment", "Bottom");
            }
            else if (rdbLeft.Checked)
            {
                lSystemConfigHash.Add("MDITabAlignment", "Left");
            }
            else if (rdbRight.Checked)
            {
                lSystemConfigHash.Add("MDITabAlignment", "Right");
            }
            else if (rdbTop.Checked)
            {
                lSystemConfigHash.Add("MDITabAlignment", "Top");
            }
            else
            {
                lSystemConfigHash.Add("MDITabAlignment", "Top");
            }
        }

        public void refresh()
        {
            char[] _params = { ' ' };
            string[] _Date = DateTime.Now.ToString().Split(_params);
            DataTable _dt = new DataTable();
            _dt = loSystemConfiguration.getAllData();
            foreach (DataRow _dr in _dt.Rows)
            {
                if (_dr["Key"].ToString() == "ApplicationName")
                {
                    lblApplicationName.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "VersionNo")
                {
                    lblVersionNo.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "DevelopedBy")
                {
                    lblDevelopedBy.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "NoOfConcurrentUsers")
                {
                    lblNoOfConcurrentUsers.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "NoOfLicenses")
                {
                    lblNoOfLicenses.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "CompanyName")
                {
                    txtCompanyName.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "CompanyAddress")
                {
                    txtCompanyAddress.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "ContactNumber")
                {
                    txtContactNumber.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "CompanyLogo")
                {
                    try
                    {
                        lCompanyLogo = _dr["Value"].ToString();
                        byte[] hextobyte = GlobalFunctions.HexToBytes(lCompanyLogo);
                        pctCompanyLogo.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                        pctCompanyLogo.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch { }
                }
                else if (_dr["Key"].ToString() == "BackupPath")
                {
                    txtBackupPath.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "BackupMySqlDumpAddress")
                {
                    txtBackupMySqlDumpAddress.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "RestoreMySqlAddress")
                {
                    txtRestoreMySqlAddress.Text = _dr["Value"].ToString();
                }
                else if (_dr["Key"].ToString() == "DisplayRecordLimit")
                {
                    nudRecordLimit.Value = int.Parse(_dr["Value"].ToString());
                }
                else if (_dr["Key"].ToString() == "ProcessingFeePercentage")
                {
                    txtProcessingFeePercentage.Text = string.Format("{0:n}", decimal.Parse(_dr["Value"].ToString()));
                }
                else if (_dr["Key"].ToString() == "InterestRatePercentage")
                {
                    txtInterestRatePercentage.Text = string.Format("{0:n}", decimal.Parse(_dr["Value"].ToString()));
                }
                else if (_dr["Key"].ToString() == "MiscellaneousPercentage")
                {
                    txtMiscellaneousPercentage.Text = string.Format("{0:n}", decimal.Parse(_dr["Value"].ToString()));
                }
                else if (_dr["Key"].ToString() == "ScreenSaverImage")
                {
                    try
                    {
                        lScreenSaverImage = _dr["Value"].ToString();
                        byte[] hextobyte = GlobalFunctions.HexToBytes(lScreenSaverImage);
                        pctScreenSaver.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                        pctScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch { }
                }
                else if (_dr["Key"].ToString() == "MDITabAlignment")
                {
                    if (_dr["Value"].ToString() == "Top")
                    {
                        rdbTop.Checked = true;
                    }
                    else if (_dr["Value"].ToString() == "Bottom")
                    {
                        rdbBottom.Checked = true;
                    }
                    else if (_dr["Value"].ToString() == "Left")
                    {
                        rdbLeft.Checked = true;
                    }
                    else if (_dr["Value"].ToString() == "Right")
                    {
                        rdbRight.Checked = true;
                    }
                    else
                    {
                        rdbTop.Checked = true;
                    }
                }
            }
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void SystemConfigurationUI_Load(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmSystemConfiguration", "Save"))
            {
                return;
            }
            try
            {
                lOperation = GlobalVariables.Operation.Edit;
                loadDataToHash();
                foreach (DictionaryEntry Hash in lSystemConfigHash)
                {
                    try
                    {
                        loSystemConfiguration.Key = Hash.Key.ToString();
                        loSystemConfiguration.Value = Hash.Value.ToString();
                    }
                    catch { }
                    loSystemConfiguration.saveSystemConfiguration(lOperation);

                }
                MessageBoxUI _mb = new MessageBoxUI("System Configuration has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                _mb.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBoxUI _mb = new MessageBoxUI(ex.Message, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }
        #endregion "END OF EVENTS"

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = ".../Main/ScreenSaverImages/";
                openFD.Title = "Insert an Image";
                openFD.Filter = "JPEG Images|*.jpg|GIF Images|*.gif";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBoxUI mb = new MessageBoxUI("Operation Cancelled", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    string _ChosenFile = openFD.FileName;
                    string _FileName = openFD.SafeFileName;

                    byte[] m_Bitmap = null;

                    FileStream fs = new FileStream(_ChosenFile, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    br.Close();
                    fs.Close();

                    lCompanyLogo = GlobalFunctions.ToHex(m_Bitmap);

                    pctCompanyLogo.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(m_Bitmap);
                    pctCompanyLogo.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                return;
            }
        }

        private void btnFindScreenSaver_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFD = new OpenFileDialog();
                openFD.InitialDirectory = ".../Main/ScreenSaverImages/";
                openFD.Title = "Insert an Image";
                openFD.Filter = "JPEG Images|*.jpg|GIF Images|*.gif";
                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBoxUI mb = new MessageBoxUI("Operation Cancelled", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                }
                else
                {
                    string _ChosenFile = openFD.FileName;
                    string _FileName = openFD.SafeFileName;

                    byte[] m_Bitmap = null;

                    FileStream fs = new FileStream(_ChosenFile, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)br.BaseStream.Length;
                    m_Bitmap = new byte[length];
                    m_Bitmap = br.ReadBytes(length);
                    br.Close();
                    fs.Close();

                    lScreenSaverImage = GlobalFunctions.ToHex(m_Bitmap);

                    pctScreenSaver.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(m_Bitmap);
                    pctScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                return;
            }
        }
    }
}
