using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.GetDataProcessor;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using System.Linq;

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Wait);
            var context = new NorthwindDbContext();
            var result = context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
            DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        public Order GetOrder(int orderId)
        {
            DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Wait);

            var context = new NorthwindDbContext();
            var result = context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.Shipper)
                .FirstOrDefault(f => f.OrderID == orderId);

            DbDataProcessor.WindowCursor.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }
    }
}
