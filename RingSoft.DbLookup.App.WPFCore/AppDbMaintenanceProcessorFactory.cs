﻿using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.App.WPFCore
{
    public class AppDbMaintenanceProcessorFactory : RingSoft.DbLookup.Controls.WPF.DbMaintenanceProcessorFactory
    {
        public override DbMaintenanceWindowProcessor GetProcessor()
        {
            return new AppDbMaintenanceWindowProcessor();
        }
    }
}
