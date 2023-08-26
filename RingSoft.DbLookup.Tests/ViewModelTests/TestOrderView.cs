using RingSoft.DbLookup.App.Library.Northwind.ViewModels;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Tests.ViewModelTests
{
    public class TestOrderView : TestDbMaintenanceView, IOrderView
    {
        public object OwnerWindow { get; }
        public void SetFocusToGrid(OrderDetailsGridRow row, int columnId)
        {
            
        }
    }
}
