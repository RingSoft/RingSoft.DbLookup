using RingSoft.DataEntryControls.Engine;
using RingSoft.DbLookup.App.Library.Northwind;
using RingSoft.DbLookup.App.Library.Northwind.Model;
using RingSoft.DbLookup.QueryBuilder;
using System;
using System.Linq;

namespace RingSoft.DbLookup.App.Library.EfCore.Northwind
{
    public class NorthwindEfDataProcessorCore : INorthwindEfDataProcessor
    {
        public void SetDataContext()
        {
            SystemDataRepositoryEfCore.RepositoryMode = DataRepositoryModes.Northwind;
        }

        public void CheckDataExists()
        {
            SetDataContext();
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
    }
}
