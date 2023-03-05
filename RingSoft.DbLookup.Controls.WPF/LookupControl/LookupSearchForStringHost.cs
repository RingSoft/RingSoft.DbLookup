using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Lookup;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public class LookupSearchForStringHost : LookupSearchForHost<StringEditControl>
    {
        public override string SearchText
        {
            get => Control.Text;
            set => Control.Text = value;
        }

        protected override StringEditControl ConstructControl()
        {
            var result = new StringEditControl();
            return result;
        }

        protected override void Initialize(StringEditControl control, LookupColumnDefinitionBase columnDefinition)
        {
            
        }

        protected override void Initialize(StringEditControl control)
        {
            Control.TextChanged += (sender, args) => OnTextChanged();
        }

        public override void SelectAll()
        {
            Control.SelectAll();
        }
    }
}
