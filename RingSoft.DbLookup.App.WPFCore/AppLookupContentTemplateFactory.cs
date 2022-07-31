

using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using System.Windows;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppLookupContentTemplateFactory : LookupControlContentTemplateFactory
    {
        private Application _application;

        public AppLookupContentTemplateFactory(Application application)
        {
            _application = application;
        }

        public override DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId)
        {
            if (contentTemplateId == RsDbLookupAppGlobals.IconTypeTemplateId)
                return _application.Resources["IconTypeCustomContent"] as DataEntryCustomContentTemplate;

            return base.GetContentTemplate(contentTemplateId);
        }
    }
}
