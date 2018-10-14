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
    public partial class ReorderLevelUI : Form
    {
        #region "VARIABLES"
        Stock loItem;
        DataTable ldtReports;
        ReorderLevelRpt loReorderLevelRpt;
        ReportViewerUI loReportViewer;
        #endregion "END OF VARIABLES"

        public ReorderLevelUI()
        {
            InitializeComponent();
            loItem = new Stock();
            ldtReports = new DataTable();
            loReorderLevelRpt = new ReorderLevelRpt();
            loReportViewer = new ReportViewerUI();
        }

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        private void ReorderLevelUI_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ParentList.GetType().GetMethod("closeTabPage").Invoke(ParentList, null);
        }

        private void viewAllRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalFunctions.refreshAll(ref dgvReport, ldtReports);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmReorderLevelReport", "Preview"))
            {
                return;
            }
            try
            {
                loReorderLevelRpt.SetDataSource(GlobalVariables.DTCompanyLogo);
                loReorderLevelRpt.Database.Tables[1].SetDataSource(ldtReports);
                loReorderLevelRpt.SetParameterValue("CompanyName", GlobalVariables.CompanyName);
                loReorderLevelRpt.SetParameterValue("CompanyAddress", GlobalVariables.CompanyAddress);
                loReorderLevelRpt.SetParameterValue("CompanyContactNumber", GlobalVariables.ContactNumber);
                loReorderLevelRpt.SetParameterValue("Username", GlobalVariables.Userfullname);
                loReorderLevelRpt.SetParameterValue("Title", "Reorder Level");
                loReorderLevelRpt.SetParameterValue("SubTitle", "Reorder Level");
                loReportViewer.crystalReportViewer.ReportSource = loReorderLevelRpt;
                loReportViewer.ShowDialog();
            }
            catch { }
        }

        private void dgvReport_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = dgvReport.PointToScreen(e.Location);
                cmsFunctions.Show(pt);
            }
        }

        private void dgvReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (this.dgvReport.Columns[e.ColumnIndex].Name == "Stock Id" || this.dgvReport.Columns[e.ColumnIndex].Name == "Unit")
                {
                    if (e.Value != null)
                    {
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                else if (this.dgvReport.Columns[e.ColumnIndex].Name == "Qty on Hand" || this.dgvReport.Columns[e.ColumnIndex].Name == "Variance" ||
                    this.dgvReport.Columns[e.ColumnIndex].Name == "Reorder Level")
                {
                    if (e.Value != null)
                    {
                        e.Value = String.Format("{0:n}", decimal.Parse(e.Value.ToString()));
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch { }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!GlobalFunctions.checkRights("tsmReorderLevelReport", "Refresh"))
            {
                return;
            }
            try
            {
                dgvReport.Rows.Clear();
            }
            catch
            {
                dgvReport.DataSource = null;
            }
            
            try
            {
                ldtReports = loItem.getStockReorderLevel();
                GlobalFunctions.refreshGrid(ref dgvReport, ldtReports);
            }
            catch
            {
                
            }
        }
    }
}
