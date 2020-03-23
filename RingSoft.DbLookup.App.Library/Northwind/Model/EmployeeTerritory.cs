namespace RSDbLookupApp.Library.Northwind.Model
{
    public class EmployeeTerritory
    {
        public int EmployeeID { get; set; }

        public virtual Employee Employee { get; set; }

        public string TerritoryID { get; set; }

        public virtual Territory Territory { get; set; }
    }
}
