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
using NSites.ApplicationObjects.UserInterfaces;
using NSites.ApplicationObjects.UserInterfaces.MasterFile;
using NSites.ApplicationObjects.UserInterfaces.Systems;
using NSites.ApplicationObjects.UserInterfaces.Report;
using NSites.ApplicationObjects.UserInterfaces.Transaction;
using MySql.Data.MySqlClient;

namespace NSites.ApplicationObjects.UserInterfaces
{
    public partial class MDIUI : Form
    {
        #region "VARIABLES"
        UserGroup loUserGroup;
        DataView ldvUserGroup;
        DataTable ldtUserGroup;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public MDIUI()
        {
            InitializeComponent();
            loUserGroup = new UserGroup();
            ldtUserGroup = new DataTable();
        }
        #endregion "END OF CONSTRUCTORS"

        #region "METHODS"
        private void disabledMenuStrip()
        {
            foreach (ToolStripMenuItem item in mnsAssetManagement.Items)
            {
                item.Enabled = false;
                foreach (ToolStripItem subitem in item.DropDownItems)
                {
                    if (subitem is ToolStripMenuItem)
                    {
                        subitem.Enabled = false;
                    }
                }
            }
        }
        private void enabledMenuStrip()
        {
            ldtUserGroup = loUserGroup.getUserGroupMenuItems();
            GlobalVariables.DVRights = new DataView(loUserGroup.getUserGroupRights());
            ldvUserGroup = new DataView(ldtUserGroup);
            foreach (ToolStripMenuItem item in mnsAssetManagement.Items)
            {
                ldvUserGroup.RowFilter = "Menu = '" + item.Name + "'";
                if (ldvUserGroup.Count != 0)
                {
                    item.Enabled = true;
                    processMenuItems(item);
                }
            }
        }
        private void processMenuItems(ToolStripMenuItem pitem)
        {
            if (true)
            {
                pitem.Enabled = true;
            }

            foreach (ToolStripItem subitem in pitem.DropDownItems)
            {
                if (subitem is ToolStripMenuItem)
                {
                    ldvUserGroup.RowFilter = "Item = '" + subitem.Name + "'";
                    if (ldvUserGroup.Count != 0)
                    {
                        subitem.Enabled = true;
                    }
                }
            }
        }
        private int displayControlOnTab(Control pControl, TabPage pTabPage)
        {
            // The tabpage.
            Form _FormControl = new Form();
            _FormControl = (Form)pControl;

            // Add to the tab control.
            pTabPage.Text = _FormControl.Text;
            pTabPage.Name = _FormControl.Name;
            tbcSalesAndInventorySystem.TabPages.Add(pTabPage);
            tbcSalesAndInventorySystem.SelectTab(pTabPage);
            _FormControl.TopLevel = false;
            _FormControl.Parent = this;
            _FormControl.Dock = DockStyle.Fill;
            _FormControl.FormBorderStyle = FormBorderStyle.None;
            pTabPage.Controls.Add(_FormControl);
            tbcSalesAndInventorySystem.SelectTab(tbcSalesAndInventorySystem.SelectedIndex);
            _FormControl.Show();
            return tbcSalesAndInventorySystem.SelectedIndex;
        }
        public void closeTabPage()
        {
            tbcSalesAndInventorySystem.TabPages.RemoveAt(tbcSalesAndInventorySystem.SelectedIndex);
        }
        public void changeHomeImage()
        {
            try
            {
                byte[] hextobyte = GlobalFunctions.HexToBytes(GlobalVariables.ScreenSaverImage);
                pctScreenSaver.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                pctScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch
            {
                pctScreenSaver.BackgroundImage = null;
            }
        }
        #endregion "END OF METHODS"

        #region "EVENTS"
        private void MDIFrameWork_Load(object sender, EventArgs e)
        {
            this.Text = GlobalVariables.CompanyName;
            try
            {
                byte[] hextobyte = GlobalFunctions.HexToBytes(GlobalVariables.ScreenSaverImage);
                pctScreenSaver.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyte);
                pctScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }
            try
            {
                byte[] hextobyteLogo = GlobalFunctions.HexToBytes(GlobalVariables.CompanyLogo);
                pctCompanyLogo.BackgroundImage = GlobalFunctions.ConvertByteArrayToImage(hextobyteLogo);
                pctCompanyLogo.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }
            
            try
            {
                switch (GlobalVariables.TabAlignment)
                {
                    case "Top":
                        tbcSalesAndInventorySystem.Alignment = TabAlignment.Top;
                        break;
                    case "Bottom":
                        tbcSalesAndInventorySystem.Alignment = TabAlignment.Bottom;
                        break;
                    case "Left":
                        tbcSalesAndInventorySystem.Alignment = TabAlignment.Left;
                        break;
                    case "Right":
                        tbcSalesAndInventorySystem.Alignment = TabAlignment.Right;
                        break;
                    default:
                        tbcSalesAndInventorySystem.Alignment = TabAlignment.Top;
                        break;
                }
            }
            catch { }
            lblUsername.Text = "Welcome!  " + GlobalVariables.Username;
            tssDateTime.Text = DateTime.Now.ToLongDateString();
            lblOwnerName.Text = GlobalVariables.CompanyName;
            lblApplicationName.Text = GlobalVariables.ApplicationName;
            if (GlobalVariables.Username != "admin" && GlobalVariables.Username != "jbcsupport")
            {
                disabledMenuStrip();
                enabledMenuStrip();
            }
        }

        private void tsmSystemConfiguration_Click(object sender, EventArgs e)
        {
            SystemConfigurationUI _SystemConfiguration = new SystemConfigurationUI();
            TabPage _SystemConfigurationTab = new TabPage();
            _SystemConfigurationTab.ImageIndex = 1;
            _SystemConfiguration.ParentList = this;
            displayControlOnTab(_SystemConfiguration, _SystemConfigurationTab);
        }
        private void tsmUser_Click(object sender, EventArgs e)
        {
            User _User = new User();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(User);
            ListFormUI _ListForm = new ListFormUI((object)_User, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 2;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }
        private void tsmUserGroup_Click(object sender, EventArgs e)
        {
           UserGroupListUI _UserGroupList = new UserGroupListUI();
           TabPage _UserGroupTab = new TabPage();
           _UserGroupTab.ImageIndex = 3;
           _UserGroupList.ParentList = this;
           displayControlOnTab(_UserGroupList, _UserGroupTab);
        }
        private void tsmChangeUserPassword_Click(object sender, EventArgs e)
        {
            ChangePasswordUI _ChangePassword = new ChangePasswordUI();
            TabPage _ChangePasswordTab = new TabPage();
            _ChangePasswordTab.ImageIndex = 4;
            _ChangePassword.ParentList = this;
            displayControlOnTab(_ChangePassword, _ChangePasswordTab);
        }

        private void tsmLockScreen_Click(object sender, EventArgs e)
        {
            UnlockScreenUI _UnlockScreen = new UnlockScreenUI();
            _UnlockScreen.ShowDialog();
        }

        private void tsmBackupRestoreDatabase_Click(object sender, EventArgs e)
        {
            BackupRestoreDataUI _ImportExportData = new BackupRestoreDataUI();
            TabPage _ImportExportDataTab = new TabPage();
            _ImportExportDataTab.ImageIndex = 8;
            _ImportExportData.ParentList = this;
            displayControlOnTab(_ImportExportData, _ImportExportDataTab);
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsmScreenSaver_Click(object sender, EventArgs e)
        {
            ScreenSaverUI _ScreenSaver = new ScreenSaverUI();
            TabPage _ScreenSaverTab = new TabPage();
            _ScreenSaverTab.ImageIndex = 5;
            _ScreenSaver.ParentList = this;
            displayControlOnTab(_ScreenSaver, _ScreenSaverTab);
        }
        #endregion "END OF EVENTS"

        private void tsmItemCategory_Click(object sender, EventArgs e)
        {
            Category _Category = new Category();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Category);
            ListFormUI _ListForm = new ListFormUI((object)_Category, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 13;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmItem_Click(object sender, EventArgs e)
        {
            Stock _Item = new Stock();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Stock);
            ListFormUI _ListForm = new ListFormUI((object)_Item, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 14;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmStockAdjustment_Click(object sender, EventArgs e)
        {
            StockAdjustmentUI _StockAdjustment = new StockAdjustmentUI();
            TabPage _StockAdjustmentTab = new TabPage();
            _StockAdjustmentTab.ImageIndex = 25;
            _StockAdjustment.ParentList = this;
            displayControlOnTab(_StockAdjustment, _StockAdjustmentTab);
        }

        private void tsmStockReceiving_Click(object sender, EventArgs e)
        {
            StockReceivingUI _StockReceiving = new StockReceivingUI();
            TabPage _StockReceivingTab = new TabPage();
            _StockReceivingTab.ImageIndex = 23;
            _StockReceiving.ParentList = this;
            displayControlOnTab(_StockReceiving, _StockReceivingTab);
        }

        private void tsmReorderLevelReport_Click(object sender, EventArgs e)
        {
            ReorderLevelUI _ReorderLevel = new ReorderLevelUI();
            TabPage _ReorderLevelTab = new TabPage();
            _ReorderLevelTab.ImageIndex = 28;
            _ReorderLevel.ParentList = this;
            displayControlOnTab(_ReorderLevel, _ReorderLevelTab);
        }

        private void tsmStockCard_Click(object sender, EventArgs e)
        {
            StockCardUI _StockCard = new StockCardUI();
            TabPage _StockCardTab = new TabPage();
            _StockCardTab.ImageIndex = 27;
            _StockCard.ParentList = this;
            displayControlOnTab(_StockCard, _StockCardTab);
        }

        private void tsmCustomer_Click(object sender, EventArgs e)
        {
            Customer _Customer = new Customer();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Customer);
            ListFormUI _ListForm = new ListFormUI((object)_Customer, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 15;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmAuditTrail_Click(object sender, EventArgs e)
        {
            AuditTrailUI _ElectronicJournal = new AuditTrailUI();
            TabPage _ElectronicJournalTab = new TabPage();
            _ElectronicJournalTab.ImageIndex = 7;
            _ElectronicJournal.ParentList = this;
            displayControlOnTab(_ElectronicJournal, _ElectronicJournalTab);
        }

        private void tsmSupplier_Click(object sender, EventArgs e)
        {
            Supplier _Supplier = new Supplier();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Supplier);
            ListFormUI _ListForm = new ListFormUI((object)_Supplier, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 16;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmLocation_Click(object sender, EventArgs e)
        {
            Location _Location = new Location();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Location);
            ListFormUI _ListForm = new ListFormUI((object)_Location, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 10;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmStockInventory_Click(object sender, EventArgs e)
        {
            StockInventoryUI _StockInventory = new StockInventoryUI();
            TabPage _StockInventoryTab = new TabPage();
            _StockInventoryTab.ImageIndex = 26;
            _StockInventory.ParentList = this;
            displayControlOnTab(_StockInventory, _StockInventoryTab);
        }

        private void tsmTechnicalUpdate_Click(object sender, EventArgs e)
        {
            TechnicalUpdateUI _TechnicalUpdate = new TechnicalUpdateUI();
            TabPage _TechnicalUpdateTab = new TabPage();
            _TechnicalUpdateTab.ImageIndex = 9;
            _TechnicalUpdate.ParentList = this;
            displayControlOnTab(_TechnicalUpdate, _TechnicalUpdateTab);
        }

        private void tsmUnit_Click(object sender, EventArgs e)
        {
            Unit _Unit = new Unit();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(Unit);
            ListFormUI _ListForm = new ListFormUI((object)_Unit, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 11;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmInventoryGroup_Click(object sender, EventArgs e)
        {
            InventoryGroup _InventoryGroup = new InventoryGroup();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(InventoryGroup);
            ListFormUI _ListForm = new ListFormUI((object)_InventoryGroup, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 12;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmStockWithdrawal_Click(object sender, EventArgs e)
        {
            StockWithdrawalUI _StockWithdrawal = new StockWithdrawalUI();
            TabPage _StockWithdrawalTab = new TabPage();
            _StockWithdrawalTab.ImageIndex = 21;
            _StockWithdrawal.ParentList = this;
            displayControlOnTab(_StockWithdrawal, _StockWithdrawalTab);
        }

        private void tsmInventoryType_Click(object sender, EventArgs e)
        {
            InventoryType _InventoryType = new InventoryType();
            FormUI _Form = new FormUI();
            Type _TypeUI = typeof(FormUI);
            Type _Type = typeof(InventoryType);
            ListFormUI _ListForm = new ListFormUI((object)_InventoryType, (object)_Form, _TypeUI, _Type);
            TabPage _ListFormTab = new TabPage();
            _ListFormTab.ImageIndex = 15;
            _ListForm.ParentList = this;
            displayControlOnTab(_ListForm, _ListFormTab);
        }

        private void tsmCustomerTransactions_Click(object sender, EventArgs e)
        {
            CustomerTransactionsUI _CustomerTransactions = new CustomerTransactionsUI();
            TabPage _CustomerTransactionsTab = new TabPage();
            _CustomerTransactionsTab.ImageIndex = 27;
            _CustomerTransactions.ParentList = this;
            displayControlOnTab(_CustomerTransactions, _CustomerTransactionsTab);
        }
    }
}
