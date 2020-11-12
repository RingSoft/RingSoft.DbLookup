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

        public decimal ExtendedPrice => Math.Round(Quantity * Price, 2);

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
                    break;
                case OrderDetailsGridColumns.ExtendedPrice:
                    return  new DataEntryGridCellStyle{CellStyle = DataEntryGridCellStyles.Disabled};
                default:
                {
                    if (IsNew)
                        return new DataEntryGridCellStyle {CellStyle = DataEntryGridCellStyles.Disabled};
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
                        if (autoFillCellProps.AutoFillValue.PrimaryKeyValue.IsValid)
                        {
                            ProductAutoFillValue = autoFillCellProps.AutoFillValue;
                            LoadFromProduct(ProductAutoFillValue.PrimaryKeyValue);
                        }
                        else
                        {
                            var validProduct = false;
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
                                    ProductAutoFillValue = new AutoFillValue(newProductResult.NewPrimaryKeyValue,
                                        newProductResult.NewLookupEntity.ProductName);
                                    LoadFromProduct(newProductResult.NewPrimaryKeyValue);
                                    validProduct = true;
                                }
                            }

                            if (!validProduct)
                            {
                                autoFillCellProps.OverrideCellMovement = true;
                                return; //So IsNew isn't set.
                            }
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

        private void LoadFromProduct(PrimaryKeyValue primaryKeyValue)
        {
            var product =
                _lookupContext.Products.GetEntityFromPrimaryKeyValue(primaryKeyValue);
            LoadFromProduct(product);
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
