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
    public partial class StockDetailUI : Form
    {
        Stock loItem;
        string[] lRecordData = new string[7];

        string lDetailId;
        string lItemId;
        string lItemDescription;
        string lUnit;
        decimal lQty;
        string lRemarks;
        string lOperator;
        
        public StockDetailUI()
        {
            InitializeComponent();
            loItem = new Stock();
            lDetailId = "";
            lOperator = "Add";
        }

        public StockDetailUI(string pDetailId, string pItemId, string pItemDescription,string pUnit, decimal pQty, string pRemarks)
        {
            InitializeComponent();
            loItem = new Stock();
            lDetailId = pDetailId;
            lItemId = pItemId;
            lItemDescription = pItemDescription;
            lUnit = pUnit;
            lQty = pQty;
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
            cboStock.SelectedIndex = -1;
            cboStock.Text = "";
            txtUnit.Clear();
            txtQty.Text = "0.00";
            txtRemarks.Clear();
            cboStock.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (decimal.Parse(txtQty.Text) == 0)
            {
                MessageBoxUI _mbStatus = new MessageBoxUI("Qty must not be Zero(0)!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                return;
            }

            lRecordData[0] = lDetailId;
            try
            {
                lRecordData[1] = cboStock.SelectedValue.ToString();
            }
            catch
            {
                MessageBoxUI _mbStatus = new MessageBoxUI("You must select a correct Item!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                cboStock.Focus();
                return;
            }
            lRecordData[2] = cboStock.Text;
            lRecordData[3] = txtUnit.Text;
            lRecordData[4] = string.Format("{0:n}", decimal.Parse(txtQty.Text));
            lRecordData[5] = GlobalFunctions.replaceChar(txtRemarks.Text);

            object[] _params = { lRecordData };
            if (lOperator == "Add")
            {
                lRecordData[6] = "Add";
                ParentList.GetType().GetMethod("addData").Invoke(ParentList, _params);
                MessageBoxUI _mbStatus = new MessageBoxUI("Successfully added!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                _mbStatus.ShowDialog();
                clear();
            }
            else if (lOperator == "Edit")
            {
                lRecordData[6] = "Edit";
                ParentList.GetType().GetMethod("updateData").Invoke(ParentList, _params);
                Close();
            }
        }

        private void txtQty_Leave(object sender, EventArgs e)
        {
            try
            {
                txtQty.Text = string.Format("{0:n}", decimal.Parse(txtQty.Text));
            }
            catch
            {
                txtQty.Text = "0.00";
            }
        }

        private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow _dr in loItem.getStock(cboStock.SelectedValue.ToString()).Rows)
                {
                    txtUnit.Text = _dr["Unit"].ToString();
                }
            }
            catch 
            {
                txtUnit.Clear();
            }
        }

        private void StockDetailUI_Load(object sender, EventArgs e)
        {
            cboStock.DataSource = loItem.getAllData("ViewAll", "");
            cboStock.DisplayMember = "Item Description";
            cboStock.ValueMember = "Id";
            cboStock.SelectedIndex = -1;

            if (lOperator == "Edit")
            {
                cboStock.TabStop = false;
                //btnGetStocks.TabStop = false;
                cboStock.Text = lItemDescription;
                txtUnit.Text = lUnit;
                txtQty.Text = string.Format("{0:n}", lQty);
                txtRemarks.Text = lRemarks;
            }
            else
            {
                txtUnit.Clear();
                txtQty.Text = "0.00";
                txtRemarks.Clear();
            }
        }

        private void btnGetStocks_Click(object sender, EventArgs e)
        {

        }
    }
}
