using System.Windows;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ILookupControlContentTemplateFactory

    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LookupControlsGlobals.LookupControlContentTemplateFactory = this;

            var wpfAppStart = new WpfAppStart(this);
            wpfAppStart.StartApp("WPF", e.Args);

            base.OnStartup(e);
        }

        public DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId)
        {
            if (contentTemplateId == RsDbLookupAppGlobals.IconTypeTemplateId)
                return Resources["IconTypeCustomContent"] as DataEntryCustomContentTemplate;

            return null;
        }
    }
}
