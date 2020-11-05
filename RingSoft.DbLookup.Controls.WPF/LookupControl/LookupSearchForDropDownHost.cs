using RingSoft.DataEntryControls.WPF;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class LookupSearchForDropDownHost<TDropDownControl> : LookupSearchForHost<TDropDownControl>
        where TDropDownControl : DropDownEditControl
    {
        public override void SetFocusToControl()
        {
            Control.TextBox?.Focus();
            base.SetFocusToControl();
        }

        public override bool CanProcessSearchForKey(Key key)
        {
            switch (key)
            {
                case Key.Enter:
                case Key.Escape:
                    return !Control.IsPopupOpen();
            }
            return base.CanProcessSearchForKey(key);
        }
    }
}
