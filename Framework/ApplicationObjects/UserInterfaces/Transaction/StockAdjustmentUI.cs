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
    public partial class StockAdjustmentUI : Form
    {
        #region "VARIABLES"
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;

        SearchesUI loSearches;
        Common loCommon;
        StockAdjustmentRpt loStockAdjustmentRpt;
        StockAdjustmentDetailRpt loStockAdjustmentDetailRpt;
        ReportViewerUI loReportViewer;
        System.Data.DataTable ldtStockAdjustment;
        System.Data.DataTable ldtStockAdjustmentDetail;
        #endregion "END OF VARIABLES"

        public StockAdjustmentUI()
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();

            loCommon = new Common();
            loStockAdjustmentRpt = new StockAdjustmentRpt();
            loStockAdjustmentDetailRpt = new StockAdjustmentDetailRpt();
            loReportViewer = new ReportViewerUI();
            ldtStockAdjustment = new System.Data.DataTable();
            ldtStockAdjustmentDetail = new System.Data.DataTable();
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
            ldtStockAdjustment = loInventoryHeader.getAllData(pDisplay, pPrimaryKey, pSearchString, pType);
            GlobalFunctions.refreshGrid(ref dgvList, ldtStockAdjustment);
            dgvDetails.DataSource = null;
            try
            {
                ldtStockAdjustmentDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtStockAdjustmentDetail;
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
                GlobalFunctions.refreshAll(ref dgvList, ldtStockAdjustment);
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
                foreach (DataRow _dr in loInventoryHeader.getAllData("", pHeaderId, "", "").Rows)
                {
                    DataTable _dt = loInventoryDetail.getAllData(_dr[0].ToString());
                    loStockAdjustmentDetailRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                    loStockAdjustmentDetailRpt.Database.Tables[1].SetDataSource(_dt);
                    loStockAdjustmentDetailRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                    loStockAdjustmentDetailRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                    loStockAdjustmentDetailRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                    loStockAdjustmentDetailRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                    loStockAdjustmentDetailRpt.SetParameterValue("Title", "Stock Adjustment Slip");
                    loStockAdjustmentDetailRpt.SetParameterValue("SubTitle", "Stock Adjustment Slip");
                    loStockAdjustmentDetailRpt.SetParameterValue("Id", _dr["Id"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("Date", string.Format("{0:MM-dd-yyyy}", DateTime.Parse(_dr["Date"].ToString())));
                    foreach (DataRow _dr1 in _dt.Rows)
                    {
                        loStockAdjustmentDetailRpt.SetParameterValue("Location", _dr1["Location"].ToString());
                    }
                    loStockAdjustmentDetailRpt.SetParameterValue("Type", _dr["Type"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("Supplier", _dr["Supplier"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("Customer", _dr["Customer"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("Reference", _dr["Reference"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("AdjustedBy", _dr["Username"].ToString());
                    loStockAdjustmentDetailRpt.SetParameterValue("Remarks", _dr["Remarks"].ToString());
                    loReportViewer.crystalReportViewer.ReportSource = loStockAdjustmentDetailRpt;
                    loReportViewer.ShowDialog();
                }
            }
            catch { }
        }

        #endregion "END OF METHODS"

        private void StockAdjustmentUI_Load(object sender, EventArgs e)
        {
            Type _Type = typeof(InventoryHeader);
            FieldInfo[] myFieldInfo;
            myFieldInfo = _Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);
            loSearches = new SearchesUI(myFieldInfo, _Type, "tsmStockAdjustment");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Refresh"))
            {
                return;
            }
            refresh("ViewAll", "", "", "");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ldtStockAdjustmentDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtStockAdjustmentDetail;
            }
            catch { }
        }

        private void dgvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvList.Columns[e.ColumnIndex].Name == "Total Qty-IN" || this.dgvList.Columns[e.ColumnIndex].Name == "Total Qty-OUT" ||
                    this.dgvList.Columns[e.ColumnIndex].Name == "Total Amount")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvList.Columns[e.ColumnIndex].Name == "Id" || this.dgvList.Columns[e.ColumnIndex].Name == "Adjusted By" ||
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
                if (this.dgvDetails.Columns[e.ColumnIndex].Name == "Qty-IN" || this.dgvDetails.Columns[e.ColumnIndex].Name == "Qty-OUT" ||
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
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Create"))
            {
                return;
            }
            StockAdjustmentDetailUI loStockAdjustmentDetailUI = new StockAdjustmentDetailUI();
            loStockAdjustmentDetailUI.ParentList = this;
            loStockAdjustmentDetailUI.ShowDialog();
        }

        private void dgvList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point pt = dgvList.PointToScreen(e.Location);
                cmsFunctions.Show(pt);
            }
        }

        private void tsmViewHiddenRecords_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvList, ldtStockAdjustment);
        }
        /*
        private void tsmExportToExcel_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Export to Excel"))
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Search"))
            {
                return;
            }
            try
            {
                string _DisplayFields = "SELECT ih.HeaderId AS `Id`,DATE_FORMAT(ih.`Date`,'%m-%d-%Y') AS `Date`,ih.Type,ih.Final, "+
					    "ih.Reference,s.Name AS `Supplier`,c.Name AS `Customer`,ih.TotalIn AS `Total Qty-IN`,ih.TotalOUT AS `Total Qty-OUT`, "+
					    "ih.TotalAmount AS `Total Amount`, "+
					    "ih.Username AS `Adjusted By`,DATE_FORMAT(ih.FinalDate,'%m-%d-%Y') AS `Final Date`, "+
					    "ih.FinalizedBy AS `Finalized By`,ih.Remarks "+
					    "FROM inventoryheader ih "+
					    "LEFT JOIN customer c "+
					    "ON ih.CustomerId = c.Id "+
                        "LEFT JOIN supplier s " +
					    "ON ih.SupplierId = s.Id ";
                string _WhereFields = " AND ih.`Status` = 'Active' AND ih.Type = 'Stock Adjustment' ORDER BY ih.HeaderId DESC;";
                loSearches.lAlias = "ih.";
                loSearches.ShowDialog();
                if (loSearches.lFromShow)
                {
                    ldtStockAdjustment = loCommon.getDataFromSearch(_DisplayFields + loSearches.lQuery + _WhereFields);
                    GlobalFunctions.refreshGrid(ref dgvList, ldtStockAdjustment);
                    dgvDetails.DataSource = null;
                    try
                    {
                        ldtStockAdjustmentDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                        dgvDetails.DataSource = ldtStockAdjustmentDetail;
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

        private void tsmPreview_Click(object sender, EventArgs e)
        {
            btnPreview_Click(null, new EventArgs());
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Preview"))
            {
                return;
            }
            try
            {
                loStockAdjustmentRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loStockAdjustmentRpt.Database.Tables[1].SetDataSource(ldtStockAdjustment);
                loStockAdjustmentRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loStockAdjustmentRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loStockAdjustmentRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loStockAdjustmentRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loStockAdjustmentRpt.SetParameterValue("Title", "Stock Adjustment");
                loStockAdjustmentRpt.SetParameterValue("SubTitle", "Stock Adjustment");
                loReportViewer.crystalReportViewer.ReportSource = loStockAdjustmentRpt;
                loReportViewer.ShowDialog();
            }
            catch { }
        }

        private void tsmPreviewDetails_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Preview Details"))
            {
                return;
            }

            foreach (DataRow _dr in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Adjustment").Rows)
            {
                if (_dr["Final"].ToString() == "N")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Adjustment must be FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            previewDetails(dgvList.CurrentRow.Cells[0].Value.ToString());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Update"))
            {
                return;
            }
            foreach (DataRow _dr in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Adjustment").Rows)
            {
                if (_dr["Final"].ToString() == "Y")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Adjustment is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            StockAdjustmentDetailUI loStockAdjustmentDetail = new StockAdjustmentDetailUI(dgvList.CurrentRow.Cells[0].Value.ToString());
            loStockAdjustmentDetail.ParentList = this;
            loStockAdjustmentDetail.ShowDialog();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Remove"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Adjustment").Rows)
                    {
                        if (_drF["Final"].ToString() == "Y")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Adjustment is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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
                                refresh("ViewAll", "", "", "Stock Adjustment");
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

        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Finalize"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Adjustment").Rows)
                    {
                        if (_drF["Final"].ToString() == "Y")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Adjustment is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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

                                refresh("ViewAll", "", "", "Stock Adjustment");
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
            if (!GlobalFunctions.checkRights("tsmStockAdjustment", "Forced Remove"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Adjustment").Rows)
                    {
                        if (_drF["Final"].ToString() == "N")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Adjustment must be finalized to be forced removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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
                                refresh("ViewAll", "", "", "Stock Adjustment");
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
