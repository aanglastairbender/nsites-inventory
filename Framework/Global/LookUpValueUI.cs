using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using NSites.ApplicationObjects.UserInterfaces.MasterFile;

namespace NSites.Global
{
    public partial class LookUpValueUI : Form
    {
        #region "VARIABLES"
        public string lCode;
        public string lDesc;
        public string lValue3 = "";
        public string lValue4 = "";
        public string lValue5 = "";
        public string lValue6 = "";
        public bool lFromSelection;
        public object lObject;
        public Type lType;
        Type lTypeUI;
        public string lTableName;
        string[] lColumnName;
        DataTable loRecord;
        #endregion "END OF VARIABLES"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "CONSTRUCTORS"

        public LookUpValueUI()
        {
            InitializeComponent();
            loRecord = new DataTable();
            Type _type = typeof(FormUI);
            lTypeUI = _type;
        }

        #endregion "END OF CONSTRUCTORS"

        #region "METHODS"

        public void refresh(string pDisplayType, string pSearchString)
        {
            try
            {
                loRecord.Rows.Clear();
                object[] _params = { pDisplayType, pSearchString };
                loRecord = (DataTable)lObject.GetType().GetMethod("getAllData").Invoke(lObject, _params);
                GlobalFunctions.refreshGrid(ref dgvLookUp, loRecord);
            }
            catch(Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.Close);
                mb.ShowDialog();
                this.Close();
            }
        }

        public void addData(string[] pRecordData)
        {
            int n = dgvLookUp.Rows.Add();
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvLookUp.Rows[n].Cells[i].Value = pRecordData[i];
            }
        }
        #endregion "END OF METHODS"

        #region "EVENTS"

        private void LookUpValueUI_Load(object sender, EventArgs e)
        {
            lblTableName.Text = lTableName;
            lFromSelection = false;
            refresh("ViewAll","");
        }

        private void dgvLookUp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lCode = dgvLookUp.CurrentRow.Cells[0].Value.ToString();
            lDesc = dgvLookUp.CurrentRow.Cells[1].Value.ToString();
            try
            {
                lValue3 = dgvLookUp.CurrentRow.Cells[2].Value.ToString();
            }
            catch { }
            try
            {
                lValue4 = dgvLookUp.CurrentRow.Cells[3].Value.ToString();
            }
            catch { }
            try
            {
                lValue5 = dgvLookUp.CurrentRow.Cells[4].Value.ToString();
            }
            catch { }
            try
            {
                lValue6 = dgvLookUp.CurrentRow.Cells[5].Value.ToString();
            }
            catch { }
        }

        private void dgvLookUp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                try
                {
                    int row = dgvLookUp.CurrentRow.Index - 1;
                    lCode = dgvLookUp.Rows[row].Cells[0].Value.ToString();
                    lDesc = dgvLookUp.Rows[row].Cells[1].Value.ToString();
                    try
                    {
                        lValue3 = dgvLookUp.Rows[row].Cells[2].Value.ToString();
                    }
                    catch { }
                    try
                    {
                        lValue4 = dgvLookUp.Rows[row].Cells[3].Value.ToString();
                    }
                    catch { }
                    try
                    {
                        lValue5 = dgvLookUp.Rows[row].Cells[4].Value.ToString();
                    }
                    catch { }
                    try
                    {
                        lValue6 = dgvLookUp.Rows[row].Cells[5].Value.ToString();
                    }
                    catch { }
                    lFromSelection = true;
                    this.Close();
                }
                catch
                {
                    try
                    {
                        lCode = dgvLookUp.CurrentRow.Cells[0].Value.ToString();
                        lDesc = dgvLookUp.CurrentRow.Cells[1].Value.ToString();
                        try
                        {
                            lValue3 = dgvLookUp.CurrentRow.Cells[2].Value.ToString();
                        }
                        catch { }
                        try
                        {
                            lValue4 = dgvLookUp.CurrentRow.Cells[3].Value.ToString();
                        }
                        catch { }
                        try
                        {
                            lValue5 = dgvLookUp.CurrentRow.Cells[4].Value.ToString();
                        }
                        catch { }
                        try
                        {
                            lValue6 = dgvLookUp.CurrentRow.Cells[5].Value.ToString();
                        }
                        catch { }
                        lFromSelection = true;
                        this.Close();
                    }
                    catch
                    {
                        lFromSelection = false;
                        this.Close();
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            refresh("", txtSearch.Text);
        }

        private void viewAllRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refresh("ViewAll", "");
        }

        private void dgvLookUp_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvLookUp.PointToScreen(e.Location);
                cmsFunctions.Show(pt);
            }
        }

        private void viewHiddenRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvLookUp, loRecord);        
        }
        #endregion "END OF EVENTS"

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (lType.Name)
            {
                /*
                case "Client":
                    ClientDetailUI loClientDetail = new ClientDetailUI();
                    loClientDetail.ParentList = this;
                    loClientDetail.ShowDialog();
                    break;
                */
            }
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            lCode = "";
            lDesc = "";
            lValue3 = "";
            lValue4 = "";
            lValue5 = "";
            lValue6 = "";
            lFromSelection = true;
            this.Close();
        }

        private void dgvLookUp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lCode = dgvLookUp.CurrentRow.Cells[0].Value.ToString();
            lDesc = dgvLookUp.CurrentRow.Cells[1].Value.ToString();
            try
            {
                lValue3 = dgvLookUp.CurrentRow.Cells[2].Value.ToString();
            }
            catch { }
            try
            {
                lValue4 = dgvLookUp.CurrentRow.Cells[3].Value.ToString();
            }
            catch { }
            try
            {
                lValue5 = dgvLookUp.CurrentRow.Cells[4].Value.ToString();
            }
            catch { }
            try
            {
                lValue6 = dgvLookUp.CurrentRow.Cells[5].Value.ToString();
            }
            catch { }
            lFromSelection = true;
            this.Close();
        }

        private void dgvLookUp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvLookUp.Columns[e.ColumnIndex].Name == "Credit Limit")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvLookUp.Columns[e.ColumnIndex].Name == "Id" || this.dgvLookUp.Columns[e.ColumnIndex].Name == "For Billing" ||
                    this.dgvLookUp.Columns[e.ColumnIndex].Name == "Terms" || this.dgvLookUp.Columns[e.ColumnIndex].Name == "Username" ||
                    this.dgvLookUp.Columns[e.ColumnIndex].Name == "Sales No.")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch { }
        }
    }
}
