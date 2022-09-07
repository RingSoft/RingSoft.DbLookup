﻿using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupControlContentTemplateFactory
    {
        public virtual DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId)
        {
            return null;
        }

        public virtual Image GetImageForAlertLevel(AlertLevels alertLevel)
        {
            return new Image{Source = Application.Current.MainWindow.Icon};
        }
    }
}
