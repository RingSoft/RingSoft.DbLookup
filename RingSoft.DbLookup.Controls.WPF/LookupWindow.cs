// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 12-19-2022
//
// Last Modified By : petem
// Last Modified On : 11-29-2023
// ***********************************************************************
// <copyright file="LookupWindow.cs" company="Peter Ringering">
//     Copyright (c) 2023. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
    /// Displays a lookup control to the user and allows the user to select, add, or view an item.
    /// Implements the <see cref="BaseWindow" />
    /// Implements the <see cref="ILookupWindow" />
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="BaseWindow" />
    /// <seealso cref="ILookupWindow" />
    /// <seealso cref="INotifyPropertyChanged" />
    /// <font color="red">Badly formed XML comment.</font>
    [TemplatePart(Name = "LookupControl", Type = typeof(LookupControl))]
    [TemplatePart(Name = "SelectButton", Type = typeof(Button))]
    [TemplatePart(Name = "AddButton", Type = typeof(Button))]
    [TemplatePart(Name = "ViewButton", Type = typeof(Button))]
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public class LookupWindow : BaseWindow, ILookupWindow, INotifyPropertyChanged
    {
        /// <summary>
        /// The lookup definition
        /// </summary>
        private LookupDefinitionBase _lookupDefinition;
        /// <summary>
        /// Gets or sets the lookup definition.
        /// </summary>
        /// <value>The lookup definition.</value>
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
        /// The lookup control
        /// </summary>
        private LookupControl _lookupControl;

        /// <summary>
        /// Gets or sets the lookup control.
        /// </summary>
        /// <value>The lookup control.</value>
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


        /// <summary>
        /// The select button
        /// </summary>
        private Button _selectButton;

        /// <summary>
        /// Gets or sets the select button.
        /// </summary>
        /// <value>The select button.</value>
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

        /// <summary>
        /// The add button
        /// </summary>
        private Button _addButton;

        /// <summary>
        /// Gets or sets the add button.
        /// </summary>
        /// <value>The add button.</value>
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

        /// <summary>
        /// The view button
        /// </summary>
        private Button _viewButton;

        /// <summary>
        /// Gets or sets the view button.
        /// </summary>
        /// <value>The view button.</value>
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

        /// <summary>
        /// The close button
        /// </summary>
        private Button _closeButton;

        /// <summary>
        /// Gets or sets the close button.
        /// </summary>
        /// <value>The close button.</value>
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
        /// Gets or sets the add view parameter.
        /// </summary>
        /// <value>The add view parameter.</value>
        public object AddViewParameter { get; set; }

        /// <summary>
        /// Gets or sets the initial search for primary key value.
        /// </summary>
        /// <value>The initial search for primary key value.</value>
        public PrimaryKeyValue InitialSearchForPrimaryKeyValue { get; set; }

        /// <summary>
        /// Gets the automatic fill control.
        /// </summary>
        /// <value>The automatic fill control.</value>
        public IAutoFillControl AutoFillControl { get; }

        /// <summary>
        /// Gets a value indicating whether [read only mode].
        /// </summary>
        /// <value><c>true</c> if [read only mode]; otherwise, <c>false</c>.</value>
        public bool ReadOnlyMode => _readOnlyMode;

        /// <summary>
        /// Gets or sets a value indicating whether [allow advanced find].
        /// </summary>
        /// <value><c>true</c> if [allow advanced find]; otherwise, <c>false</c>.</value>
        public bool AllowAdvancedFind
        {
            get => _allowAdvancedFind;
            set
            {
                if (value == _allowAdvancedFind) return;
                _allowAdvancedFind = value;
                if (LookupControl != null && !_allowAdvancedFind)
                {
                    LookupControl.AdvancedFindButton.Visibility = Visibility.Collapsed;
                }
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

        public event EventHandler<LookupAddViewArgs> LookupAdd;

        /// <summary>
        /// Occurs when [refresh data].
        /// </summary>
        public event EventHandler RefreshData;
        /// <summary>
        /// Occurs when [apply new lookup].
        /// </summary>
        public event EventHandler ApplyNewLookup;

        /// <summary>
        /// The allow view
        /// </summary>
        private bool _allowView;

        /// <summary>
        /// The initial search for
        /// </summary>
        private string _initialSearchFor;

        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;
        /// <summary>
        /// The read only primary key value
        /// </summary>
        private PrimaryKeyValue _readOnlyPrimaryKeyValue;
        /// <summary>
        /// The old size
        /// </summary>
        private Size _oldSize;
        /// <summary>
        /// The allow advanced find
        /// </summary>
        private bool _allowAdvancedFind = true;

        /// <summary>
        /// Initializes static members of the <see cref="LookupWindow"/> class.
        /// </summary>
        static LookupWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LookupWindow), new FrameworkPropertyMetadata(typeof(LookupWindow)));
            ShowInTaskbarProperty.OverrideMetadata(typeof(LookupWindow), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupWindow"/> class.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        /// <param name="allowAdd">if set to <c>true</c> [allow add].</param>
        /// <param name="allowView">if set to <c>true</c> [allow view].</param>
        /// <param name="initialSearchFor">The initial search for.</param>
        /// <param name="autoFillControl">The automatic fill control.</param>
        /// <param name="readOnlyValue">The read only value.</param>
        /// <exception cref="System.ArgumentException">Lookup definition does not have any visible columns defined or its initial sort column is null.</exception>
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
            var tableRegistered = LookupControlsGlobals.WindowRegistry
                .IsTableRegistered(LookupDefinition.TableDefinition);
            if (allowAdd)
            {
                allowAdd = tableRegistered;
            }

            if (allowView)
            {
                allowView = tableRegistered;
            }

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

                if (!LookupDefinition.TableDefinition.CanViewTable || !AllowAdvancedFind)
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

        /// <summary>
        /// Handles the Loaded event of the LookupControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void LookupControl_Loaded(object sender, RoutedEventArgs e)
        {
            Reload();
            LookupControl.RefreshData(false, _initialSearchFor, null,
                true, InitialSearchForPrimaryKeyValue);
            LookupControl.AddViewParameter = AddViewParameter;

        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
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
                if (!AllowAdvancedFind)
                {
                    LookupControl.ShowAdvancedFindButton = false;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event of the ViewButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (LookupControl == null)
                return;

            var args = new LookupAddViewArgs(LookupControl.LookupDataMaui, false, LookupFormModes.View, string.Empty, this)
            {
                InputParameter = AddViewParameter,
                AllowEdit = ViewButton.IsEnabled,
                AllowAdd = AddButton.IsEnabled,
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

        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

            LookupAdd?.Invoke(this, args);
            if (!args.Handled)
                _lookupDefinition.TableDefinition.Context.OnAddViewLookup(args);
        }

        /// <summary>
        /// Handles the Click event of the SelectButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            OnSelectButtonClick();
        }

        /// <summary>
        /// Called when [select button click].
        /// </summary>
        public void OnSelectButtonClick()
        {
            if (LookupControl == null)
                return;

            Close();
            LookupControl.LookupDataMaui.GetSelectedPrimaryKeyValue();
            var args = new LookupSelectArgs(LookupControl.LookupDataMaui);
            LookupSelect?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the LookupData control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectedIndexChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Lookups the data lookup view.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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

        /// <summary>
        /// Lookups the call back refresh data.
        /// </summary>
        /// <param name="token">The token.</param>
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

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Called when [read only mode set].
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            _readOnlyMode = readOnlyValue || LookupDefinition.ReadOnlyMode;

            base.OnReadOnlyModeSet(readOnlyValue);
        }

        /// <summary>
        /// Sets the control read only mode.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
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

        /// <summary>
        /// Applies the new lookup definition.
        /// </summary>
        /// <param name="lookupDefinition">The lookup definition.</param>
        public void ApplyNewLookupDefinition(LookupDefinitionBase lookupDefinition)
        {
            LookupDefinition = lookupDefinition;
            ApplyNewLookup?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Selects the primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
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
