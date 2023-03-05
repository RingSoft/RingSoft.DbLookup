using System;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DbLookup.Lookup;

// ReSharper disable once CheckNamespace
namespace RingSoft.DbLookup.Controls.WPF
{
    public abstract class LookupSearchForHost<TControl> : LookupSearchForHost
        where TControl : Control
    {
        public new TControl Control { get; private set; }

        protected abstract TControl ConstructControl();

        internal override void InternalInitialize(LookupColumnDefinitionBase columnDefinition)
        {
            Control = ConstructControl();
            base.Control = Control;

            base.InternalInitialize(columnDefinition);
            Initialize(Control, columnDefinition);
        }

        internal override void InternalInitialize()
        {
            Control = ConstructControl();
            base.Control = Control;

            base.InternalInitialize();
        }

        protected abstract void Initialize(TControl control, LookupColumnDefinitionBase columnDefinition);
    }
    public abstract class LookupSearchForHost
    {
        public Control Control { get; protected internal set; }

        public LookupColumnDefinitionBase LookupColumn { get; private set; }

        public abstract string SearchText { get; set; }

        public event EventHandler<KeyEventArgs> PreviewKeyDown;
        public event EventHandler TextChanged;

        protected internal LookupSearchForHost()
        {
        }

        internal virtual void InternalInitialize(LookupColumnDefinitionBase columnDefinition)
        {
            Control.PreviewKeyDown += (sender, args) => OnPreviewKeyDown(args);
            LookupColumn = columnDefinition;
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

        public virtual void SetFocusToControl()
        {
        }

        public virtual bool CanProcessSearchForKey(Key key)
        {
            return true;
        }
    }
}
