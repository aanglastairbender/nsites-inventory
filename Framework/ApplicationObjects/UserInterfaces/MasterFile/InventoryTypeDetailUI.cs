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
    public partial class InventoryTypeDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[5];
        GlobalVariables.Operation lOperation;
        InventoryType loInventoryType;
        string lId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryTypeDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loInventoryType = new InventoryType();
        }

        public InventoryTypeDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loInventoryType = new InventoryType();
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
            cboQty.SelectedIndex = -1;
            cboSource.SelectedIndex = -1;
            txtRemarks.Clear();
            txtDescription.Focus();
        }
        #endregion "END OF METHODS"

        private void btnSave_Click(object sender, EventArgs e)
        {
            loInventoryType.Id = lId;
            loInventoryType.Description = GlobalFunctions.replaceChar(txtDescription.Text);
            if (cboQty.Text != "")
            {
                loInventoryType.Qty = cboQty.Text;
            }
            else
            {
                MessageBoxUI _mb = new MessageBoxUI("You must select a Qty!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.showDialog();
                cboQty.Focus();
                return;
            }
            if (cboSource.Text != "")
            {
                loInventoryType.Source = cboSource.Text;
            }
            else
            {
                MessageBoxUI _mb = new MessageBoxUI("You must select a Source!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.showDialog();
                cboSource.Focus();
                return;
            }
            loInventoryType.Remarks = GlobalFunctions.replaceChar(txtRemarks.Text);

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _InventoryTypeId = loInventoryType.save(lOperation, ref _Trans);
                if (_InventoryTypeId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Inventory Type has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _InventoryTypeId;
                    lRecords[1] = txtDescription.Text;
                    lRecords[2] = cboQty.Text;
                    lRecords[3] = cboSource.Text;
                    lRecords[4] = txtRemarks.Text;

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
                    MessageBoxUI _mb = new MessageBoxUI("Inventory Type Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void InventoryTypeDetailUI_Load(object sender, EventArgs e)
        {
            if (lOperation == GlobalVariables.Operation.Edit)
            {
                lId = lRecords[0];
                txtDescription.Text = lRecords[1];
                cboQty.Text = lRecords[2];
                cboSource.Text = lRecords[3];
                txtRemarks.Text = lRecords[4];
            }
            else
            {
                cboQty.SelectedIndex = -1;
                cboSource.SelectedIndex = -1;
            }
        }
    }
}
