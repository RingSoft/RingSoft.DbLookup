using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DbStatusBar/>
    ///
    /// </summary>
    public class DbMaintenanceStatusBar : Control
    {
        public Border Border { get; private set; }
        public DateReadOnlyBox DateReadOnlyBox { get; set; }
        public StringReadOnlyBox StatusTextBox { get; set; }
        public DbMaintenanceStatusBarViewModel ViewModel { get; private set; }

        public static readonly DependencyProperty LastSavedDateProperty =
            DependencyProperty.Register("LastSavedDate", typeof(DateTime?), typeof(DbMaintenanceStatusBar),
                new FrameworkPropertyMetadata(DateChangedCallback));

        public DateTime? LastSavedDate
        {
            get { return (DateTime?)GetValue(LastSavedDateProperty); }
            set { SetValue(LastSavedDateProperty, value); }
        }

        private static void DateChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var statusBarControl = (DbMaintenanceStatusBar)obj;
            statusBarControl.ViewModel.LastSavedDate = statusBarControl.LastSavedDate;
        }

        private bool _isActive = true;

        static DbMaintenanceStatusBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceStatusBar), new FrameworkPropertyMetadata(typeof(DbMaintenanceStatusBar)));

            IsTabStopProperty.OverrideMetadata(typeof(DbMaintenanceStatusBar), new FrameworkPropertyMetadata(false));
        }

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

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as DbMaintenanceStatusBarViewModel;

            DateReadOnlyBox = GetTemplateChild(nameof(DateReadOnlyBox)) as DateReadOnlyBox;
            StatusTextBox = GetTemplateChild(nameof(StatusTextBox)) as StringReadOnlyBox;
            StatusTextBox.Visibility = Visibility.Collapsed;

            var window = Window.GetWindow(this);
            if ((window.Width < 550 && window.Width > 0)
                || (window.ActualWidth < 550 && window.ActualWidth > 0))
            {
                Grid.SetRow(StatusTextBox, 1);
                Grid.SetColumn(StatusTextBox, 0);
                Grid.SetColumnSpan(StatusTextBox, 2);
            }
            base.OnApplyTemplate();
        }

        public void SetSaveStatus(string message, AlertLevels alertLevel)
        {
            if (!_isActive)
            {
                return;
            }
            Dispatcher.Invoke(() =>
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
            });
        }
    }
}
