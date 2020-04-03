using RingSoft.DbLookup.GetDataProcessor;
using System;
using System.Windows.Media;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for DataProcessResultWindow.xaml
    /// </summary>
    public partial class DataProcessResultWindow
    {
        protected override bool SetFocusToFirstControl => false;

        public DataProcessResultWindow(DataProcessResult dataProcessResult)
        {
            InitializeComponent();

            if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            {
                TitleLabel.Content = @"Data Process Success!";
                TitleLabel.Background = new SolidColorBrush(Colors.Green);
                TitleLabel.Foreground = new SolidColorBrush(Colors.White);
                SqlStatementLabel.Content = @"Processed SQL Statement";
                ResultTextBox.Text = $@"Debug Message:{Environment.NewLine}{Environment.NewLine}{dataProcessResult.DebugMessage}";
                SqlStatementTextBox.Text = dataProcessResult.ProcessedSqlStatement;
            }
            else
            {
                TitleLabel.Background = new SolidColorBrush(Colors.Red);
                TitleLabel.Foreground = new SolidColorBrush(Colors.Black);
                ResultTextBox.Text =
                    $@"Debug Message:{Environment.NewLine}{dataProcessResult.DebugMessage}{Environment.NewLine}{Environment.NewLine}";
                ResultTextBox.Text += $@"Error Message:{Environment.NewLine}{dataProcessResult.Message}";
                SqlStatementTextBox.Text = dataProcessResult.ProcessedSqlStatement;
            }

            Loaded += (sender, args) => SqlStatementTextBox.Focus();

            CloseButton.Click += (sender, args) => Close();
        }
    }
}
