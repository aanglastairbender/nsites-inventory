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
using NSites.ApplicationObjects.UserInterfaces.Report.ReportRpt;
using NSites.ApplicationObjects.UserInterfaces.Report;

namespace NSites.ApplicationObjects.UserInterfaces.Report
{
    public partial class CustomerTransactionsUI : Form
    {
        #region "VARIABLES"
        Customer loCustomer;
        InventoryDetail loInventoryDetail;

        DataTable ldtCustomer;
        DataTable ldtReports;
        CustomerTransactionsRpt loCustomerTransactionsRpt;
        ReportViewerUI loReportViewer;
        #endregion "END OF VARIABLES"
        
        public CustomerTransactionsUI()
        {
            InitializeComponent();
            loCustomer = new Customer();
            loInventoryDetail = new InventoryDetail();

            ldtCustomer = new DataTable();
            ldtReports = new DataTable();
            loCustomerTransactionsRpt = new CustomerTransactionsRpt();
            loReportViewer = new ReportViewerUI();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void getDetails()
        {
            loCustomerTransactionsRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
            loCustomerTransactionsRpt.Database.Tables[1].SetDataSource(loCustomer.getCustomerTransactions(dtpFromDate.Value, dtpToDate.Value, dgvCustomerList.CurrentRow.Cells[0].Value.ToString()));
            loCustomerTransactionsRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
            loCustomerTransactionsRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
            loCustomerTransactionsRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
            loCustomerTransactionsRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
            loCustomerTransactionsRpt.SetParameterValue("DateFrom", string.Format("{0:MM-dd-yyyy}", dtpFromDate.Value));
            loCustomerTransactionsRpt.SetParameterValue("DateTo", string.Format("{0:MM-dd-yyyy}", dtpToDate.Value));
            loCustomerTransactionsRpt.SetParameterValue("Customer", dgvCustomerList.CurrentRow.Cells[1].Value.ToString());
            loCustomerTransactionsRpt.SetParameterValue("Title", "Customer Transactions");
            loCustomerTransactionsRpt.SetParameterValue("SubTitle", "Customer Transactions");
            crystalReportViewer.ReportSource = loCustomerTransactionsRpt;
        }

        private void CustomerTransactionsUI_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmCustomerTransactions", "Refresh"))
            {
                return;
            }
            try
            {
                ldtCustomer = loCustomer.getAllData("ViewAll","");
                GlobalFunctions.refreshGrid(ref dgvCustomerList, ldtCustomer);
                //getDetails();
            }
            catch { }
        }

        private void dgvCustomerList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvCustomerList.Columns[e.ColumnIndex].Name == "Id")
                {
                    if (e.Value != null)
                    {
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                else if (this.dgvCustomerList.Columns[e.ColumnIndex].Name == "Credit Limit")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                else if (this.dgvCustomerList.Columns[e.ColumnIndex].Name == "Terms")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:0}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch { }
        }

        private void txtSearchStock_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ldtCustomer = loCustomer.getAllData("", txtSearchStock.Text);
                GlobalFunctions.refreshGrid(ref dgvCustomerList, ldtCustomer);
                //getDetails();
            }
            catch { }
        }

        private void dgvCustomerList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvCustomerList.PointToScreen(e.Location);
                cmsFunctionsItem.Show(pt);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvCustomerList, ldtCustomer);
        }

        private void dgvCustomerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            getDetails();
        }
    }
}
