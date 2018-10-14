using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NSites.Global;
using NSites.ApplicationObjects.Classes;

namespace NSites.ApplicationObjects.UserInterfaces.Transaction.Details
{
    public partial class StockWithdrawalItemDetailUI : Form
    {
        Stock loStock;
        Location loLocation;
        string[] lRecordData = new string[12];

        string lDetailId;
        string lStockId;
        string lStockDescription;
        string lUnit;
        string lLocationId;
        decimal lQtyOUT;
        decimal lBalance;
        decimal lUnitPrice;
        decimal lTotalPrice;
        string lRemarks;
        string lOperator;
        
        public StockWithdrawalItemDetailUI()
        {
            InitializeComponent();
            loStock = new Stock();
            loLocation = new Location();
            lDetailId = "";
            lOperator = "Add";
        }

        public StockWithdrawalItemDetailUI(string pDetailId, string pStockId, string pStockDescription, string pUnit,
            string pLocationId, decimal pQtyOUT, decimal pBalance, decimal pUnitPrice, decimal pTotalPrice, string pRemarks)
        {
            InitializeComponent();
            loStock = new Stock();
            loLocation = new Location();
            lDetailId = pDetailId;
            lStockId = pStockId;
            lStockDescription = pStockDescription;
            lUnit = pUnit;
            lLocationId = pLocationId;
            lQtyOUT = pQtyOUT;
            lBalance = pBalance;
            lUnitPrice = pUnitPrice;
            lTotalPrice = pTotalPrice;
            lRemarks = pRemarks;
            lOperator = "Edit";
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void clear()
        {
            cboStockDescription.SelectedIndex = -1;
            cboStockDescription.Text = "";
            txtUnit.Clear();
            cboLocation.SelectedIndex = 0;
            txtQtyOUT.Text = "1.00";
            lblRunningBalance.Text = "0.00";
            txtBalance.Text = "0.00";
            txtUnitPrice.Text = "0.00";
            txtTotalPrice.Text = "0.00";
            txtRemarks.Clear();
            cboStockDescription.Focus();
        }

        private void computeTotalPrice()
        {
            try
            {
                txtTotalPrice.Text = string.Format("{0:n}", decimal.Parse(txtQtyOUT.Text) * decimal.Parse(txtUnitPrice.Text));
            }
            catch
            {
                txtTotalPrice.Text = "0.00";
            }
        }

        private void getQtyOnHand()
        {
            try
            {
                foreach (DataRow _dr in loStock.getStockQtyOnHand(cboLocation.SelectedValue.ToString(), cboStockDescription.SelectedValue.ToString()).Rows)
                {
                    lblRunningBalance.Text = string.Format("{0:n}", decimal.Parse(_dr[0].ToString()));
                }
            }
            catch
            {
                lblRunningBalance.Text = "0.00";
            }
            computeTotalQtyOnHand();
        }

        private void computeTotalQtyOnHand()
        {
            txtBalance.Text = string.Format("{0:n}", decimal.Parse(lblRunningBalance.Text) - decimal.Parse(txtQtyOUT.Text));
        }

        private void StockWithdrawalItemDetailUI_Load(object sender, EventArgs e)
        {
            cboStockDescription.DataSource = loStock.getAllData("ViewAll", "");
            cboStockDescription.DisplayMember = "Description";
            cboStockDescription.ValueMember = "Id";
            cboStockDescription.SelectedIndex = -1;

            cboLocation.DataSource = loLocation.getAllData("ViewAll", "");
            cboLocation.DisplayMember = "Description";
            cboLocation.ValueMember = "Id";
            cboLocation.SelectedIndex = 0;

            if (lOperator == "Edit")
            {
                cboStockDescription.SelectedValue = lStockId;
                txtUnit.Text = lUnit;
                cboLocation.SelectedValue = lLocationId;
                txtQtyOUT.Text = string.Format("{0:n}", lQtyOUT);
                txtBalance.Text = string.Format("{0:n}", lBalance);
                txtUnitPrice.Text = string.Format("{0:n}", lUnitPrice);
                txtTotalPrice.Text = string.Format("{0:n}", lTotalPrice);
                txtRemarks.Text = lRemarks;
            }
            else
            {
                lblRunningBalance.Text = "0.00";
                txtBalance.Text = "0.00";
            }
        }

        private void cboStockDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow _dr in loStock.getStock(cboStockDescription.SelectedValue.ToString()).Rows)
                {
                    txtUnit.Text = _dr["Unit"].ToString();
                    txtUnitPrice.Text = string.Format("{0:n}", decimal.Parse(_dr["Unit Price"].ToString()));
                    computeTotalPrice();
                    getQtyOnHand();
                }
            }
            catch
            { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (decimal.Parse(txtQtyOUT.Text) == 0)
            {
                MessageBoxUI _mbStatus = new MessageBoxUI("Qty-OUT must not be Zero(0)!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                return;
            }

            lRecordData[0] = lDetailId;
            try
            {
                lRecordData[1] = cboStockDescription.SelectedValue.ToString();
            }
            catch
            {
                MessageBoxUI _mbStatus = new MessageBoxUI("You must select a correct Stock!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                cboStockDescription.Focus();
                return;
            }
            lRecordData[2] = cboStockDescription.Text;
            lRecordData[3] = txtUnit.Text;
            lRecordData[4] = cboLocation.SelectedValue.ToString();
            lRecordData[5] = cboLocation.Text;
            lRecordData[6] = string.Format("{0:n}", decimal.Parse(txtQtyOUT.Text));
            lRecordData[7] = string.Format("{0:n}", decimal.Parse(txtBalance.Text));
            lRecordData[8] = string.Format("{0:n}", decimal.Parse(txtUnitPrice.Text));
            lRecordData[9] = string.Format("{0:n}", decimal.Parse(txtTotalPrice.Text));
            lRecordData[10] = GlobalFunctions.replaceChar(txtRemarks.Text);

            object[] _params = { lRecordData };
            if (lOperator == "Add")
            {
                lRecordData[11] = "Add";
                ParentList.GetType().GetMethod("addData").Invoke(ParentList, _params);
                MessageBoxUI _mbStatus = new MessageBoxUI("Successfully added!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                clear();
            }
            else if (lOperator == "Edit")
            {
                lRecordData[11] = "Edit";
                ParentList.GetType().GetMethod("updateData").Invoke(ParentList, _params);
                Close();
            }
        }

        private void txtQtyOUT_Leave(object sender, EventArgs e)
        {
            try
            {
                txtQtyOUT.Text = string.Format("{0:n}", decimal.Parse(txtQtyOUT.Text));
            }
            catch
            {
                txtQtyOUT.Text = "0.00";
            }
        }

        private void txtQtyOUT_TextChanged(object sender, EventArgs e)
        {
            computeTotalPrice();
            computeTotalQtyOnHand();
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            computeTotalPrice();
        }

        private void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            try
            {
                txtUnitPrice.Text = string.Format("{0:n}", decimal.Parse(txtUnitPrice.Text));
            }
            catch
            {
                txtUnitPrice.Text = "0.00";
            }
        }

        private void cboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            getQtyOnHand();
            computeTotalQtyOnHand();
        }
    }
}
