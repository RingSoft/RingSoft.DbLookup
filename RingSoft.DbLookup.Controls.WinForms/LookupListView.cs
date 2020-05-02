using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RingSoft.DbLookup.Lookup;

// ReSharper disable InconsistentNaming

namespace RingSoft.DbLookup.Controls.WinForms
{
    internal class LookupListView : ListView
    {
        private const int WM_KILLFOCUS = 0x0008;

        public class LookupListViewEnterKeyEventArgs
        {
            public bool Handled { get; set; }
            public LookupListViewEnterKeyEventArgs()
            {
                Handled = false;
            }
        }

        public event EventHandler<LookupListViewEnterKeyEventArgs> LookupListViewEnterKey;
        protected virtual void OnLookupListViewEnterKey(LookupListViewEnterKeyEventArgs e)
        {
            if (LookupListViewEnterKey != null)
                LookupListViewEnterKey(this, e);
        }
        //-----------------------------------------------------------------------------
        public LookupListView()
        {
            View = View.Details;
            MultiSelect = false;
            FullRowSelect = true;
            HideSelection = false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x83: // WM_NCCALCSIZE
                    //Turn off the vertical scrollbar.
                    int style = GetWindowLong(this.Handle, GWL_STYLE);
                    if ((style & WS_VSCROLL) == WS_VSCROLL)
                        SetWindowLong(this.Handle, GWL_STYLE, style & ~WS_VSCROLL);
                    base.WndProc(ref m);
                    break;
                default:
                    //We have to do this focus dance so the selected item appears selected even when the listview does not have focus.
                    //In Windows 8, the listview's selection bar is barely seen when the listview does not have focus.
                    if (m.Msg != WM_KILLFOCUS)
                        base.WndProc(ref m);
                    break;
            }
        }

        private const int GWL_STYLE = -16;
        private const int WS_VSCROLL = 0x00200000;

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
                return (int)GetWindowLong32(hWnd, nIndex);
            else
                return (int)(long)GetWindowLongPtr64(hWnd, nIndex);
        }

        public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 4)
                return (int)SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            else
                return (int)(long)SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);

        public void SetupColumns(LookupDefinitionBase lookupDefinition)
        {
            Items.Clear();
            Columns.Clear();

            foreach (var lookupDefinitionColumn in lookupDefinition.VisibleColumns)
            {
                var columnWdth = GblMethods.GetWidthFromPercent(this, lookupDefinitionColumn.PercentWidth, false,
                    lookupDefinition.VisibleColumns.Count * 2);

                var column = Columns.Add(lookupDefinitionColumn.Caption, columnWdth);
                switch (lookupDefinitionColumn.HorizontalAlignment)
                {
                    case LookupColumnAlignmentTypes.Left:
                        column.TextAlign = HorizontalAlignment.Left;
                        break;
                    case LookupColumnAlignmentTypes.Center:
                        column.TextAlign = HorizontalAlignment.Center;
                        break;
                    case LookupColumnAlignmentTypes.Right:
                        column.TextAlign = HorizontalAlignment.Right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SelectItem(int index)
        {
            if (index < 0 || index >= this.Items.Count)
                return;

            Items[index].Selected = true;
            Items[index].Focused = true;
        }
        public int GetSelectedIndex()
        {
            if (this.SelectedIndices.Count == 0)
                return -1;

            return this.SelectedIndices[0];
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ReSharper disable once InconsistentNaming
            const int WM_KEYDOWN = 0x100;

            // ReSharper disable once InconsistentNaming
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Enter:
                        LookupListViewEnterKeyEventArgs e = new LookupListViewEnterKeyEventArgs();
                        OnLookupListViewEnterKey(e);
                        if (e.Handled)
                            return true;
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ListViewExtensions
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public Mask mask;
            public int cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public Format fmt;
            public IntPtr lParam;
            // _WIN32_IE >= 0x0300 
            public int iImage;
            public int iOrder;
            // _WIN32_IE >= 0x0500
            public uint type;
            public IntPtr pvFilter;
            // _WIN32_WINNT >= 0x0600
            public uint state;

            [Flags]
            public enum Mask
            {
                Format = 0x4,       // HDI_FORMAT
            };

            [Flags]
            public enum Format
            {
                SortDown = 0x200,   // HDF_SORTDOWN
                SortUp = 0x400,     // HDF_SORTUP
            };
        };

        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETHEADER = LVM_FIRST + 31;

        public const int HDM_FIRST = 0x1200;
        public const int HDM_GETITEM = HDM_FIRST + 11;
        public const int HDM_SETITEM = HDM_FIRST + 12;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, ref HDITEM lParam);

        public static void SetSortIcon(this ListView listViewControl, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listViewControl.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
            for (int columnNumber = 0; columnNumber <= listViewControl.Columns.Count - 1; columnNumber++)
            {
                var columnPtr = new IntPtr(columnNumber);
                var item = new HDITEM
                {
                    mask = HDITEM.Mask.Format
                };

                if (SendMessage(columnHeader, HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Exception();
                }

                if (order != SortOrder.None && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            item.fmt &= ~HDITEM.Format.SortDown;
                            item.fmt |= HDITEM.Format.SortUp;
                            break;
                        case SortOrder.Descending:
                            item.fmt &= ~HDITEM.Format.SortUp;
                            item.fmt |= HDITEM.Format.SortDown;
                            break;
                    }
                }
                else
                {
                    item.fmt &= ~HDITEM.Format.SortDown & ~HDITEM.Format.SortUp;
                }

                if (SendMessage(columnHeader, HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Exception();
                }
            }
        }

        //[DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        //private static extern long ShowScrollBar(uint hwnd, uint wBar, uint bShow);
        //private const uint SB_HORZ = 0;
        //private const uint SB_VERT = 1;
        //private const uint SB_BOTH = 3;

        //public static void HideHorizontalScrollBar(this ListView listView)
        //{
        //    ShowScrollBar((uint)listView.Handle.ToInt64(), SB_HORZ, 0);
        //}

        //public static void HideVerticalScrollBar(this ListView listView)
        //{
        //    ShowScrollBar((uint)listView.Handle.ToInt64(), SB_VERT, 0);
        //}
    }
}
