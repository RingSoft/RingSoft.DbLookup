using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.Printing.Interop;

namespace RingSoft.DbLookup
{
    public enum GenericFocusControls
    {
        Current = 0,
        Start = 1,
    }
    public interface IGenericReportFilterView
    {
        void RefreshView();

        void CloseWindow();

        void PrintOutput();

        void FocusControl(GenericFocusControls control);
    }
    public class GenericReportFilterViewModel : INotifyPropertyChanged
    {
        private bool _isCurrentOnly;

        public bool IsCurrentOnly
        {
            get => _isCurrentOnly;
            set
            {
                if (_isCurrentOnly == value)
                {
                    return;
                }
                _isCurrentOnly = value;
                OnPropertyChanged();

                if (!_loading)
                {
                    View.RefreshView();
                }
            }
        }

        private AutoFillSetup _currentAutoFillSetup;

        public AutoFillSetup CurrentAutoFillSetup
        {
            get => _currentAutoFillSetup;
            set
            {
                if (_currentAutoFillSetup == value)
                {
                    return;
                }
                _currentAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _currentAutoFillValue;

        public AutoFillValue CurrentAutoFillValue
        {
            get => _currentAutoFillValue;
            set
            {
                if (_currentAutoFillValue == value)
                    return;

                _currentAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _beginAutoFillSetup;

        public AutoFillSetup BeginAutoFillSetup
        {
            get => _beginAutoFillSetup;
            set
            {
                if (_beginAutoFillSetup == value)
                {
                    return;
                }
                _beginAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _beginAutoFillValue;

        public AutoFillValue BeginAutoFillValue
        {
            get => _beginAutoFillValue;
            set
            {
                if (_beginAutoFillValue == value)
                    return;

                _beginAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private AutoFillSetup _endAutoFillSetup;

        public AutoFillSetup EndAutoFillSetup
        {
            get => _endAutoFillSetup;
            set
            {
                if (_endAutoFillSetup == value)
                {
                    return;
                }
                _endAutoFillSetup = value;
                OnPropertyChanged();
            }
        }

        private AutoFillValue _endAutoFillValue;

        public AutoFillValue EndAutoFillValue
        {
            get => _endAutoFillValue;
            set
            {
                if (_endAutoFillValue == value)
                    return;

                _endAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private string _printCurrentCodeLabel;

        public string PrintCurrentCodeLabel
        {
            get => _printCurrentCodeLabel;
            set
            {
                if (_printCurrentCodeLabel == value)
                    return;

                _printCurrentCodeLabel = value;
                OnPropertyChanged();
            }
        }

        private string _currentCodeLabel;

        public string CurrentCodeLabel
        {
            get => _currentCodeLabel;
            set
            {
                if (_currentCodeLabel == value)
                    return;

                _currentCodeLabel = value;
                OnPropertyChanged();
            }
        }

        private string _beginCodeLabel;

        public string BeginCodeLabel
        {
            get => _beginCodeLabel;
            set
            {
                if (_beginCodeLabel == value)
                    return;

                _beginCodeLabel = value;
                OnPropertyChanged();
            }
        }

        private string _endCodeLabel;

        public string EndCodeLabel
        {
            get => _endCodeLabel;
            set
            {
                if (_endCodeLabel == value)
                    return;

                _endCodeLabel = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxControlSetup _reportTypeBoxControlSetup;

        public TextComboBoxControlSetup ReportTypeBoxControlSetup
        {
            get => _reportTypeBoxControlSetup;
            set
            {
                if (_reportTypeBoxControlSetup == value)
                    return;

                _reportTypeBoxControlSetup = value;
                OnPropertyChanged();
            }
        }

        private TextComboBoxItem _reportTypeBoxItem;

        public TextComboBoxItem ReportTypeBoxItem
        {
            get => _reportTypeBoxItem;
            set
            {
                if (_reportTypeBoxItem == value)
                    return;

                _reportTypeBoxItem = value;
                OnPropertyChanged();
            }
        }

        public ReportTypes ReportType
        {
            get => (ReportTypes)ReportTypeBoxItem.NumericValue;
            set => ReportTypeBoxItem = ReportTypeBoxControlSetup.GetItem((int)value);
        }

        public IGenericReportFilterView View { get; private set; }

        public PrinterSetupArgs PrinterSetup { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        private bool _loading = true;

        public GenericReportFilterViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        public void Initialize(IGenericReportFilterView view, PrinterSetupArgs printerSetup)
        {
            View = view;
            PrinterSetup = printerSetup;

            CurrentAutoFillSetup = new AutoFillSetup(printerSetup.LookupDefinition);
            BeginAutoFillSetup = new AutoFillSetup(printerSetup.LookupDefinition);
            EndAutoFillSetup = new AutoFillSetup(printerSetup.LookupDefinition);

            PrintCurrentCodeLabel = $" Print Current {printerSetup.CodeDescription} Only";
            CurrentCodeLabel = $"Current {printerSetup.CodeDescription}";
            BeginCodeLabel = $"Beginning {printerSetup.CodeDescription}";
            EndCodeLabel = $"Ending {printerSetup.CodeDescription}";

            ReportTypeBoxControlSetup = new TextComboBoxControlSetup();
            ReportTypeBoxControlSetup.LoadFromEnum<ReportTypes>();
            var item = ReportTypeBoxControlSetup.GetItem((int)ReportTypes.Custom);
            ReportTypeBoxControlSetup.Items.Remove(item);
            ReportType = ReportTypes.Details;

            if (printerSetup.CodeAutoFillValue.IsValid())
            {
                CurrentAutoFillValue = printerSetup.CodeAutoFillValue;
                IsCurrentOnly = true;
            }

            View.RefreshView();
            if (IsCurrentOnly)
            {
                View.FocusControl(GenericFocusControls.Current);
            }
            else
            {
                View.FocusControl(GenericFocusControls.Start);
            }

            _loading = false;
        }

        private void OnOk()
        {
            if (!Validate())
            {
                return;
            }

            if (IsCurrentOnly)
            {
                SetupCurrentFilter();
            }
            else
            {
                ProcessBeginEndCode(BeginAutoFillValue, true);
                ProcessBeginEndCode(EndAutoFillValue, false);
            }

            PrinterSetup.PrintingProperties.PrintCurrentCode = IsCurrentOnly;
            PrinterSetup.PrintingProperties.ReportType = ReportType;

            View.PrintOutput();
        }

        private bool Validate()
        {
            var caption = "Validation Error.";
            if (IsCurrentOnly)
            {
                if (CurrentAutoFillValue == null || CurrentAutoFillValue.Text.IsNullOrEmpty())
                {
                    var message = $"Current {PrinterSetup.CodeDescription} cannot be empty.";
                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    View.FocusControl(GenericFocusControls.Current);
                    return false;
                }
            }
            else
            {
                var beginText = string.Empty;
                var endText = string.Empty;
                if (BeginAutoFillValue != null)
                {
                    beginText = BeginAutoFillValue.Text;
                }
                if (EndAutoFillValue != null)
                {
                    endText = EndAutoFillValue.Text;
                }

                if (!beginText.IsNullOrEmpty() && !endText.IsNullOrEmpty())
                {
                    var compare = endText.CompareTo(beginText);
                    
                    if (compare == -1)
                    {
                        var message =
                            $"The Ending {PrinterSetup.CodeDescription} cannot be greater than the Beginning {PrinterSetup.CodeDescription}";
                        ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                        View.FocusControl(GenericFocusControls.Start);
                        return false;
                    }
                }
            }
            return true;
        }

        private void ProcessBeginEndCode(AutoFillValue autoFillValue, bool start)
        {
            Conditions condition = Conditions.LessThanEquals;
            var filterText = string.Empty;
            if (start)
            {
                condition = Conditions.GreaterThanEquals;
                var beginText = "Start";
                if (autoFillValue != null && !autoFillValue.Text.IsNullOrEmpty())
                {
                    filterText = beginText = autoFillValue.Text;
                }
                PrinterSetup.PrintingProperties.BeginCode = beginText;
            }
            else 
            {
                var endText = "End";
                if (autoFillValue != null && !autoFillValue.Text.IsNullOrEmpty())
                {
                    filterText = endText = autoFillValue.Text;
                }
                PrinterSetup.PrintingProperties.EndCode = endText;
            }

            if (!filterText.IsNullOrEmpty())
            {
                if (PrinterSetup.LookupDefinition.InitialSortColumnDefinition
                    is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    PrinterSetup.AddReportFilter(PrinterSetup.LookupDefinition.FilterDefinition.AddFixedFilter(lookupFormulaColumn.Description,
                        condition, filterText, lookupFormulaColumn.Formula,
                        lookupFormulaColumn.DataType));
                }
                else if (PrinterSetup.LookupDefinition.InitialSortColumnDefinition
                         is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    PrinterSetup.AddReportFilter(PrinterSetup.LookupDefinition.FilterDefinition.AddFixedFilter(lookupFieldColumn.FieldDefinition, condition, filterText));
                }
            }
        }

        private void SetupCurrentFilter()
        {
            if (CurrentAutoFillValue.IsValid())
            {
                if (PrinterSetup.LookupDefinition.InitialSortColumnDefinition
                    is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    var description = lookupFormulaColumn.Caption;
                    PrinterSetup.AddReportFilter(PrinterSetup.LookupDefinition.FilterDefinition.AddFixedFilter(description, Conditions.Equals,
                        CurrentAutoFillValue.Text, lookupFormulaColumn.Formula,
                        lookupFormulaColumn.DataType));
                }
                else if (PrinterSetup.LookupDefinition.InitialSortColumnDefinition
                         is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    foreach (var keyValueField in CurrentAutoFillValue.PrimaryKeyValue.KeyValueFields)
                    {
                        PrinterSetup.AddReportFilter(PrinterSetup.LookupDefinition.FilterDefinition.AddFixedFilter(keyValueField.FieldDefinition,
                            Conditions.Equals, keyValueField.Value));
                    }
                }

                PrinterSetup.PrintingProperties.CurrentCode = CurrentAutoFillValue.Text;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
