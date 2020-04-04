namespace RingSoft.DbLookup.App.WinForms.Forms.MegaDb
{
    partial class ItemForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NameControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.LocationControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.ManufacturerControl = new RingSoft.DbLookup.Controls.WinForms.AutoFillControl();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Manufacturer";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Location";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(260, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ItemIdLabel.Location = new System.Drawing.Point(66, 68);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(100, 20);
            this.ItemIdLabel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item ID";
            // 
            // NameControl
            // 
            this.NameControl.IsDirty = false;
            this.NameControl.Location = new System.Drawing.Point(302, 69);
            this.NameControl.Name = "NameControl";
            this.NameControl.Setup = null;
            this.NameControl.Size = new System.Drawing.Size(149, 20);
            this.NameControl.TabIndex = 3;
            this.NameControl.TabOutAfterLookupSelect = true;
            this.NameControl.Value = null;
            // 
            // LocationControl
            // 
            this.LocationControl.IsDirty = false;
            this.LocationControl.Location = new System.Drawing.Point(66, 102);
            this.LocationControl.Name = "LocationControl";
            this.LocationControl.Setup = null;
            this.LocationControl.Size = new System.Drawing.Size(154, 20);
            this.LocationControl.TabIndex = 5;
            this.LocationControl.TabOutAfterLookupSelect = true;
            this.LocationControl.Value = null;
            // 
            // ManufacturerControl
            // 
            this.ManufacturerControl.IsDirty = false;
            this.ManufacturerControl.Location = new System.Drawing.Point(303, 102);
            this.ManufacturerControl.Name = "ManufacturerControl";
            this.ManufacturerControl.Setup = null;
            this.ManufacturerControl.Size = new System.Drawing.Size(148, 20);
            this.ManufacturerControl.TabIndex = 7;
            this.ManufacturerControl.TabOutAfterLookupSelect = true;
            this.ManufacturerControl.Value = null;
            // 
            // ItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 132);
            this.Controls.Add(this.ManufacturerControl);
            this.Controls.Add(this.LocationControl);
            this.Controls.Add(this.NameControl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.label1);
            this.Name = "ItemForm";
            this.Text = "Items";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ItemIdLabel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.NameControl, 0);
            this.Controls.SetChildIndex(this.LocationControl, 0);
            this.Controls.SetChildIndex(this.ManufacturerControl, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label label1;
        private Controls.WinForms.AutoFillControl NameControl;
        private Controls.WinForms.AutoFillControl LocationControl;
        private Controls.WinForms.AutoFillControl ManufacturerControl;
    }
}