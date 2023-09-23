using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.AutoFill
{
    public enum AutoFillRefreshModes
    {
        DbSelect = 1,
        PkRefresh = 2,
        DbDelete = 3,
    }

    public interface IAutoFillControl
    {
        string EditText { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }

        void RefreshValue(LookupCallBackToken token);

        void OnSelect();
    }
}
