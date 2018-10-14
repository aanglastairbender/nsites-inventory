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

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class UserGroupUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[3];
        GlobalVariables.Operation lOperation;
        UserGroup loUserGroup;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public UserGroupUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loUserGroup = new UserGroup();
            txtId.Text = "AUTO";
        }
        public UserGroupUI(string pUserGroupId, string pUserGroupDescription, string pRemarks)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loUserGroup = new UserGroup();
            txtId.Text = pUserGroupId;
            txtId.ReadOnly = true;
            txtId.BackColor = SystemColors.Info;
            txtId.TabStop = false;
            txtDescription.Text = pUserGroupDescription;
            txtRemarks.Text = pRemarks;
        }
        #endregion "END OF CONSTRUCTORS"

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
            txtId.Clear();
            txtDescription.Clear();
            txtRemarks.Focus();
            txtDescription.Focus();
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void UserGroupUI_Load(object sender, EventArgs e)
        {

        }

        #endregion "END OF EVENTS"

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            loUserGroup.Id = txtId.Text;
            loUserGroup.Description = GlobalFunctions.replaceChar(txtDescription.Text);
            loUserGroup.Remarks = GlobalFunctions.replaceChar(txtRemarks.Text);

            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                string _UserGroupId = loUserGroup.save(lOperation, ref _Trans);
                if (_UserGroupId != "")
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("Usergroup has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    lRecords[0] = _UserGroupId;
                    lRecords[1] = txtDescription.Text;
                    lRecords[2] = txtRemarks.Text;
                    
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
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _Trans.Rollback();
                if (ex.Message.Contains("Duplicate"))
                {
                    MessageBoxUI _mb = new MessageBoxUI("Usergroup Id already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }
    }
}
