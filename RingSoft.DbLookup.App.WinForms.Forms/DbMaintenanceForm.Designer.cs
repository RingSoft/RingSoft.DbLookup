namespace RingSoft.DbLookup.App.WinForms.Forms
{
    partial class DbMaintenanceForm
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
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.CenterPanel = new System.Windows.Forms.Panel();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.NewButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.SelectButton = new System.Windows.Forms.Button();
            this.FindButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.ButtonsPanel.SuspendLayout();
            this.CenterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.CenterPanel);
            this.ButtonsPanel.Controls.Add(this.NextButton);
            this.ButtonsPanel.Controls.Add(this.PreviousButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(800, 65);
            this.ButtonsPanel.TabIndex = 2;
            // 
            // CenterPanel
            // 
            this.CenterPanel.Controls.Add(this.DeleteButton);
            this.CenterPanel.Controls.Add(this.SaveButton);
            this.CenterPanel.Controls.Add(this.NewButton);
            this.CenterPanel.Controls.Add(this.CloseButton);
            this.CenterPanel.Controls.Add(this.SelectButton);
            this.CenterPanel.Controls.Add(this.FindButton);
            this.CenterPanel.Location = new System.Drawing.Point(292, 3);
            this.CenterPanel.Name = "CenterPanel";
            this.CenterPanel.Size = new System.Drawing.Size(170, 60);
            this.CenterPanel.TabIndex = 3;
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(115, 0);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(50, 25);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.TabStop = false;
            this.DeleteButton.Text = "&Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(59, 0);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(50, 25);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.TabStop = false;
            this.SaveButton.Text = "&Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // NewButton
            // 
            this.NewButton.Location = new System.Drawing.Point(3, 0);
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(50, 25);
            this.NewButton.TabIndex = 0;
            this.NewButton.TabStop = false;
            this.NewButton.Text = "&New";
            this.NewButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(115, 31);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(50, 25);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.TabStop = false;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            // 
            // SelectButton
            // 
            this.SelectButton.Location = new System.Drawing.Point(59, 31);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(50, 25);
            this.SelectButton.TabIndex = 4;
            this.SelectButton.TabStop = false;
            this.SelectButton.Text = "Se&lect";
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // FindButton
            // 
            this.FindButton.Location = new System.Drawing.Point(3, 31);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(50, 25);
            this.FindButton.TabIndex = 3;
            this.FindButton.TabStop = false;
            this.FindButton.Text = "&Find";
            this.FindButton.UseVisualStyleBackColor = true;
            // 
            // NextButton
            // 
            this.NextButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.NextButton.Location = new System.Drawing.Point(750, 0);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(50, 65);
            this.NextButton.TabIndex = 1;
            this.NextButton.TabStop = false;
            this.NextButton.Text = "-->";
            this.NextButton.UseVisualStyleBackColor = true;
            // 
            // PreviousButton
            // 
            this.PreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.PreviousButton.Location = new System.Drawing.Point(0, 0);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(50, 65);
            this.PreviousButton.TabIndex = 0;
            this.PreviousButton.TabStop = false;
            this.PreviousButton.Text = "<--";
            this.PreviousButton.UseVisualStyleBackColor = true;
            // 
            // DbMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ButtonsPanel);
            this.Name = "DbMaintenanceForm";
            this.Text = "DbMaintenanceForm";
            this.ButtonsPanel.ResumeLayout(false);
            this.CenterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Panel CenterPanel;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PreviousButton;
    }
}