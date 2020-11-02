using Microsoft.EntityFrameworkCore;
using RingSoft.DataEntryControls.Engine;
using RingSoft.SimpleDemo.WPF.Northwind.Model;
using System.Linq;

namespace RingSoft.SimpleDemo.WPF.Northwind
{
    public class NorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);
            var context = new NorthwindDbContext();
            var result = context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }

        public Order GetOrder(int orderId)
        {
            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Wait);

            var context = new NorthwindDbContext();
            var result = context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .FirstOrDefault(f => f.OrderID == orderId);

            ControlsGlobals.UserInterface.SetWindowCursor(WindowCursorTypes.Default);
            return result;
        }
    }
}
