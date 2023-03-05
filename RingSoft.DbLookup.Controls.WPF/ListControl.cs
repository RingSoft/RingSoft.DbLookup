using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;

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
    ///     <MyNamespace:ListControl/>
    ///
    /// </summary>
    public class ListControl : Control, IListControlView
    {
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(ListControlSetup), typeof(ListControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        public ListControlSetup Setup
        {
            get { return (ListControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
                listControl.SetupControl();
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(ListControlDataSource), typeof(ListControl),
                new FrameworkPropertyMetadata(DataSourceChangedCallback));

        public ListControlDataSource DataSource
        {
            get { return (ListControlDataSource)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        private static void DataSourceChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
            {
                listControl.ViewModel.DataSource = listControl.DataSource;
            }
        }

        public static readonly DependencyProperty DataItemProperty =
            DependencyProperty.Register("DataItem", typeof(ListControlDataSourceItem), typeof(ListControl),
                new FrameworkPropertyMetadata(DataItemChangedCallback));

        public ListControlDataSourceItem DataItem
        {
            get { return (ListControlDataSourceItem)GetValue(DataItemProperty); }
            set { SetValue(DataItemProperty, value); }
        }

        private static void DataItemChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
            {
                listControl.ViewModel.Text = listControl.DataItem.DataItem;
            }
        }

        public Border Border { get; private set; }

        public ListControlViewModel ViewModel { get; private set; }

        public StringEditControl TextBox { get; private set; }

        public Button Button { get; private set; }

        private bool _controlLoaded;

        static ListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListControl), new FrameworkPropertyMetadata(typeof(ListControl)));
        }

        public ListControl()
        {
            Loaded += (sender, args) =>
            {
                _controlLoaded = true;
                if (Setup != null)
                {
                    SetupControl();
                }
                ViewModel.Initialize(this);
                ViewModel.Setup = Setup;
                ViewModel.DataSource = DataSource;
                ViewModel.Text = DataItem?.DataItem;
            };
            GotFocus += (sender, args) =>
            {
                if (TextBox != null)
                {
                    TextBox.Focus();
                }
            };
        }

        private void SetupControl()
        {
            ViewModel.Setup = Setup;
            ViewModel.DataSource = DataSource;
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as ListControlViewModel;
            TextBox = GetTemplateChild(nameof(TextBox)) as StringEditControl;
            Button = GetTemplateChild(nameof(Button)) as Button;

            base.OnApplyTemplate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ViewModel.ShowLookupCommand.Execute(null);
            }
            base.OnKeyDown(e);
        }

        public ListControlDataSourceItem ShowLookupWindow()
        {
            var window = new ListWindow(ViewModel.Setup, ViewModel.DataSource);
            window.Owner = Window.GetWindow(this);
            window.ShowInTaskbar = false;
            window.ShowDialog();
            return null;
        }
    }
}
