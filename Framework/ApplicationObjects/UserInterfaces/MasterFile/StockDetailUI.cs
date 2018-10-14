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

namespace NSites.ApplicationObjects.UserInterfaces.MasterFile
{
    public partial class StockDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[8];
        GlobalVariables.Operation lOperation;
        Stock loStock;
        Category loCategory;
        Unit loUnit;
        string lId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public StockDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loStock = new Stock();
            loCategory = new Category();
            loUnit = new Unit();
        }

        public StockDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loStock = new Stock();
            loCategory = new Category();
            loUnit = new Unit();
            lRecords = pRecords;
        }
        #endregion "END OF VARIABLES"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        private void clear()
        {
            lId = "";
            txtDescription.Clear();
            cboCategory.SelectedIndex = -1;
            cboUnit.SelectedIndex = -1;
            txtUnitCost.Text = "0.00";
            txtUnitPrice.Text = "0.00";
            txtReorderLevel.Text = "0.00";
            txtRemarks.Clear();
            txtDescription.Focus();
        }
        #endregion "END OF METHODS"

        private void StockDetailUI_Load(object sender, EventArgs e)
        {
            cboCategory.DataSource = loCategory.getAllData("ViewAll","");
            cboCategory.ValueMember = "Id";
            cboCategory.DisplayMember = "Description";
            cboCategory.SelectedIndex = -1;

            cboUnit.DataSource = loUnit.getAllData("ViewAll", "");
            cboUnit.ValueMember = "Id";
            cboUnit.DisplayMember = "Description";
            cboUnit.SelectedIndex = -1;
            
            if (lOperation == GlobalVariables.Operation.Edit)
            {
                lId = lRecords[0];
                txtDescription.Text = lRecords[1];
                cboCategory.Text = lRecords[2];
                cboUnit.Text = lRecords[3];
                txtUnitCost.Text = lRecords[4];
                txtUnitPrice.Text = lRecords[5];
                txtReorderLevel.Text = string.Format("{0:n}", decimal.Parse(lRecords[6]));
                txtRemarks.Text = lRecords[7];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loStock.Id = lId;
            loStock.Description = GlobalFunctions.replaceChar(txtDescription.Text);
            try
            {
                loStock.CategoryId = cboCategory.SelectedValue.ToString();
            }
            catch
            {
                loStock.CategoryId = "";
                /*
                MessageBoxUI _mb = new MessageBoxUI("You must select a correct Category!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.showDialog();
                cboCategory.Focus();
                return;
                */
            }
            try
            {
                loStock.UnitId = cboUnit.SelectedValue.ToString();
            }
            catch
            {
                loStock.UnitId = "";
                /*
                MessageBoxUI _mb = new MessageBoxUI("You must select a correct Unit!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.showDialog();
                cboUnit.Focus();
                return;
                */
            }
            loStock.UnitCost = decimal.Parse(txtUnitCost.Text);
            loStock.UnitPrice = decimal.Parse(txtUnitPrice.Text);
            loStock.ReorderLevel = decimal.Parse(txtReorderLevel.Text);
            loStock.Remarks = GlobalFunctions.replaceChar(txtRemarks.Text);

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _StockId = loStock.save(lOperation, ref _Trans);
                if (_StockId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Stock has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _StockId;
                    lRecords[1] = txtDescription.Text;
                    lRecords[2] = cboCategory.Text;
                    lRecords[3] = cboUnit.Text;
                    lRecords[4] = decimal.Parse(txtUnitCost.Text).ToString();
                    lRecords[5] = decimal.Parse(txtUnitPrice.Text).ToString();
                    lRecords[6] = decimal.Parse(txtReorderLevel.Text).ToString();
                    lRecords[7] = txtRemarks.Text;

                    object[] _params = { lRecords };
                    if (lOperation == GlobalVariables.Operation.Edit)
                    {
                        try
                        {
                            ParentList.GetType().GetMethod("updateData").Invoke(ParentList, _params);
                        }
                        catch { }
                        this.Close();
                    }
                    else
                    {
                        ParentList.GetType().GetMethod("addData").Invoke(ParentList, _params);
                        clear();
                        //this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _Trans.Rollback();
                if (ex.Message.Contains("Duplicate"))
                {
                    MessageBoxUI _mb = new MessageBoxUI("Stock Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void txtReorderLevel_Leave(object sender, EventArgs e)
        {
            try
            {
                txtReorderLevel.Text = string.Format("{0:n}", decimal.Parse(txtReorderLevel.Text));
            }
            catch
            {
                txtReorderLevel.Text = "0.00";
            }
        }

        private void txtUnitCost_Leave(object sender, EventArgs e)
        {
            try
            {
                txtUnitCost.Text = string.Format("{0:n}", decimal.Parse(txtUnitCost.Text));
            }
            catch
            {
                txtUnitCost.Text = "0.00";
            }
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
    }
}
