using System;
using System.Windows.Forms;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace RingSoft.DbLookup.Controls.WinForms
{
    public static class GblMethods
    {
        public const Int32
            SM_CXVSCROLL = 2,
            SM_CYHSCROLL = 3;

        public static int GetWidthFromPercent(Control control, double percentWidth, bool adjustScroll, int offset = 0)
        {
            int width = 0;
            if (percentWidth > 0)
            {
                var controlWidth = control.Width - (adjustScroll ? (GetSystemMetrics(SM_CXVSCROLL) + 7) : 0);
                controlWidth -= offset;
                width = (int)Math.Floor(controlWidth * (percentWidth / 100));
            }
            return width;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
    }
}
