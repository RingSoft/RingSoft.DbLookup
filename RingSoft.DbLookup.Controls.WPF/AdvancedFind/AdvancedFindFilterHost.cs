using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class AdvancedFindFilterHost : DataEntryGridAdvancedFindMemoHost
    {
        protected string Text { get; set; }

        protected AdvancedFindFilterCellProps CellProps { get; set; }

        private bool _dirty;

        public AdvancedFindFilterHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridEditingCellProps GetCellValue()
        {
            return CellProps;
        }

        public override bool HasDataChanged()
        {
            return _dirty;
        }

        protected override void OnControlLoaded(AutoFillMemoCellControl control, DataEntryGridEditingCellProps cellProps,
            DataEntryGridCellStyle cellStyle)
        {
            CellProps = cellProps as AdvancedFindFilterCellProps;
            
            base.OnControlLoaded(control, cellProps, cellStyle);
            Control.Text = CellProps.Text;
            control.TextBox.IsReadOnly = true;

        }

        protected override void ShowMemoEditor()
        {
            var filterWindow = new AdvancedFilterWindow();
            filterWindow.Initialize(CellProps.FilterReturn);
            filterWindow.Owner = Window.GetWindow(Control);
            filterWindow.ShowInTaskbar = false;
            var result = filterWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                CellProps = new AdvancedFindFilterCellProps(Row, ColumnId, Text, filterWindow.FilterReturn);
                if (Row is AdvancedFindFilterRow advancedFindFilterRow)
                {
                    advancedFindFilterRow.SetCellValueFromLookupReturn(filterWindow.FilterReturn);
                    //advancedFindFilterRow.Condition = CellProps.FilterReturn.Condition;
                    advancedFindFilterRow.MakeSearchValueText(CellProps.FilterReturn.SearchValue);
                    Control.TextBox.Text = advancedFindFilterRow.SearchValueText;
                }

                _dirty = true;
                OnUpdateSource(CellProps);
            }
        }

        public override bool CanGridProcessKey(Key key)
        {
            switch (key)
            {
                case Key.F2:
                    return true;
            }
            return base.CanGridProcessKey(key);
        }
    }
}
