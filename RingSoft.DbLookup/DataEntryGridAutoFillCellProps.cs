using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup
{
    public class DataEntryGridAutoFillCellProps : DataEntryGridEditingCellProps
    {
        protected override string GetDataValue(DataEntryGridRow row, int columnId, bool controlMode)
        {
            if (AutoFillValue != null)
                return AutoFillValue.Text;

            return string.Empty;
        }

        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; set; }

        public const int AutoFillControlHostId = 51;

        public bool AlwaysUpdateOnSelect { get; set; }

        public bool TabOnSelect { get; set; } = true;

        public DataEntryGridAutoFillCellProps(DataEntryGridRow row, int columnId, AutoFillSetup setup, AutoFillValue value) : base(row, columnId)
        {
            AutoFillSetup = setup;
            AutoFillValue = value;
        }

        public override int EditingControlId => AutoFillControlHostId;
    }
}
