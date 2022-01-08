
namespace NTPrincipalParts
{
    partial class frmInitialisation
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
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.labProgressMsg = new System.Windows.Forms.Label();
            this.labProgressLbl = new System.Windows.Forms.Label();
            this.labPatience = new System.Windows.Forms.Label();
            this.labExplanation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(17, 134);
            this.pbProgress.Maximum = 64;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(568, 5);
            this.pbProgress.Step = 1;
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbProgress.TabIndex = 9;
            // 
            // labProgressMsg
            // 
            this.labProgressMsg.AutoSize = true;
            this.labProgressMsg.Location = new System.Drawing.Point(123, 159);
            this.labProgressMsg.Name = "labProgressMsg";
            this.labProgressMsg.Size = new System.Drawing.Size(45, 13);
            this.labProgressMsg.TabIndex = 8;
            this.labProgressMsg.Text = "Inactive";
            // 
            // labProgressLbl
            // 
            this.labProgressLbl.AutoSize = true;
            this.labProgressLbl.Location = new System.Drawing.Point(12, 159);
            this.labProgressLbl.Name = "labProgressLbl";
            this.labProgressLbl.Size = new System.Drawing.Size(105, 13);
            this.labProgressLbl.TabIndex = 7;
            this.labProgressLbl.Text = "Currently processing:";
            // 
            // labPatience
            // 
            this.labPatience.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labPatience.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPatience.ForeColor = System.Drawing.Color.Red;
            this.labPatience.Location = new System.Drawing.Point(12, 95);
            this.labPatience.Name = "labPatience";
            this.labPatience.Size = new System.Drawing.Size(585, 23);
            this.labPatience.TabIndex = 6;
            this.labPatience.Text = "Please be patient";
            this.labPatience.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labExplanation
            // 
            this.labExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labExplanation.Location = new System.Drawing.Point(12, 40);
            this.labExplanation.Name = "labExplanation";
            this.labExplanation.Size = new System.Drawing.Size(585, 23);
            this.labExplanation.TabIndex = 5;
            this.labExplanation.Text = "The Application is loading text information and performing other initialisation t" +
    "asks.";
            this.labExplanation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmInitialisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 201);
            this.ControlBox = false;
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.labProgressMsg);
            this.Controls.Add(this.labProgressLbl);
            this.Controls.Add(this.labPatience);
            this.Controls.Add(this.labExplanation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmInitialisation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NT Principal Parts - Application Initialisation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label labProgressMsg;
        private System.Windows.Forms.Label labProgressLbl;
        private System.Windows.Forms.Label labPatience;
        private System.Windows.Forms.Label labExplanation;
    }
}