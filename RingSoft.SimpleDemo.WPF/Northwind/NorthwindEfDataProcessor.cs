using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup;
using RingSoft.DbLookup.DataProcessor;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using System.Linq;

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            DbDataProcessor.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var context = new NorthwindDbContext();
            var result = context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
            DbDataProcessor.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        public Order GetOrder(int orderId)
        {
            DbDataProcessor.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            var context = new NorthwindDbContext();
            var result = context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .FirstOrDefault(f => f.OrderID == orderId);

            DbDataProcessor.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }
    }
}
