// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-07-2023
// ***********************************************************************
// <copyright file="AdvancedFindWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using TreeView = System.Windows.Controls.TreeView;
using TreeViewItem = RingSoft.DbLookup.AdvancedFind.TreeViewItem;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// The Advanced Find Window.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="IAdvancedFindView" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="IAdvancedFindView" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "ButtonsPanel", Type = typeof(StackPanel))]
    public class AdvancedFindWindow : BaseWindow, IAdvancedFindView, IDbMaintenanceVisualView
    {
        /// <summary>
        /// Gets or sets the buttons panel.
        /// </summary>
        /// <value>The buttons panel.</value>
        public StackPanel ButtonsPanel { get; set; }
        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }
        /// <summary>
        /// Gets or sets the status bar.
        /// </summary>
        /// <value>The status bar.</value>
        public DbMaintenanceStatusBar StatusBar { get; set; }
        /// <summary>
        /// Gets or sets the name automatic fill control.
        /// </summary>
        /// <value>The name automatic fill control.</value>
        public AutoFillControl NameAutoFillControl { get; set; }
        /// <summary>
        /// Gets or sets the TreeView.
        /// </summary>
        /// <value>The TreeView.</value>
        public TreeView TreeView { get; set; }
        //public TextComboBoxControl TableComboBoxControl { get; set; }

        /// <summary>
        /// Gets or sets the table list control.
        /// </summary>
        /// <value>The table list control.</value>
        public ListControl TableListControl { get; set; }

        /// <summary>
        /// Gets or sets the lookup control.
        /// </summary>
        /// <value>The lookup control.</value>
        public LookupControl LookupControl { get; set; }
        /// <summary>
        /// Gets or sets the tab control.
        /// </summary>
        /// <value>The tab control.</value>
        public TabControl TabControl { get; set; }
        /// <summary>
        /// Gets or sets the columns tab item.
        /// </summary>
        /// <value>The columns tab item.</value>
        public TabItem ColumnsTabItem { get; set; }
        /// <summary>
        /// Gets or sets the columns grid.
        /// </summary>
        /// <value>The columns grid.</value>
        public DataEntryGrid ColumnsGrid { get; set; }
        /// <summary>
        /// Gets or sets the filter ellipse.
        /// </summary>
        /// <value>The filter ellipse.</value>
        public Ellipse FilterEllipse { get; set; }
        /// <summary>
        /// Gets or sets the filters tab item.
        /// </summary>
        /// <value>The filters tab item.</value>
        public TabItem FiltersTabItem { get; set; }
        /// <summary>
        /// Gets or sets the filters grid.
        /// </summary>
        /// <value>The filters grid.</value>
        public DataEntryGrid FiltersGrid { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [apply to lookup definition].
        /// </summary>
        /// <value><c>true</c> if [apply to lookup definition]; otherwise, <c>false</c>.</value>
        public bool ApplyToLookupDefinition { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public AdvancedFindViewModel ViewModel { get; set; }

        /// <summary>
        /// The buttons control
        /// </summary>
        private Control _buttonsControl;
        /// <summary>
        /// The add view arguments
        /// </summary>
        private LookupAddViewArgs _addViewArgs;

        /// <summary>
        /// Gets or sets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public IDbMaintenanceProcessor Processor { get; set; }

        public BaseWindow MaintenanceWindow => this;

        /// <summary>
        /// The notify from formula exists
        /// </summary>
        private bool _notifyFromFormulaExists;
        /// <summary>
        /// The name tab key pressed
        /// </summary>
        private bool _nameTabKeyPressed;
        /// <summary>
        /// The refresh after load
        /// </summary>
        private bool _refreshAfterLoad;
        /// <summary>
        /// The tree has focus
        /// </summary>
        private bool _treeHasFocus;

        /// <summary>
        /// Shows from formula editor.
        /// </summary>
        /// <param name="fromFormula">From formula.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowFromFormulaEditor(ref string fromFormula)
        {
            var editor = new DataEntryGridMemoEditor(new DataEntryGridMemoValue(0){Text = fromFormula});
            editor.SnugWidth = 700;
            editor.SnugHeight = 500;
            editor.Loaded += (sender, args) =>
            {
                editor.MemoEditor.CollapseDateButton();
            };
            editor.ShowInTaskbar = false;
            editor.Owner = this;
            if (editor.ShowDialog())
            {
                fromFormula = editor.GridMemoValue.Text;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Shows the advanced filter window.
        /// </summary>
        /// <param name="treeViewItem">The tree view item.</param>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <returns>AdvancedFilterReturn.</returns>
        public AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            var filterWindow = new AdvancedFilterWindow();
            filterWindow.Owner = this;
            filterWindow.ShowInTaskbar = false;
            filterWindow.Initialize(treeViewItem, lookupDefinition);
            var result = filterWindow.ShowDialog();
            if (result != null && result == true)
            {
                //FocusFiltersTab();
                return filterWindow.FilterReturn;
            }

            return null;
        }

        /// <summary>
        /// Shows the filters ellipse.
        /// </summary>
        /// <param name="showFiltersEllipse">if set to <c>true</c> [show filters ellipse].</param>
        public void ShowFiltersEllipse(bool showFiltersEllipse = true)
        {
            if (showFiltersEllipse)
            {
                FilterEllipse.Visibility = Visibility.Visible;
            }
            else
            {
                FilterEllipse.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Applies to lookup.
        /// </summary>
        public void ApplyToLookup()
        {
            ApplyToLookupDefinition = true;
            Close();
        }

        /// <summary>
        /// Shows the SQL statement.
        /// </summary>
        public void ShowSqlStatement()
        {
            //var sql = LookupControl.LookupData.GetSqlStatement();
            //var window = new AdvancedFindGridMemoEditor(new DataEntryGridMemoValue(0) {Text = sql});
            //window.SnugWidth = 700;
            //window.SnugHeight = 500;
            //window.Loaded += (sender, args) =>
            //{
            //    window.MemoEditor.TextBox.IsReadOnly = true;
            //    window.MemoEditor.TextBox.TextChanged += (o, eventArgs) =>
            //    {
            //        window.MemoEditor.TextBox.SelectionLength = 0;
            //        window.MemoEditor.TextBox.SelectionStart = 0;
            //    };
            //    window.MemoEditor.TextBox.TextWrapping = TextWrapping.NoWrap;
            //    window.MemoEditor.TextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            //    window.MemoEditor.TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            //    window.UpdateLayout();
            //};
            //window.Owner = this;
            //window.ShowInTaskbar = false;
            //window.ShowDialog();
        }

        /// <summary>
        /// Shows the refresh settings.
        /// </summary>
        /// <param name="advancedFind">The advanced find.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowRefreshSettings(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            var refreshRateWindow = new AdvancedFindRefreshRateWindow(advancedFind);
            refreshRateWindow.Owner = this;
            refreshRateWindow.ShowInTaskbar = false;
            refreshRateWindow.ShowDialog();
            return refreshRateWindow.DialogResult.Value;
        }

        /// <summary>
        /// Sets the alert level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="showCount">if set to <c>true</c> [show count].</param>
        /// <param name="recordCount">The record count.</param>
        public void SetAlertLevel(AlertLevels level, string message, bool showCount, int recordCount)
        {
            LookupControl.ShowRecordCount(recordCount, showCount);
            LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(level, ViewModel.LookupRefresher.Disabled, this, message);
            if (ViewModel.LookupRefresher.RefreshRate == RefreshRate.None)
            {
                LookupControl.ShowRecordCountProps = false;
                if (recordCount == 0)
                {
                    showCount = true;
                }
                LookupControl.ShowRecordCount(0, showCount);
            }
            else
            {
                LookupControl.ShowRecordCountProps = true;
            }
        }

        /// <summary>
        /// Gets the record count.
        /// </summary>
        /// <param name="showRecordCount">if set to <c>true</c> [show record count].</param>
        /// <returns>System.Int32.</returns>
        public int GetRecordCount(bool showRecordCount)
        {
            var recordCount = 0;
            Dispatcher.Invoke(() =>
            {
                if (ViewModel.LookupDefinition != null && showRecordCount)
                {
                    if (LookupControl.LookupDataMaui == null)
                    {
                        _refreshAfterLoad = true;
                        return recordCount;
                    }

                    recordCount = LookupControl.LookupDataMaui.GetRecordCount();
                    //var countQuerySet = new QuerySet();
                    //ViewModel.LookupDefinition.GetCountQuery(countQuerySet, "Count");
                    //var countResult =
                    //    ViewModel.LookupDefinition.TableDefinition.Context.DataProcessor.GetData(countQuerySet);
                    //recordCount = ViewModel.LookupDefinition.GetCount(countResult, "Count");
                }

                return recordCount;
            });
            return recordCount;
        }

        /// <summary>
        /// The template applied
        /// </summary>
        private bool _templateApplied;

        /// <summary>
        /// Sets the add on fly focus.
        /// </summary>
        public void SetAddOnFlyFocus()
        {
            LookupControl.Focus();
        }

        /// <summary>
        /// Prints the output.
        /// </summary>
        /// <param name="printerSetup">The printer setup.</param>
        public void PrintOutput(PrinterSetupArgs printerSetup)
        {
            LookupControlsGlobals.PrintDocument(printerSetup);
        }

        /// <summary>
        /// Checks the table is focused.
        /// </summary>
        public void CheckTableIsFocused()
        {
            if (TableListControl.IsKeyboardFocusWithin)
            {
                NameAutoFillControl.Focus();
            }
        }

        public void SelectFiltersTab()
        {
            TabControl.SelectedItem = FiltersTabItem;
        }

        public void ResetLookup()
        {
            var command = new LookupCommand(LookupCommands.Reset, null, true);
            command.ClearColumns = true;
            LookupControl.Command = command;
        }

        //public void LockTable(bool lockValue)
        //{
        //    TableListControl.IsEnabled = !lockValue;
        //}

        /// <summary>
        /// Initializes static members of the <see cref="AdvancedFindWindow"/> class.
        /// </summary>
        static AdvancedFindWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindWindow)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindWindow"/> class.
        /// </summary>
        /// <param name="addViewArgs">The add view arguments.</param>
        public AdvancedFindWindow(LookupAddViewArgs addViewArgs)
        {
            _addViewArgs = addViewArgs;
            
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedFindWindow"/> class.
        /// </summary>
        public AdvancedFindWindow()
        {
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.O:
                        if (!ColumnsGrid.IsKeyboardFocusWithin)
                        {
                            TabControl.SelectedItem = ColumnsTabItem;
                            ColumnsTabItem.UpdateLayout();
                            ColumnsGrid.Focus();
                        }
                        break;
                    case Key.I:
                        if (!FiltersGrid.IsKeyboardFocusWithin)
                        {
                            FocusFiltersTab();
                        }
                        break;
                    case Key.L:
                        LookupControl.Focus();
                        break;
                    case Key.F:
                        TreeView.Focus();
                        break;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Focuses the filters tab.
        /// </summary>
        private void FocusFiltersTab()
        {
            TabControl.SelectedItem = FiltersTabItem;
            FiltersTabItem.UpdateLayout();
            //FiltersGrid.Focus();
            FiltersGrid.RefreshDataSource();
            if (ViewModel.FiltersManager.Rows.Any())
            {
                FiltersGrid.GotoCell(ViewModel.FiltersManager.Rows[0], (AdvancedFindFiltersManager.SearchColumnId));
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            if (Processor == null)
            {
                throw new Exception(
                    "You must inherit and override LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor() and not return null.");
            }
            ViewModel = Border.TryFindResource("AdvancedFindViewModel") as AdvancedFindViewModel;
            ViewModel.CreateCommands();
            var advFindButtons = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetAdvancedFindButtonsControl(ViewModel);

            if (advFindButtons == null)
            {
                throw new Exception(
                    "You must inherit LookupControlsGlobals.DbMaintenanceButtonsFactory return valid control in GetAdvancedFindButtonsControl.");
            }
            _buttonsControl = advFindButtons;

            if (ButtonsPanel != null)
            {
                ButtonsPanel.Children.Add(_buttonsControl);
                ButtonsPanel.UpdateLayout();
            }

            ViewModel.View = this;
            Processor.Initialize(this, _buttonsControl, ViewModel, this, StatusBar);
            Processor.LookupAddView += (sender, args) =>
            {
                if (args.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    TableListControl.IsEnabled = false;
                    if (!advancedFindInput.LookupWidth.Equals(0))
                    {
                        LookupControl.MinWidth = 0;
                        LookupControl.Width = advancedFindInput.LookupWidth;
                        LookupControl.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                }
            };
            if (_addViewArgs != null)
            {
                Processor.InitializeFromLookupData(_addViewArgs);
            }
            Processor.CheckAddOnFlyMode();

        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ButtonsPanel = GetTemplateChild(nameof(ButtonsPanel)) as StackPanel;
            StatusBar = GetTemplateChild(nameof(StatusBar)) as DbMaintenanceStatusBar;
            NameAutoFillControl = GetTemplateChild(nameof(NameAutoFillControl)) as AutoFillControl;
            TreeView = GetTemplateChild(nameof(TreeView)) as TreeView;
            TableListControl = GetTemplateChild(nameof(TableListControl)) as ListControl;
            LookupControl = GetTemplateChild(nameof(LookupControl)) as LookupControl;
            TabControl = GetTemplateChild(nameof(TabControl)) as TabControl;
            ColumnsTabItem = GetTemplateChild(nameof(ColumnsTabItem)) as TabItem;
            ColumnsGrid = GetTemplateChild(nameof(ColumnsGrid)) as DataEntryGrid;
            FiltersTabItem = GetTemplateChild(nameof(FiltersTabItem)) as TabItem;
            FilterEllipse = GetTemplateChild(nameof(FilterEllipse)) as Ellipse;
            FiltersGrid = GetTemplateChild(nameof(FiltersGrid)) as DataEntryGrid;
            ShowFiltersEllipse(false);
            if (LookupControl != null)
            {
                LookupControl.Loaded += (sender, args) =>
                {
                    if (_refreshAfterLoad)
                    {
                        GetRecordCount(true);
                    }
                    
                };
                LookupControl.ColumnWidthChanged += (sender, args) =>
                {
                    if (ViewModel.LookupDefinition.VisibleColumns.Contains(args.ColumnDefinition))
                    {
                        ViewModel.ColumnsManager.UpdateColumnWidth(args.ColumnDefinition);
                    }
                };
            }

            Initialize();
            if (NameAutoFillControl != null)
            {
                Processor.RegisterFormKeyControl(NameAutoFillControl);
                NameAutoFillControl.KeyDown += (sender, args) =>
                {
                    if (args.Key == Key.Tab && !LookupControlsGlobals.IsShiftKeyDown())
                    {
                        if (NameAutoFillControl.Value.IsValid())
                        {
                            _nameTabKeyPressed = true;
                        }
                    }
                };
            }

            //if (TableListControl != null)
            //{
            //    TableListControl.KeyDown += (sender, args) =>
            //    {
            //        if (args.Key == Key.Tab && !LookupControlsGlobals.IsShiftKeyDown())
            //        {
            //            if (ViewModel.AdvancedFindTree == null 
            //                || ViewModel.AdvancedFindTree.TreeRoot == null)
            //            {
            //                NameAutoFillControl.Focus();
            //            }
            //        }
            //    };
            //}

            Border.GotFocus += Border_GotFocus;

            if (!_templateApplied)
            {
                TreeView.GotFocus += TreeView_GotFocus;
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Handles the GotFocus event of the TreeView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TreeView_GotFocus(object sender, RoutedEventArgs e)
        {
            var sendTab = true;
            if (TreeView.SelectedItem == null)
            {
                if (ViewModel.AdvancedFindTree != null)
                {
                    if (ViewModel.AdvancedFindTree.TreeRoot != null)
                    {
                        if (ViewModel.AdvancedFindTree.TreeRoot.Count > 0)
                        {
                            _treeHasFocus = true;
                            ViewModel.AdvancedFindTree.TreeRoot[0].IsSelected = true;
                            _treeHasFocus = false;
                            sendTab = false;
                        }
                    }
                }
            }
            else
            {
                sendTab = false;
            }

            if (sendTab && !_treeHasFocus)
            {
                WPFControlsGlobals.SendKey(Key.Tab);
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the Border control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Border_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_nameTabKeyPressed)
            {
                _nameTabKeyPressed = false;
                TreeView.Focus();
            }
        }

        /// <summary>
        /// Called when [validation fail].
        /// </summary>
        /// <param name="fieldDefinition">The field definition.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Processor.OnValidationFail(fieldDefinition, text, caption);
            if (fieldDefinition == SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p => p.Name))
            {
                NameAutoFillControl.Focus();
            }

            if (fieldDefinition == SystemGlobals.AdvancedFindLookupContext.AdvancedFinds.GetFieldDefinition(p => p.Table))
            {
                TableListControl.Focus();
            }
        }

        /// <summary>
        /// Handles the automatic fill value fail.
        /// </summary>
        /// <param name="autoFillMap">The automatic fill map.</param>
        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            LookupControlsGlobals.HandleValFail(this, autoFillMap);
        }

        /// <summary>
        /// Reset the view for new record.
        /// </summary>
        public void ResetViewForNewRecord()
        {
            NameAutoFillControl?.Focus();
            _templateApplied = false;
        }

        /// <summary>
        /// Gets the automatic fills.
        /// </summary>
        /// <returns>List&lt;DbAutoFillMap&gt;.</returns>
        public List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        /// <summary>
        /// Shows the formula editor.
        /// </summary>
        /// <param name="formulaTreeViewItem">The formula TreeView item.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ShowFormulaEditor(TreeViewItem formulaTreeViewItem)
        {
            var editor = new AdvancedFindFormulaColumnWindow(new DataEntryGridMemoValue(0));
            if (formulaTreeViewItem.Parent != null)
            {
                editor.ParentTable = formulaTreeViewItem.Parent.Name;
            }
            else
            {
                editor.ParentTable = ViewModel.LookupDefinition.TableDefinition.Description;
            }

            if (formulaTreeViewItem.FormulaData != null)
            {
                editor.DataType = formulaTreeViewItem.FormulaData.DataType;
            }
            editor.Owner =this;
            editor.ShowInTaskbar = false;
            if (editor.ShowDialog())
            {
                formulaTreeViewItem.FormulaData = new TreeViewFormulaData();
                formulaTreeViewItem.FormulaData.DataType = editor.ViewModel.DataType;
                formulaTreeViewItem.FormulaData.Formula = editor.MemoEditor.Text;
                if (formulaTreeViewItem.FormulaData.DataType == FieldDataTypes.Decimal)
                {
                    formulaTreeViewItem.FormulaData.DecimalFormatType = editor.ViewModel.DecimalFormatType;
                }
                return true;
            }
            return false;
        }
    }
}
