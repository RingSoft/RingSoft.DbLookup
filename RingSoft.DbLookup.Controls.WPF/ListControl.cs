// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 03-04-2023
//
// Last Modified By : petem
// Last Modified On : 12-16-2023
// ***********************************************************************
// <copyright file="ListControl.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Breaks a list into pages to display to the user.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="RingSoft.DbLookup.IListControlView" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="RingSoft.DbLookup.IListControlView" />
    /// <font color="red">Badly formed XML comment.</font>
    public class ListControl : Control, IListControlView
    {
        /// <summary>
        /// The setup property
        /// </summary>
        public static readonly DependencyProperty SetupProperty =
            DependencyProperty.Register("Setup", typeof(ListControlSetup), typeof(ListControl),
                new FrameworkPropertyMetadata(SetupChangedCallback));

        /// <summary>
        /// Gets or sets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public ListControlSetup Setup
        {
            get { return (ListControlSetup)GetValue(SetupProperty); }
            set { SetValue(SetupProperty, value); }
        }

        /// <summary>
        /// Setups the changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void SetupChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
                listControl.SetupControl();
        }

        /// <summary>
        /// The data source property
        /// </summary>
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(ListControlDataSource), typeof(ListControl),
                new FrameworkPropertyMetadata(DataSourceChangedCallback));

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public ListControlDataSource DataSource
        {
            get { return (ListControlDataSource)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        /// <summary>
        /// Datas the source changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void DataSourceChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
            {
                listControl.ViewModel.DataSource = listControl.DataSource;
            }
        }

        /// <summary>
        /// The data row property
        /// </summary>
        public static readonly DependencyProperty DataRowProperty =
            DependencyProperty.Register("DataRow", typeof(ListControlDataSourceRow), typeof(ListControl),
                new FrameworkPropertyMetadata(DataRowChangedCallback));

        /// <summary>
        /// Gets or sets the data row.
        /// </summary>
        /// <value>The data row.</value>
        public ListControlDataSourceRow DataRow
        {
            get { return (ListControlDataSourceRow)GetValue(DataRowProperty); }
            set { SetValue(DataRowProperty, value); }
        }

        /// <summary>
        /// Datas the row changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void DataRowChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._controlLoaded)
            {
                if (listControl.DataRow == null)
                {
                    listControl.ViewModel.Text = string.Empty;
                    listControl.ViewModel.SelectedDataRow = null;
                }
                else
                {
                    listControl.ViewModel.Text = listControl.DataRow.GetCellItem(0);
                    listControl.TextBox.SelectAll();
                }
            }
        }

        /// <summary>
        /// The UI command property
        /// </summary>
        public static readonly DependencyProperty UiCommandProperty =
            DependencyProperty.Register(nameof(UiCommand), typeof(UiCommand), typeof(ListControl),
                new FrameworkPropertyMetadata(UiCommandChangedCallback));

        /// <summary>
        /// Gets or sets the UI command.
        /// </summary>
        /// <value>The UI command.</value>
        public UiCommand UiCommand
        {
            get { return (UiCommand)GetValue(UiCommandProperty); }
            set { SetValue(UiCommandProperty, value); }
        }

        /// <summary>
        /// UIs the command changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiCommandChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._vmUiControl == null)
            {
                listControl._vmUiControl = WPFControlsGlobals.VmUiFactory.CreateUiControl(
                    listControl, listControl.UiCommand);
                if (listControl.UiLabel != null)
                {
                    listControl._vmUiControl.SetLabel(listControl.UiLabel);
                }
            }
        }

        /// <summary>
        /// The UI label property
        /// </summary>
        public static readonly DependencyProperty UiLabelProperty =
            DependencyProperty.Register(nameof(UiLabel), typeof(Label), typeof(ListControl),
                new FrameworkPropertyMetadata(UiLabelChangedCallback));

        /// <summary>
        /// Gets or sets the UI label.
        /// </summary>
        /// <value>The UI label.</value>
        public Label UiLabel
        {
            get { return (Label)GetValue(UiLabelProperty); }
            set { SetValue(UiLabelProperty, value); }
        }

        /// <summary>
        /// UIs the label changed callback.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void UiLabelChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var listControl = (ListControl)obj;
            if (listControl._vmUiControl != null)
                listControl._vmUiControl.SetLabel(listControl.UiLabel);
        }


        /// <summary>
        /// Gets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; private set; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public ListControlViewModel ViewModel { get; private set; }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        /// <value>The text box.</value>
        public StringEditControl TextBox { get; private set; }

        /// <summary>
        /// Gets the button.
        /// </summary>
        /// <value>The button.</value>
        public Button Button { get; private set; }

        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The vm UI control
        /// </summary>
        private VmUiControl _vmUiControl;

        /// <summary>
        /// Initializes static members of the <see cref="ListControl" /> class.
        /// </summary>
        static ListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListControl), new FrameworkPropertyMetadata(typeof(ListControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControl" /> class.
        /// </summary>
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
                ViewModel.Text = DataRow?.GetCellItem(0);
            };
            GotFocus += (sender, args) =>
            {
                if (TextBox != null)
                {
                    TextBox.Focus();
                }
            };
        }

        /// <summary>
        /// Setups the control.
        /// </summary>
        private void SetupControl()
        {
            ViewModel.Setup = Setup;
            ViewModel.DataSource = DataSource;
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as ListControlViewModel;
            TextBox = GetTemplateChild(nameof(TextBox)) as StringEditControl;
            Button = GetTemplateChild(nameof(Button)) as Button;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                ViewModel.ShowLookupCommand.Execute(null);
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Shows the lookup window.
        /// </summary>
        /// <returns>ListControlDataSourceRow.</returns>
        public ListControlDataSourceRow ShowLookupWindow()
        {
            var window = new ListWindow(ViewModel.Setup, ViewModel.DataSource, DataRow);
            window.Owner = Window.GetWindow(this);
            window.ShowInTaskbar = false;
            window.ShowDialog();
            if (window.ViewModel.DialogResult)
            {
                return window.ViewModel.SelectedItem;
            }
            return null;
        }

        /// <summary>
        /// Selects the data row.
        /// </summary>
        /// <param name="selectedIRow">The selected i row.</param>
        public void SelectDataRow(ListControlDataSourceRow selectedIRow)
        {
            DataRow = selectedIRow;
        }
    }
}
