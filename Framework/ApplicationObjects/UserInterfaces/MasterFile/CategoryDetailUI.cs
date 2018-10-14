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
    public partial class CategoryDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[4];
        GlobalVariables.Operation lOperation;
        Category loCategory;
        InventoryGroup loInventoryGroup;
        string lId;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public CategoryDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loCategory = new Category();
            loInventoryGroup = new InventoryGroup();
        }

        public CategoryDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loCategory = new Category();
            loInventoryGroup = new InventoryGroup();
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
            cboInventoryGroup.SelectedIndex = -1;
            txtRemarks.Clear();
            txtDescription.Focus();
        }
        #endregion "END OF METHODS"

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loCategory.Id = lId;
            loCategory.Description = GlobalFunctions.replaceChar(txtDescription.Text);
            try
            {
                loCategory.InventoryGroupId = cboInventoryGroup.SelectedValue.ToString();
            }
            catch
            {
                MessageBoxUI _mb = new MessageBoxUI("You must select a correct Inventory Group!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                _mb.showDialog();
                cboInventoryGroup.Focus();
                return;
            }
            loCategory.Remarks = txtRemarks.Text;

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _ItemCategoryId = loCategory.save(lOperation, ref _Trans);
                if (_ItemCategoryId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Category has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _ItemCategoryId;
                    lRecords[1] = txtDescription.Text;
                    lRecords[2] = cboInventoryGroup.Text;
                    lRecords[3] = txtRemarks.Text;

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
                    MessageBoxUI _mb = new MessageBoxUI("Category Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void ItemCategoryDetailUI_Load(object sender, EventArgs e)
        {
            cboInventoryGroup.DataSource = loInventoryGroup.getAllData("ViewAll","");
            cboInventoryGroup.ValueMember = "Id";
            cboInventoryGroup.DisplayMember = "Description";
            cboInventoryGroup.SelectedIndex = -1;
            
            if (lOperation == GlobalVariables.Operation.Edit)
            {
                lId = lRecords[0];
                txtDescription.Text = lRecords[1];
                cboInventoryGroup.Text = lRecords[2];
                txtRemarks.Text = lRecords[3];
            }
        }
    }
}
