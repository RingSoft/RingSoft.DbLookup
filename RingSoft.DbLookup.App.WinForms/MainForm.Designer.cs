﻿namespace RingSoft.DbLookup.App.WinForms
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
            this.ExitButton = new System.Windows.Forms.Button();
            this.StockTrackerButton = new System.Windows.Forms.Button();
            this.MegaDbButton = new System.Windows.Forms.Button();
            this.NorthwindButton = new System.Windows.Forms.Button();
            this.DatabaseSettingsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(11, 150);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(290, 31);
            this.ExitButton.TabIndex = 4;
            this.ExitButton.Text = "&Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // StockTrackerButton
            // 
            this.StockTrackerButton.Location = new System.Drawing.Point(160, 81);
            this.StockTrackerButton.Name = "StockTrackerButton";
            this.StockTrackerButton.Size = new System.Drawing.Size(143, 63);
            this.StockTrackerButton.TabIndex = 3;
            this.StockTrackerButton.Text = "&Stock Tracker Demo";
            this.StockTrackerButton.UseVisualStyleBackColor = true;
            // 
            // MegaDbButton
            // 
            this.MegaDbButton.Location = new System.Drawing.Point(11, 81);
            this.MegaDbButton.Name = "MegaDbButton";
            this.MegaDbButton.Size = new System.Drawing.Size(143, 63);
            this.MegaDbButton.TabIndex = 2;
            this.MegaDbButton.Text = "&Mega Database Demo";
            this.MegaDbButton.UseVisualStyleBackColor = true;
            // 
            // NorthwindButton
            // 
            this.NorthwindButton.Location = new System.Drawing.Point(160, 12);
            this.NorthwindButton.Name = "NorthwindButton";
            this.NorthwindButton.Size = new System.Drawing.Size(143, 63);
            this.NorthwindButton.TabIndex = 1;
            this.NorthwindButton.Text = "&Northwind Demo";
            this.NorthwindButton.UseVisualStyleBackColor = true;
            // 
            // DatabaseSettingsButton
            // 
            this.DatabaseSettingsButton.Location = new System.Drawing.Point(11, 12);
            this.DatabaseSettingsButton.Name = "DatabaseSettingsButton";
            this.DatabaseSettingsButton.Size = new System.Drawing.Size(143, 63);
            this.DatabaseSettingsButton.TabIndex = 0;
            this.DatabaseSettingsButton.Text = "&Database Setup";
            this.DatabaseSettingsButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 193);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.StockTrackerButton);
            this.Controls.Add(this.MegaDbButton);
            this.Controls.Add(this.NorthwindButton);
            this.Controls.Add(this.DatabaseSettingsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MainForm";
            this.Text = "DB Lookup Demo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button StockTrackerButton;
        private System.Windows.Forms.Button MegaDbButton;
        private System.Windows.Forms.Button NorthwindButton;
        private System.Windows.Forms.Button DatabaseSettingsButton;
    }
}