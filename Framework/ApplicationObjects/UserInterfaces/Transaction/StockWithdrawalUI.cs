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
using NSites.ApplicationObjects.UserInterfaces.Transaction;
using NSites.ApplicationObjects.UserInterfaces.Transaction.Details;
using NSites.ApplicationObjects.UserInterfaces.Report.ReportRpt;
using NSites.ApplicationObjects.UserInterfaces.Report.TransactionRpt;
using NSites.ApplicationObjects.UserInterfaces.Report;

namespace NSites.ApplicationObjects.UserInterfaces.Transaction
{
    public partial class StockWithdrawalUI : Form
    {
        #region "VARIABLES"
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;

        SearchesUI loSearches;
        Common loCommon;
        StockWithdrawalRpt loStockWithdrawalRpt;
        StockWithdrawalDetailRpt loStockWithdrawalDetailRpt;
        ReportViewerUI loReportViewer;

        System.Data.DataTable ldtHeader;
        System.Data.DataTable ldtDetail;
        #endregion "END OF VARIABLES"

        public StockWithdrawalUI()
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();

            loCommon = new Common();
            loStockWithdrawalRpt = new StockWithdrawalRpt();
            loStockWithdrawalDetailRpt = new StockWithdrawalDetailRpt();
            loReportViewer = new ReportViewerUI();
            ldtHeader = new System.Data.DataTable();
            ldtDetail = new System.Data.DataTable();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public void refresh(string pDisplay, string pPrimaryKey, string pSearchString, string pType)
        {
            ldtHeader = loInventoryHeader.getAllData(pDisplay, pPrimaryKey, pSearchString, pType);
            GlobalFunctions.refreshGrid(ref dgvList, ldtHeader);
            dgvDetails.DataSource = null;
            try
            {
                ldtDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtDetail;
            }
            catch { }
        }

        public void addData(string[] pRecordData)
        {
            try
            {
                int n = dgvList.Rows.Add();
                for (int i = 0; i < pRecordData.Length; i++)
                {
                    dgvList.Rows[n].Cells[i].Value = pRecordData[i];
                }
            }
            catch
            {
                GlobalFunctions.refreshAll(ref dgvList, ldtHeader);
            }
        }

        public void updateData(string[] pRecordData)
        {
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvList.CurrentRow.Cells[i].Value = pRecordData[i];
            }
        }

        private void previewDetails(string pHeaderId)
        {
            try
            {
                foreach (DataRow _dr in loInventoryHeader.getAllData("", pHeaderId, "", "Stock Withdrawal").Rows)
                {
                    DataTable _dt = loInventoryDetail.getAllData(_dr[0].ToString());
                    loStockWithdrawalDetailRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                    loStockWithdrawalDetailRpt.Database.Tables[1].SetDataSource(_dt);
                    loStockWithdrawalDetailRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                    loStockWithdrawalDetailRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                    loStockWithdrawalDetailRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                    loStockWithdrawalDetailRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                    loStockWithdrawalDetailRpt.SetParameterValue("Title", "Stock Withdrawal Slip");
                    loStockWithdrawalDetailRpt.SetParameterValue("SubTitle", "Stock Withdrawal Slip");
                    loStockWithdrawalDetailRpt.SetParameterValue("Id", _dr["Id"].ToString());
                    loStockWithdrawalDetailRpt.SetParameterValue("Date", string.Format("{0:MM-dd-yyyy}", DateTime.Parse(_dr["Date"].ToString())));
                    foreach (DataRow _dr1 in _dt.Rows)
                    {
                        loStockWithdrawalDetailRpt.SetParameterValue("Location", _dr1["Location"].ToString());
                    }
                    loStockWithdrawalDetailRpt.SetParameterValue("Customer", _dr["Customer"].ToString());
                    loStockWithdrawalDetailRpt.SetParameterValue("Reference", _dr["Reference"].ToString());
                    loStockWithdrawalDetailRpt.SetParameterValue("ReleasedBy", _dr["Username"].ToString());
                    loStockWithdrawalDetailRpt.SetParameterValue("Remarks", _dr["Remarks"].ToString());
                    loReportViewer.crystalReportViewer.ReportSource = loStockWithdrawalDetailRpt;
                    loReportViewer.ShowDialog();
                }
            }
            catch { }
        }
        #endregion "END OF METHODS"

        private void WarehouseWithdrawalUI_Load(object sender, EventArgs e)
        {
            Type _Type = typeof(InventoryHeader);
            FieldInfo[] myFieldInfo;
            myFieldInfo = _Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);
            loSearches = new SearchesUI(myFieldInfo, _Type, "tsmStockWithdrawal");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Search"))
            {
                return;
            }
            try
            {
                string _DisplayFields = "SELECT ih.HeaderId AS `Id`,DATE_FORMAT(ih.`Date`,'%m-%d-%Y') AS `Date`,ih.Final, "+
					"ih.Reference,c.Name AS `Customer`,ih.TotalOut AS `Total Qty-OUT`, "+
					"ih.TotalAmount AS `Total Amount`, "+
					"ih.Username AS `Released By`,DATE_FORMAT(ih.FinalDate,'%m-%d-%Y') AS `Final Date`, "+
					"ih.FinalizedBy AS `Finalized By`,ih.Remarks "+
					"FROM inventoryheader ih "+
                    "LEFT JOIN customer c " +
					"ON ih.CustomerId = c.Id ";
                string _WhereFields = " AND ih.`Status` = 'Active' AND ih.Type = 'Stock Withdrawal' ORDER BY ih.HeaderId DESC;";
                loSearches.lAlias = "ih.";
                loSearches.ShowDialog();
                if (loSearches.lFromShow)
                {
                    ldtHeader = loCommon.getDataFromSearch(_DisplayFields + loSearches.lQuery + _WhereFields);
                    GlobalFunctions.refreshGrid(ref dgvList, ldtHeader);
                    dgvDetails.DataSource = null;
                    try
                    {
                        ldtDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                        dgvDetails.DataSource = ldtDetail;
                    }
                    catch { }
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
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Preview"))
            {
                return;
            }
            try
            {
                loStockWithdrawalRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loStockWithdrawalRpt.Database.Tables[1].SetDataSource(ldtHeader);
                loStockWithdrawalRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loStockWithdrawalRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loStockWithdrawalRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loStockWithdrawalRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loStockWithdrawalRpt.SetParameterValue("Title", "Stock Withdrawals");
                loStockWithdrawalRpt.SetParameterValue("SubTitle", "Stock Withdrawals");
                loReportViewer.crystalReportViewer.ReportSource = loStockWithdrawalRpt;
                loReportViewer.ShowDialog();
            }
            catch { }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Remove"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Receiving").Rows)
                    {
                        if (_drF["Final"].ToString() == "Y")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Withdrawal is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                            mb.ShowDialog();
                            return;
                        }
                    }

                    DialogResult _dr = new DialogResult();
                    MessageBoxUI _mb = new MessageBoxUI("Are sure you want to continue removing this record?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                    _mb.ShowDialog();
                    _dr = _mb.Operation;
                    if (_dr == DialogResult.Yes)
                    {
                        MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
                        try
                        {
                            if (loInventoryHeader.remove(dgvList.CurrentRow.Cells[0].Value.ToString(), ref Trans))
                            {
                                Trans.Commit();
                                MessageBoxUI _mb1 = new MessageBoxUI("Record has been successfully removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                                _mb1.ShowDialog();
                                refresh("ViewAll", "", "", "Stock Withdrawal");
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
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Refresh"))
            {
                return;
            }
            refresh("ViewAll", "", "", "Stock Withdrawal");
        }

        private void tsmViewHiddenRecords_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvList, ldtHeader);
        }
        /*
        private void tsmExportToExcel_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmWarehouseWithdrawal", "Export to Excel"))
            {
                return;
            }
            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
                    return;
                }
                xlApp.Visible = true;

                Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = (Worksheet)wb.Worksheets[1];

                if (ws == null)
                {
                    Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
                }
                try
                {
                    for (int j = 1; j <= dgvList.Columns.Count; j++)
                    {
                        ws.Cells[1, j] = dgvList.Columns[j - 1].Name;
                    }
                }
                catch { }

                for (int i = 2; i <= dgvList.Rows.Count + 1; i++)
                {
                    for (int j = 1; j <= dgvList.Columns.Count; j++)
                    {
                        ws.Cells[i, j] = dgvList.Rows[i - 2].Cells[j - 1].Value.ToString();
                    }
                }
            }
            catch { }
        }
        */
        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvList.Columns[e.ColumnIndex].Name == "Total Qty-OUT" || this.dgvList.Columns[e.ColumnIndex].Name == "Total Amount")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvList.Columns[e.ColumnIndex].Name == "Id" || this.dgvList.Columns[e.ColumnIndex].Name == "Released By" ||
                    this.dgvList.Columns[e.ColumnIndex].Name == "Reference" || this.dgvList.Columns[e.ColumnIndex].Name == "Finalized By" ||
                    this.dgvList.Columns[e.ColumnIndex].Name == "Final")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch { }
        }

        private void dgvDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvDetails.Columns[e.ColumnIndex].Name == "Qty-OUT" ||
                    this.dgvDetails.Columns[e.ColumnIndex].Name == "Balance" || this.dgvDetails.Columns[e.ColumnIndex].Name == "Unit Price" ||
                    this.dgvDetails.Columns[e.ColumnIndex].Name == "Total Price")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvDetails.Columns[e.ColumnIndex].Name == "Id" || this.dgvDetails.Columns[e.ColumnIndex].Name == "Stock Id" ||
                    this.dgvDetails.Columns[e.ColumnIndex].Name == "Unit")
                {
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch { }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Create"))
            {
                return;
            }
            StockWithdrawalDetailUI loStockWithdrawalDetail = new StockWithdrawalDetailUI();
            loStockWithdrawalDetail.ParentList = this;
            loStockWithdrawalDetail.ShowDialog();
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ldtDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtDetail;
            }
            catch { }
        }

        private void dgvList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point pt = dgvList.PointToScreen(e.Location);
                cmsFunctions.Show(pt);
            }
        }

        private void tsmPreviewDetails_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Preview Details"))
            {
                return;
            }
            foreach (DataRow _dr in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Withdrawal").Rows)
            {
                if (_dr["Final"].ToString() == "N")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Withdrawal must be FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            previewDetails(dgvList.CurrentRow.Cells[0].Value.ToString());
            
        }

        private void tsmRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh_Click(null, new EventArgs());
        }

        private void tsmCreate_Click(object sender, EventArgs e)
        {
            btnCreate_Click(null, new EventArgs());
        }

        private void tsmSearch_Click(object sender, EventArgs e)
        {
            btnSearch_Click(null, new EventArgs());
        }

        private void tsmPreview_Click(object sender, EventArgs e)
        {
            btnPreview_Click(null, new EventArgs());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Update"))
            {
                return;
            }
            foreach (DataRow _dr in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Withdrawal").Rows)
            {
                if (_dr["Final"].ToString() == "Y")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Withdrawal is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            StockWithdrawalDetailUI loStockWithdrawalDetail = new StockWithdrawalDetailUI(dgvList.CurrentRow.Cells[0].Value.ToString());
            loStockWithdrawalDetail.ParentList = this;
            loStockWithdrawalDetail.ShowDialog();
        }

        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Finalize"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Withdtrawal").Rows)
                    {
                        if (_drF["Final"].ToString() == "Y")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Withdrawal is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                            mb.ShowDialog();
                            return;
                        }
                    }

                    DialogResult _dr = new DialogResult();
                    MessageBoxUI _mb = new MessageBoxUI("Are sure you want to continue finalizing this record?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                    _mb.ShowDialog();
                    _dr = _mb.Operation;
                    if (_dr == DialogResult.Yes)
                    {
                        MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
                        try
                        {
                            if (loInventoryHeader.finalize(dgvList.CurrentRow.Cells[0].Value.ToString(), ref Trans))
                            {
                                foreach (DataRow _dr1 in loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString()).Rows)
                                {
                                    loInventoryDetail.updateQtyOnHand(_dr1[0].ToString(), ref Trans);
                                }
                                
                                Trans.Commit();
                                MessageBoxUI _mb1 = new MessageBoxUI("Record has been successfully finalized!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                                _mb1.ShowDialog();

                                previewDetails(dgvList.CurrentRow.Cells[0].Value.ToString());

                                refresh("ViewAll", "", "", "Stock Withdrawal");
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
            catch { }
        }

        private void tsmUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, new EventArgs());
        }

        private void tsmRemove_Click(object sender, EventArgs e)
        {
            btnRemove_Click(null, new EventArgs());
        }

        private void tsmFinalize_Click(object sender, EventArgs e)
        {
            btnFinalize_Click(null, new EventArgs());
        }

        private void tsmiForcedRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockWithdrawal", "Forced Remove"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Withdrawal").Rows)
                    {
                        if (_drF["Final"].ToString() == "N")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Withdrawal must be finalized to be forced removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                            mb.ShowDialog();
                            return;
                        }
                    }

                    DialogResult _dr = new DialogResult();
                    MessageBoxUI _mb = new MessageBoxUI("Are sure you want to continue removing this record?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
                    _mb.ShowDialog();
                    _dr = _mb.Operation;
                    if (_dr == DialogResult.Yes)
                    {
                        MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
                        try
                        {
                            if (loInventoryHeader.remove(dgvList.CurrentRow.Cells[0].Value.ToString(), ref Trans))
                            {
                                Trans.Commit();
                                MessageBoxUI _mb1 = new MessageBoxUI("Record has been successfully forced remove!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                                _mb1.ShowDialog();
                                refresh("ViewAll", "", "", "Stock Withdrawal");
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
            catch { }
        }
    }
}
