using System;
using System.Linq;
using System.Threading.Tasks;
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

        public double Price { get; set; }

        public double ExtendedPrice => Math.Round(Quantity * Price, 2);

        public double Discount { get; set; }

        private INorthwindLookupContext _lookupContext;
        private OrderDetailsGridManager _manager;
        private AutoFillSetup _productAutoFillSetup;
        private IntegerEditControlSetup _quantitySetup;
        private DecimalEditControlSetup _decimalSetup;

        public OrderDetailsGridRow(OrderDetailsGridManager manager) : base(manager)
        {
            _lookupContext = RsDbLookupAppGlobals.EfProcessor.NorthwindLookupContext;
            _manager = manager;

            _productAutoFillSetup = new AutoFillSetup(
                TableDefinition
                    .GetFieldDefinition(p => p.ProductID))
            {
                AddViewParameter = _manager.OrderViewModel.ViewModelInput
            };
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
                    return  new DataEntryGridCellStyle{State = DataEntryGridCellStates.Disabled};
                default:
                {
                    if (IsNew)
                        return new DataEntryGridCellStyle {State = DataEntryGridCellStates.Disabled};
                }
                    break;
            }

            return base.GetCellStyle(columnId);
        }

        public async override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var column = (OrderDetailsGridColumns) value.ColumnId;
            switch (column)
            {
                case OrderDetailsGridColumns.Product:
                    if (value is DataEntryGridAutoFillCellProps autoFillCellProps)
                    {
                        bool validProduct;
                        if (autoFillCellProps.AutoFillValue.IsValid())
                        {
                            validProduct = await SetProduct(autoFillCellProps.AutoFillValue);
                        }
                        else
                        {
                            validProduct = false;
                            var item = string.Empty;
                            if (autoFillCellProps.AutoFillValue != null)
                            {
                                item = autoFillCellProps.AutoFillValue.Text;
                            }

                            if (item.IsNullOrEmpty())
                            {
                                ProductAutoFillValue = null;
                                ProductId = 0;
                                Quantity = 0;
                                Price = 0;
                                IsNew = true;
                                Manager.Grid?.RefreshGridView();
                                return;
                            }
                            var message =
                                $"'{item}' is not a valid Product.  Do you wish to create a new Product?";
                            if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Invalid Product") ==
                                MessageBoxButtonsResult.Yes)
                            {
                                var newProductResult =
                                    _lookupContext.NorthwindContextConfiguration.ProductsLookup.ShowAddOnTheFlyWindow(
                                        item);
                                if (newProductResult.NewPrimaryKeyValue.IsValid())
                                {
                                    var newAutoFillValue = RsDbLookupAppGlobals
                                        .EfProcessor
                                        .NorthwindLookupContext
                                        .Products
                                        .GetAutoFillValue(newProductResult.NewPrimaryKeyValue.KeyString);

                                    validProduct = await SetProduct(newAutoFillValue);
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
                            Price = (double) decimalCellProps.Value;
                        _manager.OrderViewModel.RefreshTotalControls();
                    }
                    break;
                case OrderDetailsGridColumns.Discount:
                    if (value is DataEntryGridDecimalCellProps discountCellProps)
                    {
                        if (discountCellProps.Value != null)
                            Discount = (double)discountCellProps.Value;
                        _manager.OrderViewModel.RefreshTotalControls();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            base.SetCellValue(value);
        }

        private async Task<bool> SetProduct(AutoFillValue productValue)
        {
            var product = productValue.GetEntity<Product>();
            var orderDetails = Manager.Rows.OfType<OrderDetailsGridRow>();

            var existingRow = orderDetails.FirstOrDefault(f => f.ProductId == product.ProductID);
            if (existingRow != null && existingRow != this)
            {
                var message = $"'{productValue.Text}' already exists in this order.  Do you wish to modify it instead?";
                if (await ControlsGlobals.UserInterface.ShowYesNoMessageBox(message, "Duplicate Product detected.") ==
                    MessageBoxButtonsResult.Yes)
                {
                    _manager.GotoCell(existingRow, (int)OrderDetailsGridColumns.Product);
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
            product = product.FillOutProperties(false);
            Quantity = 1;
            if (product.UnitPrice != null)
                Price = (double) product.UnitPrice;
            _manager.OrderViewModel.RefreshTotalControls();
        }

        public override void LoadFromEntity(Order_Detail entity)
        {
            ProductId = entity.ProductID;
            ProductAutoFillValue = entity.Product.GetAutoFillValue();
            Quantity = entity.Quantity;
            Price = entity.UnitPrice;
            Discount = (double) entity.Discount;
        }

        public override void SaveToEntity(Order_Detail entity, int rowIndex)
        {
            entity.ProductID = ProductId;
            entity.Quantity = (short)Quantity;
            entity.UnitPrice = (float)Price;
            entity.Discount = (float)Discount;
        }
    }
}
