using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup
{
    public class DataEntryGridAutoFillCellProps : DataEntryGridEditingCellProps
    {
        public override string DataValue
        {
            get
            {
                if (AutoFillValue != null)
                    return AutoFillValue.Text;

                return string.Empty;
            }
        }

        public AutoFillSetup AutoFillSetup { get; }

        public AutoFillValue AutoFillValue { get; set; }

        public const int AutoFillControlHostId = 51;

        public DataEntryGridAutoFillCellProps(DataEntryGridRow row, int columnId, AutoFillSetup setup, AutoFillValue value) : base(row, columnId)
        {
            AutoFillSetup = setup;
            AutoFillValue = value;
        }

        public override int EditingControlId => AutoFillControlHostId;
    }
}
