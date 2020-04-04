namespace RingSoft.DbLookup.App.WinForms.Forms.Northwind
{
    partial class EmployeeForm
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
            this.ExtensionTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.HomePhoneTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.CountryTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.PostalCodeTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.RegionTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CityTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.AddressTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.OrdersPage = new System.Windows.Forms.TabPage();
            this.AddModifyButton = new System.Windows.Forms.Button();
            this.HireDateControl = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.BirthDateControl = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.CourtesyTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TitleTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LastNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EmployeeIdLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FirstNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SupervisorControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
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
            this.TabControl.Location = new System.Drawing.Point(16, 175);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(518, 230);
            this.TabControl.TabIndex = 16;
            // 
            // ContactPage
            // 
            this.ContactPage.Controls.Add(this.ExtensionTextBox);
            this.ContactPage.Controls.Add(this.label9);
            this.ContactPage.Controls.Add(this.HomePhoneTextBox);
            this.ContactPage.Controls.Add(this.label10);
            this.ContactPage.Controls.Add(this.CountryTextBox);
            this.ContactPage.Controls.Add(this.label14);
            this.ContactPage.Controls.Add(this.PostalCodeTextBox);
            this.ContactPage.Controls.Add(this.label13);
            this.ContactPage.Controls.Add(this.RegionTextBox);
            this.ContactPage.Controls.Add(this.label12);
            this.ContactPage.Controls.Add(this.CityTextBox);
            this.ContactPage.Controls.Add(this.label11);
            this.ContactPage.Controls.Add(this.AddressTextBox);
            this.ContactPage.Controls.Add(this.label15);
            this.ContactPage.Location = new System.Drawing.Point(4, 22);
            this.ContactPage.Name = "ContactPage";
            this.ContactPage.Padding = new System.Windows.Forms.Padding(3);
            this.ContactPage.Size = new System.Drawing.Size(510, 204);
            this.ContactPage.TabIndex = 0;
            this.ContactPage.Text = "Contact Info";
            this.ContactPage.UseVisualStyleBackColor = true;
            // 
            // ExtensionTextBox
            // 
            this.ExtensionTextBox.Location = new System.Drawing.Point(297, 84);
            this.ExtensionTextBox.Name = "ExtensionTextBox";
            this.ExtensionTextBox.Size = new System.Drawing.Size(54, 20);
            this.ExtensionTextBox.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(238, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Extension";
            // 
            // HomePhoneTextBox
            // 
            this.HomePhoneTextBox.Location = new System.Drawing.Point(121, 84);
            this.HomePhoneTextBox.Name = "HomePhoneTextBox";
            this.HomePhoneTextBox.Size = new System.Drawing.Size(100, 20);
            this.HomePhoneTextBox.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Home Phone Number";
            // 
            // CountryTextBox
            // 
            this.CountryTextBox.Location = new System.Drawing.Point(263, 58);
            this.CountryTextBox.Name = "CountryTextBox";
            this.CountryTextBox.Size = new System.Drawing.Size(151, 20);
            this.CountryTextBox.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(214, 61);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Country";
            // 
            // PostalCodeTextBox
            // 
            this.PostalCodeTextBox.Location = new System.Drawing.Point(121, 58);
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
            this.RegionTextBox.Location = new System.Drawing.Point(297, 32);
            this.RegionTextBox.Name = "RegionTextBox";
            this.RegionTextBox.Size = new System.Drawing.Size(117, 20);
            this.RegionTextBox.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(250, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Region";
            // 
            // CityTextBox
            // 
            this.CityTextBox.Location = new System.Drawing.Point(121, 32);
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
            this.AddressTextBox.Location = new System.Drawing.Point(121, 6);
            this.AddressTextBox.Name = "AddressTextBox";
            this.AddressTextBox.Size = new System.Drawing.Size(293, 20);
            this.AddressTextBox.TabIndex = 1;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(59, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Address";
            // 
            // OrdersPage
            // 
            this.OrdersPage.Controls.Add(this.OrdersControl);
            this.OrdersPage.Controls.Add(this.AddModifyButton);
            this.OrdersPage.Location = new System.Drawing.Point(4, 22);
            this.OrdersPage.Name = "OrdersPage";
            this.OrdersPage.Padding = new System.Windows.Forms.Padding(3);
            this.OrdersPage.Size = new System.Drawing.Size(510, 204);
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
            // HireDateControl
            // 
            this.HireDateControl.CustomFormat = "MM/dd/yyyy";
            this.HireDateControl.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.HireDateControl.Location = new System.Drawing.Point(380, 123);
            this.HireDateControl.Name = "HireDateControl";
            this.HireDateControl.Size = new System.Drawing.Size(86, 20);
            this.HireDateControl.TabIndex = 11;
            this.HireDateControl.Value = new System.DateTime(2019, 12, 30, 0, 0, 0, 0);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(319, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 19);
            this.label7.TabIndex = 10;
            this.label7.Text = "Hire Date";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BirthDateControl
            // 
            this.BirthDateControl.CustomFormat = "MM/dd/yyyy";
            this.BirthDateControl.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.BirthDateControl.Location = new System.Drawing.Point(227, 123);
            this.BirthDateControl.Name = "BirthDateControl";
            this.BirthDateControl.Size = new System.Drawing.Size(86, 20);
            this.BirthDateControl.TabIndex = 9;
            this.BirthDateControl.Value = new System.DateTime(2019, 12, 30, 0, 0, 0, 0);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(164, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 19);
            this.label8.TabIndex = 8;
            this.label8.Text = "Birth Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CourtesyTextBox
            // 
            this.CourtesyTextBox.Location = new System.Drawing.Point(85, 123);
            this.CourtesyTextBox.Name = "CourtesyTextBox";
            this.CourtesyTextBox.Size = new System.Drawing.Size(73, 20);
            this.CourtesyTextBox.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Courtesy";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(272, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Supervisor";
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.Location = new System.Drawing.Point(85, 149);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(180, 20);
            this.TitleTextBox.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Title";
            // 
            // LastNameTextBox
            // 
            this.LastNameTextBox.Location = new System.Drawing.Point(335, 97);
            this.LastNameTextBox.Name = "LastNameTextBox";
            this.LastNameTextBox.Size = new System.Drawing.Size(203, 20);
            this.LastNameTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Last Name";
            // 
            // EmployeeIdLabel
            // 
            this.EmployeeIdLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.EmployeeIdLabel.Location = new System.Drawing.Point(85, 71);
            this.EmployeeIdLabel.Margin = new System.Windows.Forms.Padding(3);
            this.EmployeeIdLabel.Name = "EmployeeIdLabel";
            this.EmployeeIdLabel.Size = new System.Drawing.Size(100, 20);
            this.EmployeeIdLabel.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Employee ID";
            // 
            // FirstNameTextBox
            // 
            this.FirstNameTextBox.Location = new System.Drawing.Point(85, 97);
            this.FirstNameTextBox.Name = "FirstNameTextBox";
            this.FirstNameTextBox.Size = new System.Drawing.Size(180, 20);
            this.FirstNameTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "First Name";
            // 
            // SupervisorControl
            // 
            this.SupervisorControl.IsDirty = false;
            this.SupervisorControl.Location = new System.Drawing.Point(335, 149);
            this.SupervisorControl.Name = "SupervisorControl";
            this.SupervisorControl.Setup = null;
            this.SupervisorControl.Size = new System.Drawing.Size(203, 20);
            this.SupervisorControl.TabIndex = 15;
            this.SupervisorControl.TabOutAfterLookupSelect = true;
            this.SupervisorControl.Value = null;
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
            this.OrdersControl.Size = new System.Drawing.Size(416, 191);
            this.OrdersControl.TabIndex = 1;
            // 
            // EmployeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 414);
            this.Controls.Add(this.SupervisorControl);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.HireDateControl);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.BirthDateControl);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CourtesyTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TitleTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LastNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EmployeeIdLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FirstNameTextBox);
            this.Controls.Add(this.label1);
            this.Name = "EmployeeForm";
            this.Text = "Employee";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.FirstNameTextBox, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.EmployeeIdLabel, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.LastNameTextBox, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.TitleTextBox, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.CourtesyTextBox, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.BirthDateControl, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.HireDateControl, 0);
            this.Controls.SetChildIndex(this.TabControl, 0);
            this.Controls.SetChildIndex(this.SupervisorControl, 0);
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
        private System.Windows.Forms.TextBox ExtensionTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox HomePhoneTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox CountryTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox PostalCodeTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox RegionTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox CityTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox AddressTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage OrdersPage;
        private System.Windows.Forms.Button AddModifyButton;
        private System.Windows.Forms.DateTimePicker HireDateControl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker BirthDateControl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox CourtesyTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TitleTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox LastNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label EmployeeIdLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FirstNameTextBox;
        private System.Windows.Forms.Label label1;
        private Controls.WinForms.LookupControl OrdersControl;
        private Controls.WinForms.AutoFillControl SupervisorControl;
    }
}