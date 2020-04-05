namespace RingSoft.DbLookup.Controls.WinForms
{
    partial class AutoFillControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LookupButton = new System.Windows.Forms.Button();
            this.AutoFillText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LookupButton
            // 
            this.LookupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LookupButton.Image = global::RingSoft.DbLookup.Controls.WinForms.Properties.Resources.Search16;
            this.LookupButton.Location = new System.Drawing.Point(177, -3);
            this.LookupButton.Margin = new System.Windows.Forms.Padding(0);
            this.LookupButton.Name = "LookupButton";
            this.LookupButton.Size = new System.Drawing.Size(23, 25);
            this.LookupButton.TabIndex = 1;
            this.LookupButton.TabStop = false;
            this.LookupButton.UseVisualStyleBackColor = true;
            // 
            // AutoFillText
            // 
            this.AutoFillText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoFillText.Location = new System.Drawing.Point(0, 0);
            this.AutoFillText.Margin = new System.Windows.Forms.Padding(0);
            this.AutoFillText.Name = "AutoFillText";
            this.AutoFillText.Size = new System.Drawing.Size(177, 20);
            this.AutoFillText.TabIndex = 0;
            // 
            // AutoFillControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LookupButton);
            this.Controls.Add(this.AutoFillText);
            this.Name = "AutoFillControl";
            this.Size = new System.Drawing.Size(203, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LookupButton;
        private System.Windows.Forms.TextBox AutoFillText;
    }
}
