using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.SimpleDemo.WPF
{
    /// <summary>
    /// Interaction logic for DemoLookupWindow.xaml
    /// </summary>
    public partial class DemoLookupWindow : LookupWindow
    {
        public DemoLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor) : base(lookupDefinition, allowAdd, allowView, initialSearchFor)
        {
            InitializeComponent();
        }
    }
}
