namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    partial class ManufacturerForm
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
            this.AddModifyButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ManufacturerIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ItemsControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.NameControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.SuspendLayout();
            // 
            // AddModifyButton
            // 
            this.AddModifyButton.Location = new System.Drawing.Point(21, 119);
            this.AddModifyButton.Name = "AddModifyButton";
            this.AddModifyButton.Size = new System.Drawing.Size(75, 23);
            this.AddModifyButton.TabIndex = 5;
            this.AddModifyButton.Text = "&Add/Modify";
            this.AddModifyButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(99, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Items";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // ManufacturerIdLabel
            // 
            this.ManufacturerIdLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ManufacturerIdLabel.Location = new System.Drawing.Point(102, 68);
            this.ManufacturerIdLabel.Name = "ManufacturerIdLabel";
            this.ManufacturerIdLabel.Size = new System.Drawing.Size(100, 20);
            this.ManufacturerIdLabel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Manufacturer ID";
            // 
            // ItemsControl
            // 
            this.ItemsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemsControl.Command = null;
            this.ItemsControl.DataSourceChanged = null;
            this.ItemsControl.Location = new System.Drawing.Point(102, 119);
            this.ItemsControl.LookupDefinition = null;
            this.ItemsControl.Name = "ItemsControl";
            this.ItemsControl.SearchText = "";
            this.ItemsControl.Size = new System.Drawing.Size(414, 269);
            this.ItemsControl.TabIndex = 6;
            // 
            // NameControl
            // 
            this.NameControl.IsDirty = false;
            this.NameControl.Location = new System.Drawing.Point(249, 65);
            this.NameControl.Name = "NameControl";
            this.NameControl.Setup = null;
            this.NameControl.Size = new System.Drawing.Size(153, 20);
            this.NameControl.TabIndex = 3;
            this.NameControl.TabOutAfterLookupSelect = true;
            this.NameControl.Value = null;
            // 
            // ManufacturerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 400);
            this.Controls.Add(this.ItemsControl);
            this.Controls.Add(this.NameControl);
            this.Controls.Add(this.AddModifyButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ManufacturerIdLabel);
            this.Controls.Add(this.label1);
            this.Name = "ManufacturerForm";
            this.Text = "Manufacturers";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ManufacturerIdLabel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.AddModifyButton, 0);
            this.Controls.SetChildIndex(this.NameControl, 0);
            this.Controls.SetChildIndex(this.ItemsControl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddModifyButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ManufacturerIdLabel;
        private System.Windows.Forms.Label label1;
        private Controls.WinForms.LookupControl ItemsControl;
        private Controls.WinForms.AutoFillControl NameControl;
    }
}