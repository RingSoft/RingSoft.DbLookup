namespace RSDbLookupApp.Library.DevLogix.LookupModel
{
    public class IssueLookup
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Project { get; set; }

        public string Task { get; set; }

        public string Resolved { get; set; }

        public double TaskPercentComplete { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
