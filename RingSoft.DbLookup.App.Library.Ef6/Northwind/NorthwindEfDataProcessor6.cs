using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.Ef6;

namespace RingSoft.DbLookup.App.Library.Ef6.Northwind
{
    public class NorthwindEfDataProcessor6 : INorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            var context = new NorthwindDbContextEf6();
            return context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
        }

        public bool SaveCustomer(Customer customer)
        {
            var context = new NorthwindDbContextEf6();
            return context.SaveEntity(context.Customers, customer, "Saving Customer");
        }

        public bool DeleteCustomer(string customerId)
        {
            var context = new NorthwindDbContextEf6();
            var customer = context.Customers.FirstOrDefault(p => p.CustomerID == customerId);
            return context.DeleteEntity(context.Customers, customer, "Deleting Customer");
        }

        public Employee GetEmployee(int employeeId)
        {
            var context = new NorthwindDbContextEf6();
            return context.Employees.Include(i => i.Employee1).FirstOrDefault(f => f.EmployeeID == employeeId);
        }

        public bool SaveEmployee(Employee employee)
        {
            var context = new NorthwindDbContextEf6();
            return context.SaveEntity(context.Employees, employee, "Saving Employee");
        }

        public bool DeleteEmployee(int employeeId)
        {
            var context = new NorthwindDbContextEf6();
            var employee = context.Employees.FirstOrDefault(p => p.EmployeeID == employeeId);
            return context.DeleteEntity(context.Employees, employee, "Deleting Employee");
        }

        public Order GetOrder(int orderId, bool gridMode)
        {
            var context = new NorthwindDbContextEf6();
            if (gridMode)
            {
                return context.Orders.Include(i => i.Customer)
                    .Include(i => i.Employee)
                    .Include(i => i.Shipper)
                    .Include(i => i.Order_Details.Select(s => s.Product))
                    .FirstOrDefault(f => f.OrderID == orderId);
            }
            else
            {
                return context.Orders.Include(i => i.Customer)
                    .Include(i => i.Employee)
                    .Include(i => i.Shipper)
                    .FirstOrDefault(f => f.OrderID == orderId);
            }
        }

        public bool SaveOrder(Order order)
        {
            var context = new NorthwindDbContextEf6();
            return context.SaveEntity(context.Orders, order, "Saving Order");
        }

        public bool SaveOrder(Order order, List<Order_Detail> details)
        {
            var context = new NorthwindDbContextEf6();
            var result = context.SaveEntity(context.Orders, order, "Saving Order");

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
            var context = new NorthwindDbContextEf6();
            var order = context.Orders.FirstOrDefault(p => p.OrderID == orderId);
            context.OrderDetails.RemoveRange(context.OrderDetails.Where(w => w.OrderID == order.OrderID));
            return context.DeleteEntity(context.Orders, order, "Deleting Order");
        }

        public List<Order_Detail> GetOrderDetails(int orderId)
        {
            var context = new NorthwindDbContextEf6();
            var result = context.OrderDetails.Where(w => w.OrderID == orderId);
            return result.ToList();
        }

        public Order_Detail GetOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContextEf6();
            return context.OrderDetails.Include(i => i.Product)
                .Include(i => i.Order)
                .FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
        }

        public bool SaveOrderDetail(Order_Detail orderDetail)
        {
            var context = new NorthwindDbContextEf6();
            return context.SaveEntity(context.OrderDetails, orderDetail, "Saving Order Detail.");
        }

        public bool DeleteOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContextEf6();
            var orderDetail = context.OrderDetails.FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
            return context.DeleteEntity(context.OrderDetails, orderDetail, "Deleting Order Detail.");
        }

        public Product GetProduct(int productId)
        {
            var context = new NorthwindDbContextEf6();
            return context.Products.FirstOrDefault(f => f.ProductID == productId);
        }
    }
}
