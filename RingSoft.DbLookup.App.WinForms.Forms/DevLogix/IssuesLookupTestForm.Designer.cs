namespace RingSoft.DbLookup.App.WinForms.Forms.DevLogix
{
    partial class IssuesLookupTestForm
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
            this.IssuesLookupControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // IssuesLookupControl
            // 
            this.IssuesLookupControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IssuesLookupControl.Command = null;
            this.IssuesLookupControl.DataSourceChanged = null;
            this.IssuesLookupControl.Location = new System.Drawing.Point(13, 13);
            this.IssuesLookupControl.LookupDefinition = null;
            this.IssuesLookupControl.Name = "IssuesLookupControl";
            this.IssuesLookupControl.SearchText = "";
            this.IssuesLookupControl.Size = new System.Drawing.Size(775, 348);
            this.IssuesLookupControl.TabIndex = 0;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(713, 367);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // IssuesLookupTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 402);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.IssuesLookupControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "IssuesLookupTestForm";
            this.Text = "Issues Lookup";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.WinForms.LookupControl IssuesLookupControl;
        private System.Windows.Forms.Button CloseButton;
    }
}