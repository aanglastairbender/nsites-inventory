namespace NSites.Global
{
    partial class LookUpValueUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookUpValueUI));
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTableName = new System.Windows.Forms.Label();
            this.dgvLookUp = new System.Windows.Forms.DataGridView();
            this.cmsFunctions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewHiddenRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnNull = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLookUp)).BeginInit();
            this.cmsFunctions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Location = new System.Drawing.Point(423, 17);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(156, 25);
            this.txtSearch.TabIndex = 19;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(370, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Search";
            // 
            // lblTableName
            // 
            this.lblTableName.AutoSize = true;
            this.lblTableName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(59)))), ((int)(((byte)(117)))));
            this.lblTableName.Location = new System.Drawing.Point(12, 8);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(0, 30);
            this.lblTableName.TabIndex = 17;
            // 
            // dgvLookUp
            // 
            this.dgvLookUp.AllowUserToAddRows = false;
            this.dgvLookUp.AllowUserToDeleteRows = false;
            this.dgvLookUp.AllowUserToResizeRows = false;
            this.dgvLookUp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLookUp.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLookUp.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLookUp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLookUp.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLookUp.Location = new System.Drawing.Point(12, 49);
            this.dgvLookUp.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvLookUp.MultiSelect = false;
            this.dgvLookUp.Name = "dgvLookUp";
            this.dgvLookUp.ReadOnly = true;
            this.dgvLookUp.RowHeadersVisible = false;
            this.dgvLookUp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLookUp.Size = new System.Drawing.Size(567, 332);
            this.dgvLookUp.TabIndex = 16;
            this.dgvLookUp.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLookUp_CellClick);
            this.dgvLookUp.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLookUp_CellDoubleClick);
            this.dgvLookUp.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLookUp_CellFormatting);
            this.dgvLookUp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvLookUp_KeyPress);
            this.dgvLookUp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvLookUp_MouseClick);
            // 
            // cmsFunctions
            // 
            this.cmsFunctions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHiddenRecordsToolStripMenuItem,
            this.viewAllRecordsToolStripMenuItem});
            this.cmsFunctions.Name = "cmsFunctions";
            this.cmsFunctions.Size = new System.Drawing.Size(187, 48);
            // 
            // viewHiddenRecordsToolStripMenuItem
            // 
            this.viewHiddenRecordsToolStripMenuItem.Name = "viewHiddenRecordsToolStripMenuItem";
            this.viewHiddenRecordsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewHiddenRecordsToolStripMenuItem.Text = "View Hidden Records";
            this.viewHiddenRecordsToolStripMenuItem.Click += new System.EventHandler(this.viewHiddenRecordsToolStripMenuItem_Click);
            // 
            // viewAllRecordsToolStripMenuItem
            // 
            this.viewAllRecordsToolStripMenuItem.Name = "viewAllRecordsToolStripMenuItem";
            this.viewAllRecordsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewAllRecordsToolStripMenuItem.Text = "View All Records";
            this.viewAllRecordsToolStripMenuItem.Click += new System.EventHandler(this.viewAllRecordsToolStripMenuItem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(238, 16);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 28);
            this.btnAdd.TabIndex = 20;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnNull
            // 
            this.btnNull.BackColor = System.Drawing.SystemColors.Control;
            this.btnNull.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNull.Location = new System.Drawing.Point(301, 16);
            this.btnNull.Name = "btnNull";
            this.btnNull.Size = new System.Drawing.Size(54, 28);
            this.btnNull.TabIndex = 21;
            this.btnNull.Text = "Null";
            this.btnNull.UseVisualStyleBackColor = false;
            this.btnNull.Click += new System.EventHandler(this.btnNull_Click);
            // 
            // LookUpValueUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(207)))), ((int)(((byte)(140)))));
            this.ClientSize = new System.Drawing.Size(591, 395);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnNull);
            this.Controls.Add(this.lblTableName);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvLookUp);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LookUpValueUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LookUp Value";
            this.Load += new System.EventHandler(this.LookUpValueUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLookUp)).EndInit();
            this.cmsFunctions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.DataGridView dgvLookUp;
        private System.Windows.Forms.ContextMenuStrip cmsFunctions;
        private System.Windows.Forms.ToolStripMenuItem viewAllRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewHiddenRecordsToolStripMenuItem;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnNull;
    }
}