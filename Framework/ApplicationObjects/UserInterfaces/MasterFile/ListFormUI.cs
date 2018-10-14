using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.Classes;
using NSites.ApplicationObjects.UserInterfaces.Systems;

namespace NSites.ApplicationObjects.UserInterfaces.MasterFile
{
    public partial class ListFormUI : Form
    {
        #region "VARIABLES"
        public object lObject;
        public object lObjectUI;
        Type lTypeUI;
        Type lType;
        string[] lRecord;
        string[] lColumnName;
        int lCountCol;
        SearchUI loSearch;
        Common loCommon;
        DataTable ldtShow;
        DataTable ldtReport;
        DataTable ldtReportSum;
        bool lFromRefresh;
        #endregion "END OF VARIABLES"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "CONSTRUCTORS"
        public ListFormUI(object pObject, object pObjectUI, Type pTypeUI, Type pType)
        {
            InitializeComponent();
            lObject = pObject;
            lObjectUI = pObjectUI;
            lTypeUI = pTypeUI;
            lType = pType;
            this.Text = pObject.GetType().Name + " List";
            loCommon = new Common();
            ldtShow = new DataTable();
            ldtReport = new DataTable();
            ldtReportSum = new DataTable();
            lFromRefresh = false;
        }
        #endregion "END OF CONSTRUCTORS"

        #region "METHODS"
        public void refresh(string pDisplayType,string pSearchString, bool pShowRecord)
        {
            lFromRefresh = true;
            try
            {
                dgvLists.Rows.Clear();
                dgvLists.Columns.Clear();
            }
            catch
            {
                dgvLists.DataSource = null;
            }
            tsmiViewAllRecords.Visible = false;
            object[] _params = { pDisplayType, pSearchString };

            ldtShow = (DataTable)lObject.GetType().GetMethod("getAllData").Invoke(lObject, _params);
            if (ldtShow == null)
            {
                return;
            }
            lCountCol = ldtShow.Columns.Count;
            lColumnName = new string[lCountCol];
            lRecord = new string[lCountCol];
            for (int i = 0; i < lCountCol; i++)
            {
                dgvLists.Columns.Add(ldtShow.Columns[i].ColumnName, ldtShow.Columns[i].ColumnName);
            }
            if (pShowRecord)
            {
                foreach (DataRow _dr in ldtShow.Rows)
                {
                    int n = dgvLists.Rows.Add();
                    if (n < GlobalVariables.DisplayRecordLimit)
                    {
                        for (int i = 0; i < lCountCol; i++)
                        {
                            dgvLists.Rows[n].Cells[i].Value = _dr[i].ToString();
                        }
                    }
                    else
                    {
                        dgvLists.Rows[n].DefaultCellStyle.BackColor = Color.Red;
                        dgvLists.Rows[n].DefaultCellStyle.ForeColor = Color.White;
                        dgvLists.Rows[n].Height = 5;
                        dgvLists.Rows[n].ReadOnly = true;
                        tsmiViewAllRecords.Visible = true;
                        break;
                    }
                }
            }
            try
            {
                for (int i = 0; i < lCountCol; i++)
                {
                    lRecord[i] = dgvLists.CurrentRow.Cells[i].Value.ToString();
                }
            }
            catch { }
        }

        public void addData(string[] pRecordData)
        {
            try
            {
                int n = dgvLists.Rows.Add();
                for (int i = 0; i < pRecordData.Length; i++)
                {
                    dgvLists.Rows[n].Cells[i].Value = pRecordData[i];
                }
            }
            catch
            {
                GlobalFunctions.refreshAll(ref dgvLists, ldtShow);
            }
            try
            {
                for (int i = 0; i < lCountCol; i++)
                {
                    lRecord[i] = dgvLists.CurrentRow.Cells[i].Value.ToString();
                }
            }
            catch { }
        }

        public void updateData(string[] pRecordData)
        {
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvLists.CurrentRow.Cells[i].Value = pRecordData[i];
            }
            try
            {
                for (int i = 0; i < lCountCol; i++)
                {
                    lRecord[i] = dgvLists.CurrentRow.Cells[i].Value.ToString();
                }
            }
            catch { }
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void ListFormUI_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.btnExit, "Close Tab");
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.txtSearch, "Enter to View");

            FieldInfo[] myFieldInfo;
            myFieldInfo = lType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);
            loSearch = new SearchUI(myFieldInfo, lType, "tsm"+lType.Name);
        }

        private void dgvLists_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                for (int i = 0; i < lCountCol; i++)
                {
                    lRecord[i] = dgvLists.CurrentRow.Cells[i].Value.ToString();
                }
            }
            catch { }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Update"))
            {
                return;
            }
            try
            {
                try
                {
                    for (int i = 0; i < lCountCol; i++)
                    {
                        lRecord[i] = dgvLists.CurrentRow.Cells[i].Value.ToString();
                    }
                }
                catch
                {
                    MessageBoxUI mb = new MessageBoxUI("Fields are not complete!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
                if (lRecord.Length > 0)
                {
                    try
                    {
                        if (lRecord[0].ToString() != "")
                        {
                            switch (lType.Name)
                            {
                                case "Location":
                                    LocationDetailUI loLocationDetail = new LocationDetailUI(lRecord);
                                    loLocationDetail.ParentList = this;
                                    loLocationDetail.ShowDialog();
                                    break;
                                case "Unit":
                                    UnitDetailUI loUnitDetail = new UnitDetailUI(lRecord);
                                    loUnitDetail.ParentList = this;
                                    loUnitDetail.ShowDialog();
                                    break;
                                case "InventoryGroup":
                                    InventoryGroupDetailUI loInventoryGroupDetail = new InventoryGroupDetailUI(lRecord);
                                    loInventoryGroupDetail.ParentList = this;
                                    loInventoryGroupDetail.ShowDialog();
                                    break;
                                case "Category":
                                    CategoryDetailUI loCategoryDetail = new CategoryDetailUI(lRecord);
                                    loCategoryDetail.ParentList = this;
                                    loCategoryDetail.ShowDialog();
                                    break;
                                case "Stock":
                                    StockDetailUI loStockDetail = new StockDetailUI(lRecord);
                                    loStockDetail.ParentList = this;
                                    loStockDetail.ShowDialog();
                                    break;
                                case "Customer":
                                    CustomerDetailUI loCustomerDetail = new CustomerDetailUI(lRecord);
                                    loCustomerDetail.ParentList = this;
                                    loCustomerDetail.ShowDialog();
                                    break;
                                case "Supplier":
                                    SupplierDetailUI loSupplierDetail = new SupplierDetailUI(lRecord);
                                    loSupplierDetail.ParentList = this;
                                    loSupplierDetail.ShowDialog();
                                    break;
                                case "InventoryType":
                                    InventoryTypeDetailUI loInventoryTypeDetail = new InventoryTypeDetailUI(lRecord);
                                    loInventoryTypeDetail.ParentList = this;
                                    loInventoryTypeDetail.ShowDialog();
                                    break;
                                case "User":
                                    UserDetailUI loUserDetail = new UserDetailUI(lRecord);
                                    loUserDetail.ParentList = this;
                                    loUserDetail.ShowDialog();
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.Close);
                        mb.ShowDialog();
                        return;
                    }
                }
            }
            catch { }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Create"))
            {
                return;
            }
            if (dgvLists.Rows.Count == 0)
            {
                refresh("ViewAll", "", false);
            }
            switch (lType.Name)
            {
                case "Location":
                    LocationDetailUI loLocationDetail = new LocationDetailUI();
                    loLocationDetail.ParentList = this;
                    loLocationDetail.ShowDialog();
                    break;
                case "Unit":
                    UnitDetailUI loUnitDetail = new UnitDetailUI();
                    loUnitDetail.ParentList = this;
                    loUnitDetail.ShowDialog();
                    break;
                case "InventoryGroup":
                    InventoryGroupDetailUI loInventoryGroupDetail = new InventoryGroupDetailUI();
                    loInventoryGroupDetail.ParentList = this;
                    loInventoryGroupDetail.ShowDialog();
                    break;
                case "Category":
                    CategoryDetailUI loCategoryDetail = new CategoryDetailUI();
                    loCategoryDetail.ParentList = this;
                    loCategoryDetail.ShowDialog();
                    break;
                case "Stock":
                    StockDetailUI loStockDetail = new StockDetailUI();
                    loStockDetail.ParentList = this;
                    loStockDetail.ShowDialog();
                    break;
                case "Customer":
                    CustomerDetailUI loCustomerDetail = new CustomerDetailUI();
                    loCustomerDetail.ParentList = this;
                    loCustomerDetail.ShowDialog();
                    break;
                case "Supplier":
                    SupplierDetailUI loSupplierDetail = new SupplierDetailUI();
                    loSupplierDetail.ParentList = this;
                    loSupplierDetail.ShowDialog();
                    break;
                case "InventoryType":
                    InventoryTypeDetailUI loInventoryTypeDetail = new InventoryTypeDetailUI();
                    loInventoryTypeDetail.ParentList = this;
                    loInventoryTypeDetail.ShowDialog();
                    break;
                case "User":
                    UserDetailUI loUserDetail = new UserDetailUI();
                    loUserDetail.ParentList = this;
                    loUserDetail.ShowDialog();
                    break;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Refresh"))
            {
                return;
            }
            refresh("ViewAll", "", true);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Remove"))
            {
                return;
            }
            try
            {
                if (lRecord.Length > 0)
                {
                    if (lRecord[0].ToString() != null)
                    {
                        DialogResult _dr = new DialogResult();
                        MessageBoxUI _mb = new MessageBoxUI("Are sure you want to continue removing this record?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                        _mb.ShowDialog();
                        _dr = _mb.Operation;
                        if (_dr == DialogResult.Yes)
                        {
                            MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
                            object[] param = { lRecord[0].ToString(), Trans };
                            try
                            {
                                if ((bool)lObject.GetType().GetMethod("remove").Invoke(lObject, param))
                                {
                                    Trans.Commit();
                                    MessageBoxUI _mb1 = new MessageBoxUI(lType.Name + " has been successfully removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                                    _mb1.ShowDialog();
                                    refresh("ViewAll", "", true);
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
            }
            catch { }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Search"))
            {
                return;
            }
            try
            {
                loSearch.ShowDialog();
                if (loSearch.lFromShow)
                {
                    ldtShow = loCommon.getDataFromSearch(loSearch.lQueryShow);
                    GlobalFunctions.refreshGrid(ref dgvLists, ldtShow);
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
        #endregion "END OF EVENTS"

        private void dgvLists_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvLists.PointToScreen(e.Location);
                cmsFunction.Show(pt);
            }
        }

        private void tsmiViewAllRecords_Click(object sender, EventArgs e)
        {
            try
            {
                dgvLists.Rows.Clear();
                dgvLists.Columns.Clear();
            }
            catch { }
            try
            {
                dgvLists.DataSource = ldtShow;
            }
            catch { }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsm" + lType.Name, "Preview"))
            {
                return;
            }
            if (dgvLists.Rows.Count != 0)
            {
                if (lFromRefresh)
                {
                    DataTable dtReportViewer = GlobalFunctions.convertDataGridToReportViewerDataTable(dgvLists);
                    GlobalFunctions.displayReportPreview(dgvLists, dtReportViewer, lType.Name, lType.Name);
                }
                else
                {
                    ldtReport = ldtReport = GlobalFunctions.convertDataTableToReportDataTable(loCommon.getDataFromSearch(loSearch.lQueryReport), loSearch.lNoOfParameters);
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnUpdate_Click(null, new EventArgs());
            }
        }

        private void dgvLists_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvLists.Columns[e.ColumnIndex].Name == "Id" || this.dgvLists.Columns[e.ColumnIndex].Name == "Unit")
                {
                    if (e.Value != null)
                    {
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                else if (this.dgvLists.Columns[e.ColumnIndex].Name == "Unit Cost" ||
                    this.dgvLists.Columns[e.ColumnIndex].Name == "Reorder Level" || this.dgvLists.Columns[e.ColumnIndex].Name == "Unit Price" ||
                    this.dgvLists.Columns[e.ColumnIndex].Name == "Credit Limit")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvLists.Columns[e.ColumnIndex].Name == "Terms")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:0}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch { }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            refresh("",txtSearch.Text, true);
        }

        private void dgvLists_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate_Click(null, new EventArgs());
        }

        private void tsmiRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(null, new EventArgs());
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
    }
}
