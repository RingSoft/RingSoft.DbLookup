using System.Collections.Generic;
using RingSoft.DbLookup.App.Library.Northwind.Model;

namespace RingSoft.DbLookup.App.Library.Northwind
{
    public interface INorthwindEfDataProcessor
    {
        void CheckDataExists();

        Customer GetCustomer(string customerId);

        bool SaveCustomer(Customer customer);

        bool DeleteCustomer(string customerId);

        Employee GetEmployee(int employeeId);

        bool SaveEmployee(Employee employee);

        bool DeleteEmployee(int employeeId);

        Order GetOrder(int orderId, bool gridMode);

        bool SaveOrder(Order order, List<Order_Detail> details);

        bool DeleteOrder(int orderId);

        List<Order_Detail> GetOrderDetails(int orderId);

        Order_Detail GetOrderDetail(int orderId, int productId);

        bool SaveOrderDetail(Order_Detail orderDetail);

        bool DeleteOrderDetail(int orderId, int productId);

        Product GetProduct(int productId);

        bool SaveProduct(Product product);

        bool DeleteProduct(int productId);

        void SetAdvancedFindDbContext();

        void SetAdvancedFindLookupContext();

        Shipper GetShipper(int shipperId);
    }
}
