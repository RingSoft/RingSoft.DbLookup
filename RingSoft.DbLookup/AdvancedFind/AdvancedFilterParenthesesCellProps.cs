using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DbLookup.AdvancedFind
{
    public class AdvancedFilterParenthesesCellProps : DataEntryGridTextCellProps
    {
        public const int ParenthesesHostId = 60;

        public char LimitChar { get; set; }

        public AdvancedFilterParenthesesCellProps(DataEntryGridRow row, int columnId, char limitChar) : base(row,
            columnId)
        {
            LimitChar = limitChar;  
        }

        public AdvancedFilterParenthesesCellProps(DataEntryGridRow row, int columnId, string text, char limitChar) :
            base(row, columnId, text)
        {
            LimitChar = limitChar;
        }

        public override int EditingControlId => ParenthesesHostId;
    }
}
