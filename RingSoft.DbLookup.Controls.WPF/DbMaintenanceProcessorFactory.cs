using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public class DbMaintenanceProcessorFactory
    {
        public virtual IDbMaintenanceProcessor GetProcessor()
        {
            return null;
        }
    }
}
