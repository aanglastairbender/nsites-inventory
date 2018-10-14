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
    public partial class SupplierDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[8];
        GlobalVariables.Operation lOperation;
        Supplier loSupplier;
        string lId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public SupplierDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loSupplier = new Supplier();
        }

        public SupplierDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loSupplier = new Supplier();
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
            txtName.Clear();
            txtAddress.Clear();
            txtContactPerson.Clear();
            txtContactNo.Clear();
            txtTerms.Text = "0";
            txtCreditLimit.Text = "0.00";
            txtRemarks.Clear();
            txtName.Focus();
        }
        #endregion "END OF METHODS"

        private void SupplierDetailUI_Load(object sender, EventArgs e)
        {
            if (lOperation == GlobalVariables.Operation.Edit)
            {
                lId = lRecords[0];
                txtName.Text = lRecords[1];
                txtAddress.Text = lRecords[2];
                txtContactPerson.Text = lRecords[3];
                txtContactNo.Text = lRecords[4];
                txtTerms.Text = string.Format("{0:0}", decimal.Parse(lRecords[5]));
                txtCreditLimit.Text = string.Format("{0:n}", decimal.Parse(lRecords[6]));
                txtRemarks.Text = lRecords[7];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loSupplier.Id = lId;
            loSupplier.Name = GlobalFunctions.replaceChar(txtName.Text);
            loSupplier.Address = GlobalFunctions.replaceChar(txtAddress.Text);
            loSupplier.ContactPerson = GlobalFunctions.replaceChar(txtContactPerson.Text);
            loSupplier.ContactNo = GlobalFunctions.replaceChar(txtContactNo.Text);
            loSupplier.Terms = int.Parse(txtTerms.Text);
            loSupplier.CreditLimit = decimal.Parse(txtCreditLimit.Text);
            loSupplier.Remarks = txtRemarks.Text;

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _SupplierId = loSupplier.save(lOperation, ref _Trans);
                if (_SupplierId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Supplier has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _SupplierId;
                    lRecords[1] = txtName.Text;
                    lRecords[2] = txtAddress.Text;
                    lRecords[3] = txtContactPerson.Text;
                    lRecords[4] = txtContactNo.Text;
                    lRecords[5] = int.Parse(txtTerms.Text).ToString();
                    lRecords[6] = decimal.Parse(txtCreditLimit.Text).ToString();
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
                    MessageBoxUI _mb = new MessageBoxUI("Supplier Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void txtCreditLimit_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCreditLimit.Text = string.Format("{0:n}", decimal.Parse(txtCreditLimit.Text));
            }
            catch
            {
                txtCreditLimit.Text = "0.00";
            }
        }
    }
}
