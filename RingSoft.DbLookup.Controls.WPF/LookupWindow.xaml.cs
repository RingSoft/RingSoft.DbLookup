using RingSoft.DbLookup.Controls.WPFCore.Annotations;
using RingSoft.DbLookup.Lookup;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for LookupWindow.xaml
    /// </summary>
    public partial class LookupWindow : INotifyPropertyChanged
    {
        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                _lookupDefinition = value;
                OnPropertyChanged(nameof(LookupDefinition));
            }
        }

        /// <summary>
        /// Occurs when a lookup row is selected by the user.
        /// </summary>
        public event EventHandler<LookupSelectArgs> LookupSelect;

        public event EventHandler RefreshData;

        private LookupDefinitionBase _lookupDefinition;
        private bool _allowView;

        public LookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            LookupDefinition = lookupDefinition;
            _allowView = allowView;

            InitializeComponent();

            ContentRendered += (sender, args) =>
            {
                LookupControl.LookupData.SelectedIndexChanged += LookupData_SelectedIndexChanged;
                LookupControl.LookupData.LookupView += (o, viewArgs) =>
                {
                    SelectButtonClick();
                    viewArgs.Handled = true;
                };
                LookupControl.RefreshData(false, initialSearchFor);
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            };

            AddButton.IsEnabled = allowAdd;
            SelectButton.Click += (sender, args) => { SelectButtonClick(); };
            AddButton.Click += (sender, args) => AddButtonClick();
            ViewButton.Click += (sender, args) => ViewButtonClick();
            CloseButton.Click += (sender, args) => { Close(); };

            PreviewKeyDown += (sender, args) =>
            {
                switch (args.Key)
                {
                    case Key.Escape:
                        Close();
                        args.Handled = true;
                        break;
                }
            };
        }

        private void LookupData_SelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (e.NewIndex >= 0)
            {
                ViewButton.IsEnabled = _allowView;
                SelectButton.IsEnabled = true;
            }
            else
            {
                ViewButton.IsEnabled = SelectButton.IsEnabled = false;
            }
        }

        private void SelectButtonClick()
        {
            Close();
            OnSelectLookupRow();
        }

        protected virtual void OnSelectLookupRow()
        {
            var args = new LookupSelectArgs(LookupControl.LookupData);
            LookupSelect?.Invoke(this, args);
        }

        private void ViewButtonClick()
        {
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.View, string.Empty);
            args.CallBackToken.RefreshData += (sender, eventArgs) => LookupCallBackRefreshData();
            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void AddButtonClick()
        {
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.Add, LookupControl.SearchText);
            args.CallBackToken.RefreshData += (sender, eventArgs) => LookupCallBackRefreshData();
            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void LookupCallBackRefreshData()
        {
            LookupControl.LookupData.RefreshData();
            RefreshData?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}