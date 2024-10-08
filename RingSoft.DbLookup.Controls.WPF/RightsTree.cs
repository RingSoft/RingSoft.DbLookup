using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;

namespace RingSoft.DbLookup.Controls.WPF
{
    public enum RightsModes
    {
        None = 0,
        Reset = 1,
        Load = 2,
    }

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.DevLogix;assembly=RingSoft.DevLogix"
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
    ///     <MyNamespace:RightsTree/>
    ///
    /// </summary>
    public class RightsTree : Control, IReadOnlyControl, IRightsTreeControl
    {
        public static readonly DependencyProperty DataChangedProperty =
            DependencyProperty.Register(nameof(DataChanged), typeof(bool), typeof(RightsTree));

        public bool DataChanged
        {
            get { return (bool)GetValue(DataChangedProperty); }
            set { SetValue(DataChangedProperty, value); }
        }

        public Border Border { get; set; }

        public RightsTreeViewModel ViewModel { get; set; }

        public TreeView TreeView { get; set; }

        private bool _controlLoaded;
        private RightsModes _rightsMode;
        private string _rightsString;
        private bool _setFocus;
        private bool _gotFocusRan;
        private bool _readOnlyMode;

        static RightsTree()
        {
            IsTabStopProperty.OverrideMetadata(typeof(RightsTree), new FrameworkPropertyMetadata(false));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(RightsTree), new FrameworkPropertyMetadata(typeof(RightsTree)));
        }

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

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            ViewModel.SetReadOnlyMode(readOnlyValue);
            _readOnlyMode = readOnlyValue;
        }

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

        public void SetDataChanged()
        {
            DataChanged = true;
        }
    }
}
