using System.Collections.Generic;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbMaintenance;
using System.Collections.Specialized;
using System.Linq;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public enum OrderDetailsGridColumns
    {
        Product = OrderDetailsGridManager.ProductColumnId,
        Quantity = OrderDetailsGridManager.QuantityColumnId,
        Price = OrderDetailsGridManager.PriceColumnId,
        ExtendedPrice = OrderDetailsGridManager.ExtendedPriceColumnId,
        Discount = OrderDetailsGridManager.DiscountColumnId
    }

    public class OrderDetailsGridManager : DbMaintenanceDataEntryGridManager<Order_Detail>
    {
        public const int ProductColumnId = 0;
        public const int QuantityColumnId = 1;
        public const int PriceColumnId = 2;
        public const int ExtendedPriceColumnId = 3;
        public const int DiscountColumnId = 4;

        public OrderViewModel OrderViewModel { get; }

        public OrderDetailsGridManager(OrderViewModel viewModel) : base(viewModel)
        {
            OrderViewModel = viewModel;
        }

        protected override DataEntryGridRow GetNewRow()
        {
            return new OrderDetailsGridRow(this);
        }

        protected override DbMaintenanceDataEntryGridRow<Order_Detail> ConstructNewRowFromEntity(Order_Detail entity)
        {
            return new OrderDetailsGridRow(this);
        }

        protected override void OnRowsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnRowsChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    OrderViewModel.RefreshTotalControls();
                    break;
            }
        }

        public override void LoadGrid(IEnumerable<Order_Detail> entityList)
        {
            base.LoadGrid(entityList);
            if (OrderViewModel.SetInitialFocusToGrid)
            {
                OrderViewModel.SetInitialFocusToGrid = false;
                var orderDetails = Rows.OfType<OrderDetailsGridRow>();
                var productRow = orderDetails.FirstOrDefault(f => f.ProductId == OrderViewModel.FilterProductId);
                if (productRow != null)
                {
                    OrderViewModel.OrderView.SetFocusToGrid(productRow, ProductColumnId);
                }
            }
        }
    }
}
