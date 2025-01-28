// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 05-24-2023
//
// Last Modified By : petem
// Last Modified On : 07-09-2023
// ***********************************************************************
// <copyright file="DbMaintenanceStatusBar.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Class DbMaintenanceStatusBar.
    /// Implements the <see cref="Control" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <font color="red">Badly formed XML comment.</font>
    public class DbMaintenanceStatusBar : Control
    {
        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }
        /// <summary>
        /// Gets or sets the date read only box.
        /// </summary>
        /// <value>The date read only box.</value>
        public DateReadOnlyBox DateReadOnlyBox { get; set; }
        /// <summary>
        /// Gets or sets the status text box.
        /// </summary>
        /// <value>The status text box.</value>
        public StringReadOnlyBox StatusTextBox { get; set; }
        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public DbMaintenanceStatusBarViewModel ViewModel { get; private set; }

        /// <summary>
        /// The last saved date property
        /// </summary>
        public static readonly DependencyProperty LastSavedDateProperty =
            DependencyProperty.Register("LastSavedDate", typeof(DateTime?), typeof(DbMaintenanceStatusBar),
                new FrameworkPropertyMetadata(DateChangedCallback));

        /// <summary>
        /// Gets or sets the last saved date.
        /// </summary>
        /// <value>The last saved date.</value>
        public DateTime? LastSavedDate
        {
            get { return (DateTime?)GetValue(LastSavedDateProperty); }
            set { SetValue(LastSavedDateProperty, value); }
        }

        /// <summary>
        /// Dates the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void DateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var statusBarControl = (DbMaintenanceStatusBar)obj;
            statusBarControl.ViewModel.LastSavedDate = statusBarControl.LastSavedDate;
        }

        /// <summary>
        /// The is active
        /// </summary>
        private bool _isActive = true;

        /// <summary>
        /// Initializes static members of the <see cref="DbMaintenanceStatusBar"/> class.
        /// </summary>
        static DbMaintenanceStatusBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceStatusBar), new FrameworkPropertyMetadata(typeof(DbMaintenanceStatusBar)));

            IsTabStopProperty.OverrideMetadata(typeof(DbMaintenanceStatusBar), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMaintenanceStatusBar"/> class.
        /// </summary>
        public DbMaintenanceStatusBar()
        {
            Loaded += (sender, args) =>
            {
                var window = Window.GetWindow(this);
                window.Closing += (sender, args) =>
                {
                    _isActive = false;
                };
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as DbMaintenanceStatusBarViewModel;

            DateReadOnlyBox = GetTemplateChild(nameof(DateReadOnlyBox)) as DateReadOnlyBox;
            StatusTextBox = GetTemplateChild(nameof(StatusTextBox)) as StringReadOnlyBox;
            StatusTextBox.Visibility = Visibility.Collapsed;

            var window = Window.GetWindow(this);
            if (window != LookupControlsGlobals.MainWindow)
            {
                window.Loaded += (sender, args) =>
                {
                    if ((window.Width < 550 && window.Width > 0)
                        || (window.ActualWidth < 550 && window.ActualWidth > 0))
                    {
                        Grid.SetRow(StatusTextBox, 1);
                        Grid.SetColumn(StatusTextBox, 0);
                        Grid.SetColumnSpan(StatusTextBox, 2);
                    }
                };
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Sets the save status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="alertLevel">The alert level.</param>
        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            if (!_isActive)
            {
                return;
            }
            Dispatcher.Invoke(() =>
            {
                if (StatusTextBox != null)
                {
                    StatusTextBox.Visibility = Visibility.Collapsed;
                    switch (alertLevel)
                    {
                        case AlertLevels.Yellow:
                            StatusTextBox.Visibility = Visibility.Visible;
                            StatusTextBox.Background = new SolidColorBrush(Colors.Yellow);
                            StatusTextBox.Foreground = new SolidColorBrush(Colors.Black);
                            break;
                        case AlertLevels.Red:
                            StatusTextBox.Visibility = Visibility.Visible;
                            StatusTextBox.Background = new SolidColorBrush(Colors.Red);
                            StatusTextBox.Foreground = new SolidColorBrush(Colors.White);
                            break;
                    }

                    StatusTextBox.Text = message;
                }
            });
        }

        public void SetPendingSaveStatus(string message)
        {
            Dispatcher.Invoke(() =>
            {
                StatusTextBox.Visibility = Visibility.Visible;
                StatusTextBox.Background = new SolidColorBrush(Colors.LightYellow);
                StatusTextBox.Foreground = new SolidColorBrush(Colors.Black);
                StatusTextBox.Text = message;
            });
        }
    }
}
