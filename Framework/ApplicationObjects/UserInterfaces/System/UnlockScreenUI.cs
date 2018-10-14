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
    public partial class UnlockScreenUI : Form
    {
        #region "VARIABLES"
        User loUser = new User();
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public UnlockScreenUI()
        {
            InitializeComponent();
        }
        #endregion "END OF CONSTRUCTORS"

        #region "EVENTS"
        private void UnlockScreenUI_Load(object sender, EventArgs e)
        {
            lblUsername.Text = GlobalVariables.Username;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                btnUnlock_Click(null, new EventArgs());
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            User _User = new User();
            if (_User.autenticateUser(GlobalVariables.Username, txtPassword.Text))
            {
                this.Close();
            }
            else
            {
                MessageBoxUI ms = new MessageBoxUI("User password is incorrect!", GlobalVariables.Icons.Information, GlobalVariables.Buttons.OK);
                ms.showDialog();
                txtPassword.Focus();
                return;
            }
        }
        #endregion "EVENTS"
    }
}
