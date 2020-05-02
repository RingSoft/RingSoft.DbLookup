namespace RingSoft.DbLookup.Controls.WinForms
{
    partial class LookupControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RecordCountLabel = new System.Windows.Forms.Label();
            this.GetRecordCountButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ContainsRadioButton = new System.Windows.Forms.RadioButton();
            this.EqualsRadioButton = new System.Windows.Forms.RadioButton();
            this.ScrollBar = new System.Windows.Forms.VScrollBar();
            this.SearchForTextBox = new System.Windows.Forms.TextBox();
            this.SearchForLabel = new System.Windows.Forms.Label();
            this.RecordCountTimer = new System.Windows.Forms.Timer(this.components);
            this.LookupListView = new LookupListView();
            this.SuspendLayout();
            // 
            // RecordCountLabel
            // 
            this.RecordCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RecordCountLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RecordCountLabel.Location = new System.Drawing.Point(105, 271);
            this.RecordCountLabel.Name = "RecordCountLabel";
            this.RecordCountLabel.Size = new System.Drawing.Size(150, 23);
            this.RecordCountLabel.TabIndex = 7;
            this.RecordCountLabel.Text = "1,000,000 Records Found";
            this.RecordCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RecordCountLabel.Visible = false;
            // 
            // GetRecordCountButton
            // 
            this.GetRecordCountButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GetRecordCountButton.Location = new System.Drawing.Point(261, 271);
            this.GetRecordCountButton.Name = "GetRecordCountButton";
            this.GetRecordCountButton.Size = new System.Drawing.Size(150, 23);
            this.GetRecordCountButton.TabIndex = 8;
            this.GetRecordCountButton.TabStop = false;
            this.GetRecordCountButton.Text = "&Get Record Count";
            this.GetRecordCountButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Click on a column header to change the sort and search criteria.";
            // 
            // ContainsRadioButton
            // 
            this.ContainsRadioButton.AutoSize = true;
            this.ContainsRadioButton.Location = new System.Drawing.Point(136, 2);
            this.ContainsRadioButton.Name = "ContainsRadioButton";
            this.ContainsRadioButton.Size = new System.Drawing.Size(66, 17);
            this.ContainsRadioButton.TabIndex = 2;
            this.ContainsRadioButton.Text = "Contains";
            this.ContainsRadioButton.UseVisualStyleBackColor = true;
            // 
            // EqualsRadioButton
            // 
            this.EqualsRadioButton.AutoSize = true;
            this.EqualsRadioButton.Checked = true;
            this.EqualsRadioButton.Location = new System.Drawing.Point(72, 2);
            this.EqualsRadioButton.Name = "EqualsRadioButton";
            this.EqualsRadioButton.Size = new System.Drawing.Size(57, 17);
            this.EqualsRadioButton.TabIndex = 1;
            this.EqualsRadioButton.TabStop = true;
            this.EqualsRadioButton.Text = "Equals";
            this.EqualsRadioButton.UseVisualStyleBackColor = true;
            // 
            // ScrollBar
            // 
            this.ScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollBar.Location = new System.Drawing.Point(397, 40);
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.Size = new System.Drawing.Size(17, 228);
            this.ScrollBar.TabIndex = 6;
            // 
            // SearchForTextBox
            // 
            this.SearchForTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchForTextBox.Location = new System.Drawing.Point(208, 3);
            this.SearchForTextBox.Name = "SearchForTextBox";
            this.SearchForTextBox.Size = new System.Drawing.Size(205, 20);
            this.SearchForTextBox.TabIndex = 3;
            // 
            // SearchForLabel
            // 
            this.SearchForLabel.AutoSize = true;
            this.SearchForLabel.Location = new System.Drawing.Point(7, 3);
            this.SearchForLabel.Name = "SearchForLabel";
            this.SearchForLabel.Size = new System.Drawing.Size(59, 13);
            this.SearchForLabel.TabIndex = 0;
            this.SearchForLabel.Text = "Search For";
            // 
            // RecordCountTimer
            // 
            this.RecordCountTimer.Interval = 300;
            // 
            // LookupListView
            // 
            this.LookupListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LookupListView.FullRowSelect = true;
            this.LookupListView.HideSelection = false;
            this.LookupListView.Location = new System.Drawing.Point(0, 44);
            this.LookupListView.MultiSelect = false;
            this.LookupListView.Name = "LookupListView";
            this.LookupListView.Size = new System.Drawing.Size(396, 224);
            this.LookupListView.TabIndex = 5;
            this.LookupListView.TabStop = false;
            this.LookupListView.UseCompatibleStateImageBehavior = false;
            this.LookupListView.View = System.Windows.Forms.View.Details;
            // 
            // LookupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LookupListView);
            this.Controls.Add(this.RecordCountLabel);
            this.Controls.Add(this.GetRecordCountButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContainsRadioButton);
            this.Controls.Add(this.EqualsRadioButton);
            this.Controls.Add(this.ScrollBar);
            this.Controls.Add(this.SearchForTextBox);
            this.Controls.Add(this.SearchForLabel);
            this.Name = "LookupControl";
            this.Size = new System.Drawing.Size(413, 297);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RecordCountLabel;
        private System.Windows.Forms.Button GetRecordCountButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton ContainsRadioButton;
        private System.Windows.Forms.RadioButton EqualsRadioButton;
        private System.Windows.Forms.VScrollBar ScrollBar;
        private System.Windows.Forms.TextBox SearchForTextBox;
        private System.Windows.Forms.Label SearchForLabel;
        private System.Windows.Forms.Timer RecordCountTimer;
        private LookupListView LookupListView;
    }
}
