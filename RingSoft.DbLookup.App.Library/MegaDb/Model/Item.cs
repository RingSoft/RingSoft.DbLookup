namespace RSDbLookupApp.Library.MegaDb.Model
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
    }
}
