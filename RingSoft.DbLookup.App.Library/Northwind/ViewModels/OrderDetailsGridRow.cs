using System;
using System.Linq;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
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

        public decimal ExtendedPrice => Math.Round(Quantity * Price, 2);

        public decimal Discount { get; set; }

        private INorthwindLookupContext _lookupContext;
        private OrderDetailsGridManager _manager;
        private AutoFillSetup _productAutoFillSetup;
        private IntegerEditControlSetup _quantitySetup;
        private DecimalEditControlSetup _decimalSetup;

        public OrderDetailsGridRow(OrderDetailsGridManager manager) : base(manager)
        {
            //DisplayStyleId = OrderDetailsGridManager.BlueDisplayStyleId;

            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
            _manager = manager;

            _productAutoFillSetup = new AutoFillSetup(_lookupContext.OrderDetails.GetFieldDefinition(p => p.ProductID));
            _quantitySetup = new IntegerEditControlSetup();
            _quantitySetup.InitializeFromType(typeof(short));
            _decimalSetup = new DecimalEditControlSetup(){FormatType = DecimalEditFormatTypes.Currency};
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var column = (OrderDetailsGridColumns) columnId;

            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    break;
                default:
                    if (IsNew)
                        return new DataEntryGridTextCellProps(this, columnId);
                    break;
            }
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

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (OrderDetailsGridColumns) columnId;
            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    //return new DataEntryGridCellStyle {DisplayStyleId = OrderDetailsGridManager.BlueDisplayStyleId};
                    break;
                case OrderDetailsGridColumns.ExtendedPrice:
                    return  new DataEntryGridCellStyle{CellStyleType = DataEntryGridCellStyleTypes.Disabled};
                default:
                {
                    if (IsNew)
                        return new DataEntryGridCellStyle {CellStyleType = DataEntryGridCellStyleTypes.Disabled};
                }
                    break;
            }

            return base.GetCellStyle(columnId);
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var column = (OrderDetailsGridColumns) value.ColumnId;
            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        bool validProduct;
                        if (autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid)
                        {
                            validProduct = SetProduct(autoFillCellProps.AutoFillValue);
                        }
                        else
                        {
                            validProduct = false;
                            var message =
                                $"'{autoFillCellProps.AutoFillValue.Text}' is not a valid Product.  Do you wish to create a new Product?";
                            if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Invalid Product") ==
                                MessageBoxButtonsResult.Yes)
                            {
                                var newProductResult =
                                    _lookupContext.NorthwindContextConfiguration.ProductsLookup.ShowAddOnTheFlyWindow(
                                        autoFillCellProps.Text, _manager.OrderViewModel.OrderView.OwnerWindow);
                                if (newProductResult.NewPrimaryKeyValue != null && newProductResult.NewPrimaryKeyValue.IsValid)
                                {
                                    var newAutoFillValue = new AutoFillValue(newProductResult.NewPrimaryKeyValue,
                                        newProductResult.NewLookupEntity.ProductName);
                                    validProduct = SetProduct(newAutoFillValue);
                                }
                            }
                        }
                        if (!validProduct)
                        {
                            autoFillCellProps.OverrideCellMovement = true;
                            return; //So IsNew isn't set.
                        }
                    }
                    break;
                case OrderDetailsGridColumns.Quantity:
                    if (value is DataEntryGridIntegerCellProps integerCellProps)
                    {
                        if (integerCellProps.Value != null) 
                            Quantity = (int) integerCellProps.Value;
                        _manager.OrderViewModel.RefreshTotalControls();
                    }
                    break;
                case OrderDetailsGridColumns.Price:
                    if (value is DataEntryGridDecimalCellProps decimalCellProps)
                    {
                        if (decimalCellProps.Value != null)
                            Price = (decimal) decimalCellProps.Value;
                        _manager.OrderViewModel.RefreshTotalControls();
                    }
                    break;
                case OrderDetailsGridColumns.Discount:
                    if (value is DataEntryGridDecimalCellProps discountCellProps)
                    {
                        if (discountCellProps.Value != null)
                            Discount = (decimal)discountCellProps.Value;
                        _manager.OrderViewModel.RefreshTotalControls();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        private bool SetProduct(AutoFillValue productValue)
        {
            var product = _lookupContext.Products.GetEntityFromPrimaryKeyValue(productValue.PrimaryKeyValue);
            var orderDetails = Manager.Rows.OfType<OrderDetailsGridRow>();

            var existingRow = orderDetails.FirstOrDefault(f => f.ProductId == product.ProductID);
            if (existingRow != null)
            {
                var message = $"'{productValue.Text}' already exists in this order.  Do you wish to modify it instead?";
                if (ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Duplicate Product detected.") ==
                    MessageBoxButtonsResult.Yes)
                {
                    _manager.Grid.GotoCell(existingRow, (int)OrderDetailsGridColumns.Product);
                }

                return false;
            }
            ProductId = product.ProductID;
            ProductAutoFillValue = productValue;
            LoadFromProduct(product);

            return true;
        }

        private void LoadFromProduct(Product product)
        {
            product = RsDbLookupAppGlobals.EfProcessor.NorthwindEfDataProcessor.GetProduct(product.ProductID);
            Quantity = 1;
            if (product.UnitPrice != null)
                Price = (decimal) product.UnitPrice;
            _manager.OrderViewModel.RefreshTotalControls();
        }

        public override void LoadFromEntity(Order_Detail entity)
        {
            ProductId = entity.ProductID;
            var productPrimaryKey = _lookupContext.Products.GetPrimaryKeyValueFromEntity(entity.Product);
            ProductAutoFillValue = new AutoFillValue(productPrimaryKey, entity.Product.ProductName);
            Quantity = entity.Quantity;
            Price = entity.UnitPrice;
            Discount = (decimal) entity.Discount;
        }

        public override bool ValidateRow()
        {
            return true;
        }

        public override void SaveToEntity(Order_Detail entity, int rowIndex)
        {
            var product = _lookupContext.Products.GetEntityFromPrimaryKeyValue(ProductAutoFillValue.PrimaryKeyValue);
            entity.ProductID = product.ProductID;
            entity.Quantity = (short)Quantity;
            entity.UnitPrice = Price;
            entity.Discount = (float)Discount;
        }
    }
}
