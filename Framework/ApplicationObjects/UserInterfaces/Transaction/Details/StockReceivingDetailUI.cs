using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.Classes;
using NSites.ApplicationObjects.UserInterfaces.Report;
using NSites.ApplicationObjects.UserInterfaces.Report.TransactionRpt;

namespace NSites.ApplicationObjects.UserInterfaces.Transaction.Details
{
    public partial class StockReceivingDetailUI : Form
    {
        #region "VARIABLES"
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;
        Supplier loSupplier;
        Stock loStock;
        GlobalVariables.Operation lOperation;
        StockReceivingDetailRpt loStockReceivingDetailRpt;
        ReportViewerUI loReportViewer;
        string lInventoryId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public StockReceivingDetailUI()
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();
            loSupplier = new Supplier();
            loStock = new Stock();
            lOperation = GlobalVariables.Operation.Add;
            loStockReceivingDetailRpt = new StockReceivingDetailRpt();
            loReportViewer = new ReportViewerUI();
            lInventoryId = "";
        }
        public StockReceivingDetailUI(string pInventoryId)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();
            loSupplier = new Supplier();
            loStock = new Stock();
            loStockReceivingDetailRpt = new StockReceivingDetailRpt();
            loReportViewer = new ReportViewerUI();
            lInventoryId = pInventoryId;
        }
        #endregion "END OF CONSTRUCTORS"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        public void addData(string[] pRecordData)
        {
            try
            {
                int n = dgvDetail.Rows.Add();
                for (int i = 0; i < pRecordData.Length; i++)
                {
                    dgvDetail.Rows[n].Cells[i].Value = pRecordData[i];
                }
                dgvDetail.CurrentRow.Selected = false;
                dgvDetail.FirstDisplayedScrollingRowIndex = dgvDetail.Rows[n].Index;
                dgvDetail.Rows[n].Selected = true;
            }
            catch { }
            computeTotalAmount();
        }

        public void updateData(string[] pRecordData)
        {
            string _operator = dgvDetail.CurrentRow.Cells[11].Value.ToString();
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvDetail.CurrentRow.Cells[i].Value = pRecordData[i];
            }
            if (_operator == "Add")
            {
                dgvDetail.CurrentRow.Cells[11].Value = "Add";
            }
            computeTotalAmount();
        }

        private void computeTotalAmount()
        {
            decimal _TotalQtyIN = 0;
            decimal _TotalAmount = 0;

            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                if (dgvDetail.Rows[i].Visible == true)
                {
                    try
                    {
                        _TotalQtyIN += decimal.Parse(dgvDetail.Rows[i].Cells[6].Value.ToString());
                        _TotalAmount += decimal.Parse(dgvDetail.Rows[i].Cells[9].Value.ToString());
                    }
                    catch { }
                }
            }
            txtTotalQty.Text = string.Format("{0:n}", _TotalQtyIN);
            txtTotalAmount.Text = string.Format("{0:n}", _TotalAmount);
        }

        private void StockReceivingDetailUI_Load(object sender, EventArgs e)
        {
            cboSupplier.DataSource = loSupplier.getAllData("ViewAll","");
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
            cboSupplier.SelectedIndex = -1;

            if (lOperation == GlobalVariables.Operation.Edit)
            {
                foreach (DataRow _dr in loInventoryHeader.getAllData("",lInventoryId,"","Stock Receiving").Rows)
                {
                    txtStockReceivingNo.Text = _dr["Id"].ToString();
                    dtpDate.Value = GlobalFunctions.ConvertToDate(_dr["Date"].ToString());
                    txtReference.Text = _dr["Reference"].ToString();
                    cboSupplier.Text = _dr["Supplier"].ToString();
                    txtTotalQty.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Qty-IN"].ToString()));
                    txtTotalAmount.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Amount"].ToString()));
                    txtRemarks.Text = _dr["Remarks"].ToString();
                    foreach (DataRow _drDetails in loInventoryDetail.getInventoryDetail(lInventoryId).Rows)
                    {
                        int i = dgvDetail.Rows.Add();
                        dgvDetail.Rows[i].Cells[0].Value = _drDetails["Id"].ToString();
                        dgvDetail.Rows[i].Cells[1].Value = _drDetails["Stock Id"].ToString();
                        dgvDetail.Rows[i].Cells[2].Value = _drDetails["Stock Description"].ToString();
                        dgvDetail.Rows[i].Cells[3].Value = _drDetails["Unit"].ToString();
                        dgvDetail.Rows[i].Cells[4].Value = _drDetails["LocationId"].ToString();
                        dgvDetail.Rows[i].Cells[5].Value = _drDetails["Location"].ToString();
                        dgvDetail.Rows[i].Cells[6].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Qty-IN"].ToString()));
                        dgvDetail.Rows[i].Cells[7].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Balance"].ToString()));
                        dgvDetail.Rows[i].Cells[8].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Unit Price"].ToString()));
                        dgvDetail.Rows[i].Cells[9].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Total Price"].ToString()));
                        dgvDetail.Rows[i].Cells[10].Value = _drDetails["Remarks"].ToString();
                        dgvDetail.Rows[i].Cells[11].Value = "Saved";
                    }
                    computeTotalAmount();
                }
            }
            else
            {
                foreach (DataRow _dr in loInventoryHeader.getNextInventoryHeaderNo().Rows)
                {
                    txtStockReceivingNo.Text = _dr[0].ToString();
                }
                cboSupplier.Focus();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDetail.CurrentRow.Cells[11].Value.ToString() == "Saved" || dgvDetail.CurrentRow.Cells[11].Value.ToString() == "Edit")
            {
                dgvDetail.CurrentRow.Cells[11].Value = "Delete";
                dgvDetail.CurrentRow.Visible = false;
            }
            else if (dgvDetail.CurrentRow.Cells[11].Value.ToString() == "Add")
            {
                if (this.dgvDetail.SelectedRows.Count > 0)
                {
                    dgvDetail.Rows.RemoveAt(this.dgvDetail.SelectedRows[0].Index);
                }
            }
            computeTotalAmount();
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                if (dgvDetail.Rows[i].Cells[11].Value.ToString() == "Saved" || dgvDetail.Rows[i].Cells[11].Value.ToString() == "Edit")
                {
                    dgvDetail.Rows[i].Cells[11].Value = "Delete";
                    dgvDetail.Rows[i].Visible = false;
                }
                else if (dgvDetail.Rows[i].Cells[11].Value.ToString() == "Add")
                {
                    dgvDetail.Rows.RemoveAt(this.dgvDetail.Rows[i].Index);
                }
            }
            computeTotalAmount();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (decimal.Parse(txtTotalQty.Text) == 0)
            {
                MessageBoxUI _mb1 = new MessageBoxUI("Totals of Qty-IN must not be Zero(0)!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb1.showDialog();
                return;
            }

            DialogResult _dr = new DialogResult();
            MessageBoxUI _mb = new MessageBoxUI("Continue saving this Stock Receiving?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
            _mb.ShowDialog();
            _dr = _mb.Operation;
            if (_dr == DialogResult.Yes)
            {
                
                loInventoryHeader.HeaderId = lInventoryId;
                loInventoryHeader.Date = dtpDate.Value;
                loInventoryHeader.Type = "Stock Receiving";
                loInventoryHeader.Reference = GlobalFunctions.replaceChar(txtReference.Text);
                loInventoryHeader.CustomerId = "";
                try
                {
                    loInventoryHeader.SupplierId = cboSupplier.SelectedValue.ToString();
                }
                catch
                {
                    MessageBoxUI _mb0 = new MessageBoxUI("You must select a correct Supplier!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                    _mb0.showDialog();
                    return;
                }
                loInventoryHeader.TotalIN = decimal.Parse(txtTotalQty.Text);
                loInventoryHeader.TotalOUT = 0;
                loInventoryHeader.TotalAmount = decimal.Parse(txtTotalAmount.Text);
                loInventoryHeader.Remarks = GlobalFunctions.replaceChar(txtRemarks.Text);

                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
                try
                {
                    string _InventoryId = loInventoryHeader.save(lOperation, ref _Trans);
                    if (_InventoryId != "")
                    {
                        for (int i = 0; i < dgvDetail.Rows.Count; i++)
                        {
                            if (dgvDetail.Rows[i].Cells[11].Value.ToString() == "Add")
                            {
                                try
                                {
                                    loInventoryDetail.DetailId = dgvDetail.Rows[i].Cells[0].Value.ToString();
                                }
                                catch
                                {
                                    loInventoryDetail.DetailId = "";
                                }
                                try
                                {
                                    loInventoryDetail.HeaderId = _InventoryId;
                                    loInventoryDetail.StockId = dgvDetail.Rows[i].Cells[1].Value.ToString();
                                    loInventoryDetail.StockDescription = dgvDetail.Rows[i].Cells[2].Value.ToString();
                                    loInventoryDetail.Unit = dgvDetail.Rows[i].Cells[3].Value.ToString();
                                    loInventoryDetail.LocationId = dgvDetail.Rows[i].Cells[4].Value.ToString();
                                    loInventoryDetail.IN = decimal.Parse(dgvDetail.Rows[i].Cells[6].Value.ToString());
                                    loInventoryDetail.OUT = 0;
                                    loInventoryDetail.Balance = decimal.Parse(dgvDetail.Rows[i].Cells[7].Value.ToString());
                                    loInventoryDetail.UnitPrice = decimal.Parse(dgvDetail.Rows[i].Cells[8].Value.ToString());
                                    loInventoryDetail.TotalPrice = decimal.Parse(dgvDetail.Rows[i].Cells[9].Value.ToString());
                                    loInventoryDetail.Remarks = dgvDetail.Rows[i].Cells[10].Value.ToString();
                                    loInventoryDetail.save(GlobalVariables.Operation.Add, ref _Trans);
                                }
                                catch { }
                            }
                            else if (dgvDetail.Rows[i].Cells[11].Value.ToString() == "Edit")
                            {
                                try
                                {
                                    loInventoryDetail.DetailId = dgvDetail.Rows[i].Cells[0].Value.ToString();
                                }
                                catch
                                {
                                    loInventoryDetail.DetailId = "";
                                }
                                loInventoryDetail.HeaderId = _InventoryId;
                                loInventoryDetail.StockId = dgvDetail.Rows[i].Cells[1].Value.ToString();
                                loInventoryDetail.StockDescription = dgvDetail.Rows[i].Cells[2].Value.ToString();
                                loInventoryDetail.Unit = dgvDetail.Rows[i].Cells[3].Value.ToString();
                                loInventoryDetail.LocationId = dgvDetail.Rows[i].Cells[4].Value.ToString();
                                loInventoryDetail.IN = decimal.Parse(dgvDetail.Rows[i].Cells[6].Value.ToString());
                                loInventoryDetail.OUT = 0;
                                loInventoryDetail.Balance = decimal.Parse(dgvDetail.Rows[i].Cells[7].Value.ToString());
                                loInventoryDetail.UnitPrice = decimal.Parse(dgvDetail.Rows[i].Cells[8].Value.ToString());
                                loInventoryDetail.TotalPrice = decimal.Parse(dgvDetail.Rows[i].Cells[9].Value.ToString());
                                loInventoryDetail.Remarks = dgvDetail.Rows[i].Cells[10].Value.ToString();
                                loInventoryDetail.save(GlobalVariables.Operation.Edit, ref _Trans);
                                
                            }
                            else if (dgvDetail.Rows[i].Cells[11].Value.ToString() == "Delete")
                            {
                                loInventoryDetail.remove(dgvDetail.Rows[i].Cells[0].Value.ToString(), ref _Trans);
                            }
                        }
                        _Trans.Commit();
                        if (txtStockReceivingNo.Text == _InventoryId)
                        {
                            MessageBoxUI _mb2 = new MessageBoxUI("Stock Receiving has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                            _mb2.showDialog();
                        }
                        else
                        {
                            MessageBoxUI _mb2 = new MessageBoxUI("Stock Receiving has been saved successfully! New Receiving No. : " + _InventoryId, GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                            _mb2.showDialog();
                        }

                        //previewDetail(_InventoryId);

                        object[] _params = { "ViewAll", "", "", "Stock Receiving" };
                        ParentList.GetType().GetMethod("refresh").Invoke(ParentList, _params);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Unclosed quotation mark after the character string"))
                    {
                        MessageBoxUI _mb3 = new MessageBoxUI("Do not use this character( ' ).", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                        _mb3.showDialog();
                        return;
                    }
                    else
                    {
                        MessageBoxUI _mb3 = new MessageBoxUI(ex.Message, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                        _mb3.showDialog();
                        return;
                    }
                }
            }
        }

        private void previewDetail(string pId)
        {
            try
            {
                foreach (DataRow _dr in loInventoryHeader.getAllData("ViewAll",pId,"","Stock Receiving").Rows)
                {
                    loStockReceivingDetailRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                    loStockReceivingDetailRpt.Database.Tables[1].SetDataSource(loInventoryDetail.getAllData(_dr[0].ToString()));
                    loStockReceivingDetailRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                    loStockReceivingDetailRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                    loStockReceivingDetailRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                    loStockReceivingDetailRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                    loStockReceivingDetailRpt.SetParameterValue("Title", "Stock Receiving Details");
                    loStockReceivingDetailRpt.SetParameterValue("SubTitle", "Stock Receiving Details");
                    loStockReceivingDetailRpt.SetParameterValue("Id", _dr["Id"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Date", _dr["Date"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Explanation", _dr["Explanation"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("ReceivedBy", _dr["Received By"].ToString());
                    loStockReceivingDetailRpt.SetParameterValue("Remarks", _dr["Remarks"].ToString());
                    loReportViewer.crystalReportViewer.ReportSource = loStockReceivingDetailRpt;
                    loReportViewer.ShowDialog();
                }
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            StockReceivingItemDetailUI loStockReceivingItemDetail = new StockReceivingItemDetailUI();
            loStockReceivingItemDetail.ParentList = this;
            loStockReceivingItemDetail.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            StockReceivingItemDetailUI loStockReceivingItemDetail = new StockReceivingItemDetailUI(dgvDetail.CurrentRow.Cells[0].Value.ToString(),
                dgvDetail.CurrentRow.Cells[1].Value.ToString(),
                dgvDetail.CurrentRow.Cells[2].Value.ToString(),
                dgvDetail.CurrentRow.Cells[3].Value.ToString(),
                dgvDetail.CurrentRow.Cells[4].Value.ToString(),
                decimal.Parse(dgvDetail.CurrentRow.Cells[6].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[7].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[8].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[9].Value.ToString()),
                dgvDetail.CurrentRow.Cells[10].Value.ToString());
            loStockReceivingItemDetail.ParentList = this;
            loStockReceivingItemDetail.ShowDialog();
        }

        private void dgvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit_Click(null, new EventArgs());
        }

        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow _dr in loSupplier.getSupplier(cboSupplier.SelectedValue.ToString()).Rows)
                {
                    txtAddress.Text = _dr["Address"].ToString();
                }
            }
            catch 
            { }
        }
    }
}
