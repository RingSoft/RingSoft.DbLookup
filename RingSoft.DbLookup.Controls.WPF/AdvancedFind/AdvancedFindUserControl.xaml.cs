using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using System.Windows.Input;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.DbMaintenance;
using TreeViewItem = RingSoft.DbLookup.AdvancedFind.TreeViewItem;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    /// <summary>
    /// Interaction logic for AdvancedFindUserControl.xaml
    /// </summary>
    public partial class AdvancedFindUserControl : IAdvancedFindView
    {
        private Control _buttonsControl;
        private bool _refreshAfterLoad;
        private bool _treeHasFocus;
        private bool _buttonsPanelSet;
        private VmUiControl _selectLookupRowButtonUiControl;

        public AdvancedFindUserControl()
        {
            InitializeComponent();
            _selectLookupRowButtonUiControl =
                new VmUiControl(SelectLookupButton, LocalViewModel.SelectLookupRowUiCommand);

            LookupControl.SelectedIndexChanged += (sender, args) =>
            {
                if (LookupControl.SelectedIndex >= 0)
                {
                    if (SelectLookupButton.Visibility == Visibility.Visible)
                    {
                        LocalViewModel.SelectLookupRowCommand.IsEnabled = true;
                    }
                }
                else
                {
                    LocalViewModel.SelectLookupRowCommand.IsEnabled = false;
                }
            };
            RegisterFormKeyControl(NameAutoFillControl);

            LookupControl.Loaded += (sender, args) =>
            {
                if (_refreshAfterLoad)
                {
                    GetRecordCount(true);
                }

            };

            Loaded += (sender, args) =>
            {
                if (ButtonsPanel != null && !_buttonsPanelSet)
                {
                    ButtonsPanel.Children.Add(_buttonsControl);
                    ButtonsPanel.UpdateLayout();
                    _buttonsPanelSet = true;
                }
            };

            LookupControl.ColumnWidthChanged += (sender, args) =>
            {
                if (LocalViewModel.LookupDefinition.VisibleColumns.Contains(args.ColumnDefinition))
                {
                    LocalViewModel.ColumnsManager.UpdateColumnWidth(args.ColumnDefinition);
                }
            };

            LookupControl.Loaded += (sender, args) =>
            {
                if (_refreshAfterLoad)
                {
                    GetRecordCount(true);
                }
            };

            var gotoLookupCommand = new RelayCommand((() =>
            {
                LookupControl.Focus();
            }));
            var gotoLookupHotKey = new HotKey(gotoLookupCommand);
            gotoLookupHotKey.AddKey(Key.L);
            AddHotKey(gotoLookupHotKey);

            var gotoTreeViewCommand = new RelayCommand((() =>
            {
                TreeView.Focus();
            }));
            var gotoTreeViewHotKey = new HotKey(gotoTreeViewCommand);
            gotoTreeViewHotKey.AddKey(Key.F);
            AddHotKey(gotoTreeViewHotKey);

            var gotoColumnsCommand = new RelayCommand((() =>
            {
                if (!ColumnsGrid.IsKeyboardFocusWithin)
                {
                    TabControl.SelectedItem = ColumnsTabItem;
                    ColumnsTabItem.UpdateLayout();
                    ColumnsGrid.Focus();
                }
            }));
            var gotoColumnsHotKey = new HotKey(gotoColumnsCommand);
            gotoColumnsHotKey.AddKey(Key.O);
            AddHotKey(gotoColumnsHotKey);

            var gotoFiltersCommand = new RelayCommand((() =>
            {
                if (!FiltersGrid.IsKeyboardFocusWithin)
                {
                    FocusFiltersTab();
                }
            }));
            var gotoFiltersHotKey = new HotKey(gotoFiltersCommand);
            gotoFiltersHotKey.AddKey(Key.I);
            AddHotKey(gotoFiltersHotKey);

            var importDefaultHotKey = new HotKey(LocalViewModel.ImportDefaultLookupCommand);
            importDefaultHotKey.AddKey(Key.A);
            importDefaultHotKey.AddKey(Key.I);
            AddHotKey(importDefaultHotKey);

            var printLookupOutputHotKey = new HotKey(LocalViewModel.PrintLookupOutputCommand);
            printLookupOutputHotKey.AddKey(Key.A);
            printLookupOutputHotKey.AddKey(Key.P);
            AddHotKey(printLookupOutputHotKey);

            var refreshSettingsHotKey = new HotKey(LocalViewModel.RefreshSettingsCommand);
            refreshSettingsHotKey.AddKey(Key.A);
            refreshSettingsHotKey.AddKey(Key.R);
            AddHotKey(refreshSettingsHotKey);

            var selectLookupRowsHotKey = new HotKey(LocalViewModel.SelectLookupRowCommand);
            selectLookupRowsHotKey.AddKey(Key.A);
            selectLookupRowsHotKey.AddKey(Key.L);
            AddHotKey(selectLookupRowsHotKey);

            var hotKey = new HotKey(LocalViewModel.RefreshNowCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.R);
            AddHotKey(hotKey);

            hotKey = new HotKey(LocalViewModel.AddColumnCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.O);
            AddHotKey(hotKey);

            hotKey = new HotKey(LocalViewModel.AddFilterCommand);
            hotKey.AddKey(Key.B);
            hotKey.AddKey(Key.I);
            AddHotKey(hotKey);

            AddFilterButton.ToolTip.HeaderText = "Add Filter (Ctrl + B, Ctrl + I)";
            AddFilterButton.ToolTip.DescriptionText = "Add filter to lookup.";

            AddColumnButton.ToolTip.HeaderText = "Add Column (Ctrl + B, Ctrl + O)";
            AddColumnButton.ToolTip.DescriptionText = "Add column to lookup.";

            RefreshNowButton.ToolTip.HeaderText = "Refresh Now (Ctrl + B, Ctrl + R)";
            RefreshNowButton.ToolTip.DescriptionText = "Refresh Lookup now.";

            LocalViewModel.LookupCreated += (sender, args) =>
            {
                if (Host.HostType == HostTypes.Tab && LookupControlsGlobals.TabControl != null)
                {
                    LocalViewModel.LookupDefinition.Destination = LookupControlsGlobals.TabControl;
                }
            };

            TreeView.GotFocus += TreeView_GotFocus;
        }

        public override void SetInitialFocus()
        {
            if (LookupAddViewArgs != null && LookupAddViewArgs.LookupFormMode == LookupFormModes.View)
            {
                SetAddOnFlyFocus();
                return;
            }

            base.SetInitialFocus();
        }

        private void TreeView_GotFocus(object sender, RoutedEventArgs e)
        {
            var sendTab = true;
            if (TreeView.SelectedItem == null)
            {
                if (LocalViewModel.AdvancedFindTree != null)
                {
                    if (LocalViewModel.AdvancedFindTree.TreeRoot != null)
                    {
                        if (LocalViewModel.AdvancedFindTree.TreeRoot.Count > 0)
                        {
                            _treeHasFocus = true;
                            LocalViewModel.AdvancedFindTree.TreeRoot[0].IsSelected = true;
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


        protected override DbMaintenanceViewModelBase OnGetViewModel()
        {
            return LocalViewModel;
        }

        protected override Control OnGetMaintenanceButtons()
        {
            if (_buttonsControl == null)
            {
                _buttonsControl =
                    LookupControlsGlobals.DbMaintenanceButtonsFactory.GetAdvancedFindButtonsControl(LocalViewModel);
            }
            return _buttonsControl;
        }

        protected override DbMaintenanceStatusBar OnGetStatusBar()
        {
            return StatusBar;
        }

        protected override string GetTitle()
        {
            return "Advanced Find";
        }

        private void FocusFiltersTab()
        {
            TabControl.SelectedItem = FiltersTabItem;
            FiltersTabItem.UpdateLayout();
            //FiltersGrid.Focus();
            FiltersGrid.RefreshDataSource();
            if (LocalViewModel.FiltersManager.Rows.Any())
            {
                //Peter Ringering - 01/14/2025 08:35:49 PM - E-121
                FiltersGrid.Manager.GotoCell(LocalViewModel.FiltersManager.Rows[0]
                    , AdvancedFindFiltersManager.SearchColumnId);
                //FiltersGrid.GotoCell(LocalViewModel.FiltersManager.Rows[0], (AdvancedFindFiltersManager.SearchColumnId));
            }
        }
        public AdvancedFilterReturn ShowAdvancedFilterWindow(TreeViewItem treeViewItem, LookupDefinitionBase lookupDefinition)
        {
            var filterWindow = new AdvancedFilterWindow();
            filterWindow.Owner = OwnerWindow;
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

        public bool ShowRefreshSettings(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            var refreshRateWindow = new AdvancedFindRefreshRateWindow(advancedFind);
            refreshRateWindow.Owner = OwnerWindow;
            refreshRateWindow.ShowInTaskbar = false;
            refreshRateWindow.ShowDialog();
            return refreshRateWindow.DialogResult.Value;

        }

        public void SetAlertLevel(AlertLevels level, string message, bool showCount, int recordCount)
        {
            LookupControl.ShowRecordCount(recordCount, showCount);
            LookupControlsGlobals.LookupWindowFactory.SetAlertLevel(level, LocalViewModel.LookupRefresher.Disabled, OwnerWindow, message);
            if (LocalViewModel.LookupRefresher.RefreshRate == RefreshRate.None)
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
                if (LocalViewModel.LookupDefinition != null && showRecordCount)
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

        public PrimaryKeyValue GetSelectedPrimaryKeyValue()
        {
            return LookupControl.LookupDataMaui.GetSelectedPrimaryKeyValue();
        }
    }
}
