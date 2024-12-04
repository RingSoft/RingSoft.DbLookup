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

        public AdvancedFindUserControl()
        {
            InitializeComponent();
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
                if (ButtonsPanel != null)
                {
                    ButtonsPanel.Children.Add(_buttonsControl);
                    ButtonsPanel.UpdateLayout();
                }

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
            };
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
                FiltersGrid.GotoCell(LocalViewModel.FiltersManager.Rows[0], (AdvancedFindFiltersManager.SearchColumnId));
                FiltersGrid.GotoCell(LocalViewModel.FiltersManager.Rows[0], (AdvancedFindFiltersManager.SearchColumnId));
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
    }
}
