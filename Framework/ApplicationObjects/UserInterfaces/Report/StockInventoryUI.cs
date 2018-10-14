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
using NSites.ApplicationObjects.UserInterfaces.Report.ReportRpt;

namespace NSites.ApplicationObjects.UserInterfaces.Report
{
    public partial class StockInventoryUI : Form
    {
        StockInventoryRpt loStockInventoryRpt;
        InventoryDetail loInventoryDetail;
        Location loLocation;

        public StockInventoryUI()
        {
            InitializeComponent();
            loStockInventoryRpt = new StockInventoryRpt();
            loInventoryDetail = new InventoryDetail();
            loLocation = new Location();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void StockInventoryUI_Load(object sender, EventArgs e)
        {
            cboLocation.DataSource = loLocation.getAllData("ViewAll", "");
            cboLocation.DisplayMember = "Description";
            cboLocation.ValueMember = "Id";
            cboLocation.SelectedIndex = 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmStockInventory", "Preview"))
            {
                return;
            }
            try
            {
                loStockInventoryRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loStockInventoryRpt.Database.Tables[1].SetDataSource(loInventoryDetail.getStockInventory(cboLocation.SelectedValue.ToString()));
                loStockInventoryRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loStockInventoryRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loStockInventoryRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loStockInventoryRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loStockInventoryRpt.SetParameterValue("Title", "Stock Inventory");
                loStockInventoryRpt.SetParameterValue("SubTitle", "StockInventory");
                loStockInventoryRpt.SetParameterValue("Location", cboLocation.Text);
                crvStockInventory.ReportSource = loStockInventoryRpt;
            }
            catch
            { }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }
    }
}
