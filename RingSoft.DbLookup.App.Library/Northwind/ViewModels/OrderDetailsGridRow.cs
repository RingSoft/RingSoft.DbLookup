using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class OrderDetailsGridRow : DbMaintenanceDataEntryGridRow<Order_Detail>
    {
        public int ProductId { get; private set; }

        public AutoFillValue ProductAutoFillValue { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal ExtendedPrice { get; set; }

        public decimal Discount { get; set; }

        private INorthwindLookupContext _lookupContext;
        private OrderDetailsGridManager _manager;
        private AutoFillSetup _productAutoFillSetup;
        private IntegerEditControlSetup _quantitySetup;
        private DecimalEditControlSetup _decimalSetup;

        public OrderDetailsGridRow(OrderDetailsGridManager manager) : base(manager)
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
            _manager = manager;

            _productAutoFillSetup = new AutoFillSetup(_lookupContext.OrderDetails.GetFieldDefinition(p => p.ProductID));
            _quantitySetup = new IntegerEditControlSetup();
            _quantitySetup.InitializeFromType(typeof(short));
            _decimalSetup = new DecimalEditControlSetup();
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (OrderDetailsGridColumns) columnId;
            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    return new DataEntryGridAutoFillCellProps(this, columnId, _productAutoFillSetup,
                        ProductAutoFillValue);
                case OrderDetailsGridColumns.Quantity:
                    return new DataEntryGridIntegerCellProps(this, columnId, _quantitySetup, Quantity);
                case OrderDetailsGridColumns.Price:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalSetup, Price);
                case OrderDetailsGridColumns.ExtendedPrice:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalSetup, ExtendedPrice);
                case OrderDetailsGridColumns.Discount:
                    return new DataEntryGridDecimalCellProps(this, columnId, _decimalSetup, Discount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (OrderDetailsGridColumns) value.ColumnId;
            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    break;
                case OrderDetailsGridColumns.Quantity:
                    break;
                case OrderDetailsGridColumns.Price:
                    break;
                case OrderDetailsGridColumns.ExtendedPrice:
                    break;
                case OrderDetailsGridColumns.Discount:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
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
