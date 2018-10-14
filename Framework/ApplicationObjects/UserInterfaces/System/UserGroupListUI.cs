using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.Classes;
using oUserGroup = NSites.ApplicationObjects.Classes.UserGroup;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class UserGroupListUI : Form
    {
        #region "VARIABLES"
        UserGroup loUserGroup;
        string lUserGroupId;
        string lUserGroupDesc;
        DataTable lMenuItemsDT;
        DataTable lAllMenuItemDT;
        DataView lUserGroupDV;
        SearchUI loSearch;
        Common loCommon;
        DataTable ldtShow;
        DataTable ldtReport;
        DataTable ldtReportSum;
        bool lFromRefresh;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public UserGroupListUI()
        {
            InitializeComponent();
            loUserGroup = new UserGroup();
            lMenuItemsDT = new DataTable();
            lAllMenuItemDT = new DataTable();
            loCommon = new Common();
            ldtShow = new DataTable();
            ldtReport = new DataTable();
            ldtReportSum = new DataTable();
            lFromRefresh = false;
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
        public void refresh(string pDisplayType,string pSearchString)
        {
            ldtShow = loUserGroup.getAllData(pDisplayType, pSearchString);
            GlobalFunctions.refreshGrid(ref dgvUserGroups, ldtShow);
            lAllMenuItemDT = loUserGroup.getAllMenuItems();
            lFromRefresh = true;

            try
            {
                lUserGroupId = dgvUserGroups.CurrentRow.Cells[0].Value.ToString();
                lUserGroupDesc = dgvUserGroups.CurrentRow.Cells[1].Value.ToString();
                loadAllMenuItems();
            }
            catch { }
        }

        public void addData(string[] pRecordData)
        {
            int n = dgvUserGroups.Rows.Add();
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvUserGroups.Rows[n].Cells[i].Value = pRecordData[i];
            }
        }

        public void updateData(string[] pRecordData)
        {
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvUserGroups.CurrentRow.Cells[i].Value = pRecordData[i];
            }
        }

        public void loadAllMenuItems()
        {
            lMenuItemsDT = loUserGroup.getMenuItemsByGroup(dgvUserGroups.CurrentRow.Cells[0].Value.ToString());
            lUserGroupDV = new DataView(lMenuItemsDT);
            dgvMenuItems.Rows.Clear();
            string _menuText = "";
            foreach (DataRow _dr in lAllMenuItemDT.Rows)
            {
                int n = dgvMenuItems.Rows.Add();

                dgvMenuItems.Rows[n].Cells[0].Value = _dr["MenuName"].ToString();

                if (_menuText != _dr["MenuText"].ToString())
                {
                    dgvMenuItems.Rows[n].Cells[1].Value = _dr["MenuText"].ToString();
                    _menuText = _dr["MenuText"].ToString();
                }
                dgvMenuItems.Rows[n].Cells[2].Value = _dr["ItemName"].ToString();
                dgvMenuItems.Rows[n].Cells[3].Value = _dr["ItemText"].ToString();
                dgvMenuItems.Rows[n].Cells[4].Value = "Disable";
                lUserGroupDV.RowFilter = "Menu = '" + _dr["MenuName"].ToString() + "'";
                lUserGroupDV.RowFilter = "Item = '" + _dr["ItemName"].ToString() + "'";
                if (lUserGroupDV.Count != 0)
                {
                    dgvMenuItems.Rows[n].Cells[4].Value = "Enable";
                }
            }
            if (dgvMenuItems.CurrentRow.Cells[4].Value.ToString() == "Enable")
            {
                loadAllEnableRights(dgvMenuItems.CurrentRow.Cells[2].Value.ToString(), lUserGroupId);
            }
            else
            {
                dgvRights.Rows.Clear();
            }
        }

        public void loadAllEnableRights(string pItemName, string pUserGroupId)
        {
            DataTable _dtAll = new DataTable();
            DataTable _dtEnable = new DataTable();
            _dtAll = loUserGroup.getAllRights(pItemName);
            _dtEnable = loUserGroup.getEnableRights(pItemName, pUserGroupId);
            DataView _dvEnable = new DataView(_dtEnable);
            dgvRights.Rows.Clear();
            foreach (DataRow _dr in _dtAll.Rows)
            {
                int n = dgvRights.Rows.Add();
                dgvRights.Rows[n].Cells[0].Value = _dr["ItemName"].ToString();
                dgvRights.Rows[n].Cells[1].Value = _dr["Rights"].ToString();
                dgvRights.Rows[n].Cells[2].Value = "Disable";
                _dvEnable.RowFilter = "Rights = '" + _dr["Rights"].ToString() + "'";
                if (_dvEnable.Count != 0)
                {
                    dgvRights.Rows[n].Cells[2].Value = "Enable";
                }
            }
        }
        public void loadAllRights(string pItemName, string pUserGroupId)
        {
            DataTable _dtAll = new DataTable();
            DataTable _dtEnable = new DataTable();
            _dtAll = loUserGroup.getAllRights(pItemName);
            dgvRights.Rows.Clear();
            foreach (DataRow _dr in _dtAll.Rows)
            {
                int n = dgvRights.Rows.Add();
                dgvRights.Rows[n].Cells[0].Value = _dr["ItemName"].ToString();
                dgvRights.Rows[n].Cells[1].Value = _dr["Rights"].ToString();
                dgvRights.Rows[n].Cells[2].Value = "Disable";
            }
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void UserGroupListUI_Load(object sender, EventArgs e)
        {
            Type _Type = typeof(UserGroup);
            FieldInfo[] myFieldInfo;
            myFieldInfo = _Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);

            loSearch = new SearchUI(myFieldInfo, _Type, "tsmUserGroup");
        }

        private void viewHiddenRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvUserGroups, ldtShow);
        }

        private void dgvUserGroups_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvUserGroups.PointToScreen(e.Location);
                cmsFunctions.Show(pt);
            }
        }

        private void dgvUserGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lUserGroupId = dgvUserGroups.CurrentRow.Cells[0].Value.ToString();
                lUserGroupDesc = dgvUserGroups.CurrentRow.Cells[1].Value.ToString();
                loadAllMenuItems();
            }
            catch { }
        }

        private void dgvUserGroups_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lUserGroupId = dgvUserGroups.CurrentRow.Cells[0].Value.ToString();
            lUserGroupDesc = dgvUserGroups.CurrentRow.Cells[1].Value.ToString();
            btnUpdate_Click(null, new EventArgs());
        }

        private void dgvMenuItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dgvMenuItems.Columns[e.ColumnIndex].Name == "Status")
                {
                    DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
                    ch1 = (DataGridViewCheckBoxCell)dgvMenuItems.Rows[dgvMenuItems.CurrentRow.Index].Cells[4];

                    if (ch1.Value == null)
                        ch1.Value = false;
                    switch (ch1.Value.ToString())
                    {
                        case "Enable":
                            ch1.Value = "Disable";
                            dgvRights.Rows.Clear();
                            break;
                        case "Disable":
                            ch1.Value = "Enable";
                            loadAllEnableRights(dgvMenuItems.CurrentRow.Cells[2].Value.ToString(), lUserGroupId);
                            break;
                    }
                }
                else
                {
                    if (dgvMenuItems.CurrentRow.Cells[4].Value.ToString() == "Enable")
                    {
                        loadAllEnableRights(dgvMenuItems.CurrentRow.Cells[2].Value.ToString(), lUserGroupId);
                    }
                    else
                    {
                        loadAllRights(dgvMenuItems.CurrentRow.Cells[2].Value.ToString(), lUserGroupId);
                    }
                }
            }
            catch { }
        }

        private void dgvRights_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvRights.Columns[e.ColumnIndex].Name == "RightStatus")
            {
                DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
                ch1 = (DataGridViewCheckBoxCell)dgvRights.Rows[dgvRights.CurrentRow.Index].Cells[2];

                if (ch1.Value == null)
                    ch1.Value = false;
                switch (ch1.Value.ToString())
                {
                    case "Enable":
                        ch1.Value = "Disable";
                        break;
                    case "Disable":
                        ch1.Value = "Enable";
                        break;
                }
            }
        }

        private void btnSaveRights_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Save Rights"))
            {
                return;
            }
            try
            {
                if (loUserGroup.updateUserGroupRights(lUserGroupId, dgvMenuItems.CurrentRow.Cells[2].Value.ToString(), GlobalFunctions.convertDataGridToDataTable(dgvRights)))
                {
                    btnSaveMenu_Click(null, new System.EventArgs());
                   // MessageBoxUI _mb = new MessageBoxUI("Rights has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                   // _mb.showDialog();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate"))
                {
                    MessageBoxUI _mb = new MessageBoxUI(ex.Message, GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void chkRights_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRights.Checked)
            {
                for (int i = 0; i < dgvRights.Rows.Count; i++)
                {
                    dgvRights.Rows[i].Cells[2].Value = "Enable";
                }
            }
            else
            {
                for (int i = 0; i < dgvRights.Rows.Count; i++)
                {
                    dgvRights.Rows[i].Cells[2].Value = "Disable";
                }
            }
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckAll.Checked)
            {
                for (int i = 0; i < dgvMenuItems.Rows.Count; i++)
                {
                    dgvMenuItems.Rows[i].Cells[4].Value = "Enable";
                }
            }
            else
            {
                for (int i = 0; i < dgvMenuItems.Rows.Count; i++)
                {
                    dgvMenuItems.Rows[i].Cells[4].Value = "Disable";
                }
            }
        }
        #endregion "END OF REGION"

        private void btnExit_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Refresh"))
            {
                return;
            }
            refresh("ViewAll", "");
            
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Create"))
            {
                return;
            }
            UserGroupUI _UserGroup = new UserGroupUI();
            _UserGroup.ParentList = this;
            _UserGroup.ShowDialog();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Update"))
            {
                return;
            }
            if (lUserGroupId != null)
            {
                UserGroupUI _UserGroup = new UserGroupUI(dgvUserGroups.CurrentRow.Cells[0].Value.ToString(),
                    dgvUserGroups.CurrentRow.Cells[1].Value.ToString(), dgvUserGroups.CurrentRow.Cells[2].Value.ToString());
                _UserGroup.ParentList = this;
                _UserGroup.ShowDialog();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Remove"))
            {
                return;
            }
            if (lUserGroupId != null)
            {
                DialogResult _dr = new DialogResult();
                MessageBoxUI _mb = new MessageBoxUI("Are sure you want to continue removing this record?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                _mb.ShowDialog();
                _dr = _mb.Operation;
                if (_dr == DialogResult.Yes)
                {
                    MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
                    try
                    {
                        if (loUserGroup.removeUserGroup(lUserGroupId,ref Trans))
                        {
                            Trans.Commit();
                            MessageBoxUI _mb1 = new MessageBoxUI("Usergroup has been successfully removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                            _mb1.ShowDialog();
                            refresh("ViewAll", "");
                            loadAllMenuItems();
                        }
                    }
                    catch (Exception ex)
                    {
                        Trans.Rollback();
                        MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Search"))
            {
                return;
            }
            try
            {
                loSearch.ShowDialog();
                if (loSearch.lFromShow)
                {
                    ldtShow = loCommon.getDataFromSearch(loSearch.lQueryShow);
                    GlobalFunctions.refreshGrid(ref dgvUserGroups, ldtShow);
                    lFromRefresh = false;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.Close);
                mb.ShowDialog();
                return;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Preview"))
            {
                return;
            }
            if (dgvUserGroups.Rows.Count != 0)
            {
                if (lFromRefresh)
                {
                    DataTable dtReportViewer = GlobalFunctions.convertDataGridToReportViewerDataTable(dgvUserGroups);
                    GlobalFunctions.displayReportPreview(dgvUserGroups, dtReportViewer, "User Groups", "User Groups");
                }
                else
                {
                    ldtReport = loCommon.getDataFromSearch(loSearch.lQueryReport);
                    try
                    {
                        ldtReportSum = loCommon.getDataFromSearch(loSearch.lQuerySum);
                    }
                    catch { }
                    GlobalFunctions.displayReportPreviewFromSearch(ldtReport, ldtReportSum, loSearch.lParamFields,
                        loSearch.lTitle, loSearch.lSubTitle, loSearch.lPaperLayout, loSearch.lPaperSize);
                }
            }
        }

        private void dgvUserGroups_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                lUserGroupId = dgvUserGroups.CurrentRow.Cells[0].Value.ToString();
                lUserGroupDesc = dgvUserGroups.CurrentRow.Cells[1].Value.ToString();
                loadAllMenuItems();
            }
            catch { }
        }

        private void dgvMenuItems_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void dgvUserGroups_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvUserGroups.Columns[e.ColumnIndex].Name == "Id")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            refresh("", txtSearch.Text);
        }

        private void btnSaveMenu_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Save Menu"))
            {
                return;
            }
            try
            {
                if (loUserGroup.updateUserGroupMenuItem(dgvUserGroups.CurrentRow.Cells[0].Value.ToString(), GlobalFunctions.convertDataGridToDataTable(dgvMenuItems)))
                {
                    MessageBoxUI _mb = new MessageBoxUI("User Rights has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate"))
                {
                    MessageBoxUI _mb = new MessageBoxUI(ex.Message, GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            btnCreate_Click(null, new EventArgs());
        }

        private void tsmiUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, new EventArgs());
        }

        private void tsmiRemove_Click(object sender, EventArgs e)
        {
            btnRemove_Click(null, new EventArgs());
        }

        private void tsmiSearch_Click(object sender, EventArgs e)
        {
            btnSearch_Click(null, new EventArgs());
        }

        private void tsmiPreview_Click(object sender, EventArgs e)
        {
            btnPreview_Click(null, new EventArgs());
        }

        private void tsmiSaveMenu_Click(object sender, EventArgs e)
        {
            btnSaveMenu_Click(null, new EventArgs());
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmUserGroups", "Reload"))
            {
                return;
            }
            loadAllMenuItems();
        }

        private void tsmiReload_Click(object sender, EventArgs e)
        {
            btnReload_Click(null, new EventArgs());
        }

        private void tsmiSaveRights_Click(object sender, EventArgs e)
        {
            btnSaveRights_Click(null, new EventArgs());
        }
    }
}
