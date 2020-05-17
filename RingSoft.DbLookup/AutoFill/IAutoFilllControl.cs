namespace RingSoft.DbLookup.AutoFill
{
    public interface IAutoFilllControl
    {
        string Text { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }
    }
}
