namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    partial class StockCostQuantityForm
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
            this.LocationLabel = new System.Windows.Forms.Label();
            this.StockNumberLabel = new System.Windows.Forms.Label();
            this.CostTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.QuantityTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PurchaseDateControl = new System.Windows.Forms.DateTimePicker();
            this.PurchaseDateLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LocationLabel
            // 
            this.LocationLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LocationLabel.Location = new System.Drawing.Point(89, 94);
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new System.Drawing.Size(247, 20);
            this.LocationLabel.TabIndex = 3;
            this.LocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StockNumberLabel
            // 
            this.StockNumberLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StockNumberLabel.Location = new System.Drawing.Point(89, 68);
            this.StockNumberLabel.Name = "StockNumberLabel";
            this.StockNumberLabel.Size = new System.Drawing.Size(247, 20);
            this.StockNumberLabel.TabIndex = 1;
            this.StockNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CostTextBox
            // 
            this.CostTextBox.Location = new System.Drawing.Point(236, 146);
            this.CostTextBox.Name = "CostTextBox";
            this.CostTextBox.Size = new System.Drawing.Size(100, 20);
            this.CostTextBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(202, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Cost";
            // 
            // QuantityTextBox
            // 
            this.QuantityTextBox.Location = new System.Drawing.Point(93, 146);
            this.QuantityTextBox.Name = "QuantityTextBox";
            this.QuantityTextBox.Size = new System.Drawing.Size(100, 20);
            this.QuantityTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Quantity";
            // 
            // PurchaseDateControl
            // 
            this.PurchaseDateControl.CustomFormat = "MM/dd/yyyy";
            this.PurchaseDateControl.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.PurchaseDateControl.Location = new System.Drawing.Point(93, 120);
            this.PurchaseDateControl.Name = "PurchaseDateControl";
            this.PurchaseDateControl.Size = new System.Drawing.Size(86, 20);
            this.PurchaseDateControl.TabIndex = 5;
            this.PurchaseDateControl.Value = new System.DateTime(2019, 12, 30, 0, 0, 0, 0);
            // 
            // PurchaseDateLabel
            // 
            this.PurchaseDateLabel.Location = new System.Drawing.Point(9, 120);
            this.PurchaseDateLabel.Name = "PurchaseDateLabel";
            this.PurchaseDateLabel.Size = new System.Drawing.Size(78, 19);
            this.PurchaseDateLabel.TabIndex = 4;
            this.PurchaseDateLabel.Text = "Purchase Date";
            this.PurchaseDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(38, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Location";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Stock Number";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StockCostQuantityForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 178);
            this.Controls.Add(this.LocationLabel);
            this.Controls.Add(this.StockNumberLabel);
            this.Controls.Add(this.CostTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QuantityTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PurchaseDateControl);
            this.Controls.Add(this.PurchaseDateLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Name = "StockCostQuantityForm";
            this.Text = "Stock Purchases";
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.PurchaseDateLabel, 0);
            this.Controls.SetChildIndex(this.PurchaseDateControl, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.QuantityTextBox, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.CostTextBox, 0);
            this.Controls.SetChildIndex(this.StockNumberLabel, 0);
            this.Controls.SetChildIndex(this.LocationLabel, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LocationLabel;
        private System.Windows.Forms.Label StockNumberLabel;
        private System.Windows.Forms.TextBox CostTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox QuantityTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker PurchaseDateControl;
        private System.Windows.Forms.Label PurchaseDateLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}