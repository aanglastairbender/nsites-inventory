using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;

using NSites.ApplicationObjects.UserInterfaces.Report;
using NSites.ApplicationObjects.UserInterfaces.Report.GlobalRpt;

namespace NSites.Global
{
    public class GlobalVariables
    {
        public static MySqlConnection Connection = new MySqlConnection();
        public static MySqlConnection ConnectionBackup = new MySqlConnection();
        public static string DatabaseServer = "";
        public static string DatabaseName = "";
        public static string DatabaseUID = "";
        public static string DatabasePWD = "";
        public static string DatabasePort = "";
        //application
        public static string ApplicationId = "";
        public static string ApplicationName = "";
        public static string ProcessorId = "";
        public static string TrialVersion = "";
        public static string LicenseKey = "";
        public static DateTime lLicenseExpiry;
        public static string VersionNo = "";
        public static string DevelopedBy = "";
        //company
        public static string CompanyName = "";
        public static string CompanyAddress = "";
        public static string ContactNumber = "";
        public static string CompanyLogo = "";
        //pos settings
        public static string BackupPath = "";
        public static string BackupMySqlDumpAddress = "";
        public static string RestoreMySqlAddress = "";
        //
        public static decimal ProcessingFeePercentage = 0;
        public static decimal InterestRatePercentage = 0;
        public static decimal MiscellaneousPercentage = 0;
        //display
        public static string ScreenSaverImage = "";
        public static string TabAlignment = "";
        public static int DisplayRecordLimit = 0;
        //general use
        public static string Username = "";
        public static string Userfullname = "";
        public static string Hostname = "";
        public static int xLocation;
        public static int yLocation;
        //Dataview
        public static DataView DVRights;
        //data table
        public static DataTable DTCompanyLogo = new DataTable();
        public static ReportViewerUI loReportPreviewer = new ReportViewerUI();
        public static CrystalReport loCrystalReport = new CrystalReport();
        public enum Operation
        {
            Add = 0,
            Edit = 1,
            Remove = 3,
            Open = 4,
            Close = 5
        };
        public enum Icons
        {
            Information = 0,
            Save = 1,
            Ok = 2,
            QuestionMark = 3,
            Delete = 4,
            Warning = 5,
            Error = 6,
            Close = 7
        };
        public enum Buttons
        {
            OK = 0,
            OKCancel = 1,
            YesNo = 2,
            YesNoCancel = 3,
            Close = 4
        };
    }
}
