using System.Collections.Generic;

namespace RingSoft.DbLookup.App.Library.MegaDb.ViewModels
{
    public class MegaDbViewModelInput
    {
        public List<ItemViewModel> ItemViewModels { get; } = new List<ItemViewModel>();

        public List<LocationViewModel> LocationViewModels { get; } = new List<LocationViewModel>();

        public List<ManufacturerViewModel> ManufacturerViewModels { get; } = new List<ManufacturerViewModel>();
    }
}
