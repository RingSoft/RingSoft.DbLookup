using RingSoft.DataEntryControls.WPF;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class LookupSearchForDropDownHost<TDropDownControl> : LookupSearchForHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        public override void SelectAll()
        {
            Control.TextBox?.SelectAll();
        }

        public override void SetFocusToControl()
        {
            Control.TextBox?.Focus();
            base.SetFocusToControl();
        }

        public override bool CanProcessSearchForKey(Key key)
        {
            if (Control.IsPopupOpen())
                return false;

            switch (key)
            {
                case Key.Home:
                    return Control.SelectionStart == 0 && Control.SelectionLength == 0;
                case Key.End:
                    return Control.SelectionStart == Control.Text.Length;
            }

            return base.CanProcessSearchForKey(key);
        }
    }
}
