using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.Classes;
using NSites.ApplicationObjects.UserInterfaces.MasterFile;

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class UserDetailUI : Form
    {
        #region "VARIABLES"
        string[] lRecords = new string[5];
        GlobalVariables.Operation lOperation;
        User loUser;
        UserGroup loUserGroup;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public UserDetailUI()
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Add;
            loUser = new User();
            loUserGroup = new UserGroup();
        }

        public UserDetailUI(string[] pRecords)
        {
            InitializeComponent();
            lOperation = GlobalVariables.Operation.Edit;
            loUser = new User();
            loUserGroup = new UserGroup();
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
            txtUsername.Clear();
            txtFullname.Clear();
            txtPosition.Clear();
            cboUserGroup.SelectedIndex = -1;
            txtRemarks.Clear();
            txtUsername.Focus();
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void UserUI_Load(object sender, EventArgs e)
        {
            cboUserGroup.DataSource = loUserGroup.getAllData("ViewAll","");
            cboUserGroup.DisplayMember = "Description";
            cboUserGroup.ValueMember = "Id";
            cboUserGroup.SelectedIndex = -1;
            
            if (lOperation == GlobalVariables.Operation.Edit)
            {
                txtUsername.Text = lRecords[0];
                txtUsername.ReadOnly = true;
                txtUsername.BackColor = SystemColors.Info;
                txtUsername.TabStop = false;
                txtFullname.Text = lRecords[1];
                txtPosition.Text = lRecords[2];
                cboUserGroup.Text = lRecords[3];
                txtRemarks.Text = lRecords[4];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                MessageBoxUI ms = new MessageBoxUI("Username must have a value!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                ms.showDialog();
                txtUsername.Focus();
                return;
            }

            loUser.Username = GlobalFunctions.replaceChar(txtUsername.Text);
            loUser.Fullname = GlobalFunctions.replaceChar(txtFullname.Text);
            try
            {
                loUser.UserGroupId = cboUserGroup.SelectedValue.ToString();
            }
            catch
            {
                MessageBoxUI ms = new MessageBoxUI("You must select a User Group!", GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                ms.showDialog();
                cboUserGroup.Focus();
                return;
            }
            loUser.Position = GlobalFunctions.replaceChar(txtPosition.Text);
            loUser.Password = "";
            loUser.Remarks = GlobalFunctions.replaceChar(txtRemarks.Text);
            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                if (loUser.save(lOperation, ref _Trans))
                {
                    _Trans.Commit();
                    MessageBoxUI _mb = new MessageBoxUI("User has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();

                    lRecords[0] = txtUsername.Text;
                    lRecords[1] = txtFullname.Text;
                    lRecords[2] = txtPosition.Text;
                    lRecords[3] = cboUserGroup.Text;
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
                    MessageBoxUI _mb = new MessageBoxUI("Username already exist!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    return;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion "END OF EVENTS"
    }
}
