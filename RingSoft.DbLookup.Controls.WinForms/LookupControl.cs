using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using RingSoft.DbLookup.Controls.WinForms.Annotations;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;
using RSDbLookup.Controls.WinForms;

namespace RingSoft.DbLookup.Controls.WinForms
{
    /// <summary>
    /// Displays lookup data based on the setup defined in the lookup definition.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="ILookupUserInterface" />
    public partial class LookupControl : UserControl, ILookupUserInterface, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the number of rows on a page.
        /// </summary>
        /// <value>
        /// The number of rows on the page.
        /// </value>
        public int PageSize => _currentPageSize;
        /// <summary>
        /// Gets the type of the search.
        /// </summary>
        /// <value>
        /// The type of the search.
        /// </value>
        public LookupSearchTypes SearchType => GetLookupSearchType();

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        public string SearchText
        {
            get => SearchForTextBox.Text;
            set
            {
                _resettingSearchFor = true;
                SearchForTextBox.Text = value;
                _resettingSearchFor = false;
            }
        }

        /// <summary>
        /// Gets the lookup data.
        /// </summary>
        /// <value>
        /// The lookup data.
        /// </value>
        public LookupDataBase LookupData { get; private set; }

        private LookupDefinitionBase _lookupDefinition;
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>
        /// The lookup definition.
        /// </value>
        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                if (_lookupDefinition == value)
                    return;

                _lookupDefinition = value;
                SetupControl(_lookupDefinition);
            }
        }

        private LookupCommand _lookupCommand;

        public LookupCommand Command
        {
            get => _lookupCommand;
            set
            {
                if (_lookupCommand == value)
                    return;

                _lookupCommand = value;
                ExecuteCommand(_lookupCommand);
                OnPropertyChanged(nameof(Command));
            }
        }

        private LookupDataSourceChanged _dataSourceChanged;

        public LookupDataSourceChanged DataSourceChanged
        {
            get => _dataSourceChanged;
            set
            {
                if (_dataSourceChanged == value)
                    return;

                _dataSourceChanged = value;
                OnPropertyChanged(nameof(DataSourceChanged));
            }
        }

        public bool IsCleared
        {
            get
            {
                return LookupData == null || LookupData.LookupResultsDataTable == null ||
                       LookupData.LookupResultsDataTable.Rows.Count == 0;
            }
        }

        private int _originalPageSize;
        private int _currentPageSize;
        private int _timerInterval;
        private bool _refreshPending;
        private string _refreshPendingSearchFor;
        private bool _resettingSearchFor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupControl"/> class.
        /// </summary>
        public LookupControl()
        {
            InitializeComponent();
            ContainsRadioButton.TabStop = EqualsRadioButton.TabStop = false;
            LookupListView.ColumnClick += LookupListView_ColumnClick;
            LookupListView.ColumnWidthChanging += LookupListView_ColumnWidthChanging;
            LookupListView.SelectedIndexChanged += LookupListView_SelectedIndexChanged;

            SearchForTextBox.KeyDown += SearchForTextBox_KeyDown;
            LookupListView.KeyDown += SearchForTextBox_KeyDown;
            SearchForTextBox.TextChanged += SearchForTextBox_TextChanged;

            LookupListView.MouseDoubleClick += LookupListView_MouseDoubleClick;

            ScrollBar.Scroll += ScrollBarOnScroll;
            EqualsRadioButton.Click += SearchTypeRadioButton_Click;
            ContainsRadioButton.Click += SearchTypeRadioButton_Click;

            GetRecordCountButton.Click += GetRecordCountButton_Click;
            RecordCountTimer.Tick += RecordCountTimer_Tick;
        }

        private void ScrollBarOnScroll(object sender, ScrollEventArgs e)
        {
            if (IsCleared)
                return;

            switch (e.Type)
            {
                case ScrollEventType.SmallDecrement:
                    OnUpArrow();
                    break;
                case ScrollEventType.SmallIncrement:
                    OnDownArrow();
                    break;
                case ScrollEventType.LargeDecrement:
                    OnPageUp(false);
                    break;
                case ScrollEventType.LargeIncrement:
                    OnPageDown(false);
                    break;
                case ScrollEventType.ThumbPosition:
                    if (e.NewValue == ScrollBar.Minimum)
                        OnHome(false);
                    else if (e.NewValue + ScrollBar.LargeChange >= ScrollBar.Maximum)
                        OnEnd(false);
                    break;
            }

            e.NewValue = ScrollBar.Value;
        }

        private void LookupListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (IsCleared)
                return;

            OnEnter();
        }

        private void LookupListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            LookupData.SelectedRowIndex = LookupListView.GetSelectedIndex();
        }

        private void SearchForTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsCleared)
                return;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    OnDownArrow();
                    e.Handled = true;
                    break;
                case Keys.Up:
                    OnUpArrow();
                    e.Handled = true;
                    break;
                case Keys.PageDown:
                    OnPageDown();
                    e.Handled = true;
                    break;
                case Keys.PageUp:
                    OnPageUp();
                    e.Handled = true;
                    break;
                case Keys.End:
                    OnEnd();
                    e.Handled = true;
                    break;
                case Keys.Home:
                    OnHome();
                    e.Handled = true;
                    break;
                case Keys.Enter:
                    OnEnter();
                    break;
            }
        }

        private void LookupListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            //if (IsCleared)
            //    return;
        }

        private void LookupListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (IsCleared)
                return;

            bool resetSortOrder = !(ModifierKeys.HasFlag(Keys.Control)
                                    && LookupData.LookupDefinition.GetIndexOfVisibleColumn(LookupData.SortColumnDefinition) != e.Column);
            LookupData.OnColumnClick(e.Column, resetSortOrder);

            if (resetSortOrder)
            {
                var sortOrder = SortOrder.Ascending;
                if (LookupData.OrderByType == OrderByTypes.Descending)
                    sortOrder = SortOrder.Descending;

                LookupListView.SetSortIcon(e.Column, sortOrder);
                SetActiveColumn();
                SearchForTextBox.Text = string.Empty;
            }

            for (int i = 0; i < LookupListView.Columns.Count; i++)
            {
                var listColumn = LookupListView.Columns[i];
                var lookColumn = LookupData.LookupDefinition.VisibleColumns[i];
                listColumn.Text = lookColumn.Caption;
            }

            var columnNumber = 1;
            foreach (var lookupColumnDefinition in LookupData.OrderByList)
            {
                LookupListView.Columns[LookupData.LookupDefinition.GetIndexOfVisibleColumn(lookupColumnDefinition)]
                    .Text += $@" - {columnNumber.ToString()}";
                columnNumber++;
            }
            SearchForTextBox.Focus();
        }

        private void SetupControl([NotNull] LookupDefinitionBase lookupDefinition)
        {
            LookupData = new LookupDataBase(lookupDefinition, this);

            LookupData.LookupDataChanged += LookupData_LookupDataChanged;
            LookupData.DataSourceChanged += LookupData_DataSourceChanged;
            LookupListView.SetupColumns(LookupData.LookupDefinition);

            var sortOrder = SortOrder.Ascending;
            if (LookupData.OrderByType == OrderByTypes.Descending)
                sortOrder = SortOrder.Descending;

            LookupListView.SetSortIcon(LookupData.LookupDefinition.GetIndexOfVisibleColumn(LookupData.SortColumnDefinition), sortOrder);
            SetActiveColumn();

            if (ContainsFocus)
                ActiveControl = SearchForTextBox;

            if (_refreshPending)
            {
                RefreshData(true, _refreshPendingSearchFor);
                _refreshPending = false;
                _refreshPendingSearchFor = string.Empty;
            }
        }

        private void SearchTypeRadioButton_Click(object sender, EventArgs e)
        {
            if (IsCleared)
                return;

            if (!SearchText.IsNullOrEmpty())
                LookupData.ResetRecordCount();

            SearchForTextBox.Focus();
            RefreshData(false, SearchForTextBox.Text);
        }

        private void SetActiveColumn()
        {
            var labelWidth = SearchForLabel.Width;
            SearchForLabel.Text = $@"Search For {LookupData.SortColumnDefinition.Caption}";
            if (LookupData.SortColumnDefinition.DataType == FieldDataTypes.String)
            {
                ContainsRadioButton.Enabled = true;
            }
            else
            {
                ContainsRadioButton.Enabled = false;
                EqualsRadioButton.Checked = true;
            }

            var delta = SearchForLabel.Width - labelWidth;
            SearchForTextBox.Left += delta;
            SearchForTextBox.Width -= delta;
            ContainsRadioButton.Left += delta;
            EqualsRadioButton.Left += delta;
        }

        private void LookupData_LookupDataChanged(object sender, EventArgs e)
        {
            LookupListView.Items.Clear();
            foreach (DataRow dataRow in LookupData.LookupResultsDataTable.Rows)
            {
                ListViewItem item = null;
                foreach (var lookupDefinitionColumn in LookupData.LookupDefinition.VisibleColumns)
                {
                    var cellValue = dataRow.GetRowValue(lookupDefinitionColumn.SelectSqlAlias);
                    cellValue = lookupDefinitionColumn.FormatValue(cellValue);
                    if (item == null)
                    {
                        item = LookupListView.Items.Add(cellValue);
                    }
                    else
                    {
                        item.SubItems.Add(cellValue);
                    }
                }
            }

            ScrollBar.Enabled = true;
            switch (LookupData.ScrollPosition)
            {
                case LookupScrollPositions.Disabled:
                    ScrollBar.Enabled = false;
                    break;
                case LookupScrollPositions.Top:
                    ScrollBar.Value = ScrollBar.Minimum;
                    break;
                case LookupScrollPositions.Middle:
                    double middleValue = Math.Floor((double)(ScrollBar.Maximum - ScrollBar.Minimum) / 2);
                    ScrollBar.Value = (int)middleValue - 5;
                    break;
                case LookupScrollPositions.Bottom:
                    ScrollBar.Value = ScrollBar.Maximum;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            LookupListView.SelectItem(LookupData.SelectedRowIndex);
            SetupRecordCount();
        }

        private void LookupData_DataSourceChanged(object sender, EventArgs e)
        {
            DataSourceChanged = new LookupDataSourceChanged();
        }

        public void RefreshData(bool resetSearchFor, string initialSearchFor = "")
        {
            if (LookupData == null)
            {
                _refreshPending = true;
                _refreshPendingSearchFor = initialSearchFor;
                return;
            }

            if (!resetSearchFor && initialSearchFor.IsNullOrEmpty())
                initialSearchFor = SearchForTextBox.Text;

            _currentPageSize = GetPageSize();

            if (string.IsNullOrEmpty(initialSearchFor))
            {
                SearchForTextBox.Text = string.Empty;
                LookupData.GetInitData(true);
            }
            else
            {
                var forceRefresh = SearchForTextBox.Text == initialSearchFor || IsCleared;
                SearchForTextBox.Text = initialSearchFor; //This automatically triggers LookupData.OnSearchForChange.  Only if the text value has changed.
                if (forceRefresh)
                    LookupData.OnSearchForChange(initialSearchFor);
            }
        }
        private int GetPageSize()
        {
            int itemHeight;
            var lookupHeight = LookupListView.DisplayRectangle.Height;
            if (LookupListView.Items.Count <= 0)
            {
                LookupListView.Items.Add("text");
                itemHeight = LookupListView.GetItemRect(0).Height;
                lookupHeight -= LookupListView.TopItem.Bounds.Top;
                LookupListView.Items.Clear();
            }
            else
            {
                lookupHeight -= LookupListView.TopItem.Bounds.Top;
                itemHeight = LookupListView.GetItemRect(0).Height;
            }

            double items = 10;
            if (itemHeight > 0)
                items = (double)lookupHeight / itemHeight;

            var pageSize = (int)(Math.Floor(items));

            var columnsWidth = 0;
            foreach (ColumnHeader columnHeader in LookupListView.Columns)
            {
                columnsWidth += columnHeader.Width;
            }

            if (columnsWidth > LookupListView.DisplayRectangle.Width)
                pageSize--;

            _originalPageSize = pageSize;
            return pageSize;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            var originalPageSize = _originalPageSize;
            var newPageSize = _currentPageSize = GetPageSize();

            if (originalPageSize != newPageSize && originalPageSize > 0)
            {
                LookupData.OnChangePageSize();
            }
            SearchForTextBox.Width = ScrollBar.Right - SearchForTextBox.Left;
        }

        protected void OnDownArrow()
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex >= LookupListView.Items.Count - 1)
            {
                LookupData.GotoNextRecord();
            }
            else
            {
                LookupListView.SelectItem(selIndex + 1);
            }
        }

        protected void OnUpArrow()
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex <= 0)
            {
                LookupData.GotoPreviousRecord();
            }
            else
            {
                LookupListView.SelectItem(selIndex - 1);
            }
        }

        private void OnPageDown(bool checkSelectedIndex = true)
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex >= LookupListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoNextPage();

            LookupListView.SelectItem(LookupListView.Items.Count - 1);
        }

        private void OnPageUp(bool checkSelectedIndex = true)
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoPreviousPage();

            LookupListView.SelectItem(0);
        }

        private void OnEnd(bool checkSelectedIndex = true)
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex >= LookupListView.Items.Count - 1 || !checkSelectedIndex)
                LookupData.GotoBottom();

            LookupListView.SelectItem(LookupListView.Items.Count - 1);
        }

        private void OnHome(bool checkSelectedIndex = true)
        {
            var selIndex = LookupListView.GetSelectedIndex();
            if (selIndex <= 0 || !checkSelectedIndex)
                LookupData.GotoTop();

            LookupListView.SelectItem(0);
        }

        private void OnEnter()
        {
            var selectedIndex = LookupListView.GetSelectedIndex();
            if (selectedIndex >= 0)
                LookupData.ViewSelectedRow(selectedIndex);
        }

        private LookupSearchTypes GetLookupSearchType()
        {
            if (EqualsRadioButton.Checked)
                return LookupSearchTypes.Equals;

            return LookupSearchTypes.Contains;
        }

        private void SearchForTextBox_TextChanged(object sender, EventArgs e)
        {
            if (LookupData == null || LookupData.LookupResultsDataTable == null)
                return;

            if (_resettingSearchFor)
                return;

            LookupData.OnSearchForChange(SearchForTextBox.Text);
        }

        private async void GetRecordCountButton_Click(object sender, EventArgs e)
        {
            ShowRecordCountLabel();
            RecordCountTimer.Start();
            var processComplete = await LookupData.GetRecordCount();
            RecordCountTimer.Stop();
            _timerInterval = 0;
            if (processComplete)
            {
                if (!GetRecordCountButton.Visible)
                    SetupRecordCount();
            }
        }

        public void SetupRecordCount()
        {
            var showRecordCount = false;
            if (LookupData.ScrollPosition == LookupScrollPositions.Disabled)
            {
                showRecordCount = true;
            }
            else if (LookupData.RecordCount > 0)
                showRecordCount = true;

            if (showRecordCount)
            {
                ShowRecordCountLabel();
                var recordsText = LookupData.RecordCount == 1 ? "" : "s";
                RecordCountLabel.Text =
                    $@"{LookupData.RecordCount.ToString(GblMethods.GetNumFormat(0, false))} Record{recordsText} Found";
            }
            else
            {
                RecordCountLabel.Visible = false;
                GetRecordCountButton.Visible = true;
            }
        }

        private void ShowRecordCountLabel()
        {
            GetRecordCountButton.Visible = false;
            RecordCountLabel.Visible = true;
            RecordCountLabel.Location = GetRecordCountButton.Location;
            RecordCountLabel.Text = @"Counting Records";
        }

        private void RecordCountTimer_Tick(object sender, EventArgs e)
        {
            _timerInterval++;
            RecordCountLabel.Text = $@"Counting Records {GblMethods.StringDuplicate(".", _timerInterval)}";
            if (_timerInterval > 8)
                _timerInterval = 0;
        }

        private void ExecuteCommand(LookupCommand command)
        {
            if (command != null)
            {
                switch (command.Command)
                {
                    case LookupCommands.Clear:
                        ClearLookupControl();
                        break;
                    case LookupCommands.Refresh:
                        LookupData.ParentWindowPrimaryKeyValue = command.ParentWindowPrimaryKeyValue;
                        RefreshData(command.ResetSearchFor);
                        break;
                    case LookupCommands.AddModify:
                        var selectedIndex = LookupListView.GetSelectedIndex();
                        if (selectedIndex >= 0)
                            LookupData.ViewSelectedRow(selectedIndex);
                        else
                            LookupData.AddNewRow();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        /// <summary>
        /// Clears the lookup control.
        /// </summary>
        public void ClearLookupControl()
        {
            _refreshPending = false;
            _refreshPendingSearchFor = string.Empty;
            if (LookupData != null)
            {
                LookupData.ClearLookupData();
            }

            EqualsRadioButton.Checked = true;
            SearchForTextBox.Text = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
