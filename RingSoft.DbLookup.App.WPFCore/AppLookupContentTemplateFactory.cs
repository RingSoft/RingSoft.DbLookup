

using System;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.App.Library;
using RingSoft.DbLookup.Controls.WPF;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DbMaintenance;

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

        public override Image GetImageForAlertLevel(AlertLevels alertLevel)
        {
            switch (alertLevel)
            {
                case AlertLevels.Green:
                    return _application.Resources["GreenIcon"] as Image;
                case AlertLevels.Yellow:
                    return _application.Resources["YellowIcon"] as Image;
                case AlertLevels.Red:
                    return _application.Resources["RedIcon"] as Image;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alertLevel), alertLevel, null);
            }
            return base.GetImageForAlertLevel(alertLevel);
        }
    }
}
