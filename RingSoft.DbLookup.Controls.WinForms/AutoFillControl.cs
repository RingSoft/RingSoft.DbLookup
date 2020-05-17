using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WinForms.Properties;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WinForms
{
    /// <summary>
    /// An AutoFill control that automatically fills text from the database table as a user types.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public partial class AutoFillControl : UserControl, IAutoFillControl, INotifyPropertyChanged
    {
        public string EditText
        {
            get => AutoFillText.Text;
            set => AutoFillText.Text = value;
        }

        public int SelectionStart
        {
            get => AutoFillText.SelectionStart;
            set => AutoFillText.SelectionStart = value;
        }

        public int SelectionLength
        {
            get => AutoFillText.SelectionLength;
            set => AutoFillText.SelectionLength = value;
        }

        /// <summary>
        /// Gets the AutoFill data.
        /// </summary>
        /// <value>
        /// The auto fill data.
        /// </value>
        public AutoFillData AutoFillData { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically tab out after lookup select.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to tab out after lookup select; otherwise, <c>false</c>.
        /// </value>
        public bool TabOutAfterLookupSelect { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user has typed into this control.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (value == _isDirty)
                    return;
                _isDirty = value;
                OnPropertyChanged(nameof(IsDirty));
            }
        }

        public override string Text
        {
            get { return AutoFillText.Text; }
            set { AutoFillText.Text = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public AutoFillValue Value
        {
            get => _autoFillValue;
            set
            {
                if (_autoFillValue == value)
                    return;

                _autoFillValue = value;
                if (!_onAutoFillDataChanged)
                {
                    _autoFillPropertyChanging = true;
                    SetValue(value);
                    _autoFillPropertyChanging = false;
                }
                OnPropertyChanged(nameof(Value));
            }
        }

        private AutoFillSetup _autoFillSetup;
        /// <summary>
        /// Gets or sets the automatic fill setup.
        /// </summary>
        /// <value>
        /// The automatic fill setup.
        /// </value>
        public AutoFillSetup Setup
        {
            get => _autoFillSetup;
            set
            {
                if (_autoFillSetup == value)
                    return;

                _autoFillSetup = value;

                if (value != null)
                    SetupControl(value.LookupDefinition, value.AllowLookupAdd, value.AllowLookupView,
                        value.ShowContainsBox, value.Distinct);
            }
        }

        private LookupDefinitionBase _lookupDefinition;
        private AutoFillContainsForm _autoFillContainsForm;
        private bool _lookupAllowAdd = true;
        private bool _lookupAllowView = true;
        private bool _isDirty;
        private AutoFillValue _autoFillValue;
        private bool _onAutoFillDataChanged;
        private bool _autoFillPropertyChanging;
        private AutoFillValue _pendingAutoFillValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillControl"/> class.
        /// </summary>
        public AutoFillControl()
        {
            _autoFillContainsForm = new AutoFillContainsForm();
            InitializeComponent();

            AutoFillText.KeyPress += AutoFillText_KeyPress;
            AutoFillText.KeyDown += AutoFillTextOnKeyDown;
            LookupButton.Click += LookupButton_Click;
            SizeChanged += AutoFillControl_SizeChanged;
        }
        private void AutoFillTextOnKeyDown(object sender, KeyEventArgs e)
        {
            if (AutoFillData == null)
                return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    AutoFillData.OnDeleteKeyDown();
                    e.Handled = true;
                    IsDirty = true;
                    break;
                case Keys.Down:
                    _autoFillContainsForm.ChangeIndex(true);
                    e.Handled = true;
                    break;
                case Keys.Up:
                    _autoFillContainsForm.ChangeIndex(false);
                    e.Handled = true;
                    break;
                case Keys.F5:
                    LookupButton_Click(this, EventArgs.Empty);
                    break;
            }
        }

        private void AutoFillText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AutoFillData == null)
                return;

            if (((ModifierKeys & Keys.Control) != 0) || (ModifierKeys & Keys.Alt) != 0)
                return;

            if (AutoFillData.OnKeyCharPressed(e.KeyChar))
                e.Handled = true;
            IsDirty = true;
        }

        private void SetupControl(LookupDefinitionBase lookupDefinition, bool lookupAllowAdd, bool lookupAllowView,
            bool showContainsBox, bool distinct)
        {
            if (_lookupDefinition != null)
                ClearValue();

            _lookupDefinition = lookupDefinition;
            _lookupAllowAdd = lookupAllowAdd;
            _lookupAllowView = lookupAllowView;

            AutoFillData = new AutoFillData(this, _lookupDefinition, distinct) { ShowContainsBox = showContainsBox };

            AutoFillData.AutoFillDataChanged += AutoFillData_AutoFillDataChanged;

            _autoFillContainsForm.SetupControl(this, AutoFillText);

            if (_pendingAutoFillValue != null)
            {
                SetValue(_pendingAutoFillValue);
                _pendingAutoFillValue = null;
            }
        }

        private void LookupButton_Click(object sender, EventArgs e)
        {
            if (AutoFillData == null)
                return;

            var lookupForm = new LookupForm(_lookupDefinition, _lookupAllowAdd, _lookupAllowView, AutoFillText.Text);
            lookupForm.RefreshData += (o, args) => { AutoFillData.RefreshData(_autoFillContainsForm.Visible); };
            lookupForm.Icon = FindForm()?.Icon;
            SetFocusToTextBox();
            var lookupSelectArgs = lookupForm.ShowDialog();
            if (lookupSelectArgs != null)
                LookupForm_LookupSelect(lookupSelectArgs);
        }

        private void LookupForm_LookupSelect(LookupSelectArgs e)
        {
            var text = e.LookupData.GetSelectedRow()
                .GetRowValue(_lookupDefinition.InitialSortColumnDefinition.SelectSqlAlias);
            Value = new AutoFillValue(e.LookupData.PrimaryKeyValue, text);
            IsDirty = true;
            if (TabOutAfterLookupSelect)
            {
                var form = FindForm();
                if (form != null)
                {
                    form.SelectNextControl(this, true, true, true, true);
                }
            }
        }

        private void SetValue(AutoFillValue autoFillValue)
        {
            if (autoFillValue == null)
            {
                ClearValue();
            }
            else
            {
                if (AutoFillData == null)
                {
                    _pendingAutoFillValue = autoFillValue;
                    return;
                }
                AutoFillData.SetValue(autoFillValue.PrimaryKeyValue, autoFillValue.Text, _autoFillContainsForm.Visible);
            }
        }

        /// <summary>
        /// Clears the value.
        /// </summary>
        public void ClearValue()
        {
            if (AutoFillData == null)
                return;

            AutoFillData.ClearValue();
        }

        protected override void OnLoad(EventArgs e)
        {
            var textBoxRectangle = AutoFillText.RectangleToScreen(AutoFillText.DisplayRectangle);
            _autoFillContainsForm.Left = textBoxRectangle.Left;
            _autoFillContainsForm.Top = textBoxRectangle.Top + textBoxRectangle.Height;
            _autoFillContainsForm.Width = textBoxRectangle.Width;
            base.OnLoad(e);
        }

        private void AutoFillControl_SizeChanged(object sender, EventArgs e)
        {
            var textBoxRectangle = AutoFillText.RectangleToScreen(AutoFillText.DisplayRectangle);
            _autoFillContainsForm.Width = textBoxRectangle.Width;
        }

        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    var textBoxRectangle = AutoFillText.RectangleToScreen(AutoFillText.DisplayRectangle);
        //    _autoFillContainsForm.Width = textBoxRectangle.Width;

        //    base.OnSizeChanged(e);
        //}

        private void AutoFillData_AutoFillDataChanged(object sender, AutoFillDataChangedArgs e)
        {
            //Unit Test
            //var startText = AutoFillText.Text;
            //if (!string.IsNullOrEmpty(startText))
            //{
            //    Clipboard.SetText(AutoFillText.Text);
            //}
            //var preSelStart = AutoFillText.SelectionStart;
            //var preSelLen = AutoFillText.SelectionLength;
            //MessageBox.Show($"PreText = {startText}");

            if (!_autoFillPropertyChanging)
            {
                _onAutoFillDataChanged = true;
                var autoFillValue = new AutoFillValue(AutoFillData.PrimaryKeyValue, AutoFillText.Text);
                Value = autoFillValue;
                _onAutoFillDataChanged = false;
            }

            //Unit Test
            //if (!string.IsNullOrEmpty(AutoFillText.Text))
            //    Clipboard.SetText(AutoFillText.Text);
            //MessageBox.Show(
            //    $"PreSelStart = {preSelStart.ToString()}, PreSelLen = {preSelLen.ToString()}\r\nPostSelStart = {AutoFillText.SelectionStart}, PostSelLen = {AutoFillText.SelectionLength}",
            //    "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (_autoFillContainsForm != null)
                _autoFillContainsForm.Hide();
            AutoFillText.SelectionStart = 0;
            AutoFillText.SelectionLength = AutoFillText.TextLength;
            base.OnLeave(e);
        }

        internal void SetFocusToTextBox()
        {
            AutoFillText.Focus();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
