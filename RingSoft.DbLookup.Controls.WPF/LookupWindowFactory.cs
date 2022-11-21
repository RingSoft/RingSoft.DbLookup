using Hardcodet.Wpf.TaskbarNotification;
using RingSoft.DbLookup.Lookup;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupWindowFactory
    {
        private TaskbarIcon _taskbarIcon;

        public LookupWindowFactory()
        {
            _taskbarIcon = new TaskbarIcon();
        }
        public virtual LookupWindow CreateLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd,
            bool allowView, string initialSearchFor, PrimaryKeyValue initialPrimaryKeyValue = null, PrimaryKeyValue readOnlyPrimaryKeyValue = null)
        {
            return new LookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor, readOnlyPrimaryKeyValue)
                {InitialSearchForPrimaryKeyValue = initialPrimaryKeyValue};
        }

        public void SetAlertLevel(AlertLevels level, bool disabled, Window window, string message = "")
        {
            //var advancedFindWindows = Dispatcher.Invoke(() => Application.Current.Windows.OfType<AdvancedFindWindow>().ToList());
            var image = LookupControlsGlobals.LookupControlContentTemplateFactory
                .GetImageForAlertLevel(level);
            var title = string.Empty;
            var baloonIcon = BalloonIcon.Info;
            switch (level)
            {
                case AlertLevels.Green:
                    break;
                case AlertLevels.Yellow:
                    title = "Warning!";
                    break;
                case AlertLevels.Red:
                    title = "Red Alert!";
                    baloonIcon = BalloonIcon.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            if (image == null)
            {
                var errorMessage = "No icon found for alert level: ";
                switch (level)
                {
                    case AlertLevels.Green:
                        errorMessage += "Green";
                        break;
                    case AlertLevels.Yellow:
                        errorMessage += "Yellow";
                        break;
                    case AlertLevels.Red:
                        errorMessage += "Red";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(level), level, null);
                }

                throw new ApplicationException(errorMessage);

            }
            window.Dispatcher.Invoke(() => window.Icon = image.Source);


            //if (advancedFindWindows.Count >= 2)
            //{
            //    if (SystemGlobals.WindowAlertLevel < level)
            //    {
            //        SystemGlobals.WindowAlertLevel = level;
            //        Dispatcher.Invoke(() =>
            //        {
            //            ShowLevelIcon(level, message, image, title, baloonIcon);
            //            return;
            //        });
            //    }

            //}
            //else
            //{
            SystemGlobals.WindowAlertLevel = level;
            window.Dispatcher.Invoke(() => { ShowLevelIcon(level, message, image, title, baloonIcon, disabled); });

            //}

        }
        private void ShowLevelIcon(AlertLevels level, string message, Image image, string title, BalloonIcon baloonIcon, bool disabled)
        {
            if (level == AlertLevels.Green)
            {
                _taskbarIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (image.Source != null)
                {
                    _taskbarIcon.ToolTipText = message;
                    _taskbarIcon.IconSource = image.Source;
                    _taskbarIcon.Visibility = Visibility.Visible;

                    if (!disabled)
                    {
                        _taskbarIcon.ShowBalloonTip(title, message, baloonIcon);

                        _taskbarIcon.HideBalloonTip();
                    }

                    return;

                    //return Application.Current.MainWindow.Icon = image.Source;
                }

                return;
            }
        }

    }
}
