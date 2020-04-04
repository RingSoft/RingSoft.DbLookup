namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    partial class CustomerForm
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
            this.ContactPage = new System.Windows.Forms.TabPage();
            this.FaxTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PhoneTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CountryTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.PostalCodeTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.RegionTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CityTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.OrdersPage = new System.Windows.Forms.TabPage();
            this.AddModifyButton = new System.Windows.Forms.Button();
            this.ContactTitleTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ContactNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CompanyNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CustomerIdLabel = new System.Windows.Forms.Label();
            this.CustomerIdControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.OrdersControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.TabControl.SuspendLayout();
            this.ContactPage.SuspendLayout();
            this.OrdersPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.ContactPage);
            this.TabControl.Controls.Add(this.OrdersPage);
            this.TabControl.Location = new System.Drawing.Point(13, 123);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(422, 230);
            this.TabControl.TabIndex = 8;
            // 
            // ContactPage
            // 
            this.ContactPage.Controls.Add(this.FaxTextBox);
            this.ContactPage.Controls.Add(this.label5);
            this.ContactPage.Controls.Add(this.PhoneTextBox);
            this.ContactPage.Controls.Add(this.label6);
            this.ContactPage.Controls.Add(this.CountryTextBox);
            this.ContactPage.Controls.Add(this.label14);
            this.ContactPage.Controls.Add(this.PostalCodeTextBox);
            this.ContactPage.Controls.Add(this.label13);
            this.ContactPage.Controls.Add(this.RegionTextBox);
            this.ContactPage.Controls.Add(this.label12);
            this.ContactPage.Controls.Add(this.CityTextBox);
            this.ContactPage.Controls.Add(this.label11);
            this.ContactPage.Controls.Add(this.AddressTextBox);
            this.ContactPage.Controls.Add(this.label10);
            this.ContactPage.Location = new System.Drawing.Point(4, 22);
            this.ContactPage.Name = "ContactPage";
            this.ContactPage.Padding = new System.Windows.Forms.Padding(3);
            this.ContactPage.Size = new System.Drawing.Size(414, 204);
            this.ContactPage.TabIndex = 0;
            this.ContactPage.Text = "Contact Info";
            this.ContactPage.UseVisualStyleBackColor = true;
            // 
            // FaxTextBox
            // 
            this.FaxTextBox.Location = new System.Drawing.Point(286, 84);
            this.FaxTextBox.Name = "FaxTextBox";
            this.FaxTextBox.Size = new System.Drawing.Size(117, 20);
            this.FaxTextBox.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Fax Number";
            // 
            // PhoneTextBox
            // 
            this.PhoneTextBox.Location = new System.Drawing.Point(110, 84);
            this.PhoneTextBox.Name = "PhoneTextBox";
            this.PhoneTextBox.Size = new System.Drawing.Size(100, 20);
            this.PhoneTextBox.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Telephone Number";
            // 
            // CountryTextBox
            // 
            this.CountryTextBox.Location = new System.Drawing.Point(252, 58);
            this.CountryTextBox.Name = "CountryTextBox";
            this.CountryTextBox.Size = new System.Drawing.Size(151, 20);
            this.CountryTextBox.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(203, 61);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Country";
            // 
            // PostalCodeTextBox
            // 
            this.PostalCodeTextBox.Location = new System.Drawing.Point(110, 58);
            this.PostalCodeTextBox.Name = "PostalCodeTextBox";
            this.PostalCodeTextBox.Size = new System.Drawing.Size(87, 20);
            this.PostalCodeTextBox.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(40, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Postal Code";
            // 
            // RegionTextBox
            // 
            this.RegionTextBox.Location = new System.Drawing.Point(286, 32);
            this.RegionTextBox.Name = "RegionTextBox";
            this.RegionTextBox.Size = new System.Drawing.Size(117, 20);
            this.RegionTextBox.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(239, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Region";
            // 
            // CityTextBox
            // 
            this.CityTextBox.Location = new System.Drawing.Point(110, 32);
            this.CityTextBox.Name = "CityTextBox";
            this.CityTextBox.Size = new System.Drawing.Size(123, 20);
            this.CityTextBox.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(80, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "City";
            // 
            // AddressTextBox
            // 
            this.AddressTextBox.Location = new System.Drawing.Point(110, 6);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(293, 20);
            this.AddressTextBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(59, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(45, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Address";
            // 
            // OrdersPage
            // 
            this.OrdersPage.Controls.Add(this.OrdersControl);
            this.OrdersPage.Controls.Add(this.AddModifyButton);
            this.OrdersPage.Location = new System.Drawing.Point(4, 22);
            this.OrdersPage.Name = "OrdersPage";
            this.OrdersPage.Padding = new System.Windows.Forms.Padding(3);
            this.OrdersPage.Size = new System.Drawing.Size(414, 204);
            this.OrdersPage.TabIndex = 1;
            this.OrdersPage.Text = "Orders";
            this.OrdersPage.UseVisualStyleBackColor = true;
            // 
            // AddModifyButton
            // 
            this.AddModifyButton.Location = new System.Drawing.Point(6, 6);
            this.AddModifyButton.Name = "AddModifyButton";
            this.AddModifyButton.Size = new System.Drawing.Size(75, 23);
            this.AddModifyButton.TabIndex = 0;
            this.AddModifyButton.Text = "&Add/Modify";
            this.AddModifyButton.UseVisualStyleBackColor = true;
            // 
            // ContactTitleTextBox
            // 
            this.ContactTitleTextBox.Location = new System.Drawing.Point(84, 97);
            this.ContactTitleTextBox.Name = "ContactTitleTextBox";
            this.ContactTitleTextBox.Size = new System.Drawing.Size(125, 20);
            this.ContactTitleTextBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Contact Title";
            // 
            // ContactNameTextBox
            // 
            this.ContactNameTextBox.Location = new System.Drawing.Point(296, 97);
            this.ContactNameTextBox.Name = "ContactNameTextBox";
            this.ContactNameTextBox.Size = new System.Drawing.Size(135, 20);
            this.ContactNameTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(215, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Contact Name";
            // 
            // CompanyNameTextBox
            // 
            this.CompanyNameTextBox.Location = new System.Drawing.Point(296, 71);
            this.CompanyNameTextBox.Name = "CompanyNameTextBox";
            this.CompanyNameTextBox.Size = new System.Drawing.Size(135, 20);
            this.CompanyNameTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Company Name";
            // 
            // CustomerIdLabel
            // 
            this.CustomerIdLabel.AutoSize = true;
            this.CustomerIdLabel.Location = new System.Drawing.Point(13, 74);
            this.CustomerIdLabel.Name = "CustomerIdLabel";
            this.CustomerIdLabel.Size = new System.Drawing.Size(65, 13);
            this.CustomerIdLabel.TabIndex = 0;
            this.CustomerIdLabel.Text = "Customer ID";
            // 
            // CustomerIdControl
            // 
            this.CustomerIdControl.IsDirty = false;
            this.CustomerIdControl.Location = new System.Drawing.Point(85, 71);
            this.CustomerIdControl.Name = "CustomerIdControl";
            this.CustomerIdControl.Setup = null;
            this.CustomerIdControl.Size = new System.Drawing.Size(117, 20);
            this.CustomerIdControl.TabIndex = 1;
            this.CustomerIdControl.TabOutAfterLookupSelect = true;
            this.CustomerIdControl.Value = null;
            // 
            // OrdersControl
            // 
            this.OrdersControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OrdersControl.Command = null;
            this.OrdersControl.DataSourceChanged = null;
            this.OrdersControl.Location = new System.Drawing.Point(88, 7);
            this.OrdersControl.LookupDefinition = null;
            this.OrdersControl.Name = "OrdersControl";
            this.OrdersControl.SearchText = "";
            this.OrdersControl.Size = new System.Drawing.Size(320, 191);
            this.OrdersControl.TabIndex = 1;
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 361);
            this.Controls.Add(this.CustomerIdControl);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.ContactTitleTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ContactNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CompanyNameTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CustomerIdLabel);
            this.Name = "CustomerForm";
            this.Text = "Customers";
            this.Controls.SetChildIndex(this.CustomerIdLabel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.CompanyNameTextBox, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.ContactNameTextBox, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.ContactTitleTextBox, 0);
            this.Controls.SetChildIndex(this.TabControl, 0);
            this.Controls.SetChildIndex(this.CustomerIdControl, 0);
            this.TabControl.ResumeLayout(false);
            this.ContactPage.ResumeLayout(false);
            this.ContactPage.PerformLayout();
            this.OrdersPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage ContactPage;
        private System.Windows.Forms.TextBox FaxTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PhoneTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CountryTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox PostalCodeTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox RegionTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox CityTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage OrdersPage;
        private System.Windows.Forms.Button AddModifyButton;
        private System.Windows.Forms.TextBox ContactTitleTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ContactNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CompanyNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CustomerIdLabel;
        private Controls.WinForms.AutoFillControl CustomerIdControl;
        private Controls.WinForms.LookupControl OrdersControl;
    }
}