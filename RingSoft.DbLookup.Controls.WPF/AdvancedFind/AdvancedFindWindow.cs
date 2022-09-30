using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.Lookup;
using TreeViewItem = RingSoft.DbLookup.AdvancedFind.TreeViewItem;

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
        public AutoFillControl NameAutoFillControl { get; set; }
        public TextComboBoxControl TableComboBoxControl { get; set; }
        public LookupControl LookupControl { get; set; }
        public NotificationButton NotificationButton { get; set; }
        public bool ApplyToLookupDefinition { get; set; }

        public AdvancedFindViewModel ViewModel { get; set; }

        private Control _buttonsControl;
        private LookupAddViewArgs _addViewArgs;
        private TaskbarIcon _taskbarIcon;

        public IDbMaintenanceProcessor Processor { get; set; }

        private bool _notifyFromFormulaExists;
        public bool ShowFromFormulaEditor(ref string fromFormula)
        {
            var editor = new DataEntryGridMemoEditor(new DataEntryGridMemoValue(0){Text = fromFormula});
            editor.Loaded += (sender, args) => editor.MemoEditor.CollapseDateButton();
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

        public bool NotifyFromFormulaExists
        {
            get => _notifyFromFormulaExists;
            set
            {
                _notifyFromFormulaExists = value;
                NotificationButton.MemoHasText = value;
            }
        }

        public void ApplyToLookup()
        {
            ApplyToLookupDefinition = true;
            Close();
        }

        public void ShowSqlStatement()
        {
            var sql = LookupControl.LookupData.GetSqlStatement();
            var window = new AdvancedFindGridMemoEditor(new DataEntryGridMemoValue(0) {Text = sql});
            window.Loaded += (sender, args) =>
            {
                window.MemoEditor.TextBox.IsReadOnly = true;
                window.MemoEditor.TextBox.TextChanged += (o, eventArgs) =>
                {
                    window.MemoEditor.TextBox.SelectionLength = 0;
                    window.MemoEditor.TextBox.SelectionStart = 0;
                };
                window.MemoEditor.TextBox.TextWrapping = TextWrapping.NoWrap;
                window.MemoEditor.TextBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                window.MemoEditor.TextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                window.UpdateLayout();
            };
            window.Owner = this;
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }

        public bool ShowRefreshSettings(DbLookup.AdvancedFind.AdvancedFind advancedFind)
        {
            var refreshRateWindow = new AdvancedFindRefreshRateWindow(advancedFind);
            refreshRateWindow.Owner = this;
            refreshRateWindow.ShowInTaskbar = false;
            refreshRateWindow.ShowDialog();
            return refreshRateWindow.DialogResult.Value;
        }

        public void SetAlertLevel(AlertLevels level, string message = "")
        {
            var advancedFindWindows = Dispatcher.Invoke(() => Application.Current.Windows.OfType<AdvancedFindWindow>().ToList());
            var image = LookupControlsGlobals.LookupControlContentTemplateFactory
                .GetImageForAlertLevel(level);
            var title = string.Empty;
            var baloonIcon = BalloonIcon.Info;
            switch (level)
            {
                case AlertLevels.Green:
                    break;
                case AlertLevels.Yellow:
                    title = "Warning!";
                    break;
                case AlertLevels.Red:
                    title = "Red Alert!";
                    baloonIcon = BalloonIcon.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            if (image == null)
            {
                var errorMessage = "No icon found for alert level: ";
                switch (level)
                {
                    case AlertLevels.Green:
                        errorMessage += "Green";
                        break;
                    case AlertLevels.Yellow:
                        errorMessage += "Yellow";
                        break;
                    case AlertLevels.Red:
                        errorMessage += "Red";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }

                throw new ApplicationException(errorMessage);

            }
            Dispatcher.Invoke(() => Icon = image.Source);
            //if (advancedFindWindows.Count >= 2)
            //{
            //    if (SystemGlobals.WindowAlertLevel < level)
            //    {
            //        SystemGlobals.WindowAlertLevel = level;
            //        Dispatcher.Invoke(() =>
            //        {
            //            ShowLevelIcon(level, message, image, title, baloonIcon);
            //            return;
            //        });
            //    }

            //}
            //else
            //{
                SystemGlobals.WindowAlertLevel = level;
                Dispatcher.Invoke(() => { ShowLevelIcon(level, message, image, title, baloonIcon); });

            //}
        }

        private void ShowLevelIcon(AlertLevels level, string message, Image image, string title, BalloonIcon baloonIcon)
        {
            if (level == AlertLevels.Green)
            {
                _taskbarIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (image.Source != null)
                {
                    _taskbarIcon.ToolTipText = message;
                    _taskbarIcon.IconSource = image.Source;
                    _taskbarIcon.Visibility = Visibility.Visible;

                    if (!ViewModel.Disabled)
                    {
                        _taskbarIcon.ShowBalloonTip(title, message, baloonIcon);

                        _taskbarIcon.HideBalloonTip();
                    }

                    return;

                    //return Application.Current.MainWindow.Icon = image.Source;
                }

                return;
            }
        }

        public int GetRecordCount(bool showRecordCount)
        {
            LookupControl.ShowRecordCountWait = showRecordCount;
            var count = LookupControl.RecordCountWait;
            //Dispatcher.Invoke(() =>
            //{
            //    count = LookupControl.GetRecordCountWait();
            //});

            return count;
        }

        public void LockTable(bool lockValue)
        {
            TableComboBoxControl.IsEnabled = !lockValue;
        }

        static AdvancedFindWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedFindWindow), new FrameworkPropertyMetadata(typeof(AdvancedFindWindow)));
        }

        public AdvancedFindWindow(LookupAddViewArgs addViewArgs)
        {
            _addViewArgs = addViewArgs;
            _taskbarIcon = new TaskbarIcon();
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
        }

        public AdvancedFindWindow()
        {
            Closing += (sender, args) => ViewModel.OnWindowClosing(args);
            _taskbarIcon = new TaskbarIcon();
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
            Processor.Initialize(this, _buttonsControl, ViewModel, this);
            Processor.LookupAddView += (sender, args) =>
            {
                if (args.InputParameter is AdvancedFindInput advancedFindInput)
                {
                    TableComboBoxControl.IsEnabled = false;
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
            NameAutoFillControl = GetTemplateChild(nameof(NameAutoFillControl)) as AutoFillControl;
            TableComboBoxControl = GetTemplateChild(nameof(TableComboBoxControl)) as TextComboBoxControl;
            LookupControl = GetTemplateChild(nameof(LookupControl)) as LookupControl;
            NotificationButton = GetTemplateChild(nameof(NotificationButton)) as NotificationButton;

            if (LookupControl != null)
            {
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
            }

            base.OnApplyTemplate();
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
                TableComboBoxControl.Focus();
            }
        }

        public void ResetViewForNewRecord()
        {
            NameAutoFillControl?.Focus();
        }

        public bool ShowFormulaEditor(TreeViewItem formulaTreeViewItem)
        {
            var editor = new AdvancedFindFormulaColumnWindow(new DataEntryGridMemoValue(0));
            if (formulaTreeViewItem.Parent != null)
            {
                editor.ParentTable = formulaTreeViewItem.Parent.FieldDefinition.ParentJoinForeignKeyDefinition
                    .ForeignTable.Description;
                editor.ParentField = formulaTreeViewItem.Parent.FieldDefinition.ParentJoinForeignKeyDefinition.FieldJoins[0]
                    .ForeignField.Description;
            }
            else
            {
                //editor.ParentTable = formulaTreeViewItem.ViewModel.LookupDefinition.TableDefinition.Description;
                editor.ParentField = "<Lookup Root>";
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
