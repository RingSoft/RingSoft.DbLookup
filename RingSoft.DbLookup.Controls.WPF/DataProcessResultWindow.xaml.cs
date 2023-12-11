// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 12-19-2022
// ***********************************************************************
// <copyright file="DataProcessResultWindow.xaml.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Media;
using RingSoft.DbLookup.DataProcessor;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for DataProcessResultWindow.xaml
    /// </summary>
    public partial class DataProcessResultWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataProcessResultWindow"/> class.
        /// </summary>
        /// <param name="dataProcessResult">The data process result.</param>
        public DataProcessResultWindow(DataProcessResult dataProcessResult)
        {
            InitializeComponent();
            SqlStatementTextBox.GotFocus += (sender, args) => SqlStatementTextBox.SelectAll();

            if (dataProcessResult.ResultCode == GetDataResultCodes.Success)
            {
                TitleLabel.Content = @"Data Process Success!";
                TitleLabel.Background = new SolidColorBrush(Colors.Green);
                TitleLabel.Foreground = new SolidColorBrush(Colors.White);
                ResultTextBox.Text = $@"Debug Message:{Environment.NewLine}{Environment.NewLine}{dataProcessResult.DebugMessage}";
                SqlStatementTextBox.Text = dataProcessResult.ProcessedSqlStatement;
                DataGrid.ItemsSource = dataProcessResult.DataSet.Tables[0].DefaultView;
            }
            else
            {
                SqlTabItem.Header = "Failed SQL Statement";
                TitleLabel.Background = new SolidColorBrush(Colors.Red);
                TitleLabel.Foreground = new SolidColorBrush(Colors.Black);
                ResultTextBox.Text =
                    $@"Debug Message:{Environment.NewLine}{dataProcessResult.DebugMessage}{Environment.NewLine}{Environment.NewLine}";
                ResultTextBox.Text += $@"Error Message:{Environment.NewLine}{dataProcessResult.Message}";
                SqlStatementTextBox.Text = dataProcessResult.ProcessedSqlStatement;
                DataResultsTabItem.Visibility = Visibility.Collapsed;
            }

            Loaded += (sender, args) =>
            {
                SqlStatementTextBox.SelectionStart = 0;
                TabControl.SelectedItem = SqlTabItem;
                SqlTabItem.Focus();
            };

            CloseButton.Click += (sender, args) => Close();
        }
    }
}
