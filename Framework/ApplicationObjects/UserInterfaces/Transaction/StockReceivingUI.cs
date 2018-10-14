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
    public partial class StockReceivingUI : Form
    {
        #region "VARIABLES"
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;

        SearchesUI loSearches;
        Common loCommon;
        StockReceivingRpt loStockReceivingRpt;
        StockReceivingDetailRpt loStockReceivingDetailRpt;
        ReportViewerUI loReportViewer;
        System.Data.DataTable ldtStockReceiving;
        System.Data.DataTable ldtStockReceivingDetail;
        #endregion "END OF VARIABLES"

        public StockReceivingUI()
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();

            loCommon = new Common();
            loStockReceivingRpt = new StockReceivingRpt();
            loStockReceivingDetailRpt = new StockReceivingDetailRpt();
            loReportViewer = new ReportViewerUI();
            ldtStockReceiving = new System.Data.DataTable();
            ldtStockReceivingDetail = new System.Data.DataTable();
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
            ldtStockReceiving = loInventoryHeader.getAllData(pDisplay, pPrimaryKey, pSearchString, pType);
            GlobalFunctions.refreshGrid(ref dgvList, ldtStockReceiving);
            dgvDetails.DataSource = null;
            try
            {
                ldtStockReceivingDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtStockReceivingDetail;
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
                GlobalFunctions.refreshAll(ref dgvList, ldtStockReceiving);
            }
        }

        public void updateData(string[] pRecordData)
        {
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvList.CurrentRow.Cells[i].Value = pRecordData[i];
            }
        }

        private void previewDetails(string pSRId)
        {
            try
            {
                foreach (DataRow _dr in loInventoryHeader.getAllData("", pSRId, "", "Stock Receiving").Rows)
                {
                    DataTable _dt = loInventoryDetail.getAllData(_dr[0].ToString());
                    loStockReceivingDetailRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                    loStockReceivingDetailRpt.Database.Tables[1].SetDataSource(_dt);
                    loStockReceivingDetailRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                    loStockReceivingDetailRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                    loStockReceivingDetailRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                    loStockReceivingDetailRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                    loStockReceivingDetailRpt.SetParameterValue("Title", "Stock Receiving Slip");
                    loStockReceivingDetailRpt.SetParameterValue("SubTitle", "Stock Receiving Slip");
                    loStockReceivingDetailRpt.SetParameterValue("Id", _dr["Id"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Date", string.Format("{0:MM-dd-yyyy}", DateTime.Parse(_dr["Date"].ToString())));
                    foreach (DataRow _dr1 in _dt.Rows)
                    {
                        loStockReceivingDetailRpt.SetParameterValue("Location", _dr1["Location"].ToString());
                    }
                    loStockReceivingDetailRpt.SetParameterValue("Supplier", _dr["Supplier"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Reference", _dr["Reference"].ToString());
                    //loStockReceivingDetailRpt.SetParameterValue("TotalAmount", string.Format("{0:n}", decimal.Parse(_dr["Total Amount"].ToString())));
                    loStockReceivingDetailRpt.SetParameterValue("ReceivedBy", _dr["Username"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Remarks", _dr["Remarks"].ToString());
                    loReportViewer.crystalReportViewer.ReportSource = loStockReceivingDetailRpt;
                    loReportViewer.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion "END OF METHODS"

        private void StockReceivingUI_Load(object sender, EventArgs e)
        {
            Type _Type = typeof(InventoryHeader);
            FieldInfo[] myFieldInfo;
            myFieldInfo = _Type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.Public);
            loSearches = new SearchesUI(myFieldInfo, _Type, "tsmStockReceiving");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Refresh"))
            {
                return;
            }
            refresh("ViewAll","", "", "Stock Receiving");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void dgvList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //ldtStockReceivingDetail = loStockReceivingDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                ldtStockReceivingDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                dgvDetails.DataSource = ldtStockReceivingDetail;
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
                else if (this.dgvList.Columns[e.ColumnIndex].Name == "Id" || this.dgvList.Columns[e.ColumnIndex].Name == "Received By" ||
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
                if (this.dgvDetails.Columns[e.ColumnIndex].Name == "Qty-IN" || 
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
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Create"))
            {
                return;
            }
            StockReceivingDetailUI loStockReceivingDetail = new StockReceivingDetailUI();
            loStockReceivingDetail.ParentList = this;
            loStockReceivingDetail.ShowDialog();
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
            GlobalFunctions.refreshAll(ref dgvList, ldtStockReceiving);
        }
        /*
        private void tsmExportToExcel_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Export to Excel"))
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
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Search"))
            {
                return;
            }
            try
            {
                string _DisplayFields = "SELECT ih.HeaderId AS `Id`,DATE_FORMAT(ih.`Date`,'%m-%d-%Y') AS `Date`,ih.Final, "+
				    "ih.Reference,s.Name AS `Supplier`,ih.TotalIn AS `Total Qty-IN`, "+
				    "ih.TotalAmount AS `Total Amount`, "+
				    "ih.Username AS `Received By`,DATE_FORMAT(ih.FinalDate,'%m-%d-%Y') AS `Final Date`, "+
				    "ih.FinalizedBy AS `Finalized By`,ih.Remarks "+
				    "FROM inventoryheader ih "+
                    "LEFT JOIN supplier s " +
				    "ON ih.SupplierId = s.Id ";
                string _WhereFields = " AND ih.`Status` = 'Active' AND ih.Type = 'Stock Receiving' ORDER BY ih.HeaderId DESC;";
                loSearches.lAlias = "ih.";
                loSearches.ShowDialog();
                if (loSearches.lFromShow)
                {
                    ldtStockReceiving = loCommon.getDataFromSearch(_DisplayFields + loSearches.lQuery + _WhereFields);
                    GlobalFunctions.refreshGrid(ref dgvList, ldtStockReceiving);
                    dgvDetails.DataSource = null;
                    try
                    {
                        ldtStockReceivingDetail = loInventoryDetail.getAllData(dgvList.CurrentRow.Cells[0].Value.ToString());
                        dgvDetails.DataSource = ldtStockReceivingDetail;
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
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Preview"))
            {
                return;
            }
            try
            {
                loStockReceivingRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loStockReceivingRpt.Database.Tables[1].SetDataSource(ldtStockReceiving);
                loStockReceivingRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loStockReceivingRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loStockReceivingRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loStockReceivingRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loStockReceivingRpt.SetParameterValue("Title", "Stock Receiving");
                loStockReceivingRpt.SetParameterValue("SubTitle", "Stock Receiving");
                loReportViewer.crystalReportViewer.ReportSource = loStockReceivingRpt;
                loReportViewer.ShowDialog();
            }
            catch { }
        }

        private void tsmPreviewDetails_Click(object sender, EventArgs e)
        {
            foreach (DataRow _dr in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Receiving").Rows)
            {
                if (_dr["Final"].ToString() == "N")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Receiving must be FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            previewDetails(dgvList.CurrentRow.Cells[0].Value.ToString());
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Remove"))
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
                            MessageBoxUI mb = new MessageBoxUI("Stock Receiving is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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
                                refresh("ViewAll","", "","Stock Receiving");
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
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Finalize"))
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
                            MessageBoxUI mb = new MessageBoxUI("Stock Receiving is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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

                                refresh("ViewAll", "", "", "Stock Receiving");
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Update"))
            {
                return;
            }
            foreach (DataRow _dr in loInventoryHeader.getAllData("",dgvList.CurrentRow.Cells[0].Value.ToString(),"","Stock Receiving").Rows)
            {
                if (_dr["Final"].ToString() == "Y")
                {
                    MessageBoxUI mb = new MessageBoxUI("Stock Receiving is already FINALIZED!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            StockReceivingDetailUI loStockReceivingDetail = new StockReceivingDetailUI(dgvList.CurrentRow.Cells[0].Value.ToString());
            loStockReceivingDetail.ParentList = this;
            loStockReceivingDetail.ShowDialog();
        }

        private void dgvList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate_Click(null, new EventArgs());
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
            if (!GlobalFunctions.checkRights("tsmStockReceiving", "Forced Remove"))
            {
                return;
            }
            try
            {
                if (dgvList.Rows.Count > 0)
                {
                    foreach (DataRow _drF in loInventoryHeader.getAllData("", dgvList.CurrentRow.Cells[0].Value.ToString(), "", "Stock Receiving").Rows)
                    {
                        if (_drF["Final"].ToString() == "N")
                        {
                            MessageBoxUI mb = new MessageBoxUI("Stock Receiving must be finalized to be forced removed!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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
                                refresh("ViewAll", "", "", "Stock Receiving");
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
