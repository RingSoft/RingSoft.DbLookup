namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    partial class StockMasterForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.PriceTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LocationLabel = new System.Windows.Forms.Label();
            this.StockNumberLabel = new System.Windows.Forms.Label();
            this.StockNumberControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.LocationControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.CostQuantityLookupControl = new RingSoft.DbLookup.Controls.WinForms.LookupControl();
            this.SuspendLayout();
            // 
            // AddModifyButton
            // 
            this.AddModifyButton.Location = new System.Drawing.Point(15, 144);
            this.AddModifyButton.Name = "AddModifyButton";
            this.AddModifyButton.Size = new System.Drawing.Size(75, 23);
            this.AddModifyButton.TabIndex = 7;
            this.AddModifyButton.Text = "&Add/Modify";
            this.AddModifyButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Purchases";
            // 
            // PriceTextBox
            // 
            this.PriceTextBox.Location = new System.Drawing.Point(93, 97);
            this.PriceTextBox.Name = "PriceTextBox";
            this.PriceTextBox.Size = new System.Drawing.Size(100, 20);
            this.PriceTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Price";
            // 
            // LocationLabel
            // 
            this.LocationLabel.Location = new System.Drawing.Point(303, 71);
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(49, 20);
            this.LocationLabel.TabIndex = 2;
            this.LocationLabel.Text = "Location";
            this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StockNumberLabel
            // 
            this.StockNumberLabel.Location = new System.Drawing.Point(12, 71);
            this.StockNumberLabel.Name = "StockNumberLabel";
            this.StockNumberLabel.Size = new System.Drawing.Size(75, 20);
            this.StockNumberLabel.TabIndex = 0;
            this.StockNumberLabel.Text = "Stock Number";
            this.StockNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StockNumberControl
            // 
            this.StockNumberControl.IsDirty = false;
            this.StockNumberControl.Location = new System.Drawing.Point(93, 71);
            this.StockNumberControl.Name = "StockNumberControl";
            this.StockNumberControl.Setup = null;
            this.StockNumberControl.Size = new System.Drawing.Size(204, 20);
            this.StockNumberControl.TabIndex = 1;
            this.StockNumberControl.TabOutAfterLookupSelect = true;
            this.StockNumberControl.Value = null;
            // 
            // LocationControl
            // 
            this.LocationControl.IsDirty = false;
            this.LocationControl.Location = new System.Drawing.Point(359, 71);
            this.LocationControl.Name = "LocationControl";
            this.LocationControl.Setup = null;
            this.LocationControl.Size = new System.Drawing.Size(194, 20);
            this.LocationControl.TabIndex = 3;
            this.LocationControl.TabOutAfterLookupSelect = true;
            this.LocationControl.Value = null;
            // 
            // CostQuantityLookupControl
            // 
            this.CostQuantityLookupControl.Command = null;
            this.CostQuantityLookupControl.DataSourceChanged = null;
            this.CostQuantityLookupControl.Location = new System.Drawing.Point(93, 144);
            this.CostQuantityLookupControl.LookupDefinition = null;
            this.CostQuantityLookupControl.Name = "CostQuantityLookupControl";
            this.CostQuantityLookupControl.SearchText = "";
            this.CostQuantityLookupControl.Size = new System.Drawing.Size(460, 201);
            this.CostQuantityLookupControl.TabIndex = 8;
            // 
            // StockMasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 357);
            this.Controls.Add(this.CostQuantityLookupControl);
            this.Controls.Add(this.LocationControl);
            this.Controls.Add(this.StockNumberControl);
            this.Controls.Add(this.AddModifyButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PriceTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.StockNumberLabel);
            this.Name = "StockMasterForm";
            this.Text = "Stock Master";
            this.Controls.SetChildIndex(this.StockNumberLabel, 0);
            this.Controls.SetChildIndex(this.LocationLabel, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.PriceTextBox, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.AddModifyButton, 0);
            this.Controls.SetChildIndex(this.StockNumberControl, 0);
            this.Controls.SetChildIndex(this.LocationControl, 0);
            this.Controls.SetChildIndex(this.CostQuantityLookupControl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddModifyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PriceTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LocationLabel;
        private System.Windows.Forms.Label StockNumberLabel;
        private Controls.WinForms.AutoFillControl StockNumberControl;
        private Controls.WinForms.AutoFillControl LocationControl;
        private Controls.WinForms.LookupControl CostQuantityLookupControl;
    }
}