using RingSoft.DbLookup.Lookup;
using System.Windows.Controls;

namespace RingSoft.SimpleDemo.WPF
{
    /// <summary>
    /// Interaction logic for DemoLookupWindow.xaml
    /// </summary>
    public partial class DemoLookupWindow
    {
        public TextBlock HeaderTextBlock { get; set; }

        public DemoLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor) : base(lookupDefinition, allowAdd, allowView, initialSearchFor)
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                if (HeaderTextBlock != null && lookupDefinition is IDemoLookupDefinition demoLookupDefinition)
                    HeaderTextBlock.Text = demoLookupDefinition.TopHeader;
            };
        }

        public override void OnApplyTemplate()
        {
            HeaderTextBlock = GetTemplateChild("HeaderTextBlock") as TextBlock;
            
            base.OnApplyTemplate();
        }
    }
}
