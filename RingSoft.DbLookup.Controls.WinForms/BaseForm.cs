using System.Windows.Forms;

namespace RingSoft.DbLookup.Controls.WinForms
{
    public partial class BaseForm : Form
    {
        public bool CloseOnEscape { get; set; } = true;

        public BaseForm()
        {
            InitializeComponent();
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
                    case Keys.Escape:
                        if (CloseOnEscape)
                        {
                            Close();
                            return true;
                        }

                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
