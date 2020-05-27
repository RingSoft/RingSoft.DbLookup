using System;
using System.Windows.Forms;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WinForms
{
    /// <summary>
    /// A form with a LookupControl with the ability to select or view a lookup row.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class LookupForm : Form
    {
        public event EventHandler RefreshData;

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        private LookupDefinitionBase _lookupDefinition;
        private bool _allowView;
        private string _initialSearchFor;
        private LookupSelectArgs _lookupSelectArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupForm" /> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> allow add.</param>
        /// <param name="allowView">if set to <c>true</c> allow view.</param>
        /// <param name="initialSearchFor">The initial search for text.</param>
        public LookupForm(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            if (lookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            InitializeComponent();

            _lookupDefinition = lookupDefinition;
            _allowView = allowView;
            _initialSearchFor = initialSearchFor;
            AddButton.Enabled = allowAdd;

            LookupControl.LookupDefinition = _lookupDefinition;
            LookupControl.LookupData.SelectedIndexChanged += LookupData_SelectedIndexChanged;
            LookupControl.LookupData.LookupView += LookupData_LookupView;
            ViewButton.Click += ViewButton_Click;
            AddButton.Click += AddButton_Click;
            SelectButton.Click += SelectButton_Click;
            CloseButton.Click += (sender, args) => Close();
        }

        public new LookupSelectArgs ShowDialog(IWin32Window owner = null)
        {
            if (owner == null)
                base.ShowDialog();
            else
            {
                base.ShowDialog(owner);
            }

            return _lookupSelectArgs;
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            Close();
            OnSelectLookupRow();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.View, string.Empty, this);
            args.CallBackToken.RefreshData += LookupCallBack_RefreshData;

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.Add, LookupControl.SearchText, this);
            args.CallBackToken.RefreshData += LookupCallBack_RefreshData;

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void LookupCallBack_RefreshData(object sender, EventArgs e)
        {
            LookupControl.LookupData.RefreshData();
            RefreshData?.Invoke(this, EventArgs.Empty);
        }

        private void LookupData_LookupView(object sender, LookupAddViewArgs e)
        {
            e.Handled = true;
            SelectButton_Click(this, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            var title = _lookupDefinition.Title;
            if (title.IsNullOrEmpty())
                title = _lookupDefinition.TableDefinition.ToString();

            Text = $@"{title} Lookup";

            LookupControl.RefreshData(false, _initialSearchFor);
            base.OnLoad(e);
        }

        private void LookupData_SelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (e.NewIndex >= 0)
            {
                ViewButton.Enabled = _allowView;
                SelectButton.Enabled = true;
            }
            else
            {
                ViewButton.Enabled = SelectButton.Enabled = false;
            }
        }

        protected virtual void OnSelectLookupRow()
        {
            _lookupSelectArgs = new LookupSelectArgs(LookupControl.LookupData);
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
                        Close();
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}