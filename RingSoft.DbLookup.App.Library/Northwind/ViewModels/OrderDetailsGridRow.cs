using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class OrderDetailsGridRow : DbMaintenanceDataEntryGridRow<Order_Detail>
    {
        public int ProductId { get; private set; }

        private OrderDetailsGridManager _manager;
        private AutoFillSetup _productAutoFillSetup;

        public OrderDetailsGridRow(OrderDetailsGridManager manager) : base(manager)
        {
            _manager = manager;
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            throw new System.NotImplementedException();
        }

        public override void LoadFromEntity(Order_Detail entity)
        {
            throw new System.NotImplementedException();
        }

        public override bool ValidateRow()
        {
            throw new System.NotImplementedException();
        }

        public override void SaveToEntity(Order_Detail entity, int rowIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
