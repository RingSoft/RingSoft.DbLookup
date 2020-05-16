using System;
using System.Drawing;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Controls.WinForms
{
    public partial class DataProcessResultForm : BaseForm
    {
        public DataProcessResultForm(DataProcessResult dataProcessResult)
        {
            InitializeComponent();

            CloseButton.Click += (sender, args) => Close();

            if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            {
                TitleLabel.Text = @"Data Process Success!";
                TitleLabel.BackColor = Color.Green;
                TitleLabel.ForeColor = Color.White;
                SqlStatementLabel.Text = @"Processed SQL Statement";
                ResultText.Text = $@"Debug Message:{Environment.NewLine}{Environment.NewLine}{dataProcessResult.DebugMessage}";
                SQLStatementText.Text = dataProcessResult.ProcessedSqlStatement;
            }
            else
            {
                TitleLabel.BackColor = Color.Red;
                TitleLabel.ForeColor = Color.Black;
                ResultText.Text =
                    $@"Debug Message:{Environment.NewLine}{dataProcessResult.DebugMessage}{Environment.NewLine}{Environment.NewLine}";
                ResultText.Text += $@"Error Message:{Environment.NewLine}{dataProcessResult.Message}";
                SQLStatementText.Text = dataProcessResult.ProcessedSqlStatement;
            }

            ActiveControl = SQLStatementText;
        }
    }
}
