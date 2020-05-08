using System;
using System.Data;
using System.Windows.Forms;
using RingSoft.DbLookup.AutoFill;

namespace RingSoft.DbLookup.Controls.WinForms
{
    public partial class AutoFillContainsForm : Form
    {
        protected override bool ShowWithoutActivation => true;

        private AutoFillControl _autoFillControl;
        private TextBox _owner;

        public AutoFillContainsForm()
        {
            InitializeComponent();
            AutoFillContainsList.Click += (sender, args) => _autoFillControl.SetFocusToTextBox();
            AutoFillContainsList.SelectedIndexChanged += AutoFillContainsListOnSelectedIndexChanged;
        }

        internal void SetupControl(AutoFillControl autofillControl, TextBox owner)
        {
            _owner = owner;
            _autoFillControl = autofillControl;
            _autoFillControl.AutoFillData.AutoFillDataChanged += AutoFillDataOnAutoFillDataChanged;
        }

        private void AutoFillDataOnAutoFillDataChanged(object sender, AutoFillDataChangedArgs e)
        {
            if (e.RefreshContainsList)
                FillList();
        }

        private void AutoFillContainsListOnSelectedIndexChanged(object sender, EventArgs e)
        {
            _autoFillControl.AutoFillData.OnChangeContainsIndex(AutoFillContainsList.SelectedIndex);
        }

        public void FillList()
        {
            AutoFillContainsList.Items.Clear();
            var height = 0;
            if (_autoFillControl.AutoFillData.ShowContainsBox && _autoFillControl.AutoFillData.ContainsBoxDataTable != null)
            {
                var itemIndex = 0;
                foreach (DataRow dataRow in _autoFillControl.AutoFillData.ContainsBoxDataTable.Rows)
                {
                    var text = dataRow.GetRowValue(_autoFillControl.AutoFillData.AutoFillDefinition
                        .SelectSqlAlias);
                    AutoFillContainsList.Items.Add(text);
                    height += AutoFillContainsList.GetItemHeight(itemIndex);
                    itemIndex++;
                }
            }

            if (AutoFillContainsList.Items.Count > 0)
            {
                Height = height + 5;
                if (!Visible)
                    Show(_owner);
            }
            else
            {
                Hide();
            }
        }

        public void ChangeIndex(bool down)
        {
            if (down)
            {
                if (AutoFillContainsList.SelectedIndex < AutoFillContainsList.Items.Count - 1)
                {
                    AutoFillContainsList.SelectedIndex++;
                }
            }
            else
                if (AutoFillContainsList.SelectedIndex > 0)
                    AutoFillContainsList.SelectedIndex--;
        }
    }
}
