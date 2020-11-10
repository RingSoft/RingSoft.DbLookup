using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DataEntryControls.NorthwindApp.Library
{
    public class DataEntryGridAutoFillCellProps : DataEntryGridCellProps
    {
        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; set; }

        public override string Text
        {
            get
            {
                if (AutoFillValue != null)
                    return AutoFillValue.Text;

                return string.Empty;
            }
        }

        public const int AutoFillControlHostId = 51;

        public DataEntryGridAutoFillCellProps(DataEntryGridRow row, int columnId, AutoFillSetup setup, AutoFillValue value) : base(row, columnId)
        {
            AutoFillSetup = setup;
            AutoFillValue = value;
        }

        public override int EditingControlId => AutoFillControlHostId;
    }
}
