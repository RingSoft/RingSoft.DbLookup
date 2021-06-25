using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DbLookup.Controls.WPF
{
    public interface ILookupControlContentTemplateFactory
    {
        DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId);
    }
}
