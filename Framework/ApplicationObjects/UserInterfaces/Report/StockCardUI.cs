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
using NSites.ApplicationObjects.UserInterfaces.Report.ReportRpt;
using NSites.ApplicationObjects.UserInterfaces.Report;

namespace NSites.ApplicationObjects.UserInterfaces.Report
{
    public partial class StockCardUI : Form
    {
        #region "VARIABLES"
        Stock loStock;
        InventoryDetail loInventoryDetail;
        Location loLocation;
        DataTable ldtItem;
        DataTable ldtReports;
        StockCardRpt loStockCardRpt;
        ReportViewerUI loReportViewer;
        #endregion "END OF VARIABLES"

        public StockCardUI()
        {
            InitializeComponent();
            loStock = new Stock();
            loInventoryDetail = new InventoryDetail();
            loLocation = new Classes.Location();
            ldtItem = new DataTable();
            ldtReports = new DataTable();
            loStockCardRpt = new StockCardRpt();
            loReportViewer = new ReportViewerUI();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void StockCardUI_Load(object sender, EventArgs e)
        {
            cboLocation.DataSource = loLocation.getAllData("ViewAll", "");
            cboLocation.DisplayMember = "Description";
            cboLocation.ValueMember = "Id";
            cboLocation.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void viewAllRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GlobalFunctions.refreshAll(ref dgvReport, ldtReports);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmSalesReport", "Preview"))
            {
                return;
            }
            try
            {
                loStockCardRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loStockCardRpt.Database.Tables[1].SetDataSource(ldtReports);
                loStockCardRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loStockCardRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loStockCardRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loStockCardRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loStockCardRpt.SetParameterValue("Title", "Stock Card");
                loStockCardRpt.SetParameterValue("SubTitle", "Stock Card");
                loStockCardRpt.SetParameterValue("ItemDescription", dgvStockList.CurrentRow.Cells[1].Value.ToString());
                loStockCardRpt.SetParameterValue("Unit", dgvStockList.CurrentRow.Cells[4].Value.ToString());
                loStockCardRpt.SetParameterValue("QtyOnHand",string.Format("{0:n}", decimal.Parse(dgvStockList.CurrentRow.Cells[5].Value.ToString())));
                loReportViewer.crystalReportViewer.ReportSource = loStockCardRpt;
                loReportViewer.ShowDialog();
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockInventory", "Refresh"))
            {
                return;
            }
            try
            {
                ldtItem = loInventoryDetail.getStockInventoryList(cboLocation.SelectedValue.ToString(), "");
                GlobalFunctions.refreshGrid(ref dgvStockList, ldtItem);
                //getDetails();
            }
            catch { }
        }

        private void dgvItemList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvStockList.Columns[e.ColumnIndex].Name == "Stock Id")
                {
                    if (e.Value != null)
                    {
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                else if (this.dgvStockList.Columns[e.ColumnIndex].Name == "Qty on Hand")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch { }
        }

        private void dgvItemList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvStockList.PointToScreen(e.Location);
                cmsFunctionsItem.Show(pt);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvStockList, ldtItem);
        }

        private void dgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getDetails();
        }

        private void getDetails()
        {
            decimal _qtyTotalIn = 0;
            decimal _qtyTotalOut = 0;
            decimal _qtyBegBal = 0;

            foreach (DataRow _dr in loStock.getStockCardBegBal(dtpFromDate.Value, dgvStockList.CurrentRow.Cells[0].Value.ToString()).Rows)
            {
                _qtyTotalIn += decimal.Parse(_dr[0].ToString());
                _qtyTotalOut += decimal.Parse(_dr[1].ToString());
                _qtyBegBal += decimal.Parse(_dr[2].ToString());
            }

            loStockCardRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
            loStockCardRpt.Database.Tables[1].SetDataSource(loStock.getStockCard(dtpFromDate.Value, dtpToDate.Value, dgvStockList.CurrentRow.Cells[0].Value.ToString()));
            loStockCardRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
            loStockCardRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
            loStockCardRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
            loStockCardRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
            loStockCardRpt.SetParameterValue("DateFrom", string.Format("{0:MM-dd-yyyy}", dtpFromDate.Value));
            loStockCardRpt.SetParameterValue("DateTo", string.Format("{0:MM-dd-yyyy}", dtpToDate.Value));
            loStockCardRpt.SetParameterValue("Stock", dgvStockList.CurrentRow.Cells[1].Value.ToString());
            loStockCardRpt.SetParameterValue("Location", cboLocation.Text);
            //loStockCardRpt.SetParameterValue("BegBalQty", string.Format("{0:n}", _qtyBegBal));
            //loStockCardRpt.SetParameterValue("TotalQtyIn", string.Format("{0:n}", _qtyTotalIn));
            //loStockCardRpt.SetParameterValue("TotalQtyOut", string.Format("{0:n}", _qtyTotalOut));
            loStockCardRpt.SetParameterValue("Title", "Stock Card");
            loStockCardRpt.SetParameterValue("SubTitle", "Stock Card");
            crystalReportViewer.ReportSource = loStockCardRpt;
        }

        private void txtSearchStock_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ldtItem = loInventoryDetail.getStockInventoryList(cboLocation.SelectedValue.ToString(), txtSearchStock.Text);
                GlobalFunctions.refreshGrid(ref dgvStockList, ldtItem);
                //getDetails();
            }
            catch { }
        }
    }
}
