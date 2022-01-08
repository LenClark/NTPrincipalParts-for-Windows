
namespace NTPrincipalParts
{
    partial class frmReferences
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReferences));
            this.pnlBase = new System.Windows.Forms.Panel();
            this.saveCSV = new System.Windows.Forms.Button();
            this.btnKeep = new System.Windows.Forms.Button();
            this.btnClosePrev = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvReferences = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.pnlBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferences)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.saveCSV);
            this.pnlBase.Controls.Add(this.btnKeep);
            this.pnlBase.Controls.Add(this.btnClosePrev);
            this.pnlBase.Controls.Add(this.btnClose);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 475);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(671, 30);
            this.pnlBase.TabIndex = 4;
            // 
            // saveCSV
            // 
            this.saveCSV.Location = new System.Drawing.Point(93, 4);
            this.saveCSV.Name = "saveCSV";
            this.saveCSV.Size = new System.Drawing.Size(80, 23);
            this.saveCSV.TabIndex = 3;
            this.saveCSV.Text = "Save as CSV";
            this.saveCSV.UseVisualStyleBackColor = true;
            this.saveCSV.Click += new System.EventHandler(this.saveCSV_Click);
            // 
            // btnKeep
            // 
            this.btnKeep.Location = new System.Drawing.Point(12, 4);
            this.btnKeep.Name = "btnKeep";
            this.btnKeep.Size = new System.Drawing.Size(75, 23);
            this.btnKeep.TabIndex = 2;
            this.btnKeep.Text = "Display";
            this.btnKeep.UseVisualStyleBackColor = true;
            this.btnKeep.Click += new System.EventHandler(this.displayButton_Click);
            // 
            // btnClosePrev
            // 
            this.btnClosePrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClosePrev.Location = new System.Drawing.Point(499, 4);
            this.btnClosePrev.Name = "btnClosePrev";
            this.btnClosePrev.Size = new System.Drawing.Size(79, 23);
            this.btnClosePrev.TabIndex = 1;
            this.btnClosePrev.Text = "Back to Base";
            this.btnClosePrev.UseVisualStyleBackColor = true;
            this.btnClosePrev.Click += new System.EventHandler(this.btnClosePrev_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(584, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvReferences
            // 
            this.dgvReferences.AllowUserToAddRows = false;
            this.dgvReferences.AllowUserToDeleteRows = false;
            this.dgvReferences.AllowUserToResizeColumns = false;
            this.dgvReferences.AllowUserToResizeRows = false;
            this.dgvReferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReferences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReferences.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvReferences.Location = new System.Drawing.Point(0, 0);
            this.dgvReferences.Name = "dgvReferences";
            this.dgvReferences.ReadOnly = true;
            this.dgvReferences.RowHeadersVisible = false;
            this.dgvReferences.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReferences.Size = new System.Drawing.Size(671, 475);
            this.dgvReferences.TabIndex = 5;
            this.dgvReferences.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvReferences_CellMouseDoubleClick);
            // 
            // Column1
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column1.HeaderText = "Book";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column2.HeaderText = "Chap";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 40;
            // 
            // Column3
            // 
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column3.DefaultCellStyle = dataGridViewCellStyle9;
            this.Column3.HeaderText = "Reference(s)";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "BookCode";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "VerseList";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Visible = false;
            // 
            // dlgSave
            // 
            this.dlgSave.DefaultExt = "csv";
            this.dlgSave.FileName = "NTPPartsRef.csv";
            this.dlgSave.Filter = "\"Tab delimited CSV\"|*.csv|\"Text file\"|*.txt|\"Doc file\"|*.doc|\"All files\"|*.*";
            this.dlgSave.Title = "Save references for the specific word as CSV";
            // 
            // frmReferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 505);
            this.Controls.Add(this.dgvReferences);
            this.Controls.Add(this.pnlBase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "References";
            this.pnlBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferences)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button saveCSV;
        private System.Windows.Forms.Button btnKeep;
        private System.Windows.Forms.Button btnClosePrev;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvReferences;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.SaveFileDialog dlgSave;
    }
}