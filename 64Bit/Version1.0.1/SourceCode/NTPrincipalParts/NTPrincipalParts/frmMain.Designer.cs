
namespace NTPrincipalParts
{
    partial class frmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbOrder = new System.Windows.Forms.GroupBox();
            this.rbtnLeastUsedNT = new System.Windows.Forms.RadioButton();
            this.rbtnMostUsedNT = new System.Windows.Forms.RadioButton();
            this.rbtnAlpha = new System.Windows.Forms.RadioButton();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.labTextEnteredMsg = new System.Windows.Forms.Label();
            this.labTextEnteredLbl = new System.Windows.Forms.Label();
            this.labVirtualKeyboardLbl = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlKeys = new System.Windows.Forms.Panel();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labWordSummaryMsg = new System.Windows.Forms.Label();
            this.labWordSummaryLbl = new System.Windows.Forms.Label();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.lbAvailableWords = new System.Windows.Forms.ListBox();
            this.labAvailableWords = new System.Windows.Forms.Label();
            this.tmrInitial = new System.Windows.Forms.Timer(this.components);
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.gbOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOrder
            // 
            this.gbOrder.Controls.Add(this.rbtnLeastUsedNT);
            this.gbOrder.Controls.Add(this.rbtnMostUsedNT);
            this.gbOrder.Controls.Add(this.rbtnAlpha);
            this.gbOrder.Location = new System.Drawing.Point(468, 375);
            this.gbOrder.Name = "gbOrder";
            this.gbOrder.Size = new System.Drawing.Size(144, 83);
            this.gbOrder.TabIndex = 30;
            this.gbOrder.TabStop = false;
            this.gbOrder.Text = "Order entries:  ";
            // 
            // rbtnLeastUsedNT
            // 
            this.rbtnLeastUsedNT.AutoSize = true;
            this.rbtnLeastUsedNT.Location = new System.Drawing.Point(15, 53);
            this.rbtnLeastUsedNT.Name = "rbtnLeastUsedNT";
            this.rbtnLeastUsedNT.Size = new System.Drawing.Size(125, 17);
            this.rbtnLeastUsedNT.TabIndex = 2;
            this.rbtnLeastUsedNT.Tag = "3";
            this.rbtnLeastUsedNT.Text = "Least used in NT first";
            this.rbtnLeastUsedNT.UseVisualStyleBackColor = true;
            this.rbtnLeastUsedNT.CheckedChanged += new System.EventHandler(this.orderEntryCheckedChanged);
            // 
            // rbtnMostUsedNT
            // 
            this.rbtnMostUsedNT.AutoSize = true;
            this.rbtnMostUsedNT.Location = new System.Drawing.Point(15, 33);
            this.rbtnMostUsedNT.Name = "rbtnMostUsedNT";
            this.rbtnMostUsedNT.Size = new System.Drawing.Size(122, 17);
            this.rbtnMostUsedNT.TabIndex = 1;
            this.rbtnMostUsedNT.Tag = "2";
            this.rbtnMostUsedNT.Text = "Most used in NT first";
            this.rbtnMostUsedNT.UseVisualStyleBackColor = true;
            this.rbtnMostUsedNT.CheckedChanged += new System.EventHandler(this.orderEntryCheckedChanged);
            // 
            // rbtnAlpha
            // 
            this.rbtnAlpha.AutoSize = true;
            this.rbtnAlpha.Checked = true;
            this.rbtnAlpha.Location = new System.Drawing.Point(15, 13);
            this.rbtnAlpha.Name = "rbtnAlpha";
            this.rbtnAlpha.Size = new System.Drawing.Size(90, 17);
            this.rbtnAlpha.TabIndex = 0;
            this.rbtnAlpha.TabStop = true;
            this.rbtnAlpha.Tag = "1";
            this.rbtnAlpha.Text = "Alphabetically";
            this.rbtnAlpha.UseVisualStyleBackColor = true;
            this.rbtnAlpha.CheckedChanged += new System.EventHandler(this.orderEntryCheckedChanged);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewDetails.Location = new System.Drawing.Point(632, 210);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(75, 23);
            this.btnViewDetails.TabIndex = 29;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // labTextEnteredMsg
            // 
            this.labTextEnteredMsg.AutoSize = true;
            this.labTextEnteredMsg.Location = new System.Drawing.Point(370, 369);
            this.labTextEnteredMsg.Name = "labTextEnteredMsg";
            this.labTextEnteredMsg.Size = new System.Drawing.Size(33, 13);
            this.labTextEnteredMsg.TabIndex = 28;
            this.labTextEnteredMsg.Text = "None";
            // 
            // labTextEnteredLbl
            // 
            this.labTextEnteredLbl.AutoSize = true;
            this.labTextEnteredLbl.Location = new System.Drawing.Point(265, 369);
            this.labTextEnteredLbl.Name = "labTextEnteredLbl";
            this.labTextEnteredLbl.Size = new System.Drawing.Size(99, 13);
            this.labTextEnteredLbl.TabIndex = 27;
            this.labTextEnteredLbl.Text = "Text entered so far:";
            // 
            // labVirtualKeyboardLbl
            // 
            this.labVirtualKeyboardLbl.AutoSize = true;
            this.labVirtualKeyboardLbl.Location = new System.Drawing.Point(265, 220);
            this.labVirtualKeyboardLbl.Name = "labVirtualKeyboardLbl";
            this.labVirtualKeyboardLbl.Size = new System.Drawing.Size(87, 13);
            this.labVirtualKeyboardLbl.TabIndex = 26;
            this.labVirtualKeyboardLbl.Text = "Virtual Keyboard:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(632, 434);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlKeys
            // 
            this.pnlKeys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlKeys.Location = new System.Drawing.Point(256, 239);
            this.pnlKeys.Name = "pnlKeys";
            this.pnlKeys.Size = new System.Drawing.Size(430, 118);
            this.pnlKeys.TabIndex = 25;
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.AllowUserToOrderColumns = true;
            this.dgvSummary.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.ColumnHeadersVisible = false;
            this.dgvSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dgvSummary.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSummary.Location = new System.Drawing.Point(282, 38);
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSummary.Size = new System.Drawing.Size(404, 166);
            this.dgvSummary.TabIndex = 24;
            // 
            // Column1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.Width = 200;
            // 
            // labWordSummaryMsg
            // 
            this.labWordSummaryMsg.AutoSize = true;
            this.labWordSummaryMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labWordSummaryMsg.Location = new System.Drawing.Point(430, 9);
            this.labWordSummaryMsg.Name = "labWordSummaryMsg";
            this.labWordSummaryMsg.Size = new System.Drawing.Size(114, 20);
            this.labWordSummaryMsg.TabIndex = 23;
            this.labWordSummaryMsg.Text = "None Selected";
            // 
            // labWordSummaryLbl
            // 
            this.labWordSummaryLbl.AutoSize = true;
            this.labWordSummaryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labWordSummaryLbl.Location = new System.Drawing.Point(261, 9);
            this.labWordSummaryLbl.Name = "labWordSummaryLbl";
            this.labWordSummaryLbl.Size = new System.Drawing.Size(163, 20);
            this.labWordSummaryLbl.TabIndex = 22;
            this.labWordSummaryLbl.Text = "Summary of the word:";
            // 
            // pnlControl
            // 
            this.pnlControl.Controls.Add(this.lbAvailableWords);
            this.pnlControl.Controls.Add(this.labAvailableWords);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlControl.Location = new System.Drawing.Point(0, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(235, 475);
            this.pnlControl.TabIndex = 21;
            // 
            // lbAvailableWords
            // 
            this.lbAvailableWords.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAvailableWords.FormattingEnabled = true;
            this.lbAvailableWords.ItemHeight = 19;
            this.lbAvailableWords.Location = new System.Drawing.Point(28, 38);
            this.lbAvailableWords.Name = "lbAvailableWords";
            this.lbAvailableWords.Size = new System.Drawing.Size(192, 403);
            this.lbAvailableWords.Sorted = true;
            this.lbAvailableWords.TabIndex = 1;
            this.lbAvailableWords.SelectedIndexChanged += new System.EventHandler(this.lbAvailableWords_SelectedIndexChanged);
            // 
            // labAvailableWords
            // 
            this.labAvailableWords.AutoSize = true;
            this.labAvailableWords.Location = new System.Drawing.Point(13, 13);
            this.labAvailableWords.Name = "labAvailableWords";
            this.labAvailableWords.Size = new System.Drawing.Size(87, 13);
            this.labAvailableWords.TabIndex = 0;
            this.labAvailableWords.Text = "Available Words:";
            // 
            // tmrInitial
            // 
            this.tmrInitial.Interval = 500;
            this.tmrInitial.Tick += new System.EventHandler(this.tmrInitial_Tick);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHelp.Location = new System.Drawing.Point(632, 376);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 31;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.Location = new System.Drawing.Point(632, 405);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 32;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 475);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.gbOrder);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.labTextEnteredMsg);
            this.Controls.Add(this.labTextEnteredLbl);
            this.Controls.Add(this.labVirtualKeyboardLbl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlKeys);
            this.Controls.Add(this.dgvSummary);
            this.Controls.Add(this.labWordSummaryMsg);
            this.Controls.Add(this.labWordSummaryLbl);
            this.Controls.Add(this.pnlControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Testament Greek Principal Parts (with additional information from the Septuag" +
    "int)";
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.gbOrder.ResumeLayout(false);
            this.gbOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOrder;
        private System.Windows.Forms.RadioButton rbtnLeastUsedNT;
        private System.Windows.Forms.RadioButton rbtnMostUsedNT;
        private System.Windows.Forms.RadioButton rbtnAlpha;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Label labTextEnteredMsg;
        private System.Windows.Forms.Label labTextEnteredLbl;
        private System.Windows.Forms.Label labVirtualKeyboardLbl;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlKeys;
        private System.Windows.Forms.DataGridView dgvSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label labWordSummaryMsg;
        private System.Windows.Forms.Label labWordSummaryLbl;
        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.ListBox lbAvailableWords;
        private System.Windows.Forms.Label labAvailableWords;
        private System.Windows.Forms.Timer tmrInitial;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnAbout;
    }
}

