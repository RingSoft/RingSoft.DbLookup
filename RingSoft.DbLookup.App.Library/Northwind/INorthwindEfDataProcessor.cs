using System.Collections.Generic;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Northwind
{
    public interface INorthwindEfDataProcessor
    {
        void CheckDataExists();

        void SetDataContext();
    }
}
