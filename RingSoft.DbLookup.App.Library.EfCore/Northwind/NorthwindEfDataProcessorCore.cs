using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.AdvancedFind;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.EfCore;
using RingSoft.DbLookup.QueryBuilder;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindEfDataProcessorCore : INorthwindEfDataProcessor
    {
        public NorthwindEfDataProcessorCore()
        {
            
        }

        public void CheckDataExists()
        {
            SetAdvancedFindDbContext();
            var context = SystemGlobals.DataRepository.GetDataContext();
            var ordersTable = context.GetTable<Order>();
            var any = false;
            try
            {
                any = ordersTable.Any(p => p.OrderName == null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ControlsGlobals.UserInterface.ShowMessageBox(e.Message, "Error", RsMessageBoxIcons.Error);
                return;
            }

            if (any)
            {
                var orderList = ordersTable.Where(p => p.OrderName == null).ToList();
                foreach (var order in orderList)
                {
                    order.OrderName =
                        $"{GblMethods.FormatDateValue(order.OrderDate.GetValueOrDefault(), DbDateTypes.DateOnly)} {order.CustomerID}";

                    if (!context.SaveEntity(order, "Updating Order Name"))
                    {
                        return;
                    }
                }
            }

            var employeesTable = context.GetTable<Employee>();

            if (employeesTable.Any(p => p.FullName == null))
            {
                var employeesList = employeesTable.Where(p => p.FullName == null).ToList();
                foreach (var employee in employeesList)
                {
                    employee.FullName = $"{employee.FirstName} {employee.LastName}";

                    if (!context.SaveEntity(employee, "Updating Employee Full Name"))
                    {
                        return;
                    }
                }
            }
        }
        public Customer GetCustomer(string customerId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            return context.GetTable<Customer>().FirstOrDefault(f => f.CustomerID == customerId);
        }

        public bool SaveCustomer(Customer customer)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Customer>();
            if (table.FirstOrDefault(f => f.CustomerID == customer.CustomerID) == null)
            {
                return context.AddSaveEntity(customer, "Saving Customer");
            }

            return context.SaveEntity(customer, "Saving Customer");
        }

        public bool DeleteCustomer(string customerId)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var customer = context.GetTable<Customer>().FirstOrDefault(p => p.CustomerID == customerId);
            return context.DeleteEntity(customer, "Deleting Customer");
        }


        public Employee GetEmployee(int employeeId)
        {
            var context = new NorthwindDbContextEfCore();

            var result = context.Employees
                .Include(i => i.Employee1)
                .FirstOrDefault(f => f.EmployeeID == employeeId);

            return result;
        }

        public bool SaveEmployee(Employee employee)
        {
            var context = new NorthwindDbContextEfCore();
            return context.SaveEntity(context.Employees, employee, "Saving Employee");
        }

        public bool DeleteEmployee(int employeeId)
        {
            var context = new NorthwindDbContextEfCore();
            var employee = context.Employees.FirstOrDefault(p => p.EmployeeID == employeeId);
            return context.DeleteEntity(context.Employees, employee, "Deleting Employee");
        }


        public Order GetOrder(int orderId, bool gridMode)
        {
            var context = SystemGlobals.DataRepository.GetDataContext();
            var table = context.GetTable<Order>();
            //var entitys = context.Orders
            //    .Include(p => p.Customer)
            //    .Where(p => p.Customer.CompanyName.Contains("w", StringComparison.CurrentCultureIgnoreCase))
            //    .OrderBy(p => p.OrderName);

            if (gridMode)
            {
                return table.Include(i => i.Customer)
                    .Include(i => i.Employee)
                    .Include(i => i.Shipper)
                    .Include(i => i.Order_Details)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(f => f.OrderID == orderId);
            }
            else
            {
                return table.Include(i => i.Customer)
                    .Include(i => i.Employee)
                    .Include(i => i.Shipper)
                    .FirstOrDefault(f => f.OrderID == orderId);
            }
        }

        public bool SaveOrder(Order order, List<Order_Detail> details)
        {
            var dataContext = SystemGlobals.DataRepository.GetDataContext();
            var result = dataContext.SaveEntity(order, "Saving Order");
            var context = new NorthwindDbContextEfCore();
            //var result = context.SaveEntity(context.Orders, order, "Saving Order");
            if (result && details != null)
            {
                context.OrderDetails.RemoveRange(context.OrderDetails.Where(w => w.OrderID == order.OrderID));

                foreach (var orderDetail in details)
                {
                    orderDetail.OrderID = order.OrderID;
                    context.OrderDetails.Add(orderDetail);
                }
                result = context.SaveEfChanges("Saving Order Details");
            }
            return result;
        }

        public bool DeleteOrder(int orderId)
        {
            var context = new NorthwindDbContextEfCore();
            var order = context.Orders.FirstOrDefault(p => p.OrderID == orderId);
            context.OrderDetails.RemoveRange(context.OrderDetails.Where(w => w.OrderID == order.OrderID));
            return context.DeleteEntity(context.Orders, order, "Deleting Order");
        }

        public List<Order_Detail> GetOrderDetails(int orderId)
        {
            var context = new NorthwindDbContextEfCore();
            var result = context.OrderDetails.Where(w => w.OrderID == orderId);
            return result.ToList();
        }

        public Order_Detail GetOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContextEfCore();
            //var order = context.OrderDetails
            //    .Include(p => p.Product)
            //    .ThenInclude(p => p.Supplier)
            //    .FirstOrDefault(p => p.Product.Supplier.CompanyName.CompareTo("A") >= 0);

            return context.OrderDetails.Include(i => i.Product)
                .Include(i => i.Order)
                .ThenInclude(t => t.Customer)
                .FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
        }

        public bool SaveOrderDetail(Order_Detail orderDetail)
        {
            using (var context = new NorthwindDbContextEfCore())
            {
                if (context.OrderDetails.FirstOrDefault(f =>
                        f.OrderID == orderDetail.OrderID && f.ProductID == orderDetail.ProductID) == null)
                    return context.AddNewEntity(context.OrderDetails, orderDetail, "Saving Order Detail");
            }

            using (var context = new NorthwindDbContextEfCore())
            {
                return context.SaveEntity(context.OrderDetails, orderDetail, "Saving Order Detail");
            }
        }

        public bool DeleteOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContextEfCore();
            var orderDetail = context.OrderDetails.FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
            return context.DeleteEntity(context.OrderDetails, orderDetail, "Deleting Order Detail.");
        }

        public Product GetProduct(int productId)
        {
            var context = new NorthwindDbContextEfCore();
            return context.Products
                .Include(i => i.Category)
                .Include(i => i.Supplier)
                .FirstOrDefault(f => f.ProductID == productId);
        }

        public bool SaveProduct(Product product)
        {
            using (var context = new NorthwindDbContextEfCore())
            {
                if (context.Products.FirstOrDefault(f => f.ProductID == product.ProductID) == null)
                    return context.AddNewEntity(context.Products, product, "Saving Product");
            }

            using (var context = new NorthwindDbContextEfCore())
            {
                return context.SaveEntity(context.Products, product, "Saving Product");
            }
        }

        public bool DeleteProduct(int productId)
        {
            var context = new NorthwindDbContextEfCore();
            var product = context.Products.FirstOrDefault(p => p.ProductID == productId);
            return context.DeleteEntity(context.Products, product, "Deleting Product");
        }

        public void SetAdvancedFindDbContext()
        {
            EfCoreGlobals.DbAdvancedFindContextCore = new NorthwindDbContextEfCore();

            var processor = new AdvancedFindDataProcessorEfCore();
            SystemGlobals.DataRepository = processor;
            SystemGlobals.AdvancedFindDbProcessor = processor;
        }

        public void SetAdvancedFindLookupContext()
        {
            
        }

        public Shipper GetShipper(int shipperId)
        {
            var context = new NorthwindDbContextEfCore();
            return context.Shippers.FirstOrDefault(f => f.ShipperID == shipperId);

        }
    }
}
