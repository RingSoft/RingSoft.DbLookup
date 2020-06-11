using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DbLookup.Lookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DbLookup.Controls.WPF;assembly=RingSoft.DbLookup.Controls.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:NewLookupWindow/>
    ///
    /// </summary>
    [TemplatePart(Name = "LookupControl", Type = typeof(LookupControl))]
    [TemplatePart(Name = "SelectButton", Type = typeof(Button))]
    [TemplatePart(Name = "AddButton", Type = typeof(Button))]
    [TemplatePart(Name = "ViewButton", Type = typeof(Button))]
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public class LookupWindow : BaseWindow, INotifyPropertyChanged
    {
        private LookupDefinitionBase _lookupDefinition;
        public LookupDefinitionBase LookupDefinition
        {
            get => _lookupDefinition;
            set
            {
                _lookupDefinition = value;
                OnPropertyChanged(nameof(LookupDefinition));
            }
        }

        private LookupControl _lookupControl;

        public LookupControl LookupControl
        {
            get => _lookupControl;
            set
            {
                if (_lookupControl != null)
                {
                    _lookupControl.LookupData.SelectedIndexChanged -= LookupData_SelectedIndexChanged;
                    _lookupControl.LookupData.LookupView -= LookupData_LookupView;
                }

                _lookupControl = value;
            }
        }


        private Button _selectButton;

        public Button SelectButton
        {
            get => _selectButton;
            set
            {
                if (_selectButton != null)
                    _selectButton.Click -= SelectButton_Click;
                
                _selectButton = value;
                if (_selectButton != null)
                    _selectButton.Click += SelectButton_Click;
            }
        }

        private Button _addButton;

        public Button AddButton
        {
            get => _addButton;
            set
            {
                if (_addButton != null)
                    _addButton.Click -= AddButton_Click;

                _addButton = value;
                if (_addButton != null)
                    _addButton.Click += AddButton_Click;
            }
        }

        private Button _viewButton;

        public Button ViewButton
        {
            get => _viewButton;
            set
            {
                if (_viewButton != null)
                    _viewButton.Click -= ViewButton_Click;

                _viewButton = value;
                if (_viewButton != null)
                    _viewButton.Click += ViewButton_Click;
            }
        }

        private Button _closeButton;

        public Button CloseButton
        {
            get => _closeButton;
            set
            {
                if (_closeButton != null)
                    _closeButton.Click -= CloseButton_Click;

                _closeButton = value;
                if (_closeButton != null)
                    _closeButton.Click += CloseButton_Click;
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

        private bool _allowAdd;
        private bool _allowView;
        private string _initialSearchFor;

        static LookupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupWindow), new FrameworkPropertyMetadata(typeof(LookupWindow)));
        }

        public LookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor)
        {
            DataContext = this;

            if (lookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            LookupDefinition = lookupDefinition;
            _allowView = allowView;
            _allowAdd = allowAdd;
            _initialSearchFor = initialSearchFor;

            var title = lookupDefinition.Title;
            if (title.IsNullOrEmpty())
                title = lookupDefinition.TableDefinition.ToString();

            Title = $"{title} Lookup";
            Loaded += LookupWindow_Loaded;
        }

        private void LookupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (LookupControl != null)
            {
                LookupControl.SetupControl();
                LookupControl.LookupData.SelectedIndexChanged += LookupData_SelectedIndexChanged;
                LookupControl.LookupData.LookupView += LookupData_LookupView;
            }

            if (AddButton != null)
                AddButton.IsEnabled = _allowAdd;

            LookupControl?.RefreshData(false, _initialSearchFor);
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public override void OnApplyTemplate()
        {
            LookupControl = GetTemplateChild("LookupControl") as LookupControl;
            SelectButton = GetTemplateChild("SelectButton") as Button;
            AddButton = GetTemplateChild("AddButton") as Button;
            ViewButton = GetTemplateChild("ViewButton") as Button;
            CloseButton = GetTemplateChild("CloseButton") as Button;

            base.OnApplyTemplate();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (LookupControl == null)
                return;

            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.View, string.Empty, this);
            args.CallBackToken.RefreshData += (o, eventArgs) => LookupCallBackRefreshData();

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (LookupControl == null)
                return;

            var args = new LookupAddViewArgs(LookupControl.LookupData, false, LookupFormModes.Add,
                LookupControl.SearchText, this);
            args.CallBackToken.RefreshData += (o, eventArgs) => LookupCallBackRefreshData();

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OnSelectButtonClick();
        }

        private void OnSelectButtonClick()
        {
            if (LookupControl == null)
                return;

            Close();
            var args = new LookupSelectArgs(LookupControl.LookupData);
            LookupSelect?.Invoke(this, args);
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

        private void LookupData_LookupView(object sender, LookupAddViewArgs e)
        {
            OnSelectButtonClick();
            e.Handled = true;
        }

        private void LookupCallBackRefreshData()
        {
            if (LookupControl == null)
                return;

            LookupControl.LookupData.RefreshData();
            RefreshData?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
