// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 01-30-2023
//
// Last Modified By : petem
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="GenericReportFilterViewModel.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.Printing.Interop;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Enum GenericFocusControls
    /// </summary>
    public enum GenericFocusControls
    {
        /// <summary>
        /// The current
        /// </summary>
        Current = 0,
        /// <summary>
        /// The start
        /// </summary>
        Start = 1,
    }
    /// <summary>
    /// Interface IGenericReportFilterView
    /// </summary>
    public interface IGenericReportFilterView
    {
        /// <summary>
        /// Refreshes the view.
        /// </summary>
        void RefreshView();

        /// <summary>
        /// Closes the window.
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// Prints the output.
        /// </summary>
        void PrintOutput();

        /// <summary>
        /// Focuses the control.
        /// </summary>
        /// <param name="control">The control.</param>
        void FocusControl(GenericFocusControls control);
    }

    /// <summary>
    /// Class GenericReportLookupFilterInput.
    /// </summary>
    public class GenericReportLookupFilterInput
    {
        /// <summary>
        /// Gets or sets the lookup definition to filter.
        /// </summary>
        /// <value>The lookup definition to filter.</value>
        public LookupDefinitionBase LookupDefinitionToFilter { get; set; }

        /// <summary>
        /// Gets or sets the key automatic fill value.
        /// </summary>
        /// <value>The key automatic fill value.</value>
        public AutoFillValue KeyAutoFillValue { get; set; }

        /// <summary>
        /// Gets or sets the code name to filter.
        /// </summary>
        /// <value>The code name to filter.</value>
        public string CodeNameToFilter { get; set; }

        /// <summary>
        /// Gets or sets the process text.
        /// </summary>
        /// <value>The process text.</value>
        public string ProcessText { get; set; }
    }
    /// <summary>
    /// Class GenericReportFilterViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class GenericReportFilterViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The is current only
        /// </summary>
        private bool _isCurrentOnly;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current only.
        /// </summary>
        /// <value><c>true</c> if this instance is current only; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// The current automatic fill setup
        /// </summary>
        private AutoFillSetup _currentAutoFillSetup;

        /// <summary>
        /// Gets or sets the current automatic fill setup.
        /// </summary>
        /// <value>The current automatic fill setup.</value>
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

        /// <summary>
        /// The current automatic fill value
        /// </summary>
        private AutoFillValue _currentAutoFillValue;

        /// <summary>
        /// Gets or sets the current automatic fill value.
        /// </summary>
        /// <value>The current automatic fill value.</value>
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

        /// <summary>
        /// The begin automatic fill setup
        /// </summary>
        private AutoFillSetup _beginAutoFillSetup;

        /// <summary>
        /// Gets or sets the begin automatic fill setup.
        /// </summary>
        /// <value>The begin automatic fill setup.</value>
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

        /// <summary>
        /// The begin automatic fill value
        /// </summary>
        private AutoFillValue _beginAutoFillValue;

        /// <summary>
        /// Gets or sets the begin automatic fill value.
        /// </summary>
        /// <value>The begin automatic fill value.</value>
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

        /// <summary>
        /// The end automatic fill setup
        /// </summary>
        private AutoFillSetup _endAutoFillSetup;

        /// <summary>
        /// Gets or sets the end automatic fill setup.
        /// </summary>
        /// <value>The end automatic fill setup.</value>
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

        /// <summary>
        /// The end automatic fill value
        /// </summary>
        private AutoFillValue _endAutoFillValue;

        /// <summary>
        /// Gets or sets the end automatic fill value.
        /// </summary>
        /// <value>The end automatic fill value.</value>
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

        /// <summary>
        /// The print current code label
        /// </summary>
        private string _printCurrentCodeLabel;

        /// <summary>
        /// Gets or sets the print current code label.
        /// </summary>
        /// <value>The print current code label.</value>
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

        /// <summary>
        /// The current code label
        /// </summary>
        private string _currentCodeLabel;

        /// <summary>
        /// Gets or sets the current code label.
        /// </summary>
        /// <value>The current code label.</value>
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

        /// <summary>
        /// The begin code label
        /// </summary>
        private string _beginCodeLabel;

        /// <summary>
        /// Gets or sets the begin code label.
        /// </summary>
        /// <value>The begin code label.</value>
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

        /// <summary>
        /// The end code label
        /// </summary>
        private string _endCodeLabel;

        /// <summary>
        /// Gets or sets the end code label.
        /// </summary>
        /// <value>The end code label.</value>
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

        /// <summary>
        /// The report type box control setup
        /// </summary>
        private TextComboBoxControlSetup _reportTypeBoxControlSetup;

        /// <summary>
        /// Gets or sets the report type box control setup.
        /// </summary>
        /// <value>The report type box control setup.</value>
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

        /// <summary>
        /// The report type box item
        /// </summary>
        private TextComboBoxItem _reportTypeBoxItem;

        /// <summary>
        /// Gets or sets the report type box item.
        /// </summary>
        /// <value>The report type box item.</value>
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

        /// <summary>
        /// Gets or sets the type of the report.
        /// </summary>
        /// <value>The type of the report.</value>
        public ReportTypes ReportType
        {
            get => (ReportTypes)ReportTypeBoxItem.NumericValue;
            set => ReportTypeBoxItem = ReportTypeBoxControlSetup.GetItem((int)value);
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IGenericReportFilterView View { get; private set; }

        /// <summary>
        /// Gets the printer setup.
        /// </summary>
        /// <value>The printer setup.</value>
        public PrinterSetupArgs PrinterSetup { get; private set; }

        /// <summary>
        /// Gets the ok command.
        /// </summary>
        /// <value>The ok command.</value>
        public RelayCommand OkCommand { get; private set; }

        /// <summary>
        /// Gets the cancel command.
        /// </summary>
        /// <value>The cancel command.</value>
        public RelayCommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [lookup mode].
        /// </summary>
        /// <value><c>true</c> if [lookup mode]; otherwise, <c>false</c>.</value>
        public bool LookupMode { get; private set; }

        /// <summary>
        /// Gets the lookup to filter.
        /// </summary>
        /// <value>The lookup to filter.</value>
        public LookupDefinitionBase LookupToFilter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [dialog reesult].
        /// </summary>
        /// <value><c>true</c> if [dialog reesult]; otherwise, <c>false</c>.</value>
        public bool DialogReesult { get; private set; }

        /// <summary>
        /// Gets the lookup input.
        /// </summary>
        /// <value>The lookup input.</value>
        public GenericReportLookupFilterInput LookupInput { get; private set; }

        /// <summary>
        /// The loading
        /// </summary>
        private bool _loading = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericReportFilterViewModel"/> class.
        /// </summary>
        public GenericReportFilterViewModel()
        {
            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand((() =>
            {
                View.CloseWindow();
            }));
        }

        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="input">The input.</param>
        public void Initialize(IGenericReportFilterView view, GenericReportLookupFilterInput input)
        {
            LookupMode = true;
            LookupToFilter = input.LookupDefinitionToFilter;
            View = view;
            LookupInput = input;
            if (input.KeyAutoFillValue.IsValid())
            {
                CurrentAutoFillValue = input.KeyAutoFillValue;
                IsCurrentOnly = true;
            }
            Initialize();

            PrintCurrentCodeLabel = $" {input.ProcessText} Current {input.CodeNameToFilter} Only";
            CurrentCodeLabel = $"Current {input.CodeNameToFilter}";
            SetupStartEndLabels(input);

            _loading = false;
        }

        /// <summary>
        /// Setups the start end labels.
        /// </summary>
        /// <param name="input">The input.</param>
        protected virtual void SetupStartEndLabels(GenericReportLookupFilterInput input)
        {
            BeginCodeLabel = $"Beginning {input.CodeNameToFilter}";
            EndCodeLabel = $"Ending {input.CodeNameToFilter}";
        }

        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="printerSetup">The printer setup.</param>
        public void Initialize(IGenericReportFilterView view, PrinterSetupArgs printerSetup)
        {
            View = view;
            PrinterSetup = printerSetup;
            LookupToFilter = printerSetup.LookupDefinition;

            if (printerSetup.CodeAutoFillSetup == null
                || printerSetup.LookupDefinition.InitialSortColumnDefinition.DataType != FieldDataTypes.String)
            {
                printerSetup.PrintingProperties.ReportType = ReportTypes.Details;
                view.PrintOutput();
                view.CloseWindow();
                return;
            }

            if (printerSetup.CodeAutoFillValue.IsValid())
            {
                CurrentAutoFillValue = printerSetup.CodeAutoFillValue;
                IsCurrentOnly = true;
            }
            Initialize();

            PrintCurrentCodeLabel = $" Print Current {printerSetup.CodeDescription} Only";
            CurrentCodeLabel = $"Current {printerSetup.CodeDescription}";
            BeginCodeLabel = $"Beginning {printerSetup.CodeDescription}";
            EndCodeLabel = $"Ending {printerSetup.CodeDescription}";

            ReportTypeBoxControlSetup = new TextComboBoxControlSetup();
            ReportTypeBoxControlSetup.LoadFromEnum<ReportTypes>();
            var item = ReportTypeBoxControlSetup.GetItem((int)ReportTypes.Custom);
            ReportTypeBoxControlSetup.Items.Remove(item);
            ReportType = ReportTypes.Details;

            _loading = false;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            CurrentAutoFillSetup = new AutoFillSetup(LookupToFilter)
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            BeginAutoFillSetup = new AutoFillSetup(LookupToFilter)
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            EndAutoFillSetup = new AutoFillSetup(LookupToFilter)
            {
                AllowLookupAdd = false,
                AllowLookupView = false
            };
            View.RefreshView();
            if (IsCurrentOnly)
            {
                View.FocusControl(GenericFocusControls.Current);
            }
            else
            {
                View.FocusControl(GenericFocusControls.Start);
            }

        }

        /// <summary>
        /// Setups the filter.
        /// </summary>
        protected virtual void SetupFilter()
        {

        }

        /// <summary>
        /// Called when [ok].
        /// </summary>
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

            SetupFilter();
            if (LookupMode)
            {
                DialogReesult = true;
                View.CloseWindow();
            }
            else
            {
                PrinterSetup.PrintingProperties.PrintCurrentCode = IsCurrentOnly;
                if (PrinterSetup.PrintingProperties.ReportType != ReportTypes.Custom)
                {
                    PrinterSetup.PrintingProperties.ReportType = ReportType;
                }

                PrinterSetup.PrintingProperties.CurrentCodeOnlyCaption = PrintCurrentCodeLabel;
                PrinterSetup.PrintingProperties.CurrentCodeCaption = CurrentCodeLabel;
                PrinterSetup.PrintingProperties.BeginCodeCaption = BeginCodeLabel;
                PrinterSetup.PrintingProperties.EndCodeCaption = EndCodeLabel;

                View.PrintOutput();
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

                var codeDescription = string.Empty;
                if (LookupMode)
                {
                    codeDescription = LookupInput.CodeNameToFilter;
                }
                else
                {
                    codeDescription = PrinterSetup.CodeDescription;
                }
                if (!ValBeginEndText(beginText, endText, caption, codeDescription))
                {
                    View.FocusControl(GenericFocusControls.Start);
                    return false;
                }
            }
            return AdditionalValidate();
        }

        /// <summary>
        /// Values the begin end text.
        /// </summary>
        /// <param name="beginText">The begin text.</param>
        /// <param name="endText">The end text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="codeDescription">The code description.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool ValBeginEndText(string beginText, string endText, string caption
        , string codeDescription)
        {
            if (!beginText.IsNullOrEmpty() && !endText.IsNullOrEmpty())
            {
                var compare = endText.CompareTo(beginText);

                if (compare == -1)
                {
                    var message =
                        $"The Ending {codeDescription} cannot be greater than the Beginning {codeDescription}";

                    ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Additionals the validate.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected virtual bool AdditionalValidate()
        {
            return true;
        }

        /// <summary>
        /// Processes the begin end code.
        /// </summary>
        /// <param name="autoFillValue">The automatic fill value.</param>
        /// <param name="start">if set to <c>true</c> [start].</param>
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

                if (!LookupMode)
                    PrinterSetup.PrintingProperties.BeginCode = beginText;
                
            }
            else
            {
                var endText = "End";
                if (autoFillValue != null && !autoFillValue.Text.IsNullOrEmpty())
                {
                    filterText = endText = autoFillValue.Text;
                }
                if (!LookupMode)
                    PrinterSetup.PrintingProperties.EndCode = endText;
            }

            if (!filterText.IsNullOrEmpty())
            {
                if (LookupToFilter.InitialSortColumnDefinition
                    is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    var filter = LookupToFilter.FilterDefinition.AddFixedFilter(lookupFormulaColumn.Description,
                        condition, filterText, lookupFormulaColumn.Formula,
                        lookupFormulaColumn.DataType);
                    if (!LookupMode)
                    {
                        PrinterSetup.AddReportFilter(filter);
                    }
                }
                else if (LookupToFilter.InitialSortColumnDefinition
                         is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    var filter = LookupToFilter.FilterDefinition.AddFixedFilter(lookupFieldColumn.FieldDefinition,
                        condition, filterText);
                    filter.SetPropertyName = lookupFieldColumn.GetPropertyJoinName();
                    if (!LookupMode)
                    {
                        PrinterSetup.AddReportFilter(filter);
                    }
                }
            }
        }

        /// <summary>
        /// Setups the current filter.
        /// </summary>
        private void SetupCurrentFilter()
        {
            if (CurrentAutoFillValue.IsValid())
            {
                if (LookupToFilter.InitialSortColumnDefinition
                    is LookupFormulaColumnDefinition lookupFormulaColumn)
                {
                    var description = lookupFormulaColumn.Caption;
                    var filter = LookupToFilter.FilterDefinition.AddFixedFilter(description,
                        Conditions.Equals,
                        CurrentAutoFillValue.Text, lookupFormulaColumn.Formula,
                        lookupFormulaColumn.DataType);

                    if (!LookupMode)
                    {
                        PrinterSetup.AddReportFilter(filter);
                    }
                }
                else if (LookupToFilter.InitialSortColumnDefinition
                         is LookupFieldColumnDefinition lookupFieldColumn)
                {
                    foreach (var keyValueField in CurrentAutoFillValue.PrimaryKeyValue.KeyValueFields)
                    {
                        var filter = LookupToFilter.FilterDefinition.AddFixedFilter(keyValueField.FieldDefinition,
                            Conditions.Equals, keyValueField.Value);
                        if (!LookupMode)
                        {
                            PrinterSetup.AddReportFilter(filter);
                        }
                    }
                }

                if (!LookupMode)
                {
                    PrinterSetup.PrintingProperties.CurrentCode = CurrentAutoFillValue.Text;
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
