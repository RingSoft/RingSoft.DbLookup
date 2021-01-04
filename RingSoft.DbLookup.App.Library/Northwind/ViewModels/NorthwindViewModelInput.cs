using System.Collections.Generic;

namespace RingSoft.DbLookup.App.Library.Northwind.ViewModels
{
    public class NorthwindViewModelInput
    {
        public List<CustomerViewModel> CustomerViewModels { get; } = new List<CustomerViewModel>();

        public List<EmployeeViewModel> EmployeeViewModels { get; } = new List<EmployeeViewModel>();

        public List<OrderDetailsViewModel> OrderDetailsViewModels { get; } = new List<OrderDetailsViewModel>();

        public List<OrderViewModel> OrderViewModels { get; } = new List<OrderViewModel>();

        public List<ProductViewModel> ProductViewModels { get; } = new List<ProductViewModel>();

        public OrderInput OrderInput { get; set; }
    }
}
