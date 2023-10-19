using RingSoft.DbLookup.Lookup;
using RingSoft.DataEntryControls.Engine;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.AutoFill;

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
    public class LookupWindow : BaseWindow, ILookupWindow, INotifyPropertyChanged
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
                    _lookupControl.Loaded -= LookupControl_Loaded;

                _lookupControl = value;

                if (_lookupControl != null)
                    _lookupControl.Loaded += LookupControl_Loaded;
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

        public object AddViewParameter { get; set; }

        public PrimaryKeyValue InitialSearchForPrimaryKeyValue { get; set; }

        public IAutoFillControl AutoFillControl { get; }

        public bool ReadOnlyMode => _readOnlyMode;

        /// <summary>
        /// Occurs when a lookup row is selected by the user.
        /// </summary>
        public event EventHandler<LookupSelectArgs> LookupSelect;

        /// <summary>
        /// Occurs when a user wishes to add or view a selected lookup row.  Set Handled property to True to not send this message to the LookupContext.
        /// </summary>
        public event EventHandler<LookupAddViewArgs> LookupView;

        public event EventHandler RefreshData;
        public event EventHandler ApplyNewLookup;

        private bool _allowView;

        private string _initialSearchFor;

        private bool _readOnlyMode;
        private PrimaryKeyValue _readOnlyPrimaryKeyValue;
        private Size _oldSize;

        static LookupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupWindow), new FrameworkPropertyMetadata(typeof(LookupWindow)));
            ShowInTaskbarProperty.OverrideMetadata(typeof(LookupWindow), new FrameworkPropertyMetadata(false));
        }

        public LookupWindow(LookupDefinitionBase lookupDefinition
            , bool allowAdd
            , bool allowView
            , string initialSearchFor
            , IAutoFillControl autoFillControl = null
            , PrimaryKeyValue readOnlyValue = null)
        {
            AutoFillControl = autoFillControl;
            var loaded = false;
            _readOnlyPrimaryKeyValue = readOnlyValue;
            DataContext = this;

            if (lookupDefinition.InitialSortColumnDefinition == null)
                throw new ArgumentException(
                    "Lookup definition does not have any visible columns defined or its initial sort column is null.");

            LookupDefinition = lookupDefinition;
            _allowView = allowView;
            _initialSearchFor = initialSearchFor;

            var title = lookupDefinition.Title;
            if (title.IsNullOrEmpty())
                title = lookupDefinition.TableDefinition.ToString();

            Title = $"{title} Lookup";
            Loaded += (sender, args) =>
            {
                if (AddButton != null)
                {
                    AddButton.IsEnabled = allowAdd && LookupDefinition.AllowAddOnTheFly;
                    if (!AddButton.IsEnabled)
                    {
                        AddButton.Visibility = Visibility.Collapsed;
                    }
                }

                if (!LookupDefinition.TableDefinition.CanViewTable)
                {
                    ViewButton.Visibility = Visibility.Collapsed;
                    LookupControl.ShowAdvancedFindButton = false;
                }

                if (!LookupDefinition.TableDefinition.CanAddToTable)
                {
                    AddButton.Visibility = Visibility.Collapsed;
                }
                if (_readOnlyMode)
                {
                    SelectButton.Visibility = Visibility.Collapsed;
                }
                _oldSize = new Size(Width, Height);
                loaded = true;
                LookupControl?.Focus();
            };

            SizeChanged += (sender, args) =>
            {
                if (LookupControl != null && loaded)
                {
                    var widthDif = Width - _oldSize.Width;
                    var heightDif = Height - _oldSize.Height;
                    LookupControl.Width = LookupControl.ActualWidth + widthDif;
                    LookupControl.Height = LookupControl.ActualHeight + heightDif;
                }

                _oldSize = args.NewSize;
            };
        }

        private void LookupControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();
            LookupControl.RefreshData(false, _initialSearchFor, null,
                true, InitialSearchForPrimaryKeyValue);
            LookupControl.AddViewParameter = AddViewParameter;

        }

        public void Reload()
        {
            LookupControl.SelectedIndexChanged += (sender, args) => 
            {
                if (LookupControl.SelectedIndex >= 0)
                {
                    SelectButton.IsEnabled = true;
                    if (_allowView)
                    {
                        ViewButton.IsEnabled = true;
                    }
                }
                else
                {
                    SelectButton.IsEnabled = false;
                    ViewButton.IsEnabled = false;
                }
            };
            LookupControl.LookupDataMaui.LookupView += LookupData_LookupView;
        }

        public override void OnApplyTemplate()
        {
            LookupControl = GetTemplateChild("LookupControl") as LookupControl;
            SelectButton = GetTemplateChild("SelectButton") as Button;
            AddButton = GetTemplateChild("AddButton") as Button;
            ViewButton = GetTemplateChild("ViewButton") as Button;
            CloseButton = GetTemplateChild("CloseButton") as Button;

            SetReadOnlyMode(_readOnlyMode);
            SelectButton.IsEnabled = false;
            ViewButton.IsEnabled = false;
            base.OnApplyTemplate();

            if (!_allowView)
            {
                ViewButton.Visibility = AddButton.Visibility = Visibility.Collapsed;
                //if (LookupControl != null)
                //{
                //    LookupControl.ShowAdvancedFindButton = false;
                //}
            }

            if (LookupControl != null)
            {
                LookupControl.SetLookupWindow(this);
                LookupControl.AllowViewDetails = _allowView;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (LookupControl == null)
                return;

            var args = new LookupAddViewArgs(LookupControl.LookupDataMaui, false, LookupFormModes.View, string.Empty, this)
            {
                InputParameter = AddViewParameter,
                AllowEdit = AddButton.IsEnabled,
                LookupReadOnlyMode = _readOnlyMode,
                ReadOnlyPrimaryKeyValue = _readOnlyPrimaryKeyValue,
                SelectedPrimaryKeyValue = LookupControl.LookupDataMaui.GetSelectedPrimaryKeyValue(),
            };

            if (_readOnlyMode)
                args.AllowEdit = true;

            args.CallBackToken.RefreshData += (o, eventArgs) =>
            {
                LookupCallBackRefreshData(args.CallBackToken);
            };

            LookupView?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (LookupControl == null)
                return;

            var searchText = LookupControl.SearchText;
            if (!searchText.IsNullOrEmpty())
            {
                //if (LookupControl.LookupData.SortColumnDefinition != LookupDefinition.InitialSortColumnDefinition)
                //{
                //    searchText = string.Empty;
                //}
            }
            var args = new LookupAddViewArgs(LookupControl.LookupDataMaui, false, LookupFormModes.Add,
                searchText, this)
            {
                InputParameter = AddViewParameter,
                LookupReadOnlyMode = _readOnlyMode,
                ReadOnlyPrimaryKeyValue = _readOnlyPrimaryKeyValue
            };
            args.CallBackToken.RefreshData += (o, eventArgs) =>
            {
                LookupCallBackRefreshData(args.CallBackToken);
            };

            _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OnSelectButtonClick();
        }

        public void OnSelectButtonClick()
        {
            if (LookupControl == null)
                return;

            Close();
            var args = new LookupSelectArgs(LookupControl.LookupDataMaui);
            LookupSelect?.Invoke(this, args);
        }

        private void LookupData_SelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
        {
            if (e.NewIndex >= 0)
            {
                ViewButton.IsEnabled = _allowView && LookupDefinition.AllowAddOnTheFly;
                SelectButton.IsEnabled = true;
            }
            else
            {
                ViewButton.IsEnabled = SelectButton.IsEnabled = false;
            }
        }

        private void LookupData_LookupView(object sender, LookupAddViewArgs e)
        {
            if (_readOnlyMode)
            {
                if (_allowView)
                {
                    ViewButton_Click(this, new RoutedEventArgs());
                }
            }
            else
            {
                OnSelectButtonClick();
            }
            
            e.Handled = true;
        }

        private void LookupCallBackRefreshData(LookupCallBackToken token)
        {
            LookupControl.LookupDataMaui.RefreshData(LookupControl.SearchText);
            if (AutoFillControl != null)
            {
                AutoFillControl.RefreshValue(token);
            }
            //RefreshData?.Invoke(this, EventArgs.Empty);
            if (token.RefreshMode ==AutoFillRefreshModes.DbSelect)
            {
                Close();
                //LookupControl.LookupDataMaui.SetNewPrimaryKeyValue(token.NewAutoFillValue.PrimaryKeyValue);
                if (AutoFillControl != null)
                {
                    AutoFillControl.OnSelect();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue || LookupDefinition.ReadOnlyMode;

            base.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (readOnlyValue)
            {
                if (control == CloseButton)
                {
                    CloseButton.IsEnabled = true;
                    return;
                }

                if (control == SelectButton)
                {
                    SelectButton.IsEnabled = !_readOnlyMode;
                    return;
                }

                if (control == AddButton)
                {
                    AddButton.IsEnabled = false;
                    AddButton.Visibility = Visibility.Collapsed;
                    return;
                }

                if (control == ViewButton)
                {
                    ViewButton.IsEnabled = _allowView;
                    if (!ViewButton.IsEnabled)
                    {
                        ViewButton.Visibility = Visibility.Collapsed;
                    }
                    
                    if (LookupControl.ListView != null)
                        if (LookupControl.ListView.SelectedIndex >= 0)
                            ViewButton.IsEnabled = true;

                    return;
                }
            }

            base.SetControlReadOnlyMode(control, readOnlyValue);
        }

        public void ApplyNewLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            ApplyNewLookup?.Invoke(this, new EventArgs());
        }

        public void SelectPrimaryKey(PrimaryKeyValue primaryKey)
        {
            if (LookupControl.LookupDataMaui.GetSelectedPrimaryKeyValue() == null)
            {
                LookupControl.LookupDataMaui.SetNewPrimaryKeyValue(primaryKey);
                return;
            }
            LookupControl.LookupDataMaui.SetNewPrimaryKeyValue(primaryKey);
            //OnSelectButtonClick();
            //SelectPrimaryKey(primaryKey);
        }
    }
}
