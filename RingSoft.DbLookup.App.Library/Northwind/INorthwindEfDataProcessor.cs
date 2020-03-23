using System.Collections.Generic;
using RSDbLookupApp.Library.MegaDb.Model;
using RSDbLookupApp.Library.Northwind.Model;

namespace RSDbLookupApp.Library.Northwind
{
    public interface INorthwindEfDataProcessor
    {
        Customer GetCustomer(string customerId);

        bool SaveCustomer(Customer customer);

        bool DeleteCustomer(string customerId);

        Employee GetEmployee(int employeeId);

        bool SaveEmployee(Employee employee);

        bool DeleteEmployee(int employeeId);

        Order GetOrder(int orderId);

        bool SaveOrder(Order order);

        bool DeleteOrder(int orderId);

        List<Order_Detail> GetOrderDetails(int orderId);

        Order_Detail GetOrderDetail(int orderId, int productId);

        bool SaveOrderDetail(Order_Detail orderDetail);

        bool DeleteOrderDetail(int orderId, int productId);

        Product GetProduct(int productId);
    }
}
