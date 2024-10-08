using System.Windows;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public interface IDbMaintenanceVisualView
    {
        BaseWindow MaintenanceWindow { get; }

        bool ShowInTaskbar { get; set; }

        bool EnterToTab { get; set; }

        void Close();
    }
}
