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
using NSites.ApplicationObjects.UserInterfaces.Report.TransactionRpt;
using NSites.ApplicationObjects.UserInterfaces.Report;

namespace NSites.ApplicationObjects.UserInterfaces.Transaction.Details
{
    public partial class StockAdjustmentDetailUI : Form
    {
        #region "VARIABLES"
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;
        Supplier loSupplier;
        Customer loCustomer;
        Stock loStock;
        InventoryType loInventoryType;
        GlobalVariables.Operation lOperation;
        StockReceivingDetailRpt loStockReceivingDetailRpt;
        ReportViewerUI loReportViewer;
        string lInventoryId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public StockAdjustmentDetailUI()
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();
            loSupplier = new Supplier();
            loCustomer = new Customer();
            loStock = new Stock();
            loInventoryType = new InventoryType();
            lOperation = GlobalVariables.Operation.Add;
            loStockReceivingDetailRpt = new StockReceivingDetailRpt();
            loReportViewer = new ReportViewerUI();
            lInventoryId = "";
        }
        public StockAdjustmentDetailUI(string pInventoryId)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();
            loSupplier = new Supplier();
            loCustomer = new Customer();
            loStock = new Stock();
            loInventoryType = new InventoryType();
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
            string _operator = dgvDetail.CurrentRow.Cells[12].Value.ToString();
            for (int i = 0; i < pRecordData.Length; i++)
            {
                dgvDetail.CurrentRow.Cells[i].Value = pRecordData[i];
            }
            if (_operator == "Add")
            {
                dgvDetail.CurrentRow.Cells[12].Value = "Add";
            }
            computeTotalAmount();
        }

        private void computeTotalAmount()
        {
            decimal _TotalQtyIN = 0;
            decimal _TotalQtyOUT = 0;
            decimal _TotalAmount = 0;

            for (int i = 0; i < dgvDetail.Rows.Count; i++)
            {
                if (dgvDetail.Rows[i].Visible == true)
                {
                    try
                    {
                        _TotalQtyIN += decimal.Parse(dgvDetail.Rows[i].Cells[6].Value.ToString());
                        _TotalQtyOUT += decimal.Parse(dgvDetail.Rows[i].Cells[7].Value.ToString());
                        _TotalAmount += decimal.Parse(dgvDetail.Rows[i].Cells[10].Value.ToString());
                    }
                    catch { }
                }
            }
            txtTotalQtyIN.Text = string.Format("{0:n}", _TotalQtyIN);
            txtTotalQtyOUT.Text = string.Format("{0:n}", _TotalQtyOUT);
            txtTotalAmount.Text = string.Format("{0:n}", _TotalAmount);
        }

        private void AdjustmentDetailUI_Load(object sender, EventArgs e)
        {
            cboType.DataSource = loInventoryType.getAllData("ViewAll","");
            cboType.DisplayMember = "Description";
            cboType.ValueMember = "Id";
            cboType.SelectedIndex = -1;

            cboSupplier.DataSource = loSupplier.getAllData("ViewAll", "");
            cboSupplier.DisplayMember = "Name";
            cboSupplier.ValueMember = "Id";
            cboSupplier.SelectedIndex = -1;

            cboCustomer.DataSource = loCustomer.getAllData("ViewAll", "");
            cboCustomer.DisplayMember = "Name";
            cboCustomer.ValueMember = "Id";
            cboCustomer.SelectedIndex = -1;

            if (lOperation == GlobalVariables.Operation.Edit)
            {
                foreach (DataRow _dr in loInventoryHeader.getAllData("", lInventoryId, "", "Stock Adjustment").Rows)
                {
                    txtStockAdjustmentNo.Text = _dr["Id"].ToString();
                    dtpDate.Value = GlobalFunctions.ConvertToDate(_dr["Date"].ToString());
                    cboType.Text = _dr["Type"].ToString();
                    txtReference.Text = _dr["Reference"].ToString();
                    cboSupplier.Text = _dr["Supplier"].ToString();
                    cboCustomer.Text = _dr["Customer"].ToString();
                    txtTotalQtyIN.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Qty-IN"].ToString()));
                    txtTotalQtyOUT.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Qty-OUT"].ToString()));
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
                        dgvDetail.Rows[i].Cells[7].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Qty-OUT"].ToString()));
                        dgvDetail.Rows[i].Cells[8].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Balance"].ToString()));
                        dgvDetail.Rows[i].Cells[9].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Unit Price"].ToString()));
                        dgvDetail.Rows[i].Cells[10].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Total Price"].ToString()));
                        dgvDetail.Rows[i].Cells[11].Value = _drDetails["Remarks"].ToString();
                        dgvDetail.Rows[i].Cells[12].Value = "Saved";
                    }
                    computeTotalAmount();
                }
            }
            else
            {
                foreach (DataRow _dr in loInventoryHeader.getNextInventoryHeaderNo().Rows)
                {
                    txtStockAdjustmentNo.Text = _dr[0].ToString();
                }
                cboSupplier.Focus();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string _Qty = "";
            foreach (DataRow _dr in loInventoryType.getInventoryType(cboType.SelectedValue.ToString()).Rows)
            {
                _Qty = _dr["Qty"].ToString();
            }
            StockAdjustmentItemDetailUI loStockAdjustmentItemDetail = new StockAdjustmentItemDetailUI(_Qty);
            loStockAdjustmentItemDetail.ParentList = this;
            loStockAdjustmentItemDetail.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string _Qty = "";
            foreach (DataRow _dr in loInventoryType.getInventoryType(cboType.SelectedValue.ToString()).Rows)
            {
                _Qty = _dr["Qty"].ToString();
            }
            StockAdjustmentItemDetailUI loStockAdjustmentItemDetail = new StockAdjustmentItemDetailUI(dgvDetail.CurrentRow.Cells[0].Value.ToString(),
                dgvDetail.CurrentRow.Cells[1].Value.ToString(),
                dgvDetail.CurrentRow.Cells[2].Value.ToString(),
                dgvDetail.CurrentRow.Cells[3].Value.ToString(),
                dgvDetail.CurrentRow.Cells[4].Value.ToString(),
                decimal.Parse(dgvDetail.CurrentRow.Cells[6].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[7].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[8].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[9].Value.ToString()),
                decimal.Parse(dgvDetail.CurrentRow.Cells[10].Value.ToString()),
                dgvDetail.CurrentRow.Cells[11].Value.ToString(), _Qty);
            loStockAdjustmentItemDetail.ParentList = this;
            loStockAdjustmentItemDetail.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDetail.CurrentRow.Cells[12].Value.ToString() == "Saved" || dgvDetail.CurrentRow.Cells[12].Value.ToString() == "Edit")
            {
                dgvDetail.CurrentRow.Cells[12].Value = "Delete";
                dgvDetail.CurrentRow.Visible = false;
            }
            else if (dgvDetail.CurrentRow.Cells[12].Value.ToString() == "Add")
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
            dgvDetail.Rows.Clear();
            computeTotalAmount();
        }

        private void dgvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEdit_Click(null, new EventArgs());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (decimal.Parse(txtTotalQtyIN.Text) == 0 && decimal.Parse(txtTotalQtyOUT.Text) == 0)
            {
                MessageBoxUI _mb1 = new MessageBoxUI("Totals of Qty must not be Zero(0)!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb1.showDialog();
                return;
            }
            string _CustomerId = "";
            string _SupplierId = "";
            try
            {
                _CustomerId = cboCustomer.SelectedValue.ToString();
            }
            catch
            {
                _CustomerId = "";
            }
            try
            {
                _SupplierId = cboSupplier.SelectedValue.ToString();
            }
            catch
            {
                _SupplierId = "";
            }
            DialogResult _dr = new DialogResult();
            MessageBoxUI _mb = new MessageBoxUI("Continue saving this Stock Adjustment?", GlobalVariables.Icons.QuestionMark, GlobalVariables.Buttons.YesNo);
            _mb.ShowDialog();
            _dr = _mb.Operation;
            if (_dr == DialogResult.Yes)
            {
                loInventoryHeader.HeaderId = lInventoryId;
                loInventoryHeader.Date = dtpDate.Value;
                loInventoryHeader.Type = cboType.Text;
                loInventoryHeader.Reference = GlobalFunctions.replaceChar(txtReference.Text);
                loInventoryHeader.CustomerId = _CustomerId;
                loInventoryHeader.SupplierId = _SupplierId;
                loInventoryHeader.TotalIN = decimal.Parse(txtTotalQtyIN.Text);
                loInventoryHeader.TotalOUT = decimal.Parse(txtTotalQtyOUT.Text);
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
                            if (dgvDetail.Rows[i].Cells[12].Value.ToString() == "Add")
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
                                    loInventoryDetail.OUT = decimal.Parse(dgvDetail.Rows[i].Cells[7].Value.ToString());
                                    loInventoryDetail.Balance = decimal.Parse(dgvDetail.Rows[i].Cells[8].Value.ToString());
                                    loInventoryDetail.UnitPrice = decimal.Parse(dgvDetail.Rows[i].Cells[9].Value.ToString());
                                    loInventoryDetail.TotalPrice = decimal.Parse(dgvDetail.Rows[i].Cells[10].Value.ToString());
                                    loInventoryDetail.Remarks = dgvDetail.Rows[i].Cells[11].Value.ToString();
                                    loInventoryDetail.save(GlobalVariables.Operation.Add, ref _Trans);
                                }
                                catch { }
                            }
                            else if (dgvDetail.Rows[i].Cells[12].Value.ToString() == "Edit")
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
                                loInventoryDetail.OUT = decimal.Parse(dgvDetail.Rows[i].Cells[7].Value.ToString());
                                loInventoryDetail.Balance = decimal.Parse(dgvDetail.Rows[i].Cells[8].Value.ToString());
                                loInventoryDetail.UnitPrice = decimal.Parse(dgvDetail.Rows[i].Cells[9].Value.ToString());
                                loInventoryDetail.TotalPrice = decimal.Parse(dgvDetail.Rows[i].Cells[10].Value.ToString());
                                loInventoryDetail.Remarks = dgvDetail.Rows[i].Cells[11].Value.ToString();
                                loInventoryDetail.save(GlobalVariables.Operation.Edit, ref _Trans);

                            }
                            else if (dgvDetail.Rows[i].Cells[12].Value.ToString() == "Delete")
                            {
                                loInventoryDetail.remove(dgvDetail.Rows[i].Cells[0].Value.ToString(), ref _Trans);
                            }
                        }
                        _Trans.Commit();
                        if (txtStockAdjustmentNo.Text == _InventoryId)
                        {
                            MessageBoxUI _mb2 = new MessageBoxUI("Stock Adjustment has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                            _mb2.showDialog();
                        }
                        else
                        {
                            MessageBoxUI _mb2 = new MessageBoxUI("Stock Adjustment has been saved successfully! New Receiving No. : " + _InventoryId, GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                            _mb2.showDialog();
                        }

                        //previewDetail(_InventoryId);

                        object[] _params = { "ViewAll", "", "", "Stock Adjustment" };
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

        private void cboSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboCustomer.SelectedIndex = -1;
        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSupplier.SelectedIndex = -1;
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvDetail.Rows.Clear();
            computeTotalAmount();

            try
            {
                cboCustomer.SelectedIndex = -1;
                cboSupplier.SelectedIndex = -1;
                cboCustomer.Enabled = false;
                cboSupplier.Enabled = false;
                foreach (DataRow _dr in loInventoryType.getInventoryType(cboType.SelectedValue.ToString()).Rows)
                {
                    if (_dr["Source"].ToString() == "Customer")
                    {
                        cboSupplier.SelectedIndex = -1;
                        cboSupplier.Enabled = false;
                        cboCustomer.SelectedIndex = -1;
                        cboCustomer.Enabled = true;
                    }
                    else if (_dr["Source"].ToString() == "Supplier")
                    {
                        cboCustomer.SelectedIndex = -1;
                        cboCustomer.Enabled = false;
                        cboSupplier.SelectedIndex = -1;
                        cboSupplier.Enabled = true;
                    }
                    else if (_dr["Source"].ToString() == "Customer & Supplier")
                    {
                        cboCustomer.SelectedIndex = -1;
                        cboSupplier.SelectedIndex = -1;
                        cboCustomer.Enabled = true;
                        cboSupplier.Enabled = true;
                    }
                }
            }
            catch
            { 
                
            }
        }
    }
}
