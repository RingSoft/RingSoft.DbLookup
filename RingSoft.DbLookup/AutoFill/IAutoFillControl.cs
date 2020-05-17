namespace RingSoft.DbLookup.AutoFill
{
    public interface IAutoFillControl
    {
        string EditText { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }
    }
}
