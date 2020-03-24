﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.EfCore;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindEfDataProcessorCore : INorthwindEfDataProcessor
    {
        public Customer GetCustomer(string customerId)
        {
            var context = new NorthwindDbContextEfCore();
            return context.Customers.FirstOrDefault(f => f.CustomerID == customerId);
        }

        public bool SaveCustomer(Customer customer)
        {
            using (var context = new NorthwindDbContextEfCore())
            {
                if (context.Customers.FirstOrDefault(f => f.CustomerID == customer.CustomerID) == null)
                    return context.AddNewEntity(context.Customers, customer, "Saving Customer");
            }

            using (var context = new NorthwindDbContextEfCore())
            {
                return context.SaveEntity(context.Customers, customer, "Saving Customer");
            }
        }

        public bool DeleteCustomer(string customerId)
        {
            var context = new NorthwindDbContextEfCore();
            var customer = context.Customers.FirstOrDefault(p => p.CustomerID == customerId);
            return context.DeleteEntity(context.Customers, customer, "Deleting Customer");
        }

        public Employee GetEmployee(int employeeId)
        {
            var context = new NorthwindDbContextEfCore();
            return context.Employees.Include(i => i.Employee1).FirstOrDefault(f => f.EmployeeID == employeeId);
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


        public Order GetOrder(int orderId)
        {
            var context = new NorthwindDbContextEfCore();
            return context.Orders.Include(i => i.Customer)
                .Include(i => i.Employee)
                .Include(i => i.Shipper)
                .FirstOrDefault(f => f.OrderID == orderId);
        }

        public bool SaveOrder(Order order)
        {
            var context = new NorthwindDbContextEfCore();
            return context.SaveEntity(context.Orders, order, "Saving Order");
        }

        public bool DeleteOrder(int orderId)
        {
            var context = new NorthwindDbContextEfCore();
            var order = context.Orders.FirstOrDefault(p => p.OrderID == orderId);
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
            return context.OrderDetails.Include(i => i.Product)
                .Include(i => i.Order)
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
            return context.Products.FirstOrDefault(f => f.ProductID == productId);
        }
    }
}
