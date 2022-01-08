
namespace NTPrincipalParts
{
    partial class frmChapter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChapter));
            this.pnlBase = new System.Windows.Forms.Panel();
            this.btnBackToBase = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.rtxtChapter = new System.Windows.Forms.RichTextBox();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnBackToBase);
            this.pnlBase.Controls.Add(this.btnClose);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Location = new System.Drawing.Point(0, 499);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(1020, 27);
            this.pnlBase.TabIndex = 2;
            // 
            // btnBackToBase
            // 
            this.btnBackToBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackToBase.Location = new System.Drawing.Point(841, 3);
            this.btnBackToBase.Name = "btnBackToBase";
            this.btnBackToBase.Size = new System.Drawing.Size(86, 23);
            this.btnBackToBase.TabIndex = 1;
            this.btnBackToBase.Text = "Back to Base";
            this.btnBackToBase.UseVisualStyleBackColor = true;
            this.btnBackToBase.Click += new System.EventHandler(this.btnBackToBase_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(933, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rtxtChapter
            // 
            this.rtxtChapter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtChapter.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtChapter.Location = new System.Drawing.Point(0, 0);
            this.rtxtChapter.Name = "rtxtChapter";
            this.rtxtChapter.Size = new System.Drawing.Size(1020, 499);
            this.rtxtChapter.TabIndex = 3;
            this.rtxtChapter.Text = "";
            // 
            // frmChapter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 526);
            this.Controls.Add(this.rtxtChapter);
            this.Controls.Add(this.pnlBase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmChapter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chapter";
            this.pnlBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBase;
        private System.Windows.Forms.Button btnBackToBase;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtxtChapter;
    }
}