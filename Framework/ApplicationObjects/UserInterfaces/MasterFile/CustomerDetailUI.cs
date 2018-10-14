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
    public partial class CustomerDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[8];
        GlobalVariables.Operation lOperation;
        Customer loCustomer;
        string lId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public CustomerDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loCustomer = new Customer();
        }

        public CustomerDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loCustomer = new Customer();
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

        private void CustomerDetailUI_Load(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loCustomer.Id = lId;
            loCustomer.Name = GlobalFunctions.replaceChar(txtName.Text);
            loCustomer.Address = GlobalFunctions.replaceChar(txtAddress.Text);
            loCustomer.ContactPerson = GlobalFunctions.replaceChar(txtContactPerson.Text);
            loCustomer.ContactNo = GlobalFunctions.replaceChar(txtContactNo.Text);
            loCustomer.Terms = int.Parse(txtTerms.Text);
            loCustomer.CreditLimit = decimal.Parse(txtCreditLimit.Text);
            loCustomer.Remarks = txtRemarks.Text;

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _CustomerId = loCustomer.save(lOperation, ref _Trans);
                if (_CustomerId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Customer has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _CustomerId;
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
                    MessageBoxUI _mb = new MessageBoxUI("Customer Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
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
