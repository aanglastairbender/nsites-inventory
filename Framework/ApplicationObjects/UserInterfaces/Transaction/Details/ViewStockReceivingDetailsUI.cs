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

namespace NSites.ApplicationObjects.UserInterfaces.Transaction.Details
{
    public partial class ViewStockReceivingDetailsUI : Form
    {
        InventoryHeader loInventoryHeader;
        InventoryDetail loInventoryDetail;
        string lInventoryHeaderId;

        public ViewStockReceivingDetailsUI(string pInventoryHeaderId)
        {
            InitializeComponent();
            loInventoryHeader = new InventoryHeader();
            loInventoryDetail = new InventoryDetail();
            lInventoryHeaderId = pInventoryHeaderId;
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void ViewStockReceivingDetailsUI_Load(object sender, EventArgs e)
        {
            foreach (DataRow _dr in loInventoryHeader.getAllData("", lInventoryHeaderId, "", "Stock Receiving").Rows)
            {
                lblStockReceivingId.Text = _dr[0].ToString();
                lblDate.Text = _dr[1].ToString();
                lblFinal.Text = _dr[2].ToString();
                lblReference.Text = _dr[3].ToString();
                lblSupplier.Text = _dr["Supplier"].ToString();
                lblTotalAmount.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Amount"].ToString()));
                lblTotalQtyIN.Text = string.Format("{0:n}", decimal.Parse(_dr["Total Qty-IN"].ToString()));
                lblRemarks.Text = _dr["Remarks"].ToString();
                foreach (DataRow _drDetails in loInventoryDetail.getAllData(lInventoryHeaderId).Rows)
                {
                    int i = dgvDetail.Rows.Add();
                    dgvDetail.Rows[i].Cells[0].Value = _drDetails["Id"].ToString();
                    dgvDetail.Rows[i].Cells[1].Value = _drDetails["Stock Id"].ToString();
                    dgvDetail.Rows[i].Cells[2].Value = _drDetails["Stock Description"].ToString();
                    dgvDetail.Rows[i].Cells[3].Value = _drDetails["Unit"].ToString();
                    dgvDetail.Rows[i].Cells[4].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Qty-IN"].ToString()));
                    dgvDetail.Rows[i].Cells[5].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Balance"].ToString()));
                    dgvDetail.Rows[i].Cells[6].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Unit Price"].ToString()));
                    dgvDetail.Rows[i].Cells[7].Value = _drDetails["Discount Type"].ToString();
                    dgvDetail.Rows[i].Cells[8].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Discount Value"].ToString()));
                    dgvDetail.Rows[i].Cells[9].Value = string.Format("{0:n}", decimal.Parse(_drDetails["Total Price"].ToString()));
                    dgvDetail.Rows[i].Cells[10].Value = _drDetails["Remarks"].ToString();
                    dgvDetail.Rows[i].Cells[11].Value = "Saved";
                    dgvDetail.Rows[i].Selected = false;
                }
            }
        }
    }
}
