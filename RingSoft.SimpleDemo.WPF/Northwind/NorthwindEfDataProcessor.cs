using Microsoft.EntityFrameworkCore;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using System.Collections.Generic;
using System.Linq;

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
    }
}
