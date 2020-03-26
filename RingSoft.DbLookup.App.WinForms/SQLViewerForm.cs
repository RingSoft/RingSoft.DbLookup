using RingSoft.DbLookup.GetDataProcessor;
using System;
using System.Drawing;

namespace RingSoft.DbLookup.App.WinForms
{
    public partial class SQLViewerForm : BaseForm
    {
        public SQLViewerForm(GetDataResult getDataResult)
        {
            InitializeComponent();

            CloseButton.Click += (sender, args) => Close();

            if (getDataResult.ResultCode == GetDataResultCodes.Success)
            {
                TitleLabel.Text = @"SQL Success!";
                TitleLabel.BackColor = Color.Green;
                TitleLabel.ForeColor = Color.White;
                SqlStatementLabel.Text = @"Processed SQL Statement";
                ErrorText.Text = $@"Debug Message:{Environment.NewLine}{Environment.NewLine}{getDataResult.DebugMessage}";
                SQLStatementText.Text = getDataResult.FailedSqlStatement;
            }
            else
            {
                TitleLabel.BackColor = Color.Red;
                TitleLabel.ForeColor = Color.Black;
                ErrorText.Text =
                    $@"Debug Message:{Environment.NewLine}{getDataResult.DebugMessage}{Environment.NewLine}{Environment.NewLine}";
                ErrorText.Text += $@"Error Message:{Environment.NewLine}{getDataResult.ErrorMessage}";
                SQLStatementText.Text = getDataResult.FailedSqlStatement;
            }

            ActiveControl = SQLStatementText;
        }
    }
}