namespace RingSoft.DbLookup.App.WinForms
{
    partial class SQLViewerForm
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
            this.CloseButton = new System.Windows.Forms.Button();
            this.SQLStatementText = new System.Windows.Forms.TextBox();
            this.SqlStatementLabel = new System.Windows.Forms.Label();
            this.ErrorText = new System.Windows.Forms.TextBox();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(641, 483);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // SQLStatementText
            // 
            this.SQLStatementText.AcceptsReturn = true;
            this.SQLStatementText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLStatementText.Location = new System.Drawing.Point(13, 188);
            this.SQLStatementText.Multiline = true;
            this.SQLStatementText.Name = "SQLStatementText";
            this.SQLStatementText.ReadOnly = true;
            this.SQLStatementText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SQLStatementText.Size = new System.Drawing.Size(703, 289);
            this.SQLStatementText.TabIndex = 3;
            this.SQLStatementText.WordWrap = false;
            // 
            // SqlStatementLabel
            // 
            this.SqlStatementLabel.AutoSize = true;
            this.SqlStatementLabel.Location = new System.Drawing.Point(16, 171);
            this.SqlStatementLabel.Name = "SqlStatementLabel";
            this.SqlStatementLabel.Size = new System.Drawing.Size(113, 13);
            this.SqlStatementLabel.TabIndex = 2;
            this.SqlStatementLabel.Text = "Failed SQL Statement:";
            // 
            // ErrorText
            // 
            this.ErrorText.AcceptsReturn = true;
            this.ErrorText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorText.Location = new System.Drawing.Point(13, 35);
            this.ErrorText.Multiline = true;
            this.ErrorText.Name = "ErrorText";
            this.ErrorText.ReadOnly = true;
            this.ErrorText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ErrorText.Size = new System.Drawing.Size(703, 129);
            this.ErrorText.TabIndex = 1;
            this.ErrorText.WordWrap = false;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(12, 11);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(94, 20);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "SQL Error!";
            // 
            // ErrorViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 516);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.SQLStatementText);
            this.Controls.Add(this.SqlStatementLabel);
            this.Controls.Add(this.ErrorText);
            this.Controls.Add(this.TitleLabel);
            this.Name = "ErrorViewerForm";
            this.Text = "SQL Viewer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TextBox SQLStatementText;
        private System.Windows.Forms.Label SqlStatementLabel;
        private System.Windows.Forms.TextBox ErrorText;
        private System.Windows.Forms.Label TitleLabel;
    }
}