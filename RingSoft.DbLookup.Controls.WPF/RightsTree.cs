// ***********************************************************************
// Assembly         : RingSoft.DbLookup.Controls.WPF
// Author           : petem
// Created          : 05-21-2024
//
// Last Modified By : petem
// Last Modified On : 05-21-2024
// ***********************************************************************
// <copyright file="RightsTree.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    /// <summary>
    /// Enum RightsModes
    /// </summary>
    public enum RightsModes
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The reset
        /// </summary>
        Reset = 1,
        /// <summary>
        /// The load
        /// </summary>
        Load = 2,
    }

    /// <summary>
    /// Class RightsTree.
    /// Implements the <see cref="Control" />
    /// Implements the <see cref="IReadOnlyControl" />
    /// Implements the <see cref="IRightsTreeControl" />
    /// </summary>
    /// <seealso cref="Control" />
    /// <seealso cref="IReadOnlyControl" />
    /// <seealso cref="IRightsTreeControl" />
    public class RightsTree : Control, IReadOnlyControl, IRightsTreeControl
    {
        /// <summary>
        /// The data changed property
        /// </summary>
        public static readonly DependencyProperty DataChangedProperty =
            DependencyProperty.Register(nameof(DataChanged), typeof(bool), typeof(RightsTree));

        /// <summary>
        /// Gets or sets a value indicating whether [data changed].
        /// </summary>
        /// <value><c>true</c> if [data changed]; otherwise, <c>false</c>.</value>
        public bool DataChanged
        {
            get { return (bool)GetValue(DataChangedProperty); }
            set { SetValue(DataChangedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public Border Border { get; set; }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>The view model.</value>
        public RightsTreeViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the TreeView.
        /// </summary>
        /// <value>The TreeView.</value>
        public TreeView TreeView { get; set; }

        /// <summary>
        /// The control loaded
        /// </summary>
        private bool _controlLoaded;
        /// <summary>
        /// The rights mode
        /// </summary>
        private RightsModes _rightsMode;
        /// <summary>
        /// The rights string
        /// </summary>
        private string _rightsString;
        /// <summary>
        /// The set focus
        /// </summary>
        private bool _setFocus;
        /// <summary>
        /// The got focus ran
        /// </summary>
        private bool _gotFocusRan;
        /// <summary>
        /// The read only mode
        /// </summary>
        private bool _readOnlyMode;

        /// <summary>
        /// Initializes static members of the <see cref="RightsTree"/> class.
        /// </summary>
        static RightsTree()
        {
            IsTabStopProperty.OverrideMetadata(typeof(RightsTree), new FrameworkPropertyMetadata(false));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(RightsTree), new FrameworkPropertyMetadata(typeof(RightsTree)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RightsTree"/> class.
        /// </summary>
        public RightsTree()
        {
            Loaded += (sender, args) =>
            {
                if (IsVisible)
                {
                    if (!_controlLoaded)
                    {
                        ViewModel.Initialize(this);
                    }
                    _controlLoaded = true;
                    switch (_rightsMode)
                    {
                        case RightsModes.None:
                            break;
                        case RightsModes.Reset:
                            ViewModel.Reset();
                            break;
                        case RightsModes.Load:
                            ViewModel.LoadRights(_rightsString);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    _rightsMode = RightsModes.None;
                    _rightsString = string.Empty;

                }
            };

            GotFocus += (sender, args) =>
            {
                if (!_gotFocusRan)
                {
                    _gotFocusRan = true;
                    if (TreeView == null)
                    {
                        _setFocus = true;
                    }
                    else
                    {
                        SetFocusToFirstNode();
                    }
                }
            };

            KeyDown += (sender, args) =>
            {
                if (!_readOnlyMode)
                {
                    if (args.Key == Key.Space)
                    {
                        var item = TreeView.SelectedItem as RightTreeViewItem;
                        if (item != null)
                        {
                            if (item.ThreeState)
                            {
                                item.IsChecked = false;
                            }
                            else
                            {
                                item.IsChecked = !item.IsChecked;
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        /// <exception cref="System.ApplicationException">Need to set Border</exception>
        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;

            if (Border == null)
            {
                throw new ApplicationException("Need to set Border");
            }

            ViewModel = Border.TryFindResource("RightsViewModel") as RightsTreeViewModel;
            TreeView = GetTemplateChild(nameof(TreeView)) as TreeView;
            SetReadOnlyMode(false);

            if (_setFocus)
            {
                SetFocusToFirstNode();
                _setFocus = false;
            }


            base.OnApplyTemplate();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            if (_controlLoaded)
            {
                ViewModel.Reset();
            }
            else
            {
                _rightsMode = RightsModes.Reset;
            }
        }

        /// <summary>
        /// Gets the rights.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetRights()
        {
            var result = string.Empty;
            if (_controlLoaded)
            {
                result = ViewModel.Rights.GetRightsString();
            }
            else
            {
                result = _rightsString;
            }

            return result;
        }

        /// <summary>
        /// Loads the rights.
        /// </summary>
        /// <param name="rightsString">The rights string.</param>
        public void LoadRights(string rightsString)
        {
            if (_controlLoaded)
            {
                ViewModel.LoadRights(rightsString);
            }
            else
            {
                _rightsString = rightsString;
                _rightsMode = RightsModes.Load;
            }
        }

        /// <summary>
        /// Sets the read only mode.
        /// </summary>
        /// <param name="readOnlyValue">if set to <c>true</c> [read only value].</param>
        public void SetReadOnlyMode(bool readOnlyValue)
        {
            ViewModel.SetReadOnlyMode(readOnlyValue);
            _readOnlyMode = readOnlyValue;
        }

        /// <summary>
        /// Sets the focus to first node.
        /// </summary>
        public void SetFocusToFirstNode()
        {
            TreeView.Focus();
            try
            {
                var tvi = TreeView.ItemContainerGenerator.ContainerFromItem(TreeView.Items[0])
                    as System.Windows.Controls.TreeViewItem;
                if (tvi != null)
                {
                    tvi.IsSelected = true;
                }

            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Sets the data changed.
        /// </summary>
        public void SetDataChanged()
        {
            DataChanged = true;
        }
    }
}
