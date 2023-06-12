namespace RingSoft.DbLookup.App.Library.MegaDb.Model
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public int ManufacturerId { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public byte IconType { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
