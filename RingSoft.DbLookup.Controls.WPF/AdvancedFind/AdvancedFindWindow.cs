using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Hardcodet.Wpf.TaskbarNotification;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using TreeViewItem = RingSoft.DbLookup.AdvancedFind.TreeViewItem;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.Forms.MessageBox;
using TreeView = System.Windows.Controls.TreeView;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF.AdvancedFind;assembly=RingSoft.DbLookup.Controls.WPF.AdvancedFind"
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
    ///     <MyNamespace:AdvancedFindWindow/>
    ///
    /// </summary>
    [TemplatePart(Name = "ButtonsPanel", Type = typeof(StackPanel))]
    public class AdvancedFindWindow : BaseWindow, IAdvancedFindView
    {
        public StackPanel ButtonsPanel { get; set; }
        public Border Border { get; set; }
        public DbMaintenanceStatusBar StatusBar { get; set; }
        public AutoFillControl NameAutoFillControl { get; set; }
        public TreeView TreeView { get; set; }
        //public TextComboBoxControl TableComboBoxControl { get; set; }

        public ListControl TableListControl { get; set; }

        public LookupControl LookupControl { get; set; }
        public TabControl TabControl { get; set; }
        public TabItem ColumnsTabItem { get; set; }
        public DataEntryGrid ColumnsGrid { get; set; }
        public Ellipse FilterEllipse { get; set; }
        public TabItem FiltersTabItem { get; set; }
        public DataEntryGrid FiltersGrid { get; set; }
        public bool ApplyToLookupDefinition { get; set; }

        public AdvancedFindViewModel ViewModel { get; set; }

        private Control _buttonsControl;
        private LookupAddViewArgs _addViewArgs;
        
        public IDbMaintenanceProcessor Processor { get; set; }

        private bool _notifyFromFormulaExists;
        private bool _nameTabKeyPressed;
        private bool _refreshAfterLoad;
        private bool _treeHasFocus;

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

        public AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            var filterWindow = new AdvancedFilterWindow();
            filterWindow.Owner = this;
            filterWindow.ShowInTaskbar = false;
            filterWindow.Initialize(treeViewItem, lookupDefinition);
            var result = filterWindow.ShowDialog();
            if (result != null && result == true)
            {
                return filterWindow.FilterReturn;
            }

            return null;
        }

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

        public void ApplyToLookup()
        {
            ApplyToLookupDefinition = true;
            Close();
        }

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

        public bool ShowRefreshSettings(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            var refreshRateWindow = new AdvancedFindRefreshRateWindow(advancedFind);
            refreshRateWindow.Owner = this;
            refreshRateWindow.ShowInTaskbar = false;
            refreshRateWindow.ShowDialog();
            return refreshRateWindow.DialogResult.Value;
        }

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

        private bool _templateApplied;

        public void SetAddOnFlyFocus()
        {
            LookupControl.Focus();
        }

        public void PrintOutput(PrinterSetupArgs printerSetup)
        {
            LookupControlsGlobals.PrintDocument(printerSetup);
        }

        public void CheckTableIsFocused()
        {
            if (TableListControl.IsKeyboardFocusWithin)
            {
                NameAutoFillControl.Focus();
            }
        }

        public void LockTable(bool lockValue)
        {
            TableListControl.IsEnabled = !lockValue;
        }

        static AdvancedFindWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindWindow)));
        }

        public AdvancedFindWindow(LookupAddViewArgs addViewArgs)
        {
            _addViewArgs = addViewArgs;
            
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        public AdvancedFindWindow()
        {
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

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
                            TabControl.SelectedItem = FiltersTabItem;
                            FiltersTabItem.UpdateLayout();
                            //FiltersGrid.Focus();
                            FiltersGrid.RefreshDataSource();
                            if (ViewModel.FiltersManager.Rows.Any())
                            {
                                FiltersGrid.GotoCell(ViewModel.FiltersManager.Rows[0], (AdvancedFindFiltersManager.SearchColumnId));
                            }
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

        public void Initialize()
        {
            Processor = LookupControlsGlobals.DbMaintenanceProcessorFactory.GetProcessor();
            ViewModel = Border.TryFindResource("AdvancedFindViewModel") as AdvancedFindViewModel;
            ViewModel.CreateCommands();
            _buttonsControl = LookupControlsGlobals.DbMaintenanceButtonsFactory.GetAdvancedFindButtonsControl(ViewModel);
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

        private void Border_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_nameTabKeyPressed)
            {
                _nameTabKeyPressed = false;
                TreeView.Focus();
            }
        }

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

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            LookupControlsGlobals.HandleValFail(this, autoFillMap);
        }

        public void ResetViewForNewRecord()
        {
            NameAutoFillControl?.Focus();
            _templateApplied = false;
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

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
