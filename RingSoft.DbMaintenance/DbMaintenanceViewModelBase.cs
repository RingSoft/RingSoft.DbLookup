using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DbLookup.QueryBuilder;
using RingSoft.DbLookup.TableProcessing;
using RingSoft.Printing.Interop;

namespace RingSoft.DbMaintenance
{
    public class SelectArgs
    {
        public bool Cancel { get; set; }
    }

    public enum DbMaintenanceResults
    {
        Success,
        ValidationError,
        DatabaseError,
        NotAllowed
    }

    public enum DbMaintenanceModes
    {
        AddMode = 0,
        EditMode = 1
    }

    public class CheckDirtyResultArgs
    {
        public MessageButtons Result { get; private set; }

        internal CheckDirtyResultArgs(MessageButtons result)
        {
            Result = result;
        }
    }

    /// <summary>
    /// The base class for database maintenance operations.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class DbMaintenanceViewModelBase : INotifyPropertyChanged, IPrintProcessor
    {
        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <value>
        /// The table definition.
        /// </value>
        public abstract TableDefinitionBase TableDefinitionBase { get; }

        /// <summary>
        /// Gets the view lookup definition used to get the next and previous record.
        /// </summary>
        /// <value>
        /// The view lookup definition.
        /// </value>
        protected LookupDefinitionBase ViewLookupDefinition { get; private set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        protected IDbMaintenanceView View { get; private set; }

        /// <summary>
        /// Gets the add-on-the fly arguments sent by the LookupControl or the LookupWindow.
        /// </summary>
        /// <value>
        /// The lookup add on the fly arguments.
        /// </value>
        public LookupAddViewArgs LookupAddViewArgs { get; internal set; }

        /// <summary>
        /// Gets the initial search for text when the Find button is clicked.  By default it is the key auto fill text.
        /// </summary>
        /// <value>
        /// The find button initial search for.
        /// </value>
        protected virtual string FindButtonInitialSearchFor { get; } = string.Empty;

        private AutoFillSetup _keyAutoFillSetup;

        /// <summary>
        /// Gets the unique key control auto fill setup.
        /// </summary>
        /// <value>
        /// The unique key control auto fill setup.
        /// </value>
        public AutoFillSetup KeyAutoFillSetup
        {
            get => _keyAutoFillSetup;
            set
            {
                if (_keyAutoFillSetup == value)
                    return;

                _keyAutoFillSetup = value;
                OnPropertyChanged(nameof(KeyAutoFillSetup));
            }
        }

        /// <summary>
        /// Gets or sets the find button lookup definition.  By default it is the Lookup definition attached to the Table definition.
        /// </summary>
        /// <value>
        /// The find button lookup definition.
        /// </value>
        public LookupDefinitionBase FindButtonLookupDefinition { get; set; }


        private AutoFillValue _keyAutoFillValue;
        /// <summary>
        /// Gets or sets the unique key control auto fill value.
        /// </summary>
        /// <value>
        /// The key auto fill value.
        /// </value>
        public AutoFillValue KeyAutoFillValue
        {
            get => _keyAutoFillValue;
            set
            {
                if (_keyAutoFillValue == value)
                    return;

                _keyAutoFillValue = value;
                OnPropertyChanged();
            }
        }

        private bool _primaryKeyControlsEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the primary key controls are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if primary key controls enabled; otherwise, <c>false</c>.
        /// </value>
        public bool PrimaryKeyControlsEnabled
        {
            get => _primaryKeyControlsEnabled;
            set
            {
                if (_primaryKeyControlsEnabled == value)
                    return;

                _primaryKeyControlsEnabled = value;
                OnPropertyChanged(nameof(PrimaryKeyControlsEnabled), false);
            }
        }

        private bool _selectButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Select button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the select button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool SelectButtonEnabled
        {
            get => _selectButtonEnabled;
            set
            {
                if (_selectButtonEnabled == value)
                    return;
                SelectCommand.IsEnabled = _selectButtonEnabled = value;
                OnPropertyChanged(nameof(SelectButtonEnabled));
            }
        }

        private bool _deleteButtonEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether the Delete button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the delete button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteButtonEnabled
        {
            get => _deleteButtonEnabled;
            set
            {
                if (_deleteButtonEnabled == value)
                    return;

                DeleteCommand.IsEnabled = _deleteButtonEnabled = value;
                OnPropertyChanged(nameof(DeleteButtonEnabled), false);
            }
        }

        private bool _newButtonEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the New button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the new button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool NewButtonEnabled
        {
            get => _newButtonEnabled;
            set
            {
                if (_newButtonEnabled == value)
                    return;

                NewCommand.IsEnabled = _newButtonEnabled = value;
                OnPropertyChanged(nameof(NewButtonEnabled));
            }
        }

        private bool _saveButtonEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether the Save button is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the save button is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool SaveButtonEnabled
        {
            get => _saveButtonEnabled;
            set
            {
                if (_saveButtonEnabled == value)
                    return;

                SaveCommand.IsEnabled = _saveButtonEnabled = value;
                OnPropertyChanged(nameof(SaveButtonEnabled), false);
            }
        }

        private bool _keyValueDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the data in the user-editable unique key value control has been changed.  Used when focus leaves the unique user-editable control.  When that happens and this value is true, then the record matching that unique value is automatically loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data in the key value control has changed; otherwise, <c>false</c>.
        /// </value>
        public bool KeyValueDirty
        {
            get => _keyValueDirty;
            set
            {
                if (_keyValueDirty == value)
                    return;

                _keyValueDirty = value;
                var recordDirty = RecordDirty;
                OnPropertyChanged(nameof(KeyValueDirty));
                RecordDirty = recordDirty;
            }
        }

        /// <summary>
        /// Gets or sets the maintenance mode.
        /// </summary>
        /// <value>
        /// The maintenance mode.
        /// </value>
        protected DbMaintenanceModes MaintenanceMode { get; set; }

        private bool _recordDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the user has changed the data on this record and has yet to save.
        /// </summary>
        /// <value>
        ///   <c>true</c> if there are unsaved changes in this record; otherwise, <c>false</c>.
        /// </value>
        public bool RecordDirty
        {
            get => _recordDirty;
            set
            {
                if (_recordDirty == value)
                    return;

                _recordDirty = value;
                OnRecordDirtyChanged(_recordDirty);
            }
        }

        private bool _readOnlyMode;

        public bool ReadOnlyMode
        {
            get => _readOnlyMode;
            set
            {
                if (_readOnlyMode == value)
                    return;

                _readOnlyMode = value;
                OnReadOnlyModeChanged(_readOnlyMode);
            }
        }

        public object InputParameter { get; set; }

        public DateTime LockDate { get; set; }

        public IDbMaintenanceDataProcessor Processor { get; set; }

        public RelayCommand PreviousCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand NewCommand { get; private set; }
        public RelayCommand FindCommand { get; private set; }
        public RelayCommand SelectCommand { get; private set; }
        public RelayCommand PrintCommand { get; private set; }

        public event EventHandler<CheckDirtyResultArgs> CheckDirtyMessageShown;
        public event EventHandler InitializeEvent;
        public event EventHandler SaveEvent;
        public event EventHandler<SelectArgs> SelectEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler FindEvent;
        public event EventHandler NewEvent;
        public event EventHandler CloseEvent;
        public event EventHandler NextEvent;
        public event EventHandler PreviousEvent;


        public DbMaintenanceViewModelBase()
        {
            PreviousCommand = new RelayCommand(OnGotoPreviousButton);
            NextCommand = new RelayCommand(OnGotoNextButton);
            SaveCommand = new RelayCommand(OnSaveButton){IsEnabled = SaveButtonEnabled};
            DeleteCommand = new RelayCommand(OnDeleteButton){IsEnabled = DeleteButtonEnabled};
            NewCommand = new RelayCommand(OnNewButton){IsEnabled = NewButtonEnabled};
            FindCommand = new RelayCommand(OnFindButton);
            SelectCommand = new RelayCommand(OnSelectButton){IsEnabled = SelectButtonEnabled};
            PrintCommand = new RelayCommand(PrintOutput);

            NewButtonEnabled = SaveButtonEnabled = true;
        }

        protected internal void Setup(LookupDefinitionBase lookupDefinition)
        {
            ViewLookupDefinition = lookupDefinition;
            KeyAutoFillSetup = new AutoFillSetup(lookupDefinition)
            {
                AllowLookupView = false,
                AllowLookupAdd = false
            };
        }

        /// <summary>
        /// Executed when the view is loaded.  Sets the View interface and initializes this object.
        /// </summary>
        /// <param name="view">The View interface.</param>
        public void OnViewLoaded(IDbMaintenanceView view)
        {
            View = view;
            InternalInitialize();
        }

        protected internal virtual void InternalInitialize()
        {
        }

        /// <summary>
        /// Executed when Previous button is clicked.
        /// </summary>
        public abstract void OnGotoPreviousButton();

        /// <summary>
        /// Executed when Next button is clicked.
        /// </summary>
        public abstract void OnGotoNextButton();

        /// <summary>
        /// Executed when the Find button is clicked.
        /// </summary>
        public abstract void OnFindButton();

        /// <summary>
        /// Executed when the Select button is clicked.
        /// </summary>
        public abstract void OnSelectButton();

        /// <summary>
        /// Executed when the New button is clicked.
        /// </summary>
        public abstract void OnNewButton();

        /// <summary>
        /// Executed when the Save button is clicked.
        /// </summary>
        /// <param name="unitTestMode"></param>
        /// <returns>The result.</returns>
        public abstract DbMaintenanceResults DoSave(bool unitTestMode = false);

        /// <summary>
        /// Executed when the Save button is clicked.
        /// </summary>
        public void OnSaveButton() => DoSave();

        /// <summary>
        /// Executed when the Delete button is clicked.
        /// </summary>
        /// <returns>The result.</returns>
        public abstract DbMaintenanceResults DoDelete();

        /// <summary>
        /// Executed when the Delete button is clicked.
        /// </summary>
        public void OnDeleteButton() => DoDelete();

        /// <summary>
        /// Executed when the key control looses focus.  Used to ensure no duplicate value in the key control is entered.
        /// </summary>
        public abstract void OnKeyControlLeave();

        /// <summary>
        /// Executed when the user is trying to close the view.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        public abstract void OnWindowClosing(CancelEventArgs e);

        /// <summary>
        /// Initializes from lookup data that is sent when the user wishes to add/view lookup data on the fly.
        /// </summary>
        /// <param name="e">The e.</param>
        public abstract void InitializeFromLookupData(LookupAddViewArgs e);

        public abstract void OnRecordSelected(LookupSelectArgs e);

        protected virtual void OnRecordDirtyChanged(bool newValue)
        {
        }

        protected void FireInitializeEvent()
        {
            InitializeEvent?.Invoke(this, new EventArgs());
        }

        protected void FirePreviousEvent()
        {
            PreviousEvent?.Invoke(this, new EventArgs());
        }

        protected void FireNextEvent()
        {
            NextEvent?.Invoke(this, new EventArgs());
        }

        protected void FireFindEvent()
        {
            FindEvent?.Invoke(this, new EventArgs());
        }

        protected SelectArgs FireSelectEvent()
        {
            var selectArgs = new SelectArgs();
            SelectEvent?.Invoke(this, selectArgs);
            return selectArgs;
        }

        protected void FireNewEvent()
        {
            NewEvent?.Invoke(this, new EventArgs());
        }

        protected void FireSaveEvent()
        {
            SaveEvent?.Invoke(this, new EventArgs());
        }

        protected void FireDeleteEvent()
        {
            DeleteEvent?.Invoke(this, new EventArgs());
        }

        protected void FireCloseEvent()
        {
            CloseEvent?.Invoke(this, new EventArgs());
        }

        protected virtual void OnReadOnlyModeChanged(bool newValue)
        {
            if (newValue)
            {
                DeleteButtonEnabled = SaveButtonEnabled = false;
            }
            else
            {
                if (MaintenanceMode != DbMaintenanceModes.AddMode)
                    DeleteButtonEnabled = true;

                SaveButtonEnabled = true;
            }

            View?.SetReadOnlyMode(newValue);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnCheckDirtyFlagMessageShown(CheckDirtyResultArgs e)
        {
            CheckDirtyMessageShown?.Invoke(this, e);
        }

        protected virtual void PrintOutput()
        {
            Processor.PrintOutput(CreatePrinterSetupArgs());
        }

        protected PrinterSetupArgs CreatePrinterSetupArgs()
        {
            var printLookup = FindButtonLookupDefinition.Clone();
            var printerSetupArgs = new PrinterSetupArgs
            {
                CodeAutoFillValue = KeyAutoFillValue,
                CodeAutoFillSetup = KeyAutoFillSetup,
                LookupDefinition = printLookup,
                DataProcessor = this,
                CodeDescription = TableDefinitionBase.RecordDescription,
            };
            printerSetupArgs.PrintingProperties.ReportTitle = $"{TableDefinitionBase.Description} Report";
            SetupPrinterArgs(printerSetupArgs);

            return printerSetupArgs;
        }

        protected virtual void SetupPrinterArgs(PrinterSetupArgs printerSetupArgs, int stringFieldIndex = 1
            , int numericFieldIndex = 1, int memoFieldIndex = 1)
        {
            printerSetupArgs.LookupDefinition.AddAllFieldsAsHiddenColumns();

            foreach (var hiddenColumn in printerSetupArgs.LookupDefinition.HiddenColumns)
            {
                var columnMap = MapLookupColumns(ref stringFieldIndex, ref numericFieldIndex, ref memoFieldIndex, hiddenColumn);
                printerSetupArgs.ColumnMaps.Add(columnMap);
            }
        }

        public static PrintingColumnMap MapLookupColumns(ref int stringFieldIndex, ref int numericFieldIndex,
            ref int memoFieldIndex, LookupColumnDefinitionBase hiddenColumn)
        {
            var columnMap = new PrintingColumnMap();
            switch (hiddenColumn.DataType)
            {
                case FieldDataTypes.String:
                    if (hiddenColumn is LookupFieldColumnDefinition stringColumn)
                    {
                        if (stringColumn.FieldDefinition is StringFieldDefinition stringField)
                        {
                            if (stringField.MemoField)
                            {
                                MapMemoField(memoFieldIndex, columnMap, hiddenColumn);
                                memoFieldIndex++;
                            }
                            else
                            {
                                MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                                stringFieldIndex++;
                            }
                        }
                        else
                        {
                            MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                            stringFieldIndex++;
                        }
                    }
                    else
                    {
                        MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                        stringFieldIndex++;
                    }

                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.DateTime:
                case FieldDataTypes.Bool:
                    MapStringField(columnMap, hiddenColumn, stringFieldIndex);
                    stringFieldIndex++;
                    break;
                case FieldDataTypes.Decimal:
                    columnMap.MapNumber(hiddenColumn, numericFieldIndex,
                        hiddenColumn.SelectSqlAlias);
                    PrintingInteropGlobals.PropertiesProcessor.SetNumberCaption(numericFieldIndex,
                        hiddenColumn.Caption.Replace("\n", " "));
                    numericFieldIndex++;
                    break;
                case FieldDataTypes.Memo:
                    MapMemoField(memoFieldIndex, columnMap, hiddenColumn);
                    memoFieldIndex++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return columnMap;
        }

        private static void MapMemoField(int memoFieldIndex, PrintingColumnMap columnMap,
            LookupColumnDefinitionBase hiddenColumn)
        {
            columnMap.MapMemo(hiddenColumn, memoFieldIndex,
                hiddenColumn.SelectSqlAlias);
            PrintingInteropGlobals.PropertiesProcessor.SetMemoCaption(memoFieldIndex,
                hiddenColumn.Caption.Replace("\r\n", " "));
        }

        private static void MapStringField(PrintingColumnMap columnMap, LookupColumnDefinitionBase lookupColumn,
            int stringFieldIndex)
        {
            columnMap.MapString(lookupColumn, stringFieldIndex, lookupColumn.SelectSqlAlias);
            PrintingInteropGlobals.PropertiesProcessor.SetStringCaption(stringFieldIndex,
                lookupColumn.Caption.Replace("\r\n", " "));
        }

        public virtual void ProcessPrintOutputData(PrinterSetupArgs printerSetupArgs)
        {
            ProcessLookupPrintOutput(printerSetupArgs, this);
        }

        public static void ProcessLookupPrintOutput(PrinterSetupArgs printerSetupArgs, IPrintProcessor printProcessor)
        {
            var lookupUi = new LookupUserInterface
            {
                PageSize = 10,
            };
            var lookupData = new LookupDataBase(printerSetupArgs.LookupDefinition, lookupUi);

            printerSetupArgs.PrintingProcessingViewModel.UpdateStatus(0, 0, ProcessTypes.CountingHeaderRecords);
            printerSetupArgs.TotalRecords = lookupData.GetRecordCountWait();

            var page = 0;
            lookupData.PrintDataChanged += (sender, args) =>
            {
                if (printerSetupArgs.PrintingProcessingViewModel.Abort)
                {
                    args.Abort = true;
                }

                var headerRows = new List<PrintingInputHeaderRow>();
                foreach (DataRow outputTableRow in args.OutputTable.Rows)
                {
                    var headerRow = new PrintingInputHeaderRow();
                    headerRow.RowKey = lookupData.LookupDefinition.InitialSortColumnDefinition
                        .FormatColumnForHeaderRowKey(outputTableRow);
                    var columnMapId = 0;
                    foreach (var columnMap in printerSetupArgs.ColumnMaps)
                    {
                        var value = outputTableRow.GetRowValue(columnMap.FieldName);
                        value = columnMap.ColumnDefinition.FormatValueForColumnMap(value);
                        switch (columnMap.ColumnType)
                        {
                            case PrintColumnTypes.String:
                                PrintingInteropGlobals.HeaderProcessor.SetStringValue(headerRow
                                    , columnMap.StringFieldIndex, value);
                                break;
                            case PrintColumnTypes.Number:
                                PrintingInteropGlobals.HeaderProcessor.SetNumberValue(headerRow
                                    , columnMap.NumericFieldIndex, value);
                                break;
                            case PrintColumnTypes.Memo:
                                PrintingInteropGlobals.HeaderProcessor.SetMemoValue(headerRow
                                    , columnMap.MemoFieldIndex, value);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        columnMapId++;
                    }

                    headerRows.Add(headerRow);

                    var dataProcessedArgs = new PrinterDataProcessedEventArgs
                    {
                        HeaderRow = headerRow,
                        LookupDataChangedArgs = args,
                        PrinterSetup = printerSetupArgs,
                        LookupData = lookupData,
                        OutputRow = outputTableRow,
                    };

                    printProcessor.NotifyProcessingHeader(dataProcessedArgs);
                }

                var result = PrintingInteropGlobals.HeaderProcessor.AddChunk(headerRows, printerSetupArgs.PrintingProperties);
                if (!result.IsNullOrEmpty())
                {
                    printerSetupArgs.PrintingProcessingViewModel.Abort = true;
                    ControlsGlobals.UserInterface.ShowMessageBox(result, "Print Error!", RsMessageBoxIcons.Error);
                    args.Abort = true;
                }

                page += headerRows.Count;
                printerSetupArgs.PrintingProcessingViewModel
                    .UpdateStatus(page, printerSetupArgs.TotalRecords, ProcessTypes.ImportHeader);
            };
            
            lookupData.GetPrintData();
            MakeAdditionalFilter(printerSetupArgs);
        }

        public static void MakeAdditionalFilter(PrinterSetupArgs printerSetupArgs)
        {
            var fixedText = GetFiltersText(printerSetupArgs.LookupDefinition.FilterDefinition
                .FixedFilters.ToList(), printerSetupArgs);

            var userText = GetFiltersText(printerSetupArgs.LookupDefinition.FilterDefinition.UserFilters.ToList(),
                printerSetupArgs);

            var result = string.Empty;
            if (!fixedText.IsNullOrEmpty() && !userText.IsNullOrEmpty())
            {
                result = $"({fixedText}) AND\r\n({userText})";
            }
            else
            {
                result = fixedText + userText;
            }

            printerSetupArgs.PrintingProperties.AdditionalFilter = result;
        }

        private static string GetFiltersText(List<FilterItemDefinition> filterItemDefinitions, PrinterSetupArgs printerSetupArgs)
        {
            var result = string.Empty;
            var count = filterItemDefinitions.Count;
            if (count == 0)
            {
                return result;
            }
            var index = 0;
            var endText = string.Empty;
            foreach (var filterItem in filterItemDefinitions)
            {
                if (!printerSetupArgs.ReportFilters.Contains(filterItem))
                {
                    endText = filterItem.PrintEndLogicText();
                    result += filterItem.GetPrintText(printerSetupArgs.LookupDefinition);
                    var end = index == filterItemDefinitions.Count - 1;

                    if (!end)
                    {
                        result += filterItem.PrintEndLogicText();
                    }
                }
                index++;
            }

            if (!endText.IsNullOrEmpty() && !result.IsNullOrEmpty())
            {
                var indexOfEnd = result.LastIndexOf(endText);
                if (indexOfEnd != -1)
                {
                    var rightText = result.GetRightText(indexOfEnd, 0);
                    if (indexOfEnd + rightText.Length == indexOfEnd + endText.Length)
                    {
                        result = result.LeftStr(indexOfEnd);
                    }
                }

            }

            return result;
        }

        public event EventHandler<PrinterDataProcessedEventArgs> PrintProcessingHeader;
        public void NotifyProcessingHeader(PrinterDataProcessedEventArgs args)
        {
            PrintProcessingHeader?.Invoke(this, args);
        }

        /// <summary>
        /// Executed when a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="raiseDirtyFlag">if set to <c>true</c> set RecordDirty property to true indicating the user has changed data.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null, bool raiseDirtyFlag = true)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
