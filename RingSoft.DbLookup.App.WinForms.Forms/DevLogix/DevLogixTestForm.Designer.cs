namespace RingSoft.DbLookup.App.WinForms.Forms.DevLogix
{
    partial class DevLogixTestForm
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
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ErrorsLookupControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ReusableLookupControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.TestAutoFillControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.ChangeButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.IssuesLookupButton = new System.Windows.Forms.Button();
            this.TestLookupExceptionButton = new System.Windows.Forms.Button();
            this.TestAutoFillExceptionButton = new System.Windows.Forms.Button();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Location = new System.Drawing.Point(12, 12);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(775, 399);
            this.TabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ErrorsLookupControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Errors";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ErrorsLookupControl
            // 
            this.ErrorsLookupControl.Command = null;
            this.ErrorsLookupControl.DataSourceChanged = null;
            this.ErrorsLookupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsLookupControl.Location = new System.Drawing.Point(3, 3);
            this.ErrorsLookupControl.LookupDefinition = null;
            this.ErrorsLookupControl.Name = "ErrorsLookupControl";
            this.ErrorsLookupControl.SearchText = "";
            this.ErrorsLookupControl.Size = new System.Drawing.Size(761, 367);
            this.ErrorsLookupControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TestAutoFillExceptionButton);
            this.tabPage2.Controls.Add(this.TestLookupExceptionButton);
            this.tabPage2.Controls.Add(this.ReusableLookupControl);
            this.tabPage2.Controls.Add(this.TestAutoFillControl);
            this.tabPage2.Controls.Add(this.ChangeButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 373);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ReusableLookupControl
            // 
            this.ReusableLookupControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReusableLookupControl.Command = null;
            this.ReusableLookupControl.DataSourceChanged = null;
            this.ReusableLookupControl.Location = new System.Drawing.Point(7, 36);
            this.ReusableLookupControl.LookupDefinition = null;
            this.ReusableLookupControl.Name = "ReusableLookupControl";
            this.ReusableLookupControl.SearchText = "";
            this.ReusableLookupControl.Size = new System.Drawing.Size(754, 331);
            this.ReusableLookupControl.TabIndex = 4;
            // 
            // TestAutoFillControl
            // 
            this.TestAutoFillControl.IsDirty = false;
            this.TestAutoFillControl.Location = new System.Drawing.Point(6, 9);
            this.TestAutoFillControl.Name = "TestAutoFillControl";
            this.TestAutoFillControl.Setup = null;
            this.TestAutoFillControl.Size = new System.Drawing.Size(203, 20);
            this.TestAutoFillControl.TabIndex = 0;
            this.TestAutoFillControl.TabOutAfterLookupSelect = true;
            this.TestAutoFillControl.Value = null;
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(215, 6);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(75, 23);
            this.ChangeButton.TabIndex = 1;
            this.ChangeButton.Text = "C&hange";
            this.ChangeButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(681, 417);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(106, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // IssuesLookupButton
            // 
            this.IssuesLookupButton.Location = new System.Drawing.Point(569, 417);
            this.IssuesLookupButton.Name = "IssuesLookupButton";
            this.IssuesLookupButton.Size = new System.Drawing.Size(106, 23);
            this.IssuesLookupButton.TabIndex = 1;
            this.IssuesLookupButton.Text = "&Issues Lookup";
            this.IssuesLookupButton.UseVisualStyleBackColor = true;
            // 
            // TestLookupExceptionButton
            // 
            this.TestLookupExceptionButton.Location = new System.Drawing.Point(297, 9);
            this.TestLookupExceptionButton.Name = "TestLookupExceptionButton";
            this.TestLookupExceptionButton.Size = new System.Drawing.Size(130, 23);
            this.TestLookupExceptionButton.TabIndex = 2;
            this.TestLookupExceptionButton.Text = "Test Lookup Exception";
            this.TestLookupExceptionButton.UseVisualStyleBackColor = true;
            // 
            // TestAutoFillExceptionButton
            // 
            this.TestAutoFillExceptionButton.Location = new System.Drawing.Point(434, 9);
            this.TestAutoFillExceptionButton.Name = "TestAutoFillExceptionButton";
            this.TestAutoFillExceptionButton.Size = new System.Drawing.Size(136, 23);
            this.TestAutoFillExceptionButton.TabIndex = 3;
            this.TestAutoFillExceptionButton.Text = "Test AutoFill Exception";
            this.TestAutoFillExceptionButton.UseVisualStyleBackColor = true;
            // 
            // DevLogixTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.IssuesLookupButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DevLogixTestForm";
            this.Text = "Errors Lookup";
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button IssuesLookupButton;
        private Controls.WinForms.LookupControl ErrorsLookupControl;
        private Controls.WinForms.LookupControl ReusableLookupControl;
        private Controls.WinForms.AutoFillControl TestAutoFillControl;
        private System.Windows.Forms.Button TestLookupExceptionButton;
        private System.Windows.Forms.Button TestAutoFillExceptionButton;
    }
}