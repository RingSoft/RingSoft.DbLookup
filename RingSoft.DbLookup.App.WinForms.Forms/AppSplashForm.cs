using System;
using System.Windows.Forms;
using RingSoft.DbLookup.App.Library;

namespace RingSoft.DbLookup.App.WinForms.Forms
{
    public partial class AppSplashForm : Form, IAppSplashWindow
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

        public void CloseSplash()
        {
            Invoke(new Action(() => Close()));
        }
    }
}
