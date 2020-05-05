using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.EfCore;
using RingSoft.SimpleDemo.WPF.Northwind.Model;

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            var context = new NorthwindDbContext();
            return context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
        }

        public Employee GetEmployee(int employeeId)
        {
            var context = new NorthwindDbContext();
            return context.Employees.Include(i => i.Employee1).FirstOrDefault(f => f.EmployeeID == employeeId);
        }

        public Order GetOrder(int orderId)
        {
            var context = new NorthwindDbContext();
            return context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.Shipper)
                .FirstOrDefault(f => f.OrderID == orderId);
        }

        public bool SaveOrder(Order order)
        {
            var context = new NorthwindDbContext();
            return context.SaveEntity(context.Orders, order, "Saving Order");
        }

        public bool DeleteOrder(int orderId)
        {
            var context = new NorthwindDbContext();
            var order = context.Orders.FirstOrDefault(p => p.OrderID == orderId);
            return context.DeleteEntity(context.Orders, order, "Deleting Order");
        }

        public List<Order_Detail> GetOrderDetails(int orderId)
        {
            var context = new NorthwindDbContext();
            var result = context.OrderDetails.Where(w => w.OrderID == orderId);
            return result.ToList();
        }

        public Order_Detail GetOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContext();
            return context.OrderDetails.Include(i => i.Product)
                .Include(i => i.Order)
                .FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
        }

        public bool SaveOrderDetail(Order_Detail orderDetail)
        {
            using (var context = new NorthwindDbContext())
            {
                if (context.OrderDetails.FirstOrDefault(f =>
                        f.OrderID == orderDetail.OrderID && f.ProductID == orderDetail.ProductID) == null)
                    return context.AddNewEntity(context.OrderDetails, orderDetail, "Saving Order Detail");
            }

            using (var context = new NorthwindDbContext())
            {
                return context.SaveEntity(context.OrderDetails, orderDetail, "Saving Order Detail");
            }
        }

        public bool DeleteOrderDetail(int orderId, int productId)
        {
            var context = new NorthwindDbContext();
            var orderDetail = context.OrderDetails.FirstOrDefault(f => f.OrderID == orderId && f.ProductID == productId);
            return context.DeleteEntity(context.OrderDetails, orderDetail, "Deleting Order Detail.");
        }

        public Product GetProduct(int productId)
        {
            var context = new NorthwindDbContext();
            return context.Products.FirstOrDefault(f => f.ProductID == productId);
        }
    }
}
