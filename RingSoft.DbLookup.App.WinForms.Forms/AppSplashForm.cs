using System;
using System.Windows.Forms;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class AppSplashForm : Form
    {
        public AppSplashForm()
        {
            InitializeComponent();
        }

        public void SetProgress(string progressText)
        {
            if (ProgressLabel.InvokeRequired)
            {
                ProgressLabel.Invoke((Action)(() =>
                    ProgressLabel.Text = progressText));
            }
            else
            {
                ProgressLabel.Text = progressText;
            }
        }
    }
}
