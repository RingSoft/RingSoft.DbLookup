using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFindMemoCellProps : DataEntryGridEditingCellProps
    {
        public enum MemoFormMode
        {
            Formula = 0,
            Caption = 1
        }

        public MemoFormMode FormMode { get; set; }

        public bool ReadOnlyMode { get; set; }

        public string Text { get; set; }

        public const int AdvancedFindMemoHostId = 52;

        public AdvancedFindMemoCellProps(DataEntryGridRow row, int columnId, string text) : base(row, columnId)
        {
            Text = text;
        }

        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            switch (FormMode)
            {
                case MemoFormMode.Caption:
                    if (!Text.IsNullOrEmpty() && Text.Contains('\n'))
                        return "<Multi-Line Caption>";
                    else
                        return Text;
                    break;
                default:
                    return "<Formula>";
            }
        }

        public override int EditingControlId => AdvancedFindMemoHostId;
    }
}
