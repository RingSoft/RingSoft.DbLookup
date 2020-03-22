namespace RingSoft.DbLookupCore.AutoFill
{
    public class AutoFillContainsItem
    {
        public string PrefixText { get; set; }

        public string ContainsText { get; set; }

        public string SuffixText { get; set; }

        public override string ToString()
        {
            return PrefixText + ContainsText + SuffixText;
        }
    }
}
