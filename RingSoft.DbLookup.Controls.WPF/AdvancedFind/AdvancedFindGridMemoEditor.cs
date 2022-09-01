using System.Windows;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;

namespace RingSoft.DbLookup.Controls.WPF.AdvancedFind
{
    public class AdvancedFindGridMemoEditor : DataEntryGridMemoEditor
    {
        public AdvancedFindGridMemoEditor(DataEntryGridMemoValue gridMemoValue) : base(gridMemoValue)
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (MemoEditor != null)
            {
                MemoEditor.CollapseDateButton();
            }
        }

        protected override bool Validate()
        {
            if (MemoEditor.Text.IsNullOrEmpty())
            {
                var message = "Column Header cannot be empty.";
                var caption = "Invalid Column Header";
                ControlsGlobals.UserInterface.ShowMessageBox(message, caption, RsMessageBoxIcons.Exclamation);
                MemoEditor.TextBox.Focus();
                return false;
            }
            return base.Validate();
        }
    }
}
