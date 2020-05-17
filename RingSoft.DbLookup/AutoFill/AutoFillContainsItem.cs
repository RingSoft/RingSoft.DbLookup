namespace RingSoft.DbLookup.AutoFill
{
    /// <summary>
    /// This class represents an item in the Auto Fill control's drop down contains box.
    /// </summary>
    public class AutoFillContainsItem
    {
        /// <summary>
        /// Gets or sets the prefix text.
        /// </summary>
        /// <value>
        /// The prefix text.
        /// </value>
        public string PrefixText { get; set; }

        /// <summary>
        /// Gets or sets the contains text that is in bold.
        /// </summary>
        /// <value>
        /// The contains text.
        /// </value>
        public string ContainsText { get; set; }

        /// <summary>
        /// Gets or sets the suffix text.
        /// </summary>
        /// <value>
        /// The suffix text.
        /// </value>
        public string SuffixText { get; set; }

        public override string ToString()
        {
            return PrefixText + ContainsText + SuffixText;
        }
    }
}
