using RingSoft.DbLookup.Lookup;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RingSoft.DbLookup.Controls.WPF.Properties;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Interaction logic for LookupWindow.xaml
    /// </summary>
    public partial class OldLookupWindow : INotifyPropertyChanged
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

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;


        public event EventHandler RefreshData;

        private LookupDefinitionBase _lookupDefinition;
        private bool _allowView;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupWindow"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <exception cref="ArgumentException">Lookup definition does not have any visible columns defined or its initial sort column is null.</exception>
        public OldLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            if (lookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            LookupDefinition = lookupDefinition;
            _allowView = allowView;

            InitializeComponent();

            var title = lookupDefinition.Title;
            if (title.IsNullOrEmpty())
                title = lookupDefinition.TableDefinition.ToString();

            Title = $"{title} Lookup";
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
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.View, string.Empty, this);
            args.CallBackToken.RefreshData += (sender, eventArgs) => LookupCallBackRefreshData();

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void AddButtonClick()
        {
            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.Add,
                LookupControl.SearchText, this);
            args.CallBackToken.RefreshData += (sender, eventArgs) => LookupCallBackRefreshData();

            LookupView?.Invoke(this, args);
            if (!args.Handled)
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