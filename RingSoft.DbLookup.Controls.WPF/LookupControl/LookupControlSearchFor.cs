using System;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DbLookup.Lookup;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class LookupSearchForControl<TControl> : LookupSearchForControl
        where TControl : Control
    {
        public new TControl Control { get; private set; }

        protected abstract TControl ConstructControl();

        protected LookupSearchForControl(LookupColumnDefinitionBase lookupColumn) : base(lookupColumn)
        {
        }

        internal override void InternalInitialize()
        {
            Control = ConstructControl();
            base.Control = Control;

            base.InternalInitialize();
            Initialize(Control);
        }

        protected abstract void Initialize(TControl control);
    }
    public abstract class LookupSearchForControl
    {
        public Control Control { get; protected internal set; }

        public LookupColumnDefinitionBase LookupColumn { get; }

        public abstract string SearchText { get; set; }

        public event EventHandler<KeyEventArgs> PreviewKeyDown;
        public event EventHandler TextChanged;

        protected internal LookupSearchForControl(LookupColumnDefinitionBase lookupColumn)
        {
            LookupColumn = lookupColumn;
        }

        internal virtual void InternalInitialize()
        {
            Control.PreviewKeyDown += (sender, args) => OnPreviewKeyDown(args);
        }

        public abstract void SelectAll();

        protected void OnPreviewKeyDown(KeyEventArgs e)
        {
            PreviewKeyDown?.Invoke(Control, e);
        }

        protected void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
