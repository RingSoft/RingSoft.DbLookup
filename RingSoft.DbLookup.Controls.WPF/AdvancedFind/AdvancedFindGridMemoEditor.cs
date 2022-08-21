using System.Windows;
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
    }
}
