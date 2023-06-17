using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.Tests
{
    public class TestAutoFillControl : IAutoFillControl
    {
        public string EditText { get; set; }
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public void RefreshValue(AutoFillValue autoFillValue)
        {
            
        }

        public void OnSelect()
        {
            
        }
    }
}
