namespace RingSoft.DbLookup.App.WinForms.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.ExitButton = new System.Windows.Forms.Button();
            this.StockTrackerButton = new System.Windows.Forms.Button();
            this.MegaDbButton = new System.Windows.Forms.Button();
            this.NorthwindButton = new System.Windows.Forms.Button();
            this.DatabaseSettingsButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(12, 184);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(325, 31);
            this.ExitButton.TabIndex = 4;
            this.ExitButton.Text = "&Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // StockTrackerButton
            // 
            this.StockTrackerButton.Location = new System.Drawing.Point(177, 98);
            this.StockTrackerButton.Name = "StockTrackerButton";
            this.StockTrackerButton.Size = new System.Drawing.Size(160, 80);
            this.StockTrackerButton.TabIndex = 3;
            this.StockTrackerButton.Text = "&Stock Tracker Demo";
            this.StockTrackerButton.UseVisualStyleBackColor = true;
            // 
            // MegaDbButton
            // 
            this.MegaDbButton.Location = new System.Drawing.Point(12, 98);
            this.MegaDbButton.Name = "MegaDbButton";
            this.MegaDbButton.Size = new System.Drawing.Size(160, 80);
            this.MegaDbButton.TabIndex = 2;
            this.MegaDbButton.Text = "&Mega Database Demo";
            this.MegaDbButton.UseVisualStyleBackColor = true;
            // 
            // NorthwindButton
            // 
            this.NorthwindButton.Location = new System.Drawing.Point(177, 12);
            this.NorthwindButton.Name = "NorthwindButton";
            this.NorthwindButton.Size = new System.Drawing.Size(160, 80);
            this.NorthwindButton.TabIndex = 1;
            this.NorthwindButton.Text = "&Northwind Demo";
            this.NorthwindButton.UseVisualStyleBackColor = true;
            // 
            // DatabaseSettingsButton
            // 
            this.DatabaseSettingsButton.Location = new System.Drawing.Point(11, 12);
            this.DatabaseSettingsButton.Name = "DatabaseSettingsButton";
            this.DatabaseSettingsButton.Size = new System.Drawing.Size(160, 80);
            this.DatabaseSettingsButton.TabIndex = 0;
            this.DatabaseSettingsButton.Text = "&Database Setup";
            this.DatabaseSettingsButton.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 227);
            this.ControlBox = false;
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.StockTrackerButton);
            this.Controls.Add(this.MegaDbButton);
            this.Controls.Add(this.NorthwindButton);
            this.Controls.Add(this.DatabaseSettingsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.ShowInTaskbar = true;
            this.Text = "RingSoft Database Lookup Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button StockTrackerButton;
        private System.Windows.Forms.Button MegaDbButton;
        private System.Windows.Forms.Button NorthwindButton;
        private System.Windows.Forms.Button DatabaseSettingsButton;
        private System.Windows.Forms.Timer timer1;
    }
}