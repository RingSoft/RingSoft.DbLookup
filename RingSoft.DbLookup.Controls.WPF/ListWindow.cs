using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlX.XDevAPI.Relational;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
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
    ///     <MyNamespace:ListWindow/>
    ///
    /// </summary>
    public class ListWindow : BaseWindow, IListWindowView
    {
        public Border Border { get; private set; }

        public ListWindowViewModel ViewModel { get; private set; }

        public StackPanel SearchForStackPanel { get; set; }

        public Label SearchForLabel { get; set; }

        public ListView ListView { get; set; }

        public GridView LookupGridView { get; set; }

        public ScrollBar ScrollBar { get; set; }

        private LookupSearchForHost _lookupSearchForHost;
        public LookupSearchForHost SearchForHost
        {
            get => _lookupSearchForHost;
            set
            {
                if (_lookupSearchForHost != null)
                {
                    SearchForHost.PreviewKeyDown -= SearchForControl_PreviewKeyDown;
                    SearchForHost.TextChanged -= SearchForControl_TextChanged;
                    SearchForHost.Control.PreviewLostKeyboardFocus -= Control_PreviewLostKeyboardFocus;
                }

                _lookupSearchForHost = value;

                if (_lookupSearchForHost != null)
                {
                    SearchForHost.PreviewKeyDown += SearchForControl_PreviewKeyDown;
                    SearchForHost.TextChanged += SearchForControl_TextChanged;
                    SearchForHost.Control.PreviewLostKeyboardFocus += Control_PreviewLostKeyboardFocus;
                }
            }
        }

        private DataTable _dataSource = new DataTable("DataSourceTable");
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        GridViewColumnHeader _lastHeaderClicked;
        private ListControlColumn _sortColumn;

        static ListWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListWindow), new FrameworkPropertyMetadata(typeof(ListWindow)));
        }

        public ListWindow(ListControlSetup setup, ListControlDataSource dataSource)
        {
            Loaded += (sender, args) =>
            {
                SnugWidth = 500;
                SnugHeight = 500;
                SnugWindow();
                ViewModel.Initialize(setup, dataSource, this);
            };
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("ViewModel") as ListWindowViewModel;

            SearchForLabel = GetTemplateChild(nameof(SearchForLabel)) as Label;
            SearchForStackPanel = GetTemplateChild(nameof(SearchForStackPanel)) as StackPanel;
            ListView = GetTemplateChild(nameof(ListView)) as ListView;
            LookupGridView = GetTemplateChild(nameof(LookupGridView)) as GridView;
            ScrollBar = GetTemplateChild(nameof(ScrollBar)) as ScrollBar;

            base.OnApplyTemplate();
        }

        public void CloseWindow()
        {
            Close();
        }

        private void SearchForControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            OnListViewKeyDown(e);
        }

        private void SearchForControl_TextChanged(object sender, EventArgs e)
        {
        }

        private void Control_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ReferenceEquals(e.NewFocus, ListView))
                e.Handled = true;
        }

        private void OnListViewKeyDown(KeyEventArgs e)
        {
            if (SearchForHost != null && SearchForHost.Control.IsKeyboardFocusWithin)
            {
                if (!SearchForHost.CanProcessSearchForKey(e.Key))
                {
                    return;
                }
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                int? sortColumnIndex = null;
                switch (e.Key)
                {
                    case Key.D1:
                    case Key.NumPad1:
                        sortColumnIndex = 0;
                        break;
                    case Key.D2:
                    case Key.NumPad2:
                        sortColumnIndex = 1;
                        break;
                    case Key.D3:
                    case Key.NumPad3:
                        sortColumnIndex = 2;
                        break;
                    case Key.D4:
                    case Key.NumPad4:
                        sortColumnIndex = 3;
                        break;
                    case Key.D5:
                    case Key.NumPad5:
                        sortColumnIndex = 4;
                        break;
                    case Key.D6:
                    case Key.NumPad6:
                        sortColumnIndex = 5;
                        break;
                    case Key.D7:
                    case Key.NumPad7:
                        sortColumnIndex = 6;
                        break;
                    case Key.D8:
                    case Key.NumPad8:
                        sortColumnIndex = 7;
                        break;
                    case Key.D9:
                    case Key.NumPad9:
                        sortColumnIndex = 8;
                        break;
                    case Key.D0:
                    case Key.NumPad0:
                        sortColumnIndex = 9;
                        break;
                }

                if (sortColumnIndex != null)
                {
                    OnColumnClick((int)sortColumnIndex, false);
                    e.Handled = true;
                    return;
                }
            }

            switch (e.Key)
            {
                case Key.Down:
                    OnDownArrow();
                    e.Handled = true;
                    break;
                case Key.Up:
                    OnUpArrow();
                    e.Handled = true;
                    break;
                case Key.PageDown:
                    OnPageDown();
                    e.Handled = true;
                    break;
                case Key.PageUp:
                    OnPageUp();
                    e.Handled = true;
                    break;
                case Key.End:
                    OnEnd();
                    e.Handled = true;
                    break;
                case Key.Home:
                    OnHome();
                    e.Handled = true;
                    break;
                case Key.Enter:
                    if (OnEnter())
                        e.Handled = true;
                    break;
            }
        }

        protected void OnDownArrow()
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1)
            {
                {
                    //LookupData.GotoNextRecord();
                }
            }
            else
            {
                ListView.SelectedIndex = selIndex + 1;
            }
        }

        protected void OnUpArrow()
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0)
            {
                {
                    //LookupData.GotoPreviousRecord();
                }
            }
            else
            {
                ListView.SelectedIndex = selIndex - 1;
            }
        }

        private void OnPageDown(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //LookupData.OnMouseWheelForward(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
            {
                //LookupData.GotoNextPage();
            }
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnPageUp(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            //LookupData.OnMouseWheelBack(); //For debugging purposes only. I'm a quadriplegic and it's very difficult for me to use a mouse wheel.

            //Comment out below code block when debugging mouse wheel.

            var selIndex = ListView.SelectedIndex;
            if (selIndex <= 0 || !checkSelectedIndex)
            {
                //LookupData.GotoPreviousPage();
            }
            else
                ListView.SelectedIndex = 0;
        }

        private void OnEnd(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex >= ListView.Items.Count - 1 || !checkSelectedIndex)
            {
                
                //LookupData.GotoBottom();
            }
            else
                ListView.SelectedIndex = ListView.Items.Count - 1;
        }

        private void OnHome(bool checkSelectedIndex = true)
        {
            if (ListView == null)
                return;

            var selIndex = ListView.SelectedIndex;

            if (selIndex <= 0 || !checkSelectedIndex)
            {

            }
            else
                ListView.SelectedIndex = 0;
        }

        private bool OnEnter()
        {
            return true;
        }

        public void InitializeListView()
        {
            LookupGridView?.Columns.Clear();
            foreach (var column in ViewModel.ControlSetup.ColumnList)
            {
                double columnWidth = 100;
                if (ListView != null)
                    columnWidth = LookupControl.GetWidthFromPercent(ListView, column.PercentWidth);

                AddColumnToGrid(column, columnWidth);

            }

            InitializeHeader(0);
            SetActiveColumn(0, FieldDataTypes.String);
        }

        private void AddColumnToGrid(ListControlColumn column, double width)
        {
            var gridColumn = AddGridViewColumn(column.Caption, width, $"COLUMN{column.ColumnId}");
        }


        private GridViewColumn AddGridViewColumn(string caption, double width, string dataColumnName)
        {
            var columnHeader = new GridViewColumnHeader { Content = caption }; // + "\r\nLine2\r\nLine3"};
            columnHeader.Click += GridViewColumnHeaderClickedHandler;

            if (width < 0)
            {
                width = 20;
            }
            var gridColumn = new GridViewColumn
            {
                Header = columnHeader,
                Width = width,
            };
            LookupGridView?.Columns.Add(gridColumn);
            _dataSource.Columns.Add(dataColumnName);

            return gridColumn;
        }
        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null)
            {
                var columnIndex = LookupGridView.Columns.IndexOf(headerClicked.Column);
                if (columnIndex != -1)
                    OnColumnClick(columnIndex, true);
            }
        }

        private void OnColumnClick(int columnIndex, bool mouseClick)
        {
            if (LookupGridView == null)
                return;

            if (columnIndex > LookupGridView.Columns.Count - 1)
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            _sortColumn = ViewModel.ControlSetup.ColumnList[columnIndex];
            var headerClicked = LookupGridView.Columns[columnIndex].Header as GridViewColumnHeader;
            if (headerClicked == null)
                return;

            ListSortDirection direction = _lastDirection;

            if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                var controlKeyPressed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                var resetSortOrder = !controlKeyPressed || _lastHeaderClicked == headerClicked;
                if (!mouseClick)
                    resetSortOrder = true;

                if (resetSortOrder)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                }

                if (resetSortOrder)
                {
                    GridViewSort.ApplySort(direction, ListView, headerClicked);
                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }

                if (SearchForHost != null && resetSortOrder)
                    SearchForHost.SearchText = String.Empty;


                for (int i = 0; i < LookupGridView.Columns.Count; i++)
                {
                    var gridColumn = LookupGridView.Columns[i];
                    if (gridColumn != _lastHeaderClicked.Column)
                    {
                        var columnHeader = gridColumn.Header as GridViewColumnHeader;
                        GridViewSort.RemoveSortGlyph(columnHeader);
                    }
                }

                var columnNumber = 1;
                foreach (var lookupColumn in ViewModel.OrderByList)
                {
                    var orderColumnIndex = GetIndexOfVisibleColumnDefinition(lookupColumn);
                    var orderGridColumnHeader =
                        LookupGridView.Columns[orderColumnIndex].Header as GridViewColumnHeader;
                    GridViewSort.AddNonPrimarySortGlyph(orderGridColumnHeader, columnNumber);
                    columnNumber++;
                }

                if (resetSortOrder)
                    SetActiveColumn(columnIndex, _sortColumn.DataType);

                SearchForHost?.Control.Focus();
            }
        }
        private int GetIndexOfVisibleColumnDefinition(ListControlColumn listControlColumn)
        {
            var lookupColumn = ViewModel.ControlSetup.ColumnList.FirstOrDefault(listControlColumn);
            return ViewModel.ControlSetup.GetIndexOfColumn(lookupColumn);
        }
        private void SetActiveColumn(int sortColumnIndex, FieldDataTypes dataType)
        {
            if (SearchForStackPanel != null)
            {
                if (SearchForHost != null)
                {
                    SearchForHost = null;
                    SearchForStackPanel.Children.Clear();
                }
                SearchForHost =
                    LookupControlsGlobals.LookupControlSearchForFactory.CreateSearchForHost(dataType);

                SearchForStackPanel.Children.Add(SearchForHost.Control);
                SearchForStackPanel.UpdateLayout();

                SearchForHost.Control.Loaded += (sender, args) =>
                {
                    SearchForStackPanel.Height = SearchForHost.Control.ActualHeight;
                    if (IsKeyboardFocusWithin)
                    {
                        SearchForHost.Control.Focus();
                        SearchForHost.SetFocusToControl();
                    }
                };
            }

            var column = ViewModel.ControlSetup.ColumnList[sortColumnIndex];

            if (!column.Caption.IsNullOrEmpty())
            {
                var headerText = column.Caption.Replace('\n', ' ').Replace("\r", "");
                if (SearchForLabel != null)
                    SearchForLabel.Content = $@"Search For {headerText}";
            }

        }

        private void InitializeHeader(int sortColumnIndex)
        {
            if (ListView == null || LookupGridView == null)
                return;

            GridViewHeaderRowPresenter header = (GridViewHeaderRowPresenter)LookupGridView.GetType()
                .GetProperty("HeaderRowPresenter", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(LookupGridView);

            if (header != null && LookupGridView.Columns.Any())
            {

                var column = LookupGridView.Columns[sortColumnIndex];
                var columnHeader = (GridViewColumnHeader)column.Header;
                var glyphSize = GridViewSort.GetGlyphSize(columnHeader, ListSortDirection.Ascending, ListView);

                Style style = new Style();
                style.TargetType = typeof(GridViewColumnHeader);
                var height = GetHeaderHeight(header);
                style.Setters.Add(new Setter(GridViewColumnHeader.HeightProperty, height + glyphSize.Height + 5));
                style.Setters.Add(new Setter(GridViewColumnHeader.VerticalContentAlignmentProperty, VerticalAlignment.Bottom));

                LookupGridView.ColumnHeaderContainerStyle = style;
                LookupGridView.ColumnHeaderTemplate = GetColumnHeaderDataTemplate();

                header.UpdateLayout();
            }

            ResetColumnHeaderSort(sortColumnIndex);
        }

        private double GetHeaderHeight(GridViewHeaderRowPresenter header)
        {
            var height = header.ActualHeight;
            var lineFeedCount = 0;
            var startIndex = 0;

            foreach (var column in ViewModel.ControlSetup.ColumnList)
            {
                var columnLineCount = 0;
                if (!column.Caption.IsNullOrEmpty())
                {
                    while (startIndex >= 0)
                    {
                        startIndex = column.Caption.IndexOf('\n', startIndex);
                        if (startIndex >= 0)
                        {
                            columnLineCount++;
                            startIndex++;
                        }
                    }

                    startIndex = 0;
                }

                if (lineFeedCount < columnLineCount)
                    lineFeedCount = columnLineCount;
            }

            height = 20 * (lineFeedCount + 1);

            return height;
        }
        private DataTemplate GetColumnHeaderDataTemplate()
        {
            var template = new DataTemplate();

            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);

            var binding = new Binding(TextBlock.TextProperty.Name);
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent);
            binding.Path = new PropertyPath(nameof(GridViewColumnHeader.Content));
            factory.SetBinding(TextBlock.TextProperty, binding);

            template.VisualTree = factory;

            return template;
        }

        private void ResetColumnHeaderSort(int sortColumnIndex)
        {
            if (ListView == null || LookupGridView == null)
                return;

            if (!LookupGridView.Columns.Any())
                return;

            var sortColumn = LookupGridView.Columns[sortColumnIndex];
            _lastHeaderClicked = sortColumn.Header as GridViewColumnHeader;

            _lastDirection = ListSortDirection.Ascending;

            GridViewSort.ApplySort(_lastDirection, ListView, _lastHeaderClicked);
        }


    }
}
