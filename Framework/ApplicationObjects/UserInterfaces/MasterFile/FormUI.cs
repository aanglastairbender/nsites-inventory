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

namespace NSites.ApplicationObjects.UserInterfaces.MasterFile
{
    public partial class FormUI : Form
    {
        #region "VARIABLES"
        string[] lRecords;
        string[] lColumnName;
        Hashtable lObjectArgHash = new Hashtable();
        object[] lObjectArg;
        object lObject;
        Type lType;
        bool lFromAdd = true;
        string lButtonClassName;
        string lClassName;
        LookUpValueUI loLookupValue;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public FormUI()
        {
            InitializeComponent();
        }

        public FormUI(string[] pColumnName, object pObject, Type pType)
        {
            InitializeComponent();
            lColumnName = pColumnName;
            lObject = pObject;
            lType = pType;
            lFromAdd = true;
            lClassName = lType.Name;
            this.Text = pType.Name;
            this.Location = new Point(GlobalVariables.xLocation+150, GlobalVariables.yLocation+100);
            loLookupValue = new LookUpValueUI();
        }

        public FormUI(string[] pRecords, string[] pColumnName, object pObject, Type pType)
        {
            InitializeComponent();
            lRecords = pRecords;
            lColumnName = pColumnName;
            lObject = pObject;
            lType = pType;
            lFromAdd = false;
            lClassName = lType.Name;
            this.Text = pType.Name;
            this.Location = new Point(GlobalVariables.xLocation+150, GlobalVariables.yLocation+100);
            loLookupValue = new LookUpValueUI();
        }
        #endregion "END OF VARIABLES"

        #region "PROPERTIES"
        public Form ParentList
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "EVENTS"
        private void FormUI_Load(object sender, EventArgs e)
        {
            pnlBody.Controls.Clear();
            int x = 19;
            int y = 19;
            int width = 160;
            int height = 16;

            int xtxt = 180;
            int ytxt = 16;
            int widthtxt = 163;
            int heighttxt = 22;
            int widhttxtbtn = 132;

            int ybtn = 16;
            int widthbtn = 30;
            int heightbtn = 25;

            pnlBody.Width = 370;
            pnlBody.Height = 46;
            this.Width = 400;
            this.Height = 165;
            bool withbtn = false;

            var er = from colname in lColumnName
                     select colname;

            for (int i = 0; i < er.Count(); i++)
            {
                #region "For Default,Saleable"
                if (lColumnName[i].Contains("Default") || lColumnName[i].Contains("Saleable") ||
                    lColumnName[i].Contains("Cash Payment") || lColumnName[i].Contains("V A T Exempted"))
                {
                    this.Height += height * 2;
                    CheckBox chkCheckBox = new CheckBox();
                    if (!lFromAdd)
                    {
                        if (lRecords[i] == "Y")
                        {
                            chkCheckBox.Checked = true;
                        }
                        else
                        {
                            chkCheckBox.Checked = false;
                        }
                    }
                    chkCheckBox.Text = lColumnName[i];
                    chkCheckBox.Name = lColumnName[i];
                    chkCheckBox.Width = widthtxt;
                    chkCheckBox.Height = heighttxt;
                    chkCheckBox.TabIndex = i;
                    chkCheckBox.Location = new Point(xtxt, ytxt);

                    pnlBody.Height += height * 2;
                    pnlBody.Controls.Add(chkCheckBox);
                    y += height * 2;
                    ytxt += height * 2;
                    ybtn += height * 2;
                    continue;
                }
                #endregion

                #region "For Color"
                if (lColumnName[i].Contains("Color"))
                {
                    this.Height += height * 2;
                    Label lblColorName = new Label();
                    lblColorName.Text = lColumnName[i];
                    lblColorName.Width = width;
                    lblColorName.Height = height;
                    lblColorName.Location = new Point(x, y);

                    Label lblColor = new Label();
                    if (!lFromAdd)
                    {
                        lblColor.Text = lRecords[i];
                        try
                        {
                            lblColor.BackColor = Color.FromArgb(int.Parse(lRecords[i].ToString()));
                            lblColor.ForeColor = Color.FromArgb(int.Parse(lRecords[i].ToString()));
                        }
                        catch { }
                    }
                    lblColor.Name = lColumnName[i];
                    lblColor.Width = widhttxtbtn;
                    lblColor.Height = heighttxt;
                    lblColor.TabIndex = i;
                    lblColor.TabStop = false;
                    lblColor.BorderStyle = BorderStyle.FixedSingle;
                    lblColor.AutoSize = false;
                    lblColor.Location = new Point(xtxt, ytxt);
                    Button btnColorButton = new Button();
                    btnColorButton.Name = lColumnName[i];
                    btnColorButton.Width = widthbtn;
                    btnColorButton.Height = heightbtn;
                    btnColorButton.TabIndex = i;
                    btnColorButton.Image = Image.FromFile(".../Main/Images/Common/lookup.PNG");
                    btnColorButton.Location = new Point(xtxt + widhttxtbtn + 2, ytxt);
                    btnColorButton.Click += new EventHandler(btnColorButton_Click);
                    
                    pnlBody.Controls.Add(btnColorButton);

                    pnlBody.Height += height * 2;
                    pnlBody.Controls.Add(lblColorName);
                    pnlBody.Controls.Add(lblColor);

                    y += height * 2;
                    ytxt += height * 2;
                    ybtn += height * 2;
                    continue;
                }
                #endregion

                #region "For Texboxes"
                this.Height += height * 2;
                Label lblName = new Label();
                lblName.Text = lColumnName[i];
                lblName.Width = width;
                lblName.Height = heighttxt;
                lblName.Location = new Point(x, y);

                TextBox txtTextBox = new TextBox();
                if (i == 0)
                {
                    if (!lFromAdd)
                    {
                        txtTextBox.Text = lRecords[i];
                        txtTextBox.TabStop = false;
                        txtTextBox.ReadOnly = true;
                        txtTextBox.BackColor = Color.FromName("Info");
                    }
                    else
                    {
                        if (lColumnName[i].Contains("Id"))
                        {
                            txtTextBox.Text = "AUTO";
                            txtTextBox.TextAlign = HorizontalAlignment.Center;
                            txtTextBox.TabStop = false;
                            txtTextBox.ReadOnly = true;
                            txtTextBox.BackColor = Color.FromName("Info");
                        }
                    }
                    txtTextBox.Name = lColumnName[i];
                    txtTextBox.Width = widthtxt;
                    txtTextBox.Height = heighttxt;
                    txtTextBox.Location = new Point(xtxt, ytxt);
                }
                else
                {
                    if (lColumnName[i].ToString().Contains("Code") || lColumnName[i].ToString().Contains("Id"))
                    {
                        if (!lFromAdd)
                        {
                            txtTextBox.Text = lRecords[i];
                        }
                        txtTextBox.Name = lColumnName[i];
                        txtTextBox.Width = widhttxtbtn;
                        txtTextBox.Height = heighttxt;
                        txtTextBox.TabIndex = i;
                        txtTextBox.TabStop = false;
                        txtTextBox.ReadOnly = true;
                        txtTextBox.Location = new Point(xtxt, ytxt);
                        Button btnButton = new Button();
                        btnButton.Name = lColumnName[i];
                        btnButton.Width = widthbtn;
                        btnButton.Height = heightbtn;
                        btnButton.TabIndex = i;
                        btnButton.Image = Image.FromFile(".../Main/Images/Common/lookup.PNG");
                        btnButton.Location = new Point(xtxt + widhttxtbtn + 2, ytxt);
                        btnButton.Click += new EventHandler(btnButton_Click);
                        pnlBody.Controls.Add(btnButton);
                        withbtn = true;
                    }
                    else
                    {
                        if (withbtn)
                        {
                            if (!lFromAdd)
                            {
                                txtTextBox.Text = lRecords[i];
                            }
                            txtTextBox.Name = lColumnName[i];
                            txtTextBox.Width = widthtxt;
                            txtTextBox.Height = heighttxt;
                            txtTextBox.TabStop = false;
                            txtTextBox.ReadOnly = true;
                            txtTextBox.BackColor = Color.FromName("Info");
                            txtTextBox.Location = new Point(xtxt, ytxt);
                            withbtn = false;
                        }
                        else
                        {
                            if (!lFromAdd)
                            {
                                txtTextBox.Text = lRecords[i];
                            }
                            if (lColumnName[i] == " Amount" || lColumnName[i] == " Percentage")
                            {
                                txtTextBox.TextAlign = HorizontalAlignment.Right;
                            }
                            txtTextBox.Name = lColumnName[i];
                            txtTextBox.Width = widthtxt;
                            txtTextBox.Height = heighttxt;
                            txtTextBox.TabIndex = i;
                            txtTextBox.Location = new Point(xtxt, ytxt);
                        }
                    }
                }
                pnlBody.Height += height * 2;
                pnlBody.Controls.Add(lblName);
                pnlBody.Controls.Add(txtTextBox);

                y += height * 2;
                ytxt += height * 2;
                ybtn += height * 2;
                #endregion
            }
            pnlBody.Height -= 20;
            //this.Height = pnlBody.Height + pnlFooter.Height + 60;
        }

        private void btnButton_Click(object sender, EventArgs e)
        {
            lButtonClassName = "";
            bool _fromlookup = false;
            int _desctab = 2;
            int _tag = -1;
            char[] splitter = { ' ' };
            string[] name = ((Control)sender).Name.Split(splitter);
            for (int i = 0; i < name.Length - 1; i++)
            {
                lButtonClassName += name[i];
            }
            GlobalClassHandler ch = new GlobalClassHandler();
            Type _ObjectType = ch.createObjectFromClass(lButtonClassName);
            object ClassInstance = Activator.CreateInstance(_ObjectType);
            loLookupValue.lObject = ClassInstance;
            loLookupValue.lType = _ObjectType;
            loLookupValue.lTableName = lButtonClassName;
            loLookupValue.ShowDialog();
            if (loLookupValue.lCode != null)
            {
                foreach (Control ctrl in this.pnlBody.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        ctrl.Text = loLookupValue.lCode;
                        _tag = ctrl.TabIndex;
                        _fromlookup = true;
                        _desctab++;
                    }
                    if (_fromlookup)
                    {
                        //if (ctrl.Name.Replace(" ", "") == loLookupValue.lDescName && ctrl is TextBox)
                        //{
                        //    ctrl.Text = loLookupValue.lDesc;
                        //}
                    }
                }
                _tag = -1;
            }
        }

        private void btnColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog clrColorDialog = new ColorDialog();
            string txtBoxName = ((Control)sender).Name;
            if (clrColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (Control ctrl in this.pnlBody.Controls)
                {
                    if (ctrl.Name == txtBoxName && ctrl is Label)
                    {
                        ctrl.Text = clrColorDialog.Color.ToArgb().ToString();
                        ctrl.BackColor = clrColorDialog.Color;
                        ctrl.ForeColor = clrColorDialog.Color;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                char[] splitter = { '_' };
                foreach (Control ctrl in this.pnlBody.Controls)
                {
                    if (ctrl is TextBox && (ctrl.Name.Contains("Code") || ctrl.Name.Contains("Id")) && ctrl.Text == "")
                    {
                        MessageBoxUI mb = new MessageBoxUI(ctrl.Name + " must have a value!", GlobalVariables.Icons.Warning, GlobalVariables.Buttons.OK);
                        mb.ShowDialog();
                        ctrl.Focus();
                        return;
                    }
                    else if (ctrl is TextBox)
                    {
                        string namestr = ctrl.Name.ToString().Replace(" ", "");
                        string textstr = ctrl.Text.ToString();
                        lObjectArgHash.Add(namestr, textstr);
                    }
                    else if (ctrl is CheckBox)
                    {
                        string namestr = ctrl.Name.ToString().Replace(" ", "");
                        CheckBox chk = ctrl as CheckBox;
                        if (chk.Checked)
                        {
                            lObjectArgHash.Add(namestr, "Y");
                        }
                        else
                        {
                            lObjectArgHash.Add(namestr, "N");
                        }
                    }
                    else if (ctrl is Label)
                    {
                        if (ctrl.Name == " Color")
                        {
                            string namestr = ctrl.Name.ToString().Replace(" ", "");
                            string textstr = ctrl.Text.ToString();
                            lObjectArgHash.Add(namestr, textstr);
                        }
                    }
                }
                if (!lFromAdd)
                {
                    lObjectArgHash["Operation"] = "Edit";
                }
                else
                {
                    lObjectArgHash["Operation"] = "Add";
                }
                lObjectArg = new object[1];
                lObjectArg[0] = lObjectArgHash;
            }
            catch (Exception ex)
            {
                this.Show();
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                return;
            }
            MySqlTransaction Trans = GlobalVariables.Connection.BeginTransaction();
            object[] param = { lObjectArgHash, Trans };

            try
            {
                if ((bool)lObject.GetType().GetMethod("save").Invoke(lObject, param))
                {
                    Trans.Commit();
                    this.Hide();
                    MessageBoxUI _mb = new MessageBoxUI(lClassName + " has been saved successfully!", GlobalVariables.Icons.Save, GlobalVariables.Buttons.OK);
                    _mb.showDialog();
                    try
                    {
                        object[] _params = { "ViewAll", "" };
                        ParentList.GetType().GetMethod("refresh").Invoke(ParentList, _params);
                    }
                    catch { }
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                this.Show();
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
                return;
            }
        }
        #endregion "END OF EVENTS"
    }
}
