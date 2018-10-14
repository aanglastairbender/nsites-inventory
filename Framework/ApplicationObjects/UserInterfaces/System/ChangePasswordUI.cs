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

namespace NSites.ApplicationObjects.UserInterfaces.Systems
{
    public partial class ChangePasswordUI : Form
    {
        #region "VARIABLES"
        User loUser;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public ChangePasswordUI()
        {
            InitializeComponent();
            loUser = new User();
        }
        #endregion "END OF CONSTRUCTORS"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "EVENTS"

        private void ChangePasswordUI_Load(object sender, EventArgs e)
        {
            txtCurrentPassword.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmChangeUserPassword", "Save"))
            {
                return;
            }
            if (loUser.checkUserPassword(txtCurrentPassword.Text))
            {
                MessageBoxUI ms = new MessageBoxUI("Please specify the correct current password.", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                ms.showDialog();
                txtCurrentPassword.Focus();
                return;
            }

            if (txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                MessageBoxUI ms = new MessageBoxUI("Your new password entries did not match.", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                ms.showDialog();
                txtNewPassword.Focus();
                return;
            }

            if (loUser.changePassword(txtNewPassword.Text, txtCurrentPassword.Text))
            {
                MessageBoxUI ms = new MessageBoxUI("Your password has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                ms.showDialog();
                txtNewPassword.Focus();
                ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
            }
            catch
            {
                this.Close();
            }
        }

        #endregion "END OF EVENTS"
    }
}
