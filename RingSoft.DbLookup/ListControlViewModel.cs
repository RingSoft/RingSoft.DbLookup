// ***********************************************************************
// Assembly         : RingSoft.DbLookup
// Author           : petem
// Created          : 03-04-2023
//
// Last Modified By : petem
// Last Modified On : 07-24-2023
// ***********************************************************************
// <copyright file="ListControlViewModel.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup
{
    /// <summary>
    /// Interface IListControlView
    /// </summary>
    public interface IListControlView
    {
        /// <summary>
        /// Shows the lookup window.
        /// </summary>
        /// <returns>ListControlDataSourceRow.</returns>
        ListControlDataSourceRow ShowLookupWindow();

        /// <summary>
        /// Selects the data row.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        void SelectDataRow(ListControlDataSourceRow selectedRow);
    }
    /// <summary>
    /// Class ListControlColumn.
    /// </summary>
    public class ListControlColumn
    {
        /// <summary>
        /// Gets the column identifier.
        /// </summary>
        /// <value>The column identifier.</value>
        public int ColumnId { get; internal set; }
        /// <summary>
        /// Gets the caption.
        /// </summary>
        /// <value>The caption.</value>
        public string Caption { get; internal set; }
        /// <summary>
        /// Gets the width of the percent.
        /// </summary>
        /// <value>The width of the percent.</value>
        public double PercentWidth { get; internal set; }
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public FieldDataTypes DataType { get; internal set; }
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        /// <value>The name of the column.</value>
        public string ColumnName => $"COLUMN{ColumnId}";
    }

    /// <summary>
    /// List Control Setup.
    /// </summary>
    public class ListControlSetup
    {
        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public ListControlColumn SortColumn { get; set; }

        /// <summary>
        /// Gets or sets the type of the order by.
        /// </summary>
        /// <value>The type of the order by.</value>
        public OrderByTypes OrderByType { get; set; }

        /// <summary>
        /// Gets the column list.
        /// </summary>
        /// <value>The column list.</value>
        public IReadOnlyList<ListControlColumn> ColumnList => _columns.AsReadOnly();

        /// <summary>
        /// The columns
        /// </summary>
        private List<ListControlColumn> _columns = new List<ListControlColumn>();

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="columnId">The column identifier.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="percentWidth">Width of the percent.</param>
        /// <returns>ListControlColumn.</returns>
        public ListControlColumn AddColumn(int columnId, string caption, FieldDataTypes dataType, double percentWidth)
        {
            var column = new ListControlColumn
            {
                ColumnId = columnId,
                Caption = caption,
                DataType = dataType,
                PercentWidth = percentWidth,
            };
            _columns.Add(column);
            if (SortColumn == null)
            {
                SortColumn = column;
            }
            return column;
        }

        /// <summary>
        /// Gets the index of column.
        /// </summary>
        /// <param name="listControlColumn">The list control column.</param>
        /// <returns>System.Int32.</returns>
        public int GetIndexOfColumn(ListControlColumn listControlColumn)
        {
            return _columns.IndexOf(listControlColumn);
        }


    }

    /// <summary>
    /// Class ListControlDataCell.
    /// </summary>
    public class ListControlDataCell
    {
        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        /// <value>The text value.</value>
        public string TextValue { get; set; }

        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        /// <value>The numeric value.</value>
        public double NumericValue { get; set; }

        /// <summary>
        /// Gets the column.
        /// </summary>
        /// <value>The column.</value>
        public ListControlColumn Column { get; internal set; }

        /// <summary>
        /// Gets the sort value.
        /// </summary>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public object GetSortValue()
        {
            switch (Column.DataType)
            {
                case FieldDataTypes.String:
                    return TextValue;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return NumericValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public override string ToString()
        {
            switch (Column.DataType)
            {
                case FieldDataTypes.String:
                    return TextValue;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    return NumericValue.ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    /// <summary>
    /// Class ListControlDataSourceRow.
    /// </summary>
    public class ListControlDataSourceRow
    {
        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>The columns.</value>
        public IReadOnlyList<ListControlColumn> Columns => _columns.AsReadOnly();

        /// <summary>
        /// Gets the data cells.
        /// </summary>
        /// <value>The data cells.</value>
        public IReadOnlyList<ListControlDataCell> DataCells => _dataCells.AsReadOnly();

        /// <summary>
        /// The columns
        /// </summary>
        private List<ListControlColumn> _columns = new List<ListControlColumn>();
        /// <summary>
        /// The data cells
        /// </summary>
        private List<ListControlDataCell> _dataCells = new List<ListControlDataCell>();
        /// <summary>
        /// Gets the cell item.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>System.String.</returns>
        public string GetCellItem(int columnIndex)
        {
            return DataCells[columnIndex].TextValue;
        }

        /// <summary>
        /// Adds the column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void AddColumn(ListControlColumn column, string value)
        {
            _columns.Add(column);
            var cell = new ListControlDataCell();
            switch (column.DataType)
            {
                case FieldDataTypes.String:
                    cell.TextValue = value;
                    break;
                case FieldDataTypes.Integer:
                case FieldDataTypes.Decimal:
                    cell.NumericValue = value.ToDecimal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            cell.Column = column;
            _dataCells.Add(cell);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return DataCells[0].TextValue;
        }
    }

    /// <summary>
    /// Class ListControlDataSource.
    /// </summary>
    public class ListControlDataSource
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public IReadOnlyList<ListControlDataSourceRow> Items => _dataSourceItems.AsReadOnly();

        /// <summary>
        /// The data source items
        /// </summary>
        private List<ListControlDataSourceRow> _dataSourceItems = new List<ListControlDataSourceRow>();

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddRow(ListControlDataSourceRow item)
        {
            _dataSourceItems.Add(item);
        }
    }
    /// <summary>
    /// Class ListControlViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class ListControlViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The text
        /// </summary>
        private string _text;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                {
                    return;
                }
                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public ListControlDataSource DataSource { get; set; }

        /// <summary>
        /// Gets or sets the setup.
        /// </summary>
        /// <value>The setup.</value>
        public ListControlSetup Setup { get; set; }

        /// <summary>
        /// The selected data row
        /// </summary>
        private ListControlDataSourceRow _selectedDataRow;

        /// <summary>
        /// Gets or sets the selected data row.
        /// </summary>
        /// <value>The selected data row.</value>
        public ListControlDataSourceRow SelectedDataRow
        {
            get => _selectedDataRow;
            set
            {
                if (_selectedDataRow == value)
                {
                    return;
                }
                _selectedDataRow = value;
                View.SelectDataRow(_selectedDataRow);
            }
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>The view.</value>
        public IListControlView View { get; private set; }

        /// <summary>
        /// Gets the show lookup command.
        /// </summary>
        /// <value>The show lookup command.</value>
        public RelayCommand ShowLookupCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListControlViewModel"/> class.
        /// </summary>
        public ListControlViewModel()
        {
            ShowLookupCommand = new RelayCommand(() =>
            {
                var selectedRow = View.ShowLookupWindow();
                if (selectedRow != null)
                {
                    SelectedDataRow = selectedRow;
                }
            });
        }

        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        public void Initialize(IListControlView view)
        {
            View = view;
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
