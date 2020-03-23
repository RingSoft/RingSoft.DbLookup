using System;
using RingSoft.DbLookup.App.Library.DevLogix.Model;

namespace RingSoft.DbLookup.App.Library.DevLogix.LookupModel
{
    public class ErrorLookup
    {
        public int Id { get; set; }

        public string ErrorNumber { get; set; }

        public DateTime Date { get; set; }

        public double HoursSpent { get; set; }

        public string DeveloperType { get; set; }

        public DeveloperTypes DevTypeEnum { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
