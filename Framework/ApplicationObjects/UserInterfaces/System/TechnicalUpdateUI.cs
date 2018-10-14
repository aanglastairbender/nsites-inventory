using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using NSites.Global;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class TechnicalUpdateUI : Form
    {
        MySqlConnection connNew;
        MySqlConnection connCurrent;
        public TechnicalUpdateUI()
        {
            InitializeComponent();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void CheckTableUpdateUI_Load(object sender, EventArgs e)
        {
            txtServer.Text = GlobalVariables.DatabaseServer;
            txtCurrentDatabase.Text = GlobalVariables.DatabaseName;
            txtNewDatabase.Text = GlobalVariables.DatabaseName+"_update";
            txtUserId.Text = GlobalVariables.DatabaseUID;
            txtPassword.Text = GlobalVariables.DatabasePWD;
            txtPort.Text = GlobalVariables.DatabasePort;

            try
            {
                
                string connstringNew = "SERVER=" + txtServer.Text + "; DATABASE=" + txtNewDatabase.Text + "; UID=" + txtUserId.Text + "; PWD=" + txtPassword.Text + "; PORT=" + txtPort.Text;
                connNew = new MySqlConnection(connstringNew);
                connNew.Open();
                
                string connstringOld = "SERVER=" + txtServer.Text + "; DATABASE=" + txtCurrentDatabase.Text + "; UID=" + txtUserId.Text + "; PWD=" + txtPassword.Text + "; PORT=" + txtPort.Text;
                connCurrent = new MySqlConnection(connstringOld);
                connCurrent.Open();
            }
            catch
            {
                MessageBox.Show("Cannot Connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCheckTableFields_Click(object sender, EventArgs e)
        {
            dgvCheckTables.Rows.Clear();
            dgvSequence.Rows.Clear();
            dgvLookUpValue.Rows.Clear();
            dgvMenuItems.Rows.Clear();
            dgvItemRights.Rows.Clear();
            dgvSystemConfiguration.Rows.Clear();
            DataTable _dtNew = new DataTable();
            DataTable _dtOld = new DataTable();
            DataTable _dtNewSequence = new DataTable();
            DataTable _dtOldSequence = new DataTable();
            DataTable _dtNewLookUpValue = new DataTable();
            DataTable _dtOldLookUpValue = new DataTable();
            DataTable _dtNewMenuItem = new DataTable();
            DataTable _dtOldMenuItem = new DataTable();
            DataTable _dtNewItemRights = new DataTable();
            DataTable _dtOldItemRights = new DataTable();
            DataTable _dtNewSystemConfiguration = new DataTable();
            DataTable _dtOldSystemConfiguration = new DataTable();
            try
            {
                MySqlDataAdapter _daNew = new MySqlDataAdapter("SELECT Table_Name,Column_Name,Column_Type FROM information_schema.COLUMNS WHERE Table_Schema = '" + txtNewDatabase.Text + "' ORDER BY Table_Name ASC;", connNew);
                _daNew.Fill(_dtNew);
                MySqlDataAdapter _daOld = new MySqlDataAdapter("SELECT Table_Name,Column_Name,Column_Type FROM information_schema.COLUMNS WHERE Table_Schema = '" + txtCurrentDatabase.Text + "' ORDER BY Table_Name ASC;", connNew);
                _daOld.Fill(_dtOld);
                //sequence
                MySqlDataAdapter _daNewSequence = new MySqlDataAdapter("SELECT `Table`,OutletCode,Prefix,`Length`,LastNumber FROM sequence WHERE `Status` = 'Active';", connNew);
                _daNewSequence.Fill(_dtNewSequence);
                MySqlDataAdapter _daOldSequence = new MySqlDataAdapter("SELECT `Table`,OutletCode,Prefix,`Length`,LastNumber FROM sequence WHERE `Status` = 'Active';", connCurrent);
                _daOldSequence.Fill(_dtOldSequence);
                //look up Value
                MySqlDataAdapter _daNewLookUp = new MySqlDataAdapter("SELECT `Key`,`Value` FROM lookuptable WHERE `Status` = 'Active';", connNew);
                _daNewLookUp.Fill(_dtNewLookUpValue);
                MySqlDataAdapter _daOldLookUp = new MySqlDataAdapter("SELECT `Key`,`Value` FROM lookuptable WHERE `Status` = 'Active';", connCurrent);
                _daOldLookUp.Fill(_dtOldLookUpValue);
                //menu item
                MySqlDataAdapter _daNewMenuItem = new MySqlDataAdapter("SELECT MenuName,MenuText,ItemName,ItemText,MenuSeqNo,ItemSeqNo FROM menuitem ORDER BY MenuSeqNo,ItemSeqNo,MenuName;", connNew);
                _daNewMenuItem.Fill(_dtNewMenuItem);
                MySqlDataAdapter _daOldMenuItem = new MySqlDataAdapter("SELECT MenuName,MenuText,ItemName,ItemText,MenuSeqNo,ItemSeqNo FROM menuitem ORDER BY MenuSeqNo,ItemSeqNo,MenuName;", connCurrent);
                _daOldMenuItem.Fill(_dtOldMenuItem);
                //item rights
                MySqlDataAdapter _daNewItemRights = new MySqlDataAdapter("SELECT ItemName,Rights,RightsSeqNo FROM itemrights;", connNew);
                _daNewItemRights.Fill(_dtNewItemRights);
                MySqlDataAdapter _daOldItemRights = new MySqlDataAdapter("SELECT ItemName,Rights,RightsSeqNo FROM itemrights;", connCurrent);
                _daOldItemRights.Fill(_dtOldItemRights);
                //system configuration
                MySqlDataAdapter _daNewSystemConfiguration = new MySqlDataAdapter("SELECT `Key`,`Value`,OutletCode FROM systemconfiguration where `Status`='Active';", connNew);
                _daNewSystemConfiguration.Fill(_dtNewSystemConfiguration);
                MySqlDataAdapter _daOldSystemConfiguration = new MySqlDataAdapter("SELECT `Key`,`Value`,OutletCode FROM systemconfiguration where `Status`='Active';", connCurrent);
                _daOldSystemConfiguration.Fill(_dtOldSystemConfiguration);
            }
            catch { }

            #region "TABLES"
            foreach (DataRow _dr in _dtNew.Rows)
            {
                int n = dgvCheckTables.Rows.Add();
                dgvCheckTables.Rows[n].Cells[0].Value = _dr["Table_Name"].ToString();
                dgvCheckTables.Rows[n].Cells[1].Value = _dr["Column_Name"].ToString();
                dgvCheckTables.Rows[n].Cells[2].Value = _dr["Column_Type"].ToString();
            }

            int i = 0;
            int error = 0;
            string _status;
            foreach (DataRow _dr in _dtOld.Rows)
            {
                _status = "";
                try
                {
                    dgvCheckTables.Rows[i].Cells[4].Value = _dr["Table_Name"].ToString();
                    dgvCheckTables.Rows[i].Cells[5].Value = _dr["Column_Name"].ToString();
                    dgvCheckTables.Rows[i].Cells[6].Value = _dr["Column_Type"].ToString();

                    if (dgvCheckTables.Rows[i].Cells[0].Value.ToString() != _dr["Table_Name"].ToString())
                    {
                        _status = ";TableName";
                    }
                    if (dgvCheckTables.Rows[i].Cells[1].Value.ToString() != _dr["Column_Name"].ToString())
                    {
                        _status = _status + ";Fields";
                    }
                    if (dgvCheckTables.Rows[i].Cells[2].Value.ToString() != _dr["Column_Type"].ToString())
                    {
                        _status = _status + ";DataType";
                    }

                    if (_status != "")
                    {
                        error++;
                        dgvCheckTables.Rows[i].Cells[7].Value = "Error!" + _status;
                    }
                    i++;
                }
                catch
                {
                    int n = dgvCheckTables.Rows.Add();
                    dgvCheckTables.Rows[n].Cells[0].Value = "";
                    dgvCheckTables.Rows[n].Cells[1].Value = "";
                    dgvCheckTables.Rows[n].Cells[2].Value = "";
                    dgvCheckTables.Rows[n].Cells[3].Value = "";
                    dgvCheckTables.Rows[n].Cells[4].Value = _dr["Table_Name"].ToString();
                    dgvCheckTables.Rows[n].Cells[5].Value = _dr["Column_Name"].ToString();
                    dgvCheckTables.Rows[n].Cells[6].Value = _dr["Column_Type"].ToString();
                    dgvCheckTables.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (error > 0)
            {
                lblErrorTable.ForeColor = Color.Red;
            }
            lblErrorTable.Text = "Table Error(s) : " + error.ToString();
            #endregion

            #region "SEQUENCES"
            foreach (DataRow _dr in _dtNewSequence.Rows)
            {
                int n = dgvSequence.Rows.Add();
                dgvSequence.Rows[n].Cells[0].Value = _dr["Table"].ToString();
                dgvSequence.Rows[n].Cells[1].Value = _dr["OutletCode"].ToString();
                dgvSequence.Rows[n].Cells[2].Value = _dr["Prefix"].ToString();
                dgvSequence.Rows[n].Cells[3].Value = _dr["Length"].ToString();
                dgvSequence.Rows[n].Cells[4].Value = _dr["LastNumber"].ToString();
            }

            int j = 0;
            int errorSequence = 0;
            string _statusSequence;
            foreach (DataRow _dr in _dtOldSequence.Rows)
            {
                _statusSequence = "";
                try
                {
                    dgvSequence.Rows[j].Cells[6].Value = _dr["Table"].ToString();
                    dgvSequence.Rows[j].Cells[7].Value = _dr["OutletCode"].ToString();
                    dgvSequence.Rows[j].Cells[8].Value = _dr["Prefix"].ToString();
                    dgvSequence.Rows[j].Cells[9].Value = _dr["Length"].ToString();
                    dgvSequence.Rows[j].Cells[10].Value = _dr["LastNumber"].ToString();

                    if (dgvSequence.Rows[j].Cells[0].Value.ToString() != _dr["Table"].ToString())
                    {
                        _statusSequence = ";Table";
                    }
                    if (dgvSequence.Rows[j].Cells[1].Value.ToString() != _dr["OutletCode"].ToString())
                    {
                        _statusSequence = _statusSequence + ";OutletCode";
                    }
                    if (dgvSequence.Rows[j].Cells[2].Value.ToString() != _dr["Prefix"].ToString())
                    {
                        _statusSequence = _statusSequence + ";Prefix";
                    }
                    if (dgvSequence.Rows[j].Cells[3].Value.ToString() != _dr["Length"].ToString())
                    {
                        _statusSequence = _statusSequence + ";Length";
                    }
                    if (dgvSequence.Rows[j].Cells[4].Value.ToString() != _dr["LastNumber"].ToString())
                    {
                        _statusSequence = _statusSequence + ";LastNumber";
                    }

                    if (_statusSequence != "")
                    {
                        errorSequence++;
                        dgvSequence.Rows[j].Cells[11].Value = "Error!" + _statusSequence;
                    }

                    j++;
                }
                catch
                {
                    int n = dgvSequence.Rows.Add();
                    dgvSequence.Rows[n].Cells[0].Value = "";
                    dgvSequence.Rows[n].Cells[1].Value = "";
                    dgvSequence.Rows[n].Cells[2].Value = "";
                    dgvSequence.Rows[n].Cells[3].Value = "";
                    dgvSequence.Rows[n].Cells[4].Value = "";
                    dgvSequence.Rows[n].Cells[5].Value = "";
                    dgvSequence.Rows[n].Cells[6].Value = _dr["Table"].ToString();
                    dgvSequence.Rows[n].Cells[7].Value = _dr["OutletCode"].ToString();
                    dgvSequence.Rows[n].Cells[8].Value = _dr["Prefix"].ToString();
                    dgvSequence.Rows[n].Cells[9].Value = _dr["Length"].ToString();
                    dgvSequence.Rows[n].Cells[10].Value = _dr["LastNumber"].ToString();
                    dgvSequence.Rows[n].Cells[11].Value = "Error!";
                }
            }
            if (errorSequence > 0)
            {
                lblErrorSequence.ForeColor = Color.Red;
            }
            lblErrorSequence.Text = "Sequence Error(s)  : " + errorSequence.ToString();
            #endregion

            #region "LOOK UP VALUE"
            foreach (DataRow _dr in _dtNewLookUpValue.Rows)
            {
                int n = dgvLookUpValue.Rows.Add();
                dgvLookUpValue.Rows[n].Cells[0].Value = _dr["Key"].ToString();
                dgvLookUpValue.Rows[n].Cells[1].Value = _dr["Value"].ToString();
            }

            int k = 0;
            int errorLookUpValue = 0;
            string _statusLookUpValue;
            foreach (DataRow _dr in _dtOldLookUpValue.Rows)
            {
                _statusLookUpValue = "";
                try
                {
                    dgvLookUpValue.Rows[k].Cells[3].Value = _dr["Key"].ToString();
                    dgvLookUpValue.Rows[k].Cells[4].Value = _dr["Value"].ToString();

                    if (dgvLookUpValue.Rows[k].Cells[0].Value.ToString() != _dr["Key"].ToString())
                    {
                        _statusLookUpValue = ";Key";
                    }
                    if (dgvLookUpValue.Rows[k].Cells[1].Value.ToString() != _dr["Value"].ToString())
                    {
                        _statusLookUpValue = _statusLookUpValue + ";Value";
                    }

                    if (_statusLookUpValue != "")
                    {
                        errorLookUpValue++;
                        dgvLookUpValue.Rows[k].Cells[5].Value = "Error!" + _statusLookUpValue;
                    }

                    k++;
                }
                catch
                {
                    int n = dgvLookUpValue.Rows.Add();
                    dgvLookUpValue.Rows[n].Cells[0].Value = "";
                    dgvLookUpValue.Rows[n].Cells[1].Value = "";
                    dgvLookUpValue.Rows[n].Cells[2].Value = "";
                    dgvLookUpValue.Rows[n].Cells[3].Value = _dr["Key"].ToString();
                    dgvLookUpValue.Rows[n].Cells[4].Value = _dr["Value"].ToString();
                    dgvLookUpValue.Rows[n].Cells[5].Value = "Error!";
                }
            }
            if (errorLookUpValue > 0)
            {
                lblErrorLookUp.ForeColor = Color.Red;
            }
            lblErrorLookUp.Text = "Look Up Value Error(s) : " + errorLookUpValue.ToString();
            #endregion

            #region "MENU ITEMS"
            foreach (DataRow _dr in _dtNewMenuItem.Rows)
            {
                int n = dgvMenuItems.Rows.Add();
                dgvMenuItems.Rows[n].Cells[0].Value = _dr["MenuName"].ToString();
                dgvMenuItems.Rows[n].Cells[1].Value = _dr["MenuText"].ToString();
                dgvMenuItems.Rows[n].Cells[2].Value = _dr["ItemName"].ToString();
                dgvMenuItems.Rows[n].Cells[3].Value = _dr["ItemText"].ToString();
                dgvMenuItems.Rows[n].Cells[4].Value = _dr["MenuSeqNo"].ToString();
                dgvMenuItems.Rows[n].Cells[5].Value = _dr["ItemSeqNo"].ToString();
            }

            int l = 0;
            int errorMenuItem = 0;
            string _statusMenuItem;
            foreach (DataRow _dr in _dtOldMenuItem.Rows)
            {
                _statusMenuItem = "";
                try
                {
                    dgvMenuItems.Rows[l].Cells[7].Value = _dr["MenuName"].ToString();
                    dgvMenuItems.Rows[l].Cells[8].Value = _dr["MenuText"].ToString();
                    dgvMenuItems.Rows[l].Cells[9].Value = _dr["ItemName"].ToString();
                    dgvMenuItems.Rows[l].Cells[10].Value = _dr["ItemText"].ToString();
                    dgvMenuItems.Rows[l].Cells[11].Value = _dr["MenuSeqNo"].ToString();
                    dgvMenuItems.Rows[l].Cells[12].Value = _dr["ItemSeqNo"].ToString();

                    if (dgvMenuItems.Rows[l].Cells[0].Value.ToString() != _dr["MenuName"].ToString())
                    {
                        _statusMenuItem = ";MenuName";
                    }
                    if (dgvMenuItems.Rows[l].Cells[1].Value.ToString() != _dr["MenuText"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";MenuText";
                    }
                    if (dgvMenuItems.Rows[l].Cells[2].Value.ToString() != _dr["ItemName"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemName";
                    }
                    if (dgvMenuItems.Rows[l].Cells[3].Value.ToString() != _dr["ItemText"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemText";
                    }
                    if (dgvMenuItems.Rows[l].Cells[4].Value.ToString() != _dr["MenuSeqNo"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";MenuSeqNo";
                    }
                    if (dgvMenuItems.Rows[l].Cells[5].Value.ToString() != _dr["ItemSeqNo"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemSeqNo";
                    }

                    if (_statusMenuItem != "")
                    {
                        errorMenuItem++;
                        dgvMenuItems.Rows[l].Cells[13].Value = "Error!" + _statusMenuItem;
                    }

                    l++;
                }
                catch
                {
                    int n = dgvMenuItems.Rows.Add();
                    dgvMenuItems.Rows[n].Cells[0].Value = "";
                    dgvMenuItems.Rows[n].Cells[1].Value = "";
                    dgvMenuItems.Rows[n].Cells[2].Value = "";
                    dgvMenuItems.Rows[n].Cells[3].Value = "";
                    dgvMenuItems.Rows[n].Cells[4].Value = "";
                    dgvMenuItems.Rows[n].Cells[5].Value = "";
                    dgvMenuItems.Rows[n].Cells[6].Value = "";
                    dgvMenuItems.Rows[n].Cells[7].Value = _dr["MenuName"].ToString();
                    dgvMenuItems.Rows[n].Cells[8].Value = _dr["MenuText"].ToString();
                    dgvMenuItems.Rows[n].Cells[9].Value = _dr["ItemName"].ToString();
                    dgvMenuItems.Rows[n].Cells[10].Value = _dr["ItemText"].ToString();
                    dgvMenuItems.Rows[n].Cells[11].Value = _dr["MenuSeqNo"].ToString();
                    dgvMenuItems.Rows[n].Cells[12].Value = _dr["ItemSeqNo"].ToString();
                    dgvMenuItems.Rows[n].Cells[13].Value = "Error!";
                }
            }
            if (errorMenuItem > 0)
            {
                lblErrorMenuItem.ForeColor = Color.Red;
            }
            lblErrorMenuItem.Text = "Menu Item Error(s)  : " + errorMenuItem.ToString();
            #endregion

            #region "ITEM RIGHTS"
            foreach (DataRow _dr in _dtNewItemRights.Rows)
            {
                int n = dgvItemRights.Rows.Add();
                dgvItemRights.Rows[n].Cells[0].Value = _dr["ItemName"].ToString();
                dgvItemRights.Rows[n].Cells[1].Value = _dr["Rights"].ToString();
                dgvItemRights.Rows[n].Cells[2].Value = _dr["RightsSeqNo"].ToString();
            }

            int m = 0;
            int errorItemRights = 0;
            string _statusItemRights;
            foreach (DataRow _dr in _dtOldItemRights.Rows)
            {
                _statusItemRights = "";
                try
                {
                    dgvItemRights.Rows[m].Cells[4].Value = _dr["ItemName"].ToString();
                    dgvItemRights.Rows[m].Cells[5].Value = _dr["Rights"].ToString();
                    dgvItemRights.Rows[m].Cells[6].Value = _dr["RightsSeqNo"].ToString();

                    if (dgvItemRights.Rows[m].Cells[0].Value.ToString() != _dr["ItemName"].ToString())
                    {
                        _statusItemRights = ";ItemName";
                    }
                    if (dgvItemRights.Rows[m].Cells[1].Value.ToString() != _dr["Rights"].ToString())
                    {
                        _statusItemRights = _statusItemRights + ";Rights";
                    }
                    if (dgvItemRights.Rows[m].Cells[2].Value.ToString() != _dr["RightsSeqNo"].ToString())
                    {
                        _statusItemRights = _statusItemRights + ";RightsSeqNo";
                    }

                    if (_statusItemRights != "")
                    {
                        errorItemRights++;
                        dgvItemRights.Rows[m].Cells[7].Value = "Error!" + _statusItemRights;
                    }

                    m++;
                }
                catch
                {
                    int n = dgvItemRights.Rows.Add();
                    dgvItemRights.Rows[n].Cells[0].Value = "";
                    dgvItemRights.Rows[n].Cells[1].Value = "";
                    dgvItemRights.Rows[n].Cells[2].Value = "";
                    dgvItemRights.Rows[n].Cells[3].Value = "";
                    dgvItemRights.Rows[n].Cells[4].Value = _dr["ItemName"].ToString();
                    dgvItemRights.Rows[n].Cells[5].Value = _dr["Rights"].ToString();
                    dgvItemRights.Rows[n].Cells[6].Value = _dr["RightsSeqNo"].ToString();
                    dgvItemRights.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (errorItemRights > 0)
            {
                lblErrorItemRight.ForeColor = Color.Red;
            }
            lblErrorItemRight.Text = "Item Rights Error(s) : " + errorItemRights.ToString();
            #endregion

            #region "SYSTEM CONFIGURATION"
            foreach (DataRow _dr in _dtNewSystemConfiguration.Rows)
            {
                int n = dgvSystemConfiguration.Rows.Add();
                dgvSystemConfiguration.Rows[n].Cells[0].Value = _dr["Key"].ToString();
                dgvSystemConfiguration.Rows[n].Cells[1].Value = _dr["Value"].ToString();
                dgvSystemConfiguration.Rows[n].Cells[2].Value = _dr["OutletCode"].ToString();
            }

            int o = 0;
            int errorSystemConfiguration = 0;
            string _statusSystemConfiguration;
            foreach (DataRow _dr in _dtOldSystemConfiguration.Rows)
            {
                _statusSystemConfiguration = "";
                try
                {
                    dgvSystemConfiguration.Rows[o].Cells[4].Value = _dr["Key"].ToString();
                    dgvSystemConfiguration.Rows[o].Cells[5].Value = _dr["Value"].ToString();
                    dgvSystemConfiguration.Rows[o].Cells[6].Value = _dr["OutletCode"].ToString();

                    if (dgvSystemConfiguration.Rows[o].Cells[0].Value.ToString() != _dr["Key"].ToString())
                    {
                        _statusSystemConfiguration = ";Key";
                    }
                    if (dgvSystemConfiguration.Rows[o].Cells[1].Value.ToString() != _dr["Value"].ToString())
                    {
                        _statusSystemConfiguration = _statusSystemConfiguration + ";Value";
                    }
                    if (dgvSystemConfiguration.Rows[o].Cells[2].Value.ToString() != _dr["OutletCode"].ToString())
                    {
                        _statusSystemConfiguration = _statusSystemConfiguration + ";OutletCode";
                    }

                    if (_statusSystemConfiguration != "")
                    {
                        errorSystemConfiguration++;
                        dgvSystemConfiguration.Rows[o].Cells[7].Value = "Error!" + _statusSystemConfiguration;
                    }

                    o++;
                }
                catch
                {
                    int n = dgvSystemConfiguration.Rows.Add();
                    dgvSystemConfiguration.Rows[n].Cells[0].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[1].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[2].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[3].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[4].Value = _dr["Key"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[5].Value = _dr["Value"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[6].Value = _dr["OutletCode"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (errorSystemConfiguration > 0)
            {
                lblErrorSystemConfiguration.ForeColor = Color.Red;
            }
            lblErrorSystemConfiguration.Text = "System Configuration Error(s) : " + errorSystemConfiguration.ToString();
            #endregion

            MessageBox.Show("All records has been loaded successfully!", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBackUp_Click(object sender, EventArgs e)
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
                path = "C://Users//Public//Documents//POSBackUp" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + ".sql";
                StreamWriter file = new StreamWriter(path);

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "C://Program Files//MySQL//MySQL Server 5.5//bin//mysqldump.exe";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    txtUserId.Text, txtPassword.Text, txtServer.Text, txtCurrentDatabase.Text);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
                MessageBox.Show("Database has been backup successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Failure to Backup!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void dgvCheckTables_SizeChanged(object sender, EventArgs e)
        {
            dgvCheckTables.Columns[3].FillWeight = 2;
        }

        private void dgvSequence_SizeChanged(object sender, EventArgs e)
        {
            dgvSequence.Columns[5].FillWeight = 2;
        }

        private void dgvLookUpValue_SizeChanged(object sender, EventArgs e)
        {
            dgvLookUpValue.Columns[2].FillWeight = 2;
        }

        private void dgvMenuItems_SizeChanged(object sender, EventArgs e)
        {
            dgvMenuItems.Columns[6].FillWeight = 2;
        }

        private void dgvItemRights_SizeChanged(object sender, EventArgs e)
        {
            dgvItemRights.Columns[3].FillWeight = 2;
        }

        private void dgvSystemConfiguration_SizeChanged(object sender, EventArgs e)
        {
            dgvSystemConfiguration.Columns[3].FillWeight = 2;
        }

        private void dgvCheckTables_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvCheckTables.PointToScreen(e.Location);
                cmsFunctionTables.Show(pt);
            }
        }

        private void dgvSequence_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvSequence.PointToScreen(e.Location);
                cmsFunctionSequence.Show(pt);
            }
        }

        private void dgvLookUpValue_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvLookUpValue.PointToScreen(e.Location);
                cmsFunctionLookUp.Show(pt);
            }
        }

        private void dgvMenuItems_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvMenuItems.PointToScreen(e.Location);
                cmsFunctionMenuItems.Show(pt);
            }
        }

        private void dgvItemRights_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvItemRights.PointToScreen(e.Location);
                cmsFunctionItemRight.Show(pt);
            }
        }

        private void dgvSystemConfiguration_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvSystemConfiguration.PointToScreen(e.Location);
                cmsFunctionSystemConfiguration.Show(pt);
            }
        }

        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            dgvCheckTables.Rows.Clear();
            DataTable _dtNew = new DataTable();
            DataTable _dtOld = new DataTable();
            MySqlDataAdapter _daNew = new MySqlDataAdapter("SELECT Table_Name,Column_Name,Column_Type FROM information_schema.COLUMNS WHERE Table_Schema = '" + txtNewDatabase.Text + "' ORDER BY Table_Name ASC;", connNew);
            _daNew.Fill(_dtNew);
            MySqlDataAdapter _daOld = new MySqlDataAdapter("SELECT Table_Name,Column_Name,Column_Type FROM information_schema.COLUMNS WHERE Table_Schema = '" + txtCurrentDatabase.Text + "' ORDER BY Table_Name ASC;", connNew);
            _daOld.Fill(_dtOld);

            #region "TABLES"
            foreach (DataRow _dr in _dtNew.Rows)
            {
                int n = dgvCheckTables.Rows.Add();
                dgvCheckTables.Rows[n].Cells[0].Value = _dr["Table_Name"].ToString();
                dgvCheckTables.Rows[n].Cells[1].Value = _dr["Column_Name"].ToString();
                dgvCheckTables.Rows[n].Cells[2].Value = _dr["Column_Type"].ToString();
            }

            int i = 0;
            int error = 0;
            string _status;
            foreach (DataRow _dr in _dtOld.Rows)
            {
                _status = "";
                try
                {
                    dgvCheckTables.Rows[i].Cells[4].Value = _dr["Table_Name"].ToString();
                    dgvCheckTables.Rows[i].Cells[5].Value = _dr["Column_Name"].ToString();
                    dgvCheckTables.Rows[i].Cells[6].Value = _dr["Column_Type"].ToString();

                    if (dgvCheckTables.Rows[i].Cells[0].Value.ToString() != _dr["Table_Name"].ToString())
                    {
                        _status = ";TableName";
                    }
                    if (dgvCheckTables.Rows[i].Cells[1].Value.ToString() != _dr["Column_Name"].ToString())
                    {
                        _status = _status + ";Fields";
                    }
                    if (dgvCheckTables.Rows[i].Cells[2].Value.ToString() != _dr["Column_Type"].ToString())
                    {
                        _status = _status + ";DataType";
                    }

                    if (_status != "")
                    {
                        error++;
                        dgvCheckTables.Rows[i].Cells[7].Value = "Error!" + _status;
                    }
                    i++;
                }
                catch
                {
                    int n = dgvCheckTables.Rows.Add();
                    dgvCheckTables.Rows[n].Cells[0].Value = "";
                    dgvCheckTables.Rows[n].Cells[1].Value = "";
                    dgvCheckTables.Rows[n].Cells[2].Value = "";
                    dgvCheckTables.Rows[n].Cells[3].Value = "";
                    dgvCheckTables.Rows[n].Cells[4].Value = _dr["Table_Name"].ToString();
                    dgvCheckTables.Rows[n].Cells[5].Value = _dr["Column_Name"].ToString();
                    dgvCheckTables.Rows[n].Cells[6].Value = _dr["Column_Type"].ToString();
                    dgvCheckTables.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (error > 0)
            {
                lblErrorTable.ForeColor = Color.Red;
            }
            lblErrorTable.Text = "Table Error(s) : " + error.ToString();
            #endregion
        }

        private void tsmiRefreshSequence_Click(object sender, EventArgs e)
        {
            dgvSequence.Rows.Clear();
            DataTable _dtNewSequence = new DataTable();
            DataTable _dtOldSequence = new DataTable();
            //sequence
            MySqlDataAdapter _daNewSequence = new MySqlDataAdapter("SELECT `Table`,OutletCode,Prefix,`Length`,LastNumber FROM sequence WHERE `Status` = 'Active';", connNew);
            _daNewSequence.Fill(_dtNewSequence);
            MySqlDataAdapter _daOldSequence = new MySqlDataAdapter("SELECT `Table`,OutletCode,Prefix,`Length`,LastNumber FROM sequence WHERE `Status` = 'Active';", connCurrent);
            _daOldSequence.Fill(_dtOldSequence);

            #region "SEQUENCES"
            foreach (DataRow _dr in _dtNewSequence.Rows)
            {
                int n = dgvSequence.Rows.Add();
                dgvSequence.Rows[n].Cells[0].Value = _dr["Table"].ToString();
                dgvSequence.Rows[n].Cells[1].Value = _dr["OutletCode"].ToString();
                dgvSequence.Rows[n].Cells[2].Value = _dr["Prefix"].ToString();
                dgvSequence.Rows[n].Cells[3].Value = _dr["Length"].ToString();
                dgvSequence.Rows[n].Cells[4].Value = _dr["LastNumber"].ToString();
            }

            int j = 0;
            int errorSequence = 0;
            string _statusSequence;
            foreach (DataRow _dr in _dtOldSequence.Rows)
            {
                _statusSequence = "";
                try
                {
                    dgvSequence.Rows[j].Cells[6].Value = _dr["Table"].ToString();
                    dgvSequence.Rows[j].Cells[7].Value = _dr["OutletCode"].ToString();
                    dgvSequence.Rows[j].Cells[8].Value = _dr["Prefix"].ToString();
                    dgvSequence.Rows[j].Cells[9].Value = _dr["Length"].ToString();
                    dgvSequence.Rows[j].Cells[10].Value = _dr["LastNumber"].ToString();

                    if (dgvSequence.Rows[j].Cells[0].Value.ToString() != _dr["Table"].ToString())
                    {
                        _statusSequence = ";Table";
                    }
                    if (dgvSequence.Rows[j].Cells[1].Value.ToString() != _dr["OutletCode"].ToString())
                    {
                        _statusSequence = _statusSequence + ";OutletCode";
                    }
                    if (dgvSequence.Rows[j].Cells[2].Value.ToString() != _dr["Prefix"].ToString())
                    {
                        _statusSequence = _statusSequence + ";Prefix";
                    }
                    if (dgvSequence.Rows[j].Cells[3].Value.ToString() != _dr["Length"].ToString())
                    {
                        _statusSequence = _statusSequence + ";Length";
                    }
                    if (dgvSequence.Rows[j].Cells[4].Value.ToString() != _dr["LastNumber"].ToString())
                    {
                        _statusSequence = _statusSequence + ";LastNumber";
                    }

                    if (_statusSequence != "")
                    {
                        errorSequence++;
                        dgvSequence.Rows[j].Cells[11].Value = "Error!" + _statusSequence;
                    }

                    j++;
                }
                catch
                {
                    int n = dgvSequence.Rows.Add();
                    dgvSequence.Rows[n].Cells[0].Value = "";
                    dgvSequence.Rows[n].Cells[1].Value = "";
                    dgvSequence.Rows[n].Cells[2].Value = "";
                    dgvSequence.Rows[n].Cells[3].Value = "";
                    dgvSequence.Rows[n].Cells[4].Value = "";
                    dgvSequence.Rows[n].Cells[5].Value = "";
                    dgvSequence.Rows[n].Cells[6].Value = _dr["Table"].ToString();
                    dgvSequence.Rows[n].Cells[7].Value = _dr["OutletCode"].ToString();
                    dgvSequence.Rows[n].Cells[8].Value = _dr["Prefix"].ToString();
                    dgvSequence.Rows[n].Cells[9].Value = _dr["Length"].ToString();
                    dgvSequence.Rows[n].Cells[10].Value = _dr["LastNumber"].ToString();
                    dgvSequence.Rows[n].Cells[11].Value = "Error!";
                }
            }
            if (errorSequence > 0)
            {
                lblErrorSequence.ForeColor = Color.Red;
            }
            lblErrorSequence.Text = "Sequence Error(s)  : " + errorSequence.ToString();
            #endregion
        }

        private void tsmiRefreshLookUp_Click(object sender, EventArgs e)
        {
            dgvLookUpValue.Rows.Clear();
            DataTable _dtNewLookUpValue = new DataTable();
            DataTable _dtOldLookUpValue = new DataTable();
            //look up Value
            MySqlDataAdapter _daNewLookUp = new MySqlDataAdapter("SELECT `Key`,`Value` FROM lookuptable WHERE `Status` = 'Active';", connNew);
            _daNewLookUp.Fill(_dtNewLookUpValue);
            MySqlDataAdapter _daOldLookUp = new MySqlDataAdapter("SELECT `Key`,`Value` FROM lookuptable WHERE `Status` = 'Active';", connCurrent);
            _daOldLookUp.Fill(_dtOldLookUpValue);

            #region "LOOK UP VALUE"
            foreach (DataRow _dr in _dtNewLookUpValue.Rows)
            {
                int n = dgvLookUpValue.Rows.Add();
                dgvLookUpValue.Rows[n].Cells[0].Value = _dr["Key"].ToString();
                dgvLookUpValue.Rows[n].Cells[1].Value = _dr["Value"].ToString();
            }

            int k = 0;
            int errorLookUpValue = 0;
            string _statusLookUpValue;
            foreach (DataRow _dr in _dtOldLookUpValue.Rows)
            {
                _statusLookUpValue = "";
                try
                {
                    dgvLookUpValue.Rows[k].Cells[3].Value = _dr["Key"].ToString();
                    dgvLookUpValue.Rows[k].Cells[4].Value = _dr["Value"].ToString();

                    if (dgvLookUpValue.Rows[k].Cells[0].Value.ToString() != _dr["Key"].ToString())
                    {
                        _statusLookUpValue = ";Key";
                    }
                    if (dgvLookUpValue.Rows[k].Cells[1].Value.ToString() != _dr["Value"].ToString())
                    {
                        _statusLookUpValue = _statusLookUpValue + ";Value";
                    }

                    if (_statusLookUpValue != "")
                    {
                        errorLookUpValue++;
                        dgvLookUpValue.Rows[k].Cells[5].Value = "Error!" + _statusLookUpValue;
                    }

                    k++;
                }
                catch
                {
                    int n = dgvLookUpValue.Rows.Add();
                    dgvLookUpValue.Rows[n].Cells[0].Value = "";
                    dgvLookUpValue.Rows[n].Cells[1].Value = "";
                    dgvLookUpValue.Rows[n].Cells[2].Value = "";
                    dgvLookUpValue.Rows[n].Cells[3].Value = _dr["Key"].ToString();
                    dgvLookUpValue.Rows[n].Cells[4].Value = _dr["Value"].ToString();
                    dgvLookUpValue.Rows[n].Cells[5].Value = "Error!";
                }
            }
            if (errorLookUpValue > 0)
            {
                lblErrorLookUp.ForeColor = Color.Red;
            }
            lblErrorLookUp.Text = "Look Up Value Error(s) : " + errorLookUpValue.ToString();
            #endregion
        }

        private void tsmiRefreshMenuItem_Click(object sender, EventArgs e)
        {
            dgvMenuItems.Rows.Clear();
            DataTable _dtNewMenuItem = new DataTable();
            DataTable _dtOldMenuItem = new DataTable();
            //menu item
            MySqlDataAdapter _daNewMenuItem = new MySqlDataAdapter("SELECT MenuName,MenuText,ItemName,ItemText,MenuSeqNo,ItemSeqNo FROM menuitem ORDER BY MenuSeqNo,ItemSeqNo,MenuName;", connNew);
            _daNewMenuItem.Fill(_dtNewMenuItem);
            MySqlDataAdapter _daOldMenuItem = new MySqlDataAdapter("SELECT MenuName,MenuText,ItemName,ItemText,MenuSeqNo,ItemSeqNo FROM menuitem ORDER BY MenuSeqNo,ItemSeqNo,MenuName;", connCurrent);
            _daOldMenuItem.Fill(_dtOldMenuItem);

            #region "MENU ITEMS"
            foreach (DataRow _dr in _dtNewMenuItem.Rows)
            {
                int n = dgvMenuItems.Rows.Add();
                dgvMenuItems.Rows[n].Cells[0].Value = _dr["MenuName"].ToString();
                dgvMenuItems.Rows[n].Cells[1].Value = _dr["MenuText"].ToString();
                dgvMenuItems.Rows[n].Cells[2].Value = _dr["ItemName"].ToString();
                dgvMenuItems.Rows[n].Cells[3].Value = _dr["ItemText"].ToString();
                dgvMenuItems.Rows[n].Cells[4].Value = _dr["MenuSeqNo"].ToString();
                dgvMenuItems.Rows[n].Cells[5].Value = _dr["ItemSeqNo"].ToString();
            }

            int l = 0;
            int errorMenuItem = 0;
            string _statusMenuItem;
            foreach (DataRow _dr in _dtOldMenuItem.Rows)
            {
                _statusMenuItem = "";
                try
                {
                    dgvMenuItems.Rows[l].Cells[7].Value = _dr["MenuName"].ToString();
                    dgvMenuItems.Rows[l].Cells[8].Value = _dr["MenuText"].ToString();
                    dgvMenuItems.Rows[l].Cells[9].Value = _dr["ItemName"].ToString();
                    dgvMenuItems.Rows[l].Cells[10].Value = _dr["ItemText"].ToString();
                    dgvMenuItems.Rows[l].Cells[11].Value = _dr["MenuSeqNo"].ToString();
                    dgvMenuItems.Rows[l].Cells[12].Value = _dr["ItemSeqNo"].ToString();

                    if (dgvMenuItems.Rows[l].Cells[0].Value.ToString() != _dr["MenuName"].ToString())
                    {
                        _statusMenuItem = ";MenuName";
                    }
                    if (dgvMenuItems.Rows[l].Cells[1].Value.ToString() != _dr["MenuText"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";MenuText";
                    }
                    if (dgvMenuItems.Rows[l].Cells[2].Value.ToString() != _dr["ItemName"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemName";
                    }
                    if (dgvMenuItems.Rows[l].Cells[3].Value.ToString() != _dr["ItemText"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemText";
                    }
                    if (dgvMenuItems.Rows[l].Cells[4].Value.ToString() != _dr["MenuSeqNo"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";MenuSeqNo";
                    }
                    if (dgvMenuItems.Rows[l].Cells[5].Value.ToString() != _dr["ItemSeqNo"].ToString())
                    {
                        _statusMenuItem = _statusMenuItem + ";ItemSeqNo";
                    }

                    if (_statusMenuItem != "")
                    {
                        errorMenuItem++;
                        dgvMenuItems.Rows[l].Cells[13].Value = "Error!" + _statusMenuItem;
                    }

                    l++;
                }
                catch
                {
                    int n = dgvMenuItems.Rows.Add();
                    dgvMenuItems.Rows[n].Cells[0].Value = "";
                    dgvMenuItems.Rows[n].Cells[1].Value = "";
                    dgvMenuItems.Rows[n].Cells[2].Value = "";
                    dgvMenuItems.Rows[n].Cells[3].Value = "";
                    dgvMenuItems.Rows[n].Cells[4].Value = "";
                    dgvMenuItems.Rows[n].Cells[5].Value = "";
                    dgvMenuItems.Rows[n].Cells[6].Value = "";
                    dgvMenuItems.Rows[n].Cells[7].Value = _dr["MenuName"].ToString();
                    dgvMenuItems.Rows[n].Cells[8].Value = _dr["MenuText"].ToString();
                    dgvMenuItems.Rows[n].Cells[9].Value = _dr["ItemName"].ToString();
                    dgvMenuItems.Rows[n].Cells[10].Value = _dr["ItemText"].ToString();
                    dgvMenuItems.Rows[n].Cells[11].Value = _dr["MenuSeqNo"].ToString();
                    dgvMenuItems.Rows[n].Cells[12].Value = _dr["ItemSeqNo"].ToString();
                    dgvMenuItems.Rows[n].Cells[13].Value = "Error!";
                }
            }
            if (errorMenuItem > 0)
            {
                lblErrorMenuItem.ForeColor = Color.Red;
            }
            lblErrorMenuItem.Text = "Menu Item Error(s)  : " + errorMenuItem.ToString();
            #endregion
        }

        private void tsmiRefreshItemRight_Click(object sender, EventArgs e)
        {
            dgvItemRights.Rows.Clear();
            DataTable _dtNewItemRights = new DataTable();
            DataTable _dtOldItemRights = new DataTable();
            //item rights
            MySqlDataAdapter _daNewItemRights = new MySqlDataAdapter("SELECT ItemName,Rights,RightsSeqNo FROM itemrights;", connNew);
            _daNewItemRights.Fill(_dtNewItemRights);
            MySqlDataAdapter _daOldItemRights = new MySqlDataAdapter("SELECT ItemName,Rights,RightsSeqNo FROM itemrights;", connCurrent);
            _daOldItemRights.Fill(_dtOldItemRights);
            #region "ITEM RIGHTS"
            foreach (DataRow _dr in _dtNewItemRights.Rows)
            {
                int n = dgvItemRights.Rows.Add();
                dgvItemRights.Rows[n].Cells[0].Value = _dr["ItemName"].ToString();
                dgvItemRights.Rows[n].Cells[1].Value = _dr["Rights"].ToString();
                dgvItemRights.Rows[n].Cells[2].Value = _dr["RightsSeqNo"].ToString();
            }

            int m = 0;
            int errorItemRights = 0;
            string _statusItemRights;
            foreach (DataRow _dr in _dtOldItemRights.Rows)
            {
                _statusItemRights = "";
                try
                {
                    dgvItemRights.Rows[m].Cells[4].Value = _dr["ItemName"].ToString();
                    dgvItemRights.Rows[m].Cells[5].Value = _dr["Rights"].ToString();
                    dgvItemRights.Rows[m].Cells[6].Value = _dr["RightsSeqNo"].ToString();

                    if (dgvItemRights.Rows[m].Cells[0].Value.ToString() != _dr["ItemName"].ToString())
                    {
                        _statusItemRights = ";ItemName";
                    }
                    if (dgvItemRights.Rows[m].Cells[1].Value.ToString() != _dr["Rights"].ToString())
                    {
                        _statusItemRights = _statusItemRights + ";Rights";
                    }
                    if (dgvItemRights.Rows[m].Cells[2].Value.ToString() != _dr["RightsSeqNo"].ToString())
                    {
                        _statusItemRights = _statusItemRights + ";RightsSeqNo";
                    }

                    if (_statusItemRights != "")
                    {
                        errorItemRights++;
                        dgvItemRights.Rows[m].Cells[7].Value = "Error!" + _statusItemRights;
                    }

                    m++;
                }
                catch
                {
                    int n = dgvItemRights.Rows.Add();
                    dgvItemRights.Rows[n].Cells[0].Value = "";
                    dgvItemRights.Rows[n].Cells[1].Value = "";
                    dgvItemRights.Rows[n].Cells[2].Value = "";
                    dgvItemRights.Rows[n].Cells[3].Value = "";
                    dgvItemRights.Rows[n].Cells[4].Value = _dr["ItemName"].ToString();
                    dgvItemRights.Rows[n].Cells[5].Value = _dr["Rights"].ToString();
                    dgvItemRights.Rows[n].Cells[6].Value = _dr["RightsSeqNo"].ToString();
                    dgvItemRights.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (errorItemRights > 0)
            {
                lblErrorItemRight.ForeColor = Color.Red;
            }
            lblErrorItemRight.Text = "Item Rights Error(s) : " + errorItemRights.ToString();
            #endregion
        }

        private void tsmiRefreshSystemConfig_Click(object sender, EventArgs e)
        {
            dgvSystemConfiguration.Rows.Clear();
            DataTable _dtNewSystemConfiguration = new DataTable();
            DataTable _dtOldSystemConfiguration = new DataTable();
            //system configuration
            MySqlDataAdapter _daNewSystemConfiguration = new MySqlDataAdapter("SELECT `Key`,`Value`,OutletCode FROM systemconfiguration where `Status`='Active';", connNew);
            _daNewSystemConfiguration.Fill(_dtNewSystemConfiguration);
            MySqlDataAdapter _daOldSystemConfiguration = new MySqlDataAdapter("SELECT `Key`,`Value`,OutletCode FROM systemconfiguration where `Status`='Active';", connCurrent);
            _daOldSystemConfiguration.Fill(_dtOldSystemConfiguration);
            #region "SYSTEM CONFIGURATION"
            foreach (DataRow _dr in _dtNewSystemConfiguration.Rows)
            {
                int n = dgvSystemConfiguration.Rows.Add();
                dgvSystemConfiguration.Rows[n].Cells[0].Value = _dr["Key"].ToString();
                dgvSystemConfiguration.Rows[n].Cells[1].Value = _dr["Value"].ToString();
                dgvSystemConfiguration.Rows[n].Cells[2].Value = _dr["OutletCode"].ToString();
            }

            int o = 0;
            int errorSystemConfiguration = 0;
            string _statusSystemConfiguration;
            foreach (DataRow _dr in _dtOldSystemConfiguration.Rows)
            {
                _statusSystemConfiguration = "";
                try
                {
                    dgvSystemConfiguration.Rows[o].Cells[4].Value = _dr["Key"].ToString();
                    dgvSystemConfiguration.Rows[o].Cells[5].Value = _dr["Value"].ToString();
                    dgvSystemConfiguration.Rows[o].Cells[6].Value = _dr["OutletCode"].ToString();

                    if (dgvSystemConfiguration.Rows[o].Cells[0].Value.ToString() != _dr["Key"].ToString())
                    {
                        _statusSystemConfiguration = ";Key";
                    }
                    if (dgvSystemConfiguration.Rows[o].Cells[1].Value.ToString() != _dr["Value"].ToString())
                    {
                        _statusSystemConfiguration = _statusSystemConfiguration + ";Value";
                    }
                    if (dgvSystemConfiguration.Rows[o].Cells[2].Value.ToString() != _dr["OutletCode"].ToString())
                    {
                        _statusSystemConfiguration = _statusSystemConfiguration + ";OutletCode";
                    }

                    if (_statusSystemConfiguration != "")
                    {
                        errorSystemConfiguration++;
                        dgvSystemConfiguration.Rows[o].Cells[7].Value = "Error!" + _statusSystemConfiguration;
                    }

                    o++;
                }
                catch
                {
                    int n = dgvSystemConfiguration.Rows.Add();
                    dgvSystemConfiguration.Rows[n].Cells[0].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[1].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[2].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[3].Value = "";
                    dgvSystemConfiguration.Rows[n].Cells[4].Value = _dr["Key"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[5].Value = _dr["Value"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[6].Value = _dr["OutletCode"].ToString();
                    dgvSystemConfiguration.Rows[n].Cells[7].Value = "Error!";
                }
            }
            if (errorSystemConfiguration > 0)
            {
                lblErrorSystemConfiguration.ForeColor = Color.Red;
            }
            lblErrorSystemConfiguration.Text = "System Configuration Error(s) : " + errorSystemConfiguration.ToString();
            #endregion
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }
    }
}
